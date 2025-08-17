namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class Control_Add_PartID
    {
        #region Fields

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label itemNumberLabel;
        private System.Windows.Forms.TextBox itemNumberTextBox;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox Control_Add_PartID_ComboBox_ItemType;
        private System.Windows.Forms.Label issuedByLabel;
        private System.Windows.Forms.Label issuedByValueLabel;
        private System.Windows.Forms.Button saveButton;
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

        #endregion

        #region Component Designer generated code

        private void InitializeComponent()
        {
            itemNumberLabel = new Label();
            itemNumberTextBox = new TextBox();
            typeLabel = new Label();
            Control_Add_PartID_ComboBox_ItemType = new ComboBox();
            issuedByLabel = new Label();
            issuedByValueLabel = new Label();
            saveButton = new Button();
            cancelButton = new Button();
            Control_Add_PartID_GroupBox_NewPartID = new GroupBox();
            Control_Add_PartID_TableLayout_NewPartIDEntry = new TableLayoutPanel();
            Control_Add_PartID_GroupBox_NewPartID.SuspendLayout();
            Control_Add_PartID_TableLayout_NewPartIDEntry.SuspendLayout();
            SuspendLayout();
            // 
            // itemNumberLabel
            // 
            itemNumberLabel.Location = new Point(3, 0);
            itemNumberLabel.Name = "itemNumberLabel";
            itemNumberLabel.Size = new Size(81, 29);
            itemNumberLabel.TabIndex = 1;
            itemNumberLabel.Text = "Item Number:";
            itemNumberLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // itemNumberTextBox
            // 
            Control_Add_PartID_TableLayout_NewPartIDEntry.SetColumnSpan(itemNumberTextBox, 3);
            itemNumberTextBox.Dock = DockStyle.Fill;
            itemNumberTextBox.Location = new Point(90, 3);
            itemNumberTextBox.Name = "itemNumberTextBox";
            itemNumberTextBox.Size = new Size(343, 23);
            itemNumberTextBox.TabIndex = 2;
            // 
            // typeLabel
            // 
            typeLabel.Location = new Point(3, 49);
            typeLabel.Name = "typeLabel";
            typeLabel.Size = new Size(81, 29);
            typeLabel.TabIndex = 7;
            typeLabel.Text = "Type:";
            typeLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_Add_PartID_ComboBox_ItemType
            // 
            Control_Add_PartID_TableLayout_NewPartIDEntry.SetColumnSpan(Control_Add_PartID_ComboBox_ItemType, 3);
            Control_Add_PartID_ComboBox_ItemType.Dock = DockStyle.Fill;
            Control_Add_PartID_ComboBox_ItemType.DropDownStyle = ComboBoxStyle.DropDownList;
            Control_Add_PartID_ComboBox_ItemType.FormattingEnabled = true;
            Control_Add_PartID_ComboBox_ItemType.Location = new Point(90, 52);
            Control_Add_PartID_ComboBox_ItemType.Name = "Control_Add_PartID_ComboBox_ItemType";
            Control_Add_PartID_ComboBox_ItemType.Size = new Size(343, 23);
            Control_Add_PartID_ComboBox_ItemType.TabIndex = 8;
            // 
            // issuedByLabel
            // 
            issuedByLabel.Location = new Point(3, 98);
            issuedByLabel.Name = "issuedByLabel";
            issuedByLabel.Size = new Size(81, 15);
            issuedByLabel.TabIndex = 9;
            issuedByLabel.Text = "Issued By:";
            issuedByLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // issuedByValueLabel
            // 
            issuedByValueLabel.AutoSize = true;
            Control_Add_PartID_TableLayout_NewPartIDEntry.SetColumnSpan(issuedByValueLabel, 3);
            issuedByValueLabel.Dock = DockStyle.Fill;
            issuedByValueLabel.Location = new Point(90, 98);
            issuedByValueLabel.Name = "issuedByValueLabel";
            issuedByValueLabel.Size = new Size(343, 15);
            issuedByValueLabel.TabIndex = 10;
            issuedByValueLabel.Text = "Current User";
            issuedByValueLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // saveButton
            // 
            saveButton.AutoSize = true;
            saveButton.Dock = DockStyle.Fill;
            saveButton.Location = new Point(257, 136);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(66, 25);
            saveButton.TabIndex = 11;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.AutoSize = true;
            cancelButton.Dock = DockStyle.Fill;
            cancelButton.Location = new Point(329, 136);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(104, 25);
            cancelButton.TabIndex = 12;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // Control_Add_PartID_GroupBox_NewPartID
            // 
            Control_Add_PartID_GroupBox_NewPartID.Controls.Add(Control_Add_PartID_TableLayout_NewPartIDEntry);
            Control_Add_PartID_GroupBox_NewPartID.Dock = DockStyle.Fill;
            Control_Add_PartID_GroupBox_NewPartID.Location = new Point(0, 0);
            Control_Add_PartID_GroupBox_NewPartID.Name = "Control_Add_PartID_GroupBox_NewPartID";
            Control_Add_PartID_GroupBox_NewPartID.Size = new Size(442, 360);
            Control_Add_PartID_GroupBox_NewPartID.TabIndex = 13;
            Control_Add_PartID_GroupBox_NewPartID.TabStop = false;
            Control_Add_PartID_GroupBox_NewPartID.Text = "New Part ID Entry";
            // 
            // Control_Add_PartID_TableLayout_NewPartIDEntry
            // 
            Control_Add_PartID_TableLayout_NewPartIDEntry.AutoSize = true;
            Control_Add_PartID_TableLayout_NewPartIDEntry.ColumnCount = 4;
            Control_Add_PartID_TableLayout_NewPartIDEntry.ColumnStyles.Add(new ColumnStyle());
            Control_Add_PartID_TableLayout_NewPartIDEntry.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_Add_PartID_TableLayout_NewPartIDEntry.ColumnStyles.Add(new ColumnStyle());
            Control_Add_PartID_TableLayout_NewPartIDEntry.ColumnStyles.Add(new ColumnStyle());
            Control_Add_PartID_TableLayout_NewPartIDEntry.Controls.Add(itemNumberLabel, 0, 0);
            Control_Add_PartID_TableLayout_NewPartIDEntry.Controls.Add(issuedByLabel, 0, 4);
            Control_Add_PartID_TableLayout_NewPartIDEntry.Controls.Add(typeLabel, 0, 2);
            Control_Add_PartID_TableLayout_NewPartIDEntry.Controls.Add(Control_Add_PartID_ComboBox_ItemType, 1, 2);
            Control_Add_PartID_TableLayout_NewPartIDEntry.Controls.Add(issuedByValueLabel, 1, 4);
            Control_Add_PartID_TableLayout_NewPartIDEntry.Controls.Add(itemNumberTextBox, 1, 0);
            Control_Add_PartID_TableLayout_NewPartIDEntry.Controls.Add(cancelButton, 3, 6);
            Control_Add_PartID_TableLayout_NewPartIDEntry.Controls.Add(saveButton, 2, 6);
            Control_Add_PartID_TableLayout_NewPartIDEntry.Dock = DockStyle.Fill;
            Control_Add_PartID_TableLayout_NewPartIDEntry.Location = new Point(3, 19);
            Control_Add_PartID_TableLayout_NewPartIDEntry.Name = "Control_Add_PartID_TableLayout_NewPartIDEntry";
            Control_Add_PartID_TableLayout_NewPartIDEntry.RowCount = 8;
            Control_Add_PartID_TableLayout_NewPartIDEntry.RowStyles.Add(new RowStyle());
            Control_Add_PartID_TableLayout_NewPartIDEntry.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            Control_Add_PartID_TableLayout_NewPartIDEntry.RowStyles.Add(new RowStyle());
            Control_Add_PartID_TableLayout_NewPartIDEntry.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            Control_Add_PartID_TableLayout_NewPartIDEntry.RowStyles.Add(new RowStyle());
            Control_Add_PartID_TableLayout_NewPartIDEntry.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            Control_Add_PartID_TableLayout_NewPartIDEntry.RowStyles.Add(new RowStyle());
            Control_Add_PartID_TableLayout_NewPartIDEntry.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_Add_PartID_TableLayout_NewPartIDEntry.Size = new Size(436, 338);
            Control_Add_PartID_TableLayout_NewPartIDEntry.TabIndex = 0;
            // 
            // Control_Add_PartID
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            Controls.Add(Control_Add_PartID_GroupBox_NewPartID);
            Name = "Control_Add_PartID";
            Size = new Size(442, 360);
            Control_Add_PartID_GroupBox_NewPartID.ResumeLayout(false);
            Control_Add_PartID_GroupBox_NewPartID.PerformLayout();
            Control_Add_PartID_TableLayout_NewPartIDEntry.ResumeLayout(false);
            Control_Add_PartID_TableLayout_NewPartIDEntry.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox Control_Add_PartID_GroupBox_NewPartID;
        private TableLayoutPanel Control_Add_PartID_TableLayout_NewPartIDEntry;
    }
}
