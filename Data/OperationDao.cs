using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Data;

internal static class OperationDao
{
    internal static async Task<bool> OperationExists(string operationNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@operationNumber"] = operationNumber
        };
        var result = await SqlHelper.ExecuteScalar(
            "SELECT COUNT(*) FROM `operation_numbers` WHERE `Operation` = @operationNumber",
            parameters,
            useAsync: useAsync);
        return Convert.ToInt32(result) > 0;
    }

    internal static async Task InsertOperation(string operationNumber, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@operationNumber"] = operationNumber,
            ["@user"] = user
        };
        await SqlHelper.ExecuteNonQuery(
            "INSERT INTO `operation_numbers` (`Operation`, `ID`, `Issued By`) VALUES (@operationNumber, NULL, @user);",
            parameters,
            useAsync: useAsync);
    }

    internal static async Task<DataTable> GetAllOperations(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable("SELECT * FROM `operation_numbers`", useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
            return new DataTable();
        }
    }

    internal static async Task<DataRow?> GetOperationByNumber(string operationNumber, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@operationNumber"] = operationNumber
            };
            var table = await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `operation_numbers` WHERE `Operation` = @operationNumber",
                parameters,
                useAsync: useAsync);
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
            return null;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
            return null;
        }
    }

    internal static async Task UpdateOperation(string operationNumber, string newOperationNumber, string user,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@operationNumber"] = operationNumber,
                ["@newOperationNumber"] = newOperationNumber,
                ["@user"] = user
            };
            await SqlHelper.ExecuteNonQuery(
                "UPDATE `operation_numbers` SET `Operation` = @newOperationNumber, `Issued By` = @user WHERE `Operation` = @operationNumber",
                parameters,
                useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }

    internal static async Task DeleteOperation(string operationNumber, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@operationNumber"] = operationNumber
            };
            await SqlHelper.ExecuteNonQuery(
                "DELETE FROM `operation_numbers` WHERE `Operation` = @operationNumber",
                parameters,
                useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }
}