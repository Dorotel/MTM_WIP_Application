using MTM_WIP_Application.ErrorHandling;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Services;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Timers;

namespace MTM_WIP_Application;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Debug.WriteLine("Main method started.");
        AppLogger.Log("Main method started.");

        try
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

            var mainForm = new MainForm();

            AppLogger.InitializeLogging();
            AppLogger.Log("Application starting...");
            AppLogger.CleanUpOldLogsIfNeeded();


            Debug.WriteLine("Running VersionChecker...");
            AppLogger.Log("Running VersionChecker...");
            VersionCheckerService.Initialize();


            Debug.WriteLine("Starting main form...");
            AppLogger.Log("Starting main form...");
            Application.Run(mainForm);

            Debug.WriteLine("Application started.");
            AppLogger.Log("Application started.");
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
}