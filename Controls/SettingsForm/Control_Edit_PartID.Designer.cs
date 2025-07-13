namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Edit_PartID
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

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
            selectPartLabel = new Label();
            partsComboBox = new ComboBox();
            itemNumberLabel = new Label();
            itemNumberTextBox = new TextBox();
            customerLabel = new Label();
            customerTextBox = new TextBox();
            descriptionLabel = new Label();
            descriptionTextBox = new TextBox();
            typeLabel = new Label();
            typeComboBox = new ComboBox();
            issuedByLabel = new Label();
            issuedByValueLabel = new Label();
            saveButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(142, 21);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Edit Part Number";
            selectPartLabel.AutoSize = true;
            selectPartLabel.Location = new Point(20, 60);
            selectPartLabel.Name = "selectPartLabel";
            selectPartLabel.Size = new Size(65, 15);
            selectPartLabel.TabIndex = 1;
            selectPartLabel.Text = "Select Part:";
            partsComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            partsComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            partsComboBox.FormattingEnabled = true;
            partsComboBox.Location = new Point(120, 57);
            partsComboBox.Name = "partsComboBox";
            partsComboBox.Size = new Size(300, 23);
            partsComboBox.TabIndex = 2;
            partsComboBox.SelectedIndexChanged += PartsComboBox_SelectedIndexChanged;
            itemNumberLabel.AutoSize = true;
            itemNumberLabel.Location = new Point(20, 100);
            itemNumberLabel.Name = "itemNumberLabel";
            itemNumberLabel.Size = new Size(81, 15);
            itemNumberLabel.TabIndex = 3;
            itemNumberLabel.Text = "Item Number:";
            itemNumberTextBox.Enabled = false;
            itemNumberTextBox.Location = new Point(120, 97);
            itemNumberTextBox.Name = "itemNumberTextBox";
            itemNumberTextBox.Size = new Size(300, 23);
            itemNumberTextBox.TabIndex = 4;
            customerLabel.AutoSize = true;
            customerLabel.Location = new Point(20, 140);
            customerLabel.Name = "customerLabel";
            customerLabel.Size = new Size(62, 15);
            customerLabel.TabIndex = 5;
            customerLabel.Text = "Customer:";
            customerTextBox.Enabled = false;
            customerTextBox.Location = new Point(120, 137);
            customerTextBox.Name = "customerTextBox";
            customerTextBox.Size = new Size(300, 23);
            customerTextBox.TabIndex = 6;
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new Point(20, 180);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(70, 15);
            descriptionLabel.TabIndex = 7;
            descriptionLabel.Text = "Description:";
            descriptionTextBox.Enabled = false;
            descriptionTextBox.Location = new Point(120, 177);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.Size = new Size(300, 60);
            descriptionTextBox.TabIndex = 8;
            typeLabel.AutoSize = true;
            typeLabel.Location = new Point(20, 260);
            typeLabel.Name = "typeLabel";
            typeLabel.Size = new Size(35, 15);
            typeLabel.TabIndex = 9;
            typeLabel.Text = "Type:";
            typeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            typeComboBox.Enabled = false;
            typeComboBox.FormattingEnabled = true;
            typeComboBox.Location = new Point(120, 257);
            typeComboBox.Name = "typeComboBox";
            typeComboBox.Size = new Size(200, 23);
            typeComboBox.TabIndex = 10;
            issuedByLabel.AutoSize = true;
            issuedByLabel.Location = new Point(20, 300);
            issuedByLabel.Name = "issuedByLabel";
            issuedByLabel.Size = new Size(59, 15);
            issuedByLabel.TabIndex = 11;
            issuedByLabel.Text = "Issued By:";
            issuedByValueLabel.AutoSize = true;
            issuedByValueLabel.Location = new Point(120, 300);
            issuedByValueLabel.Name = "issuedByValueLabel";
            issuedByValueLabel.Size = new Size(0, 15);
            issuedByValueLabel.TabIndex = 12;
            saveButton.Enabled = false;
            saveButton.Location = new Point(265, 350);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 13;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            cancelButton.Location = new Point(345, 350);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 14;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(issuedByValueLabel);
            Controls.Add(issuedByLabel);
            Controls.Add(typeComboBox);
            Controls.Add(typeLabel);
            Controls.Add(descriptionTextBox);
            Controls.Add(descriptionLabel);
            Controls.Add(customerTextBox);
            Controls.Add(customerLabel);
            Controls.Add(itemNumberTextBox);
            Controls.Add(itemNumberLabel);
            Controls.Add(partsComboBox);
            Controls.Add(selectPartLabel);
            Controls.Add(titleLabel);
            Name = "Control_Edit_PartID";
            Size = new Size(450, 400);
            ResumeLayout(false);
            PerformLayout();

        }
    }

        
        #endregion
    }
