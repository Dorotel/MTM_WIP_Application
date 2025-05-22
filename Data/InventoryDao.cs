using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MTM_WIP_Application.Models;

namespace MTM_WIP_Application.Data;

internal static class InventoryDao
{
    // --- Inventory Save/Remove ---

    internal static async Task<List<CurrentInventory>?> InventoryTab_SaveAsync(bool useAsync = false)
    {
        try
        {
            var type = await InventoryTab_GetItemTypeAsync(useAsync);
            await ExecuteNonQueryAsync(
                "INSERT INTO `saved_locations` (`ID`, `Location`, `Item Number`, `Op`, `Notes`, `Quantity`, `Date_Time`, `User` , `Item Type`) " +
                "VALUES (NULL, @Location, @PartID, @Op, @Notes, @Quantity, CURRENT_TIMESTAMP, @User, @Type);",
                new Dictionary<string, object>
                {
                    ["@Location"] = WipAppVariables.Location,
                    ["@PartID"] = WipAppVariables.partId,
                    ["@Op"] = WipAppVariables.Operation,
                    ["@Notes"] = WipAppVariables.Notes,
                    ["@Quantity"] = WipAppVariables.InventoryQuantity,
                    ["@User"] = WipAppVariables.User,
                    ["@Type"] = type
                }, useAsync);

            await ExecuteNonQueryAsync(
                "INSERT INTO `input_history` (`User`, `Part ID`, `Location`, `Type`, `Quantity`) VALUES(@User, @PartID, @Location, @Type, @Quantity)",
                new Dictionary<string, object>
                {
                    ["@User"] = WipAppVariables.User,
                    ["@PartID"] = WipAppVariables.partId,
                    ["@Location"] = WipAppVariables.Location,
                    ["@Type"] = WipAppVariables.PartType,
                    ["@Quantity"] = WipAppVariables.InventoryQuantity
                }, useAsync);

            AppLogger.Log("InventoryTab_Save executed successfully.");
            return new List<CurrentInventory>();
        }
        catch (Exception ex)
        {
            await HandleInventoryExceptionAsync(ex, "InventoryTab_Save", useAsync);
            return null;
        }
    }

    internal static async Task<List<CurrentInventory>?> RemoveTab_DeleteAsync(bool useAsync = false)
    {
        try
        {
            await ExecuteNonQueryAsync(
                "DELETE FROM saved_locations WHERE `saved_locations`.`ID` = @RemoveID",
                new Dictionary<string, object> { ["@RemoveID"] = WipAppVariables.removeId }, useAsync);

            await ExecuteNonQueryAsync(
                "INSERT INTO `output_history` (`User`, `Part ID`, `Location`, `Quantity`) VALUES(@User, @PartID, @Location, @Quantity)",
                new Dictionary<string, object>
                {
                    ["@User"] = WipAppVariables.RemoveUser,
                    ["@PartID"] = WipAppVariables.RemovePartNumber,
                    ["@Location"] = WipAppVariables.RemoveLocation,
                    ["@Quantity"] = WipAppVariables.RemoveQuantity
                }, useAsync);

            AppLogger.Log("RemoveTab_Delete executed successfully for RemoveID: " + WipAppVariables.removeId);
            return new List<CurrentInventory>();
        }
        catch (Exception ex)
        {
            await HandleInventoryExceptionAsync(ex, "RemoveTab_Delete", useAsync);
            return null;
        }
    }

    // --- Inventory Queries ---

    internal static async Task<DataTable> GetAllInventoryAsync(bool useAsync = false)
    {
        return await GetInventoryByQueryAsync("SELECT * FROM `saved_locations` ORDER BY `Date_Time` DESC", null,
            useAsync);
    }

    internal static async Task<DataTable> GetInventoryByPartIdAsync(string partId, bool useAsync = false)
    {
        return await GetInventoryByQueryAsync(
            "SELECT * FROM `saved_locations` WHERE `Item Number` = @PartID ORDER BY `Date_Time` DESC",
            new Dictionary<string, object> { ["@PartID"] = partId }, useAsync);
    }

    internal static async Task<DataTable> GetInventoryByLocationAsync(string location, bool useAsync = false)
    {
        return await GetInventoryByQueryAsync(
            "SELECT * FROM `saved_locations` WHERE `Location` = @Location ORDER BY `Date_Time` DESC",
            new Dictionary<string, object> { ["@Location"] = location }, useAsync);
    }

    internal static async Task<DataTable> GetInventoryByDateRangeAsync(DateTime start, DateTime end,
        bool useAsync = false)
    {
        return await GetInventoryByQueryAsync(
            "SELECT * FROM `saved_locations` WHERE `Date_Time` BETWEEN @Start AND @End ORDER BY `Date_Time` DESC",
            new Dictionary<string, object> { ["@Start"] = start, ["@End"] = end }, useAsync);
    }

    internal static async Task<DataTable> GetInventoryByUserAsync(string user, bool useAsync = false)
    {
        return await GetInventoryByQueryAsync(
            "SELECT * FROM `saved_locations` WHERE `User` = @User ORDER BY `Date_Time` DESC",
            new Dictionary<string, object> { ["@User"] = user }, useAsync);
    }

    internal static async Task<DataRow?> GetInventoryByIdAsync(int id, bool useAsync = false)
    {
        var table = await GetInventoryByQueryAsync(
            "SELECT * FROM `saved_locations` WHERE `ID` = @Id",
            new Dictionary<string, object> { ["@Id"] = id }, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    // --- Inventory Updates/Deletes ---

    internal static async Task UpdateInventoryQuantityAsync(int id, int newQuantity, bool useAsync = false)
    {
        await ExecuteNonQueryAsync(
            "UPDATE `saved_locations` SET `Quantity` = @Quantity WHERE `ID` = @Id",
            new Dictionary<string, object> { ["@Id"] = id, ["@Quantity"] = newQuantity }, useAsync);
    }

    internal static async Task UpdateInventoryNotesAsync(int id, string notes, bool useAsync = false)
    {
        await ExecuteNonQueryAsync(
            "UPDATE `saved_locations` SET `Notes` = @Notes WHERE `ID` = @Id",
            new Dictionary<string, object> { ["@Id"] = id, ["@Notes"] = notes }, useAsync);
    }

    internal static async Task DeleteInventoryByIdAsync(int id, bool useAsync = false)
    {
        await ExecuteNonQueryAsync(
            "DELETE FROM `saved_locations` WHERE `ID` = @Id",
            new Dictionary<string, object> { ["@Id"] = id }, useAsync);
    }

    internal static async Task BulkDeleteInventoryByLocationAsync(string location, bool useAsync = false)
    {
        await ExecuteNonQueryAsync(
            "DELETE FROM `saved_locations` WHERE `Location` = @Location",
            new Dictionary<string, object> { ["@Location"] = location }, useAsync);
    }

    // --- Inventory Aggregates ---

    internal static async Task<int> GetTotalQuantityByPartIdAsync(string partId, bool useAsync = false)
    {
        try
        {
            var result = await SqlHelper.ExecuteScalar(
                "SELECT SUM(`Quantity`) FROM `saved_locations` WHERE `Item Number` = @PartID",
                new Dictionary<string, object> { ["@PartID"] = partId }, useAsync: useAsync);
            return result != DBNull.Value && result != null ? Convert.ToInt32(result) : 0;
        }
        catch (Exception ex)
        {
            await HandleInventoryExceptionAsync(ex, "GetTotalQuantityByPartId", useAsync);
            return 0;
        }
    }

    // --- Helper Methods ---

    private static async Task<DataTable> GetInventoryByQueryAsync(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            return parameters == null
                ? await SqlHelper.ExecuteDataTable(sql, useAsync: useAsync)
                : await SqlHelper.ExecuteDataTable(sql, parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            await HandleInventoryExceptionAsync(ex, "GetInventoryByQueryAsync", useAsync, true);
            return new DataTable();
        }
        catch (Exception ex)
        {
            await HandleInventoryExceptionAsync(ex, "GetInventoryByQueryAsync", useAsync);
            return new DataTable();
        }
    }

    private static async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object>? parameters, bool useAsync)
    {
        try
        {
            if (parameters == null)
                await SqlHelper.ExecuteNonQuery(sql, useAsync: useAsync);
            else
                await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            await HandleInventoryExceptionAsync(ex, "ExecuteNonQueryAsync", useAsync, true);
        }
        catch (Exception ex)
        {
            await HandleInventoryExceptionAsync(ex, "ExecuteNonQueryAsync", useAsync);
        }
    }

    private static async Task HandleInventoryExceptionAsync(Exception ex, string method, bool useAsync,
        bool isDbError = false)
    {
        if (isDbError || ex is MySqlException)
            AppLogger.LogDatabaseError(ex);
        else
            AppLogger.LogApplicationError(ex);

        AppLogger.Log($"Error in {method}: {ex.Message}");
        if (ex is MySqlException)
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        else
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
    }

    // --- Item Type Lookup ---

    internal static async Task<string> InventoryTab_GetItemTypeAsync(bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@PartID"] = WipAppVariables.partId };
            using var reader = useAsync
                ? await SqlHelper.ExecuteReader("SELECT * FROM `part_ids` WHERE `Item Number` = @PartID", parameters,
                    useAsync: true)
                : SqlHelper.ExecuteReader("SELECT * FROM `part_ids` WHERE `Item Number` = @PartID", parameters).Result;
            while (reader.Read())
            {
                var partId = reader.GetString(0);
                if (partId == WipAppVariables.partId)
                    return reader.GetString(3);
            }

            AppLogger.Log("InventoryTab_GetItemType executed successfully for part ID: " + WipAppVariables.partId);
            return "Unknown";
        }
        catch (MySqlException ex)
        {
            await HandleInventoryExceptionAsync(ex, "InventoryTab_GetItemType", useAsync, true);
            return "Unknown";
        }
        catch (Exception ex)
        {
            await HandleInventoryExceptionAsync(ex, "InventoryTab_GetItemType", useAsync);
            return "Unknown";
        }
    }
}