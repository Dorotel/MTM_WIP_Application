using System.Data;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Data;

internal static class LocationDao
{
    // --- Delete ---
    internal static async Task DeleteLocation(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@location"] = location };
        await ExecuteNonQueryAsync(
            "DELETE FROM `locations` WHERE `Location` = @location",
            parameters, useAsync);
    }

    private static async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters, bool useAsync)
    {
        try
        {
            await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    // --- Get All ---
    internal static async Task<DataTable> GetAllLocations(bool useAsync = false)
    {
        return await GetLocationByQueryAsync("SELECT * FROM `locations`", null, useAsync);
    }

    // --- Get By Name ---
    internal static async Task<DataRow?> GetLocationByName(string location, bool useAsync = false)
    {
        var table = await GetLocationByQueryAsync(
            "SELECT * FROM `locations` WHERE `Location` = @location",
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
                ? await SqlHelper.ExecuteDataTable(sql, useAsync: useAsync)
                : await SqlHelper.ExecuteDataTable(sql, parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
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
            "INSERT INTO `locations` (`Location`, `ID`, `Issued By`) VALUES (@location, NULL, @user);",
            parameters, useAsync);
    }

    // --- Existence Check ---
    internal static async Task<bool> LocationExists(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@location"] = location };
        var result = await SqlHelper.ExecuteScalar(
            "SELECT COUNT(*) FROM `locations` WHERE `Location` = @location",
            parameters, useAsync: useAsync);
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
            "UPDATE `locations` SET `Location` = @newLocation, `Issued By` = @user WHERE `Location` = @location",
            parameters, useAsync);
    }
}