using MTM_WIP_Application.Controls.MainForm;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Services;

internal class Service_OnStartup
{
    internal static async Task RunStartupSequenceAsync()
    {
        Service_OnStartup_AppDataCleaner.WipeAppDataFolders();

        LoggingUtility.Log("Setting High DPI mode...");
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

        LoggingUtility.Log("Checking DPI scaling...");
        Helper_DpiChecker.CheckDpiScaling();

        ApplicationConfiguration.Initialize();

        LoggingUtility.Log("Initializing application...");
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        LoggingUtility.InitializeLogging();
        LoggingUtility.Log("Application starting...");
        LoggingUtility.CleanUpOldLogsIfNeeded();

        LoggingUtility.Log("Initializing ComboBox data sources...");
        await Helper_ComboBoxes.SetupPartDataTable();
        await Helper_ComboBoxes.SetupOperationDataTable();
        await Helper_ComboBoxes.SetupLocationDataTable();
        await Helper_ComboBoxes.SetupUserDataTable();

        LoggingUtility.Log("Running VersionChecker...");
        Service_Timer_VersionChecker.Initialize();
    }

    internal static void RunApplication()
    {
        var mainForm = new MainForm();


        ControlRemoveTab.MainFormInstance = mainForm;
        ControlInventoryTab.MainFormInstance = mainForm;
        ControlTransferTab.MainFormInstance = mainForm;
        Control_AdvancedInventory.MainFormInstance = mainForm;

        Service_Timer_VersionChecker.MainFormInstance = mainForm;

        Debug.WriteLine("Starting main form...");
        LoggingUtility.Log("Starting main form...");
        Application.Run(mainForm);

        Debug.WriteLine("Application started.");
        LoggingUtility.Log("Application started.");
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