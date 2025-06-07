using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.ErrorHandling;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace MTM_WIP_Application.Services;

internal static class VersionCheckerService
{
    private static readonly Timer VersionTimer = new(30000); // 30 seconds

    // Store the latest database version for Program.cs to access
    public static string? LastCheckedDatabaseVersion { get; private set; }

    // Allow MainForm instance to be set so we can update UI from here if desired
    public static MainForm? MainFormInstance { get; set; }

    public static void Initialize()
    {
        try
        {
            VersionTimer.Elapsed += VersionChecker;
            VersionTimer.Enabled = true;
            VersionTimer.AutoReset = true;
            VersionTimer.Start();

            Debug.WriteLine("VersionTimer initialized and started.");
            AppLogger.Log("VersionTimer initialized and started.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing VersionTimer: {ex.Message}");
            AppLogger.Log($"Error initializing VersionTimer: {ex.Message}");
        }

        // Also do an immediate version check at startup
        VersionChecker(null, null);
    }

    public static void VersionChecker(object? sender, ElapsedEventArgs? e)
    {
        Debug.WriteLine("Running VersionChecker...");
        AppLogger.Log("Running VersionChecker...");

        MySqlConnection? connection = null;
        MySqlCommand? command = null;
        MySqlDataReader? reader = null;

        try
        {
            connection = new MySqlConnection(SqlVariables.GetConnectionString(null, null, null, null));
            connection.Open();

            command = new MySqlCommand("log_changelog_Get_Current", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            using (reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    // Use column name for clarity and safety
                    var databaseVersion = reader.GetString(reader.GetOrdinal("Version"));
                    LastCheckedDatabaseVersion = databaseVersion;

                    // Always show both app and server version for debugging
                    if (MainFormInstance != null)
                        MainFormInstance.SetVersionLabel(WipAppVariables.UserVersion, databaseVersion);

                    if (WipAppVariables.UserVersion != databaseVersion)
                    {
                        AppLogger.Log(
                            $"Version mismatch detected. Current: {WipAppVariables.UserVersion}, Expected: {databaseVersion}");
                        Debug.WriteLine(
                            $"Version mismatch detected. Current: {WipAppVariables.UserVersion}, Expected: {databaseVersion}");

                        Task.Run(() =>
                        {
                            var message = "You are using an older version of the WIP Application.\n" +
                                          "This normally means a newer version is just about to be released.\n" +
                                          "The program will close in 30 seconds, or by clicking OK.";
                            var caption = $"Version Conflict Error ({WipAppVariables.UserVersion}/{databaseVersion})";
                            MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                            Application.Exit();
                        });
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ExceptionHandler.HandleDatabaseError();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
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
}