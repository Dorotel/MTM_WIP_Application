namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class AddLocationControl
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
            this.locationLabel = new System.Windows.Forms.Label();
            this.locationTextBox = new System.Windows.Forms.TextBox();
            this.buildingLabel = new System.Windows.Forms.Label();
            this.buildingComboBox = new System.Windows.Forms.ComboBox();
            this.issuedByLabel = new System.Windows.Forms.Label();
            this.issuedByValueLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(106, 21);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Add Location";
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(20, 60);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(57, 15);
            this.locationLabel.TabIndex = 1;
            this.locationLabel.Text = "Location:";
            // 
            // locationTextBox
            // 
            this.locationTextBox.Location = new System.Drawing.Point(120, 57);
            this.locationTextBox.Name = "locationTextBox";
            this.locationTextBox.Size = new System.Drawing.Size(300, 23);
            this.locationTextBox.TabIndex = 2;
            // 
            // buildingLabel
            // 
            this.buildingLabel.AutoSize = true;
            this.buildingLabel.Location = new System.Drawing.Point(20, 100);
            this.buildingLabel.Name = "buildingLabel";
            this.buildingLabel.Size = new System.Drawing.Size(54, 15);
            this.buildingLabel.TabIndex = 3;
            this.buildingLabel.Text = "Building:";
            // 
            // buildingComboBox
            // 
            this.buildingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.buildingComboBox.FormattingEnabled = true;
            this.buildingComboBox.Location = new System.Drawing.Point(120, 97);
            this.buildingComboBox.Name = "buildingComboBox";
            this.buildingComboBox.Size = new System.Drawing.Size(200, 23);
            this.buildingComboBox.TabIndex = 4;
            // 
            // issuedByLabel
            // 
            this.issuedByLabel.AutoSize = true;
            this.issuedByLabel.Location = new System.Drawing.Point(20, 140);
            this.issuedByLabel.Name = "issuedByLabel";
            this.issuedByLabel.Size = new System.Drawing.Size(61, 15);
            this.issuedByLabel.TabIndex = 5;
            this.issuedByLabel.Text = "Issued By:";
            // 
            // issuedByValueLabel
            // 
            this.issuedByValueLabel.AutoSize = true;
            this.issuedByValueLabel.Location = new System.Drawing.Point(120, 140);
            this.issuedByValueLabel.Name = "issuedByValueLabel";
            this.issuedByValueLabel.Size = new System.Drawing.Size(83, 15);
            this.issuedByValueLabel.TabIndex = 6;
            this.issuedByValueLabel.Text = "Current User";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(265, 190);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(345, 190);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 8;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // AddLocationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.issuedByValueLabel);
            this.Controls.Add(this.issuedByLabel);
            this.Controls.Add(this.buildingComboBox);
            this.Controls.Add(this.buildingLabel);
            this.Controls.Add(this.locationTextBox);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "AddLocationControl";
            this.Size = new System.Drawing.Size(450, 240);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.TextBox locationTextBox;
        private System.Windows.Forms.Label buildingLabel;
        private System.Windows.Forms.ComboBox buildingComboBox;
        private System.Windows.Forms.Label issuedByLabel;
        private System.Windows.Forms.Label issuedByValueLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button clearButton;
    }
}