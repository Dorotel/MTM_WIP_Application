using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Data
{
    public static class Dao_QuickButtons
    {
        #region Quick Button Methods

        public static async Task UpdateQuickButtonAsync(string user, int position, string partId, string operation, int quantity)
        {
            try
            {
                // Ensure position is always 1-10 (never 0)
                int safePosition = Math.Max(1, Math.Min(10, position + 1));
                
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core
                Dictionary<string, object> parameters = new()
                {
                    ["User"] = user,
                    ["Position"] = safePosition,
                    ["PartID"] = partId,
                    ["Operation"] = operation,
                    ["Quantity"] = quantity
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_last_10_transactions_Update_ByUserAndPosition_1",
                    parameters,
                    null, // No progress helper for this method
                    true  // Use async
                );

                if (!result.IsSuccess)
                {
                    LoggingUtility.Log($"UpdateQuickButtonAsync failed: {result.ErrorMessage}");
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, true, "UpdateQuickButtonAsync");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "UpdateQuickButtonAsync");
            }
        }

        public static async Task RemoveQuickButtonAndShiftAsync(string user, int position)
        {
            try
            {
                // Ensure position is always 1-10 (never 0)
                int safePosition = Math.Max(1, Math.Min(10, position + 1));
                
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core
                Dictionary<string, object> parameters = new()
                {
                    ["User"] = user,
                    ["Position"] = safePosition
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_last_10_transactions_RemoveAndShift_ByUser_1",
                    parameters,
                    null, // No progress helper for this method
                    true  // Use async
                );

                if (!result.IsSuccess)
                {
                    LoggingUtility.Log($"RemoveQuickButtonAndShiftAsync failed: {result.ErrorMessage}");
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, true, "RemoveQuickButtonAndShiftAsync");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "RemoveQuickButtonAndShiftAsync");
            }
        }

        public static async Task AddQuickButtonAsync(string user, string partId, string operation, int quantity, int position)
        {
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core
                Dictionary<string, object> parameters = new()
                {
                    ["User"] = user,
                    ["PartID"] = partId,
                    ["Operation"] = operation,
                    ["Quantity"] = quantity,
                    ["Position"] = position
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_last_10_transactions_Add_AtPosition_1",
                    parameters,
                    null, // No progress helper for this method
                    true  // Use async
                );

                if (!result.IsSuccess)
                {
                    LoggingUtility.Log($"AddQuickButtonAsync failed: {result.ErrorMessage}");
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, true, "AddQuickButtonAsync");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "AddQuickButtonAsync");
            }
        }

        public static async Task MoveQuickButtonAsync(string user, int fromPosition, int toPosition)
        {
            try
            {
                // Ensure positions are always 1-10 (never 0)
                int safeFrom = Math.Max(1, Math.Min(10, fromPosition + 1));
                int safeTo = Math.Max(1, Math.Min(10, toPosition + 1));
                
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core
                Dictionary<string, object> parameters = new()
                {
                    ["User"] = user,
                    ["FromPosition"] = safeFrom,
                    ["ToPosition"] = safeTo
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_last_10_transactions_Move_1",
                    parameters,
                    null, // No progress helper for this method
                    true  // Use async
                );

                if (!result.IsSuccess)
                {
                    LoggingUtility.Log($"MoveQuickButtonAsync failed: {result.ErrorMessage}");
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, true, "MoveQuickButtonAsync");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "MoveQuickButtonAsync");
            }
        }

        public static async Task DeleteAllQuickButtonsForUserAsync(string user)
        {
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core
                Dictionary<string, object> parameters = new()
                {
                    ["User"] = user
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_last_10_transactions_DeleteAll_ByUser",
                    parameters,
                    null, // No progress helper for this method
                    true  // Use async
                );

                if (!result.IsSuccess)
                {
                    LoggingUtility.Log($"DeleteAllQuickButtonsForUserAsync failed: {result.ErrorMessage}");
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, true, "DeleteAllQuickButtonsForUserAsync");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "DeleteAllQuickButtonsForUserAsync");
            }
        }

        public static async Task AddOrShiftQuickButtonAsync(string user, string partId, string operation, int quantity)
        {
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core
                Dictionary<string, object> parameters = new()
                {
                    ["User"] = user,
                    ["PartID"] = partId,
                    ["Operation"] = operation,
                    ["Quantity"] = quantity
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_last_10_transactions_AddOrShift_ByUser",
                    parameters,
                    null, // No progress helper for this method
                    true  // Use async
                );

                if (!result.IsSuccess)
                {
                    LoggingUtility.Log($"AddOrShiftQuickButtonAsync failed: {result.ErrorMessage}");
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, true, "AddOrShiftQuickButtonAsync");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "AddOrShiftQuickButtonAsync");
            }
        }

        public static async Task RemoveAndShiftQuickButtonAsync(string user, int position)
        {
            try
            {
                int safePosition = Math.Max(1, Math.Min(10, position + 1));
                
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core
                Dictionary<string, object> parameters = new()
                {
                    ["User"] = user,
                    ["Position"] = safePosition
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_last_10_transactions_RemoveAndShift_ByUser_1",
                    parameters,
                    null, // No progress helper for this method
                    true  // Use async
                );

                if (!result.IsSuccess)
                {
                    LoggingUtility.Log($"RemoveAndShiftQuickButtonAsync failed: {result.ErrorMessage}");
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, true, "RemoveAndShiftQuickButtonAsync");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "RemoveAndShiftQuickButtonAsync");
            }
        }

        public static async Task AddQuickButtonAtPositionAsync(string user, string partId, string operation, int quantity, int position)
        {
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core
                Dictionary<string, object> parameters = new()
                {
                    ["User"] = user,
                    ["PartID"] = partId,
                    ["Operation"] = operation,
                    ["Quantity"] = quantity,
                    ["Position"] = position
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_last_10_transactions_Add_AtPosition_1",
                    parameters,
                    null, // No progress helper for this method
                    true  // Use async
                );

                if (!result.IsSuccess)
                {
                    LoggingUtility.Log($"AddQuickButtonAtPositionAsync failed: {result.ErrorMessage}");
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, true, "AddQuickButtonAtPositionAsync");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "AddQuickButtonAtPositionAsync");
            }
        }

        #endregion
    }
}
