namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Add_Operation
    {
        #region Fields

        private System.ComponentModel.IContainer components = null;

        #endregion
        private System.Windows.Forms.Label Control_Add_Operation_Label_Operation;
        private System.Windows.Forms.TextBox Control_Add_Operation_TextBox_Operation;
        private System.Windows.Forms.Label Control_Add_Operation_Label_IssuedBy;
        private System.Windows.Forms.Label Control_Add_Operation_Label_IssuedByValue;
        private System.Windows.Forms.Button Control_Add_Operation_Button_Save;
        private System.Windows.Forms.Button Control_Add_Operation_Button_Clear;

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
            Control_Add_Operation_Label_Operation = new Label();
            Control_Add_Operation_TextBox_Operation = new TextBox();
            Control_Add_Operation_Label_IssuedBy = new Label();
            Control_Add_Operation_Label_IssuedByValue = new Label();
            Control_Add_Operation_Button_Save = new Button();
            Control_Add_Operation_Button_Clear = new Button();
            groupBox1 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // Control_Add_Operation_Label_Operation
            // 
            Control_Add_Operation_Label_Operation.AutoSize = true;
            Control_Add_Operation_Label_Operation.Dock = DockStyle.Fill;
            Control_Add_Operation_Label_Operation.Location = new Point(3, 0);
            Control_Add_Operation_Label_Operation.Name = "Control_Add_Operation_Label_Operation";
            Control_Add_Operation_Label_Operation.Size = new Size(120, 29);
            Control_Add_Operation_Label_Operation.TabIndex = 1;
            Control_Add_Operation_Label_Operation.Text = "Operation Number:";
            Control_Add_Operation_Label_Operation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Add_Operation_TextBox_Operation
            // 
            Control_Add_Operation_TextBox_Operation.Dock = DockStyle.Top;
            Control_Add_Operation_TextBox_Operation.Location = new Point(129, 3);
            Control_Add_Operation_TextBox_Operation.Name = "Control_Add_Operation_TextBox_Operation";
            Control_Add_Operation_TextBox_Operation.Size = new Size(462, 23);
            Control_Add_Operation_TextBox_Operation.TabIndex = 2;
            // 
            // Control_Add_Operation_Label_IssuedBy
            // 
            Control_Add_Operation_Label_IssuedBy.AutoSize = true;
            Control_Add_Operation_Label_IssuedBy.Dock = DockStyle.Fill;
            Control_Add_Operation_Label_IssuedBy.Location = new Point(3, 29);
            Control_Add_Operation_Label_IssuedBy.Name = "Control_Add_Operation_Label_IssuedBy";
            Control_Add_Operation_Label_IssuedBy.Size = new Size(120, 15);
            Control_Add_Operation_Label_IssuedBy.TabIndex = 3;
            Control_Add_Operation_Label_IssuedBy.Text = "Issued By:";
            Control_Add_Operation_Label_IssuedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Add_Operation_Label_IssuedByValue
            // 
            Control_Add_Operation_Label_IssuedByValue.AutoSize = true;
            Control_Add_Operation_Label_IssuedByValue.Dock = DockStyle.Fill;
            Control_Add_Operation_Label_IssuedByValue.Location = new Point(129, 29);
            Control_Add_Operation_Label_IssuedByValue.Name = "Control_Add_Operation_Label_IssuedByValue";
            Control_Add_Operation_Label_IssuedByValue.Size = new Size(462, 15);
            Control_Add_Operation_Label_IssuedByValue.TabIndex = 4;
            Control_Add_Operation_Label_IssuedByValue.Text = "Current User";
            Control_Add_Operation_Label_IssuedByValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Control_Add_Operation_Button_Save
            // 
            Control_Add_Operation_Button_Save.AutoSize = true;
            Control_Add_Operation_Button_Save.Dock = DockStyle.Fill;
            Control_Add_Operation_Button_Save.Location = new Point(3, 53);
            Control_Add_Operation_Button_Save.Name = "Control_Add_Operation_Button_Save";
            Control_Add_Operation_Button_Save.Size = new Size(120, 25);
            Control_Add_Operation_Button_Save.TabIndex = 5;
            Control_Add_Operation_Button_Save.Text = "Save";
            Control_Add_Operation_Button_Save.UseVisualStyleBackColor = true;
            Control_Add_Operation_Button_Save.Click += Control_Add_Operation_Button_Save_Click;
            // 
            // Control_Add_Operation_Button_Clear
            // 
            Control_Add_Operation_Button_Clear.AutoSize = true;
            Control_Add_Operation_Button_Clear.Dock = DockStyle.Fill;
            Control_Add_Operation_Button_Clear.Location = new Point(129, 53);
            Control_Add_Operation_Button_Clear.Name = "Control_Add_Operation_Button_Clear";
            Control_Add_Operation_Button_Clear.Size = new Size(462, 25);
            Control_Add_Operation_Button_Clear.TabIndex = 6;
            Control_Add_Operation_Button_Clear.Text = "Clear";
            Control_Add_Operation_Button_Clear.UseVisualStyleBackColor = true;
            Control_Add_Operation_Button_Clear.Click += Control_Add_Operation_Button_Clear_Click;
            // 
            // Control_Shortcuts_GroupBox_Main
            // 
            groupBox1.AutoSize = true;
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "Control_Shortcuts_GroupBox_Main";
            groupBox1.Size = new Size(600, 103);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "AddOperation";
            // 
            // Control_Shortcuts_TableLayout_Main
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(Control_Add_Operation_Label_Operation, 0, 0);
            tableLayoutPanel1.Controls.Add(Control_Add_Operation_Button_Clear, 1, 3);
            tableLayoutPanel1.Controls.Add(Control_Add_Operation_Label_IssuedBy, 0, 1);
            tableLayoutPanel1.Controls.Add(Control_Add_Operation_Button_Save, 0, 3);
            tableLayoutPanel1.Controls.Add(Control_Add_Operation_Label_IssuedByValue, 1, 1);
            tableLayoutPanel1.Controls.Add(Control_Add_Operation_TextBox_Operation, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 19);
            tableLayoutPanel1.Name = "Control_Shortcuts_TableLayout_Main";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(594, 81);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // Control_Add_Operation
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(groupBox1);
            Name = "Control_Add_Operation";
            Size = new Size(600, 103);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel1;
    }
    #endregion
}
