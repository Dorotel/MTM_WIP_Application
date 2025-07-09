using MTM_Inventory_Application.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class RemoveUserControl : UserControl
    {
        public event EventHandler? UserRemoved;

        public RemoveUserControl()
        {
            InitializeComponent();
            RemoveUserControl_Button_Remove.Click += RemoveUserControl_Button_Remove_Click;
            RemoveUserControl_ComboBox_Users.SelectedIndexChanged += RemoveUserControl_ComboBox_Users_SelectedIndexChanged;
            LoadUsersAsync();
        }

        private async void LoadUsersAsync()
        {
            RemoveUserControl_ComboBox_Users.Items.Clear();
            var users = await Dao_User.GetAllUsersAsync();
            foreach (DataRow row in users.Rows)
            {
                RemoveUserControl_ComboBox_Users.Items.Add(row["User"].ToString());
            }
            if (RemoveUserControl_ComboBox_Users.Items.Count > 0)
                RemoveUserControl_ComboBox_Users.SelectedIndex = 0;
        }

        private async void RemoveUserControl_ComboBox_Users_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (RemoveUserControl_ComboBox_Users.SelectedItem is not string userName)
            {
                RemoveUserControl_Label_FullName.Text = "";
                RemoveUserControl_Label_Role.Text = "";
                RemoveUserControl_Label_Shift.Text = "";
                return;
            }

            var userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
            if (userRow != null)
            {
                RemoveUserControl_Label_FullName.Text = userRow["Full Name"]?.ToString() ?? "";
                RemoveUserControl_Label_Shift.Text = userRow["Shift"]?.ToString() ?? "";

                // Get role
                if (userRow.Table.Columns.Contains("ID") && int.TryParse(userRow["ID"]?.ToString(), out int userId))
                {
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
        }

        private async void RemoveUserControl_Button_Remove_Click(object? sender, EventArgs e)
        {
            if (RemoveUserControl_ComboBox_Users.SelectedItem is not string userName)
                return;

            var confirm = MessageBox.Show($"Are you sure you want to remove user '{userName}'?",
                "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                // Get user ID and role ID
                var userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
                if (userRow != null && userRow.Table.Columns.Contains("ID"))
                {
                    int userId = Convert.ToInt32(userRow["ID"]);
                    int roleId = await Dao_User.GetUserRoleIdAsync(userId);

                    // Remove user role
                    await Dao_User.RemoveUserRoleAsync(userId, roleId);

                    // Remove user (calls usr_users_Delete_User)
                    await Dao_User.DeleteUserAsync(userName);

                    UserRemoved?.Invoke(this, EventArgs.Empty);

                    MessageBox.Show(@"User removed successfully!", @"Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUsersAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error removing user: {ex.Message}", @"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}