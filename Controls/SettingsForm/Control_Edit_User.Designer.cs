using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Edit_User
    {
        #region Fields
        private System.ComponentModel.IContainer components = null;
        #endregion

        private ComboBox Control_Edit_User_ComboBox_Users;
        private Label Control_Edit_User_Label_SelectUser;
        private GroupBox Control_Edit_User_GroupBox_UserInfo;
        private Label Control_Edit_User_Label_FirstName;
        private TextBox Control_Edit_User_TextBox_FirstName;
        private Label Control_Edit_User_Label_LastName;
        private TextBox Control_Edit_User_TextBox_LastName;
        private Label Control_Edit_User_Label_UserName;
        private TextBox Control_Edit_User_TextBox_UserName;
        private Label Control_Edit_User_Label_Shift;
        private ComboBox Control_Edit_User_ComboBox_Shift;
        private Label Control_Edit_User_Label_Pin;
        private TextBox Control_Edit_User_TextBox_Pin;
        private GroupBox Control_Edit_User_GroupBox_VisualInfo;
        private CheckBox Control_Edit_User_CheckBox_VisualAccess;
        private Label Control_Edit_User_Label_VisualUserName;
        private TextBox Control_Edit_User_TextBox_VisualUserName;
        private Label Control_Edit_User_Label_VisualPassword;
        private TextBox Control_Edit_User_TextBox_VisualPassword;
        private GroupBox Control_Edit_User_GroupBox_UserPrivileges;
        private RadioButton Control_Edit_User_RadioButton_ReadOnly;
        private RadioButton Control_Edit_User_RadioButton_NormalUser;
        private RadioButton Control_Edit_User_RadioButton_Administrator;
        private Panel Control_Edit_User_Panel_LoginInfo;
        private Label Control_Edit_User_Label_UserNameWarning;
        private LinkLabel Control_Edit_User_LinkLabel_3;
        private LinkLabel Control_Edit_User_LinkLabel_2;
        private LinkLabel Control_Edit_User_LinkLabel_1;
        private Panel Control_Edit_User_Panel_Buttons;
        private Button Control_Edit_User_Button_Save;
        private Button Control_Edit_User_Button_Clear;
        private CheckBox Control_Edit_User_CheckBox_ViewHidePasswords;

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
            Control_Edit_User_ComboBox_Users = new ComboBox();
            Control_Edit_User_Label_SelectUser = new Label();
            Control_Edit_User_GroupBox_UserInfo = new GroupBox();
            Control_Edit_User_Panel_LoginInfo = new Panel();
            Control_Edit_User_Label_UserNameWarning = new Label();
            Control_Edit_User_TextBox_Pin = new TextBox();
            Control_Edit_User_Label_Pin = new Label();
            Control_Edit_User_TextBox_UserName = new TextBox();
            Control_Edit_User_Label_UserName = new Label();
            Control_Edit_User_Label_FirstName = new Label();
            Control_Edit_User_TextBox_FirstName = new TextBox();
            Control_Edit_User_Label_LastName = new Label();
            Control_Edit_User_TextBox_LastName = new TextBox();
            Control_Edit_User_Label_Shift = new Label();
            Control_Edit_User_ComboBox_Shift = new ComboBox();
            Control_Edit_User_GroupBox_VisualInfo = new GroupBox();
            Control_Edit_User_CheckBox_VisualAccess = new CheckBox();
            Control_Edit_User_Label_VisualUserName = new Label();
            Control_Edit_User_TextBox_VisualUserName = new TextBox();
            Control_Edit_User_Label_VisualPassword = new Label();
            Control_Edit_User_TextBox_VisualPassword = new TextBox();
            Control_Edit_User_GroupBox_UserPrivileges = new GroupBox();
            Control_Edit_User_LinkLabel_3 = new LinkLabel();
            Control_Edit_User_LinkLabel_2 = new LinkLabel();
            Control_Edit_User_LinkLabel_1 = new LinkLabel();
            Control_Edit_User_RadioButton_ReadOnly = new RadioButton();
            Control_Edit_User_RadioButton_NormalUser = new RadioButton();
            Control_Edit_User_RadioButton_Administrator = new RadioButton();
            Control_Edit_User_Panel_Buttons = new Panel();
            Control_Edit_User_CheckBox_ViewHidePasswords = new CheckBox();
            Control_Edit_User_Button_Clear = new Button();
            Control_Edit_User_Button_Save = new Button();
            Control_Edit_User_GroupBox_UserInfo.SuspendLayout();
            Control_Edit_User_Panel_LoginInfo.SuspendLayout();
            Control_Edit_User_GroupBox_VisualInfo.SuspendLayout();
            Control_Edit_User_GroupBox_UserPrivileges.SuspendLayout();
            Control_Edit_User_Panel_Buttons.SuspendLayout();
            SuspendLayout();
            // 
            // Control_Edit_User_ComboBox_Users
            // 
            Control_Edit_User_ComboBox_Users.AutoCompleteMode = AutoCompleteMode.Suggest;
            Control_Edit_User_ComboBox_Users.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_Edit_User_ComboBox_Users.Location = new Point(100, 10);
            Control_Edit_User_ComboBox_Users.MaxDropDownItems = 6;
            Control_Edit_User_ComboBox_Users.Name = "Control_Edit_User_ComboBox_Users";
            Control_Edit_User_ComboBox_Users.Size = new Size(250, 23);
            Control_Edit_User_ComboBox_Users.Sorted = true;
            Control_Edit_User_ComboBox_Users.TabIndex = 1;
            // 
            // Control_Edit_User_Label_SelectUser
            // 
            Control_Edit_User_Label_SelectUser.Location = new Point(10, 10);
            Control_Edit_User_Label_SelectUser.Name = "Control_Edit_User_Label_SelectUser";
            Control_Edit_User_Label_SelectUser.Size = new Size(80, 23);
            Control_Edit_User_Label_SelectUser.TabIndex = 0;
            Control_Edit_User_Label_SelectUser.Text = "Select User:";
            // 
            // Control_Edit_User_GroupBox_UserInfo
            // 
            Control_Edit_User_GroupBox_UserInfo.Controls.Add(Control_Edit_User_Panel_LoginInfo);
            Control_Edit_User_GroupBox_UserInfo.Controls.Add(Control_Edit_User_Label_FirstName);
            Control_Edit_User_GroupBox_UserInfo.Controls.Add(Control_Edit_User_TextBox_FirstName);
            Control_Edit_User_GroupBox_UserInfo.Controls.Add(Control_Edit_User_Label_LastName);
            Control_Edit_User_GroupBox_UserInfo.Controls.Add(Control_Edit_User_TextBox_LastName);
            Control_Edit_User_GroupBox_UserInfo.Controls.Add(Control_Edit_User_Label_Shift);
            Control_Edit_User_GroupBox_UserInfo.Controls.Add(Control_Edit_User_ComboBox_Shift);
            Control_Edit_User_GroupBox_UserInfo.Location = new Point(10, 40);
            Control_Edit_User_GroupBox_UserInfo.Name = "Control_Edit_User_GroupBox_UserInfo";
            Control_Edit_User_GroupBox_UserInfo.Size = new Size(430, 209);
            Control_Edit_User_GroupBox_UserInfo.TabIndex = 0;
            Control_Edit_User_GroupBox_UserInfo.TabStop = false;
            Control_Edit_User_GroupBox_UserInfo.Text = "User Information";
            // 
            // Control_Edit_User_Panel_LoginInfo
            // 
            Control_Edit_User_Panel_LoginInfo.Controls.Add(Control_Edit_User_Label_UserNameWarning);
            Control_Edit_User_Panel_LoginInfo.Controls.Add(Control_Edit_User_TextBox_Pin);
            Control_Edit_User_Panel_LoginInfo.Controls.Add(Control_Edit_User_Label_Pin);
            Control_Edit_User_Panel_LoginInfo.Controls.Add(Control_Edit_User_TextBox_UserName);
            Control_Edit_User_Panel_LoginInfo.Controls.Add(Control_Edit_User_Label_UserName);
            Control_Edit_User_Panel_LoginInfo.Location = new Point(6, 116);
            Control_Edit_User_Panel_LoginInfo.Name = "Control_Edit_User_Panel_LoginInfo";
            Control_Edit_User_Panel_LoginInfo.Size = new Size(414, 87);
            Control_Edit_User_Panel_LoginInfo.TabIndex = 10;
            // 
            // Control_Edit_User_Label_UserNameWarning
            // 
            Control_Edit_User_Label_UserNameWarning.ForeColor = Color.Red;
            Control_Edit_User_Label_UserNameWarning.Location = new Point(6, 58);
            Control_Edit_User_Label_UserNameWarning.Name = "Control_Edit_User_Label_UserNameWarning";
            Control_Edit_User_Label_UserNameWarning.Size = new Size(403, 23);
            Control_Edit_User_Label_UserNameWarning.TabIndex = 10;
            Control_Edit_User_Label_UserNameWarning.Text = "User Name cannot be changed.";
            Control_Edit_User_Label_UserNameWarning.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Control_Edit_User_TextBox_Pin
            // 
            Control_Edit_User_TextBox_Pin.Location = new Point(80, 32);
            Control_Edit_User_TextBox_Pin.Name = "Control_Edit_User_TextBox_Pin";
            Control_Edit_User_TextBox_Pin.Size = new Size(329, 23);
            Control_Edit_User_TextBox_Pin.TabIndex = 9;
            // 
            // Control_Edit_User_Label_Pin
            // 
            Control_Edit_User_Label_Pin.AutoSize = true;
            Control_Edit_User_Label_Pin.Location = new Point(47, 36);
            Control_Edit_User_Label_Pin.Name = "Control_Edit_User_Label_Pin";
            Control_Edit_User_Label_Pin.Size = new Size(27, 15);
            Control_Edit_User_Label_Pin.TabIndex = 8;
            Control_Edit_User_Label_Pin.Text = "Pin:";
            // 
            // Control_Edit_User_TextBox_UserName
            // 
            Control_Edit_User_TextBox_UserName.Location = new Point(81, 3);
            Control_Edit_User_TextBox_UserName.Name = "Control_Edit_User_TextBox_UserName";
            Control_Edit_User_TextBox_UserName.ReadOnly = true;
            Control_Edit_User_TextBox_UserName.Size = new Size(328, 23);
            Control_Edit_User_TextBox_UserName.TabIndex = 5;
            // 
            // Control_Edit_User_Label_UserName
            // 
            Control_Edit_User_Label_UserName.AutoSize = true;
            Control_Edit_User_Label_UserName.Location = new Point(6, 7);
            Control_Edit_User_Label_UserName.Name = "Control_Edit_User_Label_UserName";
            Control_Edit_User_Label_UserName.Size = new Size(68, 15);
            Control_Edit_User_Label_UserName.TabIndex = 4;
            Control_Edit_User_Label_UserName.Text = "User Name:";
            // 
            // Control_Edit_User_Label_FirstName
            // 
            Control_Edit_User_Label_FirstName.AutoSize = true;
            Control_Edit_User_Label_FirstName.Location = new Point(18, 32);
            Control_Edit_User_Label_FirstName.Name = "Control_Edit_User_Label_FirstName";
            Control_Edit_User_Label_FirstName.Size = new Size(67, 15);
            Control_Edit_User_Label_FirstName.TabIndex = 0;
            Control_Edit_User_Label_FirstName.Text = "First Name:";
            // 
            // Control_Edit_User_TextBox_FirstName
            // 
            Control_Edit_User_TextBox_FirstName.Location = new Point(90, 29);
            Control_Edit_User_TextBox_FirstName.Name = "Control_Edit_User_TextBox_FirstName";
            Control_Edit_User_TextBox_FirstName.Size = new Size(330, 23);
            Control_Edit_User_TextBox_FirstName.TabIndex = 1;
            // 
            // Control_Edit_User_Label_LastName
            // 
            Control_Edit_User_Label_LastName.AutoSize = true;
            Control_Edit_User_Label_LastName.Location = new Point(15, 61);
            Control_Edit_User_Label_LastName.Name = "Control_Edit_User_Label_LastName";
            Control_Edit_User_Label_LastName.Size = new Size(66, 15);
            Control_Edit_User_Label_LastName.TabIndex = 2;
            Control_Edit_User_Label_LastName.Text = "Last Name:";
            // 
            // Control_Edit_User_TextBox_LastName
            // 
            Control_Edit_User_TextBox_LastName.Location = new Point(90, 58);
            Control_Edit_User_TextBox_LastName.Name = "Control_Edit_User_TextBox_LastName";
            Control_Edit_User_TextBox_LastName.Size = new Size(330, 23);
            Control_Edit_User_TextBox_LastName.TabIndex = 3;
            // 
            // Control_Edit_User_Label_Shift
            // 
            Control_Edit_User_Label_Shift.AutoSize = true;
            Control_Edit_User_Label_Shift.Location = new Point(47, 90);
            Control_Edit_User_Label_Shift.Name = "Control_Edit_User_Label_Shift";
            Control_Edit_User_Label_Shift.Size = new Size(34, 15);
            Control_Edit_User_Label_Shift.TabIndex = 6;
            Control_Edit_User_Label_Shift.Text = "Shift:";
            // 
            // Control_Edit_User_ComboBox_Shift
            // 
            Control_Edit_User_ComboBox_Shift.DropDownStyle = ComboBoxStyle.DropDownList;
            Control_Edit_User_ComboBox_Shift.Location = new Point(90, 87);
            Control_Edit_User_ComboBox_Shift.Name = "Control_Edit_User_ComboBox_Shift";
            Control_Edit_User_ComboBox_Shift.Size = new Size(330, 23);
            Control_Edit_User_ComboBox_Shift.TabIndex = 7;
            // 
            // Control_Edit_User_GroupBox_VisualInfo
            // 
            Control_Edit_User_GroupBox_VisualInfo.Controls.Add(Control_Edit_User_CheckBox_VisualAccess);
            Control_Edit_User_GroupBox_VisualInfo.Controls.Add(Control_Edit_User_Label_VisualUserName);
            Control_Edit_User_GroupBox_VisualInfo.Controls.Add(Control_Edit_User_TextBox_VisualUserName);
            Control_Edit_User_GroupBox_VisualInfo.Controls.Add(Control_Edit_User_Label_VisualPassword);
            Control_Edit_User_GroupBox_VisualInfo.Controls.Add(Control_Edit_User_TextBox_VisualPassword);
            Control_Edit_User_GroupBox_VisualInfo.Location = new Point(10, 255);
            Control_Edit_User_GroupBox_VisualInfo.Name = "Control_Edit_User_GroupBox_VisualInfo";
            Control_Edit_User_GroupBox_VisualInfo.Size = new Size(430, 113);
            Control_Edit_User_GroupBox_VisualInfo.TabIndex = 10;
            Control_Edit_User_GroupBox_VisualInfo.TabStop = false;
            Control_Edit_User_GroupBox_VisualInfo.Text = "Infor VISUAL Information";
            // 
            // Control_Edit_User_CheckBox_VisualAccess
            // 
            Control_Edit_User_CheckBox_VisualAccess.AutoSize = true;
            Control_Edit_User_CheckBox_VisualAccess.Location = new Point(6, 22);
            Control_Edit_User_CheckBox_VisualAccess.Name = "Control_Edit_User_CheckBox_VisualAccess";
            Control_Edit_User_CheckBox_VisualAccess.Size = new Size(101, 19);
            Control_Edit_User_CheckBox_VisualAccess.TabIndex = 0;
            Control_Edit_User_CheckBox_VisualAccess.Text = "Visual Access?";
            Control_Edit_User_CheckBox_VisualAccess.UseVisualStyleBackColor = true;
            // 
            // Control_Edit_User_Label_VisualUserName
            // 
            Control_Edit_User_Label_VisualUserName.AutoSize = true;
            Control_Edit_User_Label_VisualUserName.Location = new Point(14, 54);
            Control_Edit_User_Label_VisualUserName.Name = "Control_Edit_User_Label_VisualUserName";
            Control_Edit_User_Label_VisualUserName.Size = new Size(68, 15);
            Control_Edit_User_Label_VisualUserName.TabIndex = 1;
            Control_Edit_User_Label_VisualUserName.Text = "User Name:";
            // 
            // Control_Edit_User_TextBox_VisualUserName
            // 
            Control_Edit_User_TextBox_VisualUserName.Location = new Point(88, 47);
            Control_Edit_User_TextBox_VisualUserName.Name = "Control_Edit_User_TextBox_VisualUserName";
            Control_Edit_User_TextBox_VisualUserName.Size = new Size(336, 23);
            Control_Edit_User_TextBox_VisualUserName.TabIndex = 2;
            // 
            // Control_Edit_User_Label_VisualPassword
            // 
            Control_Edit_User_Label_VisualPassword.AutoSize = true;
            Control_Edit_User_Label_VisualPassword.Location = new Point(22, 79);
            Control_Edit_User_Label_VisualPassword.Name = "Control_Edit_User_Label_VisualPassword";
            Control_Edit_User_Label_VisualPassword.Size = new Size(60, 15);
            Control_Edit_User_Label_VisualPassword.TabIndex = 3;
            Control_Edit_User_Label_VisualPassword.Text = "Password:";
            // 
            // Control_Edit_User_TextBox_VisualPassword
            // 
            Control_Edit_User_TextBox_VisualPassword.Location = new Point(88, 76);
            Control_Edit_User_TextBox_VisualPassword.Name = "Control_Edit_User_TextBox_VisualPassword";
            Control_Edit_User_TextBox_VisualPassword.Size = new Size(336, 23);
            Control_Edit_User_TextBox_VisualPassword.TabIndex = 4;
            // 
            // Control_Edit_User_GroupBox_UserPrivileges
            // 
            Control_Edit_User_GroupBox_UserPrivileges.Controls.Add(Control_Edit_User_LinkLabel_3);
            Control_Edit_User_GroupBox_UserPrivileges.Controls.Add(Control_Edit_User_LinkLabel_2);
            Control_Edit_User_GroupBox_UserPrivileges.Controls.Add(Control_Edit_User_LinkLabel_1);
            Control_Edit_User_GroupBox_UserPrivileges.Controls.Add(Control_Edit_User_RadioButton_ReadOnly);
            Control_Edit_User_GroupBox_UserPrivileges.Controls.Add(Control_Edit_User_RadioButton_NormalUser);
            Control_Edit_User_GroupBox_UserPrivileges.Controls.Add(Control_Edit_User_RadioButton_Administrator);
            Control_Edit_User_GroupBox_UserPrivileges.Location = new Point(446, 40);
            Control_Edit_User_GroupBox_UserPrivileges.Name = "Control_Edit_User_GroupBox_UserPrivileges";
            Control_Edit_User_GroupBox_UserPrivileges.Size = new Size(151, 328);
            Control_Edit_User_GroupBox_UserPrivileges.TabIndex = 11;
            Control_Edit_User_GroupBox_UserPrivileges.TabStop = false;
            Control_Edit_User_GroupBox_UserPrivileges.Text = "User Privileges";
            // 
            // Control_Edit_User_LinkLabel_3
            // 
            Control_Edit_User_LinkLabel_3.Location = new Point(6, 246);
            Control_Edit_User_LinkLabel_3.Name = "Control_Edit_User_LinkLabel_3";
            Control_Edit_User_LinkLabel_3.Size = new Size(139, 79);
            Control_Edit_User_LinkLabel_3.TabIndex = 5;
            Control_Edit_User_LinkLabel_3.TabStop = true;
            Control_Edit_User_LinkLabel_3.Text = "Can view all data but cannot make any changes.";
            Control_Edit_User_LinkLabel_3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Control_Edit_User_LinkLabel_2
            // 
            Control_Edit_User_LinkLabel_2.Location = new Point(6, 142);
            Control_Edit_User_LinkLabel_2.Name = "Control_Edit_User_LinkLabel_2";
            Control_Edit_User_LinkLabel_2.Size = new Size(139, 79);
            Control_Edit_User_LinkLabel_2.TabIndex = 4;
            Control_Edit_User_LinkLabel_2.TabStop = true;
            Control_Edit_User_LinkLabel_2.Text = "Has full access, including user management and all system settings.";
            Control_Edit_User_LinkLabel_2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Control_Edit_User_LinkLabel_1
            // 
            Control_Edit_User_LinkLabel_1.Location = new Point(6, 38);
            Control_Edit_User_LinkLabel_1.Name = "Control_Edit_User_LinkLabel_1";
            Control_Edit_User_LinkLabel_1.Size = new Size(139, 79);
            Control_Edit_User_LinkLabel_1.TabIndex = 3;
            Control_Edit_User_LinkLabel_1.TabStop = true;
            Control_Edit_User_LinkLabel_1.Text = "Can add and edit records needed for daily work, but cannot manage users or settings.";
            Control_Edit_User_LinkLabel_1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Control_Edit_User_RadioButton_ReadOnly
            // 
            Control_Edit_User_RadioButton_ReadOnly.AutoSize = true;
            Control_Edit_User_RadioButton_ReadOnly.Location = new Point(6, 224);
            Control_Edit_User_RadioButton_ReadOnly.Name = "Control_Edit_User_RadioButton_ReadOnly";
            Control_Edit_User_RadioButton_ReadOnly.Size = new Size(107, 19);
            Control_Edit_User_RadioButton_ReadOnly.TabIndex = 0;
            Control_Edit_User_RadioButton_ReadOnly.TabStop = true;
            Control_Edit_User_RadioButton_ReadOnly.Text = "Read-Only User";
            Control_Edit_User_RadioButton_ReadOnly.UseVisualStyleBackColor = true;
            // 
            // Control_Edit_User_RadioButton_NormalUser
            // 
            Control_Edit_User_RadioButton_NormalUser.AutoSize = true;
            Control_Edit_User_RadioButton_NormalUser.Location = new Point(6, 16);
            Control_Edit_User_RadioButton_NormalUser.Name = "Control_Edit_User_RadioButton_NormalUser";
            Control_Edit_User_RadioButton_NormalUser.Size = new Size(91, 19);
            Control_Edit_User_RadioButton_NormalUser.TabIndex = 1;
            Control_Edit_User_RadioButton_NormalUser.TabStop = true;
            Control_Edit_User_RadioButton_NormalUser.Text = "Normal User";
            Control_Edit_User_RadioButton_NormalUser.UseVisualStyleBackColor = true;
            // 
            // Control_Edit_User_RadioButton_Administrator
            // 
            Control_Edit_User_RadioButton_Administrator.AutoSize = true;
            Control_Edit_User_RadioButton_Administrator.Location = new Point(6, 120);
            Control_Edit_User_RadioButton_Administrator.Name = "Control_Edit_User_RadioButton_Administrator";
            Control_Edit_User_RadioButton_Administrator.Size = new Size(98, 19);
            Control_Edit_User_RadioButton_Administrator.TabIndex = 2;
            Control_Edit_User_RadioButton_Administrator.TabStop = true;
            Control_Edit_User_RadioButton_Administrator.Text = "Administrator";
            Control_Edit_User_RadioButton_Administrator.UseVisualStyleBackColor = true;
            // 
            // Control_Edit_User_Panel_Buttons
            // 
            Control_Edit_User_Panel_Buttons.Controls.Add(Control_Edit_User_CheckBox_ViewHidePasswords);
            Control_Edit_User_Panel_Buttons.Controls.Add(Control_Edit_User_Button_Clear);
            Control_Edit_User_Panel_Buttons.Controls.Add(Control_Edit_User_Button_Save);
            Control_Edit_User_Panel_Buttons.Location = new Point(10, 374);
            Control_Edit_User_Panel_Buttons.Name = "Control_Edit_User_Panel_Buttons";
            Control_Edit_User_Panel_Buttons.Size = new Size(581, 53);
            Control_Edit_User_Panel_Buttons.TabIndex = 12;
            // 
            // Control_Edit_User_CheckBox_ViewHidePasswords
            // 
            Control_Edit_User_CheckBox_ViewHidePasswords.AutoSize = true;
            Control_Edit_User_CheckBox_ViewHidePasswords.Location = new Point(12, 18);
            Control_Edit_User_CheckBox_ViewHidePasswords.Name = "Control_Edit_User_CheckBox_ViewHidePasswords";
            Control_Edit_User_CheckBox_ViewHidePasswords.Size = new Size(141, 19);
            Control_Edit_User_CheckBox_ViewHidePasswords.TabIndex = 2;
            Control_Edit_User_CheckBox_ViewHidePasswords.Text = "Show Password Fields";
            Control_Edit_User_CheckBox_ViewHidePasswords.UseVisualStyleBackColor = true;
            // 
            // Control_Edit_User_Button_Clear
            // 
            Control_Edit_User_Button_Clear.Location = new Point(409, 16);
            Control_Edit_User_Button_Clear.Name = "Control_Edit_User_Button_Clear";
            Control_Edit_User_Button_Clear.Size = new Size(75, 23);
            Control_Edit_User_Button_Clear.TabIndex = 1;
            Control_Edit_User_Button_Clear.Text = "Clear";
            Control_Edit_User_Button_Clear.UseVisualStyleBackColor = true;
            // 
            // Control_Edit_User_Button_Save
            // 
            Control_Edit_User_Button_Save.Location = new Point(490, 16);
            Control_Edit_User_Button_Save.Name = "Control_Edit_User_Button_Save";
            Control_Edit_User_Button_Save.Size = new Size(75, 23);
            Control_Edit_User_Button_Save.TabIndex = 0;
            Control_Edit_User_Button_Save.Text = "Save";
            Control_Edit_User_Button_Save.UseVisualStyleBackColor = true;
            // 
            // Control_Edit_User
            // 
            Controls.Add(Control_Edit_User_Label_SelectUser);
            Controls.Add(Control_Edit_User_ComboBox_Users);
            Controls.Add(Control_Edit_User_Panel_Buttons);
            Controls.Add(Control_Edit_User_GroupBox_UserPrivileges);
            Controls.Add(Control_Edit_User_GroupBox_VisualInfo);
            Controls.Add(Control_Edit_User_GroupBox_UserInfo);
            Name = "Control_Edit_User";
            Size = new Size(600, 440);
            Control_Edit_User_GroupBox_UserInfo.ResumeLayout(false);
            Control_Edit_User_GroupBox_UserInfo.PerformLayout();
            Control_Edit_User_Panel_LoginInfo.ResumeLayout(false);
            Control_Edit_User_Panel_LoginInfo.PerformLayout();
            Control_Edit_User_GroupBox_VisualInfo.ResumeLayout(false);
            Control_Edit_User_GroupBox_VisualInfo.PerformLayout();
            Control_Edit_User_GroupBox_UserPrivileges.ResumeLayout(false);
            Control_Edit_User_GroupBox_UserPrivileges.PerformLayout();
            Control_Edit_User_Panel_Buttons.ResumeLayout(false);
            Control_Edit_User_Panel_Buttons.PerformLayout();
            ResumeLayout(false);
        }
        #endregion
    }
}
