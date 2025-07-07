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