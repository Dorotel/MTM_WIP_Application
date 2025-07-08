using System.Data;
using MTM_Inventory_Application.Helpers;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_Inventory

public static class Dao_Inventory
{
    #region Fields

    private static readonly Helper_Database_Core HelperDatabaseCore =
        new(Helper_Database_Variables.GetConnectionString(null, null, null, null));

    #endregion

    #region Search Methods

    public static async Task<DataTable> GetInventoryByPartIdAsync(string partId, bool useAsync = false)
    {
        // Ensure the stored procedure returns BatchNumber AS 'Batch Number'
        return await HelperDatabaseCore.ExecuteDataTable(
            "mtm_wip_application.inv_inventory_Get_ByPartID",
            new Dictionary<string, object> { { "p_PartID", partId } },
            useAsync, CommandType.StoredProcedure);
    }

    public static async Task<DataTable> GetInventoryByPartIdAndOperationAsync(string partId, string operation,
        bool useAsync = false)
    {
        // Ensure the stored procedure returns BatchNumber AS 'Batch Number'
        return await HelperDatabaseCore.ExecuteDataTable(
            "mtm_wip_application.inv_inventory_Get_ByPartIDAndOperation",
            new Dictionary<string, object>
            {
                { "p_PartID", partId },
                { "o_Operation", operation }
            },
            useAsync, CommandType.StoredProcedure);
    }

    #endregion

    #region Modification Methods

    public static async Task<int> AddInventoryItemAsync(
        string partId, string location, string operation, int quantity, string itemType,
        string user, string batchNumber, string notes, bool useAsync = false)
    {
        return await HelperDatabaseCore.ExecuteNonQuery(
            "inv_inventory_Add_Item",
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

    public static async Task<int> DeleteInventoryByPartIdLocationOperationQuantityAsync(
        string partId,
        string location,
        string operation,
        string batchNumber,
        int quantity,
        bool useAsync = false)
    {
        return await HelperDatabaseCore.ExecuteNonQuery(
            "mtm_wip_application.inv_inventory_Remove_Item",
            new Dictionary<string, object>
            {
                { "p_PartID", partId },
                { "p_Location", location },
                { "p_Operation", operation },
                { "p_Quantity", quantity },
                { "p_BatchNumber", batchNumber }
            },
            useAsync, CommandType.StoredProcedure);
    }

    public static async Task TransferPartSimpleAsync(string batchNumber, string partId, string operation,
        string quantity, string newLocation)
    {
        var connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);

        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        await using var command = new MySqlCommand("inv_inventory_Transfer_Part", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@in_BatchNumber", batchNumber);
        command.Parameters.AddWithValue("@in_PartID", partId);
        command.Parameters.AddWithValue("@in_Operation", operation);
        command.Parameters.AddWithValue("@in_Quantity", quantity);
        command.Parameters.AddWithValue("@in_NewLocation", newLocation);

        await command.ExecuteNonQueryAsync();
    }

    public static async Task TransferInventoryQuantityAsync(string batchNumber, string partId, string operation,
        int transferQuantity, int originalQuantity, string newLocation, string user)
    {
        var connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand("inv_inventory_transfer_quantity", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@in_BatchNumber", batchNumber);
        command.Parameters.AddWithValue("@in_PartID", partId);
        command.Parameters.AddWithValue("@in_Operation", operation);
        command.Parameters.AddWithValue("@in_TransferQuantity", transferQuantity);
        command.Parameters.AddWithValue("@in_OriginalQuantity", originalQuantity);
        command.Parameters.AddWithValue("@in_NewLocation", newLocation);
        command.Parameters.AddWithValue("@in_User", user);
        await command.ExecuteNonQueryAsync();
        await FixBatchNumbersAsync();
    }

    public static async Task FixBatchNumbersAsync()
    {
        var connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand("inv_inventory_Fix_BatchNumbers", connection);
        command.CommandType = CommandType.StoredProcedure;
        await command.ExecuteNonQueryAsync();
    }

    #endregion
}

#endregion