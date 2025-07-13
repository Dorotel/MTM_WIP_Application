using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Remove_User
    {
        #region Fields
        

        private System.ComponentModel.IContainer components = null;

        #endregion

        private Label RemoveUserControl_Label_SelectUser;
        private ComboBox RemoveUserControl_ComboBox_Users;
        private Button RemoveUserControl_Button_Remove;

        private GroupBox RemoveUserControl_GroupBox_UserInfo;
        private Label RemoveUserControl_Label_FullNameTitle;
        private Label RemoveUserControl_Label_FullName;
        private Label RemoveUserControl_Label_RoleTitle;
        private Label RemoveUserControl_Label_Role;
        private Label RemoveUserControl_Label_ShiftTitle;
        private Label RemoveUserControl_Label_Shift;
        

        
        #region Methods


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
        #region Component Designer generated code

        private void InitializeComponent()
        {
            RemoveUserControl_Label_SelectUser = new Label();
            RemoveUserControl_ComboBox_Users = new ComboBox();
            RemoveUserControl_Button_Remove = new Button();

            RemoveUserControl_GroupBox_UserInfo = new GroupBox();
            RemoveUserControl_Label_FullNameTitle = new Label();
            RemoveUserControl_Label_FullName = new Label();
            RemoveUserControl_Label_RoleTitle = new Label();
            RemoveUserControl_Label_Role = new Label();
            RemoveUserControl_Label_ShiftTitle = new Label();
            RemoveUserControl_Label_Shift = new Label();

            RemoveUserControl_Label_SelectUser.Text = "Select User:";
            RemoveUserControl_Label_SelectUser.Location = new System.Drawing.Point(10, 20);
            RemoveUserControl_Label_SelectUser.Size = new System.Drawing.Size(80, 23);

            RemoveUserControl_ComboBox_Users.Location = new System.Drawing.Point(100, 20);
            RemoveUserControl_ComboBox_Users.Size = new System.Drawing.Size(250, 23);
            RemoveUserControl_ComboBox_Users.DropDownStyle = ComboBoxStyle.DropDownList;

            RemoveUserControl_Button_Remove.Text = "Remove User";
            RemoveUserControl_Button_Remove.Location = new System.Drawing.Point(370, 20);
            RemoveUserControl_Button_Remove.Size = new System.Drawing.Size(120, 23);

            RemoveUserControl_GroupBox_UserInfo.Text = "User Information";
            RemoveUserControl_GroupBox_UserInfo.Location = new System.Drawing.Point(10, 55);
            RemoveUserControl_GroupBox_UserInfo.Size = new System.Drawing.Size(480, 90);

            RemoveUserControl_Label_FullNameTitle.Text = "Full Name:";
            RemoveUserControl_Label_FullNameTitle.Location = new System.Drawing.Point(15, 25);
            RemoveUserControl_Label_FullNameTitle.Size = new System.Drawing.Size(70, 15);

            RemoveUserControl_Label_FullName.Text = "";
            RemoveUserControl_Label_FullName.Location = new System.Drawing.Point(90, 25);
            RemoveUserControl_Label_FullName.Size = new System.Drawing.Size(350, 15);

            RemoveUserControl_Label_RoleTitle.Text = "Role:";
            RemoveUserControl_Label_RoleTitle.Location = new System.Drawing.Point(15, 45);
            RemoveUserControl_Label_RoleTitle.Size = new System.Drawing.Size(70, 15);

            RemoveUserControl_Label_Role.Text = "";
            RemoveUserControl_Label_Role.Location = new System.Drawing.Point(90, 45);
            RemoveUserControl_Label_Role.Size = new System.Drawing.Size(350, 15);

            RemoveUserControl_Label_ShiftTitle.Text = "Shift:";
            RemoveUserControl_Label_ShiftTitle.Location = new System.Drawing.Point(15, 65);
            RemoveUserControl_Label_ShiftTitle.Size = new System.Drawing.Size(70, 15);

            RemoveUserControl_Label_Shift.Text = "";
            RemoveUserControl_Label_Shift.Location = new System.Drawing.Point(90, 65);
            RemoveUserControl_Label_Shift.Size = new System.Drawing.Size(350, 15);

            RemoveUserControl_GroupBox_UserInfo.Controls.Add(RemoveUserControl_Label_FullNameTitle);
            RemoveUserControl_GroupBox_UserInfo.Controls.Add(RemoveUserControl_Label_FullName);
            RemoveUserControl_GroupBox_UserInfo.Controls.Add(RemoveUserControl_Label_RoleTitle);
            RemoveUserControl_GroupBox_UserInfo.Controls.Add(RemoveUserControl_Label_Role);
            RemoveUserControl_GroupBox_UserInfo.Controls.Add(RemoveUserControl_Label_ShiftTitle);
            RemoveUserControl_GroupBox_UserInfo.Controls.Add(RemoveUserControl_Label_Shift);

            Controls.Add(RemoveUserControl_Label_SelectUser);
            Controls.Add(RemoveUserControl_ComboBox_Users);
            Controls.Add(RemoveUserControl_Button_Remove);
            Controls.Add(RemoveUserControl_GroupBox_UserInfo);

            Name = "Control_Remove_User";
            Size = new System.Drawing.Size(520, 160);
        }
    }

        
        #endregion
    }
