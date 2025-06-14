using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.AdvancedInventoryEntryForm;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Models;
using MTM_WIP_Application.Services;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace MTM_WIP_Application.Controls.MainForm;

#region ControlTransferTab

public partial class ControlTransferTab : UserControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    #region Initialization

    public ControlTransferTab()
    {
        InitializeComponent();
        Control_TransferTab_Initialize();
        Control_TransferTab_ComboBox_Part.ForeColor = Color.Red;
        Control_TransferTab_ComboBox_Operation.ForeColor = Color.Red;
        Control_TransferTab_ComboBox_ToLocation.ForeColor = Color.Red;
        Control_TransferTab_Image_NothingFound.Visible = false;
        _ = Control_TransferTab_OnStartup_LoadComboBoxesAsync();
    }

    public void Control_TransferTab_Initialize()
    {
        Control_TransferTab_ComboBox_Operation.Visible = false;
        Control_TransferTab_ComboBox_Part.Visible = false;
        Control_TransferTab_Button_Reset.TabStop = false;
    }

    public void UpdateToggleRightPanelButton()
    {
        var panelCollapsed = MainFormInstance?.MainForm_SplitContainer_Middle.Panel2Collapsed ?? true;
        Control_TransferTab_Button_Toggle_RightPanel.Text = panelCollapsed ? "←" : "→";
        Control_TransferTab_Button_Toggle_RightPanel.ForeColor = panelCollapsed ? Color.Red : Color.Green;
    }

    #endregion

    #region Key Processing

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        try
        {
            if (keyData == Keys.Enter)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                return true;
            }

            if (MainFormInstance != null)
            {
                var panelCollapsed = MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed;
                if ((!panelCollapsed && keyData == (Keys.Alt | Keys.Right)) ||
                    (panelCollapsed && keyData == (Keys.Alt | Keys.Left)))
                {
                    Control_TransferTab_Button_Toggle_RightPanel.PerformClick();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_ProcessCmdKey");
            return false;
        }
    }

    #endregion

    #region Startup / ComboBox Loading

    private async Task Control_TransferTab_OnStartup_LoadComboBoxesAsync()
    {
        try
        {
            await Control_TransferTab_OnStartup_LoadDataComboBoxesAsync();
            Control_TransferTab_OnStartup_WireUpEvents();
            ApplicationLog.Log("Initial setup of ComboBoxes in the Inventory Tab.");
            MainFormTabResetHelper.ResetTransferTab(
                Control_TransferTab_ComboBox_Part,
                Control_TransferTab_ComboBox_Operation,
                Control_TransferTab_Button_Search,
                Control_TransferTab_Button_Transfer);
            Control_TransferTab_ComboBox_Operation.Visible = true;
            Control_TransferTab_ComboBox_Part.Visible = true;
            try
            {
                Core_WipAppVariables.UserFullName =
                    await Dao_User.GetUserFullNameAsync(Core_WipAppVariables.User, true);
                ApplicationLog.Log($"User full name loaded: {Core_WipAppVariables.UserFullName}");
            }
            catch (Exception ex)
            {
                ApplicationLog.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    "Control_TransferTab_OnStartup_GetUserFullName");
            }
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "Control_TransferTab_OnStartup");
        }
    }

    private async Task Control_TransferTab_OnStartup_LoadDataComboBoxesAsync()
    {
        try
        {
            await using var connection = new MySqlConnection(Core_WipAppVariables.ConnectionString);
            var comboBoxSets = new[]
            {
                (new MySqlDataAdapter(), new DataTable(), Control_TransferTab_ComboBox_Part, "md_part_ids_Get_All",
                    "Item Number", "ID", "[ Enter Part ID ]", CommandType.StoredProcedure),
                (new MySqlDataAdapter(), new DataTable(), Control_TransferTab_ComboBox_Operation,
                    "md_operation_numbers_Get_All", "Operation", "Operation", "[ Enter Op # ]",
                    CommandType.StoredProcedure),
                (new MySqlDataAdapter(), new DataTable(), Control_TransferTab_ComboBox_ToLocation,
                    "md_locations_Get_All", "Location", "Location", "[ Enter Location ]", CommandType.StoredProcedure)
            };
            foreach (var (adapter, table, comboBox, procName, display, value, placeholder, cmdType) in comboBoxSets)
                await Helper_ComboBoxes.FillComboBoxAsync(
                    procName, connection, adapter, table, comboBox, display, value, placeholder, cmdType);
            ApplicationLog.Log("Transfer tab ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "MainForm_LoadTransferTabComboBoxesAsync");
        }
    }

    #endregion

    #region Button Clicks

    private async void Control_TransferTab_Button_Reset_Click()
    {
        try
        {
            ApplicationLog.Log("Inventory Reset button clicked.");
            Control_TransferTab_ComboBox_Operation.Visible = false;
            Control_TransferTab_ComboBox_Part.Visible = false;
            Control_TransferTab_ComboBox_ToLocation.Visible = false;
            Control_TransferTab_Image_NothingFound.Visible = false;
            Control_TransferTab_DataGridView_Main.DataSource = null;
            Control_TransferTab_DataGridView_Main.Rows.Clear();
            Control_TransferTab_DataGridView_Main.Refresh();
            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_part_ids_Get_All",
                new MySqlConnection(Core_WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                Control_TransferTab_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);
            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                new MySqlConnection(Core_WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                Control_TransferTab_ComboBox_Operation,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);
            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_locations_Get_All",
                new MySqlConnection(Core_WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                Control_TransferTab_ComboBox_ToLocation,
                "Location",
                "Location",
                "[ Enter Location ]",
                CommandType.StoredProcedure);
            MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_Part, Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_Operation, Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_ToLocation, Color.Red, 0);
            Control_TransferTab_NumericUpDown_Quantity.Value = Control_TransferTab_NumericUpDown_Quantity.Minimum;
            MainFormTabResetHelper.ResetTransferTab(
                Control_TransferTab_ComboBox_Part,
                Control_TransferTab_ComboBox_Operation,
                Control_TransferTab_Button_Search,
                Control_TransferTab_Button_Transfer);
            Control_TransferTab_ComboBox_Operation.Visible = true;
            Control_TransferTab_ComboBox_Part.Visible = true;
            Control_TransferTab_ComboBox_ToLocation.Visible = true;
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_Button_Reset");
        }
    }

    private async void Control_TransferTab_Button_Search_Click(object? sender, EventArgs? e)
    {
        try
        {
            ApplicationLog.Log("TransferTab Search button clicked.");
            var partId = Control_TransferTab_ComboBox_Part.Text;
            var op = Control_TransferTab_ComboBox_Operation.Text;
            if (string.IsNullOrWhiteSpace(partId) || Control_TransferTab_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Control_TransferTab_ComboBox_Part.Focus();
                return;
            }

            DataTable results;
            if (Control_TransferTab_ComboBox_Operation.SelectedIndex > 0 &&
                Control_TransferTab_ComboBox_Operation.Text != @"[ Enter Op # ]")
            {
                ApplicationLog.Log($"Searching inventory for Part ID: {partId} and Operation: {op}");
                results = await Dao_Inventory.GetInventoryByPartIdAndOperationAsync(partId, op, true);
            }
            else
            {
                ApplicationLog.Log($"Searching inventory for Part ID: {partId} without specific operation.");
                results = await Dao_Inventory.GetInventoryByPartIdAsync(partId, true);
            }

            Control_TransferTab_DataGridView_Main.DataSource = results;
            Control_TransferTab_DataGridView_Main.ClearSelection();
            foreach (DataGridViewColumn column in Control_TransferTab_DataGridView_Main.Columns)
                column.Visible = true;
            Core_DgvDesigner.ApplyThemeToDataGridView(
                Control_TransferTab_DataGridView_Main,
                Core_AppThemes.GetTheme(Core_WipAppVariables.WipDataGridTheme ?? "Default (Black and White)")
            );
            Core_DgvDesigner.SizeDataGrid(Control_TransferTab_DataGridView_Main);
            Control_TransferTab_Image_NothingFound.Visible = results.Rows.Count == 0;
            if (Control_TransferTab_DataGridView_Main.Rows.Count > 0)
            {
                Control_TransferTab_DataGridView_Main.ClearSelection();
                Control_TransferTab_DataGridView_Main.Rows[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "Control_TransferTab_Button_Search_Click");
        }
    }

    private async Task Control_TransferTab_Button_Save_ClickAsync(object? sender, EventArgs? e)
    {
        try
        {
            var selectedRows = Control_TransferTab_DataGridView_Main.SelectedRows;
            if (selectedRows.Count == 0)
            {
                MessageBox.Show(@"Please select a row to transfer from.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (Control_TransferTab_ComboBox_ToLocation.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(Control_TransferTab_ComboBox_ToLocation.Text))
            {
                MessageBox.Show(@"Please select a valid destination location.", @"Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedRows.Count == 1)
                await TransferSingleRowAsync(selectedRows[0]);
            else
                await TransferMultipleRowsAsync(selectedRows);

            Control_TransferTab_Button_Search_Click(null, null);
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "Control_TransferTab_Button_Transfer_Click");
        }
    }

    #endregion

    #region Transfer Logic

    private async Task TransferSingleRowAsync(DataGridViewRow row)
    {
        if (row.DataBoundItem is not DataRowView drv)
            return;
        var batchNumber = drv["Batch Number"]?.ToString() ?? "";
        var partId = drv["PartID"]?.ToString() ?? "";
        var fromLocation = drv["Location"]?.ToString() ?? "";
        var itemType = drv.Row.Table.Columns.Contains("ItemType") ? drv["ItemType"]?.ToString() ?? "" : "";
        var notes = drv["Notes"]?.ToString() ?? "";
        var operation = drv["Operation"]?.ToString() ?? "";
        var quantityStr = drv["Quantity"]?.ToString() ?? "";
        int.TryParse(quantityStr, out var originalQuantity);
        var transferQuantity = Math.Min((int)Control_TransferTab_NumericUpDown_Quantity.Value, originalQuantity);
        var newLocation = Control_TransferTab_ComboBox_ToLocation.Text;
        var user = Core_WipAppVariables.User ?? Environment.UserName;
        if (transferQuantity < originalQuantity)
            await Dao_Inventory.TransferInventoryQuantityAsync(
                batchNumber, partId, operation, transferQuantity, originalQuantity, newLocation, user);
        else
            await Dao_Inventory.TransferPartSimpleAsync(
                batchNumber, partId, operation, quantityStr, newLocation);
        await Dao_History.AddTransactionHistoryAsync(new Model_TransactionHistory
        {
            TransactionType = "TRANSFER",
            PartId = partId,
            FromLocation = fromLocation,
            ToLocation = newLocation,
            Operation = operation,
            Quantity = transferQuantity,
            Notes = notes,
            User = user,
            ItemType = itemType,
            BatchNumber = batchNumber,
            DateTime = DateTime.Now
        });
    }

    private async Task TransferMultipleRowsAsync(DataGridViewSelectedRowCollection selectedRows)
    {
        var newLocation = Control_TransferTab_ComboBox_ToLocation.Text;
        var user = Core_WipAppVariables.User ?? Environment.UserName;
        foreach (DataGridViewRow row in selectedRows)
        {
            if (row.DataBoundItem is not DataRowView drv) continue;
            var batchNumber = drv["Batch Number"]?.ToString() ?? "";
            var partId = drv["PartID"]?.ToString() ?? "";
            var fromLocation = drv["Location"]?.ToString() ?? "";
            var itemType = drv.Row.Table.Columns.Contains("ItemType") ? drv["ItemType"]?.ToString() ?? "" : "";
            var operation = drv["Operation"]?.ToString() ?? "";
            var quantityStr = drv["Quantity"]?.ToString() ?? "";
            var notes = drv["Notes"]?.ToString() ?? "";
            int.TryParse(quantityStr, out var originalQuantity);
            var transferQuantity = Math.Min((int)Control_TransferTab_NumericUpDown_Quantity.Value, originalQuantity);
            await Dao_Inventory.TransferPartSimpleAsync(
                batchNumber, partId, operation, quantityStr, newLocation);
            await Dao_History.AddTransactionHistoryAsync(new Model_TransactionHistory
            {
                TransactionType = "TRANSFER",
                PartId = partId,
                FromLocation = fromLocation,
                ToLocation = newLocation,
                Operation = operation,
                Quantity = transferQuantity,
                Notes = notes,
                User = user,
                ItemType = itemType,
                BatchNumber = batchNumber,
                DateTime = DateTime.Now
            });
        }
    }

    #endregion

    #region ComboBox & UI Events

    private void Control_TransferTab_ComboBox_Operation_SelectedIndexChanged()
    {
        try
        {
            ApplicationLog.Log("Inventory Op ComboBox selection changed.");
            if (Control_TransferTab_ComboBox_Operation.SelectedIndex > 0)
            {
                Control_TransferTab_ComboBox_Operation.ForeColor = Color.Black;
                Core_WipAppVariables.Operation = Control_TransferTab_ComboBox_Operation.Text;
            }
            else
            {
                Control_TransferTab_ComboBox_Operation.ForeColor = Color.Red;
                if (Control_TransferTab_ComboBox_Operation.SelectedIndex != 0 &&
                    Control_TransferTab_ComboBox_Operation.Items.Count > 0)
                    Control_TransferTab_ComboBox_Operation.SelectedIndex = 0;
                Core_WipAppVariables.Operation = null;
            }
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Op");
        }
    }

    private void Control_TransferTab_ComboBox_Part_SelectedIndexChanged()
    {
        try
        {
            ApplicationLog.Log("Inventory Part ComboBox selection changed.");
            if (Control_TransferTab_ComboBox_Part.SelectedIndex > 0)
            {
                Control_TransferTab_ComboBox_Part.ForeColor = Color.Black;
                Core_WipAppVariables.PartId = Control_TransferTab_ComboBox_Part.Text;
            }
            else
            {
                Control_TransferTab_ComboBox_Part.ForeColor = Color.Red;
                if (Control_TransferTab_ComboBox_Part.SelectedIndex != 0 &&
                    Control_TransferTab_ComboBox_Part.Items.Count > 0)
                    Control_TransferTab_ComboBox_Part.SelectedIndex = 0;
                Core_WipAppVariables.PartId = null;
            }
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Part");
        }
    }

    private void Control_TransferTab_Update_ButtonStates()
    {
        try
        {
            Control_TransferTab_Button_Search.Enabled = Control_TransferTab_ComboBox_Part.SelectedIndex > 0;
            var hasData = Control_TransferTab_DataGridView_Main.Rows.Count > 0;
            var hasSelection = Control_TransferTab_DataGridView_Main.SelectedRows.Count > 0;
            var hasToLocation = Control_TransferTab_ComboBox_ToLocation.SelectedIndex > 0 &&
                                !string.IsNullOrWhiteSpace(Control_TransferTab_ComboBox_ToLocation.Text);
            var hasPart = Control_TransferTab_ComboBox_Part.SelectedIndex > 0;
            var hasQuantity = Control_TransferTab_NumericUpDown_Quantity.Value > 0;

            // Disable/enable location combobox and number box based on data presence
            Control_TransferTab_ComboBox_ToLocation.Enabled = hasData;
            Control_TransferTab_NumericUpDown_Quantity.Enabled =
                hasData && Control_TransferTab_DataGridView_Main.SelectedRows.Count <= 1;

            // Disable transfer if to location matches selected row's location
            var toLocationIsSameAsRow = false;
            if (hasSelection && hasToLocation)
                foreach (DataGridViewRow row in Control_TransferTab_DataGridView_Main.SelectedRows)
                    if (row.DataBoundItem is DataRowView drv)
                    {
                        var rowLocation = drv["Location"]?.ToString() ?? string.Empty;
                        if (string.Equals(rowLocation, Control_TransferTab_ComboBox_ToLocation.Text,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            toLocationIsSameAsRow = true;
                            break;
                        }
                    }

            Control_TransferTab_Button_Transfer.Enabled =
                hasData && hasSelection && hasToLocation && hasPart && hasQuantity && !toLocationIsSameAsRow;
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "Control_TransferTab_Update_ButtonStates");
        }
    }

    private void Control_TransferTab_OnStartup_WireUpEvents()
    {
        try
        {
            Control_TransferTab_Button_Reset.Click += (s, e) => Control_TransferTab_Button_Reset_Click();
            Control_TransferTab_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                Control_TransferTab_ComboBox_Part_SelectedIndexChanged();
                Control_TransferTab_Update_ButtonStates();
            };
            Control_TransferTab_ComboBox_Operation.SelectedIndexChanged += (s, e) =>
            {
                Control_TransferTab_ComboBox_Operation_SelectedIndexChanged();
                Control_TransferTab_Update_ButtonStates();
            };
            Control_TransferTab_ComboBox_ToLocation.SelectedIndexChanged +=
                (s, e) => Control_TransferTab_Update_ButtonStates();
            Control_TransferTab_NumericUpDown_Quantity.ValueChanged +=
                (s, e) => Control_TransferTab_Update_ButtonStates();
            Control_TransferTab_ComboBox_Part.Enter += (s, e) =>
            {
                Control_TransferTab_ComboBox_Part.BackColor = Color.LightBlue;
            };
            Control_TransferTab_ComboBox_Part.Leave += (s, e) =>
            {
                Control_TransferTab_ComboBox_Part.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_Part, "[ Enter Part ID ]");
            };
            Control_TransferTab_ComboBox_Operation.Enter += (s, e) =>
            {
                Control_TransferTab_ComboBox_Operation.BackColor = Color.LightBlue;
            };
            Control_TransferTab_ComboBox_Operation.Leave += (s, e) =>
            {
                Control_TransferTab_ComboBox_Operation.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_Operation, "[ Enter Op # ]");
            };
            Control_TransferTab_ComboBox_ToLocation.Enter += (s, e) =>
            {
                Control_TransferTab_ComboBox_ToLocation.BackColor = Color.LightBlue;
            };
            Control_TransferTab_ComboBox_ToLocation.Leave += (s, e) =>
            {
                Control_TransferTab_ComboBox_ToLocation.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_ToLocation, "[ Enter Location ]");
            };
            Control_TransferTab_DataGridView_Main.SelectionChanged +=
                (s, e) => Control_TransferTab_Update_ButtonStates();
            Control_TransferTab_DataGridView_Main.SelectionChanged +=
                Control_TransferTab_DataGridView_Main_SelectionChanged;
            Control_TransferTab_Button_Transfer.Click +=
                async (s, e) => await Control_TransferTab_Button_Save_ClickAsync(s, e);
            ApplicationLog.Log("Transfer tab events wired up.");
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_WireUpTransferTabEvents");
        }
    }

    private void Control_TransferTab_DataGridView_Main_SelectionChanged(object? sender, EventArgs? e)
    {
        try
        {
            if (Control_TransferTab_DataGridView_Main.SelectedRows.Count == 1)
            {
                var row = Control_TransferTab_DataGridView_Main.SelectedRows[0];
                if (row.DataBoundItem is DataRowView drv && int.TryParse(drv["Quantity"]?.ToString(), out var qty))
                {
                    Control_TransferTab_NumericUpDown_Quantity.Maximum = qty;
                    Control_TransferTab_NumericUpDown_Quantity.Value = qty;
                    Control_TransferTab_NumericUpDown_Quantity.Enabled = true;
                }
            }
            else if (Control_TransferTab_DataGridView_Main.SelectedRows.Count > 1)
            {
                Control_TransferTab_NumericUpDown_Quantity.Enabled = false;
            }
            else
            {
                Control_TransferTab_NumericUpDown_Quantity.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ApplicationLog.LogApplicationError(ex);
        }
    }

    private void Control_TransferTab_Button_Toggle_RightPanel_Click(object sender, EventArgs e)
    {
        if (MainFormInstance != null)
        {
            var panelCollapsed = MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed;
            MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = !panelCollapsed;
            Control_TransferTab_Button_Toggle_RightPanel.Text = panelCollapsed ? "→" : "←";
            Control_TransferTab_Button_Toggle_RightPanel.ForeColor = panelCollapsed ? Color.Green : Color.Red;
        }

        Helper_ComboBoxes.DeselectAllComboBoxText(this);
    }

    #endregion
}

#endregion