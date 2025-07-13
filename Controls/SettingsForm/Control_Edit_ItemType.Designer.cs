using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Edit_ItemType
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private Label titleLabel;
        private ComboBox itemTypesComboBox;
        private Label selectItemTypeLabel;
        private Label itemTypeLabel;
        private TextBox itemTypeTextBox;
        private Label originalIssuedByLabel;
        private Label originalIssuedByValueLabel;
        private Label issuedByLabel;
        private Label issuedByValueLabel;
        private Button saveButton;
        private Button cancelButton;
        

        
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
            itemTypeTextBox = new TextBox();
            originalIssuedByLabel = new Label();
            originalIssuedByValueLabel = new Label();
            issuedByLabel = new Label();
            issuedByValueLabel = new Label();
            saveButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(125, 20);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Edit ItemType";
            itemTypesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            itemTypesComboBox.FormattingEnabled = true;
            itemTypesComboBox.Location = new Point(20, 80);
            itemTypesComboBox.Name = "itemTypesComboBox";
            itemTypesComboBox.Size = new Size(300, 23);
            itemTypesComboBox.TabIndex = 1;
            itemTypesComboBox.SelectedIndexChanged += ItemTypesComboBox_SelectedIndexChanged;
            selectItemTypeLabel.AutoSize = true;
            selectItemTypeLabel.Location = new Point(20, 60);
            selectItemTypeLabel.Name = "selectItemTypeLabel";
            selectItemTypeLabel.Size = new Size(130, 15);
            selectItemTypeLabel.TabIndex = 2;
            selectItemTypeLabel.Text = "Select ItemType to Edit:";
            itemTypeLabel.AutoSize = true;
            itemTypeLabel.Location = new Point(20, 120);
            itemTypeLabel.Name = "itemTypeLabel";
            itemTypeLabel.Size = new Size(64, 15);
            itemTypeLabel.TabIndex = 3;
            itemTypeLabel.Text = "ItemType:";
            itemTypeTextBox.Enabled = false;
            itemTypeTextBox.Location = new Point(20, 140);
            itemTypeTextBox.MaxLength = 100;
            itemTypeTextBox.Name = "itemTypeTextBox";
            itemTypeTextBox.Size = new Size(300, 23);
            itemTypeTextBox.TabIndex = 4;
            originalIssuedByLabel.AutoSize = true;
            originalIssuedByLabel.Location = new Point(20, 180);
            originalIssuedByLabel.Name = "originalIssuedByLabel";
            originalIssuedByLabel.Size = new Size(102, 15);
            originalIssuedByLabel.TabIndex = 5;
            originalIssuedByLabel.Text = "Originally Issued By:";
            originalIssuedByValueLabel.AutoSize = true;
            originalIssuedByValueLabel.Location = new Point(20, 200);
            originalIssuedByValueLabel.Name = "originalIssuedByValueLabel";
            originalIssuedByValueLabel.Size = new Size(0, 15);
            originalIssuedByValueLabel.TabIndex = 6;
            issuedByLabel.AutoSize = true;
            issuedByLabel.Location = new Point(20, 240);
            issuedByLabel.Name = "issuedByLabel";
            issuedByLabel.Size = new Size(60, 15);
            issuedByLabel.TabIndex = 7;
            issuedByLabel.Text = "Issued By:";
            issuedByValueLabel.AutoSize = true;
            issuedByValueLabel.Location = new Point(20, 260);
            issuedByValueLabel.Name = "issuedByValueLabel";
            issuedByValueLabel.Size = new Size(77, 15);
            issuedByValueLabel.TabIndex = 8;
            issuedByValueLabel.Text = "Current User";
            saveButton.Enabled = false;
            saveButton.Location = new Point(20, 300);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 9;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            cancelButton.Location = new Point(110, 300);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 10;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(titleLabel);
            Controls.Add(itemTypesComboBox);
            Controls.Add(selectItemTypeLabel);
            Controls.Add(itemTypeLabel);
            Controls.Add(itemTypeTextBox);
            Controls.Add(originalIssuedByLabel);
            Controls.Add(originalIssuedByValueLabel);
            Controls.Add(issuedByLabel);
            Controls.Add(issuedByValueLabel);
            Controls.Add(saveButton);
            Controls.Add(cancelButton);
            Name = "Control_Edit_ItemType";
            Size = new Size(400, 350);
            ResumeLayout(false);
            PerformLayout();
        }
    }

        
        #endregion
    }
