using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Forms.MainForm.Classes;
using System;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using Color = System.Drawing.Color;
using MySql.Data.MySqlClient;
using MTM_WIP_Application.Helpers;

namespace MTM_WIP_Application.Forms.AdvancedInventoryEntryForm;

public partial class AdvancedInventoryEntryForm : Form
{
    #region Constructor and Initialization

    public AdvancedInventoryEntryForm()
    {
        try
        {
            LoggingUtility.Log("AdvancedInventoryEntryForm constructor entered.");
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
                LoggingUtility.LogApplicationError(
                    new InvalidOperationException("TabControl 'AdvancedEntry_TabControl' not found."));
                throw new InvalidOperationException("TabControl 'AdvancedEntry_TabControl' not found.");
            }

            if (AdvancedEntry_TabControl_Import == null)
            {
                LoggingUtility.LogApplicationError(
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

            AdvancedEntry_Import_Button_OpenExcel.Click += AdvancedEntry_Import_Button_OpenExcel_Click;

            LoggingUtility.Log("AdvancedInventoryEntryForm constructor exited.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "AdvancedInventoryEntryForm_Ctor");
        }
    }

    #endregion

    #region TabControl Event

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
                ClientSize = new Size(835, 600);
                break;
        }

        // Recenter the form on its parent (Owner) if available
        if (Owner != null)
        {
            var x = Owner.Location.X + (Owner.Width - Width) / 2;
            var y = Owner.Location.Y + (Owner.Height - Height) / 2;
            Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
        }
    }

    #endregion

    #region Form Load

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

            if (Owner != null)
            {
                var x = Owner.Location.X + (Owner.Width - Width) / 2;
                var y = Owner.Location.Y + (Owner.Height - Height) / 2;
                Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "AdvancedInventoryEntryForm.OnLoad");
        }
    }

    #endregion

    #region ComboBox Loading

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

            LoggingUtility.Log("AdvancedInventoryEntryForm ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "AdvancedInventoryEntryForm_LoadAllComboBoxesAsync");
        }
    }

    #endregion

    #region Event Wiring

    private void WireUpEvents()
    {
        try
        {
            AdvancedEntry_Single_Button_Reset.Click += AdvancedEntry_Single_Button_Reset_Click;
            AdvancedEntry_Single_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_Single_ComboBox_Part, "[ Enter Part ID ]");
                UpdateSingleSaveButtonState();
                LoggingUtility.Log("Single Part ComboBox selection changed.");
            };
            AdvancedEntry_Single_ComboBox_Op.SelectedIndexChanged += (s, e) =>
            {
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_Single_ComboBox_Op, "[ Enter Op # ]");
                UpdateSingleSaveButtonState();
                LoggingUtility.Log("Single Op ComboBox selection changed.");
            };
            AdvancedEntry_Single_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
            {
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_Single_ComboBox_Loc, "[ Enter Location ]");
                UpdateSingleSaveButtonState();
                LoggingUtility.Log("Single Loc ComboBox selection changed.");
            };
            AdvancedEntry_Single_TextBox_Qty.Text = "[ Enter Valid Quantity ]";
            AdvancedEntry_Single_TextBox_Qty.TextChanged += (s, e) =>
            {
                InventoryTextBoxQty_TextChanged(AdvancedEntry_Single_TextBox_Qty, "[ Enter Valid Quantity ]");
                UpdateSingleSaveButtonState();
                LoggingUtility.Log("Single Qty TextBox changed.");
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
                LoggingUtility.Log("Single Count TextBox changed.");
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
                LoggingUtility.Log("Multi Part ComboBox selection changed.");
            };
            AdvancedEntry_MultiLoc_ComboBox_Op.SelectedIndexChanged += (s, e) =>
            {
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_MultiLoc_ComboBox_Op, "[ Enter Op # ]");
                UpdateMultiSaveButtonState();
                LoggingUtility.Log("Multi Op ComboBox selection changed.");
            };
            AdvancedEntry_MultiLoc_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
            {
                Helper_ComboBoxes.ValidateComboBoxItem(AdvancedEntry_MultiLoc_ComboBox_Loc, "[ Enter Location ]");
                UpdateMultiSaveButtonState();
                LoggingUtility.Log("Multi Loc ComboBox selection changed.");
            };

            AdvancedEntry_MultiLoc_TextBox_Qty.Text = "[ Enter Valid Quantity ]";
            AdvancedEntry_MultiLoc_TextBox_Qty.TextChanged += (s, e) =>
            {
                InventoryTextBoxQty_TextChanged(AdvancedEntry_MultiLoc_TextBox_Qty, "[ Enter Valid Quantity ]");
                UpdateMultiSaveButtonState();
                LoggingUtility.Log("Multi Qty TextBox changed.");
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

            LoggingUtility.Log("AdvancedInventoryEntryForm events wired up.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventoryEntryForm_WireUpEvents");
        }
    }

    #endregion

    #region Validation and Utility Methods

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
            LoggingUtility.LogApplicationError(ex);
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
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventoryEntryForm_ProcessCmdKey");
            return false;
        }
    }

    #endregion

    #region Single Entry Actions

    private async void AdvancedEntry_Single_Button_Reset_Click(object? sender, EventArgs e)
    {
        try
        {
            LoggingUtility.Log("Single Reset button clicked.");

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
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedEntry_Single_Button_Reset_Click");
        }
    }

    private async void AdvancedEntry_Single_Button_Save_Click(object sender, EventArgs e)
    {
        try
        {
            LoggingUtility.Log("AdvancedEntry_Single_Button_Save_Click entered.");

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

            LoggingUtility.Log(
                $"Saved {count} inventory transaction(s) for Part: {partId}, Op: {op}, Loc: {loc}, Qty: {qty}");

            // Optionally reset the form after save
            AdvancedEntry_Single_Button_Reset_Click(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "AdvancedEntry_Single_Button_Save_Click");
        }
    }

    #endregion

    #region Multi-Location Actions

    private async void AdvancedEntry_MultiLoc_Button_Reset_Click(object? sender, EventArgs e)
    {
        try
        {
            LoggingUtility.Log("Multi Reset button clicked.");

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
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedEntry_MultiLoc_Button_Reset_Click");
        }
    }

    private void AdvancedEntry_MultiLoc_Button_AddLoc_Click(object sender, EventArgs e)
    {
        try
        {
            LoggingUtility.Log("AdvancedEntry_MultiLoc_Button_AddLoc_Click entered.");

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
            var listViewItem = new ListViewItem([loc, qty.ToString(), notes]);
            AdvancedEntry_MultiLoc_ListView_Preview.Items.Add(listViewItem);

            LoggingUtility.Log($"Added MultiLoc entry: Loc={loc}, Qty={qty}, Notes={notes}");

            // Disable part ComboBox after the first location is added
            if (AdvancedEntry_MultiLoc_ListView_Preview.Items.Count == 1)
                AdvancedEntry_MultiLoc_ComboBox_Part.Enabled = false;

            // Reset only the location, quantity, and notes fields for next entry
            MainFormControlHelper.ResetComboBox(AdvancedEntry_MultiLoc_ComboBox_Loc, Color.Red, 0);
            AdvancedEntry_MultiLoc_ComboBox_Loc.Focus();

            UpdateMultiSaveButtonState();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedEntry_MultiLoc_Button_AddLoc_Click");
        }
    }

    private async void AdvancedEntry_MultiLoc_Button_SaveAll_Click(object sender, EventArgs e)
    {
        try
        {
            LoggingUtility.Log("AdvancedEntry_MultiLoc_Button_SaveAll_Click entered.");

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
                    LoggingUtility.LogApplicationError(
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

            LoggingUtility.Log(
                $"Saved {savedCount} multi-location inventory transaction(s) for Part: {partId}, Op: {op}");

            // Optionally reset the form after save
            AdvancedEntry_MultiLoc_Button_Reset_Click(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "AdvancedEntry_MultiLoc_Button_SaveAll_Click");
        }
    }

    #endregion

    #region Excel Export/Import Helpers

    private static string GetWipAppExcelUserFolder()
    {
        // Get the log file path to determine the log directory
        var server = new MySqlConnectionStringBuilder(Core_WipAppVariables.ConnectionString).Server;
        var userName = Core_WipAppVariables.User ?? Environment.UserName;
        var logFilePath = Helper_SqlVariables.GetLogFilePath(server, userName);
        var logDir = Directory.GetParent(logFilePath)?.Parent?.FullName ?? "";
        // Place Excel files as a sibling to the log folder
        var excelRoot = Path.Combine(logDir, "WIP App Excel Files");
        var userFolder = Path.Combine(excelRoot, userName);
        if (!Directory.Exists(userFolder))
            Directory.CreateDirectory(userFolder);
        return userFolder;
    }

    private static string GetUserExcelFilePath()
    {
        var userFolder = GetWipAppExcelUserFolder();
        var fileName = $"{Core_WipAppVariables.User ?? Environment.UserName}_import.xlsx";
        return Path.Combine(userFolder, fileName);
    }

    private void AdvancedEntry_Import_Button_OpenExcel_Click(object? sender, EventArgs e)
    {
        try
        {
            var excelPath = GetUserExcelFilePath();
            if (!File.Exists(excelPath))
            {
                // Ensure the user folder exists
                var userFolder = Path.GetDirectoryName(excelPath);
                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder!);
                // Copy template file to user's Excel file path
                var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Forms",
                    "AdvancedInventoryEntryForm", "WIPAppTemplate.xlsx");
                if (File.Exists(templatePath))
                {
                    File.Copy(templatePath, excelPath, false);
                }
                else
                {
                    MessageBox.Show($"Excel template not found: {templatePath}", "Template Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string[] possibleExcelPaths =
            [
                @"C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.EXE",
                @"C:\\Program Files (x86)\\Microsoft Office\\root\\Office16\\EXCEL.EXE"
            ];
            string? excelExe = null;
            foreach (var path in possibleExcelPaths)
                if (File.Exists(path))
                {
                    excelExe = path;
                    break;
                }

            if (excelExe != null)
                Process.Start(new ProcessStartInfo(excelExe, '"' + excelPath + '"') { UseShellExecute = true });
            else
                // Fallback: open with default Excel handler
                Process.Start(new ProcessStartInfo(excelPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show($"Failed to open Excel file: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    #endregion

    #region Excel Import/Export Actions

    private void AdvancedEntry_Import_Button_ImportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            var excelPath = GetUserExcelFilePath();
            if (!File.Exists(excelPath))
            {
                MessageBox.Show("Excel file not found. Please create or open the Excel file first.", "File Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dt = new DataTable();
            using (var workbook = new XLWorkbook(excelPath))
            {
                var worksheet = workbook.Worksheet("Tab 1");
                if (worksheet == null)
                {
                    MessageBox.Show("Worksheet 'Tab 1' not found in the Excel file.", "Worksheet Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the used range
                var usedRange = worksheet.RangeUsed();
                if (usedRange == null)
                {
                    MessageBox.Show("No data found in 'Tab 1'.", "No Data", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var colCount = usedRange.ColumnCount();
                var rowCount = usedRange.RowCount();

                // Add columns from the first row
                var headerRow = usedRange.Row(1);
                for (var col = 1; col <= colCount; col++)
                {
                    var colName = headerRow.Cell(col).GetValue<string>();
                    if (string.IsNullOrWhiteSpace(colName))
                        colName = $"Column{col}";
                    dt.Columns.Add(colName);
                }

                // Add data rows
                for (var row = 2; row <= rowCount; row++)
                {
                    var dataRow = dt.NewRow();
                    for (var col = 1; col <= colCount; col++)
                        dataRow[col - 1] = usedRange.Row(row).Cell(col).GetValue<string>();
                    dt.Rows.Add(dataRow);
                }
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No data found in the Excel file to import.", "No Data", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            AdvancedEntry_Import_DataGridView.DataSource = dt;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show($"Failed to import Excel data: {ex.Message}", "Import Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void AdvancedEntry_Import_Button_Save_Click(object sender, EventArgs e)
    {
        if (AdvancedEntry_Import_DataGridView.DataSource == null)
            return;

        var dgv = AdvancedEntry_Import_DataGridView;
        var rowsToRemove = new List<DataGridViewRow>();
        var anyError = false;

        // Get DataTables from ComboBoxes' DataSource
        var partTable = AdvancedEntry_Single_ComboBox_Part.DataSource as DataTable;
        var opTable = AdvancedEntry_Single_ComboBox_Op.DataSource as DataTable;
        var locTable = AdvancedEntry_Single_ComboBox_Loc.DataSource as DataTable;

        // Get valid values from DataTables
        var validParts =
            partTable?.AsEnumerable().Select(r => r.Field<string>("Item Number"))
                .Where(s => !string.IsNullOrWhiteSpace(s)).ToHashSet(StringComparer.OrdinalIgnoreCase) ??
            [];
        var validOps =
            opTable?.AsEnumerable().Select(r => r.Field<string>("Operation")).Where(s => !string.IsNullOrWhiteSpace(s))
                .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];
        var validLocs =
            locTable?.AsEnumerable().Select(r => r.Field<string>("Location")).Where(s => !string.IsNullOrWhiteSpace(s))
                .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];

        // Load Excel file for row removal
        var excelPath = GetUserExcelFilePath();
        XLWorkbook? workbook = null;
        IXLWorksheet? worksheet = null;
        if (File.Exists(excelPath))
        {
            workbook = new XLWorkbook(excelPath);
            worksheet = workbook.Worksheet("Tab 1");
        }

        // Collect all Excel row numbers to delete (1-based)
        var excelRowsToDelete = new List<int>();

        foreach (DataGridViewRow row in dgv.Rows)
        {
            if (row.IsNewRow) continue;

            var rowValid = true;
            foreach (DataGridViewCell cell in row.Cells)
                cell.Style.ForeColor = Color.Black;

            var part = row.Cells["Part"].Value?.ToString() ?? "";
            var op = row.Cells["Operation"].Value?.ToString() ?? "";
            var loc = row.Cells["Location"].Value?.ToString() ?? "";
            var qtyText = row.Cells["Quantity"].Value?.ToString() ?? "";
            var notesOriginal = row.Cells["Notes"].Value?.ToString() ?? "";
            var notes = "Excel Import: " + notesOriginal;

            // Validate against ComboBox DataTables
            if (!validParts.Contains(part))
            {
                row.Cells["Part"].Style.ForeColor = Color.Red;
                rowValid = false;
            }

            if (!validOps.Contains(op))
            {
                row.Cells["Operation"].Style.ForeColor = Color.Red;
                rowValid = false;
            }

            if (!validLocs.Contains(loc))
            {
                row.Cells["Location"].Style.ForeColor = Color.Red;
                rowValid = false;
            }

            // Quantity must be a number above 0
            if (!int.TryParse(qtyText, out var qty) || qty <= 0)
            {
                row.Cells["Quantity"].Style.ForeColor = Color.Red;
                rowValid = false;
            }

            foreach (ListViewItem item in AdvancedEntry_MultiLoc_ListView_Preview.Items)
                if (string.Equals(item.SubItems[0].Text, loc, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(@"This location has already been added.", @"Duplicate Entry", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AdvancedEntry_MultiLoc_ComboBox_Loc.Focus();
                    return;
                }

            if (rowValid)
                try
                {
                    await Dao_Inventory.AddInventoryItemAsync(
                        part, loc, op, qty, "", Core_WipAppVariables.User ?? Environment.UserName, "", notes, true);

                    // Find the Excel row number to delete (match by order, not by value)
                    if (worksheet != null)
                    {
                        var usedRange = worksheet.RangeUsed();
                        if (usedRange != null)
                        {
                            var headerRow = usedRange.FirstRow().RowNumber();
                            var lastRow = usedRange.LastRow().RowNumber();
                            var excelRowIndex = headerRow + 1 + row.Index;
                            if (excelRowIndex <= lastRow)
                                excelRowsToDelete.Add(excelRowIndex);
                        }
                    }

                    rowsToRemove.Add(row);
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    foreach (DataGridViewCell cell in row.Cells)
                        cell.Style.ForeColor = Color.Red;
                    rowValid = false;
                    anyError = true;
                }
            else
                anyError = true;
        }

        // Delete rows from Excel in descending order to avoid shifting
        if (worksheet != null && excelRowsToDelete.Count > 0)
        {
            excelRowsToDelete.Sort((a, b) => b.CompareTo(a));
            foreach (var rowNum in excelRowsToDelete)
                worksheet.Row(rowNum).Delete();

            // Push remaining rows up: remove empty rows at the end
            var usedRange = worksheet.RangeUsed();
            if (usedRange != null)
            {
                var headerRow = usedRange.FirstRow().RowNumber();
                var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? headerRow;
                for (var i = lastRow; i > headerRow; i--)
                {
                    var isEmpty = worksheet.Row(i).CellsUsed().All(c => string.IsNullOrWhiteSpace(c.GetString()));
                    if (isEmpty)
                        worksheet.Row(i).Delete();
                }
            }

            workbook?.Save();
        }

        foreach (var row in rowsToRemove)
            if (!row.IsNewRow)
                dgv.Rows.Remove(row);

        RefreshImportDataGridView();

        if (!anyError)
            MessageBox.Show("All transactions saved successfully.", "Success", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        else
            MessageBox.Show("Some rows could not be saved. Please correct highlighted errors.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void RefreshImportDataGridView()
    {
        var excelPath = GetUserExcelFilePath();
        if (!File.Exists(excelPath))
            return;

        // Store highlighted cells before refresh
        var highlightMap = new Dictionary<int, HashSet<string>>();
        if (AdvancedEntry_Import_DataGridView.DataSource is DataTable)
            foreach (DataGridViewRow row in AdvancedEntry_Import_DataGridView.Rows)
            {
                if (row.IsNewRow) continue;
                var cols = new HashSet<string>();
                foreach (DataGridViewCell cell in row.Cells)
                    if (cell.Style.ForeColor == Color.Red)
                        if (cell.OwningColumn != null)
                            cols.Add(cell.OwningColumn.Name);
                if (cols.Count > 0)
                    highlightMap[row.Index] = cols;
            }

        var dt = new DataTable();
        using (var workbook = new XLWorkbook(excelPath))
        {
            var worksheet = workbook.Worksheet("Tab 1");
            if (worksheet == null)
                return;

            var usedRange = worksheet.RangeUsed();
            if (usedRange == null)
                return;

            var colCount = usedRange.ColumnCount();
            var rowCount = usedRange.RowCount();

            var headerRow = usedRange.Row(1);
            for (var col = 1; col <= colCount; col++)
            {
                var colName = headerRow.Cell(col).GetValue<string>();
                if (string.IsNullOrWhiteSpace(colName))
                    colName = $"Column{col}";
                dt.Columns.Add(colName);
            }

            for (var row = 2; row <= rowCount; row++)
            {
                var dataRow = dt.NewRow();
                for (var col = 1; col <= colCount; col++)
                    dataRow[col - 1] = usedRange.Row(row).Cell(col).GetValue<string>();
                dt.Rows.Add(dataRow);
            }
        }

        AdvancedEntry_Import_DataGridView.DataSource = dt;

        // Restore highlights after refresh and re-validate Quantity
        foreach (DataGridViewRow row in AdvancedEntry_Import_DataGridView.Rows)
        {
            if (row.IsNewRow) continue;

            // Restore previous highlights
            if (highlightMap.TryGetValue(row.Index, out var cols))
                foreach (DataGridViewCell cell in row.Cells)
                    if (cell.OwningColumn != null && cols.Contains(cell.OwningColumn.Name))
                        cell.Style.ForeColor = Color.Red;

            // Always validate Quantity column
            if (row.DataGridView != null && row.DataGridView.Columns.Contains("Quantity"))
            {
                var qtyCell = row.Cells["Quantity"];
                var qtyText = qtyCell.Value?.ToString() ?? "";
                if (!int.TryParse(qtyText, out var qty) || qty <= 0) qtyCell.Style.ForeColor = Color.Red;
            }
        }
    }

    #endregion
}