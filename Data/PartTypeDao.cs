using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace MTM_WIP_Application.Data;

internal static class PartTypeDao
{
    // --- Existence Check ---
    internal static async Task<bool> PartTypeExists(string partType, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@partType"] = partType };
        var result = await SqlHelper.ExecuteScalar(
            "SELECT COUNT(*) FROM `item_types` WHERE `Type` = @partType",
            parameters, useAsync: useAsync);
        return Convert.ToInt32(result) > 0;
    }

    // --- Insert ---
    internal static async Task InsertPartType(string partType, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@partType"] = partType,
            ["@user"] = user
        };
        await ExecuteNonQueryAsync(
            "INSERT INTO `item_types` (`Type`, `ID`, `Issued By`) VALUES (@partType, NULL, @user);",
            parameters, useAsync);
    }

    // --- Get All ---
    internal static async Task<DataTable> GetAllPartTypes(bool useAsync = false)
    {
        return await GetPartTypeByQueryAsync("SELECT * FROM `item_types`", null, useAsync);
    }

    // --- Get By Name ---
    internal static async Task<DataRow?> GetPartTypeByName(string partType, bool useAsync = false)
    {
        var table = await GetPartTypeByQueryAsync(
            "SELECT * FROM `item_types` WHERE `Type` = @partType",
            new Dictionary<string, object> { ["@partType"] = partType }, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    // --- Update ---
    internal static async Task UpdatePartType(string partType, string newType, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@partType"] = partType,
            ["@newType"] = newType,
            ["@user"] = user
        };
        await ExecuteNonQueryAsync(
            "UPDATE `item_types` SET `Type` = @newType, `Issued By` = @user WHERE `Type` = @partType",
            parameters, useAsync);
    }

    // --- Delete ---
    internal static async Task DeletePartType(string partType, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@partType"] = partType };
        await ExecuteNonQueryAsync(
            "DELETE FROM `item_types` WHERE `Type` = @partType",
            parameters, useAsync);
    }

    // --- Helpers ---
    private static async Task<DataTable> GetPartTypeByQueryAsync(string sql, Dictionary<string, object>? parameters,
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
}