namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Add_Location
    {
        #region Fields

        private System.ComponentModel.IContainer components = null;

        #endregion
        private System.Windows.Forms.Label Control_Add_Location_Label_Location;
        private System.Windows.Forms.TextBox Control_Add_Location_TextBox_Location;
        private System.Windows.Forms.Label Control_Add_Location_Label_Building;
        private System.Windows.Forms.ComboBox Control_Add_Location_ComboBox_Building;
        private System.Windows.Forms.Label Control_Add_Location_Label_IssuedBy;
        private System.Windows.Forms.Label Control_Add_Location_Label_IssuedByValue;
        private System.Windows.Forms.Button Control_Add_Location_Button_Save;
        private System.Windows.Forms.Button Control_Add_Location_Button_Clear;

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
            Control_Add_Location_Label_Location = new Label();
            Control_Add_Location_TextBox_Location = new TextBox();
            Control_Add_Location_Label_Building = new Label();
            Control_Add_Location_ComboBox_Building = new ComboBox();
            Control_Add_Location_Label_IssuedBy = new Label();
            Control_Add_Location_Label_IssuedByValue = new Label();
            Control_Add_Location_Button_Save = new Button();
            Control_Add_Location_Button_Clear = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // Control_Add_Location_Label_Location
            // 
            Control_Add_Location_Label_Location.AutoSize = true;
            Control_Add_Location_Label_Location.Dock = DockStyle.Fill;
            Control_Add_Location_Label_Location.Location = new Point(3, 0);
            Control_Add_Location_Label_Location.Name = "Control_Add_Location_Label_Location";
            Control_Add_Location_Label_Location.Size = new Size(120, 29);
            Control_Add_Location_Label_Location.TabIndex = 1;
            Control_Add_Location_Label_Location.Text = "Location:";
            Control_Add_Location_Label_Location.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Add_Location_TextBox_Location
            // 
            Control_Add_Location_TextBox_Location.Dock = DockStyle.Fill;
            Control_Add_Location_TextBox_Location.Location = new Point(129, 3);
            Control_Add_Location_TextBox_Location.Name = "Control_Add_Location_TextBox_Location";
            Control_Add_Location_TextBox_Location.Size = new Size(462, 23);
            Control_Add_Location_TextBox_Location.TabIndex = 2;
            // 
            // Control_Add_Location_Label_Building
            // 
            Control_Add_Location_Label_Building.AutoSize = true;
            Control_Add_Location_Label_Building.Dock = DockStyle.Fill;
            Control_Add_Location_Label_Building.Location = new Point(3, 29);
            Control_Add_Location_Label_Building.Name = "Control_Add_Location_Label_Building";
            Control_Add_Location_Label_Building.Size = new Size(120, 29);
            Control_Add_Location_Label_Building.TabIndex = 3;
            Control_Add_Location_Label_Building.Text = "Building:";
            Control_Add_Location_Label_Building.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Add_Location_ComboBox_Building
            // 
            Control_Add_Location_ComboBox_Building.Dock = DockStyle.Fill;
            Control_Add_Location_ComboBox_Building.DropDownStyle = ComboBoxStyle.DropDownList;
            Control_Add_Location_ComboBox_Building.FormattingEnabled = true;
            Control_Add_Location_ComboBox_Building.Location = new Point(129, 32);
            Control_Add_Location_ComboBox_Building.Name = "Control_Add_Location_ComboBox_Building";
            Control_Add_Location_ComboBox_Building.Size = new Size(462, 23);
            Control_Add_Location_ComboBox_Building.TabIndex = 4;
            // 
            // Control_Add_Location_Label_IssuedBy
            // 
            Control_Add_Location_Label_IssuedBy.AutoSize = true;
            Control_Add_Location_Label_IssuedBy.Dock = DockStyle.Fill;
            Control_Add_Location_Label_IssuedBy.Location = new Point(3, 58);
            Control_Add_Location_Label_IssuedBy.Name = "Control_Add_Location_Label_IssuedBy";
            Control_Add_Location_Label_IssuedBy.Size = new Size(120, 15);
            Control_Add_Location_Label_IssuedBy.TabIndex = 5;
            Control_Add_Location_Label_IssuedBy.Text = "Issued By:";
            Control_Add_Location_Label_IssuedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Add_Location_Label_IssuedByValue
            // 
            Control_Add_Location_Label_IssuedByValue.AutoSize = true;
            Control_Add_Location_Label_IssuedByValue.Dock = DockStyle.Fill;
            Control_Add_Location_Label_IssuedByValue.Location = new Point(129, 58);
            Control_Add_Location_Label_IssuedByValue.Name = "Control_Add_Location_Label_IssuedByValue";
            Control_Add_Location_Label_IssuedByValue.Size = new Size(462, 15);
            Control_Add_Location_Label_IssuedByValue.TabIndex = 6;
            Control_Add_Location_Label_IssuedByValue.Text = "Current User";
            Control_Add_Location_Label_IssuedByValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Control_Add_Location_Button_Save
            // 
            Control_Add_Location_Button_Save.AutoSize = true;
            Control_Add_Location_Button_Save.Dock = DockStyle.Fill;
            Control_Add_Location_Button_Save.Location = new Point(3, 85);
            Control_Add_Location_Button_Save.Name = "Control_Add_Location_Button_Save";
            Control_Add_Location_Button_Save.Size = new Size(120, 25);
            Control_Add_Location_Button_Save.TabIndex = 7;
            Control_Add_Location_Button_Save.Text = "Save";
            Control_Add_Location_Button_Save.UseVisualStyleBackColor = true;
            Control_Add_Location_Button_Save.Click += Control_Add_Location_Button_Save_Click;
            // 
            // Control_Add_Location_Button_Clear
            // 
            Control_Add_Location_Button_Clear.AutoSize = true;
            Control_Add_Location_Button_Clear.Dock = DockStyle.Fill;
            Control_Add_Location_Button_Clear.Location = new Point(129, 85);
            Control_Add_Location_Button_Clear.Name = "Control_Add_Location_Button_Clear";
            Control_Add_Location_Button_Clear.Size = new Size(462, 25);
            Control_Add_Location_Button_Clear.TabIndex = 8;
            Control_Add_Location_Button_Clear.Text = "Clear";
            Control_Add_Location_Button_Clear.UseVisualStyleBackColor = true;
            Control_Add_Location_Button_Clear.Click += Control_Add_Location_Button_Clear_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(Control_Add_Location_Label_Location, 0, 0);
            tableLayoutPanel1.Controls.Add(Control_Add_Location_Button_Clear, 1, 4);
            tableLayoutPanel1.Controls.Add(Control_Add_Location_TextBox_Location, 1, 0);
            tableLayoutPanel1.Controls.Add(Control_Add_Location_Button_Save, 0, 4);
            tableLayoutPanel1.Controls.Add(Control_Add_Location_Label_Building, 0, 1);
            tableLayoutPanel1.Controls.Add(Control_Add_Location_Label_IssuedByValue, 1, 2);
            tableLayoutPanel1.Controls.Add(Control_Add_Location_ComboBox_Building, 1, 1);
            tableLayoutPanel1.Controls.Add(Control_Add_Location_Label_IssuedBy, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(594, 113);
            tableLayoutPanel1.TabIndex = 9;
            // 
            // groupBox1
            // 
            groupBox1.AutoSize = true;
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(600, 135);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Add Location";
            // 
            // Control_Add_Location
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(groupBox1);
            Name = "Control_Add_Location";
            Size = new Size(600, 135);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
    }
    #endregion
}
