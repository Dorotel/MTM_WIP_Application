using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.MainForm
{
    partial class ControlRemoveTab
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
            Control_RemoveTab_GroupBox_MainControl = new GroupBox();
            Control_RemoveTab_Panel_Main = new TableLayoutPanel();
            Control_RemoveTab_Panel_DataGridView = new Panel();
            Control_RemoveTab_Image_NothingFound = new PictureBox();
            Control_RemoveTab_DataGridView_Main = new DataGridView();
            Control_RemoveTab_Panel_Footer = new Panel();
            Control_RemoveTab_Button_Undo = new Button();
            Control_RemoveTab_Button_Toggle_RightPanel = new Button();
            Control_RemoveTab_Button_AdvancedItemRemoval = new Button();
            Control_RemoveTab_Button_Reset = new Button();
            Control_RemoveTab_Button_Delete = new Button();
            Control_RemoveTab_Button_Search = new Button();
            Control_RemoveTab_Panel_Header = new Panel();
            Control_RemoveTab_ComboBox_Part = new ComboBox();
            Control_RemoveTab_Label_Part = new Label();
            Control_RemoveTab_Label_Operation = new Label();
            Control_RemoveTab_ComboBox_Operation = new ComboBox();
            Control_RemoveTab_GroupBox_MainControl.SuspendLayout();
            Control_RemoveTab_Panel_Main.SuspendLayout();
            Control_RemoveTab_Panel_DataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Control_RemoveTab_Image_NothingFound).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Control_RemoveTab_DataGridView_Main).BeginInit();
            Control_RemoveTab_Panel_Footer.SuspendLayout();
            Control_RemoveTab_Panel_Header.SuspendLayout();
            SuspendLayout();
            // 
            // Control_RemoveTab_GroupBox_MainControl
            // 
            Control_RemoveTab_GroupBox_MainControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Control_RemoveTab_GroupBox_MainControl.Controls.Add(Control_RemoveTab_Panel_Main);
            Control_RemoveTab_GroupBox_MainControl.Dock = DockStyle.Fill;
            Control_RemoveTab_GroupBox_MainControl.FlatStyle = FlatStyle.Flat;
            Control_RemoveTab_GroupBox_MainControl.Location = new Point(0, 0);
            Control_RemoveTab_GroupBox_MainControl.Name = "Control_RemoveTab_GroupBox_MainControl";
            Control_RemoveTab_GroupBox_MainControl.Size = new Size(815, 384);
            Control_RemoveTab_GroupBox_MainControl.TabIndex = 17;
            Control_RemoveTab_GroupBox_MainControl.TabStop = false;
            Control_RemoveTab_GroupBox_MainControl.Text = "Part Lookup and Remove";
            // 
            // Control_RemoveTab_Panel_Main
            // 
            Control_RemoveTab_Panel_Main.ColumnCount = 1;
            Control_RemoveTab_Panel_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Control_RemoveTab_Panel_Main.Controls.Add(Control_RemoveTab_Panel_DataGridView, 0, 1);
            Control_RemoveTab_Panel_Main.Controls.Add(Control_RemoveTab_Panel_Footer, 0, 2);
            Control_RemoveTab_Panel_Main.Controls.Add(Control_RemoveTab_Panel_Header, 0, 0);
            Control_RemoveTab_Panel_Main.Dock = DockStyle.Fill;
            Control_RemoveTab_Panel_Main.Location = new Point(3, 19);
            Control_RemoveTab_Panel_Main.Name = "Control_RemoveTab_Panel_Main";
            Control_RemoveTab_Panel_Main.RowCount = 3;
            Control_RemoveTab_Panel_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            Control_RemoveTab_Panel_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Control_RemoveTab_Panel_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            Control_RemoveTab_Panel_Main.Size = new Size(809, 362);
            Control_RemoveTab_Panel_Main.TabIndex = 0;
            // 
            // Control_RemoveTab_Panel_DataGridView
            // 
            Control_RemoveTab_Panel_DataGridView.Controls.Add(Control_RemoveTab_Image_NothingFound);
            Control_RemoveTab_Panel_DataGridView.Controls.Add(Control_RemoveTab_DataGridView_Main);
            Control_RemoveTab_Panel_DataGridView.Dock = DockStyle.Fill;
            Control_RemoveTab_Panel_DataGridView.Location = new Point(3, 45);
            Control_RemoveTab_Panel_DataGridView.Name = "Control_RemoveTab_Panel_DataGridView";
            Control_RemoveTab_Panel_DataGridView.Size = new Size(803, 272);
            Control_RemoveTab_Panel_DataGridView.TabIndex = 21;
            // 
            // Control_RemoveTab_Image_NothingFound
            // 
            Control_RemoveTab_Image_NothingFound.Dock = DockStyle.Fill;
            Control_RemoveTab_Image_NothingFound.ErrorImage = null;
            Control_RemoveTab_Image_NothingFound.Image = Properties.Resources.NothingFound;
            Control_RemoveTab_Image_NothingFound.InitialImage = null;
            Control_RemoveTab_Image_NothingFound.Location = new Point(0, 0);
            Control_RemoveTab_Image_NothingFound.Name = "Control_RemoveTab_Image_NothingFound";
            Control_RemoveTab_Image_NothingFound.Size = new Size(803, 272);
            Control_RemoveTab_Image_NothingFound.SizeMode = PictureBoxSizeMode.Zoom;
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
            Control_RemoveTab_DataGridView_Main.Size = new Size(803, 272);
            Control_RemoveTab_DataGridView_Main.StandardTab = true;
            Control_RemoveTab_DataGridView_Main.TabIndex = 4;
            // 
            // Control_RemoveTab_Panel_Footer
            // 
            Control_RemoveTab_Panel_Footer.Controls.Add(Control_RemoveTab_Button_Undo);
            Control_RemoveTab_Panel_Footer.Controls.Add(Control_RemoveTab_Button_Toggle_RightPanel);
            Control_RemoveTab_Panel_Footer.Controls.Add(Control_RemoveTab_Button_AdvancedItemRemoval);
            Control_RemoveTab_Panel_Footer.Controls.Add(Control_RemoveTab_Button_Reset);
            Control_RemoveTab_Panel_Footer.Controls.Add(Control_RemoveTab_Button_Delete);
            Control_RemoveTab_Panel_Footer.Controls.Add(Control_RemoveTab_Button_Search);
            Control_RemoveTab_Panel_Footer.Dock = DockStyle.Fill;
            Control_RemoveTab_Panel_Footer.Location = new Point(3, 323);
            Control_RemoveTab_Panel_Footer.Name = "Control_RemoveTab_Panel_Footer";
            Control_RemoveTab_Panel_Footer.Padding = new Padding(3);
            Control_RemoveTab_Panel_Footer.Size = new Size(803, 36);
            Control_RemoveTab_Panel_Footer.TabIndex = 20;
            // 
            // Control_RemoveTab_Button_Undo
            // 
            Control_RemoveTab_Button_Undo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_RemoveTab_Button_Undo.Enabled = false;
            Control_RemoveTab_Button_Undo.Location = new Point(637, 5);
            Control_RemoveTab_Button_Undo.Name = "Control_RemoveTab_Button_Undo";
            Control_RemoveTab_Button_Undo.Size = new Size(60, 28);
            Control_RemoveTab_Button_Undo.TabIndex = 15;
            Control_RemoveTab_Button_Undo.TabStop = false;
            Control_RemoveTab_Button_Undo.Text = "Undo";
            Control_RemoveTab_Button_Undo.UseVisualStyleBackColor = true;
            Control_RemoveTab_Button_Undo.Click += Control_RemoveTab_Button_Undo_Click;
            // 
            // Control_RemoveTab_Button_Toggle_RightPanel
            // 
            Control_RemoveTab_Button_Toggle_RightPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_RemoveTab_Button_Toggle_RightPanel.Font = new Font("Segoe UI", 10F);
            Control_RemoveTab_Button_Toggle_RightPanel.ForeColor = Color.Green;
            Control_RemoveTab_Button_Toggle_RightPanel.Location = new Point(769, 4);
            Control_RemoveTab_Button_Toggle_RightPanel.Name = "Control_RemoveTab_Button_Toggle_RightPanel";
            Control_RemoveTab_Button_Toggle_RightPanel.Size = new Size(28, 28);
            Control_RemoveTab_Button_Toggle_RightPanel.TabIndex = 14;
            Control_RemoveTab_Button_Toggle_RightPanel.Text = "←";
            Control_RemoveTab_Button_Toggle_RightPanel.UseVisualStyleBackColor = true;
            Control_RemoveTab_Button_Toggle_RightPanel.Click += Control_RemoveTab_Button_Toggle_RightPanel_Click;
            // 
            // Control_RemoveTab_Button_AdvancedItemRemoval
            // 
            Control_RemoveTab_Button_AdvancedItemRemoval.ForeColor = Color.DarkRed;
            Control_RemoveTab_Button_AdvancedItemRemoval.Location = new Point(136, 4);
            Control_RemoveTab_Button_AdvancedItemRemoval.Name = "Control_RemoveTab_Button_AdvancedItemRemoval";
            Control_RemoveTab_Button_AdvancedItemRemoval.Size = new Size(90, 28);
            Control_RemoveTab_Button_AdvancedItemRemoval.TabIndex = 13;
            Control_RemoveTab_Button_AdvancedItemRemoval.Text = "Advanced";
            Control_RemoveTab_Button_AdvancedItemRemoval.UseVisualStyleBackColor = true;
            // 
            // Control_RemoveTab_Button_Reset
            // 
            Control_RemoveTab_Button_Reset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Control_RemoveTab_Button_Reset.Location = new Point(703, 4);
            Control_RemoveTab_Button_Reset.Name = "Control_RemoveTab_Button_Reset";
            Control_RemoveTab_Button_Reset.Size = new Size(60, 28);
            Control_RemoveTab_Button_Reset.TabIndex = 5;
            Control_RemoveTab_Button_Reset.TabStop = false;
            Control_RemoveTab_Button_Reset.Text = "Reset";
            Control_RemoveTab_Button_Reset.UseVisualStyleBackColor = true;
            // 
            // Control_RemoveTab_Button_Delete
            // 
            Control_RemoveTab_Button_Delete.Location = new Point(72, 4);
            Control_RemoveTab_Button_Delete.Name = "Control_RemoveTab_Button_Delete";
            Control_RemoveTab_Button_Delete.Size = new Size(60, 28);
            Control_RemoveTab_Button_Delete.TabIndex = 8;
            Control_RemoveTab_Button_Delete.Text = "Delete";
            Control_RemoveTab_Button_Delete.UseVisualStyleBackColor = true;
            // 
            // Control_RemoveTab_Button_Search
            // 
            Control_RemoveTab_Button_Search.Location = new Point(6, 4);
            Control_RemoveTab_Button_Search.Name = "Control_RemoveTab_Button_Search";
            Control_RemoveTab_Button_Search.Size = new Size(60, 28);
            Control_RemoveTab_Button_Search.TabIndex = 3;
            Control_RemoveTab_Button_Search.Text = "Search";
            Control_RemoveTab_Button_Search.UseVisualStyleBackColor = true;
            Control_RemoveTab_Button_Search.Click += Control_RemoveTab_Button_Search_Click;
            // 
            // Control_RemoveTab_Panel_Header
            // 
            Control_RemoveTab_Panel_Header.Controls.Add(Control_RemoveTab_ComboBox_Part);
            Control_RemoveTab_Panel_Header.Controls.Add(Control_RemoveTab_Label_Part);
            Control_RemoveTab_Panel_Header.Controls.Add(Control_RemoveTab_Label_Operation);
            Control_RemoveTab_Panel_Header.Controls.Add(Control_RemoveTab_ComboBox_Operation);
            Control_RemoveTab_Panel_Header.Dock = DockStyle.Fill;
            Control_RemoveTab_Panel_Header.Location = new Point(3, 3);
            Control_RemoveTab_Panel_Header.Name = "Control_RemoveTab_Panel_Header";
            Control_RemoveTab_Panel_Header.Size = new Size(803, 36);
            Control_RemoveTab_Panel_Header.TabIndex = 22;
            // 
            // Control_RemoveTab_ComboBox_Part
            // 
            Control_RemoveTab_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_RemoveTab_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_RemoveTab_ComboBox_Part.FormattingEnabled = true;
            Control_RemoveTab_ComboBox_Part.Location = new Point(101, 7);
            Control_RemoveTab_ComboBox_Part.Name = "Control_RemoveTab_ComboBox_Part";
            Control_RemoveTab_ComboBox_Part.Size = new Size(376, 23);
            Control_RemoveTab_ComboBox_Part.TabIndex = 1;
            // 
            // Control_RemoveTab_Label_Part
            // 
            Control_RemoveTab_Label_Part.Location = new Point(3, 7);
            Control_RemoveTab_Label_Part.Name = "Control_RemoveTab_Label_Part";
            Control_RemoveTab_Label_Part.Size = new Size(92, 23);
            Control_RemoveTab_Label_Part.TabIndex = 4;
            Control_RemoveTab_Label_Part.Text = "Part Number:";
            Control_RemoveTab_Label_Part.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_RemoveTab_Label_Operation
            // 
            Control_RemoveTab_Label_Operation.Location = new Point(483, 7);
            Control_RemoveTab_Label_Operation.Name = "Control_RemoveTab_Label_Operation";
            Control_RemoveTab_Label_Operation.Size = new Size(90, 23);
            Control_RemoveTab_Label_Operation.TabIndex = 5;
            Control_RemoveTab_Label_Operation.Text = "Operation:";
            Control_RemoveTab_Label_Operation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Control_RemoveTab_ComboBox_Operation
            // 
            Control_RemoveTab_ComboBox_Operation.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Control_RemoveTab_ComboBox_Operation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Control_RemoveTab_ComboBox_Operation.AutoCompleteSource = AutoCompleteSource.ListItems;
            Control_RemoveTab_ComboBox_Operation.FormattingEnabled = true;
            Control_RemoveTab_ComboBox_Operation.Location = new Point(579, 7);
            Control_RemoveTab_ComboBox_Operation.Name = "Control_RemoveTab_ComboBox_Operation";
            Control_RemoveTab_ComboBox_Operation.Size = new Size(218, 23);
            Control_RemoveTab_ComboBox_Operation.TabIndex = 2;
            // 
            // ControlRemoveTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(Control_RemoveTab_GroupBox_MainControl);
            Name = "ControlRemoveTab";
            Size = new Size(815, 384);
            Control_RemoveTab_GroupBox_MainControl.ResumeLayout(false);
            Control_RemoveTab_Panel_Main.ResumeLayout(false);
            Control_RemoveTab_Panel_DataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Control_RemoveTab_Image_NothingFound).EndInit();
            ((System.ComponentModel.ISupportInitialize)Control_RemoveTab_DataGridView_Main).EndInit();
            Control_RemoveTab_Panel_Footer.ResumeLayout(false);
            Control_RemoveTab_Panel_Header.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox Control_RemoveTab_GroupBox_MainControl;
        private Panel Control_RemoveTab_Panel_Header;
        private ComboBox Control_RemoveTab_ComboBox_Part;
        private Label Control_RemoveTab_Label_Part;
        private Label Control_RemoveTab_Label_Operation;
        private ComboBox Control_RemoveTab_ComboBox_Operation;
        private Panel Control_RemoveTab_Panel_DataGridView;
        private PictureBox Control_RemoveTab_Image_NothingFound;
        private DataGridView Control_RemoveTab_DataGridView_Main;
        private Panel Control_RemoveTab_Panel_Footer;
        private Button Control_RemoveTab_Button_AdvancedItemRemoval;
        private Button Control_RemoveTab_Button_Reset;
        private Button Control_RemoveTab_Button_Delete;
        private Button Control_RemoveTab_Button_Search;
        private Button Control_RemoveTab_Button_Toggle_RightPanel;
        private TableLayoutPanel Control_RemoveTab_Panel_Main;
        private Button Control_RemoveTab_Button_Undo;
    }
}
