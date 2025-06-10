using MTM_WIP_Application.Logging;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MTM_WIP_Application.Core;

/// <summary>
/// 
/// Testing Passed: 05/31/2025
/// 
/// DgvDesigner provides utilities for applying visual themes to Windows Forms and DataGridView controls.
/// 
/// Features:
/// - Recursively applies theme colors, fonts, and styles to forms and all child controls.
/// - Supports control-specific theming for buttons, tab controls, text boxes, and labels.
/// - Customizes DataGridView appearance, including background, headers, selection, and column sizing.
/// - Exposes helper methods for theme information, color retrieval, and testing support.
/// - Integrates with WipAppVariables for theme selection and AppLogger for logging actions and errors.
/// 
/// Usage:
/// 1. Call ApplyTheme with a Form to apply the current global theme to the form and its controls.
/// 2. Use SizeDataGrid to optimize DataGridView column sizing.
/// 3. Use static helpers to query theme names, colors, and test theme application.
/// </summary>
public class DgvDesigner
{
    #region Theme Application

    public static void ApplyTheme(Form form)
    {
        var requestedThemeName = WipAppVariables.WipDataGridTheme ?? "Default (Black and White)";
        var theme = AppThemes.GetTheme(requestedThemeName);
        var themeName = AppThemes.ThemeExists(requestedThemeName) ? requestedThemeName : "Default (Black and White)";

        form.SuspendLayout();

        form.BackColor = theme.FormBackColor;
        form.ForeColor = theme.FormForeColor;
        form.Font = theme.FormFont ?? new Font(form.Font.Name, WipAppVariables.ThemeFontSize, form.Font.Style);

        if (!form.Text.Contains($"[{themeName}]"))
        {
            var idx = form.Text.LastIndexOf('[');
            if (idx > 0)
                form.Text = form.Text[..idx].TrimEnd();
            form.Text = @$"{form.Text} [{themeName}]";
        }

        ApplyThemeToControls(form.Controls, theme);

        form.ResumeLayout();
        AppLogger.Log($"Global theme '{themeName}' applied to form '{form.Name}'.");
    }

    private static void ApplyThemeToControls(Control.ControlCollection controls, AppThemes.AppTheme theme)
    {
        foreach (Control ctrl in controls)
        {
            if (ctrl is DataGridView dgv)
            {
                ApplyThemeToDataGridView(dgv, theme);
            }
            else
            {
                ctrl.BackColor = theme.FormBackColor;
                ctrl.ForeColor = theme.FormForeColor;
                ctrl.Font = theme.FormFont ?? new Font(ctrl.Font.Name, WipAppVariables.ThemeFontSize, ctrl.Font.Style);
                ApplyControlSpecificTheme(ctrl, theme);
            }

            if (ctrl.HasChildren)
                ApplyThemeToControls(ctrl.Controls, theme);
        }
    }

    private static void ApplyControlSpecificTheme(Control control, AppThemes.AppTheme theme)
    {
        switch (control)
        {
            case Button btn:
                if (theme.ButtonBackColor.HasValue) btn.BackColor = theme.ButtonBackColor.Value;
                if (theme.ButtonForeColor.HasValue) btn.ForeColor = theme.ButtonForeColor.Value;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
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
        }
    }

    public static void ApplyThemeToDataGridView(DataGridView dataGridView, AppThemes.AppTheme theme)
    {
        dataGridView.Dock = DockStyle.Fill; // Ensure the DataGridView fills its parent

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

    #endregion

    #region DataGrid Utilities

    public static void SizeDataGrid(DataGridView dataGridView)
    {
        try
        {
            if (dataGridView.Columns.Count == 0)
            {
                AppLogger.Log("SizeDataGrid: No columns to size.");
                return;
            }

            for (var i = 0; i < dataGridView.Columns.Count; i++)
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dataGridView.Columns[dataGridView.Columns.Count - 1].Frozen = false;
            dataGridView.Columns[dataGridView.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView.ColumnHeadersVisible = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.Font =
                new Font(dataGridView.Font.Name, WipAppVariables.ThemeFontSize, dataGridView.Font.Style);

            AppLogger.Log("SizeDataGrid: DataGridView sized successfully.");
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in SizeDataGrid: " + ex.Message);
        }
    }

    #endregion

    #region Theme Information Helpers

    public static IEnumerable<string> GetThemeNames()
    {
        return AppThemes.GetThemeNames();
    }

    public static string GetEffectiveThemeName()
    {
        return AppThemes.GetEffectiveThemeName();
    }

    public static bool ThemeExists(string themeName)
    {
        return AppThemes.ThemeExists(themeName);
    }

    public static Color GetThemeColor(string themeName, string propertyName)
    {
        return AppThemes.GetThemeColor(themeName, propertyName);
    }

    public static float GetDefaultFontSize()
    {
        return WipAppVariables.ThemeFontSize;
    }

    #endregion

    #region Testing Helper Methods

    public static object GetThemeForTesting(string themeName)
    {
        if (!AppThemes.ThemeExists(themeName))
            throw new InvalidOperationException($"Theme '{themeName}' not found.");

        return AppThemes.GetTheme(themeName);
    }

    public static void ApplyThemeToControlForTesting(Control control, string themeName)
    {
        var theme = AppThemes.GetTheme(themeName);

        control.BackColor = theme.FormBackColor;
        control.ForeColor = theme.FormForeColor;
        control.Font = theme.FormFont ?? new Font(control.Font.Name, WipAppVariables.ThemeFontSize, control.Font.Style);

        if (control is Button btn)
        {
            if (theme.ButtonBackColor.HasValue) btn.BackColor = theme.ButtonBackColor.Value;
            if (theme.ButtonForeColor.HasValue) btn.ForeColor = theme.ButtonForeColor.Value;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }

        ApplyThemeToControls(control.Controls, theme);
    }

    public static void ApplySpecificThemePropertiesToControl(Control control, string themeName,
        params string[] propertyNames)
    {
        var theme = AppThemes.GetTheme(themeName);

        foreach (var propName in propertyNames)
        {
            var themeProperty = typeof(AppThemes.AppTheme).GetProperty(propName)
                                ?? throw new InvalidOperationException($"Property '{propName}' not found on AppTheme.");

            var value = themeProperty.GetValue(theme);

            switch (propName)
            {
                case "FormBackColor" when value is Color c:
                    control.BackColor = c;
                    break;
                case "FormForeColor" when value is Color c:
                    control.ForeColor = c;
                    break;
                case "FormFont" when value is Font f:
                    control.Font = f;
                    break;
            }
        }
    }

    #endregion
}