using MTM_Inventory_Application.Controls.Addons;
using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Core;

public static class Core_Themes
{
    private delegate void ControlThemeApplier(Control control, Model_UserUiColors colors);

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
        [typeof(ContextMenuStrip)] = ThemeAppliersInternal.ApplyContextMenuTheme,
        [typeof(Control_QuickButtons)] = ThemeAppliersInternal.ApplyQuickButtonsTheme,
        [typeof(Control_AdvancedInventory)] = ThemeAppliersInternal.ApplyAdvancedInventoryTheme,
        [typeof(Controls.Shared.ProgressBarUserControl)] = ThemeAppliersInternal.ApplyProgressBarUserControlTheme,
        [typeof(Control_AdvancedRemove)] = ThemeAppliersInternal.ApplyAdvancedRemoveTheme,
        [typeof(ConnectionStrengthControl)] = ThemeAppliersInternal.ApplyConnectionStrengthTheme,
        [typeof(ControlInventoryTab)] = ThemeAppliersInternal.ApplyInventoryTabTheme,
        [typeof(ControlRemoveTab)] = ThemeAppliersInternal.ApplyRemoveTabTheme,
        [typeof(ControlTransferTab)] = ThemeAppliersInternal.ApplyTransferTabTheme
    };

    public static void ApplyTheme(Form form)
    {
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
        Model_AppVariables.ThemeName = await Dao_User.GetSettingsJsonAsync("Theme_Name", userId, true) ?? "Default";
        if (!Core_AppThemes.GetThemeNames().Contains(Model_AppVariables.ThemeName))
            await Core_AppThemes.LoadThemesFromDatabaseAsync();
        var appTheme = Core_AppThemes.GetTheme(Model_AppVariables.ThemeName);
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
            var themeDisplay = $"[Privlage Type: {Model_AppVariables.User} {Model_AppVariables.UserTypeAdmin}] | Change (Shift + Alt + S)";
            if (!form.Text.Contains(themeDisplay))
                form.Text = @$"{form.Text} {themeDisplay}";
        }
    }

    private static void LogControlColor(Control ctrl, string colorType, string colorSource, Color colorValue)
    {
        var themeName = Core_AppThemes.GetEffectiveThemeName();

        Debug.WriteLine(
            $"[THEME] {ctrl.Name} ({ctrl.GetType().Name}) - {colorType}: {colorSource} = {colorValue} | Theme: {themeName}");
    }

    private static void ApplyThemeToControls(Control.ControlCollection controls)
    {
        var theme = Core_AppThemes.GetCurrentTheme();
        foreach (Control ctrl in controls)
            try
            {
                if (ctrl is DataGridView dgv)
                {
                    ApplyThemeToDataGridView(dgv);
                    if (theme.Colors.DataGridBackColor.HasValue)
                        LogControlColor(dgv, "BackColor", "DataGridBackColor", theme.Colors.DataGridBackColor.Value);
                    if (theme.Colors.DataGridForeColor.HasValue)
                        LogControlColor(dgv, "ForeColor", "DataGridForeColor", theme.Colors.DataGridForeColor.Value);
                }
                else
                {
                    var backColor = theme.Colors.FormBackColor ?? Color.White;
                    var foreColor = theme.Colors.FormForeColor ?? Color.Black;
                    LogControlColor(ctrl, "BackColor",
                        theme.Colors.FormBackColor.HasValue ? "FormBackColor" : "Default", backColor);
                    LogControlColor(ctrl, "ForeColor",
                        theme.Colors.FormForeColor.HasValue ? "FormForeColor" : "Default", foreColor);

                    ApplyBaseThemeColors(ctrl, theme);
                    ApplyControlSpecificTheme(ctrl);
                    FocusUtils.ApplyFocusEventHandling(ctrl, theme.Colors);
                }

                if (ctrl.HasChildren && ctrl.Controls.Count < 10000)
                    ApplyThemeToControls(ctrl.Controls);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
    }

    private static void ApplyBaseThemeColors(Control control, Core_AppThemes.AppTheme theme)
    {
        var backColor = theme.Colors.FormBackColor ?? Color.White;
        var foreColor = theme.Colors.FormForeColor ?? Color.Black;
        if (control.BackColor != backColor)
            control.BackColor = backColor;
        if (control.ForeColor != foreColor)
            control.ForeColor = foreColor;
        var font = theme.FormFont ?? new Font(control.Font.Name, Model_AppVariables.ThemeFontSize, control.Font.Style);
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
            if (ThemeAppliers.TryGetValue(controlType, out var applier))
            {
                applier(control, colors);
                return;
            }

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
            LoggingUtility.LogApplicationError(ex);
            ThemeAppliersInternal.ApplyCustomControlTheme(control, colors);
        }
    }

    private static class ThemeAppliersInternal
    {
        public static void ApplyQuickButtonsTheme(Control control, Model_UserUiColors colors)
        {
            if (control is Control_QuickButtons quickButtons)
            {
                quickButtons.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                quickButtons.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
                foreach (var btn in quickButtons.Controls.OfType<Button>())
                {
                    btn.BackColor = colors.ButtonBackColor ?? Color.White;
                    btn.ForeColor = colors.ButtonForeColor ?? Color.Black;
                }
            }
        }

        public static void ApplyAdvancedInventoryTheme(Control control, Model_UserUiColors colors)
        {
            if (control is Control_AdvancedInventory advInv)
            {
                advInv.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                advInv.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
            }
        }

        public static void ApplyAdvancedRemoveTheme(Control control, Model_UserUiColors colors)
        {
            if (control is Control_AdvancedRemove advRem)
            {
                advRem.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                advRem.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
            }
        }

        public static void ApplyConnectionStrengthTheme(Control control, Model_UserUiColors colors)
        {
            if (control is ConnectionStrengthControl conn)
            {
                conn.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                conn.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
                conn.Invalidate();
            }
        }

        public static void ApplyInventoryTabTheme(Control control, Model_UserUiColors colors)
        {
            if (control is ControlInventoryTab tab)
            {
                tab.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                tab.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
            }
        }

        public static void ApplyRemoveTabTheme(Control control, Model_UserUiColors colors)
        {
            if (control is ControlRemoveTab tab)
            {
                tab.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                tab.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
            }
        }

        public static void ApplyTransferTabTheme(Control control, Model_UserUiColors colors)
        {
            if (control is ControlTransferTab tab)
            {
                tab.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                tab.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
            }
        }

        public static void ApplyCustomControlTheme(Control control, Model_UserUiColors colors)
        {
            if (colors.CustomControlBackColor.HasValue) control.BackColor = colors.CustomControlBackColor.Value;
            if (colors.CustomControlForeColor.HasValue) control.ForeColor = colors.CustomControlForeColor.Value;
        }

        // NEW: Owner-draw theme appliers for border, hover, selected, pressed, and other states
        // These route to OwnerDrawThemeHelper
        public static void ApplyOwnerDrawThemes(Control control, Model_UserUiColors colors)
        {
            OwnerDrawThemeHelper.ApplyOwnerDrawTheme(control, colors);
        }

        // Each of the following Apply*Theme methods should call OwnerDrawThemeHelper where needed.

        public static void ApplyButtonTheme(Control control, Model_UserUiColors colors)
        {
            if (control is Button btn)
            {
                btn.Margin = new Padding(1);
                var backColor = colors.ButtonBackColor ?? SystemColors.Control;
                var foreColor = colors.ButtonForeColor ?? SystemColors.ControlText;
                btn.BackColor = backColor;
                btn.ForeColor = foreColor;
                btn.FlatStyle = FlatStyle.System;
                btn.Paint -= Button_AutoShrinkText_Paint;
                btn.Paint += Button_AutoShrinkText_Paint;

                // Button border, hover, pressed states
                ApplyOwnerDrawThemes(btn, colors);
            }
        }

        public static void ApplyTabControlTheme(Control control, Model_UserUiColors colors)
        {
            if (control is TabControl tab)
            {
                var backColor = colors.TabControlBackColor ?? colors.FormBackColor ?? Color.White;
                var foreColor = colors.TabControlForeColor ?? colors.FormForeColor ?? Color.Black;
                tab.BackColor = backColor;
                tab.ForeColor = foreColor;

                // OwnerDraw for selected/unselected/border states
                ApplyOwnerDrawThemes(tab, colors);
            }
        }

        public static void ApplyTabPageTheme(Control control, Model_UserUiColors colors)
        {
            if (control is TabPage tabPage)
            {
                if (colors.TabPageBackColor.HasValue) tabPage.BackColor = colors.TabPageBackColor.Value;
                if (colors.TabPageForeColor.HasValue) tabPage.ForeColor = colors.TabPageForeColor.Value;

                // OwnerDraw for border, selected/unselected states
                ApplyOwnerDrawThemes(tabPage, colors);
            }
        }

        public static void ApplyTextBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is TextBox txt)
            {
                if (colors.TextBoxBackColor.HasValue) txt.BackColor = colors.TextBoxBackColor.Value;
                if (colors.TextBoxForeColor.HasValue) txt.ForeColor = colors.TextBoxForeColor.Value;

                // Border color
                ApplyOwnerDrawThemes(txt, colors);
            }
        }

        public static void ApplyMaskedTextBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is MaskedTextBox mtxt)
            {
                if (colors.MaskedTextBoxBackColor.HasValue) mtxt.BackColor = colors.MaskedTextBoxBackColor.Value;
                if (colors.MaskedTextBoxForeColor.HasValue) mtxt.ForeColor = colors.MaskedTextBoxForeColor.Value;

                ApplyOwnerDrawThemes(mtxt, colors);
            }
        }

        public static void ApplyRichTextBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is RichTextBox rtxt)
            {
                if (colors.RichTextBoxBackColor.HasValue) rtxt.BackColor = colors.RichTextBoxBackColor.Value;
                if (colors.RichTextBoxForeColor.HasValue) rtxt.ForeColor = colors.RichTextBoxForeColor.Value;

                ApplyOwnerDrawThemes(rtxt, colors);
            }
        }

        public static void ApplyComboBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is ComboBox cb)
            {
                if (colors.ComboBoxBackColor.HasValue) cb.BackColor = colors.ComboBoxBackColor.Value;
                if (colors.ComboBoxForeColor.HasValue) cb.ForeColor = colors.ComboBoxForeColor.Value;

                ApplyOwnerDrawThemes(cb, colors);
            }
        }

        public static void ApplyListBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is ListBox lb)
            {
                if (colors.ListBoxBackColor.HasValue) lb.BackColor = colors.ListBoxBackColor.Value;
                if (colors.ListBoxForeColor.HasValue) lb.ForeColor = colors.ListBoxForeColor.Value;

                ApplyOwnerDrawThemes(lb, colors);
            }
        }

        public static void ApplyCheckedListBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is CheckedListBox clb)
            {
                if (colors.CheckedListBoxBackColor.HasValue) clb.BackColor = colors.CheckedListBoxBackColor.Value;
                if (colors.CheckedListBoxForeColor.HasValue) clb.ForeColor = colors.CheckedListBoxForeColor.Value;

                ApplyOwnerDrawThemes(clb, colors);
            }
        }

        public static void ApplyLabelTheme(Control control, Model_UserUiColors colors)
        {
            if (control is Label lbl)
            {
                if (colors.LabelBackColor.HasValue) lbl.BackColor = colors.LabelBackColor.Value;
                if (colors.LabelForeColor.HasValue) lbl.ForeColor = colors.LabelForeColor.Value;
            }
        }

        public static void ApplyRadioButtonTheme(Control control, Model_UserUiColors colors)
        {
            if (control is RadioButton rb)
            {
                if (colors.RadioButtonBackColor.HasValue) rb.BackColor = colors.RadioButtonBackColor.Value;
                if (colors.RadioButtonForeColor.HasValue) rb.ForeColor = colors.RadioButtonForeColor.Value;

                ApplyOwnerDrawThemes(rb, colors);
            }
        }

        public static void ApplyCheckBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is CheckBox cbx)
            {
                if (colors.CheckBoxBackColor.HasValue) cbx.BackColor = colors.CheckBoxBackColor.Value;
                if (colors.CheckBoxForeColor.HasValue) cbx.ForeColor = colors.CheckBoxForeColor.Value;

                ApplyOwnerDrawThemes(cbx, colors);
            }
        }

        public static void ApplyTreeViewTheme(Control control, Model_UserUiColors colors)
        {
            if (control is TreeView tv)
            {
                if (colors.TreeViewBackColor.HasValue) tv.BackColor = colors.TreeViewBackColor.Value;
                if (colors.TreeViewForeColor.HasValue) tv.ForeColor = colors.TreeViewForeColor.Value;
                if (colors.TreeViewLineColor.HasValue) tv.LineColor = colors.TreeViewLineColor.Value;

                ApplyOwnerDrawThemes(tv, colors);
            }
        }

        public static void ApplyListViewTheme(Control control, Model_UserUiColors colors)
        {
            if (control is ListView lv)
            {
                if (colors.ListViewBackColor.HasValue) lv.BackColor = colors.ListViewBackColor.Value;
                if (colors.ListViewForeColor.HasValue) lv.ForeColor = colors.ListViewForeColor.Value;

                ApplyOwnerDrawThemes(lv, colors);
            }
        }

        public static void ApplyMenuStripTheme(Control control, Model_UserUiColors colors)
        {
            if (control is MenuStrip ms)
            {
                if (colors.MenuStripBackColor.HasValue) ms.BackColor = colors.MenuStripBackColor.Value;
                if (colors.MenuStripForeColor.HasValue) ms.ForeColor = colors.MenuStripForeColor.Value;

                ApplyOwnerDrawThemes(ms, colors);
            }
        }

        public static void ApplyStatusStripTheme(Control control, Model_UserUiColors colors)
        {
            if (control is StatusStrip ss)
            {
                if (colors.StatusStripBackColor.HasValue) ss.BackColor = colors.StatusStripBackColor.Value;
                if (colors.StatusStripForeColor.HasValue) ss.ForeColor = colors.StatusStripForeColor.Value;

                ApplyOwnerDrawThemes(ss, colors);
            }
        }

        public static void ApplyToolStripTheme(Control control, Model_UserUiColors colors)
        {
            if (control is ToolStrip ts)
            {
                if (colors.ToolStripBackColor.HasValue) ts.BackColor = colors.ToolStripBackColor.Value;
                if (colors.ToolStripForeColor.HasValue) ts.ForeColor = colors.ToolStripForeColor.Value;

                ApplyOwnerDrawThemes(ts, colors);
            }
        }

        public static void ApplyGroupBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is GroupBox gb)
            {
                if (colors.GroupBoxBackColor.HasValue) gb.BackColor = colors.GroupBoxBackColor.Value;
                if (colors.GroupBoxForeColor.HasValue) gb.ForeColor = colors.GroupBoxForeColor.Value;

                ApplyOwnerDrawThemes(gb, colors);
            }
        }

        public static void ApplyPanelTheme(Control control, Model_UserUiColors colors)
        {
            if (control is Panel pnl)
            {
                if (colors.PanelForeColor.HasValue)
                    pnl.ForeColor = colors.PanelForeColor.Value;

                ApplyOwnerDrawThemes(pnl, colors);
            }
        }

        public static void ApplySplitContainerTheme(Control control, Model_UserUiColors colors)
        {
            if (control is SplitContainer sc)
            {
                if (colors.SplitContainerBackColor.HasValue) sc.BackColor = colors.SplitContainerBackColor.Value;
                if (colors.SplitContainerForeColor.HasValue) sc.ForeColor = colors.SplitContainerForeColor.Value;

                ApplyOwnerDrawThemes(sc, colors);
            }
        }

        public static void ApplyFlowLayoutPanelTheme(Control control, Model_UserUiColors colors)
        {
            if (control is FlowLayoutPanel flp)
            {
                if (colors.FlowLayoutPanelForeColor.HasValue)
                    flp.ForeColor = colors.FlowLayoutPanelForeColor.Value;

                ApplyOwnerDrawThemes(flp, colors);
            }
        }

        public static void ApplyTableLayoutPanelTheme(Control control, Model_UserUiColors colors)
        {
            if (control is TableLayoutPanel tlp)
            {
                if (colors.TableLayoutPanelForeColor.HasValue)
                    tlp.ForeColor = colors.TableLayoutPanelForeColor.Value;

                ApplyOwnerDrawThemes(tlp, colors);
            }
        }

        public static void ApplyDateTimePickerTheme(Control control, Model_UserUiColors colors)
        {
            if (control is DateTimePicker dtp)
            {
                if (colors.DateTimePickerBackColor.HasValue) dtp.BackColor = colors.DateTimePickerBackColor.Value;
                if (colors.DateTimePickerForeColor.HasValue) dtp.ForeColor = colors.DateTimePickerForeColor.Value;

                ApplyOwnerDrawThemes(dtp, colors);
            }
        }

        public static void ApplyMonthCalendarTheme(Control control, Model_UserUiColors colors)
        {
            if (control is MonthCalendar mc)
            {
                if (colors.MonthCalendarBackColor.HasValue) mc.BackColor = colors.MonthCalendarBackColor.Value;
                if (colors.MonthCalendarForeColor.HasValue) mc.ForeColor = colors.MonthCalendarForeColor.Value;
                if (colors.MonthCalendarTitleBackColor.HasValue)
                    mc.TitleBackColor = colors.MonthCalendarTitleBackColor.Value;
                if (colors.MonthCalendarTitleForeColor.HasValue)
                    mc.TitleForeColor = colors.MonthCalendarTitleForeColor.Value;
                if (colors.MonthCalendarTrailingForeColor.HasValue)
                    mc.TrailingForeColor = colors.MonthCalendarTrailingForeColor.Value;

                ApplyOwnerDrawThemes(mc, colors);
            }
        }

        public static void ApplyNumericUpDownTheme(Control control, Model_UserUiColors colors)
        {
            if (control is NumericUpDown nud)
            {
                if (colors.NumericUpDownBackColor.HasValue) nud.BackColor = colors.NumericUpDownBackColor.Value;
                if (colors.NumericUpDownForeColor.HasValue) nud.ForeColor = colors.NumericUpDownForeColor.Value;

                ApplyOwnerDrawThemes(nud, colors);
            }
        }

        public static void ApplyTrackBarTheme(Control control, Model_UserUiColors colors)
        {
            if (control is TrackBar tb)
            {
                if (colors.TrackBarBackColor.HasValue) tb.BackColor = colors.TrackBarBackColor.Value;
                if (colors.TrackBarForeColor.HasValue) tb.ForeColor = colors.TrackBarForeColor.Value;

                ApplyOwnerDrawThemes(tb, colors);
            }
        }

        public static void ApplyProgressBarTheme(Control control, Model_UserUiColors colors)
        {
            if (control is ProgressBar pb)
            {
                if (colors.ProgressBarBackColor.HasValue) pb.BackColor = colors.ProgressBarBackColor.Value;
                if (colors.ProgressBarForeColor.HasValue) pb.ForeColor = colors.ProgressBarForeColor.Value;

                ApplyOwnerDrawThemes(pb, colors);
            }
        }

        public static void ApplyProgressBarUserControlTheme(Control control, Model_UserUiColors colors)
        {
            if (control is Controls.Shared.ProgressBarUserControl pbuc)
            {
                if (colors.UserControlBackColor.HasValue) pbuc.BackColor = colors.UserControlBackColor.Value;
                if (colors.UserControlForeColor.HasValue) pbuc.ForeColor = colors.UserControlForeColor.Value;

                ApplyOwnerDrawThemes(pbuc, colors);
            }
        }

        public static void ApplyHScrollBarTheme(Control control, Model_UserUiColors colors)
        {
            if (control is HScrollBar hsb)
            {
                if (colors.HScrollBarBackColor.HasValue) hsb.BackColor = colors.HScrollBarBackColor.Value;
                if (colors.HScrollBarForeColor.HasValue) hsb.ForeColor = colors.HScrollBarForeColor.Value;

                ApplyOwnerDrawThemes(hsb, colors);
            }
        }

        public static void ApplyVScrollBarTheme(Control control, Model_UserUiColors colors)
        {
            if (control is VScrollBar vsb)
            {
                if (colors.VScrollBarBackColor.HasValue) vsb.BackColor = colors.VScrollBarBackColor.Value;
                if (colors.VScrollBarForeColor.HasValue) vsb.ForeColor = colors.VScrollBarForeColor.Value;

                ApplyOwnerDrawThemes(vsb, colors);
            }
        }

        public static void ApplyPictureBoxTheme(Control control, Model_UserUiColors colors)
        {
            if (control is PictureBox pic)
            {
                if (colors.PictureBoxBackColor.HasValue)
                    pic.BackColor = colors.PictureBoxBackColor.Value;

                ApplyOwnerDrawThemes(pic, colors);
            }
        }

        public static void ApplyPropertyGridTheme(Control control, Model_UserUiColors colors)
        {
            if (control is PropertyGrid pg)
            {
                if (colors.PropertyGridBackColor.HasValue) pg.BackColor = colors.PropertyGridBackColor.Value;
                if (colors.PropertyGridForeColor.HasValue) pg.ForeColor = colors.PropertyGridForeColor.Value;

                ApplyOwnerDrawThemes(pg, colors);
            }
        }

        public static void ApplyDomainUpDownTheme(Control control, Model_UserUiColors colors)
        {
            if (control is DomainUpDown dud)
            {
                if (colors.DomainUpDownBackColor.HasValue) dud.BackColor = colors.DomainUpDownBackColor.Value;
                if (colors.DomainUpDownForeColor.HasValue) dud.ForeColor = colors.DomainUpDownForeColor.Value;

                ApplyOwnerDrawThemes(dud, colors);
            }
        }

        public static void ApplyWebBrowserTheme(Control control, Model_UserUiColors colors)
        {
            if (control is WebBrowser wb)
            {
                if (colors.WebBrowserBackColor.HasValue)
                    wb.BackColor = colors.WebBrowserBackColor.Value;

                ApplyOwnerDrawThemes(wb, colors);
            }
        }

        public static void ApplyUserControlTheme(Control control, Model_UserUiColors colors)
        {
            if (control is UserControl uc)
            {
                if (colors.UserControlBackColor.HasValue) uc.BackColor = colors.UserControlBackColor.Value;
                if (colors.UserControlForeColor.HasValue) uc.ForeColor = colors.UserControlForeColor.Value;

                ApplyOwnerDrawThemes(uc, colors);
            }
        }

        public static void ApplyLinkLabelTheme(Control control, Model_UserUiColors colors)
        {
            if (control is LinkLabel ll)
            {
                if (colors.LinkLabelLinkColor.HasValue) ll.LinkColor = colors.LinkLabelLinkColor.Value;
                if (colors.LinkLabelActiveLinkColor.HasValue)
                    ll.ActiveLinkColor = colors.LinkLabelActiveLinkColor.Value;
                if (colors.LinkLabelVisitedLinkColor.HasValue)
                    ll.VisitedLinkColor = colors.LinkLabelVisitedLinkColor.Value;
                if (colors.LinkLabelBackColor.HasValue) ll.BackColor = colors.LinkLabelBackColor.Value;
                if (colors.LinkLabelForeColor.HasValue) ll.ForeColor = colors.LinkLabelForeColor.Value;
                if (colors.LinkLabelHoverColor.HasValue)
                    OwnerDrawThemeHelper.AttachLinkLabelHoverColor(ll, colors.LinkLabelHoverColor.Value);
            }
        }

        public static void ApplyContextMenuTheme(Control control, Model_UserUiColors colors)
        {
            if (control is ContextMenuStrip cms)
            {
                if (colors.ContextMenuBackColor.HasValue) cms.BackColor = colors.ContextMenuBackColor.Value;
                if (colors.ContextMenuForeColor.HasValue) cms.ForeColor = colors.ContextMenuForeColor.Value;

                ApplyOwnerDrawThemes(cms, colors);
            }
        }

        public static void ApplyThemeToDataGridView(DataGridView dataGridView)
        {
            if (dataGridView == null) return;

            var colors = Core_AppThemes.GetCurrentTheme().Colors;

            if (colors.DataGridBackColor.HasValue)
                dataGridView.BackgroundColor = colors.DataGridBackColor.Value;
            if (colors.DataGridForeColor.HasValue) dataGridView.ForeColor = colors.DataGridForeColor.Value;

            if (dataGridView.ColumnHeadersDefaultCellStyle != null)
            {
                if (colors.DataGridHeaderBackColor.HasValue)
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = colors.DataGridHeaderBackColor.Value;
                if (colors.DataGridHeaderForeColor.HasValue)
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = colors.DataGridHeaderForeColor.Value;
            }

            if (dataGridView.RowsDefaultCellStyle != null)
            {
                if (colors.DataGridRowBackColor.HasValue)
                    dataGridView.RowsDefaultCellStyle.BackColor = colors.DataGridRowBackColor.Value;
                if (colors.DataGridForeColor.HasValue)
                    dataGridView.RowsDefaultCellStyle.ForeColor = colors.DataGridForeColor.Value;
            }

            if (dataGridView.AlternatingRowsDefaultCellStyle != null)
            {
                if (colors.DataGridAltRowBackColor.HasValue)
                    dataGridView.AlternatingRowsDefaultCellStyle.BackColor = colors.DataGridAltRowBackColor.Value;
                if (colors.DataGridForeColor.HasValue)
                    dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = colors.DataGridForeColor.Value;
            }

            if (colors.DataGridGridColor.HasValue) dataGridView.GridColor = colors.DataGridGridColor.Value;

            if (colors.DataGridSelectionBackColor.HasValue)
                dataGridView.DefaultCellStyle.SelectionBackColor = colors.DataGridSelectionBackColor.Value;
            if (colors.DataGridSelectionForeColor.HasValue)
                dataGridView.DefaultCellStyle.SelectionForeColor = colors.DataGridSelectionForeColor.Value;
            if (colors.DataGridBorderColor.HasValue)
                OwnerDrawThemeHelper.ApplyDataGridViewBorderColor(dataGridView, colors.DataGridBorderColor.Value);
        }

        public static void SizeDataGrid(DataGridView dataGridView)
        {
            if (dataGridView == null) throw new ArgumentNullException(nameof(dataGridView));

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            var preferredWidths = new int[dataGridView.Columns.Count];
            var totalPreferredWidth = 0;
            for (var i = 0; i < dataGridView.Columns.Count; i++)
            {
                preferredWidths[i] = dataGridView.Columns[i].Width;
                totalPreferredWidth += preferredWidths[i];
            }

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            for (var i = 0; i < dataGridView.Columns.Count; i++)
                if (totalPreferredWidth > 0)
                    dataGridView.Columns[i].FillWeight = (float)preferredWidths[i] / totalPreferredWidth * 100f;
                else
                    dataGridView.Columns[i].FillWeight = 100f / dataGridView.Columns.Count;
        }

        private static void Button_AutoShrinkText_Paint(object? sender, PaintEventArgs e)
        {
        }
    }

    // Owner draw theme helper for borders, hover, pressed, selected, etc.
    private static class OwnerDrawThemeHelper
    {
        public static void ApplyOwnerDrawTheme(Control control, Model_UserUiColors colors)
        {
            // This is a stub for all owner-draw logic.
            // For brevity, only examples for Button and TabControl are shown.
            // Extend for other controls and states as needed.

            // Button example
            if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                if (colors.ButtonBorderColor.HasValue)
                    btn.FlatAppearance.BorderColor = colors.ButtonBorderColor.Value;
                if (colors.ButtonHoverBackColor.HasValue || colors.ButtonPressedBackColor.HasValue)
                {
                    btn.MouseEnter -= (s, e) => { };
                    btn.MouseLeave -= (s, e) => { };
                    btn.MouseDown -= (s, e) => { };
                    btn.MouseUp -= (s, e) => { };
                    btn.MouseEnter += (s, e) =>
                    {
                        if (colors.ButtonHoverBackColor.HasValue)
                            btn.BackColor = colors.ButtonHoverBackColor.Value;
                        if (colors.ButtonHoverForeColor.HasValue)
                            btn.ForeColor = colors.ButtonHoverForeColor.Value;
                    };
                    btn.MouseLeave += (s, e) =>
                    {
                        btn.BackColor = colors.ButtonBackColor ?? SystemColors.Control;
                        btn.ForeColor = colors.ButtonForeColor ?? SystemColors.ControlText;
                    };
                    btn.MouseDown += (s, e) =>
                    {
                        if (colors.ButtonPressedBackColor.HasValue)
                            btn.BackColor = colors.ButtonPressedBackColor.Value;
                        if (colors.ButtonPressedForeColor.HasValue)
                            btn.ForeColor = colors.ButtonPressedForeColor.Value;
                    };
                    btn.MouseUp += (s, e) =>
                    {
                        btn.BackColor = colors.ButtonBackColor ?? SystemColors.Control;
                        btn.ForeColor = colors.ButtonForeColor ?? SystemColors.ControlText;
                    };
                }
            }

            // TabControl example (OwnerDraw)
            if (control is TabControl tab)
            {
                if (tab.DrawMode != TabDrawMode.OwnerDrawFixed)
                    tab.DrawMode = TabDrawMode.OwnerDrawFixed;

                tab.DrawItem -= TabControl_DrawItem;
                tab.DrawItem += TabControl_DrawItem;

                void TabControl_DrawItem(object? sender, DrawItemEventArgs e)
                {
                    var tabPage = tab.TabPages[e.Index];
                    var rect = e.Bounds;
                    var selected = tab.SelectedIndex == e.Index;

                    var backColor = selected
                        ? colors.TabSelectedBackColor ??
                          colors.TabPageBackColor ?? colors.TabControlBackColor ?? Color.White
                        : colors.TabUnselectedBackColor ??
                          colors.TabPageBackColor ?? colors.TabControlBackColor ?? Color.White;
                    var foreColor = selected
                        ? colors.TabSelectedForeColor ??
                          colors.TabPageForeColor ?? colors.TabControlForeColor ?? Color.Black
                        : colors.TabUnselectedForeColor ??
                          colors.TabPageForeColor ?? colors.TabControlForeColor ?? Color.Black;

                    using (var b = new SolidBrush(backColor))
                    {
                        e.Graphics.FillRectangle(b, rect);
                    }


                    TextRenderer.DrawText(e.Graphics, tabPage.Text, e.Font, rect, foreColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }

            // TODO: Add owner draw for ListBox, ComboBox, other controls as needed
        }

        // Attach link label hover color
        public static void AttachLinkLabelHoverColor(LinkLabel ll, Color hoverColor)
        {
            ll.MouseEnter -= LinkLabel_MouseEnter;
            ll.MouseLeave -= LinkLabel_MouseLeave;
            ll.MouseEnter += LinkLabel_MouseEnter;
            ll.MouseLeave += LinkLabel_MouseLeave;

            void LinkLabel_MouseEnter(object? sender, EventArgs e)
            {
                ll.LinkColor = hoverColor;
            }

            void LinkLabel_MouseLeave(object? sender, EventArgs e)
            {
                // revert to normal color
                ll.LinkColor = ll.VisitedLinkColor;
            }
        }

        // DataGridView border color
        public static void ApplyDataGridViewBorderColor(DataGridView dgv, Color borderColor)
        {
            dgv.GridColor = borderColor;
        }
    }

    private static class FocusUtils
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

        private static void Control_Enter_Handler(object? sender, EventArgs e)
        {
            if (sender is Control ctrl && ctrl.Focused)
            {
                var colors = Core_AppThemes.GetCurrentTheme().Colors;
                var focusBackColor = colors.ControlFocusedBackColor ?? Color.LightBlue;
                var normalForeColor = GetControlThemeForeColor(ctrl, colors);
                ctrl.BackColor = focusBackColor;
                ctrl.ForeColor = normalForeColor;
                switch (ctrl)
                {
                    case TextBox tb: tb.SelectAll(); break;
                    case MaskedTextBox mtb: mtb.SelectAll(); break;
                    case RichTextBox rtb: rtb.SelectAll(); break;
                    case ComboBox cb when cb.DropDownStyle != ComboBoxStyle.DropDownList: cb.SelectAll(); break;
                }
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

    #region Core_AppThemes

    public static class Core_AppThemes
    {
        #region Theme Definition

        public class AppTheme
        {
            public Model_UserUiColors Colors { get; set; } = new();
            public Font? FormFont { get; set; }
        }

        #endregion

        #region Theme Registry

        private static Dictionary<string, AppTheme> Themes = new();

        private static async Task<string?> LoadAndSetUserThemeNameAsync(string userId)
        {
            try
            {
                LoggingUtility.Log($"Loaded user theme name for user: {userId} = {Model_AppVariables.ThemeName}");
                return await Dao_User.GetSettingsJsonAsync("Theme_Name", userId, true);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        public static async Task LoadThemesFromDatabaseAsync()
        {
            try
            {
                var themes = new Dictionary<string, AppTheme>();
                var helper = new Helper_Database_Core(Model_AppVariables.ConnectionString);
                var dt = await helper.ExecuteDataTable("SELECT ThemeName, SettingsJson FROM app_themes", null, true,
                    CommandType.Text);
                foreach (DataRow row in dt.Rows)
                {
                    var themeName = row["ThemeName"]?.ToString();
                    var SettingsJson = row["SettingsJson"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(themeName) && !string.IsNullOrWhiteSpace(SettingsJson))
                        try
                        {
                            var options = new System.Text.Json.JsonSerializerOptions();
                            options.Converters.Add(new JsonColorConverter());
                            var colors =
                                System.Text.Json.JsonSerializer.Deserialize<Model_UserUiColors>(SettingsJson, options);
                            if (colors != null) themes[themeName] = new AppTheme { Colors = colors, FormFont = null };
                        }
                        catch (System.Text.Json.JsonException jsonEx)
                        {
                            LoggingUtility.LogApplicationError(jsonEx);
                        }
                }

                Themes = themes;
                LoggingUtility.Log("Loaded themes from database.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        #endregion

        #region Theme Accessors

        public static IEnumerable<string> GetThemeNames()
        {
            try
            {
                Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
                return Themes.Keys;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        public static bool ThemeExists(string themeName)
        {
            try
            {
                Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
                return Themes.ContainsKey(themeName);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        public static AppTheme GetCurrentTheme()
        {
            try
            {
                var themeName = Model_AppVariables.ThemeName ?? "Default";
                if (Themes.TryGetValue(themeName, out var theme))
                    return theme;
                Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
                return Themes.ContainsKey("Default") ? Themes["Default"] : new AppTheme();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        public static AppTheme GetTheme(string themeName)
        {
            try
            {
                if (Themes.TryGetValue(themeName, out var theme))
                    return theme;
                Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
                return Themes.ContainsKey("Default") ? Themes["Default"] : new AppTheme();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        public static string GetEffectiveThemeName()
        {
            try
            {
                var themeName = Model_AppVariables.ThemeName ?? "Default";
                if (!Themes.ContainsKey(themeName)) themeName = "Default";
                Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
                return themeName;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        public static Color GetThemeColor(string propertyName)
        {
            try
            {
                var theme = GetCurrentTheme();
                var colors = theme.Colors;
                var property = typeof(Model_UserUiColors).GetProperty(propertyName)
                               ?? throw new InvalidOperationException(
                                   $"Property '{propertyName}' not found on Model_UserUiColors.");
                var value = property.GetValue(colors);
                if (value is Color color)
                    return color;
                if (value is Color nullableColor)
                    return nullableColor;
                throw new InvalidOperationException($"Property '{propertyName}' is not a Color or is null.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        #endregion

        #region Theme Startup Sequence

        public static async Task InitializeThemeSystemAsync(string userId)
        {
            try
            {
                await LoadAndSetUserThemeNameAsync(userId);
                await LoadThemesFromDatabaseAsync();

                foreach (var theme in Themes.Values)
                    if (theme.FormFont == null)
                        theme.FormFont = new Font("Segoe UI", Model_AppVariables.ThemeFontSize);
                    else if (Math.Abs(theme.FormFont.Size - Model_AppVariables.ThemeFontSize) > 0.01f)
                        theme.FormFont = new Font(theme.FormFont.FontFamily, Model_AppVariables.ThemeFontSize,
                            theme.FormFont.Style);

                Debug.WriteLine($"Themes count: {Themes.Count}, using font size: {Model_AppVariables.ThemeFontSize}");
                LoggingUtility.Log($"Theme system initialized with font size: {Model_AppVariables.ThemeFontSize}");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        #endregion
    }

    #endregion
}