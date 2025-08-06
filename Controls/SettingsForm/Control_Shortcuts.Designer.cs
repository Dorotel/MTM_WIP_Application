// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Shortcuts
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
            Control_Shortcuts_GroupBox_Main = new GroupBox();
            Control_Shortcuts_TableLayout_Main = new TableLayoutPanel();
            Control_Shortcuts_DataGridView_Shortcuts = new DataGridView();
            Control_Shortcuts_TableLayout_Bottom = new TableLayoutPanel();
            Control_Shortcuts_Button_Reset = new Button();
            Control_Shortcuts_Button_Save = new Button();
            Control_Shortcuts_GroupBox_Main.SuspendLayout();
            Control_Shortcuts_TableLayout_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_Shortcuts_DataGridView_Shortcuts).BeginInit();
            Control_Shortcuts_TableLayout_Bottom.SuspendLayout();
            SuspendLayout();
            // 
            // Control_Shortcuts_GroupBox_Main
            // 
            Control_Shortcuts_GroupBox_Main.AutoSize = true;
            Control_Shortcuts_GroupBox_Main.Controls.Add(Control_Shortcuts_TableLayout_Main);
            Control_Shortcuts_GroupBox_Main.Dock = DockStyle.Fill;
            Control_Shortcuts_GroupBox_Main.Location = new Point(0, 0);
            Control_Shortcuts_GroupBox_Main.Name = "Control_Shortcuts_GroupBox_Main";
            Control_Shortcuts_GroupBox_Main.Size = new Size(635, 389);
            Control_Shortcuts_GroupBox_Main.TabIndex = 0;
            Control_Shortcuts_GroupBox_Main.TabStop = false;
            Control_Shortcuts_GroupBox_Main.Text = "Edit Shortcuts";
            // 
            // Control_Shortcuts_TableLayout_Main
            // 
            Control_Shortcuts_TableLayout_Main.AutoSize = true;
            Control_Shortcuts_TableLayout_Main.ColumnCount = 1;
            Control_Shortcuts_TableLayout_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_Shortcuts_TableLayout_Main.Controls.Add(Control_Shortcuts_DataGridView_Shortcuts, 0, 0);
            Control_Shortcuts_TableLayout_Main.Controls.Add(Control_Shortcuts_TableLayout_Bottom, 0, 1);
            Control_Shortcuts_TableLayout_Main.Dock = DockStyle.Fill;
            Control_Shortcuts_TableLayout_Main.Location = new Point(3, 19);
            Control_Shortcuts_TableLayout_Main.Name = "Control_Shortcuts_TableLayout_Main";
            Control_Shortcuts_TableLayout_Main.RowCount = 2;
            Control_Shortcuts_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_Shortcuts_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_Shortcuts_TableLayout_Main.Size = new Size(629, 367);
            Control_Shortcuts_TableLayout_Main.TabIndex = 0;
            // 
            // Control_Shortcuts_DataGridView_Shortcuts
            // 
            Control_Shortcuts_DataGridView_Shortcuts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Control_Shortcuts_DataGridView_Shortcuts.Dock = DockStyle.Fill;
            Control_Shortcuts_DataGridView_Shortcuts.Location = new Point(3, 3);
            Control_Shortcuts_DataGridView_Shortcuts.Name = "Control_Shortcuts_DataGridView_Shortcuts";
            Control_Shortcuts_DataGridView_Shortcuts.Size = new Size(623, 326);
            Control_Shortcuts_DataGridView_Shortcuts.TabIndex = 1;
            // 
            // Control_Shortcuts_TableLayout_Bottom
            // 
            Control_Shortcuts_TableLayout_Bottom.AutoSize = true;
            Control_Shortcuts_TableLayout_Bottom.ColumnCount = 3;
            Control_Shortcuts_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_Shortcuts_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_Shortcuts_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_Shortcuts_TableLayout_Bottom.Controls.Add(Control_Shortcuts_Button_Reset, 1, 0);
            Control_Shortcuts_TableLayout_Bottom.Controls.Add(Control_Shortcuts_Button_Save, 2, 0);
            Control_Shortcuts_TableLayout_Bottom.Dock = DockStyle.Fill;
            Control_Shortcuts_TableLayout_Bottom.Location = new Point(3, 335);
            Control_Shortcuts_TableLayout_Bottom.Name = "Control_Shortcuts_TableLayout_Bottom";
            Control_Shortcuts_TableLayout_Bottom.RowCount = 1;
            Control_Shortcuts_TableLayout_Bottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_Shortcuts_TableLayout_Bottom.Size = new Size(623, 29);
            Control_Shortcuts_TableLayout_Bottom.TabIndex = 2;
            // 
            // Control_Shortcuts_Button_Reset
            // 
            Control_Shortcuts_Button_Reset.Location = new Point(464, 3);
            Control_Shortcuts_Button_Reset.Name = "Control_Shortcuts_Button_Reset";
            Control_Shortcuts_Button_Reset.Size = new Size(75, 23);
            Control_Shortcuts_Button_Reset.TabIndex = 0;
            Control_Shortcuts_Button_Reset.Text = "Reset";
            Control_Shortcuts_Button_Reset.UseVisualStyleBackColor = true;
            // 
            // Control_Shortcuts_Button_Save
            // 
            Control_Shortcuts_Button_Save.Location = new Point(545, 3);
            Control_Shortcuts_Button_Save.Name = "Control_Shortcuts_Button_Save";
            Control_Shortcuts_Button_Save.Size = new Size(75, 23);
            Control_Shortcuts_Button_Save.TabIndex = 1;
            Control_Shortcuts_Button_Save.Text = "Save";
            Control_Shortcuts_Button_Save.UseVisualStyleBackColor = true;
            // 
            // Control_Shortcuts
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Control_Shortcuts_GroupBox_Main);
            Name = "Control_Shortcuts";
            Size = new Size(635, 389);
            Control_Shortcuts_GroupBox_Main.ResumeLayout(false);
            Control_Shortcuts_GroupBox_Main.PerformLayout();
            Control_Shortcuts_TableLayout_Main.ResumeLayout(false);
            Control_Shortcuts_TableLayout_Main.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Control_Shortcuts_DataGridView_Shortcuts).EndInit();
            Control_Shortcuts_TableLayout_Bottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox Control_Shortcuts_GroupBox_Main;
        private TableLayoutPanel Control_Shortcuts_TableLayout_Main;
        private DataGridView Control_Shortcuts_DataGridView_Shortcuts;
        private TableLayoutPanel Control_Shortcuts_TableLayout_Bottom;
        private Button Control_Shortcuts_Button_Reset;
        private Button Control_Shortcuts_Button_Save;
    }
}
