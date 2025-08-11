using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.MainForm
{
    partial class Control_AdvancedRemove
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private System.Windows.Forms.GroupBox Control_AdvancedRemove_GroupBox_Main;
        private System.Windows.Forms.TableLayoutPanel Control_AdvancedRemove_TableLayout_Main;
        private System.Windows.Forms.Label Control_AdvancedRemove_Label_Op;
        private System.Windows.Forms.Label Control_AdvancedRemove_Label_Loc;
        private System.Windows.Forms.Label Control_AdvancedRemove_Label_Qty;
        private System.Windows.Forms.TextBox Control_AdvancedRemove_TextBox_QtyMin;
        private System.Windows.Forms.Label Control_AdvancedRemove_Label_QtyDash;
        private System.Windows.Forms.TextBox Control_AdvancedRemove_TextBox_QtyMax;
        private System.Windows.Forms.DateTimePicker Control_AdvancedRemove_DateTimePicker_From;
        private System.Windows.Forms.Label Control_AdvancedRemove_Label_DateDash;
        private System.Windows.Forms.DateTimePicker Control_AdvancedRemove_DateTimePicker_To;
        private System.Windows.Forms.Label Control_AdvancedRemove_Label_Notes;
        private System.Windows.Forms.TextBox Control_AdvancedRemove_TextBox_Notes;
        private System.Windows.Forms.DataGridView Control_AdvancedRemove_DataGridView_Results;
        private Panel Control_AdvancedRemove_Panel_Row4_Center;
        internal ComboBox Control_AdvancedRemove_ComboBox_User;
        private Label Control_AdvancedRemove_Label_User;
        private PictureBox Control_AdvancedRemove_Image_NothingFound;
        private CheckBox Control_AdvancedRemove_CheckBox_Date;
        private TableLayoutPanel Control_AdvancedRemove_TableLayout_Row4;
        private Button Control_AdvancedRemove_Button_Reset;
        private Button Control_AdvancedRemove_Button_Normal;
        private Button Control_AdvancedRemove_Button_Undo;
        private Button Control_AdvancedRemove_Button_Delete;
        private Button Control_AdvancedRemove_Button_Search;
        private Panel Control_AdvancedRemove_Panel_Top;
        private SplitContainer Control_AdvancedRemove_SplitContainer_Main;
        private TableLayoutPanel Control_AdvancedRemove_TableLayout_TopLeft;
        private Button Control_AdvancedRemove_Button_SidePanel;
        
        
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
            Control_AdvancedRemove_GroupBox_Main = new GroupBox();
            Control_AdvancedRemove_TableLayout_Main = new TableLayoutPanel();
            Control_AdvancedRemove_TableLayout_Row4 = new TableLayoutPanel();
            Control_AdvancedRemove_TableLayout_BottomRight = new TableLayoutPanel();
            Control_AdvancedRemove_Button_Reset = new Button();
            Control_AdvancedRemove_Button_Print = new Button();
            Control_AdvancedRemove_Button_Normal = new Button();
            Control_AdvancedRemove_TableLayout_BottomLeft = new TableLayoutPanel();
            Control_AdvancedRemove_Button_Search = new Button();
            Control_AdvancedRemove_Button_Undo = new Button();
            Control_AdvancedRemove_Button_SidePanel = new Button();
            Control_AdvancedRemove_Button_Delete = new Button();
            Control_AdvancedRemove_Panel_Top = new Panel();
            Control_AdvancedRemove_SplitContainer_Main = new SplitContainer();
            Control_AdvancedRemove_TableLayout_TopLeft = new TableLayoutPanel();
            Control_AdvancedRemove_TableLayout_DateRange = new TableLayoutPanel();
            Control_AdvancedRemove_DateTimePicker_To = new DateTimePicker();
            Control_AdvancedRemove_DateTimePicker_From = new DateTimePicker();
            Control_AdvancedRemove_Label_DateDash = new Label();
            Control_AdvancedRemove_TextBox_Location = new TextBox();
            Control_AdvancedRemove_TextBox_Part = new TextBox();
            Control_AdvancedRemove_Label_Loc = new Label();
            Control_AdvancedRemove_Label_Op = new Label();
            Control_AdvancedRemove_Label_User = new Label();
            Control_AdvancedRemove_Label_Notes = new Label();
            Control_AdvancedRemove_CheckBox_Date = new CheckBox();
            Control_AdvancedRemove_Label_Qty = new Label();
            Control_AdvancedRemove_ComboBox_User = new ComboBox();
            Control_AdvancedRemove_TextBox_Operation = new TextBox();
            Control_AdvancedRemove_Label_Part = new Label();
            Control_AdvancedRemove_TextBox_Notes = new TextBox();
            Control_AdvancedRemove_TableLayout_Quantity = new TableLayoutPanel();
            Control_AdvancedRemove_TextBox_QtyMin = new TextBox();
            Control_AdvancedRemove_TextBox_QtyMax = new TextBox();
            Control_AdvancedRemove_Label_QtyDash = new Label();
            Control_AdvancedRemove_Panel_Row4_Center = new Panel();
            Control_AdvancedRemove_Image_NothingFound = new PictureBox();
            Control_AdvancedRemove_DataGridView_Results = new DataGridView();
            Control_AdvancedRemove_GroupBox_Main.SuspendLayout();
            Control_AdvancedRemove_TableLayout_Main.SuspendLayout();
            Control_AdvancedRemove_TableLayout_Row4.SuspendLayout();
            Control_AdvancedRemove_TableLayout_BottomRight.SuspendLayout();
            Control_AdvancedRemove_TableLayout_BottomLeft.SuspendLayout();
            Control_AdvancedRemove_Panel_Top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_AdvancedRemove_SplitContainer_Main).BeginInit();
            Control_AdvancedRemove_SplitContainer_Main.Panel1.SuspendLayout();
            Control_AdvancedRemove_SplitContainer_Main.Panel2.SuspendLayout();
            Control_AdvancedRemove_SplitContainer_Main.SuspendLayout();
            Control_AdvancedRemove_TableLayout_TopLeft.SuspendLayout();
            Control_AdvancedRemove_TableLayout_DateRange.SuspendLayout();
            Control_AdvancedRemove_TableLayout_Quantity.SuspendLayout();
            Control_AdvancedRemove_Panel_Row4_Center.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_AdvancedRemove_Image_NothingFound).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Control_AdvancedRemove_DataGridView_Results).BeginInit();
            SuspendLayout();
            // 
            // Control_AdvancedRemove_GroupBox_Main
            // 
            Control_AdvancedRemove_GroupBox_Main.Controls.Add(Control_AdvancedRemove_TableLayout_Main);
            Control_AdvancedRemove_GroupBox_Main.Dock = DockStyle.Fill;
            Control_AdvancedRemove_GroupBox_Main.Location = new Point(0, 0);
            Control_AdvancedRemove_GroupBox_Main.Name = "Control_AdvancedRemove_GroupBox_Main";
            Control_AdvancedRemove_GroupBox_Main.Size = new Size(800, 400);
            Control_AdvancedRemove_GroupBox_Main.TabIndex = 0;
            Control_AdvancedRemove_GroupBox_Main.TabStop = false;
            Control_AdvancedRemove_GroupBox_Main.Text = "Advanced Inventory Removal";
            // 
            // Control_AdvancedRemove_TableLayout_Main
            // 
            Control_AdvancedRemove_TableLayout_Main.ColumnCount = 1;
            Control_AdvancedRemove_TableLayout_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_Main.Controls.Add(Control_AdvancedRemove_TableLayout_Row4, 0, 5);
            Control_AdvancedRemove_TableLayout_Main.Controls.Add(Control_AdvancedRemove_Panel_Top, 0, 4);
            Control_AdvancedRemove_TableLayout_Main.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TableLayout_Main.Location = new Point(3, 19);
            Control_AdvancedRemove_TableLayout_Main.Name = "Control_AdvancedRemove_TableLayout_Main";
            Control_AdvancedRemove_TableLayout_Main.RowCount = 6;
            Control_AdvancedRemove_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_Main.Size = new Size(794, 378);
            Control_AdvancedRemove_TableLayout_Main.TabIndex = 1;
            // 
            // Control_AdvancedRemove_TableLayout_Row4
            // 
            Control_AdvancedRemove_TableLayout_Row4.ColumnCount = 2;
            Control_AdvancedRemove_TableLayout_Row4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Control_AdvancedRemove_TableLayout_Row4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Control_AdvancedRemove_TableLayout_Row4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            Control_AdvancedRemove_TableLayout_Row4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            Control_AdvancedRemove_TableLayout_Row4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            Control_AdvancedRemove_TableLayout_Row4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            Control_AdvancedRemove_TableLayout_Row4.Controls.Add(Control_AdvancedRemove_TableLayout_BottomRight, 1, 0);
            Control_AdvancedRemove_TableLayout_Row4.Controls.Add(Control_AdvancedRemove_TableLayout_BottomLeft, 0, 0);
            Control_AdvancedRemove_TableLayout_Row4.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TableLayout_Row4.Location = new Point(3, 341);
            Control_AdvancedRemove_TableLayout_Row4.Name = "Control_AdvancedRemove_TableLayout_Row4";
            Control_AdvancedRemove_TableLayout_Row4.RowCount = 1;
            Control_AdvancedRemove_TableLayout_Row4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_Row4.Size = new Size(788, 34);
            Control_AdvancedRemove_TableLayout_Row4.TabIndex = 3;
            // 
            // Control_AdvancedRemove_TableLayout_BottomRight
            // 
            Control_AdvancedRemove_TableLayout_BottomRight.AutoSize = true;
            Control_AdvancedRemove_TableLayout_BottomRight.ColumnCount = 4;
            Control_AdvancedRemove_TableLayout_BottomRight.ColumnStyles.Add(new ColumnStyle());
            Control_AdvancedRemove_TableLayout_BottomRight.ColumnStyles.Add(new ColumnStyle());
            Control_AdvancedRemove_TableLayout_BottomRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_BottomRight.ColumnStyles.Add(new ColumnStyle());
            Control_AdvancedRemove_TableLayout_BottomRight.Controls.Add(Control_AdvancedRemove_Button_Reset, 0, 0);
            Control_AdvancedRemove_TableLayout_BottomRight.Controls.Add(Control_AdvancedRemove_Button_Print, 1, 0);
            Control_AdvancedRemove_TableLayout_BottomRight.Controls.Add(Control_AdvancedRemove_Button_Normal, 4, 0);
            Control_AdvancedRemove_TableLayout_BottomRight.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TableLayout_BottomRight.Location = new Point(397, 3);
            Control_AdvancedRemove_TableLayout_BottomRight.Name = "Control_AdvancedRemove_TableLayout_BottomRight";
            Control_AdvancedRemove_TableLayout_BottomRight.RowCount = 1;
            Control_AdvancedRemove_TableLayout_BottomRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_BottomRight.Size = new Size(388, 28);
            Control_AdvancedRemove_TableLayout_BottomRight.TabIndex = 1;
            // 
            // Control_AdvancedRemove_Button_Reset
            // 
            Control_AdvancedRemove_Button_Reset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_AdvancedRemove_Button_Reset.AutoSize = true;
            Control_AdvancedRemove_Button_Reset.Location = new Point(3, 3);
            Control_AdvancedRemove_Button_Reset.Name = "Control_AdvancedRemove_Button_Reset";
            Control_AdvancedRemove_Button_Reset.Size = new Size(80, 22);
            Control_AdvancedRemove_Button_Reset.TabIndex = 1;
            Control_AdvancedRemove_Button_Reset.Text = "Reset";
            // 
            // Control_AdvancedRemove_Button_Print
            // 
            Control_AdvancedRemove_Button_Print.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_AdvancedRemove_Button_Print.AutoSize = true;
            Control_AdvancedRemove_Button_Print.Enabled = false;
            Control_AdvancedRemove_Button_Print.Location = new Point(89, 3);
            Control_AdvancedRemove_Button_Print.Name = "Control_AdvancedRemove_Button_Print";
            Control_AdvancedRemove_Button_Print.Size = new Size(80, 22);
            Control_AdvancedRemove_Button_Print.TabIndex = 16;
            Control_AdvancedRemove_Button_Print.Text = "Print";
            // 
            // Control_AdvancedRemove_Button_Normal
            // 
            Control_AdvancedRemove_Button_Normal.AutoSize = true;
            Control_AdvancedRemove_Button_Normal.ForeColor = Color.DarkRed;
            Control_AdvancedRemove_Button_Normal.Location = new Point(264, 3);
            Control_AdvancedRemove_Button_Normal.Name = "Control_AdvancedRemove_Button_Normal";
            Control_AdvancedRemove_Button_Normal.Size = new Size(121, 22);
            Control_AdvancedRemove_Button_Normal.TabIndex = 15;
            Control_AdvancedRemove_Button_Normal.TabStop = false;
            Control_AdvancedRemove_Button_Normal.Text = "Back to Normal";
            // 
            // Control_AdvancedRemove_TableLayout_BottomLeft
            // 
            Control_AdvancedRemove_TableLayout_BottomLeft.AutoSize = true;
            Control_AdvancedRemove_TableLayout_BottomLeft.ColumnCount = 5;
            Control_AdvancedRemove_TableLayout_BottomLeft.ColumnStyles.Add(new ColumnStyle());
            Control_AdvancedRemove_TableLayout_BottomLeft.ColumnStyles.Add(new ColumnStyle());
            Control_AdvancedRemove_TableLayout_BottomLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_BottomLeft.ColumnStyles.Add(new ColumnStyle());
            Control_AdvancedRemove_TableLayout_BottomLeft.ColumnStyles.Add(new ColumnStyle());
            Control_AdvancedRemove_TableLayout_BottomLeft.Controls.Add(Control_AdvancedRemove_Button_Search, 0, 0);
            Control_AdvancedRemove_TableLayout_BottomLeft.Controls.Add(Control_AdvancedRemove_Button_Undo, 1, 0);
            Control_AdvancedRemove_TableLayout_BottomLeft.Controls.Add(Control_AdvancedRemove_Button_SidePanel, 4, 0);
            Control_AdvancedRemove_TableLayout_BottomLeft.Controls.Add(Control_AdvancedRemove_Button_Delete, 3, 0);
            Control_AdvancedRemove_TableLayout_BottomLeft.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TableLayout_BottomLeft.Location = new Point(3, 3);
            Control_AdvancedRemove_TableLayout_BottomLeft.Name = "Control_AdvancedRemove_TableLayout_BottomLeft";
            Control_AdvancedRemove_TableLayout_BottomLeft.RowCount = 1;
            Control_AdvancedRemove_TableLayout_BottomLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_BottomLeft.Size = new Size(388, 28);
            Control_AdvancedRemove_TableLayout_BottomLeft.TabIndex = 0;
            // 
            // Control_AdvancedRemove_Button_Search
            // 
            Control_AdvancedRemove_Button_Search.AutoSize = true;
            Control_AdvancedRemove_Button_Search.Location = new Point(3, 3);
            Control_AdvancedRemove_Button_Search.Name = "Control_AdvancedRemove_Button_Search";
            Control_AdvancedRemove_Button_Search.Size = new Size(80, 22);
            Control_AdvancedRemove_Button_Search.TabIndex = 11;
            Control_AdvancedRemove_Button_Search.Text = "Search";
            // 
            // Control_AdvancedRemove_Button_Undo
            // 
            Control_AdvancedRemove_Button_Undo.AutoSize = true;
            Control_AdvancedRemove_Button_Undo.Enabled = false;
            Control_AdvancedRemove_Button_Undo.Location = new Point(89, 3);
            Control_AdvancedRemove_Button_Undo.Name = "Control_AdvancedRemove_Button_Undo";
            Control_AdvancedRemove_Button_Undo.Size = new Size(80, 22);
            Control_AdvancedRemove_Button_Undo.TabIndex = 2;
            Control_AdvancedRemove_Button_Undo.Text = "Undo";
            // 
            // Control_AdvancedRemove_Button_SidePanel
            // 
            Control_AdvancedRemove_Button_SidePanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_AdvancedRemove_Button_SidePanel.AutoSize = true;
            Control_AdvancedRemove_Button_SidePanel.Location = new Point(305, 3);
            Control_AdvancedRemove_Button_SidePanel.Name = "Control_AdvancedRemove_Button_SidePanel";
            Control_AdvancedRemove_Button_SidePanel.Size = new Size(80, 22);
            Control_AdvancedRemove_Button_SidePanel.TabIndex = 10;
            Control_AdvancedRemove_Button_SidePanel.Text = "Collapse ⬅️";
            Control_AdvancedRemove_Button_SidePanel.UseVisualStyleBackColor = true;
            Control_AdvancedRemove_Button_SidePanel.Click += Control_AdvancedRemove_Button_SidePanel_Click;
            // 
            // Control_AdvancedRemove_Button_Delete
            // 
            Control_AdvancedRemove_Button_Delete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_AdvancedRemove_Button_Delete.AutoSize = true;
            Control_AdvancedRemove_Button_Delete.Location = new Point(219, 3);
            Control_AdvancedRemove_Button_Delete.Name = "Control_AdvancedRemove_Button_Delete";
            Control_AdvancedRemove_Button_Delete.Size = new Size(80, 22);
            Control_AdvancedRemove_Button_Delete.TabIndex = 1;
            Control_AdvancedRemove_Button_Delete.Text = "Delete";
            // 
            // Control_AdvancedRemove_Panel_Top
            // 
            Control_AdvancedRemove_Panel_Top.Controls.Add(Control_AdvancedRemove_SplitContainer_Main);
            Control_AdvancedRemove_Panel_Top.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Top.Location = new Point(3, 3);
            Control_AdvancedRemove_Panel_Top.Name = "Control_AdvancedRemove_Panel_Top";
            Control_AdvancedRemove_Panel_Top.Size = new Size(788, 332);
            Control_AdvancedRemove_Panel_Top.TabIndex = 4;
            // 
            // Control_AdvancedRemove_SplitContainer_Main
            // 
            Control_AdvancedRemove_SplitContainer_Main.Dock = DockStyle.Fill;
            Control_AdvancedRemove_SplitContainer_Main.Location = new Point(0, 0);
            Control_AdvancedRemove_SplitContainer_Main.Name = "Control_AdvancedRemove_SplitContainer_Main";
            // 
            // Control_AdvancedRemove_SplitContainer_Main.Panel1
            // 
            Control_AdvancedRemove_SplitContainer_Main.Panel1.Controls.Add(Control_AdvancedRemove_TableLayout_TopLeft);
            Control_AdvancedRemove_SplitContainer_Main.Panel1MinSize = 0;
            // 
            // Control_AdvancedRemove_SplitContainer_Main.Panel2
            // 
            Control_AdvancedRemove_SplitContainer_Main.Panel2.Controls.Add(Control_AdvancedRemove_Panel_Row4_Center);
            Control_AdvancedRemove_SplitContainer_Main.Panel2MinSize = 400;
            Control_AdvancedRemove_SplitContainer_Main.Size = new Size(788, 332);
            Control_AdvancedRemove_SplitContainer_Main.SplitterDistance = 384;
            Control_AdvancedRemove_SplitContainer_Main.TabIndex = 0;
            // 
            // Control_AdvancedRemove_TableLayout_TopLeft
            // 
            Control_AdvancedRemove_TableLayout_TopLeft.ColumnCount = 2;
            Control_AdvancedRemove_TableLayout_TopLeft.ColumnStyles.Add(new ColumnStyle());
            Control_AdvancedRemove_TableLayout_TopLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_TableLayout_DateRange, 1, 6);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_TextBox_Location, 1, 1);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_TextBox_Part, 1, 0);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_Label_Loc, 0, 1);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_Label_Op, 0, 2);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_Label_User, 0, 3);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_Label_Notes, 0, 4);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_CheckBox_Date, 0, 6);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_Label_Qty, 0, 5);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_ComboBox_User, 1, 3);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_TextBox_Operation, 1, 2);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_Label_Part, 0, 0);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_TextBox_Notes, 1, 4);
            Control_AdvancedRemove_TableLayout_TopLeft.Controls.Add(Control_AdvancedRemove_TableLayout_Quantity, 1, 5);
            Control_AdvancedRemove_TableLayout_TopLeft.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TableLayout_TopLeft.Location = new Point(0, 0);
            Control_AdvancedRemove_TableLayout_TopLeft.Name = "Control_AdvancedRemove_TableLayout_TopLeft";
            Control_AdvancedRemove_TableLayout_TopLeft.RowCount = 9;
            Control_AdvancedRemove_TableLayout_TopLeft.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_TopLeft.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_TopLeft.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_TopLeft.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_TopLeft.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_TopLeft.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_TopLeft.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_TopLeft.RowStyles.Add(new RowStyle());
            Control_AdvancedRemove_TableLayout_TopLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_TopLeft.Size = new Size(384, 332);
            Control_AdvancedRemove_TableLayout_TopLeft.TabIndex = 0;
            // 
            // Control_AdvancedRemove_TableLayout_DateRange
            // 
            Control_AdvancedRemove_TableLayout_DateRange.AutoSize = true;
            Control_AdvancedRemove_TableLayout_DateRange.ColumnCount = 3;
            Control_AdvancedRemove_TableLayout_DateRange.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Control_AdvancedRemove_TableLayout_DateRange.ColumnStyles.Add(new ColumnStyle());
            Control_AdvancedRemove_TableLayout_DateRange.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Control_AdvancedRemove_TableLayout_DateRange.Controls.Add(Control_AdvancedRemove_DateTimePicker_To, 2, 0);
            Control_AdvancedRemove_TableLayout_DateRange.Controls.Add(Control_AdvancedRemove_DateTimePicker_From, 0, 0);
            Control_AdvancedRemove_TableLayout_DateRange.Controls.Add(Control_AdvancedRemove_Label_DateDash, 1, 0);
            Control_AdvancedRemove_TableLayout_DateRange.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TableLayout_DateRange.Location = new Point(98, 183);
            Control_AdvancedRemove_TableLayout_DateRange.Name = "Control_AdvancedRemove_TableLayout_DateRange";
            Control_AdvancedRemove_TableLayout_DateRange.RowCount = 1;
            Control_AdvancedRemove_TableLayout_DateRange.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_DateRange.Size = new Size(283, 29);
            Control_AdvancedRemove_TableLayout_DateRange.TabIndex = 26;
            // 
            // Control_AdvancedRemove_DateTimePicker_To
            // 
            Control_AdvancedRemove_DateTimePicker_To.Dock = DockStyle.Fill;
            Control_AdvancedRemove_DateTimePicker_To.Format = DateTimePickerFormat.Short;
            Control_AdvancedRemove_DateTimePicker_To.Location = new Point(153, 3);
            Control_AdvancedRemove_DateTimePicker_To.Name = "Control_AdvancedRemove_DateTimePicker_To";
            Control_AdvancedRemove_DateTimePicker_To.Size = new Size(127, 23);
            Control_AdvancedRemove_DateTimePicker_To.TabIndex = 10;
            // 
            // Control_AdvancedRemove_DateTimePicker_From
            // 
            Control_AdvancedRemove_DateTimePicker_From.Dock = DockStyle.Fill;
            Control_AdvancedRemove_DateTimePicker_From.Format = DateTimePickerFormat.Short;
            Control_AdvancedRemove_DateTimePicker_From.Location = new Point(3, 3);
            Control_AdvancedRemove_DateTimePicker_From.Name = "Control_AdvancedRemove_DateTimePicker_From";
            Control_AdvancedRemove_DateTimePicker_From.Size = new Size(126, 23);
            Control_AdvancedRemove_DateTimePicker_From.TabIndex = 9;
            // 
            // Control_AdvancedRemove_Label_DateDash
            // 
            Control_AdvancedRemove_Label_DateDash.AutoSize = true;
            Control_AdvancedRemove_Label_DateDash.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Label_DateDash.Location = new Point(135, 0);
            Control_AdvancedRemove_Label_DateDash.Name = "Control_AdvancedRemove_Label_DateDash";
            Control_AdvancedRemove_Label_DateDash.Size = new Size(12, 29);
            Control_AdvancedRemove_Label_DateDash.TabIndex = 1;
            Control_AdvancedRemove_Label_DateDash.Text = "-";
            Control_AdvancedRemove_Label_DateDash.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Control_AdvancedRemove_TextBox_Location
            // 
            Control_AdvancedRemove_TextBox_Location.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TextBox_Location.Location = new Point(98, 32);
            Control_AdvancedRemove_TextBox_Location.Name = "Control_AdvancedRemove_TextBox_Location";
            Control_AdvancedRemove_TextBox_Location.Size = new Size(283, 23);
            Control_AdvancedRemove_TextBox_Location.TabIndex = 2;
            // 
            // Control_AdvancedRemove_TextBox_Part
            // 
            Control_AdvancedRemove_TextBox_Part.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TextBox_Part.Location = new Point(98, 3);
            Control_AdvancedRemove_TextBox_Part.Name = "Control_AdvancedRemove_TextBox_Part";
            Control_AdvancedRemove_TextBox_Part.Size = new Size(283, 23);
            Control_AdvancedRemove_TextBox_Part.TabIndex = 1;
            // 
            // Control_AdvancedRemove_Label_Loc
            // 
            Control_AdvancedRemove_Label_Loc.AutoSize = true;
            Control_AdvancedRemove_Label_Loc.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Label_Loc.Location = new Point(3, 29);
            Control_AdvancedRemove_Label_Loc.Name = "Control_AdvancedRemove_Label_Loc";
            Control_AdvancedRemove_Label_Loc.Size = new Size(89, 29);
            Control_AdvancedRemove_Label_Loc.TabIndex = 4;
            Control_AdvancedRemove_Label_Loc.Text = "Location:";
            Control_AdvancedRemove_Label_Loc.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_Label_Op
            // 
            Control_AdvancedRemove_Label_Op.AutoSize = true;
            Control_AdvancedRemove_Label_Op.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Label_Op.Location = new Point(3, 58);
            Control_AdvancedRemove_Label_Op.Name = "Control_AdvancedRemove_Label_Op";
            Control_AdvancedRemove_Label_Op.Size = new Size(89, 29);
            Control_AdvancedRemove_Label_Op.TabIndex = 2;
            Control_AdvancedRemove_Label_Op.Text = "Op:";
            Control_AdvancedRemove_Label_Op.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_Label_User
            // 
            Control_AdvancedRemove_Label_User.AutoSize = true;
            Control_AdvancedRemove_Label_User.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Label_User.Location = new Point(3, 87);
            Control_AdvancedRemove_Label_User.Name = "Control_AdvancedRemove_Label_User";
            Control_AdvancedRemove_Label_User.Size = new Size(89, 29);
            Control_AdvancedRemove_Label_User.TabIndex = 2;
            Control_AdvancedRemove_Label_User.Text = "User:";
            Control_AdvancedRemove_Label_User.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_Label_Notes
            // 
            Control_AdvancedRemove_Label_Notes.AutoSize = true;
            Control_AdvancedRemove_Label_Notes.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Label_Notes.Location = new Point(3, 116);
            Control_AdvancedRemove_Label_Notes.Name = "Control_AdvancedRemove_Label_Notes";
            Control_AdvancedRemove_Label_Notes.Size = new Size(89, 29);
            Control_AdvancedRemove_Label_Notes.TabIndex = 10;
            Control_AdvancedRemove_Label_Notes.Text = "Notes:";
            Control_AdvancedRemove_Label_Notes.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_CheckBox_Date
            // 
            Control_AdvancedRemove_CheckBox_Date.AutoSize = true;
            Control_AdvancedRemove_CheckBox_Date.Dock = DockStyle.Fill;
            Control_AdvancedRemove_CheckBox_Date.Location = new Point(3, 183);
            Control_AdvancedRemove_CheckBox_Date.Name = "Control_AdvancedRemove_CheckBox_Date";
            Control_AdvancedRemove_CheckBox_Date.Size = new Size(89, 29);
            Control_AdvancedRemove_CheckBox_Date.TabIndex = 8;
            Control_AdvancedRemove_CheckBox_Date.Text = "Date Range:";
            Control_AdvancedRemove_CheckBox_Date.TextAlign = ContentAlignment.MiddleRight;
            Control_AdvancedRemove_CheckBox_Date.UseVisualStyleBackColor = true;
            // 
            // Control_AdvancedRemove_Label_Qty
            // 
            Control_AdvancedRemove_Label_Qty.AutoSize = true;
            Control_AdvancedRemove_Label_Qty.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Label_Qty.Location = new Point(3, 145);
            Control_AdvancedRemove_Label_Qty.Name = "Control_AdvancedRemove_Label_Qty";
            Control_AdvancedRemove_Label_Qty.Size = new Size(89, 35);
            Control_AdvancedRemove_Label_Qty.TabIndex = 6;
            Control_AdvancedRemove_Label_Qty.Text = "Quantity:";
            Control_AdvancedRemove_Label_Qty.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_ComboBox_User
            // 
            Control_AdvancedRemove_ComboBox_User.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_AdvancedRemove_ComboBox_User.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_AdvancedRemove_ComboBox_User.Dock = DockStyle.Fill;
            Control_AdvancedRemove_ComboBox_User.Location = new Point(98, 90);
            Control_AdvancedRemove_ComboBox_User.Name = "Control_AdvancedRemove_ComboBox_User";
            Control_AdvancedRemove_ComboBox_User.Size = new Size(283, 23);
            Control_AdvancedRemove_ComboBox_User.TabIndex = 4;
            // 
            // Control_AdvancedRemove_TextBox_Operation
            // 
            Control_AdvancedRemove_TextBox_Operation.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TextBox_Operation.Location = new Point(98, 61);
            Control_AdvancedRemove_TextBox_Operation.Name = "Control_AdvancedRemove_TextBox_Operation";
            Control_AdvancedRemove_TextBox_Operation.Size = new Size(283, 23);
            Control_AdvancedRemove_TextBox_Operation.TabIndex = 3;
            // 
            // Control_AdvancedRemove_Label_Part
            // 
            Control_AdvancedRemove_Label_Part.AutoSize = true;
            Control_AdvancedRemove_Label_Part.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Label_Part.Location = new Point(3, 0);
            Control_AdvancedRemove_Label_Part.Name = "Control_AdvancedRemove_Label_Part";
            Control_AdvancedRemove_Label_Part.Size = new Size(89, 29);
            Control_AdvancedRemove_Label_Part.TabIndex = 0;
            Control_AdvancedRemove_Label_Part.Text = "Part ID:";
            Control_AdvancedRemove_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_TextBox_Notes
            // 
            Control_AdvancedRemove_TextBox_Notes.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TextBox_Notes.Location = new Point(98, 119);
            Control_AdvancedRemove_TextBox_Notes.Name = "Control_AdvancedRemove_TextBox_Notes";
            Control_AdvancedRemove_TextBox_Notes.Size = new Size(283, 23);
            Control_AdvancedRemove_TextBox_Notes.TabIndex = 5;
            // 
            // Control_AdvancedRemove_TableLayout_Quantity
            // 
            Control_AdvancedRemove_TableLayout_Quantity.AutoSize = true;
            Control_AdvancedRemove_TableLayout_Quantity.ColumnCount = 3;
            Control_AdvancedRemove_TableLayout_Quantity.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Control_AdvancedRemove_TableLayout_Quantity.ColumnStyles.Add(new ColumnStyle());
            Control_AdvancedRemove_TableLayout_Quantity.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Control_AdvancedRemove_TableLayout_Quantity.Controls.Add(Control_AdvancedRemove_TextBox_QtyMin, 0, 0);
            Control_AdvancedRemove_TableLayout_Quantity.Controls.Add(Control_AdvancedRemove_TextBox_QtyMax, 2, 0);
            Control_AdvancedRemove_TableLayout_Quantity.Controls.Add(Control_AdvancedRemove_Label_QtyDash, 1, 0);
            Control_AdvancedRemove_TableLayout_Quantity.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TableLayout_Quantity.Location = new Point(98, 148);
            Control_AdvancedRemove_TableLayout_Quantity.Name = "Control_AdvancedRemove_TableLayout_Quantity";
            Control_AdvancedRemove_TableLayout_Quantity.RowCount = 1;
            Control_AdvancedRemove_TableLayout_Quantity.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_Quantity.Size = new Size(283, 29);
            Control_AdvancedRemove_TableLayout_Quantity.TabIndex = 25;
            // 
            // Control_AdvancedRemove_TextBox_QtyMin
            // 
            Control_AdvancedRemove_TextBox_QtyMin.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TextBox_QtyMin.Location = new Point(3, 3);
            Control_AdvancedRemove_TextBox_QtyMin.Name = "Control_AdvancedRemove_TextBox_QtyMin";
            Control_AdvancedRemove_TextBox_QtyMin.PlaceholderText = "Min";
            Control_AdvancedRemove_TextBox_QtyMin.Size = new Size(126, 23);
            Control_AdvancedRemove_TextBox_QtyMin.TabIndex = 6;
            // 
            // Control_AdvancedRemove_TextBox_QtyMax
            // 
            Control_AdvancedRemove_TextBox_QtyMax.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TextBox_QtyMax.Location = new Point(153, 3);
            Control_AdvancedRemove_TextBox_QtyMax.Name = "Control_AdvancedRemove_TextBox_QtyMax";
            Control_AdvancedRemove_TextBox_QtyMax.PlaceholderText = "Max";
            Control_AdvancedRemove_TextBox_QtyMax.Size = new Size(127, 23);
            Control_AdvancedRemove_TextBox_QtyMax.TabIndex = 7;
            // 
            // Control_AdvancedRemove_Label_QtyDash
            // 
            Control_AdvancedRemove_Label_QtyDash.AutoSize = true;
            Control_AdvancedRemove_Label_QtyDash.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Label_QtyDash.Location = new Point(135, 0);
            Control_AdvancedRemove_Label_QtyDash.Name = "Control_AdvancedRemove_Label_QtyDash";
            Control_AdvancedRemove_Label_QtyDash.Size = new Size(12, 29);
            Control_AdvancedRemove_Label_QtyDash.TabIndex = 1;
            Control_AdvancedRemove_Label_QtyDash.Text = "-";
            Control_AdvancedRemove_Label_QtyDash.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Control_AdvancedRemove_Panel_Row4_Center
            // 
            Control_AdvancedRemove_Panel_Row4_Center.Controls.Add(Control_AdvancedRemove_Image_NothingFound);
            Control_AdvancedRemove_Panel_Row4_Center.Controls.Add(Control_AdvancedRemove_DataGridView_Results);
            Control_AdvancedRemove_Panel_Row4_Center.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Row4_Center.Location = new Point(0, 0);
            Control_AdvancedRemove_Panel_Row4_Center.Name = "Control_AdvancedRemove_Panel_Row4_Center";
            Control_AdvancedRemove_Panel_Row4_Center.Size = new Size(400, 332);
            Control_AdvancedRemove_Panel_Row4_Center.TabIndex = 4;
            // 
            // Control_AdvancedRemove_Image_NothingFound
            // 
            Control_AdvancedRemove_Image_NothingFound.BackColor = Color.White;
            Control_AdvancedRemove_Image_NothingFound.BorderStyle = BorderStyle.FixedSingle;
            Control_AdvancedRemove_Image_NothingFound.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Image_NothingFound.ErrorImage = null;
            Control_AdvancedRemove_Image_NothingFound.Image = Properties.Resources.NothingFound;
            Control_AdvancedRemove_Image_NothingFound.InitialImage = null;
            Control_AdvancedRemove_Image_NothingFound.Location = new Point(0, 0);
            Control_AdvancedRemove_Image_NothingFound.Name = "Control_AdvancedRemove_Image_NothingFound";
            Control_AdvancedRemove_Image_NothingFound.Size = new Size(400, 332);
            Control_AdvancedRemove_Image_NothingFound.SizeMode = PictureBoxSizeMode.CenterImage;
            Control_AdvancedRemove_Image_NothingFound.TabIndex = 21;
            Control_AdvancedRemove_Image_NothingFound.TabStop = false;
            Control_AdvancedRemove_Image_NothingFound.Visible = false;
            // 
            // Control_AdvancedRemove_DataGridView_Results
            // 
            Control_AdvancedRemove_DataGridView_Results.AllowUserToAddRows = false;
            Control_AdvancedRemove_DataGridView_Results.AllowUserToDeleteRows = false;
            Control_AdvancedRemove_DataGridView_Results.Dock = DockStyle.Fill;
            Control_AdvancedRemove_DataGridView_Results.Location = new Point(0, 0);
            Control_AdvancedRemove_DataGridView_Results.Name = "Control_AdvancedRemove_DataGridView_Results";
            Control_AdvancedRemove_DataGridView_Results.ReadOnly = true;
            Control_AdvancedRemove_DataGridView_Results.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Control_AdvancedRemove_DataGridView_Results.Size = new Size(400, 332);
            Control_AdvancedRemove_DataGridView_Results.TabIndex = 20;
            // 
            // Control_AdvancedRemove
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            Controls.Add(Control_AdvancedRemove_GroupBox_Main);
            Name = "Control_AdvancedRemove";
            Size = new Size(800, 400);
            Control_AdvancedRemove_GroupBox_Main.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_Main.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_Row4.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_Row4.PerformLayout();
            Control_AdvancedRemove_TableLayout_BottomRight.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_BottomRight.PerformLayout();
            Control_AdvancedRemove_TableLayout_BottomLeft.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_BottomLeft.PerformLayout();
            Control_AdvancedRemove_Panel_Top.ResumeLayout(false);
            Control_AdvancedRemove_SplitContainer_Main.Panel1.ResumeLayout(false);
            Control_AdvancedRemove_SplitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_AdvancedRemove_SplitContainer_Main).EndInit();
            Control_AdvancedRemove_SplitContainer_Main.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_TopLeft.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_TopLeft.PerformLayout();
            Control_AdvancedRemove_TableLayout_DateRange.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_DateRange.PerformLayout();
            Control_AdvancedRemove_TableLayout_Quantity.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_Quantity.PerformLayout();
            Control_AdvancedRemove_Panel_Row4_Center.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_AdvancedRemove_Image_NothingFound).EndInit();
            ((System.ComponentModel.ISupportInitialize)Control_AdvancedRemove_DataGridView_Results).EndInit();
            ResumeLayout(false);
        }
        private Button Control_AdvancedRemove_Button_Print;
        private TextBox Control_AdvancedRemove_TextBox_Location;
        private TextBox Control_AdvancedRemove_TextBox_Part;
        private TextBox Control_AdvancedRemove_TextBox_Operation;
        private Label Control_AdvancedRemove_Label_Part;
        private TableLayoutPanel Control_AdvancedRemove_TableLayout_Quantity;
        private TableLayoutPanel Control_AdvancedRemove_TableLayout_DateRange;
        private TableLayoutPanel Control_AdvancedRemove_TableLayout_BottomLeft;
        private TableLayoutPanel Control_AdvancedRemove_TableLayout_BottomRight;
    }

        
        #endregion
    }
