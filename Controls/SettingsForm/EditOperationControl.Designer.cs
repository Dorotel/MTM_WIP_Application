namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class EditOperationControl
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
            this.selectOperationLabel = new System.Windows.Forms.Label();
            this.operationsComboBox = new System.Windows.Forms.ComboBox();
            this.operationLabel = new System.Windows.Forms.Label();
            this.operationTextBox = new System.Windows.Forms.TextBox();
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
            this.titleLabel.Size = new System.Drawing.Size(122, 21);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Edit Operation";
            // 
            // selectOperationLabel
            // 
            this.selectOperationLabel.AutoSize = true;
            this.selectOperationLabel.Location = new System.Drawing.Point(20, 60);
            this.selectOperationLabel.Name = "selectOperationLabel";
            this.selectOperationLabel.Size = new System.Drawing.Size(96, 15);
            this.selectOperationLabel.TabIndex = 1;
            this.selectOperationLabel.Text = "Select Operation:";
            // 
            // operationsComboBox
            // 
            this.operationsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.operationsComboBox.FormattingEnabled = true;
            this.operationsComboBox.Location = new System.Drawing.Point(140, 57);
            this.operationsComboBox.Name = "operationsComboBox";
            this.operationsComboBox.Size = new System.Drawing.Size(280, 23);
            this.operationsComboBox.TabIndex = 2;
            this.operationsComboBox.SelectedIndexChanged += new System.EventHandler(this.OperationsComboBox_SelectedIndexChanged);
            // 
            // operationLabel
            // 
            this.operationLabel.AutoSize = true;
            this.operationLabel.Location = new System.Drawing.Point(20, 100);
            this.operationLabel.Name = "operationLabel";
            this.operationLabel.Size = new System.Drawing.Size(108, 15);
            this.operationLabel.TabIndex = 3;
            this.operationLabel.Text = "Operation Number:";
            // 
            // operationTextBox
            // 
            this.operationTextBox.Enabled = false;
            this.operationTextBox.Location = new System.Drawing.Point(140, 97);
            this.operationTextBox.Name = "operationTextBox";
            this.operationTextBox.Size = new System.Drawing.Size(280, 23);
            this.operationTextBox.TabIndex = 4;
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
            this.issuedByValueLabel.Location = new System.Drawing.Point(140, 140);
            this.issuedByValueLabel.Name = "issuedByValueLabel";
            this.issuedByValueLabel.Size = new System.Drawing.Size(83, 15);
            this.issuedByValueLabel.TabIndex = 6;
            this.issuedByValueLabel.Text = "Current User";
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(265, 190);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(345, 190);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EditOperationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.issuedByValueLabel);
            this.Controls.Add(this.issuedByLabel);
            this.Controls.Add(this.operationTextBox);
            this.Controls.Add(this.operationLabel);
            this.Controls.Add(this.operationsComboBox);
            this.Controls.Add(this.selectOperationLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "EditOperationControl";
            this.Size = new System.Drawing.Size(450, 240);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label selectOperationLabel;
        private System.Windows.Forms.ComboBox operationsComboBox;
        private System.Windows.Forms.Label operationLabel;
        private System.Windows.Forms.TextBox operationTextBox;
        private System.Windows.Forms.Label issuedByLabel;
        private System.Windows.Forms.Label issuedByValueLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
    }
}