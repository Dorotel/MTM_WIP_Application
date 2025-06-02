using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.Data;

/// <summary>
///     Provides connection string and log file path utilities for database and logging operations.
/// </summary>
public static class SqlVariables
{
    /// <summary>
    ///     Builds a MySQL connection string using provided or default values.
    /// </summary>
    /// <param name="server">The server address. Defaults to 'localhost' if null or empty.</param>
    /// <param name="database">The database name. Defaults to 'mtm database' if null or empty.</param>
    /// <param name="uid">The user ID. Defaults to WipAppVariables.User if null or empty.</param>
    /// <param name="password">The password. Defaults to empty string if null or empty.</param>
    /// <returns>A MySQL connection string, or an empty string if an error occurs.</returns>
    public static string GetConnectionString(string? server, string? database, string? uid, string? password)
    {
        try
        {
            server ??= "localhost"; // "172.16.1.104"
            database ??= "mtm_wip_application";
            uid ??= WipAppVariables.User != null ? WipAppVariables.User.ToUpper() : "";
            password ??= "";

            return $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};Allow User Variables=True";
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            return string.Empty;
        }
    }

    /// <summary>
    ///     Returns the log file path for a given server and user, creating the directory if needed.
    /// </summary>
    /// <param name="server">The server address.</param>
    /// <param name="userName">The user name.</param>
    /// <returns>Full path to the log file.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the server is not recognized.</exception>
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
}