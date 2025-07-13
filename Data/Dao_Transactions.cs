

using System.Text;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

internal class Dao_Transactions
{
    private readonly string _connectionString;

    public Dao_Transactions(string connectionString) => _connectionString = connectionString;

    public List<Model_Transactions> SearchTransactions(
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
        List<Model_Transactions> transactions = new();
        StringBuilder query = new("SELECT * FROM inv_transaction WHERE 1=1");
        List<MySqlParameter> parameters = new();

        if (!isAdmin && !string.IsNullOrEmpty(userName))
        {
            query.Append(" AND User = @User");
            parameters.Add(new MySqlParameter("@User", userName));
        }
        else if (isAdmin && !string.IsNullOrEmpty(userName))
        {
            query.Append(" AND (@User IS NULL OR User = @User)");
            parameters.Add(new MySqlParameter("@User", userName));
        }

        if (!string.IsNullOrEmpty(partID))
        {
            query.Append(" AND PartID = @PartID");
            parameters.Add(new MySqlParameter("@PartID", partID));
        }

        if (!string.IsNullOrEmpty(batchNumber))
        {
            query.Append(" AND `Batch Number` = @BatchNumber");
            parameters.Add(new MySqlParameter("@BatchNumber", batchNumber));
        }

        if (!string.IsNullOrEmpty(fromLocation))
        {
            query.Append(" AND FromLocation = @FromLocation");
            parameters.Add(new MySqlParameter("@FromLocation", fromLocation));
        }

        if (!string.IsNullOrEmpty(toLocation))
        {
            query.Append(" AND ToLocation = @ToLocation");
            parameters.Add(new MySqlParameter("@ToLocation", toLocation));
        }

        if (!string.IsNullOrEmpty(operation))
        {
            query.Append(" AND Operation = @Operation");
            parameters.Add(new MySqlParameter("@Operation", operation));
        }

        if (transactionType.HasValue)
        {
            query.Append(" AND TransactionType = @TransactionType");
            parameters.Add(new MySqlParameter("@TransactionType", transactionType.ToString()));
        }

        if (quantity.HasValue)
        {
            query.Append(" AND Quantity = @Quantity");
            parameters.Add(new MySqlParameter("@Quantity", quantity.Value));
        }

        if (!string.IsNullOrEmpty(notes))
        {
            query.Append(" AND Notes LIKE @Notes");
            parameters.Add(new MySqlParameter("@Notes", "%" + notes + "%"));
        }

        if (!string.IsNullOrEmpty(itemType))
        {
            query.Append(" AND ItemType = @ItemType");
            parameters.Add(new MySqlParameter("@ItemType", itemType));
        }

        if (fromDate.HasValue)
        {
            query.Append(" AND DateTime >= @FromDate");
            parameters.Add(new MySqlParameter("@FromDate", fromDate.Value));
        }

        if (toDate.HasValue)
        {
            query.Append(" AND DateTime <= @ToDate");
            parameters.Add(new MySqlParameter("@ToDate", toDate.Value));
        }

        HashSet<string> validColumns = new()
        {
            "ID",
            "TransactionType",
            "BatchNumber",
            "PartID",
            "FromLocation",
            "ToLocation",
            "Operation",
            "Quantity",
            "Notes",
            "User",
            "ItemType",
            "ReceiveDate"
        };
        if (!validColumns.Contains(sortColumn))
        {
            sortColumn = "ReceiveDate";
        }

        query.Append($" ORDER BY `{sortColumn}` {(sortDescending ? "DESC" : "ASC")}");

        int offset = (page - 1) * pageSize;
        query.Append(" LIMIT @PageSize OFFSET @Offset");
        parameters.Add(new MySqlParameter("@PageSize", pageSize));
        parameters.Add(new MySqlParameter("@Offset", offset));

        using (MySqlConnection conn = new(_connectionString))
        {
            conn.Open();
            using (MySqlCommand cmd = new(query.ToString(), conn))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                using (MySqlDataReader? reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transactions.Add(MapTransaction(reader));
                    }
                }
            }
        }

        return transactions;
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
                : Convert.ToDateTime(reader["DateTime"])
        };
}
