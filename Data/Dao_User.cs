using System.Data;
using System.Diagnostics;
using System.Text.Json;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data
{
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
            await SetUserSettingAsync("LastShownVersion", user, value, useAsync);
        }

        internal static async Task<string> GetHideChangeLogAsync(string user, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetHideChangeLogAsync(user={user}, useAsync={useAsync})");
            return await GetSettingsJsonAsync("HideChangeLog", user, useAsync);
        }

        internal static async Task SetHideChangeLogAsync(string user, string value, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetHideChangeLogAsync(user={user}, value={value}, useAsync={useAsync})");
            await SetUserSettingAsync("HideChangeLog", user, value, useAsync);
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
                string str = await GetSettingsJsonAsync("Theme_FontSize", user, useAsync);
                return int.TryParse(str, out int val) ? val : null;
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
            Debug.WriteLine(
                $"[Dao_User] Entering SetThemeFontSizeAsync(user={user}, value={value}, useAsync={useAsync})");
            await SetUserSettingAsync("Theme_FontSize", user, value.ToString(), useAsync);
        }

        internal static async Task<string> GetVisualUserNameAsync(string user, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetVisualUserNameAsync(user={user}, useAsync={useAsync})");
            string value = await GetSettingsJsonAsync("VisualUserName", user, useAsync);
            Model_Users.VisualUserName = value;
            return Model_Users.VisualUserName;
        }

        internal static async Task SetVisualUserNameAsync(string user, string value, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetVisualUserNameAsync(user={user}, value={value}, useAsync={useAsync})");
            await SetUserSettingAsync("VisualUserName", user, value, useAsync);
        }

        internal static async Task<string> GetVisualPasswordAsync(string user, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetVisualPasswordAsync(user={user}, useAsync={useAsync})");
            string value = await GetSettingsJsonAsync("VisualPassword", user, useAsync);
            Model_Users.VisualPassword = value;
            return Model_Users.VisualPassword;
        }

        internal static async Task SetVisualPasswordAsync(string user, string value, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetVisualPasswordAsync(user={user}, value={value}, useAsync={useAsync})");
            await SetUserSettingAsync("VisualPassword", user, value, useAsync);
        }

        internal static async Task<string> GetWipServerAddressAsync(string user, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetWipServerAddressAsync(user={user}, useAsync={useAsync})");
            string value = await GetSettingsJsonAsync("WipServerAddress", user, useAsync);
            Model_Users.WipServerAddress = value;
            return Model_Users.WipServerAddress;
        }

        internal static async Task SetWipServerAddressAsync(string user, string value, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetWipServerAddressAsync(user={user}, value={value}, useAsync={useAsync})");
            await SetUserSettingAsync("WipServerAddress", user, value, useAsync);
        }

        #region Get/Set Database

        internal static async Task<string> GetDatabaseAsync(string user, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetDatabaseAsync(user={user}, useAsync={useAsync})");
            string value = await GetSettingsJsonAsync("WIPDatabase", user, useAsync);
            Model_Users.Database = value;
            return Model_Users.Database;
        }

        internal static async Task SetDatabaseAsync(string user, string value, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetDatabaseAsync(user={user}, value={value}, useAsync={useAsync})");
            Model_Users.Database = value;
            await SetUserSettingAsync("WIPDatabase", user, value, useAsync);
        }

        #endregion

        #region Get/Set WipServerPort

        internal static async Task<string> GetWipServerPortAsync(string user, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetWipServerPortAsync(user={user}, useAsync={useAsync})");
            string value = await GetSettingsJsonAsync("WipServerPort", user, useAsync);
            Model_Users.WipServerPort = value;
            return Model_Users.WipServerPort;
        }

        internal static async Task SetWipServerPortAsync(string user, string value, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetWipServerPortAsync(user={user}, value={value}, useAsync={useAsync})");
            Model_Users.WipServerPort = value;
            await SetUserSettingAsync("WipServerPort", user, value, useAsync);
        }

        #endregion

        internal static async Task<string?> GetUserFullNameAsync(string user, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetUserFullNameAsync(user={user}, useAsync={useAsync})");
            try
            {
                Dictionary<string, object> parameters = new() { ["@User"] = user };
                object? result = await HelperDatabaseCore.ExecuteScalar(
                    "SELECT `Full Name` FROM `usr_users` WHERE `User` = @User",
                    parameters, useAsync, CommandType.Text);
                Debug.WriteLine($"[Dao_User] GetUserFullNameAsync result: {result}");
                Model_Users.FullName = result?.ToString() ?? string.Empty;
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
            Debug.WriteLine(
                $"[Dao_User] Entering GetSettingsJsonAsync(field={field}, user={user}, useAsync={useAsync})");
            try
            {
                using MySqlConnection conn = new(Model_AppVariables.ConnectionString);
                await conn.OpenAsync();

                using MySqlCommand cmd = new(
                    "SELECT SettingsJson FROM usr_ui_settings WHERE UserId = @UserId LIMIT 1", conn);

                cmd.Parameters.AddWithValue("UserId", user);

                object? result = await cmd.ExecuteScalarAsync();

                if (result != null && result != DBNull.Value)
                {
                    string? json = result.ToString();
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        try
                        {
                            using JsonDocument doc = JsonDocument.Parse(json);
                            if (doc.RootElement.TryGetProperty(field, out JsonElement fieldElement))
                            {
                                string? value;

                                switch (fieldElement.ValueKind)
                                {
                                    case JsonValueKind.Number:
                                        value = fieldElement.ToString();
                                        break;

                                    case JsonValueKind.String:
                                        value = fieldElement.GetString();
                                        break;

                                    case JsonValueKind.True:
                                        value = "true";
                                        break;

                                    case JsonValueKind.False:
                                        value = "false";
                                        break;

                                    default:
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
                        }
                    }
                }

                using MySqlCommand legacyCmd = new(
                    $"SELECT `{field}` FROM `usr_users` WHERE `User` = @User LIMIT 1", conn);

                legacyCmd.Parameters.AddWithValue("User", user);

                object? legacyResult = await legacyCmd.ExecuteScalarAsync();
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
                using MySqlConnection conn = new(Model_AppVariables.ConnectionString);
                await conn.OpenAsync();

                using MySqlCommand cmd = new("usr_ui_settings_SetThemeJson", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("p_UserId", userId);
                cmd.Parameters.AddWithValue("p_ThemeJson", themeJson);

                MySqlParameter statusParam =
                    new("p_Status", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(statusParam);

                MySqlParameter errorMsgParam = new("p_ErrorMsg", MySqlDbType.VarChar, 255)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorMsgParam);

                await cmd.ExecuteNonQueryAsync();

                int status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
                string errorMsg = errorMsgParam.Value?.ToString() ?? "";

                Debug.WriteLine($"[Dao_User] SetSettingsJsonAsync status: {status}, errorMsg: {errorMsg}");

                if (status != 0)
                {
                    throw new Exception(errorMsg);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetSettingsJsonAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
            }
        }

        // Add method to support saving named settings JSON for grid view settings
        public static async Task SetGridViewSettingsJsonAsync(string userId, string dgvName, string settingsJson)
        {
            Debug.WriteLine($"[Dao_User] Entering SetGridViewSettingsJsonAsync(userId={userId})");
            try
            {
                using MySqlConnection conn = new(Model_AppVariables.ConnectionString);
                await conn.OpenAsync();
                using (MySqlCommand cmd = new("usr_ui_settings_SetJsonSetting", conn)
                       {
                           CommandType = CommandType.StoredProcedure
                       })
                {
                    cmd.Parameters.AddWithValue("p_UserId", userId);
                    cmd.Parameters.AddWithValue("p_DgvName", dgvName); // <-- ADD THIS LINE
                    cmd.Parameters.AddWithValue("p_SettingJson", settingsJson);
                    MySqlParameter statusParam =
                        new("p_Status", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(statusParam);
                    MySqlParameter errorMsgParam = new("p_ErrorMsg", MySqlDbType.VarChar, 255)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(errorMsgParam);
                    await cmd.ExecuteNonQueryAsync();
                    int status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
                    string errorMsg = errorMsgParam.Value?.ToString() ?? "";
                    Debug.WriteLine($"[Dao_User] SetGridViewSettingsJsonAsync status: {status}, errorMsg: {errorMsg}");
                    if (status != 0)
                    {
                        throw new Exception(errorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetGridViewSettingsJsonAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
            }
        }

        // Add method to load named settings JSON for grid view settings
        public static async Task<string> GetGridViewSettingsJsonAsync(string userId)
        {
            Debug.WriteLine($"[Dao_User] Entering GetGridViewSettingsJsonAsync(userId={userId})");
            try
            {
                using MySqlConnection conn = new(Model_AppVariables.ConnectionString);
                await conn.OpenAsync();
                using MySqlCommand cmd = new("usr_ui_settings_GetJsonSetting", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("p_UserId", userId);
                MySqlParameter jsonParam = new("p_SettingJson", MySqlDbType.JSON)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(jsonParam);
                await cmd.ExecuteNonQueryAsync();
                string? json = jsonParam.Value?.ToString();
                Debug.WriteLine($"[Dao_User] GetGridViewSettingsJsonAsync result: {json}");
                return json ?? "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in GetGridViewSettingsJsonAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
                return "";
            }
        }

        private static async Task SetUserSettingAsync(string field, string user, string value, bool useAsync)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetUserSettingAsync(field={field}, user={user}, value={value}, useAsync={useAsync})");
            try
            {
                Dictionary<string, object> parameters = new() { ["@User"] = user, [$"@{field}"] = value };
                await HelperDatabaseCore.ExecuteNonQuery(
                    $@"
INSERT INTO `usr_users` (`User`, `{field}`)
VALUES (@User, @{field})
ON DUPLICATE KEY UPDATE `{field}` = VALUES(`{field}`);
",
                    parameters, useAsync, CommandType.Text);
                Debug.WriteLine("[Dao_User] SetUserSettingAsync completed successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetUserSettingAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            }
        }

        #endregion

        #region Add / Update / Delete

        internal static async Task DeleteUserSettingsAsync(string userName, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering DeleteUserSettingsAsync(userName={userName}, useAsync={useAsync})");
            try
            {
                Dictionary<string, object> parameters = new() { ["p_UserId"] = userName };
                await HelperDatabaseCore.ExecuteNonQuery(
                    "usr_ui_settings_Delete_ByUserId",
                    parameters, useAsync, CommandType.StoredProcedure);
                Debug.WriteLine("[Dao_User] DeleteUserSettingsAsync completed successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in DeleteUserSettingsAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            }
        }

        internal static async Task InsertUserAsync(
            string user, string fullName, string shift, bool vitsUser, string pin,
            string lastShownVersion, string hideChangeLog, string themeName, int themeFontSize,
            string visualUserName, string visualPassword, string wipServerAddress, string database,
            string wipServerPort,
            bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering InsertUserAsync(user={user}, fullName={fullName}, shift={shift}, vitsUser={vitsUser}, pin={pin}, lastShownVersion={lastShownVersion}, hideChangeLog={hideChangeLog}, themeName={themeName}, themeFontSize={themeFontSize}, visualUserName={visualUserName}, visualPassword={visualPassword}, wipServerAddress={wipServerAddress}, database = {database},wipServerPort={wipServerPort}, useAsync={useAsync})");
            try
            {
                Dictionary<string, object> parameters = new()
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
                    ["p_WIPDatabase"] = database,
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
            string user,
            string fullName,
            string shift,
            string pin,
            string visualUserName,
            string visualPassword,
            bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering UpdateUserAsync(user={user}, fullName={fullName}, shift={shift}, pin={pin}, visualUserName={visualUserName}, visualPassword={visualPassword}, useAsync={useAsync})");
            try
            {
                Dictionary<string, object> parameters = new()
                {
                    ["p_User"] = user,
                    ["p_FullName"] = fullName,
                    ["p_Shift"] = shift,
                    ["p_Pin"] = pin,
                    ["p_VisualUserName"] = visualUserName,
                    ["p_VisualPassword"] = visualPassword
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
                Dictionary<string, object> parameters = new() { ["p_User"] = user };
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
                Dictionary<string, object> parameters = new() { ["p_User"] = user };
                DataTable table = await HelperDatabaseCore.ExecuteDataTable(
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
                Dictionary<string, object> parameters = new() { ["p_User"] = user };
                DataTable result = await HelperDatabaseCore.ExecuteDataTable(
                    "usr_users_Exists",
                    parameters, useAsync, CommandType.StoredProcedure);
                bool exists = result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["UserExists"]) > 0;
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

        #region User UI Settings

        internal static async Task<string> GetShortcutsJsonAsync(string userId)
        {
            Debug.WriteLine($"[Dao_User] Entering GetShortcutsJsonAsync(userId={userId})");
            try
            {
                using MySqlConnection conn = new(Model_AppVariables.ConnectionString);
                await conn.OpenAsync();

                using MySqlCommand cmd = new("usr_ui_settings_GetShortcutsJson", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("p_UserId", userId);

                MySqlParameter jsonParam = new("p_ShortcutsJson", MySqlDbType.JSON)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(jsonParam);

                await cmd.ExecuteNonQueryAsync();

                string? json = jsonParam.Value?.ToString();
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
                using MySqlConnection conn = new(Model_AppVariables.ConnectionString);
                await conn.OpenAsync();

                using MySqlCommand cmd = new("usr_ui_settings_SetShortcutsJson", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("p_UserId", userId);
                cmd.Parameters.AddWithValue("p_ShortcutsJson", shortcutsJson);

                MySqlParameter statusParam =
                    new("p_Status", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(statusParam);

                MySqlParameter errorMsgParam = new("p_ErrorMsg", MySqlDbType.VarChar, 255)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorMsgParam);

                await cmd.ExecuteNonQueryAsync();

                int status = statusParam.Value is int s ? s : Convert.ToInt32(statusParam.Value ?? 0);
                string errorMsg = errorMsgParam.Value?.ToString() ?? "";

                Debug.WriteLine($"[Dao_User] SetShortcutsJsonAsync status: {status}, errorMsg: {errorMsg}");

                if (status != 0)
                {
                    throw new Exception(errorMsg);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetShortcutsJsonAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
            }
        }

        #endregion

        #region User Roles

        internal static async Task AddUserRoleAsync(int userId, int roleId, string assignedBy, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering AddUserRoleAsync(userId={userId}, roleId={roleId}, assignedBy={assignedBy}, useAsync={useAsync})");
            try
            {
                Dictionary<string, object> parameters = new()
                {
                    ["p_UserID"] = userId, ["p_RoleID"] = roleId, ["p_AssignedBy"] = assignedBy
                };
                await HelperDatabaseCore.ExecuteNonQuery(
                    "sys_user_roles_Add",
                    parameters, useAsync, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in AddUserRoleAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            }
        }

        internal static async Task<int> GetUserRoleIdAsync(int userId, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetUserRoleIdAsync(userId={userId}, useAsync={useAsync})");
            try
            {
                Dictionary<string, object> parameters = new() { ["@UserID"] = userId };
                DataTable result = await HelperDatabaseCore.ExecuteDataTable(
                    "SELECT RoleID FROM sys_user_roles WHERE UserID = @UserID LIMIT 1",
                    parameters, useAsync, CommandType.Text);

                if (result.Rows.Count > 0 && int.TryParse(result.Rows[0]["RoleID"]?.ToString(), out int roleId))
                {
                    DataTable roleInfo = await HelperDatabaseCore.ExecuteDataTable(
                        "sys_roles_Get_ById",
                        new Dictionary<string, object> { ["p_ID"] = roleId },
                        useAsync, CommandType.StoredProcedure);
                    return roleId;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in GetUserRoleIdAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
                return 0;
            }
        }

        internal static async Task SetUserRoleAsync(int userId, int newRoleId, string assignedBy, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetUserRoleAsync(userId={userId}, newRoleId={newRoleId}, assignedBy={assignedBy}, useAsync={useAsync})");
            try
            {
                Dictionary<string, object> parameters = new()
                {
                    ["p_UserID"] = userId, ["p_NewRoleID"] = newRoleId, ["p_AssignedBy"] = assignedBy
                };
                await HelperDatabaseCore.ExecuteNonQuery(
                    "sys_user_roles_Update",
                    parameters, useAsync, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetUserRoleAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            }
        }

        internal static async Task SetUsersRoleAsync(IEnumerable<int> userIds, int newRoleId, string assignedBy,
            bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetUsersRoleAsync(userIds=[{string.Join(",", userIds)}], newRoleId={newRoleId}, assignedBy={assignedBy}, useAsync={useAsync})");
            try
            {
                foreach (int userId in userIds)
                {
                    Dictionary<string, object> parameters = new()
                    {
                        ["p_UserID"] = userId, ["p_NewRoleID"] = newRoleId, ["p_AssignedBy"] = assignedBy
                    };
                    await HelperDatabaseCore.ExecuteNonQuery(
                        "sys_user_roles_Update",
                        parameters, useAsync, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetUsersRoleAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            }
        }

        internal static async Task RemoveUserRoleAsync(int userId, int roleId, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering RemoveUserRoleAsync(userId={userId}, roleId={roleId}, useAsync={useAsync})");
            try
            {
                Dictionary<string, object> parameters = new() { ["p_UserID"] = userId, ["p_RoleID"] = roleId };
                await HelperDatabaseCore.ExecuteNonQuery(
                    "sys_user_roles_Delete",
                    parameters, useAsync, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in RemoveUserRoleAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            }
        }

        #endregion
    }

    #endregion
}
