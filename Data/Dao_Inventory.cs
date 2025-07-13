// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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

    public static async Task<DataTable> GetInventoryByPartIdAsync(string partId, bool useAsync = false) =>
        // Ensure the stored procedure returns BatchNumber AS 'Batch Number'
        await HelperDatabaseCore.ExecuteDataTable(
            "mtm_wip_application.inv_inventory_Get_ByPartID",
            new Dictionary<string, object> { { "p_PartID", partId } },
            useAsync, CommandType.StoredProcedure);

    public static async Task<DataTable> GetInventoryByPartIdAndOperationAsync(string partId, string operation,
        bool useAsync = false) =>
        // Ensure the stored procedure returns BatchNumber AS 'Batch Number'
        await HelperDatabaseCore.ExecuteDataTable(
            "mtm_wip_application.inv_inventory_Get_ByPartIDAndOperation",
            new Dictionary<string, object> { { "p_PartID", partId }, { "o_Operation", operation } },
            useAsync, CommandType.StoredProcedure);

    #endregion

    #region Modification Methods

    public static async Task<int> AddInventoryItemAsync(
        string partId,
        string location,
        string operation,
        int quantity,
        string? itemType,
        string user,
        string? batchNumber,
        string notes,
        bool useAsync = false)
    {
        // If itemType is null or empty, retrieve it from md_part_ids
        if (string.IsNullOrWhiteSpace(itemType))
        {
            object? itemTypeObj = await HelperDatabaseCore.ExecuteScalar(
                "SELECT `ItemType` FROM `md_part_ids` WHERE `PartID` = @PartID",
                new Dictionary<string, object> { { "@PartID", partId } },
                useAsync, CommandType.Text);

            itemType = itemTypeObj?.ToString() ?? "None";
        }

        // If batchNumber is null or empty, get the next sequential batch number (max 10 digits, no gaps)
        if (string.IsNullOrWhiteSpace(batchNumber))
        {
            object? batchNumberObj = await HelperDatabaseCore.ExecuteScalar(
                "SELECT IFNULL(MAX(CAST(`BatchNumber` AS UNSIGNED)), 0) + 1 FROM `inv_inventory` WHERE LENGTH(`BatchNumber`) <= 10",
                null, useAsync, CommandType.Text);

            int batchNumInt = 1;
            if (batchNumberObj != null && int.TryParse(batchNumberObj.ToString(), out int bn))
            {
                batchNumInt = bn;
            }

            // Pad only if the batch number is 10 digits or fewer
            batchNumber = batchNumInt.ToString().Length > 10
                ? batchNumInt.ToString()
                : batchNumInt.ToString("D10");
        }

        int result = await HelperDatabaseCore.ExecuteNonQuery(
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

        await FixBatchNumbersAsync(); // <-- Added

        return result;
    }

    public static async Task<int> RemoveInventoryItemsFromDataGridViewAsync(DataGridView dgv, bool useAsync = false)
    {
        int removedCount = 0;

        if (dgv == null || dgv.SelectedRows.Count == 0)
        {
            return 0;
        }

        foreach (DataGridViewRow row in dgv.SelectedRows)
        {
            // Use standard column names, or map as needed
            string partId = row.Cells["PartID"].Value?.ToString() ?? "";
            string location = row.Cells["Location"].Value?.ToString() ?? "";
            string operation = row.Cells["Operation"].Value?.ToString() ?? "";
            int quantity = int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int qty) ? qty : 0;
            string batchNumber =
                row.Cells["BatchNumber"].Value?.ToString() ?? ""; // if your column is named "BatchNumber"
            string itemType = row.Cells["ItemType"].Value?.ToString() ?? "";
            string user = row.Cells["User"].Value?.ToString() ?? "";
            string notes = row.Cells["Notes"].Value?.ToString() ?? "";

            // Optionally skip rows with missing required fields
            if (string.IsNullOrWhiteSpace(partId) || string.IsNullOrWhiteSpace(location) ||
                string.IsNullOrWhiteSpace(operation))
            {
                continue;
            }

            int result = await RemoveInventoryItemAsync(
                partId,
                location,
                operation,
                quantity,
                itemType,
                user,
                batchNumber,
                notes,
                useAsync);

            if (result > 0)
            {
                removedCount += result;
            }
        }

        return removedCount;
    }

    // Must match your stored procedure signature
    public static async Task<int> RemoveInventoryItemAsync(
        string partId,
        string location,
        string operation,
        int quantity,
        string itemType,
        string user,
        string batchNumber,
        string notes,
        bool useAsync = false)
    {
        int result = await HelperDatabaseCore.ExecuteNonQuery(
            "mtm_wip_application.inv_inventory_Remove_Item",
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

        return result;
    }

    public static async Task TransferPartSimpleAsync(string batchNumber, string partId, string operation,
        string quantity, string newLocation)
    {
        string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);

        await using MySqlConnection connection = new(connectionString);
        await connection.OpenAsync();

        await using MySqlCommand command = new("inv_inventory_Transfer_Part", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@in_BatchNumber", batchNumber);
        command.Parameters.AddWithValue("@in_PartID", partId);
        command.Parameters.AddWithValue("@in_Operation", operation);
        command.Parameters.AddWithValue("@in_Quantity", quantity);
        command.Parameters.AddWithValue("@in_NewLocation", newLocation);

        await command.ExecuteNonQueryAsync();

        await FixBatchNumbersAsync(); // <-- Added
    }

    public static async Task TransferInventoryQuantityAsync(string batchNumber, string partId, string operation,
        int transferQuantity, int originalQuantity, string newLocation, string user)
    {
        string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
        await using MySqlConnection connection = new(connectionString);
        await connection.OpenAsync();
        await using MySqlCommand command = new("inv_inventory_transfer_quantity", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@in_BatchNumber", batchNumber);
        command.Parameters.AddWithValue("@in_PartID", partId);
        command.Parameters.AddWithValue("@in_Operation", operation);
        command.Parameters.AddWithValue("@in_TransferQuantity", transferQuantity);
        command.Parameters.AddWithValue("@in_OriginalQuantity", originalQuantity);
        command.Parameters.AddWithValue("@in_NewLocation", newLocation);
        command.Parameters.AddWithValue("@in_User", user);
        await command.ExecuteNonQueryAsync();
        await FixBatchNumbersAsync(); // <-- Already present
    }

    public static async Task FixBatchNumbersAsync()
    {
        string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
        await using MySqlConnection connection = new(connectionString);
        await connection.OpenAsync();
        await using MySqlCommand command = new("inv_inventory_Fix_BatchNumbers", connection);
        command.CommandType = CommandType.StoredProcedure;
        await command.ExecuteNonQueryAsync();
    }

    #endregion
}

#endregion
