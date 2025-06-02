using System.Data;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Data;

internal static class SqlHelper
{
    // Add this static property to support testing
    public static string? DefaultConnectionString { get; set; }

    private static void AddParameters(MySqlCommand command, Dictionary<string, object>? parameters)
    {
        if (parameters == null) return;
        foreach (var param in parameters)
            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
    }

    public static async Task<DataTable> ExecuteDataTable(
        string commandText,
        Dictionary<string, object>? parameters = null,
        int commandTimeout = 30,
        bool useAsync = false)
    {
        try
        {
            if (useAsync)
            {
                await using var connection = new MySqlConnection(GetConnectionString());
                await using var command = new MySqlCommand(commandText, connection)
                {
                    CommandTimeout = commandTimeout
                };
                AddParameters(command, parameters);
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                var table = new DataTable();
                table.Load(reader);
                return table;
            }
            else
            {
                using var connection = new MySqlConnection(GetConnectionString());
                using var command = new MySqlCommand(commandText, connection)
                {
                    CommandTimeout = commandTimeout
                };
                AddParameters(command, parameters);
                using var adapter = new MySqlDataAdapter(command);
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            throw;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            throw;
        }
    }

    public static async Task<int> ExecuteNonQuery(
        string commandText,
        Dictionary<string, object>? parameters = null,
        int commandTimeout = 30,
        bool useAsync = false)
    {
        try
        {
            if (useAsync)
            {
                await using var connection = new MySqlConnection(GetConnectionString());
                await using var command = new MySqlCommand(commandText, connection)
                {
                    CommandTimeout = commandTimeout
                };
                AddParameters(command, parameters);
                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
            else
            {
                using var connection = new MySqlConnection(GetConnectionString());
                using var command = new MySqlCommand(commandText, connection)
                {
                    CommandTimeout = commandTimeout
                };
                AddParameters(command, parameters);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            throw;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            throw;
        }
    }

    public static async Task<MySqlDataReader> ExecuteReader(
        string commandText,
        Dictionary<string, object>? parameters = null,
        int commandTimeout = 30,
        bool useAsync = false)
    {
        try
        {
            if (useAsync)
            {
                var connection = new MySqlConnection(GetConnectionString());
                var command = new MySqlCommand(commandText, connection)
                {
                    CommandTimeout = commandTimeout
                };
                AddParameters(command, parameters);
                await connection.OpenAsync();
                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            else
            {
                var connection = new MySqlConnection(GetConnectionString());
                var command = new MySqlCommand(commandText, connection)
                {
                    CommandTimeout = commandTimeout
                };
                AddParameters(command, parameters);
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            throw;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            throw;
        }
    }

    public static async Task<object?> ExecuteScalar(
        string commandText,
        Dictionary<string, object>? parameters = null,
        int commandTimeout = 30,
        bool useAsync = false)
    {
        try
        {
            if (useAsync)
            {
                await using var connection = new MySqlConnection(GetConnectionString());
                await using var command = new MySqlCommand(commandText, connection)
                {
                    CommandTimeout = commandTimeout
                };
                AddParameters(command, parameters);
                await connection.OpenAsync();
                return await command.ExecuteScalarAsync();
            }
            else
            {
                using var connection = new MySqlConnection(GetConnectionString());
                using var command = new MySqlCommand(commandText, connection)
                {
                    CommandTimeout = commandTimeout
                };
                AddParameters(command, parameters);
                connection.Open();
                return command.ExecuteScalar();
            }
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            throw;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            throw;
        }
    }

    private static string GetConnectionString()
    {
        // If DefaultConnectionString is set (e.g., during testing), use it
        if (!string.IsNullOrEmpty(DefaultConnectionString))
            return DefaultConnectionString;

        // Otherwise fall back to the application's connection string
        return WipAppVariables.ConnectionString ?? throw new InvalidOperationException("Connection string is not set.");
    }
}