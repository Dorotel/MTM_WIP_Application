using System.Diagnostics;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Services;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Logging;

/// <summary>
///     Centralized application logging utility for thread-safe log collection and file output.
///     Now supports separate files for normal, database error, and application error logs.
/// </summary>
internal static class AppLogger
{
    private static string _appErrorLogFile = string.Empty;
    private static string _dbErrorLogFile = string.Empty;
    private static string _logDirectory = string.Empty;
    private static string _normalLogFile = string.Empty;
    private static readonly object LogLock = new();

    /// <summary>
    ///     Deletes old log files and cleans up application data folders.
    /// </summary>
    private static void CleanUpOldLogs(string logDirectory, int maxLogs)
    {
        Debug.WriteLine($"Cleaning up old logs in directory: {logDirectory}");
        Log($"Cleaning up old logs in directory: {logDirectory}");

        try
        {
            var logFiles = Directory.GetFiles(logDirectory, "*.log")
                .OrderByDescending(File.GetCreationTime)
                .ToList();

            if (logFiles.Count > maxLogs)
            {
                var filesToDelete = logFiles.Skip(maxLogs).ToList();
                foreach (var logFile in filesToDelete)
                {
                    Debug.WriteLine($"Deleting old log file: {logFile}");
                    Log($"Deleting old log file: {logFile}");
                    File.Delete(logFile);
                }
            }

            if (Debugger.IsAttached)
            {
                Debug.WriteLine("Skipping cleaning of %AppData% and %LocalAppData% in debug mode.");
                Log("Skipping cleaning of %AppData% and %LocalAppData% in debug mode.");
                return;
            }

            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MTM_WIP_APP");
            var localAppDataPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MTM_WIP_APP");

            AppDataCleaner.DeleteDirectoryContents(appDataPath);
            AppDataCleaner.DeleteDirectoryContents(localAppDataPath);

            Log("Cleaned up application data folders in %AppData% and %LocalAppData%.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to clean up old log files or application data: {ex.Message}");
            Log($"Failed to clean up old log files or application data: {ex.Message}");
        }
    }

    /// <summary>
    ///     Cleans up old log files and application data folders if needed.
    /// </summary>
    public static void CleanUpOldLogsIfNeeded()
    {
        Debug.WriteLine("Cleaning up old logs if needed...");
        Log("Cleaning up old logs if needed...");

        if (!string.IsNullOrEmpty(_logDirectory))
            CleanUpOldLogs(_logDirectory, 20);
    }

    /// <summary>
    ///     Writes a single log entry to the specified log file immediately.
    /// </summary>
    private static void FlushLogEntryToDisk(string filePath, string logEntry)
    {
        try
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                using var writer = new StreamWriter(filePath, true);
                writer.WriteLine(logEntry);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to write log entry to file: {ex.Message}");
            // Do not log this error to avoid recursion
        }
    }

    /// <summary>
    ///     Initializes logging, sets up log file paths, and hooks process exit for log flush.
    /// </summary>
    public static void InitializeLogging()
    {
        Debug.WriteLine("Initializing logging...");
        var server = new MySqlConnectionStringBuilder(WipAppVariables.ConnectionString).Server;
        var userName = WipAppVariables.User;
        var logFilePath = SqlVariables.GetLogFilePath(server, userName);

        _logDirectory = Path.GetDirectoryName(logFilePath) ?? "";
        var baseFileName = Path.GetFileNameWithoutExtension(logFilePath);

        _normalLogFile = Path.Combine(_logDirectory, $"{baseFileName}_normal.log");
        _dbErrorLogFile = Path.Combine(_logDirectory, $"{baseFileName}_db_error.log");
        _appErrorLogFile = Path.Combine(_logDirectory, $"{baseFileName}_app_error.log");

        Log("Initializing logging...");
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
    }

    /// <summary>
    ///     Adds a log message to the normal log file with a timestamp and immediately writes it to disk.
    /// </summary>
    public static void Log(string message)
    {
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        lock (LogLock)
        {
            FlushLogEntryToDisk(_normalLogFile, logEntry);
        }
    }

    /// <summary>
    ///     Logs an application error and its stack trace, and immediately writes them to the application error log file.
    /// </summary>
    public static void LogApplicationError(Exception ex)
    {
        var errorEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Application Error - {ex.Message}";
        var stackEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Stack Trace - {ex.StackTrace}";
        lock (LogLock)
        {
            FlushLogEntryToDisk(_appErrorLogFile, errorEntry);
            FlushLogEntryToDisk(_appErrorLogFile, stackEntry);
        }
    }

    /// <summary>
    ///     Logs a database error and its stack trace, and immediately writes them to the database error log file.
    /// </summary>
    public static void LogDatabaseError(Exception ex)
    {
        var errorEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Database Error - {ex.Message}";
        var stackEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Stack Trace - {ex.StackTrace}";
        lock (LogLock)
        {
            FlushLogEntryToDisk(_dbErrorLogFile, errorEntry);
            FlushLogEntryToDisk(_dbErrorLogFile, stackEntry);
        }
    }

    /// <summary>
    ///     Writes a shutdown message to all log files on process exit.
    /// </summary>
    private static void OnProcessExit(object? sender, EventArgs e)
    {
        Debug.WriteLine("OnProcessExit triggered. Writing shutdown message to all log files...");
        var shutdownMsg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Application exiting.";
        lock (LogLock)
        {
            FlushLogEntryToDisk(_normalLogFile, shutdownMsg);
            FlushLogEntryToDisk(_dbErrorLogFile, shutdownMsg);
            FlushLogEntryToDisk(_appErrorLogFile, shutdownMsg);
        }
    }
}