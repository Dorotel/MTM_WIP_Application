namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Remove_Operation
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label selectOperationLabel;
        private System.Windows.Forms.ComboBox operationsComboBox;
        private System.Windows.Forms.Label operationLabel;
        private System.Windows.Forms.Label operationValueLabel;
        private System.Windows.Forms.Label issuedByLabel;
        private System.Windows.Forms.Label issuedByValueLabel;
        private System.Windows.Forms.Button removeButton;
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
            operationValueLabel = new Label();
            issuedByLabel = new Label();
            issuedByValueLabel = new Label();
            removeButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Emoji", 12F, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(153, 21);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Remove Operation";
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
            // operationValueLabel
            // 
            operationValueLabel.AutoSize = true;
            operationValueLabel.Location = new Point(140, 100);
            operationValueLabel.Name = "operationValueLabel";
            operationValueLabel.Size = new Size(0, 15);
            operationValueLabel.TabIndex = 4;
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
            issuedByValueLabel.Size = new Size(0, 15);
            issuedByValueLabel.TabIndex = 6;
            // 
            // removeButton
            // 
            removeButton.Enabled = false;
            removeButton.Location = new Point(265, 190);
            removeButton.Name = "removeButton";
            removeButton.Size = new Size(75, 23);
            removeButton.TabIndex = 7;
            removeButton.Text = "Remove";
            removeButton.UseVisualStyleBackColor = true;
            removeButton.Click += RemoveButton_Click;
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
            // Control_Remove_Operation
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(cancelButton);
            Controls.Add(removeButton);
            Controls.Add(issuedByValueLabel);
            Controls.Add(issuedByLabel);
            Controls.Add(operationValueLabel);
            Controls.Add(operationLabel);
            Controls.Add(operationsComboBox);
            Controls.Add(selectOperationLabel);
            Controls.Add(titleLabel);
            Name = "Control_Remove_Operation";
            Size = new Size(450, 240);
            ResumeLayout(false);
            PerformLayout();

        }
    }

        
        #endregion
    }
