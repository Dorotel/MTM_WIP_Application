using System.Data;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Data;

internal static class OperationDao
{
    public static SqlHelper SqlHelper =
        new(SqlVariables.GetConnectionString(
            WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            WipAppVariables.User,
            WipAppVariables.UserPin
        ));

    // --- Delete ---
    internal static async Task DeleteOperation(string operationNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@operationNumber"] = operationNumber };
        await ExecuteNonQueryAsync(
            "DELETE FROM `md_operation_numbers` WHERE `Operation` = @operationNumber",
            parameters, useAsync);
    }

    private static async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters, bool useAsync)
    {
        try
        {
            await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    // --- Get All ---
    internal static async Task<DataTable> GetAllOperations(bool useAsync = false)
    {
        return await GetOperationByQueryAsync("SELECT * FROM `md_operation_numbers`", null, useAsync);
    }

    // --- Get By Number ---
    internal static async Task<DataRow?> GetOperationByNumber(string operationNumber, bool useAsync = false)
    {
        var table = await GetOperationByQueryAsync(
            "SELECT * FROM `md_operation_numbers` WHERE `Operation` = @operationNumber",
            new Dictionary<string, object> { ["@operationNumber"] = operationNumber }, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    // --- Helpers ---
    private static async Task<DataTable> GetOperationByQueryAsync(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            return parameters == null
                ? await SqlHelper.ExecuteDataTable(sql, useAsync: useAsync)
                : await SqlHelper.ExecuteDataTable(sql, parameters, useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    // --- Insert ---
    internal static async Task InsertOperation(string operationNumber, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@operationNumber"] = operationNumber,
            ["@user"] = user
        };
        await ExecuteNonQueryAsync(
            "INSERT INTO `md_operation_numbers` (`Operation`, `Issued By`) VALUES (@operationNumber, @user);",
            parameters, useAsync);
    }

    // --- Existence Check ---
    internal static async Task<bool> OperationExists(string operationNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@operationNumber"] = operationNumber };
        var result = await SqlHelper.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_operation_numbers` WHERE `Operation` = @operationNumber",
            parameters, useAsync);
        return Convert.ToInt32(result) > 0;
    }

    // --- Update ---
    internal static async Task UpdateOperation(string operationNumber, string newOperationNumber, string user,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@operationNumber"] = operationNumber,
            ["@newOperationNumber"] = newOperationNumber,
            ["@user"] = user
        };
        await ExecuteNonQueryAsync(
            "UPDATE `md_operation_numbers` SET `Operation` = @newOperationNumber, `Issued By` = @user WHERE `Operation` = @operationNumber",
            parameters, useAsync);
    }
}