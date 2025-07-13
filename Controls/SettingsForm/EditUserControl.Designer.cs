using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class EditUserControl
    {
        #region Fields
        

        private System.ComponentModel.IContainer components = null;

        #endregion

        private ComboBox EditUserControl_ComboBox_Users;
        private Label EditUserControl_Label_SelectUser;
        private GroupBox EditUserControl_GroupBox_UserInfo;
        private Label EditUserControl_Label_FirstName;
        private TextBox EditUserControl_TextBox_FirstName;
        private Label EditUserControl_Label_LastName;
        private TextBox EditUserControl_TextBox_LastName;
        private Label EditUserControl_Label_UserName;
        private TextBox EditUserControl_TextBox_UserName;
        private Label EditUserControl_Label_Shift;
        private ComboBox EditUserControl_ComboBox_Shift;
        private Label EditUserControl_Label_Pin;
        private TextBox EditUserControl_TextBox_Pin;
        private GroupBox EditUserControl_GroupBox_VisualInfo;
        private CheckBox EditUserControl_CheckBox_VisualAccess;
        private Label EditUserControl_Label_VisualUserName;
        private TextBox EditUserControl_TextBox_VisualUserName;
        private Label EditUserControl_Label_VisualPassword;
        private TextBox EditUserControl_TextBox_VisualPassword;
        private GroupBox EditUserControl_GroupBox_UserPrivileges;
        private RadioButton EditUserControl_RadioButton_ReadOnly;
        private RadioButton EditUserControl_RadioButton_NormalUser;
        private RadioButton EditUserControl_RadioButton_Administrator;
        private Panel EditUserControl_Panel_LoginInfo;
        private Label EditUserControl_Label_UserNameWarning;
        private LinkLabel linkLabel3;
        private LinkLabel linkLabel2;
        private LinkLabel linkLabel1;
        private Panel panel1;
        private Button EditUserControl_Button_Save;
        private Button EditUserControl_Button_Clear;
        private CheckBox EditUserControl_CheckBox_ViewHidePasswords;
        
        #endregion
        
        #region Methods


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            EditUserControl_ComboBox_Users = new ComboBox();
            EditUserControl_Label_SelectUser = new Label();
            EditUserControl_GroupBox_UserInfo = new GroupBox();
            EditUserControl_Panel_LoginInfo = new Panel();
            EditUserControl_Label_UserNameWarning = new Label();
            EditUserControl_TextBox_Pin = new TextBox();
            EditUserControl_Label_Pin = new Label();
            EditUserControl_TextBox_UserName = new TextBox();
            EditUserControl_Label_UserName = new Label();
            EditUserControl_Label_FirstName = new Label();
            EditUserControl_TextBox_FirstName = new TextBox();
            EditUserControl_Label_LastName = new Label();
            EditUserControl_TextBox_LastName = new TextBox();
            EditUserControl_Label_Shift = new Label();
            EditUserControl_ComboBox_Shift = new ComboBox();
            EditUserControl_GroupBox_VisualInfo = new GroupBox();
            EditUserControl_CheckBox_VisualAccess = new CheckBox();
            EditUserControl_Label_VisualUserName = new Label();
            EditUserControl_TextBox_VisualUserName = new TextBox();
            EditUserControl_Label_VisualPassword = new Label();
            EditUserControl_TextBox_VisualPassword = new TextBox();
            EditUserControl_GroupBox_UserPrivileges = new GroupBox();
            linkLabel3 = new LinkLabel();
            linkLabel2 = new LinkLabel();
            linkLabel1 = new LinkLabel();
            EditUserControl_RadioButton_ReadOnly = new RadioButton();
            EditUserControl_RadioButton_NormalUser = new RadioButton();
            EditUserControl_RadioButton_Administrator = new RadioButton();
            panel1 = new Panel();
            EditUserControl_Button_Clear = new Button();
            EditUserControl_Button_Save = new Button();
            EditUserControl_CheckBox_ViewHidePasswords = new CheckBox();

            EditUserControl_Label_SelectUser.Text = "Select User:";
            EditUserControl_Label_SelectUser.Location = new Point(10, 10);
            EditUserControl_Label_SelectUser.Size = new Size(80, 23);

            EditUserControl_ComboBox_Users.Location = new Point(100, 10);
            EditUserControl_ComboBox_Users.Size = new Size(250, 23);
            EditUserControl_ComboBox_Users.DropDownStyle = ComboBoxStyle.DropDownList;

            EditUserControl_GroupBox_UserInfo.Controls.Add(EditUserControl_Panel_LoginInfo);
            EditUserControl_GroupBox_UserInfo.Controls.Add(EditUserControl_Label_FirstName);
            EditUserControl_GroupBox_UserInfo.Controls.Add(EditUserControl_TextBox_FirstName);
            EditUserControl_GroupBox_UserInfo.Controls.Add(EditUserControl_Label_LastName);
            EditUserControl_GroupBox_UserInfo.Controls.Add(EditUserControl_TextBox_LastName);
            EditUserControl_GroupBox_UserInfo.Controls.Add(EditUserControl_Label_Shift);
            EditUserControl_GroupBox_UserInfo.Controls.Add(EditUserControl_ComboBox_Shift);
            EditUserControl_GroupBox_UserInfo.Location = new Point(10, 40);
            EditUserControl_GroupBox_UserInfo.Name = "EditUserControl_GroupBox_UserInfo";
            EditUserControl_GroupBox_UserInfo.Size = new Size(430, 209);
            EditUserControl_GroupBox_UserInfo.TabIndex = 0;
            EditUserControl_GroupBox_UserInfo.TabStop = false;
            EditUserControl_GroupBox_UserInfo.Text = "User Information";

            EditUserControl_Panel_LoginInfo.Controls.Add(EditUserControl_Label_UserNameWarning);
            EditUserControl_Panel_LoginInfo.Controls.Add(EditUserControl_TextBox_Pin);
            EditUserControl_Panel_LoginInfo.Controls.Add(EditUserControl_Label_Pin);
            EditUserControl_Panel_LoginInfo.Controls.Add(EditUserControl_TextBox_UserName);
            EditUserControl_Panel_LoginInfo.Controls.Add(EditUserControl_Label_UserName);
            EditUserControl_Panel_LoginInfo.Location = new Point(6, 116);
            EditUserControl_Panel_LoginInfo.Name = "EditUserControl_Panel_LoginInfo";
            EditUserControl_Panel_LoginInfo.Size = new Size(414, 87);
            EditUserControl_Panel_LoginInfo.TabIndex = 10;

            EditUserControl_Label_UserNameWarning.ForeColor = Color.Red;
            EditUserControl_Label_UserNameWarning.Location = new Point(6, 58);
            EditUserControl_Label_UserNameWarning.Name = "EditUserControl_Label_UserNameWarning";
            EditUserControl_Label_UserNameWarning.Size = new Size(403, 23);
            EditUserControl_Label_UserNameWarning.TabIndex = 10;
            EditUserControl_Label_UserNameWarning.Text = "User Name cannot be changed.";
            EditUserControl_Label_UserNameWarning.TextAlign = ContentAlignment.MiddleCenter;

            EditUserControl_TextBox_Pin.Location = new Point(80, 32);
            EditUserControl_TextBox_Pin.Name = "EditUserControl_TextBox_Pin";
            EditUserControl_TextBox_Pin.Size = new Size(329, 23);
            EditUserControl_TextBox_Pin.TabIndex = 9;

            EditUserControl_Label_Pin.AutoSize = true;
            EditUserControl_Label_Pin.Location = new Point(47, 36);
            EditUserControl_Label_Pin.Name = "EditUserControl_Label_Pin";
            EditUserControl_Label_Pin.Size = new Size(27, 15);
            EditUserControl_Label_Pin.TabIndex = 8;
            EditUserControl_Label_Pin.Text = "Pin:";

            EditUserControl_TextBox_UserName.Location = new Point(81, 3);
            EditUserControl_TextBox_UserName.Name = "EditUserControl_TextBox_UserName";
            EditUserControl_TextBox_UserName.Size = new Size(328, 23);
            EditUserControl_TextBox_UserName.TabIndex = 5;
            EditUserControl_TextBox_UserName.ReadOnly = true;

            EditUserControl_Label_UserName.AutoSize = true;
            EditUserControl_Label_UserName.Location = new Point(6, 7);
            EditUserControl_Label_UserName.Name = "EditUserControl_Label_UserName";
            EditUserControl_Label_UserName.Size = new Size(68, 15);
            EditUserControl_Label_UserName.TabIndex = 4;
            EditUserControl_Label_UserName.Text = "User Name:";

            EditUserControl_Label_FirstName.AutoSize = true;
            EditUserControl_Label_FirstName.Location = new Point(18, 32);
            EditUserControl_Label_FirstName.Name = "EditUserControl_Label_FirstName";
            EditUserControl_Label_FirstName.Size = new Size(67, 15);
            EditUserControl_Label_FirstName.TabIndex = 0;
            EditUserControl_Label_FirstName.Text = "First Name:";

            EditUserControl_TextBox_FirstName.Location = new Point(90, 29);
            EditUserControl_TextBox_FirstName.Name = "EditUserControl_TextBox_FirstName";
            EditUserControl_TextBox_FirstName.Size = new Size(330, 23);
            EditUserControl_TextBox_FirstName.TabIndex = 1;

            EditUserControl_Label_LastName.AutoSize = true;
            EditUserControl_Label_LastName.Location = new Point(15, 61);
            EditUserControl_Label_LastName.Name = "EditUserControl_Label_LastName";
            EditUserControl_Label_LastName.Size = new Size(66, 15);
            EditUserControl_Label_LastName.TabIndex = 2;
            EditUserControl_Label_LastName.Text = "Last Name:";

            EditUserControl_TextBox_LastName.Location = new Point(90, 58);
            EditUserControl_TextBox_LastName.Name = "EditUserControl_TextBox_LastName";
            EditUserControl_TextBox_LastName.Size = new Size(330, 23);
            EditUserControl_TextBox_LastName.TabIndex = 3;

            EditUserControl_Label_Shift.AutoSize = true;
            EditUserControl_Label_Shift.Location = new Point(47, 90);
            EditUserControl_Label_Shift.Name = "EditUserControl_Label_Shift";
            EditUserControl_Label_Shift.Size = new Size(34, 15);
            EditUserControl_Label_Shift.TabIndex = 6;
            EditUserControl_Label_Shift.Text = "Shift:";

            EditUserControl_ComboBox_Shift.DropDownStyle = ComboBoxStyle.DropDownList;
            EditUserControl_ComboBox_Shift.Location = new Point(90, 87);
            EditUserControl_ComboBox_Shift.Name = "EditUserControl_ComboBox_Shift";
            EditUserControl_ComboBox_Shift.Size = new Size(330, 23);
            EditUserControl_ComboBox_Shift.TabIndex = 7;

            EditUserControl_GroupBox_VisualInfo.Controls.Add(EditUserControl_CheckBox_VisualAccess);
            EditUserControl_GroupBox_VisualInfo.Controls.Add(EditUserControl_Label_VisualUserName);
            EditUserControl_GroupBox_VisualInfo.Controls.Add(EditUserControl_TextBox_VisualUserName);
            EditUserControl_GroupBox_VisualInfo.Controls.Add(EditUserControl_Label_VisualPassword);
            EditUserControl_GroupBox_VisualInfo.Controls.Add(EditUserControl_TextBox_VisualPassword);
            EditUserControl_GroupBox_VisualInfo.Location = new Point(10, 255);
            EditUserControl_GroupBox_VisualInfo.Name = "EditUserControl_GroupBox_VisualInfo";
            EditUserControl_GroupBox_VisualInfo.Size = new Size(430, 113);
            EditUserControl_GroupBox_VisualInfo.TabIndex = 10;
            EditUserControl_GroupBox_VisualInfo.TabStop = false;
            EditUserControl_GroupBox_VisualInfo.Text = "Infor VISUAL Information";

            EditUserControl_CheckBox_VisualAccess.AutoSize = true;
            EditUserControl_CheckBox_VisualAccess.Location = new Point(6, 22);
            EditUserControl_CheckBox_VisualAccess.Name = "EditUserControl_CheckBox_VisualAccess";
            EditUserControl_CheckBox_VisualAccess.Size = new Size(101, 19);
            EditUserControl_CheckBox_VisualAccess.TabIndex = 0;
            EditUserControl_CheckBox_VisualAccess.Text = "Visual Access?";
            EditUserControl_CheckBox_VisualAccess.UseVisualStyleBackColor = true;

            EditUserControl_Label_VisualUserName.AutoSize = true;
            EditUserControl_Label_VisualUserName.Location = new Point(14, 54);
            EditUserControl_Label_VisualUserName.Name = "EditUserControl_Label_VisualUserName";
            EditUserControl_Label_VisualUserName.Size = new Size(68, 15);
            EditUserControl_Label_VisualUserName.TabIndex = 1;
            EditUserControl_Label_VisualUserName.Text = "User Name:";

            EditUserControl_TextBox_VisualUserName.Location = new Point(88, 47);
            EditUserControl_TextBox_VisualUserName.Name = "EditUserControl_TextBox_VisualUserName";
            EditUserControl_TextBox_VisualUserName.Size = new Size(336, 23);
            EditUserControl_TextBox_VisualUserName.TabIndex = 2;

            EditUserControl_Label_VisualPassword.AutoSize = true;
            EditUserControl_Label_VisualPassword.Location = new Point(22, 79);
            EditUserControl_Label_VisualPassword.Name = "EditUserControl_Label_VisualPassword";
            EditUserControl_Label_VisualPassword.Size = new Size(60, 15);
            EditUserControl_Label_VisualPassword.TabIndex = 3;
            EditUserControl_Label_VisualPassword.Text = "Password:";

            EditUserControl_TextBox_VisualPassword.Location = new Point(88, 76);
            EditUserControl_TextBox_VisualPassword.Name = "EditUserControl_TextBox_VisualPassword";
            EditUserControl_TextBox_VisualPassword.Size = new Size(336, 23);
            EditUserControl_TextBox_VisualPassword.TabIndex = 4;

            EditUserControl_GroupBox_UserPrivileges.Controls.Add(linkLabel3);
            EditUserControl_GroupBox_UserPrivileges.Controls.Add(linkLabel2);
            EditUserControl_GroupBox_UserPrivileges.Controls.Add(linkLabel1);
            EditUserControl_GroupBox_UserPrivileges.Controls.Add(EditUserControl_RadioButton_ReadOnly);
            EditUserControl_GroupBox_UserPrivileges.Controls.Add(EditUserControl_RadioButton_NormalUser);
            EditUserControl_GroupBox_UserPrivileges.Controls.Add(EditUserControl_RadioButton_Administrator);
            EditUserControl_GroupBox_UserPrivileges.Location = new Point(446, 40);
            EditUserControl_GroupBox_UserPrivileges.Name = "EditUserControl_GroupBox_UserPrivileges";
            EditUserControl_GroupBox_UserPrivileges.Size = new Size(151, 328);
            EditUserControl_GroupBox_UserPrivileges.TabIndex = 11;
            EditUserControl_GroupBox_UserPrivileges.TabStop = false;
            EditUserControl_GroupBox_UserPrivileges.Text = "User Privileges";

            linkLabel3.Location = new Point(6, 246);
            linkLabel3.Name = "linkLabel3";
            linkLabel3.Size = new Size(139, 79);
            linkLabel3.TabIndex = 5;
            linkLabel3.TabStop = true;
            linkLabel3.Text = "Can view all data but cannot make any changes.";
            linkLabel3.TextAlign = ContentAlignment.MiddleCenter;

            linkLabel2.Location = new Point(6, 142);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new Size(139, 79);
            linkLabel2.TabIndex = 4;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "Has full access, including user management and all system settings.";
            linkLabel2.TextAlign = ContentAlignment.MiddleCenter;

            linkLabel1.Location = new Point(6, 38);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(139, 79);
            linkLabel1.TabIndex = 3;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Can add and edit records needed for daily work, but cannot manage users or settings.";
            linkLabel1.TextAlign = ContentAlignment.MiddleCenter;

            EditUserControl_RadioButton_ReadOnly.AutoSize = true;
            EditUserControl_RadioButton_ReadOnly.Location = new Point(6, 224);
            EditUserControl_RadioButton_ReadOnly.Name = "EditUserControl_RadioButton_ReadOnly";
            EditUserControl_RadioButton_ReadOnly.Size = new Size(107, 19);
            EditUserControl_RadioButton_ReadOnly.TabIndex = 0;
            EditUserControl_RadioButton_ReadOnly.TabStop = true;
            EditUserControl_RadioButton_ReadOnly.Text = "Read-Only User";
            EditUserControl_RadioButton_ReadOnly.UseVisualStyleBackColor = true;

            EditUserControl_RadioButton_NormalUser.AutoSize = true;
            EditUserControl_RadioButton_NormalUser.Location = new Point(6, 16);
            EditUserControl_RadioButton_NormalUser.Name = "EditUserControl_RadioButton_NormalUser";
            EditUserControl_RadioButton_NormalUser.Size = new Size(91, 19);
            EditUserControl_RadioButton_NormalUser.TabIndex = 1;
            EditUserControl_RadioButton_NormalUser.TabStop = true;
            EditUserControl_RadioButton_NormalUser.Text = "Normal User";
            EditUserControl_RadioButton_NormalUser.UseVisualStyleBackColor = true;

            EditUserControl_RadioButton_Administrator.AutoSize = true;
            EditUserControl_RadioButton_Administrator.Location = new Point(6, 120);
            EditUserControl_RadioButton_Administrator.Name = "EditUserControl_RadioButton_Administrator";
            EditUserControl_RadioButton_Administrator.Size = new Size(98, 19);
            EditUserControl_RadioButton_Administrator.TabIndex = 2;
            EditUserControl_RadioButton_Administrator.TabStop = true;
            EditUserControl_RadioButton_Administrator.Text = "Administrator";
            EditUserControl_RadioButton_Administrator.UseVisualStyleBackColor = true;

            panel1.Controls.Add(EditUserControl_CheckBox_ViewHidePasswords);
            panel1.Controls.Add(EditUserControl_Button_Clear);
            panel1.Controls.Add(EditUserControl_Button_Save);
            panel1.Location = new Point(10, 374);
            panel1.Name = "panel1";
            panel1.Size = new Size(581, 53);
            panel1.TabIndex = 12;

            EditUserControl_Button_Clear.Location = new Point(409, 16);
            EditUserControl_Button_Clear.Name = "EditUserControl_Button_Clear";
            EditUserControl_Button_Clear.Size = new Size(75, 23);
            EditUserControl_Button_Clear.TabIndex = 1;
            EditUserControl_Button_Clear.Text = "Clear";
            EditUserControl_Button_Clear.UseVisualStyleBackColor = true;

            EditUserControl_Button_Save.Location = new Point(490, 16);
            EditUserControl_Button_Save.Name = "EditUserControl_Button_Save";
            EditUserControl_Button_Save.Size = new Size(75, 23);
            EditUserControl_Button_Save.TabIndex = 0;
            EditUserControl_Button_Save.Text = "Save";
            EditUserControl_Button_Save.UseVisualStyleBackColor = true;

            EditUserControl_CheckBox_ViewHidePasswords.AutoSize = true;
            EditUserControl_CheckBox_ViewHidePasswords.Location = new Point(12, 18);
            EditUserControl_CheckBox_ViewHidePasswords.Name = "EditUserControl_CheckBox_ViewHidePasswords";
            EditUserControl_CheckBox_ViewHidePasswords.Size = new Size(141, 19);
            EditUserControl_CheckBox_ViewHidePasswords.TabIndex = 2;
            EditUserControl_CheckBox_ViewHidePasswords.Text = "Show Password Fields";
            EditUserControl_CheckBox_ViewHidePasswords.UseVisualStyleBackColor = true;

            Controls.Add(EditUserControl_Label_SelectUser);
            Controls.Add(EditUserControl_ComboBox_Users);
            Controls.Add(panel1);
            Controls.Add(EditUserControl_GroupBox_UserPrivileges);
            Controls.Add(EditUserControl_GroupBox_VisualInfo);
            Controls.Add(EditUserControl_GroupBox_UserInfo);
            Name = "EditUserControl";
            Size = new Size(600, 440);
        }
    }

        
        #endregion
    }