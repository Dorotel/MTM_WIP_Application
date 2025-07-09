using System.ComponentModel;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.Addons;

#region ConnectionStrengthControl

public partial class ConnectionStrengthControl : UserControl
{
    private int _strength; // 0-5
    private int _ping = -1; // Ping in ms, -1 means unknown
    private readonly ToolTip _toolTip = new();

    #region Properties

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int Strength
    {
        get => _strength;
        set
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    _strength = Math.Max(0, Math.Min(5, value));
                    UpdateToolTip();
                    Invalidate();
                }));
                return;
            }

            _strength = Math.Max(0, Math.Min(5, value));
            UpdateToolTip();
            Invalidate();
        }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int Ping
    {
        get => _ping;
        set
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    _ping = value;
                    UpdateToolTip();
                }));
                return;
            }

            _ping = value;
            UpdateToolTip();
        }
    }

    #endregion

    #region Initialization

    public ConnectionStrengthControl()
    {
        InitializeComponent();
        Size = new Size(80, 14);
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint,
            true);
        Paint += ConnectionStrengthControl_Paint;
        UpdateToolTip();
        MouseHover += ConnectionStrengthControl_MouseHover;
        Core.Core_Themes.ApplyFocusHighlighting(this);
    }

    #endregion

    #region UI Events

    private void ConnectionStrengthControl_MouseHover(object? sender, EventArgs e)
    {
        UpdateToolTip();
    }

    protected override void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);
        SyncBackgroundWithParent();
    }

    #endregion

    #region Drawing

    private void ConnectionStrengthControl_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        var barCount = 5;
        var spacing = 4;
        var barWidth = 5;
        var barMaxHeight = 10;
        var barMinHeight = 4;
        var totalWidth = barCount * barWidth + (barCount - 1) * spacing;
        var rightPadding = 4; // Leave 4 pixels padding on the right
        var startX = Width - totalWidth - rightPadding; // Align to right with padding
        var baseY = Height - 2;

        for (var i = 0; i < barCount; i++)
        {
            var color = i < _strength
                ? GetBarColor(i, barCount)
                : Model_AppVariables.UserUiColors.ControlBackColor ?? Color.LightGray;
            var height = barMinHeight + (barMaxHeight - barMinHeight) * i / (barCount - 1);
            var x = startX + i * (barWidth + spacing);
            var y = baseY - height;
            using (var brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, x, y, barWidth, height);
            }

            using var pen = new Pen(Model_AppVariables.UserUiColors.ControlForeColor ?? Color.DarkGray);
            g.DrawRectangle(pen, x, y, barWidth, height);
        }
    }

    private static Color GetBarColor(int barIndex, int barCount)
    {
        var lowColor = Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
        var highColor = Model_AppVariables.UserUiColors.SuccessColor ?? Color.LimeGreen;

        // Interpolate from red (low) to green (high)
        var t = barCount == 1 ? 1f : (float)barIndex / (barCount - 1);
        var r = (int)(lowColor.R * (1 - t) + highColor.R * t);
        var g = (int)(lowColor.G * (1 - t) + highColor.G * t);
        var b = (int)(lowColor.B * (1 - t) + highColor.B * t);
        return Color.FromArgb(r, g, b);
    }

    #endregion

    #region Helpers

    private void UpdateToolTip()
    {
        var quality = _strength switch
        {
            5 => "Excellent",
            4 => "Very Good",
            3 => "Good",
            2 => "Fair",
            1 => "Poor",
            _ => "No Signal"
        };
        var pingText = _ping >= 0 ? $"{_ping} ms" : "N/A";
        _toolTip.SetToolTip(this, $"Ping: {pingText} ({quality})");
    }

    // Call this method from MainForm after adding to the SplitContainer.Panel2
    private void SyncBackgroundWithParent()
    {
        if (Parent != null)
            BackColor = Parent.BackColor;
        else
            BackColor = Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Control;
        Invalidate();
    }

    #endregion
}

#endregion