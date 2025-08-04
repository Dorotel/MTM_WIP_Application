// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Theme
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SettingsForm_GroupBox_Theme = new GroupBox();
            SettingsForm_ComboBox_Theme = new ComboBox();
            SettingsForm_Button_SwitchTheme = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            SettingsForm_Button_SaveTheme = new Button();
            SettingsForm_GroupBox_Theme.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // SettingsForm_GroupBox_Theme
            // 
            SettingsForm_GroupBox_Theme.AutoSize = true;
            SettingsForm_GroupBox_Theme.Controls.Add(tableLayoutPanel1);
            SettingsForm_GroupBox_Theme.Dock = DockStyle.Fill;
            SettingsForm_GroupBox_Theme.Location = new Point(0, 0);
            SettingsForm_GroupBox_Theme.Name = "SettingsForm_GroupBox_Theme";
            SettingsForm_GroupBox_Theme.Size = new Size(277, 80);
            SettingsForm_GroupBox_Theme.TabIndex = 3;
            SettingsForm_GroupBox_Theme.TabStop = false;
            SettingsForm_GroupBox_Theme.Text = "Select A Theme";
            // 
            // SettingsForm_ComboBox_Theme
            // 
            SettingsForm_ComboBox_Theme.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsForm_ComboBox_Theme.FormattingEnabled = true;
            SettingsForm_ComboBox_Theme.Location = new Point(3, 3);
            SettingsForm_ComboBox_Theme.Name = "SettingsForm_ComboBox_Theme";
            SettingsForm_ComboBox_Theme.Size = new Size(184, 23);
            SettingsForm_ComboBox_Theme.TabIndex = 1;
            // 
            // SettingsForm_Button_SwitchTheme
            // 
            SettingsForm_Button_SwitchTheme.Location = new Point(193, 3);
            SettingsForm_Button_SwitchTheme.Name = "SettingsForm_Button_SwitchTheme";
            SettingsForm_Button_SwitchTheme.Size = new Size(75, 23);
            SettingsForm_Button_SwitchTheme.TabIndex = 2;
            SettingsForm_Button_SwitchTheme.Text = "Preview";
            SettingsForm_Button_SwitchTheme.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(SettingsForm_Button_SwitchTheme, 1, 0);
            tableLayoutPanel1.Controls.Add(SettingsForm_ComboBox_Theme, 0, 0);
            tableLayoutPanel1.Controls.Add(SettingsForm_Button_SaveTheme, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(271, 58);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // SettingsForm_Button_SaveTheme
            // 
            SettingsForm_Button_SaveTheme.Location = new Point(193, 32);
            SettingsForm_Button_SaveTheme.Name = "SettingsForm_Button_SaveTheme";
            SettingsForm_Button_SaveTheme.Size = new Size(75, 23);
            SettingsForm_Button_SaveTheme.TabIndex = 3;
            SettingsForm_Button_SaveTheme.Text = "Save";
            SettingsForm_Button_SaveTheme.UseVisualStyleBackColor = true;
            // 
            // Control_Theme
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(SettingsForm_GroupBox_Theme);
            Name = "Control_Theme";
            Size = new Size(277, 80);
            SettingsForm_GroupBox_Theme.ResumeLayout(false);
            SettingsForm_GroupBox_Theme.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox SettingsForm_GroupBox_Theme;
        private TableLayoutPanel tableLayoutPanel1;
        private Button SettingsForm_Button_SwitchTheme;
        private ComboBox SettingsForm_ComboBox_Theme;
        private Button SettingsForm_Button_SaveTheme;
    }
}
