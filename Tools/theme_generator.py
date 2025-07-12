#!/usr/bin/env python3
"""
Theme SQL Generator - Python implementation
Generates SQL files for each theme with all properties following the requirements
"""

import json
import os
from typing import Dict, Any

class ThemeGenerator:
    def __init__(self, output_dir: str = "Generated_Themes"):
        self.output_dir = output_dir
        self.high_contrast_forecolor = "#1A1A1A"
        
    def generate_all_themes(self):
        """Generate SQL files for all predefined themes"""
        if not os.path.exists(self.output_dir):
            os.makedirs(self.output_dir)
            
        themes = self.create_theme_definitions()
        
        for theme in themes:
            self.generate_theme_sql(theme)
            
        print(f"Generated {len(themes)} theme SQL files in {self.output_dir}")
        
    def generate_theme_sql(self, theme: Dict[str, Any]):
        """Generate SQL file for a single theme"""
        colors = self.create_theme_colors(theme)
        json_str = json.dumps(colors, indent=2)
        sql = self.create_sql_insert(theme["name"], json_str)
        
        filename = f"{theme['name']}_Theme_Fixed.sql"
        filepath = os.path.join(self.output_dir, filename)
        
        with open(filepath, 'w') as f:
            f.write(sql)
            
        print(f"Generated: {filename}")
        
    def create_theme_colors(self, theme: Dict[str, Any]) -> Dict[str, str]:
        """Create theme color configuration based on theme definition"""
        colors = {}
        
        form_back = theme["form_back_color"]
        high_contrast = self.high_contrast_forecolor
        
        # Set form colors
        colors["FormBackColor"] = form_back
        colors["FormForeColor"] = high_contrast
        
        # Set all container and label backgrounds to FormBackColor as per requirements
        # Set all container and label forecolors to high-contrast value
        colors["ControlBackColor"] = form_back
        colors["ControlForeColor"] = high_contrast
        colors["ControlFocusedBackColor"] = theme["accent_color"]
        
        # Labels
        colors["LabelBackColor"] = form_back
        colors["LabelForeColor"] = high_contrast
        
        # Containers - Panel, GroupBox, TabPage, etc.
        colors["PanelBackColor"] = form_back
        colors["PanelForeColor"] = high_contrast
        colors["PanelBorderColor"] = theme["border_color"]
        
        colors["GroupBoxBackColor"] = form_back
        colors["GroupBoxForeColor"] = high_contrast
        colors["GroupBoxBorderColor"] = theme["border_color"]
        
        colors["TabPageBackColor"] = form_back
        colors["TabPageForeColor"] = high_contrast
        colors["TabPageBorderColor"] = theme["border_color"]
        
        colors["TabControlBackColor"] = form_back
        colors["TabControlForeColor"] = high_contrast
        colors["TabControlBorderColor"] = theme["border_color"]
        
        colors["FlowLayoutPanelBackColor"] = form_back
        colors["FlowLayoutPanelForeColor"] = high_contrast
        colors["FlowLayoutPanelBorderColor"] = theme["border_color"]
        
        colors["TableLayoutPanelBackColor"] = form_back
        colors["TableLayoutPanelForeColor"] = high_contrast
        colors["TableLayoutPanelBorderColor"] = theme["border_color"]
        colors["TableLayoutPanelCellBorderColor"] = theme["border_color"]
        
        colors["SplitContainerBackColor"] = form_back
        colors["SplitContainerForeColor"] = high_contrast
        colors["SplitContainerSplitterColor"] = theme["border_color"]
        
        colors["UserControlBackColor"] = form_back
        colors["UserControlForeColor"] = high_contrast
        colors["UserControlBorderColor"] = theme["border_color"]
        
        colors["CustomControlBackColor"] = form_back
        colors["CustomControlForeColor"] = high_contrast
        colors["CustomControlBorderColor"] = theme["border_color"]
        
        colors["PictureBoxBackColor"] = "transparent"
        colors["PictureBoxBorderColor"] = theme["border_color"]
        
        colors["PropertyGridBackColor"] = form_back
        colors["PropertyGridForeColor"] = high_contrast
        colors["PropertyGridLineColor"] = theme["border_color"]
        colors["PropertyGridCategoryBackColor"] = form_back
        colors["PropertyGridCategoryForeColor"] = high_contrast
        colors["PropertyGridSelectedBackColor"] = theme["accent_color"]
        colors["PropertyGridSelectedForeColor"] = "#FFFFFF"
        
        colors["DomainUpDownBackColor"] = form_back
        colors["DomainUpDownForeColor"] = high_contrast
        colors["DomainUpDownErrorForeColor"] = theme["error_color"]
        colors["DomainUpDownBorderColor"] = theme["border_color"]
        colors["DomainUpDownButtonBackColor"] = theme["button_back_color"]
        colors["DomainUpDownButtonForeColor"] = high_contrast
        
        colors["WebBrowserBackColor"] = form_back
        colors["WebBrowserBorderColor"] = theme["border_color"]
        
        # Input controls
        colors["TextBoxBackColor"] = theme["input_back_color"]
        colors["TextBoxForeColor"] = high_contrast
        colors["TextBoxSelectionBackColor"] = theme["accent_color"]
        colors["TextBoxSelectionForeColor"] = "#FFFFFF"
        colors["TextBoxErrorForeColor"] = theme["error_color"]
        colors["TextBoxBorderColor"] = theme["border_color"]
        
        colors["MaskedTextBoxBackColor"] = theme["input_back_color"]
        colors["MaskedTextBoxForeColor"] = high_contrast
        colors["MaskedTextBoxErrorForeColor"] = theme["error_color"]
        colors["MaskedTextBoxBorderColor"] = theme["border_color"]
        
        colors["RichTextBoxBackColor"] = theme["input_back_color"]
        colors["RichTextBoxForeColor"] = high_contrast
        colors["RichTextBoxSelectionBackColor"] = theme["accent_color"]
        colors["RichTextBoxSelectionForeColor"] = "#FFFFFF"
        colors["RichTextBoxErrorForeColor"] = theme["error_color"]
        colors["RichTextBoxBorderColor"] = theme["border_color"]
        
        colors["ComboBoxBackColor"] = theme["input_back_color"]
        colors["ComboBoxForeColor"] = high_contrast
        colors["ComboBoxSelectionBackColor"] = theme["accent_color"]
        colors["ComboBoxSelectionForeColor"] = "#FFFFFF"
        colors["ComboBoxErrorForeColor"] = theme["error_color"]
        colors["ComboBoxBorderColor"] = theme["border_color"]
        colors["ComboBoxDropDownBackColor"] = theme["input_back_color"]
        colors["ComboBoxDropDownForeColor"] = high_contrast
        
        colors["ListBoxBackColor"] = theme["input_back_color"]
        colors["ListBoxForeColor"] = high_contrast
        colors["ListBoxSelectionBackColor"] = theme["accent_color"]
        colors["ListBoxSelectionForeColor"] = "#FFFFFF"
        colors["ListBoxBorderColor"] = theme["border_color"]
        
        colors["CheckedListBoxBackColor"] = theme["input_back_color"]
        colors["CheckedListBoxForeColor"] = high_contrast
        colors["CheckedListBoxBorderColor"] = theme["border_color"]
        colors["CheckedListBoxCheckBackColor"] = theme["input_back_color"]
        colors["CheckedListBoxCheckForeColor"] = theme["accent_color"]
        
        # Buttons and interactive controls
        colors["ButtonBackColor"] = theme["button_back_color"]
        colors["ButtonForeColor"] = high_contrast
        colors["ButtonBorderColor"] = theme["border_color"]
        colors["ButtonHoverBackColor"] = theme["accent_color"]
        colors["ButtonHoverForeColor"] = "#FFFFFF"
        colors["ButtonPressedBackColor"] = theme["accent_color_dark"]
        colors["ButtonPressedForeColor"] = "#FFFFFF"
        
        colors["RadioButtonBackColor"] = form_back
        colors["RadioButtonForeColor"] = high_contrast
        colors["RadioButtonCheckColor"] = theme["accent_color"]
        
        colors["CheckBoxBackColor"] = form_back
        colors["CheckBoxForeColor"] = high_contrast
        colors["CheckBoxCheckColor"] = theme["accent_color"]
        colors["CheckBoxCheckBackColor"] = form_back
        
        # Data controls
        colors["DataGridBackColor"] = theme["input_back_color"]
        colors["DataGridForeColor"] = high_contrast
        colors["DataGridSelectionBackColor"] = theme["accent_color"]
        colors["DataGridSelectionForeColor"] = "#FFFFFF"
        colors["DataGridRowBackColor"] = theme["input_back_color"]
        colors["DataGridAltRowBackColor"] = theme["alternate_row_color"]
        colors["DataGridHeaderBackColor"] = theme["header_back_color"]
        colors["DataGridHeaderForeColor"] = high_contrast
        colors["DataGridGridColor"] = theme["border_color"]
        colors["DataGridBorderColor"] = theme["border_color"]
        
        colors["TreeViewBackColor"] = theme["input_back_color"]
        colors["TreeViewForeColor"] = high_contrast
        colors["TreeViewLineColor"] = theme["border_color"]
        colors["TreeViewSelectedNodeBackColor"] = theme["accent_color"]
        colors["TreeViewSelectedNodeForeColor"] = "#FFFFFF"
        colors["TreeViewBorderColor"] = theme["border_color"]
        
        colors["ListViewBackColor"] = theme["input_back_color"]
        colors["ListViewForeColor"] = high_contrast
        colors["ListViewSelectionBackColor"] = theme["accent_color"]
        colors["ListViewSelectionForeColor"] = "#FFFFFF"
        colors["ListViewBorderColor"] = theme["border_color"]
        colors["ListViewHeaderBackColor"] = theme["header_back_color"]
        colors["ListViewHeaderForeColor"] = high_contrast
        
        # Menu and toolstrip controls
        colors["MenuStripBackColor"] = form_back
        colors["MenuStripForeColor"] = high_contrast
        colors["MenuStripBorderColor"] = theme["border_color"]
        colors["MenuStripItemHoverBackColor"] = theme["accent_color"]
        colors["MenuStripItemHoverForeColor"] = "#FFFFFF"
        colors["MenuStripItemSelectedBackColor"] = theme["accent_color"]
        colors["MenuStripItemSelectedForeColor"] = "#FFFFFF"
        
        colors["StatusStripBackColor"] = theme["header_back_color"]
        colors["StatusStripForeColor"] = high_contrast
        colors["StatusStripBorderColor"] = theme["border_color"]
        
        colors["ToolStripBackColor"] = theme["header_back_color"]
        colors["ToolStripForeColor"] = high_contrast
        colors["ToolStripBorderColor"] = theme["border_color"]
        colors["ToolStripItemHoverBackColor"] = theme["accent_color"]
        colors["ToolStripItemHoverForeColor"] = "#FFFFFF"
        
        # Tab controls
        colors["TabSelectedBackColor"] = theme["accent_color"]
        colors["TabSelectedForeColor"] = "#FFFFFF"
        colors["TabUnselectedBackColor"] = theme["button_back_color"]
        colors["TabUnselectedForeColor"] = high_contrast
        
        # Link controls
        colors["LinkLabelLinkColor"] = theme["accent_color"]
        colors["LinkLabelActiveLinkColor"] = theme["accent_color_light"]
        colors["LinkLabelVisitedLinkColor"] = theme["accent_color_dark"]
        colors["LinkLabelHoverColor"] = theme["accent_color_light"]
        colors["LinkLabelBackColor"] = form_back
        colors["LinkLabelForeColor"] = theme["accent_color"]
        
        # Progress and track controls
        colors["ProgressBarBackColor"] = form_back
        colors["ProgressBarForeColor"] = theme["accent_color"]
        colors["ProgressBarBorderColor"] = theme["border_color"]
        
        colors["TrackBarBackColor"] = form_back
        colors["TrackBarForeColor"] = theme["accent_color"]
        colors["TrackBarThumbColor"] = theme["accent_color"]
        colors["TrackBarTickColor"] = theme["border_color"]
        
        # Date/time controls
        colors["DateTimePickerBackColor"] = theme["input_back_color"]
        colors["DateTimePickerForeColor"] = high_contrast
        colors["DateTimePickerBorderColor"] = theme["border_color"]
        colors["DateTimePickerDropDownBackColor"] = theme["input_back_color"]
        colors["DateTimePickerDropDownForeColor"] = high_contrast
        
        colors["MonthCalendarBackColor"] = theme["input_back_color"]
        colors["MonthCalendarForeColor"] = high_contrast
        colors["MonthCalendarTitleBackColor"] = theme["accent_color"]
        colors["MonthCalendarTitleForeColor"] = "#FFFFFF"
        colors["MonthCalendarTrailingForeColor"] = theme["border_color"]
        colors["MonthCalendarTodayBackColor"] = theme["header_back_color"]
        colors["MonthCalendarTodayForeColor"] = high_contrast
        colors["MonthCalendarBorderColor"] = theme["border_color"]
        
        # Numeric controls
        colors["NumericUpDownBackColor"] = theme["input_back_color"]
        colors["NumericUpDownForeColor"] = high_contrast
        colors["NumericUpDownErrorForeColor"] = theme["error_color"]
        colors["NumericUpDownBorderColor"] = theme["border_color"]
        colors["NumericUpDownButtonBackColor"] = theme["button_back_color"]
        colors["NumericUpDownButtonForeColor"] = high_contrast
        
        # Scrollbar controls
        colors["HScrollBarBackColor"] = form_back
        colors["HScrollBarForeColor"] = high_contrast
        colors["HScrollBarThumbColor"] = theme["button_back_color"]
        colors["HScrollBarTrackColor"] = form_back
        
        colors["VScrollBarBackColor"] = form_back
        colors["VScrollBarForeColor"] = high_contrast
        colors["VScrollBarThumbColor"] = theme["button_back_color"]
        colors["VScrollBarTrackColor"] = form_back
        
        # Tooltip controls
        colors["ToolTipBackColor"] = theme["header_back_color"]
        colors["ToolTipForeColor"] = high_contrast
        colors["ToolTipBorderColor"] = theme["border_color"]
        
        # Context menu controls
        colors["ContextMenuBackColor"] = form_back
        colors["ContextMenuForeColor"] = high_contrast
        colors["ContextMenuBorderColor"] = theme["border_color"]
        colors["ContextMenuItemHoverBackColor"] = theme["accent_color"]
        colors["ContextMenuItemHoverForeColor"] = "#FFFFFF"
        colors["ContextMenuSeparatorColor"] = theme["border_color"]
        
        # Semantic colors
        colors["AccentColor"] = theme["accent_color"]
        colors["SecondaryAccentColor"] = theme["accent_color_light"]
        colors["ErrorColor"] = theme["error_color"]
        colors["WarningColor"] = theme["warning_color"]
        colors["SuccessColor"] = theme["success_color"]
        colors["InfoColor"] = theme["info_color"]
        
        # Window chrome
        colors["WindowTitleBarBackColor"] = theme["header_back_color"]
        colors["WindowTitleBarForeColor"] = high_contrast
        colors["WindowTitleBarInactiveBackColor"] = theme["header_back_color"]
        colors["WindowTitleBarInactiveForeColor"] = theme["border_color"]
        colors["WindowBorderColor"] = theme["border_color"]
        colors["WindowResizeHandleColor"] = theme["border_color"]
        
        return colors
        
    def create_sql_insert(self, theme_name: str, json_str: str) -> str:
        """Create SQL INSERT statement"""
        escaped_json = json_str.replace("'", "''")
        
        sql = f"""-- Theme SQL Insert for {theme_name}
-- Generated automatically by ThemeGenerator
-- All container and label backgrounds set to FormBackColor
-- All container and label forecolors set to high-contrast value #1A1A1A

INSERT INTO app_themes(ThemeName, SettingsJson) VALUES
('{theme_name}', '{escaped_json}');
"""
        return sql
        
    def create_theme_definitions(self) -> list:
        """Create predefined theme definitions"""
        return [
            {
                "name": "Arctic",
                "form_back_color": "#F8F9FA",
                "input_back_color": "#FFFFFF",
                "button_back_color": "#F0F0F0",
                "header_back_color": "#E9ECEF",
                "alternate_row_color": "#F8F9FA",
                "border_color": "#CED4DA",
                "accent_color": "#007BFF",
                "accent_color_light": "#66B5FF",
                "accent_color_dark": "#0056B3",
                "error_color": "#DC3545",
                "warning_color": "#FFC107",
                "success_color": "#28A745",
                "info_color": "#17A2B8"
            },
            {
                "name": "Midnight",
                "form_back_color": "#1E1E1E",
                "input_back_color": "#2D2D2D",
                "button_back_color": "#3C3C3C",
                "header_back_color": "#252526",
                "alternate_row_color": "#2A2D2E",
                "border_color": "#3C3C3C",
                "accent_color": "#007ACC",
                "accent_color_light": "#66CCFF",
                "accent_color_dark": "#005A9E",
                "error_color": "#E57373",
                "warning_color": "#FFA726",
                "success_color": "#66BB6A",
                "info_color": "#3399FF"
            },
            {
                "name": "Forest",
                "form_back_color": "#F0F8F0",
                "input_back_color": "#FFFFFF",
                "button_back_color": "#E6F5E6",
                "header_back_color": "#DCF0DC",
                "alternate_row_color": "#F5FAF5",
                "border_color": "#B4C8B4",
                "accent_color": "#4CAF50",
                "accent_color_light": "#81C784",
                "accent_color_dark": "#388E3C",
                "error_color": "#F44336",
                "warning_color": "#FF9800",
                "success_color": "#4CAF50",
                "info_color": "#2196F3"
            },
            {
                "name": "Ocean",
                "form_back_color": "#F0F8FF",
                "input_back_color": "#FFFFFF",
                "button_back_color": "#E6F5FF",
                "header_back_color": "#DCF0FF",
                "alternate_row_color": "#F5FAFF",
                "border_color": "#B4C8DC",
                "accent_color": "#2196F3",
                "accent_color_light": "#64B5F6",
                "accent_color_dark": "#1976D2",
                "error_color": "#F44336",
                "warning_color": "#FF9800",
                "success_color": "#4CAF50",
                "info_color": "#2196F3"
            },
            {
                "name": "Sunset",
                "form_back_color": "#FFF8F0",
                "input_back_color": "#FFFFFF",
                "button_back_color": "#FFF5E6",
                "header_back_color": "#FFF0DC",
                "alternate_row_color": "#FFFAF5",
                "border_color": "#DCBEB4",
                "accent_color": "#FF5722",
                "accent_color_light": "#FF8A65",
                "accent_color_dark": "#D84315",
                "error_color": "#F44336",
                "warning_color": "#FF9800",
                "success_color": "#4CAF50",
                "info_color": "#2196F3"
            },
            {
                "name": "Lavender",
                "form_back_color": "#F8F0FF",
                "input_back_color": "#FFFFFF",
                "button_back_color": "#F5E6FF",
                "header_back_color": "#F0DCFF",
                "alternate_row_color": "#FAF5FF",
                "border_color": "#C8B4DC",
                "accent_color": "#9C27B0",
                "accent_color_light": "#BA68C8",
                "accent_color_dark": "#7B1FA2",
                "error_color": "#F44336",
                "warning_color": "#FF9800",
                "success_color": "#4CAF50",
                "info_color": "#2196F3"
            }
        ]

def main():
    """Main execution function"""
    print("Starting Theme SQL Generation...")
    print("=" * 50)
    
    generator = ThemeGenerator()
    generator.generate_all_themes()
    
    print("=" * 50)
    print("Theme SQL generation completed successfully!")
    print("Files generated in 'Generated_Themes' directory.")

if __name__ == "__main__":
    main()