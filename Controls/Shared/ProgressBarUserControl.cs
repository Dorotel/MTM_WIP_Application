using System.ComponentModel;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Models;
using Timer = System.Windows.Forms.Timer;

namespace MTM_Inventory_Application.Controls.Shared;

public partial class ProgressBarUserControl : UserControl
{
    #region Fields
    

    private PictureBox? _loadingImage;
    private ProgressBar? _progressBar;
    private Label? _statusLabel;

    public void UpdateProgress(int value, string? status = null)
    
    #endregion
    
    #region Constructors
    

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

    [DefaultValue("Loading...")]
    public string StatusText
    {
        get => _statusLabel?.Text ?? string.Empty;
        set
        {
            if (_statusLabel != null)
            {
                _statusLabel.Text = value;
            }
        }
    }

    [DefaultValue(true)]
    public bool ShowLoadingImage
    {
        get => _loadingImage?.Visible ?? false;
        set
        {
            if (_loadingImage != null)
            {
                _loadingImage.Visible = value;
            }
        }
    }

    public ProgressBarUserControl()
    {
        InitializeComponent();
        InitializeControls();
        ApplyTheme();
    }
    
    #endregion
    
    #region Methods
    

    private void InitializeControls()
    {
        _loadingImage = new PictureBox
        {
            Size = new Size(48, 48),
            SizeMode = PictureBoxSizeMode.CenterImage,
            Anchor = AnchorStyles.Top,
            BackColor = Color.Transparent
        };

        _loadingImage.Paint += LoadingImage_Paint;

        _progressBar = new ProgressBar
        {
            Style = ProgressBarStyle.Continuous,
            Height = 23,
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            Minimum = 0,
            Maximum = 100,
            Value = 0
        };

        _statusLabel = new Label
        {
            Text = "Loading...",
            TextAlign = ContentAlignment.MiddleCenter,
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            Height = 20,
            AutoSize = false
        };

        LayoutControls();

        Controls.Add(_loadingImage);
        Controls.Add(_progressBar);
        Controls.Add(_statusLabel);
    }

    private void LayoutControls()
    {
        int spacing = 8;
        int currentY = spacing;

        if (_loadingImage != null)
        {
            _loadingImage.Location = new Point((Width - _loadingImage.Width) / 2, currentY);
            currentY += _loadingImage.Height + spacing;
        }

        if (_progressBar != null)
        {
            _progressBar.Location = new Point(spacing, currentY);
            _progressBar.Width = Width - spacing * 2;
            currentY += _progressBar.Height + spacing;
        }

        if (_statusLabel != null)
        {
            _statusLabel.Location = new Point(spacing, currentY);
            _statusLabel.Width = Width - spacing * 2;
        }

        Height = currentY + (_statusLabel?.Height ?? 0) + spacing;
    }

    private void LoadingImage_Paint(object? sender, PaintEventArgs e)
    {
        if (_loadingImage == null)
        {
            return;
        }

        Graphics g = e.Graphics;
        Rectangle rect = _loadingImage.ClientRectangle;
        Point center = new(rect.Width / 2, rect.Height / 2);
        int radius = Math.Min(rect.Width, rect.Height) / 2 - 4;

        using Pen pen = new(Model_AppVariables.UserUiColors?.ProgressBarForeColor ?? Color.Blue, 3);

        int startAngle = Environment.TickCount / 10 % 360;
        g.DrawArc(pen, center.X - radius, center.Y - radius, radius * 2, radius * 2, startAngle, 270);
    }

    private void UpdateStatusText()
    {
        if (_progressBar == null)
        {
            return;
        }

        if (_progressBar.Value == 0)
        {
            StatusText = "Initializing...";
        }
        else if (_progressBar.Value == 100)
        {
            StatusText = "Complete";
        }
        else
        {
            StatusText = $"Loading... {_progressBar.Value}%";
        }
    }

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

        if (_loadingImage != null)
        {
            Timer timer = new() { Interval = 50 };
            timer.Tick += (s, e) => _loadingImage.Invalidate();
            timer.Start();

            Tag = timer;
        }
    }

    public void HideProgress()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(HideProgress));
            return;
        }

        if (Tag is Timer timer)
        {
            timer.Stop();
            timer.Dispose();
            Tag = null;
        }

        Visible = false;
    }
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => UpdateProgress(value, status)));
            return;
        }

        ProgressValue = value;
        if (!string.IsNullOrEmpty(status))
        {
            StatusText = status;
        }
    }

    public async Task CompleteProgressAsync()
    {
        UpdateProgress(100, "Complete");
        await Task.Delay(500);
        HideProgress();
    }

    private void ApplyTheme()
    {
        try
        {
            Core_Themes.Core_AppThemes.AppTheme theme = Core_Themes.Core_AppThemes.GetCurrentTheme();
            Model_UserUiColors colors = theme.Colors;
            if (colors.UserControlBackColor.HasValue)
            {
                BackColor = colors.UserControlBackColor.Value;
            }

            if (colors.UserControlForeColor.HasValue)
            {
                ForeColor = colors.UserControlForeColor.Value;
            }
        }
        catch
        {
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (_loadingImage != null && _progressBar != null && _statusLabel != null)
        {
            LayoutControls();
        }
    }

    
    #endregion
}
