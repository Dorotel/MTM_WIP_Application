using System;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Models;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace MTM_Inventory_Application.Controls.Shared;

/// <summary>
/// A reusable progress bar user control with loading image for application-wide use
/// </summary>
public partial class ProgressBarUserControl : UserControl
{
    private PictureBox? _loadingImage;
    private ProgressBar? _progressBar;
    private Label? _statusLabel;

    /// <summary>
    /// Gets or sets the current progress value (0-100)
    /// </summary>
    [DefaultValue(0)]
    public int ProgressValue
    {
        get => _progressBar?.Value ?? 0;
        set
        {
            if (value >= 0 && value <= 100 && _progressBar != null)
            {
                _progressBar.Value = value;
                UpdateStatusText();
            }
        }
    }

    /// <summary>
    /// Gets or sets the status text displayed below the progress bar
    /// </summary>
    [DefaultValue("Loading...")]
    public string StatusText
    {
        get => _statusLabel?.Text ?? string.Empty;
        set
        {
            if (_statusLabel != null) _statusLabel.Text = value;
        }
    }

    /// <summary>
    /// Gets or sets whether the loading image is visible
    /// </summary>
    [DefaultValue(true)]
    public bool ShowLoadingImage
    {
        get => _loadingImage?.Visible ?? false;
        set
        {
            if (_loadingImage != null) _loadingImage.Visible = value;
        }
    }

    public ProgressBarUserControl()
    {
        InitializeComponent();
        InitializeControls();
        ApplyTheme();
    }

    private void InitializeControls()
    {
        // Create loading image
        _loadingImage = new PictureBox
        {
            Size = new Size(48, 48),
            SizeMode = PictureBoxSizeMode.CenterImage,
            Anchor = AnchorStyles.Top,
            BackColor = Color.Transparent
        };

        // Create a simple loading animation using text for now
        // In a real implementation, you might use an animated GIF or custom drawing
        _loadingImage.Paint += LoadingImage_Paint;

        // Create progress bar
        _progressBar = new ProgressBar
        {
            Style = ProgressBarStyle.Continuous,
            Height = 23,
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            Minimum = 0,
            Maximum = 100,
            Value = 0
        };

        // Create status label
        _statusLabel = new Label
        {
            Text = "Loading...",
            TextAlign = ContentAlignment.MiddleCenter,
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            Height = 20,
            AutoSize = false
        };

        // Layout controls
        LayoutControls();

        // Add controls to the user control
        Controls.Add(_loadingImage);
        Controls.Add(_progressBar);
        Controls.Add(_statusLabel);
    }

    private void LayoutControls()
    {
        var spacing = 8;
        var currentY = spacing;

        if (_loadingImage != null)
        {
            // Position loading image at top center
            _loadingImage.Location = new Point((Width - _loadingImage.Width) / 2, currentY);
            currentY += _loadingImage.Height + spacing;
        }

        if (_progressBar != null)
        {
            // Position progress bar below image
            _progressBar.Location = new Point(spacing, currentY);
            _progressBar.Width = Width - spacing * 2;
            currentY += _progressBar.Height + spacing;
        }

        if (_statusLabel != null)
        {
            // Position status label below progress bar
            _statusLabel.Location = new Point(spacing, currentY);
            _statusLabel.Width = Width - spacing * 2;
        }

        // Set total height
        Height = currentY + (_statusLabel?.Height ?? 0) + spacing;
    }

    private void LoadingImage_Paint(object? sender, PaintEventArgs e)
    {
        if (_loadingImage == null) return;

        // Simple loading indicator - draw a rotating circle or spinner
        var g = e.Graphics;
        var rect = _loadingImage.ClientRectangle;
        var center = new Point(rect.Width / 2, rect.Height / 2);
        var radius = Math.Min(rect.Width, rect.Height) / 2 - 4;

        using var pen = new Pen(Model_AppVariables.UserUiColors?.ProgressBarForeColor ?? Color.Blue, 3);

        // Draw spinning arc
        var startAngle = Environment.TickCount / 10 % 360;
        g.DrawArc(pen, center.X - radius, center.Y - radius, radius * 2, radius * 2, startAngle, 270);
    }

    private void UpdateStatusText()
    {
        if (_progressBar == null) return;

        if (_progressBar.Value == 0)
            StatusText = "Initializing...";
        else if (_progressBar.Value == 100)
            StatusText = "Complete";
        else
            StatusText = $"Loading... {_progressBar.Value}%";
    }

    /// <summary>
    /// Shows the progress control and starts the loading animation
    /// </summary>
    public void ShowProgress()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(ShowProgress));
            return;
        }

        Visible = true;
        ProgressValue = 0;
        StatusText = "Loading...";

        // Start animation timer for loading image
        if (_loadingImage != null)
        {
            var timer = new Timer { Interval = 50 };
            timer.Tick += (s, e) => _loadingImage.Invalidate();
            timer.Start();

            // Store timer reference to stop it later
            Tag = timer;
        }
    }

    /// <summary>
    /// Hides the progress control and stops the loading animation
    /// </summary>
    public void HideProgress()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(HideProgress));
            return;
        }

        // Stop animation timer
        if (Tag is Timer timer)
        {
            timer.Stop();
            timer.Dispose();
            Tag = null;
        }

        Visible = false;
    }

    /// <summary>
    /// Updates the progress value and status text
    /// </summary>
    /// <param name="value">Progress value (0-100)</param>
    /// <param name="status">Optional status text</param>
    public void UpdateProgress(int value, string? status = null)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => UpdateProgress(value, status)));
            return;
        }

        ProgressValue = value;
        if (!string.IsNullOrEmpty(status))
            StatusText = status;
    }

    /// <summary>
    /// Completes the progress and hides the control after a brief delay
    /// </summary>
    public async Task CompleteProgressAsync()
    {
        UpdateProgress(100, "Complete");
        await Task.Delay(500); // Brief delay to show completion
        HideProgress();
    }

    private void ApplyTheme()
    {
        try
        {
            // Use the correct class and namespace for GetCurrentTheme
            var theme = Core_Themes.Core_AppThemes.GetCurrentTheme();
            var colors = theme.Colors;
            if (colors.UserControlBackColor.HasValue) BackColor = colors.UserControlBackColor.Value;
            if (colors.UserControlForeColor.HasValue) ForeColor = colors.UserControlForeColor.Value;
        }
        catch
        {
            // Ignore theme errors during design time
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (_loadingImage != null && _progressBar != null && _statusLabel != null) LayoutControls();
    }
}