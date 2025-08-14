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
            Control_TransferTab_TableLayout_Main = new TableLayoutPanel();
            Control_TransferTab_SplitContainer_Main = new SplitContainer();
            Control_TransferTab_TableLayout_Inputs = new TableLayoutPanel();
            Control_TransferTab_Panel_Row_SortBy = new Panel();
            Control_TransferTab_Label_SortBy = new Label();
            Control_TransferTab_ComboBox_SortBy = new ComboBox();
            Control_TransferTab_Panel_Row_PartID = new Panel();
            Control_TransferTab_Label_Part = new Label();
            Control_TransferTab_ComboBox_Part = new ComboBox();
            Control_TransferTab_Panel_Row_Operation = new Panel();
            Control_TransferTab_Label_Operation = new Label();
            Control_TransferTab_ComboBox_Operation = new ComboBox();
            Control_TransferTab_Panel_Row_FromLocation = new Panel();
            Control_TransferTab_Label_FromLocation = new Label();
            Control_TransferTab_ComboBox_FromLocation = new ComboBox();
            Control_TransferTab_Panel_Row_ToLocation = new Panel();
            Control_TransferTab_Label_ToLocation = new Label();
            Control_TransferTab_ComboBox_ToLocation = new ComboBox();
            Control_TransferTab_Panel_Row_Quantity = new Panel();
            Control_TransferTab_Label_Quantity = new Label();
            Control_TransferTab_NumericUpDown_Quantity = new NumericUpDown();
            Control_TransferTab_Panel_Row_SmartSearch = new Panel();
            Control_TransferTab_Label_SmartSearch = new Label();
            Control_TransferTab_TextBox_SmartSearch = new TextBox();
            Control_TransferTab_Button_SmartSearch = new Button();
            Control_TransferTab_Label_SmartSearchHelp = new Label();
            Control_TransferTab_TableLayout_SelectionReport = new TableLayoutPanel();
            Control_TransferTab_Label_Report_PartID = new Label();
            Control_TransferTab_TextBox_Report_PartID = new TextBox();
            Control_TransferTab_Label_Report_Operation = new Label();
            Control_TransferTab_TextBox_Report_Operation = new TextBox();
            Control_TransferTab_Label_Report_FromLocation = new Label();
            Control_TransferTab_TextBox_Report_FromLocation = new TextBox();
            Control_TransferTab_Label_Report_ToLocation = new Label();
            Control_TransferTab_TextBox_Report_ToLocation = new TextBox();
            Control_TransferTab_Label_Report_Quantity = new Label();
            Control_TransferTab_TextBox_Report_Quantity = new TextBox();
            Control_TransferTab_Label_Report_User = new Label();
            Control_TransferTab_TextBox_Report_User = new TextBox();
            Control_TransferTab_Label_Report_BatchNumber = new Label();
            Control_TransferTab_TextBox_Report_BatchNumber = new TextBox();
            Control_TransferTab_Label_Report_TransferType = new Label();
            Control_TransferTab_TextBox_Report_TransferType = new TextBox();
            Control_TransferTab_TableLayout_Right = new TableLayoutPanel();
            Control_TransferTab_Panel_DataGridView = new Panel();
            Control_TransferTab_Image_NothingFound = new PictureBox();
            Control_TransferTab_DataGridView_Main = new DataGridView();
            Control_TransferTab_Panel_PageButtons = new Panel();
            Control_TransferTab_Button_SelectionHistory = new Button();
            Control_TransferTab_Button_Next = new Button();
            Control_TransferTab_Button_Previous = new Button();
            Control_TransferTab_TableLayoutPanel_Bottom = new TableLayoutPanel();
            Control_TransferTab_Panel_Bottom_Left = new Panel();
            Control_TransferTab_Button_Search = new Button();
            Control_TransferTab_Button_SidePanel = new Button();
            Control_TransferTab_Panel_Bottom_Right = new Panel();
            Control_TransferTab_Button_Transfer = new Button();
            Control_TransferTab_Button_Print = new Button();
            Control_TransferTab_Button_Reset = new Button();
            Control_TransferTab_GroupBox_MainControl.SuspendLayout();
            Control_TransferTab_TableLayout_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_SplitContainer_Main).BeginInit();
            Control_TransferTab_SplitContainer_Main.Panel1.SuspendLayout();
            Control_TransferTab_SplitContainer_Main.Panel2.SuspendLayout();
            Control_TransferTab_SplitContainer_Main.SuspendLayout();
            Control_TransferTab_TableLayout_Inputs.SuspendLayout();
            Control_TransferTab_Panel_Row_SortBy.SuspendLayout();
            Control_TransferTab_Panel_Row_PartID.SuspendLayout();
            Control_TransferTab_Panel_Row_Operation.SuspendLayout();
            Control_TransferTab_Panel_Row_FromLocation.SuspendLayout();
            Control_TransferTab_Panel_Row_ToLocation.SuspendLayout();
            Control_TransferTab_Panel_Row_Quantity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_NumericUpDown_Quantity).BeginInit();
            Control_TransferTab_Panel_Row_SmartSearch.SuspendLayout();
            Control_TransferTab_TableLayout_SelectionReport.SuspendLayout();
            Control_TransferTab_TableLayout_Right.SuspendLayout();
            Control_TransferTab_Panel_DataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_Image_NothingFound).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_DataGridView_Main).BeginInit();
            Control_TransferTab_Panel_PageButtons.SuspendLayout();
            Control_TransferTab_TableLayoutPanel_Bottom.SuspendLayout();
            Control_TransferTab_Panel_Bottom_Left.SuspendLayout();
            Control_TransferTab_Panel_Bottom_Right.SuspendLayout();
            SuspendLayout();
            // 
            // Control_TransferTab_GroupBox_MainControl
            // 
            Control_TransferTab_GroupBox_MainControl.AutoSize = true;
            Control_TransferTab_GroupBox_MainControl.Controls.Add(Control_TransferTab_TableLayout_Main);
            Control_TransferTab_GroupBox_MainControl.Dock = DockStyle.Fill;
            Control_TransferTab_GroupBox_MainControl.FlatStyle = FlatStyle.Flat;
            Control_TransferTab_GroupBox_MainControl.Location = new Point(0, 0);
            Control_TransferTab_GroupBox_MainControl.Name = "Control_TransferTab_GroupBox_MainControl";
            Control_TransferTab_GroupBox_MainControl.Size = new Size(967, 510);
            Control_TransferTab_GroupBox_MainControl.TabIndex = 17;
            Control_TransferTab_GroupBox_MainControl.TabStop = false;
            Control_TransferTab_GroupBox_MainControl.Text = "Inventory Transfer Management";
            // 
            // Control_TransferTab_TableLayout_Main
            // 
            Control_TransferTab_TableLayout_Main.ColumnCount = 1;
            Control_TransferTab_TableLayout_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_TransferTab_TableLayout_Main.Controls.Add(Control_TransferTab_SplitContainer_Main, 0, 0);
            Control_TransferTab_TableLayout_Main.Controls.Add(Control_TransferTab_TableLayoutPanel_Bottom, 0, 1);
            Control_TransferTab_TableLayout_Main.Dock = DockStyle.Fill;
            Control_TransferTab_TableLayout_Main.Location = new Point(3, 19);
            Control_TransferTab_TableLayout_Main.Name = "Control_TransferTab_TableLayout_Main";
            Control_TransferTab_TableLayout_Main.RowCount = 2;
            Control_TransferTab_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_TransferTab_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Control_TransferTab_TableLayout_Main.Size = new Size(961, 488);
            Control_TransferTab_TableLayout_Main.TabIndex = 0;
            // 
            // Control_TransferTab_SplitContainer_Main
            // 
            Control_TransferTab_SplitContainer_Main.Dock = DockStyle.Fill;
            Control_TransferTab_SplitContainer_Main.Location = new Point(3, 3);
            Control_TransferTab_SplitContainer_Main.Name = "Control_TransferTab_SplitContainer_Main";
            // 
            // Control_TransferTab_SplitContainer_Main.Panel1
            // 
            Control_TransferTab_SplitContainer_Main.Panel1.Controls.Add(Control_TransferTab_TableLayout_Inputs);
            Control_TransferTab_SplitContainer_Main.Panel1MinSize = 420;
            // 
            // Control_TransferTab_SplitContainer_Main.Panel2
            // 
            Control_TransferTab_SplitContainer_Main.Panel2.Controls.Add(Control_TransferTab_TableLayout_Right);
            Control_TransferTab_SplitContainer_Main.Panel2MinSize = 380;
            Control_TransferTab_SplitContainer_Main.Size = new Size(955, 442);
            Control_TransferTab_SplitContainer_Main.SplitterDistance = 420;
            Control_TransferTab_SplitContainer_Main.TabIndex = 0;
            // 
            // Control_TransferTab_TableLayout_Inputs
            // 
            Control_TransferTab_TableLayout_Inputs.ColumnCount = 1;
            Control_TransferTab_TableLayout_Inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_TransferTab_TableLayout_Inputs.Controls.Add(Control_TransferTab_Panel_Row_SortBy, 0, 0);
            Control_TransferTab_TableLayout_Inputs.Controls.Add(Control_TransferTab_Panel_Row_PartID, 0, 1);
            Control_TransferTab_TableLayout_Inputs.Controls.Add(Control_TransferTab_Panel_Row_Operation, 0, 2);
            Control_TransferTab_TableLayout_Inputs.Controls.Add(Control_TransferTab_Panel_Row_FromLocation, 0, 3);
            Control_TransferTab_TableLayout_Inputs.Controls.Add(Control_TransferTab_Panel_Row_ToLocation, 0, 4);
            Control_TransferTab_TableLayout_Inputs.Controls.Add(Control_TransferTab_Panel_Row_Quantity, 0, 5);
            Control_TransferTab_TableLayout_Inputs.Controls.Add(Control_TransferTab_Panel_Row_SmartSearch, 0, 6);
            Control_TransferTab_TableLayout_Inputs.Controls.Add(Control_TransferTab_TableLayout_SelectionReport, 0, 7);
            Control_TransferTab_TableLayout_Inputs.Dock = DockStyle.Fill;
            Control_TransferTab_TableLayout_Inputs.Location = new Point(0, 0);
            Control_TransferTab_TableLayout_Inputs.Name = "Control_TransferTab_TableLayout_Inputs";
            Control_TransferTab_TableLayout_Inputs.RowCount = 8;
            Control_TransferTab_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Control_TransferTab_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Control_TransferTab_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Control_TransferTab_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Control_TransferTab_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Control_TransferTab_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Control_TransferTab_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            Control_TransferTab_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_TransferTab_TableLayout_Inputs.Size = new Size(420, 442);
            Control_TransferTab_TableLayout_Inputs.TabIndex = 0;
            // 
            // Control_TransferTab_Panel_Row_SortBy
            // 
            Control_TransferTab_Panel_Row_SortBy.Controls.Add(Control_TransferTab_Label_SortBy);
            Control_TransferTab_Panel_Row_SortBy.Controls.Add(Control_TransferTab_ComboBox_SortBy);
            Control_TransferTab_Panel_Row_SortBy.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Row_SortBy.Location = new Point(3, 3);
            Control_TransferTab_Panel_Row_SortBy.Name = "Control_TransferTab_Panel_Row_SortBy";
            Control_TransferTab_Panel_Row_SortBy.Size = new Size(414, 34);
            Control_TransferTab_Panel_Row_SortBy.TabIndex = 0;
            // 
            // Control_TransferTab_Label_SortBy
            // 
            Control_TransferTab_Label_SortBy.Location = new Point(5, 6);
            Control_TransferTab_Label_SortBy.Name = "Control_TransferTab_Label_SortBy";
            Control_TransferTab_Label_SortBy.Size = new Size(90, 23);
            Control_TransferTab_Label_SortBy.TabIndex = 0;
            Control_TransferTab_Label_SortBy.Text = "Sort By:";
            Control_TransferTab_Label_SortBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_ComboBox_SortBy
            // 
            Control_TransferTab_ComboBox_SortBy.DropDownStyle = ComboBoxStyle.DropDownList;
            Control_TransferTab_ComboBox_SortBy.FormattingEnabled = true;
            Control_TransferTab_ComboBox_SortBy.Items.AddRange(new object[] { "Part ID", "Operation", "Location", "Quantity", "Last Updated" });
            Control_TransferTab_ComboBox_SortBy.Location = new Point(101, 6);
            Control_TransferTab_ComboBox_SortBy.Name = "Control_TransferTab_ComboBox_SortBy";
            Control_TransferTab_ComboBox_SortBy.Size = new Size(306, 23);
            Control_TransferTab_ComboBox_SortBy.TabIndex = 1;
            // 
            // Control_TransferTab_Panel_Row_PartID
            // 
            Control_TransferTab_Panel_Row_PartID.Controls.Add(Control_TransferTab_Label_Part);
            Control_TransferTab_Panel_Row_PartID.Controls.Add(Control_TransferTab_ComboBox_Part);
            Control_TransferTab_Panel_Row_PartID.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Row_PartID.Location = new Point(3, 43);
            Control_TransferTab_Panel_Row_PartID.Name = "Control_TransferTab_Panel_Row_PartID";
            Control_TransferTab_Panel_Row_PartID.Size = new Size(414, 34);
            Control_TransferTab_Panel_Row_PartID.TabIndex = 1;
            // 
            // Control_TransferTab_Label_Part
            // 
            Control_TransferTab_Label_Part.Location = new Point(5, 6);
            Control_TransferTab_Label_Part.Name = "Control_TransferTab_Label_Part";
            Control_TransferTab_Label_Part.Size = new Size(90, 23);
            Control_TransferTab_Label_Part.TabIndex = 4;
            Control_TransferTab_Label_Part.Text = "Part Number:";
            Control_TransferTab_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_ComboBox_Part
            // 
            Control_TransferTab_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_TransferTab_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_TransferTab_ComboBox_Part.FormattingEnabled = true;
            Control_TransferTab_ComboBox_Part.Location = new Point(101, 6);
            Control_TransferTab_ComboBox_Part.Name = "Control_TransferTab_ComboBox_Part";
            Control_TransferTab_ComboBox_Part.Size = new Size(306, 23);
            Control_TransferTab_ComboBox_Part.TabIndex = 1;
            // 
            // Control_TransferTab_Panel_Row_Operation
            // 
            Control_TransferTab_Panel_Row_Operation.Controls.Add(Control_TransferTab_Label_Operation);
            Control_TransferTab_Panel_Row_Operation.Controls.Add(Control_TransferTab_ComboBox_Operation);
            Control_TransferTab_Panel_Row_Operation.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Row_Operation.Location = new Point(3, 83);
            Control_TransferTab_Panel_Row_Operation.Name = "Control_TransferTab_Panel_Row_Operation";
            Control_TransferTab_Panel_Row_Operation.Size = new Size(414, 34);
            Control_TransferTab_Panel_Row_Operation.TabIndex = 2;
            // 
            // Control_TransferTab_Label_Operation
            // 
            Control_TransferTab_Label_Operation.Location = new Point(5, 6);
            Control_TransferTab_Label_Operation.Name = "Control_TransferTab_Label_Operation";
            Control_TransferTab_Label_Operation.Size = new Size(90, 23);
            Control_TransferTab_Label_Operation.TabIndex = 5;
            Control_TransferTab_Label_Operation.Text = "Operation:";
            Control_TransferTab_Label_Operation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_ComboBox_Operation
            // 
            Control_TransferTab_ComboBox_Operation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_TransferTab_ComboBox_Operation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_TransferTab_ComboBox_Operation.FormattingEnabled = true;
            Control_TransferTab_ComboBox_Operation.Location = new Point(101, 6);
            Control_TransferTab_ComboBox_Operation.Name = "Control_TransferTab_ComboBox_Operation";
            Control_TransferTab_ComboBox_Operation.Size = new Size(306, 23);
            Control_TransferTab_ComboBox_Operation.TabIndex = 2;
            // 
            // Control_TransferTab_Panel_Row_FromLocation
            // 
            Control_TransferTab_Panel_Row_FromLocation.Controls.Add(Control_TransferTab_Label_FromLocation);
            Control_TransferTab_Panel_Row_FromLocation.Controls.Add(Control_TransferTab_ComboBox_FromLocation);
            Control_TransferTab_Panel_Row_FromLocation.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Row_FromLocation.Location = new Point(3, 123);
            Control_TransferTab_Panel_Row_FromLocation.Name = "Control_TransferTab_Panel_Row_FromLocation";
            Control_TransferTab_Panel_Row_FromLocation.Size = new Size(414, 34);
            Control_TransferTab_Panel_Row_FromLocation.TabIndex = 3;
            // 
            // Control_TransferTab_Label_FromLocation
            // 
            Control_TransferTab_Label_FromLocation.Location = new Point(5, 6);
            Control_TransferTab_Label_FromLocation.Name = "Control_TransferTab_Label_FromLocation";
            Control_TransferTab_Label_FromLocation.Size = new Size(90, 23);
            Control_TransferTab_Label_FromLocation.TabIndex = 6;
            Control_TransferTab_Label_FromLocation.Text = "From Location:";
            Control_TransferTab_Label_FromLocation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_ComboBox_FromLocation
            // 
            Control_TransferTab_ComboBox_FromLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_TransferTab_ComboBox_FromLocation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_TransferTab_ComboBox_FromLocation.FormattingEnabled = true;
            Control_TransferTab_ComboBox_FromLocation.Location = new Point(101, 6);
            Control_TransferTab_ComboBox_FromLocation.Name = "Control_TransferTab_ComboBox_FromLocation";
            Control_TransferTab_ComboBox_FromLocation.Size = new Size(306, 23);
            Control_TransferTab_ComboBox_FromLocation.TabIndex = 3;
            // 
            // Control_TransferTab_Panel_Row_ToLocation
            // 
            Control_TransferTab_Panel_Row_ToLocation.Controls.Add(Control_TransferTab_Label_ToLocation);
            Control_TransferTab_Panel_Row_ToLocation.Controls.Add(Control_TransferTab_ComboBox_ToLocation);
            Control_TransferTab_Panel_Row_ToLocation.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Row_ToLocation.Location = new Point(3, 163);
            Control_TransferTab_Panel_Row_ToLocation.Name = "Control_TransferTab_Panel_Row_ToLocation";
            Control_TransferTab_Panel_Row_ToLocation.Size = new Size(414, 34);
            Control_TransferTab_Panel_Row_ToLocation.TabIndex = 4;
            // 
            // Control_TransferTab_Label_ToLocation
            // 
            Control_TransferTab_Label_ToLocation.Location = new Point(5, 6);
            Control_TransferTab_Label_ToLocation.Name = "Control_TransferTab_Label_ToLocation";
            Control_TransferTab_Label_ToLocation.Size = new Size(90, 23);
            Control_TransferTab_Label_ToLocation.TabIndex = 8;
            Control_TransferTab_Label_ToLocation.Text = "To Location:";
            Control_TransferTab_Label_ToLocation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_ComboBox_ToLocation
            // 
            Control_TransferTab_ComboBox_ToLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_TransferTab_ComboBox_ToLocation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_TransferTab_ComboBox_ToLocation.Enabled = false;
            Control_TransferTab_ComboBox_ToLocation.FormattingEnabled = true;
            Control_TransferTab_ComboBox_ToLocation.Location = new Point(101, 6);
            Control_TransferTab_ComboBox_ToLocation.Name = "Control_TransferTab_ComboBox_ToLocation";
            Control_TransferTab_ComboBox_ToLocation.Size = new Size(306, 23);
            Control_TransferTab_ComboBox_ToLocation.TabIndex = 3;
            // 
            // Control_TransferTab_Panel_Row_Quantity
            // 
            Control_TransferTab_Panel_Row_Quantity.Controls.Add(Control_TransferTab_Label_Quantity);
            Control_TransferTab_Panel_Row_Quantity.Controls.Add(Control_TransferTab_NumericUpDown_Quantity);
            Control_TransferTab_Panel_Row_Quantity.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Row_Quantity.Location = new Point(3, 203);
            Control_TransferTab_Panel_Row_Quantity.Name = "Control_TransferTab_Panel_Row_Quantity";
            Control_TransferTab_Panel_Row_Quantity.Size = new Size(414, 34);
            Control_TransferTab_Panel_Row_Quantity.TabIndex = 5;
            // 
            // Control_TransferTab_Label_Quantity
            // 
            Control_TransferTab_Label_Quantity.Location = new Point(5, 6);
            Control_TransferTab_Label_Quantity.Name = "Control_TransferTab_Label_Quantity";
            Control_TransferTab_Label_Quantity.Size = new Size(90, 23);
            Control_TransferTab_Label_Quantity.TabIndex = 10;
            Control_TransferTab_Label_Quantity.Text = "Quantity:";
            Control_TransferTab_Label_Quantity.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_NumericUpDown_Quantity
            // 
            Control_TransferTab_NumericUpDown_Quantity.Enabled = false;
            Control_TransferTab_NumericUpDown_Quantity.Location = new Point(101, 6);
            Control_TransferTab_NumericUpDown_Quantity.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            Control_TransferTab_NumericUpDown_Quantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            Control_TransferTab_NumericUpDown_Quantity.Name = "Control_TransferTab_NumericUpDown_Quantity";
            Control_TransferTab_NumericUpDown_Quantity.Size = new Size(306, 23);
            Control_TransferTab_NumericUpDown_Quantity.TabIndex = 4;
            Control_TransferTab_NumericUpDown_Quantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // Control_TransferTab_Panel_Row_SmartSearch
            // 
            Control_TransferTab_Panel_Row_SmartSearch.Controls.Add(Control_TransferTab_Label_SmartSearchHelp);
            Control_TransferTab_Panel_Row_SmartSearch.Controls.Add(Control_TransferTab_Button_SmartSearch);
            Control_TransferTab_Panel_Row_SmartSearch.Controls.Add(Control_TransferTab_TextBox_SmartSearch);
            Control_TransferTab_Panel_Row_SmartSearch.Controls.Add(Control_TransferTab_Label_SmartSearch);
            Control_TransferTab_Panel_Row_SmartSearch.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Row_SmartSearch.Location = new Point(3, 243);
            Control_TransferTab_Panel_Row_SmartSearch.Name = "Control_TransferTab_Panel_Row_SmartSearch";
            Control_TransferTab_Panel_Row_SmartSearch.Size = new Size(414, 54);
            Control_TransferTab_Panel_Row_SmartSearch.TabIndex = 6;
            // 
            // Control_TransferTab_Label_SmartSearch
            // 
            Control_TransferTab_Label_SmartSearch.Location = new Point(5, 6);
            Control_TransferTab_Label_SmartSearch.Name = "Control_TransferTab_Label_SmartSearch";
            Control_TransferTab_Label_SmartSearch.Size = new Size(90, 23);
            Control_TransferTab_Label_SmartSearch.TabIndex = 0;
            Control_TransferTab_Label_SmartSearch.Text = "Smart Search:";
            Control_TransferTab_Label_SmartSearch.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_TransferTab_TextBox_SmartSearch
            // 
            Control_TransferTab_TextBox_SmartSearch.Location = new Point(101, 6);
            Control_TransferTab_TextBox_SmartSearch.Name = "Control_TransferTab_TextBox_SmartSearch";
            Control_TransferTab_TextBox_SmartSearch.PlaceholderText = "Search inventory items...";
            Control_TransferTab_TextBox_SmartSearch.Size = new Size(230, 23);
            Control_TransferTab_TextBox_SmartSearch.TabIndex = 1;
            // 
            // Control_TransferTab_Button_SmartSearch
            // 
            Control_TransferTab_Button_SmartSearch.Location = new Point(337, 6);
            Control_TransferTab_Button_SmartSearch.Name = "Control_TransferTab_Button_SmartSearch";
            Control_TransferTab_Button_SmartSearch.Size = new Size(70, 23);
            Control_TransferTab_Button_SmartSearch.TabIndex = 2;
            Control_TransferTab_Button_SmartSearch.Text = "Search";
            Control_TransferTab_Button_SmartSearch.UseVisualStyleBackColor = true;
            // 
            // Control_TransferTab_Label_SmartSearchHelp
            // 
            Control_TransferTab_Label_SmartSearchHelp.Font = new Font("Segoe UI", 7.5F, FontStyle.Italic);
            Control_TransferTab_Label_SmartSearchHelp.ForeColor = SystemColors.GrayText;
            Control_TransferTab_Label_SmartSearchHelp.Location = new Point(101, 32);
            Control_TransferTab_Label_SmartSearchHelp.Name = "Control_TransferTab_Label_SmartSearchHelp";
            Control_TransferTab_Label_SmartSearchHelp.Size = new Size(306, 18);
            Control_TransferTab_Label_SmartSearchHelp.TabIndex = 3;
            Control_TransferTab_Label_SmartSearchHelp.Text = "Examples: \"partid:PART123\", \"qty:>50\", \"location:A1\"";
            Control_TransferTab_Label_SmartSearchHelp.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Control_TransferTab_TableLayout_SelectionReport
            // 
            Control_TransferTab_TableLayout_SelectionReport.ColumnCount = 2;
            Control_TransferTab_TableLayout_SelectionReport.ColumnStyles.Add(new ColumnStyle());
            Control_TransferTab_TableLayout_SelectionReport.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_Label_Report_TransferType, 0, 0);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_TextBox_Report_TransferType, 1, 0);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_Label_Report_BatchNumber, 0, 1);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_TextBox_Report_BatchNumber, 1, 1);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_Label_Report_PartID, 0, 2);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_TextBox_Report_PartID, 1, 2);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_Label_Report_Operation, 0, 3);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_TextBox_Report_Operation, 1, 3);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_Label_Report_FromLocation, 0, 4);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_TextBox_Report_FromLocation, 1, 4);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_Label_Report_ToLocation, 0, 5);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_TextBox_Report_ToLocation, 1, 5);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_Label_Report_Quantity, 0, 6);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_TextBox_Report_Quantity, 1, 6);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_Label_Report_User, 0, 7);
            Control_TransferTab_TableLayout_SelectionReport.Controls.Add(Control_TransferTab_TextBox_Report_User, 1, 7);
            Control_TransferTab_TableLayout_SelectionReport.Dock = DockStyle.Fill;
            Control_TransferTab_TableLayout_SelectionReport.Location = new Point(3, 303);
            Control_TransferTab_TableLayout_SelectionReport.Name = "Control_TransferTab_TableLayout_SelectionReport";
            Control_TransferTab_TableLayout_SelectionReport.RowCount = 8;
            Control_TransferTab_TableLayout_SelectionReport.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            Control_TransferTab_TableLayout_SelectionReport.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            Control_TransferTab_TableLayout_SelectionReport.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            Control_TransferTab_TableLayout_SelectionReport.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            Control_TransferTab_TableLayout_SelectionReport.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            Control_TransferTab_TableLayout_SelectionReport.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            Control_TransferTab_TableLayout_SelectionReport.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            Control_TransferTab_TableLayout_SelectionReport.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_TransferTab_TableLayout_SelectionReport.Size = new Size(414, 136);
            Control_TransferTab_TableLayout_SelectionReport.TabIndex = 7;
            // 
            // Selection Report Labels and TextBoxes
            // 
            // Control_TransferTab_Label_Report_TransferType
            // 
            Control_TransferTab_Label_Report_TransferType.Anchor = AnchorStyles.Left;
            Control_TransferTab_Label_Report_TransferType.AutoSize = true;
            Control_TransferTab_Label_Report_TransferType.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            Control_TransferTab_Label_Report_TransferType.Location = new Point(3, 6);
            Control_TransferTab_Label_Report_TransferType.Name = "Control_TransferTab_Label_Report_TransferType";
            Control_TransferTab_Label_Report_TransferType.Size = new Size(82, 13);
            Control_TransferTab_Label_Report_TransferType.TabIndex = 0;
            Control_TransferTab_Label_Report_TransferType.Text = "Transfer Type:";
            // 
            // Control_TransferTab_TextBox_Report_TransferType
            // 
            Control_TransferTab_TextBox_Report_TransferType.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            Control_TransferTab_TextBox_Report_TransferType.BackColor = SystemColors.Control;
            Control_TransferTab_TextBox_Report_TransferType.Location = new Point(101, 3);
            Control_TransferTab_TextBox_Report_TransferType.Name = "Control_TransferTab_TextBox_Report_TransferType";
            Control_TransferTab_TextBox_Report_TransferType.ReadOnly = true;
            Control_TransferTab_TextBox_Report_TransferType.Size = new Size(310, 23);
            Control_TransferTab_TextBox_Report_TransferType.TabIndex = 1;
            Control_TransferTab_TextBox_Report_TransferType.Text = "TRANSFER";
            // 
            // Control_TransferTab_Label_Report_BatchNumber
            // 
            Control_TransferTab_Label_Report_BatchNumber.Anchor = AnchorStyles.Left;
            Control_TransferTab_Label_Report_BatchNumber.AutoSize = true;
            Control_TransferTab_Label_Report_BatchNumber.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            Control_TransferTab_Label_Report_BatchNumber.Location = new Point(3, 31);
            Control_TransferTab_Label_Report_BatchNumber.Name = "Control_TransferTab_Label_Report_BatchNumber";
            Control_TransferTab_Label_Report_BatchNumber.Size = new Size(92, 13);
            Control_TransferTab_Label_Report_BatchNumber.TabIndex = 2;
            Control_TransferTab_Label_Report_BatchNumber.Text = "Batch Number:";
            // 
            // Control_TransferTab_TextBox_Report_BatchNumber
            // 
            Control_TransferTab_TextBox_Report_BatchNumber.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            Control_TransferTab_TextBox_Report_BatchNumber.BackColor = SystemColors.Control;
            Control_TransferTab_TextBox_Report_BatchNumber.Location = new Point(101, 28);
            Control_TransferTab_TextBox_Report_BatchNumber.Name = "Control_TransferTab_TextBox_Report_BatchNumber";
            Control_TransferTab_TextBox_Report_BatchNumber.ReadOnly = true;
            Control_TransferTab_TextBox_Report_BatchNumber.Size = new Size(310, 23);
            Control_TransferTab_TextBox_Report_BatchNumber.TabIndex = 3;
            // 
            // Control_TransferTab_Label_Report_PartID
            // 
            Control_TransferTab_Label_Report_PartID.Anchor = AnchorStyles.Left;
            Control_TransferTab_Label_Report_PartID.AutoSize = true;
            Control_TransferTab_Label_Report_PartID.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            Control_TransferTab_Label_Report_PartID.Location = new Point(3, 56);
            Control_TransferTab_Label_Report_PartID.Name = "Control_TransferTab_Label_Report_PartID";
            Control_TransferTab_Label_Report_PartID.Size = new Size(50, 13);
            Control_TransferTab_Label_Report_PartID.TabIndex = 4;
            Control_TransferTab_Label_Report_PartID.Text = "Part ID:";
            // 
            // Control_TransferTab_TextBox_Report_PartID
            // 
            Control_TransferTab_TextBox_Report_PartID.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            Control_TransferTab_TextBox_Report_PartID.BackColor = SystemColors.Control;
            Control_TransferTab_TextBox_Report_PartID.Location = new Point(101, 53);
            Control_TransferTab_TextBox_Report_PartID.Name = "Control_TransferTab_TextBox_Report_PartID";
            Control_TransferTab_TextBox_Report_PartID.ReadOnly = true;
            Control_TransferTab_TextBox_Report_PartID.Size = new Size(310, 23);
            Control_TransferTab_TextBox_Report_PartID.TabIndex = 5;
            // 
            // Control_TransferTab_Label_Report_Operation
            // 
            Control_TransferTab_Label_Report_Operation.Anchor = AnchorStyles.Left;
            Control_TransferTab_Label_Report_Operation.AutoSize = true;
            Control_TransferTab_Label_Report_Operation.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            Control_TransferTab_Label_Report_Operation.Location = new Point(3, 81);
            Control_TransferTab_Label_Report_Operation.Name = "Control_TransferTab_Label_Report_Operation";
            Control_TransferTab_Label_Report_Operation.Size = new Size(66, 13);
            Control_TransferTab_Label_Report_Operation.TabIndex = 6;
            Control_TransferTab_Label_Report_Operation.Text = "Operation:";
            // 
            // Control_TransferTab_TextBox_Report_Operation
            // 
            Control_TransferTab_TextBox_Report_Operation.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            Control_TransferTab_TextBox_Report_Operation.BackColor = SystemColors.Control;
            Control_TransferTab_TextBox_Report_Operation.Location = new Point(101, 78);
            Control_TransferTab_TextBox_Report_Operation.Name = "Control_TransferTab_TextBox_Report_Operation";
            Control_TransferTab_TextBox_Report_Operation.ReadOnly = true;
            Control_TransferTab_TextBox_Report_Operation.Size = new Size(310, 23);
            Control_TransferTab_TextBox_Report_Operation.TabIndex = 7;
            // 
            // Control_TransferTab_Label_Report_FromLocation
            // 
            Control_TransferTab_Label_Report_FromLocation.Anchor = AnchorStyles.Left;
            Control_TransferTab_Label_Report_FromLocation.AutoSize = true;
            Control_TransferTab_Label_Report_FromLocation.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            Control_TransferTab_Label_Report_FromLocation.Location = new Point(3, 106);
            Control_TransferTab_Label_Report_FromLocation.Name = "Control_TransferTab_Label_Report_FromLocation";
            Control_TransferTab_Label_Report_FromLocation.Size = new Size(86, 13);
            Control_TransferTab_Label_Report_FromLocation.TabIndex = 8;
            Control_TransferTab_Label_Report_FromLocation.Text = "From Location:";
            // 
            // Control_TransferTab_TextBox_Report_FromLocation
            // 
            Control_TransferTab_TextBox_Report_FromLocation.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            Control_TransferTab_TextBox_Report_FromLocation.BackColor = SystemColors.Control;
            Control_TransferTab_TextBox_Report_FromLocation.Location = new Point(101, 103);
            Control_TransferTab_TextBox_Report_FromLocation.Name = "Control_TransferTab_TextBox_Report_FromLocation";
            Control_TransferTab_TextBox_Report_FromLocation.ReadOnly = true;
            Control_TransferTab_TextBox_Report_FromLocation.Size = new Size(310, 23);
            Control_TransferTab_TextBox_Report_FromLocation.TabIndex = 9;
            // 
            // Control_TransferTab_Label_Report_ToLocation
            // 
            Control_TransferTab_Label_Report_ToLocation.Anchor = AnchorStyles.Left;
            Control_TransferTab_Label_Report_ToLocation.AutoSize = true;
            Control_TransferTab_Label_Report_ToLocation.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            Control_TransferTab_Label_Report_ToLocation.Location = new Point(3, 131);
            Control_TransferTab_Label_Report_ToLocation.Name = "Control_TransferTab_Label_Report_ToLocation";
            Control_TransferTab_Label_Report_ToLocation.Size = new Size(74, 13);
            Control_TransferTab_Label_Report_ToLocation.TabIndex = 10;
            Control_TransferTab_Label_Report_ToLocation.Text = "To Location:";
            // 
            // Control_TransferTab_TextBox_Report_ToLocation
            // 
            Control_TransferTab_TextBox_Report_ToLocation.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            Control_TransferTab_TextBox_Report_ToLocation.BackColor = SystemColors.Control;
            Control_TransferTab_TextBox_Report_ToLocation.Location = new Point(101, 128);
            Control_TransferTab_TextBox_Report_ToLocation.Name = "Control_TransferTab_TextBox_Report_ToLocation";
            Control_TransferTab_TextBox_Report_ToLocation.ReadOnly = true;
            Control_TransferTab_TextBox_Report_ToLocation.Size = new Size(310, 23);
            Control_TransferTab_TextBox_Report_ToLocation.TabIndex = 11;
            // 
            // Control_TransferTab_Label_Report_Quantity
            // 
            Control_TransferTab_Label_Report_Quantity.Anchor = AnchorStyles.Left;
            Control_TransferTab_Label_Report_Quantity.AutoSize = true;
            Control_TransferTab_Label_Report_Quantity.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            Control_TransferTab_Label_Report_Quantity.Location = new Point(3, 156);
            Control_TransferTab_Label_Report_Quantity.Name = "Control_TransferTab_Label_Report_Quantity";
            Control_TransferTab_Label_Report_Quantity.Size = new Size(57, 13);
            Control_TransferTab_Label_Report_Quantity.TabIndex = 12;
            Control_TransferTab_Label_Report_Quantity.Text = "Quantity:";
            // 
            // Control_TransferTab_TextBox_Report_Quantity
            // 
            Control_TransferTab_TextBox_Report_Quantity.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            Control_TransferTab_TextBox_Report_Quantity.BackColor = SystemColors.Control;
            Control_TransferTab_TextBox_Report_Quantity.Location = new Point(101, 153);
            Control_TransferTab_TextBox_Report_Quantity.Name = "Control_TransferTab_TextBox_Report_Quantity";
            Control_TransferTab_TextBox_Report_Quantity.ReadOnly = true;
            Control_TransferTab_TextBox_Report_Quantity.Size = new Size(310, 23);
            Control_TransferTab_TextBox_Report_Quantity.TabIndex = 13;
            // 
            // Control_TransferTab_Label_Report_User
            // 
            Control_TransferTab_Label_Report_User.Anchor = AnchorStyles.Left;
            Control_TransferTab_Label_Report_User.AutoSize = true;
            Control_TransferTab_Label_Report_User.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            Control_TransferTab_Label_Report_User.Location = new Point(3, 187);
            Control_TransferTab_Label_Report_User.Name = "Control_TransferTab_Label_Report_User";
            Control_TransferTab_Label_Report_User.Size = new Size(34, 13);
            Control_TransferTab_Label_Report_User.TabIndex = 14;
            Control_TransferTab_Label_Report_User.Text = "User:";
            // 
            // Control_TransferTab_TextBox_Report_User
            // 
            Control_TransferTab_TextBox_Report_User.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            Control_TransferTab_TextBox_Report_User.BackColor = SystemColors.Control;
            Control_TransferTab_TextBox_Report_User.Location = new Point(101, 182);
            Control_TransferTab_TextBox_Report_User.Name = "Control_TransferTab_TextBox_Report_User";
            Control_TransferTab_TextBox_Report_User.ReadOnly = true;
            Control_TransferTab_TextBox_Report_User.Size = new Size(310, 23);
            Control_TransferTab_TextBox_Report_User.TabIndex = 15;
            // 
            // Control_TransferTab_TableLayout_Right
            // 
            Control_TransferTab_TableLayout_Right.ColumnCount = 1;
            Control_TransferTab_TableLayout_Right.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_TransferTab_TableLayout_Right.Controls.Add(Control_TransferTab_Panel_DataGridView, 0, 0);
            Control_TransferTab_TableLayout_Right.Controls.Add(Control_TransferTab_Panel_PageButtons, 0, 1);
            Control_TransferTab_TableLayout_Right.Dock = DockStyle.Fill;
            Control_TransferTab_TableLayout_Right.Location = new Point(0, 0);
            Control_TransferTab_TableLayout_Right.Name = "Control_TransferTab_TableLayout_Right";
            Control_TransferTab_TableLayout_Right.RowCount = 2;
            Control_TransferTab_TableLayout_Right.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_TransferTab_TableLayout_Right.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Control_TransferTab_TableLayout_Right.Size = new Size(531, 442);
            Control_TransferTab_TableLayout_Right.TabIndex = 0;
            // 
            // Control_TransferTab_Panel_DataGridView
            // 
            Control_TransferTab_Panel_DataGridView.Controls.Add(Control_TransferTab_Image_NothingFound);
            Control_TransferTab_Panel_DataGridView.Controls.Add(Control_TransferTab_DataGridView_Main);
            Control_TransferTab_Panel_DataGridView.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_DataGridView.Location = new Point(3, 3);
            Control_TransferTab_Panel_DataGridView.Name = "Control_TransferTab_Panel_DataGridView";
            Control_TransferTab_Panel_DataGridView.Size = new Size(525, 396);
            Control_TransferTab_Panel_DataGridView.TabIndex = 0;
            // 
            // Control_TransferTab_Panel_PageButtons
            // 
            Control_TransferTab_Panel_PageButtons.Controls.Add(Control_TransferTab_Button_SelectionHistory);
            Control_TransferTab_Panel_PageButtons.Controls.Add(Control_TransferTab_Button_Next);
            Control_TransferTab_Panel_PageButtons.Controls.Add(Control_TransferTab_Button_Previous);
            Control_TransferTab_Panel_PageButtons.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_PageButtons.Location = new Point(3, 405);
            Control_TransferTab_Panel_PageButtons.Name = "Control_TransferTab_Panel_PageButtons";
            Control_TransferTab_Panel_PageButtons.Size = new Size(525, 34);
            Control_TransferTab_Panel_PageButtons.TabIndex = 1;
            // 
            // Control_TransferTab_Button_Previous
            // 
            Control_TransferTab_Button_Previous.Enabled = false;
            Control_TransferTab_Button_Previous.Location = new Point(3, 6);
            Control_TransferTab_Button_Previous.Name = "Control_TransferTab_Button_Previous";
            Control_TransferTab_Button_Previous.Size = new Size(75, 23);
            Control_TransferTab_Button_Previous.TabIndex = 0;
            Control_TransferTab_Button_Previous.Text = "◀ Previous";
            Control_TransferTab_Button_Previous.UseVisualStyleBackColor = true;
            // 
            // Control_TransferTab_Button_Next
            // 
            Control_TransferTab_Button_Next.Enabled = false;
            Control_TransferTab_Button_Next.Location = new Point(84, 6);
            Control_TransferTab_Button_Next.Name = "Control_TransferTab_Button_Next";
            Control_TransferTab_Button_Next.Size = new Size(75, 23);
            Control_TransferTab_Button_Next.TabIndex = 1;
            Control_TransferTab_Button_Next.Text = "Next ▶";
            Control_TransferTab_Button_Next.UseVisualStyleBackColor = true;
            // 
            // Control_TransferTab_Button_SelectionHistory
            // 
            Control_TransferTab_Button_SelectionHistory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_Button_SelectionHistory.Location = new Point(421, 6);
            Control_TransferTab_Button_SelectionHistory.Name = "Control_TransferTab_Button_SelectionHistory";
            Control_TransferTab_Button_SelectionHistory.Size = new Size(101, 23);
            Control_TransferTab_Button_SelectionHistory.TabIndex = 2;
            Control_TransferTab_Button_SelectionHistory.Text = "Transfer History";
            Control_TransferTab_Button_SelectionHistory.UseVisualStyleBackColor = true;
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
            Control_TransferTab_Image_NothingFound.Size = new Size(525, 396);
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
            Control_TransferTab_DataGridView_Main.Size = new Size(525, 396);
            Control_TransferTab_DataGridView_Main.StandardTab = true;
            Control_TransferTab_DataGridView_Main.TabIndex = 4;
            // 
            // Control_TransferTab_TableLayoutPanel_Bottom
            // 
            Control_TransferTab_TableLayoutPanel_Bottom.ColumnCount = 2;
            Control_TransferTab_TableLayoutPanel_Bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Control_TransferTab_TableLayoutPanel_Bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Control_TransferTab_TableLayoutPanel_Bottom.Controls.Add(Control_TransferTab_Panel_Bottom_Left, 0, 0);
            Control_TransferTab_TableLayoutPanel_Bottom.Controls.Add(Control_TransferTab_Panel_Bottom_Right, 1, 0);
            Control_TransferTab_TableLayoutPanel_Bottom.Dock = DockStyle.Fill;
            Control_TransferTab_TableLayoutPanel_Bottom.Location = new Point(3, 451);
            Control_TransferTab_TableLayoutPanel_Bottom.Name = "Control_TransferTab_TableLayoutPanel_Bottom";
            Control_TransferTab_TableLayoutPanel_Bottom.RowCount = 1;
            Control_TransferTab_TableLayoutPanel_Bottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_TransferTab_TableLayoutPanel_Bottom.Size = new Size(955, 34);
            Control_TransferTab_TableLayoutPanel_Bottom.TabIndex = 1;
            // 
            // Control_TransferTab_Panel_Bottom_Left
            // 
            Control_TransferTab_Panel_Bottom_Left.Controls.Add(Control_TransferTab_Button_Search);
            Control_TransferTab_Panel_Bottom_Left.Controls.Add(Control_TransferTab_Button_SidePanel);
            Control_TransferTab_Panel_Bottom_Left.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Bottom_Left.Location = new Point(3, 3);
            Control_TransferTab_Panel_Bottom_Left.Name = "Control_TransferTab_Panel_Bottom_Left";
            Control_TransferTab_Panel_Bottom_Left.Size = new Size(471, 28);
            Control_TransferTab_Panel_Bottom_Left.TabIndex = 0;
            // 
            // Control_TransferTab_Button_Search
            // 
            Control_TransferTab_Button_Search.Location = new Point(3, 3);
            Control_TransferTab_Button_Search.Name = "Control_TransferTab_Button_Search";
            Control_TransferTab_Button_Search.Size = new Size(75, 23);
            Control_TransferTab_Button_Search.TabIndex = 0;
            Control_TransferTab_Button_Search.Text = "Search";
            Control_TransferTab_Button_Search.UseVisualStyleBackColor = true;
            Control_TransferTab_Button_Search.Click += Control_TransferTab_Button_Search_Click;
            // 
            // Control_TransferTab_Button_SidePanel
            // 
            Control_TransferTab_Button_SidePanel.Font = new Font("Segoe UI Emoji", 9F);
            Control_TransferTab_Button_SidePanel.Location = new Point(84, 3);
            Control_TransferTab_Button_SidePanel.Name = "Control_TransferTab_Button_SidePanel";
            Control_TransferTab_Button_SidePanel.Size = new Size(100, 23);
            Control_TransferTab_Button_SidePanel.TabIndex = 1;
            Control_TransferTab_Button_SidePanel.Text = "Hide Panel ⬅️";
            Control_TransferTab_Button_SidePanel.UseVisualStyleBackColor = true;
            // 
            // Control_TransferTab_Panel_Bottom_Right
            // 
            Control_TransferTab_Panel_Bottom_Right.Controls.Add(Control_TransferTab_Button_Transfer);
            Control_TransferTab_Panel_Bottom_Right.Controls.Add(Control_TransferTab_Button_Print);
            Control_TransferTab_Panel_Bottom_Right.Controls.Add(Control_TransferTab_Button_Reset);
            Control_TransferTab_Panel_Bottom_Right.Dock = DockStyle.Fill;
            Control_TransferTab_Panel_Bottom_Right.Location = new Point(480, 3);
            Control_TransferTab_Panel_Bottom_Right.Name = "Control_TransferTab_Panel_Bottom_Right";
            Control_TransferTab_Panel_Bottom_Right.Size = new Size(472, 28);
            Control_TransferTab_Panel_Bottom_Right.TabIndex = 1;
            // 
            // Control_TransferTab_Button_Transfer
            // 
            Control_TransferTab_Button_Transfer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_Button_Transfer.Enabled = false;
            Control_TransferTab_Button_Transfer.Location = new Point(238, 3);
            Control_TransferTab_Button_Transfer.Name = "Control_TransferTab_Button_Transfer";
            Control_TransferTab_Button_Transfer.Size = new Size(75, 23);
            Control_TransferTab_Button_Transfer.TabIndex = 0;
            Control_TransferTab_Button_Transfer.Text = "Transfer";
            Control_TransferTab_Button_Transfer.UseVisualStyleBackColor = true;
            // 
            // Control_TransferTab_Button_Print
            // 
            Control_TransferTab_Button_Print.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_Button_Print.Enabled = false;
            Control_TransferTab_Button_Print.Location = new Point(319, 3);
            Control_TransferTab_Button_Print.Name = "Control_TransferTab_Button_Print";
            Control_TransferTab_Button_Print.Size = new Size(75, 23);
            Control_TransferTab_Button_Print.TabIndex = 1;
            Control_TransferTab_Button_Print.TabStop = false;
            Control_TransferTab_Button_Print.Text = "Print";
            Control_TransferTab_Button_Print.UseVisualStyleBackColor = true;
            // 
            // Control_TransferTab_Button_Reset
            // 
            Control_TransferTab_Button_Reset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_TransferTab_Button_Reset.Location = new Point(400, 3);
            Control_TransferTab_Button_Reset.Name = "Control_TransferTab_Button_Reset";
            Control_TransferTab_Button_Reset.Size = new Size(75, 23);
            Control_TransferTab_Button_Reset.TabIndex = 2;
            Control_TransferTab_Button_Reset.TabStop = false;
            Control_TransferTab_Button_Reset.Text = "Reset";
            Control_TransferTab_Button_Reset.UseVisualStyleBackColor = true;
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
            Control_TransferTab_TableLayout_Main.ResumeLayout(false);
            Control_TransferTab_SplitContainer_Main.Panel1.ResumeLayout(false);
            Control_TransferTab_SplitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_SplitContainer_Main).EndInit();
            Control_TransferTab_SplitContainer_Main.ResumeLayout(false);
            Control_TransferTab_TableLayout_Inputs.ResumeLayout(false);
            Control_TransferTab_Panel_Row_SortBy.ResumeLayout(false);
            Control_TransferTab_Panel_Row_PartID.ResumeLayout(false);
            Control_TransferTab_Panel_Row_Operation.ResumeLayout(false);
            Control_TransferTab_Panel_Row_FromLocation.ResumeLayout(false);
            Control_TransferTab_Panel_Row_ToLocation.ResumeLayout(false);
            Control_TransferTab_Panel_Row_Quantity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_NumericUpDown_Quantity).EndInit();
            Control_TransferTab_Panel_Row_SmartSearch.ResumeLayout(false);
            Control_TransferTab_Panel_Row_SmartSearch.PerformLayout();
            Control_TransferTab_TableLayout_SelectionReport.ResumeLayout(false);
            Control_TransferTab_TableLayout_SelectionReport.PerformLayout();
            Control_TransferTab_TableLayout_Right.ResumeLayout(false);
            Control_TransferTab_Panel_DataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_Image_NothingFound).EndInit();
            ((System.ComponentModel.ISupportInitialize)Control_TransferTab_DataGridView_Main).EndInit();
            Control_TransferTab_Panel_PageButtons.ResumeLayout(false);
            Control_TransferTab_TableLayoutPanel_Bottom.ResumeLayout(false);
            Control_TransferTab_Panel_Bottom_Left.ResumeLayout(false);
            Control_TransferTab_Panel_Bottom_Right.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        // Field declarations for the redesigned transfer form
        private TableLayoutPanel Control_TransferTab_TableLayout_Main;
        private SplitContainer Control_TransferTab_SplitContainer_Main;
        private TableLayoutPanel Control_TransferTab_TableLayout_Inputs;
        
        // Input row panels
        private Panel Control_TransferTab_Panel_Row_SortBy;
        private Label Control_TransferTab_Label_SortBy;
        private ComboBox Control_TransferTab_ComboBox_SortBy;
        private Panel Control_TransferTab_Panel_Row_PartID;
        private Panel Control_TransferTab_Panel_Row_Operation;
        private Panel Control_TransferTab_Panel_Row_FromLocation;
        private Label Control_TransferTab_Label_FromLocation;
        private ComboBox Control_TransferTab_ComboBox_FromLocation;
        private Panel Control_TransferTab_Panel_Row_ToLocation;
        private Panel Control_TransferTab_Panel_Row_Quantity;
        private Panel Control_TransferTab_Panel_Row_SmartSearch;
        private Label Control_TransferTab_Label_SmartSearch;
        private TextBox Control_TransferTab_TextBox_SmartSearch;
        private Button Control_TransferTab_Button_SmartSearch;
        private Label Control_TransferTab_Label_SmartSearchHelp;
        
        // Selection report
        private TableLayoutPanel Control_TransferTab_TableLayout_SelectionReport;
        private Label Control_TransferTab_Label_Report_PartID;
        private TextBox Control_TransferTab_TextBox_Report_PartID;
        private Label Control_TransferTab_Label_Report_Operation;
        private TextBox Control_TransferTab_TextBox_Report_Operation;
        private Label Control_TransferTab_Label_Report_FromLocation;
        private TextBox Control_TransferTab_TextBox_Report_FromLocation;
        private Label Control_TransferTab_Label_Report_ToLocation;
        private TextBox Control_TransferTab_TextBox_Report_ToLocation;
        private Label Control_TransferTab_Label_Report_Quantity;
        private TextBox Control_TransferTab_TextBox_Report_Quantity;
        private Label Control_TransferTab_Label_Report_User;
        private TextBox Control_TransferTab_TextBox_Report_User;
        private Label Control_TransferTab_Label_Report_BatchNumber;
        private TextBox Control_TransferTab_TextBox_Report_BatchNumber;
        private Label Control_TransferTab_Label_Report_TransferType;
        private TextBox Control_TransferTab_TextBox_Report_TransferType;
        
        // Right panel
        private TableLayoutPanel Control_TransferTab_TableLayout_Right;
        private Panel Control_TransferTab_Panel_PageButtons;
        private Button Control_TransferTab_Button_SelectionHistory;
        private Button Control_TransferTab_Button_Next;
        private Button Control_TransferTab_Button_Previous;
        
        // Bottom panel
        private TableLayoutPanel Control_TransferTab_TableLayoutPanel_Bottom;
        private Panel Control_TransferTab_Panel_Bottom_Left;
        private Panel Control_TransferTab_Panel_Bottom_Right;
        private Button Control_TransferTab_Button_SidePanel;

        // Existing controls (kept for compatibility)
        internal ComboBox Control_TransferTab_ComboBox_Part;
        private Label Control_TransferTab_Label_Part;
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
        private Panel Control_TransferTab_Panel_DataGridView;
        private PictureBox Control_TransferTab_Image_NothingFound;
    }

        
        #endregion
    }
