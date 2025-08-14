using System.Text;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using System.Data;

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
                ["UserName"] = userName ?? "",
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

            // Use Helper_Database_StoredProcedure.ExecuteDataTableWithStatus for proper status handling
            var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "inv_transactions_Search",
                parameters,
                null, // No progress helper for this method
                true // Use async
            );

            if (!result.IsSuccess)
            {
                return DaoResult<List<Model_Transactions>>.Failure(
                    result.ErrorMessage ?? "Failed to search transactions"
                );
            }

            var transactions = new List<Model_Transactions>();
            if (result.Data != null)
            {
                foreach (DataRow row in result.Data.Rows)
                {
                    transactions.Add(MapTransactionFromDataRow(row));
                }
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
    /// Advanced smart search for transactions with intelligent parsing using string builder approach
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
            // DEBUG: Show what the DAO received
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] === DAO SmartSearchAsync Called ===");
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Received parameters:");
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] - userName: '{userName}'");
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] - isAdmin: {isAdmin}");
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] - searchTerms count: {searchTerms.Count}");
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] - page: {page}, pageSize: {pageSize}");

            // Build WHERE clause in application using StringBuilder for full control
            var whereBuilder = new StringBuilder("1=1");
            
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Starting WHERE clause build: '{whereBuilder}'");
            
            // User filtering logic - only apply if user is not admin AND no specific search terms
            bool hasSpecificSearchTerms = searchTerms.Count > 0;
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] hasSpecificSearchTerms: {hasSpecificSearchTerms}");
            
            if (!isAdmin && !hasSpecificSearchTerms && !string.IsNullOrWhiteSpace(userName))
            {
                whereBuilder.Append($" AND User = '{userName.Replace("'", "''")}'");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added user filter - WHERE clause now: '{whereBuilder}'");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] No user filter applied - isAdmin: {isAdmin}, hasSpecificSearchTerms: {hasSpecificSearchTerms}, userName: '{userName}'");
            }
            
            // Add specific search term filters
            if (searchTerms.ContainsKey("partid") && !string.IsNullOrWhiteSpace(searchTerms["partid"]))
            {
                whereBuilder.Append($" AND PartID = '{searchTerms["partid"].Replace("'", "''")}'");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added partid filter - WHERE clause now: '{whereBuilder}'");
            }
            
            if (searchTerms.ContainsKey("batch") && !string.IsNullOrWhiteSpace(searchTerms["batch"]))
            {
                whereBuilder.Append($" AND BatchNumber = '{searchTerms["batch"].Replace("'", "''")}'");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added batch filter - WHERE clause now: '{whereBuilder}'");
            }
            
            if (searchTerms.ContainsKey("operation") && !string.IsNullOrWhiteSpace(searchTerms["operation"]))
            {
                whereBuilder.Append($" AND Operation = '{searchTerms["operation"].Replace("'", "''")}'");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added operation filter - WHERE clause now: '{whereBuilder}'");
            }
            
            if (searchTerms.ContainsKey("notes") && !string.IsNullOrWhiteSpace(searchTerms["notes"]))
            {
                whereBuilder.Append($" AND Notes LIKE '%{searchTerms["notes"].Replace("'", "''")}%'");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added notes filter - WHERE clause now: '{whereBuilder}'");
            }
            
            if (searchTerms.ContainsKey("user") && !string.IsNullOrWhiteSpace(searchTerms["user"]))
            {
                whereBuilder.Append($" AND User = '{searchTerms["user"].Replace("'", "''")}'");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added specific user filter - WHERE clause now: '{whereBuilder}'");
            }
            
            if (searchTerms.ContainsKey("itemtype") && !string.IsNullOrWhiteSpace(searchTerms["itemtype"]))
            {
                whereBuilder.Append($" AND ItemType = '{searchTerms["itemtype"].Replace("'", "''")}'");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added itemtype filter - WHERE clause now: '{whereBuilder}'");
            }
            
            if (searchTerms.ContainsKey("quantity") && int.TryParse(searchTerms["quantity"], out int qty))
            {
                whereBuilder.Append($" AND Quantity = {qty}");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added quantity filter - WHERE clause now: '{whereBuilder}'");
            }
            
            // Handle transaction types filter
            if (transactionTypes.Count > 0 && transactionTypes.Count < 3) // Only filter if not all types selected
            {
                var typeList = string.Join(",", transactionTypes.Select(t => $"'{t}'"));
                whereBuilder.Append($" AND TransactionType IN ({typeList})");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added transaction type filter - WHERE clause now: '{whereBuilder}'");
            }
            
            // Handle time range filter
            if (timeRange.from.HasValue)
            {
                whereBuilder.Append($" AND ReceiveDate >= '{timeRange.from.Value:yyyy-MM-dd HH:mm:ss}'");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added from date filter - WHERE clause now: '{whereBuilder}'");
            }
            
            if (timeRange.to.HasValue)
            {
                whereBuilder.Append($" AND ReceiveDate <= '{timeRange.to.Value:yyyy-MM-dd HH:mm:ss}'");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added to date filter - WHERE clause now: '{whereBuilder}'");
            }
            
            // Handle locations filter
            if (locations.Count > 0)
            {
                var locationList = string.Join(",", locations.Select(l => $"'{l.Replace("'", "''")}'"));
                whereBuilder.Append($" AND FromLocation IN ({locationList})");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added location filter - WHERE clause now: '{whereBuilder}'");
            }
            
            // Handle general search term (searches across multiple fields)
            if (searchTerms.ContainsKey("general") && !string.IsNullOrWhiteSpace(searchTerms["general"]))
            {
                var generalTerm = searchTerms["general"].Replace("'", "''");
                whereBuilder.Append($" AND (PartID LIKE '%{generalTerm}%' OR BatchNumber LIKE '%{generalTerm}%' OR Operation LIKE '%{generalTerm}%' OR Notes LIKE '%{generalTerm}%' OR User LIKE '%{generalTerm}%' OR ItemType LIKE '%{generalTerm}%' OR FromLocation LIKE '%{generalTerm}%' OR ToLocation LIKE '%{generalTerm}%')");
                System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Added general search filter - WHERE clause now: '{whereBuilder}'");
            }
            
            // Build parameters for the simplified stored procedure
            var parameters = new Dictionary<string, object>
            {
                ["WhereClause"] = whereBuilder.ToString(),
                ["Page"] = page,
                ["PageSize"] = pageSize
            };

            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] === FINAL WHERE CLAUSE ===");
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] WhereClause parameter: '{whereBuilder}'");
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Full SQL will be: SELECT ... FROM inv_transaction WHERE {whereBuilder} ORDER BY ReceiveDate DESC LIMIT ...");
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] === CALLING STORED PROCEDURE ===");

            // Use Helper_Database_StoredProcedure.ExecuteDataTableWithStatus for proper status handling
            var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "inv_transactions_SmartSearch",
                parameters,
                null, // No progress helper for this method
                true // Use async
            );

            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Stored procedure result: IsSuccess={result.IsSuccess}, ErrorMessage='{result.ErrorMessage}'");

            if (!result.IsSuccess)
            {
                return DaoResult<List<Model_Transactions>>.Failure(
                    result.ErrorMessage ?? "Smart search failed"
                );
            }

            var transactions = new List<Model_Transactions>();
            if (result.Data != null)
            {
                foreach (DataRow row in result.Data.Rows)
                {
                    transactions.Add(MapTransactionFromDataRow(row));
                }
            }

            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Mapped {transactions.Count} transactions from result");
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] === DAO SmartSearchAsync Complete ===");

            return DaoResult<List<Model_Transactions>>.Success(
                transactions,
                $"Smart search retrieved {transactions.Count} transactions matching criteria"
            );
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Exception in SmartSearchAsync: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"[DAO DEBUG] Stack trace: {ex.StackTrace}");
            
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

            // Use Helper_Database_StoredProcedure.ExecuteDataTableWithStatus for proper status handling
            var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "inv_transactions_GetAnalytics",
                parameters,
                null, // No progress helper for this method
                true // Use async
            );

            if (!result.IsSuccess)
            {
                return DaoResult<Dictionary<string, object>>.Failure(
                    result.ErrorMessage ?? "Failed to retrieve transaction analytics"
                );
            }

            var analytics = new Dictionary<string, object>();

            if (result.Data != null && result.Data.Rows.Count > 0)
            {
                var row = result.Data.Rows[0];
                analytics["TotalTransactions"] = Convert.ToInt32(row["TotalTransactions"]);
                analytics["InTransactions"] = Convert.ToInt32(row["InTransactions"]);
                analytics["OutTransactions"] = Convert.ToInt32(row["OutTransactions"]);
                analytics["TransferTransactions"] = Convert.ToInt32(row["TransferTransactions"]);
                analytics["TotalQuantity"] = Convert.ToInt64(row["TotalQuantity"]);
                analytics["UniquePartIds"] = Convert.ToInt32(row["UniquePartIds"]);
                analytics["ActiveUsers"] = Convert.ToInt32(row["ActiveUsers"]);
                analytics["TopPartId"] = row["TopPartId"]?.ToString() ?? "";
                analytics["TopUser"] = row["TopUser"]?.ToString() ?? "";
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
    /// Maps DataRow to Model_Transactions object
    /// </summary>
    /// <param name="row">DataRow from stored procedure result</param>
    /// <returns>Mapped Model_Transactions object</returns>
    private Model_Transactions MapTransactionFromDataRow(DataRow row) =>
        new()
        {
            ID = Convert.ToInt32(row["ID"]),
            TransactionType =
                Enum.TryParse<TransactionType>(row["TransactionType"].ToString(), out TransactionType type)
                    ? type
                    : TransactionType.IN,
            BatchNumber = row["BatchNumber"] == DBNull.Value ? null : row["BatchNumber"].ToString(),
            PartID = row["PartID"] == DBNull.Value ? null : row["PartID"].ToString(),
            FromLocation = row["FromLocation"] == DBNull.Value ? null : row["FromLocation"].ToString(),
            ToLocation = row["ToLocation"] == DBNull.Value ? null : row["ToLocation"].ToString(),
            Operation = row["Operation"] == DBNull.Value ? null : row["Operation"].ToString(),
            Quantity = row["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(row["Quantity"]),
            Notes = row["Notes"] == DBNull.Value ? null : row["Notes"].ToString(),
            User = row["User"] == DBNull.Value ? null : row["User"].ToString(),
            ItemType = row["ItemType"] == DBNull.Value ? null : row["ItemType"].ToString(),
            DateTime = row["ReceiveDate"] == DBNull.Value
                ? DateTime.MinValue
                : Convert.ToDateTime(row["ReceiveDate"])
        };

    #endregion
}
