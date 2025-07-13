

using System.Diagnostics;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Services;

internal static class Service_OnStartup
{
    internal static async Task RunStartupSequenceAsync()
    {
        async Task LogStep(string msg)
        {
            var logMsg = $"[STARTUP TRACE] {msg}";
            Trace.WriteLine(logMsg);
            Trace.WriteLine(logMsg);
            var logPath = Path.Combine(Path.GetTempPath(), "startup_trace.log");
            try
            {
                await File.AppendAllTextAsync(logPath, logMsg + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Log write error: " + ex);
            }
        }

        await LogStep("Starting startup sequence...");

        await LogStep("Initializing logging...");
        await LoggingUtility.InitializeLoggingAsync();
        await LogStep("Logging initialized.");

        await LogStep("Cleaning up old logs...");
        await LoggingUtility.CleanUpOldLogsIfNeededAsync();
        await LogStep("Old logs cleaned up.");

        await LogStep("Wiping app data folders...");
        await Task.Run(() => Service_OnStartup_AppDataCleaner.WipeAppDataFolders());
        await LogStep("App data folders wiped.");

        await LogStep("Setting up part data table...");
        await Helper_UI_ComboBoxes.SetupPartDataTable();
        await LogStep("Part data table set up.");

        await LogStep("Setting up operation data table...");
        await Helper_UI_ComboBoxes.SetupOperationDataTable();
        await LogStep("Operation data table set up.");

        await LogStep("Setting up location data table...");
        await Helper_UI_ComboBoxes.SetupLocationDataTable();
        await LogStep("Location data table set up.");

        await LogStep("Setting up 2nd location data table...");
        await Helper_UI_ComboBoxes.Setup2ndLocationDataTable();
        await LogStep("2nd Location data table set up.");

        await LogStep("Setting up user data table...");
        await Helper_UI_ComboBoxes.SetupUserDataTable();
        await LogStep("User data table set up.");

        await LogStep("Initializing version checker...");
        Service_Timer_VersionChecker.Initialize();
        await LogStep("Version checker initialized.");

        await LogStep("Initializing theme system...");
        await Core_Themes.Core_AppThemes.InitializeThemeSystemAsync(Model_AppVariables.User);
        await LogStep("Theme system initialized.");

        await LogStep($"User Full Name loaded: {Model_AppVariables.User}");

        await LogStep("Loading theme settings...");
        var fontSize = await Dao_User.GetThemeFontSizeAsync(Model_AppVariables.User);
        Model_AppVariables.ThemeFontSize = fontSize ?? 9;
        Model_AppVariables.UserUiColors = await Core_Themes.GetUserThemeColorsAsync(Model_AppVariables.User);
        await LogStep("Theme settings loaded.");

        await LogStep("Startup sequence completed.");
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