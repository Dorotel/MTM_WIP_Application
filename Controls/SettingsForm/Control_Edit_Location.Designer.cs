namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Edit_Location
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label selectLocationLabel;
        private System.Windows.Forms.ComboBox locationsComboBox;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.TextBox locationTextBox;
        private System.Windows.Forms.Label buildingLabel;
        private System.Windows.Forms.ComboBox buildingComboBox;
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
            selectLocationLabel = new Label();
            locationsComboBox = new ComboBox();
            locationLabel = new Label();
            locationTextBox = new TextBox();
            buildingLabel = new Label();
            buildingComboBox = new ComboBox();
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
            titleLabel.Size = new Size(110, 21);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Edit Location";
            // 
            // selectLocationLabel
            // 
            selectLocationLabel.AutoSize = true;
            selectLocationLabel.Location = new Point(20, 60);
            selectLocationLabel.Name = "selectLocationLabel";
            selectLocationLabel.Size = new Size(90, 15);
            selectLocationLabel.TabIndex = 1;
            selectLocationLabel.Text = "Select Location:";
            // 
            // locationsComboBox
            // 
            locationsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            locationsComboBox.FormattingEnabled = true;
            locationsComboBox.Location = new Point(120, 57);
            locationsComboBox.Name = "locationsComboBox";
            locationsComboBox.Size = new Size(300, 23);
            locationsComboBox.TabIndex = 2;
            locationsComboBox.SelectedIndexChanged += LocationsComboBox_SelectedIndexChanged;
            // 
            // locationLabel
            // 
            locationLabel.AutoSize = true;
            locationLabel.Location = new Point(20, 100);
            locationLabel.Name = "locationLabel";
            locationLabel.Size = new Size(56, 15);
            locationLabel.TabIndex = 3;
            locationLabel.Text = "Location:";
            // 
            // locationTextBox
            // 
            locationTextBox.Enabled = false;
            locationTextBox.Location = new Point(120, 97);
            locationTextBox.Name = "locationTextBox";
            locationTextBox.Size = new Size(300, 23);
            locationTextBox.TabIndex = 4;
            // 
            // buildingLabel
            // 
            buildingLabel.AutoSize = true;
            buildingLabel.Location = new Point(20, 140);
            buildingLabel.Name = "buildingLabel";
            buildingLabel.Size = new Size(54, 15);
            buildingLabel.TabIndex = 5;
            buildingLabel.Text = "Building:";
            // 
            // buildingComboBox
            // 
            buildingComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            buildingComboBox.Enabled = false;
            buildingComboBox.FormattingEnabled = true;
            buildingComboBox.Location = new Point(120, 137);
            buildingComboBox.Name = "buildingComboBox";
            buildingComboBox.Size = new Size(200, 23);
            buildingComboBox.TabIndex = 6;
            // 
            // issuedByLabel
            // 
            issuedByLabel.AutoSize = true;
            issuedByLabel.Location = new Point(20, 180);
            issuedByLabel.Name = "issuedByLabel";
            issuedByLabel.Size = new Size(59, 15);
            issuedByLabel.TabIndex = 7;
            issuedByLabel.Text = "Issued By:";
            // 
            // issuedByValueLabel
            // 
            issuedByValueLabel.AutoSize = true;
            issuedByValueLabel.Location = new Point(120, 180);
            issuedByValueLabel.Name = "issuedByValueLabel";
            issuedByValueLabel.Size = new Size(73, 15);
            issuedByValueLabel.TabIndex = 8;
            issuedByValueLabel.Text = "Current User";
            // 
            // saveButton
            // 
            saveButton.Enabled = false;
            saveButton.Location = new Point(265, 230);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 9;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Enabled = false;
            cancelButton.Location = new Point(345, 230);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 10;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // Control_Edit_Location
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(issuedByValueLabel);
            Controls.Add(issuedByLabel);
            Controls.Add(buildingComboBox);
            Controls.Add(buildingLabel);
            Controls.Add(locationTextBox);
            Controls.Add(locationLabel);
            Controls.Add(locationsComboBox);
            Controls.Add(selectLocationLabel);
            Controls.Add(titleLabel);
            Name = "Control_Edit_Location";
            Size = new Size(450, 280);
            ResumeLayout(false);
            PerformLayout();

        }
    }

        
        #endregion
    }
