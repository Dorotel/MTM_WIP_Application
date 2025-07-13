namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class RemoveLocationControl
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label selectLocationLabel;
        private System.Windows.Forms.ComboBox locationsComboBox;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.Label locationValueLabel;
        private System.Windows.Forms.Label buildingLabel;
        private System.Windows.Forms.Label buildingValueLabel;
        private System.Windows.Forms.Label issuedByLabel;
        private System.Windows.Forms.Label issuedByValueLabel;
        private System.Windows.Forms.Button removeButton;
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.selectLocationLabel = new System.Windows.Forms.Label();
            this.locationsComboBox = new System.Windows.Forms.ComboBox();
            this.locationLabel = new System.Windows.Forms.Label();
            this.locationValueLabel = new System.Windows.Forms.Label();
            this.buildingLabel = new System.Windows.Forms.Label();
            this.buildingValueLabel = new System.Windows.Forms.Label();
            this.issuedByLabel = new System.Windows.Forms.Label();
            this.issuedByValueLabel = new System.Windows.Forms.Label();
            this.removeButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(135, 21);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Remove Location";
            this.selectLocationLabel.AutoSize = true;
            this.selectLocationLabel.Location = new System.Drawing.Point(20, 60);
            this.selectLocationLabel.Name = "selectLocationLabel";
            this.selectLocationLabel.Size = new System.Drawing.Size(84, 15);
            this.selectLocationLabel.TabIndex = 1;
            this.selectLocationLabel.Text = "Select Location:";
            this.locationsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.locationsComboBox.FormattingEnabled = true;
            this.locationsComboBox.Location = new System.Drawing.Point(120, 57);
            this.locationsComboBox.Name = "locationsComboBox";
            this.locationsComboBox.Size = new System.Drawing.Size(300, 23);
            this.locationsComboBox.TabIndex = 2;
            this.locationsComboBox.SelectedIndexChanged += new System.EventHandler(this.LocationsComboBox_SelectedIndexChanged);
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(20, 100);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(57, 15);
            this.locationLabel.TabIndex = 3;
            this.locationLabel.Text = "Location:";
            this.locationValueLabel.AutoSize = true;
            this.locationValueLabel.Location = new System.Drawing.Point(120, 100);
            this.locationValueLabel.Name = "locationValueLabel";
            this.locationValueLabel.Size = new System.Drawing.Size(0, 15);
            this.locationValueLabel.TabIndex = 4;
            this.buildingLabel.AutoSize = true;
            this.buildingLabel.Location = new System.Drawing.Point(20, 140);
            this.buildingLabel.Name = "buildingLabel";
            this.buildingLabel.Size = new System.Drawing.Size(54, 15);
            this.buildingLabel.TabIndex = 5;
            this.buildingLabel.Text = "Building:";
            this.buildingValueLabel.AutoSize = true;
            this.buildingValueLabel.Location = new System.Drawing.Point(120, 140);
            this.buildingValueLabel.Name = "buildingValueLabel";
            this.buildingValueLabel.Size = new System.Drawing.Size(0, 15);
            this.buildingValueLabel.TabIndex = 6;
            this.issuedByLabel.AutoSize = true;
            this.issuedByLabel.Location = new System.Drawing.Point(20, 180);
            this.issuedByLabel.Name = "issuedByLabel";
            this.issuedByLabel.Size = new System.Drawing.Size(61, 15);
            this.issuedByLabel.TabIndex = 7;
            this.issuedByLabel.Text = "Issued By:";
            this.issuedByValueLabel.AutoSize = true;
            this.issuedByValueLabel.Location = new System.Drawing.Point(120, 180);
            this.issuedByValueLabel.Name = "issuedByValueLabel";
            this.issuedByValueLabel.Size = new System.Drawing.Size(0, 15);
            this.issuedByValueLabel.TabIndex = 8;
            this.removeButton.Enabled = false;
            this.removeButton.Location = new System.Drawing.Point(265, 230);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 9;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(345, 230);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.issuedByValueLabel);
            this.Controls.Add(this.issuedByLabel);
            this.Controls.Add(this.buildingValueLabel);
            this.Controls.Add(this.buildingLabel);
            this.Controls.Add(this.locationValueLabel);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.locationsComboBox);
            this.Controls.Add(this.selectLocationLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "RemoveLocationControl";
            this.Size = new System.Drawing.Size(450, 280);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }

        
        #endregion
    }