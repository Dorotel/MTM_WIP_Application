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
        private Button Control_InventoryTab_Button_Reset;
        private Button Control_InventoryTab_Button_Save;
        private Label Control_InventoryTab_Label_Version;
        private Button Control_InventoryTab_Button_AdvancedEntry;
        private ComboBox Control_InventoryTab_ComboBox_Operation;
        private TextBox Control_InventoryTab_TextBox_Quantity;
        private TableLayoutPanel Control_InventoryTab_TableLayout_Main;
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
            Control_InventoryTab_TableLayout_MiddleGroup = new TableLayoutPanel();
            Control_InventoryTab_Label_Notes = new Label();
            Control_InventoryTab_RichTextBox_Notes = new RichTextBox();
            Control_InventoryTab_TableLayout_TopGroup = new TableLayoutPanel();
            Control_InventoryTab_ComboBox_Location = new ComboBox();
            Control_InventoryTab_TextBox_Quantity = new TextBox();
            Control_InventoryTab_ComboBox_Operation = new ComboBox();
            Control_InventoryTab_ComboBox_Part = new ComboBox();
            Control_InventoryTab_Label_Loc = new Label();
            Control_InventoryTab_Label_Qty = new Label();
            Control_InventoryTab_Label_Op = new Label();
            Control_InventoryTab_Label_Part = new Label();
            Control_InventoryTab_TableLayout_BottomGroup = new TableLayoutPanel();
            Control_InventoryTab_Button_Toggle_RightPanel = new Button();
            Control_InventoryTab_Label_Version = new Label();
            Control_InventoryTab_Button_Save = new Button();
            Control_InventoryTab_Button_AdvancedEntry = new Button();
            Control_InventoryTab_Button_Reset = new Button();
            Control_InventoryTab_Tooltip = new ToolTip(components);
            Control_InventoryTab_GroupBox_Main.SuspendLayout();
            Control_InventoryTab_TableLayout_Main.SuspendLayout();
            Control_InventoryTab_TableLayout_MiddleGroup.SuspendLayout();
            Control_InventoryTab_TableLayout_TopGroup.SuspendLayout();
            Control_InventoryTab_TableLayout_BottomGroup.SuspendLayout();
            SuspendLayout();
            // 
            // Control_InventoryTab_GroupBox_Main
            // 
            Control_InventoryTab_GroupBox_Main.AutoSize = true;
            Control_InventoryTab_GroupBox_Main.Controls.Add(Control_InventoryTab_TableLayout_Main);
            Control_InventoryTab_GroupBox_Main.Dock = DockStyle.Fill;
            Control_InventoryTab_GroupBox_Main.Location = new Point(0, 0);
            Control_InventoryTab_GroupBox_Main.Name = "Control_InventoryTab_GroupBox_Main";
            Control_InventoryTab_GroupBox_Main.Size = new Size(1175, 622);
            Control_InventoryTab_GroupBox_Main.TabIndex = 1;
            Control_InventoryTab_GroupBox_Main.TabStop = false;
            Control_InventoryTab_GroupBox_Main.Text = "Inventory Entry";
            // 
            // Control_InventoryTab_TableLayout_Main
            // 
            Control_InventoryTab_TableLayout_Main.AutoSize = true;
            Control_InventoryTab_TableLayout_Main.ColumnCount = 1;
            Control_InventoryTab_TableLayout_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_InventoryTab_TableLayout_Main.Controls.Add(Control_InventoryTab_TableLayout_MiddleGroup, 0, 1);
            Control_InventoryTab_TableLayout_Main.Controls.Add(Control_InventoryTab_TableLayout_TopGroup, 0, 0);
            Control_InventoryTab_TableLayout_Main.Controls.Add(Control_InventoryTab_TableLayout_BottomGroup, 0, 2);
            Control_InventoryTab_TableLayout_Main.Dock = DockStyle.Fill;
            Control_InventoryTab_TableLayout_Main.Location = new Point(3, 19);
            Control_InventoryTab_TableLayout_Main.Name = "Control_InventoryTab_TableLayout_Main";
            Control_InventoryTab_TableLayout_Main.RowCount = 3;
            Control_InventoryTab_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_InventoryTab_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_InventoryTab_TableLayout_Main.RowStyles.Add(new RowStyle());
            Control_InventoryTab_TableLayout_Main.Size = new Size(1169, 600);
            Control_InventoryTab_TableLayout_Main.TabIndex = 0;
            // 
            // Control_InventoryTab_TableLayout_MiddleGroup
            // 
            Control_InventoryTab_TableLayout_MiddleGroup.AutoSize = true;
            Control_InventoryTab_TableLayout_MiddleGroup.ColumnCount = 1;
            Control_InventoryTab_TableLayout_MiddleGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_InventoryTab_TableLayout_MiddleGroup.Controls.Add(Control_InventoryTab_Label_Notes, 0, 0);
            Control_InventoryTab_TableLayout_MiddleGroup.Controls.Add(Control_InventoryTab_RichTextBox_Notes, 0, 1);
            Control_InventoryTab_TableLayout_MiddleGroup.Dock = DockStyle.Fill;
            Control_InventoryTab_TableLayout_MiddleGroup.Location = new Point(3, 124);
            Control_InventoryTab_TableLayout_MiddleGroup.Margin = new Padding(3, 2, 3, 2);
            Control_InventoryTab_TableLayout_MiddleGroup.Name = "Control_InventoryTab_TableLayout_MiddleGroup";
            Control_InventoryTab_TableLayout_MiddleGroup.RowCount = 2;
            Control_InventoryTab_TableLayout_MiddleGroup.RowStyles.Add(new RowStyle());
            Control_InventoryTab_TableLayout_MiddleGroup.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_InventoryTab_TableLayout_MiddleGroup.Size = new Size(1163, 433);
            Control_InventoryTab_TableLayout_MiddleGroup.TabIndex = 28;
            // 
            // Control_InventoryTab_Label_Notes
            // 
            Control_InventoryTab_Label_Notes.AutoSize = true;
            Control_InventoryTab_Label_Notes.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Notes.Font = new Font("Segoe UI Emoji", 9F, FontStyle.Underline);
            Control_InventoryTab_Label_Notes.Location = new Point(3, 0);
            Control_InventoryTab_Label_Notes.Name = "Control_InventoryTab_Label_Notes";
            Control_InventoryTab_Label_Notes.Size = new Size(1157, 15);
            Control_InventoryTab_Label_Notes.TabIndex = 9;
            Control_InventoryTab_Label_Notes.Text = "Notes";
            Control_InventoryTab_Label_Notes.TextAlign = ContentAlignment.BottomCenter;
            // 
            // Control_InventoryTab_RichTextBox_Notes
            // 
            Control_InventoryTab_RichTextBox_Notes.Dock = DockStyle.Fill;
            Control_InventoryTab_RichTextBox_Notes.Location = new Point(3, 18);
            Control_InventoryTab_RichTextBox_Notes.Name = "Control_InventoryTab_RichTextBox_Notes";
            Control_InventoryTab_RichTextBox_Notes.Size = new Size(1157, 412);
            Control_InventoryTab_RichTextBox_Notes.TabIndex = 5;
            Control_InventoryTab_RichTextBox_Notes.Text = "";
            // 
            // Control_InventoryTab_TableLayout_TopGroup
            // 
            Control_InventoryTab_TableLayout_TopGroup.AutoSize = true;
            Control_InventoryTab_TableLayout_TopGroup.ColumnCount = 2;
            Control_InventoryTab_TableLayout_TopGroup.ColumnStyles.Add(new ColumnStyle());
            Control_InventoryTab_TableLayout_TopGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_InventoryTab_TableLayout_TopGroup.Controls.Add(Control_InventoryTab_ComboBox_Location, 1, 3);
            Control_InventoryTab_TableLayout_TopGroup.Controls.Add(Control_InventoryTab_TextBox_Quantity, 1, 2);
            Control_InventoryTab_TableLayout_TopGroup.Controls.Add(Control_InventoryTab_ComboBox_Operation, 1, 1);
            Control_InventoryTab_TableLayout_TopGroup.Controls.Add(Control_InventoryTab_ComboBox_Part, 1, 0);
            Control_InventoryTab_TableLayout_TopGroup.Controls.Add(Control_InventoryTab_Label_Loc, 0, 3);
            Control_InventoryTab_TableLayout_TopGroup.Controls.Add(Control_InventoryTab_Label_Qty, 0, 2);
            Control_InventoryTab_TableLayout_TopGroup.Controls.Add(Control_InventoryTab_Label_Op, 0, 1);
            Control_InventoryTab_TableLayout_TopGroup.Controls.Add(Control_InventoryTab_Label_Part, 0, 0);
            Control_InventoryTab_TableLayout_TopGroup.Dock = DockStyle.Fill;
            Control_InventoryTab_TableLayout_TopGroup.Location = new Point(3, 3);
            Control_InventoryTab_TableLayout_TopGroup.Name = "Control_InventoryTab_TableLayout_TopGroup";
            Control_InventoryTab_TableLayout_TopGroup.RowCount = 4;
            Control_InventoryTab_TableLayout_TopGroup.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            Control_InventoryTab_TableLayout_TopGroup.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            Control_InventoryTab_TableLayout_TopGroup.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            Control_InventoryTab_TableLayout_TopGroup.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            Control_InventoryTab_TableLayout_TopGroup.Size = new Size(1163, 116);
            Control_InventoryTab_TableLayout_TopGroup.TabIndex = 7;
            // 
            // Control_InventoryTab_ComboBox_Location
            // 
            Control_InventoryTab_ComboBox_Location.AutoCompleteMode = AutoCompleteMode.Suggest;
            Control_InventoryTab_ComboBox_Location.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_InventoryTab_ComboBox_Location.Dock = DockStyle.Fill;
            Control_InventoryTab_ComboBox_Location.Location = new Point(87, 90);
            Control_InventoryTab_ComboBox_Location.MaxDropDownItems = 6;
            Control_InventoryTab_ComboBox_Location.Name = "Control_InventoryTab_ComboBox_Location";
            Control_InventoryTab_ComboBox_Location.Size = new Size(1073, 23);
            Control_InventoryTab_ComboBox_Location.TabIndex = 12;
            // 
            // Control_InventoryTab_TextBox_Quantity
            // 
            Control_InventoryTab_TextBox_Quantity.Dock = DockStyle.Fill;
            Control_InventoryTab_TextBox_Quantity.Location = new Point(87, 61);
            Control_InventoryTab_TextBox_Quantity.Name = "Control_InventoryTab_TextBox_Quantity";
            Control_InventoryTab_TextBox_Quantity.Size = new Size(1073, 23);
            Control_InventoryTab_TextBox_Quantity.TabIndex = 3;
            // 
            // Control_InventoryTab_ComboBox_Operation
            // 
            Control_InventoryTab_ComboBox_Operation.AutoCompleteMode = AutoCompleteMode.Suggest;
            Control_InventoryTab_ComboBox_Operation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_InventoryTab_ComboBox_Operation.Dock = DockStyle.Fill;
            Control_InventoryTab_ComboBox_Operation.Location = new Point(87, 32);
            Control_InventoryTab_ComboBox_Operation.MaxDropDownItems = 6;
            Control_InventoryTab_ComboBox_Operation.Name = "Control_InventoryTab_ComboBox_Operation";
            Control_InventoryTab_ComboBox_Operation.Size = new Size(1073, 23);
            Control_InventoryTab_ComboBox_Operation.TabIndex = 2;
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
            Control_InventoryTab_ComboBox_Part.Size = new Size(1073, 23);
            Control_InventoryTab_ComboBox_Part.TabIndex = 1;
            // 
            // Control_InventoryTab_Label_Loc
            // 
            Control_InventoryTab_Label_Loc.AutoSize = true;
            Control_InventoryTab_Label_Loc.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Loc.Location = new Point(3, 87);
            Control_InventoryTab_Label_Loc.Name = "Control_InventoryTab_Label_Loc";
            Control_InventoryTab_Label_Loc.Size = new Size(78, 29);
            Control_InventoryTab_Label_Loc.TabIndex = 11;
            Control_InventoryTab_Label_Loc.Text = "Location:";
            Control_InventoryTab_Label_Loc.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_InventoryTab_Label_Qty
            // 
            Control_InventoryTab_Label_Qty.AutoSize = true;
            Control_InventoryTab_Label_Qty.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Qty.Location = new Point(3, 58);
            Control_InventoryTab_Label_Qty.Name = "Control_InventoryTab_Label_Qty";
            Control_InventoryTab_Label_Qty.Size = new Size(78, 29);
            Control_InventoryTab_Label_Qty.TabIndex = 10;
            Control_InventoryTab_Label_Qty.Text = "Quantity:";
            Control_InventoryTab_Label_Qty.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_InventoryTab_Label_Op
            // 
            Control_InventoryTab_Label_Op.AutoSize = true;
            Control_InventoryTab_Label_Op.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Op.Location = new Point(3, 29);
            Control_InventoryTab_Label_Op.Name = "Control_InventoryTab_Label_Op";
            Control_InventoryTab_Label_Op.Size = new Size(78, 29);
            Control_InventoryTab_Label_Op.TabIndex = 9;
            Control_InventoryTab_Label_Op.Text = "Operation:";
            Control_InventoryTab_Label_Op.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_InventoryTab_Label_Part
            // 
            Control_InventoryTab_Label_Part.AutoSize = true;
            Control_InventoryTab_Label_Part.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Part.Location = new Point(3, 0);
            Control_InventoryTab_Label_Part.Name = "Control_InventoryTab_Label_Part";
            Control_InventoryTab_Label_Part.Size = new Size(78, 29);
            Control_InventoryTab_Label_Part.TabIndex = 8;
            Control_InventoryTab_Label_Part.Text = "Part Number:";
            Control_InventoryTab_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_InventoryTab_TableLayout_BottomGroup
            // 
            Control_InventoryTab_TableLayout_BottomGroup.AutoSize = true;
            Control_InventoryTab_TableLayout_BottomGroup.ColumnCount = 5;
            Control_InventoryTab_TableLayout_BottomGroup.ColumnStyles.Add(new ColumnStyle());
            Control_InventoryTab_TableLayout_BottomGroup.ColumnStyles.Add(new ColumnStyle());
            Control_InventoryTab_TableLayout_BottomGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_InventoryTab_TableLayout_BottomGroup.ColumnStyles.Add(new ColumnStyle());
            Control_InventoryTab_TableLayout_BottomGroup.ColumnStyles.Add(new ColumnStyle());
            Control_InventoryTab_TableLayout_BottomGroup.Controls.Add(Control_InventoryTab_Button_Toggle_RightPanel, 4, 0);
            Control_InventoryTab_TableLayout_BottomGroup.Controls.Add(Control_InventoryTab_Label_Version, 2, 0);
            Control_InventoryTab_TableLayout_BottomGroup.Controls.Add(Control_InventoryTab_Button_Save, 0, 0);
            Control_InventoryTab_TableLayout_BottomGroup.Controls.Add(Control_InventoryTab_Button_AdvancedEntry, 1, 0);
            Control_InventoryTab_TableLayout_BottomGroup.Controls.Add(Control_InventoryTab_Button_Reset, 3, 0);
            Control_InventoryTab_TableLayout_BottomGroup.Dock = DockStyle.Fill;
            Control_InventoryTab_TableLayout_BottomGroup.Location = new Point(3, 562);
            Control_InventoryTab_TableLayout_BottomGroup.Name = "Control_InventoryTab_TableLayout_BottomGroup";
            Control_InventoryTab_TableLayout_BottomGroup.RowCount = 1;
            Control_InventoryTab_TableLayout_BottomGroup.RowStyles.Add(new RowStyle());
            Control_InventoryTab_TableLayout_BottomGroup.Size = new Size(1163, 35);
            Control_InventoryTab_TableLayout_BottomGroup.TabIndex = 27;
            // 
            // Control_InventoryTab_Button_Toggle_RightPanel
            // 
            Control_InventoryTab_Button_Toggle_RightPanel.AutoSize = true;
            Control_InventoryTab_Button_Toggle_RightPanel.Dock = DockStyle.Fill;
            Control_InventoryTab_Button_Toggle_RightPanel.Font = new Font("Segoe UI Emoji", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Control_InventoryTab_Button_Toggle_RightPanel.Location = new Point(1122, 3);
            Control_InventoryTab_Button_Toggle_RightPanel.Name = "Control_InventoryTab_Button_Toggle_RightPanel";
            Control_InventoryTab_Button_Toggle_RightPanel.Size = new Size(38, 29);
            Control_InventoryTab_Button_Toggle_RightPanel.TabIndex = 9;
            Control_InventoryTab_Button_Toggle_RightPanel.Text = "➡";
            Control_InventoryTab_Button_Toggle_RightPanel.UseVisualStyleBackColor = true;
            Control_InventoryTab_Button_Toggle_RightPanel.Click += Control_InventoryTab_Button_Toggle_RightPanel_Click;
            // 
            // Control_InventoryTab_Label_Version
            // 
            Control_InventoryTab_Label_Version.AutoSize = true;
            Control_InventoryTab_Label_Version.Dock = DockStyle.Fill;
            Control_InventoryTab_Label_Version.Font = new Font("Segoe UI Emoji", 7F);
            Control_InventoryTab_Label_Version.Location = new Point(160, 0);
            Control_InventoryTab_Label_Version.Name = "Control_InventoryTab_Label_Version";
            Control_InventoryTab_Label_Version.Size = new Size(869, 35);
            Control_InventoryTab_Label_Version.TabIndex = 8;
            Control_InventoryTab_Label_Version.Text = "Version: ";
            Control_InventoryTab_Label_Version.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Control_InventoryTab_Button_Save
            // 
            Control_InventoryTab_Button_Save.AutoSize = true;
            Control_InventoryTab_Button_Save.Dock = DockStyle.Fill;
            Control_InventoryTab_Button_Save.Font = new Font("Segoe UI Emoji", 8F);
            Control_InventoryTab_Button_Save.Location = new Point(3, 3);
            Control_InventoryTab_Button_Save.Name = "Control_InventoryTab_Button_Save";
            Control_InventoryTab_Button_Save.Size = new Size(60, 29);
            Control_InventoryTab_Button_Save.TabIndex = 6;
            Control_InventoryTab_Button_Save.Text = "Save";
            Control_InventoryTab_Button_Save.UseVisualStyleBackColor = true;
            // 
            // Control_InventoryTab_Button_AdvancedEntry
            // 
            Control_InventoryTab_Button_AdvancedEntry.AutoSize = true;
            Control_InventoryTab_Button_AdvancedEntry.Dock = DockStyle.Fill;
            Control_InventoryTab_Button_AdvancedEntry.ForeColor = Color.DarkRed;
            Control_InventoryTab_Button_AdvancedEntry.Location = new Point(69, 3);
            Control_InventoryTab_Button_AdvancedEntry.Name = "Control_InventoryTab_Button_AdvancedEntry";
            Control_InventoryTab_Button_AdvancedEntry.Size = new Size(85, 29);
            Control_InventoryTab_Button_AdvancedEntry.TabIndex = 8;
            Control_InventoryTab_Button_AdvancedEntry.Text = "Advanced";
            Control_InventoryTab_Button_AdvancedEntry.UseVisualStyleBackColor = true;
            // 
            // Control_InventoryTab_Button_Reset
            // 
            Control_InventoryTab_Button_Reset.AutoSize = true;
            Control_InventoryTab_Button_Reset.Dock = DockStyle.Fill;
            Control_InventoryTab_Button_Reset.Font = new Font("Segoe UI Emoji", 9F);
            Control_InventoryTab_Button_Reset.Location = new Point(1035, 3);
            Control_InventoryTab_Button_Reset.Name = "Control_InventoryTab_Button_Reset";
            Control_InventoryTab_Button_Reset.Size = new Size(81, 29);
            Control_InventoryTab_Button_Reset.TabIndex = 7;
            Control_InventoryTab_Button_Reset.TabStop = false;
            Control_InventoryTab_Button_Reset.Text = "Reset";
            Control_InventoryTab_Button_Reset.UseVisualStyleBackColor = true;
            // 
            // Control_InventoryTab
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            Controls.Add(Control_InventoryTab_GroupBox_Main);
            Name = "Control_InventoryTab";
            Size = new Size(1175, 622);
            Control_InventoryTab_GroupBox_Main.ResumeLayout(false);
            Control_InventoryTab_GroupBox_Main.PerformLayout();
            Control_InventoryTab_TableLayout_Main.ResumeLayout(false);
            Control_InventoryTab_TableLayout_Main.PerformLayout();
            Control_InventoryTab_TableLayout_MiddleGroup.ResumeLayout(false);
            Control_InventoryTab_TableLayout_MiddleGroup.PerformLayout();
            Control_InventoryTab_TableLayout_TopGroup.ResumeLayout(false);
            Control_InventoryTab_TableLayout_TopGroup.PerformLayout();
            Control_InventoryTab_TableLayout_BottomGroup.ResumeLayout(false);
            Control_InventoryTab_TableLayout_BottomGroup.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private TableLayoutPanel Control_InventoryTab_TableLayout_BottomGroup;
        private TableLayoutPanel Control_InventoryTab_TableLayout_TopGroup;
        private TableLayoutPanel Control_InventoryTab_TableLayout_MiddleGroup;
        private Label Control_InventoryTab_Label_Notes;
        private RichTextBox Control_InventoryTab_RichTextBox_Notes;
        private ComboBox Control_InventoryTab_ComboBox_Location;
        private Label Control_InventoryTab_Label_Loc;
        private Label Control_InventoryTab_Label_Qty;
        private Label Control_InventoryTab_Label_Op;
        private Label Control_InventoryTab_Label_Part;
        private TableLayoutPanel tableLayoutPanel3;
    }

        
        #endregion
    }
