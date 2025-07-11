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
        /// Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            categoryTreeView = new TreeView();
            settingsPanel = new Panel();
            bottomPanel = new Panel();
            statusLabel = new Label();
            cancelButton = new Button();
            saveButton = new Button();
            resetDefaultsButton = new Button();
            databasePanel = new Panel();
            databaseTabControl = new TabControl();
            connectionTabPage = new TabPage();
            connectionGroupBox = new GroupBox();
            passwordTextBox = new TextBox();
            passwordLabel = new Label();
            usernameTextBox = new TextBox();
            usernameLabel = new Label();
            databaseTextBox = new TextBox();
            databaseLabel = new Label();
            portTextBox = new TextBox();
            portLabel = new Label();
            serverTextBox = new TextBox();
            serverLabel = new Label();
            themePanel = new Panel();
            fontSizeNumericUpDown = new NumericUpDown();
            fontSizeLabel = new Label();
            themeComboBox = new ComboBox();
            themeLabel = new Label();
            shortcutsPanel = new Panel();
            shortcutsDataGridView = new DataGridView();
            aboutPanel = new Panel();
            versionLabel = new Label();
            appNameLabel = new Label();
            addPartPanel = new Panel();
            editPartPanel = new Panel();
            removePartPanel = new Panel();
            addOperationPanel = new Panel();
            editOperationPanel = new Panel();
            removeOperationPanel = new Panel();
            addLocationPanel = new Panel();
            editLocationPanel = new Panel();
            removeLocationPanel = new Panel();
            addItemTypePanel = new Panel();
            editItemTypePanel = new Panel();
            removeItemTypePanel = new Panel();
            addUserPanel = new Panel();
            editUserPanel = new Panel();
            deleteUserPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            bottomPanel.SuspendLayout();
            databasePanel.SuspendLayout();
            databaseTabControl.SuspendLayout();
            connectionTabPage.SuspendLayout();
            connectionGroupBox.SuspendLayout();
            themePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fontSizeNumericUpDown).BeginInit();
            shortcutsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)shortcutsDataGridView).BeginInit();
            aboutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(categoryTreeView);
            splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(settingsPanel);
            splitContainer1.Panel2.Controls.Add(bottomPanel);
            splitContainer1.Size = new Size(838, 492);
            splitContainer1.SplitterDistance = 200;
            splitContainer1.TabIndex = 0;
            // 
            // categoryTreeView
            // 
            categoryTreeView.Dock = DockStyle.Fill;
            categoryTreeView.Location = new Point(0, 0);
            categoryTreeView.Name = "categoryTreeView";
            categoryTreeView.Size = new Size(200, 492);
            categoryTreeView.TabIndex = 0;
            categoryTreeView.AfterSelect += CategoryTreeView_AfterSelect;
            // 
            // settingsPanel
            // 
            settingsPanel.Dock = DockStyle.Fill;
            settingsPanel.Location = new Point(0, 0);
            settingsPanel.Name = "settingsPanel";
            settingsPanel.Size = new Size(634, 452);
            settingsPanel.TabIndex = 1;
            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(statusLabel);
            bottomPanel.Controls.Add(cancelButton);
            bottomPanel.Controls.Add(saveButton);
            bottomPanel.Controls.Add(resetDefaultsButton);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 452);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(634, 40);
            bottomPanel.TabIndex = 0;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(12, 13);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(39, 15);
            statusLabel.TabIndex = 2;
            statusLabel.Text = "Ready";
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cancelButton.Location = new Point(471, 9);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // saveButton
            // 
            saveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            saveButton.Location = new Point(552, 9);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 0;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // resetDefaultsButton
            // 
            resetDefaultsButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            resetDefaultsButton.Location = new Point(390, 9);
            resetDefaultsButton.Name = "resetDefaultsButton";
            resetDefaultsButton.Size = new Size(75, 23);
            resetDefaultsButton.TabIndex = 3;
            resetDefaultsButton.Text = "Reset";
            resetDefaultsButton.UseVisualStyleBackColor = true;
            resetDefaultsButton.Click += resetDefaultsButton_Click;
            // 
            // databasePanel
            // 
            databasePanel.Controls.Add(databaseTabControl);
            databasePanel.Dock = DockStyle.Fill;
            databasePanel.Location = new Point(0, 0);
            databasePanel.Name = "databasePanel";
            databasePanel.Size = new Size(838, 492);
            databasePanel.TabIndex = 2;
            databasePanel.Visible = false;
            // 
            // databaseTabControl
            // 
            databaseTabControl.Controls.Add(connectionTabPage);
            databaseTabControl.Dock = DockStyle.Fill;
            databaseTabControl.Location = new Point(0, 0);
            databaseTabControl.Name = "databaseTabControl";
            databaseTabControl.SelectedIndex = 0;
            databaseTabControl.Size = new Size(838, 492);
            databaseTabControl.TabIndex = 0;
            // 
            // connectionTabPage
            // 
            connectionTabPage.Controls.Add(connectionGroupBox);
            connectionTabPage.Location = new Point(4, 24);
            connectionTabPage.Name = "connectionTabPage";
            connectionTabPage.Padding = new Padding(3);
            connectionTabPage.Size = new Size(830, 464);
            connectionTabPage.TabIndex = 0;
            connectionTabPage.Text = "Connection Settings";
            connectionTabPage.UseVisualStyleBackColor = true;
            // 
            // connectionGroupBox
            // 
            connectionGroupBox.Controls.Add(passwordTextBox);
            connectionGroupBox.Controls.Add(passwordLabel);
            connectionGroupBox.Controls.Add(usernameTextBox);
            connectionGroupBox.Controls.Add(usernameLabel);
            connectionGroupBox.Controls.Add(databaseTextBox);
            connectionGroupBox.Controls.Add(databaseLabel);
            connectionGroupBox.Controls.Add(portTextBox);
            connectionGroupBox.Controls.Add(portLabel);
            connectionGroupBox.Controls.Add(serverTextBox);
            connectionGroupBox.Controls.Add(serverLabel);
            connectionGroupBox.Location = new Point(6, 6);
            connectionGroupBox.Name = "connectionGroupBox";
            connectionGroupBox.Size = new Size(576, 200);
            connectionGroupBox.TabIndex = 0;
            connectionGroupBox.TabStop = false;
            connectionGroupBox.Text = "Database Configuration";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(150, 111);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(200, 23);
            passwordTextBox.TabIndex = 9;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(15, 114);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(60, 15);
            passwordLabel.TabIndex = 8;
            passwordLabel.Text = "Password:";
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(150, 82);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(200, 23);
            usernameTextBox.TabIndex = 7;
            // 
            // usernameLabel
            // 
            usernameLabel.AutoSize = true;
            usernameLabel.Location = new Point(15, 85);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new Size(63, 15);
            usernameLabel.TabIndex = 6;
            usernameLabel.Text = "Username:";
            // 
            // databaseTextBox
            // 
            databaseTextBox.Location = new Point(150, 53);
            databaseTextBox.Name = "databaseTextBox";
            databaseTextBox.Size = new Size(200, 23);
            databaseTextBox.TabIndex = 5;
            // 
            // databaseLabel
            // 
            databaseLabel.AutoSize = true;
            databaseLabel.Location = new Point(15, 56);
            databaseLabel.Name = "databaseLabel";
            databaseLabel.Size = new Size(93, 15);
            databaseLabel.TabIndex = 4;
            databaseLabel.Text = "Database Name:";
            // 
            // portTextBox
            // 
            portTextBox.Location = new Point(410, 24);
            portTextBox.Name = "portTextBox";
            portTextBox.Size = new Size(100, 23);
            portTextBox.TabIndex = 3;
            // 
            // portLabel
            // 
            portLabel.AutoSize = true;
            portLabel.Location = new Point(375, 27);
            portLabel.Name = "portLabel";
            portLabel.Size = new Size(32, 15);
            portLabel.TabIndex = 2;
            portLabel.Text = "Port:";
            // 
            // serverTextBox
            // 
            serverTextBox.Location = new Point(150, 24);
            serverTextBox.Name = "serverTextBox";
            serverTextBox.Size = new Size(200, 23);
            serverTextBox.TabIndex = 1;
            // 
            // serverLabel
            // 
            serverLabel.AutoSize = true;
            serverLabel.Location = new Point(15, 27);
            serverLabel.Name = "serverLabel";
            serverLabel.Size = new Size(87, 15);
            serverLabel.TabIndex = 0;
            serverLabel.Text = "Server Address:";
            // 
            // themePanel
            // 
            themePanel.Controls.Add(fontSizeNumericUpDown);
            themePanel.Controls.Add(fontSizeLabel);
            themePanel.Controls.Add(themeComboBox);
            themePanel.Controls.Add(themeLabel);
            themePanel.Dock = DockStyle.Fill;
            themePanel.Location = new Point(0, 0);
            themePanel.Name = "themePanel";
            themePanel.Size = new Size(838, 492);
            themePanel.TabIndex = 3;
            themePanel.Visible = false;
            // 
            // fontSizeNumericUpDown
            // 
            fontSizeNumericUpDown.Location = new Point(100, 50);
            fontSizeNumericUpDown.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            fontSizeNumericUpDown.Minimum = new decimal(new int[] { 8, 0, 0, 0 });
            fontSizeNumericUpDown.Name = "fontSizeNumericUpDown";
            fontSizeNumericUpDown.Size = new Size(120, 23);
            fontSizeNumericUpDown.TabIndex = 3;
            fontSizeNumericUpDown.Value = new decimal(new int[] { 9, 0, 0, 0 });
            // 
            // fontSizeLabel
            // 
            fontSizeLabel.AutoSize = true;
            fontSizeLabel.Location = new Point(30, 52);
            fontSizeLabel.Name = "fontSizeLabel";
            fontSizeLabel.Size = new Size(57, 15);
            fontSizeLabel.TabIndex = 2;
            fontSizeLabel.Text = "Font Size:";
            // 
            // themeComboBox
            // 
            themeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            themeComboBox.FormattingEnabled = true;
            themeComboBox.Location = new Point(100, 20);
            themeComboBox.Name = "themeComboBox";
            themeComboBox.Size = new Size(200, 23);
            themeComboBox.TabIndex = 1;
            // 
            // themeLabel
            // 
            themeLabel.AutoSize = true;
            themeLabel.Location = new Point(30, 23);
            themeLabel.Name = "themeLabel";
            themeLabel.Size = new Size(46, 15);
            themeLabel.TabIndex = 0;
            themeLabel.Text = "Theme:";
            // 
            // shortcutsPanel
            // 
            shortcutsPanel.Controls.Add(shortcutsDataGridView);
            shortcutsPanel.Dock = DockStyle.Fill;
            shortcutsPanel.Location = new Point(0, 0);
            shortcutsPanel.Name = "shortcutsPanel";
            shortcutsPanel.Size = new Size(838, 492);
            shortcutsPanel.TabIndex = 4;
            shortcutsPanel.Visible = false;
            // 
            // shortcutsDataGridView
            // 
            shortcutsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            shortcutsDataGridView.Dock = DockStyle.Fill;
            shortcutsDataGridView.Location = new Point(0, 0);
            shortcutsDataGridView.Name = "shortcutsDataGridView";
            shortcutsDataGridView.Size = new Size(838, 492);
            shortcutsDataGridView.TabIndex = 0;
            // 
            // aboutPanel
            // 
            aboutPanel.Controls.Add(versionLabel);
            aboutPanel.Controls.Add(appNameLabel);
            aboutPanel.Dock = DockStyle.Fill;
            aboutPanel.Location = new Point(0, 0);
            aboutPanel.Name = "aboutPanel";
            aboutPanel.Size = new Size(838, 492);
            aboutPanel.TabIndex = 5;
            aboutPanel.Visible = false;
            // 
            // versionLabel
            // 
            versionLabel.AutoSize = true;
            versionLabel.Location = new Point(30, 50);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(48, 15);
            versionLabel.TabIndex = 1;
            versionLabel.Text = "Version:";
            // 
            // appNameLabel
            // 
            appNameLabel.AutoSize = true;
            appNameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            appNameLabel.Location = new Point(30, 20);
            appNameLabel.Name = "appNameLabel";
            appNameLabel.Size = new Size(177, 21);
            appNameLabel.TabIndex = 0;
            appNameLabel.Text = "MTM WIP Application";
            // 
            // addPartPanel
            // 
            addPartPanel.Dock = DockStyle.Fill;
            addPartPanel.Location = new Point(0, 0);
            addPartPanel.Name = "addPartPanel";
            addPartPanel.Size = new Size(838, 492);
            addPartPanel.TabIndex = 6;
            addPartPanel.Visible = false;
            // 
            // editPartPanel
            // 
            editPartPanel.Dock = DockStyle.Fill;
            editPartPanel.Location = new Point(0, 0);
            editPartPanel.Name = "editPartPanel";
            editPartPanel.Size = new Size(838, 492);
            editPartPanel.TabIndex = 7;
            editPartPanel.Visible = false;
            // 
            // removePartPanel
            // 
            removePartPanel.Dock = DockStyle.Fill;
            removePartPanel.Location = new Point(0, 0);
            removePartPanel.Name = "removePartPanel";
            removePartPanel.Size = new Size(838, 492);
            removePartPanel.TabIndex = 8;
            removePartPanel.Visible = false;
            // 
            // addOperationPanel
            // 
            addOperationPanel.Dock = DockStyle.Fill;
            addOperationPanel.Location = new Point(0, 0);
            addOperationPanel.Name = "addOperationPanel";
            addOperationPanel.Size = new Size(838, 492);
            addOperationPanel.TabIndex = 9;
            addOperationPanel.Visible = false;
            // 
            // editOperationPanel
            // 
            editOperationPanel.Dock = DockStyle.Fill;
            editOperationPanel.Location = new Point(0, 0);
            editOperationPanel.Name = "editOperationPanel";
            editOperationPanel.Size = new Size(838, 492);
            editOperationPanel.TabIndex = 10;
            editOperationPanel.Visible = false;
            // 
            // removeOperationPanel
            // 
            removeOperationPanel.Dock = DockStyle.Fill;
            removeOperationPanel.Location = new Point(0, 0);
            removeOperationPanel.Name = "removeOperationPanel";
            removeOperationPanel.Size = new Size(838, 492);
            removeOperationPanel.TabIndex = 11;
            removeOperationPanel.Visible = false;
            // 
            // addLocationPanel
            // 
            addLocationPanel.Dock = DockStyle.Fill;
            addLocationPanel.Location = new Point(0, 0);
            addLocationPanel.Name = "addLocationPanel";
            addLocationPanel.Size = new Size(838, 492);
            addLocationPanel.TabIndex = 12;
            addLocationPanel.Visible = false;
            // 
            // editLocationPanel
            // 
            editLocationPanel.Dock = DockStyle.Fill;
            editLocationPanel.Location = new Point(0, 0);
            editLocationPanel.Name = "editLocationPanel";
            editLocationPanel.Size = new Size(838, 492);
            editLocationPanel.TabIndex = 13;
            editLocationPanel.Visible = false;
            // 
            // removeLocationPanel
            // 
            removeLocationPanel.Dock = DockStyle.Fill;
            removeLocationPanel.Location = new Point(0, 0);
            removeLocationPanel.Name = "removeLocationPanel";
            removeLocationPanel.Size = new Size(838, 492);
            removeLocationPanel.TabIndex = 14;
            removeLocationPanel.Visible = false;
            // 
            // addItemTypePanel
            // 
            addItemTypePanel.Dock = DockStyle.Fill;
            addItemTypePanel.Location = new Point(0, 0);
            addItemTypePanel.Name = "addItemTypePanel";
            addItemTypePanel.Size = new Size(838, 492);
            addItemTypePanel.TabIndex = 15;
            addItemTypePanel.Visible = false;
            // 
            // editItemTypePanel
            // 
            editItemTypePanel.Dock = DockStyle.Fill;
            editItemTypePanel.Location = new Point(0, 0);
            editItemTypePanel.Name = "editItemTypePanel";
            editItemTypePanel.Size = new Size(838, 492);
            editItemTypePanel.TabIndex = 16;
            editItemTypePanel.Visible = false;
            // 
            // removeItemTypePanel
            // 
            removeItemTypePanel.Dock = DockStyle.Fill;
            removeItemTypePanel.Location = new Point(0, 0);
            removeItemTypePanel.Name = "removeItemTypePanel";
            removeItemTypePanel.Size = new Size(838, 492);
            removeItemTypePanel.TabIndex = 17;
            removeItemTypePanel.Visible = false;
            // 
            // addUserPanel
            // 
            addUserPanel.Dock = DockStyle.Fill;
            addUserPanel.Location = new Point(0, 0);
            addUserPanel.Name = "addUserPanel";
            addUserPanel.Size = new Size(838, 492);
            addUserPanel.TabIndex = 18;
            addUserPanel.Visible = false;
            // 
            // editUserPanel
            // 
            editUserPanel.Dock = DockStyle.Fill;
            editUserPanel.Location = new Point(0, 0);
            editUserPanel.Name = "editUserPanel";
            editUserPanel.Size = new Size(838, 492);
            editUserPanel.TabIndex = 19;
            editUserPanel.Visible = false;
            // 
            // deleteUserPanel
            // 
            deleteUserPanel.Dock = DockStyle.Fill;
            deleteUserPanel.Location = new Point(0, 0);
            deleteUserPanel.Name = "deleteUserPanel";
            deleteUserPanel.Size = new Size(838, 492);
            deleteUserPanel.TabIndex = 20;
            deleteUserPanel.Visible = false;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(838, 492);
            Controls.Add(deleteUserPanel);
            Controls.Add(editUserPanel);
            Controls.Add(addUserPanel);
            Controls.Add(removeItemTypePanel);
            Controls.Add(editItemTypePanel);
            Controls.Add(addItemTypePanel);
            Controls.Add(removeLocationPanel);
            Controls.Add(editLocationPanel);
            Controls.Add(addLocationPanel);
            Controls.Add(removeOperationPanel);
            Controls.Add(editOperationPanel);
            Controls.Add(addOperationPanel);
            Controls.Add(removePartPanel);
            Controls.Add(editPartPanel);
            Controls.Add(addPartPanel);
            Controls.Add(aboutPanel);
            Controls.Add(shortcutsPanel);
            Controls.Add(themePanel);
            Controls.Add(databasePanel);
            Controls.Add(splitContainer1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings - MTM WIP Application";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            bottomPanel.ResumeLayout(false);
            bottomPanel.PerformLayout();
            databasePanel.ResumeLayout(false);
            databaseTabControl.ResumeLayout(false);
            connectionTabPage.ResumeLayout(false);
            connectionGroupBox.ResumeLayout(false);
            connectionGroupBox.PerformLayout();
            themePanel.ResumeLayout(false);
            themePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)fontSizeNumericUpDown).EndInit();
            shortcutsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)shortcutsDataGridView).EndInit();
            aboutPanel.ResumeLayout(false);
            aboutPanel.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView categoryTreeView;
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
        private System.Windows.Forms.Panel addOperationPanel;
        private System.Windows.Forms.Panel editOperationPanel;
        private System.Windows.Forms.Panel removeOperationPanel;
        private System.Windows.Forms.Panel addLocationPanel;
        private System.Windows.Forms.Panel editLocationPanel;
        private System.Windows.Forms.Panel removeLocationPanel;
        private System.Windows.Forms.Panel addItemTypePanel;
        private System.Windows.Forms.Panel editItemTypePanel;
        private System.Windows.Forms.Panel removeItemTypePanel;
        private System.Windows.Forms.Panel addUserPanel;
        private System.Windows.Forms.Panel editUserPanel;
        private System.Windows.Forms.Panel deleteUserPanel;
    }
}