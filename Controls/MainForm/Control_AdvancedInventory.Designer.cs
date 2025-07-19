using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.MainForm
{
    partial class Control_AdvancedInventory
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private TabControl AdvancedInventory_TabControl;
        private TabPage AdvancedInventory_TabControl_Single;
        private Button AdvancedInventory_Single_Button_Save;
        private Button AdvancedInventory_Single_Button_Reset;
        private TabPage AdvancedInventory_TabControl_MultiLoc;
        private GroupBox AdvancedInventory_MultiLoc_GroupBox_Item;
        private Label AdvancedInventory_MultiLoc_Label_Part;
        private ComboBox AdvancedInventory_MultiLoc_ComboBox_Part;
        private Label AdvancedInventory_MultiLoc_Label_Op;
        private ComboBox AdvancedInventory_MultiLoc_ComboBox_Op;
        private Label AdvancedInventory_MultiLoc_Label_Qty;
        private TextBox AdvancedInventory_MultiLoc_TextBox_Qty;
        private Label AdvancedInventory_MultiLoc_Label_Notes;
        private RichTextBox AdvancedInventory_MultiLoc_RichTextBox_Notes;
        private Label AdvancedInventory_MultiLoc_Label_Loc;
        private ComboBox AdvancedInventory_MultiLoc_ComboBox_Loc;
        private Button AdvancedInventory_MultiLoc_Button_AddLoc;
        private GroupBox AdvancedInventory_MultiLoc_GroupBox_Preview;
        private ListView AdvancedInventory_MultiLoc_ListView_Preview;
        private Button AdvancedInventory_MultiLoc_Button_SaveAll;
        private Button AdvancedInventory_MultiLoc_Button_Reset;
        private TabPage AdvancedInventory_TabControl_Import;
        private TableLayoutPanel AdvancedInventory_Import_TableLayout;
        private Panel AdvancedInventory_Import_Panel_Middle;
        private DataGridView AdvancedInventory_Import_DataGridView;
        private Button AdvancedInventory_Import_Button_OpenExcel;
        private Button AdvancedInventory_Import_Button_ImportExcel;
        private Button AdvancedInventory_Import_Button_Save;
        private GroupBox AdvancedInventory_Single_GroupBox_Right;
        private ListView AdvancedInventory_Single_ListView;
        private Button AdvancedInventory_Single_Button_Normal;
        private Button AdvancedInventory_Multi_Button_Normal;
        private Button AdvancedInventory_Import_Button_Normal;
        private GroupBox AdvancedInventory_GroupBox_Main;
        private TableLayoutPanel AdvancedInventory_TableLayout_Single;
        private TableLayoutPanel AdvancedInventory_TableLayoutPanel_Multi;
        


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
            AdvancedInventory_TabControl = new TabControl();
            AdvancedInventory_TabControl_Single = new TabPage();
            AdvancedInventory_TableLayout_Single = new TableLayoutPanel();
            AdvancedInventory_Single_GroupBox_Right = new GroupBox();
            AdvancedInventory_Single_Button_Normal = new Button();
            AdvancedInventory_Single_ListView = new ListView();
            AdvancedInventory_Single_Button_Reset = new Button();
            AdvancedInventory_Single_Button_Save = new Button();
            AdvancedInventory_TabControl_MultiLoc = new TabPage();
            AdvancedInventory_TableLayoutPanel_Multi = new TableLayoutPanel();
            AdvancedInventory_MultiLoc_GroupBox_Preview = new GroupBox();
            AdvancedInventory_Multi_Button_Normal = new Button();
            AdvancedInventory_MultiLoc_ListView_Preview = new ListView();
            AdvancedInventory_MultiLoc_Button_SaveAll = new Button();
            AdvancedInventory_MultiLoc_Button_Reset = new Button();
            AdvancedInventory_MultiLoc_GroupBox_Item = new GroupBox();
            AdvancedInventory_MultiLoc_Label_Part = new Label();
            AdvancedInventory_MultiLoc_ComboBox_Part = new ComboBox();
            AdvancedInventory_MultiLoc_Label_Op = new Label();
            AdvancedInventory_MultiLoc_ComboBox_Op = new ComboBox();
            AdvancedInventory_MultiLoc_Label_Qty = new Label();
            AdvancedInventory_MultiLoc_TextBox_Qty = new TextBox();
            AdvancedInventory_MultiLoc_Label_Notes = new Label();
            AdvancedInventory_MultiLoc_RichTextBox_Notes = new RichTextBox();
            AdvancedInventory_MultiLoc_Label_Loc = new Label();
            AdvancedInventory_MultiLoc_ComboBox_Loc = new ComboBox();
            AdvancedInventory_MultiLoc_Button_AddLoc = new Button();
            AdvancedInventory_TabControl_Import = new TabPage();
            AdvancedInventory_Import_TableLayout = new TableLayoutPanel();
            AdvancedInventory_Import_Panel_Middle = new Panel();
            AdvancedInventory_Import_DataGridView = new DataGridView();
            AdvancedInventory_Import_Button_OpenExcel = new Button();
            AdvancedInventory_Import_Button_ImportExcel = new Button();
            AdvancedInventory_Import_Button_Normal = new Button();
            AdvancedInventory_Import_Button_Save = new Button();
            AdvancedInventory_GroupBox_Main = new GroupBox();
            AdvancedInventory_Single_RichTextBox_Notes = new RichTextBox();
            AdvancedInventory_Single_Label_Notes = new Label();
            AdvancedInventory_Single_TextBox_Count = new TextBox();
            AdvancedInventory_Single_Label_Count = new Label();
            AdvancedInventory_Single_TextBox_Qty = new TextBox();
            AdvancedInventory_Single_Label_Qty = new Label();
            AdvancedInventory_Single_ComboBox_Loc = new ComboBox();
            AdvancedInventory_Single_Label_Loc = new Label();
            AdvancedInventory_Single_ComboBox_Op = new ComboBox();
            AdvancedInventory_Single_Label_Op = new Label();
            AdvancedInventory_Single_ComboBox_Part = new ComboBox();
            AdvancedInventory_Single_Button_Send = new Button();
            AdvancedInventory_Single_Label_Part = new Label();
            AdvancedInventory_Single_GroupBox_Left = new GroupBox();
            AdvancedInventory_Single_TableLayout_Left = new TableLayoutPanel();
            AdvancedInventory_Single_TableLayout_Right = new TableLayoutPanel();
            AdvancedInventory_Single_TableLayout_LowerRight = new TableLayoutPanel();
            AdvancedInventory_Multi_TableLayout_Left = new TableLayoutPanel();
            AdvancedInventory_Multi_TableLayout_Right = new TableLayoutPanel();
            AdvancedInventory_Multi_TableLayout_BottomRight = new TableLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            AdvancedInventory_TabControl.SuspendLayout();
            AdvancedInventory_TabControl_Single.SuspendLayout();
            AdvancedInventory_TableLayout_Single.SuspendLayout();
            AdvancedInventory_Single_GroupBox_Right.SuspendLayout();
            AdvancedInventory_TabControl_MultiLoc.SuspendLayout();
            AdvancedInventory_TableLayoutPanel_Multi.SuspendLayout();
            AdvancedInventory_MultiLoc_GroupBox_Preview.SuspendLayout();
            AdvancedInventory_MultiLoc_GroupBox_Item.SuspendLayout();
            AdvancedInventory_TabControl_Import.SuspendLayout();
            AdvancedInventory_Import_TableLayout.SuspendLayout();
            AdvancedInventory_Import_Panel_Middle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AdvancedInventory_Import_DataGridView).BeginInit();
            AdvancedInventory_GroupBox_Main.SuspendLayout();
            AdvancedInventory_Single_GroupBox_Left.SuspendLayout();
            AdvancedInventory_Single_TableLayout_Left.SuspendLayout();
            AdvancedInventory_Single_TableLayout_Right.SuspendLayout();
            AdvancedInventory_Single_TableLayout_LowerRight.SuspendLayout();
            AdvancedInventory_Multi_TableLayout_Left.SuspendLayout();
            AdvancedInventory_Multi_TableLayout_Right.SuspendLayout();
            AdvancedInventory_Multi_TableLayout_BottomRight.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // AdvancedInventory_TabControl
            // 
            AdvancedInventory_TabControl.Controls.Add(AdvancedInventory_TabControl_Single);
            AdvancedInventory_TabControl.Controls.Add(AdvancedInventory_TabControl_MultiLoc);
            AdvancedInventory_TabControl.Controls.Add(AdvancedInventory_TabControl_Import);
            AdvancedInventory_TabControl.Dock = DockStyle.Fill;
            AdvancedInventory_TabControl.Location = new Point(3, 19);
            AdvancedInventory_TabControl.Name = "AdvancedInventory_TabControl";
            AdvancedInventory_TabControl.SelectedIndex = 0;
            AdvancedInventory_TabControl.Size = new Size(809, 362);
            AdvancedInventory_TabControl.TabIndex = 1;
            // 
            // AdvancedInventory_TabControl_Single
            // 
            AdvancedInventory_TabControl_Single.Controls.Add(AdvancedInventory_TableLayout_Single);
            AdvancedInventory_TabControl_Single.Location = new Point(4, 24);
            AdvancedInventory_TabControl_Single.Margin = new Padding(0);
            AdvancedInventory_TabControl_Single.Name = "AdvancedInventory_TabControl_Single";
            AdvancedInventory_TabControl_Single.Size = new Size(801, 334);
            AdvancedInventory_TabControl_Single.TabIndex = 0;
            AdvancedInventory_TabControl_Single.Text = "Single Item, Multiple Times";
            // 
            // AdvancedInventory_TableLayout_Single
            // 
            AdvancedInventory_TableLayout_Single.ColumnCount = 2;
            AdvancedInventory_TableLayout_Single.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            AdvancedInventory_TableLayout_Single.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            AdvancedInventory_TableLayout_Single.Controls.Add(AdvancedInventory_Single_GroupBox_Right, 1, 0);
            AdvancedInventory_TableLayout_Single.Controls.Add(AdvancedInventory_Single_GroupBox_Left, 0, 0);
            AdvancedInventory_TableLayout_Single.Dock = DockStyle.Fill;
            AdvancedInventory_TableLayout_Single.Location = new Point(0, 0);
            AdvancedInventory_TableLayout_Single.Margin = new Padding(0);
            AdvancedInventory_TableLayout_Single.Name = "AdvancedInventory_TableLayout_Single";
            AdvancedInventory_TableLayout_Single.RowCount = 1;
            AdvancedInventory_TableLayout_Single.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            AdvancedInventory_TableLayout_Single.Size = new Size(801, 334);
            AdvancedInventory_TableLayout_Single.TabIndex = 2;
            // 
            // AdvancedInventory_Single_GroupBox_Right
            // 
            AdvancedInventory_Single_GroupBox_Right.AutoSize = true;
            AdvancedInventory_Single_GroupBox_Right.Controls.Add(AdvancedInventory_Single_TableLayout_Right);
            AdvancedInventory_Single_GroupBox_Right.Dock = DockStyle.Fill;
            AdvancedInventory_Single_GroupBox_Right.Location = new Point(400, 0);
            AdvancedInventory_Single_GroupBox_Right.Margin = new Padding(0);
            AdvancedInventory_Single_GroupBox_Right.Name = "AdvancedInventory_Single_GroupBox_Right";
            AdvancedInventory_Single_GroupBox_Right.Size = new Size(401, 334);
            AdvancedInventory_Single_GroupBox_Right.TabIndex = 1;
            AdvancedInventory_Single_GroupBox_Right.TabStop = false;
            AdvancedInventory_Single_GroupBox_Right.Text = "Transaction Preview";
            // 
            // AdvancedInventory_Single_Button_Normal
            // 
            AdvancedInventory_Single_Button_Normal.AutoSize = true;
            AdvancedInventory_Single_Button_Normal.Dock = DockStyle.Fill;
            AdvancedInventory_Single_Button_Normal.ForeColor = Color.DarkRed;
            AdvancedInventory_Single_Button_Normal.Location = new Point(175, 3);
            AdvancedInventory_Single_Button_Normal.Name = "AdvancedInventory_Single_Button_Normal";
            AdvancedInventory_Single_Button_Normal.Size = new Size(211, 32);
            AdvancedInventory_Single_Button_Normal.TabIndex = 14;
            AdvancedInventory_Single_Button_Normal.TabStop = false;
            AdvancedInventory_Single_Button_Normal.Text = "Back to Normal";
            AdvancedInventory_Single_Button_Normal.Click += AdvancedInventory_Button_Normal_Click;
            // 
            // AdvancedInventory_Single_ListView
            // 
            AdvancedInventory_Single_ListView.Dock = DockStyle.Fill;
            AdvancedInventory_Single_ListView.FullRowSelect = true;
            AdvancedInventory_Single_ListView.GridLines = true;
            AdvancedInventory_Single_ListView.Location = new Point(3, 3);
            AdvancedInventory_Single_ListView.Name = "AdvancedInventory_Single_ListView";
            AdvancedInventory_Single_ListView.Size = new Size(389, 262);
            AdvancedInventory_Single_ListView.TabIndex = 0;
            AdvancedInventory_Single_ListView.UseCompatibleStateImageBehavior = false;
            AdvancedInventory_Single_ListView.View = View.Details;
            // 
            // AdvancedInventory_Single_Button_Reset
            // 
            AdvancedInventory_Single_Button_Reset.AutoSize = true;
            AdvancedInventory_Single_Button_Reset.Dock = DockStyle.Fill;
            AdvancedInventory_Single_Button_Reset.Location = new Point(89, 3);
            AdvancedInventory_Single_Button_Reset.Name = "AdvancedInventory_Single_Button_Reset";
            AdvancedInventory_Single_Button_Reset.Size = new Size(80, 32);
            AdvancedInventory_Single_Button_Reset.TabIndex = 13;
            AdvancedInventory_Single_Button_Reset.TabStop = false;
            AdvancedInventory_Single_Button_Reset.Text = "Reset";
            AdvancedInventory_Single_Button_Reset.Click += AdvancedInventory_Single_Button_Reset_Click;
            // 
            // AdvancedInventory_Single_Button_Save
            // 
            AdvancedInventory_Single_Button_Save.AutoSize = true;
            AdvancedInventory_Single_Button_Save.Dock = DockStyle.Fill;
            AdvancedInventory_Single_Button_Save.Location = new Point(3, 3);
            AdvancedInventory_Single_Button_Save.Name = "AdvancedInventory_Single_Button_Save";
            AdvancedInventory_Single_Button_Save.Size = new Size(80, 32);
            AdvancedInventory_Single_Button_Save.TabIndex = 7;
            AdvancedInventory_Single_Button_Save.Text = "Save All";
            AdvancedInventory_Single_Button_Save.Click += AdvancedInventory_Single_Button_Save_Click;
            // 
            // AdvancedInventory_TabControl_MultiLoc
            // 
            AdvancedInventory_TabControl_MultiLoc.Controls.Add(AdvancedInventory_TableLayoutPanel_Multi);
            AdvancedInventory_TabControl_MultiLoc.Location = new Point(4, 24);
            AdvancedInventory_TabControl_MultiLoc.Margin = new Padding(0);
            AdvancedInventory_TabControl_MultiLoc.Name = "AdvancedInventory_TabControl_MultiLoc";
            AdvancedInventory_TabControl_MultiLoc.Size = new Size(801, 334);
            AdvancedInventory_TabControl_MultiLoc.TabIndex = 1;
            AdvancedInventory_TabControl_MultiLoc.Text = "Same Item, Multiple Locations";
            // 
            // AdvancedInventory_TableLayoutPanel_Multi
            // 
            AdvancedInventory_TableLayoutPanel_Multi.ColumnCount = 2;
            AdvancedInventory_TableLayoutPanel_Multi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            AdvancedInventory_TableLayoutPanel_Multi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            AdvancedInventory_TableLayoutPanel_Multi.Controls.Add(AdvancedInventory_MultiLoc_GroupBox_Preview, 1, 0);
            AdvancedInventory_TableLayoutPanel_Multi.Controls.Add(AdvancedInventory_MultiLoc_GroupBox_Item, 0, 0);
            AdvancedInventory_TableLayoutPanel_Multi.Dock = DockStyle.Fill;
            AdvancedInventory_TableLayoutPanel_Multi.Location = new Point(0, 0);
            AdvancedInventory_TableLayoutPanel_Multi.Margin = new Padding(0);
            AdvancedInventory_TableLayoutPanel_Multi.Name = "AdvancedInventory_TableLayoutPanel_Multi";
            AdvancedInventory_TableLayoutPanel_Multi.RowCount = 1;
            AdvancedInventory_TableLayoutPanel_Multi.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            AdvancedInventory_TableLayoutPanel_Multi.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            AdvancedInventory_TableLayoutPanel_Multi.Size = new Size(801, 334);
            AdvancedInventory_TableLayoutPanel_Multi.TabIndex = 1;
            // 
            // AdvancedInventory_MultiLoc_GroupBox_Preview
            // 
            AdvancedInventory_MultiLoc_GroupBox_Preview.AutoSize = true;
            AdvancedInventory_MultiLoc_GroupBox_Preview.Controls.Add(AdvancedInventory_Multi_TableLayout_Right);
            AdvancedInventory_MultiLoc_GroupBox_Preview.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_GroupBox_Preview.Location = new Point(400, 0);
            AdvancedInventory_MultiLoc_GroupBox_Preview.Margin = new Padding(0);
            AdvancedInventory_MultiLoc_GroupBox_Preview.Name = "AdvancedInventory_MultiLoc_GroupBox_Preview";
            AdvancedInventory_MultiLoc_GroupBox_Preview.Size = new Size(401, 334);
            AdvancedInventory_MultiLoc_GroupBox_Preview.TabIndex = 0;
            AdvancedInventory_MultiLoc_GroupBox_Preview.TabStop = false;
            AdvancedInventory_MultiLoc_GroupBox_Preview.Text = "Transaction Preview";
            // 
            // AdvancedInventory_Multi_Button_Normal
            // 
            AdvancedInventory_Multi_Button_Normal.AutoSize = true;
            AdvancedInventory_Multi_Button_Normal.Dock = DockStyle.Fill;
            AdvancedInventory_Multi_Button_Normal.ForeColor = Color.DarkRed;
            AdvancedInventory_Multi_Button_Normal.Location = new Point(261, 3);
            AdvancedInventory_Multi_Button_Normal.Name = "AdvancedInventory_Multi_Button_Normal";
            AdvancedInventory_Multi_Button_Normal.Size = new Size(125, 32);
            AdvancedInventory_Multi_Button_Normal.TabIndex = 15;
            AdvancedInventory_Multi_Button_Normal.TabStop = false;
            AdvancedInventory_Multi_Button_Normal.Text = "Back to Normal";
            AdvancedInventory_Multi_Button_Normal.Click += AdvancedInventory_Button_Normal_Click;
            // 
            // AdvancedInventory_MultiLoc_ListView_Preview
            // 
            AdvancedInventory_MultiLoc_ListView_Preview.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_ListView_Preview.FullRowSelect = true;
            AdvancedInventory_MultiLoc_ListView_Preview.GridLines = true;
            AdvancedInventory_MultiLoc_ListView_Preview.Location = new Point(3, 3);
            AdvancedInventory_MultiLoc_ListView_Preview.Name = "AdvancedInventory_MultiLoc_ListView_Preview";
            AdvancedInventory_MultiLoc_ListView_Preview.Size = new Size(389, 262);
            AdvancedInventory_MultiLoc_ListView_Preview.TabIndex = 0;
            AdvancedInventory_MultiLoc_ListView_Preview.UseCompatibleStateImageBehavior = false;
            AdvancedInventory_MultiLoc_ListView_Preview.View = View.Details;
            // 
            // AdvancedInventory_MultiLoc_Button_SaveAll
            // 
            AdvancedInventory_MultiLoc_Button_SaveAll.AutoSize = true;
            AdvancedInventory_MultiLoc_Button_SaveAll.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_Button_SaveAll.Location = new Point(3, 3);
            AdvancedInventory_MultiLoc_Button_SaveAll.Name = "AdvancedInventory_MultiLoc_Button_SaveAll";
            AdvancedInventory_MultiLoc_Button_SaveAll.Size = new Size(123, 32);
            AdvancedInventory_MultiLoc_Button_SaveAll.TabIndex = 1;
            AdvancedInventory_MultiLoc_Button_SaveAll.TabStop = false;
            AdvancedInventory_MultiLoc_Button_SaveAll.Text = "Save All";
            AdvancedInventory_MultiLoc_Button_SaveAll.Click += AdvancedInventory_MultiLoc_Button_SaveAll_Click;
            // 
            // AdvancedInventory_MultiLoc_Button_Reset
            // 
            AdvancedInventory_MultiLoc_Button_Reset.AutoSize = true;
            AdvancedInventory_MultiLoc_Button_Reset.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_Button_Reset.Location = new Point(132, 3);
            AdvancedInventory_MultiLoc_Button_Reset.Name = "AdvancedInventory_MultiLoc_Button_Reset";
            AdvancedInventory_MultiLoc_Button_Reset.Size = new Size(123, 32);
            AdvancedInventory_MultiLoc_Button_Reset.TabIndex = 2;
            AdvancedInventory_MultiLoc_Button_Reset.TabStop = false;
            AdvancedInventory_MultiLoc_Button_Reset.Text = "Reset";
            AdvancedInventory_MultiLoc_Button_Reset.Click += AdvancedInventory_MultiLoc_Button_Reset_Click;
            // 
            // AdvancedInventory_MultiLoc_GroupBox_Item
            // 
            AdvancedInventory_MultiLoc_GroupBox_Item.Controls.Add(AdvancedInventory_Multi_TableLayout_Left);
            AdvancedInventory_MultiLoc_GroupBox_Item.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_GroupBox_Item.Location = new Point(0, 0);
            AdvancedInventory_MultiLoc_GroupBox_Item.Margin = new Padding(0);
            AdvancedInventory_MultiLoc_GroupBox_Item.Name = "AdvancedInventory_MultiLoc_GroupBox_Item";
            AdvancedInventory_MultiLoc_GroupBox_Item.Size = new Size(400, 334);
            AdvancedInventory_MultiLoc_GroupBox_Item.TabIndex = 0;
            AdvancedInventory_MultiLoc_GroupBox_Item.TabStop = false;
            AdvancedInventory_MultiLoc_GroupBox_Item.Text = "Item Entry";
            // 
            // AdvancedInventory_MultiLoc_Label_Part
            // 
            AdvancedInventory_MultiLoc_Label_Part.AutoSize = true;
            AdvancedInventory_MultiLoc_Label_Part.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_Label_Part.Location = new Point(3, 0);
            AdvancedInventory_MultiLoc_Label_Part.Name = "AdvancedInventory_MultiLoc_Label_Part";
            AdvancedInventory_MultiLoc_Label_Part.Size = new Size(56, 29);
            AdvancedInventory_MultiLoc_Label_Part.TabIndex = 0;
            AdvancedInventory_MultiLoc_Label_Part.Text = "Part:";
            AdvancedInventory_MultiLoc_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AdvancedInventory_MultiLoc_ComboBox_Part
            // 
            AdvancedInventory_MultiLoc_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AdvancedInventory_MultiLoc_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            AdvancedInventory_MultiLoc_ComboBox_Part.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_ComboBox_Part.Location = new Point(65, 3);
            AdvancedInventory_MultiLoc_ComboBox_Part.Name = "AdvancedInventory_MultiLoc_ComboBox_Part";
            AdvancedInventory_MultiLoc_ComboBox_Part.Size = new Size(326, 23);
            AdvancedInventory_MultiLoc_ComboBox_Part.TabIndex = 1;
            // 
            // AdvancedInventory_MultiLoc_Label_Op
            // 
            AdvancedInventory_MultiLoc_Label_Op.AutoSize = true;
            AdvancedInventory_MultiLoc_Label_Op.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_Label_Op.Location = new Point(3, 29);
            AdvancedInventory_MultiLoc_Label_Op.Name = "AdvancedInventory_MultiLoc_Label_Op";
            AdvancedInventory_MultiLoc_Label_Op.Size = new Size(56, 29);
            AdvancedInventory_MultiLoc_Label_Op.TabIndex = 2;
            AdvancedInventory_MultiLoc_Label_Op.Text = "Op:";
            AdvancedInventory_MultiLoc_Label_Op.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AdvancedInventory_MultiLoc_ComboBox_Op
            // 
            AdvancedInventory_MultiLoc_ComboBox_Op.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AdvancedInventory_MultiLoc_ComboBox_Op.AutoCompleteSource = AutoCompleteSource.ListItems;
            AdvancedInventory_MultiLoc_ComboBox_Op.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_ComboBox_Op.Location = new Point(65, 32);
            AdvancedInventory_MultiLoc_ComboBox_Op.Name = "AdvancedInventory_MultiLoc_ComboBox_Op";
            AdvancedInventory_MultiLoc_ComboBox_Op.Size = new Size(326, 23);
            AdvancedInventory_MultiLoc_ComboBox_Op.TabIndex = 2;
            // 
            // AdvancedInventory_MultiLoc_Label_Qty
            // 
            AdvancedInventory_MultiLoc_Label_Qty.AutoSize = true;
            AdvancedInventory_MultiLoc_Label_Qty.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_Label_Qty.Location = new Point(3, 58);
            AdvancedInventory_MultiLoc_Label_Qty.Name = "AdvancedInventory_MultiLoc_Label_Qty";
            AdvancedInventory_MultiLoc_Label_Qty.Size = new Size(56, 29);
            AdvancedInventory_MultiLoc_Label_Qty.TabIndex = 4;
            AdvancedInventory_MultiLoc_Label_Qty.Text = "Quantity:";
            AdvancedInventory_MultiLoc_Label_Qty.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AdvancedInventory_MultiLoc_TextBox_Qty
            // 
            AdvancedInventory_MultiLoc_TextBox_Qty.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_TextBox_Qty.Location = new Point(65, 61);
            AdvancedInventory_MultiLoc_TextBox_Qty.Name = "AdvancedInventory_MultiLoc_TextBox_Qty";
            AdvancedInventory_MultiLoc_TextBox_Qty.Size = new Size(326, 23);
            AdvancedInventory_MultiLoc_TextBox_Qty.TabIndex = 3;
            // 
            // AdvancedInventory_MultiLoc_Label_Notes
            // 
            AdvancedInventory_MultiLoc_Label_Notes.AutoSize = true;
            AdvancedInventory_MultiLoc_Label_Notes.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_Label_Notes.Location = new Point(3, 116);
            AdvancedInventory_MultiLoc_Label_Notes.Name = "AdvancedInventory_MultiLoc_Label_Notes";
            AdvancedInventory_MultiLoc_Label_Notes.Size = new Size(56, 158);
            AdvancedInventory_MultiLoc_Label_Notes.TabIndex = 6;
            AdvancedInventory_MultiLoc_Label_Notes.Text = "Notes:";
            AdvancedInventory_MultiLoc_Label_Notes.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AdvancedInventory_MultiLoc_RichTextBox_Notes
            // 
            AdvancedInventory_MultiLoc_RichTextBox_Notes.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_RichTextBox_Notes.Location = new Point(65, 119);
            AdvancedInventory_MultiLoc_RichTextBox_Notes.Name = "AdvancedInventory_MultiLoc_RichTextBox_Notes";
            AdvancedInventory_MultiLoc_RichTextBox_Notes.Size = new Size(326, 152);
            AdvancedInventory_MultiLoc_RichTextBox_Notes.TabIndex = 5;
            AdvancedInventory_MultiLoc_RichTextBox_Notes.Text = "";
            // 
            // AdvancedInventory_MultiLoc_Label_Loc
            // 
            AdvancedInventory_MultiLoc_Label_Loc.AutoSize = true;
            AdvancedInventory_MultiLoc_Label_Loc.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_Label_Loc.Location = new Point(3, 87);
            AdvancedInventory_MultiLoc_Label_Loc.Name = "AdvancedInventory_MultiLoc_Label_Loc";
            AdvancedInventory_MultiLoc_Label_Loc.Size = new Size(56, 29);
            AdvancedInventory_MultiLoc_Label_Loc.TabIndex = 8;
            AdvancedInventory_MultiLoc_Label_Loc.Text = "Location:";
            AdvancedInventory_MultiLoc_Label_Loc.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AdvancedInventory_MultiLoc_ComboBox_Loc
            // 
            AdvancedInventory_MultiLoc_ComboBox_Loc.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AdvancedInventory_MultiLoc_ComboBox_Loc.AutoCompleteSource = AutoCompleteSource.ListItems;
            AdvancedInventory_MultiLoc_ComboBox_Loc.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_ComboBox_Loc.Location = new Point(65, 90);
            AdvancedInventory_MultiLoc_ComboBox_Loc.Name = "AdvancedInventory_MultiLoc_ComboBox_Loc";
            AdvancedInventory_MultiLoc_ComboBox_Loc.Size = new Size(326, 23);
            AdvancedInventory_MultiLoc_ComboBox_Loc.TabIndex = 4;
            // 
            // AdvancedInventory_MultiLoc_Button_AddLoc
            // 
            AdvancedInventory_MultiLoc_Button_AddLoc.AutoSize = true;
            AdvancedInventory_Multi_TableLayout_Left.SetColumnSpan(AdvancedInventory_MultiLoc_Button_AddLoc, 2);
            AdvancedInventory_MultiLoc_Button_AddLoc.Dock = DockStyle.Fill;
            AdvancedInventory_MultiLoc_Button_AddLoc.Location = new Point(3, 277);
            AdvancedInventory_MultiLoc_Button_AddLoc.Name = "AdvancedInventory_MultiLoc_Button_AddLoc";
            AdvancedInventory_MultiLoc_Button_AddLoc.Size = new Size(388, 32);
            AdvancedInventory_MultiLoc_Button_AddLoc.TabIndex = 6;
            AdvancedInventory_MultiLoc_Button_AddLoc.Text = "Add Location";
            AdvancedInventory_MultiLoc_Button_AddLoc.Click += AdvancedInventory_MultiLoc_Button_AddLoc_Click;
            // 
            // AdvancedInventory_TabControl_Import
            // 
            AdvancedInventory_TabControl_Import.Controls.Add(AdvancedInventory_Import_TableLayout);
            AdvancedInventory_TabControl_Import.Location = new Point(4, 24);
            AdvancedInventory_TabControl_Import.Name = "AdvancedInventory_TabControl_Import";
            AdvancedInventory_TabControl_Import.Size = new Size(801, 334);
            AdvancedInventory_TabControl_Import.TabIndex = 2;
            AdvancedInventory_TabControl_Import.Text = "Import";
            // 
            // AdvancedInventory_Import_TableLayout
            // 
            AdvancedInventory_Import_TableLayout.ColumnCount = 1;
            AdvancedInventory_Import_TableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            AdvancedInventory_Import_TableLayout.Controls.Add(AdvancedInventory_Import_Panel_Middle, 0, 1);
            AdvancedInventory_Import_TableLayout.Controls.Add(tableLayoutPanel1, 0, 2);
            AdvancedInventory_Import_TableLayout.Controls.Add(tableLayoutPanel2, 0, 0);
            AdvancedInventory_Import_TableLayout.Dock = DockStyle.Fill;
            AdvancedInventory_Import_TableLayout.Location = new Point(0, 0);
            AdvancedInventory_Import_TableLayout.Name = "AdvancedInventory_Import_TableLayout";
            AdvancedInventory_Import_TableLayout.RowCount = 3;
            AdvancedInventory_Import_TableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            AdvancedInventory_Import_TableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            AdvancedInventory_Import_TableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            AdvancedInventory_Import_TableLayout.Size = new Size(801, 334);
            AdvancedInventory_Import_TableLayout.TabIndex = 6;
            // 
            // AdvancedInventory_Import_Panel_Middle
            // 
            AdvancedInventory_Import_Panel_Middle.Controls.Add(AdvancedInventory_Import_DataGridView);
            AdvancedInventory_Import_Panel_Middle.Dock = DockStyle.Fill;
            AdvancedInventory_Import_Panel_Middle.Location = new Point(3, 47);
            AdvancedInventory_Import_Panel_Middle.Name = "AdvancedInventory_Import_Panel_Middle";
            AdvancedInventory_Import_Panel_Middle.Size = new Size(795, 240);
            AdvancedInventory_Import_Panel_Middle.TabIndex = 2;
            // 
            // AdvancedInventory_Import_DataGridView
            // 
            AdvancedInventory_Import_DataGridView.Dock = DockStyle.Fill;
            AdvancedInventory_Import_DataGridView.Location = new Point(0, 0);
            AdvancedInventory_Import_DataGridView.Name = "AdvancedInventory_Import_DataGridView";
            AdvancedInventory_Import_DataGridView.Size = new Size(795, 240);
            AdvancedInventory_Import_DataGridView.TabIndex = 0;
            // 
            // AdvancedInventory_Import_Button_OpenExcel
            // 
            AdvancedInventory_Import_Button_OpenExcel.AutoSize = true;
            AdvancedInventory_Import_Button_OpenExcel.Dock = DockStyle.Fill;
            AdvancedInventory_Import_Button_OpenExcel.Location = new Point(702, 3);
            AdvancedInventory_Import_Button_OpenExcel.Name = "AdvancedInventory_Import_Button_OpenExcel";
            AdvancedInventory_Import_Button_OpenExcel.Size = new Size(90, 32);
            AdvancedInventory_Import_Button_OpenExcel.TabIndex = 0;
            AdvancedInventory_Import_Button_OpenExcel.Text = "Open Excel";
            AdvancedInventory_Import_Button_OpenExcel.Click += AdvancedInventory_Import_Button_OpenExcel_Click;
            // 
            // AdvancedInventory_Import_Button_ImportExcel
            // 
            AdvancedInventory_Import_Button_ImportExcel.AutoSize = true;
            AdvancedInventory_Import_Button_ImportExcel.Dock = DockStyle.Fill;
            AdvancedInventory_Import_Button_ImportExcel.Location = new Point(3, 3);
            AdvancedInventory_Import_Button_ImportExcel.Name = "AdvancedInventory_Import_Button_ImportExcel";
            AdvancedInventory_Import_Button_ImportExcel.Size = new Size(90, 32);
            AdvancedInventory_Import_Button_ImportExcel.TabIndex = 1;
            AdvancedInventory_Import_Button_ImportExcel.Text = "Import Excel";
            AdvancedInventory_Import_Button_ImportExcel.Click += AdvancedInventory_Import_Button_ImportExcel_Click;
            // 
            // AdvancedInventory_Import_Button_Normal
            // 
            AdvancedInventory_Import_Button_Normal.AutoSize = true;
            AdvancedInventory_Import_Button_Normal.Dock = DockStyle.Fill;
            AdvancedInventory_Import_Button_Normal.ForeColor = Color.DarkRed;
            AdvancedInventory_Import_Button_Normal.Location = new Point(671, 3);
            AdvancedInventory_Import_Button_Normal.Name = "AdvancedInventory_Import_Button_Normal";
            AdvancedInventory_Import_Button_Normal.Size = new Size(121, 32);
            AdvancedInventory_Import_Button_Normal.TabIndex = 15;
            AdvancedInventory_Import_Button_Normal.TabStop = false;
            AdvancedInventory_Import_Button_Normal.Text = "Back to Normal";
            AdvancedInventory_Import_Button_Normal.Click += AdvancedInventory_Button_Normal_Click;
            // 
            // AdvancedInventory_Import_Button_Save
            // 
            AdvancedInventory_Import_Button_Save.AutoSize = true;
            AdvancedInventory_Import_Button_Save.Dock = DockStyle.Fill;
            AdvancedInventory_Import_Button_Save.Location = new Point(3, 3);
            AdvancedInventory_Import_Button_Save.Name = "AdvancedInventory_Import_Button_Save";
            AdvancedInventory_Import_Button_Save.Size = new Size(90, 32);
            AdvancedInventory_Import_Button_Save.TabIndex = 4;
            AdvancedInventory_Import_Button_Save.Text = "Save";
            AdvancedInventory_Import_Button_Save.Click += AdvancedInventory_Import_Button_Save_Click;
            // 
            // AdvancedInventory_GroupBox_Main
            // 
            AdvancedInventory_GroupBox_Main.AutoSize = true;
            AdvancedInventory_GroupBox_Main.Controls.Add(AdvancedInventory_TabControl);
            AdvancedInventory_GroupBox_Main.Dock = DockStyle.Fill;
            AdvancedInventory_GroupBox_Main.Location = new Point(0, 0);
            AdvancedInventory_GroupBox_Main.Name = "AdvancedInventory_GroupBox_Main";
            AdvancedInventory_GroupBox_Main.Size = new Size(815, 384);
            AdvancedInventory_GroupBox_Main.TabIndex = 2;
            AdvancedInventory_GroupBox_Main.TabStop = false;
            AdvancedInventory_GroupBox_Main.Text = "Advanced Inventory Entry";
            // 
            // AdvancedInventory_Single_RichTextBox_Notes
            // 
            AdvancedInventory_Single_RichTextBox_Notes.Dock = DockStyle.Fill;
            AdvancedInventory_Single_RichTextBox_Notes.Location = new Point(84, 148);
            AdvancedInventory_Single_RichTextBox_Notes.Name = "AdvancedInventory_Single_RichTextBox_Notes";
            AdvancedInventory_Single_RichTextBox_Notes.Size = new Size(307, 119);
            AdvancedInventory_Single_RichTextBox_Notes.TabIndex = 5;
            AdvancedInventory_Single_RichTextBox_Notes.Text = "";
            // 
            // AdvancedInventory_Single_Label_Notes
            // 
            AdvancedInventory_Single_Label_Notes.AutoSize = true;
            AdvancedInventory_Single_Label_Notes.Dock = DockStyle.Fill;
            AdvancedInventory_Single_Label_Notes.Location = new Point(3, 145);
            AdvancedInventory_Single_Label_Notes.Name = "AdvancedInventory_Single_Label_Notes";
            AdvancedInventory_Single_Label_Notes.Size = new Size(75, 125);
            AdvancedInventory_Single_Label_Notes.TabIndex = 10;
            AdvancedInventory_Single_Label_Notes.Text = "Notes:";
            AdvancedInventory_Single_Label_Notes.TextAlign = ContentAlignment.TopRight;
            // 
            // AdvancedInventory_Single_TextBox_Count
            // 
            AdvancedInventory_Single_TextBox_Count.Dock = DockStyle.Fill;
            AdvancedInventory_Single_TextBox_Count.Location = new Point(84, 119);
            AdvancedInventory_Single_TextBox_Count.Name = "AdvancedInventory_Single_TextBox_Count";
            AdvancedInventory_Single_TextBox_Count.Size = new Size(307, 23);
            AdvancedInventory_Single_TextBox_Count.TabIndex = 6;
            // 
            // AdvancedInventory_Single_Label_Count
            // 
            AdvancedInventory_Single_Label_Count.AutoSize = true;
            AdvancedInventory_Single_Label_Count.Dock = DockStyle.Fill;
            AdvancedInventory_Single_Label_Count.Location = new Point(3, 116);
            AdvancedInventory_Single_Label_Count.Name = "AdvancedInventory_Single_Label_Count";
            AdvancedInventory_Single_Label_Count.Size = new Size(75, 29);
            AdvancedInventory_Single_Label_Count.TabIndex = 8;
            AdvancedInventory_Single_Label_Count.Text = "Transactions:";
            AdvancedInventory_Single_Label_Count.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AdvancedInventory_Single_TextBox_Qty
            // 
            AdvancedInventory_Single_TextBox_Qty.Dock = DockStyle.Fill;
            AdvancedInventory_Single_TextBox_Qty.Location = new Point(84, 61);
            AdvancedInventory_Single_TextBox_Qty.Name = "AdvancedInventory_Single_TextBox_Qty";
            AdvancedInventory_Single_TextBox_Qty.Size = new Size(307, 23);
            AdvancedInventory_Single_TextBox_Qty.TabIndex = 3;
            // 
            // AdvancedInventory_Single_Label_Qty
            // 
            AdvancedInventory_Single_Label_Qty.AutoSize = true;
            AdvancedInventory_Single_Label_Qty.Dock = DockStyle.Fill;
            AdvancedInventory_Single_Label_Qty.Location = new Point(3, 58);
            AdvancedInventory_Single_Label_Qty.Name = "AdvancedInventory_Single_Label_Qty";
            AdvancedInventory_Single_Label_Qty.Size = new Size(75, 29);
            AdvancedInventory_Single_Label_Qty.TabIndex = 6;
            AdvancedInventory_Single_Label_Qty.Text = "Quantity:";
            AdvancedInventory_Single_Label_Qty.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AdvancedInventory_Single_ComboBox_Loc
            // 
            AdvancedInventory_Single_ComboBox_Loc.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AdvancedInventory_Single_ComboBox_Loc.AutoCompleteSource = AutoCompleteSource.ListItems;
            AdvancedInventory_Single_ComboBox_Loc.Dock = DockStyle.Fill;
            AdvancedInventory_Single_ComboBox_Loc.Location = new Point(84, 90);
            AdvancedInventory_Single_ComboBox_Loc.Name = "AdvancedInventory_Single_ComboBox_Loc";
            AdvancedInventory_Single_ComboBox_Loc.Size = new Size(307, 23);
            AdvancedInventory_Single_ComboBox_Loc.TabIndex = 4;
            // 
            // AdvancedInventory_Single_Label_Loc
            // 
            AdvancedInventory_Single_Label_Loc.AutoSize = true;
            AdvancedInventory_Single_Label_Loc.Dock = DockStyle.Fill;
            AdvancedInventory_Single_Label_Loc.Location = new Point(3, 87);
            AdvancedInventory_Single_Label_Loc.Name = "AdvancedInventory_Single_Label_Loc";
            AdvancedInventory_Single_Label_Loc.Size = new Size(75, 29);
            AdvancedInventory_Single_Label_Loc.TabIndex = 4;
            AdvancedInventory_Single_Label_Loc.Text = "Location:";
            AdvancedInventory_Single_Label_Loc.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AdvancedInventory_Single_ComboBox_Op
            // 
            AdvancedInventory_Single_ComboBox_Op.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AdvancedInventory_Single_ComboBox_Op.AutoCompleteSource = AutoCompleteSource.ListItems;
            AdvancedInventory_Single_ComboBox_Op.Dock = DockStyle.Fill;
            AdvancedInventory_Single_ComboBox_Op.Location = new Point(84, 32);
            AdvancedInventory_Single_ComboBox_Op.Name = "AdvancedInventory_Single_ComboBox_Op";
            AdvancedInventory_Single_ComboBox_Op.Size = new Size(307, 23);
            AdvancedInventory_Single_ComboBox_Op.TabIndex = 2;
            // 
            // AdvancedInventory_Single_Label_Op
            // 
            AdvancedInventory_Single_Label_Op.AutoSize = true;
            AdvancedInventory_Single_Label_Op.Dock = DockStyle.Fill;
            AdvancedInventory_Single_Label_Op.Location = new Point(3, 29);
            AdvancedInventory_Single_Label_Op.Name = "AdvancedInventory_Single_Label_Op";
            AdvancedInventory_Single_Label_Op.Size = new Size(75, 29);
            AdvancedInventory_Single_Label_Op.TabIndex = 2;
            AdvancedInventory_Single_Label_Op.Text = "Op:";
            AdvancedInventory_Single_Label_Op.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AdvancedInventory_Single_ComboBox_Part
            // 
            AdvancedInventory_Single_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AdvancedInventory_Single_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            AdvancedInventory_Single_ComboBox_Part.Dock = DockStyle.Fill;
            AdvancedInventory_Single_ComboBox_Part.Location = new Point(84, 3);
            AdvancedInventory_Single_ComboBox_Part.Name = "AdvancedInventory_Single_ComboBox_Part";
            AdvancedInventory_Single_ComboBox_Part.Size = new Size(307, 23);
            AdvancedInventory_Single_ComboBox_Part.TabIndex = 1;
            // 
            // AdvancedInventory_Single_Button_Send
            // 
            AdvancedInventory_Single_Button_Send.AutoSize = true;
            AdvancedInventory_Single_TableLayout_Left.SetColumnSpan(AdvancedInventory_Single_Button_Send, 2);
            AdvancedInventory_Single_Button_Send.Dock = DockStyle.Fill;
            AdvancedInventory_Single_Button_Send.Location = new Point(3, 273);
            AdvancedInventory_Single_Button_Send.Name = "AdvancedInventory_Single_Button_Send";
            AdvancedInventory_Single_Button_Send.Size = new Size(388, 36);
            AdvancedInventory_Single_Button_Send.TabIndex = 1;
            AdvancedInventory_Single_Button_Send.TabStop = false;
            AdvancedInventory_Single_Button_Send.Text = "Send to Preview";
            AdvancedInventory_Single_Button_Send.Click += AdvancedInventory_Single_Button_Send_Click;
            // 
            // AdvancedInventory_Single_Label_Part
            // 
            AdvancedInventory_Single_Label_Part.AutoSize = true;
            AdvancedInventory_Single_Label_Part.Dock = DockStyle.Fill;
            AdvancedInventory_Single_Label_Part.Location = new Point(3, 0);
            AdvancedInventory_Single_Label_Part.Name = "AdvancedInventory_Single_Label_Part";
            AdvancedInventory_Single_Label_Part.Size = new Size(75, 29);
            AdvancedInventory_Single_Label_Part.TabIndex = 0;
            AdvancedInventory_Single_Label_Part.Text = "Part:";
            AdvancedInventory_Single_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AdvancedInventory_Single_GroupBox_Left
            // 
            AdvancedInventory_Single_GroupBox_Left.AutoSize = true;
            AdvancedInventory_Single_GroupBox_Left.Controls.Add(AdvancedInventory_Single_TableLayout_Left);
            AdvancedInventory_Single_GroupBox_Left.Dock = DockStyle.Fill;
            AdvancedInventory_Single_GroupBox_Left.Location = new Point(0, 0);
            AdvancedInventory_Single_GroupBox_Left.Margin = new Padding(0);
            AdvancedInventory_Single_GroupBox_Left.Name = "AdvancedInventory_Single_GroupBox_Left";
            AdvancedInventory_Single_GroupBox_Left.Size = new Size(400, 334);
            AdvancedInventory_Single_GroupBox_Left.TabIndex = 0;
            AdvancedInventory_Single_GroupBox_Left.TabStop = false;
            AdvancedInventory_Single_GroupBox_Left.Text = "Single Item to 1 Location Multiple Times";
            // 
            // AdvancedInventory_Single_TableLayout_Left
            // 
            AdvancedInventory_Single_TableLayout_Left.AutoSize = true;
            AdvancedInventory_Single_TableLayout_Left.ColumnCount = 2;
            AdvancedInventory_Single_TableLayout_Left.ColumnStyles.Add(new ColumnStyle());
            AdvancedInventory_Single_TableLayout_Left.ColumnStyles.Add(new ColumnStyle());
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_Label_Part, 0, 0);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_Button_Send, 1, 6);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_ComboBox_Part, 1, 0);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_Label_Op, 0, 1);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_Label_Qty, 0, 2);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_Label_Loc, 0, 3);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_ComboBox_Op, 1, 1);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_Label_Count, 0, 4);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_TextBox_Qty, 1, 2);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_ComboBox_Loc, 1, 3);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_RichTextBox_Notes, 1, 5);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_Label_Notes, 0, 5);
            AdvancedInventory_Single_TableLayout_Left.Controls.Add(AdvancedInventory_Single_TextBox_Count, 1, 4);
            AdvancedInventory_Single_TableLayout_Left.Dock = DockStyle.Fill;
            AdvancedInventory_Single_TableLayout_Left.Location = new Point(3, 19);
            AdvancedInventory_Single_TableLayout_Left.Name = "AdvancedInventory_Single_TableLayout_Left";
            AdvancedInventory_Single_TableLayout_Left.RowCount = 7;
            AdvancedInventory_Single_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Single_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Single_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Single_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Single_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Single_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Single_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Single_TableLayout_Left.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            AdvancedInventory_Single_TableLayout_Left.Size = new Size(394, 312);
            AdvancedInventory_Single_TableLayout_Left.TabIndex = 0;
            // 
            // AdvancedInventory_Single_TableLayout_Right
            // 
            AdvancedInventory_Single_TableLayout_Right.AutoSize = true;
            AdvancedInventory_Single_TableLayout_Right.ColumnCount = 1;
            AdvancedInventory_Single_TableLayout_Right.ColumnStyles.Add(new ColumnStyle());
            AdvancedInventory_Single_TableLayout_Right.Controls.Add(AdvancedInventory_Single_ListView, 0, 0);
            AdvancedInventory_Single_TableLayout_Right.Controls.Add(AdvancedInventory_Single_TableLayout_LowerRight, 0, 1);
            AdvancedInventory_Single_TableLayout_Right.Dock = DockStyle.Fill;
            AdvancedInventory_Single_TableLayout_Right.Location = new Point(3, 19);
            AdvancedInventory_Single_TableLayout_Right.Name = "AdvancedInventory_Single_TableLayout_Right";
            AdvancedInventory_Single_TableLayout_Right.RowCount = 2;
            AdvancedInventory_Single_TableLayout_Right.RowStyles.Add(new RowStyle());
            AdvancedInventory_Single_TableLayout_Right.RowStyles.Add(new RowStyle());
            AdvancedInventory_Single_TableLayout_Right.Size = new Size(395, 312);
            AdvancedInventory_Single_TableLayout_Right.TabIndex = 15;
            // 
            // AdvancedInventory_Single_TableLayout_LowerRight
            // 
            AdvancedInventory_Single_TableLayout_LowerRight.AutoSize = true;
            AdvancedInventory_Single_TableLayout_LowerRight.ColumnCount = 3;
            AdvancedInventory_Single_TableLayout_LowerRight.ColumnStyles.Add(new ColumnStyle());
            AdvancedInventory_Single_TableLayout_LowerRight.ColumnStyles.Add(new ColumnStyle());
            AdvancedInventory_Single_TableLayout_LowerRight.ColumnStyles.Add(new ColumnStyle());
            AdvancedInventory_Single_TableLayout_LowerRight.Controls.Add(AdvancedInventory_Single_Button_Save, 0, 0);
            AdvancedInventory_Single_TableLayout_LowerRight.Controls.Add(AdvancedInventory_Single_Button_Normal, 2, 0);
            AdvancedInventory_Single_TableLayout_LowerRight.Controls.Add(AdvancedInventory_Single_Button_Reset, 1, 0);
            AdvancedInventory_Single_TableLayout_LowerRight.Dock = DockStyle.Fill;
            AdvancedInventory_Single_TableLayout_LowerRight.Location = new Point(3, 271);
            AdvancedInventory_Single_TableLayout_LowerRight.Name = "AdvancedInventory_Single_TableLayout_LowerRight";
            AdvancedInventory_Single_TableLayout_LowerRight.RowCount = 1;
            AdvancedInventory_Single_TableLayout_LowerRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            AdvancedInventory_Single_TableLayout_LowerRight.Size = new Size(389, 38);
            AdvancedInventory_Single_TableLayout_LowerRight.TabIndex = 1;
            // 
            // AdvancedInventory_Multi_TableLayout_Left
            // 
            AdvancedInventory_Multi_TableLayout_Left.AutoSize = true;
            AdvancedInventory_Multi_TableLayout_Left.ColumnCount = 2;
            AdvancedInventory_Multi_TableLayout_Left.ColumnStyles.Add(new ColumnStyle());
            AdvancedInventory_Multi_TableLayout_Left.ColumnStyles.Add(new ColumnStyle());
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_Label_Part, 0, 0);
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_ComboBox_Part, 1, 0);
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_Label_Op, 0, 1);
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_ComboBox_Op, 1, 1);
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_Button_AddLoc, 1, 5);
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_Label_Qty, 0, 2);
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_Label_Notes, 0, 4);
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_TextBox_Qty, 1, 2);
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_Label_Loc, 0, 3);
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_RichTextBox_Notes, 1, 4);
            AdvancedInventory_Multi_TableLayout_Left.Controls.Add(AdvancedInventory_MultiLoc_ComboBox_Loc, 1, 3);
            AdvancedInventory_Multi_TableLayout_Left.Dock = DockStyle.Fill;
            AdvancedInventory_Multi_TableLayout_Left.Location = new Point(3, 19);
            AdvancedInventory_Multi_TableLayout_Left.Name = "AdvancedInventory_Multi_TableLayout_Left";
            AdvancedInventory_Multi_TableLayout_Left.RowCount = 6;
            AdvancedInventory_Multi_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Multi_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Multi_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Multi_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Multi_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Multi_TableLayout_Left.RowStyles.Add(new RowStyle());
            AdvancedInventory_Multi_TableLayout_Left.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            AdvancedInventory_Multi_TableLayout_Left.Size = new Size(394, 312);
            AdvancedInventory_Multi_TableLayout_Left.TabIndex = 0;
            // 
            // AdvancedInventory_Multi_TableLayout_Right
            // 
            AdvancedInventory_Multi_TableLayout_Right.AutoSize = true;
            AdvancedInventory_Multi_TableLayout_Right.ColumnCount = 1;
            AdvancedInventory_Multi_TableLayout_Right.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            AdvancedInventory_Multi_TableLayout_Right.Controls.Add(AdvancedInventory_Multi_TableLayout_BottomRight, 0, 1);
            AdvancedInventory_Multi_TableLayout_Right.Controls.Add(AdvancedInventory_MultiLoc_ListView_Preview, 0, 0);
            AdvancedInventory_Multi_TableLayout_Right.Dock = DockStyle.Fill;
            AdvancedInventory_Multi_TableLayout_Right.Location = new Point(3, 19);
            AdvancedInventory_Multi_TableLayout_Right.Name = "AdvancedInventory_Multi_TableLayout_Right";
            AdvancedInventory_Multi_TableLayout_Right.RowCount = 2;
            AdvancedInventory_Multi_TableLayout_Right.RowStyles.Add(new RowStyle());
            AdvancedInventory_Multi_TableLayout_Right.RowStyles.Add(new RowStyle());
            AdvancedInventory_Multi_TableLayout_Right.Size = new Size(395, 312);
            AdvancedInventory_Multi_TableLayout_Right.TabIndex = 16;
            // 
            // AdvancedInventory_Multi_TableLayout_BottomRight
            // 
            AdvancedInventory_Multi_TableLayout_BottomRight.AutoSize = true;
            AdvancedInventory_Multi_TableLayout_BottomRight.ColumnCount = 3;
            AdvancedInventory_Multi_TableLayout_BottomRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            AdvancedInventory_Multi_TableLayout_BottomRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            AdvancedInventory_Multi_TableLayout_BottomRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            AdvancedInventory_Multi_TableLayout_BottomRight.Controls.Add(AdvancedInventory_MultiLoc_Button_SaveAll, 0, 0);
            AdvancedInventory_Multi_TableLayout_BottomRight.Controls.Add(AdvancedInventory_Multi_Button_Normal, 2, 0);
            AdvancedInventory_Multi_TableLayout_BottomRight.Controls.Add(AdvancedInventory_MultiLoc_Button_Reset, 1, 0);
            AdvancedInventory_Multi_TableLayout_BottomRight.Dock = DockStyle.Fill;
            AdvancedInventory_Multi_TableLayout_BottomRight.Location = new Point(3, 271);
            AdvancedInventory_Multi_TableLayout_BottomRight.Name = "AdvancedInventory_Multi_TableLayout_BottomRight";
            AdvancedInventory_Multi_TableLayout_BottomRight.RowCount = 1;
            AdvancedInventory_Multi_TableLayout_BottomRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            AdvancedInventory_Multi_TableLayout_BottomRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            AdvancedInventory_Multi_TableLayout_BottomRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            AdvancedInventory_Multi_TableLayout_BottomRight.Size = new Size(389, 38);
            AdvancedInventory_Multi_TableLayout_BottomRight.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(AdvancedInventory_Import_Button_Normal, 2, 0);
            tableLayoutPanel1.Controls.Add(AdvancedInventory_Import_Button_Save, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 293);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(795, 38);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(AdvancedInventory_Import_Button_OpenExcel, 2, 0);
            tableLayoutPanel2.Controls.Add(AdvancedInventory_Import_Button_ImportExcel, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(795, 38);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // Control_AdvancedInventory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(AdvancedInventory_GroupBox_Main);
            Name = "Control_AdvancedInventory";
            Size = new Size(815, 384);
            AdvancedInventory_TabControl.ResumeLayout(false);
            AdvancedInventory_TabControl_Single.ResumeLayout(false);
            AdvancedInventory_TableLayout_Single.ResumeLayout(false);
            AdvancedInventory_TableLayout_Single.PerformLayout();
            AdvancedInventory_Single_GroupBox_Right.ResumeLayout(false);
            AdvancedInventory_Single_GroupBox_Right.PerformLayout();
            AdvancedInventory_TabControl_MultiLoc.ResumeLayout(false);
            AdvancedInventory_TableLayoutPanel_Multi.ResumeLayout(false);
            AdvancedInventory_TableLayoutPanel_Multi.PerformLayout();
            AdvancedInventory_MultiLoc_GroupBox_Preview.ResumeLayout(false);
            AdvancedInventory_MultiLoc_GroupBox_Preview.PerformLayout();
            AdvancedInventory_MultiLoc_GroupBox_Item.ResumeLayout(false);
            AdvancedInventory_MultiLoc_GroupBox_Item.PerformLayout();
            AdvancedInventory_TabControl_Import.ResumeLayout(false);
            AdvancedInventory_Import_TableLayout.ResumeLayout(false);
            AdvancedInventory_Import_TableLayout.PerformLayout();
            AdvancedInventory_Import_Panel_Middle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)AdvancedInventory_Import_DataGridView).EndInit();
            AdvancedInventory_GroupBox_Main.ResumeLayout(false);
            AdvancedInventory_Single_GroupBox_Left.ResumeLayout(false);
            AdvancedInventory_Single_GroupBox_Left.PerformLayout();
            AdvancedInventory_Single_TableLayout_Left.ResumeLayout(false);
            AdvancedInventory_Single_TableLayout_Left.PerformLayout();
            AdvancedInventory_Single_TableLayout_Right.ResumeLayout(false);
            AdvancedInventory_Single_TableLayout_Right.PerformLayout();
            AdvancedInventory_Single_TableLayout_LowerRight.ResumeLayout(false);
            AdvancedInventory_Single_TableLayout_LowerRight.PerformLayout();
            AdvancedInventory_Multi_TableLayout_Left.ResumeLayout(false);
            AdvancedInventory_Multi_TableLayout_Left.PerformLayout();
            AdvancedInventory_Multi_TableLayout_Right.ResumeLayout(false);
            AdvancedInventory_Multi_TableLayout_Right.PerformLayout();
            AdvancedInventory_Multi_TableLayout_BottomRight.ResumeLayout(false);
            AdvancedInventory_Multi_TableLayout_BottomRight.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private Label AdvancedInventory_Single_Label_Part;
        private Button AdvancedInventory_Single_Button_Send;
        private ComboBox AdvancedInventory_Single_ComboBox_Part;
        private Label AdvancedInventory_Single_Label_Op;
        private ComboBox AdvancedInventory_Single_ComboBox_Op;
        private RichTextBox AdvancedInventory_Single_RichTextBox_Notes;
        private Label AdvancedInventory_Single_Label_Loc;
        private Label AdvancedInventory_Single_Label_Notes;
        private ComboBox AdvancedInventory_Single_ComboBox_Loc;
        private TextBox AdvancedInventory_Single_TextBox_Count;
        private Label AdvancedInventory_Single_Label_Qty;
        private Label AdvancedInventory_Single_Label_Count;
        private TextBox AdvancedInventory_Single_TextBox_Qty;
        private GroupBox AdvancedInventory_Single_GroupBox_Left;
        private TableLayoutPanel AdvancedInventory_Single_TableLayout_Left;
        private TableLayoutPanel AdvancedInventory_Single_TableLayout_Right;
        private TableLayoutPanel AdvancedInventory_Single_TableLayout_LowerRight;
        private TableLayoutPanel AdvancedInventory_Multi_TableLayout_Left;
        private TableLayoutPanel AdvancedInventory_Multi_TableLayout_Right;
        private TableLayoutPanel AdvancedInventory_Multi_TableLayout_BottomRight;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
    }

        
        #endregion
    }
