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

/// <summary>
///     Handles adjusting font sizes and layouts for forms and controls based on DPI.
/// </summary>
public static class FontScaler
{
    /// <summary>
    ///     Adjusts the font size of a form and its controls based on the current DPI scaling.
    /// </summary>
    /// <param name="form">The form to adjust.</param>
    public static void AdjustFontAndLayout(Form? form)
    {
        try
        {
            if (form == null)
            {
                AppLogger.Log("FontScaler: Provided form is null.");
                return;
            }

            using var graphics = form.CreateGraphics();
            var dpiX = graphics.DpiX;

            AppLogger.Log($"FontScaler: Adjusting font for DPI {dpiX}.");

            var fontSize = GetFontSizeForDpi(dpiX);

            ResetControlFont(form, fontSize);

            AppLogger.Log($"FontScaler: Font adjustment complete for DPI {dpiX}.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            MessageBox.Show(@$"Error adjusting font: {ex.Message}", @"Font Adjustment Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    /// <summary>
    ///     Returns the font size for a given DPI scaling factor.
    /// </summary>
    private static float GetFontSizeForDpi(float dpi)
    {
        const float defaultFontSize = 9f;
        return dpi switch
        {
            96 => defaultFontSize, // 100% scaling
            120 => defaultFontSize - defaultFontSize * 0.25f, // 125% scaling
            _ => defaultFontSize // Default to 100% scaling font size for unsupported scales
        };
    }

    /// <summary>
    ///     Recursively resets the font size of a control and its child controls to a specified value.
    /// </summary>
    private static void ResetControlFont(Control? control, float fontSize)
    {
        if (control == null)
        {
            AppLogger.Log("FontScaler: Null control encountered during font reset.");
            return;
        }

        control.Font = new Font("Segoe UI", fontSize, control.Font.Style);

        foreach (Control childControl in control.Controls) ResetControlFont(childControl, fontSize);
    }
}