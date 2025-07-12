namespace MTM_Inventory_Application.Controls.MainForm
{
    partial class ControlTransferTab
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
            Control_TransferTab_Panel_Main = new TableLayoutPanel();
            Control_TransferTab_Panel_DataGridView = new Panel();
            Control_TransferTab_Image_NothingFound = new PictureBox();
            Control_TransferTab_DataGridView_Main = new DataGridView();
            Control_TransferTab_Panel_Header = new Panel();
            Control_TransferTab_Button_Toggle_RightPanel = new Button();
            Control_TransferTab_ComboBox_Operation = new ComboBox();
            Control_TransferTab_ComboBox_Part = new ComboBox();
            Control_TransferTab_Button_Reset = new Button();
            Control_TransferTab_Label_Part = new Label();
            Control_TransferTab_Button_Transfer = new Button();
            Control_TransferTab_Label_Operation = new Label();
            Control_TransferTab_Button_Search = new Button();
            Control_TransferTab_Label_ToLocation = new Label();
            Control_TransferTab_ComboBox_ToLocation = new ComboBox();
            Control_TransferTab_Label_Quantity = new Label();
            Control_TransferTab_NumericUpDown_Quantity = new NumericUpDown();
            Control_TransferTab_GroupBox_MainControl = new GroupBox();
            Control_TransferTab_Panel_Main.SuspendLayout();
            Control_TransferTab_Panel_DataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_Image_NothingFound).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_DataGridView_Main).BeginInit();
            Control_TransferTab_Panel_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_NumericUpDown_Quantity).BeginInit();
            Control_TransferTab_GroupBox_MainControl.SuspendLayout();
            SuspendLayout();
            // 
            // Control_TransferTab_Panel_Main
            // 
            Control_TransferTab_Panel_Main.ColumnCount = 1;
            Control_TransferTab_Panel_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_TransferTab_Panel_Main.Controls.Add(Control_TransferTab_Panel_DataGridView, 0, 1);
            Control_TransferTab_Panel_Main.Controls.Add(Control_TransferTab_Panel_Header, 0, 0);
            Control_TransferTab_Panel_Main.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Main.Location = new Point(3, 19);
            Control_TransferTab_Panel_Main.Name = "Control_TransferTab_Panel_Main";
            Control_TransferTab_Panel_Main.RowCount = 2;
            Control_TransferTab_Panel_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            Control_TransferTab_Panel_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_TransferTab_Panel_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            Control_TransferTab_Panel_Main.Size = new Size(809, 362);
            Control_TransferTab_Panel_Main.TabIndex = 0;
            // 
            // Control_TransferTab_Panel_DataGridView
            // 
            Control_TransferTab_Panel_DataGridView.Controls.Add(Control_TransferTab_Image_NothingFound);
            Control_TransferTab_Panel_DataGridView.Controls.Add(Control_TransferTab_DataGridView_Main);
            Control_TransferTab_Panel_DataGridView.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_DataGridView.Location = new Point(3, 78);
            Control_TransferTab_Panel_DataGridView.Name = "Control_TransferTab_Panel_DataGridView";
            Control_TransferTab_Panel_DataGridView.Size = new Size(803, 281);
            Control_TransferTab_Panel_DataGridView.TabIndex = 8;
            Control_TransferTab_Panel_DataGridView.TabStop = true;
            // 
            // Control_TransferTab_Image_NothingFound
            // 
            Control_TransferTab_Image_NothingFound.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_Image_NothingFound.ErrorImage = null;
            Control_TransferTab_Image_NothingFound.Image = Properties.Resources.NothingFound;
            Control_TransferTab_Image_NothingFound.InitialImage = null;
            Control_TransferTab_Image_NothingFound.Location = new Point(0, 0);
            Control_TransferTab_Image_NothingFound.Name = "Control_TransferTab_Image_NothingFound";
            Control_TransferTab_Image_NothingFound.Size = new Size(803, 281);
            Control_TransferTab_Image_NothingFound.SizeMode = PictureBoxSizeMode.Zoom;
            Control_TransferTab_Image_NothingFound.TabIndex = 6;
            Control_TransferTab_Image_NothingFound.TabStop = false;
            // 
            // Control_TransferTab_DataGridView_Main
            // 
            Control_TransferTab_DataGridView_Main.AllowUserToAddRows = false;
            Control_TransferTab_DataGridView_Main.AllowUserToDeleteRows = false;
            Control_TransferTab_DataGridView_Main.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Control_TransferTab_DataGridView_Main.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Control_TransferTab_DataGridView_Main.BorderStyle = BorderStyle.Fixed3D;
            Control_TransferTab_DataGridView_Main.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            Control_TransferTab_DataGridView_Main.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            Control_TransferTab_DataGridView_Main.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            Control_TransferTab_DataGridView_Main.ColumnHeadersHeight = 34;
            Control_TransferTab_DataGridView_Main.Dock = DockStyle.Fill;
            Control_TransferTab_DataGridView_Main.EditMode = DataGridViewEditMode.EditProgrammatically;
            Control_TransferTab_DataGridView_Main.Location = new Point(0, 0);
            Control_TransferTab_DataGridView_Main.Name = "Control_TransferTab_DataGridView_Main";
            Control_TransferTab_DataGridView_Main.ReadOnly = true;
            Control_TransferTab_DataGridView_Main.RowHeadersWidth = 62;
            Control_TransferTab_DataGridView_Main.RowTemplate.ReadOnly = true;
            Control_TransferTab_DataGridView_Main.RowTemplate.Resizable = DataGridViewTriState.True;
            Control_TransferTab_DataGridView_Main.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Control_TransferTab_DataGridView_Main.ShowCellErrors = false;
            Control_TransferTab_DataGridView_Main.ShowCellToolTips = false;
            Control_TransferTab_DataGridView_Main.ShowEditingIcon = false;
            Control_TransferTab_DataGridView_Main.ShowRowErrors = false;
            Control_TransferTab_DataGridView_Main.Size = new Size(803, 281);
            Control_TransferTab_DataGridView_Main.StandardTab = true;
            Control_TransferTab_DataGridView_Main.TabIndex = 4;
            // 
            // Control_TransferTab_Panel_Header
            // 
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_Button_Toggle_RightPanel);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_ComboBox_Operation);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_ComboBox_Part);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_Button_Reset);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_Label_Part);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_Button_Transfer);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_Label_Operation);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_Button_Search);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_Label_ToLocation);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_ComboBox_ToLocation);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_Label_Quantity);
            Control_TransferTab_Panel_Header.Controls.Add(Control_TransferTab_NumericUpDown_Quantity);
            Control_TransferTab_Panel_Header.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Header.Location = new Point(3, 3);
            Control_TransferTab_Panel_Header.Name = "Control_TransferTab_Panel_Header";
            Control_TransferTab_Panel_Header.Size = new Size(803, 69);
            Control_TransferTab_Panel_Header.TabIndex = 22;
            // 
            // Control_TransferTab_Button_Toggle_RightPanel
            // 
            Control_TransferTab_Button_Toggle_RightPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_Button_Toggle_RightPanel.Font = new Font("Segoe UI", 10F);
            Control_TransferTab_Button_Toggle_RightPanel.ForeColor = Color.Green;
            Control_TransferTab_Button_Toggle_RightPanel.Location = new Point(768, 35);
            Control_TransferTab_Button_Toggle_RightPanel.Name = "Control_TransferTab_Button_Toggle_RightPanel";
            Control_TransferTab_Button_Toggle_RightPanel.Size = new Size(28, 28);
            Control_TransferTab_Button_Toggle_RightPanel.TabIndex = 999;
            Control_TransferTab_Button_Toggle_RightPanel.TabStop = false;
            Control_TransferTab_Button_Toggle_RightPanel.Text = "←";
            Control_TransferTab_Button_Toggle_RightPanel.UseVisualStyleBackColor = true;
            Control_TransferTab_Button_Toggle_RightPanel.Click += Control_TransferTab_Button_Toggle_RightPanel_Click;
            // 
            // Control_TransferTab_ComboBox_Operation
            // 
            Control_TransferTab_ComboBox_Operation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_TransferTab_ComboBox_Operation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_TransferTab_ComboBox_Operation.FormattingEnabled = true;
            Control_TransferTab_ComboBox_Operation.Location = new Point(94, 37);
            Control_TransferTab_ComboBox_Operation.Name = "Control_TransferTab_ComboBox_Operation";
            Control_TransferTab_ComboBox_Operation.Size = new Size(171, 23);
            Control_TransferTab_ComboBox_Operation.TabIndex = 2;
            // 
            // Control_TransferTab_ComboBox_Part
            // 
            Control_TransferTab_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_TransferTab_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_TransferTab_ComboBox_Part.FormattingEnabled = true;
            Control_TransferTab_ComboBox_Part.Location = new Point(94, 8);
            Control_TransferTab_ComboBox_Part.Name = "Control_TransferTab_ComboBox_Part";
            Control_TransferTab_ComboBox_Part.Size = new Size(171, 23);
            Control_TransferTab_ComboBox_Part.TabIndex = 1;
            // 
            // Control_TransferTab_Button_Reset
            // 
            Control_TransferTab_Button_Reset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_Button_Reset.Location = new Point(678, 5);
            Control_TransferTab_Button_Reset.Name = "Control_TransferTab_Button_Reset";
            Control_TransferTab_Button_Reset.Size = new Size(118, 28);
            Control_TransferTab_Button_Reset.TabIndex = 7;
            Control_TransferTab_Button_Reset.TabStop = false;
            Control_TransferTab_Button_Reset.Text = "&Reset";
            Control_TransferTab_Button_Reset.UseVisualStyleBackColor = true;
            // 
            // Control_TransferTab_Label_Part
            // 
            Control_TransferTab_Label_Part.Location = new Point(3, 8);
            Control_TransferTab_Label_Part.Name = "Control_TransferTab_Label_Part";
            Control_TransferTab_Label_Part.Size = new Size(85, 23);
            Control_TransferTab_Label_Part.TabIndex = 4;
            Control_TransferTab_Label_Part.Text = "Part Number:";
            Control_TransferTab_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_Button_Transfer
            // 
            Control_TransferTab_Button_Transfer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_Button_Transfer.Enabled = false;
            Control_TransferTab_Button_Transfer.Location = new Point(547, 34);
            Control_TransferTab_Button_Transfer.Name = "Control_TransferTab_Button_Transfer";
            Control_TransferTab_Button_Transfer.Size = new Size(213, 28);
            Control_TransferTab_Button_Transfer.TabIndex = 6;
            Control_TransferTab_Button_Transfer.Text = "&Save";
            Control_TransferTab_Button_Transfer.UseVisualStyleBackColor = true;
            // 
            // Control_TransferTab_Label_Operation
            // 
            Control_TransferTab_Label_Operation.Location = new Point(3, 37);
            Control_TransferTab_Label_Operation.Name = "Control_TransferTab_Label_Operation";
            Control_TransferTab_Label_Operation.Size = new Size(85, 23);
            Control_TransferTab_Label_Operation.TabIndex = 5;
            Control_TransferTab_Label_Operation.Text = "Operation:";
            Control_TransferTab_Label_Operation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_Button_Search
            // 
            Control_TransferTab_Button_Search.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_Button_Search.Enabled = false;
            Control_TransferTab_Button_Search.Location = new Point(547, 5);
            Control_TransferTab_Button_Search.Name = "Control_TransferTab_Button_Search";
            Control_TransferTab_Button_Search.Size = new Size(118, 28);
            Control_TransferTab_Button_Search.TabIndex = 5;
            Control_TransferTab_Button_Search.Text = "Search";
            Control_TransferTab_Button_Search.UseVisualStyleBackColor = true;
            Control_TransferTab_Button_Search.Click += Control_TransferTab_Button_Search_Click;
            // 
            // Control_TransferTab_Label_ToLocation
            // 
            Control_TransferTab_Label_ToLocation.Location = new Point(271, 7);
            Control_TransferTab_Label_ToLocation.Name = "Control_TransferTab_Label_ToLocation";
            Control_TransferTab_Label_ToLocation.Size = new Size(86, 23);
            Control_TransferTab_Label_ToLocation.TabIndex = 8;
            Control_TransferTab_Label_ToLocation.Text = "To Location:";
            Control_TransferTab_Label_ToLocation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_ComboBox_ToLocation
            // 
            Control_TransferTab_ComboBox_ToLocation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Control_TransferTab_ComboBox_ToLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_TransferTab_ComboBox_ToLocation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_TransferTab_ComboBox_ToLocation.DrawMode = DrawMode.OwnerDrawVariable;
            Control_TransferTab_ComboBox_ToLocation.Enabled = false;
            Control_TransferTab_ComboBox_ToLocation.FormattingEnabled = true;
            Control_TransferTab_ComboBox_ToLocation.Location = new Point(363, 7);
            Control_TransferTab_ComboBox_ToLocation.Name = "Control_TransferTab_ComboBox_ToLocation";
            Control_TransferTab_ComboBox_ToLocation.Size = new Size(171, 24);
            Control_TransferTab_ComboBox_ToLocation.TabIndex = 3;
            // 
            // Control_TransferTab_Label_Quantity
            // 
            Control_TransferTab_Label_Quantity.Location = new Point(271, 37);
            Control_TransferTab_Label_Quantity.Name = "Control_TransferTab_Label_Quantity";
            Control_TransferTab_Label_Quantity.Size = new Size(86, 23);
            Control_TransferTab_Label_Quantity.TabIndex = 10;
            Control_TransferTab_Label_Quantity.Text = "Quantity:";
            Control_TransferTab_Label_Quantity.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_NumericUpDown_Quantity
            // 
            Control_TransferTab_NumericUpDown_Quantity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Control_TransferTab_NumericUpDown_Quantity.Enabled = false;
            Control_TransferTab_NumericUpDown_Quantity.Location = new Point(363, 37);
            Control_TransferTab_NumericUpDown_Quantity.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            Control_TransferTab_NumericUpDown_Quantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            Control_TransferTab_NumericUpDown_Quantity.Name = "Control_TransferTab_NumericUpDown_Quantity";
            Control_TransferTab_NumericUpDown_Quantity.Size = new Size(171, 23);
            Control_TransferTab_NumericUpDown_Quantity.TabIndex = 4;
            Control_TransferTab_NumericUpDown_Quantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // Control_TransferTab_GroupBox_MainControl
            // 
            Control_TransferTab_GroupBox_MainControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Control_TransferTab_GroupBox_MainControl.Controls.Add(Control_TransferTab_Panel_Main);
            Control_TransferTab_GroupBox_MainControl.Dock = DockStyle.Fill;
            Control_TransferTab_GroupBox_MainControl.FlatStyle = FlatStyle.Flat;
            Control_TransferTab_GroupBox_MainControl.Location = new Point(0, 0);
            Control_TransferTab_GroupBox_MainControl.Name = "Control_TransferTab_GroupBox_MainControl";
            Control_TransferTab_GroupBox_MainControl.Size = new Size(815, 384);
            Control_TransferTab_GroupBox_MainControl.TabIndex = 17;
            Control_TransferTab_GroupBox_MainControl.TabStop = false;
            Control_TransferTab_GroupBox_MainControl.Text = "Inventory Transfer";
            // 
            // ControlTransferTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(Control_TransferTab_GroupBox_MainControl);
            Name = "ControlTransferTab";
            Size = new Size(815, 384);
            Control_TransferTab_Panel_Main.ResumeLayout(false);
            Control_TransferTab_Panel_DataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_Image_NothingFound).EndInit();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_DataGridView_Main).EndInit();
            Control_TransferTab_Panel_Header.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_NumericUpDown_Quantity).EndInit();
            Control_TransferTab_GroupBox_MainControl.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel Control_TransferTab_Panel_Main;
        private Panel Control_TransferTab_Panel_DataGridView;
        private PictureBox Control_TransferTab_Image_NothingFound;
        private DataGridView Control_TransferTab_DataGridView_Main;
        private Button Control_TransferTab_Button_Toggle_RightPanel;
        private Button Control_TransferTab_Button_Reset;
        private Button Control_TransferTab_Button_Transfer;
        private Button Control_TransferTab_Button_Search;
        private Panel Control_TransferTab_Panel_Header;
        private ComboBox Control_TransferTab_ComboBox_Part;
        private Label Control_TransferTab_Label_Part;
        private Label Control_TransferTab_Label_Operation;
        private ComboBox Control_TransferTab_ComboBox_Operation;
        private ComboBox Control_TransferTab_ComboBox_ToLocation;
        private Label Control_TransferTab_Label_ToLocation;
        private NumericUpDown Control_TransferTab_NumericUpDown_Quantity;
        private Label Control_TransferTab_Label_Quantity;
        private GroupBox Control_TransferTab_GroupBox_MainControl;
    }
}
