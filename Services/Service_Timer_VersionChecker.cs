using System.Data;
using System.Diagnostics;
using System.Timers;
using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Data; // Added for Helper_Database_StoredProcedure
using Timer = System.Timers.Timer;

namespace MTM_Inventory_Application.Services
{
    /// <summary>
    /// Version checking service that periodically checks database for version information
    /// Updated: August 10, 2025 - Compatible with uniform parameter naming system
    /// </summary>
    internal static class Service_Timer_VersionChecker
    {
        #region Fields

        private static readonly Timer VersionTimer = new(30000); // Check every 30 seconds

        #endregion

        #region Properties

        public static string? LastCheckedDatabaseVersion { get; private set; }
        public static MainForm? MainFormInstance { get; set; }
        public static Control_InventoryTab? ControlInventoryInstance { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize the version checking timer service
        /// </summary>
        public static void Initialize()
        {
            try
            {
                VersionTimer.Elapsed += VersionChecker;
                VersionTimer.Enabled = true;
                VersionTimer.AutoReset = true;
                VersionTimer.Start();
                Debug.WriteLine("VersionTimer initialized and started.");
                LoggingUtility.Log("VersionTimer initialized and started successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing VersionTimer: {ex.Message}");
                LoggingUtility.Log($"Error initializing VersionTimer: {ex.Message}");
            }

            // Perform initial version check
            VersionChecker(null, null);
        }

        /// <summary>
        /// Version checker method - updated for uniform parameter naming system
        /// Handles missing stored procedures gracefully and works with new p_ parameter pattern
        /// </summary>
        public static async void VersionChecker(object? sender, ElapsedEventArgs? e)
        {
            Debug.WriteLine("Running VersionChecker...");
            LoggingUtility.Log("Running VersionChecker - checking database version information.");
            
            try
            {
                // Updated to work with uniform parameter naming system (p_ prefixes)
                // The log_changelog_Get_Current procedure now uses p_Status and p_ErrorMsg output parameters
                var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Helper_Database_Variables.GetConnectionString(null, null, null, null),
                    "log_changelog_Get_Current",
                    null, // No input parameters needed for this procedure
                    null, // No progress helper for background service
                    true  // Use async execution
                );
                
                // Handle successful execution
                if (dataResult.IsSuccess && dataResult.Data != null && dataResult.Data.Rows.Count > 0)
                {
                    // Extract version information from first row
                    string? databaseVersion = dataResult.Data.Rows[0]["Version"]?.ToString();
                    LastCheckedDatabaseVersion = databaseVersion ?? "Unknown Version";
                    
                    Debug.WriteLine($"Database version retrieved: {LastCheckedDatabaseVersion}");
                    LoggingUtility.Log($"Version check successful - Database version: {LastCheckedDatabaseVersion}");
                    
                    // Update UI with version information (thread-safe)
                    UpdateVersionLabel(Model_AppVariables.UserVersion, LastCheckedDatabaseVersion);
                    return;
                }
                
                // Handle case where procedure succeeds but returns no data
                if (dataResult.IsSuccess && (dataResult.Data == null || dataResult.Data.Rows.Count == 0))
                {
                    LoggingUtility.Log("VersionChecker: log_changelog_Get_Current returned no data - no changelog entries exist yet.");
                    LastCheckedDatabaseVersion = "No Version Data";
                    UpdateVersionLabel(Model_AppVariables.UserVersion, "No Version Data");
                    return;
                }
                
                // Handle warning status (Status = 1)
                if (dataResult.Status == 1)
                {
                    LoggingUtility.Log($"VersionChecker: Warning from stored procedure - {dataResult.ErrorMessage}");
                    LastCheckedDatabaseVersion = "Database Version Warning";
                    UpdateVersionLabel(Model_AppVariables.UserVersion, "Database Version Warning");
                    return;
                }
                
                // Handle error status (Status = -1)
                LoggingUtility.Log($"VersionChecker: Stored procedure returned error - {dataResult.ErrorMessage}");
                LastCheckedDatabaseVersion = "Database Version Error";
                UpdateVersionLabel(Model_AppVariables.UserVersion, "Database Version Error");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1305) // Procedure doesn't exist
            {
                LoggingUtility.Log("VersionChecker: log_changelog_Get_Current stored procedure not found. This is normal during development - procedure may not be deployed yet.");
                // TEMPORARY: More descriptive message to help with debugging
                LastCheckedDatabaseVersion = "Deploy Procedures Required";
                UpdateVersionLabel(Model_AppVariables.UserVersion, "Deploy Procedures Required");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1054) // Column doesn't exist  
            {
                LoggingUtility.Log($"VersionChecker: Column not found in log_changelog table - {ex.Message}. This may indicate the table structure needs updating.");
                LastCheckedDatabaseVersion = "Database Schema Issue";
                UpdateVersionLabel(Model_AppVariables.UserVersion, "Database Schema Issue");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                LoggingUtility.Log($"VersionChecker: Database error occurred - {ex.Message}");
                LastCheckedDatabaseVersion = "Database Connection Error";
                UpdateVersionLabel(Model_AppVariables.UserVersion, "Database Connection Error");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                LoggingUtility.Log($"VersionChecker: General error occurred - {ex.Message}");
                LastCheckedDatabaseVersion = "Version Check Error";
                UpdateVersionLabel(Model_AppVariables.UserVersion, "Version Check Error");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Thread-safe method to update version label on UI
        /// </summary>
        /// <param name="appVersion">Application version string</param>
        /// <param name="dbVersion">Database version string</param>
        private static void UpdateVersionLabel(string appVersion, string dbVersion)
        {
            try
            {
                // Update Control_InventoryTab version label if available
                if (ControlInventoryInstance != null)
                {
                    if (ControlInventoryInstance.InvokeRequired)
                    {
                        ControlInventoryInstance.Invoke(new Action(() => 
                            ControlInventoryInstance.SetVersionLabel(appVersion, dbVersion)));
                    }
                    else
                    {
                        ControlInventoryInstance.SetVersionLabel(appVersion, dbVersion);
                    }
                }
                
                Debug.WriteLine($"Version labels updated - App: {appVersion}, DB: {dbVersion}");
            }
            catch (Exception ex)
            {
                LoggingUtility.Log($"VersionChecker: Error updating version label - {ex.Message}");
                Debug.WriteLine($"Error updating version label: {ex.Message}");
            }
        }

        /// <summary>
        /// Stop the version checking service
        /// </summary>
        public static void Stop()
        {
            try
            {
                VersionTimer?.Stop();
                VersionTimer?.Dispose();
                LoggingUtility.Log("VersionTimer stopped and disposed successfully.");
                Debug.WriteLine("VersionTimer stopped and disposed.");
            }
            catch (Exception ex)
            {
                LoggingUtility.Log($"Error stopping VersionTimer: {ex.Message}");
                Debug.WriteLine($"Error stopping VersionTimer: {ex.Message}");
            }
        }

        #endregion
    }
}
