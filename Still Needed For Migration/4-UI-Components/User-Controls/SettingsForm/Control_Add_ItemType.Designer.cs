namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Add_ItemType
    {
        #region Fields

        private System.ComponentModel.IContainer components = null;

        #endregion
        private System.Windows.Forms.Label Control_Add_ItemType_Label_ItemType;
        private System.Windows.Forms.TextBox Control_Add_ItemType_TextBox_ItemType;
        private System.Windows.Forms.Label Control_Add_ItemType_Label_IssuedBy;
        private System.Windows.Forms.Label Control_Add_ItemType_Label_IssuedByValue;
        private System.Windows.Forms.Button Control_Add_ItemType_Button_Save;
        private System.Windows.Forms.Button Control_Add_ItemType_Button_Clear;

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
            Control_Add_ItemType_Label_ItemType = new Label();
            Control_Add_ItemType_TextBox_ItemType = new TextBox();
            Control_Add_ItemType_Label_IssuedBy = new Label();
            Control_Add_ItemType_Label_IssuedByValue = new Label();
            Control_Add_ItemType_Button_Save = new Button();
            Control_Add_ItemType_Button_Clear = new Button();
            Control_Add_ItemType_GroupBox_Main = new GroupBox();
            Control_Add_ItemType_TableLayout = new TableLayoutPanel();
            Control_Add_ItemType_GroupBox_Main.SuspendLayout();
            Control_Add_ItemType_TableLayout.SuspendLayout();
            SuspendLayout();
            // 
            // Control_Add_ItemType_Label_ItemType
            // 
            Control_Add_ItemType_Label_ItemType.AutoSize = true;
            Control_Add_ItemType_Label_ItemType.Dock = DockStyle.Fill;
            Control_Add_ItemType_Label_ItemType.Location = new Point(3, 0);
            Control_Add_ItemType_Label_ItemType.Name = "Control_Add_ItemType_Label_ItemType";
            Control_Add_ItemType_Label_ItemType.Size = new Size(120, 29);
            Control_Add_ItemType_Label_ItemType.TabIndex = 1;
            Control_Add_ItemType_Label_ItemType.Text = "ItemType:";
            Control_Add_ItemType_Label_ItemType.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Add_ItemType_TextBox_ItemType
            // 
            Control_Add_ItemType_TextBox_ItemType.Dock = DockStyle.Fill;
            Control_Add_ItemType_TextBox_ItemType.Location = new Point(129, 3);
            Control_Add_ItemType_TextBox_ItemType.Name = "Control_Add_ItemType_TextBox_ItemType";
            Control_Add_ItemType_TextBox_ItemType.Size = new Size(462, 23);
            Control_Add_ItemType_TextBox_ItemType.TabIndex = 2;
            // 
            // Control_Add_ItemType_Label_IssuedBy
            // 
            Control_Add_ItemType_Label_IssuedBy.AutoSize = true;
            Control_Add_ItemType_Label_IssuedBy.Dock = DockStyle.Fill;
            Control_Add_ItemType_Label_IssuedBy.Location = new Point(3, 29);
            Control_Add_ItemType_Label_IssuedBy.Name = "Control_Add_ItemType_Label_IssuedBy";
            Control_Add_ItemType_Label_IssuedBy.Size = new Size(120, 15);
            Control_Add_ItemType_Label_IssuedBy.TabIndex = 3;
            Control_Add_ItemType_Label_IssuedBy.Text = "Issued By:";
            Control_Add_ItemType_Label_IssuedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Add_ItemType_Label_IssuedByValue
            // 
            Control_Add_ItemType_Label_IssuedByValue.AutoSize = true;
            Control_Add_ItemType_Label_IssuedByValue.Dock = DockStyle.Fill;
            Control_Add_ItemType_Label_IssuedByValue.Location = new Point(129, 29);
            Control_Add_ItemType_Label_IssuedByValue.Name = "Control_Add_ItemType_Label_IssuedByValue";
            Control_Add_ItemType_Label_IssuedByValue.Size = new Size(462, 15);
            Control_Add_ItemType_Label_IssuedByValue.TabIndex = 4;
            Control_Add_ItemType_Label_IssuedByValue.Text = "Current User";
            // 
            // Control_Add_ItemType_Button_Save
            // 
            Control_Add_ItemType_Button_Save.AutoSize = true;
            Control_Add_ItemType_Button_Save.Dock = DockStyle.Fill;
            Control_Add_ItemType_Button_Save.Location = new Point(3, 54);
            Control_Add_ItemType_Button_Save.Name = "Control_Add_ItemType_Button_Save";
            Control_Add_ItemType_Button_Save.Size = new Size(120, 25);
            Control_Add_ItemType_Button_Save.TabIndex = 5;
            Control_Add_ItemType_Button_Save.Text = "Save";
            Control_Add_ItemType_Button_Save.UseVisualStyleBackColor = true;
            Control_Add_ItemType_Button_Save.Click += Control_Add_ItemType_Button_Save_Click;
            // 
            // Control_Add_ItemType_Button_Clear
            // 
            Control_Add_ItemType_Button_Clear.AutoSize = true;
            Control_Add_ItemType_Button_Clear.Dock = DockStyle.Fill;
            Control_Add_ItemType_Button_Clear.Location = new Point(129, 54);
            Control_Add_ItemType_Button_Clear.Name = "Control_Add_ItemType_Button_Clear";
            Control_Add_ItemType_Button_Clear.Size = new Size(462, 25);
            Control_Add_ItemType_Button_Clear.TabIndex = 6;
            Control_Add_ItemType_Button_Clear.Text = "Clear";
            Control_Add_ItemType_Button_Clear.UseVisualStyleBackColor = true;
            Control_Add_ItemType_Button_Clear.Click += Control_Add_ItemType_Button_Clear_Click;
            // 
            // Control_Add_ItemType_GroupBox_Main
            // 
            Control_Add_ItemType_GroupBox_Main.AutoSize = true;
            Control_Add_ItemType_GroupBox_Main.Controls.Add(Control_Add_ItemType_TableLayout);
            Control_Add_ItemType_GroupBox_Main.Dock = DockStyle.Fill;
            Control_Add_ItemType_GroupBox_Main.Location = new Point(0, 0);
            Control_Add_ItemType_GroupBox_Main.Name = "Control_Add_ItemType_GroupBox_Main";
            Control_Add_ItemType_GroupBox_Main.Size = new Size(600, 104);
            Control_Add_ItemType_GroupBox_Main.TabIndex = 7;
            Control_Add_ItemType_GroupBox_Main.TabStop = false;
            Control_Add_ItemType_GroupBox_Main.Text = "Add Item Type";
            // 
            // Control_Add_ItemType_TableLayout
            // 
            Control_Add_ItemType_TableLayout.AutoSize = true;
            Control_Add_ItemType_TableLayout.ColumnCount = 2;
            Control_Add_ItemType_TableLayout.ColumnStyles.Add(new ColumnStyle());
            Control_Add_ItemType_TableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_Add_ItemType_TableLayout.Controls.Add(Control_Add_ItemType_Label_ItemType, 0, 0);
            Control_Add_ItemType_TableLayout.Controls.Add(Control_Add_ItemType_Button_Clear, 1, 3);
            Control_Add_ItemType_TableLayout.Controls.Add(Control_Add_ItemType_Label_IssuedBy, 0, 1);
            Control_Add_ItemType_TableLayout.Controls.Add(Control_Add_ItemType_Button_Save, 0, 3);
            Control_Add_ItemType_TableLayout.Controls.Add(Control_Add_ItemType_TextBox_ItemType, 1, 0);
            Control_Add_ItemType_TableLayout.Controls.Add(Control_Add_ItemType_Label_IssuedByValue, 1, 1);
            Control_Add_ItemType_TableLayout.Dock = DockStyle.Fill;
            Control_Add_ItemType_TableLayout.Location = new Point(3, 19);
            Control_Add_ItemType_TableLayout.Name = "Control_Add_ItemType_TableLayout";
            Control_Add_ItemType_TableLayout.RowCount = 4;
            Control_Add_ItemType_TableLayout.RowStyles.Add(new RowStyle());
            Control_Add_ItemType_TableLayout.RowStyles.Add(new RowStyle());
            Control_Add_ItemType_TableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_Add_ItemType_TableLayout.RowStyles.Add(new RowStyle());
            Control_Add_ItemType_TableLayout.Size = new Size(594, 82);
            Control_Add_ItemType_TableLayout.TabIndex = 0;
            // 
            // Control_Add_ItemType
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            Controls.Add(Control_Add_ItemType_GroupBox_Main);
            Name = "Control_Add_ItemType";
            Size = new Size(600, 104);
            Control_Add_ItemType_GroupBox_Main.ResumeLayout(false);
            Control_Add_ItemType_GroupBox_Main.PerformLayout();
            Control_Add_ItemType_TableLayout.ResumeLayout(false);
            Control_Add_ItemType_TableLayout.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private GroupBox Control_Add_ItemType_GroupBox_Main;
        private TableLayoutPanel Control_Add_ItemType_TableLayout;
    }
    #endregion
}
