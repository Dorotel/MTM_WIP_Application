using MTM_Inventory_Application.Controls.Shared;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Forms.Splash;

/// <summary>
/// Splash screen form displayed during application startup
/// </summary>
public partial class SplashScreenForm : Form
{
    public SplashScreenForm()
    {
        System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ctor] Constructing SplashScreenForm...");
        InitializeComponent();

        // Set dynamic properties that depend on runtime values
        BackColor = Model_AppVariables.UserUiColors?.FormBackColor ?? BackColor;
        _titleLabel!.ForeColor = Model_AppVariables.UserUiColors?.LabelForeColor ?? _titleLabel.ForeColor;
        _versionLabel!.ForeColor = Model_AppVariables.UserUiColors?.LabelForeColor ?? _versionLabel.ForeColor;
        _versionLabel.Text = $"Version {Model_AppVariables.Version ?? "4.6.0.0"}";

        ApplyTheme();
        System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ctor] SplashScreenForm constructed.");
    }

    /// <summary>
    /// Shows the splash screen and starts the loading process
    /// </summary>
    public void ShowSplash()
    {
        System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ShowSplash] Showing splash screen...");
        Show();
        _progressControl!.ShowProgress();
        Application.DoEvents();
        System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ShowSplash] Splash screen shown.");
    }

    /// <summary>
    /// Updates the progress and status message
    /// </summary>
    /// <param name="progress">Progress value (0-100)</param>
    /// <param name="status">Status message</param>
    public void UpdateProgress(int progress, string status)
    {
        System.Diagnostics.Debug.WriteLine($"[DEBUG] [SplashScreenForm.UpdateProgress] Progress: {progress}, Status: {status}");
        _progressControl!.UpdateProgress(progress, status);
        Application.DoEvents();
    }

    /// <summary>
    /// Completes the loading process and closes the splash screen
    /// </summary>
    public async Task CompleteSplashAsync()
    {
        System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.CompleteSplashAsync] Completing splash...");
        await _progressControl!.CompleteProgressAsync();
        Close();
        System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.CompleteSplashAsync] Splash closed.");
    }

    private void ApplyTheme()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ApplyTheme] Applying theme...");
            Core_Themes.ApplyTheme(this);
            System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ApplyTheme] Theme applied.");
        }
        catch
        {
            System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ApplyTheme] Theme application failed (ignored).");
        }
    }

    protected override void SetVisibleCore(bool value)
    {
        base.SetVisibleCore(value && !DesignMode);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);

        var watermark = Properties.Resources.MTM;
        if (watermark != null)
        {
            var g = e.Graphics;
            int margin = 16;

            // Shrink the watermark by 10%
            float scale = 0.9f;
            int drawWidth = (int)(watermark.Width * scale);
            int drawHeight = (int)(watermark.Height * scale);

            int x = margin;
            int y = margin;

            // Set transparency (adjust alpha as needed)
            var colorMatrix = new System.Drawing.Imaging.ColorMatrix
            {
                Matrix33 = 0.15f // 15% opacity
            };
            var imageAttributes = new System.Drawing.Imaging.ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

            var destRect = new Rectangle(x, y, drawWidth, drawHeight);

            g.DrawImage(
                watermark,
                destRect,
                0, 0, watermark.Width, watermark.Height,
                GraphicsUnit.Pixel,
                imageAttributes
            );
        }
    }
}