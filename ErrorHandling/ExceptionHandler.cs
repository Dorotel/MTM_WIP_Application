using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.ErrorHandling;

internal static class ExceptionHandler
{
    public static void HandleUnauthorizedAccessException(UnauthorizedAccessException ex)
    {
        Debug.WriteLine($"UnauthorizedAccessException: {ex.Message}");
        AppLogger.Log($"UnauthorizedAccessException: {ex.Message}");

        MessageBox.Show(
            @"You do not have the necessary permissions to run this application. Please run as administrator.",
            @"Permission Denied",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void HandleGeneralException(Exception ex)
    {
        Debug.WriteLine($"Exception: {ex.Message}");
        AppLogger.Log($"Exception: {ex.Message}");

        MessageBox.Show(@"An unexpected error occurred: " + ex.Message, @"Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void HandleDatabaseError()
    {
        Debug.WriteLine("Handling database error...");
        AppLogger.Log("Handling database error...");

        try
        {
            if (Application.OpenForms.OfType<MainForm>().Any())
            {
                var mainForm = Application.OpenForms.OfType<MainForm>().First();
                mainForm.Invoke(new Action(() =>
                {
                    //mainForm.MainForm_StatusStrip_Disconnected.Visible = true;
                    //mainForm.MainForm_StatusStrip_SavedStatus.Visible = false;
                    mainForm.Enabled = false;
                }));
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error handling database error: {ex.Message}");
            AppLogger.Log($"Error handling database error: {ex.Message}");
        }
    }
}