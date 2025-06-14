using System.Data;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.Data;

#region Dao_PartType

internal static class Dao_PartType
{
    public static Helper_MySql HelperMySql =
        new(Helper_SqlVariables.GetConnectionString(
            Core_WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            Core_WipAppVariables.User,
            Core_WipAppVariables.UserPin
        ));

    #region Add / Update / Delete

    internal static async Task InsertPartTypeAsync(string partType, string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@partType"] = partType,
                ["@user"] = user
            };
            await HelperMySql.ExecuteNonQuery(
                "INSERT INTO `md_item_types` (`Type`, `Issued By`) VALUES (@partType, @user);",
                parameters, useAsync, CommandType.Text);
        }
        catch (Exception ex)
        {
            ApplicationLog.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task UpdatePartTypeAsync(string partType, string newType, string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@partType"] = partType,
                ["@newType"] = newType,
                ["@user"] = user
            };
            await HelperMySql.ExecuteNonQuery(
                "UPDATE `md_item_types` SET `Type` = @newType, `Issued By` = @user WHERE `Type` = @partType",
                parameters, useAsync, CommandType.Text);
        }
        catch (Exception ex)
        {
            ApplicationLog.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task DeletePartTypeAsync(string partType, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@partType"] = partType };
            await HelperMySql.ExecuteNonQuery(
                "DELETE FROM `md_item_types` WHERE `Type` = @partType",
                parameters, useAsync, CommandType.Text);
        }
        catch (Exception ex)
        {
            ApplicationLog.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Queries

    internal static async Task<DataTable> GetAllPartTypesAsync(bool useAsync = false)
    {
        try
        {
            return await HelperMySql.ExecuteDataTable(
                "SELECT * FROM `md_item_types`",
                null, useAsync, CommandType.Text);
        }
        catch (Exception ex)
        {
            ApplicationLog.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task<DataRow?> GetPartTypeByNameAsync(string partType, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@partType"] = partType };
            var table = await HelperMySql.ExecuteDataTable(
                "SELECT * FROM `md_item_types` WHERE `Type` = @partType",
                parameters, useAsync, CommandType.Text);
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }
        catch (Exception ex)
        {
            ApplicationLog.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    internal static async Task<bool> PartTypeExistsAsync(string partType, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@partType"] = partType };
            var result = await HelperMySql.ExecuteScalar(
                "SELECT COUNT(*) FROM `md_item_types` WHERE `Type` = @partType",
                parameters, useAsync, CommandType.Text);
            return Convert.ToInt32(result) > 0;
        }
        catch (Exception ex)
        {
            ApplicationLog.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return false;
        }
    }

    #endregion
}

#endregion