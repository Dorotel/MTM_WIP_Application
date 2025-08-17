using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Add_User : UserControl
    {
        #region Events

        public event EventHandler? UserAdded;
        public event EventHandler<string>? StatusMessageChanged;

        #endregion

        #region Fields

        private Helper_StoredProcedureProgress? _progressHelper;

        #endregion

        #region Constructors

        public Control_Add_User()
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["ControlType"] = nameof(Control_Add_User),
                ["InitializationTime"] = DateTime.Now,
                ["Thread"] = Thread.CurrentThread.ManagedThreadId
            }, nameof(Control_Add_User), nameof(Control_Add_User));

            Service_DebugTracer.TraceUIAction("ADD_USER_CONTROL_INITIALIZATION", nameof(Control_Add_User),
                new Dictionary<string, object>
                {
                    ["Phase"] = "START",
                    ["ComponentType"] = "UserControl"
                });

            InitializeComponent();

            Service_DebugTracer.TraceUIAction("THEME_APPLICATION", nameof(Control_Add_User),
                new Dictionary<string, object>
                {
                    ["DpiScaling"] = "APPLIED",
                    ["LayoutAdjustments"] = "APPLIED"
                });
            // Apply comprehensive DPI scaling and runtime layout adjustments
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

            Service_DebugTracer.TraceUIAction("DEFAULT_USER_TYPE_SET", nameof(Control_Add_User),
                new Dictionary<string, object>
                {
                    ["UserType"] = "NormalUser",
                    ["DefaultSelection"] = true
                });
            Control_Add_User_RadioButton_NormalUser.Checked = true;

            Service_DebugTracer.TraceUIAction("KEYPRESS_EVENTS_SETUP", nameof(Control_Add_User),
                new Dictionary<string, object>
                {
                    ["TextBoxes"] = new[] { "FirstName", "LastName", "UserName", "Pin", "VisualUserName", "VisualPassword" },
                    ["KeyPressHandler"] = "NoSpaces"
                });
            Control_Add_User_TextBox_FirstName.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_LastName.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_UserName.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_Pin.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_VisualUserName.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;
            Control_Add_User_TextBox_VisualPassword.KeyPress += Control_Add_User_TextBox_NoSpaces_KeyPress;

            Service_DebugTracer.TraceUIAction("PASSWORD_FIELDS_SETUP", nameof(Control_Add_User),
                new Dictionary<string, object>
                {
                    ["PasswordFields"] = new[] { "Pin", "VisualPassword" },
                    ["UseSystemPasswordChar"] = true,
                    ["VisualFieldsEnabled"] = false
                });
            Control_Add_User_TextBox_Pin.UseSystemPasswordChar = true;
            Control_Add_User_TextBox_VisualPassword.UseSystemPasswordChar = true;
            Control_Add_User_TextBox_VisualUserName.Enabled = false;
            Control_Add_User_TextBox_VisualPassword.Enabled = false;

            Service_DebugTracer.TraceUIAction("VISUAL_ACCESS_EVENT_SETUP", nameof(Control_Add_User),
                new Dictionary<string, object>
                {
                    ["CheckBoxEvent"] = "VisualAccess.CheckedChanged"
                });
            Control_Add_User_CheckBox_VisualAccess.CheckedChanged +=
                Control_Add_User_CheckBox_VisualAccess_CheckedChanged;

            Service_DebugTracer.TraceUIAction("VIEW_PASSWORDS_EVENT_SETUP", nameof(Control_Add_User),
                new Dictionary<string, object>
                {
                    ["CheckBoxEvent"] = "ViewHidePasswords.CheckedChanged"
                });
            Control_Add_User_CheckBox_ViewHidePasswords.CheckedChanged +=
                Control_Add_User_CheckBox_ViewHidePasswords_CheckedChanged;

            Service_DebugTracer.TraceUIAction("ADD_USER_CONTROL_INITIALIZATION", nameof(Control_Add_User),
                new Dictionary<string, object>
                {
                    ["Phase"] = "COMPLETE",
                    ["Success"] = true
                });

            Service_DebugTracer.TraceMethodExit(null, nameof(Control_Add_User), nameof(Control_Add_User));
        }

        #endregion

        #region Public Methods

        public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, 
                this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
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
                    ShowError("First name is required");
                    Control_Add_User_TextBox_FirstName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_LastName.Text))
                {
                    ShowError("Last name is required");
                    Control_Add_User_TextBox_LastName.Focus();
                    return;
                }

                if (Control_Add_User_ComboBox_Shift.SelectedIndex <= 0)
                {
                    ShowError("Please select a shift");
                    Control_Add_User_ComboBox_Shift.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_UserName.Text))
                {
                    ShowError("User name is required");
                    Control_Add_User_TextBox_UserName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_Pin.Text))
                {
                    ShowError("Pin is required");
                    Control_Add_User_TextBox_Pin.Focus();
                    return;
                }

                string userName = Control_Add_User_TextBox_UserName.Text.ToUpper();

                UpdateProgress(20, "Checking for existing user...");
                await Task.Delay(100);

                // Use enhanced stored procedure call with status reporting
                var userExistsResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_Exists",
                    new Dictionary<string, object> { ["User"] = userName },
                    _progressHelper,
                    true
                );

                if (!userExistsResult.IsSuccess)
                {
                    ShowError($"Error checking user existence: {userExistsResult.ErrorMessage}");
                    return;
                }

                if (userExistsResult.Data != null && userExistsResult.Data.Rows.Count > 0)
                {
                    int userExists = Convert.ToInt32(userExistsResult.Data.Rows[0]["UserExists"]);
                    if (userExists > 0)
                    {
                        ShowError("User already exists");
                        Control_Add_User_TextBox_UserName.Focus();
                        return;
                    }
                }

                UpdateProgress(30, "Processing user information...");
                string fullName = Control_Add_User_TextBox_FirstName.Text + " " +
                                  Control_Add_User_TextBox_LastName.Text;
                await Task.Delay(100);

                UpdateProgress(40, "Creating user account...");
                
                // Use enhanced stored procedure call with status reporting
                var createUserResult = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_Add_User",
                    new Dictionary<string, object>
                    {
                        ["User"] = userName,
                        ["FullName"] = fullName,
                        ["Shift"] = Control_Add_User_ComboBox_Shift.Text,
                        ["VitsUser"] = false,
                        ["Pin"] = Control_Add_User_TextBox_Pin.Text,
                        ["LastShownVersion"] = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty,
                        ["HideChangeLog"] = "false",
                        ["Theme_Name"] = "Default",
                        ["Theme_FontSize"] = 9,
                        ["VisualUserName"] = Control_Add_User_TextBox_VisualUserName.Text,
                        ["VisualPassword"] = Control_Add_User_TextBox_VisualPassword.Text,
                        ["WipServerAddress"] = Model_Users.WipServerAddress,
                        ["WIPDatabase"] = Model_Users.Database,
                        ["WipServerPort"] = Model_Users.WipServerPort
                    },
                    _progressHelper,
                    true
                );

                if (!createUserResult.IsSuccess)
                {
                    ShowError($"Error creating user: {createUserResult.ErrorMessage}");
                    return;
                }

                UpdateProgress(60, "Retrieving user information...");
                
                // Get the created user to retrieve the ID
                var getUserResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "usr_users_Get_ByUser",
                    new Dictionary<string, object> { ["User"] = userName },
                    _progressHelper,
                    true
                );

                if (!getUserResult.IsSuccess || getUserResult.Data == null || getUserResult.Data.Rows.Count == 0)
                {
                    ShowError("Could not retrieve new user ID after creation");
                    return;
                }

                UpdateProgress(70, "Processing user role assignment...");
                await Task.Delay(100);

                int userId = Convert.ToInt32(getUserResult.Data.Rows[0]["ID"]);
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
                
                // Add user role assignment
                var addRoleResult = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "sys_user_roles_Add",
                    new Dictionary<string, object>
                    {
                        ["UserID"] = userId,
                        ["RoleID"] = roleId,
                        ["AssignedBy"] = Environment.UserName
                    },
                    _progressHelper,
                    true
                );

                if (!addRoleResult.IsSuccess)
                {
                    ShowError($"User created but role assignment failed: {addRoleResult.ErrorMessage}");
                    // Note: User was created successfully, but role assignment failed
                }

                UpdateProgress(90, "Finalizing user setup...");
                await Task.Delay(100);

                UpdateProgress(95, "Clearing form...");
                ClearForm();
                Control_Add_User_TextBox_UserName.Clear();

                ShowSuccess($"User '{fullName}' created successfully!");
                await Task.Delay(500);
                HideProgress();

                UserAdded?.Invoke(this, EventArgs.Empty);
                UpdateStatus($"User '{fullName}' added successfully!");

                MessageBox.Show($@"User '{fullName}' created successfully!", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError($"Unexpected error creating user: {ex.Message}");
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
            _progressHelper?.ShowProgress(status);
        }

        private void UpdateProgress(int progress, string status)
        {
            _progressHelper?.UpdateProgress(progress, status);
        }

        private void HideProgress()
        {
            _progressHelper?.HideProgress();
        }

        private void UpdateStatus(string message)
        {
            _progressHelper?.UpdateStatus(message);
        }

        private void ShowError(string errorMessage)
        {
            _progressHelper?.ShowError(errorMessage);
        }

        private void ShowSuccess(string successMessage)
        {
            _progressHelper?.ShowSuccess(successMessage);
        }

        #endregion
    }
}
