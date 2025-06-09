using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using MTM_WIP_Application.Controls.Addons;
using MTM_WIP_Application.Controls.MainForm;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MTM_WIP_Application.Forms.MainForm
{
    public partial class MainForm : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            MainForm_MenuStrip = new MenuStrip();
            MainForm_MenuStrip_File = new ToolStripMenuItem();
            MainForm_MenuStrip_File_Save = new ToolStripMenuItem();
            MainForm_MenuStrip_File_Delete = new ToolStripMenuItem();
            MainForm_MenuStrip_File_Print = new ToolStripMenuItem();
            MainForm_MenuStrip_File_Settings = new ToolStripMenuItem();
            MainForm_MenuStrip_Exit = new ToolStripMenuItem();
            MainForm_MenuStrip_Edit = new ToolStripMenuItem();
            MainForm_MenuStrip_Edit_New_Object = new ToolStripMenuItem();
            MainForm_MenuStrip_Edit_Remove_Object = new ToolStripMenuItem();
            MainForm_MenuStrip_Edit_Separator4 = new ToolStripSeparator();
            MainForm_MenuStrip_Edit_ResetLast10Buttons = new ToolStripMenuItem();
            MainForm_MenuStrip_View = new ToolStripMenuItem();
            MainForm_MenuStrip_View_PersonalHistory = new ToolStripMenuItem();
            MainForm_MenuStrip_View_Reset = new ToolStripMenuItem();
            MainForm_MenuStrip_View_Separator1 = new ToolStripSeparator();
            MainForm_MenuStrip_View_AddToInventory = new ToolStripMenuItem();
            MainForm_MenuStrip_View_RemoveFromInventory = new ToolStripMenuItem();
            MainForm_MenuStrip_View_LocationToLocation = new ToolStripMenuItem();
            MainForm_MenuStrip_View_Separator2 = new ToolStripSeparator();
            MainForm_MenuStrip_View_ViewAllWIP = new ToolStripMenuItem();
            MainForm_MenuStrip_View_ViewOutsideService = new ToolStripMenuItem();
            MainForm_MenuStrip_View_Separator3 = new ToolStripSeparator();
            MainForm_MenuStrip_View_ViewChangelog = new ToolStripMenuItem();
            MainForm_Remove_GroupBox_Main = new GroupBox();
            MainForm_Remove_Panel_Header = new Panel();
            MainForm_Remove_ComboBox_Part = new ComboBox();
            MainForm_Remove_Label_Part = new Label();
            MainForm_Remove_Label_Op = new Label();
            MainForm_Remove_ComboBox_Op = new ComboBox();
            MainForm_Remove_Panel_DataGrid = new Panel();
            MainForm_Remove_Image_NothingFound = new PictureBox();
            MainForm_Remove_DataGrid = new DataGridView();
            MainForm_Remove_Panel_Buttons = new Panel();
            MainForm_RemoveTab_Button_AdvancedSearch = new Button();
            MainForm_Remove_Button_Reset = new Button();
            MainForm_Remove_Button_Delete = new Button();
            MainForm_Remove_Button_Search = new Button();
            MainForm_TabControl = new TabControl();
            MainForm_TabControl_Inventory = new TabPage();
            MainForm_InventoryTab_SplitContainer_Middle = new SplitContainer();
            MainForm_InventoryTab = new Control_InventoryTab();
            MainForm_TabControl_Remove = new TabPage();
            MainForm_TabControl_Transfer = new TabPage();
            MainForm_Transfer_GroupBox_Main = new GroupBox();
            MainForm_Transfer_Panel_Header = new Panel();
            MainForm_Transfer_Button_Search = new Button();
            MainForm_Transfer_ComboBox_Part = new ComboBox();
            MainForm_Transfer_Label_Part = new Label();
            MainForm_Transfer_Label_Loc = new Label();
            MainForm_Transfer_ComboBox_Loc = new ComboBox();
            MainForm_Transfer_Panel_DataGrid = new Panel();
            MainForm_Transfer_Image_Nothing = new PictureBox();
            MainForm_Transfer_DataGrid = new DataGridView();
            MainForm_Transfer_Panel_Bottom = new Panel();
            MainForm_Transfer_TextBox_Qty = new TextBox();
            MainForm_Transfer_Button_Save = new Button();
            MainForm_Transfer_Label_Qty = new Label();
            MainForm_Transfer_Button_Reset = new Button();
            MainForm_StatusStrip = new StatusStrip();
            MainForm_StatusStrip_SavedStatus = new ToolStripStatusLabel();
            MainForm_StatusStrip_Disconnected = new ToolStripStatusLabel();
            MainForm_Inventory_PrintDocument = new System.Drawing.Printing.PrintDocument();
            MainForm_Inventory_PrintDialog = new PrintPreviewDialog();
            MainForm_ToolTip = new ToolTip(components);
            MainForm_Last10_Timer = new System.Windows.Forms.Timer(components);
            MainForm_SplitContainer_Lower = new SplitContainer();
            MainForm_Control_SignalStrength = new ConnectionStrengthControl();
            BottomToolStripPanel = new ToolStripPanel();
            TopToolStripPanel = new ToolStripPanel();
            RightToolStripPanel = new ToolStripPanel();
            LeftToolStripPanel = new ToolStripPanel();
            ContentPanel = new ToolStripContentPanel();
            MainForm_TableLayout = new TableLayoutPanel();
            MainForm_MenuStrip.SuspendLayout();
            MainForm_Remove_GroupBox_Main.SuspendLayout();
            MainForm_Remove_Panel_Header.SuspendLayout();
            MainForm_Remove_Panel_DataGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainForm_Remove_Image_NothingFound).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MainForm_Remove_DataGrid).BeginInit();
            MainForm_Remove_Panel_Buttons.SuspendLayout();
            MainForm_TabControl.SuspendLayout();
            MainForm_TabControl_Inventory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainForm_InventoryTab_SplitContainer_Middle).BeginInit();
            MainForm_InventoryTab_SplitContainer_Middle.Panel1.SuspendLayout();
            MainForm_InventoryTab_SplitContainer_Middle.SuspendLayout();
            MainForm_TabControl_Remove.SuspendLayout();
            MainForm_TabControl_Transfer.SuspendLayout();
            MainForm_Transfer_GroupBox_Main.SuspendLayout();
            MainForm_Transfer_Panel_Header.SuspendLayout();
            MainForm_Transfer_Panel_DataGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainForm_Transfer_Image_Nothing).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MainForm_Transfer_DataGrid).BeginInit();
            MainForm_Transfer_Panel_Bottom.SuspendLayout();
            MainForm_StatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainForm_SplitContainer_Lower).BeginInit();
            MainForm_SplitContainer_Lower.Panel1.SuspendLayout();
            MainForm_SplitContainer_Lower.Panel2.SuspendLayout();
            MainForm_SplitContainer_Lower.SuspendLayout();
            MainForm_TableLayout.SuspendLayout();
            SuspendLayout();
            // 
            // MainForm_MenuStrip
            // 
            MainForm_MenuStrip.Dock = DockStyle.Fill;
            MainForm_MenuStrip.ImageScalingSize = new Size(24, 24);
            MainForm_MenuStrip.Items.AddRange(new ToolStripItem[] { MainForm_MenuStrip_File, MainForm_MenuStrip_Edit, MainForm_MenuStrip_View });
            MainForm_MenuStrip.Location = new Point(0, 0);
            MainForm_MenuStrip.Name = "MainForm_MenuStrip";
            MainForm_MenuStrip.Size = new Size(935, 26);
            MainForm_MenuStrip.TabIndex = 1;
            // 
            // MainForm_MenuStrip_File
            // 
            MainForm_MenuStrip_File.DropDownItems.AddRange(new ToolStripItem[] { MainForm_MenuStrip_File_Save, MainForm_MenuStrip_File_Delete, MainForm_MenuStrip_File_Print, MainForm_MenuStrip_File_Settings, MainForm_MenuStrip_Exit });
            MainForm_MenuStrip_File.Name = "MainForm_MenuStrip_File";
            MainForm_MenuStrip_File.Size = new Size(37, 22);
            MainForm_MenuStrip_File.Text = "File";
            // 
            // MainForm_MenuStrip_File_Save
            // 
            MainForm_MenuStrip_File_Save.Name = "MainForm_MenuStrip_File_Save";
            MainForm_MenuStrip_File_Save.ShortcutKeys = Keys.Control | Keys.S;
            MainForm_MenuStrip_File_Save.Size = new Size(188, 22);
            MainForm_MenuStrip_File_Save.Text = "Save";
            // 
            // MainForm_MenuStrip_File_Delete
            // 
            MainForm_MenuStrip_File_Delete.Name = "MainForm_MenuStrip_File_Delete";
            MainForm_MenuStrip_File_Delete.ShortcutKeys = Keys.Delete;
            MainForm_MenuStrip_File_Delete.Size = new Size(188, 22);
            MainForm_MenuStrip_File_Delete.Text = "Delete";
            // 
            // MainForm_MenuStrip_File_Print
            // 
            MainForm_MenuStrip_File_Print.Name = "MainForm_MenuStrip_File_Print";
            MainForm_MenuStrip_File_Print.ShortcutKeys = Keys.Control | Keys.P;
            MainForm_MenuStrip_File_Print.Size = new Size(188, 22);
            MainForm_MenuStrip_File_Print.Text = "Print";
            // 
            // MainForm_MenuStrip_File_Settings
            // 
            MainForm_MenuStrip_File_Settings.Name = "MainForm_MenuStrip_File_Settings";
            MainForm_MenuStrip_File_Settings.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            MainForm_MenuStrip_File_Settings.Size = new Size(188, 22);
            MainForm_MenuStrip_File_Settings.Text = "Settings";
            // 
            // MainForm_MenuStrip_Exit
            // 
            MainForm_MenuStrip_Exit.Name = "MainForm_MenuStrip_Exit";
            MainForm_MenuStrip_Exit.ShortcutKeys = Keys.Alt | Keys.F4;
            MainForm_MenuStrip_Exit.Size = new Size(188, 22);
            MainForm_MenuStrip_Exit.Text = "Exit";
            // 
            // MainForm_MenuStrip_Edit
            // 
            MainForm_MenuStrip_Edit.DropDownItems.AddRange(new ToolStripItem[] { MainForm_MenuStrip_Edit_New_Object, MainForm_MenuStrip_Edit_Remove_Object, MainForm_MenuStrip_Edit_Separator4, MainForm_MenuStrip_Edit_ResetLast10Buttons });
            MainForm_MenuStrip_Edit.Name = "MainForm_MenuStrip_Edit";
            MainForm_MenuStrip_Edit.Size = new Size(39, 22);
            MainForm_MenuStrip_Edit.Text = "Edit";
            // 
            // MainForm_MenuStrip_Edit_New_Object
            // 
            MainForm_MenuStrip_Edit_New_Object.Name = "MainForm_MenuStrip_Edit_New_Object";
            MainForm_MenuStrip_Edit_New_Object.Size = new Size(185, 22);
            MainForm_MenuStrip_Edit_New_Object.Text = "New Object";
            // 
            // MainForm_MenuStrip_Edit_Remove_Object
            // 
            MainForm_MenuStrip_Edit_Remove_Object.Name = "MainForm_MenuStrip_Edit_Remove_Object";
            MainForm_MenuStrip_Edit_Remove_Object.Size = new Size(185, 22);
            MainForm_MenuStrip_Edit_Remove_Object.Text = "Remove Object";
            // 
            // MainForm_MenuStrip_Edit_Separator4
            // 
            MainForm_MenuStrip_Edit_Separator4.Name = "MainForm_MenuStrip_Edit_Separator4";
            MainForm_MenuStrip_Edit_Separator4.Size = new Size(182, 6);
            // 
            // MainForm_MenuStrip_Edit_ResetLast10Buttons
            // 
            MainForm_MenuStrip_Edit_ResetLast10Buttons.Name = "MainForm_MenuStrip_Edit_ResetLast10Buttons";
            MainForm_MenuStrip_Edit_ResetLast10Buttons.Size = new Size(185, 22);
            MainForm_MenuStrip_Edit_ResetLast10Buttons.Text = "Reset Last 10 Buttons";
            // 
            // MainForm_MenuStrip_View
            // 
            MainForm_MenuStrip_View.DropDownItems.AddRange(new ToolStripItem[] { MainForm_MenuStrip_View_PersonalHistory, MainForm_MenuStrip_View_Reset, MainForm_MenuStrip_View_Separator1, MainForm_MenuStrip_View_AddToInventory, MainForm_MenuStrip_View_RemoveFromInventory, MainForm_MenuStrip_View_LocationToLocation, MainForm_MenuStrip_View_Separator2, MainForm_MenuStrip_View_ViewAllWIP, MainForm_MenuStrip_View_ViewOutsideService, MainForm_MenuStrip_View_Separator3, MainForm_MenuStrip_View_ViewChangelog });
            MainForm_MenuStrip_View.Name = "MainForm_MenuStrip_View";
            MainForm_MenuStrip_View.Size = new Size(44, 22);
            MainForm_MenuStrip_View.Text = "View";
            // 
            // MainForm_MenuStrip_View_PersonalHistory
            // 
            MainForm_MenuStrip_View_PersonalHistory.Name = "MainForm_MenuStrip_View_PersonalHistory";
            MainForm_MenuStrip_View_PersonalHistory.ShortcutKeys = Keys.Control | Keys.H;
            MainForm_MenuStrip_View_PersonalHistory.Size = new Size(234, 22);
            MainForm_MenuStrip_View_PersonalHistory.Text = "Personal History";
            // 
            // MainForm_MenuStrip_View_Reset
            // 
            MainForm_MenuStrip_View_Reset.Name = "MainForm_MenuStrip_View_Reset";
            MainForm_MenuStrip_View_Reset.ShortcutKeys = Keys.Control | Keys.R;
            MainForm_MenuStrip_View_Reset.Size = new Size(234, 22);
            MainForm_MenuStrip_View_Reset.Text = "Reset New Transaction";
            // 
            // MainForm_MenuStrip_View_Separator1
            // 
            MainForm_MenuStrip_View_Separator1.Name = "MainForm_MenuStrip_View_Separator1";
            MainForm_MenuStrip_View_Separator1.Size = new Size(231, 6);
            // 
            // MainForm_MenuStrip_View_AddToInventory
            // 
            MainForm_MenuStrip_View_AddToInventory.Name = "MainForm_MenuStrip_View_AddToInventory";
            MainForm_MenuStrip_View_AddToInventory.ShortcutKeys = Keys.Control | Keys.D1;
            MainForm_MenuStrip_View_AddToInventory.Size = new Size(234, 22);
            MainForm_MenuStrip_View_AddToInventory.Text = "New Transaction";
            // 
            // MainForm_MenuStrip_View_RemoveFromInventory
            // 
            MainForm_MenuStrip_View_RemoveFromInventory.Name = "MainForm_MenuStrip_View_RemoveFromInventory";
            MainForm_MenuStrip_View_RemoveFromInventory.ShortcutKeys = Keys.Control | Keys.D2;
            MainForm_MenuStrip_View_RemoveFromInventory.Size = new Size(234, 22);
            MainForm_MenuStrip_View_RemoveFromInventory.Text = "Remove";
            // 
            // MainForm_MenuStrip_View_LocationToLocation
            // 
            MainForm_MenuStrip_View_LocationToLocation.Name = "MainForm_MenuStrip_View_LocationToLocation";
            MainForm_MenuStrip_View_LocationToLocation.ShortcutKeys = Keys.Control | Keys.D3;
            MainForm_MenuStrip_View_LocationToLocation.Size = new Size(234, 22);
            MainForm_MenuStrip_View_LocationToLocation.Text = "Transfer";
            // 
            // MainForm_MenuStrip_View_Separator2
            // 
            MainForm_MenuStrip_View_Separator2.Name = "MainForm_MenuStrip_View_Separator2";
            MainForm_MenuStrip_View_Separator2.Size = new Size(231, 6);
            // 
            // MainForm_MenuStrip_View_ViewAllWIP
            // 
            MainForm_MenuStrip_View_ViewAllWIP.Name = "MainForm_MenuStrip_View_ViewAllWIP";
            MainForm_MenuStrip_View_ViewAllWIP.Size = new Size(234, 22);
            MainForm_MenuStrip_View_ViewAllWIP.Text = "View All WIP";
            // 
            // MainForm_MenuStrip_View_ViewOutsideService
            // 
            MainForm_MenuStrip_View_ViewOutsideService.Name = "MainForm_MenuStrip_View_ViewOutsideService";
            MainForm_MenuStrip_View_ViewOutsideService.Size = new Size(234, 22);
            MainForm_MenuStrip_View_ViewOutsideService.Text = "View Outside Service";
            // 
            // MainForm_MenuStrip_View_Separator3
            // 
            MainForm_MenuStrip_View_Separator3.Name = "MainForm_MenuStrip_View_Separator3";
            MainForm_MenuStrip_View_Separator3.Size = new Size(231, 6);
            // 
            // MainForm_MenuStrip_View_ViewChangelog
            // 
            MainForm_MenuStrip_View_ViewChangelog.Name = "MainForm_MenuStrip_View_ViewChangelog";
            MainForm_MenuStrip_View_ViewChangelog.Size = new Size(234, 22);
            MainForm_MenuStrip_View_ViewChangelog.Text = "View Changelog";
            // 
            // MainForm_Remove_GroupBox_Main
            // 
            MainForm_Remove_GroupBox_Main.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MainForm_Remove_GroupBox_Main.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MainForm_Remove_GroupBox_Main.Controls.Add(MainForm_Remove_Panel_Header);
            MainForm_Remove_GroupBox_Main.Controls.Add(MainForm_Remove_Panel_DataGrid);
            MainForm_Remove_GroupBox_Main.Controls.Add(MainForm_Remove_Panel_Buttons);
            MainForm_Remove_GroupBox_Main.FlatStyle = FlatStyle.Flat;
            MainForm_Remove_GroupBox_Main.Location = new Point(8, 6);
            MainForm_Remove_GroupBox_Main.Name = "MainForm_Remove_GroupBox_Main";
            MainForm_Remove_GroupBox_Main.Size = new Size(910, 378);
            MainForm_Remove_GroupBox_Main.TabIndex = 16;
            MainForm_Remove_GroupBox_Main.TabStop = false;
            MainForm_Remove_GroupBox_Main.Text = "Part Lookup and Remove";
            // 
            // MainForm_Remove_Panel_Header
            // 
            MainForm_Remove_Panel_Header.Controls.Add(MainForm_Remove_ComboBox_Part);
            MainForm_Remove_Panel_Header.Controls.Add(MainForm_Remove_Label_Part);
            MainForm_Remove_Panel_Header.Controls.Add(MainForm_Remove_Label_Op);
            MainForm_Remove_Panel_Header.Controls.Add(MainForm_Remove_ComboBox_Op);
            MainForm_Remove_Panel_Header.Dock = DockStyle.Top;
            MainForm_Remove_Panel_Header.Location = new Point(3, 19);
            MainForm_Remove_Panel_Header.Name = "MainForm_Remove_Panel_Header";
            MainForm_Remove_Panel_Header.Size = new Size(904, 36);
            MainForm_Remove_Panel_Header.TabIndex = 22;
            // 
            // MainForm_Remove_ComboBox_Part
            // 
            MainForm_Remove_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            MainForm_Remove_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            MainForm_Remove_ComboBox_Part.FormattingEnabled = true;
            MainForm_Remove_ComboBox_Part.Location = new Point(86, 7);
            MainForm_Remove_ComboBox_Part.Name = "MainForm_Remove_ComboBox_Part";
            MainForm_Remove_ComboBox_Part.Size = new Size(564, 23);
            MainForm_Remove_ComboBox_Part.TabIndex = 1;
            // 
            // MainForm_Remove_Label_Part
            // 
            MainForm_Remove_Label_Part.AutoSize = true;
            MainForm_Remove_Label_Part.Location = new Point(3, 11);
            MainForm_Remove_Label_Part.Name = "MainForm_Remove_Label_Part";
            MainForm_Remove_Label_Part.Size = new Size(78, 15);
            MainForm_Remove_Label_Part.TabIndex = 4;
            MainForm_Remove_Label_Part.Text = "Part Number:";
            // 
            // MainForm_Remove_Label_Op
            // 
            MainForm_Remove_Label_Op.AutoSize = true;
            MainForm_Remove_Label_Op.Location = new Point(656, 11);
            MainForm_Remove_Label_Op.Name = "MainForm_Remove_Label_Op";
            MainForm_Remove_Label_Op.Size = new Size(26, 15);
            MainForm_Remove_Label_Op.TabIndex = 5;
            MainForm_Remove_Label_Op.Text = "Op:";
            // 
            // MainForm_Remove_ComboBox_Op
            // 
            MainForm_Remove_ComboBox_Op.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            MainForm_Remove_ComboBox_Op.AutoCompleteSource = AutoCompleteSource.ListItems;
            MainForm_Remove_ComboBox_Op.FormattingEnabled = true;
            MainForm_Remove_ComboBox_Op.Location = new Point(688, 7);
            MainForm_Remove_ComboBox_Op.Name = "MainForm_Remove_ComboBox_Op";
            MainForm_Remove_ComboBox_Op.Size = new Size(117, 23);
            MainForm_Remove_ComboBox_Op.TabIndex = 2;
            // 
            // MainForm_Remove_Panel_DataGrid
            // 
            MainForm_Remove_Panel_DataGrid.Controls.Add(MainForm_Remove_Image_NothingFound);
            MainForm_Remove_Panel_DataGrid.Controls.Add(MainForm_Remove_DataGrid);
            MainForm_Remove_Panel_DataGrid.Location = new Point(3, 61);
            MainForm_Remove_Panel_DataGrid.Name = "MainForm_Remove_Panel_DataGrid";
            MainForm_Remove_Panel_DataGrid.Size = new Size(811, 281);
            MainForm_Remove_Panel_DataGrid.TabIndex = 21;
            // 
            // MainForm_Remove_Image_NothingFound
            // 
            MainForm_Remove_Image_NothingFound.Dock = DockStyle.Fill;
            MainForm_Remove_Image_NothingFound.ErrorImage = null;
            MainForm_Remove_Image_NothingFound.InitialImage = null;
            MainForm_Remove_Image_NothingFound.Location = new Point(0, 0);
            MainForm_Remove_Image_NothingFound.Name = "MainForm_Remove_Image_NothingFound";
            MainForm_Remove_Image_NothingFound.Size = new Size(811, 281);
            MainForm_Remove_Image_NothingFound.SizeMode = PictureBoxSizeMode.CenterImage;
            MainForm_Remove_Image_NothingFound.TabIndex = 6;
            MainForm_Remove_Image_NothingFound.TabStop = false;
            // 
            // MainForm_Remove_DataGrid
            // 
            MainForm_Remove_DataGrid.AllowUserToAddRows = false;
            MainForm_Remove_DataGrid.AllowUserToDeleteRows = false;
            MainForm_Remove_DataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            MainForm_Remove_DataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            MainForm_Remove_DataGrid.BorderStyle = BorderStyle.Fixed3D;
            MainForm_Remove_DataGrid.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            MainForm_Remove_DataGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            MainForm_Remove_DataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            MainForm_Remove_DataGrid.ColumnHeadersHeight = 34;
            MainForm_Remove_DataGrid.Dock = DockStyle.Fill;
            MainForm_Remove_DataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            MainForm_Remove_DataGrid.Location = new Point(0, 0);
            MainForm_Remove_DataGrid.Name = "MainForm_Remove_DataGrid";
            MainForm_Remove_DataGrid.ReadOnly = true;
            MainForm_Remove_DataGrid.RowHeadersWidth = 62;
            MainForm_Remove_DataGrid.RowTemplate.ReadOnly = true;
            MainForm_Remove_DataGrid.RowTemplate.Resizable = DataGridViewTriState.True;
            MainForm_Remove_DataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            MainForm_Remove_DataGrid.ShowCellErrors = false;
            MainForm_Remove_DataGrid.ShowCellToolTips = false;
            MainForm_Remove_DataGrid.ShowEditingIcon = false;
            MainForm_Remove_DataGrid.ShowRowErrors = false;
            MainForm_Remove_DataGrid.Size = new Size(811, 281);
            MainForm_Remove_DataGrid.StandardTab = true;
            MainForm_Remove_DataGrid.TabIndex = 4;
            // 
            // MainForm_Remove_Panel_Buttons
            // 
            MainForm_Remove_Panel_Buttons.Controls.Add(MainForm_RemoveTab_Button_AdvancedSearch);
            MainForm_Remove_Panel_Buttons.Controls.Add(MainForm_Remove_Button_Reset);
            MainForm_Remove_Panel_Buttons.Controls.Add(MainForm_Remove_Button_Delete);
            MainForm_Remove_Panel_Buttons.Controls.Add(MainForm_Remove_Button_Search);
            MainForm_Remove_Panel_Buttons.Dock = DockStyle.Bottom;
            MainForm_Remove_Panel_Buttons.Location = new Point(3, 334);
            MainForm_Remove_Panel_Buttons.Name = "MainForm_Remove_Panel_Buttons";
            MainForm_Remove_Panel_Buttons.Padding = new Padding(3);
            MainForm_Remove_Panel_Buttons.Size = new Size(904, 41);
            MainForm_Remove_Panel_Buttons.TabIndex = 20;
            // 
            // MainForm_RemoveTab_Button_AdvancedSearch
            // 
            MainForm_RemoveTab_Button_AdvancedSearch.ForeColor = Color.DarkRed;
            MainForm_RemoveTab_Button_AdvancedSearch.Location = new Point(148, 6);
            MainForm_RemoveTab_Button_AdvancedSearch.Name = "MainForm_RemoveTab_Button_AdvancedSearch";
            MainForm_RemoveTab_Button_AdvancedSearch.Size = new Size(236, 32);
            MainForm_RemoveTab_Button_AdvancedSearch.TabIndex = 13;
            MainForm_RemoveTab_Button_AdvancedSearch.Text = "Advanced Search (Ctrl + Alt + S)";
            MainForm_ToolTip.SetToolTip(MainForm_RemoveTab_Button_AdvancedSearch, "Shortcut: Ctrl+Alt+S");
            MainForm_RemoveTab_Button_AdvancedSearch.UseVisualStyleBackColor = true;
            // 
            // MainForm_Remove_Button_Reset
            // 
            MainForm_Remove_Button_Reset.Location = new Point(581, 6);
            MainForm_Remove_Button_Reset.Name = "MainForm_Remove_Button_Reset";
            MainForm_Remove_Button_Reset.Size = new Size(131, 32);
            MainForm_Remove_Button_Reset.TabIndex = 5;
            MainForm_Remove_Button_Reset.TabStop = false;
            MainForm_Remove_Button_Reset.Text = "Reset ( Ctrl + R )";
            MainForm_ToolTip.SetToolTip(MainForm_Remove_Button_Reset, "Shortcut: Ctrl+R");
            MainForm_Remove_Button_Reset.UseVisualStyleBackColor = true;
            // 
            // MainForm_Remove_Button_Delete
            // 
            MainForm_Remove_Button_Delete.Location = new Point(718, 6);
            MainForm_Remove_Button_Delete.Name = "MainForm_Remove_Button_Delete";
            MainForm_Remove_Button_Delete.Size = new Size(87, 32);
            MainForm_Remove_Button_Delete.TabIndex = 8;
            MainForm_Remove_Button_Delete.Text = "Delete ( Del )";
            MainForm_ToolTip.SetToolTip(MainForm_Remove_Button_Delete, "Shortcut: Del");
            MainForm_Remove_Button_Delete.UseVisualStyleBackColor = true;
            // 
            // MainForm_Remove_Button_Search
            // 
            MainForm_Remove_Button_Search.Location = new Point(6, 6);
            MainForm_Remove_Button_Search.Name = "MainForm_Remove_Button_Search";
            MainForm_Remove_Button_Search.Size = new Size(136, 32);
            MainForm_Remove_Button_Search.TabIndex = 3;
            MainForm_Remove_Button_Search.Text = "Search ( Ctrl + S )";
            MainForm_ToolTip.SetToolTip(MainForm_Remove_Button_Search, "Shortcut: Ctrl+S");
            MainForm_Remove_Button_Search.UseVisualStyleBackColor = true;
            // 
            // MainForm_TabControl
            // 
            MainForm_TabControl.Controls.Add(MainForm_TabControl_Inventory);
            MainForm_TabControl.Controls.Add(MainForm_TabControl_Remove);
            MainForm_TabControl.Controls.Add(MainForm_TabControl_Transfer);
            MainForm_TabControl.Dock = DockStyle.Fill;
            MainForm_TabControl.Location = new Point(3, 29);
            MainForm_TabControl.Multiline = true;
            MainForm_TabControl.Name = "MainForm_TabControl";
            MainForm_TabControl.SelectedIndex = 0;
            MainForm_TabControl.ShowToolTips = true;
            MainForm_TabControl.Size = new Size(929, 418);
            MainForm_TabControl.SizeMode = TabSizeMode.FillToRight;
            MainForm_TabControl.TabIndex = 90;
            MainForm_TabControl.TabStop = false;
            // 
            // MainForm_TabControl_Inventory
            // 
            MainForm_TabControl_Inventory.Controls.Add(MainForm_InventoryTab_SplitContainer_Middle);
            MainForm_TabControl_Inventory.Location = new Point(4, 24);
            MainForm_TabControl_Inventory.Name = "MainForm_TabControl_Inventory";
            MainForm_TabControl_Inventory.Padding = new Padding(3);
            MainForm_TabControl_Inventory.Size = new Size(921, 390);
            MainForm_TabControl_Inventory.TabIndex = 0;
            MainForm_TabControl_Inventory.Text = "New Transaction (Ctrl+1)";
            MainForm_TabControl_Inventory.ToolTipText = "Shortcut: Ctrl+1";
            MainForm_TabControl_Inventory.UseVisualStyleBackColor = true;
            // 
            // MainForm_InventoryTab_SplitContainer_Middle
            // 
            MainForm_InventoryTab_SplitContainer_Middle.Dock = DockStyle.Fill;
            MainForm_InventoryTab_SplitContainer_Middle.Location = new Point(3, 3);
            MainForm_InventoryTab_SplitContainer_Middle.Name = "MainForm_InventoryTab_SplitContainer_Middle";
            // 
            // MainForm_InventoryTab_SplitContainer_Middle.Panel1
            // 
            MainForm_InventoryTab_SplitContainer_Middle.Panel1.Controls.Add(MainForm_InventoryTab);
            MainForm_InventoryTab_SplitContainer_Middle.Panel1MinSize = 815;
            MainForm_InventoryTab_SplitContainer_Middle.Panel2MinSize = 0;
            MainForm_InventoryTab_SplitContainer_Middle.Size = new Size(915, 384);
            MainForm_InventoryTab_SplitContainer_Middle.SplitterDistance = 815;
            MainForm_InventoryTab_SplitContainer_Middle.TabIndex = 0;
            // 
            // MainForm_InventoryTab
            // 
            MainForm_InventoryTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MainForm_InventoryTab.Dock = DockStyle.Fill;
            MainForm_InventoryTab.Location = new Point(0, 0);
            MainForm_InventoryTab.Name = "MainForm_InventoryTab";
            MainForm_InventoryTab.Size = new Size(815, 384);
            MainForm_InventoryTab.TabIndex = 0;
            // 
            // MainForm_TabControl_Remove
            // 
            MainForm_TabControl_Remove.Controls.Add(MainForm_Remove_GroupBox_Main);
            MainForm_TabControl_Remove.Location = new Point(4, 24);
            MainForm_TabControl_Remove.Name = "MainForm_TabControl_Remove";
            MainForm_TabControl_Remove.Padding = new Padding(3);
            MainForm_TabControl_Remove.Size = new Size(921, 390);
            MainForm_TabControl_Remove.TabIndex = 1;
            MainForm_TabControl_Remove.Text = "Remove (Ctrl + 2)";
            MainForm_TabControl_Remove.ToolTipText = "Shortcut: Ctrl+2";
            MainForm_TabControl_Remove.UseVisualStyleBackColor = true;
            // 
            // MainForm_TabControl_Transfer
            // 
            MainForm_TabControl_Transfer.Controls.Add(MainForm_Transfer_GroupBox_Main);
            MainForm_TabControl_Transfer.Location = new Point(4, 24);
            MainForm_TabControl_Transfer.Name = "MainForm_TabControl_Transfer";
            MainForm_TabControl_Transfer.Padding = new Padding(3);
            MainForm_TabControl_Transfer.Size = new Size(921, 390);
            MainForm_TabControl_Transfer.TabIndex = 2;
            MainForm_TabControl_Transfer.Text = "Transfer (Ctrl+3)";
            MainForm_TabControl_Transfer.ToolTipText = "Shortcut: Ctrl+3";
            MainForm_TabControl_Transfer.UseVisualStyleBackColor = true;
            // 
            // MainForm_Transfer_GroupBox_Main
            // 
            MainForm_Transfer_GroupBox_Main.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MainForm_Transfer_GroupBox_Main.Controls.Add(MainForm_Transfer_Panel_Header);
            MainForm_Transfer_GroupBox_Main.Controls.Add(MainForm_Transfer_Panel_DataGrid);
            MainForm_Transfer_GroupBox_Main.Controls.Add(MainForm_Transfer_Panel_Bottom);
            MainForm_Transfer_GroupBox_Main.Location = new Point(8, 6);
            MainForm_Transfer_GroupBox_Main.Name = "MainForm_Transfer_GroupBox_Main";
            MainForm_Transfer_GroupBox_Main.Size = new Size(910, 377);
            MainForm_Transfer_GroupBox_Main.TabIndex = 0;
            MainForm_Transfer_GroupBox_Main.TabStop = false;
            MainForm_Transfer_GroupBox_Main.Text = "Location Change";
            // 
            // MainForm_Transfer_Panel_Header
            // 
            MainForm_Transfer_Panel_Header.Controls.Add(MainForm_Transfer_Button_Search);
            MainForm_Transfer_Panel_Header.Controls.Add(MainForm_Transfer_ComboBox_Part);
            MainForm_Transfer_Panel_Header.Controls.Add(MainForm_Transfer_Label_Part);
            MainForm_Transfer_Panel_Header.Controls.Add(MainForm_Transfer_Label_Loc);
            MainForm_Transfer_Panel_Header.Controls.Add(MainForm_Transfer_ComboBox_Loc);
            MainForm_Transfer_Panel_Header.Dock = DockStyle.Top;
            MainForm_Transfer_Panel_Header.Location = new Point(3, 19);
            MainForm_Transfer_Panel_Header.Name = "MainForm_Transfer_Panel_Header";
            MainForm_Transfer_Panel_Header.Size = new Size(904, 36);
            MainForm_Transfer_Panel_Header.TabIndex = 23;
            // 
            // MainForm_Transfer_Button_Search
            // 
            MainForm_Transfer_Button_Search.Dock = DockStyle.Right;
            MainForm_Transfer_Button_Search.FlatStyle = FlatStyle.System;
            MainForm_Transfer_Button_Search.Location = new Point(817, 0);
            MainForm_Transfer_Button_Search.Name = "MainForm_Transfer_Button_Search";
            MainForm_Transfer_Button_Search.Size = new Size(87, 36);
            MainForm_Transfer_Button_Search.TabIndex = 2;
            MainForm_Transfer_Button_Search.Text = "Search";
            MainForm_ToolTip.SetToolTip(MainForm_Transfer_Button_Search, "Shortcut: None");
            MainForm_Transfer_Button_Search.UseVisualStyleBackColor = true;
            // 
            // MainForm_Transfer_ComboBox_Part
            // 
            MainForm_Transfer_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            MainForm_Transfer_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            MainForm_Transfer_ComboBox_Part.FormattingEnabled = true;
            MainForm_Transfer_ComboBox_Part.Location = new Point(86, 7);
            MainForm_Transfer_ComboBox_Part.Name = "MainForm_Transfer_ComboBox_Part";
            MainForm_Transfer_ComboBox_Part.Size = new Size(282, 23);
            MainForm_Transfer_ComboBox_Part.TabIndex = 1;
            // 
            // MainForm_Transfer_Label_Part
            // 
            MainForm_Transfer_Label_Part.AutoSize = true;
            MainForm_Transfer_Label_Part.Location = new Point(3, 11);
            MainForm_Transfer_Label_Part.Name = "MainForm_Transfer_Label_Part";
            MainForm_Transfer_Label_Part.Size = new Size(78, 15);
            MainForm_Transfer_Label_Part.TabIndex = 4;
            MainForm_Transfer_Label_Part.Text = "Part Number:";
            // 
            // MainForm_Transfer_Label_Loc
            // 
            MainForm_Transfer_Label_Loc.AutoSize = true;
            MainForm_Transfer_Label_Loc.Location = new Point(374, 10);
            MainForm_Transfer_Label_Loc.Name = "MainForm_Transfer_Label_Loc";
            MainForm_Transfer_Label_Loc.Size = new Size(83, 15);
            MainForm_Transfer_Label_Loc.TabIndex = 18;
            MainForm_Transfer_Label_Loc.Text = "New Location:";
            // 
            // MainForm_Transfer_ComboBox_Loc
            // 
            MainForm_Transfer_ComboBox_Loc.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            MainForm_Transfer_ComboBox_Loc.AutoCompleteSource = AutoCompleteSource.ListItems;
            MainForm_Transfer_ComboBox_Loc.FormattingEnabled = true;
            MainForm_Transfer_ComboBox_Loc.Location = new Point(463, 7);
            MainForm_Transfer_ComboBox_Loc.Name = "MainForm_Transfer_ComboBox_Loc";
            MainForm_Transfer_ComboBox_Loc.Size = new Size(255, 23);
            MainForm_Transfer_ComboBox_Loc.TabIndex = 4;
            // 
            // MainForm_Transfer_Panel_DataGrid
            // 
            MainForm_Transfer_Panel_DataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MainForm_Transfer_Panel_DataGrid.Controls.Add(MainForm_Transfer_Image_Nothing);
            MainForm_Transfer_Panel_DataGrid.Controls.Add(MainForm_Transfer_DataGrid);
            MainForm_Transfer_Panel_DataGrid.Location = new Point(3, 61);
            MainForm_Transfer_Panel_DataGrid.Name = "MainForm_Transfer_Panel_DataGrid";
            MainForm_Transfer_Panel_DataGrid.Size = new Size(901, 272);
            MainForm_Transfer_Panel_DataGrid.TabIndex = 25;
            // 
            // MainForm_Transfer_Image_Nothing
            // 
            MainForm_Transfer_Image_Nothing.Dock = DockStyle.Fill;
            MainForm_Transfer_Image_Nothing.ErrorImage = null;
            MainForm_Transfer_Image_Nothing.InitialImage = null;
            MainForm_Transfer_Image_Nothing.Location = new Point(0, 0);
            MainForm_Transfer_Image_Nothing.Name = "MainForm_Transfer_Image_Nothing";
            MainForm_Transfer_Image_Nothing.Size = new Size(901, 272);
            MainForm_Transfer_Image_Nothing.SizeMode = PictureBoxSizeMode.CenterImage;
            MainForm_Transfer_Image_Nothing.TabIndex = 19;
            MainForm_Transfer_Image_Nothing.TabStop = false;
            MainForm_Transfer_Image_Nothing.Visible = false;
            // 
            // MainForm_Transfer_DataGrid
            // 
            MainForm_Transfer_DataGrid.AllowUserToAddRows = false;
            MainForm_Transfer_DataGrid.AllowUserToDeleteRows = false;
            MainForm_Transfer_DataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            MainForm_Transfer_DataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            MainForm_Transfer_DataGrid.BorderStyle = BorderStyle.Fixed3D;
            MainForm_Transfer_DataGrid.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            MainForm_Transfer_DataGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            MainForm_Transfer_DataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            MainForm_Transfer_DataGrid.ColumnHeadersHeight = 34;
            MainForm_Transfer_DataGrid.Dock = DockStyle.Fill;
            MainForm_Transfer_DataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            MainForm_Transfer_DataGrid.Location = new Point(0, 0);
            MainForm_Transfer_DataGrid.MultiSelect = false;
            MainForm_Transfer_DataGrid.Name = "MainForm_Transfer_DataGrid";
            MainForm_Transfer_DataGrid.ReadOnly = true;
            MainForm_Transfer_DataGrid.RowHeadersWidth = 62;
            MainForm_Transfer_DataGrid.RowTemplate.ReadOnly = true;
            MainForm_Transfer_DataGrid.RowTemplate.Resizable = DataGridViewTriState.True;
            MainForm_Transfer_DataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            MainForm_Transfer_DataGrid.ShowCellErrors = false;
            MainForm_Transfer_DataGrid.ShowCellToolTips = false;
            MainForm_Transfer_DataGrid.ShowEditingIcon = false;
            MainForm_Transfer_DataGrid.ShowRowErrors = false;
            MainForm_Transfer_DataGrid.Size = new Size(901, 272);
            MainForm_Transfer_DataGrid.StandardTab = true;
            MainForm_Transfer_DataGrid.TabIndex = 5;
            MainForm_Transfer_DataGrid.TabStop = false;
            // 
            // MainForm_Transfer_Panel_Bottom
            // 
            MainForm_Transfer_Panel_Bottom.Controls.Add(MainForm_Transfer_TextBox_Qty);
            MainForm_Transfer_Panel_Bottom.Controls.Add(MainForm_Transfer_Button_Save);
            MainForm_Transfer_Panel_Bottom.Controls.Add(MainForm_Transfer_Label_Qty);
            MainForm_Transfer_Panel_Bottom.Controls.Add(MainForm_Transfer_Button_Reset);
            MainForm_Transfer_Panel_Bottom.Dock = DockStyle.Bottom;
            MainForm_Transfer_Panel_Bottom.Location = new Point(3, 339);
            MainForm_Transfer_Panel_Bottom.Name = "MainForm_Transfer_Panel_Bottom";
            MainForm_Transfer_Panel_Bottom.Size = new Size(904, 35);
            MainForm_Transfer_Panel_Bottom.TabIndex = 24;
            // 
            // MainForm_Transfer_TextBox_Qty
            // 
            MainForm_Transfer_TextBox_Qty.Location = new Point(65, 6);
            MainForm_Transfer_TextBox_Qty.Name = "MainForm_Transfer_TextBox_Qty";
            MainForm_Transfer_TextBox_Qty.Size = new Size(187, 23);
            MainForm_Transfer_TextBox_Qty.TabIndex = 3;
            // 
            // MainForm_Transfer_Button_Save
            // 
            MainForm_Transfer_Button_Save.Dock = DockStyle.Right;
            MainForm_Transfer_Button_Save.FlatStyle = FlatStyle.System;
            MainForm_Transfer_Button_Save.Location = new Point(732, 0);
            MainForm_Transfer_Button_Save.Name = "MainForm_Transfer_Button_Save";
            MainForm_Transfer_Button_Save.Size = new Size(86, 35);
            MainForm_Transfer_Button_Save.TabIndex = 6;
            MainForm_Transfer_Button_Save.Text = "Save";
            MainForm_ToolTip.SetToolTip(MainForm_Transfer_Button_Save, "Shortcut: Ctrl+S");
            MainForm_Transfer_Button_Save.UseVisualStyleBackColor = true;
            // 
            // MainForm_Transfer_Label_Qty
            // 
            MainForm_Transfer_Label_Qty.AutoSize = true;
            MainForm_Transfer_Label_Qty.Location = new Point(3, 10);
            MainForm_Transfer_Label_Qty.Name = "MainForm_Transfer_Label_Qty";
            MainForm_Transfer_Label_Qty.Size = new Size(0, 15);
            MainForm_Transfer_Label_Qty.TabIndex = 7;
            MainForm_ToolTip.SetToolTip(MainForm_Transfer_Label_Qty, "Enter an amount to have the application subtract\r\nthat amount from your selected location.\r\n\r\nLeave this blank to transfer the entire amount.");
            // 
            // MainForm_Transfer_Button_Reset
            // 
            MainForm_Transfer_Button_Reset.Dock = DockStyle.Right;
            MainForm_Transfer_Button_Reset.FlatStyle = FlatStyle.System;
            MainForm_Transfer_Button_Reset.Location = new Point(818, 0);
            MainForm_Transfer_Button_Reset.Name = "MainForm_Transfer_Button_Reset";
            MainForm_Transfer_Button_Reset.Size = new Size(86, 35);
            MainForm_Transfer_Button_Reset.TabIndex = 19;
            MainForm_Transfer_Button_Reset.Text = "Reset";
            MainForm_ToolTip.SetToolTip(MainForm_Transfer_Button_Reset, "Shortcut: Ctrl+R");
            MainForm_Transfer_Button_Reset.UseVisualStyleBackColor = true;
            // 
            // MainForm_StatusStrip
            // 
            MainForm_StatusStrip.Dock = DockStyle.Fill;
            MainForm_StatusStrip.ImageScalingSize = new Size(24, 24);
            MainForm_StatusStrip.Items.AddRange(new ToolStripItem[] { MainForm_StatusStrip_SavedStatus, MainForm_StatusStrip_Disconnected });
            MainForm_StatusStrip.Location = new Point(0, 0);
            MainForm_StatusStrip.Name = "MainForm_StatusStrip";
            MainForm_StatusStrip.RenderMode = ToolStripRenderMode.Professional;
            MainForm_StatusStrip.Size = new Size(862, 25);
            MainForm_StatusStrip.SizingGrip = false;
            MainForm_StatusStrip.TabIndex = 18;
            // 
            // MainForm_StatusStrip_SavedStatus
            // 
            MainForm_StatusStrip_SavedStatus.Name = "MainForm_StatusStrip_SavedStatus";
            MainForm_StatusStrip_SavedStatus.Size = new Size(0, 20);
            // 
            // MainForm_StatusStrip_Disconnected
            // 
            MainForm_StatusStrip_Disconnected.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic);
            MainForm_StatusStrip_Disconnected.ForeColor = Color.Red;
            MainForm_StatusStrip_Disconnected.Name = "MainForm_StatusStrip_Disconnected";
            MainForm_StatusStrip_Disconnected.Size = new Size(240, 20);
            MainForm_StatusStrip_Disconnected.Text = "Disconnected from Server, please standby...";
            MainForm_StatusStrip_Disconnected.Visible = false;
            // 
            // MainForm_Inventory_PrintDialog
            // 
            MainForm_Inventory_PrintDialog.AutoScrollMargin = new Size(0, 0);
            MainForm_Inventory_PrintDialog.AutoScrollMinSize = new Size(0, 0);
            MainForm_Inventory_PrintDialog.ClientSize = new Size(400, 300);
            MainForm_Inventory_PrintDialog.Enabled = true;
            MainForm_Inventory_PrintDialog.Icon = (Icon)resources.GetObject("MainForm_Inventory_PrintDialog.Icon");
            MainForm_Inventory_PrintDialog.Name = "MainForm_Inventory_PrintDialog";
            MainForm_Inventory_PrintDialog.Visible = false;
            // 
            // MainForm_Last10_Timer
            // 
            MainForm_Last10_Timer.Enabled = true;
            MainForm_Last10_Timer.Interval = 15000;
            // 
            // MainForm_SplitContainer_Lower
            // 
            MainForm_SplitContainer_Lower.Dock = DockStyle.Fill;
            MainForm_SplitContainer_Lower.IsSplitterFixed = true;
            MainForm_SplitContainer_Lower.Location = new Point(3, 453);
            MainForm_SplitContainer_Lower.Name = "MainForm_SplitContainer_Lower";
            // 
            // MainForm_SplitContainer_Lower.Panel1
            // 
            MainForm_SplitContainer_Lower.Panel1.Controls.Add(MainForm_StatusStrip);
            // 
            // MainForm_SplitContainer_Lower.Panel2
            // 
            MainForm_SplitContainer_Lower.Panel2.Controls.Add(MainForm_Control_SignalStrength);
            MainForm_SplitContainer_Lower.Panel2.Padding = new Padding(4);
            MainForm_SplitContainer_Lower.Size = new Size(929, 25);
            MainForm_SplitContainer_Lower.SplitterDistance = 862;
            MainForm_SplitContainer_Lower.SplitterWidth = 1;
            MainForm_SplitContainer_Lower.TabIndex = 92;
            // 
            // MainForm_Control_SignalStrength
            // 
            MainForm_Control_SignalStrength.BackColor = SystemColors.Control;
            MainForm_Control_SignalStrength.Dock = DockStyle.Bottom;
            MainForm_Control_SignalStrength.Location = new Point(4, 6);
            MainForm_Control_SignalStrength.Margin = new Padding(0);
            MainForm_Control_SignalStrength.Name = "MainForm_Control_SignalStrength";
            MainForm_Control_SignalStrength.Ping = -1;
            MainForm_Control_SignalStrength.Size = new Size(58, 15);
            MainForm_Control_SignalStrength.Strength = 0;
            MainForm_Control_SignalStrength.TabIndex = 0;
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
            ContentPanel.Size = new Size(125, 175);
            // 
            // MainForm_TableLayout
            // 
            MainForm_TableLayout.ColumnCount = 1;
            MainForm_TableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            MainForm_TableLayout.Controls.Add(MainForm_MenuStrip, 0, 0);
            MainForm_TableLayout.Controls.Add(MainForm_TabControl, 0, 1);
            MainForm_TableLayout.Controls.Add(MainForm_SplitContainer_Lower, 0, 2);
            MainForm_TableLayout.Dock = DockStyle.Fill;
            MainForm_TableLayout.Location = new Point(0, 0);
            MainForm_TableLayout.Name = "MainForm_TableLayout";
            MainForm_TableLayout.RowCount = 3;
            MainForm_TableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            MainForm_TableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            MainForm_TableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
            MainForm_TableLayout.Size = new Size(935, 481);
            MainForm_TableLayout.TabIndex = 94;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(935, 481);
            Controls.Add(MainForm_TableLayout);
            DoubleBuffered = true;
            HelpButton = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            KeyPreview = true;
            MainMenuStrip = MainForm_MenuStrip;
            MaximizeBox = false;
            MaximumSize = new Size(951, 520);
            MinimumSize = new Size(951, 520);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manitowoc Tool and Manufacturing | WIP Inventory System |";
            MainForm_MenuStrip.ResumeLayout(false);
            MainForm_MenuStrip.PerformLayout();
            MainForm_Remove_GroupBox_Main.ResumeLayout(false);
            MainForm_Remove_Panel_Header.ResumeLayout(false);
            MainForm_Remove_Panel_Header.PerformLayout();
            MainForm_Remove_Panel_DataGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainForm_Remove_Image_NothingFound).EndInit();
            ((System.ComponentModel.ISupportInitialize)MainForm_Remove_DataGrid).EndInit();
            MainForm_Remove_Panel_Buttons.ResumeLayout(false);
            MainForm_TabControl.ResumeLayout(false);
            MainForm_TabControl_Inventory.ResumeLayout(false);
            MainForm_InventoryTab_SplitContainer_Middle.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainForm_InventoryTab_SplitContainer_Middle).EndInit();
            MainForm_InventoryTab_SplitContainer_Middle.ResumeLayout(false);
            MainForm_TabControl_Remove.ResumeLayout(false);
            MainForm_TabControl_Transfer.ResumeLayout(false);
            MainForm_Transfer_GroupBox_Main.ResumeLayout(false);
            MainForm_Transfer_Panel_Header.ResumeLayout(false);
            MainForm_Transfer_Panel_Header.PerformLayout();
            MainForm_Transfer_Panel_DataGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainForm_Transfer_Image_Nothing).EndInit();
            ((System.ComponentModel.ISupportInitialize)MainForm_Transfer_DataGrid).EndInit();
            MainForm_Transfer_Panel_Bottom.ResumeLayout(false);
            MainForm_Transfer_Panel_Bottom.PerformLayout();
            MainForm_StatusStrip.ResumeLayout(false);
            MainForm_StatusStrip.PerformLayout();
            MainForm_SplitContainer_Lower.Panel1.ResumeLayout(false);
            MainForm_SplitContainer_Lower.Panel1.PerformLayout();
            MainForm_SplitContainer_Lower.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainForm_SplitContainer_Lower).EndInit();
            MainForm_SplitContainer_Lower.ResumeLayout(false);
            MainForm_TableLayout.ResumeLayout(false);
            MainForm_TableLayout.PerformLayout();
            ResumeLayout(false);



        }

        #endregion

        // --- RENAMED FIELD DECLARATIONS ---

        private MenuStrip MainForm_MenuStrip;
        private ToolStripMenuItem MainForm_MenuStrip_File;
        public ToolStripMenuItem MainForm_MenuStrip_File_Save;
        private ToolStripMenuItem MainForm_MenuStrip_File_Delete;
        private ToolStripMenuItem MainForm_MenuStrip_File_Print;
        private ToolStripMenuItem MainForm_MenuStrip_File_Settings;
        private ToolStripMenuItem MainForm_MenuStrip_Exit;
        private ToolStripMenuItem MainForm_MenuStrip_Edit;
        private ToolStripMenuItem MainForm_MenuStrip_Edit_New_Object;
        private ToolStripMenuItem MainForm_MenuStrip_Edit_Remove_Object;
        private ToolStripSeparator MainForm_MenuStrip_Edit_Separator4;
        private ToolStripMenuItem MainForm_MenuStrip_Edit_ResetLast10Buttons;
        private ToolStripMenuItem MainForm_MenuStrip_View;
        private ToolStripMenuItem MainForm_MenuStrip_View_PersonalHistory;
        private ToolStripMenuItem MainForm_MenuStrip_View_Reset;
        private ToolStripSeparator MainForm_MenuStrip_View_Separator1;
        private ToolStripMenuItem MainForm_MenuStrip_View_AddToInventory;
        private ToolStripMenuItem MainForm_MenuStrip_View_RemoveFromInventory;
        private ToolStripMenuItem MainForm_MenuStrip_View_LocationToLocation;
        private ToolStripSeparator MainForm_MenuStrip_View_Separator2;
        private ToolStripMenuItem MainForm_MenuStrip_View_ViewAllWIP;
        private ToolStripMenuItem MainForm_MenuStrip_View_ViewOutsideService;
        private ToolStripSeparator MainForm_MenuStrip_View_Separator3;
        private ToolStripMenuItem MainForm_MenuStrip_View_ViewChangelog;

        private GroupBox MainForm_Remove_GroupBox_Main;
        private Panel MainForm_Remove_Panel_Header;
        private ComboBox MainForm_Remove_ComboBox_Part;
        private Label MainForm_Remove_Label_Part;
        private Label MainForm_Remove_Label_Op;
        private ComboBox MainForm_Remove_ComboBox_Op;
        private Panel MainForm_Remove_Panel_DataGrid;
        private PictureBox MainForm_Remove_Image_NothingFound;
        private DataGridView MainForm_Remove_DataGrid;
        private Panel MainForm_Remove_Panel_Buttons;
        private Button MainForm_Remove_Button_Reset;
        private Button MainForm_Remove_Button_Delete;
        private Button MainForm_Remove_Button_Search;

        public TabControl MainForm_TabControl;
        private TabPage MainForm_TabControl_Inventory;
        private TabPage MainForm_TabControl_Remove;
        private TabPage MainForm_TabControl_Transfer;

        private GroupBox MainForm_Transfer_GroupBox_Main;
        private Panel MainForm_Transfer_Panel_Header;
        private Button MainForm_Transfer_Button_Search;
        private ComboBox MainForm_Transfer_ComboBox_Part;
        private Label MainForm_Transfer_Label_Part;
        private Label MainForm_Transfer_Label_Loc;
        private ComboBox MainForm_Transfer_ComboBox_Loc;
        private Panel MainForm_Transfer_Panel_DataGrid;
        private PictureBox MainForm_Transfer_Image_Nothing;
        private DataGridView MainForm_Transfer_DataGrid;
        private Panel MainForm_Transfer_Panel_Bottom;
        private TextBox MainForm_Transfer_TextBox_Qty;
        private Button MainForm_Transfer_Button_Save;
        private Label MainForm_Transfer_Label_Qty;
        private Button MainForm_Transfer_Button_Reset;

        private StatusStrip MainForm_StatusStrip;
        public ToolStripStatusLabel MainForm_StatusStrip_SavedStatus;
        public ToolStripStatusLabel MainForm_StatusStrip_Disconnected;

        private System.Drawing.Printing.PrintDocument MainForm_Inventory_PrintDocument;
        private PrintPreviewDialog MainForm_Inventory_PrintDialog;
        private ToolTip MainForm_ToolTip;

        private System.Windows.Forms.Timer MainForm_Last10_Timer;
        private Button MainForm_RemoveTab_Button_AdvancedSearch;
        public SplitContainer MainForm_SplitContainer_Lower;
        public ConnectionStrengthControl MainForm_Control_SignalStrength;
        private ToolStripPanel BottomToolStripPanel;
        private ToolStripPanel TopToolStripPanel;
        private ToolStripPanel LeftToolStripPanel;
        private ToolStripContentPanel ContentPanel;
        private ToolStripPanel RightToolStripPanel;
        private TableLayoutPanel MainForm_TableLayout;
        private Control_InventoryTab MainForm_InventoryTab;
        public SplitContainer MainForm_InventoryTab_SplitContainer_Middle;
    }
}