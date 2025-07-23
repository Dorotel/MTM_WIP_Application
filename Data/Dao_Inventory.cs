using System.Data;
using MTM_Inventory_Application.Helpers;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Data;

#region Dao_Inventory

public static class Dao_Inventory
{
    #region Fields

    private static readonly Helper_Database_Core HelperDatabaseCore =
        new(Helper_Database_Variables.GetConnectionString(null, null, null, null));

    #endregion

    #region Migration Methods

    public static async Task MigrateSavedLocations()
    {
        var connectionString = Helper_Database_Variables.GetConnectionString(null, "mtm database", null, null);
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand("migrate_saved_locations", connection);
        command.CommandType = CommandType.StoredProcedure;
        await command.ExecuteNonQueryAsync();
    }

    #endregion

    #region Batch Fix Methods

    /// <summary>
    /// Splits batch numbers in inv_transaction table where a batch has multiple IN and OUT transactions,
    /// grouping by ReceiveDate (date only), assigning new batch numbers, and updating the batch sequence.
    /// Reports progress if a progress reporter is provided. Processes up to 250 batches per run and repeats until all are fixed.
    /// Before starting, calculates how many batches will need to be fixed and throws an exception with the count and runs required.
    /// </summary>
    public static async Task SplitBatchNumbersByReceiveDateAsync(
        IProgress<(int percent, string status, int cycle, int totalCycles, int batchInCycle, int batchesInCycle, int totalFixed)>? progress = null,
        CancellationToken cancellationToken = default)
    {
        string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);

        // Calculate how many batches need to be fixed before starting
        int totalProblematicBatches = 0;
        await using (var connection = new MySqlConnection(connectionString))
        {
            await connection.OpenAsync();
            const string countBatchesSql = @"
                SELECT COUNT(*)
                FROM (
                    SELECT BatchNumber
                    FROM mtm_wip_application.inv_transaction
                    GROUP BY BatchNumber
                    HAVING SUM(CASE WHEN TransactionType = 'IN' THEN 1 ELSE 0 END) > 1
                       AND SUM(CASE WHEN TransactionType = 'OUT' THEN 1 ELSE 0 END) > 1
                ) t;";
            await using (var cmd = new MySqlCommand(countBatchesSql, connection))
            {
                object? result = await cmd.ExecuteScalarAsync();
                totalProblematicBatches = Convert.ToInt32(result);
            }
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
                // 1. Get all problematic BatchNumbers
                var batchNumbers = new List<string>();
                const string getBatchesSql = @"
                    SELECT BatchNumber
                    FROM (
                        SELECT BatchNumber,
                               SUM(CASE WHEN TransactionType = 'IN' THEN 1 ELSE 0 END) AS in_count,
                               SUM(CASE WHEN TransactionType = 'OUT' THEN 1 ELSE 0 END) AS out_count
                        FROM mtm_wip_application.inv_transaction
                        GROUP BY BatchNumber
                        HAVING in_count > 1 AND out_count > 1
                    ) t;";
                await using (var cmd = new MySqlCommand(getBatchesSql, connection, transaction))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
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

                // Only process up to 250 batches per run
                var toProcess = batchNumbers.Take(250).ToList();
                progress?.Report((5, $"Run {runCount}: Processing {toProcess.Count} of {batchNumbers.Count} problematic batch numbers...", runCount, totalCycles, 0, toProcess.Count, totalFixed));

                // 2. Get the last used batch number
                long lastBatchNumber;
                const string getLastBatchSql = "SELECT last_batch_number FROM mtm_wip_application.inv_inventory_batch_seq;";
                await using (var cmd = new MySqlCommand(getLastBatchSql, connection, transaction))
                {
                    object lastBatchObj = await cmd.ExecuteScalarAsync();
                    lastBatchNumber = Convert.ToInt64(lastBatchObj);
                }

                int total = toProcess.Count;
                int current = 0;

                // 3. For each problematic batch number...
                foreach (var batchNumber in toProcess)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    current++;
                    int percent = 5 + (int)(90.0 * current / total);
                    progress?.Report((percent, $"Run {runCount}: Processing batch {current} of {total} ({batchNumber})...", runCount, totalCycles, current, total, totalFixed));

                    // 3a. Get distinct ReceiveDate (date only) for this batch
                    var dateList = new List<DateTime>();
                    const string getDatesSql = @"
                        SELECT DISTINCT DATE(ReceiveDate)
                        FROM mtm_wip_application.inv_transaction
                        WHERE BatchNumber = @BatchNumber
                        ORDER BY DATE(ReceiveDate);";
                    await using (var cmd = new MySqlCommand(getDatesSql, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                        await using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dateList.Add(reader.GetDateTime(0).Date);
                            }
                        }
                    }

                    // 3b. For each date, update that group's BatchNumber to a new one
                    foreach (var date in dateList)
                    {
                        lastBatchNumber += 1;
                        string newBatchNumber = lastBatchNumber.ToString().PadLeft(10, '0');

                        const string updateBatchSql = @"
                            UPDATE mtm_wip_application.inv_transaction
                            SET BatchNumber = @NewBatchNumber
                            WHERE BatchNumber = @BatchNumber AND DATE(ReceiveDate) = @TxDate;";
                        await using (var cmd = new MySqlCommand(updateBatchSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@NewBatchNumber", newBatchNumber);
                            cmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                            cmd.Parameters.AddWithValue("@TxDate", date);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                // 4. Update the batch sequence table
                progress?.Report((98, $"Run {runCount}: Updating batch sequence table...", runCount, totalCycles, total, total, totalFixed));
                const string updateSeqSql = "UPDATE mtm_wip_application.inv_inventory_batch_seq SET last_batch_number = @LastBatchNumber;";
                await using (var cmd = new MySqlCommand(updateSeqSql, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@LastBatchNumber", lastBatchNumber);
                    await cmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
                totalFixed += toProcess.Count;
                progress?.Report((99, $"Run {runCount} complete. {toProcess.Count} batches fixed. Total fixed: {totalFixed}", runCount, totalCycles, total, total, totalFixed));
            }
            catch
            {
                await transaction.RollbackAsync();
                progress?.Report((0, $"Error occurred in run {runCount}. Rolled back changes.", runCount, totalCycles, 0, 0, totalFixed));
                throw;
            }
        }
    }

    #endregion

    #region Search Methods

    public static async Task<DataTable> GetInventoryByPartIdAsync(string partId, bool useAsync = false) =>
        await HelperDatabaseCore.ExecuteDataTable(
            "mtm_wip_application.inv_inventory_Get_ByPartID",
            new Dictionary<string, object> { { "p_PartID", partId } },
            useAsync, CommandType.StoredProcedure);

    public static async Task<DataTable> GetInventoryByPartIdAndOperationAsync(string partId, string operation,
        bool useAsync = false) =>
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
        if (string.IsNullOrWhiteSpace(itemType))
        {
            object? itemTypeObj = await HelperDatabaseCore.ExecuteScalar(
                "SELECT `ItemType` FROM `md_part_ids` WHERE `PartID` = @PartID",
                new Dictionary<string, object> { { "@PartID", partId } },
                useAsync, CommandType.Text);

            itemType = itemTypeObj?.ToString() ?? "None";
        }

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

        await FixBatchNumbersAsync();

        return result;
    }

    public static async Task<(int RemovedCount, List<string> ErrorMessages)> RemoveInventoryItemsFromDataGridViewAsync(DataGridView dgv, bool useAsync = false)
    {
        int removedCount = 0;
        List<string> errorMessages = new();

        if (dgv == null || dgv.SelectedRows.Count == 0)
        {
            return (0, errorMessages);
        }

        foreach (DataGridViewRow row in dgv.SelectedRows)
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

            var (status, errorMsg) = await RemoveInventoryItemAsync(
                partId,
                location,
                operation,
                quantity,
                itemType,
                user,
                batchNumber,
                notes,
                useAsync);

            if (status > 0)
            {
                removedCount += status;
            }
            else if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                errorMessages.Add($"PartID: {partId}, Location: {location}, Operation: {operation}, Error: {errorMsg}");
            }
        }

        return (removedCount, errorMessages);
    }

    public static async Task<(int Status, string ErrorMsg)> RemoveInventoryItemAsync(
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
        return (status, errorMsg);
    }

    public static async Task TransferPartSimpleAsync(string batchNumber, string partId, string operation, string quantity, string newLocation)
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

        await command.ExecuteNonQueryAsync();
        await FixBatchNumbersAsync();
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
        await FixBatchNumbersAsync();
    }

    public static async Task FixBatchNumbersAsync()
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

        // Optionally, read the output values
        int status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
        string errorMsg = errorMsgParam.Value?.ToString() ?? "";

        // Handle status/error as needed
    }

    #endregion
}

#endregion
