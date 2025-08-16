using MTM_MAUI_Application.Models;
using System.Data;

namespace MTM_MAUI_Application.Services;

/// <summary>
/// Transaction service implementation
/// </summary>
public class TransactionService : ITransactionService
{
    private readonly IDatabaseService _database;
    private readonly ILoggingService _logging;

    public TransactionService(IDatabaseService database, ILoggingService logging)
    {
        _database = database;
        _logging = logging;
    }

    public async Task<List<Transaction>> GetTransactionsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var parameters = new Dictionary<string, object>();
            
            if (startDate.HasValue)
                parameters.Add("StartDate", startDate.Value);
            if (endDate.HasValue)
                parameters.Add("EndDate", endDate.Value);

            var dataTable = await _database.ExecuteStoredProcedureAsync("inv_transactions_Get_ByDateRange", parameters);
            return ConvertToTransactions(dataTable);
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync("Error getting transactions", ex);
            return new List<Transaction>();
        }
    }

    public async Task<List<Transaction>> SearchTransactionsAsync(string searchTerm)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "SearchTerm", searchTerm }
            };

            var dataTable = await _database.ExecuteStoredProcedureAsync("inv_transactions_Search", parameters);
            return ConvertToTransactions(dataTable);
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error searching transactions with term {searchTerm}", ex);
            return new List<Transaction>();
        }
    }

    public async Task<bool> AddTransactionAsync(Transaction transaction)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "TransactionType", (int)transaction.TransactionType },
                { "BatchNumber", transaction.BatchNumber ?? string.Empty },
                { "PartId", transaction.PartId },
                { "FromLocation", transaction.FromLocation ?? string.Empty },
                { "ToLocation", transaction.ToLocation ?? string.Empty },
                { "Operation", transaction.Operation },
                { "Quantity", transaction.Quantity },
                { "Notes", transaction.Notes ?? string.Empty },
                { "User", transaction.User },
                { "ItemType", transaction.ItemType }
            };

            await _database.ExecuteNonQueryAsync("inv_transactions_Add_Transaction", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error adding transaction for part {transaction.PartId}", ex);
            return false;
        }
    }

    public async Task<List<Transaction>> GetTransactionsByUserAsync(string username)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "User", username }
            };

            var dataTable = await _database.ExecuteStoredProcedureAsync("inv_transactions_Get_ByUser", parameters);
            return ConvertToTransactions(dataTable);
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error getting transactions for user {username}", ex);
            return new List<Transaction>();
        }
    }

    public async Task<List<Transaction>> GetTransactionsByPartAsync(string partId)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "PartId", partId }
            };

            var dataTable = await _database.ExecuteStoredProcedureAsync("inv_transactions_Get_ByPartId", parameters);
            return ConvertToTransactions(dataTable);
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error getting transactions for part {partId}", ex);
            return new List<Transaction>();
        }
    }

    private List<Transaction> ConvertToTransactions(DataTable dataTable)
    {
        var transactions = new List<Transaction>();

        foreach (DataRow row in dataTable.Rows)
        {
            transactions.Add(new Transaction
            {
                Id = row.Field<int>("ID") ?? 0,
                TransactionType = (TransactionType)(row.Field<int>("TransactionType") ?? 0),
                BatchNumber = row.Field<string>("BatchNumber"),
                PartId = row.Field<string>("PartID") ?? string.Empty,
                FromLocation = row.Field<string>("FromLocation"),
                ToLocation = row.Field<string>("ToLocation"),
                Operation = row.Field<string>("Operation") ?? string.Empty,
                Quantity = row.Field<int>("Quantity") ?? 0,
                Notes = row.Field<string>("Notes"),
                User = row.Field<string>("User") ?? string.Empty,
                ItemType = row.Field<string>("ItemType") ?? string.Empty,
                DateTime = row.Field<DateTime>("DateTime") ?? DateTime.Now
            });
        }

        return transactions;
    }
}