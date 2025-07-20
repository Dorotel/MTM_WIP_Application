using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.MainForm
{
    partial class Control_RemoveTab
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;

        #endregion

        private GroupBox Control_RemoveTab_GroupBox_MainControl;
        internal ComboBox Control_RemoveTab_ComboBox_Part;
        private Label Control_RemoveTab_Label_Part;
        private Label Control_RemoveTab_Label_Operation;
        private ComboBox Control_RemoveTab_ComboBox_Operation;
        private Panel Control_RemoveTab_Panel_DataGridView;
        private PictureBox Control_RemoveTab_Image_NothingFound;
        private DataGridView Control_RemoveTab_DataGridView_Main;
        private Button Control_RemoveTab_Button_AdvancedItemRemoval;
        private Button Control_RemoveTab_Button_Reset;
        private Button Control_RemoveTab_Button_Delete;
        private Button Control_RemoveTab_Button_Search;
        private Button Control_RemoveTab_Button_Toggle_RightPanel;
        private TableLayoutPanel Control_RemoveTab_Panel_Main;
        private Button Control_RemoveTab_Button_Undo;
        

        
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
            Control_RemoveTab_GroupBox_MainControl = new GroupBox();
            Control_RemoveTab_Panel_Main = new TableLayoutPanel();
            Control_RemoveTab_Panel_DataGridView = new Panel();
            Control_RemoveTab_Image_NothingFound = new PictureBox();
            Control_RemoveTab_DataGridView_Main = new DataGridView();
            Control_RemoveTab_Panel_Header = new Panel();
            Control_RemoveTab_TableLayout_Top = new TableLayoutPanel();
            Control_RemoveTab_Label_Part = new Label();
            Control_RemoveTab_ComboBox_Part = new ComboBox();
            Control_RemoveTab_Label_Operation = new Label();
            Control_RemoveTab_ComboBox_Operation = new ComboBox();
            Control_RemoveTab_TableLayout_Bottom = new TableLayoutPanel();
            Control_RemoveTab_Button_Undo = new Button();
            Control_RemoveTab_Button_Toggle_RightPanel = new Button();
            Control_RemoveTab_Button_Reset = new Button();
            Control_RemoveTab_Button_Print = new Button();
            Control_RemoveTab_Button_AdvancedItemRemoval = new Button();
            Control_RemoveTab_Button_Delete = new Button();
            Control_RemoveTab_Button_Search = new Button();
            Control_RemoveTab_GroupBox_MainControl.SuspendLayout();
            Control_RemoveTab_Panel_Main.SuspendLayout();
            Control_RemoveTab_Panel_DataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_RemoveTab_Image_NothingFound).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Control_RemoveTab_DataGridView_Main).BeginInit();
            Control_RemoveTab_Panel_Header.SuspendLayout();
            Control_RemoveTab_TableLayout_Top.SuspendLayout();
            Control_RemoveTab_TableLayout_Bottom.SuspendLayout();
            SuspendLayout();
            // 
            // Control_RemoveTab_GroupBox_MainControl
            // 
            Control_RemoveTab_GroupBox_MainControl.Controls.Add(Control_RemoveTab_Panel_Main);
            Control_RemoveTab_GroupBox_MainControl.Dock = DockStyle.Fill;
            Control_RemoveTab_GroupBox_MainControl.FlatStyle = FlatStyle.Flat;
            Control_RemoveTab_GroupBox_MainControl.Location = new Point(0, 0);
            Control_RemoveTab_GroupBox_MainControl.Name = "Control_RemoveTab_GroupBox_MainControl";
            Control_RemoveTab_GroupBox_MainControl.Size = new Size(815, 400);
            Control_RemoveTab_GroupBox_MainControl.TabIndex = 17;
            Control_RemoveTab_GroupBox_MainControl.TabStop = false;
            Control_RemoveTab_GroupBox_MainControl.Text = "Part Lookup and Remove";
            // 
            // Control_RemoveTab_Panel_Main
            // 
            Control_RemoveTab_Panel_Main.ColumnCount = 1;
            Control_RemoveTab_Panel_Main.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_Panel_Main.Controls.Add(Control_RemoveTab_Panel_DataGridView, 0, 1);
            Control_RemoveTab_Panel_Main.Controls.Add(Control_RemoveTab_Panel_Header, 0, 0);
            Control_RemoveTab_Panel_Main.Controls.Add(Control_RemoveTab_TableLayout_Bottom, 0, 2);
            Control_RemoveTab_Panel_Main.Dock = DockStyle.Fill;
            Control_RemoveTab_Panel_Main.Location = new Point(3, 19);
            Control_RemoveTab_Panel_Main.Name = "Control_RemoveTab_Panel_Main";
            Control_RemoveTab_Panel_Main.RowCount = 3;
            Control_RemoveTab_Panel_Main.RowStyles.Add(new RowStyle());
            Control_RemoveTab_Panel_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_RemoveTab_Panel_Main.RowStyles.Add(new RowStyle());
            Control_RemoveTab_Panel_Main.Size = new Size(809, 378);
            Control_RemoveTab_Panel_Main.TabIndex = 0;
            // 
            // Control_RemoveTab_Panel_DataGridView
            // 
            Control_RemoveTab_Panel_DataGridView.AutoSize = true;
            Control_RemoveTab_Panel_DataGridView.Controls.Add(Control_RemoveTab_Image_NothingFound);
            Control_RemoveTab_Panel_DataGridView.Controls.Add(Control_RemoveTab_DataGridView_Main);
            Control_RemoveTab_Panel_DataGridView.Dock = DockStyle.Fill;
            Control_RemoveTab_Panel_DataGridView.Location = new Point(3, 38);
            Control_RemoveTab_Panel_DataGridView.Name = "Control_RemoveTab_Panel_DataGridView";
            Control_RemoveTab_Panel_DataGridView.Size = new Size(803, 296);
            Control_RemoveTab_Panel_DataGridView.TabIndex = 21;
            // 
            // Control_RemoveTab_Image_NothingFound
            // 
            Control_RemoveTab_Image_NothingFound.BorderStyle = BorderStyle.FixedSingle;
            Control_RemoveTab_Image_NothingFound.Dock = DockStyle.Fill;
            Control_RemoveTab_Image_NothingFound.ErrorImage = null;
            Control_RemoveTab_Image_NothingFound.Image = Properties.Resources.NothingFound;
            Control_RemoveTab_Image_NothingFound.InitialImage = null;
            Control_RemoveTab_Image_NothingFound.Location = new Point(0, 0);
            Control_RemoveTab_Image_NothingFound.Name = "Control_RemoveTab_Image_NothingFound";
            Control_RemoveTab_Image_NothingFound.Size = new Size(803, 296);
            Control_RemoveTab_Image_NothingFound.SizeMode = PictureBoxSizeMode.CenterImage;
            Control_RemoveTab_Image_NothingFound.TabIndex = 6;
            Control_RemoveTab_Image_NothingFound.TabStop = false;
            Control_RemoveTab_Image_NothingFound.Visible = false;
            // 
            // Control_RemoveTab_DataGridView_Main
            // 
            Control_RemoveTab_DataGridView_Main.AllowUserToAddRows = false;
            Control_RemoveTab_DataGridView_Main.AllowUserToDeleteRows = false;
            Control_RemoveTab_DataGridView_Main.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Control_RemoveTab_DataGridView_Main.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Control_RemoveTab_DataGridView_Main.BorderStyle = BorderStyle.Fixed3D;
            Control_RemoveTab_DataGridView_Main.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            Control_RemoveTab_DataGridView_Main.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            Control_RemoveTab_DataGridView_Main.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            Control_RemoveTab_DataGridView_Main.ColumnHeadersHeight = 34;
            Control_RemoveTab_DataGridView_Main.Dock = DockStyle.Fill;
            Control_RemoveTab_DataGridView_Main.EditMode = DataGridViewEditMode.EditProgrammatically;
            Control_RemoveTab_DataGridView_Main.Location = new Point(0, 0);
            Control_RemoveTab_DataGridView_Main.Name = "Control_RemoveTab_DataGridView_Main";
            Control_RemoveTab_DataGridView_Main.ReadOnly = true;
            Control_RemoveTab_DataGridView_Main.RowHeadersWidth = 62;
            Control_RemoveTab_DataGridView_Main.RowTemplate.ReadOnly = true;
            Control_RemoveTab_DataGridView_Main.RowTemplate.Resizable = DataGridViewTriState.True;
            Control_RemoveTab_DataGridView_Main.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Control_RemoveTab_DataGridView_Main.ShowCellErrors = false;
            Control_RemoveTab_DataGridView_Main.ShowCellToolTips = false;
            Control_RemoveTab_DataGridView_Main.ShowEditingIcon = false;
            Control_RemoveTab_DataGridView_Main.ShowRowErrors = false;
            Control_RemoveTab_DataGridView_Main.Size = new Size(803, 296);
            Control_RemoveTab_DataGridView_Main.StandardTab = true;
            Control_RemoveTab_DataGridView_Main.TabIndex = 4;
            // 
            // Control_RemoveTab_Panel_Header
            // 
            Control_RemoveTab_Panel_Header.AutoSize = true;
            Control_RemoveTab_Panel_Header.Controls.Add(Control_RemoveTab_TableLayout_Top);
            Control_RemoveTab_Panel_Header.Dock = DockStyle.Fill;
            Control_RemoveTab_Panel_Header.Location = new Point(3, 3);
            Control_RemoveTab_Panel_Header.Name = "Control_RemoveTab_Panel_Header";
            Control_RemoveTab_Panel_Header.Size = new Size(803, 29);
            Control_RemoveTab_Panel_Header.TabIndex = 22;
            // 
            // Control_RemoveTab_TableLayout_Top
            // 
            Control_RemoveTab_TableLayout_Top.AutoSize = true;
            Control_RemoveTab_TableLayout_Top.ColumnCount = 5;
            Control_RemoveTab_TableLayout_Top.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Top.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Top.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Top.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_RemoveTab_TableLayout_Top.Controls.Add(Control_RemoveTab_Label_Part, 0, 0);
            Control_RemoveTab_TableLayout_Top.Controls.Add(Control_RemoveTab_ComboBox_Part, 1, 0);
            Control_RemoveTab_TableLayout_Top.Controls.Add(Control_RemoveTab_Label_Operation, 2, 0);
            Control_RemoveTab_TableLayout_Top.Controls.Add(Control_RemoveTab_ComboBox_Operation, 3, 0);
            Control_RemoveTab_TableLayout_Top.Dock = DockStyle.Fill;
            Control_RemoveTab_TableLayout_Top.Location = new Point(0, 0);
            Control_RemoveTab_TableLayout_Top.Name = "Control_RemoveTab_TableLayout_Top";
            Control_RemoveTab_TableLayout_Top.RowCount = 1;
            Control_RemoveTab_TableLayout_Top.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_RemoveTab_TableLayout_Top.Size = new Size(803, 29);
            Control_RemoveTab_TableLayout_Top.TabIndex = 6;
            // 
            // Control_RemoveTab_Label_Part
            // 
            Control_RemoveTab_Label_Part.AutoSize = true;
            Control_RemoveTab_Label_Part.Dock = DockStyle.Fill;
            Control_RemoveTab_Label_Part.Location = new Point(3, 0);
            Control_RemoveTab_Label_Part.Name = "Control_RemoveTab_Label_Part";
            Control_RemoveTab_Label_Part.Size = new Size(78, 29);
            Control_RemoveTab_Label_Part.TabIndex = 4;
            Control_RemoveTab_Label_Part.Text = "Part Number:";
            Control_RemoveTab_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_RemoveTab_ComboBox_Part
            // 
            Control_RemoveTab_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_RemoveTab_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_RemoveTab_ComboBox_Part.Dock = DockStyle.Fill;
            Control_RemoveTab_ComboBox_Part.FormattingEnabled = true;
            Control_RemoveTab_ComboBox_Part.Location = new Point(87, 3);
            Control_RemoveTab_ComboBox_Part.Name = "Control_RemoveTab_ComboBox_Part";
            Control_RemoveTab_ComboBox_Part.Size = new Size(279, 23);
            Control_RemoveTab_ComboBox_Part.TabIndex = 1;
            // 
            // Control_RemoveTab_Label_Operation
            // 
            Control_RemoveTab_Label_Operation.AutoSize = true;
            Control_RemoveTab_Label_Operation.Dock = DockStyle.Fill;
            Control_RemoveTab_Label_Operation.Location = new Point(372, 0);
            Control_RemoveTab_Label_Operation.Name = "Control_RemoveTab_Label_Operation";
            Control_RemoveTab_Label_Operation.Size = new Size(63, 29);
            Control_RemoveTab_Label_Operation.TabIndex = 5;
            Control_RemoveTab_Label_Operation.Text = "Operation:";
            Control_RemoveTab_Label_Operation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_RemoveTab_ComboBox_Operation
            // 
            Control_RemoveTab_ComboBox_Operation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_RemoveTab_ComboBox_Operation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_RemoveTab_ComboBox_Operation.FormattingEnabled = true;
            Control_RemoveTab_ComboBox_Operation.Location = new Point(441, 3);
            Control_RemoveTab_ComboBox_Operation.Name = "Control_RemoveTab_ComboBox_Operation";
            Control_RemoveTab_ComboBox_Operation.Size = new Size(180, 23);
            Control_RemoveTab_ComboBox_Operation.TabIndex = 2;
            // 
            // Control_RemoveTab_TableLayout_Bottom
            // 
            Control_RemoveTab_TableLayout_Bottom.AutoSize = true;
            Control_RemoveTab_TableLayout_Bottom.ColumnCount = 8;
            Control_RemoveTab_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_RemoveTab_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Bottom.ColumnStyles.Add(new ColumnStyle());
            Control_RemoveTab_TableLayout_Bottom.Controls.Add(Control_RemoveTab_Button_Undo, 4, 0);
            Control_RemoveTab_TableLayout_Bottom.Controls.Add(Control_RemoveTab_Button_Toggle_RightPanel, 7, 0);
            Control_RemoveTab_TableLayout_Bottom.Controls.Add(Control_RemoveTab_Button_Reset, 6, 0);
            Control_RemoveTab_TableLayout_Bottom.Controls.Add(Control_RemoveTab_Button_Print, 5, 0);
            Control_RemoveTab_TableLayout_Bottom.Controls.Add(Control_RemoveTab_Button_AdvancedItemRemoval, 2, 0);
            Control_RemoveTab_TableLayout_Bottom.Controls.Add(Control_RemoveTab_Button_Delete, 1, 0);
            Control_RemoveTab_TableLayout_Bottom.Controls.Add(Control_RemoveTab_Button_Search, 0, 0);
            Control_RemoveTab_TableLayout_Bottom.Dock = DockStyle.Fill;
            Control_RemoveTab_TableLayout_Bottom.Location = new Point(3, 340);
            Control_RemoveTab_TableLayout_Bottom.Name = "Control_RemoveTab_TableLayout_Bottom";
            Control_RemoveTab_TableLayout_Bottom.RowCount = 1;
            Control_RemoveTab_TableLayout_Bottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_RemoveTab_TableLayout_Bottom.Size = new Size(803, 35);
            Control_RemoveTab_TableLayout_Bottom.TabIndex = 23;
            // 
            // Control_RemoveTab_Button_Undo
            // 
            Control_RemoveTab_Button_Undo.Dock = DockStyle.Right;
            Control_RemoveTab_Button_Undo.Enabled = false;
            Control_RemoveTab_Button_Undo.Location = new Point(481, 3);
            Control_RemoveTab_Button_Undo.Name = "Control_RemoveTab_Button_Undo";
            Control_RemoveTab_Button_Undo.Size = new Size(90, 29);
            Control_RemoveTab_Button_Undo.TabIndex = 6;
            Control_RemoveTab_Button_Undo.TabStop = false;
            Control_RemoveTab_Button_Undo.Text = "Undo";
            Control_RemoveTab_Button_Undo.UseVisualStyleBackColor = true;
            Control_RemoveTab_Button_Undo.Click += Control_RemoveTab_Button_Undo_Click;
            // 
            // Control_RemoveTab_Button_Toggle_RightPanel
            // 
            Control_RemoveTab_Button_Toggle_RightPanel.Font = new Font("Segoe UI", 10F);
            Control_RemoveTab_Button_Toggle_RightPanel.ForeColor = Color.Green;
            Control_RemoveTab_Button_Toggle_RightPanel.Location = new Point(769, 3);
            Control_RemoveTab_Button_Toggle_RightPanel.Name = "Control_RemoveTab_Button_Toggle_RightPanel";
            Control_RemoveTab_Button_Toggle_RightPanel.Size = new Size(31, 29);
            Control_RemoveTab_Button_Toggle_RightPanel.TabIndex = 9;
            Control_RemoveTab_Button_Toggle_RightPanel.Text = "←";
            Control_RemoveTab_Button_Toggle_RightPanel.UseVisualStyleBackColor = true;
            Control_RemoveTab_Button_Toggle_RightPanel.Click += Control_RemoveTab_Button_Toggle_RightPanel_Click;
            // 
            // Control_RemoveTab_Button_Reset
            // 
            Control_RemoveTab_Button_Reset.Location = new Point(673, 3);
            Control_RemoveTab_Button_Reset.Name = "Control_RemoveTab_Button_Reset";
            Control_RemoveTab_Button_Reset.Size = new Size(90, 29);
            Control_RemoveTab_Button_Reset.TabIndex = 8;
            Control_RemoveTab_Button_Reset.TabStop = false;
            Control_RemoveTab_Button_Reset.Text = "Reset";
            Control_RemoveTab_Button_Reset.UseVisualStyleBackColor = true;
            // 
            // Control_RemoveTab_Button_Print
            // 
            Control_RemoveTab_Button_Print.Dock = DockStyle.Right;
            Control_RemoveTab_Button_Print.Enabled = false;
            Control_RemoveTab_Button_Print.Location = new Point(577, 3);
            Control_RemoveTab_Button_Print.Name = "Control_RemoveTab_Button_Print";
            Control_RemoveTab_Button_Print.Size = new Size(90, 29);
            Control_RemoveTab_Button_Print.TabIndex = 7;
            Control_RemoveTab_Button_Print.Text = "Print";
            Control_RemoveTab_Button_Print.UseVisualStyleBackColor = true;
            // 
            // Control_RemoveTab_Button_AdvancedItemRemoval
            // 
            Control_RemoveTab_Button_AdvancedItemRemoval.ForeColor = Color.DarkRed;
            Control_RemoveTab_Button_AdvancedItemRemoval.Location = new Point(195, 3);
            Control_RemoveTab_Button_AdvancedItemRemoval.Name = "Control_RemoveTab_Button_AdvancedItemRemoval";
            Control_RemoveTab_Button_AdvancedItemRemoval.Size = new Size(90, 28);
            Control_RemoveTab_Button_AdvancedItemRemoval.TabIndex = 5;
            Control_RemoveTab_Button_AdvancedItemRemoval.Text = "Advanced";
            Control_RemoveTab_Button_AdvancedItemRemoval.UseVisualStyleBackColor = true;
            // 
            // Control_RemoveTab_Button_Delete
            // 
            Control_RemoveTab_Button_Delete.Location = new Point(99, 3);
            Control_RemoveTab_Button_Delete.Name = "Control_RemoveTab_Button_Delete";
            Control_RemoveTab_Button_Delete.Size = new Size(90, 29);
            Control_RemoveTab_Button_Delete.TabIndex = 4;
            Control_RemoveTab_Button_Delete.Text = "Delete";
            Control_RemoveTab_Button_Delete.UseVisualStyleBackColor = true;
            // 
            // Control_RemoveTab_Button_Search
            // 
            Control_RemoveTab_Button_Search.Location = new Point(3, 3);
            Control_RemoveTab_Button_Search.Name = "Control_RemoveTab_Button_Search";
            Control_RemoveTab_Button_Search.Size = new Size(90, 29);
            Control_RemoveTab_Button_Search.TabIndex = 3;
            Control_RemoveTab_Button_Search.Text = "Search";
            Control_RemoveTab_Button_Search.UseVisualStyleBackColor = true;
            Control_RemoveTab_Button_Search.Click += Control_RemoveTab_Button_Search_Click;
            // 
            // Control_RemoveTab
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            Controls.Add(Control_RemoveTab_GroupBox_MainControl);
            Name = "Control_RemoveTab";
            Size = new Size(815, 400);
            Control_RemoveTab_GroupBox_MainControl.ResumeLayout(false);
            Control_RemoveTab_Panel_Main.ResumeLayout(false);
            Control_RemoveTab_Panel_Main.PerformLayout();
            Control_RemoveTab_Panel_DataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_RemoveTab_Image_NothingFound).EndInit();
            ((System.ComponentModel.ISupportInitialize)Control_RemoveTab_DataGridView_Main).EndInit();
            Control_RemoveTab_Panel_Header.ResumeLayout(false);
            Control_RemoveTab_Panel_Header.PerformLayout();
            Control_RemoveTab_TableLayout_Top.ResumeLayout(false);
            Control_RemoveTab_TableLayout_Top.PerformLayout();
            Control_RemoveTab_TableLayout_Bottom.ResumeLayout(false);
            ResumeLayout(false);
        }
        private Button Control_RemoveTab_Button_Print;
        private TableLayoutPanel Control_RemoveTab_TableLayout_Bottom;
        private Panel Control_RemoveTab_Panel_Header;
        private TableLayoutPanel Control_RemoveTab_TableLayout_Top;
    }

        
        #endregion
    }
