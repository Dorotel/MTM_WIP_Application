using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Forms.MainForm.Classes;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Forms.AdvancedInventoryEntryForm;

public partial class AdvancedInventoryEntryForm : Form
{
    public AdvancedInventoryEntryForm()
    {
        try
        {
            AppLogger.Log("AdvancedInventoryEntryForm constructor entered.");
            InitializeComponent();

            AdvancedEntry_Single_ComboBox_Part.Visible = false;
            AdvancedEntry_Single_ComboBox_Op.Visible = false;
            AdvancedEntry_Single_ComboBox_Loc.Visible = false;
            AdvancedEntry_MultiLoc_ComboBox_Part.Visible = false;
            AdvancedEntry_MultiLoc_ComboBox_Op.Visible = false;
            AdvancedEntry_MultiLoc_ComboBox_Loc.Visible = false;
            AdvancedEntry_Single_Button_Reset.TabStop = false;
            AdvancedEntry_MultiLoc_Button_Reset.TabStop = false;

            WireUpEvents();

            AdvancedEntry_MultiLoc_ListView_Preview.View = View.Details;
            if (AdvancedEntry_MultiLoc_ListView_Preview.Columns.Count == 0)
            {
                AdvancedEntry_MultiLoc_ListView_Preview.Columns.Add("Location", 150);
                AdvancedEntry_MultiLoc_ListView_Preview.Columns.Add("Quantity", 80);
                AdvancedEntry_MultiLoc_ListView_Preview.Columns.Add("Notes", 200);
            }

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
            ValidateQtyTextBox(AdvancedEntry_MultiLoc_TextBox_Qty, "[ Enter Valid Quantity ]"); // <-- Add this line
            AdvancedEntry_TabControl.SelectedIndexChanged += AdvancedEntry_TabControl_SelectedIndexChanged;


            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = true;
            SizeGripStyle = SizeGripStyle.Hide;

            AppLogger.Log("AdvancedInventoryEntryForm constructor exited.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "AdvancedInventoryEntryForm_Ctor");
        }
    }

    private void AdvancedEntry_TabControl_SelectedIndexChanged(object? sender, EventArgs e)
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

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        try
        {
            ClientSize = new Size(431, 309);
            await LoadAllComboBoxesAsync();
            AdvancedEntry_Single_ComboBox_Part.Visible = true;
            AdvancedEntry_Single_ComboBox_Op.Visible = true;
            AdvancedEntry_Single_ComboBox_Loc.Visible = true;
            AdvancedEntry_MultiLoc_ComboBox_Part.Visible = true;
            AdvancedEntry_MultiLoc_ComboBox_Op.Visible = true;
            AdvancedEntry_MultiLoc_ComboBox_Loc.Visible = true;
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
        try
        {
            await using var connection = new MySqlConnection(WipAppVariables.ConnectionString);
            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "SELECT * FROM part_ids", connection, new MySqlDataAdapter(), new DataTable(),
                AdvancedEntry_Single_ComboBox_Part, "Item Number", "ID", "[ Enter Part ID ]");
            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "SELECT * FROM `operation_numbers`", connection, new MySqlDataAdapter(), new DataTable(),
                AdvancedEntry_Single_ComboBox_Op, "Operation", "Operation", "[ Enter Op # ]");
            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "SELECT * FROM `locations`", connection, new MySqlDataAdapter(), new DataTable(),
                AdvancedEntry_Single_ComboBox_Loc, "Location", "Location", "[ Enter Location ]");
            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "SELECT * FROM part_ids", connection, new MySqlDataAdapter(), new DataTable(),
                AdvancedEntry_MultiLoc_ComboBox_Part, "Item Number", "ID", "[ Enter Part ID ]");
            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "SELECT * FROM `operation_numbers`", connection, new MySqlDataAdapter(), new DataTable(),
                AdvancedEntry_MultiLoc_ComboBox_Op, "Operation", "Operation", "[ Enter Op # ]");
            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "SELECT * FROM `locations`", connection, new MySqlDataAdapter(), new DataTable(),
                AdvancedEntry_MultiLoc_ComboBox_Loc, "Location", "Location", "[ Enter Location ]");
            AppLogger.Log("AdvancedInventoryEntryForm ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                "AdvancedInventoryEntryForm_LoadAllComboBoxesAsync");
        }
    }

    private void WireUpEvents()
    {
        try
        {
            AdvancedEntry_Single_Button_Reset.Click += AdvancedEntry_Single_Button_Reset_Click;
            AdvancedEntry_Single_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                SetComboBoxColor(AdvancedEntry_Single_ComboBox_Part);
                UpdateSingleSaveButtonState();
                AppLogger.Log("Single Part ComboBox selection changed.");
            };
            AdvancedEntry_Single_ComboBox_Op.SelectedIndexChanged += (s, e) =>
            {
                SetComboBoxColor(AdvancedEntry_Single_ComboBox_Op);
                UpdateSingleSaveButtonState();
                AppLogger.Log("Single Op ComboBox selection changed.");
            };
            AdvancedEntry_Single_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
            {
                SetComboBoxColor(AdvancedEntry_Single_ComboBox_Loc);
                UpdateSingleSaveButtonState();
                AppLogger.Log("Single Loc ComboBox selection changed.");
            };
            AdvancedEntry_Single_TextBox_Qty.Text = "[ Enter Valid Quantity ]";
            AdvancedEntry_Single_TextBox_Qty.TextChanged += (s, e) =>
            {
                InventoryTextBoxQty_TextChanged(AdvancedEntry_Single_TextBox_Qty, "[ Enter Valid Quantity ]");
                UpdateSingleSaveButtonState();
                AppLogger.Log("Single Qty TextBox changed.");
            };
            AdvancedEntry_Single_TextBox_Qty.Enter += (s, e) => AdvancedEntry_Single_TextBox_Qty.SelectAll();
            AdvancedEntry_Single_TextBox_Qty.KeyDown += (sender, e) =>
            {
                MainFormControlHelper.AdjustQuantityByKey_Quantity(sender, e, "[ Enter Valid Quantity ]",
                    Color.Black, Color.Red);
            };

            AdvancedEntry_Single_TextBox_Count.Text = "[ How Many Transactions ]";
            AdvancedEntry_Single_TextBox_Count.TextChanged += (s, e) =>
            {
                ValidateQtyTextBox(AdvancedEntry_Single_TextBox_Count, "[ How Many Transactions ]");
                UpdateSingleSaveButtonState();
                AppLogger.Log("Single Count TextBox changed.");
            };
            AdvancedEntry_Single_TextBox_Count.Enter += (s, e) => AdvancedEntry_Single_TextBox_Count.SelectAll();
            AdvancedEntry_Single_TextBox_Count.KeyDown += (sender, e) =>
            {
                MainFormControlHelper.AdjustQuantityByKey_Transfers(sender, e, "[ How Many Transactions ]",
                    Color.Black, Color.Red);
            };

            AdvancedEntry_MultiLoc_Button_Reset.Click += AdvancedEntry_MultiLoc_Button_Reset_Click;
            AdvancedEntry_MultiLoc_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                SetComboBoxColor(AdvancedEntry_MultiLoc_ComboBox_Part);
                UpdateMultiSaveButtonState();
                AppLogger.Log("Multi Part ComboBox selection changed.");
            };
            AdvancedEntry_MultiLoc_ComboBox_Op.SelectedIndexChanged += (s, e) =>
            {
                SetComboBoxColor(AdvancedEntry_MultiLoc_ComboBox_Op);
                UpdateMultiSaveButtonState();
                AppLogger.Log("Multi Op ComboBox selection changed.");
            };
            AdvancedEntry_MultiLoc_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
            {
                SetComboBoxColor(AdvancedEntry_MultiLoc_ComboBox_Loc);
                UpdateMultiSaveButtonState();
                AppLogger.Log("Multi Loc ComboBox selection changed.");
            };

            AdvancedEntry_MultiLoc_TextBox_Qty.Text = "[ Enter Valid Quantity ]";
            AdvancedEntry_MultiLoc_TextBox_Qty.TextChanged += (s, e) =>
            {
                InventoryTextBoxQty_TextChanged(AdvancedEntry_MultiLoc_TextBox_Qty, "[ Enter Valid Quantity ]");
                UpdateMultiSaveButtonState();
                AppLogger.Log("Multi Qty TextBox changed.");
            };
            AdvancedEntry_MultiLoc_TextBox_Qty.Enter += (s, e) => AdvancedEntry_MultiLoc_TextBox_Qty.SelectAll();
            AdvancedEntry_MultiLoc_TextBox_Qty.KeyDown += (sender, e) =>
            {
                MainFormControlHelper.AdjustQuantityByKey_Quantity(sender, e, "[ Enter Valid Quantity ]",
                    Color.Black, Color.Red);
            };

            AdvancedEntry_Import_Button_Close.Click += (s, e) => Close();
            AdvancedEntry_Single_ComboBox_Part.Enter += (s, e) => AdvancedEntry_Single_ComboBox_Part.SelectAll();
            AdvancedEntry_Single_ComboBox_Op.Enter += (s, e) => AdvancedEntry_Single_ComboBox_Op.SelectAll();
            AdvancedEntry_Single_ComboBox_Loc.Enter += (s, e) => AdvancedEntry_Single_ComboBox_Loc.SelectAll();
            AdvancedEntry_MultiLoc_ComboBox_Part.Enter += (s, e) => AdvancedEntry_MultiLoc_ComboBox_Part.SelectAll();
            AdvancedEntry_MultiLoc_ComboBox_Op.Enter += (s, e) => AdvancedEntry_MultiLoc_ComboBox_Op.SelectAll();
            AdvancedEntry_MultiLoc_ComboBox_Loc.Enter += (s, e) => AdvancedEntry_MultiLoc_ComboBox_Loc.SelectAll();
            AdvancedEntry_MultiLoc_TextBox_Qty.Click += (s, e) =>
                AdvancedEntry_MultiLoc_TextBox_Qty.SelectAll();
            AdvancedEntry_Single_TextBox_Qty.Click += (s, e) =>
                AdvancedEntry_Single_TextBox_Qty.SelectAll();
            AdvancedEntry_MultiLoc_TextBox_Qty.Enter += (s, e) => AdvancedEntry_MultiLoc_TextBox_Qty.SelectAll();
            AppLogger.Log("AdvancedInventoryEntryForm events wired up.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "AdvancedInventoryEntryForm_WireUpEvents");
        }
    }

    private void AdvancedEntry_MultiLoc_TextBox_Qty_Click(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private static void InventoryTextBoxQty_TextChanged(TextBox textBox, string placeholder)
    {
        try
        {
            var text = textBox.Text.Trim();
            var isValid = int.TryParse(text, out var qty) && qty > 0;
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
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
        }
    }

    private static void SetComboBoxColor(ComboBox cb)
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

    private void UpdateSingleSaveButtonState()
    {
        var partValid = AdvancedEntry_Single_ComboBox_Part.SelectedIndex > 0 &&
                        !string.IsNullOrWhiteSpace(AdvancedEntry_Single_ComboBox_Part.Text);
        var opValid = AdvancedEntry_Single_ComboBox_Op.SelectedIndex > 0 &&
                      !string.IsNullOrWhiteSpace(AdvancedEntry_Single_ComboBox_Op.Text);
        var locValid = AdvancedEntry_Single_ComboBox_Loc.SelectedIndex > 0 &&
                       !string.IsNullOrWhiteSpace(AdvancedEntry_Single_ComboBox_Loc.Text);
        var qtyValid = int.TryParse(AdvancedEntry_Single_TextBox_Qty.Text.Trim(), out var qty) && qty > 0;
        var countValid = int.TryParse(AdvancedEntry_Single_TextBox_Count.Text.Trim(), out var count) && count > 0;
        AdvancedEntry_Single_Button_Save.Enabled = partValid && opValid && locValid && qtyValid && countValid;
    }

    private void UpdateMultiSaveButtonState()
    {
        var partValid = AdvancedEntry_MultiLoc_ComboBox_Part.SelectedIndex > 0 &&
                        !string.IsNullOrWhiteSpace(AdvancedEntry_MultiLoc_ComboBox_Part.Text);
        var opValid = AdvancedEntry_MultiLoc_ComboBox_Op.SelectedIndex > 0 &&
                      !string.IsNullOrWhiteSpace(AdvancedEntry_MultiLoc_ComboBox_Op.Text);
        var locValid = AdvancedEntry_MultiLoc_ComboBox_Loc.SelectedIndex > 0 &&
                       !string.IsNullOrWhiteSpace(AdvancedEntry_MultiLoc_ComboBox_Loc.Text);
        var qtyValid = int.TryParse(AdvancedEntry_MultiLoc_TextBox_Qty.Text.Trim(), out var qty) && qty > 0;
        AdvancedEntry_MultiLoc_Button_AddLoc.Enabled = partValid && opValid && locValid && qtyValid;
        AdvancedEntry_MultiLoc_Button_SaveAll.Enabled =
            AdvancedEntry_MultiLoc_ListView_Preview.Items.Count > 0 && partValid && opValid;
    }

    private void AdvancedEntry_Single_Button_Reset_Click(object? sender, EventArgs e)
    {
        try
        {
            AppLogger.Log("Single Reset button clicked.");
            AdvancedEntry_Single_ComboBox_Part.Visible = false;
            AdvancedEntry_Single_ComboBox_Op.Visible = false;
            AdvancedEntry_Single_ComboBox_Loc.Visible = false;
            MainFormControlHelper.ResetComboBox(AdvancedEntry_Single_ComboBox_Part, Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedEntry_Single_ComboBox_Op, Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedEntry_Single_ComboBox_Loc, Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedEntry_Single_TextBox_Qty, Color.Red, "[ Enter Valid Quantity ]");
            MainFormControlHelper.ResetTextBox(AdvancedEntry_Single_TextBox_Count, Color.Red,
                "[ Enter How Many Times ]");
            AdvancedEntry_Single_RichTextBox_Notes.Text = string.Empty;
            AdvancedEntry_Single_ComboBox_Part.Visible = true;
            AdvancedEntry_Single_ComboBox_Op.Visible = true;
            AdvancedEntry_Single_ComboBox_Loc.Visible = true;
            AdvancedEntry_Single_ComboBox_Part.Focus();
            UpdateSingleSaveButtonState();
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "AdvancedEntry_Single_Button_Reset_Click");
        }
    }

    private void AdvancedEntry_MultiLoc_Button_Reset_Click(object? sender, EventArgs e)
    {
        try
        {
            AppLogger.Log("Multi Reset button clicked.");
            AdvancedEntry_MultiLoc_ComboBox_Part.Visible = false;
            AdvancedEntry_MultiLoc_ComboBox_Op.Visible = false;
            AdvancedEntry_MultiLoc_ComboBox_Loc.Visible = false;
            MainFormControlHelper.ResetComboBox(AdvancedEntry_MultiLoc_ComboBox_Part, Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedEntry_MultiLoc_ComboBox_Op, Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedEntry_MultiLoc_ComboBox_Loc, Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedEntry_MultiLoc_TextBox_Qty, Color.Red,
                "[ Enter Valid Quantity ]");
            AdvancedEntry_MultiLoc_RichTextBox_Notes.Text = string.Empty;
            AdvancedEntry_MultiLoc_ListView_Preview.Items.Clear();
            AdvancedEntry_MultiLoc_ComboBox_Part.Enabled = true;
            AdvancedEntry_MultiLoc_ComboBox_Op.Enabled = true;
            AdvancedEntry_MultiLoc_ComboBox_Part.Visible = true;
            AdvancedEntry_MultiLoc_ComboBox_Op.Visible = true;
            AdvancedEntry_MultiLoc_ComboBox_Loc.Visible = true;
            AdvancedEntry_MultiLoc_ComboBox_Part.Focus();
            UpdateMultiSaveButtonState();
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedEntry_MultiLoc_Button_Reset_Click");
        }
    }

    private static void ValidateQtyTextBox(TextBox textBox, string placeholder)
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
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventoryEntryForm_ProcessCmdKey");
            return false;
        }
    }
}