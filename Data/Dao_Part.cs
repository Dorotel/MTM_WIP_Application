using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Data;

#region Dao_Part

internal static class Dao_Part
{
    #region Delete

    internal static async Task<DaoResult> DeletePart(string partNumber, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new() { ["ItemNumber"] = partNumber }; // p_ prefix added automatically

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_part_ids_Delete_ByItemNumber",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess)
            {
                return DaoResult.Success($"Part {partNumber} deleted successfully");
            }
            else
            {
                return DaoResult.Failure($"Failed to delete part {partNumber}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "DeletePart");
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
                ["ItemNumber"] = partNumber,     // p_ prefix added automatically
                ["Customer"] = "", 
                ["Description"] = "", 
                ["IssuedBy"] = user, 
                ["ItemType"] = partType
            };
            
            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_part_ids_Add_Part",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess)
            {
                return DaoResult.Success($"Part {partNumber} created successfully");
            }
            else
            {
                return DaoResult.Failure($"Failed to create part {partNumber}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "InsertPart");
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
                ["ID"] = existingPart.Data["ID"],                                   // p_ prefix added automatically
                ["ItemNumber"] = partNumber,
                ["Customer"] = existingPart.Data["Customer"]?.ToString() ?? "",
                ["Description"] = existingPart.Data["Description"]?.ToString() ?? "",
                ["IssuedBy"] = user,
                ["ItemType"] = partType
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_part_ids_Update_Part",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess)
            {
                return DaoResult.Success($"Part {partNumber} updated successfully");
            }
            else
            {
                return DaoResult.Failure($"Failed to update part {partNumber}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "UpdatePart");
            return DaoResult.Failure($"Error updating part {partNumber}", ex);
        }
    }

    #endregion

    #region Read

    internal static async Task<DaoResult<DataTable>> GetAllParts(bool useAsync = false)
    {
        try
        {
            var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "md_part_ids_Get_All",
                null, // No parameters needed
                null, // No progress helper for this method
                useAsync
            );
            
            if (result.IsSuccess && result.Data != null)
            {
                return DaoResult<DataTable>.Success(result.Data, $"Retrieved {result.Data.Rows.Count} parts");
            }
            else
            {
                return DaoResult<DataTable>.Failure($"Failed to retrieve parts: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "GetAllParts");
            return DaoResult<DataTable>.Failure("Error retrieving parts", ex);
        }
    }

    internal static async Task<DaoResult<DataRow>> GetPartByNumber(string partNumber, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new() { ["ItemNumber"] = partNumber }; // p_ prefix added automatically

            var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "md_part_ids_Get_ByItemNumber",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess && result.Data != null && result.Data.Rows.Count > 0)
            {
                return DaoResult<DataRow>.Success(result.Data.Rows[0], $"Found part {partNumber}");
            }
            else
            {
                return DaoResult<DataRow>.Failure($"Part {partNumber} not found");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "GetPartByNumber");
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
            var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "md_item_types_GetDistinct",
                null, // No parameters needed
                null, // No progress helper for this method
                useAsync
            );
                
            if (result.IsSuccess && result.Data != null)
            {
                return DaoResult<DataTable>.Success(result.Data, $"Retrieved {result.Data.Rows.Count} part types");
            }
            else
            {
                return DaoResult<DataTable>.Failure($"Failed to retrieve part types: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "GetPartTypes");
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
                ["ItemNumber"] = itemNumber,     // p_ prefix added automatically
                ["Customer"] = customer,
                ["Description"] = description,
                ["IssuedBy"] = issuedBy,
                ["ItemType"] = type
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_part_ids_Add_Part",
                parameters,
                null, // No progress helper for this method
                useAsync
            );
                
            if (result.IsSuccess)
            {
                return DaoResult.Success($"Part {itemNumber} added successfully");
            }
            else
            {
                return DaoResult.Failure($"Failed to add part {itemNumber}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "AddPartWithStoredProcedure");
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
                ["ID"] = id,                     // p_ prefix added automatically
                ["ItemNumber"] = itemNumber,
                ["Customer"] = customer,
                ["Description"] = description,
                ["IssuedBy"] = issuedBy,
                ["ItemType"] = type
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_part_ids_Update_Part",
                parameters,
                null, // No progress helper for this method
                useAsync
            );
                
            if (result.IsSuccess)
            {
                return DaoResult.Success($"Part {itemNumber} updated successfully");
            }
            else
            {
                return DaoResult.Failure($"Failed to update part {itemNumber}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "UpdatePartWithStoredProcedure");
            return DaoResult.Failure($"Error updating part {itemNumber}", ex);
        }
    }

    #endregion
}

#endregion
