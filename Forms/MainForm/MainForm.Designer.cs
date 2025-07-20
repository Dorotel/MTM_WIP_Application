using System.Drawing;
using System.Windows.Forms;
using MTM_Inventory_Application.Controls.Addons;
using MTM_Inventory_Application.Controls.MainForm;

namespace MTM_Inventory_Application.Forms.MainForm
{
    public partial class MainForm : Form
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private MenuStrip MainForm_MenuStrip;
        private ToolStripMenuItem MainForm_MenuStrip_File;
        private ToolStripMenuItem MainForm_MenuStrip_File_Settings;
        private ToolStripMenuItem MainForm_MenuStrip_Exit;
        private ToolStripMenuItem MainForm_MenuStrip_View;
        private ToolStripMenuItem MainForm_MenuStrip_View_PersonalHistory;
        private ToolStripSeparator MainForm_MenuStrip_View_Separator2;

        private StatusStrip MainForm_StatusStrip;
        public ToolStripStatusLabel MainForm_StatusStrip_SavedStatus;
        public ToolStripStatusLabel MainForm_StatusStrip_Disconnected;

        private System.Drawing.Printing.PrintDocument MainForm_Inventory_PrintDocument;
        private PrintPreviewDialog MainForm_Inventory_PrintDialog;
        private ToolTip MainForm_ToolTip;

        private System.Windows.Forms.Timer MainForm_Last10_Timer;
        public Control_ConnectionStrengthControl MainForm_Control_SignalStrength;
        private TableLayoutPanel MainForm_TableLayout;
        public SplitContainer MainForm_SplitContainer_Middle;
        public TabControl MainForm_TabControl;
        private TabPage MainForm_TabPage_Inventory;
        private TabPage MainForm_TabControl_Remove;
        private TabPage MainForm_TabControl_Transfer;
        public Control_RemoveTab MainForm_RemoveTabNormalControl;
        public Control_TransferTab MainForm_Control_TransferTab;
        public Control_InventoryTab MainForm_Control_InventoryTab;
        public Control_AdvancedInventory MainForm_AdvancedInventory;
        public Control_AdvancedRemove MainForm_Control_AdvancedRemove;
        public Control_QuickButtons control_QuickButtons1;
        private FlowLayoutPanel flowLayoutPanel1;
        

        
        #region Methods


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
        #endregion
        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            MainForm_MenuStrip = new MenuStrip();
            MainForm_MenuStrip_File = new ToolStripMenuItem();
            MainForm_MenuStrip_File_Settings = new ToolStripMenuItem();
            MainForm_MenuStrip_Exit = new ToolStripMenuItem();
            MainForm_MenuStrip_View = new ToolStripMenuItem();
            MainForm_MenuStrip_View_PersonalHistory = new ToolStripMenuItem();
            MainForm_MenuStrip_View_Separator2 = new ToolStripSeparator();
            MainForm_StatusStrip = new StatusStrip();
            MainForm_StatusStrip_SavedStatus = new ToolStripStatusLabel();
            MainForm_StatusStrip_Disconnected = new ToolStripStatusLabel();
            MainForm_Inventory_PrintDocument = new System.Drawing.Printing.PrintDocument();
            MainForm_Inventory_PrintDialog = new PrintPreviewDialog();
            MainForm_ToolTip = new ToolTip(components);
            MainForm_Last10_Timer = new System.Windows.Forms.Timer(components);
            MainForm_Control_SignalStrength = new Control_ConnectionStrengthControl();
            MainForm_TableLayout = new TableLayoutPanel();
            MainForm_SplitContainer_Middle = new SplitContainer();
            MainForm_TabControl = new TabControl();
            MainForm_TabPage_Inventory = new TabPage();
            MainForm_Control_InventoryTab = new Control_InventoryTab();
            MainForm_AdvancedInventory = new Control_AdvancedInventory();
            MainForm_TabControl_Remove = new TabPage();
            MainForm_RemoveTabNormalControl = new Control_RemoveTab();
            MainForm_Control_AdvancedRemove = new Control_AdvancedRemove();
            MainForm_TabControl_Transfer = new TabPage();
            MainForm_Control_TransferTab = new Control_TransferTab();
            control_QuickButtons1 = new Control_QuickButtons();
            flowLayoutPanel1 = new FlowLayoutPanel();
            MainForm_MenuStrip.SuspendLayout();
            MainForm_StatusStrip.SuspendLayout();
            MainForm_TableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainForm_SplitContainer_Middle).BeginInit();
            MainForm_SplitContainer_Middle.Panel1.SuspendLayout();
            MainForm_SplitContainer_Middle.Panel2.SuspendLayout();
            MainForm_SplitContainer_Middle.SuspendLayout();
            MainForm_TabControl.SuspendLayout();
            MainForm_TabPage_Inventory.SuspendLayout();
            MainForm_TabControl_Remove.SuspendLayout();
            MainForm_TabControl_Transfer.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // MainForm_MenuStrip
            // 
            MainForm_MenuStrip.Dock = DockStyle.Fill;
            MainForm_MenuStrip.ImageScalingSize = new Size(24, 24);
            MainForm_MenuStrip.Items.AddRange(new ToolStripItem[] { MainForm_MenuStrip_File, MainForm_MenuStrip_View });
            MainForm_MenuStrip.Location = new Point(0, 0);
            MainForm_MenuStrip.Name = "MainForm_MenuStrip";
            MainForm_MenuStrip.Padding = new Padding(0);
            MainForm_MenuStrip.Size = new Size(982, 24);
            MainForm_MenuStrip.TabIndex = 1;
            // 
            // MainForm_MenuStrip_File
            // 
            MainForm_MenuStrip_File.DropDownItems.AddRange(new ToolStripItem[] { MainForm_MenuStrip_File_Settings, MainForm_MenuStrip_Exit });
            MainForm_MenuStrip_File.Name = "MainForm_MenuStrip_File";
            MainForm_MenuStrip_File.Size = new Size(46, 24);
            MainForm_MenuStrip_File.Text = "File";
            // 
            // MainForm_MenuStrip_File_Settings
            // 
            MainForm_MenuStrip_File_Settings.Name = "MainForm_MenuStrip_File_Settings";
            MainForm_MenuStrip_File_Settings.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            MainForm_MenuStrip_File_Settings.Size = new Size(235, 26);
            MainForm_MenuStrip_File_Settings.Text = "Settings";
            MainForm_MenuStrip_File_Settings.Click += MainForm_MenuStrip_File_Settings_Click;
            // 
            // MainForm_MenuStrip_Exit
            // 
            MainForm_MenuStrip_Exit.Name = "MainForm_MenuStrip_Exit";
            MainForm_MenuStrip_Exit.ShortcutKeys = Keys.Alt | Keys.F4;
            MainForm_MenuStrip_Exit.Size = new Size(235, 26);
            MainForm_MenuStrip_Exit.Text = "Exit";
            MainForm_MenuStrip_Exit.Click += MainForm_MenuStrip_Exit_Click;
            // 
            // MainForm_MenuStrip_View
            // 
            MainForm_MenuStrip_View.DropDownItems.AddRange(new ToolStripItem[] { MainForm_MenuStrip_View_PersonalHistory, MainForm_MenuStrip_View_Separator2 });
            MainForm_MenuStrip_View.Name = "MainForm_MenuStrip_View";
            MainForm_MenuStrip_View.Size = new Size(55, 24);
            MainForm_MenuStrip_View.Text = "View";
            // 
            // MainForm_MenuStrip_View_PersonalHistory
            // 
            MainForm_MenuStrip_View_PersonalHistory.Name = "MainForm_MenuStrip_View_PersonalHistory";
            MainForm_MenuStrip_View_PersonalHistory.ShortcutKeys = Keys.Control | Keys.H;
            MainForm_MenuStrip_View_PersonalHistory.Size = new Size(271, 26);
            MainForm_MenuStrip_View_PersonalHistory.Text = "Transaction History";
            MainForm_MenuStrip_View_PersonalHistory.Click += MainForm_MenuStrip_View_PersonalHistory_Click;
            // 
            // MainForm_MenuStrip_View_Separator2
            // 
            MainForm_MenuStrip_View_Separator2.Name = "MainForm_MenuStrip_View_Separator2";
            MainForm_MenuStrip_View_Separator2.Size = new Size(268, 6);
            // 
            // MainForm_StatusStrip
            // 
            MainForm_StatusStrip.AutoSize = false;
            MainForm_StatusStrip.BackColor = SystemColors.Control;
            MainForm_StatusStrip.Dock = DockStyle.None;
            MainForm_StatusStrip.ImageScalingSize = new Size(24, 24);
            MainForm_StatusStrip.Items.AddRange(new ToolStripItem[] { MainForm_StatusStrip_SavedStatus, MainForm_StatusStrip_Disconnected });
            MainForm_StatusStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            MainForm_StatusStrip.Location = new Point(0, 0);
            MainForm_StatusStrip.Name = "MainForm_StatusStrip";
            MainForm_StatusStrip.Size = new Size(920, 31);
            MainForm_StatusStrip.SizingGrip = false;
            MainForm_StatusStrip.TabIndex = 18;
            // 
            // MainForm_StatusStrip_SavedStatus
            // 
            MainForm_StatusStrip_SavedStatus.Name = "MainForm_StatusStrip_SavedStatus";
            MainForm_StatusStrip_SavedStatus.Size = new Size(0, 25);
            // 
            // MainForm_StatusStrip_Disconnected
            // 
            MainForm_StatusStrip_Disconnected.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic);
            MainForm_StatusStrip_Disconnected.Name = "MainForm_StatusStrip_Disconnected";
            MainForm_StatusStrip_Disconnected.Size = new Size(307, 25);
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
            // MainForm_Control_SignalStrength
            // 
            MainForm_Control_SignalStrength.BackColor = SystemColors.Control;
            MainForm_Control_SignalStrength.BackgroundImageLayout = ImageLayout.None;
            MainForm_Control_SignalStrength.Location = new Point(0, 31);
            MainForm_Control_SignalStrength.Margin = new Padding(0);
            MainForm_Control_SignalStrength.Name = "MainForm_Control_SignalStrength";
            MainForm_Control_SignalStrength.Ping = -1;
            MainForm_Control_SignalStrength.Size = new Size(64, 31);
            MainForm_Control_SignalStrength.Strength = 0;
            MainForm_Control_SignalStrength.TabIndex = 0;
            // 
            // MainForm_TableLayout
            // 
            MainForm_TableLayout.ColumnCount = 1;
            MainForm_TableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            MainForm_TableLayout.Controls.Add(MainForm_MenuStrip, 0, 0);
            MainForm_TableLayout.Controls.Add(MainForm_SplitContainer_Middle, 0, 1);
            MainForm_TableLayout.Controls.Add(flowLayoutPanel1, 0, 2);
            MainForm_TableLayout.Dock = DockStyle.Fill;
            MainForm_TableLayout.Location = new Point(0, 0);
            MainForm_TableLayout.Margin = new Padding(0);
            MainForm_TableLayout.Name = "MainForm_TableLayout";
            MainForm_TableLayout.RowCount = 3;
            MainForm_TableLayout.RowStyles.Add(new RowStyle());
            MainForm_TableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            MainForm_TableLayout.RowStyles.Add(new RowStyle());
            MainForm_TableLayout.Size = new Size(982, 473);
            MainForm_TableLayout.TabIndex = 94;
            // 
            // MainForm_SplitContainer_Middle
            // 
            MainForm_SplitContainer_Middle.Dock = DockStyle.Fill;
            MainForm_SplitContainer_Middle.Location = new Point(0, 24);
            MainForm_SplitContainer_Middle.Margin = new Padding(0);
            MainForm_SplitContainer_Middle.Name = "MainForm_SplitContainer_Middle";
            // 
            // MainForm_SplitContainer_Middle.Panel1
            // 
            MainForm_SplitContainer_Middle.Panel1.Controls.Add(MainForm_TabControl);
            MainForm_SplitContainer_Middle.Panel1.Margin = new Padding(3);
            // 
            // MainForm_SplitContainer_Middle.Panel2
            // 
            MainForm_SplitContainer_Middle.Panel2.Controls.Add(control_QuickButtons1);
            MainForm_SplitContainer_Middle.Size = new Size(982, 418);
            MainForm_SplitContainer_Middle.SplitterDistance = 823;
            MainForm_SplitContainer_Middle.TabIndex = 93;
            // 
            // MainForm_TabControl
            // 
            MainForm_TabControl.Controls.Add(MainForm_TabPage_Inventory);
            MainForm_TabControl.Controls.Add(MainForm_TabControl_Remove);
            MainForm_TabControl.Controls.Add(MainForm_TabControl_Transfer);
            MainForm_TabControl.Dock = DockStyle.Fill;
            MainForm_TabControl.HotTrack = true;
            MainForm_TabControl.Location = new Point(0, 0);
            MainForm_TabControl.Margin = new Padding(0);
            MainForm_TabControl.Multiline = true;
            MainForm_TabControl.Name = "MainForm_TabControl";
            MainForm_TabControl.SelectedIndex = 0;
            MainForm_TabControl.ShowToolTips = true;
            MainForm_TabControl.Size = new Size(823, 418);
            MainForm_TabControl.TabIndex = 91;
            MainForm_TabControl.TabStop = false;
            // 
            // MainForm_TabPage_Inventory
            // 
            MainForm_TabPage_Inventory.Controls.Add(MainForm_Control_InventoryTab);
            MainForm_TabPage_Inventory.Controls.Add(MainForm_AdvancedInventory);
            MainForm_TabPage_Inventory.Location = new Point(4, 29);
            MainForm_TabPage_Inventory.Margin = new Padding(0);
            MainForm_TabPage_Inventory.Name = "MainForm_TabPage_Inventory";
            MainForm_TabPage_Inventory.Size = new Size(815, 385);
            MainForm_TabPage_Inventory.TabIndex = 0;
            MainForm_TabPage_Inventory.Text = "New Transaction (Ctrl+1)";
            MainForm_TabPage_Inventory.ToolTipText = "Shortcut: Ctrl+1";
            MainForm_TabPage_Inventory.UseVisualStyleBackColor = true;
            // 
            // MainForm_Control_InventoryTab
            // 
            MainForm_Control_InventoryTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MainForm_Control_InventoryTab.BackColor = SystemColors.Control;
            MainForm_Control_InventoryTab.Dock = DockStyle.Fill;
            MainForm_Control_InventoryTab.Location = new Point(0, 0);
            MainForm_Control_InventoryTab.Margin = new Padding(3, 4, 3, 4);
            MainForm_Control_InventoryTab.Name = "MainForm_Control_InventoryTab";
            MainForm_Control_InventoryTab.Size = new Size(815, 385);
            MainForm_Control_InventoryTab.TabIndex = 0;
            // 
            // MainForm_AdvancedInventory
            // 
            MainForm_AdvancedInventory.Dock = DockStyle.Fill;
            MainForm_AdvancedInventory.Location = new Point(0, 0);
            MainForm_AdvancedInventory.Margin = new Padding(3, 4, 3, 4);
            MainForm_AdvancedInventory.Name = "MainForm_AdvancedInventory";
            MainForm_AdvancedInventory.Size = new Size(815, 385);
            MainForm_AdvancedInventory.TabIndex = 1;
            MainForm_AdvancedInventory.Visible = false;
            // 
            // MainForm_TabControl_Remove
            // 
            MainForm_TabControl_Remove.Controls.Add(MainForm_RemoveTabNormalControl);
            MainForm_TabControl_Remove.Controls.Add(MainForm_Control_AdvancedRemove);
            MainForm_TabControl_Remove.Location = new Point(4, 54);
            MainForm_TabControl_Remove.Margin = new Padding(0);
            MainForm_TabControl_Remove.Name = "MainForm_TabControl_Remove";
            MainForm_TabControl_Remove.Size = new Size(192, 42);
            MainForm_TabControl_Remove.TabIndex = 1;
            MainForm_TabControl_Remove.Text = "Remove (Ctrl + 2)";
            MainForm_TabControl_Remove.ToolTipText = "Shortcut: Ctrl+2";
            MainForm_TabControl_Remove.UseVisualStyleBackColor = true;
            // 
            // MainForm_RemoveTabNormalControl
            // 
            MainForm_RemoveTabNormalControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MainForm_RemoveTabNormalControl.Dock = DockStyle.Fill;
            MainForm_RemoveTabNormalControl.Location = new Point(0, 0);
            MainForm_RemoveTabNormalControl.Margin = new Padding(3, 4, 3, 4);
            MainForm_RemoveTabNormalControl.Name = "MainForm_RemoveTabNormalControl";
            MainForm_RemoveTabNormalControl.Padding = new Padding(3);
            MainForm_RemoveTabNormalControl.Size = new Size(192, 42);
            MainForm_RemoveTabNormalControl.TabIndex = 0;
            // 
            // MainForm_Control_AdvancedRemove
            // 
            MainForm_Control_AdvancedRemove.Dock = DockStyle.Fill;
            MainForm_Control_AdvancedRemove.Location = new Point(0, 0);
            MainForm_Control_AdvancedRemove.Name = "MainForm_Control_AdvancedRemove";
            MainForm_Control_AdvancedRemove.Size = new Size(192, 42);
            MainForm_Control_AdvancedRemove.TabIndex = 1;
            MainForm_Control_AdvancedRemove.Visible = false;
            // 
            // MainForm_TabControl_Transfer
            // 
            MainForm_TabControl_Transfer.Controls.Add(MainForm_Control_TransferTab);
            MainForm_TabControl_Transfer.Location = new Point(4, 79);
            MainForm_TabControl_Transfer.Margin = new Padding(0);
            MainForm_TabControl_Transfer.Name = "MainForm_TabControl_Transfer";
            MainForm_TabControl_Transfer.Size = new Size(192, 17);
            MainForm_TabControl_Transfer.TabIndex = 2;
            MainForm_TabControl_Transfer.Text = "Transfer (Ctrl+3)";
            MainForm_TabControl_Transfer.ToolTipText = "Shortcut: Ctrl+3";
            MainForm_TabControl_Transfer.UseVisualStyleBackColor = true;
            // 
            // MainForm_Control_TransferTab
            // 
            MainForm_Control_TransferTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MainForm_Control_TransferTab.Dock = DockStyle.Fill;
            MainForm_Control_TransferTab.Location = new Point(0, 0);
            MainForm_Control_TransferTab.Margin = new Padding(0);
            MainForm_Control_TransferTab.Name = "MainForm_Control_TransferTab";
            MainForm_Control_TransferTab.Size = new Size(192, 17);
            MainForm_Control_TransferTab.TabIndex = 0;
            // 
            // control_QuickButtons1
            // 
            control_QuickButtons1.AutoSize = true;
            control_QuickButtons1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            control_QuickButtons1.Dock = DockStyle.Fill;
            control_QuickButtons1.Location = new Point(0, 0);
            control_QuickButtons1.Margin = new Padding(0);
            control_QuickButtons1.Name = "control_QuickButtons1";
            control_QuickButtons1.Size = new Size(155, 418);
            control_QuickButtons1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(MainForm_StatusStrip);
            flowLayoutPanel1.Controls.Add(MainForm_Control_SignalStrength);
            flowLayoutPanel1.Location = new Point(0, 442);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(982, 31);
            flowLayoutPanel1.TabIndex = 94;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(982, 473);
            Controls.Add(MainForm_TableLayout);
            DoubleBuffered = true;
            HelpButton = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            KeyPreview = true;
            MainMenuStrip = MainForm_MenuStrip;
            MaximizeBox = false;
            MaximumSize = new Size(1000, 520);
            MinimumSize = new Size(1000, 520);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manitowoc Tool and Manufacturing | WIP Inventory System |";
            MainForm_MenuStrip.ResumeLayout(false);
            MainForm_MenuStrip.PerformLayout();
            MainForm_StatusStrip.ResumeLayout(false);
            MainForm_StatusStrip.PerformLayout();
            MainForm_TableLayout.ResumeLayout(false);
            MainForm_TableLayout.PerformLayout();
            MainForm_SplitContainer_Middle.Panel1.ResumeLayout(false);
            MainForm_SplitContainer_Middle.Panel2.ResumeLayout(false);
            MainForm_SplitContainer_Middle.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MainForm_SplitContainer_Middle).EndInit();
            MainForm_SplitContainer_Middle.ResumeLayout(false);
            MainForm_TabControl.ResumeLayout(false);
            MainForm_TabPage_Inventory.ResumeLayout(false);
            MainForm_TabControl_Remove.ResumeLayout(false);
            MainForm_TabControl_Transfer.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);

        }
    }

        
        #endregion
    }
