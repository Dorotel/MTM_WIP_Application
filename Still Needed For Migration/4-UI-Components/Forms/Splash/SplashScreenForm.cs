using System.Drawing.Imaging;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Forms.Splash
{
    public partial class SplashScreenForm : Form
    {
        #region Constructors

        #region Constructors

        public SplashScreenForm()
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["FormType"] = nameof(SplashScreenForm),
                ["Version"] = Model_AppVariables.Version ?? "4.6.0.0",
                ["InitializationTime"] = DateTime.Now,
                ["Thread"] = Thread.CurrentThread.ManagedThreadId
            }, nameof(SplashScreenForm), nameof(SplashScreenForm));

            System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ctor] Constructing SplashScreenForm...");

            Service_DebugTracer.TraceUIAction("SPLASH_FORM_INITIALIZATION", nameof(SplashScreenForm),
                new Dictionary<string, object>
                {
                    ["Phase"] = "START",
                    ["ComponentType"] = "SplashScreenForm"
                });

            InitializeComponent();

            Service_DebugTracer.TraceUIAction("THEME_APPLICATION", nameof(SplashScreenForm),
                new Dictionary<string, object>
                {
                    ["AutoScaleMode"] = "Dpi",
                    ["DpiScaling"] = "APPLIED",
                    ["LayoutAdjustments"] = "APPLIED"
                });
            // Apply comprehensive DPI scaling and runtime layout adjustments
            AutoScaleMode = AutoScaleMode.Dpi;
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

            Service_DebugTracer.TraceBusinessLogic("UI_COLORS_APPLICATION",
                inputData: new {
                    UserBackColor = Model_AppVariables.UserUiColors?.FormBackColor,
                    UserForeColor = Model_AppVariables.UserUiColors?.LabelForeColor,
                    Version = Model_AppVariables.Version
                },
                outputData: new {
                    AppliedBackColor = Model_AppVariables.UserUiColors?.FormBackColor ?? BackColor,
                    AppliedForeColor = Model_AppVariables.UserUiColors?.LabelForeColor,
                    VersionText = $"Version {Model_AppVariables.Version ?? "4.6.0.0"}"
                });
            BackColor = Model_AppVariables.UserUiColors?.FormBackColor ?? BackColor;
            _titleLabel!.ForeColor = Model_AppVariables.UserUiColors?.LabelForeColor ?? _titleLabel.ForeColor;
            _versionLabel!.ForeColor = Model_AppVariables.UserUiColors?.LabelForeColor ?? _versionLabel.ForeColor;
            _versionLabel.Text = $"Version {Model_AppVariables.Version ?? "4.6.0.0"}";

            Service_DebugTracer.TraceUIAction("THEME_APPLIED", nameof(SplashScreenForm));
            ApplyTheme();

            Service_DebugTracer.TraceUIAction("SPLASH_FORM_INITIALIZATION", nameof(SplashScreenForm),
                new Dictionary<string, object>
                {
                    ["Phase"] = "COMPLETE",
                    ["Success"] = true
                });

            Service_DebugTracer.TraceMethodExit(null, nameof(SplashScreenForm), nameof(SplashScreenForm));
            System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ctor] SplashScreenForm constructed.");
        }

        #endregion

        #region Methods

        #endregion

        #region Methods

        public void ShowSplash()
        {
            Service_DebugTracer.TraceMethodEntry(null, nameof(ShowSplash), nameof(SplashScreenForm));

            Service_DebugTracer.TraceUIAction("SPLASH_SCREEN_SHOW", nameof(SplashScreenForm),
                new Dictionary<string, object>
                {
                    ["Action"] = "Show",
                    ["ProgressControlVisible"] = true
                });

            System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ShowSplash] Showing splash screen...");
            Show();
            _progressControl!.ShowProgress();
            Application.DoEvents();
            System.Diagnostics.Debug.WriteLine("[DEBUG] [SplashScreenForm.ShowSplash] Splash screen shown.");

            Service_DebugTracer.TraceMethodExit(null, nameof(ShowSplash), nameof(SplashScreenForm));
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

        protected override void SetVisibleCore(bool value) => base.SetVisibleCore(value && !DesignMode);

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Bitmap? watermark = Properties.Resources.MTM;
            if (watermark != null)
            {
                Graphics g = e.Graphics;
                int margin = 16;

                float scale = 0.9f;
                int drawWidth = (int)(watermark.Width * scale);
                int drawHeight = (int)(watermark.Height * scale);

                int x = margin;
                int y = margin;

                ColorMatrix colorMatrix = new() { Matrix33 = 0.15f };
                ImageAttributes imageAttributes = new();
                imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default,
                    System.Drawing.Imaging.ColorAdjustType.Bitmap);

                Rectangle destRect = new(x, y, drawWidth, drawHeight);

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
}
