using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Data;

public static class SqlVariables
{
    public static string GetConnectionString(string? server, string? database, string? uid, string? password)
    {
        try
        {
            if (string.IsNullOrEmpty(server))
                //server = "172.16.1.104";
                server = "localhost";

            if (string.IsNullOrEmpty(database)) database = "mtm database";

            if (string.IsNullOrEmpty(uid)) uid = WipAppVariables.User;

            if (string.IsNullOrEmpty(password)) password = "";

            var connectionString =
                $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};Allow User Variables=True";
            return connectionString;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            return "";
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            return "";
        }
    }

    public static string GetLogFilePath(string server, string userName)
    {
        string logDirectory;

        if (server == "172.16.1.104")
            logDirectory = @"X:\MH_RESOURCE\Material_Handler\MTM WIP App\Logs";
        else if (server == "localhost")
            logDirectory = @"C:\Users\johnk\OneDrive\Documents\Work Folder\WIP App Logs";
        else
            throw new InvalidOperationException("Unknown server value.");

        // Create user-specific directory
        var userDirectory = Path.Combine(logDirectory, userName);
        if (!Directory.Exists(userDirectory)) Directory.CreateDirectory(userDirectory);

        // Generate log file name with timestamp
        var timestamp = DateTime.Now.ToString("MM-dd-yyyy @ h-mm tt");
        var logFileName = $"{userName} {timestamp}.log";

        return Path.Combine(userDirectory, logFileName);
    }
}