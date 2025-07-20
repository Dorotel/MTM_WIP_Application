using System.Data;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Add_User : UserControl
    {
        #region Events

        public event EventHandler? UserAdded;

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

        #region Initialization

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Control_Add_User_ComboBox_Shift.Items.Clear();
            Control_Add_User_ComboBox_Shift.Items.AddRange(new object[]
            {
                "[ Enter Shift ]", "First", "Second", "Third", "Weekend"
            });
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
                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_FirstName.Text))
                {
                    MessageBox.Show("First name is required.", "Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_User_TextBox_FirstName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_LastName.Text))
                {
                    MessageBox.Show("Last name is required.", "Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_User_TextBox_LastName.Focus();
                    return;
                }

                if (Control_Add_User_ComboBox_Shift.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select a shift.", "Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_User_ComboBox_Shift.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_UserName.Text))
                {
                    MessageBox.Show("User name is required.", "Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_User_TextBox_UserName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Add_User_TextBox_Pin.Text))
                {
                    MessageBox.Show("Pin is required.", "Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_User_TextBox_Pin.Focus();
                    return;
                }

                if (await Dao_User.UserExistsAsync(Control_Add_User_TextBox_UserName.Text.ToUpper()))
                {
                    MessageBox.Show("User already exists.", "Duplicate User", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                await Dao_User.InsertUserAsync(
                    Control_Add_User_TextBox_UserName.Text.ToUpper(),
                    Control_Add_User_TextBox_FirstName.Text + " " + Control_Add_User_TextBox_LastName.Text,
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
                DataRow? userRow =
                    await Dao_User.GetUserByUsernameAsync(Control_Add_User_TextBox_UserName.Text.ToUpper(), true);
                if (userRow == null || !userRow.Table.Columns.Contains("ID"))
                {
                    MessageBox.Show("Could not retrieve new user ID.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                int userId = Convert.ToInt32(userRow["ID"]);
                int roleId = 3;
                if (Control_Add_User_RadioButton_Administrator.Checked)
                {
                    roleId = 1;
                }
                else if (Control_Add_User_RadioButton_ReadOnly.Checked)
                {
                    roleId = 2;
                }

                await Dao_User.AddUserRoleAsync(userId, roleId, Environment.UserName, true);
                ClearForm();
                Control_Add_User_TextBox_UserName.Clear();
                UserAdded?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding user: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
    }
}
