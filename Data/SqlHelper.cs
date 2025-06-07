using System.Data;
using MTM_WIP_Application.Core;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Data;

public class SqlHelper : ISqlHelper
{
    public SqlHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    private readonly string _connectionString;

    public async Task<DataTable> ExecuteDataTable(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using var conn = new MySqlConnection(_connectionString);
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

    public async Task<int> ExecuteNonQuery(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using var conn = new MySqlConnection(_connectionString);
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

    public async Task<MySqlDataReader> ExecuteReader(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        var conn = new MySqlConnection(_connectionString);
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
            ? (MySqlDataReader)await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection)
            : cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }

    public async Task<object?> ExecuteScalar(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using var conn = new MySqlConnection(_connectionString);
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

    private static string NormalizeParameterName(string key, CommandType commandType)
    {
        // For stored procedures, parameter names should not start with '@'.
        return commandType == CommandType.StoredProcedure && key.StartsWith("@")
            ? key.Substring(1)
            : key;
    }
}