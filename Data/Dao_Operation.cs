using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_Operation

internal static class Dao_Operation
{
    #region Fields

    public static Helper_Database_Core HelperDatabaseCore =
        new(Helper_Database_Variables.GetConnectionString(
            Model_AppVariables.WipServerAddress,
            "mtm_wip_application",
            Model_AppVariables.User,
            Model_AppVariables.UserPin
        ));

    #endregion

    #region Delete

    internal static async Task DeleteOperation(string operationNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["p_Operation"] = operationNumber };
        await ExecuteStoredProcedureAsync("md_operation_numbers_Delete_ByOperation", parameters, useAsync);
    }

    #endregion

    #region Insert

    internal static async Task InsertOperation(string operationNumber, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["p_Operation"] = operationNumber,
            ["p_IssuedBy"] = user
        };
        await ExecuteStoredProcedureAsync("md_operation_numbers_Add_Operation", parameters, useAsync);
    }

    #endregion

    #region Update

    internal static async Task UpdateOperation(int id, string newOperationNumber, string user,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["p_ID"] = id,
            ["p_Operation"] = newOperationNumber,
            ["p_IssuedBy"] = user
        };
        await ExecuteStoredProcedureAsync("md_operation_numbers_Update_Operation", parameters, useAsync);
    }

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllOperations(bool useAsync = false)
    {
        return await GetOperationByStoredProcedureAsync("md_operation_numbers_Get_All", null, useAsync);
    }

    internal static async Task<DataRow?> GetOperationByNumber(string operationNumber, bool useAsync = false)
    {
        var table = await GetAllOperations(useAsync);
        var rows = table.Select($"Operation = '{operationNumber.Replace("'", "''")}'");
        return rows.Length > 0 ? rows[0] : null;
    }

    #endregion

    #region Existence Check

    internal static async Task<bool> OperationExists(string operationNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@operationNumber"] = operationNumber };
        var result = await HelperDatabaseCore.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_operation_numbers` WHERE `Operation` = @operationNumber",
            parameters, useAsync);
        return Convert.ToInt32(result) > 0;
    }

    #endregion

    #region Helpers

    private static async Task ExecuteStoredProcedureAsync(string storedProcedure, Dictionary<string, object> parameters, bool useAsync)
    {
        try
        {
            await HelperDatabaseCore.ExecuteStoredProcedure(storedProcedure, parameters, useAsync);
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

    private static async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters, bool useAsync)
    {
        try
        {
            await HelperDatabaseCore.ExecuteNonQuery(sql, parameters, useAsync);
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

    private static async Task<DataTable> GetOperationByStoredProcedureAsync(string storedProcedure, Dictionary<string, object>? parameters, bool useAsync)
    {
        try
        {
            return parameters == null
                ? await HelperDatabaseCore.ExecuteStoredProcedureDataTable(storedProcedure, useAsync: useAsync)
                : await HelperDatabaseCore.ExecuteStoredProcedureDataTable(storedProcedure, parameters, useAsync);
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

    private static async Task<DataTable> GetOperationByQueryAsync(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            return parameters == null
                ? await HelperDatabaseCore.ExecuteDataTable(sql, useAsync: useAsync)
                : await HelperDatabaseCore.ExecuteDataTable(sql, parameters, useAsync);
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