using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Data;

#region Dao_PartType

internal static class Dao_PartType
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

    #region Insert

    internal static async Task InsertPartTypeAsync(string partType, string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@partType"] = partType,
                ["@user"] = user
            };
            await HelperDatabaseCore.ExecuteNonQuery(
                "INSERT INTO `md_item_types` (`Type`, `Issued By`) VALUES (@partType, @user);",
                parameters, useAsync, CommandType.Text);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Update

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
            await HelperDatabaseCore.ExecuteNonQuery(
                "UPDATE `md_item_types` SET `Type` = @newType, `Issued By` = @user WHERE `Type` = @partType",
                parameters, useAsync, CommandType.Text);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Delete

    internal static async Task DeletePartTypeAsync(string partType, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@partType"] = partType };
            await HelperDatabaseCore.ExecuteNonQuery(
                "DELETE FROM `md_item_types` WHERE `Type` = @partType",
                parameters, useAsync, CommandType.Text);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllPartTypesAsync(bool useAsync = false)
    {
        try
        {
            return await HelperDatabaseCore.ExecuteDataTable(
                "SELECT * FROM `md_item_types`",
                null, useAsync, CommandType.Text);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task<DataRow?> GetPartTypeByNameAsync(string partType, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@partType"] = partType };
            var table = await HelperDatabaseCore.ExecuteDataTable(
                "SELECT * FROM `md_item_types` WHERE `Type` = @partType",
                parameters, useAsync, CommandType.Text);
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    internal static async Task<bool> PartTypeExistsAsync(string partType, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@partType"] = partType };
            var result = await HelperDatabaseCore.ExecuteScalar(
                "SELECT COUNT(*) FROM `md_item_types` WHERE `Type` = @partType",
                parameters, useAsync, CommandType.Text);
            return Convert.ToInt32(result) > 0;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return false;
        }
    }

    #endregion
}

#endregion