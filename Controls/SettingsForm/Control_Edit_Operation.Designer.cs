namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Edit_Operation
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

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
            selectOperationLabel = new Label();
            operationsComboBox = new ComboBox();
            operationLabel = new Label();
            operationTextBox = new TextBox();
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
            titleLabel.Size = new Size(121, 21);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Edit Operation";
            // 
            // selectOperationLabel
            // 
            selectOperationLabel.AutoSize = true;
            selectOperationLabel.Location = new Point(20, 60);
            selectOperationLabel.Name = "selectOperationLabel";
            selectOperationLabel.Size = new Size(97, 15);
            selectOperationLabel.TabIndex = 1;
            selectOperationLabel.Text = "Select Operation:";
            // 
            // operationsComboBox
            // 
            operationsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            operationsComboBox.FormattingEnabled = true;
            operationsComboBox.Location = new Point(140, 57);
            operationsComboBox.Name = "operationsComboBox";
            operationsComboBox.Size = new Size(280, 23);
            operationsComboBox.TabIndex = 2;
            operationsComboBox.SelectedIndexChanged += OperationsComboBox_SelectedIndexChanged;
            // 
            // operationLabel
            // 
            operationLabel.AutoSize = true;
            operationLabel.Location = new Point(20, 100);
            operationLabel.Name = "operationLabel";
            operationLabel.Size = new Size(110, 15);
            operationLabel.TabIndex = 3;
            operationLabel.Text = "Operation Number:";
            // 
            // operationTextBox
            // 
            operationTextBox.Enabled = false;
            operationTextBox.Location = new Point(140, 97);
            operationTextBox.Name = "operationTextBox";
            operationTextBox.Size = new Size(280, 23);
            operationTextBox.TabIndex = 4;
            // 
            // issuedByLabel
            // 
            issuedByLabel.AutoSize = true;
            issuedByLabel.Location = new Point(20, 140);
            issuedByLabel.Name = "issuedByLabel";
            issuedByLabel.Size = new Size(59, 15);
            issuedByLabel.TabIndex = 5;
            issuedByLabel.Text = "Issued By:";
            // 
            // issuedByValueLabel
            // 
            issuedByValueLabel.AutoSize = true;
            issuedByValueLabel.Location = new Point(140, 140);
            issuedByValueLabel.Name = "issuedByValueLabel";
            issuedByValueLabel.Size = new Size(73, 15);
            issuedByValueLabel.TabIndex = 6;
            issuedByValueLabel.Text = "Current User";
            // 
            // saveButton
            // 
            saveButton.Enabled = false;
            saveButton.Location = new Point(265, 190);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 7;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Enabled = false;
            cancelButton.Location = new Point(345, 190);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 8;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // Control_Edit_Operation
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(issuedByValueLabel);
            Controls.Add(issuedByLabel);
            Controls.Add(operationTextBox);
            Controls.Add(operationLabel);
            Controls.Add(operationsComboBox);
            Controls.Add(selectOperationLabel);
            Controls.Add(titleLabel);
            Name = "Control_Edit_Operation";
            Size = new Size(450, 240);
            ResumeLayout(false);
            PerformLayout();

        }
    }

        
        #endregion
    }
