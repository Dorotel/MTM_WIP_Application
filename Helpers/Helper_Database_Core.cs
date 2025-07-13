using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MTM_Inventory_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Helpers;

#region Helper_Database_Core

public class Helper_Database_Core(string connectionString)
{
    #region Parameter Normalization

    private static string NormalizeParameterName(string key, CommandType commandType)
    {
        return commandType == CommandType.StoredProcedure && key.StartsWith("@")
            ? key[1..]
            : key;
    }

    #endregion

    #region Execute NonQuery

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
        try
        {
            LoggingUtility.Log($"Opening MySQL connection for ExecuteNonQuery: {procedureOrSql}");
            await conn.OpenAsync();
            var result = useAsync
                ? await cmd.ExecuteNonQueryAsync()
                : cmd.ExecuteNonQuery();
            LoggingUtility.Log($"ExecuteNonQuery succeeded: {procedureOrSql}");
            return result;
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            throw;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    #endregion

    #region Execute DataTable

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
                if (param.Value is MySqlParameter mySqlParam)
                    cmd.Parameters.Add(mySqlParam);
                else
                    cmd.Parameters.AddWithValue(NormalizeParameterName(param.Key, commandType),
                        param.Value ?? DBNull.Value);
        try
        {
            LoggingUtility.Log($"Opening MySQL connection for ExecuteDataTable: {procedureOrSql}");
            await conn.OpenAsync();
            var dt = new DataTable();
            if (useAsync)
            {
                using var reader = await cmd.ExecuteReaderAsync();
                dt.Load(reader);
            }
            else
            {
                using var reader = cmd.ExecuteReader();
                dt.Load(reader);
            }

            LoggingUtility.Log($"ExecuteDataTable succeeded: {procedureOrSql}");
            return dt;
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            throw;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    #endregion

    #region Execute Scalar

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
        try
        {
            LoggingUtility.Log($"Opening MySQL connection for ExecuteScalar: {procedureOrSql}");
            await conn.OpenAsync();
            var result = useAsync
                ? await cmd.ExecuteScalarAsync()
                : cmd.ExecuteScalar();
            LoggingUtility.Log($"ExecuteScalar succeeded: {procedureOrSql}");
            return result;
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            throw;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    #endregion

    #region Execute Reader

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
        try
        {
            LoggingUtility.Log($"Opening MySQL connection for ExecuteReader: {procedureOrSql}");
            await conn.OpenAsync();
            var reader = useAsync
                ? await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection)
                : cmd.ExecuteReader(CommandBehavior.CloseConnection);
            LoggingUtility.Log($"ExecuteReader succeeded: {procedureOrSql}");
            return reader;
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            throw;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    #endregion
}

#endregion