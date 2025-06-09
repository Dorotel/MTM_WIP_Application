using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Core;

namespace MTM_WIP_Application.Forms.MainForm;

/// <summary>
/// Checks the connection "strength" to a MySQL server by pinging its host.
/// </summary>
public class MySqlConnectionStrengthChecker
{
    /// <summary>
    /// Asynchronously gets the connection strength (0-5) and ping time (ms).
    /// </summary>
    public async Task<(int strength, int pingMs)> GetStrengthAsync()
    {
        string host;
        try
        {
            var builder = new MySqlConnectionStringBuilder(DatabaseConfig.ConnectionString);
            host = builder.Server ??
                   throw new InvalidOperationException("Connection string does not specify a server.");
        }
        catch
        {
            return (0, -1);
        }

        var pingMs = -1;
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(host, 1000);
            if (reply.Status == IPStatus.Success)
                pingMs = (int)reply.RoundtripTime;
        }
        catch
        {
            pingMs = -1;
        }

        // Map ping time to strength (0-5)
        var strength = pingMs switch
        {
            < 0 => 0, // No response
            < 50 => 5,
            < 100 => 4,
            < 200 => 3,
            < 400 => 2,
            < 800 => 1,
            _ => 0
        };

        return (strength, pingMs);
    }
}

public static class DatabaseConfig
{
    public static string ConnectionString { get; set; } = SqlVariables.GetConnectionString(null, null, null, null);
}