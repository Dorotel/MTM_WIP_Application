using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MTM_WIP_Application.Core;

namespace MTM_WIP_Application.Helpers;

public class Helper_MySql(string connectionString)
{
    private static string NormalizeParameterName(string key, CommandType commandType)
    {
        // For stored procedures, parameter names should not start with '@'.
        return commandType == CommandType.StoredProcedure && "@".StartsWith(key)
            ? key[1..]
            : key;
    }

    public async Task<int> ExecuteNonQuery(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using var conn = new MySqlConnection(connectionString);
        using var cmd = new MySqlCommand(procedureOrSql, conn)
        {
            CommandType = commandType
        };

        if (parameters != null)
            foreach (var param in parameters)
                cmd.Parameters.AddWithValue(NormalizeParameterName(param.Key, commandType),
                    param.Value ?? DBNull.Value);

        await conn.OpenAsync();
        return useAsync
            ? await cmd.ExecuteNonQueryAsync()
            : cmd.ExecuteNonQuery();
    }


    public async Task<DataTable> ExecuteDataTable(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using var conn = new MySqlConnection(connectionString);
        using var cmd = new MySqlCommand(procedureOrSql, conn)

        {
            CommandType = commandType
        };

        if (parameters != null)
            foreach (var param in parameters)
                cmd.Parameters.AddWithValue(NormalizeParameterName(param.Key, commandType),
                    param.Value ?? DBNull.Value);

        using var adapter = new MySqlDataAdapter(cmd);
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

    public async Task<object?> ExecuteScalar(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using var conn = new MySqlConnection(connectionString);
        using var cmd = new MySqlCommand(procedureOrSql, conn)
        {
            CommandType = commandType
        };

        if (parameters != null)
            foreach (var param in parameters)
                cmd.Parameters.AddWithValue(NormalizeParameterName(param.Key, commandType),
                    param.Value ?? DBNull.Value);

        await conn.OpenAsync();
        return useAsync
            ? await cmd.ExecuteScalarAsync()
            : cmd.ExecuteScalar();
    }


    public async Task<MySqlDataReader> ExecuteReader(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        var conn = new MySqlConnection(connectionString);
        var cmd = new MySqlCommand(procedureOrSql, conn)
        {
            CommandType = commandType
        };

        if (parameters != null)
            foreach (var param in parameters)
                cmd.Parameters.AddWithValue(NormalizeParameterName(param.Key, commandType),
                    param.Value ?? DBNull.Value);

        await conn.OpenAsync();
        return useAsync
            ? await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection)
            : cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }
}