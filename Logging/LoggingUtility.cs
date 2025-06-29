using System.Diagnostics;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Logging;

#region LoggingUtility

internal static class LoggingUtility
{
    #region Fields

    private static string _appErrorLogFile = string.Empty;
    private static string _dbErrorLogFile = string.Empty;
    private static string _logDirectory = string.Empty;
    private static string _normalLogFile = string.Empty;
    private static readonly Lock LogLock = new();

    #endregion

    #region LogCleanup

    private static void CleanUpOldLogs(string logDirectory, int maxLogs)
    {
        try
        {
            var logFiles = Directory.GetFiles(logDirectory, "*.log")
                .OrderByDescending(File.GetCreationTime)
                .ToList();
            if (logFiles.Count > maxLogs)
            {
                var filesToDelete = logFiles.Skip(maxLogs).ToList();
                foreach (var logFile in filesToDelete) File.Delete(logFile);
            }

            if (Debugger.IsAttached) return;

            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MTM_WIP_APP");
            var localAppDataPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MTM_WIP_APP");
            Service_OnStartup_AppDataCleaner.DeleteDirectoryContents(appDataPath);
            Service_OnStartup_AppDataCleaner.DeleteDirectoryContents(localAppDataPath);
        }
        catch (Exception ex)
        {
            Log($"Failed to clean up old log files or application data: {ex.Message}");
        }
    }

    public static void CleanUpOldLogsIfNeeded()
    {
        if (!string.IsNullOrEmpty(_logDirectory))
            CleanUpOldLogs(_logDirectory, 20);
    }

    #endregion

    #region LogFileWriting

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
            Log($"Failed to write log entry to file: {ex.Message}");
        }
    }

    #endregion

    #region Initialization

    public static void InitializeLogging()
    {
        var server = new MySqlConnectionStringBuilder(Model_AppVariables.ConnectionString).Server;
        var userName = Model_AppVariables.User;
        var logFilePath = Helper_Database_Variables.GetLogFilePath(server, userName);
        _logDirectory = Path.GetDirectoryName(logFilePath) ?? "";
        var baseFileName = Path.GetFileNameWithoutExtension(logFilePath);
        _normalLogFile = Path.Combine(_logDirectory, $"{baseFileName}_normal.log");
        _dbErrorLogFile = Path.Combine(_logDirectory, $"{baseFileName}_db_error.log");
        _appErrorLogFile = Path.Combine(_logDirectory, $"{baseFileName}_app_error.log");
        Log("Initializing logging...");
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
    }

    #endregion

    #region LoggingMethods

    public static void Log(string message)
    {
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        lock (LogLock)
        {
            FlushLogEntryToDisk(_normalLogFile, logEntry);
        }
    }

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

    #endregion

    #region Shutdown

    private static void OnProcessExit(object? sender, EventArgs e)
    {
        var shutdownMsg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Application exiting.";
        lock (LogLock)
        {
            FlushLogEntryToDisk(_normalLogFile, shutdownMsg);
            FlushLogEntryToDisk(_dbErrorLogFile, shutdownMsg);
            FlushLogEntryToDisk(_appErrorLogFile, shutdownMsg);
        }
    }

    #endregion
}

#endregion