using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Data;

internal static class PartTypeDao
{
    internal static async Task<bool> PartTypeExists(string partType, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@partType"] = partType
        };
        var result = await SqlHelper.ExecuteScalar(
            "SELECT COUNT(*) FROM `item_types` WHERE `Type` = @partType",
            parameters,
            useAsync: useAsync);
        return Convert.ToInt32(result) > 0;
    }

    internal static async Task InsertPartType(string partType, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@partType"] = partType,
            ["@user"] = user
        };
        await SqlHelper.ExecuteNonQuery(
            "INSERT INTO `item_types` (`Type`, `ID`, `Issued By`) VALUES (@partType, NULL, @user);",
            parameters,
            useAsync: useAsync);
    }

    internal static async Task<DataTable> GetAllPartTypes(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable("SELECT * FROM `item_types`", useAsync: useAsync);
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

    internal static async Task<DataRow?> GetPartTypeByName(string partType, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@partType"] = partType
            };
            var table = await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `item_types` WHERE `Type` = @partType",
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

    internal static async Task UpdatePartType(string partType, string newType, string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@partType"] = partType,
                ["@newType"] = newType,
                ["@user"] = user
            };
            await SqlHelper.ExecuteNonQuery(
                "UPDATE `item_types` SET `Type` = @newType, `Issued By` = @user WHERE `Type` = @partType",
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

    internal static async Task DeletePartType(string partType, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@partType"] = partType
            };
            await SqlHelper.ExecuteNonQuery(
                "DELETE FROM `item_types` WHERE `Type` = @partType",
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