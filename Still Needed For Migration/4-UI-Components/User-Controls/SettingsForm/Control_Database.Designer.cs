// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Database
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
            Control_Database_GroupBox_Main = new GroupBox();
            Control_Database_TableLayout_Main = new TableLayoutPanel();
            Control_Database_TableLayout_Top = new TableLayoutPanel();
            Control_Database_Label_Port = new Label();
            Control_Database_TextBox_Port = new TextBox();
            Control_Database_TextBox_Database = new TextBox();
            Control_Database_Label_Database = new Label();
            Control_Database_Label_Server = new Label();
            Control_Database_TextBox_Server = new TextBox();
            Control_Database_TableLayout_Bottom = new TableLayoutPanel();
            Control_Database_Button_Reset = new Button();
            Control_Database_Button_Save = new Button();
            Control_Database_GroupBox_Main.SuspendLayout();
            Control_Database_TableLayout_Main.SuspendLayout();
            Control_Database_TableLayout_Top.SuspendLayout();
            Control_Database_TableLayout_Bottom.SuspendLayout();
            SuspendLayout();
            // 
            // Control_Database_GroupBox_Main
            // 
            Control_Database_GroupBox_Main.AutoSize = true;
            Control_Database_GroupBox_Main.Controls.Add(Control_Database_TableLayout_Main);
            Control_Database_GroupBox_Main.Dock = DockStyle.Fill;
            Control_Database_GroupBox_Main.Location = new Point(0, 0);
            Control_Database_GroupBox_Main.Name = "Control_Database_GroupBox_Main";
            Control_Database_GroupBox_Main.Size = new Size(412, 150);
            Control_Database_GroupBox_Main.TabIndex = 8;
            Control_Database_GroupBox_Main.TabStop = false;
            Control_Database_GroupBox_Main.Text = "Database Settings";
            // 
            // Control_Database_TableLayout_Main
            // 
            Control_Database_TableLayout_Main.AutoSize = true;
            Control_Database_TableLayout_Main.ColumnCount = 1;
            Control_Database_TableLayout_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_Database_TableLayout_Main.Controls.Add(Control_Database_TableLayout_Bottom, 0, 1);
            Control_Database_TableLayout_Main.Controls.Add(Control_Database_TableLayout_Top, 0, 0);
            Control_Database_TableLayout_Main.Dock = DockStyle.Fill;
            Control_Database_TableLayout_Main.Location = new Point(3, 19);
            Control_Database_TableLayout_Main.Name = "Control_Database_TableLayout_Main";
            Control_Database_TableLayout_Main.RowCount = 2;
            Control_Database_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_Database_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_Database_TableLayout_Main.Size = new Size(406, 128);
            Control_Database_TableLayout_Main.TabIndex = 3;
            // 
            // Control_Database_TableLayout_Top
            // 
            Control_Database_TableLayout_Top.AutoSize = true;
            Control_Database_TableLayout_Top.ColumnCount = 2;
            Control_Database_TableLayout_Top.ColumnStyles.Add(new ColumnStyle());
            Control_Database_TableLayout_Top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_Database_TableLayout_Top.Controls.Add(Control_Database_Label_Port, 0, 2);
            Control_Database_TableLayout_Top.Controls.Add(Control_Database_TextBox_Port, 1, 2);
            Control_Database_TableLayout_Top.Controls.Add(Control_Database_TextBox_Database, 1, 1);
            Control_Database_TableLayout_Top.Controls.Add(Control_Database_Label_Database, 0, 1);
            Control_Database_TableLayout_Top.Controls.Add(Control_Database_Label_Server, 0, 0);
            Control_Database_TableLayout_Top.Controls.Add(Control_Database_TextBox_Server, 1, 0);
            Control_Database_TableLayout_Top.Dock = DockStyle.Fill;
            Control_Database_TableLayout_Top.Location = new Point(3, 3);
            Control_Database_TableLayout_Top.MinimumSize = new Size(400, 87);
            Control_Database_TableLayout_Top.Name = "Control_Database_TableLayout_Top";
            Control_Database_TableLayout_Top.RowCount = 4;
            Control_Database_TableLayout_Top.RowStyles.Add(new RowStyle());
            Control_Database_TableLayout_Top.RowStyles.Add(new RowStyle());
            Control_Database_TableLayout_Top.RowStyles.Add(new RowStyle());
            Control_Database_TableLayout_Top.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_Database_TableLayout_Top.Size = new Size(400, 87);
            Control_Database_TableLayout_Top.TabIndex = 11;
            // 
            // Control_Database_Label_Port
            // 
            Control_Database_Label_Port.AutoSize = true;
            Control_Database_Label_Port.Dock = DockStyle.Fill;
            Control_Database_Label_Port.Location = new Point(3, 58);
            Control_Database_Label_Port.Name = "Control_Database_Label_Port";
            Control_Database_Label_Port.Size = new Size(93, 29);
            Control_Database_Label_Port.TabIndex = 2;
            Control_Database_Label_Port.Text = "Port:";
            Control_Database_Label_Port.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Database_TextBox_Port
            // 
            Control_Database_TextBox_Port.Dock = DockStyle.Fill;
            Control_Database_TextBox_Port.Location = new Point(102, 61);
            Control_Database_TextBox_Port.Name = "Control_Database_TextBox_Port";
            Control_Database_TextBox_Port.PlaceholderText = "Enter Port Number";
            Control_Database_TextBox_Port.Size = new Size(295, 23);
            Control_Database_TextBox_Port.TabIndex = 3;
            // 
            // Control_Database_TextBox_Database
            // 
            Control_Database_TextBox_Database.Dock = DockStyle.Fill;
            Control_Database_TextBox_Database.Location = new Point(102, 32);
            Control_Database_TextBox_Database.Name = "Control_Database_TextBox_Database";
            Control_Database_TextBox_Database.PlaceholderText = "Enter Database Name";
            Control_Database_TextBox_Database.Size = new Size(295, 23);
            Control_Database_TextBox_Database.TabIndex = 5;
            // 
            // Control_Database_Label_Database
            // 
            Control_Database_Label_Database.AutoSize = true;
            Control_Database_Label_Database.Dock = DockStyle.Fill;
            Control_Database_Label_Database.Location = new Point(3, 29);
            Control_Database_Label_Database.Name = "Control_Database_Label_Database";
            Control_Database_Label_Database.Size = new Size(93, 29);
            Control_Database_Label_Database.TabIndex = 4;
            Control_Database_Label_Database.Text = "Database Name:";
            Control_Database_Label_Database.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Database_Label_Server
            // 
            Control_Database_Label_Server.AutoSize = true;
            Control_Database_Label_Server.Dock = DockStyle.Fill;
            Control_Database_Label_Server.Location = new Point(3, 0);
            Control_Database_Label_Server.Name = "Control_Database_Label_Server";
            Control_Database_Label_Server.Size = new Size(93, 29);
            Control_Database_Label_Server.TabIndex = 0;
            Control_Database_Label_Server.Text = "Server Address:";
            Control_Database_Label_Server.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Database_TextBox_Server
            // 
            Control_Database_TextBox_Server.Dock = DockStyle.Fill;
            Control_Database_TextBox_Server.Location = new Point(102, 3);
            Control_Database_TextBox_Server.Name = "Control_Database_TextBox_Server";
            Control_Database_TextBox_Server.PlaceholderText = "Enter Server Address";
            Control_Database_TextBox_Server.Size = new Size(295, 23);
            Control_Database_TextBox_Server.TabIndex = 1;
            // 
            // Control_Database_TableLayout_Bottom
            // 
            Control_Database_TableLayout_Bottom.AutoSize = true;
            Control_Database_TableLayout_Bottom.ColumnCount = 3;
            Control_Database_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_Database_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_Database_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_Database_TableLayout_Bottom.Controls.Add(Control_Database_Button_Reset, 2, 0);
            Control_Database_TableLayout_Bottom.Controls.Add(Control_Database_Button_Save, 1, 0);
            Control_Database_TableLayout_Bottom.Dock = DockStyle.Fill;
            Control_Database_TableLayout_Bottom.Location = new Point(3, 96);
            Control_Database_TableLayout_Bottom.Name = "Control_Database_TableLayout_Bottom";
            Control_Database_TableLayout_Bottom.RowCount = 1;
            Control_Database_TableLayout_Bottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_Database_TableLayout_Bottom.Size = new Size(400, 29);
            Control_Database_TableLayout_Bottom.TabIndex = 12;
            // 
            // Control_Database_Button_Reset
            // 
            Control_Database_Button_Reset.Location = new Point(322, 3);
            Control_Database_Button_Reset.Name = "Control_Database_Button_Reset";
            Control_Database_Button_Reset.Size = new Size(75, 23);
            Control_Database_Button_Reset.TabIndex = 0;
            Control_Database_Button_Reset.Text = "Save";
            Control_Database_Button_Reset.UseVisualStyleBackColor = true;
            // 
            // Control_Database_Button_Save
            // 
            Control_Database_Button_Save.Location = new Point(241, 3);
            Control_Database_Button_Save.Name = "Control_Database_Button_Save";
            Control_Database_Button_Save.Size = new Size(75, 23);
            Control_Database_Button_Save.TabIndex = 1;
            Control_Database_Button_Save.Text = "Reset";
            Control_Database_Button_Save.UseVisualStyleBackColor = true;
            // 
            // Control_Database
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(Control_Database_GroupBox_Main);
            Name = "Control_Database";
            Size = new Size(412, 150);
            Control_Database_GroupBox_Main.ResumeLayout(false);
            Control_Database_GroupBox_Main.PerformLayout();
            Control_Database_TableLayout_Main.ResumeLayout(false);
            Control_Database_TableLayout_Main.PerformLayout();
            Control_Database_TableLayout_Top.ResumeLayout(false);
            Control_Database_TableLayout_Top.PerformLayout();
            Control_Database_TableLayout_Bottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox Control_Database_GroupBox_Main;
        private TableLayoutPanel Control_Database_TableLayout_Main;
        private TableLayoutPanel Control_Database_TableLayout_Bottom;
        private Button Control_Database_Button_Reset;
        private Button Control_Database_Button_Save;
        private TableLayoutPanel Control_Database_TableLayout_Top;
        private Label Control_Database_Label_Port;
        private TextBox Control_Database_TextBox_Port;
        private TextBox Control_Database_TextBox_Database;
        private Label Control_Database_Label_Database;
        private Label Control_Database_Label_Server;
        private TextBox Control_Database_TextBox_Server;
    }
}
