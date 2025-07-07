namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class RemovePartControl
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
            this.detailsGroupBox = new System.Windows.Forms.GroupBox();
            this.issuedByValueLabel = new System.Windows.Forms.Label();
            this.issuedByLabel = new System.Windows.Forms.Label();
            this.typeValueLabel = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.descriptionValueLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.customerValueLabel = new System.Windows.Forms.Label();
            this.customerLabel = new System.Windows.Forms.Label();
            this.itemNumberValueLabel = new System.Windows.Forms.Label();
            this.itemNumberLabel = new System.Windows.Forms.Label();
            this.warningLabel = new System.Windows.Forms.Label();
            this.removeButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.detailsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(168, 21);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Remove Part Number";
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
            // detailsGroupBox
            // 
            this.detailsGroupBox.Controls.Add(this.issuedByValueLabel);
            this.detailsGroupBox.Controls.Add(this.issuedByLabel);
            this.detailsGroupBox.Controls.Add(this.typeValueLabel);
            this.detailsGroupBox.Controls.Add(this.typeLabel);
            this.detailsGroupBox.Controls.Add(this.descriptionValueLabel);
            this.detailsGroupBox.Controls.Add(this.descriptionLabel);
            this.detailsGroupBox.Controls.Add(this.customerValueLabel);
            this.detailsGroupBox.Controls.Add(this.customerLabel);
            this.detailsGroupBox.Controls.Add(this.itemNumberValueLabel);
            this.detailsGroupBox.Controls.Add(this.itemNumberLabel);
            this.detailsGroupBox.Location = new System.Drawing.Point(20, 100);
            this.detailsGroupBox.Name = "detailsGroupBox";
            this.detailsGroupBox.Size = new System.Drawing.Size(400, 180);
            this.detailsGroupBox.TabIndex = 3;
            this.detailsGroupBox.TabStop = false;
            this.detailsGroupBox.Text = "Part Details";
            this.detailsGroupBox.Visible = false;
            // 
            // issuedByValueLabel
            // 
            this.issuedByValueLabel.AutoSize = true;
            this.issuedByValueLabel.Location = new System.Drawing.Point(100, 140);
            this.issuedByValueLabel.Name = "issuedByValueLabel";
            this.issuedByValueLabel.Size = new System.Drawing.Size(0, 15);
            this.issuedByValueLabel.TabIndex = 9;
            // 
            // issuedByLabel
            // 
            this.issuedByLabel.AutoSize = true;
            this.issuedByLabel.Location = new System.Drawing.Point(15, 140);
            this.issuedByLabel.Name = "issuedByLabel";
            this.issuedByLabel.Size = new System.Drawing.Size(61, 15);
            this.issuedByLabel.TabIndex = 8;
            this.issuedByLabel.Text = "Issued By:";
            // 
            // typeValueLabel
            // 
            this.typeValueLabel.AutoSize = true;
            this.typeValueLabel.Location = new System.Drawing.Point(100, 115);
            this.typeValueLabel.Name = "typeValueLabel";
            this.typeValueLabel.Size = new System.Drawing.Size(0, 15);
            this.typeValueLabel.TabIndex = 7;
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(15, 115);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(34, 15);
            this.typeLabel.TabIndex = 6;
            this.typeLabel.Text = "Type:";
            // 
            // descriptionValueLabel
            // 
            this.descriptionValueLabel.Location = new System.Drawing.Point(100, 65);
            this.descriptionValueLabel.Name = "descriptionValueLabel";
            this.descriptionValueLabel.Size = new System.Drawing.Size(280, 40);
            this.descriptionValueLabel.TabIndex = 5;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(15, 65);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(70, 15);
            this.descriptionLabel.TabIndex = 4;
            this.descriptionLabel.Text = "Description:";
            // 
            // customerValueLabel
            // 
            this.customerValueLabel.AutoSize = true;
            this.customerValueLabel.Location = new System.Drawing.Point(100, 40);
            this.customerValueLabel.Name = "customerValueLabel";
            this.customerValueLabel.Size = new System.Drawing.Size(0, 15);
            this.customerValueLabel.TabIndex = 3;
            // 
            // customerLabel
            // 
            this.customerLabel.AutoSize = true;
            this.customerLabel.Location = new System.Drawing.Point(15, 40);
            this.customerLabel.Name = "customerLabel";
            this.customerLabel.Size = new System.Drawing.Size(62, 15);
            this.customerLabel.TabIndex = 2;
            this.customerLabel.Text = "Customer:";
            // 
            // itemNumberValueLabel
            // 
            this.itemNumberValueLabel.AutoSize = true;
            this.itemNumberValueLabel.Location = new System.Drawing.Point(100, 20);
            this.itemNumberValueLabel.Name = "itemNumberValueLabel";
            this.itemNumberValueLabel.Size = new System.Drawing.Size(0, 15);
            this.itemNumberValueLabel.TabIndex = 1;
            // 
            // itemNumberLabel
            // 
            this.itemNumberLabel.AutoSize = true;
            this.itemNumberLabel.Location = new System.Drawing.Point(15, 20);
            this.itemNumberLabel.Name = "itemNumberLabel";
            this.itemNumberLabel.Size = new System.Drawing.Size(85, 15);
            this.itemNumberLabel.TabIndex = 0;
            this.itemNumberLabel.Text = "Item Number:";
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.ForeColor = System.Drawing.Color.Red;
            this.warningLabel.Location = new System.Drawing.Point(20, 300);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(308, 15);
            this.warningLabel.TabIndex = 4;
            this.warningLabel.Text = "Warning: This action cannot be undone once confirmed.";
            // 
            // removeButton
            // 
            this.removeButton.BackColor = System.Drawing.Color.IndianRed;
            this.removeButton.Enabled = false;
            this.removeButton.ForeColor = System.Drawing.Color.White;
            this.removeButton.Location = new System.Drawing.Point(265, 330);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 5;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = false;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(345, 330);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // RemovePartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.detailsGroupBox);
            this.Controls.Add(this.partsComboBox);
            this.Controls.Add(this.selectPartLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "RemovePartControl";
            this.Size = new System.Drawing.Size(450, 380);
            this.detailsGroupBox.ResumeLayout(false);
            this.detailsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

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
    }
}