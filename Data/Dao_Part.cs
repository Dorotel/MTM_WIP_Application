using System.Data;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Data;

internal static class Dao_Part
{
    public static Helper_MySql HelperMySql =
        new(Helper_SqlVariables.GetConnectionString(
            Core_WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            Core_WipAppVariables.User,
            Core_WipAppVariables.UserPin
        ));

    // --- Delete ---
    internal static async Task DeletePart(string partNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@partNumber"] = partNumber };
        await ExecuteNonQueryAsync(
            "DELETE FROM `md_part_ids` WHERE `Item Number` = @partNumber",
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
    internal static async Task<DataTable> GetAllParts(bool useAsync = false)
    {
        return await GetPartByQueryAsync("SELECT * FROM `md_part_ids`", null, useAsync);
    }

    // --- Get By Number ---
    internal static async Task<DataRow?> GetPartByNumber(string partNumber, bool useAsync = false)
    {
        var table = await GetPartByQueryAsync(
            "SELECT * FROM `md_part_ids` WHERE `Item Number` = @partNumber",
            new Dictionary<string, object> { ["@partNumber"] = partNumber }, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    // --- Helpers ---
    private static async Task<DataTable> GetPartByQueryAsync(string sql, Dictionary<string, object>? parameters,
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
    internal static async Task InsertPart(string partNumber, string user, string partType, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@partNumber"] = partNumber,
            ["@user"] = user,
            ["@partType"] = partType
        };
        await ExecuteNonQueryAsync(
            "INSERT INTO `md_part_ids` (`Item Number`, `Issued By`, `Type`) VALUES (@partNumber, @user, @partType);",
            parameters, useAsync);
    }

    // --- Existence Check ---
    internal static async Task<bool> PartExists(string partNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@partNumber"] = partNumber };
        var result = await HelperMySql.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_part_ids` WHERE `Item Number` = @partNumber",
            parameters, useAsync);
        return Convert.ToInt32(result) > 0;
    }

    // --- Update ---
    internal static async Task UpdatePart(string partNumber, string partType, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@partNumber"] = partNumber,
            ["@partType"] = partType,
            ["@user"] = user
        };
        await ExecuteNonQueryAsync(
            "UPDATE `md_part_ids` SET `Type` = @partType, `Issued By` = @user WHERE `Item Number` = @partNumber",
            parameters, useAsync);
    }
}