using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class AddUserControl : UserControl
    {
        public event EventHandler? UserAdded;

        public AddUserControl()
        {
            InitializeComponent();
            AddUserControl_RadioButton_NormalUser.Checked = true; // Set Normal User as default

            // Prevent spaces in all textboxes
            AddUserControl_TextBox_FirstName.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_LastName.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_UserName.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_Pin.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_VisualUserName.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_VisualPassword.KeyPress += TextBox_NoSpaces_KeyPress;

            // PIN and Visual password textboxes as password fields
            AddUserControl_TextBox_Pin.UseSystemPasswordChar = true;
            AddUserControl_TextBox_VisualPassword.UseSystemPasswordChar = true;

            // Visual credentials start disabled
            AddUserControl_TextBox_VisualUserName.Enabled = false;
            AddUserControl_TextBox_VisualPassword.Enabled = false;

            // Visual access checkbox event
            AddUserControl_CheckBox_VisualAccess.CheckedChanged += AddUserControl_CheckBox_VisualAccess_CheckedChanged;

            // Show/hide both password fields with one checkbox
            AddUserControl_CheckBox_ViewHidePasswords.CheckedChanged += AddUserControl_CheckBox_ViewHidePasswords_CheckedChanged;
        }

        // Prevent spaces in any textbox
        private void TextBox_NoSpaces_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                e.Handled = true;
        }

        // Toggle both password fields' visibility
        private void AddUserControl_CheckBox_ViewHidePasswords_CheckedChanged(object? sender, EventArgs e)
        {
            bool show = AddUserControl_CheckBox_ViewHidePasswords.Checked;
            AddUserControl_TextBox_Pin.UseSystemPasswordChar = !show;
            AddUserControl_TextBox_VisualPassword.UseSystemPasswordChar = !show;
        }

        // Enable/disable visual credentials
        private void AddUserControl_CheckBox_VisualAccess_CheckedChanged(object? sender, EventArgs e)
        {
            bool enabled = AddUserControl_CheckBox_VisualAccess.Checked;
            AddUserControl_TextBox_VisualUserName.Enabled = enabled;
            AddUserControl_TextBox_VisualPassword.Enabled = enabled;
            // Optionally clear when disabling
            if (!enabled)
            {
                AddUserControl_TextBox_VisualUserName.Clear();
                AddUserControl_TextBox_VisualPassword.Clear();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AddUserControl_ComboBox_Shift.Items.Clear();
            AddUserControl_ComboBox_Shift.Items.AddRange(new object[] { "First", "Second", "Third", "Weekend" });
            AddUserControl_ComboBox_Shift.SelectedIndex = 0;
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(AddUserControl_TextBox_FirstName.Text))
                {
                    MessageBox.Show(@"First name is required.", @"Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_TextBox_FirstName.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(AddUserControl_TextBox_LastName.Text))
                {
                    MessageBox.Show(@"Last name is required.", @"Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_TextBox_LastName.Focus();
                    return;
                }
                if (AddUserControl_ComboBox_Shift.SelectedIndex <= -1)
                {
                    MessageBox.Show(@"Please select a shift.", @"Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_ComboBox_Shift.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(AddUserControl_TextBox_UserName.Text))
                {
                    MessageBox.Show(@"User name is required.", @"Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_TextBox_UserName.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(AddUserControl_TextBox_Pin.Text))
                {
                    MessageBox.Show(@"Pin is required.", @"Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_TextBox_Pin.Focus();
                    return;
                }

                // Check if user already exists
                if (await Dao_User.UserExistsAsync(AddUserControl_TextBox_UserName.Text.ToUpper()))
                {
                    MessageBox.Show(@"User already exists.", @"Duplicate User",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert the user
                await Dao_User.InsertUserAsync(
                    AddUserControl_TextBox_UserName.Text.ToUpper(),
                    AddUserControl_TextBox_FirstName.Text + " " + AddUserControl_TextBox_LastName.Text,
                    AddUserControl_ComboBox_Shift.Text,
                    false,
                    AddUserControl_TextBox_Pin.Text,
                    Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    "false",
                    "Default",
                    9,
                    AddUserControl_TextBox_VisualUserName.Text,
                    AddUserControl_TextBox_VisualPassword.Text,
                    Model_Users.WipServerAddress,
                    Model_Users.Database,
                    Model_Users.WipServerPort,
                    true
                );

                // Get the new user's ID (assuming username is unique)
                var userRow = await Dao_User.GetUserByUsernameAsync(AddUserControl_TextBox_UserName.Text.ToUpper(), true);
                if (userRow == null || !userRow.Table.Columns.Contains("ID"))
                {
                    MessageBox.Show(@"Could not retrieve new user ID.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int userId = Convert.ToInt32(userRow["ID"]);

                // Determine RoleID based on selected radio button
                int roleId = 3; // Default to User
                if (AddUserControl_RadioButton_Administrator.Checked)
                    roleId = 1;
                else if (AddUserControl_RadioButton_ReadOnly.Checked)
                    roleId = 2;

                // Assign role
                await Dao_User.AddUserRoleAsync(userId, roleId, Environment.UserName, true);

                // Clear the form
                ClearForm();

                // Clear User Name textbox after save
                AddUserControl_TextBox_UserName.Clear();

                // Notify parent
                UserAdded?.Invoke(this, EventArgs.Empty);

                MessageBox.Show(@"User added successfully!", @"Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error adding user: {ex.Message}", @"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            AddUserControl_TextBox_FirstName.Clear();
            AddUserControl_TextBox_LastName.Clear();
            AddUserControl_ComboBox_Shift.SelectedIndex = 0;
            AddUserControl_TextBox_Pin.Clear();
            AddUserControl_CheckBox_VisualAccess.Checked = false;
            AddUserControl_TextBox_VisualUserName.Clear();
            AddUserControl_TextBox_VisualPassword.Clear();
            AddUserControl_RadioButton_ReadOnly.Checked = false;
            AddUserControl_RadioButton_NormalUser.Checked = true; // Set Normal User as default
            AddUserControl_RadioButton_Administrator.Checked = false;
            AddUserControl_TextBox_FirstName.Focus();

            // Visual credentials start disabled
            AddUserControl_TextBox_VisualUserName.Enabled = false;
            AddUserControl_TextBox_VisualPassword.Enabled = false;
        }
    }
}