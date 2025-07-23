namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Remove_PartID
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label selectPartLabel;
        private System.Windows.Forms.ComboBox partsComboBox;
        private System.Windows.Forms.GroupBox detailsGroupBox;
        private System.Windows.Forms.Label issuedByValueLabel;
        private System.Windows.Forms.Label issuedByLabel;
        private System.Windows.Forms.Label typeValueLabel;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label descriptionValueLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label customerValueLabel;
        private System.Windows.Forms.Label customerLabel;
        private System.Windows.Forms.Label itemNumberValueLabel;
        private System.Windows.Forms.Label itemNumberLabel;
        private System.Windows.Forms.Label warningLabel;
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
            selectPartLabel = new Label();
            partsComboBox = new ComboBox();
            detailsGroupBox = new GroupBox();
            issuedByValueLabel = new Label();
            issuedByLabel = new Label();
            typeValueLabel = new Label();
            typeLabel = new Label();
            descriptionValueLabel = new Label();
            descriptionLabel = new Label();
            customerValueLabel = new Label();
            customerLabel = new Label();
            itemNumberValueLabel = new Label();
            itemNumberLabel = new Label();
            warningLabel = new Label();
            removeButton = new Button();
            cancelButton = new Button();
            detailsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Emoji", 12F, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(174, 21);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Remove Part Number";
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
            // partsComboBox
            // 
            partsComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            partsComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            partsComboBox.FormattingEnabled = true;
            partsComboBox.Location = new Point(120, 57);
            partsComboBox.Name = "partsComboBox";
            partsComboBox.Size = new Size(300, 23);
            partsComboBox.TabIndex = 2;
            partsComboBox.SelectedIndexChanged += PartsComboBox_SelectedIndexChanged;
            // 
            // detailsGroupBox
            // 
            detailsGroupBox.Controls.Add(issuedByValueLabel);
            detailsGroupBox.Controls.Add(issuedByLabel);
            detailsGroupBox.Controls.Add(typeValueLabel);
            detailsGroupBox.Controls.Add(typeLabel);
            detailsGroupBox.Controls.Add(descriptionValueLabel);
            detailsGroupBox.Controls.Add(descriptionLabel);
            detailsGroupBox.Controls.Add(customerValueLabel);
            detailsGroupBox.Controls.Add(customerLabel);
            detailsGroupBox.Controls.Add(itemNumberValueLabel);
            detailsGroupBox.Controls.Add(itemNumberLabel);
            detailsGroupBox.Location = new Point(20, 100);
            detailsGroupBox.Name = "detailsGroupBox";
            detailsGroupBox.Size = new Size(400, 180);
            detailsGroupBox.TabIndex = 3;
            detailsGroupBox.TabStop = false;
            detailsGroupBox.Text = "Part Details";
            detailsGroupBox.Visible = false;
            // 
            // issuedByValueLabel
            // 
            issuedByValueLabel.AutoSize = true;
            issuedByValueLabel.Location = new Point(100, 140);
            issuedByValueLabel.Name = "issuedByValueLabel";
            issuedByValueLabel.Size = new Size(0, 15);
            issuedByValueLabel.TabIndex = 9;
            // 
            // issuedByLabel
            // 
            issuedByLabel.AutoSize = true;
            issuedByLabel.Location = new Point(15, 140);
            issuedByLabel.Name = "issuedByLabel";
            issuedByLabel.Size = new Size(59, 15);
            issuedByLabel.TabIndex = 8;
            issuedByLabel.Text = "Issued By:";
            // 
            // typeValueLabel
            // 
            typeValueLabel.AutoSize = true;
            typeValueLabel.Location = new Point(100, 115);
            typeValueLabel.Name = "typeValueLabel";
            typeValueLabel.Size = new Size(0, 15);
            typeValueLabel.TabIndex = 7;
            // 
            // typeLabel
            // 
            typeLabel.AutoSize = true;
            typeLabel.Location = new Point(15, 115);
            typeLabel.Name = "typeLabel";
            typeLabel.Size = new Size(35, 15);
            typeLabel.TabIndex = 6;
            typeLabel.Text = "Type:";
            // 
            // descriptionValueLabel
            // 
            descriptionValueLabel.Location = new Point(100, 65);
            descriptionValueLabel.Name = "descriptionValueLabel";
            descriptionValueLabel.Size = new Size(280, 40);
            descriptionValueLabel.TabIndex = 5;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new Point(15, 65);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(70, 15);
            descriptionLabel.TabIndex = 4;
            descriptionLabel.Text = "Description:";
            // 
            // customerValueLabel
            // 
            customerValueLabel.AutoSize = true;
            customerValueLabel.Location = new Point(100, 40);
            customerValueLabel.Name = "customerValueLabel";
            customerValueLabel.Size = new Size(0, 15);
            customerValueLabel.TabIndex = 3;
            // 
            // customerLabel
            // 
            customerLabel.AutoSize = true;
            customerLabel.Location = new Point(15, 40);
            customerLabel.Name = "customerLabel";
            customerLabel.Size = new Size(62, 15);
            customerLabel.TabIndex = 2;
            customerLabel.Text = "Customer:";
            // 
            // itemNumberValueLabel
            // 
            itemNumberValueLabel.AutoSize = true;
            itemNumberValueLabel.Location = new Point(100, 20);
            itemNumberValueLabel.Name = "itemNumberValueLabel";
            itemNumberValueLabel.Size = new Size(0, 15);
            itemNumberValueLabel.TabIndex = 1;
            // 
            // itemNumberLabel
            // 
            itemNumberLabel.AutoSize = true;
            itemNumberLabel.Location = new Point(15, 20);
            itemNumberLabel.Name = "itemNumberLabel";
            itemNumberLabel.Size = new Size(81, 15);
            itemNumberLabel.TabIndex = 0;
            itemNumberLabel.Text = "Item Number:";
            // 
            // warningLabel
            // 
            warningLabel.AutoSize = true;
            warningLabel.ForeColor = Color.Red;
            warningLabel.Location = new Point(20, 300);
            warningLabel.Name = "warningLabel";
            warningLabel.Size = new Size(306, 15);
            warningLabel.TabIndex = 4;
            warningLabel.Text = "Warning: This action cannot be undone once confirmed.";
            // 
            // removeButton
            // 
            removeButton.BackColor = Color.IndianRed;
            removeButton.Enabled = false;
            removeButton.ForeColor = Color.White;
            removeButton.Location = new Point(265, 330);
            removeButton.Name = "removeButton";
            removeButton.Size = new Size(75, 23);
            removeButton.TabIndex = 5;
            removeButton.Text = "Remove";
            removeButton.UseVisualStyleBackColor = false;
            removeButton.Click += RemoveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(345, 330);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 6;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // Control_Remove_PartID
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(cancelButton);
            Controls.Add(removeButton);
            Controls.Add(warningLabel);
            Controls.Add(detailsGroupBox);
            Controls.Add(partsComboBox);
            Controls.Add(selectPartLabel);
            Controls.Add(titleLabel);
            Name = "Control_Remove_PartID";
            Size = new Size(450, 380);
            detailsGroupBox.ResumeLayout(false);
            detailsGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
    }

        
        #endregion
    }
