using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_Part

internal static class Dao_Part
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

    internal static async Task DeletePart(string partNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@partNumber"] = partNumber };
        await ExecuteNonQueryAsync(
            "DELETE FROM `md_part_ids` WHERE `Item Number` = @partNumber",
            parameters, useAsync);
    }

    #endregion

    #region Insert

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

    #endregion

    #region Update

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

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllParts(bool useAsync = false)
    {
        return await GetPartByQueryAsync("SELECT * FROM `md_part_ids`", null, useAsync);
    }

    internal static async Task<DataRow?> GetPartByNumber(string partNumber, bool useAsync = false)
    {
        var table = await GetPartByQueryAsync(
            "SELECT * FROM `md_part_ids` WHERE `Item Number` = @partNumber",
            new Dictionary<string, object> { ["@partNumber"] = partNumber }, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    #endregion

    #region Existence Check

    internal static async Task<bool> PartExists(string partNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@partNumber"] = partNumber };
        var result = await HelperDatabaseCore.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_part_ids` WHERE `Item Number` = @partNumber",
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

    private static async Task<DataTable> GetPartByQueryAsync(string sql, Dictionary<string, object>? parameters,
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