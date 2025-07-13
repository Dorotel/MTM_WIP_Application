// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Diagnostics;
using System.Timers;
using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using Timer = System.Timers.Timer;

namespace MTM_Inventory_Application.Services;

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

    public static async void VersionChecker(object? sender, ElapsedEventArgs? e)
    {
        Debug.WriteLine("Running VersionChecker...");
        LoggingUtility.Log("Running VersionChecker...");
        try
        {
            var helper =
                new Helper_Database_Core(Helper_Database_Variables.GetConnectionString(null, null, null, null));
            var dt = await helper.ExecuteDataTable("log_changelog_Get_Current", null, true,
                CommandType.StoredProcedure);
            if (dt.Rows.Count == 0) return;
            var databaseVersion = dt.Rows[0]["Version"]?.ToString();
            LastCheckedDatabaseVersion = databaseVersion;
            Debug.WriteLine(LastCheckedDatabaseVersion);
            ControlInventoryInstance?.SetVersionLabel(Model_AppVariables.UserVersion, databaseVersion ?? "Unknown");
            if (Model_AppVariables.UserVersion != databaseVersion)
            {
                LoggingUtility.Log(
                    $"Version mismatch detected. Current: {Model_AppVariables.UserVersion}, Expected: {databaseVersion}");
                Debug.WriteLine(
                    $"Version mismatch detected. Current: {Model_AppVariables.UserVersion}, Expected: {databaseVersion}");
                await Task.Run(() =>
                {
                    var message = "You are using an older version of the WIP Application.\n" +
                                  "This normally means a newer version is just about to be released.\n" +
                                  "The program will close in 30 seconds, or by clicking OK.";
                    var caption =
                        $"Version Conflict Error ({Model_AppVariables.UserVersion}/{databaseVersion})";
                    MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    Application.Exit();
                });
            }
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