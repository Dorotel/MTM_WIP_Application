using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Data;

#region Dao_Inventory

public static class Dao_Inventory
{

    #region Search Methods

    public static async Task<DaoResult<DataTable>> GetInventoryByPartIdAsync(string partId, bool useAsync = false)
    {
        try
        {
            // MIGRATED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core for procedures with output parameters
            var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "inv_inventory_Get_ByPartID",
                new Dictionary<string, object> { ["PartID"] = partId }, // p_ prefix added automatically
                null, // No progress helper for this method
                useAsync
            );
                
            if (result.IsSuccess && result.Data != null)
            {
                return DaoResult<DataTable>.Success(result.Data, $"Retrieved {result.Data.Rows.Count} inventory items for part {partId}");
            }
            else
            {
                return DaoResult<DataTable>.Failure($"Failed to retrieve inventory for part {partId}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "GetInventoryByPartIdAsync");
            return DaoResult<DataTable>.Failure($"Failed to retrieve inventory for part {partId}", ex);
        }
    }

    public static async Task<DaoResult<DataTable>> GetInventoryByPartIdAndOperationAsync(string partId, string operation,
        bool useAsync = false)
    {
        try
        {
            // MIGRATED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core for procedures with output parameters
            var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "inv_inventory_Get_ByPartIDAndOperation",
                new Dictionary<string, object> { ["PartID"] = partId, ["Operation"] = operation }, // p_ prefix added automatically
                null, // No progress helper for this method
                useAsync
            );
                
            if (result.IsSuccess && result.Data != null)
            {
                return DaoResult<DataTable>.Success(result.Data, $"Retrieved {result.Data.Rows.Count} inventory items for part {partId}, operation {operation}");
            }
            else
            {
                return DaoResult<DataTable>.Failure($"Failed to retrieve inventory for part {partId}, operation {operation}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "GetInventoryByPartIdAndOperationAsync");
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
                var itemTypeResult = await Helper_Database_StoredProcedure.ExecuteScalarWithStatus(
                    Model_AppVariables.ConnectionString,
                    "md_part_ids_GetItemType_ByPartID",
                    new Dictionary<string, object> { ["PartID"] = partId }, // p_ prefix added automatically
                    null, // No progress helper for this method
                    useAsync
                );

                itemType = itemTypeResult.IsSuccess && itemTypeResult.Data != null 
                    ? itemTypeResult.Data.ToString() 
                    : "None";
            }

            // Generate batch number if not provided
            if (string.IsNullOrWhiteSpace(batchNumber))
            {
                var batchNumberResult = await Helper_Database_StoredProcedure.ExecuteScalarWithStatus(
                    Model_AppVariables.ConnectionString,
                    "inv_inventory_GetNextBatchNumber",
                    null, // No parameters needed
                    null, // No progress helper for this method
                    useAsync
                );

                if (batchNumberResult.IsSuccess && batchNumberResult.Data != null && 
                    int.TryParse(batchNumberResult.Data.ToString(), out int bn))
                {
                    batchNumber = bn.ToString("D10");
                }
                else
                {
                    batchNumber = "0000000001";
                }
            }

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "inv_inventory_Add_Item",
                new Dictionary<string, object>
                {
                    ["PartID"] = partId,         // p_ prefix added automatically
                    ["Location"] = location,
                    ["Operation"] = operation,
                    ["Quantity"] = quantity,
                    ["ItemType"] = itemType,
                    ["User"] = user,
                    ["BatchNumber"] = batchNumber,
                    ["Notes"] = notes
                },
                null, // No progress helper for this method
                useAsync
            );

            await FixBatchNumbersAsync();

            if (result.IsSuccess)
            {
                return DaoResult<int>.Success(1, $"Added inventory item: {partId} at {location}, quantity {quantity}", 1);
            }
            else
            {
                return DaoResult<int>.Failure($"Failed to add inventory item for part {partId}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "AddInventoryItemAsync");
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
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "RemoveInventoryItemsFromDataGridViewAsync");
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
            // MIGRATED: Use Helper_Database_StoredProcedure for proper status handling
            Dictionary<string, object> parameters = new()
            {
                ["PartID"] = partId,             // p_ prefix added automatically
                ["Location"] = location,
                ["Operation"] = operation,
                ["Quantity"] = quantity,
                ["ItemType"] = itemType,
                ["User"] = user,
                ["BatchNumber"] = batchNumber,
                ["Notes"] = notes
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "inv_inventory_Remove_Item_1_1",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess)
            {
                // MIGRATED: Use Status instead of RowsAffected and return meaningful counts
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
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "RemoveInventoryItemAsync");
            return DaoResult<(int, string)>.Failure($"Failed to remove inventory item for part {partId}", ex);
        }
    }

    public static async Task<DaoResult> TransferPartSimpleAsync(string batchNumber, string partId, string operation, string quantity, string newLocation)
    {
        try
        {
            // MIGRATED: Use Helper_Database_StoredProcedure and correct parameter names (p_ prefix added automatically)
            Dictionary<string, object> parameters = new()
            {
                ["BatchNumber"] = batchNumber,   // p_ prefix added automatically
                ["PartID"] = partId,
                ["Operation"] = operation,
                ["NewLocation"] = newLocation
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "inv_inventory_Transfer_Part",
                parameters,
                null, // No progress helper for this method
                true
            );

            await FixBatchNumbersAsync();
            
            if (result.IsSuccess)
            {
                return DaoResult.Success($"Transferred part {partId} from {operation} to {newLocation}");
            }
            else
            {
                return DaoResult.Failure($"Failed to transfer part {partId}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "TransferPartSimpleAsync");
            return DaoResult.Failure($"Failed to transfer part {partId}", ex);
        }
    }

    public static async Task<DaoResult> TransferInventoryQuantityAsync(string batchNumber, string partId, string operation,
        int transferQuantity, int originalQuantity, string newLocation, string user)
    {
        try
        {
            // MIGRATED: Use Helper_Database_StoredProcedure and correct parameter names (p_ prefix added automatically)
            Dictionary<string, object> parameters = new()
            {
                ["BatchNumber"] = batchNumber,       // p_ prefix added automatically
                ["PartID"] = partId,
                ["Operation"] = operation,
                ["TransferQuantity"] = transferQuantity,
                ["OriginalQuantity"] = originalQuantity,
                ["NewLocation"] = newLocation,
                ["User"] = user
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "inv_inventory_transfer_quantity",
                parameters,
                null, // No progress helper for this method
                true
            );

            await FixBatchNumbersAsync();
            
            if (result.IsSuccess)
            {
                return DaoResult.Success($"Transferred {transferQuantity} of part {partId} to {newLocation}");
            }
            else
            {
                return DaoResult.Failure($"Failed to transfer quantity for part {partId}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "TransferInventoryQuantityAsync");
            return DaoResult.Failure($"Failed to transfer quantity for part {partId}", ex);
        }
    }

    public static async Task<DaoResult> FixBatchNumbersAsync()
    {
        try
        {
            // MIGRATED: Use Helper_Database_StoredProcedure for proper status handling
            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
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
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "FixBatchNumbersAsync");
            return DaoResult.Failure("Failed to fix batch numbers", ex);
        }
    }

    #endregion

}

#endregion
