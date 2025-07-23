namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Edit_PartID
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label selectPartLabel;
        private System.Windows.Forms.ComboBox Control_Edit_PartID_ComboBox_Part;
        private System.Windows.Forms.Label itemNumberLabel;
        private System.Windows.Forms.TextBox itemNumberTextBox;
        private System.Windows.Forms.Label customerLabel;
        private System.Windows.Forms.TextBox customerTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox Control_Edit_PartID_ComboBox_ItemType;
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
            Control_Edit_PartID_ComboBox_Part = new ComboBox();
            itemNumberLabel = new Label();
            itemNumberTextBox = new TextBox();
            customerLabel = new Label();
            customerTextBox = new TextBox();
            descriptionLabel = new Label();
            descriptionTextBox = new TextBox();
            typeLabel = new Label();
            Control_Edit_PartID_ComboBox_ItemType = new ComboBox();
            issuedByLabel = new Label();
            issuedByValueLabel = new Label();
            saveButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Emoji", 12F, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(142, 21);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Edit Part Number";
            // 
            // selectPartLabel
            // 
            selectPartLabel.AutoSize = true;
            selectPartLabel.Location = new Point(20, 60);
            selectPartLabel.Name = "selectPartLabel";
            selectPartLabel.Size = new Size(65, 15);
            selectPartLabel.TabIndex = 1;
            selectPartLabel.Text = "Select Part:";
            // 
            // Control_Edit_PartID_ComboBox_Part
            // 
            Control_Edit_PartID_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_Edit_PartID_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_Edit_PartID_ComboBox_Part.FormattingEnabled = true;
            Control_Edit_PartID_ComboBox_Part.Location = new Point(120, 57);
            Control_Edit_PartID_ComboBox_Part.Name = "Control_Edit_PartID_ComboBox_Part";
            Control_Edit_PartID_ComboBox_Part.Size = new Size(300, 23);
            Control_Edit_PartID_ComboBox_Part.TabIndex = 2;
            Control_Edit_PartID_ComboBox_Part.SelectedIndexChanged += Control_Edit_PartID_ComboBox_Part_SelectedIndexChanged;
            // 
            // itemNumberLabel
            // 
            itemNumberLabel.AutoSize = true;
            itemNumberLabel.Location = new Point(20, 100);
            itemNumberLabel.Name = "itemNumberLabel";
            itemNumberLabel.Size = new Size(81, 15);
            itemNumberLabel.TabIndex = 3;
            itemNumberLabel.Text = "Item Number:";
            // 
            // itemNumberTextBox
            // 
            itemNumberTextBox.Enabled = false;
            itemNumberTextBox.Location = new Point(120, 97);
            itemNumberTextBox.Name = "itemNumberTextBox";
            itemNumberTextBox.Size = new Size(300, 23);
            itemNumberTextBox.TabIndex = 4;
            // 
            // customerLabel
            // 
            customerLabel.AutoSize = true;
            customerLabel.Location = new Point(20, 140);
            customerLabel.Name = "customerLabel";
            customerLabel.Size = new Size(62, 15);
            customerLabel.TabIndex = 5;
            customerLabel.Text = "Customer:";
            // 
            // customerTextBox
            // 
            customerTextBox.Enabled = false;
            customerTextBox.Location = new Point(120, 137);
            customerTextBox.Name = "customerTextBox";
            customerTextBox.Size = new Size(300, 23);
            customerTextBox.TabIndex = 6;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new Point(20, 180);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(70, 15);
            descriptionLabel.TabIndex = 7;
            descriptionLabel.Text = "Description:";
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Enabled = false;
            descriptionTextBox.Location = new Point(120, 177);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.Size = new Size(300, 60);
            descriptionTextBox.TabIndex = 8;
            // 
            // typeLabel
            // 
            typeLabel.AutoSize = true;
            typeLabel.Location = new Point(20, 260);
            typeLabel.Name = "typeLabel";
            typeLabel.Size = new Size(35, 15);
            typeLabel.TabIndex = 9;
            typeLabel.Text = "Type:";
            // 
            // Control_Edit_PartID_ComboBox_ItemType
            // 
            Control_Edit_PartID_ComboBox_ItemType.DropDownStyle = ComboBoxStyle.DropDownList;
            Control_Edit_PartID_ComboBox_ItemType.Enabled = false;
            Control_Edit_PartID_ComboBox_ItemType.FormattingEnabled = true;
            Control_Edit_PartID_ComboBox_ItemType.Location = new Point(120, 257);
            Control_Edit_PartID_ComboBox_ItemType.Name = "Control_Edit_PartID_ComboBox_ItemType";
            Control_Edit_PartID_ComboBox_ItemType.Size = new Size(200, 23);
            Control_Edit_PartID_ComboBox_ItemType.TabIndex = 10;
            // 
            // issuedByLabel
            // 
            issuedByLabel.AutoSize = true;
            issuedByLabel.Location = new Point(20, 300);
            issuedByLabel.Name = "issuedByLabel";
            issuedByLabel.Size = new Size(59, 15);
            issuedByLabel.TabIndex = 11;
            issuedByLabel.Text = "Issued By:";
            // 
            // issuedByValueLabel
            // 
            issuedByValueLabel.AutoSize = true;
            issuedByValueLabel.Location = new Point(120, 300);
            issuedByValueLabel.Name = "issuedByValueLabel";
            issuedByValueLabel.Size = new Size(0, 15);
            issuedByValueLabel.TabIndex = 12;
            // 
            // saveButton
            // 
            saveButton.Enabled = false;
            saveButton.Location = new Point(265, 350);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 13;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(345, 350);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 14;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // Control_Edit_PartID
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(issuedByValueLabel);
            Controls.Add(issuedByLabel);
            Controls.Add(Control_Edit_PartID_ComboBox_ItemType);
            Controls.Add(typeLabel);
            Controls.Add(descriptionTextBox);
            Controls.Add(descriptionLabel);
            Controls.Add(customerTextBox);
            Controls.Add(customerLabel);
            Controls.Add(itemNumberTextBox);
            Controls.Add(itemNumberLabel);
            Controls.Add(Control_Edit_PartID_ComboBox_Part);
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
