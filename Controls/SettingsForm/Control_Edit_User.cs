using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Edit_User : UserControl
    {
        #region Fields

        #region Events

        public event EventHandler? UserEdited;

        #endregion

        #region Constructors

        public Control_Edit_User()
        {
            InitializeComponent();
            Control_Edit_User_TextBox_FirstName.KeyPress += Control_Edit_User_TextBox_NoSpaces_KeyPress;
            Control_Edit_User_TextBox_LastName.KeyPress += Control_Edit_User_TextBox_NoSpaces_KeyPress;
            Control_Edit_User_TextBox_Pin.KeyPress += Control_Edit_User_TextBox_NoSpaces_KeyPress;
            Control_Edit_User_TextBox_VisualUserName.KeyPress += Control_Edit_User_TextBox_NoSpaces_KeyPress;
            Control_Edit_User_TextBox_VisualPassword.KeyPress += Control_Edit_User_TextBox_NoSpaces_KeyPress;
            Control_Edit_User_TextBox_Pin.UseSystemPasswordChar = true;
            Control_Edit_User_TextBox_VisualPassword.UseSystemPasswordChar = true;
            Control_Edit_User_TextBox_VisualUserName.Enabled = false;
            Control_Edit_User_TextBox_VisualPassword.Enabled = false;
            Control_Edit_User_CheckBox_VisualAccess.CheckedChanged +=
                Control_Edit_User_CheckBox_VisualAccess_CheckedChanged;
            Control_Edit_User_CheckBox_ViewHidePasswords.CheckedChanged +=
                Control_Edit_User_CheckBox_ViewHidePasswords_CheckedChanged;
            Control_Edit_User_ComboBox_Users.SelectedIndexChanged +=
                Control_Edit_User_ComboBox_Users_SelectedIndexChanged;
            Control_Edit_User_Button_Save.Click += Control_Edit_User_Button_Save_Click;
            Control_Edit_User_Button_Clear.Click += Control_Edit_User_Button_Clear_Click;
        }

        #endregion

        #region Methods

        #endregion

        #region Initialization

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Control_Edit_User_ComboBox_Shift.Items.Clear();
            Control_Edit_User_ComboBox_Shift.Items.AddRange(new object[] { "First", "Second", "Third", "Weekend" });
            LoadUsersAsync();
        }

        private async void LoadUsersAsync()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(Control_Edit_User_ComboBox_Users);
            }
            catch (Exception e)
            {
                Logging.LoggingUtility.LogApplicationError(e);
                MessageBox.Show($"Error loading users: {e.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        private void Control_Edit_User_TextBox_NoSpaces_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void Control_Edit_User_CheckBox_ViewHidePasswords_CheckedChanged(object? sender, EventArgs e)
        {
            bool show = Control_Edit_User_CheckBox_ViewHidePasswords.Checked;
            Control_Edit_User_TextBox_Pin.UseSystemPasswordChar = !show;
            Control_Edit_User_TextBox_VisualPassword.UseSystemPasswordChar = !show;
        }

        private void Control_Edit_User_CheckBox_VisualAccess_CheckedChanged(object? sender, EventArgs e)
        {
            bool enabled = Control_Edit_User_CheckBox_VisualAccess.Checked;
            Control_Edit_User_TextBox_VisualUserName.Enabled = enabled;
            Control_Edit_User_TextBox_VisualPassword.Enabled = enabled;
            if (!enabled)
            {
                Control_Edit_User_TextBox_VisualUserName.Clear();
                Control_Edit_User_TextBox_VisualPassword.Clear();
            }
        }

        private async void Control_Edit_User_ComboBox_Users_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (Control_Edit_User_ComboBox_Users.SelectedItem is string userName)
            {
                DataRow? userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
                if (userRow != null)
                {
                    string[] names = (userRow["Full Name"]?.ToString() ?? "").Split(' ');
                    Control_Edit_User_TextBox_FirstName.Text = names.Length > 0 ? names[0] : "";
                    Control_Edit_User_TextBox_LastName.Text = names.Length > 1 ? string.Join(" ", names.Skip(1)) : "";
                    Control_Edit_User_TextBox_UserName.Text = userRow["User"]?.ToString() ?? "";
                    Control_Edit_User_ComboBox_Shift.SelectedItem = userRow["Shift"]?.ToString() ?? "First";
                    Control_Edit_User_TextBox_Pin.Text = userRow["Pin"]?.ToString() ?? "";
                    Control_Edit_User_CheckBox_VisualAccess.Checked =
                        !string.IsNullOrWhiteSpace(userRow["VisualUserName"]?.ToString());
                    Control_Edit_User_TextBox_VisualUserName.Text = userRow["VisualUserName"]?.ToString() ?? "";
                    Control_Edit_User_TextBox_VisualPassword.Text = userRow["VisualPassword"]?.ToString() ?? "";
                    int userId = Convert.ToInt32(userRow["ID"]);
                    int roleId = await Dao_User.GetUserRoleIdAsync(userId);
                    Control_Edit_User_RadioButton_NormalUser.Checked = roleId == 3;
                    Control_Edit_User_RadioButton_Administrator.Checked = roleId == 1;
                    Control_Edit_User_RadioButton_ReadOnly.Checked = roleId == 2;
                }
            }
        }

        private async void Control_Edit_User_Button_Save_Click(object? sender, EventArgs e)
        {
            try
            {
                if (Control_Edit_User_ComboBox_Users.SelectedItem is not string userName)
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Edit_User_TextBox_FirstName.Text))
                {
                    MessageBox.Show("First name is required.", "Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Edit_User_TextBox_FirstName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Edit_User_TextBox_LastName.Text))
                {
                    MessageBox.Show("Last name is required.", "Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Edit_User_TextBox_LastName.Focus();
                    return;
                }

                if (Control_Edit_User_ComboBox_Shift.SelectedIndex <= -1)
                {
                    MessageBox.Show("Please select a shift.", "Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Edit_User_ComboBox_Shift.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Control_Edit_User_TextBox_Pin.Text))
                {
                    MessageBox.Show("Pin is required.", "Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Edit_User_TextBox_Pin.Focus();
                    return;
                }

                await Dao_User.UpdateUserAsync(
                    userName,
                    Control_Edit_User_TextBox_FirstName.Text + " " + Control_Edit_User_TextBox_LastName.Text,
                    Control_Edit_User_ComboBox_Shift.Text,
                    Control_Edit_User_TextBox_Pin.Text,
                    Control_Edit_User_TextBox_VisualUserName.Text,
                    Control_Edit_User_TextBox_VisualPassword.Text
                );
                DataRow? userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
                if (userRow != null && userRow.Table.Columns.Contains("ID"))
                {
                    int userId = Convert.ToInt32(userRow["ID"]);
                    int newRoleId = 3;
                    if (Control_Edit_User_RadioButton_Administrator.Checked)
                    {
                        newRoleId = 1;
                    }
                    else if (Control_Edit_User_RadioButton_ReadOnly.Checked)
                    {
                        newRoleId = 2;
                    }

                    await Dao_User.SetUserRoleAsync(userId, newRoleId, Environment.UserName, true);
                }

                UserEdited?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void Control_Edit_User_Button_Clear_Click(object? sender, EventArgs e) =>
            Control_Edit_User_ComboBox_Users_SelectedIndexChanged(this, EventArgs.Empty);

        #endregion
    }

    #endregion
}
