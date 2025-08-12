using System.Text;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using System.Data;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

internal class Dao_Transactions
{
    public async Task<DaoResult<List<Model_Transactions>>> SearchTransactionsAsync(
        string userName,
        bool isAdmin,
        string partID = "",
        string batchNumber = "",
        string fromLocation = "",
        string toLocation = "",
        string operation = "",
        TransactionType? transactionType = null,
        int? quantity = null,
        string notes = "",
        string itemType = "",
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string sortColumn = "ReceiveDate",
        bool sortDescending = true,
        int page = 1,
        int pageSize = 20
    )
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["UserName"] = userName ?? "",                           // p_ prefix added automatically
                ["IsAdmin"] = isAdmin,
                ["PartID"] = partID ?? "",
                ["BatchNumber"] = batchNumber ?? "",
                ["FromLocation"] = fromLocation ?? "",
                ["ToLocation"] = toLocation ?? "",
                ["Operation"] = operation ?? "",
                ["TransactionType"] = transactionType?.ToString() ?? "",
                ["Quantity"] = quantity,
                ["Notes"] = notes ?? "",
                ["ItemType"] = itemType ?? "",
                ["FromDate"] = fromDate,
                ["ToDate"] = toDate,
                ["SortColumn"] = sortColumn,
                ["SortDescending"] = sortDescending,
                ["Page"] = page,
                ["PageSize"] = pageSize
            };

            // MIGRATED: Use Helper_Database_StoredProcedure.ExecuteReader for better integration
            using var reader = await Helper_Database_StoredProcedure.ExecuteReader(
                Model_AppVariables.ConnectionString,
                "inv_transactions_Search", 
                parameters, 
                true, 
                CommandType.StoredProcedure);

            var transactions = new List<Model_Transactions>();
            while (reader.Read())
            {
                transactions.Add(MapTransaction(reader));
            }

            return DaoResult<List<Model_Transactions>>.Success(
                transactions, 
                $"Retrieved {transactions.Count} transactions for search criteria"
            );
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "SearchTransactionsAsync");
            return DaoResult<List<Model_Transactions>>.Failure(
                "Failed to search transactions", ex
            );
        }
    }

    public DaoResult<List<Model_Transactions>> SearchTransactions(
        string userName,
        bool isAdmin,
        string partID = "",
        string batchNumber = "",
        string fromLocation = "",
        string toLocation = "",
        string operation = "",
        TransactionType? transactionType = null,
        int? quantity = null,
        string notes = "",
        string itemType = "",
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string sortColumn = "ReceiveDate",
        bool sortDescending = true,
        int page = 1,
        int pageSize = 20
    )
    {
        // Synchronous wrapper for backward compatibility
        try
        {
            var result = SearchTransactionsAsync(userName, isAdmin, partID, batchNumber, fromLocation, 
                toLocation, operation, transactionType, quantity, notes, itemType, fromDate, toDate, 
                sortColumn, sortDescending, page, pageSize).GetAwaiter().GetResult();
            
            return result;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "SearchTransactions");
            return DaoResult<List<Model_Transactions>>.Failure(
                "Failed to search transactions (sync)", ex
            );
        }
    }

    private Model_Transactions MapTransaction(MySqlDataReader reader) =>
        new()
        {
            ID = reader.GetInt32("ID"),
            TransactionType =
                Enum.TryParse<TransactionType>(reader["TransactionType"].ToString(), out TransactionType type)
                    ? type
                    : TransactionType.IN,
            BatchNumber = reader["BatchNumber"] == DBNull.Value ? null : reader["BatchNumber"].ToString(),
            PartID = reader["PartID"] == DBNull.Value ? null : reader["PartID"].ToString(),
            FromLocation = reader["FromLocation"] == DBNull.Value ? null : reader["FromLocation"].ToString(),
            ToLocation = reader["ToLocation"] == DBNull.Value ? null : reader["ToLocation"].ToString(),
            Operation = reader["Operation"] == DBNull.Value ? null : reader["Operation"].ToString(),
            Quantity = reader["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Quantity"]),
            Notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString(),
            User = reader["User"] == DBNull.Value ? null : reader["User"].ToString(),
            ItemType = reader["ItemType"] == DBNull.Value ? null : reader["ItemType"].ToString(),
            DateTime = reader["ReceiveDate"] == DBNull.Value
                ? DateTime.MinValue
                : Convert.ToDateTime(reader["ReceiveDate"])
        };
}
