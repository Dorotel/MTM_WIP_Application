using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Services;

namespace MTM_WIP_Application.Logging;

internal static class AppLogger
{
    private static string _logFilePath;
    private static readonly Lock LogLock = new();

    private static readonly List<string> LogMessages = [];

    public static void InitializeLogging()
    {
        Debug.WriteLine("Initializing logging...");
        Log("Initializing logging...");

        var server = new MySqlConnectionStringBuilder(Program.connectionString).Server;
        var userName = WipAppVariables.User;
        _logFilePath = SqlVariables.GetLogFilePath(server, userName);

        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
    }

    public static void Log(string message)
    {
        using (LogLock.EnterScope())
        {
            LogMessages.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
    }

    public static void LogDatabaseError(Exception ex)
    {
        using (LogLock.EnterScope())
        {
            LogMessages.Add($"{DateTime.Now}: Database Error - {ex.Message}");
            LogMessages.Add($"{DateTime.Now}: Stack Trace - {ex.StackTrace}");
        }
    }

    public static void CleanUpOldLogsIfNeeded()
    {
        Debug.WriteLine("Cleaning up old logs if needed...");
        Log("Cleaning up old logs if needed...");

        var logDirectory = Path.GetDirectoryName(_logFilePath);
        if (!string.IsNullOrEmpty(logDirectory)) CleanUpOldLogs(logDirectory, 20);
    }

    private static void OnProcessExit(object? sender, EventArgs e)
    {
        Debug.WriteLine("OnProcessExit triggered. Writing logs to file...");
        Log("OnProcessExit triggered. Writing logs to file...");

        try
        {
            using var writer = new StreamWriter(_logFilePath, true);
            lock (LogLock)
            {
                foreach (var logMessage in LogMessages) writer.WriteLine(logMessage);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to write to log file on exit: {ex.Message}");
            Log($"Failed to write to log file on exit: {ex.Message}");
        }
    }

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
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "MTM_WIP_APP");

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
}