// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;
using MTM_Inventory_Application.Data;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class EditUserControl : UserControl
    {
        #region Events
        public event EventHandler? UserEdited;
        #endregion

        #region Constructors
        public EditUserControl()
        {
            InitializeComponent();
            EditUserControl_TextBox_FirstName.KeyPress += TextBox_NoSpaces_KeyPress;
            EditUserControl_TextBox_LastName.KeyPress += TextBox_NoSpaces_KeyPress;
            EditUserControl_TextBox_Pin.KeyPress += TextBox_NoSpaces_KeyPress;
            EditUserControl_TextBox_VisualUserName.KeyPress += TextBox_NoSpaces_KeyPress;
            EditUserControl_TextBox_VisualPassword.KeyPress += TextBox_NoSpaces_KeyPress;
            EditUserControl_TextBox_Pin.UseSystemPasswordChar = true;
            EditUserControl_TextBox_VisualPassword.UseSystemPasswordChar = true;
            EditUserControl_TextBox_VisualUserName.Enabled = false;
            EditUserControl_TextBox_VisualPassword.Enabled = false;
            EditUserControl_CheckBox_VisualAccess.CheckedChanged += EditUserControl_CheckBox_VisualAccess_CheckedChanged;
            EditUserControl_CheckBox_ViewHidePasswords.CheckedChanged += EditUserControl_CheckBox_ViewHidePasswords_CheckedChanged;
            EditUserControl_ComboBox_Users.SelectedIndexChanged += EditUserControl_ComboBox_Users_SelectedIndexChanged;
            EditUserControl_Button_Save.Click += EditUserControl_Button_Save_Click;
            EditUserControl_Button_Clear.Click += EditUserControl_Button_Clear_Click;
        }
        #endregion

        #region Initialization
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EditUserControl_ComboBox_Shift.Items.Clear();
            EditUserControl_ComboBox_Shift.Items.AddRange(new object[] { "First", "Second", "Third", "Weekend" });
            LoadUsersAsync();
        }

        private async void LoadUsersAsync()
        {
            EditUserControl_ComboBox_Users.Items.Clear();
            var users = await Dao_User.GetAllUsersAsync();
            foreach (DataRow row in users.Rows)
            {
                var user = row["User"]?.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(user))
                    EditUserControl_ComboBox_Users.Items.Add(user);
            }
            if (EditUserControl_ComboBox_Users.Items.Count > 0)
                EditUserControl_ComboBox_Users.SelectedIndex = 0;
        }
        #endregion

        #region Event Handlers
        private void TextBox_NoSpaces_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                e.Handled = true;
        }

        private void EditUserControl_CheckBox_ViewHidePasswords_CheckedChanged(object? sender, EventArgs e)
        {
            bool show = EditUserControl_CheckBox_ViewHidePasswords.Checked;
            EditUserControl_TextBox_Pin.UseSystemPasswordChar = !show;
            EditUserControl_TextBox_VisualPassword.UseSystemPasswordChar = !show;
        }

        private void EditUserControl_CheckBox_VisualAccess_CheckedChanged(object? sender, EventArgs e)
        {
            bool enabled = EditUserControl_CheckBox_VisualAccess.Checked;
            EditUserControl_TextBox_VisualUserName.Enabled = enabled;
            EditUserControl_TextBox_VisualPassword.Enabled = enabled;
            if (!enabled)
            {
                EditUserControl_TextBox_VisualUserName.Clear();
                EditUserControl_TextBox_VisualPassword.Clear();
            }
        }

        private async void EditUserControl_ComboBox_Users_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (EditUserControl_ComboBox_Users.SelectedItem is string userName)
            {
                var userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
                if (userRow != null)
                {
                    var names = (userRow["Full Name"]?.ToString() ?? "").Split(' ');
                    EditUserControl_TextBox_FirstName.Text = names.Length > 0 ? names[0] : "";
                    EditUserControl_TextBox_LastName.Text = names.Length > 1 ? string.Join(" ", names.Skip(1)) : "";
                    EditUserControl_TextBox_UserName.Text = userRow["User"]?.ToString() ?? "";
                    EditUserControl_ComboBox_Shift.SelectedItem = userRow["Shift"]?.ToString() ?? "First";
                    EditUserControl_TextBox_Pin.Text = userRow["Pin"]?.ToString() ?? "";
                    EditUserControl_CheckBox_VisualAccess.Checked = !string.IsNullOrWhiteSpace(userRow["VisualUserName"]?.ToString());
                    EditUserControl_TextBox_VisualUserName.Text = userRow["VisualUserName"]?.ToString() ?? "";
                    EditUserControl_TextBox_VisualPassword.Text = userRow["VisualPassword"]?.ToString() ?? "";
                    int userId = Convert.ToInt32(userRow["ID"]);
                    int roleId = await Dao_User.GetUserRoleIdAsync(userId);
                    EditUserControl_RadioButton_NormalUser.Checked = roleId == 3;
                    EditUserControl_RadioButton_Administrator.Checked = roleId == 1;
                    EditUserControl_RadioButton_ReadOnly.Checked = roleId == 2;
                }
            }
        }

        private async void EditUserControl_Button_Save_Click(object? sender, EventArgs e)
        {
            try
            {
                if (EditUserControl_ComboBox_Users.SelectedItem is not string userName)
                    return;
                if (string.IsNullOrWhiteSpace(EditUserControl_TextBox_FirstName.Text))
                {
                    MessageBox.Show("First name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    EditUserControl_TextBox_FirstName.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(EditUserControl_TextBox_LastName.Text))
                {
                    MessageBox.Show("Last name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    EditUserControl_TextBox_LastName.Focus();
                    return;
                }
                if (EditUserControl_ComboBox_Shift.SelectedIndex <= -1)
                {
                    MessageBox.Show("Please select a shift.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    EditUserControl_ComboBox_Shift.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(EditUserControl_TextBox_Pin.Text))
                {
                    MessageBox.Show("Pin is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    EditUserControl_TextBox_Pin.Focus();
                    return;
                }
                await Dao_User.UpdateUserAsync(
                    userName,
                    EditUserControl_TextBox_FirstName.Text + " " + EditUserControl_TextBox_LastName.Text,
                    EditUserControl_ComboBox_Shift.Text,
                    EditUserControl_TextBox_Pin.Text,
                    EditUserControl_TextBox_VisualUserName.Text,
                    EditUserControl_TextBox_VisualPassword.Text
                );
                var userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
                if (userRow != null && userRow.Table.Columns.Contains("ID"))
                {
                    int userId = Convert.ToInt32(userRow["ID"]);
                    int newRoleId = 3;
                    if (EditUserControl_RadioButton_Administrator.Checked)
                        newRoleId = 1;
                    else if (EditUserControl_RadioButton_ReadOnly.Checked)
                        newRoleId = 2;
                    await Dao_User.SetUserRoleAsync(userId, newRoleId, Environment.UserName, true);
                }
                UserEdited?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditUserControl_Button_Clear_Click(object? sender, EventArgs e)
        {
            EditUserControl_ComboBox_Users_SelectedIndexChanged(this, EventArgs.Empty);
        }
        #endregion
    }
}