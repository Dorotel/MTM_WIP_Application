using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_History

internal class Dao_History
{
    #region Fields

    public static Helper_Database_Core HelperDatabaseCore =
        new(Helper_Database_Variables.GetConnectionString(
            Model_AppVariables.WipServerAddress,
            "mtm_wip_application",
            Model_AppVariables.User,
            Model_AppVariables.UserPin
        ));

    #endregion

    #region History Methods

    public static async Task AddTransactionHistoryAsync(Model_TransactionHistory history)
    {
        try
        {
            // FIXED: Use Helper_Database_Core instead of direct MySqlConnection and correct parameter naming
            Dictionary<string, object> parameters = new()
            {
                ["p_TransactionType"] = history.TransactionType,
                ["p_PartID"] = history.PartId,
                ["p_FromLocation"] = history.FromLocation ?? (object)DBNull.Value,
                ["p_ToLocation"] = history.ToLocation ?? (object)DBNull.Value,
                ["p_Operation"] = history.Operation ?? (object)DBNull.Value,
                ["p_Quantity"] = history.Quantity,
                ["p_Notes"] = history.Notes ?? (object)DBNull.Value,
                ["p_User"] = history.User,
                ["p_ItemType"] = history.ItemType ?? (object)DBNull.Value,
                ["p_BatchNumber"] = history.BatchNumber ?? (object)DBNull.Value,
                ["p_ReceiveDate"] = history.DateTime
            };

            await HelperDatabaseCore.ExecuteNonQuery(
                "inv_transaction_Add",
                parameters, 
                true, // Use async
                CommandType.StoredProcedure);
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
