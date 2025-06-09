using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MTM_WIP_Application.Controls;

public partial class ConnectionStrengthControl : UserControl
{
    private int _strength = 0; // 0-5
    private int _ping = -1; // Ping in ms, -1 means unknown
    private readonly ToolTip _toolTip = new();

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int Strength
    {
        get => _strength;
        set
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => Strength = value));
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
                Invoke(new MethodInvoker(() => Ping = value));
                return;
            }

            _ping = value;
            UpdateToolTip();
        }
    }

    public ConnectionStrengthControl()
    {
        InitializeComponent();
        Size = new Size(80, 14);
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint,
            true);
        Paint += ConnectionStrengthControl_Paint;
        UpdateToolTip();
        MouseHover += ConnectionStrengthControl_MouseHover;
    }

    private void ConnectionStrengthControl_MouseHover(object? sender, EventArgs e)
    {
        UpdateToolTip();
    }

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
    public void SyncBackgroundWithParent()
    {
        // Try to match the immediate parent, or fallback to SystemColors.Control
        if (Parent != null)
            BackColor = Parent.BackColor;
        else
            BackColor = SystemColors.Control;
        Invalidate();
    }

    protected override void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);
        SyncBackgroundWithParent();
    }


    private Color GetBarColor(int barIndex, int barCount)
    {
        // Interpolate from red (low) to green (high)
        var t = barCount == 1 ? 1f : (float)barIndex / (barCount - 1);
        var r = (int)(255 * (1 - t));
        var g = (int)(200 * t); // 200 for a more pleasant green
        var b = 0;
        return Color.FromArgb(r, g, b);
    }

    private void ConnectionStrengthControl_Paint(object sender, PaintEventArgs e)
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
            var color = i < _strength ? GetBarColor(i, barCount) : Color.LightGray;
            var height = barMinHeight + (barMaxHeight - barMinHeight) * i / (barCount - 1);
            var x = startX + i * (barWidth + spacing);
            var y = baseY - height;
            using (var brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, x, y, barWidth, height);
            }

            g.DrawRectangle(Pens.DarkGray, x, y, barWidth, height);
        }
    }
}