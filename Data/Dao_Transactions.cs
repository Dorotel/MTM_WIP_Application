using System.Text;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using System.Data;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

/// <summary>
/// Data access object for transaction operations
/// </summary>
internal class Dao_Transactions
{
    #region Search Methods

    /// <summary>
    /// Asynchronously search for transactions based on criteria
    /// </summary>
    /// <param name="userName">User performing the search</param>
    /// <param name="isAdmin">Whether user has admin privileges</param>
    /// <param name="partID">Part ID filter</param>
    /// <param name="batchNumber">Batch number filter</param>
    /// <param name="fromLocation">From location filter</param>
    /// <param name="toLocation">To location filter</param>
    /// <param name="operation">Operation filter</param>
    /// <param name="transactionType">Transaction type filter</param>
    /// <param name="quantity">Quantity filter</param>
    /// <param name="notes">Notes filter</param>
    /// <param name="itemType">Item type filter</param>
    /// <param name="fromDate">From date filter</param>
    /// <param name="toDate">To date filter</param>
    /// <param name="sortColumn">Column to sort by</param>
    /// <param name="sortDescending">Sort direction</param>
    /// <param name="page">Page number for pagination</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>DaoResult containing list of transactions</returns>
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

    /// <summary>
    /// Synchronously search for transactions based on criteria (backward compatibility)
    /// </summary>
    /// <param name="userName">User performing the search</param>
    /// <param name="isAdmin">Whether user has admin privileges</param>
    /// <param name="partID">Part ID filter</param>
    /// <param name="batchNumber">Batch number filter</param>
    /// <param name="fromLocation">From location filter</param>
    /// <param name="toLocation">To location filter</param>
    /// <param name="operation">Operation filter</param>
    /// <param name="transactionType">Transaction type filter</param>
    /// <param name="quantity">Quantity filter</param>
    /// <param name="notes">Notes filter</param>
    /// <param name="itemType">Item type filter</param>
    /// <param name="fromDate">From date filter</param>
    /// <param name="toDate">To date filter</param>
    /// <param name="sortColumn">Column to sort by</param>
    /// <param name="sortDescending">Sort direction</param>
    /// <param name="page">Page number for pagination</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>DaoResult containing list of transactions</returns>
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

    #endregion

    #region Smart Search Methods

    /// <summary>
    /// Advanced smart search for transactions with intelligent parsing
    /// </summary>
    /// <param name="searchTerms">Parsed search terms from user input</param>
    /// <param name="transactionTypes">Selected transaction types filter</param>
    /// <param name="timeRange">Selected time range filter</param>
    /// <param name="locations">Selected locations filter</param>
    /// <param name="userName">Current user name</param>
    /// <param name="isAdmin">Whether user has admin privileges</param>
    /// <param name="page">Page number for pagination</param>
    /// <param name="pageSize">Items per page</param>
    /// <returns>DaoResult containing smart search results</returns>
    public async Task<DaoResult<List<Model_Transactions>>> SmartSearchAsync(
        Dictionary<string, string> searchTerms,
        List<TransactionType> transactionTypes,
        (DateTime? from, DateTime? to) timeRange,
        List<string> locations,
        string userName,
        bool isAdmin,
        int page = 1,
        int pageSize = 20
    )
    {
        try
        {
            // Build parameters from parsed search terms
            var parameters = new Dictionary<string, object>
            {
                ["UserName"] = userName ?? "",
                ["IsAdmin"] = isAdmin,
                ["Page"] = page,
                ["PageSize"] = pageSize
            };

            // Add search term parameters
            parameters["PartID"] = searchTerms.ContainsKey("partid") ? searchTerms["partid"] : "";
            parameters["BatchNumber"] = searchTerms.ContainsKey("batch") ? searchTerms["batch"] : "";
            parameters["Operation"] = searchTerms.ContainsKey("operation") ? searchTerms["operation"] : "";
            parameters["Notes"] = searchTerms.ContainsKey("notes") ? searchTerms["notes"] : "";
            parameters["User"] = searchTerms.ContainsKey("user") ? searchTerms["user"] : "";
            parameters["ItemType"] = searchTerms.ContainsKey("itemtype") ? searchTerms["itemtype"] : "";

            // Handle quantity search
            if (searchTerms.ContainsKey("quantity") && int.TryParse(searchTerms["quantity"], out int qty))
                parameters["Quantity"] = qty;
            else
                parameters["Quantity"] = null;

            // Handle transaction types
            parameters["TransactionTypes"] = transactionTypes.Count > 0 
                ? string.Join(",", transactionTypes.Select(t => t.ToString())) 
                : "";

            // Handle time range
            parameters["FromDate"] = timeRange.from;
            parameters["ToDate"] = timeRange.to;

            // Handle locations
            parameters["Locations"] = locations.Count > 0 
                ? string.Join(",", locations) 
                : "";

            // Handle general search term (searches across multiple fields)
            parameters["GeneralSearch"] = searchTerms.ContainsKey("general") ? searchTerms["general"] : "";

            // Execute smart search stored procedure
            using var reader = await Helper_Database_StoredProcedure.ExecuteReader(
                Model_AppVariables.ConnectionString,
                "inv_transactions_SmartSearch", 
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
                $"Smart search retrieved {transactions.Count} transactions matching criteria"
            );
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "SmartSearchAsync");
            return DaoResult<List<Model_Transactions>>.Failure(
                "Smart search failed", ex
            );
        }
    }

    /// <summary>
    /// Get transaction analytics for dashboard display
    /// </summary>
    /// <param name="userName">Current user name</param>
    /// <param name="isAdmin">Whether user has admin privileges</param>
    /// <param name="timeRange">Time range for analytics</param>
    /// <returns>DaoResult containing analytics data</returns>
    public async Task<DaoResult<Dictionary<string, object>>> GetTransactionAnalyticsAsync(
        string userName,
        bool isAdmin,
        (DateTime? from, DateTime? to) timeRange
    )
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["UserName"] = userName ?? "",
                ["IsAdmin"] = isAdmin,
                ["FromDate"] = timeRange.from,
                ["ToDate"] = timeRange.to
            };

            using var reader = await Helper_Database_StoredProcedure.ExecuteReader(
                Model_AppVariables.ConnectionString,
                "inv_transactions_GetAnalytics", 
                parameters, 
                true, 
                CommandType.StoredProcedure);

            var analytics = new Dictionary<string, object>();
            
            if (reader.Read())
            {
                analytics["TotalTransactions"] = reader.GetInt32("TotalTransactions");
                analytics["InTransactions"] = reader.GetInt32("InTransactions");
                analytics["OutTransactions"] = reader.GetInt32("OutTransactions");
                analytics["TransferTransactions"] = reader.GetInt32("TransferTransactions");
                analytics["TotalQuantity"] = reader.GetInt64("TotalQuantity");
                analytics["UniquePartIds"] = reader.GetInt32("UniquePartIds");
                analytics["ActiveUsers"] = reader.GetInt32("ActiveUsers");
                analytics["TopPartId"] = reader["TopPartId"]?.ToString() ?? "";
                analytics["TopUser"] = reader["TopUser"]?.ToString() ?? "";
            }

            return DaoResult<Dictionary<string, object>>.Success(
                analytics, 
                "Transaction analytics retrieved successfully"
            );
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "GetTransactionAnalyticsAsync");
            return DaoResult<Dictionary<string, object>>.Failure(
                "Failed to retrieve transaction analytics", ex
            );
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Maps MySqlDataReader row to Model_Transactions object
    /// </summary>
    /// <param name="reader">MySqlDataReader instance</param>
    /// <returns>Mapped Model_Transactions object</returns>
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

    #endregion
}
