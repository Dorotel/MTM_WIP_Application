using MTM_WIP_Application.Core;
using MTM_WIP_Application.ErrorHandling;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Services;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Timers;

namespace MTM_WIP_Application;

/// <summary>
///
/// Testing Passed: 05/31/2025
/// 
/// Main entry point for the MTM WIP Application.
/// This WinForms application manages Work-In-Process (WIP) inventory for manufacturing or warehouse environments.
/// Features include:
/// - Inventory management (add, update, reset, transfer, remove)
/// - Advanced entry forms for batch and multi-location transactions
/// - MySQL database integration for inventory, part, operation, and location data
/// - Theming and UI customization (multiple color themes, font size)
/// - Logging and robust error handling
/// - Version checking and update enforcement
/// - Printing support for inventory data
/// - User settings and changelog display
/// - AppData and shortcut management on startup
/// The application is modular, testable, and uses modern C# and .NET 9 features.
/// </summary>
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Debug.WriteLine("Main method started.");
        AppLogger.Log("Main method started.");

        try
        {
            RunStartupSequence();
            RunApplication();
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ExceptionHandler.HandleDatabaseError();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            MessageBox.Show(@"An error occurred on Main in Program.cs:
" + ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Extracted for testability
    internal static void RunStartupSequence()
    {
        AppDataCleaner.WipeAppDataFolders();
        ShortcutManager.EnsureApplicationShortcut();

        Debug.WriteLine("Setting High DPI mode...");
        AppLogger.Log("Setting High DPI mode...");
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

        Debug.WriteLine("Checking DPI scaling...");
        AppLogger.Log("Checking DPI scaling...");
        DpiChecker.CheckDpiScaling();

        ApplicationConfiguration.Initialize();

        Debug.WriteLine("Initializing application...");
        AppLogger.Log("Initializing application...");
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        AppLogger.InitializeLogging();
        AppLogger.Log("Application starting...");
        AppLogger.CleanUpOldLogsIfNeeded();

        Debug.WriteLine("Running VersionChecker...");
        AppLogger.Log("Running VersionChecker...");
        VersionCheckerService.Initialize();
    }

    // Extracted for testability
    internal static void RunApplication()
    {
        var mainForm = new MainForm();

        // Get current and server version
        var currentVersion = WipAppVariables.UserVersion;
        var serverVersion = VersionCheckerService.LastCheckedDatabaseVersion ?? "unknown";

        // Set version label on startup (always show both for debugging)
        mainForm.SetVersionLabel(currentVersion, serverVersion);

        // Register the MainForm instance for live updates (if desired)
        VersionCheckerService.MainFormInstance = mainForm;

        Debug.WriteLine("Starting main form...");
        AppLogger.Log("Starting main form...");
        Application.Run(mainForm);

        Debug.WriteLine("Application started.");
        AppLogger.Log("Application started.");
    }
}