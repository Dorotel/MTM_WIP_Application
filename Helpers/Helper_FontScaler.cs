using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.Helpers;

#region Helper_FontScaler

public static class Helper_FontScaler
{
    #region Font Adjustment

    public static void AdjustFontAndLayout(Form? form)
    {
        try
        {
            if (form == null)
            {
                LoggingUtility.Log("FontScaler: Provided form is null.");
                return;
            }

            using var graphics = form.CreateGraphics();
            var dpiX = graphics.DpiX;
            LoggingUtility.Log($"FontScaler: Adjusting font for DPI {dpiX}.");
            var fontSize = GetFontSizeForDpi(dpiX);
            ResetControlFont(form, fontSize);
            LoggingUtility.Log($"FontScaler: Font adjustment complete for DPI {dpiX}.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show(@$"Error adjusting font: {ex.Message}", @"Font Adjustment Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    #endregion

    #region Font Size Calculation

    private static float GetFontSizeForDpi(float dpi)
    {
        const float defaultFontSize = 9f;
        return dpi switch
        {
            96 => defaultFontSize,
            120 => defaultFontSize - defaultFontSize * 0.25f,
            _ => defaultFontSize
        };
    }

    #endregion

    #region Font Reset

    private static void ResetControlFont(Control? control, float fontSize)
    {
        if (control == null)
        {
            LoggingUtility.Log("FontScaler: Null control encountered during font reset.");
            return;
        }

        control.Font = new Font("Segoe UI", fontSize, control.Font.Style);
        foreach (Control childControl in control.Controls) ResetControlFont(childControl, fontSize);
    }

    #endregion
}

#endregion