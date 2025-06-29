using System.Collections.Concurrent;
using System.Diagnostics;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Core;

#region Core_Themes

public static class Core_Themes
{
    #region DelegatesAndThemeAppliers

    private delegate void ControlThemeApplier(Control control, Model_UserUiColors colors);

    // Thread-safe for possible concurrent calls, and easy extension for new control types.
    private static readonly ConcurrentDictionary<Type, ControlThemeApplier> ThemeAppliers = new()
    {
        [typeof(Button)] = ThemeAppliersInternal.ApplyButtonTheme,
        [typeof(TabControl)] = ThemeAppliersInternal.ApplyTabControlTheme,
        [typeof(TabPage)] = ThemeAppliersInternal.ApplyTabPageTheme,
        [typeof(TextBox)] = ThemeAppliersInternal.ApplyTextBoxTheme,
        [typeof(MaskedTextBox)] = ThemeAppliersInternal.ApplyMaskedTextBoxTheme,
        [typeof(RichTextBox)] = ThemeAppliersInternal.ApplyRichTextBoxTheme,
        [typeof(ComboBox)] = ThemeAppliersInternal.ApplyComboBoxTheme,
        [typeof(ListBox)] = ThemeAppliersInternal.ApplyListBoxTheme,
        [typeof(CheckedListBox)] = ThemeAppliersInternal.ApplyCheckedListBoxTheme,
        [typeof(Label)] = ThemeAppliersInternal.ApplyLabelTheme,
        [typeof(RadioButton)] = ThemeAppliersInternal.ApplyRadioButtonTheme,
        [typeof(CheckBox)] = ThemeAppliersInternal.ApplyCheckBoxTheme,
        [typeof(TreeView)] = ThemeAppliersInternal.ApplyTreeViewTheme,
        [typeof(ListView)] = ThemeAppliersInternal.ApplyListViewTheme,
        [typeof(MenuStrip)] = ThemeAppliersInternal.ApplyMenuStripTheme,
        [typeof(StatusStrip)] = ThemeAppliersInternal.ApplyStatusStripTheme,
        [typeof(ToolStrip)] = ThemeAppliersInternal.ApplyToolStripTheme,
        [typeof(GroupBox)] = ThemeAppliersInternal.ApplyGroupBoxTheme,
        [typeof(Panel)] = ThemeAppliersInternal.ApplyPanelTheme,
        [typeof(SplitContainer)] = ThemeAppliersInternal.ApplySplitContainerTheme,
        [typeof(FlowLayoutPanel)] = ThemeAppliersInternal.ApplyFlowLayoutPanelTheme,
        [typeof(TableLayoutPanel)] = ThemeAppliersInternal.ApplyTableLayoutPanelTheme,
        [typeof(DateTimePicker)] = ThemeAppliersInternal.ApplyDateTimePickerTheme,
        [typeof(MonthCalendar)] = ThemeAppliersInternal.ApplyMonthCalendarTheme,
        [typeof(NumericUpDown)] = ThemeAppliersInternal.ApplyNumericUpDownTheme,
        [typeof(TrackBar)] = ThemeAppliersInternal.ApplyTrackBarTheme,
        [typeof(ProgressBar)] = ThemeAppliersInternal.ApplyProgressBarTheme,
        [typeof(HScrollBar)] = ThemeAppliersInternal.ApplyHScrollBarTheme,
        [typeof(VScrollBar)] = ThemeAppliersInternal.ApplyVScrollBarTheme,
        [typeof(PictureBox)] = ThemeAppliersInternal.ApplyPictureBoxTheme,
        [typeof(PropertyGrid)] = ThemeAppliersInternal.ApplyPropertyGridTheme,
        [typeof(DomainUpDown)] = ThemeAppliersInternal.ApplyDomainUpDownTheme,
        [typeof(WebBrowser)] = ThemeAppliersInternal.ApplyWebBrowserTheme,
        [typeof(UserControl)] = ThemeAppliersInternal.ApplyUserControlTheme,
        [typeof(LinkLabel)] = ThemeAppliersInternal.ApplyLinkLabelTheme,
        [typeof(ContextMenuStrip)] = ThemeAppliersInternal.ApplyContextMenuTheme
    };

    #endregion

    #region Public API

    public static void ApplyTheme(Form form)
    {
        Debug.WriteLine($"[DEBUG] Applying theme to form '{form.Name}'");
        var theme = Core_AppThemes.GetCurrentTheme();
        var themeName = Core_AppThemes.GetEffectiveThemeName();

        form.SuspendLayout();

        SetFormTheme(form, theme, themeName);

        ApplyThemeToControls(form.Controls);

        form.ResumeLayout();
        LoggingUtility.Log($"Global theme '{themeName}' applied to form '{form.Name}'.");
    }

    public static async Task<Model_UserUiColors> GetUserThemeColorsAsync(string userId)
    {
        // 1. Get the theme name for the user from usr_ui_settings
        var themeName = await Dao_User.GetUserThemeNameFromUiSettingsAsync(userId) ?? "Default";

        // 2. Ensure themes are loaded (if not already)
        if (!Core_AppThemes.GetThemeNames().Contains(themeName))
            await Core_AppThemes.LoadThemesFromDatabaseAsync();

        // 3. Get the theme colors from app_themes
        var appTheme = Core_AppThemes.GetTheme(themeName);
        return appTheme.Colors;
    }

    public static void ApplyThemeToDataGridView(DataGridView dataGridView)
    {
        ThemeAppliersInternal.ApplyThemeToDataGridView(dataGridView);
    }

    public static void SizeDataGrid(DataGridView dataGridView)
    {
        ThemeAppliersInternal.SizeDataGrid(dataGridView);
    }

    public static void ApplyFocusHighlighting(Control parentControl)
    {
        var theme = Core_AppThemes.GetCurrentTheme();
        FocusUtils.ApplyFocusEventHandlingToControls(parentControl.Controls, theme.Colors);
    }

    #endregion

    #region Private Helpers

    private static void SetFormTheme(Form form, Core_AppThemes.AppTheme theme, string themeName)
    {
        form.BackColor = theme.Colors.FormBackColor ?? Color.White;
        form.ForeColor = theme.Colors.FormForeColor ?? Color.Black;
        form.Font = theme.FormFont ?? new Font(form.Font.Name, Model_AppVariables.ThemeFontSize, form.Font.Style);

        if (!string.IsNullOrWhiteSpace(themeName))
        {
            var idx = form.Text.LastIndexOf('[');
            if (idx > 0)
                form.Text = form.Text[..idx].TrimEnd();

            // Add theme name and shortcut hint
            var themeDisplay = $"[{themeName}] | Change (Shift + Alt + S)";
            if (!form.Text.Contains(themeDisplay))
                form.Text = @$"{form.Text} {themeDisplay}";
        }
    }

    private static void ApplyThemeToControls(Control.ControlCollection controls)
    {
        var theme = Core_AppThemes.GetCurrentTheme();
        foreach (Control ctrl in controls)
            try
            {
                Debug.WriteLine($"[DEBUG] Applying theme to control '{ctrl.Name}' of type '{ctrl.GetType().Name}'");
                if (ctrl is DataGridView dgv)
                {
                    ApplyThemeToDataGridView(dgv);
                }
                else
                {
                    ApplyBaseThemeColors(ctrl, theme);
                    ApplyControlSpecificTheme(ctrl);
                    FocusUtils.ApplyFocusEventHandling(ctrl, theme.Colors);
                }

                if (ctrl.HasChildren && ctrl.Controls.Count < 10000)
                    ApplyThemeToControls(ctrl.Controls);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in ApplyThemeToControls for '{ctrl.Name}': {ex}");
                LoggingUtility.LogApplicationError(ex);
            }
    }

    private static void ApplyBaseThemeColors(Control control, Core_AppThemes.AppTheme theme)
    {
        var backColor = theme.Colors.FormBackColor ?? Color.White;
        var foreColor = theme.Colors.FormForeColor ?? Color.Black;

        Debug.WriteLine(
            $"[THEME] {control.Name} ({control.GetType().Name}) - Base BackColor: {backColor}, ForeColor: {foreColor}");

        if (control.BackColor != backColor)
            control.BackColor = backColor;
        if (control.ForeColor != foreColor)
            control.ForeColor = foreColor;

        var font = theme.FormFont ??
                   new Font(control.Font.Name, Model_AppVariables.ThemeFontSize, control.Font.Style);
        if (control.Font == null || control.Font.Size != font.Size || control.Font.Name != font.Name)
            control.Font = font;
    }

    private static void ApplyControlSpecificTheme(Control control)
    {
        if (control == null) return;

        var theme = Core_AppThemes.GetCurrentTheme();
        var colors = theme.Colors;

        try
        {
            var controlType = control.GetType();

            // Fast path: Try exact type match first
            if (ThemeAppliers.TryGetValue(controlType, out var applier))
            {
                applier(control, colors);
                return;
            }

            // Use type hierarchy caching to avoid repeated reflection
            var currentType = controlType;
            while (currentType != null && currentType != typeof(object))
            {
                if (ThemeAppliers.TryGetValue(currentType, out applier))
                {
                    ThemeAppliers.TryAdd(controlType, applier);
                    applier(control, colors);
                    return;
                }

                currentType = currentType.BaseType;
            }

            ThemeAppliersInternal.ApplyCustomControlTheme(control, colors);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(
                $"[ERROR] Failed to apply theme to control '{control.Name}' of type '{control.GetType().Name}': {ex.Message}");
            LoggingUtility.LogApplicationError(ex);
            ThemeAppliersInternal.ApplyCustomControlTheme(control, colors);
        }
    }

    #endregion

    #region ThemeAppliersInternal (Nested Static Class Organization)

    // ThemeAppliersInternal: All theming implementations for controls, grouped for clarity.
    private static class ThemeAppliersInternal
    {
        public static void ApplyButtonTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not Button btn) return;

            btn.Margin = new Padding(1);
            var backColor = colors.ButtonBackColor ?? SystemColors.Control;
            var foreColor = colors.ButtonForeColor ?? SystemColors.ControlText;
            Debug.WriteLine($"[THEME] {btn.Name} (Button) - BackColor: {backColor}, ForeColor: {foreColor}");
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatStyle = FlatStyle.System;

            btn.Paint -= Button_AutoShrinkText_Paint;
            btn.Paint += Button_AutoShrinkText_Paint;
        }

        public static void ApplyTabControlTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not TabControl tab) return;
            var backColor = colors.TabControlBackColor ?? colors.FormBackColor ?? Color.FromArgb(30, 30, 30);
            var foreColor = colors.TabControlForeColor ?? colors.FormForeColor ?? Color.White;
            Debug.WriteLine($"[THEME] {tab.Name} (TabControl) - BackColor: {backColor}, ForeColor: {foreColor}");
            tab.BackColor = backColor;
            tab.ForeColor = foreColor;
        }

        public static void ApplyTabPageTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not TabPage tabPage) return;
            if (colors.TabPageBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {tabPage.Name} (TabPage) - BackColor: {colors.TabPageBackColor.Value}");
                tabPage.BackColor = colors.TabPageBackColor.Value;
            }

            if (colors.TabPageForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {tabPage.Name} (TabPage) - ForeColor: {colors.TabPageForeColor.Value}");
                tabPage.ForeColor = colors.TabPageForeColor.Value;
            }
        }

        public static void ApplyTextBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not TextBox txt) return;
            if (colors.TextBoxBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {txt.Name} (TextBox) - BackColor: {colors.TextBoxBackColor.Value}");
                txt.BackColor = colors.TextBoxBackColor.Value;
            }

            if (colors.TextBoxForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {txt.Name} (TextBox) - ForeColor: {colors.TextBoxForeColor.Value}");
                txt.ForeColor = colors.TextBoxForeColor.Value;
            }
        }

        public static void ApplyMaskedTextBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not MaskedTextBox mtxt) return;
            if (colors.MaskedTextBoxBackColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {mtxt.Name} (MaskedTextBox) - BackColor: {colors.MaskedTextBoxBackColor.Value}");
                mtxt.BackColor = colors.MaskedTextBoxBackColor.Value;
            }

            if (colors.MaskedTextBoxForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {mtxt.Name} (MaskedTextBox) - ForeColor: {colors.MaskedTextBoxForeColor.Value}");
                mtxt.ForeColor = colors.MaskedTextBoxForeColor.Value;
            }
        }

        public static void ApplyRichTextBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not RichTextBox rtxt) return;
            if (colors.RichTextBoxBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {rtxt.Name} (RichTextBox) - BackColor: {colors.RichTextBoxBackColor.Value}");
                rtxt.BackColor = colors.RichTextBoxBackColor.Value;
            }

            if (colors.RichTextBoxForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {rtxt.Name} (RichTextBox) - ForeColor: {colors.RichTextBoxForeColor.Value}");
                rtxt.ForeColor = colors.RichTextBoxForeColor.Value;
            }
        }

        public static void ApplyComboBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not ComboBox cb) return;
            if (colors.ComboBoxBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {cb.Name} (ComboBox) - BackColor: {colors.ComboBoxBackColor.Value}");
                cb.BackColor = colors.ComboBoxBackColor.Value;
            }

            if (colors.ComboBoxForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {cb.Name} (ComboBox) - ForeColor: {colors.ComboBoxForeColor.Value}");
                cb.ForeColor = colors.ComboBoxForeColor.Value;
            }
        }

        public static void ApplyListBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not ListBox lb) return;
            if (colors.ListBoxBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {lb.Name} (ListBox) - BackColor: {colors.ListBoxBackColor.Value}");
                lb.BackColor = colors.ListBoxBackColor.Value;
            }

            if (colors.ListBoxForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {lb.Name} (ListBox) - ForeColor: {colors.ListBoxForeColor.Value}");
                lb.ForeColor = colors.ListBoxForeColor.Value;
            }
        }

        public static void ApplyCheckedListBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not CheckedListBox clb) return;
            if (colors.CheckedListBoxBackColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {clb.Name} (CheckedListBox) - BackColor: {colors.CheckedListBoxBackColor.Value}");
                clb.BackColor = colors.CheckedListBoxBackColor.Value;
            }

            if (colors.CheckedListBoxForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {clb.Name} (CheckedListBox) - ForeColor: {colors.CheckedListBoxForeColor.Value}");
                clb.ForeColor = colors.CheckedListBoxForeColor.Value;
            }
        }

        public static void ApplyLabelTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not Label lbl) return;
            if (colors.LabelBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {lbl.Name} (Label) - BackColor: {colors.LabelBackColor.Value}");
                lbl.BackColor = colors.LabelBackColor.Value;
            }

            if (colors.LabelForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {lbl.Name} (Label) - ForeColor: {colors.LabelForeColor.Value}");
                lbl.ForeColor = colors.LabelForeColor.Value;
            }
        }

        public static void ApplyRadioButtonTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not RadioButton rb) return;
            if (colors.RadioButtonBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {rb.Name} (RadioButton) - BackColor: {colors.RadioButtonBackColor.Value}");
                rb.BackColor = colors.RadioButtonBackColor.Value;
            }

            if (colors.RadioButtonForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {rb.Name} (RadioButton) - ForeColor: {colors.RadioButtonForeColor.Value}");
                rb.ForeColor = colors.RadioButtonForeColor.Value;
            }
        }

        public static void ApplyCheckBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not CheckBox cbx) return;
            if (colors.CheckBoxBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {cbx.Name} (CheckBox) - BackColor: {colors.CheckBoxBackColor.Value}");
                cbx.BackColor = colors.CheckBoxBackColor.Value;
            }

            if (colors.CheckBoxForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {cbx.Name} (CheckBox) - ForeColor: {colors.CheckBoxForeColor.Value}");
                cbx.ForeColor = colors.CheckBoxForeColor.Value;
            }
        }

        public static void ApplyTreeViewTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not TreeView tv) return;
            if (colors.TreeViewBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {tv.Name} (TreeView) - BackColor: {colors.TreeViewBackColor.Value}");
                tv.BackColor = colors.TreeViewBackColor.Value;
            }

            if (colors.TreeViewForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {tv.Name} (TreeView) - ForeColor: {colors.TreeViewForeColor.Value}");
                tv.ForeColor = colors.TreeViewForeColor.Value;
            }

            if (colors.TreeViewLineColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {tv.Name} (TreeView) - LineColor: {colors.TreeViewLineColor.Value}");
                tv.LineColor = colors.TreeViewLineColor.Value;
            }
        }

        public static void ApplyListViewTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not ListView lv) return;
            if (colors.ListViewBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {lv.Name} (ListView) - BackColor: {colors.ListViewBackColor.Value}");
                lv.BackColor = colors.ListViewBackColor.Value;
            }

            if (colors.ListViewForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {lv.Name} (ListView) - ForeColor: {colors.ListViewForeColor.Value}");
                lv.ForeColor = colors.ListViewForeColor.Value;
            }
        }

        public static void ApplyMenuStripTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not MenuStrip ms) return;
            if (colors.MenuStripBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {ms.Name} (MenuStrip) - BackColor: {colors.MenuStripBackColor.Value}");
                ms.BackColor = colors.MenuStripBackColor.Value;
            }

            if (colors.MenuStripForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {ms.Name} (MenuStrip) - ForeColor: {colors.MenuStripForeColor.Value}");
                ms.ForeColor = colors.MenuStripForeColor.Value;
            }
        }

        public static void ApplyStatusStripTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not StatusStrip ss) return;
            if (colors.StatusStripBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {ss.Name} (StatusStrip) - BackColor: {colors.StatusStripBackColor.Value}");
                ss.BackColor = colors.StatusStripBackColor.Value;
            }

            if (colors.StatusStripForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {ss.Name} (StatusStrip) - ForeColor: {colors.StatusStripForeColor.Value}");
                ss.ForeColor = colors.StatusStripForeColor.Value;
            }
        }

        public static void ApplyToolStripTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not ToolStrip ts) return;
            if (colors.ToolStripBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {ts.Name} (ToolStrip) - BackColor: {colors.ToolStripBackColor.Value}");
                ts.BackColor = colors.ToolStripBackColor.Value;
            }

            if (colors.ToolStripForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {ts.Name} (ToolStrip) - ForeColor: {colors.ToolStripForeColor.Value}");
                ts.ForeColor = colors.ToolStripForeColor.Value;
            }
        }

        public static void ApplyGroupBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not GroupBox gb) return;
            if (colors.GroupBoxBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {gb.Name} (GroupBox) - BackColor: {colors.GroupBoxBackColor.Value}");
                gb.BackColor = colors.GroupBoxBackColor.Value;
            }

            if (colors.GroupBoxForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {gb.Name} (GroupBox) - ForeColor: {colors.GroupBoxForeColor.Value}");
                gb.ForeColor = colors.GroupBoxForeColor.Value;
            }
        }

        public static void ApplyPanelTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not Panel pnl) return;
            if (colors.PanelForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {pnl.Name} (Panel) - ForeColor: {colors.PanelForeColor.Value}");
                pnl.ForeColor = colors.PanelForeColor.Value;
            }
        }

        public static void ApplySplitContainerTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not SplitContainer sc) return;
            if (colors.SplitContainerBackColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {sc.Name} (SplitContainer) - BackColor: {colors.SplitContainerBackColor.Value}");
                sc.BackColor = colors.SplitContainerBackColor.Value;
            }

            if (colors.SplitContainerForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {sc.Name} (SplitContainer) - ForeColor: {colors.SplitContainerForeColor.Value}");
                sc.ForeColor = colors.SplitContainerForeColor.Value;
            }
        }

        public static void ApplyFlowLayoutPanelTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not FlowLayoutPanel flp) return;
            if (colors.FlowLayoutPanelForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {flp.Name} (FlowLayoutPanel) - ForeColor: {colors.FlowLayoutPanelForeColor.Value}");
                flp.ForeColor = colors.FlowLayoutPanelForeColor.Value;
            }
        }

        public static void ApplyTableLayoutPanelTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not TableLayoutPanel tlp) return;
            if (colors.TableLayoutPanelForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {tlp.Name} (TableLayoutPanel) - ForeColor: {colors.TableLayoutPanelForeColor.Value}");
                tlp.ForeColor = colors.TableLayoutPanelForeColor.Value;
            }
        }

        public static void ApplyDateTimePickerTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not DateTimePicker dtp) return;
            if (colors.DateTimePickerBackColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {dtp.Name} (DateTimePicker) - BackColor: {colors.DateTimePickerBackColor.Value}");
                dtp.BackColor = colors.DateTimePickerBackColor.Value;
            }

            if (colors.DateTimePickerForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {dtp.Name} (DateTimePicker) - ForeColor: {colors.DateTimePickerForeColor.Value}");
                dtp.ForeColor = colors.DateTimePickerForeColor.Value;
            }
        }

        public static void ApplyMonthCalendarTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not MonthCalendar mc) return;
            if (colors.MonthCalendarBackColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {mc.Name} (MonthCalendar) - BackColor: {colors.MonthCalendarBackColor.Value}");
                mc.BackColor = colors.MonthCalendarBackColor.Value;
            }

            if (colors.MonthCalendarForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {mc.Name} (MonthCalendar) - ForeColor: {colors.MonthCalendarForeColor.Value}");
                mc.ForeColor = colors.MonthCalendarForeColor.Value;
            }

            if (colors.MonthCalendarTitleBackColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {mc.Name} (MonthCalendar) - TitleBackColor: {colors.MonthCalendarTitleBackColor.Value}");
                mc.TitleBackColor = colors.MonthCalendarTitleBackColor.Value;
            }

            if (colors.MonthCalendarTitleForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {mc.Name} (MonthCalendar) - TitleForeColor: {colors.MonthCalendarTitleForeColor.Value}");
                mc.TitleForeColor = colors.MonthCalendarTitleForeColor.Value;
            }

            if (colors.MonthCalendarTrailingForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {mc.Name} (MonthCalendar) - TrailingForeColor: {colors.MonthCalendarTrailingForeColor.Value}");
                mc.TrailingForeColor = colors.MonthCalendarTrailingForeColor.Value;
            }
        }

        public static void ApplyNumericUpDownTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not NumericUpDown nud) return;
            if (colors.NumericUpDownBackColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {nud.Name} (NumericUpDown) - BackColor: {colors.NumericUpDownBackColor.Value}");
                nud.BackColor = colors.NumericUpDownBackColor.Value;
            }

            if (colors.NumericUpDownForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {nud.Name} (NumericUpDown) - ForeColor: {colors.NumericUpDownForeColor.Value}");
                nud.ForeColor = colors.NumericUpDownForeColor.Value;
            }
        }

        public static void ApplyTrackBarTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not TrackBar tb) return;
            if (colors.TrackBarBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {tb.Name} (TrackBar) - BackColor: {colors.TrackBarBackColor.Value}");
                tb.BackColor = colors.TrackBarBackColor.Value;
            }

            if (colors.TrackBarForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {tb.Name} (TrackBar) - ForeColor: {colors.TrackBarForeColor.Value}");
                tb.ForeColor = colors.TrackBarForeColor.Value;
            }
        }

        public static void ApplyProgressBarTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not ProgressBar pb) return;
            if (colors.ProgressBarBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {pb.Name} (ProgressBar) - BackColor: {colors.ProgressBarBackColor.Value}");
                pb.BackColor = colors.ProgressBarBackColor.Value;
            }

            if (colors.ProgressBarForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {pb.Name} (ProgressBar) - ForeColor: {colors.ProgressBarForeColor.Value}");
                pb.ForeColor = colors.ProgressBarForeColor.Value;
            }
        }

        public static void ApplyHScrollBarTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not HScrollBar hsb) return;
            if (colors.HScrollBarBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {hsb.Name} (HScrollBar) - BackColor: {colors.HScrollBarBackColor.Value}");
                hsb.BackColor = colors.HScrollBarBackColor.Value;
            }

            if (colors.HScrollBarForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {hsb.Name} (HScrollBar) - ForeColor: {colors.HScrollBarForeColor.Value}");
                hsb.ForeColor = colors.HScrollBarForeColor.Value;
            }
        }

        public static void ApplyVScrollBarTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not VScrollBar vsb) return;
            if (colors.VScrollBarBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {vsb.Name} (VScrollBar) - BackColor: {colors.VScrollBarBackColor.Value}");
                vsb.BackColor = colors.VScrollBarBackColor.Value;
            }

            if (colors.VScrollBarForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {vsb.Name} (VScrollBar) - ForeColor: {colors.VScrollBarForeColor.Value}");
                vsb.ForeColor = colors.VScrollBarForeColor.Value;
            }
        }

        public static void ApplyPictureBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not PictureBox pic) return;
            if (colors.PictureBoxBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {pic.Name} (PictureBox) - BackColor: {colors.PictureBoxBackColor.Value}");
                pic.BackColor = colors.PictureBoxBackColor.Value;
            }
        }

        public static void ApplyPropertyGridTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not PropertyGrid pg) return;
            if (colors.PropertyGridBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {pg.Name} (PropertyGrid) - BackColor: {colors.PropertyGridBackColor.Value}");
                pg.BackColor = colors.PropertyGridBackColor.Value;
            }

            if (colors.PropertyGridForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {pg.Name} (PropertyGrid) - ForeColor: {colors.PropertyGridForeColor.Value}");
                pg.ForeColor = colors.PropertyGridForeColor.Value;
            }
        }

        public static void ApplyDomainUpDownTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not DomainUpDown dud) return;
            if (colors.DomainUpDownBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {dud.Name} (DomainUpDown) - BackColor: {colors.DomainUpDownBackColor.Value}");
                dud.BackColor = colors.DomainUpDownBackColor.Value;
            }

            if (colors.DomainUpDownForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {dud.Name} (DomainUpDown) - ForeColor: {colors.DomainUpDownForeColor.Value}");
                dud.ForeColor = colors.DomainUpDownForeColor.Value;
            }
        }

        public static void ApplyWebBrowserTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not WebBrowser wb) return;
            if (colors.WebBrowserBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {wb.Name} (WebBrowser) - BackColor: {colors.WebBrowserBackColor.Value}");
                wb.BackColor = colors.WebBrowserBackColor.Value;
            }
        }

        public static void ApplyUserControlTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not UserControl uc) return;
            if (colors.UserControlBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {uc.Name} (UserControl) - BackColor: {colors.UserControlBackColor.Value}");
                uc.BackColor = colors.UserControlBackColor.Value;
            }

            if (colors.UserControlForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {uc.Name} (UserControl) - ForeColor: {colors.UserControlForeColor.Value}");
                uc.ForeColor = colors.UserControlForeColor.Value;
            }
        }

        public static void ApplyLinkLabelTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not LinkLabel ll) return;
            if (colors.LinkLabelLinkColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {ll.Name} (LinkLabel) - LinkColor: {colors.LinkLabelLinkColor.Value}");
                ll.LinkColor = colors.LinkLabelLinkColor.Value;
            }

            if (colors.LinkLabelActiveLinkColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {ll.Name} (LinkLabel) - ActiveLinkColor: {colors.LinkLabelActiveLinkColor.Value}");
                ll.ActiveLinkColor = colors.LinkLabelActiveLinkColor.Value;
            }

            if (colors.LinkLabelVisitedLinkColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {ll.Name} (LinkLabel) - VisitedLinkColor: {colors.LinkLabelVisitedLinkColor.Value}");
                ll.VisitedLinkColor = colors.LinkLabelVisitedLinkColor.Value;
            }

            if (colors.LinkLabelBackColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {ll.Name} (LinkLabel) - BackColor: {colors.LinkLabelBackColor.Value}");
                ll.BackColor = colors.LinkLabelBackColor.Value;
            }

            if (colors.LinkLabelForeColor.HasValue)
            {
                Debug.WriteLine($"[THEME] {ll.Name} (LinkLabel) - ForeColor: {colors.LinkLabelForeColor.Value}");
                ll.ForeColor = colors.LinkLabelForeColor.Value;
            }
        }

        public static void ApplyContextMenuTheme(Control control, Model_UserUiColors colors)
        {
            if (control is not ContextMenuStrip cms) return;
            if (colors.ContextMenuBackColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {cms.Name} (ContextMenuStrip) - BackColor: {colors.ContextMenuBackColor.Value}");
                cms.BackColor = colors.ContextMenuBackColor.Value;
            }

            if (colors.ContextMenuForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {cms.Name} (ContextMenuStrip) - ForeColor: {colors.ContextMenuForeColor.Value}");
                cms.ForeColor = colors.ContextMenuForeColor.Value;
            }
        }

        public static void ApplyCustomControlTheme(Control control, Model_UserUiColors colors)
        {
            if (colors.CustomControlBackColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {control.Name} (CustomControl) - BackColor: {colors.CustomControlBackColor.Value}");
                control.BackColor = colors.CustomControlBackColor.Value;
            }

            if (colors.CustomControlForeColor.HasValue)
            {
                Debug.WriteLine(
                    $"[THEME] {control.Name} (CustomControl) - ForeColor: {colors.CustomControlForeColor.Value}");
                control.ForeColor = colors.CustomControlForeColor.Value;
            }
        }

        public static void ApplyThemeToDataGridView(DataGridView dataGridView)
        {
            // You can add debug output here as needed
        }

        public static void SizeDataGrid(DataGridView dataGridView)
        {
            // You can add debug output here as needed
        }

        private static void Button_AutoShrinkText_Paint(object? sender, PaintEventArgs e)
        {
            // You can add debug output here as needed
        }
    }

    #endregion

    #region FocusUtils (Nested Static Class Organization)

    public static class FocusUtils
    {
        public static void ApplyFocusEventHandling(Control control, Model_UserUiColors colors)
        {
            if (!CanControlReceiveFocus(control))
                return;

            Apply(control, colors);
        }

        public static void ApplyFocusEventHandlingToControls(Control.ControlCollection controls,
            Model_UserUiColors colors)
        {
            foreach (Control ctrl in controls)
            {
                ApplyFocusEventHandling(ctrl, colors);

                if (ctrl.HasChildren)
                    ApplyFocusEventHandlingToControls(ctrl.Controls, colors);
            }
        }

        // Event Handlers
        private static void Control_Enter_Handler(object? sender, EventArgs e)
        {
            if (sender is Control ctrl && ctrl.Focused)
            {
                var colors = Core_AppThemes.GetCurrentTheme().Colors;
                var focusBackColor = colors.ControlFocusedBackColor ?? Color.LightBlue;
                var normalForeColor = GetControlThemeForeColor(ctrl, colors);
                ctrl.BackColor = focusBackColor;
                ctrl.ForeColor = normalForeColor;
                if (ctrl is TextBox tb)
                    tb.SelectAll();
                else if (ctrl is MaskedTextBox mtb)
                    mtb.SelectAll();
                else if (ctrl is RichTextBox rtb)
                    rtb.SelectAll();
                else if (ctrl is ComboBox cb && cb.DropDownStyle != ComboBoxStyle.DropDownList)
                    cb.SelectAll();
            }
        }

        private static void Control_Leave_Handler(object? sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                var colors = Core_AppThemes.GetCurrentTheme().Colors;
                var normalBackColor = GetControlThemeBackColor(ctrl, colors);
                ctrl.BackColor = normalBackColor;
            }
        }

        private static void TextBox_Click_SelectAll(object? sender, EventArgs e)
        {
            if (sender is TextBox tb)
                tb.SelectAll();
        }

        private static void ComboBox_DropDown_SelectAll(object? sender, EventArgs e)
        {
            if (sender is ComboBox cb && cb.DropDownStyle != ComboBoxStyle.DropDownList)
                cb.SelectAll();
        }

        // Synchronous handler attachment (legacy)
        private static void Apply(Control control, Model_UserUiColors colors)
        {
            control.Enter -= Control_Enter_Handler;
            control.Leave -= Control_Leave_Handler;
            if (control is TextBox tb) tb.Click -= TextBox_Click_SelectAll;
            if (control is ComboBox cb) cb.DropDown -= ComboBox_DropDown_SelectAll;

            control.Enter += Control_Enter_Handler;
            control.Leave += Control_Leave_Handler;

            if (control is TextBox tbx) tbx.Click += TextBox_Click_SelectAll;
            if (control is ComboBox cbx) cbx.DropDown += ComboBox_DropDown_SelectAll;
        }

        public static bool CanControlReceiveFocus(Control control)
        {
            if (!control.Enabled || !control.Visible || !control.TabStop)
                return false;

            return control switch
            {
                CheckedListBox => false,
                TextBox => true,
                ComboBox => true,
                RichTextBox => true,
                MaskedTextBox => true,
                NumericUpDown => true,
                DateTimePicker => true,
                ListBox => false,
                TreeView => false,
                ListView => false,
                TrackBar => false,
                DomainUpDown => false,
                Button => false,
                CheckBox => false,
                RadioButton => false,
                Label => false,
                Panel => false,
                GroupBox => false,
                PictureBox => false,
                ProgressBar => false,
                _ => false
            };
        }

        private static Color GetControlThemeBackColor(Control control, Model_UserUiColors colors)
        {
            return control switch
            {
                TextBox => colors.TextBoxBackColor ?? colors.ControlBackColor ?? Color.White,
                ComboBox => colors.ComboBoxBackColor ?? colors.ControlBackColor ?? Color.White,
                RichTextBox => colors.RichTextBoxBackColor ?? colors.ControlBackColor ?? Color.White,
                MaskedTextBox => colors.MaskedTextBoxBackColor ?? colors.ControlBackColor ?? Color.White,
                NumericUpDown => colors.NumericUpDownBackColor ?? colors.ControlBackColor ?? Color.White,
                DateTimePicker => colors.DateTimePickerBackColor ?? colors.ControlBackColor ?? Color.White,
                _ => colors.ControlBackColor ?? Color.White
            };
        }

        private static Color GetControlThemeForeColor(Control control, Model_UserUiColors colors)
        {
            return control switch
            {
                TextBox => colors.TextBoxForeColor ?? colors.ControlForeColor ?? Color.Black,
                ComboBox => colors.ComboBoxForeColor ?? colors.ControlForeColor ?? Color.Black,
                RichTextBox => colors.RichTextBoxForeColor ?? colors.ControlForeColor ?? Color.Black,
                MaskedTextBox => colors.MaskedTextBoxForeColor ?? colors.ControlForeColor ?? Color.Black,
                NumericUpDown => colors.NumericUpDownForeColor ?? colors.ControlForeColor ?? Color.Black,
                DateTimePicker => colors.DateTimePickerForeColor ?? colors.ControlForeColor ?? Color.Black,
                _ => colors.ControlForeColor ?? Color.Black
            };
        }
    }

    #endregion
}

#endregion