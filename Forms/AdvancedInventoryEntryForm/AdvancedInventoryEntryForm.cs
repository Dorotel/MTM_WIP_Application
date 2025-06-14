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
using MTM_WIP_Application.Helpers;

namespace MTM_WIP_Application.Forms.AdvancedInventoryEntryForm;

public partial class AdvancedInventoryEntryForm : Form
{
    public AdvancedInventoryEntryForm()
    {
        try
        {
            ApplicationLog.Log("AdvancedInventoryEntryForm constructor entered.");
            InitializeComponent();

            Helper_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedEntry_Single_ComboBox_Part);
            Helper_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedEntry_Single_ComboBox_Op);
            Helper_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedEntry_Single_ComboBox_Loc);
            Helper_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedEntry_MultiLoc_ComboBox_Part);
            Helper_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedEntry_MultiLoc_ComboBox_Op);
            Helper_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedEntry_MultiLoc_ComboBox_Loc);


            AdvancedEntry_Single_ComboBox_Part.Visible = false;
            AdvancedEntry_Single_ComboBox_Op.Visible = false;
            AdvancedEntry_Single_ComboBox_Loc.Visible = false;
            AdvancedEntry_MultiLoc_ComboBox_Part.Visible = false;
            AdvancedEntry_MultiLoc_ComboBox_Op.Visible = false;
            AdvancedEntry_MultiLoc_ComboBox_Loc.Visible = false;
            AdvancedEntry_Single_Button_Reset.TabStop = false;
            AdvancedEntry_MultiLoc_Button_Reset.TabStop = false;

            // Set ComboBox ForeColor to Red initially
            AdvancedEntry_Single_ComboBox_Part.ForeColor = Color.Red;
            AdvancedEntry_Single_ComboBox_Op.ForeColor = Color.Red;
            AdvancedEntry_Single_ComboBox_Loc.ForeColor = Color.Red;
            AdvancedEntry_MultiLoc_ComboBox_Part.ForeColor = Color.Red;
            AdvancedEntry_MultiLoc_ComboBox_Op.ForeColor = Color.Red;
            AdvancedEntry_MultiLoc_ComboBox_Loc.ForeColor = Color.Red;

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
                ApplicationLog.LogApplicationError(
                    new InvalidOperationException("TabControl 'AdvancedEntry_TabControl' not found."));
                throw new InvalidOperationException("TabControl 'AdvancedEntry_TabControl' not found.");
            }

            if (AdvancedEntry_TabControl_Import == null)
            {
                ApplicationLog.LogApplicationError(
                    new InvalidOperationException("Tab 'AdvancedEntry_TabControl_Import' not found."));
                throw new InvalidOperationException("Tab 'AdvancedEntry_TabControl_Import' not found.");
            }

            ValidateQtyTextBox(AdvancedEntry_Single_TextBox_Qty, "[ Enter Valid Quantity ]");
            ValidateQtyTextBox(AdvancedEntry_Single_TextBox_Count, "[ How Many Transactions ]");
            ValidateQtyTextBox(AdvancedEntry_MultiLoc_TextBox_Qty, "[ Enter Valid Quantity ]");
            AdvancedEntry_TabControl.SelectedIndexChanged += AdvancedEntry_TabControl_SelectedIndexChanged;

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = true;
            SizeGripStyle = SizeGripStyle.Hide;

            // Focus part ComboBox when form is entered
            Enter += (s, e) =>
            {
                if (AdvancedEntry_TabControl.SelectedIndex == 0 && AdvancedEntry_Single_ComboBox_Part.Visible &&
                    AdvancedEntry_Single_ComboBox_Part.Enabled)
                {
                    AdvancedEntry_Single_ComboBox_Part.Focus();
                    AdvancedEntry_Single_ComboBox_Part.SelectAll();
                }
                else if (AdvancedEntry_TabControl.SelectedIndex == 1 && AdvancedEntry_MultiLoc_ComboBox_Part.Visible &&
                         AdvancedEntry_MultiLoc_ComboBox_Part.Enabled)
                {
                    AdvancedEntry_MultiLoc_ComboBox_Part.Focus();
                    AdvancedEntry_MultiLoc_ComboBox_Part.SelectAll();
                }
            };

            ApplicationLog.Log("AdvancedInventoryEntryForm constructor exited.");
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "AdvancedInventoryEntryForm_Ctor");
        }
    }

    private void AdvancedEntry_TabControl_SelectedIndexChanged(object? sender, EventArgs e)
    {
        switch (AdvancedEntry_TabControl.SelectedIndex)
        {
            case 0: // Tab 1
                ClientSize = new Size(431, 309);
                if (AdvancedEntry_Single_ComboBox_Part.Visible && AdvancedEntry_Single_ComboBox_Part.Enabled)
                    AdvancedEntry_Single_ComboBox_Part.Focus();
                break;
            case 1: // Tab 2
                ClientSize = new Size(835, 309);
                if (AdvancedEntry_MultiLoc_ComboBox_Part.Visible && AdvancedEntry_MultiLoc_ComboBox_Part.Enabled)
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
            // Ensure ForeColor is red after async load
            AdvancedEntry_Single_ComboBox_Part.ForeColor = Color.Red;
            AdvancedEntry_Single_ComboBox_Op.ForeColor = Color.Red;
            AdvancedEntry_Single_ComboBox_Loc.ForeColor = Color.Red;
            AdvancedEntry_MultiLoc_ComboBox_Part.ForeColor = Color.Red;
            AdvancedEntry_MultiLoc_ComboBox_Op.ForeColor = Color.Red;
            AdvancedEntry_MultiLoc_ComboBox_Loc.ForeColor = Color.Red;
            AdvancedEntry_Single_ComboBox_Part.Focus();
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "AdvancedInventoryEntryForm.OnLoad");
        }
    }

    private async Task LoadAllComboBoxesAsync()
    {
        try
        {
            await using var connection = new MySqlConnection(Core_WipAppVariables.ConnectionString);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_part_ids_Get_All",
                connection,
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_Single_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                connection,
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_Single_ComboBox_Op,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_locations_Get_All",
                connection,
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_Single_ComboBox_Loc,
                "Location",
                "Location",
                "[ Enter Location ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_part_ids_Get_All",
                connection,
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_MultiLoc_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                connection,
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_MultiLoc_ComboBox_Op,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_locations_Get_All",
                connection,
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_MultiLoc_ComboBox_Loc,
                "Location",
                "Location",
                "[ Enter Location ]",
                CommandType.StoredProcedure);

            ApplicationLog.Log("AdvancedInventoryEntryForm ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
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
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_Single_ComboBox_Part, "[ Enter Part ID ]");
                UpdateSingleSaveButtonState();
                ApplicationLog.Log("Single Part ComboBox selection changed.");
            };
            AdvancedEntry_Single_ComboBox_Op.SelectedIndexChanged += (s, e) =>
            {
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_Single_ComboBox_Op, "[ Enter Op # ]");
                UpdateSingleSaveButtonState();
                ApplicationLog.Log("Single Op ComboBox selection changed.");
            };
            AdvancedEntry_Single_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
            {
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_Single_ComboBox_Loc, "[ Enter Location ]");
                UpdateSingleSaveButtonState();
                ApplicationLog.Log("Single Loc ComboBox selection changed.");
            };
            AdvancedEntry_Single_TextBox_Qty.Text = "[ Enter Valid Quantity ]";
            AdvancedEntry_Single_TextBox_Qty.TextChanged += (s, e) =>
            {
                InventoryTextBoxQty_TextChanged(AdvancedEntry_Single_TextBox_Qty, "[ Enter Valid Quantity ]");
                UpdateSingleSaveButtonState();
                ApplicationLog.Log("Single Qty TextBox changed.");
            };
            AdvancedEntry_Single_TextBox_Qty.Enter += (s, e) => AdvancedEntry_Single_TextBox_Qty.SelectAll();
            AdvancedEntry_Single_TextBox_Qty.Click += (s, e) => AdvancedEntry_Single_TextBox_Qty.SelectAll();
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
                ApplicationLog.Log("Single Count TextBox changed.");
            };
            AdvancedEntry_Single_TextBox_Count.Enter += (s, e) => AdvancedEntry_Single_TextBox_Count.SelectAll();
            AdvancedEntry_Single_TextBox_Count.Click += (s, e) => AdvancedEntry_Single_TextBox_Count.SelectAll();
            AdvancedEntry_Single_TextBox_Count.KeyDown += (sender, e) =>
            {
                MainFormControlHelper.AdjustQuantityByKey_Transfers(sender, e, "[ How Many Transactions ]",
                    Color.Black, Color.Red);
            };

            AdvancedEntry_MultiLoc_Button_Reset.Click += AdvancedEntry_MultiLoc_Button_Reset_Click;
            AdvancedEntry_MultiLoc_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_MultiLoc_ComboBox_Part, "[ Enter Part ID ]");
                UpdateMultiSaveButtonState();
                ApplicationLog.Log("Multi Part ComboBox selection changed.");
            };
            AdvancedEntry_MultiLoc_ComboBox_Op.SelectedIndexChanged += (s, e) =>
            {
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_MultiLoc_ComboBox_Op, "[ Enter Op # ]");
                UpdateMultiSaveButtonState();
                ApplicationLog.Log("Multi Op ComboBox selection changed.");
            };
            AdvancedEntry_MultiLoc_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
            {
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_MultiLoc_ComboBox_Loc, "[ Enter Location ]");
                UpdateMultiSaveButtonState();
                ApplicationLog.Log("Multi Loc ComboBox selection changed.");
            };

            AdvancedEntry_MultiLoc_TextBox_Qty.Text = "[ Enter Valid Quantity ]";
            AdvancedEntry_MultiLoc_TextBox_Qty.TextChanged += (s, e) =>
            {
                InventoryTextBoxQty_TextChanged(AdvancedEntry_MultiLoc_TextBox_Qty, "[ Enter Valid Quantity ]");
                UpdateMultiSaveButtonState();
                ApplicationLog.Log("Multi Qty TextBox changed.");
            };
            AdvancedEntry_MultiLoc_TextBox_Qty.Enter += (s, e) => AdvancedEntry_MultiLoc_TextBox_Qty.SelectAll();
            AdvancedEntry_MultiLoc_TextBox_Qty.Click += (s, e) => AdvancedEntry_MultiLoc_TextBox_Qty.SelectAll();
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

            // Highlight notes RichTextBox on Enter

            AdvancedEntry_Single_RichTextBox_Notes.Enter += (s, e) =>
                AdvancedEntry_Single_RichTextBox_Notes.BackColor = Color.LightBlue;
            AdvancedEntry_Single_RichTextBox_Notes.Leave += (s, e) =>
                AdvancedEntry_Single_RichTextBox_Notes.BackColor = SystemColors.Window;

            AdvancedEntry_MultiLoc_RichTextBox_Notes.Enter += (s, e) =>
                AdvancedEntry_MultiLoc_RichTextBox_Notes.BackColor = Color.LightBlue;
            AdvancedEntry_MultiLoc_RichTextBox_Notes.Leave += (s, e) =>
                AdvancedEntry_MultiLoc_RichTextBox_Notes.BackColor = SystemColors.Window;

            // TextBoxes: Highlight on Enter/Leave
            AdvancedEntry_Single_TextBox_Qty.Enter +=
                (s, e) => AdvancedEntry_Single_TextBox_Qty.BackColor = Color.LightBlue;
            AdvancedEntry_Single_TextBox_Qty.Leave +=
                (s, e) => AdvancedEntry_Single_TextBox_Qty.BackColor = SystemColors.Window;

            AdvancedEntry_Single_TextBox_Count.Enter +=
                (s, e) => AdvancedEntry_Single_TextBox_Count.BackColor = Color.LightBlue;
            AdvancedEntry_Single_TextBox_Count.Leave +=
                (s, e) => AdvancedEntry_Single_TextBox_Count.BackColor = SystemColors.Window;

            AdvancedEntry_MultiLoc_TextBox_Qty.Enter +=
                (s, e) => AdvancedEntry_MultiLoc_TextBox_Qty.BackColor = Color.LightBlue;
            AdvancedEntry_MultiLoc_TextBox_Qty.Leave +=
                (s, e) => AdvancedEntry_MultiLoc_TextBox_Qty.BackColor = SystemColors.Window;

            // ComboBoxes: Highlight and validate on Enter/Leave
            AdvancedEntry_Single_ComboBox_Part.Enter +=
                (s, e) => AdvancedEntry_Single_ComboBox_Part.BackColor = Color.LightBlue;
            AdvancedEntry_Single_ComboBox_Part.Leave += (s, e) =>
            {
                AdvancedEntry_Single_ComboBox_Part.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_Single_ComboBox_Part, "[ Enter Part ID ]");
            };
            AdvancedEntry_Single_ComboBox_Op.Enter +=
                (s, e) => AdvancedEntry_Single_ComboBox_Op.BackColor = Color.LightBlue;
            AdvancedEntry_Single_ComboBox_Op.Leave += (s, e) =>
            {
                AdvancedEntry_Single_ComboBox_Op.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_Single_ComboBox_Op, "[ Enter Op # ]");
            };
            AdvancedEntry_Single_ComboBox_Loc.Enter +=
                (s, e) => AdvancedEntry_Single_ComboBox_Loc.BackColor = Color.LightBlue;
            AdvancedEntry_Single_ComboBox_Loc.Leave += (s, e) =>
            {
                AdvancedEntry_Single_ComboBox_Loc.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_Single_ComboBox_Loc, "[ Enter Location ]");
            };
            AdvancedEntry_MultiLoc_ComboBox_Part.Enter +=
                (s, e) => AdvancedEntry_MultiLoc_ComboBox_Part.BackColor = Color.LightBlue;
            AdvancedEntry_MultiLoc_ComboBox_Part.Leave += (s, e) =>
            {
                AdvancedEntry_MultiLoc_ComboBox_Part.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_MultiLoc_ComboBox_Part, "[ Enter Part ID ]");
            };
            AdvancedEntry_MultiLoc_ComboBox_Op.Enter +=
                (s, e) => AdvancedEntry_MultiLoc_ComboBox_Op.BackColor = Color.LightBlue;
            AdvancedEntry_MultiLoc_ComboBox_Op.Leave += (s, e) =>
            {
                AdvancedEntry_MultiLoc_ComboBox_Op.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_MultiLoc_ComboBox_Op, "[ Enter Op # ]");
            };
            AdvancedEntry_MultiLoc_ComboBox_Loc.Enter +=
                (s, e) => AdvancedEntry_MultiLoc_ComboBox_Loc.BackColor = Color.LightBlue;
            AdvancedEntry_MultiLoc_ComboBox_Loc.Leave += (s, e) =>
            {
                AdvancedEntry_MultiLoc_ComboBox_Loc.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_MultiLoc_ComboBox_Loc, "[ Enter Location ]");
            };

            ApplicationLog.Log("AdvancedInventoryEntryForm events wired up.");
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventoryEntryForm_WireUpEvents");
        }
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
            ApplicationLog.LogApplicationError(ex);
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

    private async void AdvancedEntry_Single_Button_Reset_Click(object? sender, EventArgs e)
    {
        try
        {
            ApplicationLog.Log("Single Reset button clicked.");

            AdvancedEntry_Single_ComboBox_Part.Visible = false;
            AdvancedEntry_Single_ComboBox_Op.Visible = false;
            AdvancedEntry_Single_ComboBox_Loc.Visible = false;

            // Reinitialize ComboBox DataTables
            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_part_ids_Get_All",
                new MySqlConnection(Core_WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_Single_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                new MySqlConnection(Core_WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_Single_ComboBox_Op,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_locations_Get_All",
                new MySqlConnection(Core_WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_Single_ComboBox_Loc,
                "Location",
                "Location",
                "[ Enter Location ]",
                CommandType.StoredProcedure);

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
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedEntry_Single_Button_Reset_Click");
        }
    }

    private async void AdvancedEntry_MultiLoc_Button_Reset_Click(object? sender, EventArgs e)
    {
        try
        {
            ApplicationLog.Log("Multi Reset button clicked.");

            AdvancedEntry_MultiLoc_ComboBox_Part.Visible = false;
            AdvancedEntry_MultiLoc_ComboBox_Op.Visible = false;
            AdvancedEntry_MultiLoc_ComboBox_Loc.Visible = false;

            // Reinitialize ComboBox DataTables
            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_part_ids_Get_All",
                new MySqlConnection(Core_WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_MultiLoc_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                new MySqlConnection(Core_WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_MultiLoc_ComboBox_Op,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_locations_Get_All",
                new MySqlConnection(Core_WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                AdvancedEntry_MultiLoc_ComboBox_Loc,
                "Location",
                "Location",
                "[ Enter Location ]",
                CommandType.StoredProcedure);

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
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
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
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventoryEntryForm_ProcessCmdKey");
            return false;
        }
    }

    private async void AdvancedEntry_Single_Button_Save_Click(object sender, EventArgs e)
    {
        try
        {
            ApplicationLog.Log("AdvancedEntry_Single_Button_Save_Click entered.");

            // Get values from controls
            var partId = AdvancedEntry_Single_ComboBox_Part.Text;
            var op = AdvancedEntry_Single_ComboBox_Op.Text;
            var loc = AdvancedEntry_Single_ComboBox_Loc.Text;
            var qtyText = AdvancedEntry_Single_TextBox_Qty.Text.Trim();
            var countText = AdvancedEntry_Single_TextBox_Count.Text.Trim();
            var notes = AdvancedEntry_Single_RichTextBox_Notes.Text.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(partId) || AdvancedEntry_Single_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_Single_ComboBox_Part.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(op) || AdvancedEntry_Single_ComboBox_Op.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Operation.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_Single_ComboBox_Op.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(loc) || AdvancedEntry_Single_ComboBox_Loc.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Location.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_Single_ComboBox_Loc.Focus();
                return;
            }

            if (!int.TryParse(qtyText, out var qty) || qty <= 0)
            {
                MessageBox.Show(@"Please enter a valid quantity.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_Single_TextBox_Qty.Focus();
                return;
            }

            if (!int.TryParse(countText, out var count) || count <= 0)
            {
                MessageBox.Show(@"Please enter a valid transaction count.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_Single_TextBox_Count.Focus();
                return;
            }

            // Set Core_WipAppVariables (if needed)
            Core_WipAppVariables.PartId = partId;
            Core_WipAppVariables.Operation = op;
            Core_WipAppVariables.Location = loc;
            Core_WipAppVariables.Notes = notes;
            Core_WipAppVariables.InventoryQuantity = qty;
            Core_WipAppVariables.User ??= Environment.UserName;
            Core_WipAppVariables.PartType ??= "";

            // Save the specified number of transactions
            for (var i = 0; i < count; i++)
                await Dao_Inventory.AddInventoryItemAsync(
                    partId,
                    loc,
                    op,
                    qty,
                    Core_WipAppVariables.PartType ?? "",
                    Core_WipAppVariables.User,
                    "", // batchNumber
                    notes,
                    true);

            MessageBox.Show(
                $@"{count} inventory transaction(s) saved successfully.",
                @"Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            ApplicationLog.Log(
                $"Saved {count} inventory transaction(s) for Part: {partId}, Op: {op}, Loc: {loc}, Qty: {qty}");

            // Optionally reset the form after save
            AdvancedEntry_Single_Button_Reset_Click(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "AdvancedEntry_Single_Button_Save_Click");
        }
    }

    private void AdvancedEntry_MultiLoc_Button_AddLoc_Click(object sender, EventArgs e)
    {
        try
        {
            ApplicationLog.Log("AdvancedEntry_MultiLoc_Button_AddLoc_Click entered.");

            // Get values from controls
            var partId = AdvancedEntry_MultiLoc_ComboBox_Part.Text;
            var op = AdvancedEntry_MultiLoc_ComboBox_Op.Text;
            var loc = AdvancedEntry_MultiLoc_ComboBox_Loc.Text;
            var qtyText = AdvancedEntry_MultiLoc_TextBox_Qty.Text.Trim();
            var notes = AdvancedEntry_MultiLoc_RichTextBox_Notes.Text.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(partId) || AdvancedEntry_MultiLoc_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_MultiLoc_ComboBox_Part.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(op) || AdvancedEntry_MultiLoc_ComboBox_Op.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Operation.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_MultiLoc_ComboBox_Op.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(loc) || AdvancedEntry_MultiLoc_ComboBox_Loc.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Location.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_MultiLoc_ComboBox_Loc.Focus();
                return;
            }

            if (!int.TryParse(qtyText, out var qty) || qty <= 0)
            {
                MessageBox.Show(@"Please enter a valid quantity.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_MultiLoc_TextBox_Qty.Focus();
                return;
            }

            // Prevent duplicate location entries
            foreach (ListViewItem item in AdvancedEntry_MultiLoc_ListView_Preview.Items)
                if (string.Equals(item.SubItems[0].Text, loc, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(@"This location has already been added.", @"Duplicate Entry", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AdvancedEntry_MultiLoc_ComboBox_Loc.Focus();
                    return;
                }

            // Add to ListView
            var listViewItem = new ListViewItem(new[] { loc, qty.ToString(), notes });
            AdvancedEntry_MultiLoc_ListView_Preview.Items.Add(listViewItem);

            ApplicationLog.Log($"Added MultiLoc entry: Loc={loc}, Qty={qty}, Notes={notes}");

            // Reset only the location, quantity, and notes fields for next entry
            MainFormControlHelper.ResetComboBox(AdvancedEntry_MultiLoc_ComboBox_Loc, Color.Red, 0);
            AdvancedEntry_MultiLoc_ComboBox_Loc.Focus();

            UpdateMultiSaveButtonState();
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedEntry_MultiLoc_Button_AddLoc_Click");
        }
    }

    private async void AdvancedEntry_MultiLoc_Button_SaveAll_Click(object sender, EventArgs e)
    {
        try
        {
            ApplicationLog.Log("AdvancedEntry_MultiLoc_Button_SaveAll_Click entered.");

            // Validate that there is at least one entry to save
            if (AdvancedEntry_MultiLoc_ListView_Preview.Items.Count == 0)
            {
                MessageBox.Show(@"Please add at least one location entry before saving.", @"No Entries",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get shared values from controls
            var partId = AdvancedEntry_MultiLoc_ComboBox_Part.Text;
            var op = AdvancedEntry_MultiLoc_ComboBox_Op.Text;

            if (string.IsNullOrWhiteSpace(partId) || AdvancedEntry_MultiLoc_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_MultiLoc_ComboBox_Part.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(op) || AdvancedEntry_MultiLoc_ComboBox_Op.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Operation.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedEntry_MultiLoc_ComboBox_Op.Focus();
                return;
            }

            // Save each entry in the ListView
            var savedCount = 0;
            foreach (ListViewItem item in AdvancedEntry_MultiLoc_ListView_Preview.Items)
            {
                var loc = item.SubItems[0].Text;
                var qtyText = item.SubItems[1].Text;
                var notes = item.SubItems[2].Text;

                if (!int.TryParse(qtyText, out var qty) || qty <= 0)
                {
                    ApplicationLog.LogApplicationError(
                        new Exception($"Invalid quantity for location '{loc}': '{qtyText}'"));
                    continue;
                }

                // Set Core_WipAppVariables (if needed)
                Core_WipAppVariables.PartId = partId;
                Core_WipAppVariables.Operation = op;
                Core_WipAppVariables.Location = loc;
                Core_WipAppVariables.Notes = notes;
                Core_WipAppVariables.InventoryQuantity = qty;
                Core_WipAppVariables.User ??= Environment.UserName;
                Core_WipAppVariables.PartType ??= "";

                await Dao_Inventory.AddInventoryItemAsync(
                    partId,
                    loc,
                    op,
                    qty,
                    Core_WipAppVariables.PartType ?? "",
                    Core_WipAppVariables.User,
                    "", // batchNumber
                    notes,
                    true);

                savedCount++;
            }

            MessageBox.Show(
                $@"{savedCount} inventory transaction(s) saved successfully.",
                @"Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            ApplicationLog.Log(
                $"Saved {savedCount} multi-location inventory transaction(s) for Part: {partId}, Op: {op}");

            // Optionally reset the form after save
            AdvancedEntry_MultiLoc_Button_Reset_Click(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "AdvancedEntry_MultiLoc_Button_SaveAll_Click");
        }
    }
}