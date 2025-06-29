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
        var parameters = new Dictionary<string, object> { ["@location"] = location };
        await ExecuteNonQueryAsync(
            "DELETE FROM `md_locations` WHERE `Location` = @location",
            parameters, useAsync);
    }

    #endregion

    #region Insert

    internal static async Task InsertLocation(string location, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@location"] = location,
            ["@user"] = user
        };
        await ExecuteNonQueryAsync(
            "INSERT INTO `md_locations` (`Location`, `Issued By`) VALUES (@location, @user);",
            parameters, useAsync);
    }

    #endregion

    #region Update

    internal static async Task UpdateLocation(string location, string newLocation, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@location"] = location,
            ["@newLocation"] = newLocation,
            ["@user"] = user
        };
        await ExecuteNonQueryAsync(
            "UPDATE `md_locations` SET `Location` = @newLocation, `Issued By` = @user WHERE `Location` = @location",
            parameters, useAsync);
    }

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllLocations(bool useAsync = false)
    {
        return await GetLocationByQueryAsync("SELECT * FROM `md_locations`", null, useAsync);
    }

    internal static async Task<DataRow?> GetLocationByName(string location, bool useAsync = false)
    {
        var table = await GetLocationByQueryAsync(
            "SELECT * FROM `md_locations` WHERE `Location` = @location",
            new Dictionary<string, object> { ["@location"] = location }, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
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