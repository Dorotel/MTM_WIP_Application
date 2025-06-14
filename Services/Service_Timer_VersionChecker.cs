using MTM_WIP_Application.Core;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using MTM_WIP_Application.Controls;
using MTM_WIP_Application.Controls.MainForm;
using Timer = System.Timers.Timer;
using MTM_WIP_Application.Helpers;

namespace MTM_WIP_Application.Services;

internal static class Service_Timer_VersionChecker
{
    #region Fields

    private static readonly Timer VersionTimer = new(30000);

    #endregion

    #region Properties

    public static string? LastCheckedDatabaseVersion { get; private set; }
    public static MainForm? MainFormInstance { get; set; }
    public static ControlInventoryTab? ControlInventoryInstance { get; set; }

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

    public static void VersionChecker(object? sender, ElapsedEventArgs? e)
    {
        Debug.WriteLine("Running VersionChecker...");
        LoggingUtility.Log("Running VersionChecker...");
        MySqlConnection? connection = null;
        MySqlCommand? command = null;
        MySqlDataReader? reader = null;
        try
        {
            connection = new MySqlConnection(Helper_SqlVariables.GetConnectionString(null, null, null, null));
            connection.Open();
            command = new MySqlCommand("log_changelog_Get_Current", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            using (reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var databaseVersion = reader.GetString(reader.GetOrdinal("Version"));
                    LastCheckedDatabaseVersion = databaseVersion;
                    ControlInventoryInstance?.SetVersionLabel(Core_WipAppVariables.UserVersion, databaseVersion);
                    if (Core_WipAppVariables.UserVersion != databaseVersion)
                    {
                        LoggingUtility.Log(
                            $"Version mismatch detected. Current: {Core_WipAppVariables.UserVersion}, Expected: {databaseVersion}");
                        Debug.WriteLine(
                            $"Version mismatch detected. Current: {Core_WipAppVariables.UserVersion}, Expected: {databaseVersion}");
                        Task.Run(() =>
                        {
                            var message = "You are using an older version of the WIP Application.\n" +
                                          "This normally means a newer version is just about to be released.\n" +
                                          "The program will close in 30 seconds, or by clicking OK.";
                            var caption =
                                $"Version Conflict Error ({Core_WipAppVariables.UserVersion}/{databaseVersion})";
                            MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            Application.Exit();
                        });
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            Service_OnEvent_ExceptionHandler.HandleDatabaseError();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            MessageBox.Show(@"An error occurred in VersionChecker:
" + ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            reader?.Close();
            command?.Dispose();
            connection?.Close();
        }
    }

    #endregion
}