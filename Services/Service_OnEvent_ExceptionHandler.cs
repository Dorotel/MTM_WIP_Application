using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Services;

internal static class Service_OnEvent_ExceptionHandler
{
    #region Public Methods

    public static void HandleDatabaseError()
    {
        LoggingUtility.LogDatabaseError(new Exception("Handling database error..."));
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
            LoggingUtility.LogApplicationError(ex);
        }
    }

    public static void HandleGeneralException(Exception ex)
    {
        LoggingUtility.LogApplicationError(ex);
        MessageBox.Show(@"An unexpected error occurred: " + ex.Message, @"Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void HandleUnauthorizedAccessException(UnauthorizedAccessException ex)
    {
        LoggingUtility.LogApplicationError(ex);
        MessageBox.Show(
            @"You do not have the necessary permissions to run this application. Please run as administrator.",
            @"Permission Denied",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    #endregion
}