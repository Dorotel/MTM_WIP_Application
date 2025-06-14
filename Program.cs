using MTM_WIP_Application.Controls.MainForm;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Services;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace MTM_WIP_Application;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Debug.WriteLine("Main method started.");
        ApplicationLog.Log("Main method started.");

        try
        {
            RunStartupSequence();
            RunApplication();
        }
        catch (MySqlException ex)
        {
            ApplicationLog.LogDatabaseError(ex);
            Service_OnEvent_ExceptionHandler.HandleDatabaseError();
        }
        catch (Exception ex)
        {
            ApplicationLog.LogDatabaseError(ex);
            MessageBox.Show(@"An error occurred on Main in Program.cs:
" + ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Extracted for testability
    internal static void RunStartupSequence()
    {
        Service_OnStartup_AppDataCleaner.WipeAppDataFolders();
        Service_OnStartup_ShortcutManager.EnsureApplicationShortcut();

        Debug.WriteLine("Setting High DPI mode...");
        ApplicationLog.Log("Setting High DPI mode...");
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

        Debug.WriteLine("Checking DPI scaling...");
        ApplicationLog.Log("Checking DPI scaling...");
        Helper_DpiChecker.CheckDpiScaling();

        ApplicationConfiguration.Initialize();

        Debug.WriteLine("Initializing application...");
        ApplicationLog.Log("Initializing application...");
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        ApplicationLog.InitializeLogging();
        ApplicationLog.Log("Application starting...");
        ApplicationLog.CleanUpOldLogsIfNeeded();

        Debug.WriteLine("Running VersionChecker...");
        ApplicationLog.Log("Running VersionChecker...");
        Service_Timer_VersionChecker.Initialize();
    }


    // Extracted for testability
    internal static void RunApplication()
    {
        var mainForm = new MainForm();


        ControlRemoveTab.MainFormInstance = mainForm;
        ControlInventoryTab.MainFormInstance = mainForm;
        ControlTransferTab.MainFormInstance = mainForm;

        // Register the MainForm instance for live updates (if desired)
        Service_Timer_VersionChecker.MainFormInstance = mainForm;

        Debug.WriteLine("Starting main form...");
        ApplicationLog.Log("Starting main form...");
        Application.Run(mainForm);

        Debug.WriteLine("Application started.");
        ApplicationLog.Log("Application started.");
    }
}