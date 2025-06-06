using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MTM_WIP_Application.Core; // Adjust this namespace if your ISqlHelper is elsewhere

public class SqlHelper : ISqlHelper
{
    private readonly string _connectionString;

    public SqlHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> ExecuteNonQuery(
        string procedureName,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using (var conn = new MySqlConnection(_connectionString))
        using (var cmd = new MySqlCommand(procedureName, conn))
        {
            cmd.CommandType = commandType;
            if (parameters != null)
                foreach (var param in parameters)
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            await conn.OpenAsync();
            if (useAsync)
                return await cmd.ExecuteNonQueryAsync();
            else
                return cmd.ExecuteNonQuery();
        }
    }

    public async Task<DataTable> ExecuteDataTable(
        string procedureName,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using (var conn = new MySqlConnection(_connectionString))
        using (var cmd = new MySqlCommand(procedureName, conn))
        {
            cmd.CommandType = commandType;
            if (parameters != null)
                foreach (var param in parameters)
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            using (var adapter = new MySqlDataAdapter(cmd))
            {
                var table = new DataTable();
                if (useAsync)
                {
                    await conn.OpenAsync();
                    await Task.Run(() => adapter.Fill(table));
                }
                else
                {
                    conn.Open();
                    adapter.Fill(table);
                }

                return table;
            }
        }
    }

    public async Task<object?> ExecuteScalar(
        string procedureName,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using (var conn = new MySqlConnection(_connectionString))
        using (var cmd = new MySqlCommand(procedureName, conn))
        {
            cmd.CommandType = commandType;
            if (parameters != null)
                foreach (var param in parameters)
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            await conn.OpenAsync();
            if (useAsync)
                return await cmd.ExecuteScalarAsync();
            else
                return cmd.ExecuteScalar();
        }
    }

    public async Task<MySqlDataReader> ExecuteReader(
        string procedureName,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        var conn = new MySqlConnection(_connectionString);
        var cmd = new MySqlCommand(procedureName, conn)
        {
            CommandType = commandType
        };
        if (parameters != null)
            foreach (var param in parameters)
                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

        await conn.OpenAsync();
        if (useAsync)
            return (MySqlDataReader)await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        else
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }
}