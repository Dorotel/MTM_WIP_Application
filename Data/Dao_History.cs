// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_History

internal class Dao_History
{
    #region Fields

    // Add fields here if needed in the future

    #endregion

    #region Constructors

    // Add constructors here if needed in the future

    #endregion

    #region History Methods

    public static async Task AddTransactionHistoryAsync(Model_TransactionHistory history)
    {
        string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
        await using MySqlConnection connection = new(connectionString);
        await connection.OpenAsync();

        await using MySqlCommand command = new("inv_transaction_Add", connection);
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
        command.Parameters.AddWithValue("@in_ReceiveDate", history.DateTime);

        await command.ExecuteNonQueryAsync();
    }

    #endregion

    #region Helpers

    // Add helper methods here

    #endregion
}

#endregion
