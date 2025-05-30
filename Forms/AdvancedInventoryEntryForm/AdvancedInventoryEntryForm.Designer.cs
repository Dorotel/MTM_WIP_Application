using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace MTM_WIP_Application.Forms.AdvancedInventoryEntryForm;

partial class AdvancedInventoryEntryForm
{

    // Tab control and tabs
    private TabControl AdvancedEntry_TabControl;
    private TabPage AdvancedEntry_TabControl_Single;
    private TabPage AdvancedEntry_TabControl_MultiLoc;
    private TabPage AdvancedEntry_TabControl_Import;

    // --- Tab 1: Single Item, Multiple Times, One Location ---
    private GroupBox AdvancedEntry_Single_GroupBox_Main;
    private Label AdvancedEntry_Single_Label_Part;
    private ComboBox AdvancedEntry_Single_ComboBox_Part;
    private Label AdvancedEntry_Single_Label_Op;
    private ComboBox AdvancedEntry_Single_ComboBox_Op;
    private Label AdvancedEntry_Single_Label_Loc;
    private ComboBox AdvancedEntry_Single_ComboBox_Loc;
    private Label AdvancedEntry_Single_Label_Qty;
    private TextBox AdvancedEntry_Single_TextBox_Qty;
    private Label AdvancedEntry_Single_Label_Count;
    private TextBox AdvancedEntry_Single_TextBox_Count;
    private Label AdvancedEntry_Single_Label_Notes;
    private RichTextBox AdvancedEntry_Single_RichTextBox_Notes;
    private Button AdvancedEntry_Single_Button_Save;
    private Button AdvancedEntry_Single_Button_Reset;
    // Left: Item entry
    private GroupBox AdvancedEntry_MultiLoc_GroupBox_Item;
    private Label AdvancedEntry_MultiLoc_Label_Part;
    private ComboBox AdvancedEntry_MultiLoc_ComboBox_Part;
    private Label AdvancedEntry_MultiLoc_Label_Op;
    private ComboBox AdvancedEntry_MultiLoc_ComboBox_Op;
    private Label AdvancedEntry_MultiLoc_Label_Qty;
    private TextBox AdvancedEntry_MultiLoc_TextBox_Qty;
    private Label AdvancedEntry_MultiLoc_Label_Notes;
    private RichTextBox AdvancedEntry_MultiLoc_RichTextBox_Notes;
    private Label AdvancedEntry_MultiLoc_Label_Loc;
    private ComboBox AdvancedEntry_MultiLoc_ComboBox_Loc;
    private Button AdvancedEntry_MultiLoc_Button_AddLoc;
    // Right: Transaction preview
    private GroupBox AdvancedEntry_MultiLoc_GroupBox_Preview;
    private ListView AdvancedEntry_MultiLoc_ListView_Preview;
    private Button AdvancedEntry_MultiLoc_Button_SaveAll;
    private Button AdvancedEntry_MultiLoc_Button_Reset;

    // --- Tab 3: Import ---
    private DataGridView AdvancedEntry_Import_DataGridView;
    private Panel AdvancedEntry_Import_Panel_Buttons;
    private Button AdvancedEntry_Import_Button_OpenExcel;
    private Button AdvancedEntry_Import_Button_ImportExcel;
    private Button AdvancedEntry_Import_Button_OpenCsv;
    private Button AdvancedEntry_Import_Button_ImportCsv;
    private Button AdvancedEntry_Import_Button_Save;
    private Button AdvancedEntry_Import_Button_Close;

    private void InitializeComponent()
    {
        AdvancedEntry_TabControl = new TabControl();
        AdvancedEntry_TabControl_Single = new TabPage();
        AdvancedEntry_Single_GroupBox_Main = new GroupBox();
        AdvancedEntry_Single_Label_Part = new Label();
        AdvancedEntry_Single_ComboBox_Part = new ComboBox();
        AdvancedEntry_Single_Label_Op = new Label();
        AdvancedEntry_Single_ComboBox_Op = new ComboBox();
        AdvancedEntry_Single_Label_Loc = new Label();
        AdvancedEntry_Single_ComboBox_Loc = new ComboBox();
        AdvancedEntry_Single_Label_Qty = new Label();
        AdvancedEntry_Single_TextBox_Qty = new TextBox();
        AdvancedEntry_Single_Label_Count = new Label();
        AdvancedEntry_Single_TextBox_Count = new TextBox();
        AdvancedEntry_Single_Label_Notes = new Label();
        AdvancedEntry_Single_RichTextBox_Notes = new RichTextBox();
        AdvancedEntry_Single_Button_Save = new Button();
        AdvancedEntry_Single_Button_Reset = new Button();
        AdvancedEntry_TabControl_MultiLoc = new TabPage();
        AdvancedEntry_MultiLoc_GroupBox_Item = new GroupBox();
        AdvancedEntry_MultiLoc_Label_Part = new Label();
        AdvancedEntry_MultiLoc_ComboBox_Part = new ComboBox();
        AdvancedEntry_MultiLoc_Label_Op = new Label();
        AdvancedEntry_MultiLoc_ComboBox_Op = new ComboBox();
        AdvancedEntry_MultiLoc_Label_Qty = new Label();
        AdvancedEntry_MultiLoc_TextBox_Qty = new TextBox();
        AdvancedEntry_MultiLoc_Label_Notes = new Label();
        AdvancedEntry_MultiLoc_RichTextBox_Notes = new RichTextBox();
        AdvancedEntry_MultiLoc_Label_Loc = new Label();
        AdvancedEntry_MultiLoc_ComboBox_Loc = new ComboBox();
        AdvancedEntry_MultiLoc_Button_AddLoc = new Button();
        AdvancedEntry_MultiLoc_GroupBox_Preview = new GroupBox();
        AdvancedEntry_MultiLoc_ListView_Preview = new ListView();
        AdvancedEntry_MultiLoc_Button_SaveAll = new Button();
        AdvancedEntry_MultiLoc_Button_Reset = new Button();
        AdvancedEntry_TabControl_Import = new TabPage();
        AdvancedEntry_Import_Button_OpenExcel = new Button();
        AdvancedEntry_Import_Panel_Buttons = new Panel();
        AdvancedEntry_Import_Button_Save = new Button();
        AdvancedEntry_Import_Button_Close = new Button();
        AdvancedEntry_Import_Button_ImportExcel = new Button();
        AdvancedEntry_Import_Button_ImportCsv = new Button();
        AdvancedEntry_Import_DataGridView = new DataGridView();
        AdvancedEntry_Import_Button_OpenCsv = new Button();
        AdvancedEntry_TabControl.SuspendLayout();
        AdvancedEntry_TabControl_Single.SuspendLayout();
        AdvancedEntry_Single_GroupBox_Main.SuspendLayout();
        AdvancedEntry_TabControl_MultiLoc.SuspendLayout();
        AdvancedEntry_MultiLoc_GroupBox_Item.SuspendLayout();
        AdvancedEntry_MultiLoc_GroupBox_Preview.SuspendLayout();
        AdvancedEntry_TabControl_Import.SuspendLayout();
        AdvancedEntry_Import_Panel_Buttons.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)AdvancedEntry_Import_DataGridView).BeginInit();
        SuspendLayout();
        // 
        // AdvancedEntry_TabControl
        // 
        AdvancedEntry_TabControl.Controls.Add(AdvancedEntry_TabControl_Single);
        AdvancedEntry_TabControl.Controls.Add(AdvancedEntry_TabControl_MultiLoc);
        AdvancedEntry_TabControl.Controls.Add(AdvancedEntry_TabControl_Import);
        AdvancedEntry_TabControl.Dock = DockStyle.Fill;
        AdvancedEntry_TabControl.Location = new Point(0, 0);
        AdvancedEntry_TabControl.Name = "AdvancedEntry_TabControl";
        AdvancedEntry_TabControl.SelectedIndex = 0;
        AdvancedEntry_TabControl.Size = new Size(835, 309);
        AdvancedEntry_TabControl.TabIndex = 0;
        // 
        // AdvancedEntry_TabControl_Single
        // 
        AdvancedEntry_TabControl_Single.Controls.Add(AdvancedEntry_Single_GroupBox_Main);
        AdvancedEntry_TabControl_Single.Location = new Point(4, 24);
        AdvancedEntry_TabControl_Single.Name = "AdvancedEntry_TabControl_Single";
        AdvancedEntry_TabControl_Single.Size = new Size(827, 281);
        AdvancedEntry_TabControl_Single.TabIndex = 0;
        AdvancedEntry_TabControl_Single.Text = "Single Item, Multiple Times";
        // 
        // AdvancedEntry_Single_GroupBox_Main
        // 
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_Label_Part);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_ComboBox_Part);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_Label_Op);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_ComboBox_Op);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_Label_Loc);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_ComboBox_Loc);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_Label_Qty);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_TextBox_Qty);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_Label_Count);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_TextBox_Count);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_Label_Notes);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_RichTextBox_Notes);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_Button_Save);
        AdvancedEntry_Single_GroupBox_Main.Controls.Add(AdvancedEntry_Single_Button_Reset);
        AdvancedEntry_Single_GroupBox_Main.Dock = DockStyle.Fill;
        AdvancedEntry_Single_GroupBox_Main.Location = new Point(0, 0);
        AdvancedEntry_Single_GroupBox_Main.Name = "AdvancedEntry_Single_GroupBox_Main";
        AdvancedEntry_Single_GroupBox_Main.Size = new Size(827, 281);
        AdvancedEntry_Single_GroupBox_Main.TabIndex = 0;
        AdvancedEntry_Single_GroupBox_Main.TabStop = false;
        AdvancedEntry_Single_GroupBox_Main.Text = "Inventory Single Item to One Location Multiple Times";
        // 
        // AdvancedEntry_Single_Label_Part
        // 
        AdvancedEntry_Single_Label_Part.Location = new Point(6, 21);
        AdvancedEntry_Single_Label_Part.Name = "AdvancedEntry_Single_Label_Part";
        AdvancedEntry_Single_Label_Part.Size = new Size(100, 24);
        AdvancedEntry_Single_Label_Part.TabIndex = 0;
        AdvancedEntry_Single_Label_Part.Text = "Part:";
        AdvancedEntry_Single_Label_Part.TextAlign = ContentAlignment.MiddleRight;
        // 
        // AdvancedEntry_Single_ComboBox_Part
        // 
        AdvancedEntry_Single_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        AdvancedEntry_Single_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
        AdvancedEntry_Single_ComboBox_Part.Location = new Point(112, 22);
        AdvancedEntry_Single_ComboBox_Part.Name = "AdvancedEntry_Single_ComboBox_Part";
        AdvancedEntry_Single_ComboBox_Part.Size = new Size(284, 23);
        AdvancedEntry_Single_ComboBox_Part.TabIndex = 1;
        // 
        // AdvancedEntry_Single_Label_Op
        // 
        AdvancedEntry_Single_Label_Op.Location = new Point(8, 50);
        AdvancedEntry_Single_Label_Op.Name = "AdvancedEntry_Single_Label_Op";
        AdvancedEntry_Single_Label_Op.Size = new Size(100, 24);
        AdvancedEntry_Single_Label_Op.TabIndex = 2;
        AdvancedEntry_Single_Label_Op.Text = "Op:";
        AdvancedEntry_Single_Label_Op.TextAlign = ContentAlignment.MiddleRight;
        // 
        // AdvancedEntry_Single_ComboBox_Op
        // 
        AdvancedEntry_Single_ComboBox_Op.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        AdvancedEntry_Single_ComboBox_Op.AutoCompleteSource = AutoCompleteSource.ListItems;
        AdvancedEntry_Single_ComboBox_Op.Location = new Point(112, 51);
        AdvancedEntry_Single_ComboBox_Op.Name = "AdvancedEntry_Single_ComboBox_Op";
        AdvancedEntry_Single_ComboBox_Op.Size = new Size(284, 23);
        AdvancedEntry_Single_ComboBox_Op.TabIndex = 2;
        // 
        // AdvancedEntry_Single_Label_Loc
        // 
        AdvancedEntry_Single_Label_Loc.Location = new Point(6, 79);
        AdvancedEntry_Single_Label_Loc.Name = "AdvancedEntry_Single_Label_Loc";
        AdvancedEntry_Single_Label_Loc.Size = new Size(100, 24);
        AdvancedEntry_Single_Label_Loc.TabIndex = 4;
        AdvancedEntry_Single_Label_Loc.Text = "Location:";
        AdvancedEntry_Single_Label_Loc.TextAlign = ContentAlignment.MiddleRight;
        // 
        // AdvancedEntry_Single_ComboBox_Loc
        // 
        AdvancedEntry_Single_ComboBox_Loc.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        AdvancedEntry_Single_ComboBox_Loc.AutoCompleteSource = AutoCompleteSource.ListItems;
        AdvancedEntry_Single_ComboBox_Loc.Location = new Point(112, 80);
        AdvancedEntry_Single_ComboBox_Loc.Name = "AdvancedEntry_Single_ComboBox_Loc";
        AdvancedEntry_Single_ComboBox_Loc.Size = new Size(284, 23);
        AdvancedEntry_Single_ComboBox_Loc.TabIndex = 3;
        // 
        // AdvancedEntry_Single_Label_Qty
        // 
        AdvancedEntry_Single_Label_Qty.Location = new Point(6, 108);
        AdvancedEntry_Single_Label_Qty.Name = "AdvancedEntry_Single_Label_Qty";
        AdvancedEntry_Single_Label_Qty.Size = new Size(100, 24);
        AdvancedEntry_Single_Label_Qty.TabIndex = 6;
        AdvancedEntry_Single_Label_Qty.Text = "Quantity:";
        AdvancedEntry_Single_Label_Qty.TextAlign = ContentAlignment.MiddleRight;
        // 
        // AdvancedEntry_Single_TextBox_Qty
        // 
        AdvancedEntry_Single_TextBox_Qty.Location = new Point(112, 109);
        AdvancedEntry_Single_TextBox_Qty.Name = "AdvancedEntry_Single_TextBox_Qty";
        AdvancedEntry_Single_TextBox_Qty.Size = new Size(284, 23);
        AdvancedEntry_Single_TextBox_Qty.TabIndex = 4;
        // 
        // AdvancedEntry_Single_Label_Count
        // 
        AdvancedEntry_Single_Label_Count.Location = new Point(8, 202);
        AdvancedEntry_Single_Label_Count.Name = "AdvancedEntry_Single_Label_Count";
        AdvancedEntry_Single_Label_Count.Size = new Size(100, 24);
        AdvancedEntry_Single_Label_Count.TabIndex = 8;
        AdvancedEntry_Single_Label_Count.Text = "Transactions:";
        AdvancedEntry_Single_Label_Count.TextAlign = ContentAlignment.MiddleRight;
        // 
        // AdvancedEntry_Single_TextBox_Count
        // 
        AdvancedEntry_Single_TextBox_Count.Location = new Point(112, 203);
        AdvancedEntry_Single_TextBox_Count.Name = "AdvancedEntry_Single_TextBox_Count";
        AdvancedEntry_Single_TextBox_Count.Size = new Size(284, 23);
        AdvancedEntry_Single_TextBox_Count.TabIndex = 6;
        // 
        // AdvancedEntry_Single_Label_Notes
        // 
        AdvancedEntry_Single_Label_Notes.Location = new Point(8, 138);
        AdvancedEntry_Single_Label_Notes.Name = "AdvancedEntry_Single_Label_Notes";
        AdvancedEntry_Single_Label_Notes.Size = new Size(100, 24);
        AdvancedEntry_Single_Label_Notes.TabIndex = 10;
        AdvancedEntry_Single_Label_Notes.Text = "Notes:";
        AdvancedEntry_Single_Label_Notes.TextAlign = ContentAlignment.TopRight;
        // 
        // AdvancedEntry_Single_RichTextBox_Notes
        // 
        AdvancedEntry_Single_RichTextBox_Notes.Location = new Point(112, 138);
        AdvancedEntry_Single_RichTextBox_Notes.Name = "AdvancedEntry_Single_RichTextBox_Notes";
        AdvancedEntry_Single_RichTextBox_Notes.Size = new Size(284, 60);
        AdvancedEntry_Single_RichTextBox_Notes.TabIndex = 5;
        AdvancedEntry_Single_RichTextBox_Notes.Text = "";
        // 
        // AdvancedEntry_Single_Button_Save
        // 
        AdvancedEntry_Single_Button_Save.Location = new Point(112, 232);
        AdvancedEntry_Single_Button_Save.Name = "AdvancedEntry_Single_Button_Save";
        AdvancedEntry_Single_Button_Save.Size = new Size(80, 30);
        AdvancedEntry_Single_Button_Save.TabIndex = 7;
        AdvancedEntry_Single_Button_Save.Text = "Save";
        // 
        // AdvancedEntry_Single_Button_Reset
        // 
        AdvancedEntry_Single_Button_Reset.Location = new Point(316, 232);
        AdvancedEntry_Single_Button_Reset.Name = "AdvancedEntry_Single_Button_Reset";
        AdvancedEntry_Single_Button_Reset.Size = new Size(80, 30);
        AdvancedEntry_Single_Button_Reset.TabIndex = 13;
        AdvancedEntry_Single_Button_Reset.TabStop = false;
        AdvancedEntry_Single_Button_Reset.Text = "Reset";
        // 
        // AdvancedEntry_TabControl_MultiLoc
        // 
        AdvancedEntry_TabControl_MultiLoc.Controls.Add(AdvancedEntry_MultiLoc_GroupBox_Item);
        AdvancedEntry_TabControl_MultiLoc.Controls.Add(AdvancedEntry_MultiLoc_GroupBox_Preview);
        AdvancedEntry_TabControl_MultiLoc.Location = new Point(4, 24);
        AdvancedEntry_TabControl_MultiLoc.Name = "AdvancedEntry_TabControl_MultiLoc";
        AdvancedEntry_TabControl_MultiLoc.Size = new Size(827, 281);
        AdvancedEntry_TabControl_MultiLoc.TabIndex = 1;
        AdvancedEntry_TabControl_MultiLoc.Text = "Same Item, Multiple Locations";
        // 
        // AdvancedEntry_MultiLoc_GroupBox_Item
        // 
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_Label_Part);
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_ComboBox_Part);
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_Label_Op);
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_ComboBox_Op);
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_Label_Qty);
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_TextBox_Qty);
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_Label_Notes);
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_RichTextBox_Notes);
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_Label_Loc);
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_ComboBox_Loc);
        AdvancedEntry_MultiLoc_GroupBox_Item.Controls.Add(AdvancedEntry_MultiLoc_Button_AddLoc);
        AdvancedEntry_MultiLoc_GroupBox_Item.Location = new Point(0, 0);
        AdvancedEntry_MultiLoc_GroupBox_Item.Name = "AdvancedEntry_MultiLoc_GroupBox_Item";
        AdvancedEntry_MultiLoc_GroupBox_Item.Size = new Size(412, 282);
        AdvancedEntry_MultiLoc_GroupBox_Item.TabIndex = 0;
        AdvancedEntry_MultiLoc_GroupBox_Item.TabStop = false;
        AdvancedEntry_MultiLoc_GroupBox_Item.Text = "Item Entry";
        // 
        // AdvancedEntry_MultiLoc_Label_Part
        // 
        AdvancedEntry_MultiLoc_Label_Part.Location = new Point(6, 21);
        AdvancedEntry_MultiLoc_Label_Part.Name = "AdvancedEntry_MultiLoc_Label_Part";
        AdvancedEntry_MultiLoc_Label_Part.Size = new Size(100, 24);
        AdvancedEntry_MultiLoc_Label_Part.TabIndex = 0;
        AdvancedEntry_MultiLoc_Label_Part.Text = "Part:";
        AdvancedEntry_MultiLoc_Label_Part.TextAlign = ContentAlignment.MiddleRight;
        // 
        // AdvancedEntry_MultiLoc_ComboBox_Part
        // 
        AdvancedEntry_MultiLoc_ComboBox_Part.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        AdvancedEntry_MultiLoc_ComboBox_Part.AutoCompleteSource = AutoCompleteSource.ListItems;
        AdvancedEntry_MultiLoc_ComboBox_Part.Location = new Point(112, 22);
        AdvancedEntry_MultiLoc_ComboBox_Part.Name = "AdvancedEntry_MultiLoc_ComboBox_Part";
        AdvancedEntry_MultiLoc_ComboBox_Part.Size = new Size(284, 23);
        AdvancedEntry_MultiLoc_ComboBox_Part.TabIndex = 1;
        // 
        // AdvancedEntry_MultiLoc_Label_Op
        // 
        AdvancedEntry_MultiLoc_Label_Op.Location = new Point(6, 50);
        AdvancedEntry_MultiLoc_Label_Op.Name = "AdvancedEntry_MultiLoc_Label_Op";
        AdvancedEntry_MultiLoc_Label_Op.Size = new Size(100, 24);
        AdvancedEntry_MultiLoc_Label_Op.TabIndex = 2;
        AdvancedEntry_MultiLoc_Label_Op.Text = "Op:";
        AdvancedEntry_MultiLoc_Label_Op.TextAlign = ContentAlignment.MiddleRight;
        // 
        // AdvancedEntry_MultiLoc_ComboBox_Op
        // 
        AdvancedEntry_MultiLoc_ComboBox_Op.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        AdvancedEntry_MultiLoc_ComboBox_Op.AutoCompleteSource = AutoCompleteSource.ListItems;
        AdvancedEntry_MultiLoc_ComboBox_Op.Location = new Point(112, 51);
        AdvancedEntry_MultiLoc_ComboBox_Op.Name = "AdvancedEntry_MultiLoc_ComboBox_Op";
        AdvancedEntry_MultiLoc_ComboBox_Op.Size = new Size(284, 23);
        AdvancedEntry_MultiLoc_ComboBox_Op.TabIndex = 2;
        // 
        // AdvancedEntry_MultiLoc_Label_Qty
        // 
        AdvancedEntry_MultiLoc_Label_Qty.Location = new Point(6, 79);
        AdvancedEntry_MultiLoc_Label_Qty.Name = "AdvancedEntry_MultiLoc_Label_Qty";
        AdvancedEntry_MultiLoc_Label_Qty.Size = new Size(100, 24);
        AdvancedEntry_MultiLoc_Label_Qty.TabIndex = 4;
        AdvancedEntry_MultiLoc_Label_Qty.Text = "Quantity:";
        AdvancedEntry_MultiLoc_Label_Qty.TextAlign = ContentAlignment.MiddleRight;
        // 
        // AdvancedEntry_MultiLoc_TextBox_Qty
        // 
        AdvancedEntry_MultiLoc_TextBox_Qty.Location = new Point(112, 80);
        AdvancedEntry_MultiLoc_TextBox_Qty.Name = "AdvancedEntry_MultiLoc_TextBox_Qty";
        AdvancedEntry_MultiLoc_TextBox_Qty.Size = new Size(284, 23);
        AdvancedEntry_MultiLoc_TextBox_Qty.TabIndex = 3;
        // 
        // AdvancedEntry_MultiLoc_Label_Notes
        // 
        AdvancedEntry_MultiLoc_Label_Notes.Location = new Point(6, 138);
        AdvancedEntry_MultiLoc_Label_Notes.Name = "AdvancedEntry_MultiLoc_Label_Notes";
        AdvancedEntry_MultiLoc_Label_Notes.Size = new Size(100, 24);
        AdvancedEntry_MultiLoc_Label_Notes.TabIndex = 6;
        AdvancedEntry_MultiLoc_Label_Notes.Text = "Notes:";
        AdvancedEntry_MultiLoc_Label_Notes.TextAlign = ContentAlignment.MiddleRight;
        // 
        // AdvancedEntry_MultiLoc_RichTextBox_Notes
        // 
        AdvancedEntry_MultiLoc_RichTextBox_Notes.Location = new Point(112, 138);
        AdvancedEntry_MultiLoc_RichTextBox_Notes.Name = "AdvancedEntry_MultiLoc_RichTextBox_Notes";
        AdvancedEntry_MultiLoc_RichTextBox_Notes.Size = new Size(284, 60);
        AdvancedEntry_MultiLoc_RichTextBox_Notes.TabIndex = 4;
        AdvancedEntry_MultiLoc_RichTextBox_Notes.Text = "";
        // 
        // AdvancedEntry_MultiLoc_Label_Loc
        // 
        AdvancedEntry_MultiLoc_Label_Loc.Location = new Point(6, 108);
        AdvancedEntry_MultiLoc_Label_Loc.Name = "AdvancedEntry_MultiLoc_Label_Loc";
        AdvancedEntry_MultiLoc_Label_Loc.Size = new Size(100, 24);
        AdvancedEntry_MultiLoc_Label_Loc.TabIndex = 8;
        AdvancedEntry_MultiLoc_Label_Loc.Text = "Location:";
        AdvancedEntry_MultiLoc_Label_Loc.TextAlign = ContentAlignment.MiddleRight;
        // 
        // AdvancedEntry_MultiLoc_ComboBox_Loc
        // 
        AdvancedEntry_MultiLoc_ComboBox_Loc.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        AdvancedEntry_MultiLoc_ComboBox_Loc.AutoCompleteSource = AutoCompleteSource.ListItems;
        AdvancedEntry_MultiLoc_ComboBox_Loc.Location = new Point(112, 109);
        AdvancedEntry_MultiLoc_ComboBox_Loc.Name = "AdvancedEntry_MultiLoc_ComboBox_Loc";
        AdvancedEntry_MultiLoc_ComboBox_Loc.Size = new Size(284, 23);
        AdvancedEntry_MultiLoc_ComboBox_Loc.TabIndex = 9;
        // 
        // AdvancedEntry_MultiLoc_Button_AddLoc
        // 
        AdvancedEntry_MultiLoc_Button_AddLoc.Location = new Point(112, 204);
        AdvancedEntry_MultiLoc_Button_AddLoc.Name = "AdvancedEntry_MultiLoc_Button_AddLoc";
        AdvancedEntry_MultiLoc_Button_AddLoc.Size = new Size(284, 30);
        AdvancedEntry_MultiLoc_Button_AddLoc.TabIndex = 10;
        AdvancedEntry_MultiLoc_Button_AddLoc.Text = "Add Location";
        // 
        // AdvancedEntry_MultiLoc_GroupBox_Preview
        // 
        AdvancedEntry_MultiLoc_GroupBox_Preview.Controls.Add(AdvancedEntry_MultiLoc_ListView_Preview);
        AdvancedEntry_MultiLoc_GroupBox_Preview.Controls.Add(AdvancedEntry_MultiLoc_Button_SaveAll);
        AdvancedEntry_MultiLoc_GroupBox_Preview.Controls.Add(AdvancedEntry_MultiLoc_Button_Reset);
        AdvancedEntry_MultiLoc_GroupBox_Preview.Location = new Point(418, 0);
        AdvancedEntry_MultiLoc_GroupBox_Preview.Name = "AdvancedEntry_MultiLoc_GroupBox_Preview";
        AdvancedEntry_MultiLoc_GroupBox_Preview.Size = new Size(409, 282);
        AdvancedEntry_MultiLoc_GroupBox_Preview.TabIndex = 0;
        AdvancedEntry_MultiLoc_GroupBox_Preview.TabStop = false;
        AdvancedEntry_MultiLoc_GroupBox_Preview.Text = "Transaction Preview";
        // 
        // AdvancedEntry_MultiLoc_ListView_Preview
        // 
        AdvancedEntry_MultiLoc_ListView_Preview.FullRowSelect = true;
        AdvancedEntry_MultiLoc_ListView_Preview.GridLines = true;
        AdvancedEntry_MultiLoc_ListView_Preview.Location = new Point(10, 24);
        AdvancedEntry_MultiLoc_ListView_Preview.Name = "AdvancedEntry_MultiLoc_ListView_Preview";
        AdvancedEntry_MultiLoc_ListView_Preview.Size = new Size(390, 180);
        AdvancedEntry_MultiLoc_ListView_Preview.TabIndex = 0;
        AdvancedEntry_MultiLoc_ListView_Preview.UseCompatibleStateImageBehavior = false;
        AdvancedEntry_MultiLoc_ListView_Preview.View = View.Details;
        // 
        // AdvancedEntry_MultiLoc_Button_SaveAll
        // 
        AdvancedEntry_MultiLoc_Button_SaveAll.Location = new Point(10, 220);
        AdvancedEntry_MultiLoc_Button_SaveAll.Name = "AdvancedEntry_MultiLoc_Button_SaveAll";
        AdvancedEntry_MultiLoc_Button_SaveAll.Size = new Size(100, 30);
        AdvancedEntry_MultiLoc_Button_SaveAll.TabIndex = 1;
        AdvancedEntry_MultiLoc_Button_SaveAll.Text = "Save All";
        // 
        // AdvancedEntry_MultiLoc_Button_Reset
        // 
        AdvancedEntry_MultiLoc_Button_Reset.Location = new Point(120, 220);
        AdvancedEntry_MultiLoc_Button_Reset.Name = "AdvancedEntry_MultiLoc_Button_Reset";
        AdvancedEntry_MultiLoc_Button_Reset.Size = new Size(100, 30);
        AdvancedEntry_MultiLoc_Button_Reset.TabIndex = 2;
        AdvancedEntry_MultiLoc_Button_Reset.Text = "Reset";
        // 
        // AdvancedEntry_TabControl_Import
        // 
        AdvancedEntry_TabControl_Import.Controls.Add(AdvancedEntry_Import_Button_OpenExcel);
        AdvancedEntry_TabControl_Import.Controls.Add(AdvancedEntry_Import_Panel_Buttons);
        AdvancedEntry_TabControl_Import.Controls.Add(AdvancedEntry_Import_Button_ImportExcel);
        AdvancedEntry_TabControl_Import.Controls.Add(AdvancedEntry_Import_Button_ImportCsv);
        AdvancedEntry_TabControl_Import.Controls.Add(AdvancedEntry_Import_DataGridView);
        AdvancedEntry_TabControl_Import.Controls.Add(AdvancedEntry_Import_Button_OpenCsv);
        AdvancedEntry_TabControl_Import.Location = new Point(4, 24);
        AdvancedEntry_TabControl_Import.Name = "AdvancedEntry_TabControl_Import";
        AdvancedEntry_TabControl_Import.Size = new Size(827, 281);
        AdvancedEntry_TabControl_Import.TabIndex = 2;
        AdvancedEntry_TabControl_Import.Text = "Import";
        // 
        // AdvancedEntry_Import_Button_OpenExcel
        // 
        AdvancedEntry_Import_Button_OpenExcel.Location = new Point(8, 3);
        AdvancedEntry_Import_Button_OpenExcel.Name = "AdvancedEntry_Import_Button_OpenExcel";
        AdvancedEntry_Import_Button_OpenExcel.Size = new Size(90, 30);
        AdvancedEntry_Import_Button_OpenExcel.TabIndex = 0;
        AdvancedEntry_Import_Button_OpenExcel.Text = "Open Excel";
        // 
        // AdvancedEntry_Import_Panel_Buttons
        // 
        AdvancedEntry_Import_Panel_Buttons.Controls.Add(AdvancedEntry_Import_Button_Save);
        AdvancedEntry_Import_Panel_Buttons.Controls.Add(AdvancedEntry_Import_Button_Close);
        AdvancedEntry_Import_Panel_Buttons.Dock = DockStyle.Bottom;
        AdvancedEntry_Import_Panel_Buttons.Location = new Point(0, 241);
        AdvancedEntry_Import_Panel_Buttons.Name = "AdvancedEntry_Import_Panel_Buttons";
        AdvancedEntry_Import_Panel_Buttons.Size = new Size(827, 40);
        AdvancedEntry_Import_Panel_Buttons.TabIndex = 1;
        // 
        // AdvancedEntry_Import_Button_Save
        // 
        AdvancedEntry_Import_Button_Save.Location = new Point(8, 3);
        AdvancedEntry_Import_Button_Save.Name = "AdvancedEntry_Import_Button_Save";
        AdvancedEntry_Import_Button_Save.Size = new Size(110, 30);
        AdvancedEntry_Import_Button_Save.TabIndex = 4;
        AdvancedEntry_Import_Button_Save.Text = "Save Imported";
        // 
        // AdvancedEntry_Import_Button_Close
        // 
        AdvancedEntry_Import_Button_Close.Location = new Point(739, 3);
        AdvancedEntry_Import_Button_Close.Name = "AdvancedEntry_Import_Button_Close";
        AdvancedEntry_Import_Button_Close.Size = new Size(80, 30);
        AdvancedEntry_Import_Button_Close.TabIndex = 5;
        AdvancedEntry_Import_Button_Close.Text = "Close";
        // 
        // AdvancedEntry_Import_Button_ImportExcel
        // 
        AdvancedEntry_Import_Button_ImportExcel.Location = new Point(104, 3);
        AdvancedEntry_Import_Button_ImportExcel.Name = "AdvancedEntry_Import_Button_ImportExcel";
        AdvancedEntry_Import_Button_ImportExcel.Size = new Size(90, 30);
        AdvancedEntry_Import_Button_ImportExcel.TabIndex = 1;
        AdvancedEntry_Import_Button_ImportExcel.Text = "Import Excel";
        // 
        // AdvancedEntry_Import_Button_ImportCsv
        // 
        AdvancedEntry_Import_Button_ImportCsv.Location = new Point(729, 3);
        AdvancedEntry_Import_Button_ImportCsv.Name = "AdvancedEntry_Import_Button_ImportCsv";
        AdvancedEntry_Import_Button_ImportCsv.Size = new Size(90, 30);
        AdvancedEntry_Import_Button_ImportCsv.TabIndex = 3;
        AdvancedEntry_Import_Button_ImportCsv.Text = "Import CSV";
        // 
        // AdvancedEntry_Import_DataGridView
        // 
        AdvancedEntry_Import_DataGridView.Location = new Point(8, 39);
        AdvancedEntry_Import_DataGridView.Name = "AdvancedEntry_Import_DataGridView";
        AdvancedEntry_Import_DataGridView.Size = new Size(811, 344);
        AdvancedEntry_Import_DataGridView.TabIndex = 0;
        // 
        // AdvancedEntry_Import_Button_OpenCsv
        // 
        AdvancedEntry_Import_Button_OpenCsv.Location = new Point(633, 3);
        AdvancedEntry_Import_Button_OpenCsv.Name = "AdvancedEntry_Import_Button_OpenCsv";
        AdvancedEntry_Import_Button_OpenCsv.Size = new Size(90, 30);
        AdvancedEntry_Import_Button_OpenCsv.TabIndex = 2;
        AdvancedEntry_Import_Button_OpenCsv.Text = "Open CSV";
        // 
        // AdvancedInventoryEntryForm
        // 
        CancelButton = AdvancedEntry_Import_Button_Close;
        ClientSize = new Size(835, 309);
        Controls.Add(AdvancedEntry_TabControl);
        HelpButton = true;
        MaximizeBox = false;
        MaximumSize = new Size(851, 496);
        MinimumSize = new Size(431, 348);
        Name = "AdvancedInventoryEntryForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Advanced Inventory Entry";
        AdvancedEntry_TabControl.ResumeLayout(false);
        AdvancedEntry_TabControl_Single.ResumeLayout(false);
        AdvancedEntry_Single_GroupBox_Main.ResumeLayout(false);
        AdvancedEntry_Single_GroupBox_Main.PerformLayout();
        AdvancedEntry_TabControl_MultiLoc.ResumeLayout(false);
        AdvancedEntry_MultiLoc_GroupBox_Item.ResumeLayout(false);
        AdvancedEntry_MultiLoc_GroupBox_Item.PerformLayout();
        AdvancedEntry_MultiLoc_GroupBox_Preview.ResumeLayout(false);
        AdvancedEntry_TabControl_Import.ResumeLayout(false);
        AdvancedEntry_Import_Panel_Buttons.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)AdvancedEntry_Import_DataGridView).EndInit();
        ResumeLayout(false);
    }
}