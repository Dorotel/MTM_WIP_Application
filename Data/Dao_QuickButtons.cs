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
                // Ensure position is always 1-10 (never 0)
                int safePosition = Math.Max(1, Math.Min(10, position + 1));
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("sys_last_10_transactions_Update_ByUserAndPosition_1", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("p_User", user);
                cmd.Parameters.AddWithValue("p_Position", safePosition);
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
                // Ensure position is always 1-10 (never 0)
                int safePosition = Math.Max(1, Math.Min(10, position + 1));
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("sys_last_10_transactions_RemoveAndShift_ByUser_1", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("p_User", user);
                cmd.Parameters.AddWithValue("p_Position", safePosition);
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
                // Always insert at position 1, shift others down
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                // Shift existing buttons down (from 9 to 1, so no overwrite)
                using (var shiftCmd = new MySqlCommand(@"
                    UPDATE sys_last_10_transactions
                    SET Position = Position + 1
                    WHERE User = @User AND Position BETWEEN 1 AND 9
                    ORDER BY Position DESC
                ", conn))
                {
                    shiftCmd.Parameters.AddWithValue("@User", user);
                    await shiftCmd.ExecuteNonQueryAsync();
                }

                // Insert new button at position 1
                using (var insertCmd = new MySqlCommand(@"
                    INSERT INTO sys_last_10_transactions (User, PartID, Operation, Quantity, Position)
                    VALUES (@User, @PartID, @Operation, @Quantity, 1)
                ", conn))
                {
                    insertCmd.Parameters.AddWithValue("@User", user);
                    insertCmd.Parameters.AddWithValue("@PartID", partId);
                    insertCmd.Parameters.AddWithValue("@Operation", operation);
                    insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                    await insertCmd.ExecuteNonQueryAsync();
                }

                // Optionally, delete any at position > 10
                using (var cleanupCmd = new MySqlCommand(@"
                    DELETE FROM sys_last_10_transactions
                    WHERE User = @User AND Position > 10
                ", conn))
                {
                    cleanupCmd.Parameters.AddWithValue("@User", user);
                    await cleanupCmd.ExecuteNonQueryAsync();
                }
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
                // Ensure positions are always 1-10 (never 0)
                int safeFrom = Math.Max(1, Math.Min(10, fromPosition + 1));
                int safeTo = Math.Max(1, Math.Min(10, toPosition + 1));
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("sys_last_10_transactions_Move_1", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("p_User", user);
                cmd.Parameters.AddWithValue("p_FromPosition", safeFrom);
                cmd.Parameters.AddWithValue("p_ToPosition", safeTo);
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

        public static async Task AddOrShiftQuickButtonAsync(string user, string partId, string operation, int quantity)
        {
            try
            {
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                // Check for existing match
                using (var checkCmd = new MySqlCommand(@"
                    SELECT COUNT(*) FROM sys_last_10_transactions
                    WHERE User = @User AND PartID = @PartID AND Operation = @Operation AND Quantity = @Quantity
                ", conn))
                {
                    checkCmd.Parameters.AddWithValue("@User", user);
                    checkCmd.Parameters.AddWithValue("@PartID", partId);
                    checkCmd.Parameters.AddWithValue("@Operation", operation);
                    checkCmd.Parameters.AddWithValue("@Quantity", quantity);
                    var exists = (long)await checkCmd.ExecuteScalarAsync() > 0;
                    if (exists)
                        return; // Already present, do nothing
                }

                // Shift all existing buttons down (10->removed, 9->10, ..., 1->2)
                using (var shiftCmd = new MySqlCommand(@"
                    UPDATE sys_last_10_transactions
                    SET Position = Position + 1
                    WHERE User = @User AND Position BETWEEN 1 AND 9
                    ORDER BY Position DESC
                ", conn))
                {
                    shiftCmd.Parameters.AddWithValue("@User", user);
                    await shiftCmd.ExecuteNonQueryAsync();
                }

                // Remove any at position > 10
                using (var cleanupCmd = new MySqlCommand(@"
                    DELETE FROM sys_last_10_transactions
                    WHERE User = @User AND Position > 10
                ", conn))
                {
                    cleanupCmd.Parameters.AddWithValue("@User", user);
                    await cleanupCmd.ExecuteNonQueryAsync();
                }

                // Insert new button at position 1
                using (var insertCmd = new MySqlCommand(@"
                    INSERT INTO sys_last_10_transactions (User, PartID, Operation, Quantity, Position)
                    VALUES (@User, @PartID, @Operation, @Quantity, 1)
                ", conn))
                {
                    insertCmd.Parameters.AddWithValue("@User", user);
                    insertCmd.Parameters.AddWithValue("@PartID", partId);
                    insertCmd.Parameters.AddWithValue("@Operation", operation);
                    insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                    await insertCmd.ExecuteNonQueryAsync();
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                throw;
            }
        }

        public static async Task RemoveAndShiftQuickButtonAsync(string user, int position)
        {
            try
            {
                int safePosition = Math.Max(1, Math.Min(10, position + 1));
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                // Remove the button at the given position
                using (var deleteCmd = new MySqlCommand(@"
                    DELETE FROM sys_last_10_transactions
                    WHERE User = @User AND Position = @Position
                ", conn))
                {
                    deleteCmd.Parameters.AddWithValue("@User", user);
                    deleteCmd.Parameters.AddWithValue("@Position", safePosition);
                    await deleteCmd.ExecuteNonQueryAsync();
                }

                // Shift up all buttons below the removed one
                using (var shiftCmd = new MySqlCommand(@"
                    UPDATE sys_last_10_transactions
                    SET Position = Position - 1
                    WHERE User = @User AND Position > @Position AND Position <= 10
                    ORDER BY Position ASC
                ", conn))
                {
                    shiftCmd.Parameters.AddWithValue("@User", user);
                    shiftCmd.Parameters.AddWithValue("@Position", safePosition);
                    await shiftCmd.ExecuteNonQueryAsync();
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                throw;
            }
        }

        public static async Task AddQuickButtonAtPositionAsync(string user, string partId, string operation, int quantity, int position)
        {
            try
            {
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                // Direct insert at specific position without shifting
                using (var insertCmd = new MySqlCommand(@"
                    INSERT INTO sys_last_10_transactions (User, PartID, Operation, Quantity, Position)
                    VALUES (@User, @PartID, @Operation, @Quantity, @Position)
                ", conn))
                {
                    insertCmd.Parameters.AddWithValue("@User", user);
                    insertCmd.Parameters.AddWithValue("@PartID", partId);
                    insertCmd.Parameters.AddWithValue("@Operation", operation);
                    insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                    insertCmd.Parameters.AddWithValue("@Position", position);
                    await insertCmd.ExecuteNonQueryAsync();
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                throw;
            }
        }
    }
}
