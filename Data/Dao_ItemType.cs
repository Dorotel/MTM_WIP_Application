using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Data;

#region Dao_ItemType

internal static class Dao_ItemType
{
    #region Delete

    internal static async Task DeleteItemType(string itemType, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new() { ["ItemType"] = itemType }; // p_ prefix added automatically

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_item_types_Delete_ByType",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (!result.IsSuccess)
            {
                LoggingUtility.Log($"DeleteItemType failed: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "DeleteItemType");
        }
    }

    internal static async Task InsertItemType(string itemType, string user, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new()
            {
                ["ItemType"] = itemType,  // p_ prefix added automatically
                ["IssuedBy"] = user
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_item_types_Add_ItemType",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (!result.IsSuccess)
            {
                LoggingUtility.Log($"InsertItemType failed: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "InsertItemType");
        }
    }

    #endregion

    #region Update

    internal static async Task UpdateItemType(int id, string newItemType, string user, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new()
            {
                ["ID"] = id,                    // p_ prefix added automatically
                ["ItemType"] = newItemType,
                ["IssuedBy"] = user
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_item_types_Update_ItemType",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (!result.IsSuccess)
            {
                LoggingUtility.Log($"UpdateItemType failed: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "UpdateItemType");
        }
    }

    internal static async Task<DataTable> GetAllItemTypes(bool useAsync = false)
    {
        try
        {
            var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "md_item_types_Get_All",
                null, // No parameters needed
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess && result.Data != null)
            {
                return result.Data;
            }
            else
            {
                LoggingUtility.Log($"GetAllItemTypes failed: {result.ErrorMessage}");
                return new DataTable();
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "GetAllItemTypes");
            return new DataTable();
        }
    }

    internal static async Task<DataRow?> GetItemTypeByName(string itemType, bool useAsync = false)
    {
        try
        {
            DataTable table = await GetAllItemTypes(useAsync);
            DataRow[] rows = table.Select($"ItemType = '{itemType.Replace("'", "''")}'");
            return rows.Length > 0 ? rows[0] : null;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "GetItemTypeByName");
            return null;
        }
    }

    #endregion

    #region Existence Check

    internal static async Task<bool> ItemTypeExists(string itemType, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new() { ["ItemType"] = itemType }; // p_ prefix added automatically

            var result = await Helper_Database_StoredProcedure.ExecuteScalarWithStatus(
                Model_AppVariables.ConnectionString,
                "md_item_types_Exists_ByType",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess && result.Data != null)
            {
                return Convert.ToInt32(result.Data) > 0;
            }
            else
            {
                LoggingUtility.Log($"ItemTypeExists failed: {result.ErrorMessage}");
                return false;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "ItemTypeExists");
            return false;
        }
    }

    #endregion
}

#endregion
