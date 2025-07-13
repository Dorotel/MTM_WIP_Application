using System.Net.NetworkInformation;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Helpers;

public class Helper_Control_MySqlSignal
{

    public static async Task<(int strength, int pingMs)> GetStrengthAsync()
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

        var strength = pingMs switch
        {
            < 0 => 0,
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
    public static string ConnectionString { get; set; } =
        Helper_Database_Variables.GetConnectionString(null, null, null, null);
}