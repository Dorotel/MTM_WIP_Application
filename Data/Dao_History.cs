using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Data;

#region Dao_History

internal class Dao_History
{
    #region History Methods

    public static async Task AddTransactionHistoryAsync(Model_TransactionHistory history)
    {
        try
        {
            // MIGRATED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core
            Dictionary<string, object> parameters = new()
            {
                ["TransactionType"] = history.TransactionType,         // p_ prefix added automatically
                ["PartID"] = history.PartId,
                ["FromLocation"] = history.FromLocation ?? (object)DBNull.Value,
                ["ToLocation"] = history.ToLocation ?? (object)DBNull.Value,
                ["Operation"] = history.Operation ?? (object)DBNull.Value,
                ["Quantity"] = history.Quantity,
                ["Notes"] = history.Notes ?? (object)DBNull.Value,
                ["User"] = history.User,
                ["ItemType"] = history.ItemType ?? (object)DBNull.Value,
                ["BatchNumber"] = history.BatchNumber ?? (object)DBNull.Value,
                ["ReceiveDate"] = history.DateTime
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "inv_transaction_Add",
                parameters,
                null, // No progress helper for this method
                true  // Use async
            );

            if (!result.IsSuccess)
            {
                LoggingUtility.Log($"AddTransactionHistoryAsync failed: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "AddTransactionHistoryAsync");
        }
    }

    #endregion
}

#endregion
