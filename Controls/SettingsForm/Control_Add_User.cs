using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Add_User : UserControl
    {
        #region Events

        public event EventHandler? UserAdded;
        public event EventHandler<string>? StatusMessageChanged;

        #endregion

        #region Fields

        private ToolStripProgressBar? _progressBar;
        private ToolStripStatusLabel? _statusLabel;

        #endregion

        #region Constructors

        public Control_Add_User()
        {
            InitializeComponent();

            // Apply comprehensive DPI scaling and runtime layout adjustments
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

            Control_Add_User_RadioButton_NormalUser.Checked = true;
            Control_Add_User_TextBox_FirstName.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_LastName.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_UserName.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_Pin.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_VisualUserName.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_VisualPassword.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_Pin.UseSystemPasswordChar = true;
            Control_Add_User_TextBox_VisualPassword.UseSystemPasswordChar = true;
            Control_Add_User_TextBox_VisualUserName.Enabled = false;
            Control_Add_User_TextBox_VisualPassword.Enabled = false;
            Control_Add_User_CheckBox_VisualAccess.CheckedChanged +=
                Control_Add_User_CheckBox_VisualAccess_CheckedChanged;
            Control_Add_User_CheckBox_ViewHidePasswords.CheckedChanged +=
                Control_Add_User_CheckBox_ViewHidePasswords_CheckedChanged;
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Control_Add_User_ComboBox_Shift.Items.Clear();
            Control_Add_User_ComboBox_Shift.Items.AddRange([
                "[ Enter Shift ]", "First", "Second", "Third", "Weekend"
            ]);
            Control_Add_User_ComboBox_Shift.SelectedIndex = 0;
        }

        #endregion

        #region Event Handlers

        private void Control_Add_User_TextBox_NoSpaces_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void Control_Add_User_CheckBox_ViewHidePasswords_CheckedChanged(object? sender, EventArgs e)
        {
            bool show = Control_Add_User_CheckBox_ViewHidePasswords.Checked;
            Control_Add_User_TextBox_Pin.UseSystemPasswordChar = !show;
            Control_Add_User_TextBox_VisualPassword.UseSystemPasswordChar = !show;
        }

        private void Control_Add_User_CheckBox_VisualAccess_CheckedChanged(object? sender, EventArgs e)
        {
            bool enabled = Control_Add_User_CheckBox_VisualAccess.Checked;
            Control_Add_User_TextBox_VisualUserName.Enabled = enabled;
            Control_Add_User_TextBox_VisualPassword.Enabled = enabled;
            if (!enabled)
            {
                Control_Add_User_TextBox_VisualUserName.Clear();
                Control_Add_User_TextBox_VisualPassword.Clear();
            }
        }

        private async void Control_Add_User_Button_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Control_Add_User_Button_Save.Enabled = false;

                ShowProgress("Initializing user creation...");
                UpdateProgress(5, "Preparing to create new user...");
                await Task.Delay(100);

                UpdateProgress(10, "Validating form data...");
                await Task.Delay(50);

                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_FirstName.Text))
                {
                    HideProgress();
                    UpdateStatus("First name is required.");
                    Control_Add_User_TextBox_FirstName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_LastName.Text))
                {
                    HideProgress();
                    UpdateStatus("Last name is required.");
                    Control_Add_User_TextBox_LastName.Focus();
                    return;
                }

                if (Control_Add_User_ComboBox_Shift.SelectedIndex <= 0)
                {
                    HideProgress();
                    UpdateStatus("Please select a shift.");
                    Control_Add_User_ComboBox_Shift.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_UserName.Text))
                {
                    HideProgress();
                    UpdateStatus("User name is required.");
                    Control_Add_User_TextBox_UserName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_Pin.Text))
                {
                    HideProgress();
                    UpdateStatus("Pin is required.");
                    Control_Add_User_TextBox_Pin.Focus();
                    return;
                }

                string userName = Control_Add_User_TextBox_UserName.Text.ToUpper();

                UpdateProgress(20, "Checking for existing user...");
                await Task.Delay(100);

                if (await Dao_User.UserExistsAsync(userName))
                {
                    HideProgress();
                    UpdateStatus("User already exists.");
                    Control_Add_User_TextBox_UserName.Focus();
                    return;
                }

                UpdateProgress(30, "Processing user information...");
                string fullName = Control_Add_User_TextBox_FirstName.Text + " " +
                                  Control_Add_User_TextBox_LastName.Text;
                await Task.Delay(100);

                UpdateProgress(40, "Creating user account...");
                await Dao_User.InsertUserAsync(
                    userName,
                    fullName,
                    Control_Add_User_ComboBox_Shift.Text,
                    false,
                    Control_Add_User_TextBox_Pin.Text,
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty,
                    "false",
                    "Default",
                    9,
                    Control_Add_User_TextBox_VisualUserName.Text,
                    Control_Add_User_TextBox_VisualPassword.Text,
                    Model_Users.WipServerAddress,
                    Model_Users.Database,
                    Model_Users.WipServerPort,
                    true
                );

                UpdateProgress(60, "Retrieving user information...");
                DataRow? userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
                if (userRow == null || !userRow.Table.Columns.Contains("ID"))
                {
                    HideProgress();
                    LoggingUtility.LogApplicationError(
                        new InvalidOperationException("Could not retrieve new user ID after creation"));
                    UpdateStatus("Could not retrieve new user ID.");
                    return;
                }

                UpdateProgress(70, "Processing user role assignment...");
                await Task.Delay(100);

                int userId = Convert.ToInt32(userRow["ID"]);
                int roleId = 3; // Default to Normal User
                if (Control_Add_User_RadioButton_Administrator.Checked)
                {
                    roleId = 1;
                }
                else if (Control_Add_User_RadioButton_ReadOnly.Checked)
                {
                    roleId = 2;
                }

                UpdateProgress(80, "Assigning user role...");
                await Dao_User.AddUserRoleAsync(userId, roleId, Environment.UserName, true);

                UpdateProgress(90, "Finalizing user setup...");
                await Task.Delay(100);

                UpdateProgress(95, "Clearing form...");
                ClearForm();
                Control_Add_User_TextBox_UserName.Clear();

                UpdateProgress(100, $"User '{fullName}' created successfully!");
                await Task.Delay(500);
                HideProgress();

                UserAdded?.Invoke(this, EventArgs.Empty);
                UpdateStatus($"User '{fullName}' added successfully!");

                MessageBox.Show($@"User '{fullName}' created successfully!", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                HideProgress();
                LoggingUtility.LogApplicationError(ex);
                UpdateStatus($"Error adding user: {ex.Message}");
                MessageBox.Show($@"Error creating user: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Control_Add_User_Button_Save.Enabled = true;
            }
        }

        private void Control_Add_User_Button_Clear_Click(object sender, EventArgs e) => ClearForm();

        #endregion

        #region Methods

        private void ClearForm()
        {
            Control_Add_User_TextBox_FirstName.Clear();
            Control_Add_User_TextBox_LastName.Clear();
            Control_Add_User_ComboBox_Shift.SelectedIndex = 0;
            Control_Add_User_TextBox_Pin.Clear();
            Control_Add_User_CheckBox_VisualAccess.Checked = false;
            Control_Add_User_TextBox_VisualUserName.Clear();
            Control_Add_User_TextBox_VisualPassword.Clear();
            Control_Add_User_RadioButton_ReadOnly.Checked = false;
            Control_Add_User_RadioButton_NormalUser.Checked = true;
            Control_Add_User_RadioButton_Administrator.Checked = false;
            Control_Add_User_TextBox_FirstName.Focus();
            Control_Add_User_TextBox_VisualUserName.Enabled = false;
            Control_Add_User_TextBox_VisualPassword.Enabled = false;
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
