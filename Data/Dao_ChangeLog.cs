using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Data;

#region Dao_ChangeLog

internal static class Dao_ChangeLog
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

    internal static async Task DeleteChangeLogEntryAsync(string version, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_Version"] = version
            };
            await HelperDatabaseCore.ExecuteNonQuery(
                "log_changelog_Delete_ByVersion",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllChangeLogEntriesAsync(bool useAsync = false)
    {
        try
        {
            return await HelperDatabaseCore.ExecuteDataTable(
                "log_changelog_Get_All",
                null, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task<DataRow?> GetChangeLogEntryByVersionAsync(string version, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_Version"] = version
            };
            var table = await HelperDatabaseCore.ExecuteDataTable(
                "log_changelog_Get_ByVersion",
                parameters, useAsync, CommandType.StoredProcedure);
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    #endregion

    #region Insert

    internal static async Task InsertChangeLogEntryAsync(string version, string notes, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_Version"] = version,
                ["p_Notes"] = notes
            };
            await HelperDatabaseCore.ExecuteNonQuery(
                "log_changelog_Add_ChangeLog",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Update

    internal static async Task UpdateChangeLogNotesAsync(string version, string notes, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_Version"] = version,
                ["p_Notes"] = notes
            };
            await HelperDatabaseCore.ExecuteNonQuery(
                "log_changelog_Update_Notes",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Additional Methods

    internal static async Task<List<string>> Primary_ChangeLog_Get_AllVersionsAsync(bool useAsync = false)
    {
        var versions = new List<string>();
        try
        {
            var table = await HelperDatabaseCore.ExecuteDataTable(
                "log_changelog_Get_All",
                null, useAsync, CommandType.StoredProcedure);

            foreach (DataRow row in table.Rows)
                if (row["Version"] is string v)
                    versions.Add(v);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }

        return versions;
    }

    internal static async Task<string> Primary_ChangeLog_Get_VersionNotesAsync(string? version = null,
        bool useAsync = false)
    {
        try
        {
            if (string.IsNullOrEmpty(version)) return string.Empty;

            var table = await HelperDatabaseCore.ExecuteDataTable(
                "log_changelog_Get_ByVersion",
                new Dictionary<string, object> { ["p_Version"] = version },
                useAsync, CommandType.StoredProcedure);

            return table.Rows.Count > 0 && table.Rows[0]["Notes"] != DBNull.Value
                ? table.Rows[0]["Notes"].ToString() ?? string.Empty
                : string.Empty;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }

    #endregion
}

#endregion