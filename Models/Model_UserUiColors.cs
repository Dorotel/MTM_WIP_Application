using System.Drawing;

namespace MTM_Inventory_Application.Models;

/// <summary>
/// Represents all user-customizable color colors for the application.
/// </summary>
public class Model_UserUiColors
{
    // Form
    public Color? FormBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? FormForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    // Control (base)
    public Color? ControlBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? ControlForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    public Color? ControlFocusedBackColor { get; set; } =
        Color.FromArgb(0, 122, 204); // #007ACC - Replaces Color.LightBlue

    // Label
    public Color? LabelBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? LabelForeColor { get; set; } = Color.FromArgb(204, 204, 204); // #CCCCCC

    // TextBox
    public Color? TextBoxBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? TextBoxForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? TextBoxSelectionBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? TextBoxSelectionForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? TextBoxErrorForeColor { get; set; } = Color.FromArgb(229, 115, 115); // #E57373
    public Color? TextBoxBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // MaskedTextBox
    public Color? MaskedTextBoxBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? MaskedTextBoxForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? MaskedTextBoxErrorForeColor { get; set; } = Color.FromArgb(229, 115, 115); // #E57373
    public Color? MaskedTextBoxBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // RichTextBox
    public Color? RichTextBoxBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? RichTextBoxForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? RichTextBoxSelectionBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? RichTextBoxSelectionForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? RichTextBoxErrorForeColor { get; set; } = Color.FromArgb(229, 115, 115); // #E57373
    public Color? RichTextBoxBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // ComboBox
    public Color? ComboBoxBackColor { get; set; } = Color.FromArgb(45, 45, 48); // #2D2D30
    public Color? ComboBoxForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ComboBoxSelectionBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? ComboBoxSelectionForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ComboBoxErrorForeColor { get; set; } = Color.FromArgb(229, 115, 115); // #E57373
    public Color? ComboBoxBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? ComboBoxDropDownBackColor { get; set; } = Color.FromArgb(45, 45, 48); // #2D2D30
    public Color? ComboBoxDropDownForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    // ListBox
    public Color? ListBoxBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? ListBoxForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ListBoxSelectionBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? ListBoxSelectionForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ListBoxBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // CheckedListBox
    public Color? CheckedListBoxBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? CheckedListBoxForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? CheckedListBoxBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? CheckedListBoxCheckBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? CheckedListBoxCheckForeColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC

    // Button
    public Color? ButtonBackColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? ButtonForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ButtonBorderColor { get; set; } = Color.FromArgb(68, 68, 68); // #444444
    public Color? ButtonHoverBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? ButtonHoverForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ButtonPressedBackColor { get; set; } = Color.FromArgb(0, 90, 158); // #005A9E
    public Color? ButtonPressedForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    // RadioButton
    public Color? RadioButtonBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? RadioButtonForeColor { get; set; } = Color.FromArgb(204, 204, 204); // #CCCCCC
    public Color? RadioButtonCheckColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC

    // CheckBox
    public Color? CheckBoxBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? CheckBoxForeColor { get; set; } = Color.FromArgb(204, 204, 204); // #CCCCCC
    public Color? CheckBoxCheckColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? CheckBoxCheckBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E

    // DataGridView
    public Color? DataGridBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? DataGridForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? DataGridSelectionBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? DataGridSelectionForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? DataGridRowBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? DataGridAltRowBackColor { get; set; } = Color.FromArgb(42, 45, 46); // #2A2D2E
    public Color? DataGridHeaderBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? DataGridHeaderForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? DataGridGridColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? DataGridBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // TreeView
    public Color? TreeViewBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? TreeViewForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? TreeViewLineColor { get; set; } = Color.FromArgb(153, 153, 153); // #999999
    public Color? TreeViewSelectedNodeBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? TreeViewSelectedNodeForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? TreeViewBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // ListView
    public Color? ListViewBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? ListViewForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ListViewSelectionBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? ListViewSelectionForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ListViewBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? ListViewHeaderBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? ListViewHeaderForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    // MenuStrip/StatusStrip/ToolStrip
    public Color? MenuStripBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? MenuStripForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? MenuStripBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? MenuStripItemHoverBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? MenuStripItemHoverForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? MenuStripItemSelectedBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? MenuStripItemSelectedForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    public Color? StatusStripBackColor { get; set; } = Color.FromArgb(45, 45, 48); // #2D2D30
    public Color? StatusStripForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? StatusStripBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    public Color? ToolStripBackColor { get; set; } = Color.FromArgb(45, 45, 48); // #2D2D30
    public Color? ToolStripForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ToolStripBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? ToolStripItemHoverBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? ToolStripItemHoverForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    // TabControl/TabPage
    public Color? TabControlBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? TabControlForeColor { get; set; } = Color.FromArgb(204, 204, 204); // #CCCCCC
    public Color? TabControlBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? TabPageBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? TabPageForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? TabPageBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? TabSelectedBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? TabSelectedForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? TabUnselectedBackColor { get; set; } = Color.FromArgb(26, 26, 26); // #1A1A1A
    public Color? TabUnselectedForeColor { get; set; } = Color.FromArgb(136, 136, 136); // #888888

    // GroupBox/Panel/SplitContainer
    public Color? GroupBoxBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? GroupBoxForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? GroupBoxBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    public Color? PanelBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? PanelForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? PanelBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    public Color? SplitContainerBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? SplitContainerForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? SplitContainerSplitterColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // FlowLayoutPanel
    public Color? FlowLayoutPanelBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? FlowLayoutPanelForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? FlowLayoutPanelBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // TableLayoutPanel
    public Color? TableLayoutPanelBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? TableLayoutPanelForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? TableLayoutPanelBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? TableLayoutPanelCellBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // LinkLabel
    public Color? LinkLabelLinkColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? LinkLabelActiveLinkColor { get; set; } = Color.FromArgb(102, 204, 255); // #66CCFF
    public Color? LinkLabelVisitedLinkColor { get; set; } = Color.FromArgb(0, 90, 158); // #005A9E
    public Color? LinkLabelHoverColor { get; set; } = Color.FromArgb(102, 204, 255); // #66CCFF
    public Color? LinkLabelBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? LinkLabelForeColor { get; set; } = Color.FromArgb(51, 153, 255); // #3399FF

    // ProgressBar/TrackBar
    public Color? ProgressBarBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? ProgressBarForeColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? ProgressBarBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    public Color? TrackBarBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? TrackBarForeColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? TrackBarThumbColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? TrackBarTickColor { get; set; } = Color.FromArgb(153, 153, 153); // #999999

    // DateTimePicker
    public Color? DateTimePickerBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? DateTimePickerForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? DateTimePickerBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? DateTimePickerDropDownBackColor { get; set; } = Color.FromArgb(45, 45, 48); // #2D2D30
    public Color? DateTimePickerDropDownForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    // MonthCalendar
    public Color? MonthCalendarBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? MonthCalendarForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? MonthCalendarTitleBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? MonthCalendarTitleForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? MonthCalendarTrailingForeColor { get; set; } = Color.FromArgb(153, 153, 153); // #999999
    public Color? MonthCalendarTodayBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? MonthCalendarTodayForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? MonthCalendarBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // NumericUpDown
    public Color? NumericUpDownBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? NumericUpDownForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? NumericUpDownErrorForeColor { get; set; } = Color.FromArgb(229, 115, 115); // #E57373
    public Color? NumericUpDownBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? NumericUpDownButtonBackColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? NumericUpDownButtonForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    // HScrollBar/VScrollBar
    public Color? HScrollBarBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? HScrollBarForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? HScrollBarThumbColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? HScrollBarTrackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E

    public Color? VScrollBarBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? VScrollBarForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? VScrollBarThumbColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? VScrollBarTrackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E

    // PictureBox
    public Color? PictureBoxBackColor { get; set; } = Color.Transparent;
    public Color? PictureBoxBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // PropertyGrid
    public Color? PropertyGridBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? PropertyGridForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? PropertyGridLineColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? PropertyGridCategoryBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? PropertyGridCategoryForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? PropertyGridSelectedBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? PropertyGridSelectedForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    // DomainUpDown
    public Color? DomainUpDownBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? DomainUpDownForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? DomainUpDownErrorForeColor { get; set; } = Color.FromArgb(229, 115, 115); // #E57373
    public Color? DomainUpDownBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? DomainUpDownButtonBackColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? DomainUpDownButtonForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF

    // WebBrowser
    public Color? WebBrowserBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? WebBrowserBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // UserControl/CustomControl
    public Color? UserControlBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? UserControlForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? UserControlBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    public Color? CustomControlBackColor { get; set; } = Color.FromArgb(30, 30, 30); // #1E1E1E
    public Color? CustomControlForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? CustomControlBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // Tooltip
    public Color? ToolTipBackColor { get; set; } = Color.FromArgb(37, 37, 38); // #252526
    public Color? ToolTipForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ToolTipBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // Context Menu
    public Color? ContextMenuBackColor { get; set; } = Color.FromArgb(45, 45, 48); // #2D2D30
    public Color? ContextMenuForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ContextMenuBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? ContextMenuItemHoverBackColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? ContextMenuItemHoverForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? ContextMenuSeparatorColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C

    // Accent Color (for highlights, etc.)
    public Color? AccentColor { get; set; } = Color.FromArgb(0, 122, 204); // #007ACC
    public Color? SecondaryAccentColor { get; set; } = Color.FromArgb(102, 204, 255); // #66CCFF
    public Color? ErrorColor { get; set; } = Color.FromArgb(229, 115, 115); // #E57373
    public Color? WarningColor { get; set; } = Color.FromArgb(255, 167, 38); // #FFA726
    public Color? SuccessColor { get; set; } = Color.FromArgb(102, 187, 106); // #66BB6A
    public Color? InfoColor { get; set; } = Color.FromArgb(51, 153, 255); // #3399FF

    // Window Chrome (for custom window styling)
    public Color? WindowTitleBarBackColor { get; set; } = Color.FromArgb(26, 26, 26); // #1A1A1A
    public Color? WindowTitleBarForeColor { get; set; } = Color.FromArgb(255, 255, 255); // #FFFFFF
    public Color? WindowTitleBarInactiveBackColor { get; set; } = Color.FromArgb(45, 45, 48); // #2D2D30
    public Color? WindowTitleBarInactiveForeColor { get; set; } = Color.FromArgb(136, 136, 136); // #888888
    public Color? WindowBorderColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
    public Color? WindowResizeHandleColor { get; set; } = Color.FromArgb(60, 60, 60); // #3C3C3C
}