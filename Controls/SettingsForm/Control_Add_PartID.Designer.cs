namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Add_PartID
    {
        #region Fields

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label itemNumberLabel;
        private System.Windows.Forms.TextBox itemNumberTextBox;
        private System.Windows.Forms.Label customerLabel;
        private System.Windows.Forms.TextBox customerTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox Control_Add_PartID_ComboBox_ItemType;
        private System.Windows.Forms.Label issuedByLabel;
        private System.Windows.Forms.Label issuedByValueLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;

        #endregion

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
            titleLabel = new Label();
            itemNumberLabel = new Label();
            itemNumberTextBox = new TextBox();
            customerLabel = new Label();
            customerTextBox = new TextBox();
            descriptionLabel = new Label();
            descriptionTextBox = new TextBox();
            typeLabel = new Label();
            Control_Add_PartID_ComboBox_ItemType = new ComboBox();
            issuedByLabel = new Label();
            issuedByValueLabel = new Label();
            saveButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(143, 21);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Add Part Number";
            // 
            // itemNumberLabel
            // 
            itemNumberLabel.AutoSize = true;
            itemNumberLabel.Location = new Point(20, 60);
            itemNumberLabel.Name = "itemNumberLabel";
            itemNumberLabel.Size = new Size(81, 15);
            itemNumberLabel.TabIndex = 1;
            itemNumberLabel.Text = "Item Number:";
            // 
            // itemNumberTextBox
            // 
            itemNumberTextBox.Location = new Point(120, 57);
            itemNumberTextBox.Name = "itemNumberTextBox";
            itemNumberTextBox.Size = new Size(300, 23);
            itemNumberTextBox.TabIndex = 2;
            // 
            // customerLabel
            // 
            customerLabel.AutoSize = true;
            customerLabel.Location = new Point(20, 100);
            customerLabel.Name = "customerLabel";
            customerLabel.Size = new Size(62, 15);
            customerLabel.TabIndex = 3;
            customerLabel.Text = "Customer:";
            // 
            // customerTextBox
            // 
            customerTextBox.Location = new Point(120, 97);
            customerTextBox.Name = "customerTextBox";
            customerTextBox.Size = new Size(300, 23);
            customerTextBox.TabIndex = 4;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new Point(20, 140);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(70, 15);
            descriptionLabel.TabIndex = 5;
            descriptionLabel.Text = "Description:";
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Location = new Point(120, 137);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.Size = new Size(300, 60);
            descriptionTextBox.TabIndex = 6;
            // 
            // typeLabel
            // 
            typeLabel.AutoSize = true;
            typeLabel.Location = new Point(20, 220);
            typeLabel.Name = "typeLabel";
            typeLabel.Size = new Size(35, 15);
            typeLabel.TabIndex = 7;
            typeLabel.Text = "Type:";
            // 
            // Control_Add_PartID_ComboBox_ItemType
            // 
            Control_Add_PartID_ComboBox_ItemType.DropDownStyle = ComboBoxStyle.DropDownList;
            Control_Add_PartID_ComboBox_ItemType.FormattingEnabled = true;
            Control_Add_PartID_ComboBox_ItemType.Location = new Point(120, 217);
            Control_Add_PartID_ComboBox_ItemType.Name = "Control_Add_PartID_ComboBox_ItemType";
            Control_Add_PartID_ComboBox_ItemType.Size = new Size(200, 23);
            Control_Add_PartID_ComboBox_ItemType.TabIndex = 8;
            // 
            // issuedByLabel
            // 
            issuedByLabel.AutoSize = true;
            issuedByLabel.Location = new Point(20, 260);
            issuedByLabel.Name = "issuedByLabel";
            issuedByLabel.Size = new Size(59, 15);
            issuedByLabel.TabIndex = 9;
            issuedByLabel.Text = "Issued By:";
            // 
            // issuedByValueLabel
            // 
            issuedByValueLabel.AutoSize = true;
            issuedByValueLabel.Location = new Point(120, 260);
            issuedByValueLabel.Name = "issuedByValueLabel";
            issuedByValueLabel.Size = new Size(73, 15);
            issuedByValueLabel.TabIndex = 10;
            issuedByValueLabel.Text = "Current User";
            // 
            // saveButton
            // 
            saveButton.Location = new Point(265, 310);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 11;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(345, 310);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 12;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // Control_Add_PartID
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(issuedByValueLabel);
            Controls.Add(issuedByLabel);
            Controls.Add(Control_Add_PartID_ComboBox_ItemType);
            Controls.Add(typeLabel);
            Controls.Add(descriptionTextBox);
            Controls.Add(descriptionLabel);
            Controls.Add(customerTextBox);
            Controls.Add(customerLabel);
            Controls.Add(itemNumberTextBox);
            Controls.Add(itemNumberLabel);
            Controls.Add(titleLabel);
            Name = "Control_Add_PartID";
            Size = new Size(442, 360);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
