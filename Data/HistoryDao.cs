using MTM_WIP_Application.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MTM_WIP_Application.Data;

internal class HistoryDao
{
    public static async Task AddTransactionHistoryAsync(TransactionHistory history)
    {
        var connectionString = SqlVariables.GetConnectionString(null, null, null, null);
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        await using var command = new MySqlCommand("inv_transaction_Add", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@in_TransactionType", history.TransactionType);
        command.Parameters.AddWithValue("@in_PartID", history.PartId);
        command.Parameters.AddWithValue("@in_FromLocation", (object?)history.FromLocation ?? DBNull.Value);
        command.Parameters.AddWithValue("@in_ToLocation", (object?)history.ToLocation ?? DBNull.Value);
        command.Parameters.AddWithValue("@in_Operation", (object?)history.Operation ?? DBNull.Value);
        command.Parameters.AddWithValue("@in_Quantity", history.Quantity);
        command.Parameters.AddWithValue("@in_Notes", (object?)history.Notes ?? DBNull.Value);
        command.Parameters.AddWithValue("@in_User", history.User);
        command.Parameters.AddWithValue("@in_ItemType", (object?)history.ItemType ?? DBNull.Value);
        command.Parameters.AddWithValue("@in_BatchNumber", (object?)history.BatchNumber ?? DBNull.Value);
        command.Parameters.AddWithValue("@in_DateTime", history.DateTime);

        await command.ExecuteNonQueryAsync();
    }
}