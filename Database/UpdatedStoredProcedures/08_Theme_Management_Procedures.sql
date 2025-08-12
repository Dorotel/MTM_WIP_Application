-- ================================================================================
-- MTM INVENTORY APPLICATION - THEME MANAGEMENT STORED PROCEDURES
-- ================================================================================
-- File: 08_Theme_Management_Procedures.sql
-- Purpose: Application theme management and user interface customization procedures
-- Created: August 10, 2025
-- Updated: August 12, 2025 - ALL COLOR SETTINGS INCLUDED
-- Target Database: mtm_wip_application_test
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS app_themes_Get_All;
DROP PROCEDURE IF EXISTS app_themes_Get_ByName;
DROP PROCEDURE IF EXISTS app_themes_Add_Theme;
DROP PROCEDURE IF EXISTS app_themes_Update_Theme;
DROP PROCEDURE IF EXISTS app_themes_Delete_Theme;
DROP PROCEDURE IF EXISTS app_themes_Exists;
DROP PROCEDURE IF EXISTS app_themes_Get_UserTheme;
DROP PROCEDURE IF EXISTS app_themes_Set_UserTheme;

-- Drop tables if they exist (for clean deployment)
DROP TABLE IF EXISTS app_themes;

-- ================================================================================
-- CREATE THEME TABLES
-- ================================================================================

CREATE TABLE IF NOT EXISTS app_themes (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    ThemeName VARCHAR(50) NOT NULL UNIQUE,
    DisplayName VARCHAR(100) NOT NULL,
    SettingsJson TEXT NOT NULL,
    IsDefault TINYINT(1) DEFAULT 0,
    IsActive TINYINT(1) DEFAULT 1,
    CreatedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CreatedBy VARCHAR(100) DEFAULT 'SYSTEM',
    ModifiedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    ModifiedBy VARCHAR(100) DEFAULT 'SYSTEM',
    Description TEXT NULL,
    VERSION INT DEFAULT 1,
    INDEX idx_theme_name (ThemeName),
    INDEX idx_active (IsActive),
    INDEX idx_default (IsDefault)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ================================================================================
-- INSERT DEFAULT THEMES (Using LiveDatabase.sql themes)
-- ================================================================================

-- Remove old themes first
DELETE FROM app_themes;

-- Insert Arctic theme
INSERT INTO app_themes (ThemeName, SettingsJson) VALUES (
    'Arctic', 
    '{"InfoColor": "#17A2B8", "ErrorColor": "#DC3545", "AccentColor": "#007BFF", "SuccessColor": "#28A745", "WarningColor": "#FFC107", "FormBackColor": "#F8F9FA", "FormForeColor": "#1A1A1A", "LabelBackColor": "#F8F9FA", "LabelForeColor": "#1A1A1A", "PanelBackColor": "#F8F9FA", "PanelForeColor": "#1A1A1A", "ButtonBackColor": "#F0F0F0", "ButtonForeColor": "#1A1A1A", "ControlBackColor": "#F8F9FA", "ControlForeColor": "#1A1A1A", "ListBoxBackColor": "#FFFFFF", "ListBoxForeColor": "#1A1A1A", "PanelBorderColor": "#CED4DA", "TabPageBackColor": "#F8F9FA", "TabPageForeColor": "#1A1A1A", "TextBoxBackColor": "#FFFFFF", "TextBoxForeColor": "#1A1A1A", "ToolTipBackColor": "#E9ECEF", "ToolTipForeColor": "#1A1A1A", "ButtonBorderColor": "#CED4DA", "CheckBoxBackColor": "#F8F9FA", "CheckBoxForeColor": "#1A1A1A", "ComboBoxBackColor": "#FFFFFF", "ComboBoxForeColor": "#1A1A1A", "DataGridBackColor": "#FFFFFF", "DataGridForeColor": "#1A1A1A", "DataGridGridColor": "#CED4DA", "GroupBoxBackColor": "#F8F9FA", "GroupBoxForeColor": "#1A1A1A", "ListViewBackColor": "#FFFFFF", "ListViewForeColor": "#1A1A1A", "TrackBarBackColor": "#F8F9FA", "TrackBarForeColor": "#007BFF", "TrackBarTickColor": "#CED4DA", "TreeViewBackColor": "#FFFFFF", "TreeViewForeColor": "#1A1A1A", "TreeViewLineColor": "#CED4DA", "WindowBorderColor": "#CED4DA", "CheckBoxCheckColor": "#007BFF", "LinkLabelBackColor": "#F8F9FA", "LinkLabelForeColor": "#007BFF", "LinkLabelLinkColor": "#007BFF", "ListBoxBorderColor": "#CED4DA", "MenuStripBackColor": "#F8F9FA", "MenuStripForeColor": "#1A1A1A", "TabPageBorderColor": "#CED4DA", "TextBoxBorderColor": "#CED4DA", "ToolStripBackColor": "#E9ECEF", "ToolStripForeColor": "#1A1A1A", "ToolTipBorderColor": "#CED4DA", "TrackBarThumbColor": "#007BFF", "ComboBoxBorderColor": "#CED4DA", "DataGridBorderColor": "#CED4DA", "GroupBoxBorderColor": "#CED4DA", "HScrollBarBackColor": "#F8F9FA", "HScrollBarForeColor": "#1A1A1A", "LinkLabelHoverColor": "#66B5FF", "ListViewBorderColor": "#CED4DA", "PictureBoxBackColor": "#F8F9FA", "TabControlBackColor": "#F8F9FA", "TabControlForeColor": "#1A1A1A", "TreeViewBorderColor": "#CED4DA", "VScrollBarBackColor": "#F8F9FA", "VScrollBarForeColor": "#1A1A1A", "WebBrowserBackColor": "#F8F9FA", "ButtonHoverBackColor": "#007BFF", "ButtonHoverForeColor": "#FFFFFF", "ContextMenuBackColor": "#F8F9FA", "ContextMenuForeColor": "#1A1A1A", "DataGridRowBackColor": "#FFFFFF", "HScrollBarThumbColor": "#F0F0F0", "HScrollBarTrackColor": "#F8F9FA", "MenuStripBorderColor": "#CED4DA", "ProgressBarBackColor": "#F8F9FA", "ProgressBarForeColor": "#007BFF", "RadioButtonBackColor": "#F8F9FA", "RadioButtonForeColor": "#1A1A1A", "RichTextBoxBackColor": "#FFFFFF", "RichTextBoxForeColor": "#1A1A1A", "SecondaryAccentColor": "#66B5FF", "StatusStripBackColor": "#E9ECEF", "StatusStripForeColor": "#1A1A1A", "TabSelectedBackColor": "#007BFF", "TabSelectedForeColor": "#FFFFFF", "ToolStripBorderColor": "#CED4DA", "UserControlBackColor": "#F8F9FA", "UserControlForeColor": "#1A1A1A", "VScrollBarThumbColor": "#F0F0F0", "VScrollBarTrackColor": "#F8F9FA", "DomainUpDownBackColor": "#F8F9FA", "DomainUpDownForeColor": "#1A1A1A", "PictureBoxBorderColor": "#CED4DA", "PropertyGridBackColor": "#F8F9FA", "PropertyGridForeColor": "#1A1A1A", "PropertyGridLineColor": "#CED4DA", "RadioButtonCheckColor": "#007BFF", "TabControlBorderColor": "#CED4DA", "TextBoxErrorForeColor": "#DC3545", "WebBrowserBorderColor": "#CED4DA", "ButtonPressedBackColor": "#0056B3", "ButtonPressedForeColor": "#FFFFFF", "CheckBoxCheckBackColor": "#F8F9FA", "ComboBoxErrorForeColor": "#DC3545", "ContextMenuBorderColor": "#CED4DA", "CustomControlBackColor": "#F8F9FA", "CustomControlForeColor": "#1A1A1A", "MaskedTextBoxBackColor": "#FFFFFF", "MaskedTextBoxForeColor": "#1A1A1A", "MonthCalendarBackColor": "#FFFFFF", "MonthCalendarForeColor": "#1A1A1A", "NumericUpDownBackColor": "#FFFFFF", "NumericUpDownForeColor": "#1A1A1A", "ProgressBarBorderColor": "#CED4DA", "RichTextBoxBorderColor": "#CED4DA", "StatusStripBorderColor": "#CED4DA", "TabUnselectedBackColor": "#F0F0F0", "TabUnselectedForeColor": "#1A1A1A", "UserControlBorderColor": "#CED4DA", "CheckedListBoxBackColor": "#FFFFFF", "CheckedListBoxForeColor": "#1A1A1A", "ControlFocusedBackColor": "#007BFF", "DataGridAltRowBackColor": "#F8F9FA", "DataGridHeaderBackColor": "#E9ECEF", "DataGridHeaderForeColor": "#1A1A1A", "DateTimePickerBackColor": "#FFFFFF", "DateTimePickerForeColor": "#1A1A1A", "DomainUpDownBorderColor": "#CED4DA", "ListViewHeaderBackColor": "#E9ECEF", "ListViewHeaderForeColor": "#1A1A1A", "SplitContainerBackColor": "#F8F9FA", "SplitContainerForeColor": "#1A1A1A", "WindowResizeHandleColor": "#CED4DA", "WindowTitleBarBackColor": "#E9ECEF", "WindowTitleBarForeColor": "#1A1A1A", "CustomControlBorderColor": "#CED4DA", "FlowLayoutPanelBackColor": "#F8F9FA", "FlowLayoutPanelForeColor": "#1A1A1A", "LinkLabelActiveLinkColor": "#66B5FF", "MaskedTextBoxBorderColor": "#CED4DA", "MonthCalendarBorderColor": "#CED4DA", "NumericUpDownBorderColor": "#CED4DA", "CheckedListBoxBorderColor": "#CED4DA", "ComboBoxDropDownBackColor": "#FFFFFF", "ComboBoxDropDownForeColor": "#1A1A1A", "ContextMenuSeparatorColor": "#CED4DA", "DateTimePickerBorderColor": "#CED4DA", "LinkLabelVisitedLinkColor": "#0056B3", "ListBoxSelectionBackColor": "#007BFF", "ListBoxSelectionForeColor": "#FFFFFF", "RichTextBoxErrorForeColor": "#DC3545", "TableLayoutPanelBackColor": "#F8F9FA", "TableLayoutPanelForeColor": "#1A1A1A", "TextBoxSelectionBackColor": "#007BFF", "TextBoxSelectionForeColor": "#FFFFFF", "ComboBoxSelectionBackColor": "#007BFF", "ComboBoxSelectionForeColor": "#FFFFFF", "DataGridSelectionBackColor": "#007BFF", "DataGridSelectionForeColor": "#FFFFFF", "DomainUpDownErrorForeColor": "#DC3545", "FlowLayoutPanelBorderColor": "#CED4DA", "ListViewSelectionBackColor": "#007BFF", "ListViewSelectionForeColor": "#FFFFFF", "DomainUpDownButtonBackColor": "#F0F0F0", "DomainUpDownButtonForeColor": "#1A1A1A", "MaskedTextBoxErrorForeColor": "#DC3545", "MenuStripItemHoverBackColor": "#007BFF", "MenuStripItemHoverForeColor": "#FFFFFF", "MonthCalendarTitleBackColor": "#007BFF", "MonthCalendarTitleForeColor": "#FFFFFF", "MonthCalendarTodayBackColor": "#E9ECEF", "MonthCalendarTodayForeColor": "#1A1A1A", "NumericUpDownErrorForeColor": "#DC3545", "SplitContainerSplitterColor": "#CED4DA", "TableLayoutPanelBorderColor": "#CED4DA", "ToolStripItemHoverBackColor": "#007BFF", "ToolStripItemHoverForeColor": "#FFFFFF", "CheckedListBoxCheckBackColor": "#FFFFFF", "CheckedListBoxCheckForeColor": "#007BFF", "NumericUpDownButtonBackColor": "#F0F0F0", "NumericUpDownButtonForeColor": "#1A1A1A", "ContextMenuItemHoverBackColor": "#007BFF", "ContextMenuItemHoverForeColor": "#FFFFFF", "PropertyGridCategoryBackColor": "#F8F9FA", "PropertyGridCategoryForeColor": "#1A1A1A", "PropertyGridSelectedBackColor": "#007BFF", "PropertyGridSelectedForeColor": "#FFFFFF", "RichTextBoxSelectionBackColor": "#007BFF", "RichTextBoxSelectionForeColor": "#FFFFFF", "TreeViewSelectedNodeBackColor": "#007BFF", "TreeViewSelectedNodeForeColor": "#FFFFFF", "MenuStripItemSelectedBackColor": "#007BFF", "MenuStripItemSelectedForeColor": "#FFFFFF", "MonthCalendarTrailingForeColor": "#CED4DA", "DateTimePickerDropDownBackColor": "#FFFFFF", "DateTimePickerDropDownForeColor": "#1A1A1A", "TableLayoutPanelCellBorderColor": "#CED4DA", "WindowTitleBarInactiveBackColor": "#E9ECEF", "WindowTitleBarInactiveForeColor": "#CED4DA"}'
);
    '{
        "FormBackColor": "#FFFFFF",
        "FormForeColor": "#000000",
        "FormBorderColor": "#CCCCCC",
        "ControlBackColor": "#FFFFFF",
        "ControlForeColor": "#000000",
        "ControlFocusedBackColor": "#E0E0E0",
        "LabelBackColor": "#FFFFFF",
        "LabelForeColor": "#333333",
        "TextBoxBackColor": "#FFFFFF",
        "TextBoxForeColor": "#000000",
        "TextBoxSelectionBackColor": "#316AC5",
        "TextBoxSelectionForeColor": "#FFFFFF",
        "TextBoxErrorForeColor": "#FF0000",
        "TextBoxBorderColor": "#CCCCCC",
        "MaskedTextBoxBackColor": "#FFFFFF",
        "MaskedTextBoxForeColor": "#000000",
        "MaskedTextBoxErrorForeColor": "#FF0000",
        "MaskedTextBoxBorderColor": "#CCCCCC",
        "RichTextBoxBackColor": "#FFFFFF",
        "RichTextBoxForeColor": "#000000",
        "RichTextBoxSelectionBackColor": "#316AC5",
        "RichTextBoxSelectionForeColor": "#FFFFFF",
        "RichTextBoxErrorForeColor": "#FF0000",
        "RichTextBoxBorderColor": "#CCCCCC",
        "ComboBoxBackColor": "#FFFFFF",
        "ComboBoxForeColor": "#000000",
        "ComboBoxSelectionBackColor": "#316AC5",
        "ComboBoxSelectionForeColor": "#FFFFFF",
        "ComboBoxErrorForeColor": "#FF0000",
        "ComboBoxBorderColor": "#CCCCCC",
        "ComboBoxDropDownBackColor": "#FFFFFF",
        "ComboBoxDropDownForeColor": "#000000",
        "ListBoxBackColor": "#FFFFFF",
        "ListBoxForeColor": "#000000",
        "ListBoxSelectionBackColor": "#316AC5",
        "ListBoxSelectionForeColor": "#FFFFFF",
        "ListBoxBorderColor": "#CCCCCC",
        "CheckedListBoxBackColor": "#FFFFFF",
        "CheckedListBoxForeColor": "#000000",
        "CheckedListBoxBorderColor": "#CCCCCC",
        "CheckedListBoxCheckBackColor": "#FFFFFF",
        "CheckedListBoxCheckForeColor": "#316AC5",
        "ButtonBackColor": "#F0F0F0",
        "ButtonForeColor": "#000000",
        "ButtonBorderColor": "#CCCCCC",
        "ButtonHoverBackColor": "#E0E0E0",
        "ButtonHoverForeColor": "#000000",
        "ButtonPressedBackColor": "#D0D0D0",
        "ButtonPressedForeColor": "#000000",
        "RadioButtonBackColor": "#FFFFFF",
        "RadioButtonForeColor": "#333333",
        "RadioButtonCheckColor": "#316AC5",
        "CheckBoxBackColor": "#FFFFFF",
        "CheckBoxForeColor": "#333333",
        "CheckBoxCheckColor": "#316AC5",
        "CheckBoxCheckBackColor": "#FFFFFF",
        "DataGridBackColor": "#FFFFFF",
        "DataGridForeColor": "#000000",
        "DataGridSelectionBackColor": "#316AC5",
        "DataGridSelectionForeColor": "#FFFFFF",
        "DataGridRowBackColor": "#FFFFFF",
        "DataGridAltRowBackColor": "#F0F8FF",
        "DataGridHeaderBackColor": "#F0F0F0",
        "DataGridHeaderForeColor": "#000000",
        "DataGridGridColor": "#CCCCCC",
        "DataGridBorderColor": "#CCCCCC",
        "TreeViewBackColor": "#FFFFFF",
        "TreeViewForeColor": "#000000",
        "TreeViewLineColor": "#CCCCCC",
        "TreeViewSelectedNodeBackColor": "#316AC5",
        "TreeViewSelectedNodeForeColor": "#FFFFFF",
        "TreeViewBorderColor": "#CCCCCC",
        "ListViewBackColor": "#FFFFFF",
        "ListViewForeColor": "#000000",
        "ListViewSelectionBackColor": "#316AC5",
        "ListViewSelectionForeColor": "#FFFFFF",
        "ListViewBorderColor": "#CCCCCC",
        "ListViewHeaderBackColor": "#F0F0F0",
        "ListViewHeaderForeColor": "#000000",
        "MenuStripBackColor": "#F0F0F0",
        "MenuStripForeColor": "#000000",
        "MenuStripBorderColor": "#CCCCCC",
        "MenuStripItemHoverBackColor": "#316AC5",
        "MenuStripItemHoverForeColor": "#FFFFFF",
        "MenuStripItemSelectedBackColor": "#316AC5",
        "MenuStripItemSelectedForeColor": "#FFFFFF",
        "StatusStripBackColor": "#F0F0F0",
        "StatusStripForeColor": "#000000",
        "StatusStripBorderColor": "#CCCCCC",
        "ToolStripBackColor": "#F0F0F0",
        "ToolStripForeColor": "#000000",
        "ToolStripBorderColor": "#CCCCCC",
        "ToolStripItemHoverBackColor": "#316AC5",
        "ToolStripItemHoverForeColor": "#FFFFFF",
        "TabControlBackColor": "#F0F0F0",
        "TabControlForeColor": "#333333",
        "TabControlBorderColor": "#CCCCCC",
        "TabPageBackColor": "#FFFFFF",
        "TabPageForeColor": "#000000",
        "TabPageBorderColor": "#CCCCCC",
        "TabSelectedBackColor": "#F0F0F0",
        "TabSelectedForeColor": "#000000",
        "TabUnselectedBackColor": "#E0E0E0",
        "TabUnselectedForeColor": "#999999",
        "GroupBoxBackColor": "#F0F0F0",
        "GroupBoxForeColor": "#000000",
        "GroupBoxBorderColor": "#CCCCCC",
        "PanelBackColor": "#F0F0F0",
        "PanelForeColor": "#000000",
        "PanelBorderColor": "#CCCCCC",
        "SplitContainerBackColor": "#F0F0F0",
        "SplitContainerForeColor": "#000000",
        "SplitContainerSplitterColor": "#CCCCCC",
        "FlowLayoutPanelBackColor": "#F0F0F0",
        "FlowLayoutPanelForeColor": "#000000",
        "FlowLayoutPanelBorderColor": "#CCCCCC",
        "TableLayoutPanelBackColor": "#F0F0F0",
        "TableLayoutPanelForeColor": "#000000",
        "TableLayoutPanelBorderColor": "#CCCCCC",
        "TableLayoutPanelCellBorderColor": "#CCCCCC",
        "LinkLabelLinkColor": "#316AC5",
        "LinkLabelActiveLinkColor": "#6495ED",
        "LinkLabelVisitedLinkColor": "#1E5E8C",
        "LinkLabelHoverColor": "#6495ED",
        "LinkLabelBackColor": "#FFFFFF",
        "LinkLabelForeColor": "#316AC5",
        "ProgressBarBackColor": "#F0F0F0",
        "ProgressBarForeColor": "#316AC5",
        "ProgressBarBorderColor": "#CCCCCC",
        "TrackBarBackColor": "#F0F0F0",
        "TrackBarForeColor": "#316AC5",
        "TrackBarThumbColor": "#316AC5",
        "TrackBarTickColor": "#CCCCCC",
        "DateTimePickerBackColor": "#FFFFFF",
        "DateTimePickerForeColor": "#000000",
        "DateTimePickerBorderColor": "#CCCCCC",
        "DateTimePickerDropDownBackColor": "#F0F0F0",
        "DateTimePickerDropDownForeColor": "#000000",
        "MonthCalendarBackColor": "#FFFFFF",
        "MonthCalendarForeColor": "#000000",
        "MonthCalendarTitleBackColor": "#316AC5",
        "MonthCalendarTitleForeColor": "#FFFFFF",
        "MonthCalendarTrailingForeColor": "#CCCCCC",
        "MonthCalendarTodayBackColor": "#F0F0F0",
        "MonthCalendarTodayForeColor": "#000000",
        "MonthCalendarBorderColor": "#CCCCCC",
        "NumericUpDownBackColor": "#FFFFFF",
        "NumericUpDownForeColor": "#000000",
        "NumericUpDownErrorForeColor": "#FF0000",
        "NumericUpDownBorderColor": "#CCCCCC",
        "NumericUpDownButtonBackColor": "#F0F0F0",
        "NumericUpDownButtonForeColor": "#000000",
        "DomainUpDownBackColor": "#FFFFFF",
        "DomainUpDownForeColor": "#000000",
        "DomainUpDownErrorForeColor": "#FF0000",
        "DomainUpDownBorderColor": "#CCCCCC",
        "DomainUpDownButtonBackColor": "#F0F0F0",
        "DomainUpDownButtonForeColor": "#000000",
        "HScrollBarBackColor": "#F0F0F0",
        "HScrollBarForeColor": "#000000",
        "HScrollBarThumbColor": "#CCCCCC",
        "HScrollBarTrackColor": "#F0F0F0",
        "VScrollBarBackColor": "#F0F0F0",
        "VScrollBarForeColor": "#000000",
        "VScrollBarThumbColor": "#CCCCCC",
        "VScrollBarTrackColor": "#F0F0F0",
        "PictureBoxBackColor": "#FFFFFF",
        "PictureBoxBorderColor": "#CCCCCC",
        "PropertyGridBackColor": "#FFFFFF",
        "PropertyGridForeColor": "#000000",
        "PropertyGridLineColor": "#CCCCCC",
        "PropertyGridCategoryBackColor": "#F0F0F0",
        "PropertyGridCategoryForeColor": "#000000",
        "PropertyGridSelectedBackColor": "#316AC5",
        "PropertyGridSelectedForeColor": "#FFFFFF",
        "WebBrowserBackColor": "#FFFFFF",
        "WebBrowserBorderColor": "#CCCCCC",
        "UserControlBackColor": "#FFFFFF",
        "UserControlForeColor": "#000000",
        "UserControlBorderColor": "#CCCCCC",
        "CustomControlBackColor": "#FFFFFF",
        "CustomControlForeColor": "#000000",
        "CustomControlBorderColor": "#CCCCCC",
        "ToolTipBackColor": "#F0F0F0",
        "ToolTipForeColor": "#000000",
        "ToolTipBorderColor": "#CCCCCC",
        "ContextMenuBackColor": "#F0F0F0",
        "ContextMenuForeColor": "#000000",
        "ContextMenuBorderColor": "#CCCCCC",
        "ContextMenuItemHoverBackColor": "#316AC5",
        "ContextMenuItemHoverForeColor": "#FFFFFF",
        "ContextMenuSeparatorColor": "#CCCCCC",
        "AccentColor": "#316AC5",
        "SecondaryAccentColor": "#6495ED",
        "ErrorColor": "#FF0000",
        "WarningColor": "#FFA726",
        "SuccessColor": "#66BB6A",
        "InfoColor": "#316AC5",
        "WindowTitleBarBackColor": "#F0F0F0",
        "WindowTitleBarForeColor": "#000000",
        "WindowTitleBarInactiveBackColor": "#E0E0E0",
        "WindowTitleBarInactiveForeColor": "#999999",
        "WindowBorderColor": "#CCCCCC",
        "WindowResizeHandleColor": "#CCCCCC"
    }',
    1,
    1,
    'Default light theme with all color settings',
    'SYSTEM'
);

-- Insert Dark Theme
INSERT IGNORE INTO app_themes (ThemeName, DisplayName, SettingsJson, IsDefault, IsActive, Description, CreatedBy) VALUES (
    'Dark',
    'Dark Theme',
    '{
        "FormBackColor": "#1E1E1E",
        "FormForeColor": "#FFFFFF",
        "FormBorderColor": "#3C3C3C",
        "ControlBackColor": "#1E1E1E",
        "ControlForeColor": "#FFFFFF",
        "ControlFocusedBackColor": "#007ACC",
        "LabelBackColor": "#1E1E1E",
        "LabelForeColor": "#CCCCCC",
        "TextBoxBackColor": "#1E1E1E",
        "TextBoxForeColor": "#FFFFFF",
        "TextBoxSelectionBackColor": "#007ACC",
        "TextBoxSelectionForeColor": "#FFFFFF",
        "TextBoxErrorForeColor": "#E57373",
        "TextBoxBorderColor": "#3C3C3C",
        "MaskedTextBoxBackColor": "#1E1E1E",
        "MaskedTextBoxForeColor": "#FFFFFF",
        "MaskedTextBoxErrorForeColor": "#E57373",
        "MaskedTextBoxBorderColor": "#3C3C3C",
        "RichTextBoxBackColor": "#1E1E1E",
        "RichTextBoxForeColor": "#FFFFFF",
        "RichTextBoxSelectionBackColor": "#007ACC",
        "RichTextBoxSelectionForeColor": "#FFFFFF",
        "RichTextBoxErrorForeColor": "#E57373",
        "RichTextBoxBorderColor": "#3C3C3C",
        "ComboBoxBackColor": "#2D2D30",
        "ComboBoxForeColor": "#FFFFFF",
        "ComboBoxSelectionBackColor": "#007ACC",
        "ComboBoxSelectionForeColor": "#FFFFFF",
        "ComboBoxErrorForeColor": "#E57373",
        "ComboBoxBorderColor": "#3C3C3C",
        "ComboBoxDropDownBackColor": "#2D2D30",
        "ComboBoxDropDownForeColor": "#FFFFFF",
        "ListBoxBackColor": "#1E1E1E",
        "ListBoxForeColor": "#FFFFFF",
        "ListBoxSelectionBackColor": "#007ACC",
        "ListBoxSelectionForeColor": "#FFFFFF",
        "ListBoxBorderColor": "#3C3C3C",
        "CheckedListBoxBackColor": "#1E1E1E",
        "CheckedListBoxForeColor": "#FFFFFF",
        "CheckedListBoxBorderColor": "#3C3C3C",
        "CheckedListBoxCheckBackColor": "#1E1E1E",
        "CheckedListBoxCheckForeColor": "#007ACC",
        "ButtonBackColor": "#3C3C3C",
        "ButtonForeColor": "#FFFFFF",
        "ButtonBorderColor": "#444444",
        "ButtonHoverBackColor": "#007ACC",
        "ButtonHoverForeColor": "#FFFFFF",
        "ButtonPressedBackColor": "#005A9E",
        "ButtonPressedForeColor": "#FFFFFF",
        "RadioButtonBackColor": "#1E1E1E",
        "RadioButtonForeColor": "#CCCCCC",
        "RadioButtonCheckColor": "#007ACC",
        "CheckBoxBackColor": "#1E1E1E",
        "CheckBoxForeColor": "#CCCCCC",
        "CheckBoxCheckColor": "#007ACC",
        "CheckBoxCheckBackColor": "#1E1E1E",
        "DataGridBackColor": "#2D2D30",
        "DataGridForeColor": "#FFFFFF",
        "DataGridSelectionBackColor": "#3399FF",
        "DataGridSelectionForeColor": "#FFFFFF",
        "DataGridRowBackColor": "#2D2D30",
        "DataGridAltRowBackColor": "#37373A",
        "DataGridHeaderBackColor": "#3C3C3C",
        "DataGridHeaderForeColor": "#FFFFFF",
        "DataGridGridColor": "#3C3C3C",
        "DataGridBorderColor": "#3C3C3C",
        "TreeViewBackColor": "#1E1E1E",
        "TreeViewForeColor": "#FFFFFF",
        "TreeViewLineColor": "#999999",
        "TreeViewSelectedNodeBackColor": "#007ACC",
        "TreeViewSelectedNodeForeColor": "#FFFFFF",
        "TreeViewBorderColor": "#3C3C3C",
        "ListViewBackColor": "#1E1E1E",
        "ListViewForeColor": "#FFFFFF",
        "ListViewSelectionBackColor": "#007ACC",
        "ListViewSelectionForeColor": "#FFFFFF",
        "ListViewBorderColor": "#3C3C3C",
        "ListViewHeaderBackColor": "#3C3C3C",
        "ListViewHeaderForeColor": "#FFFFFF",
        "MenuStripBackColor": "#1E1E1E",
        "MenuStripForeColor": "#FFFFFF",
        "MenuStripBorderColor": "#3C3C3C",
        "MenuStripItemHoverBackColor": "#007ACC",
        "MenuStripItemHoverForeColor": "#FFFFFF",
        "MenuStripItemSelectedBackColor": "#007ACC",
        "MenuStripItemSelectedForeColor": "#FFFFFF",
        "StatusStripBackColor": "#2D2D30",
        "StatusStripForeColor": "#FFFFFF",
        "StatusStripBorderColor": "#3C3C3C",
        "ToolStripBackColor": "#2D2D30",
        "ToolStripForeColor": "#FFFFFF",
        "ToolStripBorderColor": "#3C3C3C",
        "ToolStripItemHoverBackColor": "#007ACC",
        "ToolStripItemHoverForeColor": "#FFFFFF",
        "TabControlBackColor": "#1E1E1E",
        "TabControlForeColor": "#CCCCCC",
        "TabControlBorderColor": "#3C3C3C",
        "TabPageBackColor": "#2D2D30",
        "TabPageForeColor": "#FFFFFF",
        "TabPageBorderColor": "#3C3C3C",
        "TabSelectedBackColor": "#2D2D30",
        "TabSelectedForeColor": "#FFFFFF",
        "TabUnselectedBackColor": "#1A1A1A",
        "TabUnselectedForeColor": "#888888",
        "GroupBoxBackColor": "#2D2D30",
        "GroupBoxForeColor": "#FFFFFF",
        "GroupBoxBorderColor": "#3C3C3C",
        "PanelBackColor": "#2D2D30",
        "PanelForeColor": "#FFFFFF",
        "PanelBorderColor": "#3C3C3C",
        "SplitContainerBackColor": "#1E1E1E",
        "SplitContainerForeColor": "#FFFFFF",
        "SplitContainerSplitterColor": "#3C3C3C",
        "FlowLayoutPanelBackColor": "#2D2D30",
        "FlowLayoutPanelForeColor": "#FFFFFF",
        "FlowLayoutPanelBorderColor": "#3C3C3C",
        "TableLayoutPanelBackColor": "#2D2D30",
        "TableLayoutPanelForeColor": "#FFFFFF",
        "TableLayoutPanelBorderColor": "#3C3C3C",
        "TableLayoutPanelCellBorderColor": "#3C3C3C",
        "LinkLabelLinkColor": "#007ACC",
        "LinkLabelActiveLinkColor": "#66CCFF",
        "LinkLabelVisitedLinkColor": "#005A9E",
        "LinkLabelHoverColor": "#66CCFF",
        "LinkLabelBackColor": "#1E1E1E",
        "LinkLabelForeColor": "#3399FF",
        "ProgressBarBackColor": "#1E1E1E",
        "ProgressBarForeColor": "#007ACC",
        "ProgressBarBorderColor": "#3C3C3C",
        "TrackBarBackColor": "#1E1E1E",
        "TrackBarForeColor": "#007ACC",
        "TrackBarThumbColor": "#007ACC",
        "TrackBarTickColor": "#999999",
        "DateTimePickerBackColor": "#1E1E1E",
        "DateTimePickerForeColor": "#FFFFFF",
        "DateTimePickerBorderColor": "#3C3C3C",
        "DateTimePickerDropDownBackColor": "#2D2D30",
        "DateTimePickerDropDownForeColor": "#FFFFFF",
        "MonthCalendarBackColor": "#1E1E1E",
        "MonthCalendarForeColor": "#FFFFFF",
        "MonthCalendarTitleBackColor": "#007ACC",
        "MonthCalendarTitleForeColor": "#FFFFFF",
        "MonthCalendarTrailingForeColor": "#999999",
        "MonthCalendarTodayBackColor": "#2D2D30",
        "MonthCalendarTodayForeColor": "#FFFFFF",
        "MonthCalendarBorderColor": "#3C3C3C",
        "NumericUpDownBackColor": "#1E1E1E",
        "NumericUpDownForeColor": "#FFFFFF",
        "NumericUpDownErrorForeColor": "#E57373",
        "NumericUpDownBorderColor": "#3C3C3C",
        "NumericUpDownButtonBackColor": "#3C3C3C",
        "NumericUpDownButtonForeColor": "#FFFFFF",
        "DomainUpDownBackColor": "#1E1E1E",
        "DomainUpDownForeColor": "#FFFFFF",
        "DomainUpDownErrorForeColor": "#E57373",
        "DomainUpDownBorderColor": "#3C3C3C",
        "DomainUpDownButtonBackColor": "#3C3C3C",
        "DomainUpDownButtonForeColor": "#FFFFFF",
        "HScrollBarBackColor": "#1E1E1E",
        "HScrollBarForeColor": "#FFFFFF",
        "HScrollBarThumbColor": "#3C3C3C",
        "HScrollBarTrackColor": "#1E1E1E",
        "VScrollBarBackColor": "#1E1E1E",
        "VScrollBarForeColor": "#FFFFFF",
        "VScrollBarThumbColor": "#3C3C3C",
        "VScrollBarTrackColor": "#1E1E1E",
        "PictureBoxBackColor": "#1E1E1E",
        "PictureBoxBorderColor": "#3C3C3C",
        "PropertyGridBackColor": "#1E1E1E",
        "PropertyGridForeColor": "#FFFFFF",
        "PropertyGridLineColor": "#3C3C3C",
        "PropertyGridCategoryBackColor": "#2D2D30",
        "PropertyGridCategoryForeColor": "#FFFFFF",
        "PropertyGridSelectedBackColor": "#007ACC",
        "PropertyGridSelectedForeColor": "#FFFFFF",
        "WebBrowserBackColor": "#1E1E1E",
        "WebBrowserBorderColor": "#3C3C3C",
        "UserControlBackColor": "#1E1E1E",
        "UserControlForeColor": "#FFFFFF",
        "UserControlBorderColor": "#3C3C3C",
        "CustomControlBackColor": "#1E1E1E",
        "CustomControlForeColor": "#FFFFFF",
        "CustomControlBorderColor": "#3C3C3C",
        "ToolTipBackColor": "#2D2D30",
        "ToolTipForeColor": "#FFFFFF",
        "ToolTipBorderColor": "#3C3C3C",
        "ContextMenuBackColor": "#2D2D30",
        "ContextMenuForeColor": "#FFFFFF",
        "ContextMenuBorderColor": "#3C3C3C",
        "ContextMenuItemHoverBackColor": "#007ACC",
        "ContextMenuItemHoverForeColor": "#FFFFFF",
        "ContextMenuSeparatorColor": "#3C3C3C",
        "AccentColor": "#007ACC",
        "SecondaryAccentColor": "#66CCFF",
        "ErrorColor": "#E57373",
        "WarningColor": "#FFA726",
        "SuccessColor": "#66BB6A",
        "InfoColor": "#3399FF",
        "WindowTitleBarBackColor": "#1A1A1A",
        "WindowTitleBarForeColor": "#FFFFFF",
        "WindowTitleBarInactiveBackColor": "#2D2D30",
        "WindowTitleBarInactiveForeColor": "#888888",
        "WindowBorderColor": "#3C3C3C",
        "WindowResizeHandleColor": "#3C3C3C"
    }',
    1,
    1,
    'Professional dark theme with all color settings',
    'SYSTEM'
);

-- Insert Blue Theme
INSERT IGNORE INTO app_themes (ThemeName, DisplayName, SettingsJson, IsDefault, IsActive, Description, CreatedBy) VALUES (
    'Blue',
    'Blue Professional Theme',
    '{
        "FormBackColor": "#F0F8FF",
        "FormForeColor": "#191919",
        "FormBorderColor": "#4682B4",
        "ControlBackColor": "#F0F8FF",
        "ControlForeColor": "#191919",
        "ControlFocusedBackColor": "#4682B4",
        "LabelBackColor": "#F0F8FF",
        "LabelForeColor": "#191919",
        "TextBoxBackColor": "#FFFFFF",
        "TextBoxForeColor": "#191919",
        "TextBoxSelectionBackColor": "#4682B4",
        "TextBoxSelectionForeColor": "#FFFFFF",
        "TextBoxErrorForeColor": "#FF0000",
        "TextBoxBorderColor": "#4682B4",
        "MaskedTextBoxBackColor": "#F0F8FF",
        "MaskedTextBoxForeColor": "#191919",
        "MaskedTextBoxErrorForeColor": "#FF0000",
        "MaskedTextBoxBorderColor": "#4682B4",
        "RichTextBoxBackColor": "#F0F8FF",
        "RichTextBoxForeColor": "#191919",
        "RichTextBoxSelectionBackColor": "#4682B4",
        "RichTextBoxSelectionForeColor": "#FFFFFF",
        "RichTextBoxErrorForeColor": "#FF0000",
        "RichTextBoxBorderColor": "#4682B4",
        "ComboBoxBackColor": "#FFFFFF",
        "ComboBoxForeColor": "#191919",
        "ComboBoxSelectionBackColor": "#4682B4",
        "ComboBoxSelectionForeColor": "#FFFFFF",
        "ComboBoxErrorForeColor": "#FF0000",
        "ComboBoxBorderColor": "#4682B4",
        "ComboBoxDropDownBackColor": "#F0F8FF",
        "ComboBoxDropDownForeColor": "#191919",
        "ListBoxBackColor": "#F0F8FF",
        "ListBoxForeColor": "#191919",
        "ListBoxSelectionBackColor": "#4682B4",
        "ListBoxSelectionForeColor": "#FFFFFF",
        "ListBoxBorderColor": "#4682B4",
        "CheckedListBoxBackColor": "#F0F8FF",
        "CheckedListBoxForeColor": "#191919",
        "CheckedListBoxBorderColor": "#4682B4",
        "CheckedListBoxCheckBackColor": "#F0F8FF",
        "CheckedListBoxCheckForeColor": "#4682B4",
        "ButtonBackColor": "#4682B4",
        "ButtonForeColor": "#FFFFFF",
        "ButtonBorderColor": "#4682B4",
        "ButtonHoverBackColor": "#6495ED",
        "ButtonHoverForeColor": "#FFFFFF",
        "ButtonPressedBackColor": "#1E5E8C",
        "ButtonPressedForeColor": "#FFFFFF",
        "RadioButtonBackColor": "#F0F8FF",
        "RadioButtonForeColor": "#191919",
        "RadioButtonCheckColor": "#4682B4",
        "CheckBoxBackColor": "#F0F8FF",
        "CheckBoxForeColor": "#191919",
        "CheckBoxCheckColor": "#4682B4",
        "CheckBoxCheckBackColor": "#F0F8FF",
        "DataGridBackColor": "#FFFFFF",
        "DataGridForeColor": "#191919",
        "DataGridSelectionBackColor": "#4682B4",
        "DataGridSelectionForeColor": "#FFFFFF",
        "DataGridRowBackColor": "#FFFFFF",
        "DataGridAltRowBackColor": "#E6F0FF",
        "DataGridHeaderBackColor": "#4682B4",
        "DataGridHeaderForeColor": "#FFFFFF",
        "DataGridGridColor": "#4682B4",
        "DataGridBorderColor": "#4682B4",
        "TreeViewBackColor": "#F0F8FF",
        "TreeViewForeColor": "#191919",
        "TreeViewLineColor": "#4682B4",
        "TreeViewSelectedNodeBackColor": "#4682B4",
        "TreeViewSelectedNodeForeColor": "#FFFFFF",
        "TreeViewBorderColor": "#4682B4",
        "ListViewBackColor": "#F0F8FF",
        "ListViewForeColor": "#191919",
        "ListViewSelectionBackColor": "#4682B4",
        "ListViewSelectionForeColor": "#FFFFFF",
        "ListViewBorderColor": "#4682B4",
        "ListViewHeaderBackColor": "#4682B4",
        "ListViewHeaderForeColor": "#FFFFFF",
        "MenuStripBackColor": "#F0F8FF",
        "MenuStripForeColor": "#191919",
        "MenuStripBorderColor": "#4682B4",
        "MenuStripItemHoverBackColor": "#4682B4",
        "MenuStripItemHoverForeColor": "#FFFFFF",
        "MenuStripItemSelectedBackColor": "#4682B4",
        "MenuStripItemSelectedForeColor": "#FFFFFF",
        "StatusStripBackColor": "#F0F8FF",
        "StatusStripForeColor": "#191919",
        "StatusStripBorderColor": "#4682B4",
        "ToolStripBackColor": "#F0F8FF",
        "ToolStripForeColor": "#191919",
        "ToolStripBorderColor": "#4682B4",
        "ToolStripItemHoverBackColor": "#4682B4",
        "ToolStripItemHoverForeColor": "#FFFFFF",
        "TabControlBackColor": "#F0F8FF",
        "TabControlForeColor": "#191919",
        "TabControlBorderColor": "#4682B4",
        "TabPageBackColor": "#E6F0FF",
        "TabPageForeColor": "#191919",
        "TabPageBorderColor": "#4682B4",
        "TabSelectedBackColor": "#E6F0FF",
        "TabSelectedForeColor": "#191919",
        "TabUnselectedBackColor": "#F0F8FF",
        "TabUnselectedForeColor": "#4682B4",
        "GroupBoxBackColor": "#E6F0FF",
        "GroupBoxForeColor": "#191919",
        "GroupBoxBorderColor": "#4682B4",
        "PanelBackColor": "#E6F0FF",
        "PanelForeColor": "#191919",
        "PanelBorderColor": "#4682B4",
        "SplitContainerBackColor": "#F0F8FF",
        "SplitContainerForeColor": "#191919",
        "SplitContainerSplitterColor": "#4682B4",
        "FlowLayoutPanelBackColor": "#E6F0FF",
        "FlowLayoutPanelForeColor": "#191919",
        "FlowLayoutPanelBorderColor": "#4682B4",
        "TableLayoutPanelBackColor": "#E6F0FF",
        "TableLayoutPanelForeColor": "#191919",
        "TableLayoutPanelBorderColor": "#4682B4",
        "TableLayoutPanelCellBorderColor": "#4682B4",
        "LinkLabelLinkColor": "#4682B4",
        "LinkLabelActiveLinkColor": "#6495ED",
        "LinkLabelVisitedLinkColor": "#1E5E8C",
        "LinkLabelHoverColor": "#6495ED",
        "LinkLabelBackColor": "#F0F8FF",
        "LinkLabelForeColor": "#4682B4",
        "ProgressBarBackColor": "#F0F8FF",
        "ProgressBarForeColor": "#4682B4",
        "ProgressBarBorderColor": "#4682B4",
        "TrackBarBackColor": "#F0F8FF",
        "TrackBarForeColor": "#4682B4",
        "TrackBarThumbColor": "#4682B4",
        "TrackBarTickColor": "#4682B4",
        "DateTimePickerBackColor": "#FFFFFF",
        "DateTimePickerForeColor": "#191919",
        "DateTimePickerBorderColor": "#4682B4",
        "DateTimePickerDropDownBackColor": "#F0F8FF",
        "DateTimePickerDropDownForeColor": "#191919",
        "MonthCalendarBackColor": "#F0F8FF",
        "MonthCalendarForeColor": "#191919",
        "MonthCalendarTitleBackColor": "#4682B4",
        "MonthCalendarTitleForeColor": "#FFFFFF",
        "MonthCalendarTrailingForeColor": "#4682B4",
        "MonthCalendarTodayBackColor": "#E6F0FF",
        "MonthCalendarTodayForeColor": "#191919",
        "MonthCalendarBorderColor": "#4682B4",
        "NumericUpDownBackColor": "#FFFFFF",
        "NumericUpDownForeColor": "#191919",
        "NumericUpDownErrorForeColor": "#FF0000",
        "NumericUpDownBorderColor": "#4682B4",
        "NumericUpDownButtonBackColor": "#4682B4",
        "NumericUpDownButtonForeColor": "#FFFFFF",
        "DomainUpDownBackColor": "#FFFFFF",
        "DomainUpDownForeColor": "#191919",
        "DomainUpDownErrorForeColor": "#FF0000",
        "DomainUpDownBorderColor": "#4682B4",
        "DomainUpDownButtonBackColor": "#4682B4",
        "DomainUpDownButtonForeColor": "#FFFFFF",
        "HScrollBarBackColor": "#F0F8FF",
        "HScrollBarForeColor": "#191919",
        "HScrollBarThumbColor": "#4682B4",
        "HScrollBarTrackColor": "#F0F8FF",
        "VScrollBarBackColor": "#F0F8FF",
        "VScrollBarForeColor": "#191919",
        "VScrollBarThumbColor": "#4682B4",
        "VScrollBarTrackColor": "#F0F8FF",
        "PictureBoxBackColor": "#F0F8FF",
        "PictureBoxBorderColor": "#4682B4",
        "PropertyGridBackColor": "#F0F8FF",
        "PropertyGridForeColor": "#191919",
        "PropertyGridLineColor": "#4682B4",
        "PropertyGridCategoryBackColor": "#E6F0FF",
        "PropertyGridCategoryForeColor": "#191919",
        "PropertyGridSelectedBackColor": "#4682B4",
        "PropertyGridSelectedForeColor": "#FFFFFF",
        "WebBrowserBackColor": "#F0F8FF",
        "WebBrowserBorderColor": "#4682B4",
        "UserControlBackColor": "#F0F8FF",
        "UserControlForeColor": "#191919",
        "UserControlBorderColor": "#4682B4",
        "CustomControlBackColor": "#F0F8FF",
        "CustomControlForeColor": "#191919",
        "CustomControlBorderColor": "#4682B4",
        "ToolTipBackColor": "#E6F0FF",
        "ToolTipForeColor": "#191919",
        "ToolTipBorderColor": "#4682B4",
        "ContextMenuBackColor": "#F0F8FF",
        "ContextMenuForeColor": "#191919",
        "ContextMenuBorderColor": "#4682B4",
        "ContextMenuItemHoverBackColor": "#4682B4",
        "ContextMenuItemHoverForeColor": "#FFFFFF",
        "ContextMenuSeparatorColor": "#4682B4",
        "AccentColor": "#4682B4",
        "SecondaryAccentColor": "#6495ED",
        "ErrorColor": "#FF0000",
        "WarningColor": "#FFA726",
        "SuccessColor": "#66BB6A",
        "InfoColor": "#4682B4",
        "WindowTitleBarBackColor": "#F0F8FF",
        "WindowTitleBarForeColor": "#191919",
        "WindowTitleBarInactiveBackColor": "#E6F0FF",
        "WindowTitleBarInactiveForeColor": "#4682B4",
        "WindowBorderColor": "#4682B4",
        "WindowResizeHandleColor": "#4682B4"
    }',
    1,
    1,
    'Professional blue theme with all color settings',
    'SYSTEM'
);

-- ================================================================================
-- THEME MANAGEMENT PROCEDURES
-- ================================================================================

-- Get all themes from app_themes table (direct table query as stored procedure)
DELIMITER $$
CREATE PROCEDURE app_themes_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving themes';
    END;
    
    -- Simply return all rows from app_themes table (like the old Helper_Database_Core.ExecuteDataTable approach)
    SELECT * FROM app_themes WHERE IsActive = 1 ORDER BY ThemeName;
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Themes retrieved successfully';
END $$
DELIMITER ;

-- Get specific theme by name with status reporting
DELIMITER $$
CREATE PROCEDURE app_themes_Get_ByName(
    IN p_ThemeName VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving theme: ', p_ThemeName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsActive = 1;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme not found or inactive: ', p_ThemeName);
        -- Return empty result set with structure
        SELECT NULL as ThemeName, NULL as SettingsJson LIMIT 0;
    ELSE
        SELECT 
            ID,
            ThemeName,
            DisplayName,
            SettingsJson,
            IsDefault,
            IsActive,
            Description,
            CreatedDate,
            CreatedBy,
            ModifiedDate,
            ModifiedBy,
            VERSION
        FROM app_themes 
        WHERE ThemeName = p_ThemeName AND IsActive = 1
        LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Theme retrieved successfully: ', p_ThemeName);
    END IF;
END $$
DELIMITER ;

-- Add new theme with validation
DELIMITER $$
CREATE PROCEDURE app_themes_Add_Theme(
    IN p_ThemeName VARCHAR(50),
    IN p_DisplayName VARCHAR(100),
    IN p_SettingsJson TEXT,
    IN p_Description TEXT,
    IN p_CreatedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ThemeId INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while creating theme: ', p_ThemeName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Validate theme name is not empty
    IF p_ThemeName IS NULL OR TRIM(p_ThemeName) = '' THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Theme name cannot be empty';
        ROLLBACK;
    -- Check if theme already exists
    ELSEIF EXISTS(SELECT 1 FROM app_themes WHERE ThemeName = p_ThemeName) THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme already exists: ', p_ThemeName);
        ROLLBACK;
    -- Validate JSON format (basic check)
    ELSEIF p_SettingsJson IS NULL OR TRIM(p_SettingsJson) = '' THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Theme settings JSON cannot be empty';
        ROLLBACK;
    ELSE
        INSERT INTO app_themes (
            ThemeName,
            DisplayName,
            SettingsJson,
            IsDefault,
            IsActive,
            Description,
            CreatedBy,
            CreatedDate
        ) VALUES (
            p_ThemeName,
            IFNULL(p_DisplayName, p_ThemeName),
            p_SettingsJson,
            0, -- New themes are never default
            1, -- New themes are active by default
            p_Description,
            IFNULL(p_CreatedBy, 'SYSTEM'),
            NOW()
        );
        
        SET v_ThemeId = LAST_INSERT_ID();
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Theme created successfully: ', p_ThemeName, ' (ID: ', v_ThemeId, ')');
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Update existing theme
DELIMITER $$
CREATE PROCEDURE app_themes_Update_Theme(
    IN p_ThemeName VARCHAR(50),
    IN p_DisplayName VARCHAR(100),
    IN p_SettingsJson TEXT,
    IN p_Description TEXT,
    IN p_ModifiedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating theme: ', p_ThemeName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if theme exists and is not a default system theme
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsDefault = 0;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme not found or is a protected system theme: ', p_ThemeName);
        ROLLBACK;
    ELSE
        UPDATE app_themes 
        SET DisplayName = IFNULL(p_DisplayName, DisplayName),
            SettingsJson = IFNULL(p_SettingsJson, SettingsJson),
            Description = p_Description,
            ModifiedBy = IFNULL(p_ModifiedBy, 'SYSTEM'),
            ModifiedDate = NOW(),
            VERSION = VERSION + 1
        WHERE ThemeName = p_ThemeName AND IsDefault = 0;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Theme updated successfully: ', p_ThemeName);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to theme: ', p_ThemeName);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Delete theme (soft delete by setting IsActive = 0)
DELIMITER $$
CREATE PROCEDURE app_themes_Delete_Theme(
    IN p_ThemeName VARCHAR(50),
    IN p_ModifiedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting theme: ', p_ThemeName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if theme exists and is not a default system theme
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsDefault = 0 AND IsActive = 1;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme not found, already deleted, or is a protected system theme: ', p_ThemeName);
        ROLLBACK;
    ELSE
        -- Soft delete by setting IsActive = 0
        UPDATE app_themes 
        SET IsActive = 0,
            ModifiedBy = IFNULL(p_ModifiedBy, 'SYSTEM'),
            ModifiedDate = NOW()
        WHERE ThemeName = p_ThemeName AND IsDefault = 0;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Theme deleted successfully: ', p_ThemeName);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete theme: ', p_ThemeName);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Check if theme exists
DELIMITER $$
CREATE PROCEDURE app_themes_Exists(
    IN p_ThemeName VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while checking theme existence: ', p_ThemeName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsActive = 1;
    SELECT v_Count as ThemeExists;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Theme existence check completed for: ', p_ThemeName, ' (Exists: ', IF(v_Count > 0, 'Yes', 'No'), ')');
END $$
DELIMITER ;

-- ================================================================================
-- USER THEME PREFERENCE PROCEDURES
-- ================================================================================

-- Get user's selected theme
DELIMITER $$
CREATE PROCEDURE app_themes_Get_UserTheme(
    IN p_UserId VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ThemeName VARCHAR(50) DEFAULT NULL;
    DECLARE v_ThemeExists INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving user theme for: ', p_UserId);
    END;
    
    -- Get user's theme preference from usr_users table
    SELECT Theme_Name INTO v_ThemeName 
    FROM usr_users 
    WHERE User = p_UserId 
    LIMIT 1;
    
    -- If no theme set or theme doesn't exist, default to 'Default' theme
    IF v_ThemeName IS NULL OR v_ThemeName = '' THEN
        SET v_ThemeName = 'Default';
    ELSE
        -- Check if the user's preferred theme actually exists and is active
        SELECT COUNT(*) INTO v_ThemeExists 
        FROM app_themes 
        WHERE ThemeName = v_ThemeName AND IsActive = 1;
        
        -- If theme doesn't exist, fall back to Default
        IF v_ThemeExists = 0 THEN
            SET v_ThemeName = 'Default';
        END IF;
    END IF;
    
    -- Return the theme data
    SELECT 
        t.ID,
        t.ThemeName,
        t.DisplayName,
        t.SettingsJson,
        t.IsDefault,
        t.Description
    FROM app_themes t
    WHERE t.ThemeName = v_ThemeName AND t.IsActive = 1
    LIMIT 1;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('User theme retrieved successfully for: ', p_UserId, ' (Theme: ', v_ThemeName, ')');
END $$
DELIMITER ;

-- Set user's theme preference
DELIMITER $$
CREATE PROCEDURE app_themes_Set_UserTheme(
    IN p_UserId VARCHAR(100),
    IN p_ThemeName VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_UserExists INT DEFAULT 0;
    DECLARE v_ThemeExists INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while setting theme for user: ', p_UserId);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if user exists
    SELECT COUNT(*) INTO v_UserExists FROM usr_users WHERE User = p_UserId;
    
    IF v_UserExists = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_UserId);
        ROLLBACK;
    ELSE
        -- Check if theme exists and is active
        SELECT COUNT(*) INTO v_ThemeExists FROM app_themes WHERE ThemeName = p_ThemeName AND IsActive = 1;
        
        IF v_ThemeExists = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Theme not found or inactive: ', p_ThemeName);
            ROLLBACK;
        ELSE
            -- Update user's theme preference
            UPDATE usr_users 
            SET Theme_Name = p_ThemeName,
                ModifiedDate = NOW()
            WHERE User = p_UserId;
            
            SET v_RowsAffected = ROW_COUNT();
            
            IF v_RowsAffected > 0 THEN
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('Theme set successfully for user: ', p_UserId, ' (Theme: ', p_ThemeName, ')');
            ELSE
                SET p_Status = 2;
                SET p_ErrorMsg = CONCAT('No changes made for user: ', p_UserId);
            END IF;
            
            COMMIT;
        END IF;
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- END OF THEME MANAGEMENT PROCEDURES
-- ================================================================================
