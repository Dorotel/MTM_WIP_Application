using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.MainForm
{
    partial class Control_TransferTab
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion
        private GroupBox Control_TransferTab_GroupBox_MainControl;



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
            Control_TransferTab_GroupBox_MainControl = new GroupBox();
            Control_TransferTab_Panel_Main = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            Control_TransferTab_Button_Toggle_Split = new Button();
            Control_TransferTab_Button_Toggle_RightPanel = new Button();
            Control_TransferTab_SplitContainer_Main = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            Control_TransferTab_ComboBox_Operation = new ComboBox();
            Control_TransferTab_ComboBox_Part = new ComboBox();
            Control_TransferTab_NumericUpDown_Quantity = new NumericUpDown();
            Control_TransferTab_ComboBox_ToLocation = new ComboBox();
            Control_TransferTab_Label_Part = new Label();
            Control_TransferTab_Label_Operation = new Label();
            Control_TransferTab_Label_ToLocation = new Label();
            Control_TransferTab_Label_Quantity = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            Control_TransferTab_Button_Transfer = new Button();
            Control_TransferTab_Button_Search = new Button();
            Control_TransferTab_Button_Print = new Button();
            Control_TransferTab_Button_Reset = new Button();
            panel1 = new Panel();
            Control_TransferTab_Panel_DataGridView = new Panel();
            Control_TransferTab_Image_NothingFound = new PictureBox();
            Control_TransferTab_DataGridView_Main = new DataGridView();
            Control_TransferTab_GroupBox_MainControl.SuspendLayout();
            Control_TransferTab_Panel_Main.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_SplitContainer_Main).BeginInit();
            Control_TransferTab_SplitContainer_Main.Panel1.SuspendLayout();
            Control_TransferTab_SplitContainer_Main.Panel2.SuspendLayout();
            Control_TransferTab_SplitContainer_Main.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_NumericUpDown_Quantity).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            Control_TransferTab_Panel_DataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_Image_NothingFound).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_DataGridView_Main).BeginInit();
            SuspendLayout();
            // 
            // Control_TransferTab_GroupBox_MainControl
            // 
            Control_TransferTab_GroupBox_MainControl.AutoSize = true;
            Control_TransferTab_GroupBox_MainControl.Controls.Add(Control_TransferTab_Panel_Main);
            Control_TransferTab_GroupBox_MainControl.Dock = DockStyle.Fill;
            Control_TransferTab_GroupBox_MainControl.FlatStyle = FlatStyle.Flat;
            Control_TransferTab_GroupBox_MainControl.Location = new Point(0, 0);
            Control_TransferTab_GroupBox_MainControl.Name = "Control_TransferTab_GroupBox_MainControl";
            Control_TransferTab_GroupBox_MainControl.Size = new Size(967, 510);
            Control_TransferTab_GroupBox_MainControl.TabIndex = 17;
            Control_TransferTab_GroupBox_MainControl.TabStop = false;
            Control_TransferTab_GroupBox_MainControl.Text = "Inventory Transfer";
            // 
            // Control_TransferTab_Panel_Main
            // 
            Control_TransferTab_Panel_Main.ColumnCount = 1;
            Control_TransferTab_Panel_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_TransferTab_Panel_Main.Controls.Add(tableLayoutPanel3, 0, 1);
            Control_TransferTab_Panel_Main.Controls.Add(Control_TransferTab_SplitContainer_Main, 0, 0);
            Control_TransferTab_Panel_Main.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Main.Location = new Point(3, 19);
            Control_TransferTab_Panel_Main.Name = "Control_TransferTab_Panel_Main";
            Control_TransferTab_Panel_Main.RowCount = 2;
            Control_TransferTab_Panel_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_TransferTab_Panel_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Control_TransferTab_Panel_Main.Size = new Size(961, 488);
            Control_TransferTab_Panel_Main.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoSize = true;
            tableLayoutPanel3.ColumnCount = 3;
            Control_TransferTab_Panel_Main.SetColumnSpan(tableLayoutPanel3, 2);
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.Controls.Add(Control_TransferTab_Button_Toggle_Split, 0, 0);
            tableLayoutPanel3.Controls.Add(Control_TransferTab_Button_Toggle_RightPanel, 2, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 451);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(955, 34);
            tableLayoutPanel3.TabIndex = 12;
            // 
            // Control_TransferTab_Button_Toggle_Split
            // 
            Control_TransferTab_Button_Toggle_Split.AutoSize = true;
            Control_TransferTab_Button_Toggle_Split.Font = new Font("Segoe UI Emoji", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Control_TransferTab_Button_Toggle_Split.Location = new Point(3, 3);
            Control_TransferTab_Button_Toggle_Split.Name = "Control_TransferTab_Button_Toggle_Split";
            Control_TransferTab_Button_Toggle_Split.Size = new Size(111, 28);
            Control_TransferTab_Button_Toggle_Split.TabIndex = 1000;
            Control_TransferTab_Button_Toggle_Split.Text = "Transfer Panel ⬅️";
            Control_TransferTab_Button_Toggle_Split.UseVisualStyleBackColor = true;
            Control_TransferTab_Button_Toggle_Split.Click += Control_TransferTab_Button_Toggle_Split_Click;
            // 
            // Control_TransferTab_Button_Toggle_RightPanel
            // 
            Control_TransferTab_Button_Toggle_RightPanel.AutoSize = true;
            Control_TransferTab_Button_Toggle_RightPanel.Font = new Font("Segoe UI Emoji", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Control_TransferTab_Button_Toggle_RightPanel.Location = new Point(833, 3);
            Control_TransferTab_Button_Toggle_RightPanel.Name = "Control_TransferTab_Button_Toggle_RightPanel";
            Control_TransferTab_Button_Toggle_RightPanel.Size = new Size(119, 27);
            Control_TransferTab_Button_Toggle_RightPanel.TabIndex = 999;
            Control_TransferTab_Button_Toggle_RightPanel.TabStop = false;
            Control_TransferTab_Button_Toggle_RightPanel.Text = "Quick Buttons ➡️";
            Control_TransferTab_Button_Toggle_RightPanel.UseVisualStyleBackColor = true;
            Control_TransferTab_Button_Toggle_RightPanel.Click += Control_TransferTab_Button_Toggle_RightPanel_Click;
            // 
            // Control_TransferTab_SplitContainer_Main
            // 
            Control_TransferTab_SplitContainer_Main.Dock = DockStyle.Fill;
            Control_TransferTab_SplitContainer_Main.Location = new Point(3, 3);
            Control_TransferTab_SplitContainer_Main.Name = "Control_TransferTab_SplitContainer_Main";
            // 
            // Control_TransferTab_SplitContainer_Main.Panel1
            // 
            Control_TransferTab_SplitContainer_Main.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // Control_TransferTab_SplitContainer_Main.Panel2
            // 
            Control_TransferTab_SplitContainer_Main.Panel2.Controls.Add(panel1);
            Control_TransferTab_SplitContainer_Main.Size = new Size(955, 442);
            Control_TransferTab_SplitContainer_Main.SplitterDistance = 386;
            Control_TransferTab_SplitContainer_Main.TabIndex = 0;
            Control_TransferTab_SplitContainer_Main.SplitterMoved += splitContainer1_SplitterMoved;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(Control_TransferTab_ComboBox_Operation, 1, 1);
            tableLayoutPanel1.Controls.Add(Control_TransferTab_ComboBox_Part, 1, 0);
            tableLayoutPanel1.Controls.Add(Control_TransferTab_NumericUpDown_Quantity, 1, 3);
            tableLayoutPanel1.Controls.Add(Control_TransferTab_ComboBox_ToLocation, 1, 2);
            tableLayoutPanel1.Controls.Add(Control_TransferTab_Label_Part, 0, 0);
            tableLayoutPanel1.Controls.Add(Control_TransferTab_Label_Operation, 0, 1);
            tableLayoutPanel1.Controls.Add(Control_TransferTab_Label_ToLocation, 0, 2);
            tableLayoutPanel1.Controls.Add(Control_TransferTab_Label_Quantity, 0, 3);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 6);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(386, 442);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // Control_TransferTab_ComboBox_Operation
            // 
            Control_TransferTab_ComboBox_Operation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_ComboBox_Operation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_TransferTab_ComboBox_Operation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_TransferTab_ComboBox_Operation.FormattingEnabled = true;
            Control_TransferTab_ComboBox_Operation.Location = new Point(103, 32);
            Control_TransferTab_ComboBox_Operation.Name = "Control_TransferTab_ComboBox_Operation";
            Control_TransferTab_ComboBox_Operation.Size = new Size(280, 23);
            Control_TransferTab_ComboBox_Operation.TabIndex = 2;
            // 
            // Control_TransferTab_ComboBox_Part
            // 
            Control_TransferTab_ComboBox_Part.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_TransferTab_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_TransferTab_ComboBox_Part.FormattingEnabled = true;
            Control_TransferTab_ComboBox_Part.Location = new Point(103, 3);
            Control_TransferTab_ComboBox_Part.Name = "Control_TransferTab_ComboBox_Part";
            Control_TransferTab_ComboBox_Part.Size = new Size(280, 23);
            Control_TransferTab_ComboBox_Part.TabIndex = 1;
            // 
            // Control_TransferTab_NumericUpDown_Quantity
            // 
            Control_TransferTab_NumericUpDown_Quantity.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_NumericUpDown_Quantity.Enabled = false;
            Control_TransferTab_NumericUpDown_Quantity.Location = new Point(103, 90);
            Control_TransferTab_NumericUpDown_Quantity.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            Control_TransferTab_NumericUpDown_Quantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            Control_TransferTab_NumericUpDown_Quantity.Name = "Control_TransferTab_NumericUpDown_Quantity";
            Control_TransferTab_NumericUpDown_Quantity.Size = new Size(280, 23);
            Control_TransferTab_NumericUpDown_Quantity.TabIndex = 4;
            Control_TransferTab_NumericUpDown_Quantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // Control_TransferTab_ComboBox_ToLocation
            // 
            Control_TransferTab_ComboBox_ToLocation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_ComboBox_ToLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_TransferTab_ComboBox_ToLocation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_TransferTab_ComboBox_ToLocation.Enabled = false;
            Control_TransferTab_ComboBox_ToLocation.FormattingEnabled = true;
            Control_TransferTab_ComboBox_ToLocation.Location = new Point(103, 61);
            Control_TransferTab_ComboBox_ToLocation.Name = "Control_TransferTab_ComboBox_ToLocation";
            Control_TransferTab_ComboBox_ToLocation.Size = new Size(280, 23);
            Control_TransferTab_ComboBox_ToLocation.TabIndex = 3;
            // 
            // Control_TransferTab_Label_Part
            // 
            Control_TransferTab_Label_Part.AutoSize = true;
            Control_TransferTab_Label_Part.Dock = DockStyle.Fill;
            Control_TransferTab_Label_Part.Location = new Point(3, 0);
            Control_TransferTab_Label_Part.Name = "Control_TransferTab_Label_Part";
            Control_TransferTab_Label_Part.Size = new Size(78, 29);
            Control_TransferTab_Label_Part.TabIndex = 4;
            Control_TransferTab_Label_Part.Text = "Part Number:";
            Control_TransferTab_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_Label_Operation
            // 
            Control_TransferTab_Label_Operation.AutoSize = true;
            Control_TransferTab_Label_Operation.Dock = DockStyle.Fill;
            Control_TransferTab_Label_Operation.Location = new Point(3, 29);
            Control_TransferTab_Label_Operation.Name = "Control_TransferTab_Label_Operation";
            Control_TransferTab_Label_Operation.Size = new Size(78, 29);
            Control_TransferTab_Label_Operation.TabIndex = 5;
            Control_TransferTab_Label_Operation.Text = "Operation:";
            Control_TransferTab_Label_Operation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_Label_ToLocation
            // 
            Control_TransferTab_Label_ToLocation.AutoSize = true;
            Control_TransferTab_Label_ToLocation.Dock = DockStyle.Fill;
            Control_TransferTab_Label_ToLocation.Location = new Point(3, 58);
            Control_TransferTab_Label_ToLocation.Name = "Control_TransferTab_Label_ToLocation";
            Control_TransferTab_Label_ToLocation.Size = new Size(78, 29);
            Control_TransferTab_Label_ToLocation.TabIndex = 8;
            Control_TransferTab_Label_ToLocation.Text = "To Location:";
            Control_TransferTab_Label_ToLocation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_Label_Quantity
            // 
            Control_TransferTab_Label_Quantity.AutoSize = true;
            Control_TransferTab_Label_Quantity.Dock = DockStyle.Fill;
            Control_TransferTab_Label_Quantity.Location = new Point(3, 87);
            Control_TransferTab_Label_Quantity.Name = "Control_TransferTab_Label_Quantity";
            Control_TransferTab_Label_Quantity.Size = new Size(78, 29);
            Control_TransferTab_Label_Quantity.TabIndex = 10;
            Control_TransferTab_Label_Quantity.Text = "Quantity:";
            Control_TransferTab_Label_Quantity.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel2, 2);
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(Control_TransferTab_Button_Transfer, 1, 0);
            tableLayoutPanel2.Controls.Add(Control_TransferTab_Button_Search, 0, 0);
            tableLayoutPanel2.Controls.Add(Control_TransferTab_Button_Print, 0, 1);
            tableLayoutPanel2.Controls.Add(Control_TransferTab_Button_Reset, 1, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 373);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(380, 66);
            tableLayoutPanel2.TabIndex = 11;
            // 
            // Control_TransferTab_Button_Transfer
            // 
            Control_TransferTab_Button_Transfer.AutoSize = true;
            Control_TransferTab_Button_Transfer.Dock = DockStyle.Fill;
            Control_TransferTab_Button_Transfer.Enabled = false;
            Control_TransferTab_Button_Transfer.Location = new Point(193, 3);
            Control_TransferTab_Button_Transfer.Name = "Control_TransferTab_Button_Transfer";
            Control_TransferTab_Button_Transfer.Size = new Size(184, 27);
            Control_TransferTab_Button_Transfer.TabIndex = 6;
            Control_TransferTab_Button_Transfer.Text = "Save";
            Control_TransferTab_Button_Transfer.UseVisualStyleBackColor = true;
            // 
            // Control_TransferTab_Button_Search
            // 
            Control_TransferTab_Button_Search.AutoSize = true;
            Control_TransferTab_Button_Search.Dock = DockStyle.Fill;
            Control_TransferTab_Button_Search.Enabled = false;
            Control_TransferTab_Button_Search.Location = new Point(3, 3);
            Control_TransferTab_Button_Search.Name = "Control_TransferTab_Button_Search";
            Control_TransferTab_Button_Search.Size = new Size(184, 27);
            Control_TransferTab_Button_Search.TabIndex = 5;
            Control_TransferTab_Button_Search.Text = "Search";
            Control_TransferTab_Button_Search.UseVisualStyleBackColor = true;
            Control_TransferTab_Button_Search.Click += Control_TransferTab_Button_Search_Click;
            // 
            // Control_TransferTab_Button_Print
            // 
            Control_TransferTab_Button_Print.AutoSize = true;
            Control_TransferTab_Button_Print.Dock = DockStyle.Fill;
            Control_TransferTab_Button_Print.Enabled = false;
            Control_TransferTab_Button_Print.Location = new Point(3, 36);
            Control_TransferTab_Button_Print.Name = "Control_TransferTab_Button_Print";
            Control_TransferTab_Button_Print.Size = new Size(184, 27);
            Control_TransferTab_Button_Print.TabIndex = 1000;
            Control_TransferTab_Button_Print.TabStop = false;
            Control_TransferTab_Button_Print.Text = "Print";
            Control_TransferTab_Button_Print.UseVisualStyleBackColor = true;
            // 
            // Control_TransferTab_Button_Reset
            // 
            Control_TransferTab_Button_Reset.AutoSize = true;
            Control_TransferTab_Button_Reset.Dock = DockStyle.Fill;
            Control_TransferTab_Button_Reset.Location = new Point(193, 36);
            Control_TransferTab_Button_Reset.Name = "Control_TransferTab_Button_Reset";
            Control_TransferTab_Button_Reset.Size = new Size(184, 27);
            Control_TransferTab_Button_Reset.TabIndex = 7;
            Control_TransferTab_Button_Reset.TabStop = false;
            Control_TransferTab_Button_Reset.Text = "Reset";
            Control_TransferTab_Button_Reset.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.Controls.Add(Control_TransferTab_Panel_DataGridView);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(565, 442);
            panel1.TabIndex = 10;
            // 
            // Control_TransferTab_Panel_DataGridView
            // 
            Control_TransferTab_Panel_DataGridView.AutoSize = true;
            Control_TransferTab_Panel_DataGridView.Controls.Add(Control_TransferTab_Image_NothingFound);
            Control_TransferTab_Panel_DataGridView.Controls.Add(Control_TransferTab_DataGridView_Main);
            Control_TransferTab_Panel_DataGridView.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_DataGridView.Location = new Point(0, 0);
            Control_TransferTab_Panel_DataGridView.Name = "Control_TransferTab_Panel_DataGridView";
            Control_TransferTab_Panel_DataGridView.Size = new Size(565, 442);
            Control_TransferTab_Panel_DataGridView.TabIndex = 9;
            Control_TransferTab_Panel_DataGridView.TabStop = true;
            // 
            // Control_TransferTab_Image_NothingFound
            // 
            Control_TransferTab_Image_NothingFound.BackColor = Color.White;
            Control_TransferTab_Image_NothingFound.BorderStyle = BorderStyle.FixedSingle;
            Control_TransferTab_Image_NothingFound.Dock = DockStyle.Fill;
            Control_TransferTab_Image_NothingFound.ErrorImage = null;
            Control_TransferTab_Image_NothingFound.Image = Properties.Resources.NothingFound;
            Control_TransferTab_Image_NothingFound.InitialImage = null;
            Control_TransferTab_Image_NothingFound.Location = new Point(0, 0);
            Control_TransferTab_Image_NothingFound.Name = "Control_TransferTab_Image_NothingFound";
            Control_TransferTab_Image_NothingFound.Size = new Size(565, 442);
            Control_TransferTab_Image_NothingFound.SizeMode = PictureBoxSizeMode.CenterImage;
            Control_TransferTab_Image_NothingFound.TabIndex = 6;
            Control_TransferTab_Image_NothingFound.TabStop = false;
            Control_TransferTab_Image_NothingFound.Visible = false;
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
            Control_TransferTab_DataGridView_Main.Size = new Size(565, 442);
            Control_TransferTab_DataGridView_Main.StandardTab = true;
            Control_TransferTab_DataGridView_Main.TabIndex = 4;
            // 
            // Control_TransferTab
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            Controls.Add(Control_TransferTab_GroupBox_MainControl);
            Name = "Control_TransferTab";
            Size = new Size(967, 510);
            Control_TransferTab_GroupBox_MainControl.ResumeLayout(false);
            Control_TransferTab_Panel_Main.ResumeLayout(false);
            Control_TransferTab_Panel_Main.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            Control_TransferTab_SplitContainer_Main.Panel1.ResumeLayout(false);
            Control_TransferTab_SplitContainer_Main.Panel2.ResumeLayout(false);
            Control_TransferTab_SplitContainer_Main.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_SplitContainer_Main).EndInit();
            Control_TransferTab_SplitContainer_Main.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_NumericUpDown_Quantity).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            Control_TransferTab_Panel_DataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_Image_NothingFound).EndInit();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_DataGridView_Main).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private TableLayoutPanel Control_TransferTab_Panel_Main;
        private SplitContainer Control_TransferTab_SplitContainer_Main;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel Control_TransferTab_Panel_DataGridView;
        private Label Control_TransferTab_Label_Part;
        internal ComboBox Control_TransferTab_ComboBox_Part;
        private Label Control_TransferTab_Label_Operation;
        private ComboBox Control_TransferTab_ComboBox_Operation;
        private Label Control_TransferTab_Label_ToLocation;
        private Label Control_TransferTab_Label_Quantity;
        private Button Control_TransferTab_Button_Print;
        private DataGridView Control_TransferTab_DataGridView_Main;
        private Button Control_TransferTab_Button_Reset;
        private ComboBox Control_TransferTab_ComboBox_ToLocation;
        private Button Control_TransferTab_Button_Transfer;
        private Button Control_TransferTab_Button_Search;
        private NumericUpDown Control_TransferTab_NumericUpDown_Quantity;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel2;
        private Button Control_TransferTab_Button_Toggle_Split;
        private Button Control_TransferTab_Button_Toggle_RightPanel;
        private Panel panel1;
        private PictureBox Control_TransferTab_Image_NothingFound;
    }

        
        #endregion
    }
