using System.Data;
using System.Diagnostics;
using System.Timers;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Forms.MainForm;

public partial class MainForm : Form
{
    public MainForm()
    {
        try
        {
            InitializeComponent();
            MainForm_Inventory_ComboBox_Loc.Visible = false;
            MainForm_Inventory_ComboBox_Op.Visible = false;
            MainForm_Inventory_ComboBox_Part.Visible = false;
            MainForm_Inventory_Button_Reset.TabStop = false;
            MainForm_Transfer_Button_Reset.TabStop = false;
            MainForm_Remove_Button_Reset.TabStop = false;
            _ = OnStartup();
            AppLogger.Log("MainForm initialized.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Ctor");
        }
    }

    private static void InventoryButtonAdvancedEntry_Click()
    {
        try
        {
            AppLogger.Log("Inventory Advanced Entry button clicked.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                "MainForm_InventoryTab_Button_AdvancedEntry");
        }
    }

    private void InventoryButtonReset_Click()
    {
        try
        {
            AppLogger.Log("Inventory Reset button clicked.");
            MainForm_Inventory_ComboBox_Loc.Visible = false;
            MainForm_Inventory_ComboBox_Op.Visible = false;
            MainForm_Inventory_ComboBox_Part.Visible = false;

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

            MainForm_Inventory_ComboBox_Loc.Visible = true;
            MainForm_Inventory_ComboBox_Op.Visible = true;
            MainForm_Inventory_ComboBox_Part.Visible = true;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_Button_Reset");
        }
    }

    private async Task InventoryButtonSave_ClickAsync()
    {
        try
        {
            AppLogger.Log("Inventory Save button clicked.");

            var partId = MainForm_Inventory_ComboBox_Part.Text;
            var op = MainForm_Inventory_ComboBox_Op.Text;
            var loc = MainForm_Inventory_ComboBox_Loc.Text;
            var qtyText = MainForm_Inventory_TextBox_Qty.Text.Trim();
            var notes = MainForm_Inventory_RichTextBox_Notes.Text.Trim();

            if (string.IsNullOrWhiteSpace(partId) || MainForm_Inventory_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                MainForm_Inventory_ComboBox_Part.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(op) || MainForm_Inventory_ComboBox_Op.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Operation.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                MainForm_Inventory_ComboBox_Op.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(loc) || MainForm_Inventory_ComboBox_Loc.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Location.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                MainForm_Inventory_ComboBox_Loc.Focus();
                return;
            }

            if (!int.TryParse(qtyText, out var qty) || qty <= 0)
            {
                MessageBox.Show(@"Please enter a valid quantity.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                MainForm_Inventory_TextBox_Qty.Focus();
                return;
            }

            WipAppVariables.PartId = partId;
            WipAppVariables.Operation = op;
            WipAppVariables.Location = loc;
            WipAppVariables.Notes = notes;
            WipAppVariables.InventoryQuantity = qty;
            WipAppVariables.User ??= Environment.UserName;
            WipAppVariables.PartType ??= "";

            await InventoryDao.InventoryTab_SaveAsync(true);

            MessageBox.Show(@"Inventory transaction saved successfully.", @"Success", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            MainForm_StatusStrip_SavedStatus.Text =
                $@"Last Inventoried Part: {partId} (Op: {op}), Location: {loc}, Quantity: {qty} @ {DateTime.Now:hh:mm tt}";

            InventoryButtonReset_Click();
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "MainForm_Inventory_Button_Save");
        }
    }

    private static void InventoryButtonShowHideLast10_Click()
    {
        try
        {
            AppLogger.Log("Inventory Show/Hide Last 10 button clicked.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                "MainForm_Inventory_Button_ShowHideLast10");
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
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Loc");
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
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Op");
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
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Part");
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
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_TextBox_Qty");
        }
    }

    private async Task LoadInventoryTabComboBoxesAsync()
    {
        try
        {
            await using var connection = new MySqlConnection(WipAppVariables.ConnectionString);

            var comboBoxSets =
                new (MySqlDataAdapter Adapter, DataTable Table, ComboBox ComboBox, string Query, string Display, string
                    Value, string Placeholder)[]
                    {
                        (new MySqlDataAdapter(), new DataTable(), MainForm_Inventory_ComboBox_Part,
                            "SELECT * FROM part_ids", "Item Number", "ID", "[ Enter Part ID ]"),
                        (new MySqlDataAdapter(), new DataTable(), MainForm_Inventory_ComboBox_Op,
                            "SELECT * FROM `operation_numbers`", "Operation", "Operation", "[ Enter Op # ]"),
                        (new MySqlDataAdapter(), new DataTable(), MainForm_Inventory_ComboBox_Loc,
                            "SELECT * FROM `locations`", "Location", "Location", "[ Enter Location ]")
                    };

            foreach (var (adapter, table, comboBox, query, display, value, placeholder) in comboBoxSets)
                await MainFormComboBoxDataHelper.FillComboBoxAsync(
                    query, connection, adapter, table, comboBox, display, value, placeholder
                );
            AppLogger.Log("Inventory tab ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                "MainForm_LoadInventoryTabComboBoxesAsync");
        }
    }

    private void MainForm_InventoryTab_Button_AdvancedEntry_Click(object sender, EventArgs e)
    {
        try
        {
            using (var advancedForm =
                   new AdvancedInventoryEntryForm.AdvancedInventoryEntryForm())
            {
                advancedForm.ShowDialog(this);
            }

            InventoryButtonReset_Click();
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }

    private async Task OnStartup()
    {
        try
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
            MainForm_Inventory_ComboBox_Loc.Visible = true;
            MainForm_Inventory_ComboBox_Op.Visible = true;
            MainForm_Inventory_ComboBox_Part.Visible = true;

            try
            {
                WipAppVariables.UserFullName = await UserDao.GetUserFullNameAsync(WipAppVariables.User, true);
                AppLogger.Log($"User full name loaded: {WipAppVariables.UserFullName}");
            }
            catch (Exception ex)
            {
                AppLogger.LogApplicationError(ex);
                await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "MainForm_OnStartup_GetUserFullName");
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "MainForm_OnStartup");
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        try
        {
            if (keyData == Keys.Enter)
            {
                SelectNextControl(
                    ActiveControl,
                    true,
                    true,
                    true,
                    true
                );
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_ProcessCmdKey");
            return false;
        }
    }

    private void UpdateInventorySaveButtonState()
    {
        try
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
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_UpdateInventorySaveButtonState");
        }
    }

    private void WireUpInventoryTabEvents()
    {
        try
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
            MainForm_Inventory_TextBox_Qty.Click += (s, e) => MainForm_Inventory_TextBox_Qty.SelectAll();
            MainForm_Inventory_Button_ShowHideLast10.Click += (s, e) => InventoryButtonShowHideLast10_Click();
            MainForm_InventoryTab_Button_AdvancedEntry.Click += (s, e) => InventoryButtonAdvancedEntry_Click();
            MainForm_Inventory_TextBox_Qty.Enter += (s, e) => MainForm_Inventory_TextBox_Qty.SelectAll();
            MainForm_Inventory_TextBox_Qty.KeyDown += (sender, e) =>
                MainFormControlHelper.AdjustQuantityByKey_Quantity(sender, e, "[ Enter Valid Quantity ]", Color.Black,
                    Color.Red);

            AppLogger.Log("Inventory tab events wired up.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_WireUpInventoryTabEvents");
        }
    }

    public static void VersionChecker(object? sender, ElapsedEventArgs e)
    {
        Debug.WriteLine("Running VersionChecker...");
        AppLogger.Log("Running VersionChecker...");

        using var connection = new MySqlConnection(SqlVariables.GetConnectionString(null, null, null, null));
        connection.Open();

        using var command = new MySqlCommand("SELECT * FROM `program_information`", connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var databaseVersion = reader.GetString(1);

            if (databaseVersion != WipAppVariables.UserVersion)
            {
                AppLogger.Log(
                    $"Version mismatch detected. Current: {WipAppVariables.UserVersion}, Expected: {databaseVersion}");
                Debug.WriteLine(
                    $"Version mismatch detected. Current: {WipAppVariables.UserVersion}, Expected: {databaseVersion}");

                Task.Run(() =>
                {
                    var message = "You are using an older version of the WIP Application.\n" +
                                  "This normally means a newer version is just about to be released.\n" +
                                  "The program will close in 30 seconds, or by clicking OK.";
                    var caption = $"Version Conflict Error ({WipAppVariables.UserVersion}/{databaseVersion})";
                    MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    Application.Exit();
                });

                break;
            }
        }
    }
}