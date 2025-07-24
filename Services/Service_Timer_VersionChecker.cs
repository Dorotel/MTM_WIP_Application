using System.Data;
using System.Diagnostics;
using System.Timers;
using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using Timer = System.Timers.Timer;

namespace MTM_Inventory_Application.Services
{
    internal static class Service_Timer_VersionChecker
    {
        #region Fields

        private static readonly Timer VersionTimer = new(30000);

        #endregion

        #region Properties

        public static string? LastCheckedDatabaseVersion { get; private set; }
        public static MainForm? MainFormInstance { get; set; }
        public static Control_InventoryTab? ControlInventoryInstance { get; set; }

        #endregion

        #region Public Methods

        public static void Initialize()
        {
            try
            {
                VersionTimer.Elapsed += VersionChecker;
                VersionTimer.Enabled = true;
                VersionTimer.AutoReset = true;
                VersionTimer.Start();
                Debug.WriteLine("VersionTimer initialized and started.");
                LoggingUtility.Log("VersionTimer initialized and started.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing VersionTimer: {ex.Message}");
                LoggingUtility.Log($"Error initializing VersionTimer: {ex.Message}");
            }

            VersionChecker(null, null);
        }

        public static async void VersionChecker(object? sender, ElapsedEventArgs? e)
        {
            Debug.WriteLine("Running VersionChecker...");
            LoggingUtility.Log("Running VersionChecker...");
            try
            {
                Helper_Database_Core helper =
                    new(Helper_Database_Variables.GetConnectionString(null, null, null, null));
                DataTable dt = await helper.ExecuteDataTable("log_changelog_Get_Current", null, true,
                    CommandType.StoredProcedure);
                if (dt.Rows.Count == 0)
                {
                    return;
                }

                string? databaseVersion = dt.Rows[0]["Version"]?.ToString();
                LastCheckedDatabaseVersion = databaseVersion;
                Debug.WriteLine(LastCheckedDatabaseVersion);
                ControlInventoryInstance?.SetVersionLabel(Model_AppVariables.UserVersion, databaseVersion ?? "Unknown");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                Service_OnEvent_ExceptionHandler.HandleDatabaseError();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogDatabaseError(ex);
                MessageBox.Show(@"An error occurred in VersionChecker:\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
