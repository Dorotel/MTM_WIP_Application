using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Data;

#region Dao_Location

internal static class Dao_Location
{
    #region Delete

    internal static async Task<DaoResult> DeleteLocation(string location, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["Location"] = location }; // p_ prefix added automatically

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_locations_Delete_ByLocation",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess)
            {
                return DaoResult.Success($"Location {location} deleted successfully");
            }
            else
            {
                return DaoResult.Failure($"Failed to delete location {location}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "DeleteLocation");
            return DaoResult.Failure($"Error deleting location {location}", ex);
        }
    }

    #endregion

    #region Insert

    internal static async Task<DaoResult> InsertLocation(string location, string building, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["Location"] = location,                         // p_ prefix added automatically
                ["IssuedBy"] = Model_AppVariables.User ?? "System",
                ["Building"] = building
            };
            
            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_locations_Add_Location",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess)
            {
                return DaoResult.Success($"Location {location} created successfully");
            }
            else
            {
                return DaoResult.Failure($"Failed to create location {location}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "InsertLocation");
            return DaoResult.Failure($"Error creating location {location}", ex);
        }
    }

    #endregion

    #region Update

    internal static async Task<DaoResult> UpdateLocation(string oldLocation, string newLocation, string building,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["OldLocation"] = oldLocation,                   // p_ prefix added automatically
                ["Location"] = newLocation,
                ["IssuedBy"] = Model_AppVariables.User ?? "System",
                ["Building"] = building
            };
            
            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "md_locations_Update_Location",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (result.IsSuccess)
            {
                return DaoResult.Success($"Location updated from {oldLocation} to {newLocation}");
            }
            else
            {
                return DaoResult.Failure($"Failed to update location {oldLocation}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "UpdateLocation");
            return DaoResult.Failure($"Error updating location {oldLocation}", ex);
        }
    }

    internal static async Task<DaoResult<DataTable>> GetAllLocations(bool useAsync = false)
    {
        try
        {
            var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "md_locations_Get_All",
                null, // No parameters needed
                null, // No progress helper for this method
                useAsync
            );
                
            if (result.IsSuccess && result.Data != null)
            {
                return DaoResult<DataTable>.Success(result.Data, $"Retrieved {result.Data.Rows.Count} locations");
            }
            else
            {
                return DaoResult<DataTable>.Failure($"Failed to retrieve locations: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "GetAllLocations");
            return DaoResult<DataTable>.Failure("Error retrieving locations", ex);
        }
    }

    internal static async Task<DaoResult<DataRow>> GetLocationByName(string location, bool useAsync = false)
    {
        try
        {
            var allLocationsResult = await GetAllLocations(useAsync);
            if (!allLocationsResult.IsSuccess)
            {
                return DaoResult<DataRow>.Failure(allLocationsResult.ErrorMessage, allLocationsResult.Exception);
            }

            var table = allLocationsResult.Data!;
            var rows = table.Select($"Location = '{location.Replace("'", "''")}'");
            
            if (rows.Length > 0)
            {
                return DaoResult<DataRow>.Success(rows[0], $"Found location {location}");
            }

            return DaoResult<DataRow>.Failure($"Location {location} not found");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            return DaoResult<DataRow>.Failure($"Error retrieving location {location}", ex);
        }
    }

    #endregion

    #region Existence Check

    internal static async Task<DaoResult<bool>> LocationExists(string location, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["Location"] = location }; // p_ prefix added automatically

            var result = await Helper_Database_StoredProcedure.ExecuteScalarWithStatus(
                Model_AppVariables.ConnectionString,
                "md_locations_Exists_ByLocation",
                parameters,
                null, // No progress helper for this method
                useAsync
            );
                
            if (result.IsSuccess && result.Data != null)
            {
                bool exists = Convert.ToInt32(result.Data) > 0;
                return DaoResult<bool>.Success(exists, exists ? $"Location {location} exists" : $"Location {location} does not exist");
            }
            else
            {
                return DaoResult<bool>.Failure($"Failed to check location {location}: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, "LocationExists");
            return DaoResult<bool>.Failure($"Error checking location {location}", ex);
        }
    }

    #endregion
}

#endregion
