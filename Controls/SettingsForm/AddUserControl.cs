

using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class AddUserControl : UserControl
    {
    #region Fields
    

        #region Events
        public event EventHandler? UserAdded;
    
    #endregion
    
    #region Constructors
    
        #endregion

        #region Constructors
        public AddUserControl()
        {
            InitializeComponent();
            AddUserControl_RadioButton_NormalUser.Checked = true;
            AddUserControl_TextBox_FirstName.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_LastName.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_UserName.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_Pin.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_VisualUserName.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_VisualPassword.KeyPress += TextBox_NoSpaces_KeyPress;
            AddUserControl_TextBox_Pin.UseSystemPasswordChar = true;
            AddUserControl_TextBox_VisualPassword.UseSystemPasswordChar = true;
            AddUserControl_TextBox_VisualUserName.Enabled = false;
            AddUserControl_TextBox_VisualPassword.Enabled = false;
            AddUserControl_CheckBox_VisualAccess.CheckedChanged += AddUserControl_CheckBox_VisualAccess_CheckedChanged;
            AddUserControl_CheckBox_ViewHidePasswords.CheckedChanged += AddUserControl_CheckBox_ViewHidePasswords_CheckedChanged;
        }
    
    #endregion
    
    #region Methods
    
        #endregion

        #region Initialization
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AddUserControl_ComboBox_Shift.Items.Clear();
            AddUserControl_ComboBox_Shift.Items.AddRange(new object[] { "First", "Second", "Third", "Weekend" });
            AddUserControl_ComboBox_Shift.SelectedIndex = 0;
        }
        #endregion

        #region Event Handlers
        private void TextBox_NoSpaces_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                e.Handled = true;
        }

        private void AddUserControl_CheckBox_ViewHidePasswords_CheckedChanged(object? sender, EventArgs e)
        {
            var show = AddUserControl_CheckBox_ViewHidePasswords.Checked;
            AddUserControl_TextBox_Pin.UseSystemPasswordChar = !show;
            AddUserControl_TextBox_VisualPassword.UseSystemPasswordChar = !show;
        }

        private void AddUserControl_CheckBox_VisualAccess_CheckedChanged(object? sender, EventArgs e)
        {
            var enabled = AddUserControl_CheckBox_VisualAccess.Checked;
            AddUserControl_TextBox_VisualUserName.Enabled = enabled;
            AddUserControl_TextBox_VisualPassword.Enabled = enabled;
            if (!enabled)
            {
                AddUserControl_TextBox_VisualUserName.Clear();
                AddUserControl_TextBox_VisualPassword.Clear();
            }
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AddUserControl_TextBox_FirstName.Text))
                {
                    MessageBox.Show("First name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_TextBox_FirstName.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(AddUserControl_TextBox_LastName.Text))
                {
                    MessageBox.Show("Last name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_TextBox_LastName.Focus();
                    return;
                }
                if (AddUserControl_ComboBox_Shift.SelectedIndex <= -1)
                {
                    MessageBox.Show("Please select a shift.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_ComboBox_Shift.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(AddUserControl_TextBox_UserName.Text))
                {
                    MessageBox.Show("User name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_TextBox_UserName.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(AddUserControl_TextBox_Pin.Text))
                {
                    MessageBox.Show("Pin is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddUserControl_TextBox_Pin.Focus();
                    return;
                }
                if (await Dao_User.UserExistsAsync(AddUserControl_TextBox_UserName.Text.ToUpper()))
                {
                    MessageBox.Show("User already exists.", "Duplicate User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                await Dao_User.InsertUserAsync(
                    AddUserControl_TextBox_UserName.Text.ToUpper(),
                    AddUserControl_TextBox_FirstName.Text + " " + AddUserControl_TextBox_LastName.Text,
                    AddUserControl_ComboBox_Shift.Text,
                    false,
                    AddUserControl_TextBox_Pin.Text,
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty,
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
                var userRow = await Dao_User.GetUserByUsernameAsync(AddUserControl_TextBox_UserName.Text.ToUpper(), true);
                if (userRow == null || !userRow.Table.Columns.Contains("ID"))
                {
                    MessageBox.Show("Could not retrieve new user ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var userId = Convert.ToInt32(userRow["ID"]);
                var roleId = 3;
                if (AddUserControl_RadioButton_Administrator.Checked)
                    roleId = 1;
                else if (AddUserControl_RadioButton_ReadOnly.Checked)
                    roleId = 2;
                await Dao_User.AddUserRoleAsync(userId, roleId, Environment.UserName, true);
                ClearForm();
                AddUserControl_TextBox_UserName.Clear();
                UserAdded?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        #endregion

        #region Methods
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
            AddUserControl_RadioButton_NormalUser.Checked = true;
            AddUserControl_RadioButton_Administrator.Checked = false;
            AddUserControl_TextBox_FirstName.Focus();
            AddUserControl_TextBox_VisualUserName.Enabled = false;
            AddUserControl_TextBox_VisualPassword.Enabled = false;
        }
        #endregion
    }

    
    #endregion
}