using MTM_WIP_Application.Logging;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MTM_WIP_Application.Forms.MainForm;

namespace MTM_WIP_Application.Core;

internal class DgvDesigner
{
    // Global theme definition
    private class AppTheme
    {
        // Form-wide
        public Color FormBackColor { get; set; }
        public Color FormForeColor { get; set; }
        public Font? FormFont { get; set; }

        // DataGridView
        public Color DataGridBackColor { get; set; }
        public BorderStyle DataGridBorderStyle { get; set; }
        public Color DataGridSelectionBackColor { get; set; }
        public Color DataGridSelectionForeColor { get; set; }
        public Color DataGridRowsBackColor { get; set; }
        public Color DataGridAltRowsBackColor { get; set; }
        public Color? DataGridColumnHeadersForeColor { get; set; }
        public Color? DataGridColumnHeadersBackColor { get; set; }
        public Color? DataGridRowHeadersBackColor { get; set; }

        // Button, TextBox, Label, etc. can be added here for further customization
        public Color? ButtonBackColor { get; set; }
        public Color? ButtonForeColor { get; set; }
        public Color? TextBoxBackColor { get; set; }
        public Color? TextBoxForeColor { get; set; }
        public Color? LabelForeColor { get; set; }
    }

    // Theme registry
    private static readonly Dictionary<string, AppTheme> Themes = new()
    {
        ["Default (Black and White)"] = new AppTheme
        {
            FormBackColor = Color.White,
            FormForeColor = Color.Black,
            FormFont = null,
            DataGridBackColor = Color.LightGray,
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = Color.Blue,
            DataGridSelectionForeColor = Color.White,
            DataGridRowsBackColor = Color.White,
            DataGridAltRowsBackColor = Color.WhiteSmoke,
            ButtonBackColor = Color.LightGray,
            ButtonForeColor = Color.Black,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = Color.Black,
            LabelForeColor = Color.Black
        },
        ["Light Blue"] = new AppTheme
        {
            FormBackColor = Color.AliceBlue,
            FormForeColor = Color.Black,
            FormFont = null,
            DataGridBackColor = Color.LightGray,
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = Color.CornflowerBlue,
            DataGridSelectionForeColor = Color.Black,
            DataGridRowsBackColor = Color.LightBlue,
            DataGridAltRowsBackColor = Color.Azure,
            DataGridColumnHeadersForeColor = Color.White,
            DataGridColumnHeadersBackColor = Color.Black,
            DataGridRowHeadersBackColor = Color.Black,
            ButtonBackColor = Color.LightBlue,
            ButtonForeColor = Color.Black,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = Color.Black,
            LabelForeColor = Color.Navy
        },
        ["Light Red"] = new AppTheme
        {
            FormBackColor = ColorTranslator.FromHtml("#f4cccc"),
            FormForeColor = Color.Black,
            FormFont = null,
            DataGridBackColor = ColorTranslator.FromHtml("#EEEEEE"),
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = ColorTranslator.FromHtml("#0000ff"),
            DataGridSelectionForeColor = ColorTranslator.FromHtml("#ffffff"),
            DataGridRowsBackColor = ColorTranslator.FromHtml("#FF0000"),
            DataGridAltRowsBackColor = ColorTranslator.FromHtml("#f4cccc"),
            ButtonBackColor = ColorTranslator.FromHtml("#FFCCCC"),
            ButtonForeColor = Color.Black,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = Color.Black,
            LabelForeColor = Color.DarkRed
        },
        ["Light Grey"] = new AppTheme
        {
            FormBackColor = ColorTranslator.FromHtml("#EEEEEE"),
            FormForeColor = Color.Black,
            FormFont = null,
            DataGridBackColor = ColorTranslator.FromHtml("#EEEEEE"),
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = ColorTranslator.FromHtml("#434C5B"),
            DataGridSelectionForeColor = ColorTranslator.FromHtml("#F5F3F5"),
            DataGridRowsBackColor = ColorTranslator.FromHtml("#a6a6a6"),
            DataGridAltRowsBackColor = ColorTranslator.FromHtml("#cccccc"),
            ButtonBackColor = ColorTranslator.FromHtml("#cccccc"),
            ButtonForeColor = Color.Black,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = Color.Black,
            LabelForeColor = Color.Black
        },
        ["Dark"] = new AppTheme
        {
            FormBackColor = Color.FromArgb(32, 32, 32),
            FormForeColor = Color.White,
            FormFont = null,
            DataGridBackColor = Color.FromArgb(48, 48, 48),
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = Color.FromArgb(64, 64, 128),
            DataGridSelectionForeColor = Color.White,
            DataGridRowsBackColor = Color.FromArgb(32, 32, 32),
            DataGridAltRowsBackColor = Color.FromArgb(48, 48, 48),
            DataGridColumnHeadersForeColor = Color.White,
            DataGridColumnHeadersBackColor = Color.FromArgb(64, 64, 64),
            DataGridRowHeadersBackColor = Color.FromArgb(64, 64, 64),
            ButtonBackColor = Color.FromArgb(64, 64, 64),
            ButtonForeColor = Color.White,
            TextBoxBackColor = Color.FromArgb(48, 48, 48),
            TextBoxForeColor = Color.White,
            LabelForeColor = Color.White
        },
        ["Green"] = new AppTheme
        {
            FormBackColor = Color.Honeydew,
            FormForeColor = Color.DarkGreen,
            FormFont = null,
            DataGridBackColor = Color.Honeydew,
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = Color.SeaGreen,
            DataGridSelectionForeColor = Color.White,
            DataGridRowsBackColor = Color.Honeydew,
            DataGridAltRowsBackColor = Color.PaleGreen,
            DataGridColumnHeadersForeColor = Color.White,
            DataGridColumnHeadersBackColor = Color.SeaGreen,
            DataGridRowHeadersBackColor = Color.SeaGreen,
            ButtonBackColor = Color.PaleGreen,
            ButtonForeColor = Color.DarkGreen,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = Color.DarkGreen,
            LabelForeColor = Color.SeaGreen
        },
        ["Solarized"] = new AppTheme
        {
            FormBackColor = ColorTranslator.FromHtml("#fdf6e3"),
            FormForeColor = ColorTranslator.FromHtml("#657b83"),
            FormFont = null,
            DataGridBackColor = ColorTranslator.FromHtml("#eee8d5"),
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = ColorTranslator.FromHtml("#b58900"),
            DataGridSelectionForeColor = ColorTranslator.FromHtml("#fdf6e3"),
            DataGridRowsBackColor = ColorTranslator.FromHtml("#eee8d5"),
            DataGridAltRowsBackColor = ColorTranslator.FromHtml("#fdf6e3"),
            DataGridColumnHeadersForeColor = ColorTranslator.FromHtml("#fdf6e3"),
            DataGridColumnHeadersBackColor = ColorTranslator.FromHtml("#657b83"),
            DataGridRowHeadersBackColor = ColorTranslator.FromHtml("#657b83"),
            ButtonBackColor = ColorTranslator.FromHtml("#eee8d5"),
            ButtonForeColor = ColorTranslator.FromHtml("#657b83"),
            TextBoxBackColor = ColorTranslator.FromHtml("#fdf6e3"),
            TextBoxForeColor = ColorTranslator.FromHtml("#657b83"),
            LabelForeColor = ColorTranslator.FromHtml("#657b83")
        },
        ["High Contrast"] = new AppTheme
        {
            FormBackColor = Color.Black,
            FormForeColor = Color.Yellow,
            FormFont = null,
            DataGridBackColor = Color.Black,
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = Color.Yellow,
            DataGridSelectionForeColor = Color.Black,
            DataGridRowsBackColor = Color.Black,
            DataGridAltRowsBackColor = Color.Gray,
            DataGridColumnHeadersForeColor = Color.Black,
            DataGridColumnHeadersBackColor = Color.Yellow,
            DataGridRowHeadersBackColor = Color.Yellow,
            ButtonBackColor = Color.Yellow,
            ButtonForeColor = Color.Black,
            TextBoxBackColor = Color.Black,
            TextBoxForeColor = Color.Yellow,
            LabelForeColor = Color.Yellow
        }
        // Add more themes as needed
    };

    /// <summary>
    /// Applies the current global theme to the given form and all its controls recursively.
    /// </summary>
    public void ApplyTheme(Form form)
    {
        var themeName = WipAppVariables.WipDataGridTheme ?? "Default (Black and White)";
        if (!Themes.TryGetValue(themeName, out var theme))
            theme = Themes["Default (Black and White)"];

        form.SuspendLayout();

        form.BackColor = theme.FormBackColor;
        form.ForeColor = theme.FormForeColor;
        form.Font = theme.FormFont ?? new Font(form.Font.Name, WipAppVariables.ThemeFontSize, form.Font.Style);

        // Update the form's title bar text to include the theme name
        if (!form.Text.Contains($"[{themeName}]"))
        {
            // Remove any previous theme tag
            var idx = form.Text.LastIndexOf('[');
            if (idx > 0)
                form.Text = form.Text.Substring(0, idx).TrimEnd();
            form.Text = $"{form.Text} [{themeName}]";
        }

        ApplyThemeToControls(form.Controls, theme);

        form.ResumeLayout();
        AppLogger.Log($"Global theme '{themeName}' applied to form '{form.Name}'.");
    }

    private static void ApplyThemeToControls(Control.ControlCollection controls, AppTheme theme)
    {
        foreach (Control ctrl in controls)
        {
            // DataGridView special handling
            if (ctrl is DataGridView dgv)
            {
                ApplyThemeToDataGridView(dgv, theme);
            }
            else
            {
                ctrl.BackColor = theme.FormBackColor;
                ctrl.ForeColor = theme.FormForeColor;
                ctrl.Font = theme.FormFont ?? new Font(ctrl.Font.Name, WipAppVariables.ThemeFontSize, ctrl.Font.Style);

                // Control-specific customization
                switch (ctrl)
                {
                    case Button btn:
                        if (theme.ButtonBackColor.HasValue) btn.BackColor = theme.ButtonBackColor.Value;
                        if (theme.ButtonForeColor.HasValue) btn.ForeColor = theme.ButtonForeColor.Value;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 0; // Remove border
                        break;
                    case TabControl tab:
                        tab.BackColor = theme.FormBackColor;
                        tab.DrawMode = TabDrawMode.OwnerDrawFixed;
                        tab.DrawItem += (s, e) =>
                        {
                            e.Graphics.FillRectangle(new SolidBrush(theme.FormBackColor), e.Bounds);
                            TextRenderer.DrawText(e.Graphics, tab.TabPages[e.Index].Text, tab.Font, e.Bounds,
                                theme.FormForeColor);
                        };
                        break;
                    case TextBox txt:
                        if (theme.TextBoxBackColor.HasValue) txt.BackColor = theme.TextBoxBackColor.Value;
                        if (theme.TextBoxForeColor.HasValue) txt.ForeColor = theme.TextBoxForeColor.Value;
                        break;
                    case Label lbl:
                        if (theme.LabelForeColor.HasValue) lbl.ForeColor = theme.LabelForeColor.Value;
                        break;
                    // Add more control types as needed
                }
            }

            // Recursively apply to child controls
            if (ctrl.HasChildren)
                ApplyThemeToControls(ctrl.Controls, theme);
        }
    }

    private static void ApplyThemeToDataGridView(DataGridView dataGridView, AppTheme theme)
    {
        dataGridView.BackgroundColor = theme.DataGridBackColor;
        dataGridView.BorderStyle = theme.DataGridBorderStyle;
        dataGridView.DefaultCellStyle.SelectionBackColor = theme.DataGridSelectionBackColor;
        dataGridView.DefaultCellStyle.SelectionForeColor = theme.DataGridSelectionForeColor;
        dataGridView.RowsDefaultCellStyle.BackColor = theme.DataGridRowsBackColor;
        dataGridView.AlternatingRowsDefaultCellStyle.BackColor = theme.DataGridAltRowsBackColor;
        dataGridView.DefaultCellStyle.Font =
            new Font(dataGridView.Font.Name, WipAppVariables.ThemeFontSize, dataGridView.Font.Style);

        if (theme.DataGridColumnHeadersForeColor.HasValue)
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = theme.DataGridColumnHeadersForeColor.Value;
        if (theme.DataGridColumnHeadersBackColor.HasValue)
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = theme.DataGridColumnHeadersBackColor.Value;
        if (theme.DataGridRowHeadersBackColor.HasValue)
            dataGridView.RowHeadersDefaultCellStyle.BackColor = theme.DataGridRowHeadersBackColor.Value;

        SizeDataGrid(dataGridView);
        dataGridView.AutoResizeColumns();
    }

    public static IEnumerable<string> GetThemeNames()
    {
        return Themes.Keys;
    }

    public static void SizeDataGrid(DataGridView dataGridView)
    {
        try
        {
            if (dataGridView.Columns.Count == 0)
            {
                AppLogger.Log("SizeDataGrid: No columns to size.");
            }
            else
            {
                int i;
                for (i = 0; i < dataGridView.Columns.Count; i++)
                    dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dataGridView.Columns[i - 1].Frozen = false;
                dataGridView.Columns[i - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView.ColumnHeadersVisible = true;
                dataGridView.RowHeadersVisible = false;
                dataGridView.Font = new Font(dataGridView.Font.Name, WipAppVariables.ThemeFontSize,
                    dataGridView.Font.Style);

                AppLogger.Log("SizeDataGrid: DataGridView sized successfully.");
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in SizeDataGrid: " + ex.Message);
        }
    }
}