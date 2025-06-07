using MTM_WIP_Application.Core;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Data;

public static class InventoryDao
{
    private static readonly SqlHelper SqlHelper = new(
        SqlVariables.GetConnectionString(
            WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            WipAppVariables.User,
            WipAppVariables.UserPin
        )
    );

    public static async Task<int> AddItemAsync(
        string partId, string location, string operation, int quantity,
        string itemType, string user, string batchNumber, string notes,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_PartID", partId },
            { "p_Location", location },
            { "p_Operation", operation },
            { "p_Quantity", quantity },
            { "p_ItemType", itemType },
            { "p_User", user },
            { "p_BatchNumber", batchNumber },
            { "p_Notes", notes }
        };

        return await SqlHelper.ExecuteNonQuery(
            "inv_inventory_Add_Item",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    public static async Task<int> BulkDeleteByLocationAsync(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_Location", location }
        };

        return await SqlHelper.ExecuteNonQuery(
            "inv_inventory_BulkDelete_ByLocation",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    public static async Task<int> DeleteByIdAsync(int id, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_ID", id }
        };

        return await SqlHelper.ExecuteNonQuery(
            "inv_inventory_Delete_ById",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    public static async Task<DataTable> GetAllAsync(bool useAsync = false)
    {
        return await SqlHelper.ExecuteDataTable(
            "inv_inventory_Get_All",
            null,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    public static async Task<DataTable> GetByDateRangeAsync(DateTime startDate, DateTime endDate, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_StartDate", startDate },
            { "p_EndDate", endDate }
        };

        return await SqlHelper.ExecuteDataTable(
            "inv_inventory_Get_ByDateRange",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    public static async Task<DataRow?> GetByIdAsync(int id, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_ID", id }
        };

        var table = await SqlHelper.ExecuteDataTable(
            "inv_inventory_Get_ById",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );

        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    public static async Task<DataTable> GetByItemTypeAsync(string itemType, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_ItemType", itemType }
        };

        return await SqlHelper.ExecuteDataTable(
            "inv_inventory_Get_ByItemType",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    public static async Task<DataTable> GetByLocationAsync(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_Location", location }
        };

        return await SqlHelper.ExecuteDataTable(
            "inv_inventory_Get_ByLocation",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    public static async Task<DataTable> GetByPartIDLocationAsync(string partId, string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_PartID", partId },
            { "p_Location", location }
        };

        return await SqlHelper.ExecuteDataTable(
            "inv_inventory_Get_ByPartIDLocation",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    public static async Task<DataTable> GetByUserAsync(string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_User", user }
        };

        return await SqlHelper.ExecuteDataTable(
            "inv_inventory_Get_ByUser",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    public static async Task<int> GetTotalQuantityByPartIdAsync(string partId, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_PartID", partId }
        };

        var table = await SqlHelper.ExecuteDataTable(
            "inv_inventory_Get_TotalQuantityByPartId",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );

        if (table.Rows.Count > 0 && int.TryParse(table.Rows[0]["TotalQuantity"].ToString(), out var totalQuantity))
            return totalQuantity;

        return 0;
    }

    public static async Task<int> UpdateNotesAsync(int id, string notes, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_ID", id },
            { "p_Notes", notes }
        };

        return await SqlHelper.ExecuteNonQuery(
            "inv_inventory_Update_Notes",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    public static async Task<int> UpdateQuantityAsync(int id, int quantity, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            { "p_ID", id },
            { "p_Quantity", quantity }
        };

        return await SqlHelper.ExecuteNonQuery(
            "inv_inventory_Update_Quantity",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }
}