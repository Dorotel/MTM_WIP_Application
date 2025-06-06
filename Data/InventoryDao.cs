using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using MTM_WIP_Application.Core; // Adjust if your ISqlHelper is elsewhere

public static class InventoryDao
{
    // Settable property for testability; default is a real SqlHelper with your connection string
    public static ISqlHelper SqlHelper { get; set; } = new SqlHelper("your-connection-string-here");

    public static async Task<DataTable> GetAllInventoryAsync(bool useAsync = false)
    {
        return await SqlHelper.ExecuteDataTable(
            "mtm_wip_application.GetAllInventory",
            null, useAsync, CommandType.StoredProcedure);
    }

    public static async Task<DataRow?> GetInventoryByIdAsync(int id, bool useAsync = false)
    {
        var table = await SqlHelper.ExecuteDataTable(
            "mtm_wip_application.GetInventoryById",
            new Dictionary<string, object> { { "p_ID", id } },
            useAsync, CommandType.StoredProcedure);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    public static async Task<DataTable> GetInventoryByLocationAsync(string location, bool useAsync = false)
    {
        return await SqlHelper.ExecuteDataTable(
            "mtm_wip_application.GetInventoryByLocation",
            new Dictionary<string, object> { { "p_Location", location } },
            useAsync, CommandType.StoredProcedure);
    }

    public static async Task<DataTable> GetInventoryByUserAsync(string user, bool useAsync = false)
    {
        return await SqlHelper.ExecuteDataTable(
            "mtm_wip_application.GetInventoryByUser",
            new Dictionary<string, object> { { "p_User", user } },
            useAsync, CommandType.StoredProcedure);
    }

    public static async Task<DataTable> GetInventoryByDateRangeAsync(DateTime start, DateTime end,
        bool useAsync = false)
    {
        return await SqlHelper.ExecuteDataTable(
            "mtm_wip_application.GetInventoryByDateRange",
            new Dictionary<string, object> { { "p_StartDate", start }, { "p_EndDate", end } },
            useAsync, CommandType.StoredProcedure);
    }

    public static async Task<int> GetTotalQuantityByPartIdAsync(string partId, bool useAsync = false)
    {
        var table = await SqlHelper.ExecuteDataTable(
            "mtm_wip_application.GetTotalQuantityByPartId",
            new Dictionary<string, object> { { "p_PartID", partId } },
            useAsync, CommandType.StoredProcedure);
        if (table.Rows.Count > 0 && int.TryParse(table.Rows[0][0].ToString(), out var qty))
            return qty;
        return 0;
    }

    public static async Task<int> AddInventoryItemAsync(
        string partId, string location, string operation, int quantity, string itemType,
        string user, string batchNumber, string notes, bool useAsync = false)
    {
        return await SqlHelper.ExecuteNonQuery(
            "mtm_wip_application.AddInventoryItem",
            new Dictionary<string, object>
            {
                { "p_PartID", partId },
                { "p_Location", location },
                { "p_Operation", operation },
                { "p_Quantity", quantity },
                { "p_ItemType", itemType },
                { "p_User", user },
                { "p_BatchNumber", batchNumber },
                { "p_Notes", notes }
            },
            useAsync, CommandType.StoredProcedure);
    }

    public static async Task<int> UpdateInventoryQuantityAsync(int id, int quantity, bool useAsync = false)
    {
        return await SqlHelper.ExecuteNonQuery(
            "mtm_wip_application.UpdateInventoryQuantity",
            new Dictionary<string, object>
            {
                { "p_ID", id },
                { "p_Quantity", quantity }
            },
            useAsync, CommandType.StoredProcedure);
    }

    public static async Task<int> UpdateInventoryNotesAsync(int id, string notes, bool useAsync = false)
    {
        return await SqlHelper.ExecuteNonQuery(
            "mtm_wip_application.UpdateInventoryNotes",
            new Dictionary<string, object>
            {
                { "p_ID", id },
                { "p_Notes", notes }
            },
            useAsync, CommandType.StoredProcedure);
    }

    public static async Task<int> DeleteInventoryByIdAsync(int id, bool useAsync = false)
    {
        return await SqlHelper.ExecuteNonQuery(
            "mtm_wip_application.DeleteInventoryItem",
            new Dictionary<string, object> { { "p_ID", id } },
            useAsync, CommandType.StoredProcedure);
    }

    public static async Task<int> BulkDeleteInventoryByLocationAsync(string location, bool useAsync = false)
    {
        return await SqlHelper.ExecuteNonQuery(
            "mtm_wip_application.BulkDeleteInventoryByLocation",
            new Dictionary<string, object> { { "p_Location", location } },
            useAsync, CommandType.StoredProcedure);
    }
}