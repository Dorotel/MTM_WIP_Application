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
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"GetThemeFontSizeAsync failed with exception: {ex.Message}");
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
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_GetFullName_ByUser",
                    new Dictionary<string, object> { ["User"] = user }, // Remove p_ prefix - added automatically
                    null, // No progress helper for this method
                    useAsync
                );

                if (dataResult.IsSuccess && dataResult.Data != null && dataResult.Data.Rows.Count > 0)
                {
                    object? result = dataResult.Data.Rows[0][0]; // Get first column of first row
                    Debug.WriteLine($"[Dao_User] GetUserFullNameAsync result: {result}");
                    Model_Users.FullName = result?.ToString() ?? string.Empty;
                    return result?.ToString();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in GetUserFullNameAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"GetUserFullNameAsync failed with exception: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GetSettingsJsonAsync(string field, string user, bool useAsync)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering GetSettingsJsonAsync(field={field}, user={user}, useAsync={useAsync})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_ui_settings_GetSettingsJson_ByUserId",
                    new Dictionary<string, object> { ["UserId"] = user }, // Remove p_ prefix - added automatically
                    null, // No progress helper for this method
                    useAsync
                );

                if (dataResult.IsSuccess && dataResult.Data != null && dataResult.Data.Rows.Count > 0)
                {
                    object? result = dataResult.Data.Rows[0][0]; // Get first column of first row
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
                }

                // FIXED: Use Helper_Database_StoredProcedure for legacy fallback too
                var legacyResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_GetUserSetting_ByUserAndField",
                    new Dictionary<string, object> 
                    { 
                        ["User"] = user,    // Remove p_ prefix - added automatically
                        ["Field"] = field   // Remove p_ prefix - added automatically
                    },
                    null, // No progress helper for this method
                    useAsync
                );

                if (legacyResult.IsSuccess && legacyResult.Data != null && legacyResult.Data.Rows.Count > 0)
                {
                    object? legacyValue = legacyResult.Data.Rows[0][0];
                    Debug.WriteLine($"[Dao_User] GetSettingsJsonAsync legacy result: {legacyValue}");
                    return legacyValue?.ToString() ?? string.Empty;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in GetSettingsJsonAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"GetSettingsJsonAsync failed with exception: {ex.Message}");
                return string.Empty;
            }
        }

        public static async Task SetSettingsJsonAsync(string userId, string themeJson)
        {
            Debug.WriteLine($"[Dao_User] Entering SetSettingsJsonAsync(userId={userId})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure for proper status handling
                Dictionary<string, object> parameters = new()
                {
                    ["UserId"] = userId,      // FIXED: Remove p_ prefix - added automatically
                    ["ThemeJson"] = themeJson // FIXED: Remove p_ prefix - added automatically
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_ui_settings_SetThemeJson",
                    parameters,
                    null, // No progress helper needed for this method
                    true  // Use async
                );

                Debug.WriteLine($"[Dao_User] SetSettingsJsonAsync status: {result.IsSuccess}, message: {result.ErrorMessage}");

                if (!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage ?? "Failed to set theme JSON");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetSettingsJsonAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"SetSettingsJsonAsync failed with exception: {ex.Message}");
            }
        }

        // Add method to support saving named settings JSON for grid view settings
        public static async Task SetGridViewSettingsJsonAsync(string userId, string dgvName, string settingsJson)
        {
            Debug.WriteLine($"[Dao_User] Entering SetGridViewSettingsJsonAsync(userId={userId})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure for proper status handling
                Dictionary<string, object> parameters = new()
                {
                    ["UserId"] = userId,        // FIXED: Remove p_ prefix - added automatically
                    ["DgvName"] = dgvName,      // FIXED: Remove p_ prefix - added automatically
                    ["SettingJson"] = settingsJson // FIXED: Remove p_ prefix - added automatically
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_ui_settings_SetJsonSetting",
                    parameters,
                    null, // No progress helper needed for this method
                    true  // Use async
                );

                Debug.WriteLine($"[Dao_User] SetGridViewSettingsJsonAsync status: {result.IsSuccess}, message: {result.ErrorMessage}");

                if (!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage ?? "Failed to set grid view settings JSON");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetGridViewSettingsJsonAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"SetGridViewSettingsJsonAsync failed with exception: {ex.Message}");
            }
        }

        // Add method to load named settings JSON for grid view settings
        public static async Task<string> GetGridViewSettingsJsonAsync(string userId)
        {
            Debug.WriteLine($"[Dao_User] Entering GetGridViewSettingsJsonAsync(userId={userId})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_ui_settings_GetJsonSetting",
                    new Dictionary<string, object> { ["UserId"] = userId }, // Remove p_ prefix - added automatically
                    null, // No progress helper for this method
                    true
                );
                
                if (dataResult.IsSuccess && dataResult.Data != null && dataResult.Data.Rows.Count > 0)
                {
                    object? result = dataResult.Data.Rows[0][0]; // Get first column of first row
                    string? json = result?.ToString();
                    Debug.WriteLine($"[Dao_User] GetGridViewSettingsJsonAsync result: {json}");
                    return json ?? "";
                }
                
                return "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in GetGridViewSettingsJsonAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"GetGridViewSettingsJsonAsync failed with exception: {ex.Message}");
                return "";
            }
        }

        private static async Task SetUserSettingAsync(string field, string user, string value, bool useAsync)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetUserSettingAsync(field={field}, user={user}, value={value}, useAsync={useAsync})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_SetUserSetting_ByUserAndField",
                    new Dictionary<string, object> 
                    { 
                        ["User"] = user,    // Remove p_ prefix - added automatically
                        ["Field"] = field,  // Remove p_ prefix - added automatically
                        ["Value"] = value   // Remove p_ prefix - added automatically
                    },
                    null, // No progress helper for this method
                    useAsync
                );

                if (result.IsSuccess)
                {
                    Debug.WriteLine("[Dao_User] SetUserSettingAsync completed successfully.");
                }
                else
                {
                    Debug.WriteLine($"[Dao_User] SetUserSettingAsync failed: {result.ErrorMessage}");
                    LoggingUtility.Log($"SetUserSettingAsync failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetUserSettingAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"SetUserSettingAsync failed with exception: {ex.Message}");
            }
        }

        #endregion

        #region Add / Update / Delete

        internal static async Task DeleteUserSettingsAsync(string userName, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering DeleteUserSettingsAsync(userName={userName}, useAsync={useAsync})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_ui_settings_Delete_ByUserId",
                    new Dictionary<string, object> { ["UserId"] = userName }, // Remove p_ prefix - added automatically
                    null, // No progress helper for this method
                    useAsync
                );

                if (result.IsSuccess)
                {
                    Debug.WriteLine("[Dao_User] DeleteUserSettingsAsync completed successfully.");
                }
                else
                {
                    Debug.WriteLine($"[Dao_User] DeleteUserSettingsAsync failed: {result.ErrorMessage}");
                    LoggingUtility.Log($"DeleteUserSettingsAsync failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in DeleteUserSettingsAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"DeleteUserSettingsAsync failed with exception: {ex.Message}");
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
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_Add_User",
                    new Dictionary<string, object>
                    {
                        ["User"] = user,                           // Remove p_ prefix - added automatically
                        ["FullName"] = fullName,
                        ["Shift"] = shift,
                        ["VitsUser"] = vitsUser,
                        ["Pin"] = pin,
                        ["LastShownVersion"] = lastShownVersion,
                        ["HideChangeLog"] = hideChangeLog,
                        ["Theme_Name"] = themeName,
                        ["Theme_FontSize"] = themeFontSize,
                        ["VisualUserName"] = visualUserName,
                        ["VisualPassword"] = visualPassword,
                        ["WipServerAddress"] = wipServerAddress,
                        ["WIPDatabase"] = database,
                        ["WipServerPort"] = wipServerPort
                    },
                    null, // No progress helper for this method
                    useAsync
                );

                if (!result.IsSuccess)
                {
                    Debug.WriteLine($"[Dao_User] InsertUserAsync failed: {result.ErrorMessage}");
                    LoggingUtility.Log($"InsertUserAsync failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in InsertUserAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"InsertUserAsync failed with exception: {ex.Message}");
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
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_Update_User",
                    new Dictionary<string, object>
                    {
                        ["User"] = user,                    // Remove p_ prefix - added automatically
                        ["FullName"] = fullName,
                        ["Shift"] = shift,
                        ["Pin"] = pin,
                        ["VisualUserName"] = visualUserName,
                        ["VisualPassword"] = visualPassword
                    },
                    null, // No progress helper for this method
                    useAsync
                );

                if (!result.IsSuccess)
                {
                    Debug.WriteLine($"[Dao_User] UpdateUserAsync failed: {result.ErrorMessage}");
                    LoggingUtility.Log($"UpdateUserAsync failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in UpdateUserAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"UpdateUserAsync failed with exception: {ex.Message}");
            }
        }

        internal static async Task DeleteUserAsync(string user, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering DeleteUserAsync(user={user}, useAsync={useAsync})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_Delete_User",
                    new Dictionary<string, object> { ["User"] = user }, // Remove p_ prefix - added automatically
                    null, // No progress helper for this method
                    useAsync
                );

                if (!result.IsSuccess)
                {
                    Debug.WriteLine($"[Dao_User] DeleteUserAsync failed: {result.ErrorMessage}");
                    LoggingUtility.Log($"DeleteUserAsync failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in DeleteUserAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"DeleteUserAsync failed with exception: {ex.Message}");
            }
        }

        #endregion

        #region Queries

        internal static async Task<DataTable> GetAllUsersAsync(bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetAllUsersAsync(useAsync={useAsync})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_Get_All",
                    null, // No parameters needed
                    null, // No progress helper for this method
                    useAsync
                );

                if (dataResult.IsSuccess && dataResult.Data != null)
                {
                    return dataResult.Data;
                }
                else
                {
                    Debug.WriteLine($"[Dao_User] GetAllUsersAsync failed: {dataResult.ErrorMessage}");
                    LoggingUtility.Log($"GetAllUsersAsync failed: {dataResult.ErrorMessage}");
                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in GetAllUsersAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"GetAllUsersAsync failed with exception: {ex.Message}");
                return new DataTable();
            }
        }

        internal static async Task<DataRow?> GetUserByUsernameAsync(string user, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetUserByUsernameAsync(user={user}, useAsync={useAsync})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_Get_ByUser",
                    new Dictionary<string, object> { ["User"] = user }, // Remove p_ prefix - added automatically
                    null, // No progress helper for this method
                    useAsync
                );

                if (dataResult.IsSuccess && dataResult.Data != null && dataResult.Data.Rows.Count > 0)
                {
                    Debug.WriteLine($"[Dao_User] GetUserByUsernameAsync result: {dataResult.Data.Rows.Count} rows");
                    return dataResult.Data.Rows[0];
                }
                else
                {
                    Debug.WriteLine($"[Dao_User] GetUserByUsernameAsync failed or no results: {dataResult.ErrorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in GetUserByUsernameAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"GetUserByUsernameAsync failed with exception: {ex.Message}");
                return null;
            }
        }

        internal static async Task<bool> UserExistsAsync(string user, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering UserExistsAsync(user={user}, useAsync={useAsync})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_Exists",
                    new Dictionary<string, object> { ["User"] = user }, // Remove p_ prefix - added automatically
                    null, // No progress helper for this method
                    useAsync
                );

                if (dataResult.IsSuccess && dataResult.Data != null && dataResult.Data.Rows.Count > 0)
                {
                    bool exists = Convert.ToInt32(dataResult.Data.Rows[0]["UserExists"]) > 0;
                    Debug.WriteLine($"[Dao_User] UserExistsAsync result: {exists}");
                    return exists;
                }
                else
                {
                    Debug.WriteLine($"[Dao_User] UserExistsAsync failed or no results: {dataResult.ErrorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in UserExistsAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"UserExistsAsync failed with exception: {ex.Message}");
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
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_ui_settings_GetShortcutsJson",
                    new Dictionary<string, object> { ["UserId"] = userId }, // Remove p_ prefix - added automatically
                    null, // No progress helper for this method
                    true
                );

                if (dataResult.IsSuccess && dataResult.Data != null && dataResult.Data.Rows.Count > 0)
                {
                    object? result = dataResult.Data.Rows[0][0]; // Get first column of first row
                    string? json = result?.ToString();
                    Debug.WriteLine($"[Dao_User] GetShortcutsJsonAsync result: {json}");
                    return json ?? "";
                }
                
                return "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in GetShortcutsJsonAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"GetShortcutsJsonAsync failed with exception: {ex.Message}");
                return "";
            }
        }

        internal static async Task SetShortcutsJsonAsync(string userId, string shortcutsJson)
        {
            Debug.WriteLine($"[Dao_User] Entering SetShortcutsJsonAsync(userId={userId})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure for proper status handling
                Dictionary<string, object> parameters = new()
                {
                    ["UserId"] = userId,              // FIXED: Remove p_ prefix - added automatically
                    ["ShortcutsJson"] = shortcutsJson // FIXED: Remove p_ prefix - added automatically
                };

                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_ui_settings_SetShortcutsJson",
                    parameters,
                    null, // No progress helper needed for this method
                    true  // Use async
                );

                Debug.WriteLine($"[Dao_User] SetShortcutsJsonAsync status: {result.IsSuccess}, message: {result.ErrorMessage}");

                if (!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage ?? "Failed to set shortcuts JSON");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetShortcutsJsonAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"SetShortcutsJsonAsync failed with exception: {ex.Message}");
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
                // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core to avoid p_Status parameter errors
                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_user_roles_Add",
                    new Dictionary<string, object>
                    {
                        ["UserID"] = userId,       // Remove p_ prefix - added automatically
                        ["RoleID"] = roleId,
                        ["AssignedBy"] = assignedBy
                    },
                    null, // No progress helper for this method
                    useAsync
                );

                if (!result.IsSuccess)
                {
                    Debug.WriteLine($"[Dao_User] AddUserRoleAsync failed: {result.ErrorMessage}");
                    LoggingUtility.Log($"AddUserRoleAsync failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in AddUserRoleAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"AddUserRoleAsync failed with exception: {ex.Message}");
            }
        }

        internal static async Task<int> GetUserRoleIdAsync(int userId, bool useAsync = false)
        {
            Debug.WriteLine($"[Dao_User] Entering GetUserRoleIdAsync(userId={userId}, useAsync={useAsync})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure вместо Helper_Database_Core для избежания ошибок параметра p_Status
                var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_user_roles_GetRoleId_ByUserId",
                    new Dictionary<string, object> { ["UserID"] = userId }, // Remove p_ prefix - added автоматически
                    null, // No progress helper for this method
                    useAsync
                );

                if (dataResult.IsSuccess && dataResult.Data != null && dataResult.Data.Rows.Count > 0)
                {
                    if (int.TryParse(dataResult.Data.Rows[0]["RoleID"]?.ToString(), out int roleId))
                    {
                        // Получить информацию о роли
                        var roleResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                            Model_AppVariables.ConnectionString,
                            "sys_roles_Get_ById",
                            new Dictionary<string, object> { ["ID"] = roleId }, // Remove p_ prefix - added автоматически
                            null, // No progress helper for this method
                            useAsync
                        );
                        
                        return roleId;
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in GetUserRoleIdAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"GetUserRoleIdAsync failed with exception: {ex.Message}");
                return 0;
            }
        }

        internal static async Task SetUserRoleAsync(int userId, int newRoleId, string assignedBy, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering SetUserRoleAsync(userId={userId}, newRoleId={newRoleId}, assignedBy={assignedBy}, useAsync={useAsync})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure вместо Helper_Database_Core для избежания ошибок параметра p_Status
                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_user_roles_Update",
                    new Dictionary<string, object>
                    {
                        ["UserID"] = userId,          // Remove p_ prefix - added автоматически
                        ["NewRoleID"] = newRoleId,
                        ["AssignedBy"] = assignedBy
                    },
                    null, // No progress helper for this method
                    useAsync
                );

                if (!result.IsSuccess)
                {
                    Debug.WriteLine($"[Dao_User] SetUserRoleAsync failed: {result.ErrorMessage}");
                    LoggingUtility.Log($"SetUserRoleAsync failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetUserRoleAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"SetUserRoleAsync failed with exception: {ex.Message}");
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
                    // FIXED: Use Helper_Database_StoredProcedure вместо Helper_Database_Core для избежания ошибок параметра p_Status
                    var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                        Model_AppVariables.ConnectionString,
                        "sys_user_roles_Update",
                        new Dictionary<string, object>
                        {
                            ["UserID"] = userId,          // Remove p_ prefix - добавлено автоматически
                            ["NewRoleID"] = newRoleId,
                            ["AssignedBy"] = assignedBy
                        },
                        null, // No progress helper for this method
                        useAsync
                    );

                    if (!result.IsSuccess)
                    {
                        Debug.WriteLine($"[Dao_User] SetUsersRoleAsync failed for user {userId}: {result.ErrorMessage}");
                        LoggingUtility.Log($"SetUsersRoleAsync failed for user {userId}: {result.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in SetUsersRoleAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"SetUsersRoleAsync failed with exception: {ex.Message}");
            }
        }

        internal static async Task RemoveUserRoleAsync(int userId, int roleId, bool useAsync = false)
        {
            Debug.WriteLine(
                $"[Dao_User] Entering RemoveUserRoleAsync(userId={userId}, roleId={roleId}, useAsync={useAsync})");
            try
            {
                // FIXED: Use Helper_Database_StoredProcedure вместо Helper_Database_Core для избежания ошибок параметра p_Status
                var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_user_roles_Delete",
                    new Dictionary<string, object> 
                    { 
                        ["UserID"] = userId,  // Remove p_ prefix - добавлено автоматически
                        ["RoleID"] = roleId   // Remove p_ prefix - добавлено автоматически
                    },
                    null, // No progress helper for this method
                    useAsync
                );

                if (!result.IsSuccess)
                {
                    Debug.WriteLine($"[Dao_User] RemoveUserRoleAsync failed: {result.ErrorMessage}");
                    LoggingUtility.Log($"RemoveUserRoleAsync failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dao_User] Exception in RemoveUserRoleAsync: {ex}");
                LoggingUtility.LogDatabaseError(ex);
                // Don't call error handlers here to avoid recursion during startup
                LoggingUtility.Log($"RemoveUserRoleAsync failed with exception: {ex.Message}");
            }
        }

        #endregion
    }

    #endregion
}
