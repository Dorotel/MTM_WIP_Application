using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_Inventory

public static class Dao_Inventory
{
    #region Fields

    private static readonly Helper_Database_Core HelperDatabaseCore =
        new(Helper_Database_Variables.GetConnectionString(null, null, null, null));

    #endregion

    #region Batch Fix Methods

    /// <summary>
    /// Splits batch numbers in inv_transaction table where a batch has multiple IN and OUT transactions,
    /// grouping by ReceiveDate (date only), assigning new batch numbers, and updating the batch sequence.
    /// Reports progress if a progress reporter is provided. Processes up to 250 batches per run and repeats until all are fixed.
    /// Before starting, calculates how many batches will need to be fixed and throws an exception with the count and runs required.
    /// </summary>
    public static async Task<DaoResult> SplitBatchNumbersByReceiveDateAsync(
        IProgress<(int percent, string status, int cycle, int totalCycles, int batchInCycle, int batchesInCycle, int totalFixed)>? progress = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // FIXED: Use Helper_Database_Core instead of direct MySqlConnection
            // Calculate how many batches need to be fixed before starting
            object? countResult = await HelperDatabaseCore.ExecuteScalar(
                "inv_transaction_GetProblematicBatchCount",
                null, true, CommandType.StoredProcedure);
            
            int totalProblematicBatches = Convert.ToInt32(countResult);
            int totalCycles = (int)Math.Ceiling(totalProblematicBatches / 250.0);
            int totalFixed = 0;
            int runCount = 0;
            
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                runCount++;
                
                progress?.Report((0, $"Finding problematic batch numbers (run {runCount})...", runCount, totalCycles, 0, 0, totalFixed));
                
                // FIXED: Use Helper_Database_Core to get problematic batches
                Dictionary<string, object> getParameters = new() { ["p_Limit"] = 250 };
                DataTable batchTable = await HelperDatabaseCore.ExecuteDataTable(
                    "inv_transaction_GetProblematicBatches", 
                    getParameters, true, CommandType.StoredProcedure);

                if (batchTable.Rows.Count == 0)
                {
                    progress?.Report((100, $"All problematic batches fixed. Total fixed: {totalFixed}", runCount, totalCycles, 0, 0, totalFixed));
                    break;
                }

                // Convert DataTable to batch numbers list
                var batchNumbers = new List<string>();
                foreach (DataRow row in batchTable.Rows)
                {
                    batchNumbers.Add(row[0].ToString() ?? "");
                }

                // FIXED: Use Helper_Database_StoredProcedure for proper status handling
                Dictionary<string, object> splitParameters = new() 
                { 
                    ["p_BatchNumbers"] = string.Join(",", batchNumbers) 
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Helper_Database_Variables.GetConnectionString(null, null, null, null),
                    "inv_transaction_SplitBatchNumbers",
                    splitParameters,
                    null, // No progress helper for this method
                    true  // Use async
                );

                if (!result.IsSuccess)
                {
                    progress?.Report((0, $"Error occurred in run {runCount}. {result.ErrorMessage}", runCount, totalCycles, 0, 0, totalFixed));
                    throw new Exception(result.ErrorMessage ?? "Failed to split batch numbers");
                }
                
                totalFixed += batchNumbers.Count; // Approximation, could get actual count from procedure
                progress?.Report((99, $"Run {runCount} complete. {batchNumbers.Count} batches fixed. Total fixed: {totalFixed}", runCount, totalCycles, batchNumbers.Count, batchNumbers.Count, totalFixed));
            }
            
            return DaoResult.Success($"Successfully split {totalFixed} problematic batch numbers", totalFixed);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            return DaoResult.Failure("Failed to split batch numbers by receive date", ex);
        }
    }

    #endregion

    #region Search Methods

    public static async Task<DaoResult<DataTable>> GetInventoryByPartIdAsync(string partId, bool useAsync = false)
    {
        try
        {
            // FIXED: Remove database prefix - stored procedure names should not have database prefix
            DataTable result = await HelperDatabaseCore.ExecuteDataTable(
                "inv_inventory_Get_ByPartID",
                new Dictionary<string, object> { { "p_PartID", partId } },
                useAsync, CommandType.StoredProcedure);
                
            return DaoResult<DataTable>.Success(result, $"Retrieved {result.Rows.Count} inventory items for part {partId}");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            return DaoResult<DataTable>.Failure($"Failed to retrieve inventory for part {partId}", ex);
        }
    }

    public static async Task<DaoResult<DataTable>> GetInventoryByPartIdAndOperationAsync(string partId, string operation,
        bool useAsync = false)
    {
        try
        {
            // FIXED: Correct parameter naming (p_Operation instead of o_Operation)
            DataTable result = await HelperDatabaseCore.ExecuteDataTable(
                "inv_inventory_Get_ByPartIDAndOperation",
                new Dictionary<string, object> { { "p_PartID", partId }, { "p_Operation", operation } },
                useAsync, CommandType.StoredProcedure);
                
            return DaoResult<DataTable>.Success(result, $"Retrieved {result.Rows.Count} inventory items for part {partId}, operation {operation}");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            return DaoResult<DataTable>.Failure($"Failed to retrieve inventory for part {partId}, operation {operation}", ex);
        }
    }

    #endregion

    #region Modification Methods

    public static async Task<DaoResult<int>> AddInventoryItemAsync(
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
        try
        {
            // Get item type if not provided
            if (string.IsNullOrWhiteSpace(itemType))
            {
                var itemTypeResult = await HelperDatabaseCore.ExecuteScalar(
                    "md_part_ids_GetItemType_ByPartID",
                    new Dictionary<string, object> { { "p_PartID", partId } },
                    useAsync, CommandType.StoredProcedure);

                itemType = itemTypeResult?.ToString() ?? "None";
            }

            // Generate batch number if not provided
            if (string.IsNullOrWhiteSpace(batchNumber))
            {
                var batchNumberResult = await HelperDatabaseCore.ExecuteScalar(
                    "inv_inventory_GetNextBatchNumber",
                    null, useAsync, CommandType.StoredProcedure);

                if (batchNumberResult != null && int.TryParse(batchNumberResult.ToString(), out int bn))
                {
                    batchNumber = bn.ToString("D10");
                }
                else
                {
                    batchNumber = "0000000001";
                }
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

            await FixBatchNumbersAsync();

            return DaoResult<int>.Success(result, $"Added inventory item: {partId} at {location}, quantity {quantity}", result);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            return DaoResult<int>.Failure($"Failed to add inventory item for part {partId}", ex);
        }
    }

    public static async Task<DaoResult<(int RemovedCount, List<string> ErrorMessages)>> RemoveInventoryItemsFromDataGridViewAsync(System.Windows.Forms.DataGridView dgv, bool useAsync = false)
    {
        int removedCount = 0;
        List<string> errorMessages = new();

        if (dgv == null || dgv.SelectedRows.Count == 0)
        {
            return DaoResult<(int, List<string>)>.Success((0, errorMessages), "No rows selected for removal");
        }

        try
        {
            foreach (System.Windows.Forms.DataGridViewRow row in dgv.SelectedRows)
            {
                string partId = row.Cells["PartID"].Value?.ToString() ?? "";
                string location = row.Cells["Location"].Value?.ToString() ?? "";
                string operation = row.Cells["Operation"].Value?.ToString() ?? "";
                int quantity = int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int qty) ? qty : 0;
                string batchNumber = row.Cells["BatchNumber"].Value?.ToString() ?? "";
                string itemType = row.Cells["ItemType"].Value?.ToString() ?? "";
                string user = row.Cells["User"].Value?.ToString() ?? "";
                string notes = row.Cells["Notes"].Value?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(partId) || string.IsNullOrWhiteSpace(location) ||
                    string.IsNullOrWhiteSpace(operation))
                {
                    continue;
                }

                var removeResult = await RemoveInventoryItemAsync(
                    partId, location, operation, quantity, itemType, user, batchNumber, notes, useAsync);

                if (removeResult.IsSuccess && removeResult.Data.Status > 0)
                {
                    removedCount += removeResult.Data.Status;
                }
                else if (!string.IsNullOrWhiteSpace(removeResult.Data.ErrorMsg))
                {
                    errorMessages.Add($"PartID: {partId}, Location: {location}, Operation: {operation}, Error: {removeResult.Data.ErrorMsg}");
                }
            }

            return DaoResult<(int, List<string>)>.Success((removedCount, errorMessages), $"Processed {dgv.SelectedRows.Count} items, removed {removedCount}");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            return DaoResult<(int, List<string>)>.Failure("Failed to remove inventory items from DataGridView", ex);
        }
    }

    public static async Task<DaoResult<(int Status, string ErrorMsg)>> RemoveInventoryItemAsync(
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
        try
        {
            // FIXED: Use Helper_Database_StoredProcedure for proper status handling
            Dictionary<string, object> parameters = new()
            {
                ["p_PartID"] = partId,
                ["p_Location"] = location,
                ["p_Operation"] = operation,
                ["p_Quantity"] = quantity,
                ["p_ItemType"] = itemType,
                ["p_User"] = user,
                ["p_BatchNumber"] = batchNumber,
                ["p_Notes"] = notes
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Helper_Database_Variables.GetConnectionString(null, null, null, null),
                "inv_inventory_Remove_Item_1_1",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess)
            {
                // FIXED: Use Status instead of RowsAffected and return meaningful counts
                return DaoResult<(int, string)>.Success((result.Status, result.ErrorMessage ?? ""), 
                    $"Successfully removed inventory item: {partId}", 1); // Return 1 for successful operations
            }
            else
            {
                return DaoResult<(int, string)>.Success((result.Status, result.ErrorMessage ?? ""), 
                    $"No inventory item removed: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            return DaoResult<(int, string)>.Failure($"Failed to remove inventory item for part {partId}", ex);
        }
    }

    public static async Task<DaoResult> TransferPartSimpleAsync(string batchNumber, string partId, string operation, string quantity, string newLocation)
    {
        try
        {
            // FIXED: Use Helper_Database_Core and correct parameter names (p_ instead of @)
            Dictionary<string, object> parameters = new()
            {
                ["p_BatchNumber"] = batchNumber,
                ["p_PartID"] = partId,
                ["p_Operation"] = operation,
                ["p_NewLocation"] = newLocation
            };

            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery(
                "inv_inventory_Transfer_Part",
                parameters, true, CommandType.StoredProcedure);

            await FixBatchNumbersAsync();
            
            return DaoResult.Success($"Transferred part {partId} from {operation} to {newLocation}", rowsAffected);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            return DaoResult.Failure($"Failed to transfer part {partId}", ex);
        }
    }

    public static async Task<DaoResult> TransferInventoryQuantityAsync(string batchNumber, string partId, string operation,
        int transferQuantity, int originalQuantity, string newLocation, string user)
    {
        try
        {
            // FIXED: Use Helper_Database_Core and correct parameter names (p_ instead of @)
            Dictionary<string, object> parameters = new()
            {
                ["p_BatchNumber"] = batchNumber,
                ["p_PartID"] = partId,
                ["p_Operation"] = operation,
                ["p_TransferQuantity"] = transferQuantity,
                ["p_OriginalQuantity"] = originalQuantity,
                ["p_NewLocation"] = newLocation,
                ["p_User"] = user
            };

            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery(
                "inv_inventory_transfer_quantity",
                parameters, true, CommandType.StoredProcedure);

            await FixBatchNumbersAsync();
            
            return DaoResult.Success($"Transferred {transferQuantity} of part {partId} to {newLocation}", rowsAffected);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            return DaoResult.Failure($"Failed to transfer quantity for part {partId}", ex);
        }
    }

    public static async Task<DaoResult> FixBatchNumbersAsync()
    {
        try
        {
            // FIXED: Use Helper_Database_StoredProcedure for proper status handling
            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Helper_Database_Variables.GetConnectionString(null, null, null, null),
                "inv_inventory_Fix_BatchNumbers",
                new Dictionary<string, object>(), // No parameters needed
                null, // No progress helper for this method
                true  // Use async
            );

            if (result.IsSuccess)
            {
                return DaoResult.Success("Batch numbers fixed successfully");
            }
            else
            {
                return DaoResult.Failure($"Failed to fix batch numbers: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            return DaoResult.Failure("Failed to fix batch numbers", ex);
        }
    }

    #endregion

    // Add a public static property to expose HelperDatabaseCore for use in other classes
    public static Helper_Database_Core PublicHelperDatabaseCore => HelperDatabaseCore;
}

#endregion
