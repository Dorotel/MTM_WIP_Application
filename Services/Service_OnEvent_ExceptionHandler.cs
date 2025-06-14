using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Logging;
using System.Diagnostics;

namespace MTM_WIP_Application.Services;

/// <summary>
///     Centralized exception handling for application-level errors.
/// </summary>
internal static class Service_OnEvent_ExceptionHandler
{
    /// <summary>
    ///     Handles database errors by logging, disabling the main form, and optionally updating UI status.
    /// </summary>
    public static void HandleDatabaseError()
    {
        Debug.WriteLine("Handling database error...");
        ApplicationLog.LogDatabaseError(new Exception("Handling database error..."));

        try
        {
            if (Application.OpenForms.OfType<MainForm>().Any())
            {
                var mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
                if (mainForm != null) mainForm.ConnectionRecoveryManager.HandleConnectionLost();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error handling database error: {ex.Message}");
            ApplicationLog.LogApplicationError(ex);
        }
    }

    /// <summary>
    ///     Handles general exceptions by logging and notifying the user.
    /// </summary>
    public static void HandleGeneralException(Exception ex)
    {
        Debug.WriteLine($"Exception: {ex.Message}");
        ApplicationLog.LogApplicationError(ex);

        MessageBox.Show(@"An unexpected error occurred: " + ex.Message, @"Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    ///     Handles unauthorized access exceptions by logging and notifying the user.
    /// </summary>
    public static void HandleUnauthorizedAccessException(UnauthorizedAccessException ex)
    {
        Debug.WriteLine($"UnauthorizedAccessException: {ex.Message}");
        ApplicationLog.LogApplicationError(ex);

        MessageBox.Show(
            @"You do not have the necessary permissions to run this application. Please run as administrator.",
            @"Permission Denied",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}