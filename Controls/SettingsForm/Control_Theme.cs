using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Theme : UserControl
    {
        public event EventHandler? ThemeChanged;

        public Control_Theme()
        {
            InitializeComponent();
            SettingsForm_Button_SaveTheme.Click += SaveButton_Click;
            SettingsForm_Button_SwitchTheme.Click += PreviewButton_Click;
            LoadThemeSettingsAsync();
        }

        public async void LoadThemeSettingsAsync()
        {
            try
            {
                SettingsForm_ComboBox_Theme.Items.Clear();
                string[] themeNames = Core_Themes.Core_AppThemes.GetThemeNames().ToArray();
                SettingsForm_ComboBox_Theme.Items.AddRange(themeNames);

                string user = Model_AppVariables.User;
                string? themeName = await Dao_User.GetThemeNameAsync(user);

                if (!string.IsNullOrEmpty(themeName) && SettingsForm_ComboBox_Theme.Items.Contains(themeName))
                {
                    SettingsForm_ComboBox_Theme.SelectedItem = themeName;
                }
                else if (SettingsForm_ComboBox_Theme.Items.Count > 0)
                {
                    SettingsForm_ComboBox_Theme.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading theme settings: {ex.Message}", "Theme Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (SettingsForm_ComboBox_Theme.Items.Count > 0)
                {
                    SettingsForm_ComboBox_Theme.SelectedIndex = 0;
                }
            }
        }

        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            try
            {
                SettingsForm_Button_SaveTheme.Enabled = false;
                string? selectedTheme = SettingsForm_ComboBox_Theme.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(selectedTheme))
                {
                    MessageBox.Show("Please select a theme.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string user = Model_AppVariables.User;
                var themeObj = new { Theme_Name = selectedTheme, Theme_FontSize = 9 };
                string themeJson = System.Text.Json.JsonSerializer.Serialize(themeObj);

                await Dao_User.SetSettingsJsonAsync(user, themeJson);

                // Update the current theme in the app variables and apply to all open forms
                Model_AppVariables.ThemeName = selectedTheme;
                foreach (Form openForm in Application.OpenForms)
                {
                    Core_Themes.ApplyTheme(openForm);
                }

                ThemeChanged?.Invoke(this, EventArgs.Empty);

                MessageBox.Show("Theme saved and applied successfully!", "Theme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving theme: {ex.Message}", "Theme Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SettingsForm_Button_SaveTheme.Enabled = true;
            }
        }

        private void PreviewButton_Click(object? sender, EventArgs e)
        {
            string? selectedTheme = SettingsForm_ComboBox_Theme.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedTheme))
                return;

            try
            {
                string? originalTheme = Model_AppVariables.ThemeName;
                Model_AppVariables.ThemeName = selectedTheme;
                foreach (Form openForm in Application.OpenForms)
                {
                    Core_Themes.ApplyTheme(openForm);
                }
                Model_AppVariables.ThemeName = originalTheme;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error previewing theme: {ex.Message}", "Theme Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
