using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.MainForm
{
    partial class Control_AdvancedRemove
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
            Control_AdvancedRemove_GroupBox_Main = new GroupBox();
            Control_AdvancedRemove_TableLayout_Main = new TableLayoutPanel();
            Control_AdvancedRemove_TableLayout_Row4 = new TableLayoutPanel();
            Control_AdvancedRemove_Panel_Row5_Right = new Panel();
            Control_AdvancedRemove_Button_Reset = new Button();
            Control_AdvancedRemove_Button_Normal = new Button();
            Control_AdvancedRemove_Panel_Row5_Left = new Panel();
            Control_AdvancedRemove_Button_SidePanel = new Button();
            Control_AdvancedRemove_Button_Undo = new Button();
            Control_AdvancedRemove_Button_Delete = new Button();
            Control_AdvancedRemove_Button_Search = new Button();
            Control_AdvancedRemove_Panel_Top = new Panel();
            Control_AdvancedRemove_SplitContainer_Main = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            Control_AdvancedRemove_TextBox_Like = new TextBox();
            Control_AdvancedRemove_ComboBox_Like = new ComboBox();
            Control_AdvancedRemove_Panel_Row3_Left = new Panel();
            Control_AdvancedRemove_ComboBox_User = new ComboBox();
            Control_AdvancedRemove_Label_User = new Label();
            Control_AdvancedRemove_Panel_Row1_Right = new Panel();
            Control_AdvancedRemove_ComboBox_Op = new ComboBox();
            Control_AdvancedRemove_Label_Op = new Label();
            Control_AdvancedRemove_Panel_Row1_Left = new Panel();
            Control_AdvancedRemove_ComboBox_Part = new ComboBox();
            Control_AdvancedRemove_Label_Part = new Label();
            Control_AdvancedRemove_Panel_Row1_Center = new Panel();
            Control_AdvancedRemove_ComboBox_Loc = new ComboBox();
            Control_AdvancedRemove_Label_Loc = new Label();
            Control_AdvancedRemove_Panel_Row2_Right = new Panel();
            Control_AdvancedRemove_Label_Notes = new Label();
            Control_AdvancedRemove_TextBox_Notes = new TextBox();
            panel2 = new Panel();
            Control_AdvancedRemove_Label_Qty = new Label();
            Control_AdvancedRemove_TextBox_QtyMin = new TextBox();
            Control_AdvancedRemove_Label_QtyDash = new Label();
            Control_AdvancedRemove_TextBox_QtyMax = new TextBox();
            panel3 = new Panel();
            Control_AdvancedRemove_Label_DateDash = new Label();
            Control_AdvancedRemove_DateTimePicker_To = new DateTimePicker();
            Control_AdvancedRemove_CheckBox_Date = new CheckBox();
            Control_AdvancedRemove_DateTimePicker_From = new DateTimePicker();
            Control_AdvancedRemove_Panel_Row4_Center = new Panel();
            Control_AdvancedRemove_Image_NothingFound = new PictureBox();
            Control_AdvancedRemove_DataGridView_Results = new DataGridView();
            BottomToolStripPanel = new ToolStripPanel();
            TopToolStripPanel = new ToolStripPanel();
            RightToolStripPanel = new ToolStripPanel();
            LeftToolStripPanel = new ToolStripPanel();
            ContentPanel = new ToolStripContentPanel();
            Control_AdvancedRemove_GroupBox_Main.SuspendLayout();
            Control_AdvancedRemove_TableLayout_Main.SuspendLayout();
            Control_AdvancedRemove_TableLayout_Row4.SuspendLayout();
            Control_AdvancedRemove_Panel_Row5_Right.SuspendLayout();
            Control_AdvancedRemove_Panel_Row5_Left.SuspendLayout();
            Control_AdvancedRemove_Panel_Top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_AdvancedRemove_SplitContainer_Main).BeginInit();
            Control_AdvancedRemove_SplitContainer_Main.Panel1.SuspendLayout();
            Control_AdvancedRemove_SplitContainer_Main.Panel2.SuspendLayout();
            Control_AdvancedRemove_SplitContainer_Main.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            Control_AdvancedRemove_Panel_Row3_Left.SuspendLayout();
            Control_AdvancedRemove_Panel_Row1_Right.SuspendLayout();
            Control_AdvancedRemove_Panel_Row1_Left.SuspendLayout();
            Control_AdvancedRemove_Panel_Row1_Center.SuspendLayout();
            Control_AdvancedRemove_Panel_Row2_Right.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
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
            Control_AdvancedRemove_GroupBox_Main.Size = new Size(817, 388);
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
            Control_AdvancedRemove_TableLayout_Main.Size = new Size(811, 366);
            Control_AdvancedRemove_TableLayout_Main.TabIndex = 1;
            // 
            // Control_AdvancedRemove_TableLayout_Row4
            // 
            Control_AdvancedRemove_TableLayout_Row4.ColumnCount = 2;
            Control_AdvancedRemove_TableLayout_Row4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Control_AdvancedRemove_TableLayout_Row4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Control_AdvancedRemove_TableLayout_Row4.Controls.Add(Control_AdvancedRemove_Panel_Row5_Right, 1, 0);
            Control_AdvancedRemove_TableLayout_Row4.Controls.Add(Control_AdvancedRemove_Panel_Row5_Left, 0, 0);
            Control_AdvancedRemove_TableLayout_Row4.Dock = DockStyle.Fill;
            Control_AdvancedRemove_TableLayout_Row4.Location = new Point(3, 329);
            Control_AdvancedRemove_TableLayout_Row4.Name = "Control_AdvancedRemove_TableLayout_Row4";
            Control_AdvancedRemove_TableLayout_Row4.RowCount = 1;
            Control_AdvancedRemove_TableLayout_Row4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_AdvancedRemove_TableLayout_Row4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            Control_AdvancedRemove_TableLayout_Row4.Size = new Size(805, 34);
            Control_AdvancedRemove_TableLayout_Row4.TabIndex = 3;
            // 
            // Control_AdvancedRemove_Panel_Row5_Right
            // 
            Control_AdvancedRemove_Panel_Row5_Right.Controls.Add(Control_AdvancedRemove_Button_Reset);
            Control_AdvancedRemove_Panel_Row5_Right.Controls.Add(Control_AdvancedRemove_Button_Normal);
            Control_AdvancedRemove_Panel_Row5_Right.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Row5_Right.Location = new Point(405, 3);
            Control_AdvancedRemove_Panel_Row5_Right.Name = "Control_AdvancedRemove_Panel_Row5_Right";
            Control_AdvancedRemove_Panel_Row5_Right.Size = new Size(397, 28);
            Control_AdvancedRemove_Panel_Row5_Right.TabIndex = 2;
            // 
            // Control_AdvancedRemove_Button_Reset
            // 
            Control_AdvancedRemove_Button_Reset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_AdvancedRemove_Button_Reset.Location = new Point(4, 2);
            Control_AdvancedRemove_Button_Reset.Name = "Control_AdvancedRemove_Button_Reset";
            Control_AdvancedRemove_Button_Reset.Size = new Size(80, 24);
            Control_AdvancedRemove_Button_Reset.TabIndex = 1;
            Control_AdvancedRemove_Button_Reset.Text = "Reset";
            // 
            // Control_AdvancedRemove_Button_Normal
            // 
            Control_AdvancedRemove_Button_Normal.ForeColor = Color.DarkRed;
            Control_AdvancedRemove_Button_Normal.Location = new Point(272, 2);
            Control_AdvancedRemove_Button_Normal.Name = "Control_AdvancedRemove_Button_Normal";
            Control_AdvancedRemove_Button_Normal.Size = new Size(121, 24);
            Control_AdvancedRemove_Button_Normal.TabIndex = 15;
            Control_AdvancedRemove_Button_Normal.TabStop = false;
            Control_AdvancedRemove_Button_Normal.Text = "Back to Normal";
            // 
            // Control_AdvancedRemove_Panel_Row5_Left
            // 
            Control_AdvancedRemove_Panel_Row5_Left.Controls.Add(Control_AdvancedRemove_Button_SidePanel);
            Control_AdvancedRemove_Panel_Row5_Left.Controls.Add(Control_AdvancedRemove_Button_Undo);
            Control_AdvancedRemove_Panel_Row5_Left.Controls.Add(Control_AdvancedRemove_Button_Delete);
            Control_AdvancedRemove_Panel_Row5_Left.Controls.Add(Control_AdvancedRemove_Button_Search);
            Control_AdvancedRemove_Panel_Row5_Left.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Row5_Left.Location = new Point(3, 3);
            Control_AdvancedRemove_Panel_Row5_Left.Name = "Control_AdvancedRemove_Panel_Row5_Left";
            Control_AdvancedRemove_Panel_Row5_Left.Size = new Size(396, 28);
            Control_AdvancedRemove_Panel_Row5_Left.TabIndex = 1;
            // 
            // Control_AdvancedRemove_Button_SidePanel
            // 
            Control_AdvancedRemove_Button_SidePanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_AdvancedRemove_Button_SidePanel.Location = new Point(312, 2);
            Control_AdvancedRemove_Button_SidePanel.Name = "Control_AdvancedRemove_Button_SidePanel";
            Control_AdvancedRemove_Button_SidePanel.Size = new Size(80, 24);
            Control_AdvancedRemove_Button_SidePanel.TabIndex = 10;
            Control_AdvancedRemove_Button_SidePanel.Text = "Collapse ◀";
            Control_AdvancedRemove_Button_SidePanel.UseVisualStyleBackColor = true;
            Control_AdvancedRemove_Button_SidePanel.Click += Control_AdvancedRemove_Button_SidePanel_Click;
            // 
            // Control_AdvancedRemove_Button_Undo
            // 
            Control_AdvancedRemove_Button_Undo.Enabled = false;
            Control_AdvancedRemove_Button_Undo.Location = new Point(89, 2);
            Control_AdvancedRemove_Button_Undo.Name = "Control_AdvancedRemove_Button_Undo";
            Control_AdvancedRemove_Button_Undo.Size = new Size(80, 24);
            Control_AdvancedRemove_Button_Undo.TabIndex = 2;
            Control_AdvancedRemove_Button_Undo.Text = "Undo";
            // 
            // Control_AdvancedRemove_Button_Delete
            // 
            Control_AdvancedRemove_Button_Delete.Location = new Point(175, 2);
            Control_AdvancedRemove_Button_Delete.Name = "Control_AdvancedRemove_Button_Delete";
            Control_AdvancedRemove_Button_Delete.Size = new Size(80, 24);
            Control_AdvancedRemove_Button_Delete.TabIndex = 1;
            Control_AdvancedRemove_Button_Delete.Text = "Delete";
            // 
            // Control_AdvancedRemove_Button_Search
            // 
            Control_AdvancedRemove_Button_Search.Location = new Point(3, 2);
            Control_AdvancedRemove_Button_Search.Name = "Control_AdvancedRemove_Button_Search";
            Control_AdvancedRemove_Button_Search.Size = new Size(80, 24);
            Control_AdvancedRemove_Button_Search.TabIndex = 0;
            Control_AdvancedRemove_Button_Search.Text = "Search";
            // 
            // Control_AdvancedRemove_Panel_Top
            // 
            Control_AdvancedRemove_Panel_Top.Controls.Add(Control_AdvancedRemove_SplitContainer_Main);
            Control_AdvancedRemove_Panel_Top.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Top.Location = new Point(3, 3);
            Control_AdvancedRemove_Panel_Top.Name = "Control_AdvancedRemove_Panel_Top";
            Control_AdvancedRemove_Panel_Top.Size = new Size(805, 320);
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
            Control_AdvancedRemove_SplitContainer_Main.Panel1.Controls.Add(tableLayoutPanel1);
            Control_AdvancedRemove_SplitContainer_Main.Panel1MinSize = 0;
            // 
            // Control_AdvancedRemove_SplitContainer_Main.Panel2
            // 
            Control_AdvancedRemove_SplitContainer_Main.Panel2.Controls.Add(Control_AdvancedRemove_Panel_Row4_Center);
            Control_AdvancedRemove_SplitContainer_Main.Panel2MinSize = 400;
            Control_AdvancedRemove_SplitContainer_Main.Size = new Size(805, 320);
            Control_AdvancedRemove_SplitContainer_Main.SplitterDistance = 399;
            Control_AdvancedRemove_SplitContainer_Main.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 7);
            tableLayoutPanel1.Controls.Add(Control_AdvancedRemove_Panel_Row3_Left, 0, 3);
            tableLayoutPanel1.Controls.Add(Control_AdvancedRemove_Panel_Row1_Right, 0, 2);
            tableLayoutPanel1.Controls.Add(Control_AdvancedRemove_Panel_Row1_Left, 0, 0);
            tableLayoutPanel1.Controls.Add(Control_AdvancedRemove_Panel_Row1_Center, 0, 1);
            tableLayoutPanel1.Controls.Add(Control_AdvancedRemove_Panel_Row2_Right, 0, 4);
            tableLayoutPanel1.Controls.Add(panel2, 0, 5);
            tableLayoutPanel1.Controls.Add(panel3, 0, 6);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 8;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.Size = new Size(399, 320);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.Controls.Add(Control_AdvancedRemove_TextBox_Like);
            panel1.Controls.Add(Control_AdvancedRemove_ComboBox_Like);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 283);
            panel1.Name = "panel1";
            panel1.Size = new Size(393, 34);
            panel1.TabIndex = 5;
            // 
            // Control_AdvancedRemove_TextBox_Like
            // 
            Control_AdvancedRemove_TextBox_Like.Location = new Point(114, 6);
            Control_AdvancedRemove_TextBox_Like.Name = "Control_AdvancedRemove_TextBox_Like";
            Control_AdvancedRemove_TextBox_Like.Size = new Size(277, 23);
            Control_AdvancedRemove_TextBox_Like.TabIndex = 11;
            // 
            // Control_AdvancedRemove_ComboBox_Like
            // 
            Control_AdvancedRemove_ComboBox_Like.AutoCompleteCustomSource.AddRange(new string[] { "1", "2", "2", "3", "4" });
            Control_AdvancedRemove_ComboBox_Like.Location = new Point(3, 6);
            Control_AdvancedRemove_ComboBox_Like.Name = "Control_AdvancedRemove_ComboBox_Like";
            Control_AdvancedRemove_ComboBox_Like.Size = new Size(105, 23);
            Control_AdvancedRemove_ComboBox_Like.TabIndex = 12;
            Control_AdvancedRemove_ComboBox_Like.SelectedIndexChanged += Control_AdvancedRemove_ComboBox_Like_SelectedIndexChanged;
            // 
            // Control_AdvancedRemove_Panel_Row3_Left
            // 
            Control_AdvancedRemove_Panel_Row3_Left.AutoSize = true;
            Control_AdvancedRemove_Panel_Row3_Left.Controls.Add(Control_AdvancedRemove_ComboBox_User);
            Control_AdvancedRemove_Panel_Row3_Left.Controls.Add(Control_AdvancedRemove_Label_User);
            Control_AdvancedRemove_Panel_Row3_Left.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Row3_Left.Location = new Point(3, 123);
            Control_AdvancedRemove_Panel_Row3_Left.Name = "Control_AdvancedRemove_Panel_Row3_Left";
            Control_AdvancedRemove_Panel_Row3_Left.Size = new Size(393, 34);
            Control_AdvancedRemove_Panel_Row3_Left.TabIndex = 1;
            // 
            // Control_AdvancedRemove_ComboBox_User
            // 
            Control_AdvancedRemove_ComboBox_User.Anchor = AnchorStyles.Left;
            Control_AdvancedRemove_ComboBox_User.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_AdvancedRemove_ComboBox_User.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_AdvancedRemove_ComboBox_User.Location = new Point(83, 6);
            Control_AdvancedRemove_ComboBox_User.Name = "Control_AdvancedRemove_ComboBox_User";
            Control_AdvancedRemove_ComboBox_User.Size = new Size(308, 23);
            Control_AdvancedRemove_ComboBox_User.TabIndex = 3;
            // 
            // Control_AdvancedRemove_Label_User
            // 
            Control_AdvancedRemove_Label_User.Location = new Point(3, 5);
            Control_AdvancedRemove_Label_User.Name = "Control_AdvancedRemove_Label_User";
            Control_AdvancedRemove_Label_User.Size = new Size(74, 23);
            Control_AdvancedRemove_Label_User.TabIndex = 2;
            Control_AdvancedRemove_Label_User.Text = "User:";
            Control_AdvancedRemove_Label_User.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_Panel_Row1_Right
            // 
            Control_AdvancedRemove_Panel_Row1_Right.AutoSize = true;
            Control_AdvancedRemove_Panel_Row1_Right.Controls.Add(Control_AdvancedRemove_ComboBox_Op);
            Control_AdvancedRemove_Panel_Row1_Right.Controls.Add(Control_AdvancedRemove_Label_Op);
            Control_AdvancedRemove_Panel_Row1_Right.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Row1_Right.Location = new Point(3, 83);
            Control_AdvancedRemove_Panel_Row1_Right.Name = "Control_AdvancedRemove_Panel_Row1_Right";
            Control_AdvancedRemove_Panel_Row1_Right.Size = new Size(393, 34);
            Control_AdvancedRemove_Panel_Row1_Right.TabIndex = 2;
            // 
            // Control_AdvancedRemove_ComboBox_Op
            // 
            Control_AdvancedRemove_ComboBox_Op.Anchor = AnchorStyles.Left;
            Control_AdvancedRemove_ComboBox_Op.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_AdvancedRemove_ComboBox_Op.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_AdvancedRemove_ComboBox_Op.Location = new Point(83, 6);
            Control_AdvancedRemove_ComboBox_Op.Name = "Control_AdvancedRemove_ComboBox_Op";
            Control_AdvancedRemove_ComboBox_Op.Size = new Size(308, 23);
            Control_AdvancedRemove_ComboBox_Op.TabIndex = 3;
            // 
            // Control_AdvancedRemove_Label_Op
            // 
            Control_AdvancedRemove_Label_Op.Location = new Point(3, 5);
            Control_AdvancedRemove_Label_Op.Name = "Control_AdvancedRemove_Label_Op";
            Control_AdvancedRemove_Label_Op.Size = new Size(74, 23);
            Control_AdvancedRemove_Label_Op.TabIndex = 2;
            Control_AdvancedRemove_Label_Op.Text = "Op:";
            Control_AdvancedRemove_Label_Op.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_Panel_Row1_Left
            // 
            Control_AdvancedRemove_Panel_Row1_Left.AutoSize = true;
            Control_AdvancedRemove_Panel_Row1_Left.Controls.Add(Control_AdvancedRemove_ComboBox_Part);
            Control_AdvancedRemove_Panel_Row1_Left.Controls.Add(Control_AdvancedRemove_Label_Part);
            Control_AdvancedRemove_Panel_Row1_Left.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Row1_Left.Location = new Point(3, 3);
            Control_AdvancedRemove_Panel_Row1_Left.Name = "Control_AdvancedRemove_Panel_Row1_Left";
            Control_AdvancedRemove_Panel_Row1_Left.Size = new Size(393, 34);
            Control_AdvancedRemove_Panel_Row1_Left.TabIndex = 0;
            // 
            // Control_AdvancedRemove_ComboBox_Part
            // 
            Control_AdvancedRemove_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_AdvancedRemove_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_AdvancedRemove_ComboBox_Part.Location = new Point(83, 5);
            Control_AdvancedRemove_ComboBox_Part.Name = "Control_AdvancedRemove_ComboBox_Part";
            Control_AdvancedRemove_ComboBox_Part.Size = new Size(308, 23);
            Control_AdvancedRemove_ComboBox_Part.TabIndex = 1;
            // 
            // Control_AdvancedRemove_Label_Part
            // 
            Control_AdvancedRemove_Label_Part.Location = new Point(3, 5);
            Control_AdvancedRemove_Label_Part.Name = "Control_AdvancedRemove_Label_Part";
            Control_AdvancedRemove_Label_Part.Size = new Size(74, 23);
            Control_AdvancedRemove_Label_Part.TabIndex = 0;
            Control_AdvancedRemove_Label_Part.Text = "Part ID:";
            Control_AdvancedRemove_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_Panel_Row1_Center
            // 
            Control_AdvancedRemove_Panel_Row1_Center.AutoSize = true;
            Control_AdvancedRemove_Panel_Row1_Center.Controls.Add(Control_AdvancedRemove_ComboBox_Loc);
            Control_AdvancedRemove_Panel_Row1_Center.Controls.Add(Control_AdvancedRemove_Label_Loc);
            Control_AdvancedRemove_Panel_Row1_Center.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Row1_Center.Location = new Point(3, 43);
            Control_AdvancedRemove_Panel_Row1_Center.Name = "Control_AdvancedRemove_Panel_Row1_Center";
            Control_AdvancedRemove_Panel_Row1_Center.Size = new Size(393, 34);
            Control_AdvancedRemove_Panel_Row1_Center.TabIndex = 1;
            // 
            // Control_AdvancedRemove_ComboBox_Loc
            // 
            Control_AdvancedRemove_ComboBox_Loc.Anchor = AnchorStyles.Left;
            Control_AdvancedRemove_ComboBox_Loc.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_AdvancedRemove_ComboBox_Loc.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_AdvancedRemove_ComboBox_Loc.Location = new Point(83, 6);
            Control_AdvancedRemove_ComboBox_Loc.Name = "Control_AdvancedRemove_ComboBox_Loc";
            Control_AdvancedRemove_ComboBox_Loc.Size = new Size(308, 23);
            Control_AdvancedRemove_ComboBox_Loc.TabIndex = 5;
            // 
            // Control_AdvancedRemove_Label_Loc
            // 
            Control_AdvancedRemove_Label_Loc.Location = new Point(3, 5);
            Control_AdvancedRemove_Label_Loc.Name = "Control_AdvancedRemove_Label_Loc";
            Control_AdvancedRemove_Label_Loc.Size = new Size(74, 23);
            Control_AdvancedRemove_Label_Loc.TabIndex = 4;
            Control_AdvancedRemove_Label_Loc.Text = "Location:";
            Control_AdvancedRemove_Label_Loc.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_Panel_Row2_Right
            // 
            Control_AdvancedRemove_Panel_Row2_Right.AutoSize = true;
            Control_AdvancedRemove_Panel_Row2_Right.Controls.Add(Control_AdvancedRemove_Label_Notes);
            Control_AdvancedRemove_Panel_Row2_Right.Controls.Add(Control_AdvancedRemove_TextBox_Notes);
            Control_AdvancedRemove_Panel_Row2_Right.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Row2_Right.Location = new Point(3, 163);
            Control_AdvancedRemove_Panel_Row2_Right.Name = "Control_AdvancedRemove_Panel_Row2_Right";
            Control_AdvancedRemove_Panel_Row2_Right.Size = new Size(393, 34);
            Control_AdvancedRemove_Panel_Row2_Right.TabIndex = 4;
            // 
            // Control_AdvancedRemove_Label_Notes
            // 
            Control_AdvancedRemove_Label_Notes.Location = new Point(3, 5);
            Control_AdvancedRemove_Label_Notes.Name = "Control_AdvancedRemove_Label_Notes";
            Control_AdvancedRemove_Label_Notes.Size = new Size(74, 23);
            Control_AdvancedRemove_Label_Notes.TabIndex = 10;
            Control_AdvancedRemove_Label_Notes.Text = "Notes:";
            Control_AdvancedRemove_Label_Notes.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_TextBox_Notes
            // 
            Control_AdvancedRemove_TextBox_Notes.Location = new Point(83, 5);
            Control_AdvancedRemove_TextBox_Notes.Name = "Control_AdvancedRemove_TextBox_Notes";
            Control_AdvancedRemove_TextBox_Notes.Size = new Size(308, 23);
            Control_AdvancedRemove_TextBox_Notes.TabIndex = 11;
            // 
            // panel2
            // 
            panel2.Controls.Add(Control_AdvancedRemove_Label_Qty);
            panel2.Controls.Add(Control_AdvancedRemove_TextBox_QtyMin);
            panel2.Controls.Add(Control_AdvancedRemove_Label_QtyDash);
            panel2.Controls.Add(Control_AdvancedRemove_TextBox_QtyMax);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 203);
            panel2.Name = "panel2";
            panel2.Size = new Size(393, 34);
            panel2.TabIndex = 6;
            // 
            // Control_AdvancedRemove_Label_Qty
            // 
            Control_AdvancedRemove_Label_Qty.Location = new Point(3, 4);
            Control_AdvancedRemove_Label_Qty.Name = "Control_AdvancedRemove_Label_Qty";
            Control_AdvancedRemove_Label_Qty.Size = new Size(74, 23);
            Control_AdvancedRemove_Label_Qty.TabIndex = 6;
            Control_AdvancedRemove_Label_Qty.Text = "Quantity:";
            Control_AdvancedRemove_Label_Qty.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_AdvancedRemove_TextBox_QtyMin
            // 
            Control_AdvancedRemove_TextBox_QtyMin.Anchor = AnchorStyles.Left;
            Control_AdvancedRemove_TextBox_QtyMin.Location = new Point(83, 5);
            Control_AdvancedRemove_TextBox_QtyMin.Name = "Control_AdvancedRemove_TextBox_QtyMin";
            Control_AdvancedRemove_TextBox_QtyMin.PlaceholderText = "Min";
            Control_AdvancedRemove_TextBox_QtyMin.Size = new Size(140, 23);
            Control_AdvancedRemove_TextBox_QtyMin.TabIndex = 0;
            // 
            // Control_AdvancedRemove_Label_QtyDash
            // 
            Control_AdvancedRemove_Label_QtyDash.Location = new Point(229, 4);
            Control_AdvancedRemove_Label_QtyDash.Name = "Control_AdvancedRemove_Label_QtyDash";
            Control_AdvancedRemove_Label_QtyDash.Size = new Size(16, 22);
            Control_AdvancedRemove_Label_QtyDash.TabIndex = 1;
            Control_AdvancedRemove_Label_QtyDash.Text = "-";
            Control_AdvancedRemove_Label_QtyDash.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Control_AdvancedRemove_TextBox_QtyMax
            // 
            Control_AdvancedRemove_TextBox_QtyMax.Anchor = AnchorStyles.Left;
            Control_AdvancedRemove_TextBox_QtyMax.Location = new Point(251, 5);
            Control_AdvancedRemove_TextBox_QtyMax.Name = "Control_AdvancedRemove_TextBox_QtyMax";
            Control_AdvancedRemove_TextBox_QtyMax.PlaceholderText = "Max";
            Control_AdvancedRemove_TextBox_QtyMax.Size = new Size(140, 23);
            Control_AdvancedRemove_TextBox_QtyMax.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(Control_AdvancedRemove_Label_DateDash);
            panel3.Controls.Add(Control_AdvancedRemove_DateTimePicker_To);
            panel3.Controls.Add(Control_AdvancedRemove_CheckBox_Date);
            panel3.Controls.Add(Control_AdvancedRemove_DateTimePicker_From);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 243);
            panel3.Name = "panel3";
            panel3.Size = new Size(393, 34);
            panel3.TabIndex = 7;
            // 
            // Control_AdvancedRemove_Label_DateDash
            // 
            Control_AdvancedRemove_Label_DateDash.Location = new Point(229, 5);
            Control_AdvancedRemove_Label_DateDash.Name = "Control_AdvancedRemove_Label_DateDash";
            Control_AdvancedRemove_Label_DateDash.Size = new Size(16, 22);
            Control_AdvancedRemove_Label_DateDash.TabIndex = 1;
            Control_AdvancedRemove_Label_DateDash.Text = "-";
            Control_AdvancedRemove_Label_DateDash.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Control_AdvancedRemove_DateTimePicker_To
            // 
            Control_AdvancedRemove_DateTimePicker_To.Anchor = AnchorStyles.Left;
            Control_AdvancedRemove_DateTimePicker_To.Format = DateTimePickerFormat.Short;
            Control_AdvancedRemove_DateTimePicker_To.Location = new Point(251, 6);
            Control_AdvancedRemove_DateTimePicker_To.Name = "Control_AdvancedRemove_DateTimePicker_To";
            Control_AdvancedRemove_DateTimePicker_To.Size = new Size(140, 23);
            Control_AdvancedRemove_DateTimePicker_To.TabIndex = 2;
            // 
            // Control_AdvancedRemove_CheckBox_Date
            // 
            Control_AdvancedRemove_CheckBox_Date.Location = new Point(3, 5);
            Control_AdvancedRemove_CheckBox_Date.Name = "Control_AdvancedRemove_CheckBox_Date";
            Control_AdvancedRemove_CheckBox_Date.Size = new Size(74, 23);
            Control_AdvancedRemove_CheckBox_Date.TabIndex = 3;
            Control_AdvancedRemove_CheckBox_Date.Text = "Range:";
            Control_AdvancedRemove_CheckBox_Date.TextAlign = ContentAlignment.MiddleRight;
            Control_AdvancedRemove_CheckBox_Date.UseVisualStyleBackColor = true;
            // 
            // Control_AdvancedRemove_DateTimePicker_From
            // 
            Control_AdvancedRemove_DateTimePicker_From.Anchor = AnchorStyles.Left;
            Control_AdvancedRemove_DateTimePicker_From.Format = DateTimePickerFormat.Short;
            Control_AdvancedRemove_DateTimePicker_From.Location = new Point(83, 6);
            Control_AdvancedRemove_DateTimePicker_From.Name = "Control_AdvancedRemove_DateTimePicker_From";
            Control_AdvancedRemove_DateTimePicker_From.Size = new Size(140, 23);
            Control_AdvancedRemove_DateTimePicker_From.TabIndex = 0;
            // 
            // Control_AdvancedRemove_Panel_Row4_Center
            // 
            Control_AdvancedRemove_Panel_Row4_Center.Controls.Add(Control_AdvancedRemove_Image_NothingFound);
            Control_AdvancedRemove_Panel_Row4_Center.Controls.Add(Control_AdvancedRemove_DataGridView_Results);
            Control_AdvancedRemove_Panel_Row4_Center.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Panel_Row4_Center.Location = new Point(0, 0);
            Control_AdvancedRemove_Panel_Row4_Center.Name = "Control_AdvancedRemove_Panel_Row4_Center";
            Control_AdvancedRemove_Panel_Row4_Center.Size = new Size(402, 320);
            Control_AdvancedRemove_Panel_Row4_Center.TabIndex = 4;
            // 
            // Control_AdvancedRemove_Image_NothingFound
            // 
            Control_AdvancedRemove_Image_NothingFound.BackColor = Color.White;
            Control_AdvancedRemove_Image_NothingFound.Dock = DockStyle.Fill;
            Control_AdvancedRemove_Image_NothingFound.ErrorImage = null;
            Control_AdvancedRemove_Image_NothingFound.Image = Properties.Resources.NothingFound;
            Control_AdvancedRemove_Image_NothingFound.InitialImage = null;
            Control_AdvancedRemove_Image_NothingFound.Location = new Point(0, 0);
            Control_AdvancedRemove_Image_NothingFound.Name = "Control_AdvancedRemove_Image_NothingFound";
            Control_AdvancedRemove_Image_NothingFound.Size = new Size(402, 320);
            Control_AdvancedRemove_Image_NothingFound.SizeMode = PictureBoxSizeMode.Zoom;
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
            Control_AdvancedRemove_DataGridView_Results.Size = new Size(402, 320);
            Control_AdvancedRemove_DataGridView_Results.TabIndex = 20;
            // 
            // BottomToolStripPanel
            // 
            BottomToolStripPanel.Location = new Point(0, 0);
            BottomToolStripPanel.Name = "BottomToolStripPanel";
            BottomToolStripPanel.Orientation = Orientation.Horizontal;
            BottomToolStripPanel.RowMargin = new Padding(3, 0, 0, 0);
            BottomToolStripPanel.Size = new Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            TopToolStripPanel.Location = new Point(0, 0);
            TopToolStripPanel.Name = "TopToolStripPanel";
            TopToolStripPanel.Orientation = Orientation.Horizontal;
            TopToolStripPanel.RowMargin = new Padding(3, 0, 0, 0);
            TopToolStripPanel.Size = new Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            RightToolStripPanel.Location = new Point(0, 0);
            RightToolStripPanel.Name = "RightToolStripPanel";
            RightToolStripPanel.Orientation = Orientation.Horizontal;
            RightToolStripPanel.RowMargin = new Padding(3, 0, 0, 0);
            RightToolStripPanel.Size = new Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            LeftToolStripPanel.Location = new Point(0, 0);
            LeftToolStripPanel.Name = "LeftToolStripPanel";
            LeftToolStripPanel.Orientation = Orientation.Horizontal;
            LeftToolStripPanel.RowMargin = new Padding(3, 0, 0, 0);
            LeftToolStripPanel.Size = new Size(0, 0);
            // 
            // ContentPanel
            // 
            ContentPanel.Size = new Size(778, 316);
            // 
            // Control_AdvancedRemove
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(Control_AdvancedRemove_GroupBox_Main);
            Name = "Control_AdvancedRemove";
            Size = new Size(817, 388);
            Control_AdvancedRemove_GroupBox_Main.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_Main.ResumeLayout(false);
            Control_AdvancedRemove_TableLayout_Row4.ResumeLayout(false);
            Control_AdvancedRemove_Panel_Row5_Right.ResumeLayout(false);
            Control_AdvancedRemove_Panel_Row5_Left.ResumeLayout(false);
            Control_AdvancedRemove_Panel_Top.ResumeLayout(false);
            Control_AdvancedRemove_SplitContainer_Main.Panel1.ResumeLayout(false);
            Control_AdvancedRemove_SplitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_AdvancedRemove_SplitContainer_Main).EndInit();
            Control_AdvancedRemove_SplitContainer_Main.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            Control_AdvancedRemove_Panel_Row3_Left.ResumeLayout(false);
            Control_AdvancedRemove_Panel_Row1_Right.ResumeLayout(false);
            Control_AdvancedRemove_Panel_Row1_Left.ResumeLayout(false);
            Control_AdvancedRemove_Panel_Row1_Center.ResumeLayout(false);
            Control_AdvancedRemove_Panel_Row2_Right.ResumeLayout(false);
            Control_AdvancedRemove_Panel_Row2_Right.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            Control_AdvancedRemove_Panel_Row4_Center.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_AdvancedRemove_Image_NothingFound).EndInit();
            ((System.ComponentModel.ISupportInitialize)Control_AdvancedRemove_DataGridView_Results).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox Control_AdvancedRemove_GroupBox_Main;
        private System.Windows.Forms.TableLayoutPanel Control_AdvancedRemove_TableLayout_Main;
        private System.Windows.Forms.Label Control_AdvancedRemove_Label_Part;
        private System.Windows.Forms.ComboBox Control_AdvancedRemove_ComboBox_Part;
        private System.Windows.Forms.Label Control_AdvancedRemove_Label_Op;
        private System.Windows.Forms.ComboBox Control_AdvancedRemove_ComboBox_Op;
        private System.Windows.Forms.Label Control_AdvancedRemove_Label_Loc;
        private System.Windows.Forms.ComboBox Control_AdvancedRemove_ComboBox_Loc;
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
        private Panel Control_AdvancedRemove_Panel_Row1_Center;
        private Panel Control_AdvancedRemove_Panel_Row1_Left;
        private Panel Control_AdvancedRemove_Panel_Row1_Right;
        private Panel Control_AdvancedRemove_Panel_Row3_Left;
        private Panel Control_AdvancedRemove_Panel_Row2_Right;
        private Panel Control_AdvancedRemove_Panel_Row4_Center;
        private ComboBox Control_AdvancedRemove_ComboBox_User;
        private Label Control_AdvancedRemove_Label_User;
        private PictureBox Control_AdvancedRemove_Image_NothingFound;
        private CheckBox Control_AdvancedRemove_CheckBox_Date;
        private Panel panel1;
        private TextBox Control_AdvancedRemove_TextBox_Like;
        private ComboBox Control_AdvancedRemove_ComboBox_Like;
        private TableLayoutPanel Control_AdvancedRemove_TableLayout_Row4;
        private Panel Control_AdvancedRemove_Panel_Row5_Right;
        private Button Control_AdvancedRemove_Button_Reset;
        private Button Control_AdvancedRemove_Button_Normal;
        private Panel Control_AdvancedRemove_Panel_Row5_Left;
        private Button Control_AdvancedRemove_Button_Undo;
        private Button Control_AdvancedRemove_Button_Delete;
        private Button Control_AdvancedRemove_Button_Search;
        private Panel Control_AdvancedRemove_Panel_Top;
        private ToolStripPanel BottomToolStripPanel;
        private ToolStripPanel TopToolStripPanel;
        private ToolStripPanel RightToolStripPanel;
        private ToolStripPanel LeftToolStripPanel;
        private ToolStripContentPanel ContentPanel;
        private SplitContainer Control_AdvancedRemove_SplitContainer_Main;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private Panel panel3;
        private Button Control_AdvancedRemove_Button_SidePanel;
    }
}
