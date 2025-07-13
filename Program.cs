// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Forms.Splash;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        try
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Model_AppVariables.User = Dao_System.System_GetUserName();
            _ = Dao_System.System_UserAccessTypeAsync(true);
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new DefaultTraceListener());
            Trace.AutoFlush = true;
            Console.WriteLine("[Trace] [Main] Application starting...");
            Trace.WriteLine("[Trace] [Main] Application starting...");

            // Run the splash screen as the main form
            Application.Run(new StartupSplashApplicationContext());

            Console.WriteLine("[Trace] [Main] Application exiting Main().");
            Trace.WriteLine("[Trace] [Main] Application exiting Main().");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(Main));
        }
    }
}

// Custom ApplicationContext to handle splash and main form
public class StartupSplashApplicationContext : ApplicationContext
{
    private SplashScreenForm? _splashScreen;
    private MainForm? _mainForm;

    public StartupSplashApplicationContext()
    {
        try
        {
            _splashScreen = new SplashScreenForm();
            _splashScreen.Shown += async (s, e) => await RunStartupAsync();
            _splashScreen.FormClosed += SplashScreen_FormClosed;
            _splashScreen.Show();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(StartupSplashApplicationContext));
        }
    }

    private void SplashScreen_FormClosed(object? sender, EventArgs e)
    {
        try
        {
            // Only exit if main form is not shown
            if (_mainForm == null || _mainForm.IsDisposed)
                ExitThread();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(SplashScreen_FormClosed));
        }
    }

    private async Task RunStartupAsync()
    {
        try
        {
            var progress = 0;
            _splashScreen?.UpdateProgress(progress, "Starting startup sequence...");
            await Task.Delay(100);

            // Step 1: Initializing logging
            progress = 5;
            _splashScreen?.UpdateProgress(progress, "Initializing logging...");
            await LoggingUtility.InitializeLoggingAsync();
            progress = 10;
            _splashScreen?.UpdateProgress(progress, "Logging initialized.");
            await Task.Delay(50);

            // Step 2: Cleaning up old logs
            progress = 15;
            _splashScreen?.UpdateProgress(progress, "Cleaning up old logs...");
            await LoggingUtility.CleanUpOldLogsIfNeededAsync();
            progress = 20;
            _splashScreen?.UpdateProgress(progress, "Old logs cleaned up.");
            await Task.Delay(50);

            // Step 3: Wiping app data folders
            progress = 25;
            _splashScreen?.UpdateProgress(progress, "Wiping app data folders...");
            await Task.Run(() => Service_OnStartup_AppDataCleaner.WipeAppDataFolders());
            progress = 30;
            _splashScreen?.UpdateProgress(progress, "App data folders wiped.");
            await Task.Delay(50);

            // Step 4: Setting up part data table
            progress = 32;
            _splashScreen?.UpdateProgress(progress, "Setting up part data table...");
            await Helper_UI_ComboBoxes.SetupPartDataTable();
            progress = 36;
            _splashScreen?.UpdateProgress(progress, "Part data table set up.");
            await Task.Delay(50);

            // Step 5: Setting up operation data table
            progress = 38;
            _splashScreen?.UpdateProgress(progress, "Setting up operation data table...");
            await Helper_UI_ComboBoxes.SetupOperationDataTable();
            progress = 42;
            _splashScreen?.UpdateProgress(progress, "Operation data table set up.");
            await Task.Delay(50);

            // Step 6: Setting up location data table
            progress = 44;
            _splashScreen?.UpdateProgress(progress, "Setting up location data table...");
            await Helper_UI_ComboBoxes.SetupLocationDataTable();
            progress = 48;
            _splashScreen?.UpdateProgress(progress, "Location data table set up.");
            await Task.Delay(50);

            // Step 7: Setting up user data table
            progress = 50;
            _splashScreen?.UpdateProgress(progress, "Setting up user data table...");
            await Helper_UI_ComboBoxes.SetupUserDataTable();
            progress = 54;
            _splashScreen?.UpdateProgress(progress, "User data table set up.");
            await Task.Delay(50);

            // Step 8: Initializing version checker
            progress = 58;
            _splashScreen?.UpdateProgress(progress, "Initializing version checker...");
            Service_Timer_VersionChecker.Initialize();
            progress = 60;
            _splashScreen?.UpdateProgress(progress, "Version checker initialized.");
            await Task.Delay(50);

            // Step 9: Initializing theme system
            progress = 65;
            _splashScreen?.UpdateProgress(progress, "Initializing theme system...");
            await Core_Themes.Core_AppThemes.InitializeThemeSystemAsync(Model_AppVariables.User);
            progress = 70;
            _splashScreen?.UpdateProgress(progress, "Theme system initialized.");
            await Task.Delay(50);

            // Step 10: User full name loaded
            progress = 72;
            _splashScreen?.UpdateProgress(progress, $"User Full Name loaded: {Model_AppVariables.User}");
            await Task.Delay(50);

            // Step 11: Loading theme settings
            progress = 75;
            _splashScreen?.UpdateProgress(progress, "Loading theme settings...");
            var fontSize = await Dao_User.GetThemeFontSizeAsync(Model_AppVariables.User);
            Model_AppVariables.ThemeFontSize = fontSize ?? 9;
            Model_AppVariables.UserUiColors = await Core_Themes.GetUserThemeColorsAsync(Model_AppVariables.User);
            progress = 80;
            _splashScreen?.UpdateProgress(progress, "Theme settings loaded.");
            await Task.Delay(50);

            // Step 12: Startup sequence completed
            progress = 85;
            _splashScreen?.UpdateProgress(progress, "Startup sequence completed.");
            await Task.Delay(100);

            // Continue with main form creation and configuration
            progress = 90;
            _splashScreen?.UpdateProgress(progress, "Creating main form...");
            await Task.Delay(200);
            _mainForm = new MainForm();
            _mainForm.FormClosed += (s, e) => ExitThread();
            progress = 92;
            _splashScreen?.UpdateProgress(progress, "Configuring form instances...");
            ControlRemoveTab.MainFormInstance = _mainForm;
            ControlInventoryTab.MainFormInstance = _mainForm;
            ControlTransferTab.MainFormInstance = _mainForm;
            Control_AdvancedInventory.MainFormInstance = _mainForm;
            Control_AdvancedRemove.MainFormInstance = _mainForm;
            Control_QuickButtons.MainFormInstance = _mainForm;
            Helper_UI_ComboBoxes.MainFormInstance = _mainForm;
            Service_Timer_VersionChecker.MainFormInstance = _mainForm;
            progress = 95;
            _splashScreen?.UpdateProgress(progress, "Applying theme...");
            Core_Themes.ApplyTheme(_mainForm);
            progress = 100;
            _splashScreen?.UpdateProgress(progress, "Ready to start!");
            await Task.Delay(500);
            _mainForm.Show();
            if (_splashScreen != null)
            {
                _splashScreen.FormClosed -= SplashScreen_FormClosed;
                _splashScreen.Close();
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(RunStartupAsync));
            MessageBox.Show($@"Startup error: {ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _splashScreen?.Close();
        }
    }
}