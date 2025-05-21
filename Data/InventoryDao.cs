using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTM_WIP_Application.Models;

namespace MTM_WIP_Application.Data;

internal static class InventoryDao
{
    internal static async Task<List<SavedLocationsTable>?> InventoryTab_SaveAsync(bool useAsync = false)
    {
        try
        {
            List<SavedLocationsTable> returnThese = [];
            var type = await InventoryTab_GetItemTypeAsync(useAsync);

            var parameters1 = new Dictionary<string, object>
            {
                ["@Location"] = WipAppVariables.Location,
                ["@PartID"] = WipAppVariables.partId,
                ["@Op"] = WipAppVariables.Operation,
                ["@Notes"] = WipAppVariables.Notes,
                ["@Quantity"] = WipAppVariables.InventoryQuantity,
                ["@User"] = WipAppVariables.User,
                ["@Type"] = type
            };
            await SqlHelper.ExecuteNonQuery(
                "INSERT INTO `saved_locations` (`ID`, `Location`, `Item Number`, `Op`, `Notes`, `Quantity`, `Date_Time`, `User` , `Item Type`) " +
                "VALUES (NULL, @Location, @PartID, @Op, @Notes, @Quantity, CURRENT_TIMESTAMP, @User, @Type);",
                parameters1, useAsync: useAsync);

            var parameters2 = new Dictionary<string, object>
            {
                ["@User"] = WipAppVariables.User,
                ["@PartID"] = WipAppVariables.partId,
                ["@Location"] = WipAppVariables.Location,
                ["@Type"] = WipAppVariables.PartType,
                ["@Quantity"] = WipAppVariables.InventoryQuantity
            };
            await SqlHelper.ExecuteNonQuery(
                "INSERT INTO `input_history` (`User`, `Part ID`, `Location`, `Type`, `Quantity`) " +
                "VALUES(@User, @PartID, @Location, @Type, @Quantity)",
                parameters2, useAsync: useAsync);

            AppLogger.Log("InventoryTab_Save executed successfully.");
            return returnThese;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in InventoryTab_Save: " + ex.Message);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return null;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in InventoryTab_Save: " + ex.Message);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    internal static async Task<string> InventoryTab_GetItemTypeAsync(bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@PartID"] = WipAppVariables.partId
            };
            using var reader = useAsync
                ? await SqlHelper.ExecuteReader("SELECT * FROM `part_ids` WHERE `Item Number` = @PartID",
                    parameters, useAsync: true)
                : SqlHelper.ExecuteReader("SELECT * FROM `part_ids` WHERE `Item Number` = @PartID", parameters)
                    .Result;
            while (reader.Read())
            {
                var partId = reader.GetString(0);
                if (partId == WipAppVariables.partId) return reader.GetString(3);
            }

            AppLogger.Log("InventoryTab_GetItemType executed successfully for part ID: " + WipAppVariables.partId);
            return "Unknown";
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in InventoryTab_GetItemType: " + ex.Message);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return "Unknown";
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in InventoryTab_GetItemType: " + ex.Message);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return "Unknown";
        }
    }

    internal static async Task<List<SavedLocationsTable>?> RemoveTab_DeleteAsync(bool useAsync = false)
    {
        try
        {
            List<SavedLocationsTable> returnThese = [];
            var parameters1 = new Dictionary<string, object>
            {
                ["@RemoveID"] = WipAppVariables.removeId
            };
            await SqlHelper.ExecuteNonQuery(
                "DELETE FROM saved_locations WHERE `saved_locations`.`ID` = @RemoveID",
                parameters1, useAsync: useAsync);

            var parameters2 = new Dictionary<string, object>
            {
                ["@User"] = WipAppVariables.RemoveUser,
                ["@PartID"] = WipAppVariables.RemovePartNumber,
                ["@Location"] = WipAppVariables.RemoveLocation,
                ["@Quantity"] = WipAppVariables.RemoveQuantity
            };
            await SqlHelper.ExecuteNonQuery(
                "INSERT INTO `output_history` (`User`, `Part ID`, `Location`, `Quantity`) VALUES(@User, @PartID, @Location, @Quantity)",
                parameters2, useAsync: useAsync);

            AppLogger.Log("RemoveTab_Delete executed successfully for RemoveID: " + WipAppVariables.removeId);
            return returnThese;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in RemoveTab_Delete: " + ex.Message);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return null;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in RemoveTab_Delete: " + ex.Message);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    internal static async Task<DataTable> GetAllInventoryAsync(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable("SELECT * FROM `saved_locations` ORDER BY `Date_Time` DESC",
                useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task<DataTable> GetInventoryByPartIdAsync(string partId, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@PartID"] = partId
            };
            return await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `saved_locations` WHERE `Item Number` = @PartID ORDER BY `Date_Time` DESC",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task<DataTable> GetInventoryByLocationAsync(string location, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Location"] = location
            };
            return await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `saved_locations` WHERE `Location` = @Location ORDER BY `Date_Time` DESC",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task UpdateInventoryQuantityAsync(int id, int newQuantity, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Id"] = id,
                ["@Quantity"] = newQuantity
            };
            await SqlHelper.ExecuteNonQuery(
                "UPDATE `saved_locations` SET `Quantity` = @Quantity WHERE `ID` = @Id",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task DeleteInventoryByIdAsync(int id, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Id"] = id
            };
            await SqlHelper.ExecuteNonQuery(
                "DELETE FROM `saved_locations` WHERE `ID` = @Id",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task<DataTable> GetInventoryByDateRangeAsync(DateTime start, DateTime end,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Start"] = start,
                ["@End"] = end
            };
            return await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `saved_locations` WHERE `Date_Time` BETWEEN @Start AND @End ORDER BY `Date_Time` DESC",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task<DataTable> GetInventoryByUserAsync(string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = user
            };
            return await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `saved_locations` WHERE `User` = @User ORDER BY `Date_Time` DESC",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task<DataRow?> GetInventoryByIdAsync(int id, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Id"] = id
            };
            var table = await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `saved_locations` WHERE `ID` = @Id",
                parameters, useAsync: useAsync);
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return null;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    internal static async Task UpdateInventoryNotesAsync(int id, string notes, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Id"] = id,
                ["@Notes"] = notes
            };
            await SqlHelper.ExecuteNonQuery(
                "UPDATE `saved_locations` SET `Notes` = @Notes WHERE `ID` = @Id",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task BulkDeleteInventoryByLocationAsync(string location, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Location"] = location
            };
            await SqlHelper.ExecuteNonQuery(
                "DELETE FROM `saved_locations` WHERE `Location` = @Location",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task<int> GetTotalQuantityByPartIdAsync(string partId, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@PartID"] = partId
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT SUM(`Quantity`) FROM `saved_locations` WHERE `Item Number` = @PartID",
                parameters, useAsync: useAsync);
            return result != DBNull.Value && result != null ? Convert.ToInt32(result) : 0;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return 0;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return 0;
        }
    }
}