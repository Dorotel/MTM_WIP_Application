namespace MTM_Inventory_Application.Forms.Settings
{
    partial class SettingsForm
    {
        #region Fields

        private System.ComponentModel.IContainer components = null;

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

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            SettingsForm_TreeView_Category = new TreeView();
            SettingsForm_SplitContainer_Main = new SplitContainer();
            SettingsForm_Panel_Right_Main = new Panel();
            SettingsForm_TableLayout_Right = new TableLayoutPanel();
            SettingsForm_StatusStrip = new StatusStrip();
            SettingsForm_StatusText = new ToolStripStatusLabel();
            SettingsForm_Panel_Right = new Panel();
            SettingsForm_Panel_Database = new Panel();
            SettingsForm_Panel_Shortcuts = new Panel();
            SettingsForm_Panel_About = new Panel();
            SettingsForm_Label_Version = new Label();
            appNameLabel = new Label();
            SettingsForm_Panel_AddPart = new Panel();
            SettingsForm_Panel_EditPart = new Panel();
            SettingsForm_Panel_RemovePart = new Panel();
            SettingsForm_Panel_AddOperation = new Panel();
            SettingsForm_Panel_EditOperation = new Panel();
            SettingsForm_Panel_RemoveOperation = new Panel();
            SettingsForm_Panel_AddLocation = new Panel();
            SettingsForm_Panel_EditLocation = new Panel();
            SettingsForm_Panel_RemoveLocation = new Panel();
            SettingsForm_Panel_AddItemType = new Panel();
            SettingsForm_Panel_EditItemType = new Panel();
            SettingsForm_Panel_RemoveItemType = new Panel();
            SettingsForm_Panel_AddUser = new Panel();
            SettingsForm_Panel_EditUser = new Panel();
            SettingsForm_Panel_DeleteUser = new Panel();
            SettingsForm_Panel_Theme = new Panel();
            ((System.ComponentModel.ISupportInitialize)SettingsForm_SplitContainer_Main).BeginInit();
            SettingsForm_SplitContainer_Main.Panel1.SuspendLayout();
            SettingsForm_SplitContainer_Main.Panel2.SuspendLayout();
            SettingsForm_SplitContainer_Main.SuspendLayout();
            SettingsForm_TableLayout_Right.SuspendLayout();
            SettingsForm_StatusStrip.SuspendLayout();
            SettingsForm_Panel_Right.SuspendLayout();
            SettingsForm_Panel_About.SuspendLayout();
            SuspendLayout();
            // 
            // SettingsForm_TreeView_Category
            // 
            SettingsForm_TreeView_Category.Dock = DockStyle.Fill;
            SettingsForm_TreeView_Category.Location = new Point(0, 0);
            SettingsForm_TreeView_Category.Name = "SettingsForm_TreeView_Category";
            SettingsForm_TreeView_Category.Size = new Size(200, 492);
            SettingsForm_TreeView_Category.TabIndex = 0;
            SettingsForm_TreeView_Category.AfterSelect += CategoryTreeView_AfterSelect;
            // 
            // SettingsForm_SplitContainer_Main
            // 
            SettingsForm_SplitContainer_Main.Dock = DockStyle.Fill;
            SettingsForm_SplitContainer_Main.FixedPanel = FixedPanel.Panel1;
            SettingsForm_SplitContainer_Main.Location = new Point(0, 0);
            SettingsForm_SplitContainer_Main.Name = "SettingsForm_SplitContainer_Main";
            // 
            // SettingsForm_SplitContainer_Main.Panel1
            // 
            SettingsForm_SplitContainer_Main.Panel1.Controls.Add(SettingsForm_TreeView_Category);
            SettingsForm_SplitContainer_Main.Panel1MinSize = 200;
            // 
            // SettingsForm_SplitContainer_Main.Panel2
            // 
            SettingsForm_SplitContainer_Main.Panel2.Controls.Add(SettingsForm_TableLayout_Right);
            SettingsForm_SplitContainer_Main.Panel2.Controls.Add(SettingsForm_Panel_Right_Main);
            SettingsForm_SplitContainer_Main.Size = new Size(838, 492);
            SettingsForm_SplitContainer_Main.SplitterDistance = 200;
            SettingsForm_SplitContainer_Main.TabIndex = 0;
            // 
            // SettingsForm_Panel_Right_Main
            // 
            SettingsForm_Panel_Right_Main.AutoSize = true;
            SettingsForm_Panel_Right_Main.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsForm_Panel_Right_Main.Location = new Point(0, 0);
            SettingsForm_Panel_Right_Main.Name = "SettingsForm_Panel_Right_Main";
            SettingsForm_Panel_Right_Main.Size = new Size(0, 0);
            SettingsForm_Panel_Right_Main.TabIndex = 0;
            // 
            // SettingsForm_TableLayout_Right
            // 
            SettingsForm_TableLayout_Right.AutoSize = true;
            SettingsForm_TableLayout_Right.ColumnCount = 1;
            SettingsForm_TableLayout_Right.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SettingsForm_TableLayout_Right.Controls.Add(SettingsForm_StatusStrip, 0, 1);
            SettingsForm_TableLayout_Right.Controls.Add(SettingsForm_Panel_Right, 0, 0);
            SettingsForm_TableLayout_Right.Dock = DockStyle.Fill;
            SettingsForm_TableLayout_Right.Location = new Point(0, 0);
            SettingsForm_TableLayout_Right.Name = "SettingsForm_TableLayout_Right";
            SettingsForm_TableLayout_Right.RowCount = 2;
            SettingsForm_TableLayout_Right.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            SettingsForm_TableLayout_Right.RowStyles.Add(new RowStyle());
            SettingsForm_TableLayout_Right.Size = new Size(634, 492);
            SettingsForm_TableLayout_Right.TabIndex = 1;
            // 
            // SettingsForm_StatusStrip
            // 
            SettingsForm_StatusStrip.Items.AddRange(new ToolStripItem[] { SettingsForm_StatusText });
            SettingsForm_StatusStrip.Location = new Point(0, 470);
            SettingsForm_StatusStrip.Name = "SettingsForm_StatusStrip";
            SettingsForm_StatusStrip.Size = new Size(634, 22);
            SettingsForm_StatusStrip.TabIndex = 0;
            // 
            // SettingsForm_StatusText
            // 
            SettingsForm_StatusText.Name = "SettingsForm_StatusText";
            SettingsForm_StatusText.Size = new Size(120, 17);
            SettingsForm_StatusText.Text = "Status Text Goes Here";
            // 
            // SettingsForm_Panel_Right
            // 
            SettingsForm_Panel_Right.AutoSize = true;
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_Database);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_Shortcuts);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_About);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_AddPart);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_EditPart);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_RemovePart);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_AddOperation);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_EditOperation);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_RemoveOperation);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_AddLocation);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_EditLocation);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_RemoveLocation);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_AddItemType);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_EditItemType);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_RemoveItemType);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_AddUser);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_EditUser);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_DeleteUser);
            SettingsForm_Panel_Right.Controls.Add(SettingsForm_Panel_Theme);
            SettingsForm_Panel_Right.Dock = DockStyle.Fill;
            SettingsForm_Panel_Right.Location = new Point(3, 3);
            SettingsForm_Panel_Right.Name = "SettingsForm_Panel_Right";
            SettingsForm_Panel_Right.Size = new Size(628, 464);
            SettingsForm_Panel_Right.TabIndex = 1;
            // 
            // SettingsForm_Panel_Database
            // 
            SettingsForm_Panel_Database.AutoSize = true;
            SettingsForm_Panel_Database.Location = new Point(143, 376);
            SettingsForm_Panel_Database.Name = "SettingsForm_Panel_Database";
            SettingsForm_Panel_Database.Size = new Size(40, 40);
            SettingsForm_Panel_Database.TabIndex = 41;
            SettingsForm_Panel_Database.Visible = false;
            // 
            // SettingsForm_Panel_Shortcuts
            // 
            SettingsForm_Panel_Shortcuts.AutoSize = true;
            SettingsForm_Panel_Shortcuts.Location = new Point(275, 376);
            SettingsForm_Panel_Shortcuts.Name = "SettingsForm_Panel_Shortcuts";
            SettingsForm_Panel_Shortcuts.Size = new Size(40, 40);
            SettingsForm_Panel_Shortcuts.TabIndex = 42;
            SettingsForm_Panel_Shortcuts.Visible = false;
            // 
            // SettingsForm_Panel_About
            // 
            SettingsForm_Panel_About.Controls.Add(SettingsForm_Label_Version);
            SettingsForm_Panel_About.Controls.Add(appNameLabel);
            SettingsForm_Panel_About.Location = new Point(23, 25);
            SettingsForm_Panel_About.Name = "SettingsForm_Panel_About";
            SettingsForm_Panel_About.Size = new Size(582, 419);
            SettingsForm_Panel_About.TabIndex = 43;
            SettingsForm_Panel_About.Visible = false;
            // 
            // SettingsForm_Label_Version
            // 
            SettingsForm_Label_Version.AutoSize = true;
            SettingsForm_Label_Version.Location = new Point(30, 50);
            SettingsForm_Label_Version.Name = "SettingsForm_Label_Version";
            SettingsForm_Label_Version.Size = new Size(48, 15);
            SettingsForm_Label_Version.TabIndex = 1;
            SettingsForm_Label_Version.Text = "Control_About_Label_Version:";
            // 
            // appNameLabel
            // 
            appNameLabel.AutoSize = true;
            appNameLabel.Font = new Font("Segoe UI Emoji", 12F, FontStyle.Bold);
            appNameLabel.Location = new Point(30, 20);
            appNameLabel.Name = "appNameLabel";
            appNameLabel.Size = new Size(179, 21);
            appNameLabel.TabIndex = 0;
            appNameLabel.Text = "MTM WIP Application";
            // 
            // SettingsForm_Panel_AddPart
            // 
            SettingsForm_Panel_AddPart.AutoSize = true;
            SettingsForm_Panel_AddPart.Location = new Point(62, 28);
            SettingsForm_Panel_AddPart.Name = "SettingsForm_Panel_AddPart";
            SettingsForm_Panel_AddPart.Size = new Size(40, 40);
            SettingsForm_Panel_AddPart.TabIndex = 44;
            SettingsForm_Panel_AddPart.Visible = false;
            // 
            // SettingsForm_Panel_EditPart
            // 
            SettingsForm_Panel_EditPart.AutoSize = true;
            SettingsForm_Panel_EditPart.Location = new Point(134, 28);
            SettingsForm_Panel_EditPart.Name = "SettingsForm_Panel_EditPart";
            SettingsForm_Panel_EditPart.Size = new Size(40, 40);
            SettingsForm_Panel_EditPart.TabIndex = 45;
            SettingsForm_Panel_EditPart.Visible = false;
            // 
            // SettingsForm_Panel_RemovePart
            // 
            SettingsForm_Panel_RemovePart.AutoSize = true;
            SettingsForm_Panel_RemovePart.Location = new Point(197, 28);
            SettingsForm_Panel_RemovePart.Name = "SettingsForm_Panel_RemovePart";
            SettingsForm_Panel_RemovePart.Size = new Size(40, 40);
            SettingsForm_Panel_RemovePart.TabIndex = 46;
            SettingsForm_Panel_RemovePart.Visible = false;
            // 
            // SettingsForm_Panel_AddOperation
            // 
            SettingsForm_Panel_AddOperation.AutoSize = true;
            SettingsForm_Panel_AddOperation.Location = new Point(258, 170);
            SettingsForm_Panel_AddOperation.Name = "SettingsForm_Panel_AddOperation";
            SettingsForm_Panel_AddOperation.Size = new Size(40, 40);
            SettingsForm_Panel_AddOperation.TabIndex = 47;
            SettingsForm_Panel_AddOperation.Visible = false;
            // 
            // SettingsForm_Panel_EditOperation
            // 
            SettingsForm_Panel_EditOperation.AutoSize = true;
            SettingsForm_Panel_EditOperation.Location = new Point(284, 170);
            SettingsForm_Panel_EditOperation.Name = "SettingsForm_Panel_EditOperation";
            SettingsForm_Panel_EditOperation.Size = new Size(40, 40);
            SettingsForm_Panel_EditOperation.TabIndex = 48;
            SettingsForm_Panel_EditOperation.Visible = false;
            // 
            // SettingsForm_Panel_RemoveOperation
            // 
            SettingsForm_Panel_RemoveOperation.AutoSize = true;
            SettingsForm_Panel_RemoveOperation.Location = new Point(310, 170);
            SettingsForm_Panel_RemoveOperation.Name = "SettingsForm_Panel_RemoveOperation";
            SettingsForm_Panel_RemoveOperation.Size = new Size(40, 40);
            SettingsForm_Panel_RemoveOperation.TabIndex = 49;
            SettingsForm_Panel_RemoveOperation.Visible = false;
            // 
            // SettingsForm_Panel_AddLocation
            // 
            SettingsForm_Panel_AddLocation.AutoSize = true;
            SettingsForm_Panel_AddLocation.Location = new Point(258, 196);
            SettingsForm_Panel_AddLocation.Name = "SettingsForm_Panel_AddLocation";
            SettingsForm_Panel_AddLocation.Size = new Size(40, 40);
            SettingsForm_Panel_AddLocation.TabIndex = 50;
            SettingsForm_Panel_AddLocation.Visible = false;
            // 
            // SettingsForm_Panel_EditLocation
            // 
            SettingsForm_Panel_EditLocation.AutoSize = true;
            SettingsForm_Panel_EditLocation.Location = new Point(284, 196);
            SettingsForm_Panel_EditLocation.Name = "SettingsForm_Panel_EditLocation";
            SettingsForm_Panel_EditLocation.Size = new Size(40, 40);
            SettingsForm_Panel_EditLocation.TabIndex = 51;
            SettingsForm_Panel_EditLocation.Visible = false;
            // 
            // SettingsForm_Panel_RemoveLocation
            // 
            SettingsForm_Panel_RemoveLocation.AutoSize = true;
            SettingsForm_Panel_RemoveLocation.Location = new Point(310, 196);
            SettingsForm_Panel_RemoveLocation.Name = "SettingsForm_Panel_RemoveLocation";
            SettingsForm_Panel_RemoveLocation.Size = new Size(40, 40);
            SettingsForm_Panel_RemoveLocation.TabIndex = 52;
            SettingsForm_Panel_RemoveLocation.Visible = false;
            // 
            // SettingsForm_Panel_AddItemType
            // 
            SettingsForm_Panel_AddItemType.AutoSize = true;
            SettingsForm_Panel_AddItemType.Location = new Point(258, 222);
            SettingsForm_Panel_AddItemType.Name = "SettingsForm_Panel_AddItemType";
            SettingsForm_Panel_AddItemType.Size = new Size(40, 40);
            SettingsForm_Panel_AddItemType.TabIndex = 53;
            SettingsForm_Panel_AddItemType.Visible = false;
            // 
            // SettingsForm_Panel_EditItemType
            // 
            SettingsForm_Panel_EditItemType.AutoSize = true;
            SettingsForm_Panel_EditItemType.Location = new Point(284, 222);
            SettingsForm_Panel_EditItemType.Name = "SettingsForm_Panel_EditItemType";
            SettingsForm_Panel_EditItemType.Size = new Size(40, 40);
            SettingsForm_Panel_EditItemType.TabIndex = 54;
            SettingsForm_Panel_EditItemType.Visible = false;
            // 
            // SettingsForm_Panel_RemoveItemType
            // 
            SettingsForm_Panel_RemoveItemType.AutoSize = true;
            SettingsForm_Panel_RemoveItemType.Location = new Point(310, 222);
            SettingsForm_Panel_RemoveItemType.Name = "SettingsForm_Panel_RemoveItemType";
            SettingsForm_Panel_RemoveItemType.Size = new Size(40, 40);
            SettingsForm_Panel_RemoveItemType.TabIndex = 55;
            SettingsForm_Panel_RemoveItemType.Visible = false;
            // 
            // SettingsForm_Panel_AddUser
            // 
            SettingsForm_Panel_AddUser.AutoSize = true;
            SettingsForm_Panel_AddUser.Location = new Point(258, 144);
            SettingsForm_Panel_AddUser.Name = "SettingsForm_Panel_AddUser";
            SettingsForm_Panel_AddUser.Size = new Size(40, 40);
            SettingsForm_Panel_AddUser.TabIndex = 56;
            SettingsForm_Panel_AddUser.Visible = false;
            // 
            // SettingsForm_Panel_EditUser
            // 
            SettingsForm_Panel_EditUser.AutoSize = true;
            SettingsForm_Panel_EditUser.Location = new Point(284, 144);
            SettingsForm_Panel_EditUser.Name = "SettingsForm_Panel_EditUser";
            SettingsForm_Panel_EditUser.Size = new Size(40, 40);
            SettingsForm_Panel_EditUser.TabIndex = 57;
            SettingsForm_Panel_EditUser.Visible = false;
            // 
            // SettingsForm_Panel_DeleteUser
            // 
            SettingsForm_Panel_DeleteUser.AutoSize = true;
            SettingsForm_Panel_DeleteUser.Location = new Point(310, 144);
            SettingsForm_Panel_DeleteUser.Name = "SettingsForm_Panel_DeleteUser";
            SettingsForm_Panel_DeleteUser.Size = new Size(40, 40);
            SettingsForm_Panel_DeleteUser.TabIndex = 58;
            SettingsForm_Panel_DeleteUser.Visible = false;
            // 
            // SettingsForm_Panel_Theme
            // 
            SettingsForm_Panel_Theme.Location = new Point(275, 299);
            SettingsForm_Panel_Theme.Name = "SettingsForm_Panel_Theme";
            SettingsForm_Panel_Theme.Size = new Size(40, 40);
            SettingsForm_Panel_Theme.TabIndex = 40;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(838, 492);
            Controls.Add(SettingsForm_SplitContainer_Main);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings - MTM WIP Application";
            SettingsForm_SplitContainer_Main.Panel1.ResumeLayout(false);
            SettingsForm_SplitContainer_Main.Panel2.ResumeLayout(false);
            SettingsForm_SplitContainer_Main.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SettingsForm_SplitContainer_Main).EndInit();
            SettingsForm_SplitContainer_Main.ResumeLayout(false);
            SettingsForm_TableLayout_Right.ResumeLayout(false);
            SettingsForm_TableLayout_Right.PerformLayout();
            SettingsForm_StatusStrip.ResumeLayout(false);
            SettingsForm_StatusStrip.PerformLayout();
            SettingsForm_Panel_Right.ResumeLayout(false);
            SettingsForm_Panel_Right.PerformLayout();
            SettingsForm_Panel_About.ResumeLayout(false);
            SettingsForm_Panel_About.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TreeView SettingsForm_TreeView_Category;
        private SplitContainer SettingsForm_SplitContainer_Main;
        private Panel SettingsForm_Panel_Right_Main;
        private TableLayoutPanel SettingsForm_TableLayout_Right;
        private StatusStrip SettingsForm_StatusStrip;
        private ToolStripStatusLabel SettingsForm_StatusText;
        private Panel SettingsForm_Panel_Right;
        private Panel SettingsForm_Panel_Database;
        private Panel SettingsForm_Panel_Shortcuts;
        private Panel SettingsForm_Panel_About;
        private Label SettingsForm_Label_Version;
        private Label appNameLabel;
        private Panel SettingsForm_Panel_AddPart;
        private Panel SettingsForm_Panel_EditPart;
        private Panel SettingsForm_Panel_RemovePart;
        private Panel SettingsForm_Panel_AddOperation;
        private Panel SettingsForm_Panel_EditOperation;
        private Panel SettingsForm_Panel_RemoveOperation;
        private Panel SettingsForm_Panel_AddLocation;
        private Panel SettingsForm_Panel_EditLocation;
        private Panel SettingsForm_Panel_RemoveLocation;
        private Panel SettingsForm_Panel_AddItemType;
        private Panel SettingsForm_Panel_EditItemType;
        private Panel SettingsForm_Panel_RemoveItemType;
        private Panel SettingsForm_Panel_AddUser;
        private Panel SettingsForm_Panel_EditUser;
        private Panel SettingsForm_Panel_DeleteUser;
        private Panel SettingsForm_Panel_Theme;
    }
}
