using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
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

    internal static async Task DeleteLocation(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["p_Location"] = location };
        await ExecuteStoredProcedureAsync("md_locations_Delete_ByLocation", parameters, useAsync);
    }

    #endregion

    #region Insert

    internal static async Task InsertLocation(string location, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["p_Location"] = location,
            ["p_IssuedBy"] = user
        };
        await ExecuteStoredProcedureAsync("md_locations_Add_Location", parameters, useAsync);
    }

    #endregion

    #region Update

    internal static async Task UpdateLocation(int id, string newLocation, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["p_ID"] = id,
            ["p_Location"] = newLocation,
            ["p_IssuedBy"] = user
        };
        await ExecuteStoredProcedureAsync("md_locations_Update_Location", parameters, useAsync);
    }

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllLocations(bool useAsync = false)
    {
        return await GetLocationByStoredProcedureAsync("md_locations_Get_All", null, useAsync);
    }

    internal static async Task<DataRow?> GetLocationByName(string location, bool useAsync = false)
    {
        var table = await GetAllLocations(useAsync);
        var rows = table.Select($"Location = '{location.Replace("'", "''")}'");
        return rows.Length > 0 ? rows[0] : null;
    }

    #endregion

    #region Existence Check

    internal static async Task<bool> LocationExists(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@location"] = location };
        var result = await HelperDatabaseCore.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_locations` WHERE `Location` = @location",
            parameters, useAsync);
        return Convert.ToInt32(result) > 0;
    }

    #endregion

    #region Helpers

    private static async Task ExecuteStoredProcedureAsync(string storedProcedure, Dictionary<string, object> parameters, bool useAsync)
    {
        try
        {
            await HelperDatabaseCore.ExecuteStoredProcedure(storedProcedure, parameters, useAsync);
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    private static async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters, bool useAsync)
    {
        try
        {
            await HelperDatabaseCore.ExecuteNonQuery(sql, parameters, useAsync);
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    private static async Task<DataTable> GetLocationByStoredProcedureAsync(string storedProcedure, Dictionary<string, object>? parameters, bool useAsync)
    {
        try
        {
            return parameters == null
                ? await HelperDatabaseCore.ExecuteStoredProcedureDataTable(storedProcedure, useAsync: useAsync)
                : await HelperDatabaseCore.ExecuteStoredProcedureDataTable(storedProcedure, parameters, useAsync);
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    private static async Task<DataTable> GetLocationByQueryAsync(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            return parameters == null
                ? await HelperDatabaseCore.ExecuteDataTable(sql, useAsync: useAsync)
                : await HelperDatabaseCore.ExecuteDataTable(sql, parameters, useAsync);
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    #endregion
}

#endregion