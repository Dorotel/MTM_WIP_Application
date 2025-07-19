using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.MainForm
{
    partial class Control_InventoryTab
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private GroupBox Control_InventoryTab_GroupBox_Main;
        private Panel Control_InventoryTab_Panel_BottomGroup;
        private Button Control_InventoryTab_Button_Reset;
        private Button Control_InventoryTab_Button_Save;
        private Label Control_InventoryTab_Label_Version;
        private Button Control_InventoryTab_Button_AdvancedEntry;
        private Label Control_InventoryTab_Label_Part;
        private Label Control_InventoryTab_Label_Op;
        private ComboBox Control_InventoryTab_ComboBox_Operation;
        private Label Control_InventoryTab_Label_Qty;
        private TextBox Control_InventoryTab_TextBox_Quantity;
        private Label Control_InventoryTab_Label_Loc;
        private ComboBox Control_InventoryTab_ComboBox_Location;
        private Label Control_InventoryTab_Label_Notes;
        private RichTextBox Control_InventoryTab_RichTextBox_Notes;
        private TableLayoutPanel Control_InventoryTab_TableLayout_Main;
        private Panel Control_InventoryTab_Panel_Top;
        private Button Control_InventoryTab_Button_Toggle_RightPanel;
        public ComboBox Control_InventoryTab_ComboBox_Part;
        private ToolTip Control_InventoryTab_Tooltip;
        

        
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
            components = new System.ComponentModel.Container();
            Control_InventoryTab_GroupBox_Main = new GroupBox();
            Control_InventoryTab_TableLayout_Main = new TableLayoutPanel();
            Control_InventoryTab_Panel_Top = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            Control_InventoryTab_Label_Part = new Label();
            Control_InventoryTab_Label_Notes = new Label();
            Control_InventoryTab_ComboBox_Part = new ComboBox();
            Control_InventoryTab_ComboBox_Operation = new ComboBox();
            Control_InventoryTab_Label_Op = new Label();
            Control_InventoryTab_TextBox_Quantity = new TextBox();
            Control_InventoryTab_ComboBox_Location = new ComboBox();
            Control_InventoryTab_Label_Qty = new Label();
            Control_InventoryTab_Label_Loc = new Label();
            Control_InventoryTab_Panel_BottomGroup = new Panel();
            panel1 = new Panel();
            Control_InventoryTab_RichTextBox_Notes = new RichTextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            Control_InventoryTab_Button_Toggle_RightPanel = new Button();
            Control_InventoryTab_Label_Version = new Label();
            Control_InventoryTab_Button_Save = new Button();
            Control_InventoryTab_Button_Reset = new Button();
            Control_InventoryTab_Button_AdvancedEntry = new Button();
            Control_InventoryTab_Tooltip = new ToolTip(components);
            Control_InventoryTab_GroupBox_Main.SuspendLayout();
            Control_InventoryTab_TableLayout_Main.SuspendLayout();
            Control_InventoryTab_Panel_Top.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            Control_InventoryTab_Panel_BottomGroup.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // Control_InventoryTab_GroupBox_Main
            // 
            Control_InventoryTab_GroupBox_Main.Controls.Add(Control_InventoryTab_TableLayout_Main);
            Control_InventoryTab_GroupBox_Main.Dock = DockStyle.Fill;
            Control_InventoryTab_GroupBox_Main.Location = new Point(0, 0);
            Control_InventoryTab_GroupBox_Main.Name = "Control_InventoryTab_GroupBox_Main";
            Control_InventoryTab_GroupBox_Main.Size = new Size(815, 384);
            Control_InventoryTab_GroupBox_Main.TabIndex = 1;
            Control_InventoryTab_GroupBox_Main.TabStop = false;
            Control_InventoryTab_GroupBox_Main.Text = "Inventory Entry";
            // 
            // Control_InventoryTab_TableLayout_Main
            // 
            Control_InventoryTab_TableLayout_Main.AutoSize = true;
            Control_InventoryTab_TableLayout_Main.ColumnCount = 1;
            Control_InventoryTab_TableLayout_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_InventoryTab_TableLayout_Main.Controls.Add(Control_InventoryTab_Panel_Top, 0, 0);
            Control_InventoryTab_TableLayout_Main.Controls.Add(Control_InventoryTab_Panel_BottomGroup, 0, 1);
            Control_InventoryTab_TableLayout_Main.Controls.Add(tableLayoutPanel1, 0, 2);
            Control_InventoryTab_TableLayout_Main.Dock = DockStyle.Fill;
            Control_InventoryTab_TableLayout_Main.Location = new Point(3, 19);
            Control_InventoryTab_TableLayout_Main.Name = "Control_InventoryTab_TableLayout_Main";
            Control_InventoryTab_TableLayout_Main.RowCount = 3;
            Control_InventoryTab_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            Control_InventoryTab_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            Control_InventoryTab_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_InventoryTab_TableLayout_Main.Size = new Size(809, 362);
            Control_InventoryTab_TableLayout_Main.TabIndex = 0;
            // 
            // Control_InventoryTab_Panel_Top
            // 
            Control_InventoryTab_Panel_Top.Controls.Add(tableLayoutPanel2);
            Control_InventoryTab_Panel_Top.Location = new Point(3, 3);
            Control_InventoryTab_Panel_Top.Name = "Control_InventoryTab_Panel_Top";
            Control_InventoryTab_Panel_Top.Size = new Size(803, 152);
            Control_InventoryTab_Panel_Top.TabIndex = 26;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(Control_InventoryTab_Label_Part, 0, 0);
            tableLayoutPanel2.Controls.Add(Control_InventoryTab_Label_Notes, 0, 4);
            tableLayoutPanel2.Controls.Add(Control_InventoryTab_ComboBox_Part, 1, 0);
            tableLayoutPanel2.Controls.Add(Control_InventoryTab_ComboBox_Operation, 1, 1);
            tableLayoutPanel2.Controls.Add(Control_InventoryTab_Label_Op, 0, 1);
            tableLayoutPanel2.Controls.Add(Control_InventoryTab_TextBox_Quantity, 1, 2);
            tableLayoutPanel2.Controls.Add(Control_InventoryTab_ComboBox_Location, 1, 3);
            tableLayoutPanel2.Controls.Add(Control_InventoryTab_Label_Qty, 0, 2);
            tableLayoutPanel2.Controls.Add(Control_InventoryTab_Label_Loc, 0, 3);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 5;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(803, 152);
            tableLayoutPanel2.TabIndex = 7;
            // 
            // Control_InventoryTab_Label_Part
            // 
            Control_InventoryTab_Label_Part.AutoSize = true;
            Control_InventoryTab_Label_Part.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Part.Location = new Point(3, 0);
            Control_InventoryTab_Label_Part.Name = "Control_InventoryTab_Label_Part";
            Control_InventoryTab_Label_Part.Size = new Size(78, 29);
            Control_InventoryTab_Label_Part.TabIndex = 0;
            Control_InventoryTab_Label_Part.Text = "Part Number:";
            Control_InventoryTab_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_InventoryTab_Label_Notes
            // 
            tableLayoutPanel2.SetColumnSpan(Control_InventoryTab_Label_Notes, 2);
            Control_InventoryTab_Label_Notes.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Notes.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            Control_InventoryTab_Label_Notes.Location = new Point(3, 116);
            Control_InventoryTab_Label_Notes.Name = "Control_InventoryTab_Label_Notes";
            Control_InventoryTab_Label_Notes.Size = new Size(803, 37);
            Control_InventoryTab_Label_Notes.TabIndex = 9;
            Control_InventoryTab_Label_Notes.Text = "Notes";
            Control_InventoryTab_Label_Notes.TextAlign = ContentAlignment.BottomCenter;
            // 
            // Control_InventoryTab_ComboBox_Part
            // 
            Control_InventoryTab_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.Suggest;
            Control_InventoryTab_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_InventoryTab_ComboBox_Part.Dock = DockStyle.Fill;
            Control_InventoryTab_ComboBox_Part.ItemHeight = 15;
            Control_InventoryTab_ComboBox_Part.Location = new Point(87, 3);
            Control_InventoryTab_ComboBox_Part.MaxDropDownItems = 6;
            Control_InventoryTab_ComboBox_Part.Name = "Control_InventoryTab_ComboBox_Part";
            Control_InventoryTab_ComboBox_Part.Size = new Size(719, 23);
            Control_InventoryTab_ComboBox_Part.TabIndex = 1;
            // 
            // Control_InventoryTab_ComboBox_Operation
            // 
            Control_InventoryTab_ComboBox_Operation.AutoCompleteMode = AutoCompleteMode.Suggest;
            Control_InventoryTab_ComboBox_Operation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_InventoryTab_ComboBox_Operation.Dock = DockStyle.Fill;
            Control_InventoryTab_ComboBox_Operation.Location = new Point(87, 32);
            Control_InventoryTab_ComboBox_Operation.MaxDropDownItems = 6;
            Control_InventoryTab_ComboBox_Operation.Name = "Control_InventoryTab_ComboBox_Operation";
            Control_InventoryTab_ComboBox_Operation.Size = new Size(719, 23);
            Control_InventoryTab_ComboBox_Operation.TabIndex = 2;
            // 
            // Control_InventoryTab_Label_Op
            // 
            Control_InventoryTab_Label_Op.AutoSize = true;
            Control_InventoryTab_Label_Op.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Op.Location = new Point(3, 29);
            Control_InventoryTab_Label_Op.Name = "Control_InventoryTab_Label_Op";
            Control_InventoryTab_Label_Op.Size = new Size(78, 29);
            Control_InventoryTab_Label_Op.TabIndex = 2;
            Control_InventoryTab_Label_Op.Text = "Operation:";
            Control_InventoryTab_Label_Op.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_InventoryTab_TextBox_Quantity
            // 
            Control_InventoryTab_TextBox_Quantity.Dock = DockStyle.Fill;
            Control_InventoryTab_TextBox_Quantity.Location = new Point(87, 61);
            Control_InventoryTab_TextBox_Quantity.Name = "Control_InventoryTab_TextBox_Quantity";
            Control_InventoryTab_TextBox_Quantity.Size = new Size(719, 23);
            Control_InventoryTab_TextBox_Quantity.TabIndex = 3;
            // 
            // Control_InventoryTab_ComboBox_Location
            // 
            Control_InventoryTab_ComboBox_Location.AutoCompleteMode = AutoCompleteMode.Suggest;
            Control_InventoryTab_ComboBox_Location.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_InventoryTab_ComboBox_Location.Dock = DockStyle.Fill;
            Control_InventoryTab_ComboBox_Location.Location = new Point(87, 90);
            Control_InventoryTab_ComboBox_Location.MaxDropDownItems = 6;
            Control_InventoryTab_ComboBox_Location.Name = "Control_InventoryTab_ComboBox_Location";
            Control_InventoryTab_ComboBox_Location.Size = new Size(719, 23);
            Control_InventoryTab_ComboBox_Location.TabIndex = 4;
            // 
            // Control_InventoryTab_Label_Qty
            // 
            Control_InventoryTab_Label_Qty.AutoSize = true;
            Control_InventoryTab_Label_Qty.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Qty.Location = new Point(3, 58);
            Control_InventoryTab_Label_Qty.Name = "Control_InventoryTab_Label_Qty";
            Control_InventoryTab_Label_Qty.Size = new Size(78, 29);
            Control_InventoryTab_Label_Qty.TabIndex = 4;
            Control_InventoryTab_Label_Qty.Text = "Quantity:";
            Control_InventoryTab_Label_Qty.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_InventoryTab_Label_Loc
            // 
            Control_InventoryTab_Label_Loc.AutoSize = true;
            Control_InventoryTab_Label_Loc.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Loc.Location = new Point(3, 87);
            Control_InventoryTab_Label_Loc.Name = "Control_InventoryTab_Label_Loc";
            Control_InventoryTab_Label_Loc.Size = new Size(78, 29);
            Control_InventoryTab_Label_Loc.TabIndex = 6;
            Control_InventoryTab_Label_Loc.Text = "Location:";
            Control_InventoryTab_Label_Loc.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_InventoryTab_Panel_BottomGroup
            // 
            Control_InventoryTab_Panel_BottomGroup.AutoSize = true;
            Control_InventoryTab_Panel_BottomGroup.Controls.Add(panel1);
            Control_InventoryTab_Panel_BottomGroup.Dock = DockStyle.Fill;
            Control_InventoryTab_Panel_BottomGroup.Location = new Point(3, 161);
            Control_InventoryTab_Panel_BottomGroup.Name = "Control_InventoryTab_Panel_BottomGroup";
            Control_InventoryTab_Panel_BottomGroup.Size = new Size(803, 152);
            Control_InventoryTab_Panel_BottomGroup.TabIndex = 25;
            // 
            // panel1
            // 
            panel1.Controls.Add(Control_InventoryTab_RichTextBox_Notes);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(803, 152);
            panel1.TabIndex = 6;
            // 
            // Control_InventoryTab_RichTextBox_Notes
            // 
            Control_InventoryTab_RichTextBox_Notes.Dock = DockStyle.Fill;
            Control_InventoryTab_RichTextBox_Notes.Location = new Point(0, 0);
            Control_InventoryTab_RichTextBox_Notes.Name = "Control_InventoryTab_RichTextBox_Notes";
            Control_InventoryTab_RichTextBox_Notes.Size = new Size(803, 152);
            Control_InventoryTab_RichTextBox_Notes.TabIndex = 5;
            Control_InventoryTab_RichTextBox_Notes.Text = "";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(Control_InventoryTab_Button_Toggle_RightPanel, 4, 0);
            tableLayoutPanel1.Controls.Add(Control_InventoryTab_Label_Version, 2, 0);
            tableLayoutPanel1.Controls.Add(Control_InventoryTab_Button_Save, 0, 0);
            tableLayoutPanel1.Controls.Add(Control_InventoryTab_Button_Reset, 3, 0);
            tableLayoutPanel1.Controls.Add(Control_InventoryTab_Button_AdvancedEntry, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 319);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(803, 40);
            tableLayoutPanel1.TabIndex = 27;
            // 
            // Control_InventoryTab_Button_Toggle_RightPanel
            // 
            Control_InventoryTab_Button_Toggle_RightPanel.AutoSize = true;
            Control_InventoryTab_Button_Toggle_RightPanel.Dock = DockStyle.Right;
            Control_InventoryTab_Button_Toggle_RightPanel.Font = new Font("Segoe UI", 10F);
            Control_InventoryTab_Button_Toggle_RightPanel.ForeColor = Color.Green;
            Control_InventoryTab_Button_Toggle_RightPanel.Location = new Point(769, 3);
            Control_InventoryTab_Button_Toggle_RightPanel.Name = "Control_InventoryTab_Button_Toggle_RightPanel";
            Control_InventoryTab_Button_Toggle_RightPanel.Size = new Size(31, 34);
            Control_InventoryTab_Button_Toggle_RightPanel.TabIndex = 9;
            Control_InventoryTab_Button_Toggle_RightPanel.Text = "←";
            Control_InventoryTab_Button_Toggle_RightPanel.UseVisualStyleBackColor = true;
            Control_InventoryTab_Button_Toggle_RightPanel.Click += Control_InventoryTab_Button_Toggle_RightPanel_Click;
            // 
            // Control_InventoryTab_Label_Version
            // 
            Control_InventoryTab_Label_Version.AutoSize = true;
            Control_InventoryTab_Label_Version.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Version.Font = new Font("Segoe UI", 7F);
            Control_InventoryTab_Label_Version.Location = new Point(155, 0);
            Control_InventoryTab_Label_Version.Name = "Control_InventoryTab_Label_Version";
            Control_InventoryTab_Label_Version.Size = new Size(521, 40);
            Control_InventoryTab_Label_Version.TabIndex = 8;
            Control_InventoryTab_Label_Version.Text = "Version: ";
            Control_InventoryTab_Label_Version.TextAlign = ContentAlignment.BottomCenter;
            // 
            // Control_InventoryTab_Button_Save
            // 
            Control_InventoryTab_Button_Save.AutoSize = true;
            Control_InventoryTab_Button_Save.Dock = DockStyle.Fill;
            Control_InventoryTab_Button_Save.Font = new Font("Segoe UI", 8F);
            Control_InventoryTab_Button_Save.Location = new Point(3, 3);
            Control_InventoryTab_Button_Save.Name = "Control_InventoryTab_Button_Save";
            Control_InventoryTab_Button_Save.Size = new Size(60, 34);
            Control_InventoryTab_Button_Save.TabIndex = 6;
            Control_InventoryTab_Button_Save.Text = "Save";
            Control_InventoryTab_Button_Save.UseVisualStyleBackColor = true;
            // 
            // Control_InventoryTab_Button_Reset
            // 
            Control_InventoryTab_Button_Reset.AutoSize = true;
            Control_InventoryTab_Button_Reset.Dock = DockStyle.Fill;
            Control_InventoryTab_Button_Reset.Font = new Font("Segoe UI", 9F);
            Control_InventoryTab_Button_Reset.Location = new Point(682, 3);
            Control_InventoryTab_Button_Reset.Name = "Control_InventoryTab_Button_Reset";
            Control_InventoryTab_Button_Reset.Size = new Size(81, 34);
            Control_InventoryTab_Button_Reset.TabIndex = 7;
            Control_InventoryTab_Button_Reset.TabStop = false;
            Control_InventoryTab_Button_Reset.Text = "Reset";
            Control_InventoryTab_Button_Reset.UseVisualStyleBackColor = true;
            // 
            // Control_InventoryTab_Button_AdvancedEntry
            // 
            Control_InventoryTab_Button_AdvancedEntry.AutoSize = true;
            Control_InventoryTab_Button_AdvancedEntry.Dock = DockStyle.Fill;
            Control_InventoryTab_Button_AdvancedEntry.ForeColor = Color.DarkRed;
            Control_InventoryTab_Button_AdvancedEntry.Location = new Point(69, 3);
            Control_InventoryTab_Button_AdvancedEntry.Name = "Control_InventoryTab_Button_AdvancedEntry";
            Control_InventoryTab_Button_AdvancedEntry.Size = new Size(80, 34);
            Control_InventoryTab_Button_AdvancedEntry.TabIndex = 8;
            Control_InventoryTab_Button_AdvancedEntry.Text = "Advanced";
            Control_InventoryTab_Button_AdvancedEntry.UseVisualStyleBackColor = true;
            // 
            // Control_InventoryTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(Control_InventoryTab_GroupBox_Main);
            Name = "Control_InventoryTab";
            Size = new Size(815, 384);
            Control_InventoryTab_GroupBox_Main.ResumeLayout(false);
            Control_InventoryTab_GroupBox_Main.PerformLayout();
            Control_InventoryTab_TableLayout_Main.ResumeLayout(false);
            Control_InventoryTab_TableLayout_Main.PerformLayout();
            Control_InventoryTab_Panel_Top.ResumeLayout(false);
            Control_InventoryTab_Panel_Top.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            Control_InventoryTab_Panel_BottomGroup.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel1;
    }

        
        #endregion
    }
