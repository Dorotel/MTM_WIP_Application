using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Remove_User : UserControl
    {
        #region Events

        public event EventHandler? UserRemoved;

        #endregion

        #region Fields

        private ToolStripProgressBar? _progressBar;
        private ToolStripStatusLabel? _statusLabel;

        #endregion

        #region Constructors

        public Control_Remove_User()
        {
            InitializeComponent();
            RemoveUserControl_Button_Remove.Click += RemoveUserControl_Button_Remove_Click;
            RemoveUserControl_ComboBox_Users.SelectedIndexChanged +=
                RemoveUserControl_ComboBox_Users_SelectedIndexChanged;
            LoadUsersAsync();
        }

        #endregion

        #region Public Methods

        public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            _progressBar = progressBar;
            _statusLabel = statusLabel;
        }

        #endregion

        #region Initialization

        private async void LoadUsersAsync()
        {
            try
            {
                ShowProgress("Initializing user list...");
                UpdateProgress(10, "Connecting to database...");
                await Task.Delay(50);

                UpdateProgress(30, "Refreshing user data...");
                // First, refresh the user data table to get the latest users
                await Helper_UI_ComboBoxes.SetupUserDataTable();

                UpdateProgress(70, "Populating user list...");
                // Then fill the ComboBox with the refreshed data
                await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(RemoveUserControl_ComboBox_Users);

                UpdateProgress(100, "User list loaded successfully");
                await Task.Delay(300);
                HideProgress();
            }
            catch (Exception e)
            {
                HideProgress();
                LoggingUtility.LogApplicationError(e);
                MessageBox.Show($@"Error loading users: {e.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        private async void RemoveUserControl_ComboBox_Users_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (RemoveUserControl_ComboBox_Users.Text is not { } userName)
            {
                RemoveUserControl_Label_FullName.Text = "";
                RemoveUserControl_Label_Role.Text = "";
                RemoveUserControl_Label_Shift.Text = "";
                return;
            }

            try
            {
                ShowProgress("Loading user details...");
                UpdateProgress(20, $"Retrieving details for {userName}...");

                DataRow? userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
                if (userRow != null)
                {
                    UpdateProgress(60, "Processing user information...");

                    RemoveUserControl_Label_FullName.Text = userRow["Full Name"]?.ToString() ?? "";
                    RemoveUserControl_Label_Shift.Text = userRow["Shift"]?.ToString() ?? "";

                    if (userRow.Table.Columns.Contains("ID") && int.TryParse(userRow["ID"]?.ToString(), out int userId))
                    {
                        UpdateProgress(80, "Loading user role information...");
                        int roleId = await Dao_User.GetUserRoleIdAsync(userId);
                        RemoveUserControl_Label_Role.Text = roleId switch
                        {
                            1 => "Administrator",
                            2 => "Read-Only User",
                            3 => "Normal User",
                            _ => "Unknown"
                        };
                    }
                    else
                    {
                        RemoveUserControl_Label_Role.Text = "";
                    }
                }
                else
                {
                    RemoveUserControl_Label_FullName.Text = "";
                    RemoveUserControl_Label_Role.Text = "";
                    RemoveUserControl_Label_Shift.Text = "";
                }

                UpdateProgress(100, "User details loaded successfully");
                await Task.Delay(200);
                HideProgress();
            }
            catch (Exception ex)
            {
                HideProgress();
                LoggingUtility.LogApplicationError(ex);
                UpdateStatus("Error loading user details");
            }
        }

        private async void RemoveUserControl_Button_Remove_Click(object? sender, EventArgs e)
        {
            if (RemoveUserControl_ComboBox_Users.Text is not { } userName)
            {
                return;
            }

            DialogResult confirm = MessageBox.Show($@"Are you sure you want to remove user '{userName}'?",
                @"Confirm Remove",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                ShowProgress("Initializing user removal...");
                UpdateProgress(5, $"Preparing to remove user '{userName}'...");
                await Task.Delay(100);

                UpdateProgress(10, "Retrieving user information...");
                DataRow? userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
                if (userRow != null && userRow.Table.Columns.Contains("ID"))
                {
                    UpdateProgress(20, "Validating user data...");
                    await Task.Delay(100);

                    UpdateProgress(30, "Deleting user settings...");
                    await Dao_User.DeleteUserSettingsAsync(userName);

                    int userId = Convert.ToInt32(userRow["ID"]);

                    UpdateProgress(45, "Retrieving user role information...");
                    int roleId = await Dao_User.GetUserRoleIdAsync(userId);

                    UpdateProgress(60, "Removing user role assignments...");
                    await Dao_User.RemoveUserRoleAsync(userId, roleId);

                    UpdateProgress(75, "Deleting user account...");
                    await Dao_User.DeleteUserAsync(userName);

                    UpdateProgress(85, "Cleaning up user data...");
                    await Task.Delay(100);

                    UpdateProgress(95, "Refreshing user list...");
                    UserRemoved?.Invoke(this, EventArgs.Empty);

                    UpdateProgress(100, "User removed successfully!");
                    await Task.Delay(500);
                    HideProgress();

                    MessageBox.Show(@"User removed successfully!", @"Success", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Reinitialize the ComboBox with detailed progress
                    LoadUsersAsync();
                }
            }
            catch (Exception ex)
            {
                HideProgress();
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($@"Error removing user: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Progress Methods

        private void ShowProgress(string status = "Loading...")
        {
            if (_progressBar != null && _statusLabel != null)
            {
                if (_progressBar.Owner?.InvokeRequired == true)
                {
                    _progressBar.Owner.Invoke(new Action(() =>
                    {
                        _progressBar.Visible = true;
                        _progressBar.Value = 0;
                        Application.DoEvents();
                        _statusLabel.Text = status;
                    }));
                }
                else
                {
                    _progressBar.Visible = true;
                    _progressBar.Value = 0;
                    Application.DoEvents();
                    _statusLabel.Text = status;
                }
            }
        }

        private void UpdateProgress(int progress, string status)
        {
            if (_progressBar != null && _statusLabel != null)
            {
                progress = Math.Max(0, Math.Min(100, progress)); // Clamp between 0-100

                if (_progressBar.Owner?.InvokeRequired == true)
                {
                    _progressBar.Owner.Invoke(new Action(() =>
                    {
                        _progressBar.Value = progress;
                        Application.DoEvents();
                        _statusLabel.Text = $"{status} ({progress}%)";
                    }));
                }
                else
                {
                    _progressBar.Value = progress;
                    Application.DoEvents();
                    _statusLabel.Text = $"{status} ({progress}%)";
                }
            }
        }

        private void HideProgress()
        {
            if (_progressBar != null && _statusLabel != null)
            {
                if (_progressBar.Owner?.InvokeRequired == true)
                {
                    _progressBar.Owner.Invoke(new Action(() =>
                    {
                        _progressBar.Visible = false;
                        Application.DoEvents();
                        _statusLabel.Text = "Ready";
                    }));
                }
                else
                {
                    _progressBar.Visible = false;
                    Application.DoEvents();
                    _statusLabel.Text = "Ready";
                }
            }
        }

        private void UpdateStatus(string message)
        {
            if (_statusLabel != null)
            {
                if (_statusLabel.Owner?.InvokeRequired == true)
                {
                    _statusLabel.Owner.Invoke(new Action(() => _statusLabel.Text = message));
                }
                else
                {
                    _statusLabel.Text = message;
                }
            }
        }

        #endregion
    }
}
