using System.Diagnostics;
using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Controls.SettingsForm;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Forms.Splash;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application
{
    internal static class Program
    {
        #region Entry Point

        [STAThread]
        private static void Main()
        {
            try
            {
                // Global exception handling setup
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

                // Register cleanup handler for application exit
                AppDomain.CurrentDomain.ProcessExit += (sender, args) => PerformAppCleanup();
                Application.ApplicationExit += (sender, args) => PerformAppCleanup();

                // Windows Forms initialization
                Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                LoggingUtility.Log("[Startup] Application initialization started");
                
                // User identification
                Model_AppVariables.User = Dao_System.System_GetUserName();
                LoggingUtility.Log($"[Startup] User identified: {Model_AppVariables.User}");
                
                // ENHANCED: Validate database connectivity using helper patterns BEFORE creating any forms
                var connectivityResult = ValidateDatabaseConnectivityWithHelper();
                if (!connectivityResult.IsSuccess)
                {
                    LoggingUtility.Log($"[Startup] Database connectivity validation failed: {connectivityResult.StatusMessage}");
                    return;
                }
                
                LoggingUtility.Log("[Startup] Database connectivity validated successfully");
                
                // Load user access permissions
                _ = Dao_System.System_UserAccessTypeAsync(true);
                
                // Trace setup
                Trace.Listeners.Clear();
                Trace.Listeners.Add(new DefaultTraceListener());
                Trace.AutoFlush = true;
                Console.WriteLine("[Trace] [Main] Application starting...");
                Trace.WriteLine("[Trace] [Main] Application starting...");

                // Start application with splash screen
                Application.Run(new StartupSplashApplicationContext());

                Console.WriteLine("[Trace] [Main] Application exiting Main().");
                Trace.WriteLine("[Trace] [Main] Application exiting Main().");
                LoggingUtility.Log("[Startup] Application shutdown completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Global Exception] Main catch: {ex}");
                LoggingUtility.LogApplicationError(ex);
                ShowFatalError("Application Startup Error", 
                    $"A fatal error occurred during application startup:\n\n{ex.Message}\n\nThe application will now exit.");
            }
            finally
            {
                // Ensure cleanup runs even if exceptions occur
                PerformAppCleanup();
            }
        }

        #endregion

        #region Database Connectivity Validation

        /// <summary>
        /// Validate database connectivity using Helper_Database_StoredProcedure patterns
        /// </summary>
        /// <returns>DaoResult indicating success or failure with user-friendly message</returns>
        private static DaoResult ValidateDatabaseConnectivityWithHelper()
        {
            try
            {
                Console.WriteLine("[Startup] Validating database connectivity...");
                LoggingUtility.Log("[Startup] Starting database connectivity validation");
                
                // First validate connection string format
                var formatResult = ValidateConnectionStringFormat();
                if (!formatResult.IsSuccess)
                {
                    return formatResult;
                }
                
                // Use helper for database health check with stored procedure approach
                var healthCheckResult = ValidateDatabaseHealthWithStoredProcedure();
                if (!healthCheckResult.IsSuccess)
                {
                    return healthCheckResult;
                }
                
                Console.WriteLine("[Startup] Database connectivity validated successfully.");
                LoggingUtility.Log("[Startup] Database connectivity validation completed successfully");
                return DaoResult.Success("Database connectivity validated successfully");
            }
            catch (Exception ex)
            {
                string errorMsg = $"Critical error during database validation: {ex.Message}";
                Console.WriteLine($"[Startup] {errorMsg}");
                LoggingUtility.LogApplicationError(ex);
                
                ShowDatabaseError("Database Validation Error", 
                    $"Unable to validate database connectivity:\n\n{ex.Message}\n\n" +
                    "Please check your network connection and database server status.");
                
                return DaoResult.Failure(errorMsg, ex);
            }
        }

        /// <summary>
        /// Validate database health using stored procedure approach consistent with application patterns
        /// </summary>
        /// <returns>DaoResult indicating database health status</returns>
        private static DaoResult ValidateDatabaseHealthWithStoredProcedure()
        {
            try
            {
                // First attempt: Try using existing stored procedure if available
                var healthResult = AttemptStoredProcedureHealthCheck();
                if (healthResult.IsSuccess)
                {
                    return healthResult;
                }

                // Fallback: Basic connectivity test using helper pattern
                return ValidateBasicConnectivityWithHelper();
            }
            catch (MySqlException ex)
            {
                string userMessage = GetDatabaseConnectionErrorMessage(ex);
                ShowDatabaseConnectionError(ex);
                return DaoResult.Failure(userMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMsg = $"Database health validation failed: {ex.Message}";
                LoggingUtility.LogApplicationError(ex);
                return DaoResult.Failure(errorMsg, ex);
            }
        }

        /// <summary>
        /// Attempt to use stored procedure for health check if available
        /// </summary>
        /// <returns>DaoResult indicating stored procedure health check result</returns>
        private static DaoResult AttemptStoredProcedureHealthCheck()
        {
            try
            {
                LoggingUtility.Log("[Startup] Attempting stored procedure health check");
                
                // Use a basic connectivity test first
                using var connection = new MySqlConnection(Model_AppVariables.ConnectionString);
                connection.Open();
                
                // Test if critical stored procedures exist
                const string checkProcedureQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.ROUTINES 
                    WHERE ROUTINE_SCHEMA = DATABASE() 
                    AND ROUTINE_NAME IN ('sys_GetUserAccessType', 'sys_SetUserAccessType')";
                
                using var command = new MySqlCommand(checkProcedureQuery, connection);
                var procedureCount = Convert.ToInt32(command.ExecuteScalar());
                
                if (procedureCount >= 1)
                {
                    LoggingUtility.Log($"[Startup] Found {procedureCount} critical stored procedures");
                    return DaoResult.Success("Critical stored procedures are available");
                }
                else
                {
                    LoggingUtility.Log("[Startup] Warning: Critical stored procedures may not be deployed");
                    // Don't fail startup - let the application handle this gracefully
                    return DaoResult.Success("Database accessible but stored procedures may need deployment");
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.Log($"[Startup] Stored procedure health check failed: {ex.Message}");
                // This is not critical - fallback to basic connectivity
                return DaoResult.Failure("Stored procedure check failed, will attempt basic connectivity", ex);
            }
        }

        /// <summary>
        /// Basic connectivity validation using helper-consistent patterns
        /// </summary>
        /// <returns>DaoResult indicating basic connectivity status</returns>
        private static DaoResult ValidateBasicConnectivityWithHelper()
        {
            try
            {
                LoggingUtility.Log("[Startup] Performing basic connectivity validation");
                
                // Build connection with appropriate timeouts for startup validation
                var connectionStringBuilder = new MySqlConnectionStringBuilder(Model_AppVariables.ConnectionString)
                {
                    ConnectionTimeout = 10, // 10 second timeout for startup validation
                    DefaultCommandTimeout = 10
                };
                
                using var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
                connection.Open();
                
                // Test basic query to ensure database is functional
                using var command = new MySqlCommand("SELECT VERSION() as mysql_version", connection);
                var version = command.ExecuteScalar();
                
                if (version != null)
                {
                    LoggingUtility.Log($"[Startup] Database connectivity confirmed. MySQL version: {version}");
                    return DaoResult.Success($"Database connectivity verified. MySQL version: {version}");
                }
                else
                {
                    string errorMsg = "Database connection established but version query returned null";
                    LoggingUtility.Log($"[Startup] {errorMsg}");
                    return DaoResult.Failure(errorMsg);
                }
            }
            catch (MySqlException ex)
            {
                string errorMsg = GetDatabaseConnectionErrorMessage(ex);
                LoggingUtility.LogDatabaseError(ex);
                return DaoResult.Failure(errorMsg, ex);
            }
            catch (Exception ex)
            {
                string errorMsg = $"Basic connectivity validation failed: {ex.Message}";
                LoggingUtility.LogApplicationError(ex);
                return DaoResult.Failure(errorMsg, ex);
            }
        }

        /// <summary>
        /// Validate connection string format before attempting connection
        /// </summary>
        /// <returns>DaoResult indicating validation success or failure</returns>
        private static DaoResult ValidateConnectionStringFormat()
        {
            try
            {
                var connectionString = Model_AppVariables.ConnectionString;
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    const string errorMsg = "Database connection string is not configured";
                    LoggingUtility.Log($"[Startup] {errorMsg}");
                    
                    ShowDatabaseError("Configuration Error", 
                        "Database connection string is not configured.\n\n" +
                        "Please contact your system administrator to configure the database connection.");
                    
                    return DaoResult.Failure(errorMsg);
                }

                // Basic connection string validation
                if (!connectionString.Contains("SERVER=") || !connectionString.Contains("DATABASE="))
                {
                    const string errorMsg = "Database connection string is invalid or incomplete";
                    LoggingUtility.Log($"[Startup] {errorMsg}");
                    
                    ShowDatabaseError("Configuration Error", 
                        "Database connection string is invalid.\n\n" +
                        "Please contact your system administrator to verify the database configuration.");
                    
                    return DaoResult.Failure(errorMsg);
                }

                LoggingUtility.Log("[Startup] Connection string format validation passed");
                return DaoResult.Success("Connection string format is valid");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                ShowDatabaseError("Configuration Error", 
                    $"Error validating database configuration:\n\n{ex.Message}");
                
                return DaoResult.Failure($"Connection string validation error: {ex.Message}", ex);
            }
        }

        #endregion

        #region Error Message Generation

        /// <summary>
        /// Get comprehensive database connection error message for user display
        /// </summary>
        /// <param name="ex">MySQL exception</param>
        /// <returns>User-friendly error message with actionable guidance</returns>
        private static string GetDatabaseConnectionErrorMessage(MySqlException ex)
        {
            if (ex.Message.Contains("Unable to connect to any of the specified MySQL hosts"))
            {
                return "Cannot connect to the database server.\n\n" +
                       "This usually means:\n" +
                       "• The database server is not running\n" +
                       "• The server address or port is incorrect\n" +
                       "• A firewall is blocking the connection\n\n" +
                       "Please check with your system administrator or verify the server is running.\n\n" +
                       "The application cannot start without a database connection.";
            }
            else if (ex.Message.Contains("Access denied"))
            {
                return "Access denied when connecting to the database.\n\n" +
                       "This usually means:\n" +
                       "• Your username or password is incorrect\n" +
                       "• Your account doesn't have permission to access the database\n\n" +
                       "Please check your credentials with your system administrator.\n\n" +
                       "The application cannot start without proper database access.";
            }
            else if (ex.Message.Contains("timeout") || ex.Message.Contains("Connection timeout"))
            {
                return "Connection to the database timed out.\n\n" +
                       "This usually means:\n" +
                       "• The database server is responding slowly\n" +
                       "• Network connectivity issues\n" +
                       "• The server is overloaded\n\n" +
                       "Please try starting the application again in a few moments.\n" +
                       "If the problem persists, contact your system administrator.";
            }
            else
            {
                return $"Database connection failed with the following error:\n\n{ex.Message}\n\n" +
                       "Please contact your system administrator for assistance.\n\n" +
                       "The application cannot start without a database connection.";
            }
        }

        #endregion

        #region UI Error Display

        /// <summary>
        /// Show user-friendly database connection error message
        /// </summary>
        /// <param name="ex">MySQL exception</param>
        private static void ShowDatabaseConnectionError(MySqlException ex)
        {
            string userMessage = GetDatabaseConnectionErrorMessage(ex);
            string title = "Database Connection Failed";
            MessageBox.Show(userMessage, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Show generic database error message
        /// </summary>
        /// <param name="title">Error dialog title</param>
        /// <param name="message">Error message</param>
        private static void ShowDatabaseError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Show fatal application error message
        /// </summary>
        /// <param name="title">Error dialog title</param>
        /// <param name="message">Error message</param>
        private static void ShowFatalError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Perform application cleanup operations
        /// </summary>
        private static void PerformAppCleanup()
        {
            try
            {
                LoggingUtility.Log("[Cleanup] Starting application cleanup");
                
                // Clean up temporary files created by Control_About
                Control_About.CleanupAllTempFiles();
                
                LoggingUtility.Log("[Cleanup] Application cleanup completed successfully");
            }
            catch (Exception ex)
            {
                // Don't let cleanup errors crash the application
                Console.WriteLine($"[Cleanup Warning] Error during application cleanup: {ex.Message}");
                LoggingUtility.LogApplicationError(ex);
            }
        }

        #endregion
    }

    #region Startup Application Context

    public class StartupSplashApplicationContext : ApplicationContext
    {
        #region Fields

        private SplashScreenForm? _splashScreen;
        private MainForm? _mainForm;

        #endregion

        #region Constructors

        public StartupSplashApplicationContext()
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
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    nameof(StartupSplashApplicationContext));
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
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(SplashScreen_FormClosed));
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

                // 1. Initialize logging
                progress = 5;
                _splashScreen?.UpdateProgress(progress, "Initializing logging...");
                await LoggingUtility.InitializeLoggingAsync();
                progress = 10;
                _splashScreen?.UpdateProgress(progress, "Logging initialized.");
                LoggingUtility.Log("[Splash] Logging system initialized");
                await Task.Delay(50);

                // 2. Clean up old logs
                progress = 15;
                _splashScreen?.UpdateProgress(progress, "Cleaning up old logs...");
                await LoggingUtility.CleanUpOldLogsIfNeededAsync();
                progress = 20;
                _splashScreen?.UpdateProgress(progress, "Old logs cleaned up.");
                LoggingUtility.Log("[Splash] Log cleanup completed");
                await Task.Delay(50);

                // 3. Clean app data folders
                progress = 25;
                _splashScreen?.UpdateProgress(progress, "Wiping app data folders...");
                await Task.Run(() => Service_OnStartup_AppDataCleaner.WipeAppDataFolders());
                progress = 30;
                _splashScreen?.UpdateProgress(progress, "App data folders wiped.");
                LoggingUtility.Log("[Splash] App data cleanup completed");
                await Task.Delay(50);

                // 4. Verify database connectivity using helper patterns
                progress = 35;
                _splashScreen?.UpdateProgress(progress, "Verifying database connectivity...");
                var connectivityResult = await VerifyDatabaseConnectivityWithHelperAsync();
                if (!connectivityResult.IsSuccess)
                {
                    LoggingUtility.Log($"[Splash] Database connectivity verification failed: {connectivityResult.StatusMessage}");
                    _splashScreen?.Close();
                    return;
                }
                progress = 40;
                _splashScreen?.UpdateProgress(progress, "Database connectivity verified.");
                LoggingUtility.Log("[Splash] Database connectivity verified during startup");
                await Task.Delay(50);

                // 5. Setup data tables
                progress = 45;
                _splashScreen?.UpdateProgress(progress, "Setting up Data Tables...");
                await Helper_UI_ComboBoxes.SetupDataTables();
                progress = 50;
                _splashScreen?.UpdateProgress(progress, "Data Tables set up.");
                LoggingUtility.Log("[Splash] Data tables setup completed");
                await Task.Delay(50);

                // 6. Initialize version checker
                progress = 60;
                _splashScreen?.UpdateProgress(progress, "Initializing version checker...");
                Service_Timer_VersionChecker.Initialize();
                progress = 65;
                _splashScreen?.UpdateProgress(progress, "Version checker initialized.");
                LoggingUtility.Log("[Splash] Version checker initialized");
                await Task.Delay(50);

                // 7. Initialize theme system
                progress = 70;
                _splashScreen?.UpdateProgress(progress, "Initializing theme system...");
                await Core_Themes.Core_AppThemes.InitializeThemeSystemAsync(Model_AppVariables.User);
                progress = 75;
                _splashScreen?.UpdateProgress(progress, "Theme system initialized.");
                LoggingUtility.Log("[Splash] Theme system initialized");
                await Task.Delay(50);

                // 8. Load user context
                progress = 80;
                _splashScreen?.UpdateProgress(progress, $"User Full Name loaded: {Model_AppVariables.User}");
                LoggingUtility.Log($"[Splash] User context loaded: {Model_AppVariables.User}");
                await Task.Delay(50);

                // 9. Load theme settings using DaoResult patterns
                progress = 85;
                _splashScreen?.UpdateProgress(progress, "Loading theme settings...");
                await LoadThemeSettingsAsync();
                progress = 90;
                _splashScreen?.UpdateProgress(progress, "Theme settings loaded.");
                LoggingUtility.Log("[Splash] Theme settings loaded");
                await Task.Delay(50);

                // 10. Complete core startup
                progress = 93;
                _splashScreen?.UpdateProgress(progress, "Startup sequence completed.");
                LoggingUtility.Log("[Splash] Core startup sequence completed");
                await Task.Delay(100);

                // 11. Create main form
                progress = 95;
                _splashScreen?.UpdateProgress(progress, "Creating main form...");
                await Task.Delay(200);
                _mainForm = new MainForm();
                _mainForm.FormClosed += (s, e) => ExitThread();
                LoggingUtility.Log("[Splash] MainForm created");

                // 12. Configure form instances
                progress = 97;
                _splashScreen?.UpdateProgress(progress, "Configuring form instances...");
                ConfigureFormInstances();
                LoggingUtility.Log("[Splash] Form instances configured");

                // 13. Apply theme
                progress = 99;
                _splashScreen?.UpdateProgress(progress, "Applying theme...");
                ApplyThemeToMainForm();
                LoggingUtility.Log("[Splash] Theme applied to MainForm");

                // 14. Show application
                progress = 100;
                _splashScreen?.UpdateProgress(progress, "Ready to start!");
                await Task.Delay(500);
                
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
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(RunStartupAsync));
                
                string userMessage = "A critical error occurred during application startup:\n\n" +
                                   $"{ex.Message}\n\n" +
                                   "The application will now close. Please check the log files for more details " +
                                   "and contact your system administrator if the problem persists.";
                
                MessageBox.Show(userMessage, @"Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _splashScreen?.Close();
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
                MessageBox.Show(userMessage, "Database Connection Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return DaoResult.Failure(userMessage, ex);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                
                string userMessage = "Unable to verify database connectivity during startup:\n\n" +
                                   $"{ex.Message}\n\n" +
                                   "Please check your network connection and database server status.\n" +
                                   "The application cannot start without database access.";
                
                MessageBox.Show(userMessage, "Database Connection Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
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

    #endregion
}
