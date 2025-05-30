using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.Helpers;

/// <summary>
///     Provides methods to check and handle DPI scaling issues for the application.
/// </summary>
public static class DpiChecker
{
    /// <summary>
    ///     Adjusts the font size and layout of the specified form and its controls based on the current DPI scaling.
    ///     Uses FontScaler for consistent scaling logic.
    /// </summary>
    /// <param name="form">The form to adjust.</param>
    public static void AdjustFormForDpi(Form? form)
    {
        FontScaler.AdjustFontAndLayout(form);
    }

    /// <summary>
    ///     Checks the current DPI scaling and prompts the user if it exceeds 125%.
    ///     Also logs the DPI value and any warnings.
    /// </summary>
    public static void CheckDpiScaling()
    {
        try
        {
            using var graphics = Graphics.FromHwnd(IntPtr.Zero);
            var dpiX = graphics.DpiX;

            AppLogger.Log($"DpiChecker: Current DPI is {dpiX}.");

            if (dpiX > 120)
            {
                MessageBox.Show(
                    @"The application does not support scaling above 125%. Please adjust your display settings.",
                    @"Unsupported Scaling",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AppLogger.Log("DpiChecker: DPI scaling above 125% detected and warning shown.");
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            MessageBox.Show(@$"Error checking DPI scaling: {ex.Message}", @"DPI Scaling Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}