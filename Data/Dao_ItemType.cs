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

    internal static async Task DeleteItemType(string itemType, bool useAsync = false)
    {
        Dictionary<string, object> parameters = new() { ["p_ItemType"] = itemType };
        try
        {
            await HelperDatabaseCore.ExecuteNonQuery(
                "md_item_types_Delete_ByType",
                parameters,
                useAsync,
                CommandType.StoredProcedure
            );
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

    internal static async Task InsertItemType(string itemType, string user, bool useAsync = false)
    {
        Dictionary<string, object> parameters = new() { ["p_ItemType"] = itemType, ["p_IssuedBy"] = user };

        await HelperDatabaseCore.ExecuteNonQuery(
            "md_item_types_Add_ItemType",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    #endregion

    #region Update

    internal static async Task UpdateItemType(int id, string newItemType, string user, bool useAsync = false)
    {
        Dictionary<string, object> parameters = new()
        {
            ["p_ID"] = id, ["p_ItemType"] = newItemType, ["p_IssuedBy"] = user
        };
        await HelperDatabaseCore.ExecuteNonQuery("md_item_types_Update_ItemType", parameters, useAsync,
            CommandType.StoredProcedure);
    }

    internal static async Task<DataTable> GetAllItemTypes(bool useAsync = false) =>
        await HelperDatabaseCore.ExecuteDataTable("md_item_types_Get_All", null, useAsync,
            CommandType.StoredProcedure);

    internal static async Task<DataRow?> GetItemTypeByName(string itemType, bool useAsync = false)
    {
        DataTable table = await GetAllItemTypes(useAsync);
        DataRow[] rows = table.Select($"ItemType = '{itemType.Replace("'", "''")}'");
        return rows.Length > 0 ? rows[0] : null;
    }

    #endregion

    #region Existence Check

    internal static async Task<bool> ItemTypeExists(string itemType, bool useAsync = false)
    {
        Dictionary<string, object> parameters = new() { ["@itemType"] = itemType };
        object? result = await HelperDatabaseCore.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_item_types` WHERE `ItemType` = @itemType",
            parameters, useAsync, CommandType.Text);
        return Convert.ToInt32(result) > 0;
    }

    #endregion
}

#endregion
