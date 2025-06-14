using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.Helpers;

/// <summary>
///     Handles adjusting font sizes and layouts for forms and controls based on DPI.
/// </summary>
public static class Helper_FontScaler
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
                ApplicationLog.Log("FontScaler: Provided form is null.");
                return;
            }

            using var graphics = form.CreateGraphics();
            var dpiX = graphics.DpiX;

            ApplicationLog.Log($"FontScaler: Adjusting font for DPI {dpiX}.");

            var fontSize = GetFontSizeForDpi(dpiX);

            ResetControlFont(form, fontSize);

            ApplicationLog.Log($"FontScaler: Font adjustment complete for DPI {dpiX}.");
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
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
            ApplicationLog.Log("FontScaler: Null control encountered during font reset.");
            return;
        }

        control.Font = new Font("Segoe UI", fontSize, control.Font.Style);

        foreach (Control childControl in control.Controls) ResetControlFont(childControl, fontSize);
    }
}