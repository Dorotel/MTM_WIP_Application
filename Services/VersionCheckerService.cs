using System.Diagnostics;
using System.Timers;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.ErrorHandling;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using Timer = System.Timers.Timer;

// Ensure this is the correct Timer namespace being used

namespace MTM_WIP_Application.Services;

internal static class VersionCheckerService
{
    private static readonly Timer VersionTimer = new(30000); // Fully qualify the Timer type

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
    }

    public static void VersionChecker(object? sender, ElapsedEventArgs e)
    {
        Debug.WriteLine("Running VersionChecker...");
        AppLogger.Log("Running VersionChecker...");

        MySqlConnection? connection = null;
        MySqlCommand? command = null;
        MySqlDataReader? reader = null;

        try
        {
            connection = new MySqlConnection(SqlVariables.GetConnectionString(null, "mtm database", null, null));
            connection.Open();

            command = new MySqlCommand("SELECT * FROM `program_information`", connection);
            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var databaseVersion = reader.GetString(1);

                    if (databaseVersion != WipAppVariables.UserVersion)
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

                        break;
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