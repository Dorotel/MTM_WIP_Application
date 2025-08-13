using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Forms.Splash;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Services
{
    public class Service_Onstartup_StartupSplashApplicationContext : ApplicationContext
    {
        #region Fields

        private SplashScreenForm? _splashScreen;
        private MainForm? _mainForm;

        #endregion

        #region Constructors

        public Service_Onstartup_StartupSplashApplicationContext()
        {
            try
            {
                LoggingUtility.Log("[Splash] Initializing splash screen");
                _splashScreen = new SplashScreenForm();
                _splashScreen.Shown += async (s, e) => await RunStartupAsync();
                _splashScreen.FormClosed += SplashScreen_FormClosed;
                _splashScreen.Show();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                HandleStartupException(ex);
            }
        }

        #endregion

        #region Event Handlers

        private void SplashScreen_FormClosed(object? sender, EventArgs e)
        {
            try
            {
                if (_mainForm == null || _mainForm.IsDisposed)
                {
                    LoggingUtility.Log("[Splash] Exiting application thread - MainForm not available");
                    ExitThread();
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                HandleStartupException(ex);
            }
        }

        #endregion

        #region Startup Sequence

        private async Task RunStartupAsync()
        {
            try
            {
                LoggingUtility.Log("[Splash] Starting startup sequence");
                int progress = 0;
                _splashScreen?.UpdateProgress(progress, "Starting startup sequence...");
                await Task.Delay(100);

                // 1. Initialize logging with error handling
                progress = 5;
                _splashScreen?.UpdateProgress(progress, "Initializing logging...");
                try
                {
                    await LoggingUtility.InitializeLoggingAsync();
                    progress = 10;
                    _splashScreen?.UpdateProgress(progress, "Logging initialized.");
                    LoggingUtility.Log("[Splash] Logging system initialized");
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    ShowStartupStepError("Logging Initialization Failed", ex);
                    return;
                }
                await Task.Delay(50);

                // 2. Clean up old logs with error handling
                progress = 15;
                _splashScreen?.UpdateProgress(progress, "Cleaning up old logs...");
                try
                {
                    await LoggingUtility.CleanUpOldLogsIfNeededAsync();
                    progress = 20;
                    _splashScreen?.UpdateProgress(progress, "Old logs cleaned up.");
                    LoggingUtility.Log("[Splash] Log cleanup completed");
                }
                catch (Exception ex)
                {
                    // Log cleanup failure is not critical - continue startup
                    LoggingUtility.LogApplicationError(ex);
                    LoggingUtility.Log("[Splash] Log cleanup failed, continuing startup");
                }
                await Task.Delay(50);

                // 3. Clean app data folders with error handling
                progress = 25;
                _splashScreen?.UpdateProgress(progress, "Wiping app data folders...");
                try
                {
                    await Task.Run(() => Service_OnStartup_AppDataCleaner.WipeAppDataFolders());
                    progress = 30;
                    _splashScreen?.UpdateProgress(progress, "App data folders wiped.");
                    LoggingUtility.Log("[Splash] App data cleanup completed");
                }
                catch (Exception ex)
                {
                    // App data cleanup failure is not critical - continue startup
                    LoggingUtility.LogApplicationError(ex);
                    LoggingUtility.Log("[Splash] App data cleanup failed, continuing startup");
                }
                await Task.Delay(50);

                // 4. Verify database connectivity using helper patterns - CRITICAL STEP
                progress = 35;
                _splashScreen?.UpdateProgress(progress, "Verifying database connectivity...");
                try
                {
                    var connectivityResult = await VerifyDatabaseConnectivityWithHelperAsync();
                    if (!connectivityResult.IsSuccess)
                    {
                        LoggingUtility.Log($"[Splash] Database connectivity verification failed: {connectivityResult.StatusMessage}");
                        // Error already shown in VerifyDatabaseConnectivityWithHelperAsync
                        _splashScreen?.Close();
                        return;
                    }
                    progress = 40;
                    _splashScreen?.UpdateProgress(progress, "Database connectivity verified.");
                    LoggingUtility.Log("[Splash] Database connectivity verified during startup");
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    ShowStartupStepError("Database Connectivity Check Failed", ex);
                    return;
                }
                await Task.Delay(50);

                // 5. Setup data tables with error handling - CRITICAL STEP
                progress = 45;
                _splashScreen?.UpdateProgress(progress, "Setting up Data Tables...");
                try
                {
                    await Helper_UI_ComboBoxes.SetupDataTables();
                    progress = 50;
                    _splashScreen?.UpdateProgress(progress, "Data Tables set up.");
                    LoggingUtility.Log("[Splash] Data tables setup completed");
                }
                catch (MySqlException ex)
                {
                    LoggingUtility.LogDatabaseError(ex);
                    string userMessage = GetUserFriendlyConnectionError(ex);
                    ShowErrorDialog("Data Tables Setup Failed", userMessage);
                    return;
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    ShowStartupStepError("Data Tables Setup Failed", ex);
                    return;
                }
                await Task.Delay(50);

                // 6. Initialize version checker with error handling
                progress = 60;
                _splashScreen?.UpdateProgress(progress, "Initializing version checker...");
                try
                {
                    Service_Timer_VersionChecker.Initialize();
                    progress = 65;
                    _splashScreen?.UpdateProgress(progress, "Version checker initialized.");
                    LoggingUtility.Log("[Splash] Version checker initialized");
                }
                catch (Exception ex)
                {
                    // Version checker failure is not critical - continue startup
                    LoggingUtility.LogApplicationError(ex);
                    LoggingUtility.Log("[Splash] Version checker initialization failed, continuing startup");
                }
                await Task.Delay(50);

                // 7. Initialize theme system with error handling
                progress = 70;
                _splashScreen?.UpdateProgress(progress, "Initializing theme system...");
                try
                {
                    await Core_Themes.Core_AppThemes.InitializeThemeSystemAsync(Model_AppVariables.User);
                    progress = 75;
                    _splashScreen?.UpdateProgress(progress, "Theme system initialized.");
                    LoggingUtility.Log("[Splash] Theme system initialized");
                }
                catch (MySqlException ex)
                {
                    // Theme system might use database - show specific error
                    LoggingUtility.LogDatabaseError(ex);
                    string userMessage = GetUserFriendlyConnectionError(ex);
                    ShowErrorDialog("Theme System Initialization Failed", userMessage);
                    return;
                }
                catch (Exception ex)
                {
                    // Theme initialization failure is not critical - continue with defaults
                    LoggingUtility.LogApplicationError(ex);
                    LoggingUtility.Log("[Splash] Theme system initialization failed, using defaults");
                }
                await Task.Delay(50);

                // 8. Load user context (no database dependency)
                progress = 80;
                _splashScreen?.UpdateProgress(progress, $"User Full Name loaded: {Model_AppVariables.User}");
                LoggingUtility.Log($"[Splash] User context loaded: {Model_AppVariables.User}");
                await Task.Delay(50);

                // 9. Load theme settings with error handling
                progress = 85;
                _splashScreen?.UpdateProgress(progress, "Loading theme settings...");
                try
                {
                    await LoadThemeSettingsAsync();
                    progress = 90;
                    _splashScreen?.UpdateProgress(progress, "Theme settings loaded.");
                    LoggingUtility.Log("[Splash] Theme settings loaded");
                }
                catch (MySqlException ex)
                {
                    // Theme settings use database - show specific error
                    LoggingUtility.LogDatabaseError(ex);
                    string userMessage = GetUserFriendlyConnectionError(ex);
                    ShowErrorDialog("Theme Settings Loading Failed", userMessage);
                    return;
                }
                catch (Exception ex)
                {
                    // Theme settings failure is not critical - continue with defaults
                    LoggingUtility.LogApplicationError(ex);
                    LoggingUtility.Log("[Splash] Theme settings loading failed, using defaults");
                }
                await Task.Delay(50);

                // 10. Complete core startup
                progress = 93;
                _splashScreen?.UpdateProgress(progress, "Startup sequence completed.");
                LoggingUtility.Log("[Splash] Core startup sequence completed");
                await Task.Delay(100);

                // 11. Create main form with error handling - CRITICAL STEP
                progress = 95;
                _splashScreen?.UpdateProgress(progress, "Creating main form...");
                try
                {
                    await Task.Delay(200);
                    _mainForm = new MainForm();
                    _mainForm.FormClosed += (s, e) => ExitThread();
                    LoggingUtility.Log("[Splash] MainForm created");
                }
                catch (MySqlException ex)
                {
                    LoggingUtility.LogDatabaseError(ex);
                    string userMessage = GetUserFriendlyConnectionError(ex);
                    ShowErrorDialog("Main Form Creation Failed", userMessage);
                    return;
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    ShowStartupStepError("Main Form Creation Failed", ex);
                    return;
                }

                // 12. Configure form instances with error handling
                progress = 97;
                _splashScreen?.UpdateProgress(progress, "Configuring form instances...");
                try
                {
                    ConfigureFormInstances();
                    LoggingUtility.Log("[Splash] Form instances configured");
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    ShowStartupStepError("Form Instance Configuration Failed", ex);
                    return;
                }

                // 13. Apply theme with error handling
                progress = 99;
                _splashScreen?.UpdateProgress(progress, "Applying theme...");
                try
                {
                    ApplyThemeToMainForm();
                    LoggingUtility.Log("[Splash] Theme applied to MainForm");
                }
                catch (Exception ex)
                {
                    // Theme application failure is not critical
                    LoggingUtility.LogApplicationError(ex);
                    LoggingUtility.Log("[Splash] Theme application failed, continuing with default theme");
                }

                // 14. Show application with error handling - CRITICAL STEP
                progress = 100;
                _splashScreen?.UpdateProgress(progress, "Ready to start!");
                await Task.Delay(500);

                try
                {
                    ShowMainForm();
                    LoggingUtility.Log("[Splash] MainForm displayed - startup complete");

                    if (_splashScreen != null)
                    {
                        _splashScreen.FormClosed -= SplashScreen_FormClosed;
                        _splashScreen.Close();
                        LoggingUtility.Log("[Splash] Splash screen closed");
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    ShowStartupStepError("Main Form Display Failed", ex);
                    return;
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                string userMessage = GetUserFriendlyConnectionError(ex);
                ShowErrorDialog("Database Error During Startup", userMessage);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                HandleStartupException(ex);
            }
        }



        #endregion

        #region Database Connectivity (Async)

        /// <summary>
        /// Verify database connectivity using helper patterns with async support
        /// </summary>
        /// <returns>DaoResult indicating connectivity status</returns>
        private async Task<DaoResult> VerifyDatabaseConnectivityWithHelperAsync()
        {
            try
            {
                LoggingUtility.Log("[Splash] Starting async database connectivity verification");

                // Use consistent timeout settings
                var connectionStringBuilder = new MySqlConnectionStringBuilder(Model_AppVariables.ConnectionString)
                {
                    ConnectionTimeout = 10,
                    DefaultCommandTimeout = 10
                };

                using var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
                await connection.OpenAsync();

                // Test database functionality with version query
                using var command = new MySqlCommand("SELECT VERSION() as mysql_version", connection);
                var version = await command.ExecuteScalarAsync();

                if (version != null)
                {
                    LoggingUtility.Log($"[Splash] Database connectivity verified. MySQL version: {version}");
                    return DaoResult.Success($"Database connectivity verified. MySQL version: {version}");
                }
                else
                {
                    const string errorMsg = "Database version query returned null";
                    LoggingUtility.Log($"[Splash] {errorMsg}");
                    return DaoResult.Failure(errorMsg);
                }
            }
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);

                string userMessage = GetUserFriendlyConnectionError(ex);

                // CRITICAL FIX: Ensure MessageBox is shown on UI thread and blocks splash screen closure
                try
                {
                    if (_splashScreen?.InvokeRequired == true)
                    {
                        _splashScreen.Invoke(new Action(() =>
                            MessageBox.Show(userMessage, "Database Connection Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)));
                    }
                    else
                    {
                        MessageBox.Show(userMessage, "Database Connection Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception msgBoxEx)
                {
                    // Fallback if MessageBox fails
                    LoggingUtility.LogApplicationError(msgBoxEx);
                    Console.WriteLine($"[CRITICAL] Failed to show error dialog: {msgBoxEx.Message}");
                    Console.WriteLine($"[CRITICAL] Original database error: {userMessage}");
                }

                return DaoResult.Failure(userMessage, ex);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);

                string userMessage = "Unable to verify database connectivity during startup:\n\n" +
                                   $"{ex.Message}\n\n" +
                                   "Please check your network connection and database server status.\n" +
                                   "The application cannot start without database access.";

                // CRITICAL FIX: Ensure MessageBox is shown on UI thread
                try
                {
                    if (_splashScreen?.InvokeRequired == true)
                    {
                        _splashScreen.Invoke(new Action(() =>
                            MessageBox.Show(userMessage, "Database Connection Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)));
                    }
                    else
                    {
                        MessageBox.Show(userMessage, "Database Connection Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception msgBoxEx)
                {
                    // Fallback if MessageBox fails
                    LoggingUtility.LogApplicationError(msgBoxEx);
                    Console.WriteLine($"[CRITICAL] Failed to show error dialog: {msgBoxEx.Message}");
                    Console.WriteLine($"[CRITICAL] Original error: {userMessage}");
                }

                return DaoResult.Failure(userMessage, ex);
            }
        }

        /// <summary>
        /// Get user-friendly error message for MySQL connection errors
        /// </summary>
        /// <param name="ex">MySQL exception</param>
        /// <returns>User-friendly error message</returns>
        private static string GetUserFriendlyConnectionError(MySqlException ex)
        {
            if (ex.Message.Contains("Unknown database"))
            {
                string dbName = Model_Users.Database;
                string serverAddress = Model_Users.WipServerAddress;

#if DEBUG
                return $"The test database '{dbName}' does not exist on server '{serverAddress}'.\n\n" +
                       "This is a DEBUG build that requires the test database.\n\n" +
                       "Please:\n" +
                       "• Create the test database '{dbName}' on MySQL server\n" +
                       "• Or build and run the application in RELEASE mode\n" +
                       "• Contact your system administrator for database setup assistance\n\n" +
                       "The application cannot start without a valid database connection.";
#else
                return $"The database '{dbName}' does not exist on server '{serverAddress}'.\n\n" +
                       "Please contact your system administrator to:\n" +
                       "• Verify the database server is running\n" +
                       "• Ensure the database '{dbName}' exists and is accessible\n" +
                       "• Check database permissions for your user account\n\n" +
                       "The application cannot start without a valid database connection.";
#endif
            }

            if (ex.Message.Contains("Unable to connect to any of the specified MySQL hosts"))
            {
                return "Cannot connect to the database server during application startup.\n\n" +
                       "This usually means:\n" +
                       "• The database server is not running\n" +
                       "• The server address or port is incorrect\n" +
                       "• A firewall is blocking the connection\n\n" +
                       "Please verify the MySQL server is running and accessible.\n" +
                       "The application cannot start without a database connection.";
            }

            if (ex.Message.Contains("Access denied"))
            {
                return "Access denied when connecting to the database during startup.\n\n" +
                       "This usually means:\n" +
                       "• Your username or password is incorrect\n" +
                       "• Your account doesn't have permission to access the database\n\n" +
                       "Please check your database credentials with your system administrator.\n" +
                       "The application cannot start without proper database access.";
            }

            if (ex.Message.Contains("timeout"))
            {
                return "Database connection timed out during application startup.\n\n" +
                       "This usually means:\n" +
                       "• The database server is responding slowly\n" +
                       "• Network connectivity issues\n" +
                       "• The server is overloaded\n\n" +
                       "Please try starting the application again in a few moments.";
            }

            return $"Database connection failed during startup:\n\n{ex.Message}\n\n" +
                   "Please contact your system administrator for assistance.\n" +
                   "The application cannot start without a database connection.";
        }

        #endregion

        #region Exception Handling

        /// <summary>
        /// Handle startup exceptions with user-friendly messaging
        /// </summary>
        /// <param name="ex">Exception to handle</param>
        private void HandleStartupException(Exception ex)
        {
            try
            {
                string userMessage;

                if (ex is MySqlException mysqlEx)
                {
                    userMessage = GetUserFriendlyConnectionError(mysqlEx);
                }
                else
                {
                    userMessage = "A critical error occurred during application startup:\n\n" +
                                 $"{ex.Message}\n\n" +
                                 "The application will now close. Please check the log files for more details " +
                                 "and contact your system administrator if the problem persists.";
                }

                ShowErrorDialog("Startup Error", userMessage);
                _splashScreen?.Close();
            }
            catch (Exception innerEx)
            {
                // Fallback error handling for critical scenarios
                Console.WriteLine($"Critical error in startup exception handler: {innerEx.Message}");
                Console.WriteLine($"Original startup error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Show error for specific startup step failures
        /// </summary>
        /// <param name="stepName">Name of the failed startup step</param>
        /// <param name="ex">Exception that occurred</param>
        private void ShowStartupStepError(string stepName, Exception ex)
        {
            try
            {
                string userMessage;

                if (ex is MySqlException mysqlEx)
                {
                    userMessage = GetUserFriendlyConnectionError(mysqlEx);
                }
                else
                {
                    userMessage = $"Failed to complete startup step: {stepName}\n\n" +
                                 $"Error: {ex.Message}\n\n" +
                                 "The application cannot continue. Please check the log files for more details " +
                                 "and contact your system administrator if the problem persists.";
                }

                ShowErrorDialog(stepName, userMessage);
                _splashScreen?.Close();
            }
            catch (Exception innerEx)
            {
                // Fallback error handling
                Console.WriteLine($"Error showing startup step error: {innerEx.Message}");
                Console.WriteLine($"Original error in {stepName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Show error dialog with proper thread handling
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Error message</param>
        private void ShowErrorDialog(string title, string message)
        {
            try
            {
                if (_splashScreen?.InvokeRequired == true)
                {
                    _splashScreen.Invoke(new Action(() =>
                        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error)));
                }
                else
                {
                    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Final fallback
                Console.WriteLine($"Failed to show error dialog: {ex.Message}");
                Console.WriteLine($"Original error message: {message}");
            }
        }

        #endregion

        #region Form Configuration

        /// <summary>
        /// Load theme settings using established DAO patterns
        /// </summary>
        private async Task LoadThemeSettingsAsync()
        {
            try
            {
                LoggingUtility.Log("[Splash] Loading theme settings");

                int? fontSize = await Dao_User.GetThemeFontSizeAsync(Model_AppVariables.User);
                Model_AppVariables.ThemeFontSize = fontSize ?? 9;

                Model_AppVariables.UserUiColors = await Core_Themes.GetUserThemeColorsAsync(Model_AppVariables.User);

                LoggingUtility.Log($"[Splash] Theme settings loaded - Font size: {Model_AppVariables.ThemeFontSize}");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                // Set defaults if theme loading fails
                Model_AppVariables.ThemeFontSize = 9;
                LoggingUtility.Log("[Splash] Using default theme settings due to loading error");
            }
        }

        /// <summary>
        /// Configure form instances with proper error handling
        /// </summary>
        private void ConfigureFormInstances()
        {
            try
            {
                if (_mainForm == null) return;

                Control_RemoveTab.MainFormInstance = _mainForm;
                Control_InventoryTab.MainFormInstance = _mainForm;
                Control_TransferTab.MainFormInstance = _mainForm;
                Control_AdvancedInventory.MainFormInstance = _mainForm;
                Control_AdvancedRemove.MainFormInstance = _mainForm;
                Control_QuickButtons.MainFormInstance = _mainForm;
                Helper_UI_ComboBoxes.MainFormInstance = _mainForm;
                Service_Timer_VersionChecker.MainFormInstance = _mainForm;

                LoggingUtility.Log("[Splash] All form instances configured successfully");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw new InvalidOperationException("Failed to configure form instances", ex);
            }
        }

        /// <summary>
        /// Apply theme to MainForm with thread safety
        /// </summary>
        private void ApplyThemeToMainForm()
        {
            try
            {
                if (_mainForm == null) return;

                if (_mainForm.InvokeRequired)
                {
                    _mainForm.Invoke(new Action(() => Core_Themes.ApplyTheme(_mainForm)));
                }
                else
                {
                    Core_Themes.ApplyTheme(_mainForm);
                }

                LoggingUtility.Log("[Splash] Theme applied to MainForm");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                // Theme application failure is not critical for startup
                LoggingUtility.Log("[Splash] Theme application failed, continuing with default theme");
            }
        }

        /// <summary>
        /// Show MainForm with thread safety
        /// </summary>
        private void ShowMainForm()
        {
            try
            {
                if (_mainForm == null) return;

                if (_mainForm.InvokeRequired)
                {
                    _mainForm.Invoke(new Action(() => _mainForm.Show()));
                }
                else
                {
                    _mainForm.Show();
                }

                LoggingUtility.Log("[Splash] MainForm displayed successfully");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw new InvalidOperationException("Failed to show MainForm", ex);
            }
        }

        #endregion
    }
}
