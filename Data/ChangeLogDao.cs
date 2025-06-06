using System.Data;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.Data;

#region ChangeLogDao

internal static class ChangeLogDao
{
    #region SQL Helper

    public static SqlHelper SqlHelper =
        new(SqlVariables.GetConnectionString(
            WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            WipAppVariables.User,
            WipAppVariables.UserPin
        ));

    #endregion

    #region ChangeLog Table Operations (log_changelog)

    internal static async Task DeleteChangeLogEntryAsync(string version, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_Version"] = version
            };
            await SqlHelper.ExecuteNonQuery(
                "log_changelog_Delete_ByVersion",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task<DataTable> GetAllChangeLogEntriesAsync(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable(
                "log_changelog_Get_All",
                null, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
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
            var table = await SqlHelper.ExecuteDataTable(
                "log_changelog_Get_ByVersion",
                parameters, useAsync, CommandType.StoredProcedure);
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    internal static async Task InsertChangeLogEntryAsync(string version, string notes, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_Version"] = version,
                ["p_Notes"] = notes
            };
            await SqlHelper.ExecuteNonQuery(
                "log_changelog_Add_ChangeLog",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task UpdateChangeLogNotesAsync(string version, string notes, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_Version"] = version,
                ["p_Notes"] = notes
            };
            await SqlHelper.ExecuteNonQuery(
                "log_changelog_Update_Notes",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Additional Methods (Version Queries)

    internal static async Task<List<string>> Primary_ChangeLog_Get_AllVersionsAsync(bool useAsync = false)
    {
        var versions = new List<string>();
        try
        {
            var table = await SqlHelper.ExecuteDataTable(
                "log_changelog_Get_All",
                null, useAsync, CommandType.StoredProcedure);

            foreach (DataRow row in table.Rows)
                if (row["Version"] is string v)
                    versions.Add(v);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }

        return versions;
    }

    internal static async Task<string> Primary_ChangeLog_Get_VersionNotesAsync(string? version = null,
        bool useAsync = false)
    {
        try
        {
            if (string.IsNullOrEmpty(version)) return string.Empty;

            var table = await SqlHelper.ExecuteDataTable(
                "log_changelog_Get_ByVersion",
                new Dictionary<string, object> { ["p_Version"] = version },
                useAsync, CommandType.StoredProcedure);

            return table.Rows.Count > 0 && table.Rows[0]["Notes"] != DBNull.Value
                ? table.Rows[0]["Notes"].ToString() ?? string.Empty
                : string.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }

    #endregion
}

#endregion