using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_ItemType

internal static class Dao_ItemType
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

    internal static async Task DeleteItemType(string itemType, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["p_Type"] = itemType };
        await ExecuteStoredProcedureAsync("md_item_types_Delete_ByType", parameters, useAsync);
    }

    #endregion

    #region Insert

    internal static async Task InsertItemType(string itemType, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["p_Type"] = itemType,
            ["p_IssuedBy"] = user
        };
        await ExecuteStoredProcedureAsync("md_item_types_Add_Type", parameters, useAsync);
    }

    #endregion

    #region Update

    internal static async Task UpdateItemType(int id, string newItemType, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["p_ID"] = id,
            ["p_Type"] = newItemType,
            ["p_IssuedBy"] = user
        };
        await ExecuteStoredProcedureAsync("md_item_types_Update_Type", parameters, useAsync);
    }

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllItemTypes(bool useAsync = false)
    {
        return await GetItemTypeByStoredProcedureAsync("md_item_types_Get_All", null, useAsync);
    }

    internal static async Task<DataRow?> GetItemTypeByName(string itemType, bool useAsync = false)
    {
        var table = await GetAllItemTypes(useAsync);
        var rows = table.Select($"Type = '{itemType.Replace("'", "''")}'");
        return rows.Length > 0 ? rows[0] : null;
    }

    #endregion

    #region Existence Check

    internal static async Task<bool> ItemTypeExists(string itemType, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@itemType"] = itemType };
        var result = await HelperDatabaseCore.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_item_types` WHERE `Type` = @itemType",
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

    private static async Task<DataTable> GetItemTypeByStoredProcedureAsync(string storedProcedure, Dictionary<string, object>? parameters, bool useAsync)
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

    private static async Task<DataTable> GetItemTypeByQueryAsync(string sql, Dictionary<string, object>? parameters,
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