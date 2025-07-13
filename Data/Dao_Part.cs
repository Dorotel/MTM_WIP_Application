using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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
            "INSERT INTO `md_part_ids` (`Item Number`, `IssuedBy`, `ItemType`) VALUES (@partNumber, @user, @partType);",
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
            "UPDATE `md_part_ids` SET `ItemType` = @partType, `IssuedBy` = @user WHERE `Item Number` = @partNumber",
            parameters, useAsync);
    }

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllParts(bool useAsync = false)
    {
        try
        {
            // No parameters needed for getting all parts
            return await HelperDatabaseCore.ExecuteDataTable(
                "md_part_ids_Get_All",
                null,
                useAsync,
                CommandType.StoredProcedure);
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

    internal static async Task<DataRow?> GetPartByNumber(string partNumber, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_ItemNumber"] = partNumber
            };

            var table = await HelperDatabaseCore.ExecuteDataTable(
                "md_part_ids_Get_ByItemNumber",
                parameters,
                useAsync,
                CommandType.StoredProcedure);

            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return null;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    #endregion

    #region Existence Check

    internal static async Task<bool> PartExists(string partNumber, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_ItemNumber"] = partNumber
            };

            var result = await HelperDatabaseCore.ExecuteScalar(
                "md_part_ids_Get_ByItemNumber",
                parameters,
                useAsync,
                CommandType.StoredProcedure);

            return Convert.ToInt32(result) > 0;
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return false;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return false;
        }
    }

    #endregion

    #region New Stored Procedure Methods

    internal static async Task AddPartWithStoredProcedure(string itemNumber, string customer, string description,
        string issuedBy, string type, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_ItemNumber"] = itemNumber,
                ["p_Customer"] = customer,
                ["p_Description"] = description,
                ["p_IssuedBy"] = issuedBy,
                ["p_ItemType"] = type
            };

            await HelperDatabaseCore.ExecuteNonQuery("md_part_ids_Add_Part", parameters, useAsync,
                CommandType.StoredProcedure);
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

    internal static async Task UpdatePartWithStoredProcedure(int id, string itemNumber, string customer,
        string description, string issuedBy, string type, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_ID"] = id,
                ["p_ItemNumber"] = itemNumber,
                ["p_Customer"] = customer,
                ["p_Description"] = description,
                ["p_IssuedBy"] = issuedBy,
                ["p_ItemType"] = type
            };

            await HelperDatabaseCore.ExecuteNonQuery("md_part_ids_Update_Part", parameters, useAsync,
                CommandType.StoredProcedure);
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

    internal static async Task DeletePartByItemNumber(string itemNumber, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_ItemNumber"] = itemNumber
            };

            await HelperDatabaseCore.ExecuteNonQuery("md_part_ids_Delete_ByItemNumber", parameters, useAsync,
                CommandType.StoredProcedure);
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

    internal static async Task<DataTable> GetPartTypes(bool useAsync = false)
    {
        try
        {
            return await HelperDatabaseCore.ExecuteDataTable(
                "SELECT DISTINCT `ItemType` FROM `md_item_types` ORDER BY `ItemType`", useAsync: useAsync,
                commandType: CommandType.Text);
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