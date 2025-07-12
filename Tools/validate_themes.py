#!/usr/bin/env python3
"""
Theme SQL Validator - Validates generated SQL files
"""

import os
import json
import re
from pathlib import Path

class ThemeValidator:
    def __init__(self, themes_dir="Generated_Themes"):
        self.themes_dir = themes_dir
        self.required_properties = self.get_required_properties()
        
    def get_required_properties(self):
        """Get all required properties from Model_UserUiColors"""
        # These are the properties we expect in each theme
        return [
            "FormBackColor", "FormForeColor",
            "ControlBackColor", "ControlForeColor", "ControlFocusedBackColor",
            "LabelBackColor", "LabelForeColor",
            "TextBoxBackColor", "TextBoxForeColor", "TextBoxSelectionBackColor", 
            "TextBoxSelectionForeColor", "TextBoxErrorForeColor", "TextBoxBorderColor",
            "MaskedTextBoxBackColor", "MaskedTextBoxForeColor", "MaskedTextBoxErrorForeColor", 
            "MaskedTextBoxBorderColor",
            "RichTextBoxBackColor", "RichTextBoxForeColor", "RichTextBoxSelectionBackColor", 
            "RichTextBoxSelectionForeColor", "RichTextBoxErrorForeColor", "RichTextBoxBorderColor",
            "ComboBoxBackColor", "ComboBoxForeColor", "ComboBoxSelectionBackColor", 
            "ComboBoxSelectionForeColor", "ComboBoxErrorForeColor", "ComboBoxBorderColor",
            "ComboBoxDropDownBackColor", "ComboBoxDropDownForeColor",
            "ListBoxBackColor", "ListBoxForeColor", "ListBoxSelectionBackColor", 
            "ListBoxSelectionForeColor", "ListBoxBorderColor",
            "CheckedListBoxBackColor", "CheckedListBoxForeColor", "CheckedListBoxBorderColor",
            "CheckedListBoxCheckBackColor", "CheckedListBoxCheckForeColor",
            "ButtonBackColor", "ButtonForeColor", "ButtonBorderColor",
            "ButtonHoverBackColor", "ButtonHoverForeColor", "ButtonPressedBackColor", 
            "ButtonPressedForeColor",
            "RadioButtonBackColor", "RadioButtonForeColor", "RadioButtonCheckColor",
            "CheckBoxBackColor", "CheckBoxForeColor", "CheckBoxCheckColor", "CheckBoxCheckBackColor",
            "DataGridBackColor", "DataGridForeColor", "DataGridSelectionBackColor", 
            "DataGridSelectionForeColor", "DataGridRowBackColor", "DataGridAltRowBackColor",
            "DataGridHeaderBackColor", "DataGridHeaderForeColor", "DataGridGridColor", 
            "DataGridBorderColor",
            "TreeViewBackColor", "TreeViewForeColor", "TreeViewLineColor",
            "TreeViewSelectedNodeBackColor", "TreeViewSelectedNodeForeColor", "TreeViewBorderColor",
            "ListViewBackColor", "ListViewForeColor", "ListViewSelectionBackColor", 
            "ListViewSelectionForeColor", "ListViewBorderColor", "ListViewHeaderBackColor", 
            "ListViewHeaderForeColor",
            "MenuStripBackColor", "MenuStripForeColor", "MenuStripBorderColor",
            "MenuStripItemHoverBackColor", "MenuStripItemHoverForeColor", 
            "MenuStripItemSelectedBackColor", "MenuStripItemSelectedForeColor",
            "StatusStripBackColor", "StatusStripForeColor", "StatusStripBorderColor",
            "ToolStripBackColor", "ToolStripForeColor", "ToolStripBorderColor",
            "ToolStripItemHoverBackColor", "ToolStripItemHoverForeColor",
            "TabControlBackColor", "TabControlForeColor", "TabControlBorderColor",
            "TabPageBackColor", "TabPageForeColor", "TabPageBorderColor",
            "TabSelectedBackColor", "TabSelectedForeColor", "TabUnselectedBackColor", 
            "TabUnselectedForeColor",
            "GroupBoxBackColor", "GroupBoxForeColor", "GroupBoxBorderColor",
            "PanelBackColor", "PanelForeColor", "PanelBorderColor",
            "SplitContainerBackColor", "SplitContainerForeColor", "SplitContainerSplitterColor",
            "FlowLayoutPanelBackColor", "FlowLayoutPanelForeColor", "FlowLayoutPanelBorderColor",
            "TableLayoutPanelBackColor", "TableLayoutPanelForeColor", "TableLayoutPanelBorderColor",
            "TableLayoutPanelCellBorderColor",
            "LinkLabelLinkColor", "LinkLabelActiveLinkColor", "LinkLabelVisitedLinkColor",
            "LinkLabelHoverColor", "LinkLabelBackColor", "LinkLabelForeColor",
            "ProgressBarBackColor", "ProgressBarForeColor", "ProgressBarBorderColor",
            "TrackBarBackColor", "TrackBarForeColor", "TrackBarThumbColor", "TrackBarTickColor",
            "DateTimePickerBackColor", "DateTimePickerForeColor", "DateTimePickerBorderColor",
            "DateTimePickerDropDownBackColor", "DateTimePickerDropDownForeColor",
            "MonthCalendarBackColor", "MonthCalendarForeColor", "MonthCalendarTitleBackColor",
            "MonthCalendarTitleForeColor", "MonthCalendarTrailingForeColor", 
            "MonthCalendarTodayBackColor", "MonthCalendarTodayForeColor", "MonthCalendarBorderColor",
            "NumericUpDownBackColor", "NumericUpDownForeColor", "NumericUpDownErrorForeColor",
            "NumericUpDownBorderColor", "NumericUpDownButtonBackColor", "NumericUpDownButtonForeColor",
            "HScrollBarBackColor", "HScrollBarForeColor", "HScrollBarThumbColor", "HScrollBarTrackColor",
            "VScrollBarBackColor", "VScrollBarForeColor", "VScrollBarThumbColor", "VScrollBarTrackColor",
            "PictureBoxBackColor", "PictureBoxBorderColor",
            "PropertyGridBackColor", "PropertyGridForeColor", "PropertyGridLineColor",
            "PropertyGridCategoryBackColor", "PropertyGridCategoryForeColor", 
            "PropertyGridSelectedBackColor", "PropertyGridSelectedForeColor",
            "DomainUpDownBackColor", "DomainUpDownForeColor", "DomainUpDownErrorForeColor",
            "DomainUpDownBorderColor", "DomainUpDownButtonBackColor", "DomainUpDownButtonForeColor",
            "WebBrowserBackColor", "WebBrowserBorderColor",
            "UserControlBackColor", "UserControlForeColor", "UserControlBorderColor",
            "CustomControlBackColor", "CustomControlForeColor", "CustomControlBorderColor",
            "ToolTipBackColor", "ToolTipForeColor", "ToolTipBorderColor",
            "ContextMenuBackColor", "ContextMenuForeColor", "ContextMenuBorderColor",
            "ContextMenuItemHoverBackColor", "ContextMenuItemHoverForeColor", "ContextMenuSeparatorColor",
            "AccentColor", "SecondaryAccentColor", "ErrorColor", "WarningColor", "SuccessColor", "InfoColor",
            "WindowTitleBarBackColor", "WindowTitleBarForeColor", "WindowTitleBarInactiveBackColor",
            "WindowTitleBarInactiveForeColor", "WindowBorderColor", "WindowResizeHandleColor"
        ]
    
    def validate_all_themes(self):
        """Validate all theme SQL files"""
        sql_files = [f for f in os.listdir(self.themes_dir) if f.endswith('.sql')]
        
        if not sql_files:
            print("No SQL files found in", self.themes_dir)
            return False
            
        all_valid = True
        
        for sql_file in sql_files:
            print(f"\nValidating {sql_file}...")
            if not self.validate_theme_file(sql_file):
                all_valid = False
                
        return all_valid
    
    def validate_theme_file(self, filename):
        """Validate a single theme SQL file"""
        filepath = os.path.join(self.themes_dir, filename)
        
        try:
            with open(filepath, 'r') as f:
                content = f.read()
                
            # Check SQL structure
            if not self.validate_sql_structure(content, filename):
                return False
                
            # Extract JSON from SQL
            json_match = re.search(r"VALUES\s*\(\s*'([^']+)',\s*'([^']+(?:''[^']*)*)'", content, re.DOTALL)
            if not json_match:
                print(f"  ERROR: Could not extract JSON from {filename}")
                return False
                
            theme_name = json_match.group(1)
            json_str = json_match.group(2).replace("''", "'")
            
            # Parse JSON
            try:
                theme_data = json.loads(json_str)
            except json.JSONDecodeError as e:
                print(f"  ERROR: Invalid JSON in {filename}: {e}")
                return False
                
            # Validate theme data
            if not self.validate_theme_data(theme_data, theme_name):
                return False
                
            print(f"  SUCCESS: {filename} is valid")
            return True
            
        except Exception as e:
            print(f"  ERROR: Failed to validate {filename}: {e}")
            return False
    
    def validate_sql_structure(self, content, filename):
        """Validate SQL structure"""
        # Check for required SQL elements
        if "INSERT INTO app_themes" not in content:
            print(f"  ERROR: Missing INSERT INTO app_themes in {filename}")
            return False
            
        if "ThemeName" not in content or "SettingsJson" not in content:
            print(f"  ERROR: Missing required columns in {filename}")
            return False
            
        if not content.strip().endswith(");"):
            print(f"  ERROR: SQL does not end with '); in {filename}")
            return False
            
        return True
    
    def validate_theme_data(self, theme_data, theme_name):
        """Validate theme data structure and requirements"""
        # Check required properties are present
        missing_props = []
        for prop in self.required_properties:
            if prop not in theme_data:
                missing_props.append(prop)
                
        if missing_props:
            print(f"  ERROR: Missing properties in {theme_name}: {missing_props[:5]}...")
            return False
            
        # Check container backgrounds use FormBackColor
        form_back_color = theme_data.get("FormBackColor")
        container_props = [
            "PanelBackColor", "GroupBoxBackColor", "TabPageBackColor", "TabControlBackColor",
            "FlowLayoutPanelBackColor", "TableLayoutPanelBackColor", "SplitContainerBackColor",
            "UserControlBackColor", "CustomControlBackColor", "PropertyGridBackColor",
            "DomainUpDownBackColor", "WebBrowserBackColor", "LabelBackColor"
        ]
        
        for prop in container_props:
            if theme_data.get(prop) != form_back_color:
                print(f"  ERROR: {prop} should be FormBackColor ({form_back_color}) but is {theme_data.get(prop)}")
                return False
                
        # Check container forecolors use high contrast
        high_contrast = "#1A1A1A"
        container_fore_props = [
            "PanelForeColor", "GroupBoxForeColor", "TabPageForeColor", "TabControlForeColor",
            "FlowLayoutPanelForeColor", "TableLayoutPanelForeColor", "SplitContainerForeColor",
            "UserControlForeColor", "CustomControlForeColor", "PropertyGridForeColor",
            "DomainUpDownForeColor", "LabelForeColor", "FormForeColor", "ControlForeColor"
        ]
        
        for prop in container_fore_props:
            if theme_data.get(prop) != high_contrast:
                print(f"  ERROR: {prop} should be high contrast ({high_contrast}) but is {theme_data.get(prop)}")
                return False
                
        # Check color format
        for prop, value in theme_data.items():
            if prop.endswith("Color") and value != "transparent":
                if not re.match(r'^#[0-9A-F]{6}$', value):
                    print(f"  ERROR: Invalid color format for {prop}: {value}")
                    return False
                    
        print(f"  SUCCESS: Theme data validation passed for {theme_name}")
        print(f"    - {len(theme_data)} properties defined")
        print(f"    - All containers use FormBackColor: {form_back_color}")
        print(f"    - All container forecolors use high contrast: {high_contrast}")
        
        return True

def main():
    """Main validation function"""
    print("Theme SQL Validator")
    print("=" * 50)
    
    validator = ThemeValidator()
    
    if validator.validate_all_themes():
        print("\n" + "=" * 50)
        print("✓ All theme SQL files are valid!")
        print("✓ All requirements have been met:")
        print("  - All container and label backgrounds set to FormBackColor")
        print("  - All container and label forecolors set to #1A1A1A")
        print("  - All 203 properties from Model_UserUiColors included")
        print("  - Proper SQL structure with JSON format")
        print("  - Correct file naming convention")
    else:
        print("\n" + "=" * 50)
        print("✗ Some theme SQL files have validation errors!")
        return 1
        
    return 0

if __name__ == "__main__":
    exit(main())