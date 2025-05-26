using System.Diagnostics;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.ErrorHandling;

/// <summary>
///     Centralized exception handling for application-level errors.
/// </summary>
internal static class ExceptionHandler
{
    /// <summary>
    ///     Handles database errors by logging, disabling the main form, and optionally updating UI status.
    /// </summary>
    public static void HandleDatabaseError()
    {
        Debug.WriteLine("Handling database error...");
        AppLogger.LogDatabaseError(new Exception("Handling database error..."));

        try
        {
            if (Application.OpenForms.OfType<MainForm>().Any())
            {
                var mainForm = Application.OpenForms.OfType<MainForm>().First();
                mainForm.Invoke(() =>
                {
                    // Optionally show status strip indicators here
                    mainForm.Enabled = false;
                });
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error handling database error: {ex.Message}");
            AppLogger.LogApplicationError(ex);
        }
    }

    /// <summary>
    ///     Handles general exceptions by logging and notifying the user.
    /// </summary>
    public static void HandleGeneralException(Exception ex)
    {
        Debug.WriteLine($"Exception: {ex.Message}");
        AppLogger.LogApplicationError(ex);

        MessageBox.Show(@"An unexpected error occurred: " + ex.Message, @"Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    ///     Handles unauthorized access exceptions by logging and notifying the user.
    /// </summary>
    public static void HandleUnauthorizedAccessException(UnauthorizedAccessException ex)
    {
        Debug.WriteLine($"UnauthorizedAccessException: {ex.Message}");
        AppLogger.LogApplicationError(ex);

        MessageBox.Show(
            @"You do not have the necessary permissions to run this application. Please run as administrator.",
            @"Permission Denied",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}