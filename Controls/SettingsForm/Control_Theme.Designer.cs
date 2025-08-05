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
            Control_Themes_GroupBox_Main = new GroupBox();
            Control_Shortcuts_TableLayout_Main = new TableLayoutPanel();
            Control_Shortcuts_Button_Switch = new Button();
            Control_Shortcuts_ComboBox_Theme = new ComboBox();
            Control_Shortcuts_Button_Save = new Button();
            Control_Themes_GroupBox_Main.SuspendLayout();
            Control_Shortcuts_TableLayout_Main.SuspendLayout();
            SuspendLayout();
            // 
            // Control_Themes_GroupBox_Main
            // 
            Control_Themes_GroupBox_Main.AutoSize = true;
            Control_Themes_GroupBox_Main.Controls.Add(Control_Shortcuts_TableLayout_Main);
            Control_Themes_GroupBox_Main.Dock = DockStyle.Fill;
            Control_Themes_GroupBox_Main.Location = new Point(0, 0);
            Control_Themes_GroupBox_Main.Name = "Control_Themes_GroupBox_Main";
            Control_Themes_GroupBox_Main.Size = new Size(277, 80);
            Control_Themes_GroupBox_Main.TabIndex = 3;
            Control_Themes_GroupBox_Main.TabStop = false;
            Control_Themes_GroupBox_Main.Text = "Select A Theme";
            // 
            // Control_Shortcuts_TableLayout_Main
            // 
            Control_Shortcuts_TableLayout_Main.AutoSize = true;
            Control_Shortcuts_TableLayout_Main.ColumnCount = 2;
            Control_Shortcuts_TableLayout_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_Shortcuts_TableLayout_Main.ColumnStyles.Add(new ColumnStyle());
            Control_Shortcuts_TableLayout_Main.Controls.Add(Control_Shortcuts_Button_Switch, 1, 0);
            Control_Shortcuts_TableLayout_Main.Controls.Add(Control_Shortcuts_ComboBox_Theme, 0, 0);
            Control_Shortcuts_TableLayout_Main.Controls.Add(Control_Shortcuts_Button_Save, 1, 2);
            Control_Shortcuts_TableLayout_Main.Dock = DockStyle.Fill;
            Control_Shortcuts_TableLayout_Main.Location = new Point(3, 19);
            Control_Shortcuts_TableLayout_Main.Name = "Control_Shortcuts_TableLayout_Main";
            Control_Shortcuts_TableLayout_Main.RowCount = 3;
            Control_Shortcuts_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_Shortcuts_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_Shortcuts_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_Shortcuts_TableLayout_Main.Size = new Size(271, 58);
            Control_Shortcuts_TableLayout_Main.TabIndex = 3;
            // 
            // Control_Shortcuts_Button_Switch
            // 
            Control_Shortcuts_Button_Switch.Location = new Point(193, 3);
            Control_Shortcuts_Button_Switch.Name = "Control_Shortcuts_Button_Switch";
            Control_Shortcuts_Button_Switch.Size = new Size(75, 23);
            Control_Shortcuts_Button_Switch.TabIndex = 2;
            Control_Shortcuts_Button_Switch.Text = "Preview";
            Control_Shortcuts_Button_Switch.UseVisualStyleBackColor = true;
            // 
            // Control_Shortcuts_ComboBox_Theme
            // 
            Control_Shortcuts_ComboBox_Theme.DropDownStyle = ComboBoxStyle.DropDownList;
            Control_Shortcuts_ComboBox_Theme.FormattingEnabled = true;
            Control_Shortcuts_ComboBox_Theme.Location = new Point(3, 3);
            Control_Shortcuts_ComboBox_Theme.Name = "Control_Shortcuts_ComboBox_Theme";
            Control_Shortcuts_ComboBox_Theme.Size = new Size(184, 23);
            Control_Shortcuts_ComboBox_Theme.TabIndex = 1;
            // 
            // Control_Shortcuts_Button_Save
            // 
            Control_Shortcuts_Button_Save.Location = new Point(193, 32);
            Control_Shortcuts_Button_Save.Name = "Control_Shortcuts_Button_Save";
            Control_Shortcuts_Button_Save.Size = new Size(75, 23);
            Control_Shortcuts_Button_Save.TabIndex = 3;
            Control_Shortcuts_Button_Save.Text = "Save";
            Control_Shortcuts_Button_Save.UseVisualStyleBackColor = true;
            // 
            // Control_Theme
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(Control_Themes_GroupBox_Main);
            Name = "Control_Theme";
            Size = new Size(277, 80);
            Control_Themes_GroupBox_Main.ResumeLayout(false);
            Control_Themes_GroupBox_Main.PerformLayout();
            Control_Shortcuts_TableLayout_Main.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox Control_Themes_GroupBox_Main;
        private TableLayoutPanel Control_Shortcuts_TableLayout_Main;
        private Button Control_Shortcuts_Button_Switch;
        private ComboBox Control_Shortcuts_ComboBox_Theme;
        private Button Control_Shortcuts_Button_Save;
    }
}
