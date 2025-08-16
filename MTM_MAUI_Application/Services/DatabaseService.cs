using MTM_MAUI_Application.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MTM_MAUI_Application.Services;

/// <summary>
/// Interface for database operations
/// </summary>
public interface IDatabaseService
{
    Task<DataTable> ExecuteStoredProcedureAsync(string procedureName, Dictionary<string, object>? parameters = null);
    Task<T?> ExecuteScalarAsync<T>(string procedureName, Dictionary<string, object>? parameters = null);
    Task<int> ExecuteNonQueryAsync(string procedureName, Dictionary<string, object>? parameters = null);
    Task<bool> TestConnectionAsync();
    string GetConnectionString();
}

/// <summary>
/// Database service implementation using MySQL
/// </summary>
public class DatabaseService : IDatabaseService
{
    private readonly IConfiguration _configuration;
    private readonly ILoggingService _logging;
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration, ILoggingService logging)
    {
        _configuration = configuration;
        _logging = logging;
        _connectionString = BuildConnectionString();
    }

    public string GetConnectionString() => _connectionString;

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection.State == ConnectionState.Open;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync("Database connection test failed", ex);
            return false;
        }
    }

    public async Task<DataTable> ExecuteStoredProcedureAsync(string procedureName, Dictionary<string, object>? parameters = null)
    {
        var dataTable = new DataTable();

        try
        {
            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 30
            };

            // Add parameters
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue($"p_{param.Key}", param.Value ?? DBNull.Value);
                }
            }

            // Add output parameters for status and error message
            var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            var errorParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(statusParam);
            command.Parameters.Add(errorParam);

            await connection.OpenAsync();
            using var adapter = new MySqlDataAdapter(command);
            adapter.Fill(dataTable);

            // Check for errors
            var status = statusParam.Value as int? ?? -1;
            var errorMessage = errorParam.Value as string ?? string.Empty;

            if (status != 0)
            {
                await _logging.LogWarningAsync($"Stored procedure {procedureName} returned status {status}: {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error executing stored procedure {procedureName}", ex);
            throw;
        }

        return dataTable;
    }

    public async Task<T?> ExecuteScalarAsync<T>(string procedureName, Dictionary<string, object>? parameters = null)
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 30
            };

            // Add parameters
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue($"p_{param.Key}", param.Value ?? DBNull.Value);
                }
            }

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
                return default(T);

            return (T)Convert.ChangeType(result, typeof(T));
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error executing scalar procedure {procedureName}", ex);
            throw;
        }
    }

    public async Task<int> ExecuteNonQueryAsync(string procedureName, Dictionary<string, object>? parameters = null)
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 30
            };

            // Add parameters
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue($"p_{param.Key}", param.Value ?? DBNull.Value);
                }
            }

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error executing non-query procedure {procedureName}", ex);
            throw;
        }
    }

    private string BuildConnectionString()
    {
        // Use automatic server detection logic similar to original WinForms app
        var serverAddress = GetServerAddress();
        var database = _configuration["Database:Name"] ?? "mtm_wip_application";
        var userId = _configuration["Database:UserId"] ?? "root";
        var password = _configuration["Database:Password"] ?? "";
        var port = _configuration["Database:Port"] ?? "3306";

        return $"Server={serverAddress};Database={database};Uid={userId};Pwd={password};Port={port};";
    }

    private string GetServerAddress()
    {
        // Environment detection logic from original Helper_Database_Variables
        var releaseServer = "172.16.1.104";
        var currentIp = GetCurrentIPAddress();

        // If current IP matches server IP pattern, use production server
        if (currentIp?.StartsWith("172.16.1.") == true)
        {
            return releaseServer;
        }

        // Otherwise use localhost for development
        return "localhost";
    }

    private string? GetCurrentIPAddress()
    {
        try
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            return host.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                ?.ToString();
        }
        catch
        {
            return null;
        }
    }
}