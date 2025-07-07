using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using System.Text.Json;

namespace MTM_Inventory_Application.Data;

#region Dao_User

internal static class Dao_User
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

    #region User Settings Getters/Setters

    internal static async Task<string> GetLastShownVersionAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetLastShownVersionAsync(user={user}, useAsync={useAsync})");
        return await GetSettingsJsonAsync("LastShownVersion", user, useAsync);
    }

    internal static async Task SetLastShownVersionAsync(string user, string value, bool useAsync = false)
    {
        Debug.WriteLine(
            $"[Dao_User] Entering SetLastShownVersionAsync(user={user}, value={value}, useAsync={useAsync})");
        await SetUsr_userFieldAsync("LastShownVersion", user, value, useAsync);
    }

    internal static async Task<string> GetHideChangeLogAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetHideChangeLogAsync(user={user}, useAsync={useAsync})");
        return await GetSettingsJsonAsync("HideChangeLog", user, useAsync);
    }

    internal static async Task SetHideChangeLogAsync(string user, string value, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering SetHideChangeLogAsync(user={user}, value={value}, useAsync={useAsync})");
        await SetUsr_userFieldAsync("HideChangeLog", user, value, useAsync);
    }

    internal static async Task<string?> GetThemeNameAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetThemeNameAsync(user={user}, useAsync={useAsync})");
        return await GetSettingsJsonAsync("Theme_Name", user, true);
    }

    internal static async Task<int?> GetThemeFontSizeAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetThemeFontSizeAsync(user={user}, useAsync={useAsync})");
        try
        {
            var str = await GetSettingsJsonAsync("Theme_FontSize", user, useAsync);
            return int.TryParse(str, out var val) ? val : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in GetThemeFontSizeAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    internal static async Task SetThemeFontSizeAsync(string user, int value, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering SetThemeFontSizeAsync(user={user}, value={value}, useAsync={useAsync})");
        await SetUsr_userFieldAsync("Theme_FontSize", user, value.ToString(), useAsync);
    }

    internal static async Task<string> GetVisualUserNameAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetVisualUserNameAsync(user={user}, useAsync={useAsync})");
        return await GetSettingsJsonAsync("VisualUserName", user, useAsync);
    }

    internal static async Task SetVisualUserNameAsync(string user, string value, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering SetVisualUserNameAsync(user={user}, value={value}, useAsync={useAsync})");
        await SetUsr_userFieldAsync("VisualUserName", user, value, useAsync);
    }

    internal static async Task<string> GetVisualPasswordAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetVisualPasswordAsync(user={user}, useAsync={useAsync})");
        return await GetSettingsJsonAsync("VisualPassword", user, useAsync);
    }

    internal static async Task SetVisualPasswordAsync(string user, string value, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering SetVisualPasswordAsync(user={user}, value={value}, useAsync={useAsync})");
        await SetUsr_userFieldAsync("VisualPassword", user, value, useAsync);
    }

    internal static async Task<string> GetWipServerAddressAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetWipServerAddressAsync(user={user}, useAsync={useAsync})");
        return await GetSettingsJsonAsync("WipServerAddress", user, useAsync);
    }

    internal static async Task SetWipServerAddressAsync(string user, string value, bool useAsync = false)
    {
        Debug.WriteLine(
            $"[Dao_User] Entering SetWipServerAddressAsync(user={user}, value={value}, useAsync={useAsync})");
        await SetUsr_userFieldAsync("WipServerAddress", user, value, useAsync);
    }

    internal static async Task<string> GetWipServerPortAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetWipServerPortAsync(user={user}, useAsync={useAsync})");
        return await GetSettingsJsonAsync("WipServerPort", user, useAsync);
    }

    internal static async Task<string> GetDatabaseAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetDatabaseAsync(user={user}, useAsync={useAsync})");
        return await GetSettingsJsonAsync("WIPDatabase", user, useAsync);
    }

    internal static async Task SetWipServerPortAsync(string user, string value, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering SetWipServerPortAsync(user={user}, value={value}, useAsync={useAsync})");
        await SetUsr_userFieldAsync("WipServerPort", user, value, useAsync);
    }

    internal static async Task<string?> GetUserFullNameAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetUserFullNameAsync(user={user}, useAsync={useAsync})");
        try
        {
            var parameters = new Dictionary<string, object> { ["@User"] = user };
            var result = await HelperDatabaseCore.ExecuteScalar(
                "SELECT `Full Name` FROM `usr_users` WHERE `User` = @User",
                parameters, useAsync, CommandType.Text);
            Debug.WriteLine($"[Dao_User] GetUserFullNameAsync result: {result}");
            return result?.ToString();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in GetUserFullNameAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    public static async Task<string> GetSettingsJsonAsync(string field, string user, bool useAsync)
    {
        Debug.WriteLine($"[Dao_User] Entering GetSettingsJsonAsync(field={field}, user={user}, useAsync={useAsync})");
        try
        {
            // First try to get the field from the usr_ui_settings table as JSON property
            using var conn = new MySqlConnection(Model_AppVariables.ConnectionString);
            await conn.OpenAsync();

            // Query the settings JSON directly
            using var cmd = new MySqlCommand(
                "SELECT SettingsJson FROM usr_ui_settings WHERE UserId = @UserId LIMIT 1", conn);

            cmd.Parameters.AddWithValue("UserId", user);

            var result = await cmd.ExecuteScalarAsync();

            // If we found a settings JSON, try to extract the requested field
            if (result != null && result != DBNull.Value)
            {
                var json = result.ToString();
                if (!string.IsNullOrWhiteSpace(json))
                    try
                    {
                        using var doc = JsonDocument.Parse(json);
                        if (doc.RootElement.TryGetProperty(field, out var fieldElement))
                        {
                            string? value;

                            // Handle different JSON value types
                            switch (fieldElement.ValueKind)
                            {
                                case JsonValueKind.Number:
                                    // For numbers, directly convert to string
                                    value = fieldElement.ToString();
                                    break;

                                case JsonValueKind.String:
                                    // For strings, use GetString()
                                    value = fieldElement.GetString();
                                    break;

                                case JsonValueKind.True:
                                    value = "true";
                                    break;

                                case JsonValueKind.False:
                                    value = "false";
                                    break;

                                default:
                                    // For other types, use ToString() as a fallback
                                    value = fieldElement.ToString();
                                    break;
                            }

                            Debug.WriteLine($"[Dao_User] GetSettingsJsonAsync found value in JSON: {value}");
                            return value ?? string.Empty;
                        }
                    }
                    catch (JsonException ex)
                    {
                        Debug.WriteLine($"[Dao_User] JSON parsing error in GetSettingsJsonAsync: {ex.Message}");
                        // Continue to legacy approach if JSON parsing fails
                    }
            }

            // Fallback to legacy table approach if JSON doesn't contain the field

            using var legacyCmd = new MySqlCommand(
                $"SELECT `{field}` FROM `usr_users` WHERE `User` = @User LIMIT 1", conn);

            legacyCmd.Parameters.AddWithValue("User", user);

            var legacyResult = await legacyCmd.ExecuteScalarAsync();
            Debug.WriteLine($"[Dao_User] GetSettingsJsonAsync legacy result: {legacyResult}");

            return legacyResult?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in GetSettingsJsonAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return string.Empty;
        }
    }

    public static async Task SetSettingsJsonAsync(string userId, string themeJson)
    {
        Debug.WriteLine($"[Dao_User] Entering SetSettingsJsonAsync(userId={userId})");
        try
        {
            using var conn = new MySqlConnection(Model_AppVariables.ConnectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("usr_ui_settings_SetThemeJson", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("p_UserId", userId);
            cmd.Parameters.AddWithValue("p_ThemeJson", themeJson);

            var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(statusParam);

            var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(errorMsgParam);

            await cmd.ExecuteNonQueryAsync();

            var status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
            var errorMsg = errorMsgParam.Value?.ToString() ?? "";

            Debug.WriteLine($"[Dao_User] SetSettingsJsonAsync status: {status}, errorMsg: {errorMsg}");

            if (status != 0)
                throw new Exception(errorMsg);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in SetSettingsJsonAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
        }
    }

    private static async Task SetUsr_userFieldAsync(string field, string user, string value, bool useAsync)
    {
        Debug.WriteLine(
            $"[Dao_User] Entering SetUsr_userFieldAsync(field={field}, user={user}, value={value}, useAsync={useAsync})");
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = user,
                [$"@{field}"] = value
            };
            await HelperDatabaseCore.ExecuteNonQuery(
                $@"
INSERT INTO `usr_users` (`User`, `{field}`)
VALUES (@User, @{field})
ON DUPLICATE KEY UPDATE `{field}` = VALUES(`{field}`);
",
                parameters, useAsync, CommandType.Text);
            Debug.WriteLine("[Dao_User] SetUsr_userFieldAsync completed successfully.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in SetUsr_userFieldAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Add / Update / Delete

    internal static async Task InsertUserAsync(
        string user, string fullName, string shift, bool vitsUser, string pin,
        string lastShownVersion, string hideChangeLog, string themeName, int themeFontSize,
        string visualUserName, string visualPassword, string wipServerAddress, string wipServerPort,
        bool useAsync = false)
    {
        Debug.WriteLine(
            $"[Dao_User] Entering InsertUserAsync(user={user}, fullName={fullName}, shift={shift}, vitsUser={vitsUser}, pin={pin}, lastShownVersion={lastShownVersion}, hideChangeLog={hideChangeLog}, themeName={themeName}, themeFontSize={themeFontSize}, visualUserName={visualUserName}, visualPassword={visualPassword}, wipServerAddress={wipServerAddress}, wipServerPort={wipServerPort}, useAsync={useAsync})");
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
            await HelperDatabaseCore.ExecuteNonQuery(
                "usr_users_Add_User",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in InsertUserAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task UpdateUserAsync(
        string user, string fullName, string shift, bool vitsUser, string pin,
        string lastShownVersion, string hideChangeLog, string themeName, int themeFontSize,
        string visualUserName, string visualPassword, string wipServerAddress, string wipServerPort,
        bool useAsync = false)
    {
        Debug.WriteLine(
            $"[Dao_User] Entering UpdateUserAsync(user={user}, fullName={fullName}, shift={shift}, vitsUser={vitsUser}, pin={pin}, lastShownVersion={lastShownVersion}, hideChangeLog={hideChangeLog}, themeName={themeName}, themeFontSize={themeFontSize}, visualUserName={visualUserName}, visualPassword={visualPassword}, wipServerAddress={wipServerAddress}, wipServerPort={wipServerPort}, useAsync={useAsync})");
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
            await HelperDatabaseCore.ExecuteNonQuery(
                "usr_users_Update_User",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in UpdateUserAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task DeleteUserAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering DeleteUserAsync(user={user}, useAsync={useAsync})");
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_User"] = user
            };
            await HelperDatabaseCore.ExecuteNonQuery(
                "usr_users_Delete_User",
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in DeleteUserAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Queries

    internal static async Task<DataTable> GetAllUsersAsync(bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetAllUsersAsync(useAsync={useAsync})");
        try
        {
            return await HelperDatabaseCore.ExecuteDataTable(
                "usr_users_Get_All",
                null, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in GetAllUsersAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task<DataRow?> GetUserByUsernameAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GetUserByUsernameAsync(user={user}, useAsync={useAsync})");
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_User"] = user
            };
            var table = await HelperDatabaseCore.ExecuteDataTable(
                "usr_users_Get_ByUser",
                parameters, useAsync, CommandType.StoredProcedure);
            Debug.WriteLine($"[Dao_User] GetUserByUsernameAsync result: {table.Rows.Count} rows");
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in GetUserByUsernameAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return null;
        }
    }

    internal static async Task<bool> UserExistsAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering UserExistsAsync(user={user}, useAsync={useAsync})");
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_User"] = user
            };
            var result = await HelperDatabaseCore.ExecuteDataTable(
                "usr_users_Exists",
                parameters, useAsync, CommandType.StoredProcedure);
            var exists = result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["UserExists"]) > 0;
            Debug.WriteLine($"[Dao_User] UserExistsAsync result: {exists}");
            return exists;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in UserExistsAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return false;
        }
    }

    #endregion

    #region Privileges

    internal static async Task GrantFullPrivilegesAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GrantFullPrivilegesAsync(user={user}, useAsync={useAsync})");
        await GrantPrivilegeAsync("usr_users_Grant_Full", user, useAsync);
    }

    internal static async Task GrantReadOnlyPrivilegesAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GrantReadOnlyPrivilegesAsync(user={user}, useAsync={useAsync})");
        await GrantPrivilegeAsync("usr_users_Grant_ReadOnly", user, useAsync);
    }

    internal static async Task GrantReadWritePrivilegesAsync(string user, bool useAsync = false)
    {
        Debug.WriteLine($"[Dao_User] Entering GrantReadWritePrivilegesAsync(user={user}, useAsync={useAsync})");
        await GrantPrivilegeAsync("usr_users_Grant_ReadWrite", user, useAsync);
    }

    private static async Task GrantPrivilegeAsync(string procName, string user, bool useAsync)
    {
        Debug.WriteLine(
            $"[Dao_User] Entering GrantPrivilegeAsync(procName={procName}, user={user}, useAsync={useAsync})");
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_User"] = user
            };
            await HelperDatabaseCore.ExecuteNonQuery(
                procName,
                parameters, useAsync, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in GrantPrivilegeAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region User UI Settings

    public static async Task SaveUserUiSettingsAsync(string userId, Model_UserUiColors colors)
    {
        Debug.WriteLine(
            $"[Dao_User] Entering SaveUserUiSettingsAsync(userId={userId}, colors={JsonSerializer.Serialize(colors)})");
        try
        {
            using var conn = new MySqlConnection(Model_AppVariables.ConnectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("usr_ui_settings_Save", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("p_UserId", userId);
            cmd.Parameters.AddWithValue("p_SettingsJson", JsonSerializer.Serialize(colors));

            var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(statusParam);

            var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(errorMsgParam);

            await cmd.ExecuteNonQueryAsync();

            var status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
            var errorMsg = errorMsgParam.Value?.ToString() ?? "";

            Debug.WriteLine($"[Dao_User] SaveUserUiSettingsAsync status: {status}, errorMsg: {errorMsg}");

            if (status != 0)
                throw new Exception(errorMsg);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in SaveUserUiSettingsAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
        }
    }

    public static async Task UpdateUserUiSettingsAsync(string userId, Model_UserUiColors colors)
    {
        Debug.WriteLine(
            $"[Dao_User] Entering UpdateUserUiSettingsAsync(userId={userId}, colors={JsonSerializer.Serialize(colors)})");
        try
        {
            using var conn = new MySqlConnection(Model_AppVariables.ConnectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("usr_ui_settings_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("p_UserId", userId);
            cmd.Parameters.AddWithValue("p_SettingsJson", JsonSerializer.Serialize(colors));

            var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(statusParam);

            var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(errorMsgParam);

            await cmd.ExecuteNonQueryAsync();

            var status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
            var errorMsg = errorMsgParam.Value?.ToString() ?? "";

            Debug.WriteLine($"[Dao_User] UpdateUserUiSettingsAsync status: {status}, errorMsg: {errorMsg}");

            if (status != 0)
                throw new Exception(errorMsg);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in UpdateUserUiSettingsAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
        }
    }

    public static async Task DeleteUserUiSettingsAsync(string userId)
    {
        Debug.WriteLine($"[Dao_User] Entering DeleteUserUiSettingsAsync(userId={userId})");
        try
        {
            using var conn = new MySqlConnection(Model_AppVariables.ConnectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("usr_ui_settings_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("p_UserId", userId);

            var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(statusParam);

            var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(errorMsgParam);

            await cmd.ExecuteNonQueryAsync();

            var status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
            var errorMsg = errorMsgParam.Value?.ToString() ?? "";

            Debug.WriteLine($"[Dao_User] DeleteUserUiSettingsAsync status: {status}, errorMsg: {errorMsg}");

            if (status != 0)
                throw new Exception(errorMsg);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in DeleteUserUiSettingsAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
        }
    }

    internal static async Task<string> GetShortcutsJsonAsync(string userId)
    {
        Debug.WriteLine($"[Dao_User] Entering GetShortcutsJsonAsync(userId={userId})");
        try
        {
            using var conn = new MySqlConnection(Model_AppVariables.ConnectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("usr_ui_settings_GetShortcutsJson", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("p_UserId", userId);

            var jsonParam = new MySqlParameter("p_ShortcutsJson", MySqlDbType.JSON)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(jsonParam);

            await cmd.ExecuteNonQueryAsync();

            var json = jsonParam.Value?.ToString();
            Debug.WriteLine($"[Dao_User] GetShortcutsJsonAsync result: {json}");
            return json ?? "";
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in GetShortcutsJsonAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
            return "";
        }
    }

    internal static async Task SetShortcutsJsonAsync(string userId, string shortcutsJson)
    {
        Debug.WriteLine($"[Dao_User] Entering SetShortcutsJsonAsync(userId={userId})");
        try
        {
            using var conn = new MySqlConnection(Model_AppVariables.ConnectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("usr_ui_settings_SetShortcutsJson", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("p_UserId", userId);
            cmd.Parameters.AddWithValue("p_ShortcutsJson", shortcutsJson);

            var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(statusParam);

            var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(errorMsgParam);

            await cmd.ExecuteNonQueryAsync();

            var status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
            var errorMsg = errorMsgParam.Value?.ToString() ?? "";

            Debug.WriteLine($"[Dao_User] SetShortcutsJsonAsync status: {status}, errorMsg: {errorMsg}");

            if (status != 0)
                throw new Exception(errorMsg);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Dao_User] Exception in SetShortcutsJsonAsync: {ex}");
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
        }
    }


    /// <summary>
    /// Sets the Theme_Name value in the SettingsJson column in usr_ui_settings for the given user.
    /// </summary>

    #endregion
}

#endregion