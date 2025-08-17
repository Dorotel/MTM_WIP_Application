using System.Diagnostics;
using MTM_Inventory_Application.Controls.SettingsForm;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.ErrorDialog;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
using MySql.Data.MySqlClient;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

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
                // Initialize debugging system first (before any other operations)
                #if DEBUG
                Service_DebugTracer.Initialize(DebugLevel.High);
                Service_DebugConfiguration.InitializeDefaults();
                Service_DebugConfiguration.SetDevelopmentMode();
                #else
                Service_DebugTracer.Initialize(DebugLevel.Medium);
                Service_DebugConfiguration.InitializeDefaults();
                #endif

                Service_DebugTracer.TraceUIAction("APPLICATION_STARTUP", "Program", new Dictionary<string, object>
                {
                    ["StartupTime"] = DateTime.Now,
                    ["IsDebugMode"] = Debugger.IsAttached,
                    ["OSVersion"] = Environment.OSVersion.ToString(),
                    ["ProcessorCount"] = Environment.ProcessorCount,
                    ["WorkingSet"] = Environment.WorkingSet
                });

                // Global exception handling setup with enhanced database error handling
                Application.ThreadException += (sender, args) =>
                {
                    Console.WriteLine($"[Global Exception] ThreadException: {args.Exception}");
                    Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
                    {
                        ["ExceptionType"] = args.Exception.GetType().Name,
                        ["ExceptionMessage"] = args.Exception.Message
                    }, "ThreadExceptionHandler", "Program");
                    
                    try
                    {
                        LoggingUtility.LogApplicationError(args.Exception);
                    }
                    catch (Exception loggingEx)
                    {
                        // If logging fails, at least show console output
                        Console.WriteLine($"[Critical] Failed to log thread exception: {loggingEx.Message}");
                    }
                    HandleGlobalException(args.Exception);
                };

                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    if (args.ExceptionObject is Exception ex)
                    {
                        Console.WriteLine($"[Global Exception] UnhandledException: {ex}");
                        Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
                        {
                            ["ExceptionType"] = ex.GetType().Name,
                            ["ExceptionMessage"] = ex.Message,
                            ["IsTerminating"] = args.IsTerminating
                        }, "UnhandledExceptionHandler", "Program");
                        
                        try
                        {
                            LoggingUtility.LogApplicationError(ex);
                        }
                        catch (Exception loggingEx)
                        {
                            // If logging fails, at least show console output
                            Console.WriteLine($"[Critical] Failed to log unhandled exception: {loggingEx.Message}");
                        }
                        HandleGlobalException(ex);
                    }
                };

                // Register cleanup handler for application exit
                AppDomain.CurrentDomain.ProcessExit += (sender, args) => PerformAppCleanup();
                Application.ApplicationExit += (sender, args) => PerformAppCleanup();

                // Windows Forms initialization with error handling
                try
                {
                    Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
                    ApplicationConfiguration.Initialize();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Startup Error] Windows Forms initialization failed: {ex.Message}");
                    ShowFatalError("Windows Forms Initialization Error",
                        $"Failed to initialize Windows Forms:\n\n{ex.Message}\n\n" +
                        "This may be caused by:\n" +
                        "• Missing .NET runtime components\n" +
                        "• System display configuration issues\n" +
                        "• Insufficient system permissions\n\n" +
                        "Please contact your system administrator.");
                    return;
                }

                try
                {
                    LoggingUtility.Log("[Startup] Application initialization started");
                }
                catch (Exception ex)
                {
                    // If initial logging fails, continue but show warning
                    Console.WriteLine($"[Warning] Initial logging failed: {ex.Message}");
                    ShowNonCriticalError("Logging System Warning",
                        $"The logging system encountered an issue during startup:\n\n{ex.Message}\n\n" +
                        "The application will continue, but some log entries may be missing.\n" +
                        "Please check file system permissions and disk space.");
                }

                // User identification with error handling
                try
                {
                    Model_AppVariables.User = Dao_System.System_GetUserName();
                    //Model_AppVariables.User = "TestUser"; // TEMPORARY OVERRIDE FOR TESTING - REMOVE IN PRODUCTION


                    LoggingUtility.Log($"[Startup] User identified: {Model_AppVariables.User}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Startup Error] User identification failed: {ex.Message}");
                    LoggingUtility.LogApplicationError(ex);

                    // Use fallback user identification
                    try
                    {
                        Model_AppVariables.User = Environment.UserName ?? "Unknown";
                        LoggingUtility.Log($"[Startup] Using fallback user identification: {Model_AppVariables.User}");

                        ShowNonCriticalError("User Identification Warning",
                            $"Could not identify user through normal methods:\n\n{ex.Message}\n\n" +
                            $"Using system username '{Model_AppVariables.User}' as fallback.\n" +
                            "Some user-specific features may not work correctly.");
                    }
                    catch (Exception fallbackEx)
                    {
                        Model_AppVariables.User = "Unknown";
                        LoggingUtility.LogApplicationError(fallbackEx);
                        ShowNonCriticalError("User Identification Error",
                            "Failed to identify current user. Using 'Unknown' as username.\n" +
                            "This may affect user-specific settings and permissions.");
                    }
                }

                // ENHANCED: Validate database connectivity using helper patterns BEFORE creating any forms
                var connectivityResult = ValidateDatabaseConnectivityWithHelper();
                if (!connectivityResult.IsSuccess)
                {
                    LoggingUtility.Log($"[Startup] Database connectivity validation failed: {connectivityResult.StatusMessage}");

                    // DEBUG: Output exactly what's in the status message
                    Console.WriteLine($"[DEBUG] connectivityResult.StatusMessage: '{connectivityResult.StatusMessage}'");
                    Console.WriteLine($"[DEBUG] connectivityResult.ErrorMessage: '{connectivityResult.ErrorMessage}'");

                    // Use ErrorMessage if StatusMessage is generic
                    string errorMessage = !string.IsNullOrEmpty(connectivityResult.StatusMessage) && 
                                        connectivityResult.StatusMessage != "Database connectivity validation failed"
                                        ? connectivityResult.StatusMessage 
                                        : connectivityResult.ErrorMessage;

                    // FIXED: Use a more specific title based on the error type
                    string dialogTitle = "Database Connection Failed";
                    if (errorMessage.Contains("does not exist"))
                    {
                        dialogTitle = "Database Does Not Exist";
                    }
                    else if (errorMessage.Contains("Cannot connect"))
                    {
                        dialogTitle = "Database Server Unavailable";
                    }
                    else if (errorMessage.Contains("Access denied"))
                    {
                        dialogTitle = "Database Access Denied";
                    }
                    else if (errorMessage.Contains("timeout"))
                    {
                        dialogTitle = "Database Connection Timeout";
                    }

                    var dialogResult = MessageBox.Show(
                        errorMessage + "\n\n" +
                        "Click 'Retry' to try connecting again, or 'Cancel' to exit the application.",
                        dialogTitle,
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error);

                    if (dialogResult == DialogResult.Retry)
                    {
                        // Recursive call to try again
                        Main();
                        return;
                    }
                    else
                    {
                        // User chose to exit
                        LoggingUtility.Log("[Startup] User chose to exit after database connection failure");
                        return;
                    }
                }

                LoggingUtility.Log("[Startup] Database connectivity validated successfully");

                // Load user access permissions with error handling
                try
                {
                    _ = Dao_System.System_UserAccessTypeAsync(true);
                }
                catch (MySqlException ex)
                {
                    LoggingUtility.LogDatabaseError(ex);
                    string userMessage = GetDatabaseConnectionErrorMessage(ex);

                    // FIXED: Show error with retry option instead of immediate exit
                    var dialogResult = MessageBox.Show(
                        userMessage + "\n\n" +
                        "Click 'Retry' to try again, or 'Cancel' to exit the application.",
                        "User Access Loading Failed",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error);

                    if (dialogResult == DialogResult.Retry)
                    {
                        Main();
                        return;
                    }
                    else
                    {
                        LoggingUtility.Log("[Startup] User chose to exit after user access loading failure");
                        return;
                    }
                }
                catch (TimeoutException ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    var dialogResult = MessageBox.Show(
                        "The request to load user access permissions timed out.\n\n" +
                        "This usually means:\n" +
                        "• The database server is responding slowly\n" +
                        "• Network connectivity issues\n" +
                        "• The server is overloaded\n\n" +
                        "Click 'Retry' to try again, or 'Cancel' to exit the application.",
                        "User Access Timeout",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Exclamation);

                    if (dialogResult == DialogResult.Retry)
                    {
                        Main();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    ShowSecurityError("Access Denied",
                        $"Access denied while loading user permissions:\n\n{ex.Message}\n\n" +
                        "This usually means:\n" +
                        "• Your account doesn't have sufficient database permissions\n" +
                        "• The user account configuration is incorrect\n\n" +
                        "Please contact your system administrator.\n\n" +
                        "The application will now exit.");
                    return;
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    ShowFatalError("User Access Error",
                        $"Unable to load user access permissions:\n\n{ex.Message}\n\n" +
                        "Error Type: {ex.GetType().Name}\n\n" +
                        "This is required for application security and cannot be bypassed.\n" +
                        "Please contact your system administrator if this problem persists.\n\n" +
                        "The application will now exit.");
                    return;
                }

                // Trace setup with error handling
                try
                {
                    Trace.Listeners.Clear();
                    Trace.Listeners.Add(new DefaultTraceListener());
                    Trace.AutoFlush = true;
                    Console.WriteLine("[Trace] [Main] Application starting...");
                    Trace.WriteLine("[Trace] [Main] Application starting...");
                }
                catch (Exception ex)
                {
                    // Trace setup failure is not critical, but log it
                    LoggingUtility.LogApplicationError(ex);
                    Console.WriteLine($"[Warning] Trace setup failed: {ex.Message}");
                }

                // Start application with splash screen
                try
                {
                    Application.Run(new Service_Onstartup_StartupSplashApplicationContext());
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("Application.Run"))
                {
                    LoggingUtility.LogApplicationError(ex);
                    ShowFatalError("Application Startup Error",
                        "Failed to start the main application interface.\n\n" +
                        "This usually means:\n" +
                        "• Another instance may already be running\n" +
                        "• System resources are insufficient\n" +
                        "• Display configuration issues\n\n" +
                        "Please restart your computer and try again.");
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    HandleGlobalException(ex);
                }

                Console.WriteLine("[Trace] [Main] Application exiting Main().");
                Trace.WriteLine("[Trace] [Main] Application exiting Main().");
                LoggingUtility.Log("[Startup] Application shutdown completed");
            }
            catch (OutOfMemoryException ex)
            {
                // Critical system error - minimal logging to avoid further memory issues
                Console.WriteLine($"[Critical] Out of memory: {ex.Message}");
                try
                {
                    Service_ErrorHandler.HandleException(
                        new OutOfMemoryException(
                            "The application has run out of available memory and must close.\n\n" +
                            "This usually means:\n" +
                            "• Insufficient system RAM\n" +
                            "• Memory leak in application or system\n" +
                            "• Too many applications running\n\n" +
                            "Please restart your computer and close unnecessary applications."),
                        ErrorSeverity.Fatal,
                        controlName: "Program_Main");
                }
                catch
                {
                    // If we can't even show a message box, exit immediately
                    Environment.Exit(1);
                }
            }
            catch (StackOverflowException)
            {
                // Stack overflow - can't do much, just exit
                Console.WriteLine("[Critical] Stack overflow occurred");
                Environment.Exit(1);
            }
            catch (AccessViolationException ex)
            {
                Console.WriteLine($"[Critical] Access violation: {ex.Message}");
                try
                {
                    MessageBox.Show(
                        "A critical system error occurred (Access Violation).\n\n" +
                        "This usually indicates:\n" +
                        "• Memory corruption\n" +
                        "• System instability\n" +
                        "• Hardware issues\n\n" +
                        "Please restart your computer immediately.",
                        "Critical System Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                }
                catch
                {
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Global Exception] Main catch: {ex}");
                try
                {
                    LoggingUtility.LogApplicationError(ex);
                }
                catch
                {
                    Console.WriteLine("[Critical] Failed to log main exception");
                }
                HandleGlobalException(ex);
            }
            finally
            {
                // Ensure cleanup runs even if exceptions occur
                PerformAppCleanup();
            }
        }

        #endregion

        #region Global Exception Handling

        /// <summary>
        /// Handle global exceptions with user-friendly messaging
        /// </summary>
        /// <param name="ex">Exception to handle</param>
        private static void HandleGlobalException(Exception ex)
        {
            try
            {
                if (ex is MySqlException mysqlEx)
                {
                    string userMessage = GetDatabaseConnectionErrorMessage(mysqlEx);

                    var dialogResult = MessageBox.Show(
                        userMessage + "\n\n" +
                        "Click 'Retry' to try connecting again, or 'OK' to exit the application.",
                        "Database Error",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error);

                    if (dialogResult == DialogResult.Retry)
                    {
                        Main();
                    }
                }
                else if (ex is InvalidOperationException && ex.Message.Contains("database"))
                {
                    ShowDatabaseError("Database Configuration Error",
                        $"Database configuration error:\n\n{ex.Message}\n\n" +
                        "Please contact your system administrator for assistance.");
                }
                else if (ex is FileNotFoundException fileEx)
                {
                    ShowFileSystemError("Missing File Error",
                        $"A required file could not be found:\n\n{fileEx.FileName ?? "Unknown file"}\n\n" +
                        $"Error: {fileEx.Message}\n\n" +
                        "Please reinstall the application or contact your system administrator.");
                }
                else if (ex is DirectoryNotFoundException dirEx)
                {
                    ShowFileSystemError("Missing Directory Error",
                        $"A required directory could not be found:\n\n{dirEx.Message}\n\n" +
                        "Please reinstall the application or contact your system administrator.");
                }
                else if (ex is UnauthorizedAccessException accessEx)
                {
                    ShowSecurityError("Access Denied",
                        $"Access was denied:\n\n{accessEx.Message}\n\n" +
                        "This usually means:\n" +
                        "• Insufficient file system permissions\n" +
                        "• Security software is blocking the application\n" +
                        "• User account restrictions\n\n" +
                        "Please contact your system administrator.");
                }
                else if (ex is SecurityException secEx)
                {
                    ShowSecurityError("Security Error",
                        $"A security error occurred:\n\n{secEx.Message}\n\n" +
                        "This usually means:\n" +
                        "• Application lacks required permissions\n" +
                        "• Security policy restrictions\n" +
                        "• Certificate or signature issues\n\n" +
                        "Please contact your system administrator.");
                }
                else if (ex is COMException comEx)
                {
                    ShowSystemError("System Component Error",
                        $"A system component error occurred:\n\n{comEx.Message}\n\n" +
                        $"Error Code: 0x{comEx.HResult:X8}\n\n" +
                        "This usually means:\n" +
                        "• Missing system components\n" +
                        "• Corrupted system files\n" +
                        "• Compatibility issues\n\n" +
                        "Please contact your system administrator.");
                }
                else if (ex is ExternalException extEx)
                {
                    ShowSystemError("External System Error",
                        $"An external system error occurred:\n\n{extEx.Message}\n\n" +
                        $"Error Code: 0x{extEx.ErrorCode:X8}\n\n" +
                        "Please contact your system administrator.");
                }
                else if (ex is TimeoutException)
                {
                    ShowTimeoutError("Operation Timeout",
                        $"An operation timed out:\n\n{ex.Message}\n\n" +
                        "This usually means:\n" +
                        "• Network connectivity issues\n" +
                        "• Server is overloaded or unresponsive\n" +
                        "• System performance issues\n\n" +
                        "Please try again or contact your system administrator.");
                }
                else
                {
                    ShowFatalError("Application Error",
                        $"An unexpected error occurred:\n\n{ex.Message}\n\n" +
                        $"Error Type: {ex.GetType().Name}\n\n" +
                        "Please check the log files and contact your system administrator if needed.\n\n" +
                        "The application will now exit.");
                }
            }
            catch (Exception innerEx)
            {
                // Fallback error handling
                Console.WriteLine($"Error in global exception handler: {innerEx.Message}");
                try
                {
                    MessageBox.Show($"A critical error occurred and could not be handled properly.\n\n" +
                                   $"Original error: {ex.Message}\n" +
                                   $"Handler error: {innerEx.Message}\n\n" +
                                   "The application will now exit.",
                        "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                catch
                {
                    // Final fallback - just exit
                    Environment.Exit(1);
                }
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
            catch (MySqlException ex)
            {
                string errorMsg = GetDatabaseConnectionErrorMessage(ex);
                Console.WriteLine($"[Startup] MySQL Error: {errorMsg}");
                LoggingUtility.LogDatabaseError(ex);

                return DaoResult.Failure(errorMsg, ex);
            }
            catch (Exception ex)
            {
                string errorMsg = $"Database connectivity validation failed: {ex.Message}";
                Console.WriteLine($"[Startup] {errorMsg}");
                LoggingUtility.LogApplicationError(ex);

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
                LoggingUtility.LogDatabaseError(ex);
                return DaoResult.Failure(userMessage, ex);
            }
            catch (TimeoutException ex)
            {
                string userMessage = "Database connection timed out during health check.\n\n" +
                                   "This usually means:\n" +
                                   "• The database server is responding slowly\n" +
                                   "• Network connectivity issues\n" +
                                   "• Server is overloaded\n\n" +
                                   "Please try again or contact your system administrator.";
                LoggingUtility.LogApplicationError(ex);
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
            catch (MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                return DaoResult.Failure($"MySQL error during health check: {ex.Message}", ex);
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
                    const string errorMsg = "Database connection string is not configured.\n\n" +
                                          "Please contact your system administrator to configure the database connection.";
                    LoggingUtility.Log($"[Startup] {errorMsg}");
                    return DaoResult.Failure(errorMsg);
                }

                // Basic connection string validation
                if (!connectionString.Contains("SERVER=") || !connectionString.Contains("DATABASE="))
                {
                    const string errorMsg = "Database connection string is invalid.\n\n" +
                                          "Please contact your system administrator to verify the database configuration.";
                    LoggingUtility.Log($"[Startup] {errorMsg}");
                    return DaoResult.Failure(errorMsg);
                }

                LoggingUtility.Log("[Startup] Connection string format validation passed");
                return DaoResult.Success("Connection string format is valid");
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error validating database configuration:\n\n{ex.Message}";
                LoggingUtility.LogApplicationError(ex);
                return DaoResult.Failure(errorMsg, ex);
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
            if (ex.Message.Contains("Unknown database"))
            {
                string dbName = Model_Users.Database;
                string serverAddress = Model_Users.WipServerAddress;

#if DEBUG
                return $"The test database '{dbName}' does not exist on server '{serverAddress}'.\n\n" +
                       "This is a DEBUG build that requires the test database.\n\n" +
                       "Please:\n" +
                       "• Create the test database '{dbName}' on MySQL server\n" +
                       "• Or run the application in RELEASE mode to use the production database\n" +
                       "• Contact your system administrator for database setup assistance";
#else
                return $"The database '{dbName}' does not exist on server '{serverAddress}'.\n\n" +
                       "Please contact your system administrator to:\n" +
                       "• Verify the database server is running\n" +
                       "• Ensure the database '{dbName}' exists and is accessible\n" +
                       "• Check database permissions for your user account";
#endif
            }
            else if (ex.Message.Contains("Unable to connect to any of the specified MySQL hosts"))
            {
                return "Cannot connect to the database server.\n\n" +
                       "This usually means:\n" +
                       "• The database server is not running\n" +
                       "• The server address or port is incorrect\n" +
                       "• A firewall is blocking the connection\n\n" +
                       "Please check with your system administrator or verify the server is running.";
            }
            else if (ex.Message.Contains("Access denied"))
            {
                return "Access denied when connecting to the database.\n\n" +
                       "This usually means:\n" +
                       "• Your username or password is incorrect\n" +
                       "• Your account doesn't have permission to access the database\n\n" +
                       "Please check your credentials with your system administrator.";
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
                       "Please contact your system administrator for assistance.";
            }
        }

        #endregion

        #region UI Error Display

        /// <summary>
        /// Show user-friendly database connection error message
        /// </summary>
        /// <param name="title">Error dialog title</param>
        /// <param name="message">Error message</param>
        private static void ShowDatabaseConnectionError(string title, string message)
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Critical] Failed to show database connection error dialog: {ex.Message}");
                Console.WriteLine($"[Critical] Original message: {message}");
            }
        }

        /// <summary>
        /// Show generic database error message
        /// </summary>
        /// <param name="title">Error dialog title</param>
        /// <param name="message">Error message</param>
        private static void ShowDatabaseError(string title, string message)
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Critical] Failed to show database error dialog: {ex.Message}");
                Console.WriteLine($"[Critical] Original message: {message}");
            }
        }

        /// <summary>
        /// Show fatal application error message
        /// </summary>
        /// <param name="title">Error dialog title</param>
        /// <param name="message">Error message</param>
        private static void ShowFatalError(string title, string message)
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Critical] Failed to show fatal error dialog: {ex.Message}");
                Console.WriteLine($"[Critical] Original message: {message}");
            }
        }

        /// <summary>
        /// Show non-critical error message that allows application to continue
        /// </summary>
        /// <param name="title">Error dialog title</param>
        /// <param name="message">Error message</param>
        private static void ShowNonCriticalError(string title, string message)
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Critical] Failed to show non-critical error dialog: {ex.Message}");
                Console.WriteLine($"[Critical] Original message: {message}");
            }
        }

        /// <summary>
        /// Show file system related error message
        /// </summary>
        /// <param name="title">Error dialog title</param>
        /// <param name="message">Error message</param>
        private static void ShowFileSystemError(string title, string message)
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Critical] Failed to show file system error dialog: {ex.Message}");
                Console.WriteLine($"[Critical] Original message: {message}");
            }
        }

        /// <summary>
        /// Show security related error message
        /// </summary>
        /// <param name="title">Error dialog title</param>
        /// <param name="message">Error message</param>
        private static void ShowSecurityError(string title, string message)
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Critical] Failed to show security error dialog: {ex.Message}");
                Console.WriteLine($"[Critical] Original message: {message}");
            }
        }

        /// <summary>
        /// Show system error message
        /// </summary>
        /// <param name="title">Error dialog title</param>
        /// <param name="message">Error message</param>
        private static void ShowSystemError(string title, string message)
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Critical] Failed to show system error dialog: {ex.Message}");
                Console.WriteLine($"[Critical] Original message: {message}");
            }
        }

        /// <summary>
        /// Show timeout error message
        /// </summary>
        /// <param name="title">Error dialog title</param>
        /// <param name="message">Error message</param>
        private static void ShowTimeoutError(string title, string message)
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Critical] Failed to show timeout error dialog: {ex.Message}");
                Console.WriteLine($"[Critical] Original message: {message}");
            }
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
                try
                {
                    Control_About.CleanupAllTempFiles();
                    LoggingUtility.Log("[Cleanup] Control_About temp files cleaned up successfully");
                }
                catch (UnauthorizedAccessException ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    Console.WriteLine($"[Cleanup Warning] Access denied cleaning temp files: {ex.Message}");
                }
                catch (IOException ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    Console.WriteLine($"[Cleanup Warning] IO error cleaning temp files: {ex.Message}");
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    Console.WriteLine($"[Cleanup Warning] Error cleaning Control_About temp files: {ex.Message}");
                }

                // Additional cleanup operations can be added here
                try
                {
                    // Cleanup any remaining database connections
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    LoggingUtility.Log("[Cleanup] Memory cleanup completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Cleanup Warning] Error during memory cleanup: {ex.Message}");
                }

                LoggingUtility.Log("[Cleanup] Application cleanup completed successfully");
            }
            catch (Exception ex)
            {
                // Don't let cleanup errors crash the application during shutdown
                Console.WriteLine($"[Cleanup Warning] Error during application cleanup: {ex.Message}");
                try
                {
                    LoggingUtility.LogApplicationError(ex);
                }
                catch
                {
                    // If even logging fails during cleanup, just output to console
                    Console.WriteLine("[Cleanup Warning] Failed to log cleanup error");
                }
            }
        }

        #endregion
    }
}
