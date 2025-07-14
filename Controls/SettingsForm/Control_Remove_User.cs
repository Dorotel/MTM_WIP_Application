using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Remove_User : UserControl
    {
        #region Events

        public event EventHandler? UserRemoved;

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

        #region Initialization

        private async void LoadUsersAsync()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(RemoveUserControl_ComboBox_Users);
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

        private async void RemoveUserControl_ComboBox_Users_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (RemoveUserControl_ComboBox_Users.Text is not { } userName)
            {
                RemoveUserControl_Label_FullName.Text = "";
                RemoveUserControl_Label_Role.Text = "";
                RemoveUserControl_Label_Shift.Text = "";
                return;
            }

            DataRow? userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
            if (userRow != null)
            {
                RemoveUserControl_Label_FullName.Text = userRow["Full Name"]?.ToString() ?? "";
                RemoveUserControl_Label_Shift.Text = userRow["Shift"]?.ToString() ?? "";
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
            if (RemoveUserControl_ComboBox_Users.Text is not { } userName)
            {
                return;
            }

            DialogResult confirm = MessageBox.Show($"Are you sure you want to remove user '{userName}'?",
                "Confirm Remove",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                DataRow? userRow = await Dao_User.GetUserByUsernameAsync(userName, true);
                if (userRow != null && userRow.Table.Columns.Contains("ID"))
                {
                    int userId = Convert.ToInt32(userRow["ID"]);
                    int roleId = await Dao_User.GetUserRoleIdAsync(userId);
                    await Dao_User.RemoveUserRoleAsync(userId, roleId);
                    await Dao_User.DeleteUserAsync(userName);
                    UserRemoved?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("User removed successfully!", "Success", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadUsersAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing user: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
