

using System.Diagnostics;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Helpers;

#region Helper_Database_Variables

public static class Helper_Database_Variables
{
    #region Connection String

    public static string GetConnectionString(string? server, string? database, string? uid, string? password)
    {
        try
        {
            server ??= Model_AppVariables.WipServerAddress ?? "localhost";
            database ??= Model_Users.Database;
            uid ??= Model_AppVariables.User.ToUpper();
            return $"SERVER={server};DATABASE={database};UID={uid};Allow User Variables=True ;";
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            return string.Empty;
        }
    }

    #endregion

    #region Log File Path

    public static string GetLogFilePath(string server, string userName)
    {
        try
        {
            var logDirectory = Environment.UserName.Equals("johnk", StringComparison.OrdinalIgnoreCase)
                ? @"C:\Users\johnk\OneDrive\Documents\Work Folder\WIP App Logs"
                : @"X:\MH_RESOURCE\Material_Handler\MTM WIP App\Logs";

            var userDirectory = Path.Combine(logDirectory, userName);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var directoryTask = Task.Run(() =>
            {
                if (!Directory.Exists(userDirectory)) Directory.CreateDirectory(userDirectory);
            }, cts.Token);

            try
            {
                directoryTask.Wait(cts.Token);
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException($"Directory creation timed out for: {userDirectory}");
            }

            var timestamp = DateTime.Now.ToString("MM-dd-yyyy @ h-mm tt");
            var logFileName = $"{userName} {timestamp}.log";
            return Path.Combine(userDirectory, logFileName);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[DEBUG] Error in GetLogFilePath: {ex.Message}");
            throw;
        }
    }

    #endregion
}

#endregion