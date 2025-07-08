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
        var parameters = new Dictionary<string, object> { ["@itemType"] = itemType };
        await ExecuteNonQueryAsync(
            "DELETE FROM `md_item_types` WHERE `Type` = @itemType",
            parameters, useAsync);
    }

    #endregion

    #region Insert

    internal static async Task InsertItemType(string itemType, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@itemType"] = itemType,
            ["@user"] = user
        };
        await ExecuteNonQueryAsync(
            "INSERT INTO `md_item_types` (`Type`, `Issued By`) VALUES (@itemType, @user);",
            parameters, useAsync);
    }

    #endregion

    #region Update

    internal static async Task UpdateItemType(string itemType, string newItemType, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@itemType"] = itemType,
            ["@newItemType"] = newItemType,
            ["@user"] = user
        };
        await ExecuteNonQueryAsync(
            "UPDATE `md_item_types` SET `Type` = @newItemType, `Issued By` = @user WHERE `Type` = @itemType",
            parameters, useAsync);
    }

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllItemTypes(bool useAsync = false)
    {
        return await GetItemTypeByQueryAsync("SELECT * FROM `md_item_types`", null, useAsync);
    }

    internal static async Task<DataRow?> GetItemTypeByName(string itemType, bool useAsync = false)
    {
        var table = await GetItemTypeByQueryAsync(
            "SELECT * FROM `md_item_types` WHERE `Type` = @itemType",
            new Dictionary<string, object> { ["@itemType"] = itemType }, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
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