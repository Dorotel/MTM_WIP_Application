using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.Helpers;

#region Helper_DpiChecker

public static class Helper_DpiChecker
{
    #region DPI Adjustment

    public static void AdjustFormForDpi(Form? form)
    {
        if (form == null) return;
        Helper_FontScaler.AdjustFontAndLayout(form);
    }

    #endregion

    #region DPI Check

    public static void CheckDpiScaling()
    {
        try
        {
            using var graphics = Graphics.FromHwnd(IntPtr.Zero);
            var dpiX = graphics.DpiX;
            LoggingUtility.Log($"DpiChecker: Current DPI is {dpiX}.");
            if (dpiX > 120)
            {
                MessageBox.Show(
                    @"The application does not support scaling above 125%. Please adjust your display settings.",
                    @"Unsupported Scaling",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                LoggingUtility.Log("DpiChecker: DPI scaling above 125% detected and warning shown.");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show(@$"Error checking DPI scaling: {ex.Message}", @"DPI Scaling Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #endregion
}

#endregion