using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using System.Diagnostics;
using System.Reflection;

namespace MTM_Inventory_Application.Services;

internal static class Service_OnStartup
{
    internal static async Task RunStartupSequenceAsync()
    {
        // THEME: Load user theme name and all themes before anything else

        LoggingUtility.InitializeLogging();
        LoggingUtility.CleanUpOldLogsIfNeeded();
        Service_OnStartup_AppDataCleaner.WipeAppDataFolders();
        await Helper_UI_ComboBoxes.SetupPartDataTable();
        await Helper_UI_ComboBoxes.SetupOperationDataTable();
        await Helper_UI_ComboBoxes.SetupLocationDataTable();
        await Helper_UI_ComboBoxes.SetupUserDataTable();

        // Write the current version to version.txt in the root folder
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
        Dao_File.WriteVersionToRoot(version);

        Service_Timer_VersionChecker.Initialize();
        await Core_Themes.Core_AppThemes.InitializeThemeSystemAsync(Model_AppVariables.User);
        Debug.WriteLine(
            $"[DEBUG] User Full Name loaded: {Model_AppVariables.User}");
        // RunStartupSequenceAsync entry        
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Set font size and user UI colors
        var fontSize = await Dao_User.GetThemeFontSizeAsync(Model_AppVariables.User);
        Model_AppVariables.ThemeFontSize = fontSize ?? 9;
        Model_AppVariables.UserUiColors = await Core_Themes.GetUserThemeColorsAsync(Model_AppVariables.User);
    }
}

internal static class Service_OnStartup_AppDataCleaner
{
    #region Public Methods

    public static void DeleteDirectoryContents(string directoryPath)
    {
        try
        {
            if (Directory.Exists(directoryPath))
            {
                foreach (var file in Directory.GetFiles(directoryPath)) File.Delete(file);
                foreach (var subDirectory in Directory.GetDirectories(directoryPath))
                    Directory.Delete(subDirectory, true);
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.Log($"Error deleting contents of directory {directoryPath}: {ex.Message}");
        }
    }

    public static void WipeAppDataFolders()
    {
        try
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MTM_WIP_APP");
            var localAppDataPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MTM_WIP_APP");
            DeleteDirectoryIfExists(appDataPath);
            DeleteDirectoryIfExists(localAppDataPath);
        }
        catch (Exception ex)
        {
            LoggingUtility.Log($"Error wiping MTM_WIP_APP folders: {ex.Message}");
        }
    }

    #endregion

    #region Private Methods

    private static void DeleteDirectoryIfExists(string path)
    {
        try
        {
            if (Directory.Exists(path)) Directory.Delete(path, true);
        }
        catch (Exception ex)
        {
            LoggingUtility.Log($"Error deleting directory {path}: {ex.Message}");
        }
    }

    #endregion
}