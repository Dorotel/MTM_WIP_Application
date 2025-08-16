using MTM_MAUI_Application.Models;
using System.Data;

namespace MTM_MAUI_Application.Services;

/// <summary>
/// Interface for inventory operations
/// </summary>
public interface IInventoryService
{
    Task<List<InventoryItem>> GetInventoryByPartIdAsync(string partId);
    Task<List<InventoryItem>> GetInventoryByPartIdAndOperationAsync(string partId, string operation);
    Task<bool> AddInventoryItemAsync(InventoryItem item);
    Task<bool> RemoveInventoryItemAsync(int inventoryId, int quantity, string user, string notes = "");
    Task<bool> TransferInventoryAsync(int inventoryId, string toLocation, int quantity, string user, string notes = "");
    Task<List<InventoryItem>> SearchInventoryAsync(string searchTerm);
}

/// <summary>
/// Inventory service implementation
/// </summary>
public class InventoryService : IInventoryService
{
    private readonly IDatabaseService _database;
    private readonly ILoggingService _logging;

    public InventoryService(IDatabaseService database, ILoggingService logging)
    {
        _database = database;
        _logging = logging;
    }

    public async Task<List<InventoryItem>> GetInventoryByPartIdAsync(string partId)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "PartId", partId }
            };

            var dataTable = await _database.ExecuteStoredProcedureAsync("inv_inventory_Get_ByPartId", parameters);
            return ConvertToInventoryItems(dataTable);
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error getting inventory for part {partId}", ex);
            return new List<InventoryItem>();
        }
    }

    public async Task<List<InventoryItem>> GetInventoryByPartIdAndOperationAsync(string partId, string operation)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "PartId", partId },
                { "Operation", operation }
            };

            var dataTable = await _database.ExecuteStoredProcedureAsync("inv_inventory_Get_ByPartIdAndOperation", parameters);
            return ConvertToInventoryItems(dataTable);
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error getting inventory for part {partId} and operation {operation}", ex);
            return new List<InventoryItem>();
        }
    }

    public async Task<bool> AddInventoryItemAsync(InventoryItem item)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "PartId", item.PartId },
                { "Location", item.Location },
                { "Operation", item.Operation },
                { "Quantity", item.Quantity },
                { "BatchNumber", item.BatchNumber ?? string.Empty },
                { "Notes", item.Notes ?? string.Empty },
                { "User", item.User },
                { "ItemType", item.ItemType }
            };

            await _database.ExecuteNonQueryAsync("inv_inventory_Add_Item", parameters);
            await _logging.LogInfoAsync($"Added inventory item: {item.PartId} at {item.Location}");
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error adding inventory item for part {item.PartId}", ex);
            return false;
        }
    }

    public async Task<bool> RemoveInventoryItemAsync(int inventoryId, int quantity, string user, string notes = "")
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "InventoryId", inventoryId },
                { "Quantity", quantity },
                { "User", user },
                { "Notes", notes }
            };

            await _database.ExecuteNonQueryAsync("inv_inventory_Remove_Item", parameters);
            await _logging.LogInfoAsync($"Removed {quantity} from inventory ID {inventoryId}");
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error removing inventory item {inventoryId}", ex);
            return false;
        }
    }

    public async Task<bool> TransferInventoryAsync(int inventoryId, string toLocation, int quantity, string user, string notes = "")
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "InventoryId", inventoryId },
                { "ToLocation", toLocation },
                { "Quantity", quantity },
                { "User", user },
                { "Notes", notes }
            };

            await _database.ExecuteNonQueryAsync("inv_inventory_Transfer_Item", parameters);
            await _logging.LogInfoAsync($"Transferred {quantity} from inventory ID {inventoryId} to {toLocation}");
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error transferring inventory item {inventoryId}", ex);
            return false;
        }
    }

    public async Task<List<InventoryItem>> SearchInventoryAsync(string searchTerm)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "SearchTerm", searchTerm }
            };

            var dataTable = await _database.ExecuteStoredProcedureAsync("inv_inventory_Search", parameters);
            return ConvertToInventoryItems(dataTable);
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error searching inventory with term {searchTerm}", ex);
            return new List<InventoryItem>();
        }
    }

    private List<InventoryItem> ConvertToInventoryItems(DataTable dataTable)
    {
        var items = new List<InventoryItem>();

        foreach (DataRow row in dataTable.Rows)
        {
            items.Add(new InventoryItem
            {
                Id = row.Field<int>("ID") ?? 0,
                PartId = row.Field<string>("PartID") ?? string.Empty,
                Location = row.Field<string>("Location") ?? string.Empty,
                Operation = row.Field<string>("Operation") ?? string.Empty,
                Quantity = row.Field<int>("Quantity") ?? 0,
                BatchNumber = row.Field<string>("BatchNumber"),
                Notes = row.Field<string>("Notes"),
                User = row.Field<string>("User") ?? string.Empty,
                ItemType = row.Field<string>("ItemType") ?? string.Empty,
                CreatedDate = row.Field<DateTime>("CreatedDate") ?? DateTime.Now,
                ModifiedDate = row.Field<DateTime?>("ModifiedDate")
            });
        }

        return items;
    }
}