using System.Data;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Data;

#region Dao_Operation

internal static class Dao_Operation
{
    #region Fields

    public static Helper_MySql HelperMySql =
        new(Helper_SqlVariables.GetConnectionString(
            Core_WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            Core_WipAppVariables.User,
            Core_WipAppVariables.UserPin
        ));

    #endregion

    #region Delete

    internal static async Task DeleteOperation(string operationNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@operationNumber"] = operationNumber };
        await ExecuteNonQueryAsync(
            "DELETE FROM `md_operation_numbers` WHERE `Operation` = @operationNumber",
            parameters, useAsync);
    }

    #endregion

    #region Insert

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

    #endregion

    #region Update

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

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllOperations(bool useAsync = false)
    {
        return await GetOperationByQueryAsync("SELECT * FROM `md_operation_numbers`", null, useAsync);
    }

    internal static async Task<DataRow?> GetOperationByNumber(string operationNumber, bool useAsync = false)
    {
        var table = await GetOperationByQueryAsync(
            "SELECT * FROM `md_operation_numbers` WHERE `Operation` = @operationNumber",
            new Dictionary<string, object> { ["@operationNumber"] = operationNumber }, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    #endregion

    #region Existence Check

    internal static async Task<bool> OperationExists(string operationNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@operationNumber"] = operationNumber };
        var result = await HelperMySql.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_operation_numbers` WHERE `Operation` = @operationNumber",
            parameters, useAsync);
        return Convert.ToInt32(result) > 0;
    }

    #endregion

    #region Helpers

    private static async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters, bool useAsync)
    {
        try
        {
            await HelperMySql.ExecuteNonQuery(sql, parameters, useAsync);
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    private static async Task<DataTable> GetOperationByQueryAsync(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            return parameters == null
                ? await HelperMySql.ExecuteDataTable(sql, useAsync: useAsync)
                : await HelperMySql.ExecuteDataTable(sql, parameters, useAsync);
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    #endregion
}

#endregion