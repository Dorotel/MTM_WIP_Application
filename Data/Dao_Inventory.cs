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
        string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);

        try
        {
            // Calculate how many batches need to be fixed before starting
            int totalProblematicBatches = 0;
            await using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await using var cmd = new MySqlCommand("inv_transaction_GetProblematicBatchCount", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                object? result = await cmd.ExecuteScalarAsync();
                totalProblematicBatches = Convert.ToInt32(result);
            }
            
            int totalCycles = (int)Math.Ceiling(totalProblematicBatches / 250.0);
            int totalFixed = 0;
            int runCount = 0;
            
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                runCount++;
                
                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                await using var transaction = await connection.BeginTransactionAsync();
                
                try
                {
                    progress?.Report((0, $"Finding problematic batch numbers (run {runCount})...", runCount, totalCycles, 0, 0, totalFixed));
                    
                    // Use stored procedure to get problematic batches
                    var batchNumbers = new List<string>();
                    await using (var cmd = new MySqlCommand("inv_transaction_GetProblematicBatches", connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_Limit", 250);
                        
                        await using var reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            batchNumbers.Add(reader.GetString(0));
                        }
                    }

                    if (batchNumbers.Count == 0)
                    {
                        progress?.Report((100, $"All problematic batches fixed. Total fixed: {totalFixed}", runCount, totalCycles, 0, 0, totalFixed));
                        await transaction.CommitAsync();
                        break;
                    }

                    // Process batches using stored procedure
                    await using (var cmd = new MySqlCommand("inv_transaction_SplitBatchNumbers", connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_BatchNumbers", string.Join(",", batchNumbers));
                        
                        var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                        var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                        var processedCountParam = new MySqlParameter("p_ProcessedCount", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                        
                        cmd.Parameters.Add(statusParam);
                        cmd.Parameters.Add(errorMsgParam);
                        cmd.Parameters.Add(processedCountParam);
                        
                        await cmd.ExecuteNonQueryAsync();
                        
                        int status = Convert.ToInt32(statusParam.Value ?? 0);
                        string errorMsg = errorMsgParam.Value?.ToString() ?? "";
                        int processedCount = Convert.ToInt32(processedCountParam.Value ?? 0);
                        
                        if (status != 0)
                        {
                            throw new Exception(errorMsg);
                        }
                        
                        totalFixed += processedCount;
                    }

                    await transaction.CommitAsync();
                    progress?.Report((99, $"Run {runCount} complete. {batchNumbers.Count} batches fixed. Total fixed: {totalFixed}", runCount, totalCycles, batchNumbers.Count, batchNumbers.Count, totalFixed));
                }
                catch
                {
                    await transaction.RollbackAsync();
                    progress?.Report((0, $"Error occurred in run {runCount}. Rolled back changes.", runCount, totalCycles, 0, 0, totalFixed));
                    throw;
                }
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
            DataTable result = await HelperDatabaseCore.ExecuteDataTable(
                "mtm_wip_application.inv_inventory_Get_ByPartID",
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
            DataTable result = await HelperDatabaseCore.ExecuteDataTable(
                "mtm_wip_application.inv_inventory_Get_ByPartIDAndOperation",
                new Dictionary<string, object> { { "p_PartID", partId }, { "o_Operation", operation } },
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
            string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand("mtm_wip_application.inv_inventory_Remove_Item_1_1", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("p_PartID", partId);
            command.Parameters.AddWithValue("p_Location", location);
            command.Parameters.AddWithValue("p_Operation", operation);
            command.Parameters.AddWithValue("p_Quantity", quantity);
            command.Parameters.AddWithValue("p_ItemType", itemType);
            command.Parameters.AddWithValue("p_User", user);
            command.Parameters.AddWithValue("p_BatchNumber", batchNumber);
            command.Parameters.AddWithValue("p_Notes", notes);

            var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(statusParam);
            command.Parameters.Add(errorMsgParam);

            await command.ExecuteNonQueryAsync();
            int status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
            string errorMsg = errorMsgParam.Value?.ToString() ?? string.Empty;
            
            if (status > 0)
            {
                return DaoResult<(int, string)>.Success((status, errorMsg), $"Successfully removed inventory item: {partId}", status);
            }
            else
            {
                return DaoResult<(int, string)>.Success((status, errorMsg), $"No inventory item removed: {errorMsg}");
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
            var connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand("inv_inventory_Transfer_Part", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@in_BatchNumber", batchNumber);
            command.Parameters.AddWithValue("@in_PartID", partId);
            command.Parameters.AddWithValue("@in_Operation", operation);
            command.Parameters.AddWithValue("@in_NewLocation", newLocation);

            int rowsAffected = await command.ExecuteNonQueryAsync();
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
            
            int rowsAffected = await command.ExecuteNonQueryAsync();
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
            string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand("inv_inventory_Fix_BatchNumbers", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add output parameters
            var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(statusParam);
            command.Parameters.Add(errorMsgParam);

            await command.ExecuteNonQueryAsync();

            int status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
            string errorMsg = errorMsgParam.Value?.ToString() ?? "";

            if (status == 0)
            {
                return DaoResult.Success("Batch numbers fixed successfully");
            }
            else
            {
                return DaoResult.Failure($"Failed to fix batch numbers: {errorMsg}");
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
