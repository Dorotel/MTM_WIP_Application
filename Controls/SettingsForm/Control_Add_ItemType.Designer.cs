namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Add_ItemType
    {
        #region Fields

        private System.ComponentModel.IContainer components = null;

        #endregion

        private System.Windows.Forms.Label Control_Add_ItemType_Label_Title;
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
            this.Control_Add_ItemType_Label_Title = new System.Windows.Forms.Label();
            this.Control_Add_ItemType_Label_ItemType = new System.Windows.Forms.Label();
            this.Control_Add_ItemType_TextBox_ItemType = new System.Windows.Forms.TextBox();
            this.Control_Add_ItemType_Label_IssuedBy = new System.Windows.Forms.Label();
            this.Control_Add_ItemType_Label_IssuedByValue = new System.Windows.Forms.Label();
            this.Control_Add_ItemType_Button_Save = new System.Windows.Forms.Button();
            this.Control_Add_ItemType_Button_Clear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.Control_Add_ItemType_Label_Title.AutoSize = true;
            this.Control_Add_ItemType_Label_Title.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Control_Add_ItemType_Label_Title.Location = new System.Drawing.Point(20, 20);
            this.Control_Add_ItemType_Label_Title.Name = "Control_Add_ItemType_Label_Title";
            this.Control_Add_ItemType_Label_Title.Size = new System.Drawing.Size(121, 21);
            this.Control_Add_ItemType_Label_Title.TabIndex = 0;
            this.Control_Add_ItemType_Label_Title.Text = "Add ItemType";
            this.Control_Add_ItemType_Label_ItemType.AutoSize = true;
            this.Control_Add_ItemType_Label_ItemType.Location = new System.Drawing.Point(20, 60);
            this.Control_Add_ItemType_Label_ItemType.Name = "Control_Add_ItemType_Label_ItemType";
            this.Control_Add_ItemType_Label_ItemType.Size = new System.Drawing.Size(65, 15);
            this.Control_Add_ItemType_Label_ItemType.TabIndex = 1;
            this.Control_Add_ItemType_Label_ItemType.Text = "ItemType:";
            this.Control_Add_ItemType_TextBox_ItemType.Location = new System.Drawing.Point(120, 57);
            this.Control_Add_ItemType_TextBox_ItemType.Name = "Control_Add_ItemType_TextBox_ItemType";
            this.Control_Add_ItemType_TextBox_ItemType.Size = new System.Drawing.Size(300, 23);
            this.Control_Add_ItemType_TextBox_ItemType.TabIndex = 2;
            this.Control_Add_ItemType_Label_IssuedBy.AutoSize = true;
            this.Control_Add_ItemType_Label_IssuedBy.Location = new System.Drawing.Point(20, 100);
            this.Control_Add_ItemType_Label_IssuedBy.Name = "Control_Add_ItemType_Label_IssuedBy";
            this.Control_Add_ItemType_Label_IssuedBy.Size = new System.Drawing.Size(61, 15);
            this.Control_Add_ItemType_Label_IssuedBy.TabIndex = 3;
            this.Control_Add_ItemType_Label_IssuedBy.Text = "Issued By:";
            this.Control_Add_ItemType_Label_IssuedByValue.AutoSize = true;
            this.Control_Add_ItemType_Label_IssuedByValue.Location = new System.Drawing.Point(120, 100);
            this.Control_Add_ItemType_Label_IssuedByValue.Name = "Control_Add_ItemType_Label_IssuedByValue";
            this.Control_Add_ItemType_Label_IssuedByValue.Size = new System.Drawing.Size(83, 15);
            this.Control_Add_ItemType_Label_IssuedByValue.TabIndex = 4;
            this.Control_Add_ItemType_Label_IssuedByValue.Text = "Current User";
            this.Control_Add_ItemType_Button_Save.Location = new System.Drawing.Point(265, 150);
            this.Control_Add_ItemType_Button_Save.Name = "Control_Add_ItemType_Button_Save";
            this.Control_Add_ItemType_Button_Save.Size = new System.Drawing.Size(75, 23);
            this.Control_Add_ItemType_Button_Save.TabIndex = 5;
            this.Control_Add_ItemType_Button_Save.Text = "Save";
            this.Control_Add_ItemType_Button_Save.UseVisualStyleBackColor = true;
            this.Control_Add_ItemType_Button_Save.Click += new System.EventHandler(this.Control_Add_ItemType_Button_Save_Click);
            this.Control_Add_ItemType_Button_Clear.Location = new System.Drawing.Point(345, 150);
            this.Control_Add_ItemType_Button_Clear.Name = "Control_Add_ItemType_Button_Clear";
            this.Control_Add_ItemType_Button_Clear.Size = new System.Drawing.Size(75, 23);
            this.Control_Add_ItemType_Button_Clear.TabIndex = 6;
            this.Control_Add_ItemType_Button_Clear.Text = "Clear";
            this.Control_Add_ItemType_Button_Clear.UseVisualStyleBackColor = true;
            this.Control_Add_ItemType_Button_Clear.Click += new System.EventHandler(this.Control_Add_ItemType_Button_Clear_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Control_Add_ItemType_Button_Clear);
            this.Controls.Add(this.Control_Add_ItemType_Button_Save);
            this.Controls.Add(this.Control_Add_ItemType_Label_IssuedByValue);
            this.Controls.Add(this.Control_Add_ItemType_Label_IssuedBy);
            this.Controls.Add(this.Control_Add_ItemType_TextBox_ItemType);
            this.Controls.Add(this.Control_Add_ItemType_Label_ItemType);
            this.Controls.Add(this.Control_Add_ItemType_Label_Title);
            this.Name = "Control_Add_ItemType";
            this.Size = new System.Drawing.Size(450, 200);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
    #endregion
}
