using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Vml.Office;
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

    private static async Task CleanUpOldLogsAsync(string logDirectory, int maxLogs)
    {
        try
        {
            await Task.Run(() =>
            {
                try
                {
                    // Add timeout for network operations
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                    var task = Task.Run(() =>
                    {
                        var logFiles = Directory.GetFiles(logDirectory, "*.log")
                            .OrderByDescending(File.GetCreationTime)
                            .ToList();
                        if (logFiles.Count > maxLogs)
                        {
                            var filesToDelete = logFiles.Skip(maxLogs).ToList();
                            foreach (var logFile in filesToDelete)
                            {
                                cts.Token.ThrowIfCancellationRequested();
                                File.Delete(logFile);
                            }
                        }
                    }, cts.Token);

                    task.Wait(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine("[DEBUG] Log cleanup timed out");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[DEBUG] Error during log file cleanup: {ex.Message}");
                }
            });

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
            Debug.WriteLine($"[DEBUG] Failed to clean up old log files or application data: {ex.Message}");
            // Don't call Log() here to avoid potential recursion
        }
    }

    public static async Task CleanUpOldLogsIfNeededAsync()
    {
        if (!string.IsNullOrEmpty(_logDirectory))
            await CleanUpOldLogsAsync(_logDirectory, 20);
    }

    #endregion

    #region LogFileWriting

    private static void FlushLogEntryToDisk(string filePath, string logEntry)
    {
        try
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                // Add timeout for file writing operations
                var task = Task.Run(() =>
                {
                    using var writer = new StreamWriter(filePath, true);
                    writer.WriteLine(logEntry);
                });

                if (!task.Wait(TimeSpan.FromSeconds(5))) Debug.WriteLine($"[DEBUG] Log write timeout for: {filePath}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[DEBUG] Failed to write log entry to file: {ex.Message}");
            // Don't call Log() here to avoid recursion
        }
    }

    #endregion

    #region Initialization

    public static async Task InitializeLoggingAsync()
    {
        try
        {
            Debug.WriteLine("[DEBUG] Starting logging initialization...");

            var server = new MySqlConnectionStringBuilder(Model_AppVariables.ConnectionString).Server;
            var userName = Model_AppVariables.User;

            Debug.WriteLine($"[DEBUG] Server: {server}, User: {userName}");

            // Add timeout for log path operations
            var logFilePath = await Task.Run(async () =>
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                try
                {
                    return await Task.Run(() => Helper_Database_Variables.GetLogFilePath(server, userName), cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine("[DEBUG] Log path creation timed out, using fallback");
                    // Fallback to local temp directory
                    var tempDir = Path.Combine(Path.GetTempPath(), "MTM_WIP_APP", "Logs", userName);
                    Directory.CreateDirectory(tempDir);
                    var timestamp = DateTime.Now.ToString("MM-dd-yyyy @ h-mm tt");
                    return Path.Combine(tempDir, $"{userName} {timestamp}.log");
                }
            });

            _logDirectory = Path.GetDirectoryName(logFilePath) ?? "";
            var baseFileName = Path.GetFileNameWithoutExtension(logFilePath);
            _normalLogFile = Path.Combine(_logDirectory, $"{baseFileName}_normal.log");
            _dbErrorLogFile = Path.Combine(_logDirectory, $"{baseFileName}_db_error.log");
            _appErrorLogFile = Path.Combine(_logDirectory, $"{baseFileName}_app_error.log");

            Debug.WriteLine($"[DEBUG] Log directory: {_logDirectory}");
            Debug.WriteLine($"[DEBUG] Normal log file: {_normalLogFile}");

            Log("Initializing logging...");
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            Debug.WriteLine("[DEBUG] Logging initialization completed");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[DEBUG] Error during logging initialization: {ex.Message}");
            // Create fallback logging to temp directory
            try
            {
                var tempDir = Path.Combine(Path.GetTempPath(), "MTM_WIP_APP", "Logs");
                Directory.CreateDirectory(tempDir);
                var timestamp = DateTime.Now.ToString("MM-dd-yyyy @ h-mm tt");
                var fallbackFile = Path.Combine(tempDir, $"fallback_{timestamp}.log");
                _logDirectory = tempDir;
                _normalLogFile = fallbackFile;
                _dbErrorLogFile = fallbackFile;
                _appErrorLogFile = fallbackFile;
                Debug.WriteLine($"[DEBUG] Using fallback logging to: {tempDir}");
            }
            catch (Exception fallbackEx)
            {
                Debug.WriteLine($"[DEBUG] Fallback logging also failed: {fallbackEx.Message}");
                // If even fallback fails, disable logging
                _logDirectory = "";
                _normalLogFile = "";
                _dbErrorLogFile = "";
                _appErrorLogFile = "";
            }
        }
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