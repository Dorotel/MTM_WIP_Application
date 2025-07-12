using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Tools
{
    /// <summary>
    /// Generates SQL files for each theme with all properties from Model_UserUiColors
    /// </summary>
    public class ThemeGenerator
    {
        private readonly List<ThemeDefinition> _themes;
        private readonly string _outputDirectory;
        private const string HighContrastForeColor = "#1A1A1A";

        public ThemeGenerator(string outputDirectory = "Generated_Themes")
        {
            _outputDirectory = outputDirectory;
            _themes = CreateThemeDefinitions();
        }

        /// <summary>
        /// Generates SQL files for all themes
        /// </summary>
        public void GenerateAllThemes()
        {
            if (!Directory.Exists(_outputDirectory))
                Directory.CreateDirectory(_outputDirectory);

            foreach (var theme in _themes)
            {
                GenerateThemeSQL(theme);
            }

            Console.WriteLine($"Generated {_themes.Count} theme SQL files in {_outputDirectory}");
        }

        /// <summary>
        /// Generates SQL file for a single theme
        /// </summary>
        private void GenerateThemeSQL(ThemeDefinition theme)
        {
            var colors = CreateThemeColors(theme);
            var json = SerializeToJson(colors);
            var sql = CreateSQLInsert(theme.Name, json);
            
            var fileName = $"{theme.Name}_Theme_Fixed.sql";
            var filePath = Path.Combine(_outputDirectory, fileName);
            
            File.WriteAllText(filePath, sql);
            Console.WriteLine($"Generated: {fileName}");
        }

        /// <summary>
        /// Creates theme color configuration based on theme definition
        /// </summary>
        private Model_UserUiColors CreateThemeColors(ThemeDefinition theme)
        {
            var colors = new Model_UserUiColors();
            
            // Set form colors
            colors.FormBackColor = theme.FormBackColor;
            colors.FormForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            
            // Set all container and label backgrounds to FormBackColor as per requirements
            // Set all container and label forecolors to high-contrast value
            colors.ControlBackColor = theme.FormBackColor;
            colors.ControlForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.ControlFocusedBackColor = theme.AccentColor;
            
            // Labels
            colors.LabelBackColor = theme.FormBackColor;
            colors.LabelForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            
            // Containers - Panel, GroupBox, TabPage, etc.
            colors.PanelBackColor = theme.FormBackColor;
            colors.PanelForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.PanelBorderColor = theme.BorderColor;
            
            colors.GroupBoxBackColor = theme.FormBackColor;
            colors.GroupBoxForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.GroupBoxBorderColor = theme.BorderColor;
            
            colors.TabPageBackColor = theme.FormBackColor;
            colors.TabPageForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.TabPageBorderColor = theme.BorderColor;
            
            colors.TabControlBackColor = theme.FormBackColor;
            colors.TabControlForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.TabControlBorderColor = theme.BorderColor;
            
            colors.FlowLayoutPanelBackColor = theme.FormBackColor;
            colors.FlowLayoutPanelForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.FlowLayoutPanelBorderColor = theme.BorderColor;
            
            colors.TableLayoutPanelBackColor = theme.FormBackColor;
            colors.TableLayoutPanelForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.TableLayoutPanelBorderColor = theme.BorderColor;
            colors.TableLayoutPanelCellBorderColor = theme.BorderColor;
            
            colors.SplitContainerBackColor = theme.FormBackColor;
            colors.SplitContainerForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.SplitContainerSplitterColor = theme.BorderColor;
            
            colors.UserControlBackColor = theme.FormBackColor;
            colors.UserControlForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.UserControlBorderColor = theme.BorderColor;
            
            colors.CustomControlBackColor = theme.FormBackColor;
            colors.CustomControlForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.CustomControlBorderColor = theme.BorderColor;
            
            colors.PictureBoxBackColor = theme.FormBackColor;
            colors.PictureBoxBorderColor = theme.BorderColor;
            
            colors.PropertyGridBackColor = theme.FormBackColor;
            colors.PropertyGridForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.PropertyGridLineColor = theme.BorderColor;
            colors.PropertyGridCategoryBackColor = theme.FormBackColor;
            colors.PropertyGridCategoryForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.PropertyGridSelectedBackColor = theme.AccentColor;
            colors.PropertyGridSelectedForeColor = Color.White;
            
            colors.DomainUpDownBackColor = theme.FormBackColor;
            colors.DomainUpDownForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.DomainUpDownErrorForeColor = theme.ErrorColor;
            colors.DomainUpDownBorderColor = theme.BorderColor;
            colors.DomainUpDownButtonBackColor = theme.ButtonBackColor;
            colors.DomainUpDownButtonForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            
            colors.WebBrowserBackColor = theme.FormBackColor;
            colors.WebBrowserBorderColor = theme.BorderColor;
            
            // Input controls
            colors.TextBoxBackColor = theme.InputBackColor;
            colors.TextBoxForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.TextBoxSelectionBackColor = theme.AccentColor;
            colors.TextBoxSelectionForeColor = Color.White;
            colors.TextBoxErrorForeColor = theme.ErrorColor;
            colors.TextBoxBorderColor = theme.BorderColor;
            
            colors.MaskedTextBoxBackColor = theme.InputBackColor;
            colors.MaskedTextBoxForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.MaskedTextBoxErrorForeColor = theme.ErrorColor;
            colors.MaskedTextBoxBorderColor = theme.BorderColor;
            
            colors.RichTextBoxBackColor = theme.InputBackColor;
            colors.RichTextBoxForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.RichTextBoxSelectionBackColor = theme.AccentColor;
            colors.RichTextBoxSelectionForeColor = Color.White;
            colors.RichTextBoxErrorForeColor = theme.ErrorColor;
            colors.RichTextBoxBorderColor = theme.BorderColor;
            
            colors.ComboBoxBackColor = theme.InputBackColor;
            colors.ComboBoxForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.ComboBoxSelectionBackColor = theme.AccentColor;
            colors.ComboBoxSelectionForeColor = Color.White;
            colors.ComboBoxErrorForeColor = theme.ErrorColor;
            colors.ComboBoxBorderColor = theme.BorderColor;
            colors.ComboBoxDropDownBackColor = theme.InputBackColor;
            colors.ComboBoxDropDownForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            
            colors.ListBoxBackColor = theme.InputBackColor;
            colors.ListBoxForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.ListBoxSelectionBackColor = theme.AccentColor;
            colors.ListBoxSelectionForeColor = Color.White;
            colors.ListBoxBorderColor = theme.BorderColor;
            
            colors.CheckedListBoxBackColor = theme.InputBackColor;
            colors.CheckedListBoxForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.CheckedListBoxBorderColor = theme.BorderColor;
            colors.CheckedListBoxCheckBackColor = theme.InputBackColor;
            colors.CheckedListBoxCheckForeColor = theme.AccentColor;
            
            // Buttons and interactive controls
            colors.ButtonBackColor = theme.ButtonBackColor;
            colors.ButtonForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.ButtonBorderColor = theme.BorderColor;
            colors.ButtonHoverBackColor = theme.AccentColor;
            colors.ButtonHoverForeColor = Color.White;
            colors.ButtonPressedBackColor = theme.AccentColorDark;
            colors.ButtonPressedForeColor = Color.White;
            
            colors.RadioButtonBackColor = theme.FormBackColor;
            colors.RadioButtonForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.RadioButtonCheckColor = theme.AccentColor;
            
            colors.CheckBoxBackColor = theme.FormBackColor;
            colors.CheckBoxForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.CheckBoxCheckColor = theme.AccentColor;
            colors.CheckBoxCheckBackColor = theme.FormBackColor;
            
            // Data controls
            colors.DataGridBackColor = theme.InputBackColor;
            colors.DataGridForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.DataGridSelectionBackColor = theme.AccentColor;
            colors.DataGridSelectionForeColor = Color.White;
            colors.DataGridRowBackColor = theme.InputBackColor;
            colors.DataGridAltRowBackColor = theme.AlternateRowColor;
            colors.DataGridHeaderBackColor = theme.HeaderBackColor;
            colors.DataGridHeaderForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.DataGridGridColor = theme.BorderColor;
            colors.DataGridBorderColor = theme.BorderColor;
            
            colors.TreeViewBackColor = theme.InputBackColor;
            colors.TreeViewForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.TreeViewLineColor = theme.BorderColor;
            colors.TreeViewSelectedNodeBackColor = theme.AccentColor;
            colors.TreeViewSelectedNodeForeColor = Color.White;
            colors.TreeViewBorderColor = theme.BorderColor;
            
            colors.ListViewBackColor = theme.InputBackColor;
            colors.ListViewForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.ListViewSelectionBackColor = theme.AccentColor;
            colors.ListViewSelectionForeColor = Color.White;
            colors.ListViewBorderColor = theme.BorderColor;
            colors.ListViewHeaderBackColor = theme.HeaderBackColor;
            colors.ListViewHeaderForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            
            // Menu and toolstrip controls
            colors.MenuStripBackColor = theme.FormBackColor;
            colors.MenuStripForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.MenuStripBorderColor = theme.BorderColor;
            colors.MenuStripItemHoverBackColor = theme.AccentColor;
            colors.MenuStripItemHoverForeColor = Color.White;
            colors.MenuStripItemSelectedBackColor = theme.AccentColor;
            colors.MenuStripItemSelectedForeColor = Color.White;
            
            colors.StatusStripBackColor = theme.HeaderBackColor;
            colors.StatusStripForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.StatusStripBorderColor = theme.BorderColor;
            
            colors.ToolStripBackColor = theme.HeaderBackColor;
            colors.ToolStripForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.ToolStripBorderColor = theme.BorderColor;
            colors.ToolStripItemHoverBackColor = theme.AccentColor;
            colors.ToolStripItemHoverForeColor = Color.White;
            
            // Tab controls
            colors.TabSelectedBackColor = theme.AccentColor;
            colors.TabSelectedForeColor = Color.White;
            colors.TabUnselectedBackColor = theme.ButtonBackColor;
            colors.TabUnselectedForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            
            // Link controls
            colors.LinkLabelLinkColor = theme.AccentColor;
            colors.LinkLabelActiveLinkColor = theme.AccentColorLight;
            colors.LinkLabelVisitedLinkColor = theme.AccentColorDark;
            colors.LinkLabelHoverColor = theme.AccentColorLight;
            colors.LinkLabelBackColor = theme.FormBackColor;
            colors.LinkLabelForeColor = theme.AccentColor;
            
            // Progress and track controls
            colors.ProgressBarBackColor = theme.FormBackColor;
            colors.ProgressBarForeColor = theme.AccentColor;
            colors.ProgressBarBorderColor = theme.BorderColor;
            
            colors.TrackBarBackColor = theme.FormBackColor;
            colors.TrackBarForeColor = theme.AccentColor;
            colors.TrackBarThumbColor = theme.AccentColor;
            colors.TrackBarTickColor = theme.BorderColor;
            
            // Date/time controls
            colors.DateTimePickerBackColor = theme.InputBackColor;
            colors.DateTimePickerForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.DateTimePickerBorderColor = theme.BorderColor;
            colors.DateTimePickerDropDownBackColor = theme.InputBackColor;
            colors.DateTimePickerDropDownForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            
            colors.MonthCalendarBackColor = theme.InputBackColor;
            colors.MonthCalendarForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.MonthCalendarTitleBackColor = theme.AccentColor;
            colors.MonthCalendarTitleForeColor = Color.White;
            colors.MonthCalendarTrailingForeColor = theme.BorderColor;
            colors.MonthCalendarTodayBackColor = theme.HeaderBackColor;
            colors.MonthCalendarTodayForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.MonthCalendarBorderColor = theme.BorderColor;
            
            // Numeric controls
            colors.NumericUpDownBackColor = theme.InputBackColor;
            colors.NumericUpDownForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.NumericUpDownErrorForeColor = theme.ErrorColor;
            colors.NumericUpDownBorderColor = theme.BorderColor;
            colors.NumericUpDownButtonBackColor = theme.ButtonBackColor;
            colors.NumericUpDownButtonForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            
            // Scrollbar controls
            colors.HScrollBarBackColor = theme.FormBackColor;
            colors.HScrollBarForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.HScrollBarThumbColor = theme.ButtonBackColor;
            colors.HScrollBarTrackColor = theme.FormBackColor;
            
            colors.VScrollBarBackColor = theme.FormBackColor;
            colors.VScrollBarForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.VScrollBarThumbColor = theme.ButtonBackColor;
            colors.VScrollBarTrackColor = theme.FormBackColor;
            
            // Tooltip controls
            colors.ToolTipBackColor = theme.HeaderBackColor;
            colors.ToolTipForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.ToolTipBorderColor = theme.BorderColor;
            
            // Context menu controls
            colors.ContextMenuBackColor = theme.FormBackColor;
            colors.ContextMenuForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.ContextMenuBorderColor = theme.BorderColor;
            colors.ContextMenuItemHoverBackColor = theme.AccentColor;
            colors.ContextMenuItemHoverForeColor = Color.White;
            colors.ContextMenuSeparatorColor = theme.BorderColor;
            
            // Semantic colors
            colors.AccentColor = theme.AccentColor;
            colors.SecondaryAccentColor = theme.AccentColorLight;
            colors.ErrorColor = theme.ErrorColor;
            colors.WarningColor = theme.WarningColor;
            colors.SuccessColor = theme.SuccessColor;
            colors.InfoColor = theme.InfoColor;
            
            // Window chrome
            colors.WindowTitleBarBackColor = theme.HeaderBackColor;
            colors.WindowTitleBarForeColor = ColorTranslator.FromHtml(HighContrastForeColor);
            colors.WindowTitleBarInactiveBackColor = theme.HeaderBackColor;
            colors.WindowTitleBarInactiveForeColor = theme.BorderColor;
            colors.WindowBorderColor = theme.BorderColor;
            colors.WindowResizeHandleColor = theme.BorderColor;
            
            return colors;
        }

        /// <summary>
        /// Serializes color model to JSON string
        /// </summary>
        private string SerializeToJson(Model_UserUiColors colors)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new ColorJsonConverter() }
            };
            
            return JsonSerializer.Serialize(colors, options);
        }

        /// <summary>
        /// Creates SQL INSERT statement
        /// </summary>
        private string CreateSQLInsert(string themeName, string json)
        {
            var sql = new StringBuilder();
            sql.AppendLine("-- Theme SQL Insert for " + themeName);
            sql.AppendLine("-- Generated automatically by ThemeGenerator");
            sql.AppendLine("-- All container and label backgrounds set to FormBackColor");
            sql.AppendLine("-- All container and label forecolors set to high-contrast value #1A1A1A");
            sql.AppendLine();
            sql.AppendLine("INSERT INTO app_themes(ThemeName, SettingsJson) VALUES");
            sql.AppendLine($"('{themeName}', '{json.Replace("'", "''")}');");
            
            return sql.ToString();
        }

        /// <summary>
        /// Creates predefined theme definitions
        /// </summary>
        private List<ThemeDefinition> CreateThemeDefinitions()
        {
            return new List<ThemeDefinition>
            {
                new ThemeDefinition
                {
                    Name = "Arctic",
                    FormBackColor = Color.FromArgb(248, 249, 250), // #F8F9FA
                    InputBackColor = Color.FromArgb(255, 255, 255), // #FFFFFF
                    ButtonBackColor = Color.FromArgb(240, 240, 240), // #F0F0F0
                    HeaderBackColor = Color.FromArgb(233, 236, 239), // #E9ECEF
                    AlternateRowColor = Color.FromArgb(248, 249, 250), // #F8F9FA
                    BorderColor = Color.FromArgb(206, 212, 218), // #CED4DA
                    AccentColor = Color.FromArgb(0, 123, 255), // #007BFF
                    AccentColorLight = Color.FromArgb(102, 181, 255), // #66B5FF
                    AccentColorDark = Color.FromArgb(0, 86, 179), // #0056B3
                    ErrorColor = Color.FromArgb(220, 53, 69), // #DC3545
                    WarningColor = Color.FromArgb(255, 193, 7), // #FFC107
                    SuccessColor = Color.FromArgb(40, 167, 69), // #28A745
                    InfoColor = Color.FromArgb(23, 162, 184) // #17A2B8
                },
                new ThemeDefinition
                {
                    Name = "Midnight",
                    FormBackColor = Color.FromArgb(30, 30, 30), // #1E1E1E
                    InputBackColor = Color.FromArgb(45, 45, 45), // #2D2D2D
                    ButtonBackColor = Color.FromArgb(60, 60, 60), // #3C3C3C
                    HeaderBackColor = Color.FromArgb(37, 37, 38), // #252526
                    AlternateRowColor = Color.FromArgb(42, 45, 46), // #2A2D2E
                    BorderColor = Color.FromArgb(60, 60, 60), // #3C3C3C
                    AccentColor = Color.FromArgb(0, 122, 204), // #007ACC
                    AccentColorLight = Color.FromArgb(102, 204, 255), // #66CCFF
                    AccentColorDark = Color.FromArgb(0, 90, 158), // #005A9E
                    ErrorColor = Color.FromArgb(229, 115, 115), // #E57373
                    WarningColor = Color.FromArgb(255, 167, 38), // #FFA726
                    SuccessColor = Color.FromArgb(102, 187, 106), // #66BB6A
                    InfoColor = Color.FromArgb(51, 153, 255) // #3399FF
                },
                new ThemeDefinition
                {
                    Name = "Forest",
                    FormBackColor = Color.FromArgb(240, 248, 240), // #F0F8F0
                    InputBackColor = Color.FromArgb(255, 255, 255), // #FFFFFF
                    ButtonBackColor = Color.FromArgb(230, 245, 230), // #E6F5E6
                    HeaderBackColor = Color.FromArgb(220, 240, 220), // #DCF0DC
                    AlternateRowColor = Color.FromArgb(245, 250, 245), // #F5FAF5
                    BorderColor = Color.FromArgb(180, 200, 180), // #B4C8B4
                    AccentColor = Color.FromArgb(76, 175, 80), // #4CAF50
                    AccentColorLight = Color.FromArgb(129, 199, 132), // #81C784
                    AccentColorDark = Color.FromArgb(56, 142, 60), // #388E3C
                    ErrorColor = Color.FromArgb(244, 67, 54), // #F44336
                    WarningColor = Color.FromArgb(255, 152, 0), // #FF9800
                    SuccessColor = Color.FromArgb(76, 175, 80), // #4CAF50
                    InfoColor = Color.FromArgb(33, 150, 243) // #2196F3
                },
                new ThemeDefinition
                {
                    Name = "Ocean",
                    FormBackColor = Color.FromArgb(240, 248, 255), // #F0F8FF
                    InputBackColor = Color.FromArgb(255, 255, 255), // #FFFFFF
                    ButtonBackColor = Color.FromArgb(230, 245, 255), // #E6F5FF
                    HeaderBackColor = Color.FromArgb(220, 240, 255), // #DCF0FF
                    AlternateRowColor = Color.FromArgb(245, 250, 255), // #F5FAFF
                    BorderColor = Color.FromArgb(180, 200, 220), // #B4C8DC
                    AccentColor = Color.FromArgb(33, 150, 243), // #2196F3
                    AccentColorLight = Color.FromArgb(100, 181, 246), // #64B5F6
                    AccentColorDark = Color.FromArgb(25, 118, 210), // #1976D2
                    ErrorColor = Color.FromArgb(244, 67, 54), // #F44336
                    WarningColor = Color.FromArgb(255, 152, 0), // #FF9800
                    SuccessColor = Color.FromArgb(76, 175, 80), // #4CAF50
                    InfoColor = Color.FromArgb(33, 150, 243) // #2196F3
                },
                new ThemeDefinition
                {
                    Name = "Sunset",
                    FormBackColor = Color.FromArgb(255, 248, 240), // #FFF8F0
                    InputBackColor = Color.FromArgb(255, 255, 255), // #FFFFFF
                    ButtonBackColor = Color.FromArgb(255, 245, 230), // #FFF5E6
                    HeaderBackColor = Color.FromArgb(255, 240, 220), // #FFF0DC
                    AlternateRowColor = Color.FromArgb(255, 250, 245), // #FFFAF5
                    BorderColor = Color.FromArgb(220, 200, 180), // #DCB8B4
                    AccentColor = Color.FromArgb(255, 87, 34), // #FF5722
                    AccentColorLight = Color.FromArgb(255, 138, 101), // #FF8A65
                    AccentColorDark = Color.FromArgb(216, 67, 21), // #D84315
                    ErrorColor = Color.FromArgb(244, 67, 54), // #F44336
                    WarningColor = Color.FromArgb(255, 152, 0), // #FF9800
                    SuccessColor = Color.FromArgb(76, 175, 80), // #4CAF50
                    InfoColor = Color.FromArgb(33, 150, 243) // #2196F3
                },
                new ThemeDefinition
                {
                    Name = "Lavender",
                    FormBackColor = Color.FromArgb(248, 240, 255), // #F8F0FF
                    InputBackColor = Color.FromArgb(255, 255, 255), // #FFFFFF
                    ButtonBackColor = Color.FromArgb(245, 230, 255), // #F5E6FF
                    HeaderBackColor = Color.FromArgb(240, 220, 255), // #F0DCFF
                    AlternateRowColor = Color.FromArgb(250, 245, 255), // #FAF5FF
                    BorderColor = Color.FromArgb(200, 180, 220), // #C8B4DC
                    AccentColor = Color.FromArgb(156, 39, 176), // #9C27B0
                    AccentColorLight = Color.FromArgb(186, 104, 200), // #BA68C8
                    AccentColorDark = Color.FromArgb(123, 31, 162), // #7B1FA2
                    ErrorColor = Color.FromArgb(244, 67, 54), // #F44336
                    WarningColor = Color.FromArgb(255, 152, 0), // #FF9800
                    SuccessColor = Color.FromArgb(76, 175, 80), // #4CAF50
                    InfoColor = Color.FromArgb(33, 150, 243) // #2196F3
                }
            };
        }

        /// <summary>
        /// Theme definition structure
        /// </summary>
        private class ThemeDefinition
        {
            public string Name { get; set; } = "";
            public Color FormBackColor { get; set; }
            public Color InputBackColor { get; set; }
            public Color ButtonBackColor { get; set; }
            public Color HeaderBackColor { get; set; }
            public Color AlternateRowColor { get; set; }
            public Color BorderColor { get; set; }
            public Color AccentColor { get; set; }
            public Color AccentColorLight { get; set; }
            public Color AccentColorDark { get; set; }
            public Color ErrorColor { get; set; }
            public Color WarningColor { get; set; }
            public Color SuccessColor { get; set; }
            public Color InfoColor { get; set; }
        }

        /// <summary>
        /// JSON converter for Color objects
        /// </summary>
        private class ColorJsonConverter : System.Text.Json.Serialization.JsonConverter<Color>
        {
            public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var colorString = reader.GetString();
                if (string.IsNullOrEmpty(colorString))
                    return Color.Empty;
                
                return ColorTranslator.FromHtml(colorString);
            }

            public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
            {
                if (value == Color.Empty || value == Color.Transparent)
                {
                    writer.WriteStringValue("transparent");
                }
                else
                {
                    writer.WriteStringValue($"#{value.R:X2}{value.G:X2}{value.B:X2}");
                }
            }
        }
    }
}