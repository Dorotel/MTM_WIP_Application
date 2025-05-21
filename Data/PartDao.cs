using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Data;

internal static class PartDao
{
    internal static async Task<bool> PartExists(string partNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@partNumber"] = partNumber
        };
        var result = await SqlHelper.ExecuteScalar(
            "SELECT COUNT(*) FROM `part_ids` WHERE `Item Number` = @partNumber",
            parameters,
            useAsync: useAsync);
        return Convert.ToInt32(result) > 0;
    }

    internal static async Task InsertPart(string partNumber, string user, string partType, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@partNumber"] = partNumber,
            ["@user"] = user,
            ["@partType"] = partType
        };
        await SqlHelper.ExecuteNonQuery(
            "INSERT INTO `part_ids` (`Item Number`, `ID`, `Issued By`, `Type`) VALUES (@partNumber, NULL, @user, @partType);",
            parameters,
            useAsync: useAsync);
    }

    internal static async Task<DataTable> GetAllParts(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable("SELECT * FROM `part_ids`", useAsync: useAsync);
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

    internal static async Task<DataRow?> GetPartByNumber(string partNumber, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@partNumber"] = partNumber
            };
            var table = await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `part_ids` WHERE `Item Number` = @partNumber",
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

    internal static async Task UpdatePart(string partNumber, string partType, string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@partNumber"] = partNumber,
                ["@partType"] = partType,
                ["@user"] = user
            };
            await SqlHelper.ExecuteNonQuery(
                "UPDATE `part_ids` SET `Type` = @partType, `Issued By` = @user WHERE `Item Number` = @partNumber",
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

    internal static async Task DeletePart(string partNumber, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@partNumber"] = partNumber
            };
            await SqlHelper.ExecuteNonQuery(
                "DELETE FROM `part_ids` WHERE `Item Number` = @partNumber",
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