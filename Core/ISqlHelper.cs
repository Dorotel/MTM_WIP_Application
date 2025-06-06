using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Core;

public interface ISqlHelper
{
    Task<int> ExecuteNonQuery(string procedureName, Dictionary<string, object>? parameters = null,
        bool useAsync = false, CommandType commandType = CommandType.StoredProcedure);

    Task<DataTable> ExecuteDataTable(string procedureName, Dictionary<string, object>? parameters = null,
        bool useAsync = false, CommandType commandType = CommandType.StoredProcedure);

    Task<object?> ExecuteScalar(string procedureName, Dictionary<string, object>? parameters = null,
        bool useAsync = false, CommandType commandType = CommandType.StoredProcedure);

    Task<MySqlDataReader> ExecuteReader(string procedureName, Dictionary<string, object>? parameters = null,
        bool useAsync = false, CommandType commandType = CommandType.StoredProcedure);
}