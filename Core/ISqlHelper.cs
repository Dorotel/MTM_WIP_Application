using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Core;

#region ISqlHelper Interface

public interface ISqlHelper
{
    Task<int> ExecuteNonQuery(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure);

    Task<DataTable> ExecuteDataTable(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure);

    Task<object?> ExecuteScalar(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure);

    Task<MySqlDataReader> ExecuteReader(
        string procedureOrSql,
        Dictionary<string, object>? parameters = null,
        bool useAsync = false,
        CommandType commandType = CommandType.StoredProcedure);
}

#endregion