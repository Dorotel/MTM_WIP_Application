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

namespace MTM_Inventory_Application
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            try
            {
                Application.ThreadException += (sender, args) =>
                {
                    Console.WriteLine($"[Global Exception] ThreadException: {args.Exception}");
                    LoggingUtility.LogApplicationError(args.Exception);
                };
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    if (args.ExceptionObject is Exception ex)
                    {
                        Console.WriteLine($"[Global Exception] UnhandledException: {ex}");
                        LoggingUtility.LogApplicationError(ex);
                    }
                };
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

                Application.Run(new StartupSplashApplicationContext());

                Console.WriteLine("[Trace] [Main] Application exiting Main().");
                Trace.WriteLine("[Trace] [Main] Application exiting Main().");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Global Exception] Main catch: {ex}");
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(Main));
            }
        }
    }

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
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    nameof(StartupSplashApplicationContext));
            }
        }

        private void SplashScreen_FormClosed(object? sender, EventArgs e)
        {
            try
            {
                if (_mainForm == null || _mainForm.IsDisposed)
                {
                    ExitThread();
                }
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
                int progress = 0;
                _splashScreen?.UpdateProgress(progress, "Starting startup sequence...");
                await Task.Delay(100);

                // 1. Initializing logging
                progress = 5;
                _splashScreen?.UpdateProgress(progress, "Initializing logging...");
                await LoggingUtility.InitializeLoggingAsync();
                progress = 10;
                _splashScreen?.UpdateProgress(progress, "Logging initialized.");
                await Task.Delay(50);

                // 2. Cleaning up old logs
                progress = 15;
                _splashScreen?.UpdateProgress(progress, "Cleaning up old logs...");
                await LoggingUtility.CleanUpOldLogsIfNeededAsync();
                progress = 20;
                _splashScreen?.UpdateProgress(progress, "Old logs cleaned up.");
                await Task.Delay(50);

                // 3. Wiping app data folders
                progress = 25;
                _splashScreen?.UpdateProgress(progress, "Wiping app data folders...");
                await Task.Run(() => Service_OnStartup_AppDataCleaner.WipeAppDataFolders());
                progress = 30;
                _splashScreen?.UpdateProgress(progress, "App data folders wiped.");
                await Task.Delay(50);

                // 4. Migrating saved locations from old WIP Application
                progress = 35;
                _splashScreen?.UpdateProgress(progress, "Migrating saved locations...");
                await Dao_Inventory.MigrateSavedLocations();
                progress = 40;
                _splashScreen?.UpdateProgress(progress, "Saved locations migrated.");
                await Task.Delay(50);

                // 5. Setting up Data Tables
                progress = 45;
                _splashScreen?.UpdateProgress(progress, "Setting up Data Tables...");
                await Helper_UI_ComboBoxes.SetupDataTables();
                progress = 50;
                _splashScreen?.UpdateProgress(progress, "Data Tables set up.");
                await Task.Delay(50);

                // 6. Initializing version checker
                progress = 60;
                _splashScreen?.UpdateProgress(progress, "Initializing version checker...");
                Service_Timer_VersionChecker.Initialize();
                progress = 65;
                _splashScreen?.UpdateProgress(progress, "Version checker initialized.");
                await Task.Delay(50);

                // 7. Initializing theme system
                progress = 70;
                _splashScreen?.UpdateProgress(progress, "Initializing theme system...");
                await Core_Themes.Core_AppThemes.InitializeThemeSystemAsync(Model_AppVariables.User);
                progress = 75;
                _splashScreen?.UpdateProgress(progress, "Theme system initialized.");
                await Task.Delay(50);

                // 8. User full name loaded
                progress = 80;
                _splashScreen?.UpdateProgress(progress, $"User Full Name loaded: {Model_AppVariables.User}");
                await Task.Delay(50);

                // 9. Loading theme settings
                progress = 85;
                _splashScreen?.UpdateProgress(progress, "Loading theme settings...");
                int? fontSize = await Dao_User.GetThemeFontSizeAsync(Model_AppVariables.User);
                Model_AppVariables.ThemeFontSize = fontSize ?? 9;
                Model_AppVariables.UserUiColors = await Core_Themes.GetUserThemeColorsAsync(Model_AppVariables.User);
                progress = 90;
                _splashScreen?.UpdateProgress(progress, "Theme settings loaded.");
                await Task.Delay(50);

                // 10. Startup sequence completed
                progress = 93;
                _splashScreen?.UpdateProgress(progress, "Startup sequence completed.");
                await Task.Delay(100);

                // 11. Creating main form
                progress = 95;
                _splashScreen?.UpdateProgress(progress, "Creating main form...");
                await Task.Delay(200);
                _mainForm = new MainForm();
                _mainForm.FormClosed += (s, e) => ExitThread();

                // 12. Configuring form instances
                progress = 97;
                _splashScreen?.UpdateProgress(progress, "Configuring form instances...");
                Control_RemoveTab.MainFormInstance = _mainForm;
                Control_InventoryTab.MainFormInstance = _mainForm;
                Control_TransferTab.MainFormInstance = _mainForm;
                Control_AdvancedInventory.MainFormInstance = _mainForm;
                Control_AdvancedRemove.MainFormInstance = _mainForm;
                Control_QuickButtons.MainFormInstance = _mainForm;
                Helper_UI_ComboBoxes.MainFormInstance = _mainForm;
                Service_Timer_VersionChecker.MainFormInstance = _mainForm;

                // 13. Applying theme
                progress = 99;
                _splashScreen?.UpdateProgress(progress, "Applying theme...");
                if (_mainForm.InvokeRequired)
                {
                    _mainForm.Invoke(new Action(() => Core_Themes.ApplyTheme(_mainForm)));
                }
                else
                {
                    Core_Themes.ApplyTheme(_mainForm);
                }

                // 14. Ready to start!
                progress = 100;
                _splashScreen?.UpdateProgress(progress, "Ready to start!");
                await Task.Delay(500);
                if (_mainForm.InvokeRequired)
                {
                    _mainForm.Invoke(new Action(() => _mainForm.Show()));
                }
                else
                {
                    _mainForm.Show();
                }
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
}
