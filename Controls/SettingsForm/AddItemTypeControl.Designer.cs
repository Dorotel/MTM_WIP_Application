namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class AddItemTypeControl
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private System.Windows.Forms.Label Control_AddItemType_Label_Title;
        private System.Windows.Forms.Label Control_AddItemType_Label_ItemType;
        private System.Windows.Forms.TextBox itemTypeTextBox;
        private System.Windows.Forms.Label issuedByLabel;
        private System.Windows.Forms.Label issuedByValueLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button clearButton;
        
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
            this.Control_AddItemType_Label_Title = new System.Windows.Forms.Label();
            this.Control_AddItemType_Label_ItemType = new System.Windows.Forms.Label();
            this.itemTypeTextBox = new System.Windows.Forms.TextBox();
            this.issuedByLabel = new System.Windows.Forms.Label();
            this.issuedByValueLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.Control_AddItemType_Label_Title.AutoSize = true;
            this.Control_AddItemType_Label_Title.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Control_AddItemType_Label_Title.Location = new System.Drawing.Point(20, 20);
            this.Control_AddItemType_Label_Title.Name = "Control_AddItemType_Label_Title";
            this.Control_AddItemType_Label_Title.Size = new System.Drawing.Size(121, 21);
            this.Control_AddItemType_Label_Title.TabIndex = 0;
            this.Control_AddItemType_Label_Title.Text = "Add ItemType";
            this.Control_AddItemType_Label_ItemType.AutoSize = true;
            this.Control_AddItemType_Label_ItemType.Location = new System.Drawing.Point(20, 60);
            this.Control_AddItemType_Label_ItemType.Name = "Control_AddItemType_Label_ItemType";
            this.Control_AddItemType_Label_ItemType.Size = new System.Drawing.Size(65, 15);
            this.Control_AddItemType_Label_ItemType.TabIndex = 1;
            this.Control_AddItemType_Label_ItemType.Text = "ItemType:";
            this.itemTypeTextBox.Location = new System.Drawing.Point(120, 57);
            this.itemTypeTextBox.Name = "itemTypeTextBox";
            this.itemTypeTextBox.Size = new System.Drawing.Size(300, 23);
            this.itemTypeTextBox.TabIndex = 2;
            this.issuedByLabel.AutoSize = true;
            this.issuedByLabel.Location = new System.Drawing.Point(20, 100);
            this.issuedByLabel.Name = "issuedByLabel";
            this.issuedByLabel.Size = new System.Drawing.Size(61, 15);
            this.issuedByLabel.TabIndex = 3;
            this.issuedByLabel.Text = "Issued By:";
            this.issuedByValueLabel.AutoSize = true;
            this.issuedByValueLabel.Location = new System.Drawing.Point(120, 100);
            this.issuedByValueLabel.Name = "issuedByValueLabel";
            this.issuedByValueLabel.Size = new System.Drawing.Size(83, 15);
            this.issuedByValueLabel.TabIndex = 4;
            this.issuedByValueLabel.Text = "Current User";
            this.saveButton.Location = new System.Drawing.Point(265, 150);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            this.clearButton.Location = new System.Drawing.Point(345, 150);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 6;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.issuedByValueLabel);
            this.Controls.Add(this.issuedByLabel);
            this.Controls.Add(this.itemTypeTextBox);
            this.Controls.Add(this.Control_AddItemType_Label_ItemType);
            this.Controls.Add(this.Control_AddItemType_Label_Title);
            this.Name = "AddItemTypeControl";
            this.Size = new System.Drawing.Size(450, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }

        
        #endregion
    }