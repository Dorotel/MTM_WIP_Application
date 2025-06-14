using System.Data;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Data;

internal static class Dao_Location
{
    public static Helper_MySql HelperMySql =
        new(Helper_SqlVariables.GetConnectionString(
            Core_WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            Core_WipAppVariables.User,
            Core_WipAppVariables.UserPin
        ));

    // --- Delete ---
    internal static async Task DeleteLocation(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@location"] = location };
        await ExecuteNonQueryAsync(
            "DELETE FROM `md_locations` WHERE `Location` = @location",
            parameters, useAsync);
    }

    private static async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters, bool useAsync)
    {
        try
        {
            await HelperMySql.ExecuteNonQuery(sql, parameters, useAsync);
        }
        catch (MySqlException ex)
        {
            ApplicationLog.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    // --- Get All ---
    internal static async Task<DataTable> GetAllLocations(bool useAsync = false)
    {
        return await GetLocationByQueryAsync("SELECT * FROM `md_locations`", null, useAsync);
    }

    // --- Get By Name ---
    internal static async Task<DataRow?> GetLocationByName(string location, bool useAsync = false)
    {
        var table = await GetLocationByQueryAsync(
            "SELECT * FROM `md_locations` WHERE `Location` = @location",
            new Dictionary<string, object> { ["@location"] = location }, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    // --- Helpers ---
    private static async Task<DataTable> GetLocationByQueryAsync(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            return parameters == null
                ? await HelperMySql.ExecuteDataTable(sql, useAsync: useAsync)
                : await HelperMySql.ExecuteDataTable(sql, parameters, useAsync);
        }
        catch (MySqlException ex)
        {
            ApplicationLog.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    // --- Insert ---
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

    // --- Existence Check ---
    internal static async Task<bool> LocationExists(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@location"] = location };
        var result = await HelperMySql.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_locations` WHERE `Location` = @location",
            parameters, useAsync);
        return Convert.ToInt32(result) > 0;
    }

    // --- Update ---
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
}