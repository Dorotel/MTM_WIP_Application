using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Data;

internal static class LocationDao
{
    internal static async Task<bool> LocationExists(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@location"] = location
        };
        var result = await SqlHelper.ExecuteScalar(
            "SELECT COUNT(*) FROM `locations` WHERE `Location` = @location",
            parameters,
            useAsync: useAsync);
        return Convert.ToInt32(result) > 0;
    }

    internal static async Task InsertLocation(string location, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@location"] = location,
            ["@user"] = user
        };
        await SqlHelper.ExecuteNonQuery(
            "INSERT INTO `locations` (`Location`, `ID`, `Issued By`) VALUES (@location, NULL, @user);",
            parameters,
            useAsync: useAsync);
    }

    internal static async Task<DataTable> GetAllLocations(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable("SELECT * FROM `locations`", useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
            return new DataTable();
        }
    }

    internal static async Task<DataRow?> GetLocationByName(string location, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@location"] = location
            };
            var table = await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `locations` WHERE `Location` = @location",
                parameters,
                useAsync: useAsync);
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
            return null;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
            return null;
        }
    }

    internal static async Task UpdateLocation(string location, string newLocation, string user,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@location"] = location,
                ["@newLocation"] = newLocation,
                ["@user"] = user
            };
            await SqlHelper.ExecuteNonQuery(
                "UPDATE `locations` SET `Location` = @newLocation, `Issued By` = @user WHERE `Location` = @location",
                parameters,
                useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }

    internal static async Task DeleteLocation(string location, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@location"] = location
            };
            await SqlHelper.ExecuteNonQuery(
                "DELETE FROM `locations` WHERE `Location` = @location",
                parameters,
                useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }
}