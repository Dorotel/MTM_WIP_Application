using System.Data;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.Data;

#region UserDao

internal static class UserDao
{
    public static SqlHelper SqlHelper =
        new(SqlVariables.GetConnectionString(
            WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            WipAppVariables.User,
            WipAppVariables.UserPin
        ));

    #region User Settings Getters/Setters

    internal static async Task<string> GetLastShownVersionAsync(string user, bool useAsync = false)
    {
        return await GetUserSettingAsync("LastShownVersion", user, useAsync);
    }

    internal static async Task SetLastShownVersionAsync(string user, string value, bool useAsync = false)
    {
        await SetUserSettingAsync("LastShownVersion", user, value, useAsync);
    }

    internal static async Task<string> GetHideChangeLogAsync(string user, bool useAsync = false)
    {
        return await GetUserSettingAsync("HideChangeLog", user, useAsync);
    }

    internal static async Task SetHideChangeLogAsync(string user, string value, bool useAsync = false)
    {
        await SetUserSettingAsync("HideChangeLog", user, value, useAsync);
    }

    internal static async Task<string> GetThemeNameAsync(string user, bool useAsync = false)
    {
        return await GetUserSettingAsync("Theme_Name", user, useAsync);
    }

    internal static async Task SetThemeNameAsync(string user, string value, bool useAsync = false)
    {
        await SetUserSettingAsync("Theme_Name", user, value, useAsync);
    }

    internal static async Task<int?> GetThemeFontSizeAsync(string user, bool useAsync = false)
    {
        try
        {
            var str = await GetUserSettingAsync("Theme_FontSize", user, useAsync);
            return int.TryParse(str, out var val) ? val : null;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    internal static async Task SetThemeFontSizeAsync(string user, int value, bool useAsync = false)
    {
        await SetUserSettingAsync("Theme_FontSize", user, value.ToString(), useAsync);
    }

    internal static async Task<string> GetVisualUserNameAsync(string user, bool useAsync = false)
    {
        return await GetUserSettingAsync("VisualUserName", user, useAsync);
    }

    internal static async Task SetVisualUserNameAsync(string user, string value, bool useAsync = false)
    {
        await SetUserSettingAsync("VisualUserName", user, value, useAsync);
    }

    internal static async Task<string> GetVisualPasswordAsync(string user, bool useAsync = false)
    {
        return await GetUserSettingAsync("VisualPassword", user, useAsync);
    }

    internal static async Task SetVisualPasswordAsync(string user, string value, bool useAsync = false)
    {
        await SetUserSettingAsync("VisualPassword", user, value, useAsync);
    }

    internal static async Task<string> GetWipServerAddressAsync(string user, bool useAsync = false)
    {
        return await GetUserSettingAsync("WipServerAddress", user, useAsync);
    }

    internal static async Task SetWipServerAddressAsync(string user, string value, bool useAsync = false)
    {
        await SetUserSettingAsync("WipServerAddress", user, value, useAsync);
    }

    internal static async Task<string> GetWipServerPortAsync(string user, bool useAsync = false)
    {
        return await GetUserSettingAsync("WipServerPort", user, useAsync);
    }

    internal static async Task SetWipServerPortAsync(string user, string value, bool useAsync = false)
    {
        await SetUserSettingAsync("WipServerPort", user, value, useAsync);
    }

    internal static async Task<string?> GetUserFullNameAsync(string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@User"] = user };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT `Full Name` FROM `usr_users` WHERE `User` = @User",
                parameters, useAsync, CommandType.Text);
            return result?.ToString();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    private static async Task<string> GetUserSettingAsync(string field, string user, bool useAsync)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@User"] = user };
            var result = await SqlHelper.ExecuteScalar(
                $"SELECT `{field}` FROM `usr_users` WHERE `User` = @User",
                parameters, useAsync, CommandType.Text);
            return result?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }

    private static async Task SetUserSettingAsync(string field, string user, string value, bool useAsync)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@User"] = user,
            [$"@{field}"] = value
        };
        await SqlHelper.ExecuteNonQuery(
            $"UPDATE `usr_users` SET `{field}` = @{field} WHERE `User` = @User",
            parameters, useAsync, CommandType.Text);
    }

    #endregion

    #region Add / Update / Delete

    internal static async Task InsertUserAsync(
        string user, string fullName, string shift, bool vitsUser, string pin,
        string lastShownVersion, string hideChangeLog, string themeName, int themeFontSize,
        string visualUserName, string visualPassword, string wipServerAddress, string wipServerPort,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_User"] = user,
                ["p_FullName"] = fullName,
                ["p_Shift"] = shift,
                ["p_VitsUser"] = vitsUser,
                ["p_Pin"] = pin,
                ["p_LastShownVersion"] = lastShownVersion,
                ["p_HideChangeLog"] = hideChangeLog,
                ["p_Theme_Name"] = themeName,
                ["p_Theme_FontSize"] = themeFontSize,
                ["p_VisualUserName"] = visualUserName,
                ["p_VisualPassword"] = visualPassword,
                ["p_WipServerAddress"] = wipServerAddress,
                ["p_WipServerPort"] = wipServerPort
            };
            await SqlHelper.ExecuteNonQuery(
                "usr_users_Add_User",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task UpdateUserAsync(
        string user, string fullName, string shift, bool vitsUser, string pin,
        string lastShownVersion, string hideChangeLog, string themeName, int themeFontSize,
        string visualUserName, string visualPassword, string wipServerAddress, string wipServerPort,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_User"] = user,
                ["p_FullName"] = fullName,
                ["p_Shift"] = shift,
                ["p_VitsUser"] = vitsUser,
                ["p_Pin"] = pin,
                ["p_LastShownVersion"] = lastShownVersion,
                ["p_HideChangeLog"] = hideChangeLog,
                ["p_Theme_Name"] = themeName,
                ["p_Theme_FontSize"] = themeFontSize,
                ["p_VisualUserName"] = visualUserName,
                ["p_VisualPassword"] = visualPassword,
                ["p_WipServerAddress"] = wipServerAddress,
                ["p_WipServerPort"] = wipServerPort
            };
            await SqlHelper.ExecuteNonQuery(
                "usr_users_Update_User",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task DeleteUserAsync(string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_User"] = user
            };
            await SqlHelper.ExecuteNonQuery(
                "usr_users_Delete_User",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Queries

    internal static async Task<DataTable> GetAllUsersAsync(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable(
                "usr_users_Get_All",
                null, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task<DataRow?> GetUserByUsernameAsync(string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_User"] = user
            };
            var table = await SqlHelper.ExecuteDataTable(
                "usr_users_Get_ByUser",
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

    internal static async Task<bool> UserExistsAsync(string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_User"] = user
            };
            var result = await SqlHelper.ExecuteDataTable(
                "usr_users_Exists",
                parameters, useAsync, CommandType.StoredProcedure);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["UserExists"]) > 0;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return false;
        }
    }

    #endregion

    #region Privileges

    internal static async Task GrantFullPrivilegesAsync(string user, bool useAsync = false)
    {
        await GrantPrivilegeAsync("usr_users_Grant_Full", user, useAsync);
    }

    internal static async Task GrantReadOnlyPrivilegesAsync(string user, bool useAsync = false)
    {
        await GrantPrivilegeAsync("usr_users_Grant_ReadOnly", user, useAsync);
    }

    internal static async Task GrantReadWritePrivilegesAsync(string user, bool useAsync = false)
    {
        await GrantPrivilegeAsync("usr_users_Grant_ReadWrite", user, useAsync);
    }

    private static async Task GrantPrivilegeAsync(string procName, string user, bool useAsync)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_User"] = user
            };
            await SqlHelper.ExecuteNonQuery(
                procName,
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion
}

#endregion