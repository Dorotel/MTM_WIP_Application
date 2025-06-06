using System.Data;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Data;

internal static class PartTypeDao
{
    public static SqlHelper SqlHelper =
        new(SqlVariables.GetConnectionString(
            WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            WipAppVariables.User,
            WipAppVariables.UserPin
        ));

    // --- Delete ---
    internal static async Task DeletePartType(string partType, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@partType"] = partType };
        await ExecuteNonQueryAsync(
            "DELETE FROM `md_item_types` WHERE `Type` = @partType",
            parameters, useAsync);
    }

    private static async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters, bool useAsync)
    {
        try
        {
            await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync);
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
    internal static async Task<DataTable> GetAllPartTypes(bool useAsync = false)
    {
        return await GetPartTypeByQueryAsync("SELECT * FROM `md_item_types`", null, useAsync);
    }

    // --- Get By Name ---
    internal static async Task<DataRow?> GetPartTypeByName(string partType, bool useAsync = false)
    {
        var table = await GetPartTypeByQueryAsync(
            "SELECT * FROM `md_item_types` WHERE `Type` = @partType",
            new Dictionary<string, object> { ["@partType"] = partType }, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    // --- Helpers ---
    private static async Task<DataTable> GetPartTypeByQueryAsync(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            return parameters == null
                ? await SqlHelper.ExecuteDataTable(sql, useAsync: useAsync)
                : await SqlHelper.ExecuteDataTable(sql, parameters, useAsync);
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
    internal static async Task InsertPartType(string partType, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@partType"] = partType,
            ["@user"] = user
        };
        await ExecuteNonQueryAsync(
            "INSERT INTO `md_item_types` (`Type`, `Issued By`) VALUES (@partType, @user);",
            parameters, useAsync);
    }

    // --- Existence Check ---
    internal static async Task<bool> PartTypeExists(string partType, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@partType"] = partType };
        var result = await SqlHelper.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_item_types` WHERE `Type` = @partType",
            parameters, useAsync);
        return Convert.ToInt32(result) > 0;
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
            "UPDATE `md_item_types` SET `Type` = @newType, `Issued By` = @user WHERE `Type` = @partType",
            parameters, useAsync);
    }
}