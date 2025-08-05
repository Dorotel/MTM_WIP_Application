using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Forms.Splash;

public partial class SplashScreenForm : Form
{
    #region Constructors
    

    #region Constructors
    
    public SplashScreenForm()
    {
        System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ctor] Constructing SplashScreenForm...");
        InitializeComponent();
        
        // Apply comprehensive DPI scaling and runtime layout adjustments
        AutoScaleMode = AutoScaleMode.Dpi;
        Core_Themes.ApplyDpiScaling(this);
        Core_Themes.ApplyRuntimeLayoutAdjustments(this);

        BackColor = Model_AppVariables.UserUiColors?.FormBackColor ?? BackColor;
        _titleLabel!.ForeColor = Model_AppVariables.UserUiColors?.LabelForeColor ?? _titleLabel.ForeColor;
        _versionLabel!.ForeColor = Model_AppVariables.UserUiColors?.LabelForeColor ?? _versionLabel.ForeColor;
        _versionLabel.Text = $"Control_About_Label_Version {Model_AppVariables.Version ?? "4.6.0.0"}";

        ApplyTheme();
        System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ctor] SplashScreenForm constructed.");
    }
    
    #endregion
    
    #region Methods
    
    
    #endregion
    
    #region Methods

    public void ShowSplash()
    {
        System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ShowSplash] Showing splash screen...");
        Show();
        _progressControl!.ShowProgress();
        Application.DoEvents();
        System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ShowSplash] Splash screen shown.");
    }

    public void UpdateProgress(int progress, string status)
    {
        System.Diagnostics.Debug.WriteLine(
            $"[DEBUG] [SplashScreenForm.UpdateProgress] Progress: {progress}, Status: {status}");
        _progressControl!.UpdateProgress(progress, status);
        Application.DoEvents();
    }

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
            System.Diagnostics.Debug.WriteLine(
                "[DEBUG] [SplashScreenForm.ApplyTheme] Theme application failed (ignored).");
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
            var margin = 16;

            var scale = 0.9f;
            var drawWidth = (int)(watermark.Width * scale);
            var drawHeight = (int)(watermark.Height * scale);

            var x = margin;
            var y = margin;

            var colorMatrix = new System.Drawing.Imaging.ColorMatrix
            {
                Matrix33 = 0.15f
            };
            var imageAttributes = new System.Drawing.Imaging.ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default,
                System.Drawing.Imaging.ColorAdjustType.Bitmap);

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
    
    #endregion

    
    #endregion
}