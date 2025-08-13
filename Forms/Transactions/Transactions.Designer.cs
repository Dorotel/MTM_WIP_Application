using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Forms.Transactions
{
    partial class Transactions : Form
    {
        #region Fields

        private System.ComponentModel.IContainer components = null;

        // Top-level containers
        private GroupBox Transactions_GroupBox_Main;
        private TableLayoutPanel Transactions_TableLayout_Main;

        // Split container for left (inputs) and right (results)
        private SplitContainer Transactions_SplitContainer_Main;

        // Inputs container (left panel of split)
        private TableLayoutPanel Transactions_TableLayout_Inputs;
        private Panel Transactions_Panel_Row_SortBy;
        private Label Transactions_Label_SortBy;
        private ComboBox Transactions_ComboBox_SortBy;
        private Panel Transactions_Panel_Row_SearchPartID;
        private Label Transactions_Label_SearchPartID;
        private Panel Transactions_Panel_Row_User;
        private Label Transactions_Label_User;
        private ComboBox Transactions_ComboBox_UserFullName;
        private Panel Transactions_Panel_Row_Shift;
        private Panel Transactions_Panel_DataGridView;

        // Results container (right panel of split)
        private DataGridView Transactions_DataGridView_Transactions;

        // Bottom container
        private Panel Transactions_Panel_Bottom_Left;
        private Button Transactions_Button_Reset;

        // Smart Search Controls
        private Panel Transactions_Panel_SmartSearch;
        private Label Transactions_Label_SmartSearch;
        private TextBox Transactions_TextBox_SmartSearch;
        private Button Transactions_Button_SmartSearch;
        private Label Transactions_Label_SmartSearchHelp;

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

        private void InitializeComponent()
        {
            Transactions_GroupBox_Main = new GroupBox();
            Transactions_TableLayout_Main = new TableLayoutPanel();
            Transactions_SplitContainer_Main = new SplitContainer();
            Transactions_TableLayout_Inputs = new TableLayoutPanel();
            Transactions_Panel_Row_Building = new Panel();
            Transactions_Label_Building = new Label();
            Transactions_ComboBox_Building = new ComboBox();
            Transactions_Panel_Row_SortBy = new Panel();
            Transactions_Label_SortBy = new Label();
            Transactions_ComboBox_SortBy = new ComboBox();
            Transactions_Panel_Row_SearchPartID = new Panel();
            Transactions_Label_SearchPartID = new Label();
            _transactionsComboBoxSearchPartId = new ComboBox();
            Transactions_Panel_Row_User = new Panel();
            Transactions_Label_User = new Label();
            Transactions_ComboBox_UserFullName = new ComboBox();
            Transactions_Panel_Row_Shift = new Panel();
            Transactions_Panel_Row_DateRange = new Panel();
            Control_AdvancedRemove_Label_DateDash = new Label();
            Control_AdvancedRemove_DateTimePicker_To = new DateTimePicker();
            Control_AdvancedRemove_CheckBox_Date = new CheckBox();
            Control_AdvancedRemove_DateTimePicker_From = new DateTimePicker();
            Transactions_TableLayout_SelectionReport = new TableLayoutPanel();
            Transactions_Label_Report_BatchNumber = new Label();
            Transactions_Label_Report_PartID = new Label();
            Transactions_TextBox_Report_PartID = new TextBox();
            Transactions_Label_Report_FromLocation = new Label();
            Transactions_TextBox_Report_FromLocation = new TextBox();
            Transactions_Label_Report_ToLocation = new Label();
            Transactions_TextBox_Report_ToLocation = new TextBox();
            Transactions_Label_Report_Operation = new Label();
            Transactions_TextBox_Report_Operation = new TextBox();
            Transactions_Label_Report_Quantity = new Label();
            Transactions_TextBox_Report_Quantity = new TextBox();
            Transactions_Label_Report_Notes = new Label();
            Transactions_TextBox_Notes = new TextBox();
            Transactions_Label_Report_User = new Label();
            Transactions_TextBox_Report_User = new TextBox();
            Transactions_Label_Report_ItemType = new Label();
            Transactions_TextBox_Report_ItemType = new TextBox();
            Transactions_Label_Report_ReceiveDate = new Label();
            Transactions_TextBox_Report_ReceiveDate = new TextBox();
            Transactions_TextBox_Report_TransactionType = new TextBox();
            Transactions_TextBox_Report_BatchNumber = new TextBox();
            Transactions_Label_Report_TransactionType = new Label();
            Transfer_TableLayout_Right = new TableLayoutPanel();
            Transactions_Panel_DataGridView = new Panel();
            Transactions_Image_NothingFound = new PictureBox();
            Transactions_DataGridView_Transactions = new DataGridView();
            Transfer_Panel_PageButtons = new Panel();
            Transfer_Button_SelectionHistory = new Button();
            Transfer_Button_Next = new Button();
            Transfer_Button_Previous = new Button();
            Transactions_TabelLayoutPanel_Bottom = new TableLayoutPanel();
            Transactions_Panel_Bottom_Right = new Panel();
            Transactions_Button_Print = new Button();
            Transactions_Button_Reset = new Button();
            Transactions_Panel_Bottom_Left = new Panel();
            Transactions_Button_Search = new Button();
            Transactions_Button_SidePanel = new Button();
            Transactions_GroupBox_Main.SuspendLayout();
            Transactions_TableLayout_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Transactions_SplitContainer_Main).BeginInit();
            Transactions_SplitContainer_Main.Panel1.SuspendLayout();
            Transactions_SplitContainer_Main.Panel2.SuspendLayout();
            Transactions_SplitContainer_Main.SuspendLayout();
            Transactions_TableLayout_Inputs.SuspendLayout();
            Transactions_Panel_Row_Building.SuspendLayout();
            Transactions_Panel_Row_SortBy.SuspendLayout();
            Transactions_Panel_Row_SearchPartID.SuspendLayout();
            Transactions_Panel_Row_User.SuspendLayout();
            Transactions_Panel_Row_Shift.SuspendLayout();
            Transactions_Panel_Row_DateRange.SuspendLayout();
            Transactions_TableLayout_SelectionReport.SuspendLayout();
            Transfer_TableLayout_Right.SuspendLayout();
            Transactions_Panel_DataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Transactions_Image_NothingFound).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Transactions_DataGridView_Transactions).BeginInit();
            Transfer_Panel_PageButtons.SuspendLayout();
            Transactions_TabelLayoutPanel_Bottom.SuspendLayout();
            Transactions_Panel_Bottom_Right.SuspendLayout();
            Transactions_Panel_Bottom_Left.SuspendLayout();
            SuspendLayout();
            // 
            // Transactions_GroupBox_Main
            // 
            Transactions_GroupBox_Main.Controls.Add(Transactions_TableLayout_Main);
            Transactions_GroupBox_Main.Dock = DockStyle.Fill;
            Transactions_GroupBox_Main.Location = new Point(0, 0);
            Transactions_GroupBox_Main.Name = "Transactions_GroupBox_Main";
            Transactions_GroupBox_Main.Size = new Size(830, 592);
            Transactions_GroupBox_Main.TabIndex = 0;
            Transactions_GroupBox_Main.TabStop = false;
            Transactions_GroupBox_Main.Text = "Transactions";
            // 
            // Transactions_TableLayout_Main
            // 
            Transactions_TableLayout_Main.ColumnCount = 1;
            Transactions_TableLayout_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            Transactions_TableLayout_Main.Controls.Add(Transactions_SplitContainer_Main, 0, 0);
            Transactions_TableLayout_Main.Controls.Add(Transactions_TabelLayoutPanel_Bottom, 0, 1);
            Transactions_TableLayout_Main.Dock = DockStyle.Fill;
            Transactions_TableLayout_Main.Location = new Point(3, 19);
            Transactions_TableLayout_Main.Name = "Transactions_TableLayout_Main";
            Transactions_TableLayout_Main.RowCount = 2;
            Transactions_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Transactions_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Transactions_TableLayout_Main.Size = new Size(824, 570);
            Transactions_TableLayout_Main.TabIndex = 0;
            // 
            // Transactions_SplitContainer_Main
            // 
            Transactions_SplitContainer_Main.Dock = DockStyle.Fill;
            Transactions_SplitContainer_Main.Location = new Point(3, 3);
            Transactions_SplitContainer_Main.Name = "Transactions_SplitContainer_Main";
            // 
            // Transactions_SplitContainer_Main.Panel1
            // 
            Transactions_SplitContainer_Main.Panel1.Controls.Add(Transactions_TableLayout_Inputs);
            Transactions_SplitContainer_Main.Panel1MinSize = 380;
            // 
            // Transactions_SplitContainer_Main.Panel2
            // 
            Transactions_SplitContainer_Main.Panel2.Controls.Add(Transfer_TableLayout_Right);
            Transactions_SplitContainer_Main.Panel2MinSize = 380;
            Transactions_SplitContainer_Main.Size = new Size(818, 524);
            Transactions_SplitContainer_Main.SplitterDistance = 409;
            Transactions_SplitContainer_Main.TabIndex = 0;
            // 
            // Transactions_TableLayout_Inputs
            // 
            Transactions_TableLayout_Inputs.ColumnCount = 1;
            Transactions_TableLayout_Inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            Transactions_TableLayout_Inputs.Controls.Add(Transactions_Panel_Row_Building, 0, 3);
            Transactions_TableLayout_Inputs.Controls.Add(Transactions_Panel_Row_SortBy, 0, 0);
            Transactions_TableLayout_Inputs.Controls.Add(Transactions_Panel_Row_SearchPartID, 0, 1);
            Transactions_TableLayout_Inputs.Controls.Add(Transactions_Panel_Row_User, 0, 2);
            Transactions_TableLayout_Inputs.Controls.Add(Transactions_Panel_Row_Shift, 0, 4);
            Transactions_TableLayout_Inputs.Controls.Add(Transactions_TableLayout_SelectionReport, 0, 5);
            Transactions_TableLayout_Inputs.Dock = DockStyle.Fill;
            Transactions_TableLayout_Inputs.Location = new Point(0, 0);
            Transactions_TableLayout_Inputs.Name = "Transactions_TableLayout_Inputs";
            Transactions_TableLayout_Inputs.RowCount = 6;
            Transactions_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Transactions_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Transactions_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Transactions_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Transactions_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Transactions_TableLayout_Inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Transactions_TableLayout_Inputs.Size = new Size(409, 524);
            Transactions_TableLayout_Inputs.TabIndex = 0;
            // 
            // Transactions_Panel_Row_Building
            // 
            Transactions_Panel_Row_Building.Controls.Add(Transactions_Label_Building);
            Transactions_Panel_Row_Building.Controls.Add(Transactions_ComboBox_Building);
            Transactions_Panel_Row_Building.Location = new Point(3, 123);
            Transactions_Panel_Row_Building.Name = "Transactions_Panel_Row_Building";
            Transactions_Panel_Row_Building.Size = new Size(403, 34);
            Transactions_Panel_Row_Building.TabIndex = 5;
            // 
            // Transactions_Label_Building
            // 
            Transactions_Label_Building.Location = new Point(6, 6);
            Transactions_Label_Building.Name = "Transactions_Label_Building";
            Transactions_Label_Building.Size = new Size(83, 23);
            Transactions_Label_Building.TabIndex = 0;
            Transactions_Label_Building.Text = "Building:";
            Transactions_Label_Building.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_ComboBox_Building
            // 
            Transactions_ComboBox_Building.Location = new Point(94, 6);
            Transactions_ComboBox_Building.Name = "Transactions_ComboBox_Building";
            Transactions_ComboBox_Building.Size = new Size(306, 23);
            Transactions_ComboBox_Building.TabIndex = 1;
            // 
            // Transactions_Panel_Row_SortBy
            // 
            Transactions_Panel_Row_SortBy.Controls.Add(Transactions_Label_SortBy);
            Transactions_Panel_Row_SortBy.Controls.Add(Transactions_ComboBox_SortBy);
            Transactions_Panel_Row_SortBy.Location = new Point(3, 3);
            Transactions_Panel_Row_SortBy.Name = "Transactions_Panel_Row_SortBy";
            Transactions_Panel_Row_SortBy.Size = new Size(403, 34);
            Transactions_Panel_Row_SortBy.TabIndex = 0;
            // 
            // Transactions_Label_SortBy
            // 
            Transactions_Label_SortBy.Location = new Point(5, 6);
            Transactions_Label_SortBy.Name = "Transactions_Label_SortBy";
            Transactions_Label_SortBy.Size = new Size(83, 23);
            Transactions_Label_SortBy.TabIndex = 0;
            Transactions_Label_SortBy.Text = "Sort By:";
            Transactions_Label_SortBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_ComboBox_SortBy
            // 
            Transactions_ComboBox_SortBy.Location = new Point(94, 6);
            Transactions_ComboBox_SortBy.Name = "Transactions_ComboBox_SortBy";
            Transactions_ComboBox_SortBy.Size = new Size(306, 23);
            Transactions_ComboBox_SortBy.TabIndex = 1;
            // 
            // Transactions_Panel_Row_SearchPartID
            // 
            Transactions_Panel_Row_SearchPartID.Controls.Add(Transactions_Label_SearchPartID);
            Transactions_Panel_Row_SearchPartID.Controls.Add(_transactionsComboBoxSearchPartId);
            Transactions_Panel_Row_SearchPartID.Location = new Point(3, 43);
            Transactions_Panel_Row_SearchPartID.Name = "Transactions_Panel_Row_SearchPartID";
            Transactions_Panel_Row_SearchPartID.Size = new Size(403, 34);
            Transactions_Panel_Row_SearchPartID.TabIndex = 1;
            // 
            // Transactions_Label_SearchPartID
            // 
            Transactions_Label_SearchPartID.Location = new Point(5, 5);
            Transactions_Label_SearchPartID.Name = "Transactions_Label_SearchPartID";
            Transactions_Label_SearchPartID.Size = new Size(83, 23);
            Transactions_Label_SearchPartID.TabIndex = 0;
            Transactions_Label_SearchPartID.Text = "Part ID:";
            Transactions_Label_SearchPartID.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_ComboBox_SearchPartID
            // 
            _transactionsComboBoxSearchPartId.Location = new Point(94, 5);
            _transactionsComboBoxSearchPartId.Name = "_transactionsComboBoxSearchPartId";
            _transactionsComboBoxSearchPartId.Size = new Size(306, 23);
            _transactionsComboBoxSearchPartId.TabIndex = 1;
            // 
            // Transactions_Panel_Row_User
            // 
            Transactions_Panel_Row_User.Controls.Add(Transactions_Label_User);
            Transactions_Panel_Row_User.Controls.Add(Transactions_ComboBox_UserFullName);
            Transactions_Panel_Row_User.Location = new Point(3, 83);
            Transactions_Panel_Row_User.Name = "Transactions_Panel_Row_User";
            Transactions_Panel_Row_User.Size = new Size(403, 34);
            Transactions_Panel_Row_User.TabIndex = 2;
            // 
            // Transactions_Label_User
            // 
            Transactions_Label_User.Location = new Point(6, 6);
            Transactions_Label_User.Name = "Transactions_Label_User";
            Transactions_Label_User.Size = new Size(83, 23);
            Transactions_Label_User.TabIndex = 0;
            Transactions_Label_User.Text = "User:";
            Transactions_Label_User.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_ComboBox_UserFullName
            // 
            Transactions_ComboBox_UserFullName.Location = new Point(94, 6);
            Transactions_ComboBox_UserFullName.Name = "Transactions_ComboBox_UserFullName";
            Transactions_ComboBox_UserFullName.Size = new Size(306, 23);
            Transactions_ComboBox_UserFullName.TabIndex = 1;
            // 
            // Transactions_Panel_Row_Shift
            // 
            Transactions_Panel_Row_Shift.Controls.Add(Transactions_Panel_Row_DateRange);
            Transactions_Panel_Row_Shift.Location = new Point(3, 163);
            Transactions_Panel_Row_Shift.Name = "Transactions_Panel_Row_Shift";
            Transactions_Panel_Row_Shift.Size = new Size(403, 34);
            Transactions_Panel_Row_Shift.TabIndex = 4;
            // 
            // Transactions_Panel_Row_DateRange
            // 
            Transactions_Panel_Row_DateRange.Controls.Add(Control_AdvancedRemove_Label_DateDash);
            Transactions_Panel_Row_DateRange.Controls.Add(Control_AdvancedRemove_DateTimePicker_To);
            Transactions_Panel_Row_DateRange.Controls.Add(Control_AdvancedRemove_CheckBox_Date);
            Transactions_Panel_Row_DateRange.Controls.Add(Control_AdvancedRemove_DateTimePicker_From);
            Transactions_Panel_Row_DateRange.Dock = DockStyle.Fill;
            Transactions_Panel_Row_DateRange.Location = new Point(0, 0);
            Transactions_Panel_Row_DateRange.Name = "Transactions_Panel_Row_DateRange";
            Transactions_Panel_Row_DateRange.Size = new Size(403, 34);
            Transactions_Panel_Row_DateRange.TabIndex = 8;
            // 
            // Control_AdvancedRemove_Label_DateDash
            // 
            Control_AdvancedRemove_Label_DateDash.Location = new Point(239, 5);
            Control_AdvancedRemove_Label_DateDash.Name = "Control_AdvancedRemove_Label_DateDash";
            Control_AdvancedRemove_Label_DateDash.Size = new Size(16, 23);
            Control_AdvancedRemove_Label_DateDash.TabIndex = 1;
            Control_AdvancedRemove_Label_DateDash.Text = "-";
            Control_AdvancedRemove_Label_DateDash.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Control_AdvancedRemove_DateTimePicker_To
            // 
            Control_AdvancedRemove_DateTimePicker_To.Anchor = AnchorStyles.Left;
            Control_AdvancedRemove_DateTimePicker_To.Format = DateTimePickerFormat.Short;
            Control_AdvancedRemove_DateTimePicker_To.Location = new Point(260, 5);
            Control_AdvancedRemove_DateTimePicker_To.Name = "Control_AdvancedRemove_DateTimePicker_To";
            Control_AdvancedRemove_DateTimePicker_To.Size = new Size(140, 23);
            Control_AdvancedRemove_DateTimePicker_To.TabIndex = 2;
            // 
            // Control_AdvancedRemove_CheckBox_Date
            // 
            Control_AdvancedRemove_CheckBox_Date.Location = new Point(6, 5);
            Control_AdvancedRemove_CheckBox_Date.Name = "Control_AdvancedRemove_CheckBox_Date";
            Control_AdvancedRemove_CheckBox_Date.Size = new Size(83, 23);
            Control_AdvancedRemove_CheckBox_Date.TabIndex = 3;
            Control_AdvancedRemove_CheckBox_Date.Text = "Range:";
            Control_AdvancedRemove_CheckBox_Date.TextAlign = ContentAlignment.MiddleRight;
            Control_AdvancedRemove_CheckBox_Date.UseVisualStyleBackColor = true;
            // 
            // Control_AdvancedRemove_DateTimePicker_From
            // 
            Control_AdvancedRemove_DateTimePicker_From.Anchor = AnchorStyles.Left;
            Control_AdvancedRemove_DateTimePicker_From.Format = DateTimePickerFormat.Short;
            Control_AdvancedRemove_DateTimePicker_From.Location = new Point(94, 5);
            Control_AdvancedRemove_DateTimePicker_From.Name = "Control_AdvancedRemove_DateTimePicker_From";
            Control_AdvancedRemove_DateTimePicker_From.Size = new Size(140, 23);
            Control_AdvancedRemove_DateTimePicker_From.TabIndex = 0;
            // 
            // Transactions_TableLayout_SelectionReport
            // 
            Transactions_TableLayout_SelectionReport.ColumnCount = 2;
            Transactions_TableLayout_SelectionReport.ColumnStyles.Add(new ColumnStyle());
            Transactions_TableLayout_SelectionReport.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_BatchNumber, 0, 1);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_PartID, 0, 2);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Report_PartID, 1, 2);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_FromLocation, 0, 3);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Report_FromLocation, 1, 3);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_ToLocation, 0, 4);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Report_ToLocation, 1, 4);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_Operation, 0, 5);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Report_Operation, 1, 5);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_Quantity, 0, 6);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Report_Quantity, 1, 6);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_Notes, 0, 7);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Notes, 1, 7);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_User, 0, 8);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Report_User, 1, 8);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_ItemType, 0, 9);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Report_ItemType, 1, 9);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_ReceiveDate, 0, 10);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Report_ReceiveDate, 1, 10);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Report_TransactionType, 1, 0);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_TextBox_Report_BatchNumber, 1, 1);
            Transactions_TableLayout_SelectionReport.Controls.Add(Transactions_Label_Report_TransactionType, 0, 0);
            Transactions_TableLayout_SelectionReport.Dock = DockStyle.Fill;
            Transactions_TableLayout_SelectionReport.Location = new Point(3, 203);
            Transactions_TableLayout_SelectionReport.Name = "Transactions_TableLayout_SelectionReport";
            Transactions_TableLayout_SelectionReport.RowCount = 11;
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.RowStyles.Add(new RowStyle());
            Transactions_TableLayout_SelectionReport.Size = new Size(403, 318);
            Transactions_TableLayout_SelectionReport.TabIndex = 6;
            // 
            // Transactions_Label_Report_BatchNumber
            // 
            Transactions_Label_Report_BatchNumber.Dock = DockStyle.Fill;
            Transactions_Label_Report_BatchNumber.Location = new Point(3, 29);
            Transactions_Label_Report_BatchNumber.Name = "Transactions_Label_Report_BatchNumber";
            Transactions_Label_Report_BatchNumber.Size = new Size(100, 29);
            Transactions_Label_Report_BatchNumber.TabIndex = 2;
            Transactions_Label_Report_BatchNumber.Text = "Batch Number:";
            Transactions_Label_Report_BatchNumber.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_Label_Report_PartID
            // 
            Transactions_Label_Report_PartID.Dock = DockStyle.Fill;
            Transactions_Label_Report_PartID.Location = new Point(3, 58);
            Transactions_Label_Report_PartID.Name = "Transactions_Label_Report_PartID";
            Transactions_Label_Report_PartID.Size = new Size(100, 29);
            Transactions_Label_Report_PartID.TabIndex = 4;
            Transactions_Label_Report_PartID.Text = "Part ID:";
            Transactions_Label_Report_PartID.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_TextBox_Report_PartID
            // 
            Transactions_TextBox_Report_PartID.Dock = DockStyle.Fill;
            Transactions_TextBox_Report_PartID.Location = new Point(109, 61);
            Transactions_TextBox_Report_PartID.Name = "Transactions_TextBox_Report_PartID";
            Transactions_TextBox_Report_PartID.ReadOnly = true;
            Transactions_TextBox_Report_PartID.Size = new Size(291, 23);
            Transactions_TextBox_Report_PartID.TabIndex = 5;
            // 
            // Transactions_Label_Report_FromLocation
            // 
            Transactions_Label_Report_FromLocation.Dock = DockStyle.Fill;
            Transactions_Label_Report_FromLocation.Location = new Point(3, 87);
            Transactions_Label_Report_FromLocation.Name = "Transactions_Label_Report_FromLocation";
            Transactions_Label_Report_FromLocation.Size = new Size(100, 29);
            Transactions_Label_Report_FromLocation.TabIndex = 6;
            Transactions_Label_Report_FromLocation.Text = "From Location:";
            Transactions_Label_Report_FromLocation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_TextBox_Report_FromLocation
            // 
            Transactions_TextBox_Report_FromLocation.Dock = DockStyle.Fill;
            Transactions_TextBox_Report_FromLocation.Location = new Point(109, 90);
            Transactions_TextBox_Report_FromLocation.Name = "Transactions_TextBox_Report_FromLocation";
            Transactions_TextBox_Report_FromLocation.ReadOnly = true;
            Transactions_TextBox_Report_FromLocation.Size = new Size(291, 23);
            Transactions_TextBox_Report_FromLocation.TabIndex = 7;
            // 
            // Transactions_Label_Report_ToLocation
            // 
            Transactions_Label_Report_ToLocation.Dock = DockStyle.Fill;
            Transactions_Label_Report_ToLocation.Location = new Point(3, 116);
            Transactions_Label_Report_ToLocation.Name = "Transactions_Label_Report_ToLocation";
            Transactions_Label_Report_ToLocation.Size = new Size(100, 29);
            Transactions_Label_Report_ToLocation.TabIndex = 8;
            Transactions_Label_Report_ToLocation.Text = "To Location:";
            Transactions_Label_Report_ToLocation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_TextBox_Report_ToLocation
            // 
            Transactions_TextBox_Report_ToLocation.Dock = DockStyle.Fill;
            Transactions_TextBox_Report_ToLocation.Location = new Point(109, 119);
            Transactions_TextBox_Report_ToLocation.Name = "Transactions_TextBox_Report_ToLocation";
            Transactions_TextBox_Report_ToLocation.ReadOnly = true;
            Transactions_TextBox_Report_ToLocation.Size = new Size(291, 23);
            Transactions_TextBox_Report_ToLocation.TabIndex = 9;
            // 
            // Transactions_Label_Report_Operation
            // 
            Transactions_Label_Report_Operation.Dock = DockStyle.Fill;
            Transactions_Label_Report_Operation.Location = new Point(3, 145);
            Transactions_Label_Report_Operation.Name = "Transactions_Label_Report_Operation";
            Transactions_Label_Report_Operation.Size = new Size(100, 29);
            Transactions_Label_Report_Operation.TabIndex = 10;
            Transactions_Label_Report_Operation.Text = "Operation:";
            Transactions_Label_Report_Operation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_TextBox_Report_Operation
            // 
            Transactions_TextBox_Report_Operation.Dock = DockStyle.Fill;
            Transactions_TextBox_Report_Operation.Location = new Point(109, 148);
            Transactions_TextBox_Report_Operation.Name = "Transactions_TextBox_Report_Operation";
            Transactions_TextBox_Report_Operation.ReadOnly = true;
            Transactions_TextBox_Report_Operation.Size = new Size(291, 23);
            Transactions_TextBox_Report_Operation.TabIndex = 11;
            // 
            // Transactions_Label_Report_Quantity
            // 
            Transactions_Label_Report_Quantity.Dock = DockStyle.Fill;
            Transactions_Label_Report_Quantity.Location = new Point(3, 174);
            Transactions_Label_Report_Quantity.Name = "Transactions_Label_Report_Quantity";
            Transactions_Label_Report_Quantity.Size = new Size(100, 29);
            Transactions_Label_Report_Quantity.TabIndex = 12;
            Transactions_Label_Report_Quantity.Text = "Quantity:";
            Transactions_Label_Report_Quantity.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_TextBox_Report_Quantity
            // 
            Transactions_TextBox_Report_Quantity.Dock = DockStyle.Fill;
            Transactions_TextBox_Report_Quantity.Location = new Point(109, 177);
            Transactions_TextBox_Report_Quantity.Name = "Transactions_TextBox_Report_Quantity";
            Transactions_TextBox_Report_Quantity.ReadOnly = true;
            Transactions_TextBox_Report_Quantity.Size = new Size(291, 23);
            Transactions_TextBox_Report_Quantity.TabIndex = 13;
            // 
            // Transactions_Label_Report_Notes
            // 
            Transactions_Label_Report_Notes.Dock = DockStyle.Fill;
            Transactions_Label_Report_Notes.Location = new Point(3, 203);
            Transactions_Label_Report_Notes.Name = "Transactions_Label_Report_Notes";
            Transactions_Label_Report_Notes.Size = new Size(100, 29);
            Transactions_Label_Report_Notes.TabIndex = 14;
            Transactions_Label_Report_Notes.Text = "Notes:";
            Transactions_Label_Report_Notes.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_TextBox_Notes
            // 
            Transactions_TextBox_Notes.Dock = DockStyle.Fill;
            Transactions_TextBox_Notes.Location = new Point(109, 206);
            Transactions_TextBox_Notes.Name = "Transactions_TextBox_Notes";
            Transactions_TextBox_Notes.ReadOnly = true;
            Transactions_TextBox_Notes.Size = new Size(291, 23);
            Transactions_TextBox_Notes.TabIndex = 15;
            // 
            // Transactions_Label_Report_User
            // 
            Transactions_Label_Report_User.Dock = DockStyle.Fill;
            Transactions_Label_Report_User.Location = new Point(3, 232);
            Transactions_Label_Report_User.Name = "Transactions_Label_Report_User";
            Transactions_Label_Report_User.Size = new Size(100, 29);
            Transactions_Label_Report_User.TabIndex = 16;
            Transactions_Label_Report_User.Text = "User:";
            Transactions_Label_Report_User.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_TextBox_Report_User
            // 
            Transactions_TextBox_Report_User.Dock = DockStyle.Fill;
            Transactions_TextBox_Report_User.Location = new Point(109, 235);
            Transactions_TextBox_Report_User.Name = "Transactions_TextBox_Report_User";
            Transactions_TextBox_Report_User.ReadOnly = true;
            Transactions_TextBox_Report_User.Size = new Size(291, 23);
            Transactions_TextBox_Report_User.TabIndex = 17;
            // 
            // Transactions_Label_Report_ItemType
            // 
            Transactions_Label_Report_ItemType.Dock = DockStyle.Fill;
            Transactions_Label_Report_ItemType.Location = new Point(3, 261);
            Transactions_Label_Report_ItemType.Name = "Transactions_Label_Report_ItemType";
            Transactions_Label_Report_ItemType.Size = new Size(100, 29);
            Transactions_Label_Report_ItemType.TabIndex = 18;
            Transactions_Label_Report_ItemType.Text = "Item Type:";
            Transactions_Label_Report_ItemType.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_TextBox_Report_ItemType
            // 
            Transactions_TextBox_Report_ItemType.Dock = DockStyle.Fill;
            Transactions_TextBox_Report_ItemType.Location = new Point(109, 264);
            Transactions_TextBox_Report_ItemType.Name = "Transactions_TextBox_Report_ItemType";
            Transactions_TextBox_Report_ItemType.ReadOnly = true;
            Transactions_TextBox_Report_ItemType.Size = new Size(291, 23);
            Transactions_TextBox_Report_ItemType.TabIndex = 19;
            // 
            // Transactions_Label_Report_ReceiveDate
            // 
            Transactions_Label_Report_ReceiveDate.Dock = DockStyle.Fill;
            Transactions_Label_Report_ReceiveDate.Location = new Point(3, 290);
            Transactions_Label_Report_ReceiveDate.Name = "Transactions_Label_Report_ReceiveDate";
            Transactions_Label_Report_ReceiveDate.Size = new Size(100, 29);
            Transactions_Label_Report_ReceiveDate.TabIndex = 20;
            Transactions_Label_Report_ReceiveDate.Text = "Receive Date:";
            Transactions_Label_Report_ReceiveDate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transactions_TextBox_Report_ReceiveDate
            // 
            Transactions_TextBox_Report_ReceiveDate.Dock = DockStyle.Fill;
            Transactions_TextBox_Report_ReceiveDate.Location = new Point(109, 293);
            Transactions_TextBox_Report_ReceiveDate.Name = "Transactions_TextBox_Report_ReceiveDate";
            Transactions_TextBox_Report_ReceiveDate.ReadOnly = true;
            Transactions_TextBox_Report_ReceiveDate.Size = new Size(291, 23);
            Transactions_TextBox_Report_ReceiveDate.TabIndex = 21;
            // 
            // Transactions_TextBox_Report_TransactionType
            // 
            Transactions_TextBox_Report_TransactionType.Dock = DockStyle.Fill;
            Transactions_TextBox_Report_TransactionType.Location = new Point(109, 3);
            Transactions_TextBox_Report_TransactionType.Name = "Transactions_TextBox_Report_TransactionType";
            Transactions_TextBox_Report_TransactionType.ReadOnly = true;
            Transactions_TextBox_Report_TransactionType.Size = new Size(291, 23);
            Transactions_TextBox_Report_TransactionType.TabIndex = 1;
            // 
            // Transactions_TextBox_Report_BatchNumber
            // 
            Transactions_TextBox_Report_BatchNumber.Dock = DockStyle.Fill;
            Transactions_TextBox_Report_BatchNumber.Location = new Point(109, 32);
            Transactions_TextBox_Report_BatchNumber.Name = "Transactions_TextBox_Report_BatchNumber";
            Transactions_TextBox_Report_BatchNumber.ReadOnly = true;
            Transactions_TextBox_Report_BatchNumber.Size = new Size(291, 23);
            Transactions_TextBox_Report_BatchNumber.TabIndex = 3;
            // 
            // Transactions_Label_Report_TransactionType
            // 
            Transactions_Label_Report_TransactionType.Dock = DockStyle.Fill;
            Transactions_Label_Report_TransactionType.Location = new Point(3, 0);
            Transactions_Label_Report_TransactionType.Name = "Transactions_Label_Report_TransactionType";
            Transactions_Label_Report_TransactionType.Size = new Size(100, 29);
            Transactions_Label_Report_TransactionType.TabIndex = 0;
            Transactions_Label_Report_TransactionType.Text = "Transaction Type:";
            Transactions_Label_Report_TransactionType.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Transfer_TableLayout_Right
            // 
            Transfer_TableLayout_Right.ColumnCount = 1;
            Transfer_TableLayout_Right.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Transfer_TableLayout_Right.Controls.Add(Transactions_Panel_DataGridView, 0, 0);
            Transfer_TableLayout_Right.Controls.Add(Transfer_Panel_PageButtons, 0, 1);
            Transfer_TableLayout_Right.Dock = DockStyle.Fill;
            Transfer_TableLayout_Right.Location = new Point(0, 0);
            Transfer_TableLayout_Right.Name = "Transfer_TableLayout_Right";
            Transfer_TableLayout_Right.RowCount = 2;
            Transfer_TableLayout_Right.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Transfer_TableLayout_Right.RowStyles.Add(new RowStyle());
            Transfer_TableLayout_Right.Size = new Size(405, 524);
            Transfer_TableLayout_Right.TabIndex = 6;
            // 
            // Transactions_Panel_DataGridView
            // 
            Transactions_Panel_DataGridView.Controls.Add(Transactions_Image_NothingFound);
            Transactions_Panel_DataGridView.Controls.Add(Transactions_DataGridView_Transactions);
            Transactions_Panel_DataGridView.Dock = DockStyle.Fill;
            Transactions_Panel_DataGridView.Location = new Point(3, 3);
            Transactions_Panel_DataGridView.Name = "Transactions_Panel_DataGridView";
            Transactions_Panel_DataGridView.Size = new Size(399, 483);
            Transactions_Panel_DataGridView.TabIndex = 5;
            // 
            // Transactions_Image_NothingFound
            // 
            Transactions_Image_NothingFound.BackColor = Color.White;
            Transactions_Image_NothingFound.Dock = DockStyle.Fill;
            Transactions_Image_NothingFound.ErrorImage = null;
            Transactions_Image_NothingFound.Image = Properties.Resources.NothingFound;
            Transactions_Image_NothingFound.InitialImage = null;
            Transactions_Image_NothingFound.Location = new Point(0, 0);
            Transactions_Image_NothingFound.Name = "Transactions_Image_NothingFound";
            Transactions_Image_NothingFound.Size = new Size(399, 483);
            Transactions_Image_NothingFound.SizeMode = PictureBoxSizeMode.Zoom;
            Transactions_Image_NothingFound.TabIndex = 22;
            Transactions_Image_NothingFound.TabStop = false;
            Transactions_Image_NothingFound.Visible = false;
            // 
            // Transactions_DataGridView_Transactions
            // 
            Transactions_DataGridView_Transactions.Dock = DockStyle.Fill;
            Transactions_DataGridView_Transactions.Location = new Point(0, 0);
            Transactions_DataGridView_Transactions.Name = "Transactions_DataGridView_Transactions";
            Transactions_DataGridView_Transactions.Size = new Size(399, 483);
            Transactions_DataGridView_Transactions.TabIndex = 0;
            // 
            // Transfer_Panel_PageButtons
            // 
            Transfer_Panel_PageButtons.AutoSize = true;
            Transfer_Panel_PageButtons.Controls.Add(Transfer_Button_Next);
            Transfer_Panel_PageButtons.Controls.Add(Transfer_Button_Previous);
            Transfer_Panel_PageButtons.Dock = DockStyle.Fill;
            Transfer_Panel_PageButtons.Location = new Point(3, 492);
            Transfer_Panel_PageButtons.Name = "Transfer_Panel_PageButtons";
            Transfer_Panel_PageButtons.Size = new Size(399, 29);
            Transfer_Panel_PageButtons.TabIndex = 6;
            // 
            // Transfer_Button_SelectionHistory
            // 
            Transfer_Button_SelectionHistory.Enabled = false;
            Transfer_Button_SelectionHistory.Location = new Point(144, 3);
            Transfer_Button_SelectionHistory.Name = "Transfer_Button_SelectionHistory";
            Transfer_Button_SelectionHistory.Size = new Size(114, 23);
            Transfer_Button_SelectionHistory.TabIndex = 2;
            Transfer_Button_SelectionHistory.Text = "Selection History";
            Transfer_Button_SelectionHistory.UseVisualStyleBackColor = true;
            Transfer_Button_SelectionHistory.Click += Transfer_Button_BranchHistory_Click;
            // 
            // Transfer_Button_Next
            // 
            Transfer_Button_Next.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Transfer_Button_Next.Enabled = false;
            Transfer_Button_Next.Location = new Point(281, 3);
            Transfer_Button_Next.Name = "Transfer_Button_Next";
            Transfer_Button_Next.Size = new Size(114, 23);
            Transfer_Button_Next.TabIndex = 1;
            Transfer_Button_Next.Text = "Next Page";
            Transfer_Button_Next.UseVisualStyleBackColor = true;
            // 
            // Transfer_Button_Previous
            // 
            Transfer_Button_Previous.Enabled = false;
            Transfer_Button_Previous.Location = new Point(3, 3);
            Transfer_Button_Previous.Name = "Transfer_Button_Previous";
            Transfer_Button_Previous.Size = new Size(114, 23);
            Transfer_Button_Previous.TabIndex = 0;
            Transfer_Button_Previous.Text = "Previous Page";
            Transfer_Button_Previous.UseVisualStyleBackColor = true;
            // 
            // Transactions_TabelLayoutPanel_Bottom
            // 
            Transactions_TabelLayoutPanel_Bottom.ColumnCount = 2;
            Transactions_TabelLayoutPanel_Bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Transactions_TabelLayoutPanel_Bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Transactions_TabelLayoutPanel_Bottom.Controls.Add(Transactions_Panel_Bottom_Right, 1, 0);
            Transactions_TabelLayoutPanel_Bottom.Controls.Add(Transactions_Panel_Bottom_Left, 0, 0);
            Transactions_TabelLayoutPanel_Bottom.Dock = DockStyle.Fill;
            Transactions_TabelLayoutPanel_Bottom.Location = new Point(3, 533);
            Transactions_TabelLayoutPanel_Bottom.Name = "Transactions_TabelLayoutPanel_Bottom";
            Transactions_TabelLayoutPanel_Bottom.RowCount = 1;
            Transactions_TabelLayoutPanel_Bottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Transactions_TabelLayoutPanel_Bottom.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            Transactions_TabelLayoutPanel_Bottom.Size = new Size(818, 34);
            Transactions_TabelLayoutPanel_Bottom.TabIndex = 1;
            // 
            // Transactions_Panel_Bottom_Right
            // 
            Transactions_Panel_Bottom_Right.Controls.Add(Transfer_Button_SelectionHistory);
            Transactions_Panel_Bottom_Right.Controls.Add(Transactions_Button_Print);
            Transactions_Panel_Bottom_Right.Controls.Add(Transactions_Button_Reset);
            Transactions_Panel_Bottom_Right.Location = new Point(412, 3);
            Transactions_Panel_Bottom_Right.Name = "Transactions_Panel_Bottom_Right";
            Transactions_Panel_Bottom_Right.Size = new Size(403, 28);
            Transactions_Panel_Bottom_Right.TabIndex = 2;
            // 
            // Transactions_Button_Print
            // 
            Transactions_Button_Print.Location = new Point(3, 2);
            Transactions_Button_Print.Name = "Transactions_Button_Print";
            Transactions_Button_Print.Size = new Size(75, 23);
            Transactions_Button_Print.TabIndex = 12;
            Transactions_Button_Print.Text = "Print";
            // 
            // Transactions_Button_Reset
            // 
            Transactions_Button_Reset.Location = new Point(324, 2);
            Transactions_Button_Reset.Name = "Transactions_Button_Reset";
            Transactions_Button_Reset.Size = new Size(75, 23);
            Transactions_Button_Reset.TabIndex = 0;
            Transactions_Button_Reset.Text = "Reset";
            // 
            // Transactions_Panel_Bottom_Left
            // 
            Transactions_Panel_Bottom_Left.Controls.Add(Transactions_Button_Search);
            Transactions_Panel_Bottom_Left.Controls.Add(Transactions_Button_SidePanel);
            Transactions_Panel_Bottom_Left.Location = new Point(3, 3);
            Transactions_Panel_Bottom_Left.Name = "Transactions_Panel_Bottom_Left";
            Transactions_Panel_Bottom_Left.Size = new Size(403, 28);
            Transactions_Panel_Bottom_Left.TabIndex = 1;
            // 
            // Transactions_Button_Search
            // 
            Transactions_Button_Search.Enabled = false;
            Transactions_Button_Search.Location = new Point(3, 2);
            Transactions_Button_Search.Name = "Transactions_Button_Search";
            Transactions_Button_Search.Size = new Size(75, 23);
            Transactions_Button_Search.TabIndex = 1;
            Transactions_Button_Search.Text = "Search";
            // 
            // Transactions_Button_SidePanel
            // 
            Transactions_Button_SidePanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Transactions_Button_SidePanel.Location = new Point(320, 2);
            Transactions_Button_SidePanel.Name = "Transactions_Button_SidePanel";
            Transactions_Button_SidePanel.Size = new Size(80, 24);
            Transactions_Button_SidePanel.TabIndex = 11;
            Transactions_Button_SidePanel.Text = "Collapse ⬅️";
            Transactions_Button_SidePanel.UseVisualStyleBackColor = true;
            // 
            // Transactions
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(830, 592);
            Controls.Add(Transactions_GroupBox_Main);
            Name = "Transactions";
            Text = "Transactions";
            Transactions_GroupBox_Main.ResumeLayout(false);
            Transactions_TableLayout_Main.ResumeLayout(false);
            Transactions_SplitContainer_Main.Panel1.ResumeLayout(false);
            Transactions_SplitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Transactions_SplitContainer_Main).EndInit();
            Transactions_SplitContainer_Main.ResumeLayout(false);
            Transactions_TableLayout_Inputs.ResumeLayout(false);
            Transactions_Panel_Row_Building.ResumeLayout(false);
            Transactions_Panel_Row_SortBy.ResumeLayout(false);
            Transactions_Panel_Row_SearchPartID.ResumeLayout(false);
            Transactions_Panel_Row_User.ResumeLayout(false);
            Transactions_Panel_Row_Shift.ResumeLayout(false);
            Transactions_Panel_Row_DateRange.ResumeLayout(false);
            Transactions_TableLayout_SelectionReport.ResumeLayout(false);
            Transactions_TableLayout_SelectionReport.PerformLayout();
            Transfer_TableLayout_Right.ResumeLayout(false);
            Transfer_TableLayout_Right.PerformLayout();
            Transactions_Panel_DataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Transactions_Image_NothingFound).EndInit();
            ((System.ComponentModel.ISupportInitialize)Transactions_DataGridView_Transactions).EndInit();
            Transfer_Panel_PageButtons.ResumeLayout(false);
            Transactions_TabelLayoutPanel_Bottom.ResumeLayout(false);
            Transactions_Panel_Bottom_Right.ResumeLayout(false);
            Transactions_Panel_Bottom_Left.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox Transactions_Image_NothingFound;
        private Button Transactions_Button_SidePanel;
        private TableLayoutPanel Transactions_TabelLayoutPanel_Bottom;
        private Panel Transactions_Panel_Bottom_Right;
        private Button Transactions_Button_Search;
        private Panel Transactions_Panel_Row_DateRange;
        private Label Control_AdvancedRemove_Label_DateDash;
        private DateTimePicker Control_AdvancedRemove_DateTimePicker_To;
        private CheckBox Control_AdvancedRemove_CheckBox_Date;
        private DateTimePicker Control_AdvancedRemove_DateTimePicker_From;
        private Panel Transactions_Panel_Row_Building;
        private Label Transactions_Label_Building;
        private ComboBox Transactions_ComboBox_Building;
        private Button Transactions_Button_Print;
        private TableLayoutPanel Transactions_TableLayout_SelectionReport;
        private Label Transactions_Label_Report_TransactionType;
        private TextBox Transactions_TextBox_Report_TransactionType;
        private Label Transactions_Label_Report_BatchNumber;
        private TextBox Transactions_TextBox_Report_BatchNumber;
        private Label Transactions_Label_Report_PartID;
        private TextBox Transactions_TextBox_Report_PartID;
        private Label Transactions_Label_Report_FromLocation;
        private TextBox Transactions_TextBox_Report_FromLocation;
        private Label Transactions_Label_Report_ToLocation;
        private TextBox Transactions_TextBox_Report_ToLocation;
        private Label Transactions_Label_Report_Operation;
        private TextBox Transactions_TextBox_Report_Operation;
        private Label Transactions_Label_Report_Quantity;
        private TextBox Transactions_TextBox_Report_Quantity;
        private Label Transactions_Label_Report_Notes;
        private TextBox Transactions_TextBox_Notes;
        private Label Transactions_Label_Report_User;
        private TextBox Transactions_TextBox_Report_User;
        private Label Transactions_Label_Report_ItemType;
        private TextBox Transactions_TextBox_Report_ItemType;
        private Label Transactions_Label_Report_ReceiveDate;
        private TextBox Transactions_TextBox_Report_ReceiveDate;
        private TableLayoutPanel Transfer_TableLayout_Right;
        private Panel Transfer_Panel_PageButtons;
        private Button Transfer_Button_Next;
        private Button Transfer_Button_Previous;
        private Button Transfer_Button_SelectionHistory;
    }
}
