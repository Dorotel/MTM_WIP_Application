using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Remove_ItemType
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private Label titleLabel;
        private ComboBox itemTypesComboBox;
        private Label selectItemTypeLabel;
        private Label itemTypeLabel;
        private Label itemTypeValueLabel;
        private Label originalIssuedByLabel;
        private Label originalIssuedByValueLabel;
        private Label issuedByLabel;
        private Label issuedByValueLabel;
        private Button removeButton;
        private Button cancelButton;
        private Label warningLabel;
        

        
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
            itemTypesComboBox = new ComboBox();
            selectItemTypeLabel = new Label();
            itemTypeLabel = new Label();
            itemTypeValueLabel = new Label();
            originalIssuedByLabel = new Label();
            originalIssuedByValueLabel = new Label();
            issuedByLabel = new Label();
            issuedByValueLabel = new Label();
            removeButton = new Button();
            cancelButton = new Button();
            warningLabel = new Label();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(153, 20);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Remove ItemType";
            // 
            // itemTypesComboBox
            // 
            itemTypesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            itemTypesComboBox.FormattingEnabled = true;
            itemTypesComboBox.Location = new Point(20, 80);
            itemTypesComboBox.Name = "itemTypesComboBox";
            itemTypesComboBox.Size = new Size(300, 23);
            itemTypesComboBox.TabIndex = 1;
            itemTypesComboBox.SelectedIndexChanged += ItemTypesComboBox_SelectedIndexChanged;
            // 
            // selectItemTypeLabel
            // 
            selectItemTypeLabel.AutoSize = true;
            selectItemTypeLabel.Location = new Point(20, 60);
            selectItemTypeLabel.Name = "selectItemTypeLabel";
            selectItemTypeLabel.Size = new Size(153, 15);
            selectItemTypeLabel.TabIndex = 2;
            selectItemTypeLabel.Text = "Select ItemType to Remove:";
            // 
            // itemTypeLabel
            // 
            itemTypeLabel.AutoSize = true;
            itemTypeLabel.Location = new Point(20, 120);
            itemTypeLabel.Name = "itemTypeLabel";
            itemTypeLabel.Size = new Size(59, 15);
            itemTypeLabel.TabIndex = 3;
            itemTypeLabel.Text = "ItemType:";
            // 
            // itemTypeValueLabel
            // 
            itemTypeValueLabel.AutoSize = true;
            itemTypeValueLabel.Location = new Point(20, 140);
            itemTypeValueLabel.Name = "itemTypeValueLabel";
            itemTypeValueLabel.Size = new Size(0, 15);
            itemTypeValueLabel.TabIndex = 4;
            // 
            // originalIssuedByLabel
            // 
            originalIssuedByLabel.AutoSize = true;
            originalIssuedByLabel.Location = new Point(20, 170);
            originalIssuedByLabel.Name = "originalIssuedByLabel";
            originalIssuedByLabel.Size = new Size(59, 15);
            originalIssuedByLabel.TabIndex = 5;
            originalIssuedByLabel.Text = "Issued By:";
            // 
            // originalIssuedByValueLabel
            // 
            originalIssuedByValueLabel.AutoSize = true;
            originalIssuedByValueLabel.Location = new Point(20, 190);
            originalIssuedByValueLabel.Name = "originalIssuedByValueLabel";
            originalIssuedByValueLabel.Size = new Size(0, 15);
            originalIssuedByValueLabel.TabIndex = 6;
            // 
            // issuedByLabel
            // 
            issuedByLabel.AutoSize = true;
            issuedByLabel.Location = new Point(20, 220);
            issuedByLabel.Name = "issuedByLabel";
            issuedByLabel.Size = new Size(76, 15);
            issuedByLabel.TabIndex = 7;
            issuedByLabel.Text = "Removed By:";
            // 
            // issuedByValueLabel
            // 
            issuedByValueLabel.AutoSize = true;
            issuedByValueLabel.Location = new Point(20, 240);
            issuedByValueLabel.Name = "issuedByValueLabel";
            issuedByValueLabel.Size = new Size(73, 15);
            issuedByValueLabel.TabIndex = 8;
            issuedByValueLabel.Text = "Current User";
            // 
            // removeButton
            // 
            removeButton.Enabled = false;
            removeButton.Location = new Point(20, 320);
            removeButton.Name = "removeButton";
            removeButton.Size = new Size(75, 23);
            removeButton.TabIndex = 9;
            removeButton.Text = "Remove";
            removeButton.UseVisualStyleBackColor = true;
            removeButton.Click += RemoveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(110, 320);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 10;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // warningLabel
            // 
            warningLabel.AutoSize = true;
            warningLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            warningLabel.ForeColor = Color.Red;
            warningLabel.Location = new Point(20, 280);
            warningLabel.Name = "warningLabel";
            warningLabel.Size = new Size(249, 13);
            warningLabel.TabIndex = 11;
            warningLabel.Text = "WARNING: This action cannot be undone!";
            // 
            // Control_Remove_ItemType
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(titleLabel);
            Controls.Add(itemTypesComboBox);
            Controls.Add(selectItemTypeLabel);
            Controls.Add(itemTypeLabel);
            Controls.Add(itemTypeValueLabel);
            Controls.Add(originalIssuedByLabel);
            Controls.Add(originalIssuedByValueLabel);
            Controls.Add(issuedByLabel);
            Controls.Add(issuedByValueLabel);
            Controls.Add(removeButton);
            Controls.Add(cancelButton);
            Controls.Add(warningLabel);
            Name = "Control_Remove_ItemType";
            Size = new Size(400, 370);
            ResumeLayout(false);
            PerformLayout();
        }
    }

        
        #endregion
    }
