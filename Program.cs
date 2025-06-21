using MTM_WIP_Application.Controls.MainForm;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Services;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Xceed.Words.NET;

namespace MTM_WIP_Application;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Debug.WriteLine("Main method started.");
        LoggingUtility.Log("Main method started.");

        try
        {
            _ = Service_OnStartup.RunStartupSequenceAsync();
            Service_OnStartup.RunApplication();
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            Service_OnEvent_ExceptionHandler.HandleDatabaseError();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            MessageBox.Show(@"An error occurred on Main in Program.cs:
" + ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}