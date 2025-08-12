using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Theme : UserControl
    {
        public event EventHandler? ThemeChanged;
        public event EventHandler<string>? StatusMessageChanged;

        public Control_Theme()
        {
            InitializeComponent();
            Control_Shortcuts_Button_Save.Click += SaveButton_Click;
            Control_Shortcuts_Button_Switch.Click += PreviewButton_Click;
            LoadThemeSettingsAsync();
        }

        public async void LoadThemeSettingsAsync()
        {
            try
            {
                Control_Shortcuts_ComboBox_Theme.Items.Clear();
                string[] themeNames = Core_Themes.Core_AppThemes.GetThemeNames().ToArray();
                Control_Shortcuts_ComboBox_Theme.Items.AddRange(themeNames);

                string user = Model_AppVariables.User;
                string? themeName = await Dao_User.GetThemeNameAsync(user);

                if (!string.IsNullOrEmpty(themeName) && Control_Shortcuts_ComboBox_Theme.Items.Contains(themeName))
                {
                    Control_Shortcuts_ComboBox_Theme.SelectedItem = themeName;
                }
                else if (Control_Shortcuts_ComboBox_Theme.Items.Count > 0)
                {
                    Control_Shortcuts_ComboBox_Theme.SelectedIndex = 0;
                }

                StatusMessageChanged?.Invoke(this, "Theme settings loaded successfully.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                StatusMessageChanged?.Invoke(this, $"Error loading theme settings: {ex.Message}");
                
                // Fallback to first theme if available
                if (Control_Shortcuts_ComboBox_Theme.Items.Count > 0)
                {
                    Control_Shortcuts_ComboBox_Theme.SelectedIndex = 0;
                }
            }
        }

        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            try
            {
                Control_Shortcuts_Button_Save.Enabled = false;
                string? selectedTheme = Control_Shortcuts_ComboBox_Theme.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(selectedTheme))
                {
                    StatusMessageChanged?.Invoke(this, "Please select a theme.");
                    return;
                }

                string user = Model_AppVariables.User;

                // FIXED: Use the proper theme setter that works with existing database structure
                await Dao_User.SetThemeNameAsync(user, selectedTheme, true);

                // Update the current theme in the app variables and apply to all open forms
                Model_AppVariables.ThemeName = selectedTheme;
                foreach (Form openForm in Application.OpenForms)
                {
                    Core_Themes.ApplyTheme(openForm);
                }

                ThemeChanged?.Invoke(this, EventArgs.Empty);
                StatusMessageChanged?.Invoke(this, "Theme saved and applied successfully!");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                StatusMessageChanged?.Invoke(this, $"Error saving theme: {ex.Message}");
            }
            finally
            {
                Control_Shortcuts_Button_Save.Enabled = true;
            }
        }

        private void PreviewButton_Click(object? sender, EventArgs e)
        {
            try
            {
                string? selectedTheme = Control_Shortcuts_ComboBox_Theme.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(selectedTheme))
                {
                    StatusMessageChanged?.Invoke(this, "Please select a theme to preview.");
                    return;
                }

                string? originalTheme = Model_AppVariables.ThemeName;
                Model_AppVariables.ThemeName = selectedTheme;
                foreach (Form openForm in Application.OpenForms)
                {
                    Core_Themes.ApplyTheme(openForm);
                }
                Model_AppVariables.ThemeName = originalTheme;
                StatusMessageChanged?.Invoke(this, $"Theme preview applied: {selectedTheme}");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                StatusMessageChanged?.Invoke(this, $"Error previewing theme: {ex.Message}");
            }
        }
    }
}
