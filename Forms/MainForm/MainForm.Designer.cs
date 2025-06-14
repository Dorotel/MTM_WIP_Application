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
            MainForm_SplitContainer_Middle = new SplitContainer();
            MainForm_TabControl = new TabControl();
            MainForm_TabControl_Inventory = new TabPage();
            MainForm_InventoryTab = new ControlInventoryTab();
            MainForm_TabControl_Remove = new TabPage();
            MainForm_RemoveTab = new ControlRemoveTab();
            MainForm_TabControl_Transfer = new TabPage();
            controlTransferTab1 = new ControlTransferTab();
            MainForm_MenuStrip.SuspendLayout();
            MainForm_StatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainForm_SplitContainer_Lower).BeginInit();
            MainForm_SplitContainer_Lower.Panel1.SuspendLayout();
            MainForm_SplitContainer_Lower.Panel2.SuspendLayout();
            MainForm_SplitContainer_Lower.SuspendLayout();
            MainForm_TableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainForm_SplitContainer_Middle).BeginInit();
            MainForm_SplitContainer_Middle.Panel1.SuspendLayout();
            MainForm_SplitContainer_Middle.SuspendLayout();
            MainForm_TabControl.SuspendLayout();
            MainForm_TabControl_Inventory.SuspendLayout();
            MainForm_TabControl_Remove.SuspendLayout();
            MainForm_TabControl_Transfer.SuspendLayout();
            SuspendLayout();
            // 
            // MainForm_MenuStrip
            // 
            MainForm_MenuStrip.Dock = DockStyle.Fill;
            MainForm_MenuStrip.ImageScalingSize = new Size(24, 24);
            MainForm_MenuStrip.Items.AddRange(new ToolStripItem[] { MainForm_MenuStrip_File, MainForm_MenuStrip_Edit, MainForm_MenuStrip_View });
            MainForm_MenuStrip.Location = new Point(0, 0);
            MainForm_MenuStrip.Name = "MainForm_MenuStrip";
            MainForm_MenuStrip.Size = new Size(959, 26);
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
            // MainForm_StatusStrip
            // 
            MainForm_StatusStrip.Dock = DockStyle.Fill;
            MainForm_StatusStrip.ImageScalingSize = new Size(24, 24);
            MainForm_StatusStrip.Items.AddRange(new ToolStripItem[] { MainForm_StatusStrip_SavedStatus, MainForm_StatusStrip_Disconnected });
            MainForm_StatusStrip.Location = new Point(0, 0);
            MainForm_StatusStrip.Name = "MainForm_StatusStrip";
            MainForm_StatusStrip.RenderMode = ToolStripRenderMode.Professional;
            MainForm_StatusStrip.Size = new Size(884, 25);
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
            MainForm_SplitContainer_Lower.Size = new Size(953, 25);
            MainForm_SplitContainer_Lower.SplitterDistance = 884;
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
            MainForm_Control_SignalStrength.Size = new Size(60, 15);
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
            MainForm_TableLayout.Controls.Add(MainForm_SplitContainer_Lower, 0, 2);
            MainForm_TableLayout.Controls.Add(MainForm_SplitContainer_Middle, 0, 1);
            MainForm_TableLayout.Dock = DockStyle.Fill;
            MainForm_TableLayout.Location = new Point(0, 0);
            MainForm_TableLayout.Name = "MainForm_TableLayout";
            MainForm_TableLayout.RowCount = 3;
            MainForm_TableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            MainForm_TableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            MainForm_TableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
            MainForm_TableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            MainForm_TableLayout.Size = new Size(959, 481);
            MainForm_TableLayout.TabIndex = 94;
            // 
            // MainForm_SplitContainer_Middle
            // 
            MainForm_SplitContainer_Middle.Dock = DockStyle.Fill;
            MainForm_SplitContainer_Middle.Location = new Point(3, 29);
            MainForm_SplitContainer_Middle.Name = "MainForm_SplitContainer_Middle";
            // 
            // MainForm_SplitContainer_Middle.Panel1
            // 
            MainForm_SplitContainer_Middle.Panel1.Controls.Add(MainForm_TabControl);
            MainForm_SplitContainer_Middle.Size = new Size(953, 418);
            MainForm_SplitContainer_Middle.SplitterDistance = 823;
            MainForm_SplitContainer_Middle.TabIndex = 93;
            // 
            // MainForm_TabControl
            // 
            MainForm_TabControl.Controls.Add(MainForm_TabControl_Inventory);
            MainForm_TabControl.Controls.Add(MainForm_TabControl_Remove);
            MainForm_TabControl.Controls.Add(MainForm_TabControl_Transfer);
            MainForm_TabControl.Dock = DockStyle.Fill;
            MainForm_TabControl.Location = new Point(0, 0);
            MainForm_TabControl.Multiline = true;
            MainForm_TabControl.Name = "MainForm_TabControl";
            MainForm_TabControl.SelectedIndex = 0;
            MainForm_TabControl.ShowToolTips = true;
            MainForm_TabControl.Size = new Size(823, 418);
            MainForm_TabControl.SizeMode = TabSizeMode.FillToRight;
            MainForm_TabControl.TabIndex = 91;
            MainForm_TabControl.TabStop = false;
            // 
            // MainForm_TabControl_Inventory
            // 
            MainForm_TabControl_Inventory.Controls.Add(MainForm_InventoryTab);
            MainForm_TabControl_Inventory.Location = new Point(4, 24);
            MainForm_TabControl_Inventory.Name = "MainForm_TabControl_Inventory";
            MainForm_TabControl_Inventory.Padding = new Padding(3);
            MainForm_TabControl_Inventory.Size = new Size(815, 390);
            MainForm_TabControl_Inventory.TabIndex = 0;
            MainForm_TabControl_Inventory.Text = "New Transaction (Ctrl+1)";
            MainForm_TabControl_Inventory.ToolTipText = "Shortcut: Ctrl+1";
            MainForm_TabControl_Inventory.UseVisualStyleBackColor = true;
            // 
            // MainForm_InventoryTab
            // 
            MainForm_InventoryTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MainForm_InventoryTab.Dock = DockStyle.Fill;
            MainForm_InventoryTab.Location = new Point(3, 3);
            MainForm_InventoryTab.Name = "MainForm_InventoryTab";
            MainForm_InventoryTab.Size = new Size(809, 384);
            MainForm_InventoryTab.TabIndex = 0;
            // 
            // MainForm_TabControl_Remove
            // 
            MainForm_TabControl_Remove.Controls.Add(MainForm_RemoveTab);
            MainForm_TabControl_Remove.Location = new Point(4, 24);
            MainForm_TabControl_Remove.Name = "MainForm_TabControl_Remove";
            MainForm_TabControl_Remove.Padding = new Padding(3);
            MainForm_TabControl_Remove.Size = new Size(815, 390);
            MainForm_TabControl_Remove.TabIndex = 1;
            MainForm_TabControl_Remove.Text = "Remove (Ctrl + 2)";
            MainForm_TabControl_Remove.ToolTipText = "Shortcut: Ctrl+2";
            MainForm_TabControl_Remove.UseVisualStyleBackColor = true;
            // 
            // MainForm_RemoveTab
            // 
            MainForm_RemoveTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MainForm_RemoveTab.Dock = DockStyle.Fill;
            MainForm_RemoveTab.Location = new Point(3, 3);
            MainForm_RemoveTab.Name = "MainForm_RemoveTab";
            MainForm_RemoveTab.Size = new Size(809, 384);
            MainForm_RemoveTab.TabIndex = 0;
            // 
            // MainForm_TabControl_Transfer
            // 
            MainForm_TabControl_Transfer.Controls.Add(controlTransferTab1);
            MainForm_TabControl_Transfer.Location = new Point(4, 24);
            MainForm_TabControl_Transfer.Name = "MainForm_TabControl_Transfer";
            MainForm_TabControl_Transfer.Padding = new Padding(3);
            MainForm_TabControl_Transfer.Size = new Size(815, 390);
            MainForm_TabControl_Transfer.TabIndex = 2;
            MainForm_TabControl_Transfer.Text = "Transfer (Ctrl+3)";
            MainForm_TabControl_Transfer.ToolTipText = "Shortcut: Ctrl+3";
            MainForm_TabControl_Transfer.UseVisualStyleBackColor = true;
            // 
            // controlTransferTab1
            // 
            controlTransferTab1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            controlTransferTab1.Dock = DockStyle.Fill;
            controlTransferTab1.Location = new Point(3, 3);
            controlTransferTab1.Name = "controlTransferTab1";
            controlTransferTab1.Size = new Size(809, 384);
            controlTransferTab1.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(959, 481);
            Controls.Add(MainForm_TableLayout);
            DoubleBuffered = true;
            HelpButton = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            KeyPreview = true;
            MainMenuStrip = MainForm_MenuStrip;
            MaximizeBox = false;
            MaximumSize = new Size(975, 520);
            MinimumSize = new Size(975, 520);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manitowoc Tool and Manufacturing | WIP Inventory System |";
            MainForm_MenuStrip.ResumeLayout(false);
            MainForm_MenuStrip.PerformLayout();
            MainForm_StatusStrip.ResumeLayout(false);
            MainForm_StatusStrip.PerformLayout();
            MainForm_SplitContainer_Lower.Panel1.ResumeLayout(false);
            MainForm_SplitContainer_Lower.Panel1.PerformLayout();
            MainForm_SplitContainer_Lower.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainForm_SplitContainer_Lower).EndInit();
            MainForm_SplitContainer_Lower.ResumeLayout(false);
            MainForm_TableLayout.ResumeLayout(false);
            MainForm_TableLayout.PerformLayout();
            MainForm_SplitContainer_Middle.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainForm_SplitContainer_Middle).EndInit();
            MainForm_SplitContainer_Middle.ResumeLayout(false);
            MainForm_TabControl.ResumeLayout(false);
            MainForm_TabControl_Inventory.ResumeLayout(false);
            MainForm_TabControl_Remove.ResumeLayout(false);
            MainForm_TabControl_Transfer.ResumeLayout(false);
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

        private StatusStrip MainForm_StatusStrip;
        public ToolStripStatusLabel MainForm_StatusStrip_SavedStatus;
        public ToolStripStatusLabel MainForm_StatusStrip_Disconnected;

        private System.Drawing.Printing.PrintDocument MainForm_Inventory_PrintDocument;
        private PrintPreviewDialog MainForm_Inventory_PrintDialog;
        private ToolTip MainForm_ToolTip;

        private System.Windows.Forms.Timer MainForm_Last10_Timer;
        public SplitContainer MainForm_SplitContainer_Lower;
        public ConnectionStrengthControl MainForm_Control_SignalStrength;
        private ToolStripPanel BottomToolStripPanel;
        private ToolStripPanel TopToolStripPanel;
        private ToolStripPanel LeftToolStripPanel;
        private ToolStripContentPanel ContentPanel;
        private ToolStripPanel RightToolStripPanel;
        private TableLayoutPanel MainForm_TableLayout;
        public SplitContainer MainForm_SplitContainer_Middle;
        public TabControl MainForm_TabControl;
        private TabPage MainForm_TabControl_Inventory;
        private TabPage MainForm_TabControl_Remove;
        private TabPage MainForm_TabControl_Transfer;
        private ControlInventoryTab MainForm_InventoryTab;
        private ControlRemoveTab MainForm_RemoveTab;
        private ControlTransferTab controlTransferTab1;
    }
}