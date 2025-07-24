using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Data
{
    public static class Dao_QuickButtons
    {
        public static async Task UpdateQuickButtonAsync(string user, int position, string partId, string operation, int quantity)
        {
            try
            {
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("sys_last_10_transactions_Update_ByUserAndPosition_1", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("p_User", user);
                cmd.Parameters.AddWithValue("p_Position", position);
                cmd.Parameters.AddWithValue("p_PartID", partId);
                cmd.Parameters.AddWithValue("p_Operation", operation);
                cmd.Parameters.AddWithValue("p_Quantity", quantity);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                throw;
            }
        }

        public static async Task RemoveQuickButtonAndShiftAsync(string user, int position)
        {
            try
            {
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("sys_last_10_transactions_RemoveAndShift_ByUser_1", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("p_User", user);
                cmd.Parameters.AddWithValue("p_Position", position);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                throw;
            }
        }

        public static async Task AddQuickButtonAsync(string user, string partId, string operation, int quantity, int position)
        {
            try
            {
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("sys_last_10_transactions_AddQuickButton_1", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("p_User", user);
                cmd.Parameters.AddWithValue("p_PartID", partId);
                cmd.Parameters.AddWithValue("p_Operation", operation);
                cmd.Parameters.AddWithValue("p_Quantity", quantity);
                cmd.Parameters.AddWithValue("p_Position", position);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                throw;
            }
        }

        public static async Task MoveQuickButtonAsync(string user, int fromPosition, int toPosition)
        {
            try
            {
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("sys_last_10_transactions_Move_1", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("p_User", user);
                cmd.Parameters.AddWithValue("p_FromPosition", fromPosition);
                cmd.Parameters.AddWithValue("p_ToPosition", toPosition);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                throw;
            }
        }

        public static async Task DeleteAllQuickButtonsForUserAsync(string user)
        {
            try
            {
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("DELETE FROM sys_last_10_transactions WHERE User = @User", conn);
                cmd.Parameters.AddWithValue("@User", user);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                throw;
            }
        }
    }
}
