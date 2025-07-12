# Generated Theme SQL Files

This directory contains automatically generated SQL files for application themes. Each file creates a complete theme configuration following the specified requirements.

## Files Generated

- **Arctic_Theme_Fixed.sql** - Light theme with Arctic blue accent (#F8F9FA background)
- **Midnight_Theme_Fixed.sql** - Dark theme with blue accent (#1E1E1E background)
- **Forest_Theme_Fixed.sql** - Light green theme with forest green accent (#F0F8F0 background)
- **Ocean_Theme_Fixed.sql** - Light blue theme with ocean blue accent (#F0F8FF background)
- **Sunset_Theme_Fixed.sql** - Warm orange theme with sunset orange accent (#FFF8F0 background)
- **Lavender_Theme_Fixed.sql** - Light purple theme with lavender accent (#F8F0FF background)

## Theme Structure

Each theme follows these requirements:
- **All container and label backgrounds** set to the theme's FormBackColor value
- **All container and label forecolors** set to high-contrast value (#1A1A1A)
- **Complete property coverage** - All 203 properties from Model_UserUiColors are included
- **JSON format** - Settings stored as JSON object in SettingsJson column
- **Proper SQL structure** - Single INSERT INTO app_themes statement per theme

## Container and Label Controls Included

The following controls have their backgrounds set to FormBackColor and forecolors set to #1A1A1A:
- Panel, GroupBox, TabPage, TabControl
- FlowLayoutPanel, TableLayoutPanel
- SplitContainer, UserControl, CustomControl
- DomainUpDown, PropertyGrid
- ContextMenu, MenuStrip, StatusStrip, ToolStrip
- WebBrowser, PictureBox
- Label, LinkLabel

## Usage

Execute any of these SQL files against your database to insert the corresponding theme:

```sql
-- Example: Insert Arctic theme
\i Arctic_Theme_Fixed.sql
```

## Generated Properties

Each theme includes all properties from Model_UserUiColors:
- Form colors (FormBackColor, FormForeColor)
- Control colors (ControlBackColor, ControlForeColor, ControlFocusedBackColor)
- Input control colors (TextBox, ComboBox, ListBox, etc.)
- Button and interactive control colors
- Data control colors (DataGridView, TreeView, ListView)
- Menu and toolbar colors
- Tab control colors
- Link control colors
- Progress and track control colors
- Date/time control colors
- Scrollbar colors
- Tooltip colors
- Context menu colors
- Semantic colors (AccentColor, ErrorColor, WarningColor, etc.)
- Window chrome colors

## Generation Details

- **Generator**: ThemeGenerator.py
- **Date**: Auto-generated
- **Total Properties**: 203 color properties per theme
- **Naming Convention**: {ThemeName}_Theme_Fixed.sql
- **High-Contrast Forecolor**: #1A1A1A (as required)
- **Container Background Rule**: All containers use FormBackColor (as required)