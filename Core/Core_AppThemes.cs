using System.Data;
using System.Diagnostics;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Core;

#region Core_AppThemes

public static class Core_AppThemes
{
    #region Theme Definition

    public class AppTheme
    {
        public Model_UserUiColors Colors { get; set; } = new();
        public Font? FormFont { get; set; }
    }

    #endregion

    #region Theme Registry

    private static Dictionary<string, AppTheme> Themes = new();

    /// <summary>
    /// Loads the user's theme name from usr_ui_settings and sets Model_AppVariables.ThemeName.
    /// </summary>
    public static async Task LoadAndSetUserThemeNameAsync(string userId)
    {
        try
        {
            using var conn = new MySql.Data.MySqlClient.MySqlConnection(Model_AppVariables.ConnectionString);
            await conn.OpenAsync();
            using var cmd = new MySql.Data.MySqlClient.MySqlCommand("usr_ui_settings_GetThemeName", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("p_UserId", userId);
            var themeNameParam =
                new MySql.Data.MySqlClient.MySqlParameter("p_ThemeName", MySql.Data.MySqlClient.MySqlDbType.VarChar,
                    255)
                {
                    Direction = ParameterDirection.Output
                };
            cmd.Parameters.Add(themeNameParam);
            await cmd.ExecuteNonQueryAsync();
            var themeName = themeNameParam.Value?.ToString();
            Model_AppVariables.ThemeName = string.IsNullOrWhiteSpace(themeName) ? "Default" : themeName;
            LoggingUtility.Log($"Loaded user theme name for user: {userId} = {Model_AppVariables.ThemeName}");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    /// <summary>
    /// Loads all themes from the app_themes table into the Themes dictionary.
    /// </summary>
    public static async Task LoadThemesFromDatabaseAsync()
    {
        try
        {
            var themes = new Dictionary<string, AppTheme>();
            var helper = new Helper_Database_Core(Model_AppVariables.ConnectionString);
            var dt = await helper.ExecuteDataTable("SELECT ThemeName, SettingsJson FROM app_themes", null, true,
                CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var themeName = row["ThemeName"]?.ToString();
                var SettingsJson = row["SettingsJson"]?.ToString();
                if (!string.IsNullOrWhiteSpace(themeName) && !string.IsNullOrWhiteSpace(SettingsJson))
                    try
                    {
                        var options = new System.Text.Json.JsonSerializerOptions();
                        options.Converters.Add(new JsonColorConverter());
                        var colors =
                            System.Text.Json.JsonSerializer.Deserialize<Model_UserUiColors>(SettingsJson, options);
                        if (colors != null) themes[themeName] = new AppTheme { Colors = colors, FormFont = null };
                    }
                    catch (System.Text.Json.JsonException jsonEx)
                    {
                        LoggingUtility.LogApplicationError(jsonEx);
                    }
            }

            Themes = themes;
            LoggingUtility.Log("Loaded themes from database.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    #endregion

    #region Theme Accessors

    public static IEnumerable<string> GetThemeNames()
    {
        try
        {
            Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
            return Themes.Keys;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    public static bool ThemeExists(string themeName)
    {
        try
        {
            Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
            return Themes.ContainsKey(themeName);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    public static AppTheme GetCurrentTheme()
    {
        try
        {
            var themeName = Model_AppVariables.ThemeName ?? "Default";
            if (Themes.TryGetValue(themeName, out var theme))
                return theme;
            Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
            return Themes.ContainsKey("Default") ? Themes["Default"] : new AppTheme();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    public static AppTheme GetTheme(string themeName)
    {
        try
        {
            if (Themes.TryGetValue(themeName, out var theme))
                return theme;
            Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
            return Themes.ContainsKey("Default") ? Themes["Default"] : new AppTheme();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    public static string GetEffectiveThemeName()
    {
        try
        {
            var themeName = Model_AppVariables.ThemeName ?? "Default";
            if (!Themes.ContainsKey(themeName)) themeName = "Default";
            Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
            return themeName;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    public static Color GetThemeColor(string propertyName)
    {
        try
        {
            var theme = GetCurrentTheme();
            var colors = theme.Colors;
            var property = typeof(Model_UserUiColors).GetProperty(propertyName)
                           ?? throw new InvalidOperationException(
                               $"Property '{propertyName}' not found on Model_UserUiColors.");
            var value = property.GetValue(colors);
            if (value is Color color)
                return color;
            if (value is Color nullableColor)
                return nullableColor;
            throw new InvalidOperationException($"Property '{propertyName}' is not a Color or is null.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    #endregion

    #region Theme Startup Sequence

    /// <summary>
    /// Call this at startup to initialize theme system for the current user.
    /// </summary>
    public static async Task InitializeThemeSystemAsync(string userId)
    {
        try
        {
            await LoadAndSetUserThemeNameAsync(userId);
            await LoadThemesFromDatabaseAsync();
            Debug.WriteLine($"Themes count: {Themes.Count}");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            throw;
        }
    }

    #endregion
}

#endregion