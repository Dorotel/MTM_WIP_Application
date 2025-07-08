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
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Example: Set default values or load shift options
            AddUserControl_ComboBox_Shift.Items.Clear();
            AddUserControl_ComboBox_Shift.Items.AddRange(new object[] { "First", "Second", "Third", "Weekend" });
            AddUserControl_ComboBox_Shift.SelectedIndex = 0;
        }

        private void shiftComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Example: Handle shift selection change if needed
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
                if (string.IsNullOrWhiteSpace(AddUserControl_TextBox_Pin.Text))
                {
                    MessageBox.Show(@"Pin is required.", @"Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_TextBox_Pin.Focus();
                    return;
                }

                // Example: Check if user already exists (pseudo-code, replace with actual data access)
                if (await Dao_User.UserExistsAsync(AddUserControl_TextBox_UserName.Text.ToUpper()))
                {
                    MessageBox.Show(@"User already exists.", @"Duplicate User",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Example: Insert the user (pseudo-code, replace with actual data access)
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

                // Clear the form
                ClearForm();

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
            AddUserControl_RadioButton_NormalUser.Checked = false;
            AddUserControl_RadioButton_Administrator.Checked = false;
            AddUserControl_TextBox_FirstName.Focus();
        }

        private void AddUserControl_TextBox_VisualUserName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}