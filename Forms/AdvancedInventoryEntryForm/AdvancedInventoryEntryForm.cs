using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Forms.MainForm.Classes;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Forms.AdvancedInventoryEntryForm;

public partial class AdvancedInventoryEntryForm : Form
{
    private string appFolder;
    private string excelPath;
    private string csvPath;

    public AdvancedInventoryEntryForm()
    {
        AppLogger.Log("AdvancedInventoryEntryForm constructor entered.");
        InitializeComponent();
        WireUpEvents();

        // Ensure ListView is set up for preview
        AdvancedEntry_MultiLoc_ListView_Preview.View = View.Details;
        if (AdvancedEntry_MultiLoc_ListView_Preview.Columns.Count == 0)
        {
            AdvancedEntry_MultiLoc_ListView_Preview.Columns.Add("Location", 150);
            AdvancedEntry_MultiLoc_ListView_Preview.Columns.Add("Quantity", 80);
            AdvancedEntry_MultiLoc_ListView_Preview.Columns.Add("Notes", 200);
        }

        // Fix: Use the correct TabControl and TabPage names as defined in the Designer
        if (AdvancedEntry_TabControl == null)
        {
            AppLogger.LogApplicationError(
                new InvalidOperationException("TabControl 'AdvancedEntry_TabControl' not found."));
            throw new InvalidOperationException("TabControl 'AdvancedEntry_TabControl' not found.");
        }

        if (AdvancedEntry_TabControl_Import == null)
        {
            AppLogger.LogApplicationError(
                new InvalidOperationException("Tab 'AdvancedEntry_TabControl_Import' not found."));
            throw new InvalidOperationException("Tab 'AdvancedEntry_TabControl_Import' not found.");
        }

        ValidateQtyTextBox(AdvancedEntry_Single_TextBox_Qty, "[ Enter Valid Quantity ]");
        ValidateQtyTextBox(AdvancedEntry_Single_TextBox_Count, "[ How Many Transactions ]");

        // Disable manual resizing
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = true; // Optional: keep minimize enabled
        SizeGripStyle = SizeGripStyle.Hide;

        AppLogger.Log("AdvancedInventoryEntryForm constructor exited.");
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        try
        {
            ClientSize = new Size(431, 309);
            await LoadAllComboBoxesAsync();

            // Set focus to Part ComboBox on Tab 1 by default
            AdvancedEntry_Single_ComboBox_Part.Focus();
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "AdvancedInventoryEntryForm.OnLoad");
        }
    }

    private async Task LoadAllComboBoxesAsync()
    {
        await using var connection = new MySqlConnection(WipAppVariables.ConnectionString);

        // Tab 1
        await MainFormComboBoxDataHelper.FillComboBoxAsync(
            "SELECT * FROM part_ids", connection, new MySqlDataAdapter(), new DataTable(),
            AdvancedEntry_Single_ComboBox_Part, "Item Number", "ID", "[ Enter Part ID ]");
        await MainFormComboBoxDataHelper.FillComboBoxAsync(
            "SELECT * FROM `operation_numbers`", connection, new MySqlDataAdapter(), new DataTable(),
            AdvancedEntry_Single_ComboBox_Op, "Operation", "Operation", "[ Enter Op # ]");
        await MainFormComboBoxDataHelper.FillComboBoxAsync(
            "SELECT * FROM `locations`", connection, new MySqlDataAdapter(), new DataTable(),
            AdvancedEntry_Single_ComboBox_Loc, "Location", "Location", "[ Enter Location ]");

        // Tab 2
        await MainFormComboBoxDataHelper.FillComboBoxAsync(
            "SELECT * FROM part_ids", connection, new MySqlDataAdapter(), new DataTable(),
            AdvancedEntry_MultiLoc_ComboBox_Part, "Item Number", "ID", "[ Enter Part ID ]");
        await MainFormComboBoxDataHelper.FillComboBoxAsync(
            "SELECT * FROM `operation_numbers`", connection, new MySqlDataAdapter(), new DataTable(),
            AdvancedEntry_MultiLoc_ComboBox_Op, "Operation", "Operation", "[ Enter Op # ]");
        await MainFormComboBoxDataHelper.FillComboBoxAsync(
            "SELECT * FROM `locations`", connection, new MySqlDataAdapter(), new DataTable(),
            AdvancedEntry_MultiLoc_ComboBox_Loc, "Location", "Location", "[ Enter Location ]");
    }

    private void WireUpEvents()
    {
        // Tab 1
        AdvancedEntry_Single_Button_Save.Click += AdvancedEntry_Single_Button_Save_Click;
        AdvancedEntry_Single_Button_Reset.Click += AdvancedEntry_Single_Button_Reset_Click;
        AdvancedEntry_Single_ComboBox_Part.SelectedIndexChanged +=
            (s, e) => SetComboBoxColor(AdvancedEntry_Single_ComboBox_Part);
        AdvancedEntry_Single_ComboBox_Op.SelectedIndexChanged +=
            (s, e) => SetComboBoxColor(AdvancedEntry_Single_ComboBox_Op);
        AdvancedEntry_Single_ComboBox_Loc.SelectedIndexChanged +=
            (s, e) => SetComboBoxColor(AdvancedEntry_Single_ComboBox_Loc);
        AdvancedEntry_Single_TextBox_Qty.Text = @"[ Enter Valid Quantity ]";
        AdvancedEntry_Single_TextBox_Qty.TextChanged += (s, e) =>
            ValidateQtyTextBox(AdvancedEntry_Single_TextBox_Qty, "[ Enter Valid Quantity ]");
        AdvancedEntry_Single_TextBox_Count.Text = @"[ How Many Transactions ]";
        AdvancedEntry_Single_TextBox_Count.TextChanged += (s, e) =>
            ValidateQtyTextBox(AdvancedEntry_Single_TextBox_Count, "[ How Many Transactions ]");

        // Tab 2
        AdvancedEntry_MultiLoc_Button_AddLoc.Click += AdvancedEntry_MultiLoc_Button_AddLoc_Click;
        AdvancedEntry_MultiLoc_Button_SaveAll.Click += AdvancedEntry_MultiLoc_Button_SaveAll_Click;
        AdvancedEntry_MultiLoc_Button_Reset.Click += AdvancedEntry_MultiLoc_Button_Reset_Click;
        AdvancedEntry_MultiLoc_ComboBox_Part.SelectedIndexChanged +=
            (s, e) => SetComboBoxColor(AdvancedEntry_MultiLoc_ComboBox_Part);
        AdvancedEntry_MultiLoc_ComboBox_Op.SelectedIndexChanged +=
            (s, e) => SetComboBoxColor(AdvancedEntry_MultiLoc_ComboBox_Op);
        AdvancedEntry_MultiLoc_ComboBox_Loc.SelectedIndexChanged +=
            (s, e) => SetComboBoxColor(AdvancedEntry_MultiLoc_ComboBox_Loc);
        AdvancedEntry_MultiLoc_TextBox_Qty.TextChanged += (s, e) =>
            ValidateQtyTextBox(AdvancedEntry_MultiLoc_TextBox_Qty, "[ Enter Valid Quantity ]");

        // Tab 3
        AdvancedEntry_Import_Button_OpenExcel.Click += AdvancedEntry_Import_Button_OpenExcel_Click;
        AdvancedEntry_Import_Button_ImportExcel.Click += AdvancedEntry_Import_Button_ImportExcel_Click;
        AdvancedEntry_Import_Button_OpenCsv.Click += AdvancedEntry_Import_Button_OpenCsv_Click;
        AdvancedEntry_Import_Button_ImportCsv.Click += AdvancedEntry_Import_Button_ImportCsv_Click;
        AdvancedEntry_Import_Button_Save.Click += AdvancedEntry_Import_Button_Save_Click;
        AdvancedEntry_Import_Button_Close.Click += (s, e) => Close();

        // Add this line to handle tab switching
        AdvancedEntry_TabControl.SelectedIndexChanged += AdvancedEntry_TabControl_SelectedIndexChanged;

        // Add this line to handle SelectAll on Enter for ComboBoxes
        AdvancedEntry_Single_ComboBox_Part.Enter += (s, e) => AdvancedEntry_Single_ComboBox_Part.SelectAll();
        AdvancedEntry_Single_ComboBox_Op.Enter += (s, e) => AdvancedEntry_Single_ComboBox_Op.SelectAll();
        AdvancedEntry_Single_ComboBox_Loc.Enter += (s, e) => AdvancedEntry_Single_ComboBox_Loc.SelectAll();
        AdvancedEntry_MultiLoc_ComboBox_Part.Enter += (s, e) => AdvancedEntry_MultiLoc_ComboBox_Part.SelectAll();
        AdvancedEntry_MultiLoc_ComboBox_Op.Enter += (s, e) => AdvancedEntry_MultiLoc_ComboBox_Op.SelectAll();
        AdvancedEntry_MultiLoc_ComboBox_Loc.Enter += (s, e) => AdvancedEntry_MultiLoc_ComboBox_Loc.SelectAll();
    }

    private void SetComboBoxColor(ComboBox cb)
    {
        if (cb.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(cb.Text))
        {
            cb.ForeColor = Color.Black;
        }
        else
        {
            cb.ForeColor = Color.Red;
            if (cb.Items.Count > 0 && cb.SelectedIndex != 0)
                cb.SelectedIndex = 0;
        }
    }

    private void AdvancedEntry_TabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Set form size and focus based on selected tab
        switch (AdvancedEntry_TabControl.SelectedIndex)
        {
            case 0: // Tab 1
                ClientSize = new Size(431, 309);
                AdvancedEntry_Single_ComboBox_Part.Focus();
                break;
            case 1: // Tab 2
                ClientSize = new Size(835, 309);
                AdvancedEntry_MultiLoc_ComboBox_Part.Focus();
                break;
            case 2: // Tab 3
                ClientSize = new Size(835, 348);
                break;
        }
    }

    // --- Tab 1: Save/Reset ---
    private async void AdvancedEntry_Single_Button_Save_Click(object sender, EventArgs e)
    {
        var part = AdvancedEntry_Single_ComboBox_Part.Text;
        var op = AdvancedEntry_Single_ComboBox_Op.Text;
        var loc = AdvancedEntry_Single_ComboBox_Loc.Text;
        var qtyText = AdvancedEntry_Single_TextBox_Qty.Text.Trim();
        var countText = AdvancedEntry_Single_TextBox_Count.Text.Trim();
        var notes = AdvancedEntry_Single_RichTextBox_Notes.Text.Trim();

        if (AdvancedEntry_Single_ComboBox_Part.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(part))
        {
            MessageBox.Show("Please select a valid Part.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_Single_ComboBox_Part.Focus();
            return;
        }

        if (AdvancedEntry_Single_ComboBox_Op.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(op))
        {
            MessageBox.Show("Please select a valid Operation.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_Single_ComboBox_Op.Focus();
            return;
        }

        if (AdvancedEntry_Single_ComboBox_Loc.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(loc))
        {
            MessageBox.Show("Please select a valid Location.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_Single_ComboBox_Loc.Focus();
            return;
        }

        if (!int.TryParse(qtyText, out var qty) || qty <= 0)
        {
            MessageBox.Show("Please enter a valid quantity.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_Single_TextBox_Qty.Focus();
            return;
        }

        if (!int.TryParse(countText, out var count) || count <= 0)
        {
            MessageBox.Show("Please enter how many times to inventory.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_Single_TextBox_Count.Focus();
            return;
        }

        var saved = 0;
        for (var i = 0; i < count; i++)
        {
            WipAppVariables.PartId = part;
            WipAppVariables.Operation = op;
            WipAppVariables.Location = loc;
            WipAppVariables.Notes = notes;
            WipAppVariables.InventoryQuantity = qty;
            WipAppVariables.User ??= Environment.UserName;
            WipAppVariables.PartType ??= "";
            await InventoryDao.InventoryTab_SaveAsync(true);
            saved++;
        }

        MessageBox.Show($"{saved} entries saved successfully.", "Success", MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        AdvancedEntry_Single_Button_Reset_Click(null, null);
    }

    private void AdvancedEntry_Single_Button_Reset_Click(object sender, EventArgs e)
    {
        MainFormControlHelper.ResetComboBox(AdvancedEntry_Single_ComboBox_Part, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(AdvancedEntry_Single_ComboBox_Op, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(AdvancedEntry_Single_ComboBox_Loc, Color.Red, 0);
        MainFormControlHelper.ResetTextBox(AdvancedEntry_Single_TextBox_Qty, Color.Red, "[ Enter Valid Quantity ]");
        MainFormControlHelper.ResetTextBox(AdvancedEntry_Single_TextBox_Count, Color.Red, "[ Enter How Many Times ]");
        AdvancedEntry_Single_RichTextBox_Notes.Text = string.Empty;
        AdvancedEntry_Single_ComboBox_Part.Focus();
    }

    // --- Tab 2: Add Location, Save All, Reset ---
    private void AdvancedEntry_MultiLoc_Button_AddLoc_Click(object sender, EventArgs e)
    {
        var part = AdvancedEntry_MultiLoc_ComboBox_Part.Text;
        var op = AdvancedEntry_MultiLoc_ComboBox_Op.Text;
        var loc = AdvancedEntry_MultiLoc_ComboBox_Loc.Text;
        var qtyText = AdvancedEntry_MultiLoc_TextBox_Qty.Text.Trim();
        var notes = AdvancedEntry_MultiLoc_RichTextBox_Notes.Text.Trim();

        if (AdvancedEntry_MultiLoc_ComboBox_Part.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(part))
        {
            MessageBox.Show("Please select a valid Part.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_MultiLoc_ComboBox_Part.Focus();
            return;
        }

        if (AdvancedEntry_MultiLoc_ComboBox_Op.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(op))
        {
            MessageBox.Show("Please select a valid Operation.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_MultiLoc_ComboBox_Op.Focus();
            return;
        }

        if (AdvancedEntry_MultiLoc_ComboBox_Loc.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(loc))
        {
            MessageBox.Show("Please select a valid Location.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_MultiLoc_ComboBox_Loc.Focus();
            return;
        }

        if (!int.TryParse(qtyText, out var qty) || qty <= 0)
        {
            MessageBox.Show("Please enter a valid quantity.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_MultiLoc_TextBox_Qty.Focus();
            return;
        }

        var item = new ListViewItem(new[] { loc, qtyText, notes });
        AdvancedEntry_MultiLoc_ListView_Preview.Items.Add(item);

        // Disable Part and Op ComboBoxes after first add
        if (AdvancedEntry_MultiLoc_ListView_Preview.Items.Count == 1)
        {
            AdvancedEntry_MultiLoc_ComboBox_Part.Enabled = false;
            AdvancedEntry_MultiLoc_ComboBox_Op.Enabled = false;
        }

        // Only reset location, notes; keep quantity as is for user convenience
        MainFormControlHelper.ResetComboBox(AdvancedEntry_MultiLoc_ComboBox_Loc, Color.Red, 0);
        AdvancedEntry_MultiLoc_RichTextBox_Notes.Text = string.Empty;
        AdvancedEntry_MultiLoc_ComboBox_Loc.Focus();
    }

    private async void AdvancedEntry_MultiLoc_Button_SaveAll_Click(object sender, EventArgs e)
    {
        var part = AdvancedEntry_MultiLoc_ComboBox_Part.Text;
        var op = AdvancedEntry_MultiLoc_ComboBox_Op.Text;

        if (AdvancedEntry_MultiLoc_ComboBox_Part.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(part))
        {
            MessageBox.Show("Please select a valid Part.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_MultiLoc_ComboBox_Part.Focus();
            return;
        }

        if (AdvancedEntry_MultiLoc_ComboBox_Op.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(op))
        {
            MessageBox.Show("Please select a valid Operation.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            AdvancedEntry_MultiLoc_ComboBox_Op.Focus();
            return;
        }

        if (AdvancedEntry_MultiLoc_ListView_Preview.Items.Count == 0)
        {
            MessageBox.Show("No locations to save.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var saved = 0;
        foreach (ListViewItem item in AdvancedEntry_MultiLoc_ListView_Preview.Items)
        {
            var loc = item.SubItems[0].Text;
            var qtyText = item.SubItems[1].Text;
            var notes = item.SubItems[2].Text;

            if (!int.TryParse(qtyText, out var qty) || qty <= 0)
                continue;

            WipAppVariables.PartId = part;
            WipAppVariables.Operation = op;
            WipAppVariables.Location = loc;
            WipAppVariables.Notes = notes;
            WipAppVariables.InventoryQuantity = qty;
            WipAppVariables.User ??= Environment.UserName;
            WipAppVariables.PartType ??= "";
            await InventoryDao.InventoryTab_SaveAsync(true);
            saved++;
        }

        MessageBox.Show($"{saved} entries saved successfully.", "Success", MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        AdvancedEntry_MultiLoc_Button_Reset_Click(null, null);
    }

    private void AdvancedEntry_MultiLoc_Button_Reset_Click(object sender, EventArgs e)
    {
        MainFormControlHelper.ResetComboBox(AdvancedEntry_MultiLoc_ComboBox_Part, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(AdvancedEntry_MultiLoc_ComboBox_Op, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(AdvancedEntry_MultiLoc_ComboBox_Loc, Color.Red, 0);
        MainFormControlHelper.ResetTextBox(AdvancedEntry_MultiLoc_TextBox_Qty, Color.Red, "[ Enter Valid Quantity ]");
        AdvancedEntry_MultiLoc_RichTextBox_Notes.Text = string.Empty;
        AdvancedEntry_MultiLoc_ListView_Preview.Items.Clear();
        AdvancedEntry_MultiLoc_ComboBox_Part.Enabled = true;
        AdvancedEntry_MultiLoc_ComboBox_Op.Enabled = true;
        AdvancedEntry_MultiLoc_ComboBox_Part.Focus();
    }

    // --- Tab 3: Import ---
    private void AdvancedEntry_Import_Button_OpenExcel_Click(object sender, EventArgs e)
    {
        // Open Excel file logic (see previous conversation for details)
    }

    private void AdvancedEntry_Import_Button_ImportExcel_Click(object sender, EventArgs e)
    {
        // Import Excel logic (see previous conversation for details)
    }

    private void AdvancedEntry_Import_Button_OpenCsv_Click(object sender, EventArgs e)
    {
        // Open CSV file logic (see previous conversation for details)
    }

    private void AdvancedEntry_Import_Button_ImportCsv_Click(object sender, EventArgs e)
    {
        // Import CSV logic (see previous conversation for details)
    }

    private async void AdvancedEntry_Import_Button_Save_Click(object sender, EventArgs e)
    {
        // Save imported entries logic (see previous conversation for details)
    }

    // Validation method for quantity/count textboxes
    private void ValidateQtyTextBox(TextBox textBox, string placeholder)
    {
        var text = textBox.Text.Trim();
        var isValid = int.TryParse(text, out var value) && value > 0;
        if (isValid)
        {
            textBox.ForeColor = Color.Black;
        }
        else
        {
            textBox.ForeColor = Color.Red;
            if (text != placeholder)
            {
                textBox.Text = placeholder;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);

        // Check for unsaved data in Tab 1
        var tab1Dirty =
            AdvancedEntry_Single_ComboBox_Part.SelectedIndex > 0 ||
            AdvancedEntry_Single_ComboBox_Op.SelectedIndex > 0 ||
            AdvancedEntry_Single_ComboBox_Loc.SelectedIndex > 0 ||
            (!string.IsNullOrWhiteSpace(AdvancedEntry_Single_TextBox_Qty.Text) &&
             AdvancedEntry_Single_TextBox_Qty.Text != "[ Enter Valid Quantity ]") ||
            (!string.IsNullOrWhiteSpace(AdvancedEntry_Single_TextBox_Count.Text) &&
             AdvancedEntry_Single_TextBox_Count.Text != "[ How Many Transactions ]") ||
            !string.IsNullOrWhiteSpace(AdvancedEntry_Single_RichTextBox_Notes.Text);

        // Check for unsaved data in Tab 2
        var tab2Dirty =
            AdvancedEntry_MultiLoc_ComboBox_Part.SelectedIndex > 0 ||
            AdvancedEntry_MultiLoc_ComboBox_Op.SelectedIndex > 0 ||
            AdvancedEntry_MultiLoc_ComboBox_Loc.SelectedIndex > 0 ||
            (!string.IsNullOrWhiteSpace(AdvancedEntry_MultiLoc_TextBox_Qty.Text) &&
             AdvancedEntry_MultiLoc_TextBox_Qty.Text != "[ Enter Valid Quantity ]") ||
            !string.IsNullOrWhiteSpace(AdvancedEntry_MultiLoc_RichTextBox_Notes.Text) ||
            AdvancedEntry_MultiLoc_ListView_Preview.Items.Count > 0;

        // You can add similar checks for Tab 3 if needed

        if (tab1Dirty || tab2Dirty)
        {
            var result = MessageBox.Show(
                "You have unsaved changes. If you close this form, all changes will be lost.\n\nAre you sure you want to close?",
                "Unsaved Changes",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes) e.Cancel = true;
        }
    }
}