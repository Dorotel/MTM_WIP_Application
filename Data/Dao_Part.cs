using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_Part

internal static class Dao_Part
{
    #region Fields

    public static Helper_Database_Core HelperDatabaseCore =
        new(Helper_Database_Variables.GetConnectionString(
            Model_AppVariables.WipServerAddress,
            "mtm_wip_application",
            Model_AppVariables.User,
            Model_AppVariables.UserPin
        ));

    #endregion

    #region Delete

    internal static async Task<DaoResult> DeletePart(string partNumber, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new() { ["p_ItemNumber"] = partNumber };
            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery(
                "md_part_ids_Delete_ByItemNumber",
                parameters, useAsync, CommandType.StoredProcedure);

            return rowsAffected > 0 
                ? DaoResult.Success($"Part {partNumber} deleted successfully", rowsAffected)
                : DaoResult.Failure($"Part {partNumber} not found or could not be deleted");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error deleting part {partNumber}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Error deleting part {partNumber}", ex);
        }
    }

    #endregion

    #region Insert

    internal static async Task<DaoResult> InsertPart(string partNumber, string user, string partType, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new()
            {
                ["p_ItemNumber"] = partNumber, 
                ["p_Customer"] = "", 
                ["p_Description"] = "", 
                ["p_IssuedBy"] = user, 
                ["p_ItemType"] = partType
            };
            
            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery(
                "md_part_ids_Add_Part",
                parameters, useAsync, CommandType.StoredProcedure);

            return rowsAffected > 0 
                ? DaoResult.Success($"Part {partNumber} created successfully", rowsAffected)
                : DaoResult.Failure($"Failed to create part {partNumber}");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error creating part {partNumber}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Error creating part {partNumber}", ex);
        }
    }

    #endregion

    #region Update

    internal static async Task<DaoResult> UpdatePart(string partNumber, string partType, string user, bool useAsync = false)
    {
        try
        {
            // First get the part ID to update
            var existingPart = await GetPartByNumber(partNumber, useAsync);
            if (!existingPart.IsSuccess || existingPart.Data == null)
            {
                return DaoResult.Failure($"Part {partNumber} not found");
            }

            Dictionary<string, object> parameters = new()
            {
                ["p_ID"] = existingPart.Data["ID"],
                ["p_ItemNumber"] = partNumber,
                ["p_Customer"] = existingPart.Data["Customer"]?.ToString() ?? "",
                ["p_Description"] = existingPart.Data["Description"]?.ToString() ?? "",
                ["p_IssuedBy"] = user,
                ["p_ItemType"] = partType
            };

            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery(
                "md_part_ids_Update_Part",
                parameters, useAsync, CommandType.StoredProcedure);

            return rowsAffected > 0 
                ? DaoResult.Success($"Part {partNumber} updated successfully", rowsAffected)
                : DaoResult.Failure($"Failed to update part {partNumber}");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error updating part {partNumber}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Error updating part {partNumber}", ex);
        }
    }

    #endregion

    #region Read

    internal static async Task<DaoResult<DataTable>> GetAllParts(bool useAsync = false)
    {
        try
        {
            DataTable result = await HelperDatabaseCore.ExecuteDataTable(
                "md_part_ids_Get_All",
                null, useAsync, CommandType.StoredProcedure);
            
            return DaoResult<DataTable>.Success(result, $"Retrieved {result.Rows.Count} parts");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult<DataTable>.Failure("Database error retrieving parts", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult<DataTable>.Failure("Error retrieving parts", ex);
        }
    }

    internal static async Task<DaoResult<DataRow>> GetPartByNumber(string partNumber, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new() { ["p_ItemNumber"] = partNumber };
            DataTable table = await HelperDatabaseCore.ExecuteDataTable(
                "md_part_ids_Get_ByItemNumber",
                parameters, useAsync, CommandType.StoredProcedure);

            if (table.Rows.Count > 0)
            {
                return DaoResult<DataRow>.Success(table.Rows[0], $"Found part {partNumber}");
            }

            return DaoResult<DataRow>.Failure($"Part {partNumber} not found");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult<DataRow>.Failure($"Database error retrieving part {partNumber}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult<DataRow>.Failure($"Error retrieving part {partNumber}", ex);
        }
    }

    internal static async Task<DaoResult<bool>> PartExists(string partNumber, bool useAsync = false)
    {
        try
        {
            var result = await GetPartByNumber(partNumber, useAsync);
            return DaoResult<bool>.Success(result.IsSuccess, result.IsSuccess ? $"Part {partNumber} exists" : $"Part {partNumber} does not exist");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            return DaoResult<bool>.Failure($"Error checking if part {partNumber} exists", ex);
        }
    }

    internal static async Task<DaoResult<DataTable>> GetPartTypes(bool useAsync = false)
    {
        try
        {
            DataTable result = await HelperDatabaseCore.ExecuteDataTable(
                "md_item_types_GetDistinct",
                null, useAsync, CommandType.StoredProcedure);
                
            return DaoResult<DataTable>.Success(result, $"Retrieved {result.Rows.Count} part types");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult<DataTable>.Failure("Database error retrieving part types", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult<DataTable>.Failure("Error retrieving part types", ex);
        }
    }

    #endregion

    #region New Stored Procedure Methods

    internal static async Task<DaoResult> AddPartWithStoredProcedure(string itemNumber, string customer, string description,
        string issuedBy, string type, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new()
            {
                ["p_ItemNumber"] = itemNumber,
                ["p_Customer"] = customer,
                ["p_Description"] = description,
                ["p_IssuedBy"] = issuedBy,
                ["p_ItemType"] = type
            };

            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery("md_part_ids_Add_Part", parameters, useAsync,
                CommandType.StoredProcedure);
                
            return rowsAffected > 0 
                ? DaoResult.Success($"Part {itemNumber} added successfully", rowsAffected)
                : DaoResult.Failure($"Failed to add part {itemNumber}");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error adding part {itemNumber}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Error adding part {itemNumber}", ex);
        }
    }

    internal static async Task<DaoResult> UpdatePartWithStoredProcedure(int id, string itemNumber, string customer,
        string description, string issuedBy, string type, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new()
            {
                ["p_ID"] = id,
                ["p_ItemNumber"] = itemNumber,
                ["p_Customer"] = customer,
                ["p_Description"] = description,
                ["p_IssuedBy"] = issuedBy,
                ["p_ItemType"] = type
            };

            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery("md_part_ids_Update_Part", parameters, useAsync,
                CommandType.StoredProcedure);
                
            return rowsAffected > 0 
                ? DaoResult.Success($"Part {itemNumber} updated successfully", rowsAffected)
                : DaoResult.Failure($"Failed to update part {itemNumber}");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error updating part {itemNumber}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Error updating part {itemNumber}", ex);
        }
    }

    #endregion
}

#endregion
