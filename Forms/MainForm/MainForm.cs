using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTM_WIP_Application.Forms.MainForm;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        _ = OnStartup();
    }

    private async Task OnStartup()
    {
        await LoadInventoryTabComboBoxesAsync();
        WireUpInventoryTabEvents();
        AppLogger.Log("Initial setup of ComboBoxes in the Inventory Tab.");
        MainFormTabResetHelper.ResetInventoryTab(
            MainForm_Inventory_ComboBox_Loc,
            MainForm_Inventory_ComboBox_Op,
            MainForm_Inventory_ComboBox_Part,
            new CheckBox(),
            new CheckBox(),
            null,
            MainForm_Inventory_TextBox_Qty,
            MainForm_Inventory_RichTextBox_Notes,
            MainForm_Inventory_Button_Save,
            MainForm_MenuStrip_File_Save
        );
    }

    private void WireUpInventoryTabEvents()
    {
        MainForm_Inventory_Button_Save.Click += async (s, e) => await InventoryButtonSave_ClickAsync();
        MainForm_Inventory_Button_Reset.Click += (s, e) => InventoryButtonReset_Click();
        MainForm_Inventory_ComboBox_Part.SelectedIndexChanged += (s, e) =>
        {
            InventoryComboBoxPart_SelectedIndexChanged();
            UpdateInventorySaveButtonState();
        };
        MainForm_Inventory_ComboBox_Op.SelectedIndexChanged += (s, e) =>
        {
            InventoryComboBoxOp_SelectedIndexChanged();
            UpdateInventorySaveButtonState();
        };
        MainForm_Inventory_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
        {
            InventoryComboBoxLoc_SelectedIndexChanged();
            UpdateInventorySaveButtonState();
        };
        MainForm_Inventory_TextBox_Qty.TextChanged += (s, e) =>
        {
            InventoryTextBoxQty_TextChanged();
            UpdateInventorySaveButtonState();
        };
        MainForm_Inventory_TextBox_Qty.Enter += (s, e) => MainForm_Inventory_TextBox_Qty.SelectAll();
        MainForm_Inventory_Button_ShowHideLast10.Click += (s, e) => InventoryButtonShowHideLast10_Click();
        MainForm_InventoryTab_Button_AdvancedEntry.Click += (s, e) => InventoryButtonAdvancedEntry_Click();
        MainForm_Inventory_TextBox_Qty.Enter += (s, e) => MainForm_Inventory_TextBox_Qty.SelectAll();
        MainForm_Inventory_TextBox_Qty.KeyDown += (sender, e) =>
            MainFormControlHelper.AdjustQuantityByKey(sender, e, "[ Enter Valid Quantity ]", 5, 1, 100, 1000,
                Color.Black, Color.Red);
    }

    private void UpdateInventorySaveButtonState()
    {
        var partValid = MainForm_Inventory_ComboBox_Part.SelectedIndex > 0 &&
                        !string.IsNullOrWhiteSpace(MainForm_Inventory_ComboBox_Part.Text);
        var opValid = MainForm_Inventory_ComboBox_Op.SelectedIndex > 0 &&
                      !string.IsNullOrWhiteSpace(MainForm_Inventory_ComboBox_Op.Text);
        var locValid = MainForm_Inventory_ComboBox_Loc.SelectedIndex > 0 &&
                       !string.IsNullOrWhiteSpace(MainForm_Inventory_ComboBox_Loc.Text);
        var qtyValid = int.TryParse(MainForm_Inventory_TextBox_Qty.Text.Trim(), out var qty) && qty > 0;
        MainForm_Inventory_Button_Save.Enabled = partValid && opValid && locValid && qtyValid;
    }

    private async Task LoadInventoryTabComboBoxesAsync()
    {
        await using var connection = new MySqlConnection(WipAppVariables.ConnectionString);

        // Define the combo box sets for each tab
        var comboBoxSets =
            new (MySqlDataAdapter Adapter, DataTable Table, ComboBox ComboBox, string Query, string Display, string
                Value, string Placeholder)[]
                {
                    // Inventory Tab
                    (new MySqlDataAdapter(), new DataTable(), MainForm_Inventory_ComboBox_Part,
                        "SELECT * FROM part_ids", "Item Number", "ID", "[ Enter Part ID ]"),
                    (new MySqlDataAdapter(), new DataTable(), MainForm_Inventory_ComboBox_Op,
                        "SELECT * FROM `operation_numbers`", "Operation", "Operation", "[ Enter Op # ]"),
                    (new MySqlDataAdapter(), new DataTable(), MainForm_Inventory_ComboBox_Loc,
                        "SELECT * FROM `locations`", "Location", "Location", "[ Enter Location ]")
                    // Add more sets for other tabs as needed
                };

        foreach (var (adapter, table, comboBox, query, display, value, placeholder) in comboBoxSets)
            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                query, connection, adapter, table, comboBox, display, value, placeholder
            );
    }

    private async Task InventoryButtonSave_ClickAsync()
    {
        try
        {
            AppLogger.Log("Inventory Save button clicked.");

            // Validate input
            var partId = MainForm_Inventory_ComboBox_Part.Text;
            var op = MainForm_Inventory_ComboBox_Op.Text;
            var loc = MainForm_Inventory_ComboBox_Loc.Text;
            var qtyText = MainForm_Inventory_TextBox_Qty.Text.Trim();
            var notes = MainForm_Inventory_RichTextBox_Notes.Text.Trim();

            if (string.IsNullOrWhiteSpace(partId) || MainForm_Inventory_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a valid Part.", "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                MainForm_Inventory_ComboBox_Part.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(op) || MainForm_Inventory_ComboBox_Op.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a valid Operation.", "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                MainForm_Inventory_ComboBox_Op.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(loc) || MainForm_Inventory_ComboBox_Loc.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a valid Location.", "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                MainForm_Inventory_ComboBox_Loc.Focus();
                return;
            }

            if (!int.TryParse(qtyText, out var qty) || qty <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                MainForm_Inventory_TextBox_Qty.Focus();
                return;
            }

            // Set WipAppVariables for InventoryDao
            WipAppVariables.PartId = partId;
            WipAppVariables.Operation = op;
            WipAppVariables.Location = loc;
            WipAppVariables.Notes = notes;
            WipAppVariables.InventoryQuantity = qty;
            // Set user and part type if needed
            WipAppVariables.User ??= Environment.UserName;
            WipAppVariables.PartType ??= "";

            // Save to database using InventoryDao
            await InventoryDao.InventoryTab_SaveAsync(true);

            MessageBox.Show("Inventory transaction saved successfully.", "Success", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Optionally reset controls
            InventoryButtonReset_Click();
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, nameof(InventoryButtonSave_ClickAsync),
                "MainForm_Inventory_Button_Save");
        }
    }

    private void InventoryComboBoxPart_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Part ComboBox selection changed.");

            if (MainForm_Inventory_ComboBox_Part.SelectedIndex > 0)
            {
                MainForm_Inventory_ComboBox_Part.ForeColor = Color.Black;
                WipAppVariables.PartId = MainForm_Inventory_ComboBox_Part.Text;
            }
            else
            {
                MainForm_Inventory_ComboBox_Part.ForeColor = Color.Red;
                if (MainForm_Inventory_ComboBox_Part.SelectedIndex != 0 &&
                    MainForm_Inventory_ComboBox_Part.Items.Count > 0)
                    MainForm_Inventory_ComboBox_Part.SelectedIndex = 0;
                WipAppVariables.PartId = null;
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                nameof(InventoryComboBoxPart_SelectedIndexChanged), "MainForm_Inventory_ComboBox_Part");
        }
    }

    private void InventoryComboBoxOp_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Op ComboBox selection changed.");

            if (MainForm_Inventory_ComboBox_Op.SelectedIndex > 0)
            {
                MainForm_Inventory_ComboBox_Op.ForeColor = Color.Black;
                WipAppVariables.Operation = MainForm_Inventory_ComboBox_Op.Text;
            }
            else
            {
                MainForm_Inventory_ComboBox_Op.ForeColor = Color.Red;
                if (MainForm_Inventory_ComboBox_Op.SelectedIndex != 0 && MainForm_Inventory_ComboBox_Op.Items.Count > 0)
                    MainForm_Inventory_ComboBox_Op.SelectedIndex = 0;
                WipAppVariables.Operation = null;
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                nameof(InventoryComboBoxOp_SelectedIndexChanged), "MainForm_Inventory_ComboBox_Op");
        }
    }

    private void InventoryComboBoxLoc_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Location ComboBox selection changed.");

            if (MainForm_Inventory_ComboBox_Loc.SelectedIndex > 0)
            {
                MainForm_Inventory_ComboBox_Loc.ForeColor = Color.Black;
                WipAppVariables.Location = MainForm_Inventory_ComboBox_Loc.Text;
            }
            else
            {
                MainForm_Inventory_ComboBox_Loc.ForeColor = Color.Red;
                if (MainForm_Inventory_ComboBox_Loc.SelectedIndex != 0 &&
                    MainForm_Inventory_ComboBox_Loc.Items.Count > 0) MainForm_Inventory_ComboBox_Loc.SelectedIndex = 0;
                WipAppVariables.Location = null;
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                nameof(InventoryComboBoxLoc_SelectedIndexChanged), "MainForm_Inventory_ComboBox_Loc");
        }
    }

    private void InventoryButtonReset_Click()
    {
        try
        {
            AppLogger.Log("Inventory Reset button clicked.");
            MainFormTabResetHelper.ResetInventoryTab(
                MainForm_Inventory_ComboBox_Loc,
                MainForm_Inventory_ComboBox_Op,
                MainForm_Inventory_ComboBox_Part,
                new CheckBox(),
                new CheckBox(),
                null,
                MainForm_Inventory_TextBox_Qty,
                MainForm_Inventory_RichTextBox_Notes,
                MainForm_Inventory_Button_Save,
                MainForm_MenuStrip_File_Save
            );
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, nameof(InventoryButtonReset_Click),
                "MainForm_Inventory_Button_Reset");
        }
    }

    private void InventoryTextBoxQty_TextChanged()
    {
        try
        {
            AppLogger.Log("Inventory Quantity TextBox changed.");

            var text = MainForm_Inventory_TextBox_Qty.Text.Trim();
            const string placeholder = "[ Enter Valid Quantity ]";
            var isValid = int.TryParse(text, out var qty) && qty > 0;

            if (isValid)
            {
                MainForm_Inventory_TextBox_Qty.ForeColor = Color.Black;
            }
            else
            {
                MainForm_Inventory_TextBox_Qty.ForeColor = Color.Red;
                if (text != placeholder)
                {
                    MainForm_Inventory_TextBox_Qty.Text = placeholder;
                    MainForm_Inventory_TextBox_Qty.SelectionStart = MainForm_Inventory_TextBox_Qty.Text.Length;
                }
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, nameof(InventoryTextBoxQty_TextChanged),
                "MainForm_Inventory_TextBox_Qty");
        }
    }

    private void InventoryButtonShowHideLast10_Click()
    {
        try
        {
            AppLogger.Log("Inventory Show/Hide Last 10 button clicked.");
            // TODO: Show/hide last 10 transactions
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                nameof(InventoryButtonShowHideLast10_Click), "MainForm_Inventory_Button_ShowHideLast10");
        }
    }

    private void InventoryButtonAdvancedEntry_Click()
    {
        try
        {
            AppLogger.Log("Inventory Advanced Entry button clicked.");
            // TODO: Open advanced entry dialog or logic
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, nameof(InventoryButtonAdvancedEntry_Click),
                "MainForm_InventoryTab_Button_AdvancedEntry");
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Enter)
        {
            // Move focus to the next control, like Tab
            SelectNextControl(
                ActiveControl,
                true,
                true,
                true,
                true
            );
            return true; // Mark as handled
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }
}