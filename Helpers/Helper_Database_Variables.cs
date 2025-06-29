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
            server ??= "localhost";
            database ??= "mtm_wip_application";
            uid ??= Model_AppVariables.User != null ? Model_AppVariables.User.ToUpper() : "";
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
        var logDirectory = server switch
        {
            "172.16.1.104" => @"X:\MH_RESOURCE\Material_Handler\MTM WIP App\Logs",
            "localhost" => @"C:\Users\johnk\OneDrive\Documents\Work Folder\WIP App Logs",
            _ => throw new InvalidOperationException("Unknown server value.")
        };
        var userDirectory = Path.Combine(logDirectory, userName);
        if (!Directory.Exists(userDirectory))
            Directory.CreateDirectory(userDirectory);
        var timestamp = DateTime.Now.ToString("MM-dd-yyyy @ h-mm tt");
        var logFileName = $"{userName} {timestamp}.log";
        return Path.Combine(userDirectory, logFileName);
    }

    #endregion
}

#endregion