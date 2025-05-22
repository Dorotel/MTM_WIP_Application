using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;
using System.Data;

namespace MTM_WIP_Application.Data;

internal static class ChangeLogDao
{
    internal static async Task<string> GetWipServerAddressAsync(string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = user
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT `WipServerAddress` FROM `users` WHERE `User` = @User",
                parameters, useAsync: useAsync);
            return result?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }

    internal static async Task<string> GetWipServerPortAsync(string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = user
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT `WipServerPort` FROM `users` WHERE `User` = @User",
                parameters, useAsync: useAsync);
            return result?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }

    internal static async Task SetWipServerAddressAsync(string user, string address, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@User"] = user,
            ["@WipServerAddress"] = address
        };
        await SqlHelper.ExecuteNonQuery(
            "UPDATE `users` SET `WipServerAddress` = @WipServerAddress WHERE `User` = @User",
            parameters, useAsync: useAsync);
    }

    internal static async Task SetWipServerPortAsync(string user, string port, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@User"] = user,
            ["@WipServerPort"] = port
        };
        await SqlHelper.ExecuteNonQuery(
            "UPDATE `users` SET `WipServerPort` = @WipServerPort WHERE `User` = @User",
            parameters, useAsync: useAsync);
    }

    internal static async Task Primary_ChangeLog_Set_Theme_FontSizeAsync(string value, string user,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@Theme_FontSize"] = value,
            ["@User"] = user
        };
        await SqlHelper.ExecuteNonQuery(
            "UPDATE `users` SET `Theme_FontSize` = @Theme_FontSize WHERE `User` = @User",
            parameters, useAsync: useAsync);
    }

    internal static async Task Primary_ChangeLog_Set_Theme_NameAsync(string value, string user,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@Theme_Name"] = value,
            ["@User"] = user
        };
        await SqlHelper.ExecuteNonQuery(
            "UPDATE `users` SET `Theme_Name` = @Theme_Name WHERE `User` = @User",
            parameters, useAsync: useAsync);
    }

    internal static async Task<string> Primary_ChangeLog_Get_Theme_NameAsync(bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = WipAppVariables.User
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT `Theme_Name` FROM `users` WHERE `User` = @User",
                parameters, useAsync: useAsync);
            return result?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }

    internal static async Task<int?> Primary_ChangeLog_Get_Visual_Theme_FontSizeAsync(bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = WipAppVariables.User
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT `Theme_FontSize` FROM `users` WHERE `User` = @User",
                parameters, useAsync: useAsync);
            return result != null && int.TryParse(result.ToString(), out var size) ? size : null;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    internal static async Task<List<string>> Primary_ChangeLog_Get_AllVersionsAsync(bool useAsync = false)
    {
        var versions = new List<string>();
        try
        {
            using var reader = useAsync
                ? await SqlHelper.ExecuteReader("SELECT `Version` FROM `changelog` ORDER BY `Version` DESC",
                    useAsync: true)
                : SqlHelper.ExecuteReader("SELECT `Version` FROM `changelog` ORDER BY `Version` DESC").Result;
            while (reader.Read()) versions.Add(reader.GetString("Version"));
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }

        return versions;
    }

    internal static async Task Primary_ChangeLog_Set_LastShownAsync(string value, string user,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@LastShown"] = value,
            ["@User"] = user
        };
        await SqlHelper.ExecuteNonQuery(
            "UPDATE `users` SET `LastShownVersion` = @LastShown WHERE `User` = @User",
            parameters, useAsync: useAsync);
    }

    internal static async Task Primary_ChangeLog_Set_SwitchAsync(string value, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@HideChangeLog"] = value,
            ["@User"] = user
        };
        await SqlHelper.ExecuteNonQuery(
            "UPDATE `users` SET `HideChangeLog` = @HideChangeLog WHERE `User` = @User",
            parameters, useAsync: useAsync);
    }

    internal static async Task Primary_ChangeLog_Set_VersionNotesAsync(string rtfNotes, string version,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@Notes"] = rtfNotes,
            ["@Version"] = version
        };
        await SqlHelper.ExecuteNonQuery(
            "UPDATE `changelog` SET `Notes` = @Notes WHERE `Version` = @Version",
            parameters, useAsync: useAsync);
    }

    internal static async Task<string> Primary_ChangeLog_Get_Visual_UserAsync(bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = WipAppVariables.User
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT `VisualUserName` FROM `users` WHERE `User` = @User",
                parameters, useAsync: useAsync);
            return result?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }


    internal static async Task<string> Primary_ChangeLog_Get_Visual_PasswordAsync(bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = WipAppVariables.User
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT `VisualPassword` FROM `users` WHERE `User` = @User",
                parameters, useAsync: useAsync);
            return result?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }


    internal static async Task Primary_ChangeLog_Set_Visual_PasswordAsync(string value, string user,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@VisualPassword"] = value,
            ["@User"] = user
        };
        await SqlHelper.ExecuteNonQuery(
            "UPDATE `users` SET `VisualPassword` = @VisualPassword WHERE `User` = @User",
            parameters, useAsync: useAsync);
    }

    internal static async Task Primary_ChangeLog_Set_Visual_UserAsync(string value, string user,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@VisualUserName"] = value,
            ["@User"] = user
        };
        await SqlHelper.ExecuteNonQuery(
            "UPDATE `users` SET `VisualUserName` = @VisualUserName WHERE `User` = @User",
            parameters, useAsync: useAsync);
    }

    internal static async Task<string> Primary_ChangeLog_Get_ToggleAsync(bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = WipAppVariables.User
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT `HideChangeLog` FROM `users` WHERE `User` = @User",
                parameters, useAsync: useAsync);
            return result?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }

    internal static async Task<string> Primary_ChangeLog_Get_LastShownAsync(bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = WipAppVariables.User
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT `LastShownVersion` FROM `users` WHERE `User` = @User",
                parameters, useAsync: useAsync);
            return result?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }

    internal static async Task<string> Primary_ChangeLog_Get_VersionNotesAsync(string? version = null,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Version"] = version ?? string.Empty
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT `Notes` FROM `changelog` WHERE `Version` = @Version",
                parameters, useAsync: useAsync);
            return result?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }

    internal static async Task<DataTable> GetAllChangeLogEntriesAsync(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable("SELECT * FROM `changelog` ORDER BY `Version` DESC",
                useAsync: useAsync);
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
                ["@Version"] = version
            };
            var table = await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `changelog` WHERE `Version` = @Version",
                parameters, useAsync: useAsync);
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
                ["@Version"] = version,
                ["@Notes"] = notes
            };
            await SqlHelper.ExecuteNonQuery(
                "INSERT INTO `changelog` (`Version`, `Notes`) VALUES (@Version, @Notes)",
                parameters, useAsync: useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task DeleteChangeLogEntryAsync(string version, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Version"] = version
            };
            await SqlHelper.ExecuteNonQuery(
                "DELETE FROM `changelog` WHERE `Version` = @Version",
                parameters, useAsync: useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }
}