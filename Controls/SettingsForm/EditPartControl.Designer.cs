namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class EditPartControl
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.selectPartLabel = new System.Windows.Forms.Label();
            this.partsComboBox = new System.Windows.Forms.ComboBox();
            this.itemNumberLabel = new System.Windows.Forms.Label();
            this.itemNumberTextBox = new System.Windows.Forms.TextBox();
            this.customerLabel = new System.Windows.Forms.Label();
            this.customerTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.typeLabel = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.issuedByLabel = new System.Windows.Forms.Label();
            this.issuedByValueLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(143, 21);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Edit Part Number";
            // 
            // selectPartLabel
            // 
            this.selectPartLabel.AutoSize = true;
            this.selectPartLabel.Location = new System.Drawing.Point(20, 60);
            this.selectPartLabel.Name = "selectPartLabel";
            this.selectPartLabel.Size = new System.Drawing.Size(68, 15);
            this.selectPartLabel.TabIndex = 1;
            this.selectPartLabel.Text = "Select Part:";
            // 
            // partsComboBox
            // 
            this.partsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.partsComboBox.FormattingEnabled = true;
            this.partsComboBox.Location = new System.Drawing.Point(120, 57);
            this.partsComboBox.Name = "partsComboBox";
            this.partsComboBox.Size = new System.Drawing.Size(300, 23);
            this.partsComboBox.TabIndex = 2;
            this.partsComboBox.SelectedIndexChanged += new System.EventHandler(this.partsComboBox_SelectedIndexChanged);
            // 
            // itemNumberLabel
            // 
            this.itemNumberLabel.AutoSize = true;
            this.itemNumberLabel.Location = new System.Drawing.Point(20, 100);
            this.itemNumberLabel.Name = "itemNumberLabel";
            this.itemNumberLabel.Size = new System.Drawing.Size(85, 15);
            this.itemNumberLabel.TabIndex = 3;
            this.itemNumberLabel.Text = "Item Number:";
            // 
            // itemNumberTextBox
            // 
            this.itemNumberTextBox.Enabled = false;
            this.itemNumberTextBox.Location = new System.Drawing.Point(120, 97);
            this.itemNumberTextBox.Name = "itemNumberTextBox";
            this.itemNumberTextBox.Size = new System.Drawing.Size(300, 23);
            this.itemNumberTextBox.TabIndex = 4;
            // 
            // customerLabel
            // 
            this.customerLabel.AutoSize = true;
            this.customerLabel.Location = new System.Drawing.Point(20, 140);
            this.customerLabel.Name = "customerLabel";
            this.customerLabel.Size = new System.Drawing.Size(62, 15);
            this.customerLabel.TabIndex = 5;
            this.customerLabel.Text = "Customer:";
            // 
            // customerTextBox
            // 
            this.customerTextBox.Enabled = false;
            this.customerTextBox.Location = new System.Drawing.Point(120, 137);
            this.customerTextBox.Name = "customerTextBox";
            this.customerTextBox.Size = new System.Drawing.Size(300, 23);
            this.customerTextBox.TabIndex = 6;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(20, 180);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(70, 15);
            this.descriptionLabel.TabIndex = 7;
            this.descriptionLabel.Text = "Description:";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Enabled = false;
            this.descriptionTextBox.Location = new System.Drawing.Point(120, 177);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(300, 60);
            this.descriptionTextBox.TabIndex = 8;
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(20, 260);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(34, 15);
            this.typeLabel.TabIndex = 9;
            this.typeLabel.Text = "Type:";
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.Enabled = false;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Location = new System.Drawing.Point(120, 257);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(200, 23);
            this.typeComboBox.TabIndex = 10;
            // 
            // issuedByLabel
            // 
            this.issuedByLabel.AutoSize = true;
            this.issuedByLabel.Location = new System.Drawing.Point(20, 300);
            this.issuedByLabel.Name = "issuedByLabel";
            this.issuedByLabel.Size = new System.Drawing.Size(61, 15);
            this.issuedByLabel.TabIndex = 11;
            this.issuedByLabel.Text = "Issued By:";
            // 
            // issuedByValueLabel
            // 
            this.issuedByValueLabel.AutoSize = true;
            this.issuedByValueLabel.Location = new System.Drawing.Point(120, 300);
            this.issuedByValueLabel.Name = "issuedByValueLabel";
            this.issuedByValueLabel.Size = new System.Drawing.Size(0, 15);
            this.issuedByValueLabel.TabIndex = 12;
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(265, 350);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 13;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(345, 350);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 14;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // EditPartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.issuedByValueLabel);
            this.Controls.Add(this.issuedByLabel);
            this.Controls.Add(this.typeComboBox);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.customerTextBox);
            this.Controls.Add(this.customerLabel);
            this.Controls.Add(this.itemNumberTextBox);
            this.Controls.Add(this.itemNumberLabel);
            this.Controls.Add(this.partsComboBox);
            this.Controls.Add(this.selectPartLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "EditPartControl";
            this.Size = new System.Drawing.Size(450, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label selectPartLabel;
        private System.Windows.Forms.ComboBox partsComboBox;
        private System.Windows.Forms.Label itemNumberLabel;
        private System.Windows.Forms.TextBox itemNumberTextBox;
        private System.Windows.Forms.Label customerLabel;
        private System.Windows.Forms.TextBox customerTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.Label issuedByLabel;
        private System.Windows.Forms.Label issuedByValueLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
    }
}