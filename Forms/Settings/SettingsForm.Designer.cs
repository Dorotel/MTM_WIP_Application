namespace MTM_Inventory_Application.Forms.Settings
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.categoryListBox = new System.Windows.Forms.ListBox();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.statusLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.resetDefaultsButton = new System.Windows.Forms.Button();
            this.databasePanel = new System.Windows.Forms.Panel();
            this.databaseTabControl = new System.Windows.Forms.TabControl();
            this.connectionTabPage = new System.Windows.Forms.TabPage();
            this.connectionGroupBox = new System.Windows.Forms.GroupBox();
            this.autoReconnectCheckBox = new System.Windows.Forms.CheckBox();
            this.timeoutTextBox = new System.Windows.Forms.TextBox();
            this.timeoutLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.databaseTextBox = new System.Windows.Forms.TextBox();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.objectsTabPage = new System.Windows.Forms.TabPage();
            this.objectsTabControl = new System.Windows.Forms.TabControl();
            this.partsTabPage = new System.Windows.Forms.TabPage();
            this.partsDataGridView = new System.Windows.Forms.DataGridView();
            this.partsToolStrip = new System.Windows.Forms.ToolStrip();
            this.addPartToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editPartToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deletePartToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.operationsTabPage = new System.Windows.Forms.TabPage();
            this.operationsDataGridView = new System.Windows.Forms.DataGridView();
            this.operationsToolStrip = new System.Windows.Forms.ToolStrip();
            this.addOperationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editOperationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteOperationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.locationsTabPage = new System.Windows.Forms.TabPage();
            this.locationsDataGridView = new System.Windows.Forms.DataGridView();
            this.locationsToolStrip = new System.Windows.Forms.ToolStrip();
            this.addLocationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editLocationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteLocationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.usersTabPage = new System.Windows.Forms.TabPage();
            this.usersDataGridView = new System.Windows.Forms.DataGridView();
            this.usersToolStrip = new System.Windows.Forms.ToolStrip();
            this.addUserToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editUserToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteUserToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.themePanel = new System.Windows.Forms.Panel();
            this.fontSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.fontSizeLabel = new System.Windows.Forms.Label();
            this.themeComboBox = new System.Windows.Forms.ComboBox();
            this.themeLabel = new System.Windows.Forms.Label();
            this.shortcutsPanel = new System.Windows.Forms.Panel();
            this.shortcutsDataGridView = new System.Windows.Forms.DataGridView();
            this.aboutPanel = new System.Windows.Forms.Panel();
            this.versionLabel = new System.Windows.Forms.Label();
            this.appNameLabel = new System.Windows.Forms.Label();
            this.addPartPanel = new System.Windows.Forms.Panel();
            this.editPartPanel = new System.Windows.Forms.Panel();
            this.removePartPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.databasePanel.SuspendLayout();
            this.databaseTabControl.SuspendLayout();
            this.connectionTabPage.SuspendLayout();
            this.connectionGroupBox.SuspendLayout();
            this.objectsTabPage.SuspendLayout();
            this.objectsTabControl.SuspendLayout();
            this.partsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partsDataGridView)).BeginInit();
            this.partsToolStrip.SuspendLayout();
            this.operationsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.operationsDataGridView)).BeginInit();
            this.operationsToolStrip.SuspendLayout();
            this.locationsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locationsDataGridView)).BeginInit();
            this.locationsToolStrip.SuspendLayout();
            this.usersTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usersDataGridView)).BeginInit();
            this.usersToolStrip.SuspendLayout();
            this.themePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeNumericUpDown)).BeginInit();
            this.shortcutsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shortcutsDataGridView)).BeginInit();
            this.aboutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.categoryListBox);
            this.splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.settingsPanel);
            this.splitContainer1.Panel2.Controls.Add(this.bottomPanel);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 0;
            // 
            // categoryListBox
            // 
            this.categoryListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryListBox.FormattingEnabled = true;
            this.categoryListBox.ItemHeight = 15;
            this.categoryListBox.Items.AddRange(new object[] {
            "Database",
            "Add Part Number",
            "Edit Part Number", 
            "Remove Part Number",
            "Theme",
            "Shortcuts",
            "About"});
            this.categoryListBox.Location = new System.Drawing.Point(0, 0);
            this.categoryListBox.Name = "categoryListBox";
            this.categoryListBox.Size = new System.Drawing.Size(200, 450);
            this.categoryListBox.TabIndex = 0;
            this.categoryListBox.SelectedIndexChanged += new System.EventHandler(this.categoryListBox_SelectedIndexChanged);
            // 
            // settingsPanel
            // 
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsPanel.Location = new System.Drawing.Point(0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(596, 410);
            this.settingsPanel.TabIndex = 1;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.statusLabel);
            this.bottomPanel.Controls.Add(this.cancelButton);
            this.bottomPanel.Controls.Add(this.saveButton);
            this.bottomPanel.Controls.Add(this.resetDefaultsButton);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 410);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(596, 40);
            this.bottomPanel.TabIndex = 0;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 13);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 15);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "Ready";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(433, 9);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(514, 9);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // resetDefaultsButton
            // 
            this.resetDefaultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resetDefaultsButton.Location = new System.Drawing.Point(352, 9);
            this.resetDefaultsButton.Name = "resetDefaultsButton";
            this.resetDefaultsButton.Size = new System.Drawing.Size(75, 23);
            this.resetDefaultsButton.TabIndex = 3;
            this.resetDefaultsButton.Text = "Reset";
            this.resetDefaultsButton.UseVisualStyleBackColor = true;
            this.resetDefaultsButton.Click += new System.EventHandler(this.resetDefaultsButton_Click);
            // 
            // databasePanel
            // 
            this.databasePanel.Controls.Add(this.databaseTabControl);
            this.databasePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.databasePanel.Location = new System.Drawing.Point(0, 0);
            this.databasePanel.Name = "databasePanel";
            this.databasePanel.Size = new System.Drawing.Size(596, 410);
            this.databasePanel.TabIndex = 2;
            this.databasePanel.Visible = false;
            // 
            // databaseTabControl
            // 
            this.databaseTabControl.Controls.Add(this.connectionTabPage);
            this.databaseTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.databaseTabControl.Location = new System.Drawing.Point(0, 0);
            this.databaseTabControl.Name = "databaseTabControl";
            this.databaseTabControl.SelectedIndex = 0;
            this.databaseTabControl.Size = new System.Drawing.Size(596, 410);
            this.databaseTabControl.TabIndex = 0;
            // 
            // connectionTabPage
            // 
            this.connectionTabPage.Controls.Add(this.connectionGroupBox);
            this.connectionTabPage.Location = new System.Drawing.Point(4, 24);
            this.connectionTabPage.Name = "connectionTabPage";
            this.connectionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.connectionTabPage.Size = new System.Drawing.Size(588, 382);
            this.connectionTabPage.TabIndex = 0;
            this.connectionTabPage.Text = "Connection Settings";
            this.connectionTabPage.UseVisualStyleBackColor = true;
            // 
            // connectionGroupBox
            // 
            this.connectionGroupBox.Controls.Add(this.autoReconnectCheckBox);
            this.connectionGroupBox.Controls.Add(this.timeoutTextBox);
            this.connectionGroupBox.Controls.Add(this.timeoutLabel);
            this.connectionGroupBox.Controls.Add(this.passwordTextBox);
            this.connectionGroupBox.Controls.Add(this.passwordLabel);
            this.connectionGroupBox.Controls.Add(this.usernameTextBox);
            this.connectionGroupBox.Controls.Add(this.usernameLabel);
            this.connectionGroupBox.Controls.Add(this.databaseTextBox);
            this.connectionGroupBox.Controls.Add(this.databaseLabel);
            this.connectionGroupBox.Controls.Add(this.portTextBox);
            this.connectionGroupBox.Controls.Add(this.portLabel);
            this.connectionGroupBox.Controls.Add(this.serverTextBox);
            this.connectionGroupBox.Controls.Add(this.serverLabel);
            this.connectionGroupBox.Location = new System.Drawing.Point(6, 6);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Size = new System.Drawing.Size(576, 200);
            this.connectionGroupBox.TabIndex = 0;
            this.connectionGroupBox.TabStop = false;
            this.connectionGroupBox.Text = "Database Configuration";
            // 
            // autoReconnectCheckBox
            // 
            this.autoReconnectCheckBox.AutoSize = true;
            this.autoReconnectCheckBox.Location = new System.Drawing.Point(150, 170);
            this.autoReconnectCheckBox.Name = "autoReconnectCheckBox";
            this.autoReconnectCheckBox.Size = new System.Drawing.Size(173, 19);
            this.autoReconnectCheckBox.TabIndex = 12;
            this.autoReconnectCheckBox.Text = "Enable automatic reconnect";
            this.autoReconnectCheckBox.UseVisualStyleBackColor = true;
            // 
            // timeoutTextBox
            // 
            this.timeoutTextBox.Location = new System.Drawing.Point(150, 140);
            this.timeoutTextBox.Name = "timeoutTextBox";
            this.timeoutTextBox.Size = new System.Drawing.Size(100, 23);
            this.timeoutTextBox.TabIndex = 11;
            // 
            // timeoutLabel
            // 
            this.timeoutLabel.AutoSize = true;
            this.timeoutLabel.Location = new System.Drawing.Point(15, 143);
            this.timeoutLabel.Name = "timeoutLabel";
            this.timeoutLabel.Size = new System.Drawing.Size(108, 15);
            this.timeoutLabel.TabIndex = 10;
            this.timeoutLabel.Text = "Connection Timeout:";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(150, 111);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(200, 23);
            this.passwordTextBox.TabIndex = 9;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(15, 114);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(60, 15);
            this.passwordLabel.TabIndex = 8;
            this.passwordLabel.Text = "Password:";
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(150, 82);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(200, 23);
            this.usernameTextBox.TabIndex = 7;
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(15, 85);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(63, 15);
            this.usernameLabel.TabIndex = 6;
            this.usernameLabel.Text = "Username:";
            // 
            // databaseTextBox
            // 
            this.databaseTextBox.Location = new System.Drawing.Point(150, 53);
            this.databaseTextBox.Name = "databaseTextBox";
            this.databaseTextBox.Size = new System.Drawing.Size(200, 23);
            this.databaseTextBox.TabIndex = 5;
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Location = new System.Drawing.Point(15, 56);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(91, 15);
            this.databaseLabel.TabIndex = 4;
            this.databaseLabel.Text = "Database Name:";
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(410, 24);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(100, 23);
            this.portTextBox.TabIndex = 3;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(375, 27);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(32, 15);
            this.portLabel.TabIndex = 2;
            this.portLabel.Text = "Port:";
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(150, 24);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(200, 23);
            this.serverTextBox.TabIndex = 1;
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(15, 27);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(86, 15);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Server Address:";
            // 
            // objectsTabPage
            // 
            this.objectsTabPage.Controls.Add(this.objectsTabControl);
            this.objectsTabPage.Location = new System.Drawing.Point(4, 24);
            this.objectsTabPage.Name = "objectsTabPage";
            this.objectsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.objectsTabPage.Size = new System.Drawing.Size(588, 382);
            this.objectsTabPage.TabIndex = 1;
            this.objectsTabPage.Text = "Database Objects";
            this.objectsTabPage.UseVisualStyleBackColor = true;
            // 
            // objectsTabControl
            // 
            this.objectsTabControl.Controls.Add(this.partsTabPage);
            this.objectsTabControl.Controls.Add(this.operationsTabPage);
            this.objectsTabControl.Controls.Add(this.locationsTabPage);
            this.objectsTabControl.Controls.Add(this.usersTabPage);
            this.objectsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectsTabControl.Location = new System.Drawing.Point(3, 3);
            this.objectsTabControl.Name = "objectsTabControl";
            this.objectsTabControl.SelectedIndex = 0;
            this.objectsTabControl.Size = new System.Drawing.Size(582, 376);
            this.objectsTabControl.TabIndex = 0;
            // 
            // partsTabPage
            // 
            this.partsTabPage.Controls.Add(this.partsDataGridView);
            this.partsTabPage.Controls.Add(this.partsToolStrip);
            this.partsTabPage.Location = new System.Drawing.Point(4, 24);
            this.partsTabPage.Name = "partsTabPage";
            this.partsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.partsTabPage.Size = new System.Drawing.Size(574, 348);
            this.partsTabPage.TabIndex = 0;
            this.partsTabPage.Text = "Parts";
            this.partsTabPage.UseVisualStyleBackColor = true;
            // 
            // partsDataGridView
            // 
            this.partsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.partsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partsDataGridView.Location = new System.Drawing.Point(3, 28);
            this.partsDataGridView.Name = "partsDataGridView";
            this.partsDataGridView.RowTemplate.Height = 25;
            this.partsDataGridView.Size = new System.Drawing.Size(568, 317);
            this.partsDataGridView.TabIndex = 1;
            // 
            // partsToolStrip
            // 
            this.partsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPartToolStripButton,
            this.editPartToolStripButton,
            this.deletePartToolStripButton});
            this.partsToolStrip.Location = new System.Drawing.Point(3, 3);
            this.partsToolStrip.Name = "partsToolStrip";
            this.partsToolStrip.Size = new System.Drawing.Size(568, 25);
            this.partsToolStrip.TabIndex = 0;
            this.partsToolStrip.Text = "toolStrip1";
            // 
            // addPartToolStripButton
            // 
            this.addPartToolStripButton.Text = "Add";
            this.addPartToolStripButton.Click += new System.EventHandler(this.addPartToolStripButton_Click);
            // 
            // editPartToolStripButton
            // 
            this.editPartToolStripButton.Text = "Edit";
            this.editPartToolStripButton.Click += new System.EventHandler(this.editPartToolStripButton_Click);
            // 
            // deletePartToolStripButton
            // 
            this.deletePartToolStripButton.Text = "Delete";
            this.deletePartToolStripButton.Click += new System.EventHandler(this.deletePartToolStripButton_Click);
            // 
            // operationsTabPage
            // 
            this.operationsTabPage.Controls.Add(this.operationsDataGridView);
            this.operationsTabPage.Controls.Add(this.operationsToolStrip);
            this.operationsTabPage.Location = new System.Drawing.Point(4, 24);
            this.operationsTabPage.Name = "operationsTabPage";
            this.operationsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.operationsTabPage.Size = new System.Drawing.Size(574, 348);
            this.operationsTabPage.TabIndex = 1;
            this.operationsTabPage.Text = "Operations";
            this.operationsTabPage.UseVisualStyleBackColor = true;
            // 
            // operationsDataGridView
            // 
            this.operationsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.operationsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationsDataGridView.Location = new System.Drawing.Point(3, 28);
            this.operationsDataGridView.Name = "operationsDataGridView";
            this.operationsDataGridView.RowTemplate.Height = 25;
            this.operationsDataGridView.Size = new System.Drawing.Size(568, 317);
            this.operationsDataGridView.TabIndex = 1;
            // 
            // operationsToolStrip
            // 
            this.operationsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addOperationToolStripButton,
            this.editOperationToolStripButton,
            this.deleteOperationToolStripButton});
            this.operationsToolStrip.Location = new System.Drawing.Point(3, 3);
            this.operationsToolStrip.Name = "operationsToolStrip";
            this.operationsToolStrip.Size = new System.Drawing.Size(568, 25);
            this.operationsToolStrip.TabIndex = 0;
            this.operationsToolStrip.Text = "toolStrip2";
            // 
            // addOperationToolStripButton
            // 
            this.addOperationToolStripButton.Text = "Add";
            this.addOperationToolStripButton.Click += new System.EventHandler(this.addOperationToolStripButton_Click);
            // 
            // editOperationToolStripButton
            // 
            this.editOperationToolStripButton.Text = "Edit";
            this.editOperationToolStripButton.Click += new System.EventHandler(this.editOperationToolStripButton_Click);
            // 
            // deleteOperationToolStripButton
            // 
            this.deleteOperationToolStripButton.Text = "Delete";
            this.deleteOperationToolStripButton.Click += new System.EventHandler(this.deleteOperationToolStripButton_Click);
            // 
            // locationsTabPage
            // 
            this.locationsTabPage.Controls.Add(this.locationsDataGridView);
            this.locationsTabPage.Controls.Add(this.locationsToolStrip);
            this.locationsTabPage.Location = new System.Drawing.Point(4, 24);
            this.locationsTabPage.Name = "locationsTabPage";
            this.locationsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.locationsTabPage.Size = new System.Drawing.Size(574, 348);
            this.locationsTabPage.TabIndex = 2;
            this.locationsTabPage.Text = "Locations";
            this.locationsTabPage.UseVisualStyleBackColor = true;
            // 
            // locationsDataGridView
            // 
            this.locationsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.locationsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.locationsDataGridView.Location = new System.Drawing.Point(3, 28);
            this.locationsDataGridView.Name = "locationsDataGridView";
            this.locationsDataGridView.RowTemplate.Height = 25;
            this.locationsDataGridView.Size = new System.Drawing.Size(568, 317);
            this.locationsDataGridView.TabIndex = 1;
            // 
            // locationsToolStrip
            // 
            this.locationsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLocationToolStripButton,
            this.editLocationToolStripButton,
            this.deleteLocationToolStripButton});
            this.locationsToolStrip.Location = new System.Drawing.Point(3, 3);
            this.locationsToolStrip.Name = "locationsToolStrip";
            this.locationsToolStrip.Size = new System.Drawing.Size(568, 25);
            this.locationsToolStrip.TabIndex = 0;
            this.locationsToolStrip.Text = "toolStrip3";
            // 
            // addLocationToolStripButton
            // 
            this.addLocationToolStripButton.Text = "Add";
            this.addLocationToolStripButton.Click += new System.EventHandler(this.addLocationToolStripButton_Click);
            // 
            // editLocationToolStripButton
            // 
            this.editLocationToolStripButton.Text = "Edit";
            this.editLocationToolStripButton.Click += new System.EventHandler(this.editLocationToolStripButton_Click);
            // 
            // deleteLocationToolStripButton
            // 
            this.deleteLocationToolStripButton.Text = "Delete";
            this.deleteLocationToolStripButton.Click += new System.EventHandler(this.deleteLocationToolStripButton_Click);
            // 
            // usersTabPage
            // 
            this.usersTabPage.Controls.Add(this.usersDataGridView);
            this.usersTabPage.Controls.Add(this.usersToolStrip);
            this.usersTabPage.Location = new System.Drawing.Point(4, 24);
            this.usersTabPage.Name = "usersTabPage";
            this.usersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.usersTabPage.Size = new System.Drawing.Size(574, 348);
            this.usersTabPage.TabIndex = 3;
            this.usersTabPage.Text = "Users";
            this.usersTabPage.UseVisualStyleBackColor = true;
            // 
            // usersDataGridView
            // 
            this.usersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.usersDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usersDataGridView.Location = new System.Drawing.Point(3, 28);
            this.usersDataGridView.Name = "usersDataGridView";
            this.usersDataGridView.RowTemplate.Height = 25;
            this.usersDataGridView.Size = new System.Drawing.Size(568, 317);
            this.usersDataGridView.TabIndex = 1;
            // 
            // usersToolStrip
            // 
            this.usersToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addUserToolStripButton,
            this.editUserToolStripButton,
            this.deleteUserToolStripButton});
            this.usersToolStrip.Location = new System.Drawing.Point(3, 3);
            this.usersToolStrip.Name = "usersToolStrip";
            this.usersToolStrip.Size = new System.Drawing.Size(568, 25);
            this.usersToolStrip.TabIndex = 0;
            this.usersToolStrip.Text = "toolStrip4";
            // 
            // addUserToolStripButton
            // 
            this.addUserToolStripButton.Text = "Add";
            this.addUserToolStripButton.Click += new System.EventHandler(this.addUserToolStripButton_Click);
            // 
            // editUserToolStripButton
            // 
            this.editUserToolStripButton.Text = "Edit";
            this.editUserToolStripButton.Click += new System.EventHandler(this.editUserToolStripButton_Click);
            // 
            // deleteUserToolStripButton
            // 
            this.deleteUserToolStripButton.Text = "Delete";
            this.deleteUserToolStripButton.Click += new System.EventHandler(this.deleteUserToolStripButton_Click);
            // 
            // themePanel
            // 
            this.themePanel.Controls.Add(this.fontSizeNumericUpDown);
            this.themePanel.Controls.Add(this.fontSizeLabel);
            this.themePanel.Controls.Add(this.themeComboBox);
            this.themePanel.Controls.Add(this.themeLabel);
            this.themePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themePanel.Location = new System.Drawing.Point(0, 0);
            this.themePanel.Name = "themePanel";
            this.themePanel.Size = new System.Drawing.Size(596, 410);
            this.themePanel.TabIndex = 3;
            this.themePanel.Visible = false;
            // 
            // fontSizeNumericUpDown
            // 
            this.fontSizeNumericUpDown.Location = new System.Drawing.Point(100, 50);
            this.fontSizeNumericUpDown.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.fontSizeNumericUpDown.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.fontSizeNumericUpDown.Name = "fontSizeNumericUpDown";
            this.fontSizeNumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.fontSizeNumericUpDown.TabIndex = 3;
            this.fontSizeNumericUpDown.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // fontSizeLabel
            // 
            this.fontSizeLabel.AutoSize = true;
            this.fontSizeLabel.Location = new System.Drawing.Point(30, 52);
            this.fontSizeLabel.Name = "fontSizeLabel";
            this.fontSizeLabel.Size = new System.Drawing.Size(57, 15);
            this.fontSizeLabel.TabIndex = 2;
            this.fontSizeLabel.Text = "Font Size:";
            // 
            // themeComboBox
            // 
            this.themeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.themeComboBox.FormattingEnabled = true;
            this.themeComboBox.Location = new System.Drawing.Point(100, 20);
            this.themeComboBox.Name = "themeComboBox";
            this.themeComboBox.Size = new System.Drawing.Size(200, 23);
            this.themeComboBox.TabIndex = 1;
            // 
            // themeLabel
            // 
            this.themeLabel.AutoSize = true;
            this.themeLabel.Location = new System.Drawing.Point(30, 23);
            this.themeLabel.Name = "themeLabel";
            this.themeLabel.Size = new System.Drawing.Size(43, 15);
            this.themeLabel.TabIndex = 0;
            this.themeLabel.Text = "Theme:";
            // 
            // shortcutsPanel
            // 
            this.shortcutsPanel.Controls.Add(this.shortcutsDataGridView);
            this.shortcutsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shortcutsPanel.Location = new System.Drawing.Point(0, 0);
            this.shortcutsPanel.Name = "shortcutsPanel";
            this.shortcutsPanel.Size = new System.Drawing.Size(596, 410);
            this.shortcutsPanel.TabIndex = 4;
            this.shortcutsPanel.Visible = false;
            // 
            // shortcutsDataGridView
            // 
            this.shortcutsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.shortcutsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shortcutsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.shortcutsDataGridView.Name = "shortcutsDataGridView";
            this.shortcutsDataGridView.RowTemplate.Height = 25;
            this.shortcutsDataGridView.Size = new System.Drawing.Size(596, 410);
            this.shortcutsDataGridView.TabIndex = 0;
            // 
            // aboutPanel
            // 
            this.aboutPanel.Controls.Add(this.versionLabel);
            this.aboutPanel.Controls.Add(this.appNameLabel);
            this.aboutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutPanel.Location = new System.Drawing.Point(0, 0);
            this.aboutPanel.Name = "aboutPanel";
            this.aboutPanel.Size = new System.Drawing.Size(596, 410);
            this.aboutPanel.TabIndex = 5;
            this.aboutPanel.Visible = false;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(30, 50);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(48, 15);
            this.versionLabel.TabIndex = 1;
            this.versionLabel.Text = "Version:";
            // 
            // appNameLabel
            // 
            this.appNameLabel.AutoSize = true;
            this.appNameLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.appNameLabel.Location = new System.Drawing.Point(30, 20);
            this.appNameLabel.Name = "appNameLabel";
            this.appNameLabel.Size = new System.Drawing.Size(186, 21);
            this.appNameLabel.TabIndex = 0;
            this.appNameLabel.Text = "MTM WIP Application";
            // 
            // addPartPanel
            // 
            this.addPartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addPartPanel.Location = new System.Drawing.Point(0, 0);
            this.addPartPanel.Name = "addPartPanel";
            this.addPartPanel.Size = new System.Drawing.Size(596, 410);
            this.addPartPanel.TabIndex = 6;
            this.addPartPanel.Visible = false;
            // 
            // editPartPanel
            // 
            this.editPartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editPartPanel.Location = new System.Drawing.Point(0, 0);
            this.editPartPanel.Name = "editPartPanel";
            this.editPartPanel.Size = new System.Drawing.Size(596, 410);
            this.editPartPanel.TabIndex = 7;
            this.editPartPanel.Visible = false;
            // 
            // removePartPanel
            // 
            this.removePartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.removePartPanel.Location = new System.Drawing.Point(0, 0);
            this.removePartPanel.Name = "removePartPanel";
            this.removePartPanel.Size = new System.Drawing.Size(596, 410);
            this.removePartPanel.TabIndex = 8;
            this.removePartPanel.Visible = false;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.removePartPanel);
            this.Controls.Add(this.editPartPanel);
            this.Controls.Add(this.addPartPanel);
            this.Controls.Add(this.aboutPanel);
            this.Controls.Add(this.shortcutsPanel);
            this.Controls.Add(this.themePanel);
            this.Controls.Add(this.databasePanel);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings - MTM WIP Application";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.databasePanel.ResumeLayout(false);
            this.databaseTabControl.ResumeLayout(false);
            this.connectionTabPage.ResumeLayout(false);
            this.connectionGroupBox.ResumeLayout(false);
            this.connectionGroupBox.PerformLayout();
            this.objectsTabPage.ResumeLayout(false);
            this.objectsTabControl.ResumeLayout(false);
            this.partsTabPage.ResumeLayout(false);
            this.partsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partsDataGridView)).EndInit();
            this.partsToolStrip.ResumeLayout(false);
            this.partsToolStrip.PerformLayout();
            this.operationsTabPage.ResumeLayout(false);
            this.operationsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.operationsDataGridView)).EndInit();
            this.operationsToolStrip.ResumeLayout(false);
            this.operationsToolStrip.PerformLayout();
            this.locationsTabPage.ResumeLayout(false);
            this.locationsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locationsDataGridView)).EndInit();
            this.locationsToolStrip.ResumeLayout(false);
            this.locationsToolStrip.PerformLayout();
            this.usersTabPage.ResumeLayout(false);
            this.usersTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usersDataGridView)).EndInit();
            this.usersToolStrip.ResumeLayout(false);
            this.usersToolStrip.PerformLayout();
            this.themePanel.ResumeLayout(false);
            this.themePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeNumericUpDown)).EndInit();
            this.shortcutsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shortcutsDataGridView)).EndInit();
            this.aboutPanel.ResumeLayout(false);
            this.aboutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox categoryListBox;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button resetDefaultsButton;
        private System.Windows.Forms.Panel databasePanel;
        private System.Windows.Forms.TabControl databaseTabControl;
        private System.Windows.Forms.TabPage connectionTabPage;
        private System.Windows.Forms.GroupBox connectionGroupBox;
        private System.Windows.Forms.CheckBox autoReconnectCheckBox;
        private System.Windows.Forms.TextBox timeoutTextBox;
        private System.Windows.Forms.Label timeoutLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox databaseTextBox;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TabPage objectsTabPage;
        private System.Windows.Forms.TabControl objectsTabControl;
        private System.Windows.Forms.TabPage partsTabPage;
        private System.Windows.Forms.DataGridView partsDataGridView;
        private System.Windows.Forms.ToolStrip partsToolStrip;
        private System.Windows.Forms.ToolStripButton addPartToolStripButton;
        private System.Windows.Forms.ToolStripButton editPartToolStripButton;
        private System.Windows.Forms.ToolStripButton deletePartToolStripButton;
        private System.Windows.Forms.TabPage operationsTabPage;
        private System.Windows.Forms.DataGridView operationsDataGridView;
        private System.Windows.Forms.ToolStrip operationsToolStrip;
        private System.Windows.Forms.ToolStripButton addOperationToolStripButton;
        private System.Windows.Forms.ToolStripButton editOperationToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteOperationToolStripButton;
        private System.Windows.Forms.TabPage locationsTabPage;
        private System.Windows.Forms.DataGridView locationsDataGridView;
        private System.Windows.Forms.ToolStrip locationsToolStrip;
        private System.Windows.Forms.ToolStripButton addLocationToolStripButton;
        private System.Windows.Forms.ToolStripButton editLocationToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteLocationToolStripButton;
        private System.Windows.Forms.TabPage usersTabPage;
        private System.Windows.Forms.DataGridView usersDataGridView;
        private System.Windows.Forms.ToolStrip usersToolStrip;
        private System.Windows.Forms.ToolStripButton addUserToolStripButton;
        private System.Windows.Forms.ToolStripButton editUserToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteUserToolStripButton;
        private System.Windows.Forms.Panel themePanel;
        private System.Windows.Forms.NumericUpDown fontSizeNumericUpDown;
        private System.Windows.Forms.Label fontSizeLabel;
        private System.Windows.Forms.ComboBox themeComboBox;
        private System.Windows.Forms.Label themeLabel;
        private System.Windows.Forms.Panel shortcutsPanel;
        private System.Windows.Forms.DataGridView shortcutsDataGridView;
        private System.Windows.Forms.Panel aboutPanel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label appNameLabel;
        private System.Windows.Forms.Panel addPartPanel;
        private System.Windows.Forms.Panel editPartPanel;
        private System.Windows.Forms.Panel removePartPanel;
    }
}