using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.MainForm.Classes;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.MainForm;

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
        Control_TransferTab_ComboBox_Part.ForeColor =
            Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        Control_TransferTab_ComboBox_Operation.ForeColor =
            Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        Control_TransferTab_ComboBox_ToLocation.ForeColor =
            Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        Control_TransferTab_Image_NothingFound.Visible = false;

        // Set tooltips for Transfer tab buttons using shortcut constants
        var toolTip = new ToolTip();
        toolTip.SetToolTip(Control_TransferTab_Button_Search,
            $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Transfer_Search)}");
        toolTip.SetToolTip(Control_TransferTab_Button_Transfer,
            $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Transfer_Transfer)}");
        toolTip.SetToolTip(Control_TransferTab_Button_Reset,
            $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Transfer_Reset)}");
        toolTip.SetToolTip(Control_TransferTab_Button_Toggle_RightPanel,
            $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Left)}/{Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Right)}");

        _ = Control_TransferTab_OnStartup_LoadComboBoxesAsync();
    }

    public void Control_TransferTab_Initialize()
    {
        Control_TransferTab_Button_Reset.TabStop = false;

        Core_Themes.ApplyFocusHighlighting(this);
    }

    #endregion

    #region Key Processing

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        try
        {
            if (keyData == Core_WipAppVariables.Shortcut_Transfer_Search)
                if (Control_TransferTab_Button_Search.Visible && Control_TransferTab_Button_Search.Enabled)
                {
                    Control_TransferTab_Button_Search.PerformClick();
                    return true;
                }

            if (keyData == Core_WipAppVariables.Shortcut_Transfer_Transfer)
                if (Control_TransferTab_Button_Transfer.Visible && Control_TransferTab_Button_Transfer.Enabled)
                {
                    Control_TransferTab_Button_Transfer.PerformClick();
                    return true;
                }

            if (keyData == Core_WipAppVariables.Shortcut_Transfer_Reset)
                if (Control_TransferTab_Button_Reset.Visible && Control_TransferTab_Button_Reset.Enabled)
                {
                    Control_TransferTab_Button_Reset.PerformClick();
                    return true;
                }

            if (keyData == Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Right)
                if (Control_TransferTab_Button_Toggle_RightPanel.Visible &&
                    Control_TransferTab_Button_Toggle_RightPanel.Enabled)
                {
                    Control_TransferTab_Button_Toggle_RightPanel.PerformClick();
                    return true;
                }

            if (keyData == Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Left)
                if (Control_TransferTab_Button_Toggle_RightPanel.Visible &&
                    Control_TransferTab_Button_Toggle_RightPanel.Enabled)
                {
                    Control_TransferTab_Button_Toggle_RightPanel.PerformClick();
                    return true;
                }

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
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_ProcessCmdKey").ToString());
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
            LoggingUtility.Log("Initial setup of ComboBoxes in the Inventory Tab.");
            Control_TransferTab_Button_Transfer.Enabled = false;
            Control_TransferTab_Button_Search.Enabled = false;
            Control_TransferTab_ComboBox_Operation.Visible = true;
            Control_TransferTab_ComboBox_Part.Visible = true;
            try
            {
                Model_AppVariables.UserFullName =
                    await Dao_User.GetUserFullNameAsync(Model_AppVariables.User, true);
                LoggingUtility.Log($"User full name loaded: {Model_AppVariables.UserFullName}");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_TransferTab_OnStartup_GetUserFullName").ToString());
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("Control_TransferTab_OnStartup").ToString());
        }
    }

    public async Task Control_TransferTab_OnStartup_LoadDataComboBoxesAsync()
    {
        try
        {
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_TransferTab_ComboBox_Part);
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(Control_TransferTab_ComboBox_Operation);
            await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(Control_TransferTab_ComboBox_ToLocation);
            LoggingUtility.Log("Transfer tab ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("MainForm_LoadTransferTabComboBoxesAsync").ToString());
        }
    }

    #endregion

    #region Button Clicks

    private async void Control_TransferTab_Button_Reset_Click()
    {
        Control_TransferTab_Button_Reset.Enabled = false;
        try
        {
            // Check if Shift key is held down
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                await Control_TransferTab_HardReset();
            else
                Control_TransferTab_SoftReset();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in TransferTab Reset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Transfer_Button_Reset_Click").ToString());
        }
    }

    private async Task Control_TransferTab_HardReset()
    {
        Control_TransferTab_Button_Reset.Enabled = false;
        try
        {
            Debug.WriteLine("[DEBUG] TransferTab HardReset - start");
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Updating status strip for hard reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            // Hide controls during reset
            Debug.WriteLine("[DEBUG] Hiding ComboBoxes");
            Control_TransferTab_ComboBox_Part.Visible = false;
            Control_TransferTab_ComboBox_Operation.Visible = false;
            Control_TransferTab_ComboBox_ToLocation.Visible = false;

            // Reset the DataTables and reinitialize them
            Debug.WriteLine("[DEBUG] Resetting and refreshing all ComboBox DataTables");
            await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
            Debug.WriteLine("[DEBUG] DataTables reset complete");

            // Refill each combobox with proper data
            Debug.WriteLine("[DEBUG] Refilling Part ComboBox");
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_TransferTab_ComboBox_Part);
            Debug.WriteLine("[DEBUG] Refilling Operation ComboBox");
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(Control_TransferTab_ComboBox_Operation);
            Debug.WriteLine("[DEBUG] Refilling ToLocation ComboBox");
            await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(Control_TransferTab_ComboBox_ToLocation);

            // Reset UI fields
            Debug.WriteLine("[DEBUG] Resetting UI fields");
            MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_Operation,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_ToLocation,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);

            Control_TransferTab_DataGridView_Main.DataSource = null;
            Control_TransferTab_DataGridView_Main.Refresh();
            Control_TransferTab_Image_NothingFound.Visible = false;

            // Restore controls and focus
            Debug.WriteLine("[DEBUG] Restoring ComboBox visibility and focus");
            Control_TransferTab_ComboBox_Operation.Visible = true;
            Control_TransferTab_ComboBox_Part.Visible = true;
            Control_TransferTab_ComboBox_ToLocation.Visible = true;
            Control_TransferTab_ComboBox_Part.Focus();

            Debug.WriteLine("[DEBUG] TransferTab HardReset - end");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in TransferTab HardReset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Transfer_HardReset").ToString());
        }
        finally
        {
            Debug.WriteLine("[DEBUG] TransferTab HardReset button re-enabled");
            Control_TransferTab_Button_Reset.Enabled = true;
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Resetting TransferTab buttons and status strip");
                MainFormTabResetHelper.ResetTransferTab(
                    Control_TransferTab_ComboBox_Part,
                    Control_TransferTab_ComboBox_Operation,
                    Control_TransferTab_Button_Search,
                    Control_TransferTab_Button_Transfer);
                Control_TransferTab_NumericUpDown_Quantity.Value = Control_TransferTab_NumericUpDown_Quantity.Minimum;
                Control_TransferTab_NumericUpDown_Quantity.Enabled = false;
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
            }
        }
    }

    private void Control_TransferTab_SoftReset()
    {
        Control_TransferTab_Button_Reset.Enabled = false;
        try
        {
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Updating status strip for Soft Reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            // Reset UI fields
            Debug.WriteLine("[DEBUG] Resetting UI fields");
            MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_Operation,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_ToLocation,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);

            Control_TransferTab_DataGridView_Main.DataSource = null;
            Control_TransferTab_DataGridView_Main.Refresh();
            Control_TransferTab_Image_NothingFound.Visible = false;

            // Update button states
            Control_TransferTab_Button_Search.Enabled = false;
            Control_TransferTab_Button_Transfer.Enabled = false;
            Control_TransferTab_NumericUpDown_Quantity.Value = Control_TransferTab_NumericUpDown_Quantity.Minimum;
            Control_TransferTab_NumericUpDown_Quantity.Enabled = false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in TransferTab SoftReset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Transfer_SoftReset").ToString());
        }
        finally
        {
            Debug.WriteLine("[DEBUG] TransferTab SoftReset button re-enabled");
            Control_TransferTab_Button_Reset.Enabled = true;
            Control_TransferTab_ComboBox_Part.Focus();
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
            }
        }
    }

    private async void Control_TransferTab_Button_Search_Click(object? sender, EventArgs? e)
    {
        try
        {
            LoggingUtility.Log("TransferTab Search button clicked.");
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
                Control_TransferTab_ComboBox_Operation.Text != @"[ Enter Operation ]")
            {
                LoggingUtility.Log($"Searching inventory for Part ID: {partId} and Operation: {op}");
                results = await Dao_Inventory.GetInventoryByPartIdAndOperationAsync(partId, op, true);
            }
            else
            {
                LoggingUtility.Log($"Searching inventory for Part ID: {partId} without specific operation.");
                results = await Dao_Inventory.GetInventoryByPartIdAsync(partId, true);
            }

            Control_TransferTab_DataGridView_Main.DataSource = results;
            Control_TransferTab_DataGridView_Main.ClearSelection();
            foreach (DataGridViewColumn column in Control_TransferTab_DataGridView_Main.Columns)
                column.Visible = true;
            Core_Themes.ApplyThemeToDataGridView(Control_TransferTab_DataGridView_Main);
            Core_Themes.SizeDataGrid(Control_TransferTab_DataGridView_Main);
            Control_TransferTab_Image_NothingFound.Visible = results.Rows.Count == 0;
            if (Control_TransferTab_DataGridView_Main.Rows.Count > 0)
            {
                Control_TransferTab_DataGridView_Main.ClearSelection();
                Control_TransferTab_DataGridView_Main.Rows[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("Control_TransferTab_Button_Search_Click").ToString());
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
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("Control_TransferTab_Button_Transfer_Click").ToString());
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
        if (!int.TryParse(quantityStr, out var originalQuantity))
        {
            LoggingUtility.LogApplicationError(
                new Exception($"Invalid quantity value: '{quantityStr}' for PartID={partId}, Location={fromLocation}"));
            return;
        }

        var transferQuantity = Math.Min((int)Control_TransferTab_NumericUpDown_Quantity.Value, originalQuantity);
        var newLocation = Control_TransferTab_ComboBox_ToLocation.Text;
        var user = Model_AppVariables.User ?? Environment.UserName;
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
        // Update status strip
        if (MainFormInstance != null)
            MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                $@"Last Transfer: {partId} (Op: {operation}), From: {fromLocation} To: {newLocation}, Qty: {transferQuantity} @ {DateTime.Now:hh:mm tt}";
    }

    private async Task TransferMultipleRowsAsync(DataGridViewSelectedRowCollection selectedRows)
    {
        var newLocation = Control_TransferTab_ComboBox_ToLocation.Text;
        var user = Model_AppVariables.User ?? Environment.UserName;
        var partIds = new HashSet<string>();
        var operations = new HashSet<string>();
        var fromLocations = new HashSet<string>();
        var totalQty = 0;
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
            if (!int.TryParse(quantityStr, out var originalQuantity))
            {
                LoggingUtility.LogApplicationError(new Exception(
                    $"Invalid quantity value: '{quantityStr}' for PartID={partId}, Location={fromLocation}"));
                continue;
            }

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
            partIds.Add(partId);
            operations.Add(operation);
            fromLocations.Add(fromLocation);
            totalQty += transferQuantity;
        }

        // Update status strip for the last transfer
        if (MainFormInstance != null)
        {
            var time = DateTime.Now.ToString("hh:mm tt");
            var fromLocDisplay = fromLocations.Count > 1 ? "Multiple Locations" : fromLocations.FirstOrDefault() ?? "";
            if (partIds.Count == 1 && operations.Count == 1)
            {
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                    $@"Last Transfer: {partIds.First()} (Op: {operations.First()}), From: {fromLocDisplay} To: {newLocation}, Qty: {totalQty} @ {time}";
            }
            else if (partIds.Count == 1 && operations.Count > 1)
            {
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                    $@"Last Transfer: {partIds.First()} (Multiple Ops), From: {fromLocDisplay} To: {newLocation}, Qty: {totalQty} @ {time}";
            }
            else
            {
                var qtyDisplay = partIds.Count == 1 ? totalQty.ToString() : "Multiple";
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                    $@"Last Transfer: Multiple Part IDs, From: {fromLocDisplay} To: {newLocation}, Qty: {qtyDisplay} @ {time}";
            }
        }
    }

    #endregion

    #region ComboBox & UI Events

    private void Control_TransferTab_ComboBox_Operation_SelectedIndexChanged()
    {
        try
        {
            LoggingUtility.Log("Inventory Op ComboBox selection changed.");
            if (Control_TransferTab_ComboBox_Operation.SelectedIndex > 0)
            {
                Control_TransferTab_ComboBox_Operation.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
                Model_AppVariables.Operation = Control_TransferTab_ComboBox_Operation.Text;
            }
            else
            {
                Control_TransferTab_ComboBox_Operation.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                if (Control_TransferTab_ComboBox_Operation.SelectedIndex != 0 &&
                    Control_TransferTab_ComboBox_Operation.Items.Count > 0)
                    Control_TransferTab_ComboBox_Operation.SelectedIndex = 0;
                Model_AppVariables.Operation = null;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Inventory_ComboBox_Op").ToString());
        }
    }

    private void Control_TransferTab_ComboBox_Part_SelectedIndexChanged()
    {
        try
        {
            LoggingUtility.Log("Inventory Part ComboBox selection changed.");
            if (Control_TransferTab_ComboBox_Part.SelectedIndex > 0)
            {
                Control_TransferTab_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
                Model_AppVariables.PartId = Control_TransferTab_ComboBox_Part.Text;
            }
            else
            {
                Control_TransferTab_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                if (Control_TransferTab_ComboBox_Part.SelectedIndex != 0 &&
                    Control_TransferTab_ComboBox_Part.Items.Count > 0)
                    Control_TransferTab_ComboBox_Part.SelectedIndex = 0;
                Model_AppVariables.PartId = null;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Inventory_ComboBox_Part").ToString());
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
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("Control_TransferTab_Update_ButtonStates").ToString());
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
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_Part, "[ Enter Part Number ]");
                Control_TransferTab_Update_ButtonStates();
            };
            Control_TransferTab_ComboBox_Part.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_Part,
                    "[ Enter Part Number ]");
            };

            Control_TransferTab_ComboBox_Operation.SelectedIndexChanged += (s, e) =>
            {
                Control_TransferTab_ComboBox_Operation_SelectedIndexChanged();
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_Operation,
                    "[ Enter Operation ]");
                Control_TransferTab_Update_ButtonStates();
            };
            Control_TransferTab_ComboBox_Operation.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_Operation,
                    "[ Enter Operation ]");
            };

            Control_TransferTab_ComboBox_ToLocation.SelectedIndexChanged +=
                (s, e) =>
                {
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_ToLocation,
                        "[ Enter Location ]");
                    Control_TransferTab_Update_ButtonStates();
                };
            Control_TransferTab_ComboBox_ToLocation.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_ToLocation,
                    "[ Enter Location ]");
            };

            Control_TransferTab_NumericUpDown_Quantity.ValueChanged +=
                (s, e) => Control_TransferTab_Update_ButtonStates();

            Control_TransferTab_ComboBox_Part.Enter += (s, e) =>
            {
                Control_TransferTab_ComboBox_Part.BackColor =
                    Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
            };
            Control_TransferTab_ComboBox_Part.Leave += (s, e) =>
            {
                Control_TransferTab_ComboBox_Part.BackColor =
                    Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_Part, "[ Enter Part Number ]");
            };

            Control_TransferTab_ComboBox_Operation.Enter += (s, e) =>
            {
                Control_TransferTab_ComboBox_Operation.BackColor =
                    Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
            };
            Control_TransferTab_ComboBox_Operation.Leave += (s, e) =>
            {
                Control_TransferTab_ComboBox_Operation.BackColor =
                    Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_Operation,
                    "[ Enter Operation ]");
            };

            Control_TransferTab_ComboBox_ToLocation.Enter += (s, e) =>
            {
                Control_TransferTab_ComboBox_ToLocation.BackColor =
                    Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
            };
            Control_TransferTab_ComboBox_ToLocation.Leave += (s, e) =>
            {
                Control_TransferTab_ComboBox_ToLocation.BackColor =
                    Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_ToLocation,
                    "[ Enter Location ]");
            };

            Control_TransferTab_DataGridView_Main.SelectionChanged +=
                (s, e) => Control_TransferTab_Update_ButtonStates();
            Control_TransferTab_DataGridView_Main.SelectionChanged +=
                Control_TransferTab_DataGridView_Main_SelectionChanged;
            Control_TransferTab_Button_Transfer.Click +=
                async (s, e) => await Control_TransferTab_Button_Save_ClickAsync(s, e);
            LoggingUtility.Log("Transfer tab events wired up.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_WireUpTransferTabEvents").ToString());
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
            LoggingUtility.LogApplicationError(ex);
        }
    }

    private void Control_TransferTab_Button_Toggle_RightPanel_Click(object sender, EventArgs e)
    {
        if (MainFormInstance != null)
        {
            var panelCollapsed = MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed;
            MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = !panelCollapsed;
            Control_TransferTab_Button_Toggle_RightPanel.Text = panelCollapsed ? "→" : "←";
            Control_TransferTab_Button_Toggle_RightPanel.ForeColor = panelCollapsed
                ? Model_AppVariables.UserUiColors.SuccessColor ?? Color.Green
                : Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
        }

        Helper_UI_ComboBoxes.DeselectAllComboBoxText(this);
    }

    #endregion
}

#endregion