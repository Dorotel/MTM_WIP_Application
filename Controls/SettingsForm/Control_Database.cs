using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Database : UserControl
    {
        public event EventHandler? DatabaseSettingsUpdated;
        public event EventHandler<string>? StatusMessageChanged;

        public Control_Database()
        {
            InitializeComponent();

            // Apply comprehensive DPI scaling and runtime layout adjustments
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

            // Wire up button events
            Control_Database_Button_Save.Click += SaveButton_Click;
            Control_Database_Button_Reset.Click += ResetButton_Click;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadDatabaseSettingsAsync();
        }

        private async Task LoadDatabaseSettingsAsync()
        {
            try
            {
                string user = Model_AppVariables.User;

                Control_Database_TextBox_Server.Text =
                    await Dao_User.GetWipServerAddressAsync(user) ?? "172.16.1.104";
                Control_Database_TextBox_Port.Text = await Dao_User.GetWipServerPortAsync(user) ?? "3306";
                Control_Database_TextBox_Database.Text = await Dao_User.GetDatabaseAsync(user) ?? "mtm_wip_application";

                StatusMessageChanged?.Invoke(this, "Database settings loaded successfully.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                StatusMessageChanged?.Invoke(this, $"Error loading database settings: {ex.Message}");

                // Set default values as fallback
                Control_Database_TextBox_Server.Text = "172.16.1.104";
                Control_Database_TextBox_Port.Text = "3306";
                Control_Database_TextBox_Database.Text = "mtm_wip_application";
            }
        }

        private async Task SaveDatabaseSettingsAsync()
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(Control_Database_TextBox_Server.Text))
                {
                    StatusMessageChanged?.Invoke(this, "Server address is required.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Database_TextBox_Port.Text))
                {
                    StatusMessageChanged?.Invoke(this, "Port is required.");
                    return;
                }

                if (!int.TryParse(Control_Database_TextBox_Port.Text, out int port) || port <= 0 || port > 65535)
                {
                    StatusMessageChanged?.Invoke(this, "Port must be a valid number between 1 and 65535.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Database_TextBox_Database.Text))
                {
                    StatusMessageChanged?.Invoke(this, "Database name is required.");
                    return;
                }

                string user = Model_AppVariables.User;

                await Dao_User.SetWipServerAddressAsync(user, Control_Database_TextBox_Server.Text.Trim());
                await Dao_User.SetDatabaseAsync(user, Control_Database_TextBox_Database.Text.Trim());
                await Dao_User.SetWipServerPortAsync(user, Control_Database_TextBox_Port.Text.Trim());

                DatabaseSettingsUpdated?.Invoke(this, EventArgs.Empty);
                StatusMessageChanged?.Invoke(this,
                    "Database settings saved successfully. Restart application for changes to take effect.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                StatusMessageChanged?.Invoke(this, $"Error saving database settings: {ex.Message}");
            }
        }

        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            try
            {
                Control_Database_Button_Save.Enabled = false;
                await SaveDatabaseSettingsAsync();
            }
            finally
            {
                Control_Database_Button_Save.Enabled = true;
            }
        }

        private async void ResetButton_Click(object? sender, EventArgs e)
        {
            try
            {
                Control_Database_Button_Reset.Enabled = false;

                // Reset to default values
                Control_Database_TextBox_Server.Text = "172.16.1.104";
                Control_Database_TextBox_Port.Text = "3306";
                Control_Database_TextBox_Database.Text = "mtm_wip_application";

                StatusMessageChanged?.Invoke(this, "Database settings reset to defaults.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                StatusMessageChanged?.Invoke(this, $"Error resetting database settings: {ex.Message}");
            }
            finally
            {
                Control_Database_Button_Reset.Enabled = true;
            }
        }
    }
}
