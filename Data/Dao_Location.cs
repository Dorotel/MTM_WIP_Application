using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_Location

internal static class Dao_Location
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

    internal static async Task<DaoResult> DeleteLocation(string location, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["p_Location"] = location };
            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery("md_locations_Delete_ByLocation", parameters, useAsync,
                CommandType.StoredProcedure);

            return rowsAffected > 0 
                ? DaoResult.Success($"Location {location} deleted successfully", rowsAffected)
                : DaoResult.Failure($"Location {location} not found or could not be deleted");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error deleting location {location}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
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
                ["p_Location"] = location,
                ["p_IssuedBy"] = Model_AppVariables.User ?? "System",
                ["p_Building"] = building
            };
            
            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery("md_locations_Add_Location", parameters, useAsync,
                CommandType.StoredProcedure);

            return rowsAffected > 0 
                ? DaoResult.Success($"Location {location} created successfully", rowsAffected)
                : DaoResult.Failure($"Failed to create location {location}");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error creating location {location}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
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
                ["p_OldLocation"] = oldLocation,
                ["p_Location"] = newLocation,
                ["p_IssuedBy"] = Model_AppVariables.User ?? "System",
                ["p_Building"] = building
            };
            
            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery("md_locations_Update_Location", parameters, useAsync,
                CommandType.StoredProcedure);

            return rowsAffected > 0 
                ? DaoResult.Success($"Location updated from {oldLocation} to {newLocation}", rowsAffected)
                : DaoResult.Failure($"Location {oldLocation} not found or could not be updated");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error updating location {oldLocation}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Error updating location {oldLocation}", ex);
        }
    }

    internal static async Task<DaoResult<DataTable>> GetAllLocations(bool useAsync = false)
    {
        try
        {
            DataTable result = await HelperDatabaseCore.ExecuteDataTable("md_locations_Get_All", null, useAsync,
                CommandType.StoredProcedure);
                
            return DaoResult<DataTable>.Success(result, $"Retrieved {result.Rows.Count} locations");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult<DataTable>.Failure("Database error retrieving locations", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
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
            var parameters = new Dictionary<string, object> { ["p_Location"] = location };
            var result = await HelperDatabaseCore.ExecuteScalar(
                "md_locations_Exists_ByLocation",
                parameters, useAsync, CommandType.StoredProcedure);
                
            bool exists = Convert.ToInt32(result) > 0;
            return DaoResult<bool>.Success(exists, exists ? $"Location {location} exists" : $"Location {location} does not exist");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult<bool>.Failure($"Database error checking location {location}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult<bool>.Failure($"Error checking location {location}", ex);
        }
    }

    #endregion
}

#endregion
