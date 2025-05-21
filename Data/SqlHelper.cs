using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Data;

internal static class SqlHelper
{
    public static async Task<int> ExecuteNonQuery(
        string commandText,
        Dictionary<string, object>? parameters = null,
        int commandTimeout = 30,
        bool useAsync = false)
    {
        if (useAsync)
        {
            await using var connection = new MySqlConnection(GetConnectionString(null, null, null, null));
            await using var command = new MySqlCommand(commandText, connection)
            {
                CommandTimeout = commandTimeout
            };
            if (parameters != null)
                foreach (var param in parameters)
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }
        else
        {
            using var connection = new MySqlConnection(GetConnectionString(null, null, null, null));
            using var command = new MySqlCommand(commandText, connection)
            {
                CommandTimeout = commandTimeout
            };
            if (parameters != null)
                foreach (var param in parameters)
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            connection.Open();
            return command.ExecuteNonQuery();
        }
    }

    public static async Task<object?> ExecuteScalar(
        string commandText,
        Dictionary<string, object>? parameters = null,
        int commandTimeout = 30,
        bool useAsync = false)
    {
        if (useAsync)
        {
            await using var connection = new MySqlConnection(GetConnectionString(null, null, null, null));
            await using var command = new MySqlCommand(commandText, connection)
            {
                CommandTimeout = commandTimeout
            };
            if (parameters != null)
                foreach (var param in parameters)
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            await connection.OpenAsync();
            return await command.ExecuteScalarAsync();
        }
        else
        {
            using var connection = new MySqlConnection(GetConnectionString(null, null, null, null));
            using var command = new MySqlCommand(commandText, connection)
            {
                CommandTimeout = commandTimeout
            };
            if (parameters != null)
                foreach (var param in parameters)
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            connection.Open();
            return command.ExecuteScalar();
        }
    }

    public static async Task<DataTable> ExecuteDataTable(
        string commandText,
        Dictionary<string, object>? parameters = null,
        int commandTimeout = 30,
        bool useAsync = false)
    {
        if (useAsync)
        {
            await using var connection = new MySqlConnection(GetConnectionString(null, null, null, null));
            await using var command = new MySqlCommand(commandText, connection)
            {
                CommandTimeout = commandTimeout
            };
            if (parameters != null)
                foreach (var param in parameters)
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var table = new DataTable();
            table.Load(reader);
            return table;
        }
        else
        {
            using var connection = new MySqlConnection(GetConnectionString(null, null, null, null));
            using var command = new MySqlCommand(commandText, connection)
            {
                CommandTimeout = commandTimeout
            };
            if (parameters != null)
                foreach (var param in parameters)
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            using var adapter = new MySqlDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }
    }

    public static async Task<MySqlDataReader> ExecuteReader(
        string commandText,
        Dictionary<string, object>? parameters = null,
        int commandTimeout = 30,
        bool useAsync = false)
    {
        if (useAsync)
        {
            var connection = new MySqlConnection(GetConnectionString(null, null, null, null));
            var command = new MySqlCommand(commandText, connection)
            {
                CommandTimeout = commandTimeout
            };
            if (parameters != null)
                foreach (var param in parameters)
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            await connection.OpenAsync();
            return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }
        else
        {
            var connection = new MySqlConnection(GetConnectionString(null, null, null, null));
            var command = new MySqlCommand(commandText, connection)
            {
                CommandTimeout = commandTimeout
            };
            if (parameters != null)
                foreach (var param in parameters)
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}