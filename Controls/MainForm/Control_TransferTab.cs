using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.MainForm.Classes;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.MainForm
{
    #region ControlTransferTab

    public partial class Control_TransferTab : UserControl
    {
        #region Fields

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

        // Cache ToolTip to avoid repeated instantiation
        private static readonly ToolTip SharedToolTip = new();

        #endregion

        #region Initialization

        public Control_TransferTab()
        {
            InitializeComponent();

            // Apply comprehensive DPI scaling and runtime layout adjustments
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

            Control_TransferTab_Initialize();
            ApplyPrivileges();
            Color errorColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            Control_TransferTab_ComboBox_Part.ForeColor = errorColor;
            Control_TransferTab_ComboBox_Operation.ForeColor = errorColor;
            Control_TransferTab_ComboBox_ToLocation.ForeColor = errorColor;
            Control_TransferTab_Image_NothingFound.Visible = false;

            // Use cached ToolTip
            SharedToolTip.SetToolTip(Control_TransferTab_Button_Search,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Transfer_Search)}");
            SharedToolTip.SetToolTip(Control_TransferTab_Button_Transfer,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Transfer_Transfer)}");
            SharedToolTip.SetToolTip(Control_TransferTab_Button_Reset,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Transfer_Reset)}");
            SharedToolTip.SetToolTip(Control_TransferTab_Button_Toggle_RightPanel,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Left)}/{Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Right)}");

            // Setup Print button event and tooltip directly
            Control_TransferTab_Button_Print.Click -= Control_TransferTab_Button_Print_Click;
            Control_TransferTab_Button_Print.Click += Control_TransferTab_Button_Print_Click;
            SharedToolTip.SetToolTip(Control_TransferTab_Button_Print, "Print the current results");

            _ = Control_TransferTab_OnStartup_LoadComboBoxesAsync();
            Control_TransferTab_Update_ButtonStates();
        }

        #endregion

        #region Methods

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
                {
                    if (Control_TransferTab_Button_Search.Visible && Control_TransferTab_Button_Search.Enabled)
                    {
                        Control_TransferTab_Button_Search.PerformClick();
                        return true;
                    }
                }

                if (keyData == Core_WipAppVariables.Shortcut_Transfer_Transfer)
                {
                    if (Control_TransferTab_Button_Transfer.Visible && Control_TransferTab_Button_Transfer.Enabled)
                    {
                        Control_TransferTab_Button_Transfer.PerformClick();
                        return true;
                    }
                }

                if (keyData == Core_WipAppVariables.Shortcut_Transfer_Reset)
                {
                    if (Control_TransferTab_Button_Reset.Visible && Control_TransferTab_Button_Reset.Enabled)
                    {
                        Control_TransferTab_Button_Reset.PerformClick();
                        return true;
                    }
                }

                if (keyData == Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Right)
                {
                    if (Control_TransferTab_Button_Toggle_RightPanel.Visible &&
                        Control_TransferTab_Button_Toggle_RightPanel.Enabled)
                    {
                        Control_TransferTab_Button_Toggle_RightPanel.PerformClick();
                        return true;
                    }
                }

                if (keyData == Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Left)
                {
                    if (Control_TransferTab_Button_Toggle_RightPanel.Visible &&
                        Control_TransferTab_Button_Toggle_RightPanel.Enabled)
                    {
                        Control_TransferTab_Button_Toggle_RightPanel.PerformClick();
                        return true;
                    }
                }

                if (keyData == Keys.Enter)
                {
                    SelectNextControl(ActiveControl, true, true, true, true);
                    return true;
                }

                if (MainFormInstance != null)
                {
                    bool panelCollapsed = MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed;
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
            // Show progress bar at the start
            MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
            MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Resetting Transfer tab...");

            Control_TransferTab_Button_Reset.Enabled = false;
            try
            {
                if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    await Control_TransferTab_HardReset();
                }
                else
                {
                    Control_TransferTab_SoftReset();
                }

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(100, "Reset complete");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in TransferTab Reset: {ex}");
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    new StringBuilder().Append("MainForm_Transfer_Button_Reset_Click").ToString());
            }
            finally
            {
                MainFormInstance?.TabLoadingControlProgress?.HideProgress();
            }
        }

        private async Task Control_TransferTab_HardReset()
        {
            Control_TransferTab_Button_Reset.Enabled = false;
            try
            {
                MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Resetting Transfer tab...");
                Debug.WriteLine("[DEBUG] TransferTab HardReset - start");
                if (MainFormInstance != null)
                {
                    Debug.WriteLine("[DEBUG] Updating status strip for hard reset");
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
                }

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(30, "Resetting data tables...");
                Debug.WriteLine("[DEBUG] Hiding ComboBoxes");

                Debug.WriteLine("[DEBUG] Resetting and refreshing all ComboBox DataTables");
                await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
                Debug.WriteLine("[DEBUG] DataTables reset complete");

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(60, "Refilling combo boxes...");
                Debug.WriteLine("[DEBUG] Refilling Part ComboBox");
                await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_TransferTab_ComboBox_Part);
                Debug.WriteLine("[DEBUG] Refilling Operation ComboBox");
                await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(Control_TransferTab_ComboBox_Operation);
                Debug.WriteLine("[DEBUG] Refilling ToLocation ComboBox");
                await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(Control_TransferTab_ComboBox_ToLocation);

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
                    Control_TransferTab_NumericUpDown_Quantity.Value =
                        Control_TransferTab_NumericUpDown_Quantity.Minimum;
                    Control_TransferTab_NumericUpDown_Quantity.Enabled = false;
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                        @"Disconnected from Server, please standby...";
                    MainFormInstance.TabLoadingControlProgress?.HideProgress();
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

                Debug.WriteLine("[DEBUG] Resetting UI fields");
                MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_Part,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_Operation,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(Control_TransferTab_ComboBox_ToLocation,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);

                Control_TransferTab_ComboBox_Part.ForeColor = Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                Control_TransferTab_ComboBox_Operation.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                Control_TransferTab_ComboBox_ToLocation.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;

                Control_TransferTab_DataGridView_Main.DataSource = null;
                Control_TransferTab_DataGridView_Main.Refresh();
                Control_TransferTab_Image_NothingFound.Visible = false;

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
                // Show progress bar at the start
                MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Searching inventory...");

                LoggingUtility.Log("TransferTab Search button clicked.");
                string partId = Control_TransferTab_ComboBox_Part.Text;
                string op = Control_TransferTab_ComboBox_Operation.Text;
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
                    MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(40,
                        "Querying by part and operation...");
                    results = await Dao_Inventory.GetInventoryByPartIdAndOperationAsync(partId, op, true);
                }
                else
                {
                    LoggingUtility.Log($"Searching inventory for Part ID: {partId} without specific operation.");
                    MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(40, "Querying by part...");
                    results = await Dao_Inventory.GetInventoryByPartIdAsync(partId, true);
                }

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(70, "Updating results...");
                DataGridView? dgv = Control_TransferTab_DataGridView_Main;
                dgv.SuspendLayout();
                dgv.DataSource = results;
                // Only show columns in this order: Location, PartID, Operation, Quantity, Notes
                string[] columnsToShowArr = { "Location", "PartID", "Operation", "Quantity", "Notes" };
                HashSet<string> columnsToShow = new(columnsToShowArr);
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    column.Visible = columnsToShow.Contains(column.Name);
                }

                // Reorder columns only if needed
                for (int i = 0; i < columnsToShowArr.Length; i++)
                {
                    string colName = columnsToShowArr[i];
                    if (dgv.Columns.Contains(colName) && dgv.Columns[colName].DisplayIndex != i)
                    {
                        dgv.Columns[colName].DisplayIndex = i;
                    }
                }

                Control_TransferTab_Image_NothingFound.Visible = results.Rows.Count == 0;
                if (results.Rows.Count > 0)
                {
                    Core_Themes.ApplyThemeToDataGridView(dgv);
                    Core_Themes.SizeDataGrid(dgv);
                    if (dgv.Rows.Count > 0)
                    {
                        DataGridViewRow firstRow = dgv.Rows[0];
                        if (!firstRow.Selected)
                        {
                            dgv.ClearSelection();
                            firstRow.Selected = true;
                        }
                    }
                }

                dgv.ResumeLayout();
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(100, "Search complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_TransferTab_Button_Search_Click").ToString());
            }
            finally
            {
                MainFormInstance?.TabLoadingControlProgress?.HideProgress();
            }
        }

        private async Task Control_TransferTab_Button_Save_ClickAsync(object? sender, EventArgs? e)
        {
            try
            {
                // Show progress bar at the start
                MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Transferring inventory...");

                DataGridViewSelectedRowCollection selectedRows = Control_TransferTab_DataGridView_Main.SelectedRows;
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

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(40, "Processing transfer...");

                if (selectedRows.Count == 1)
                {
                    await TransferSingleRowAsync(selectedRows[0]);
                }
                else
                {
                    await TransferMultipleRowsAsync(selectedRows);
                }

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(80, "Refreshing results...");
                Control_TransferTab_Button_Search_Click(null, null);

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(100, "Transfer complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_TransferTab_Button_Transfer_Click").ToString());
            }
            finally
            {
                MainFormInstance?.TabLoadingControlProgress?.HideProgress();
            }
        }

        private void Control_TransferTab_Button_Print_Click(object? sender, EventArgs? e)
        {
            // Show progress bar at the start
            MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
            MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Preparing print...");

            try
            {
                if (Control_TransferTab_DataGridView_Main.Rows.Count == 0)
                {
                    MessageBox.Show("No data to print.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Get visible column names for print
                List<string> visibleColumns = new();
                foreach (DataGridViewColumn col in Control_TransferTab_DataGridView_Main.Columns)
                {
                    if (col.Visible)
                        visibleColumns.Add(col.Name);
                }

                Core_DgvPrinter printer = new();
                Control_TransferTab_DataGridView_Main.Tag = Control_TransferTab_ComboBox_Part.Text;
                // Set visible columns for print
                printer.SetPrintVisibleColumns(visibleColumns);
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(60, "Printing...");
                printer.Print(Control_TransferTab_DataGridView_Main);
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(100, "Print complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($"Print failed: {ex.Message}", "Print Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                MainFormInstance?.TabLoadingControlProgress?.HideProgress();
            }
        }

        #endregion

        #region Transfer Logic

        private async Task TransferSingleRowAsync(DataGridViewRow row)
        {
            if (row.DataBoundItem is not DataRowView drv)
            {
                return;
            }

            string batchNumber = drv["BatchNumber"]?.ToString() ?? "";
            string partId = drv["PartID"]?.ToString() ?? "";
            string fromLocation = drv["Location"]?.ToString() ?? "";
            string itemType = drv.Row.Table.Columns.Contains("ItemType") ? drv["ItemType"]?.ToString() ?? "" : "";
            string notes = drv["Notes"]?.ToString() ?? "";
            string operation = drv["Operation"]?.ToString() ?? "";
            string quantityStr = drv["Quantity"]?.ToString() ?? "";
            if (!int.TryParse(quantityStr, out int originalQuantity))
            {
                LoggingUtility.LogApplicationError(
                    new Exception(
                        $"Invalid quantity value: '{quantityStr}' for PartID={partId}, Location={fromLocation}"));
                return;
            }

            int transferQuantity = Math.Min((int)Control_TransferTab_NumericUpDown_Quantity.Value, originalQuantity);
            string newLocation = Control_TransferTab_ComboBox_ToLocation.Text;
            string user = Model_AppVariables.User ?? Environment.UserName;
            if (transferQuantity < originalQuantity)
            {
                await Dao_Inventory.TransferInventoryQuantityAsync(
                    batchNumber, partId, operation, transferQuantity, originalQuantity, newLocation, user);
            }
            else
            {
                await Dao_Inventory.TransferPartSimpleAsync(
                    batchNumber, partId, operation, quantityStr, newLocation);
            }

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
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                    $@"Last Transfer: {partId} (Op: {operation}), From: {fromLocation} To: {newLocation}, Qty: {transferQuantity} @ {DateTime.Now:hh:mm tt}";
            }
        }

        private async Task TransferMultipleRowsAsync(DataGridViewSelectedRowCollection selectedRows)
        {
            string newLocation = Control_TransferTab_ComboBox_ToLocation.Text;
            string user = Model_AppVariables.User ?? Environment.UserName;
            HashSet<string> partIds = new();
            HashSet<string> operations = new();
            HashSet<string> fromLocations = new();
            int totalQty = 0;
            foreach (DataGridViewRow row in selectedRows)
            {
                if (row.DataBoundItem is not DataRowView drv)
                {
                    continue;
                }

                string batchNumber = drv["BatchNumber"]?.ToString() ?? "";
                string partId = drv["PartID"]?.ToString() ?? "";
                string fromLocation = drv["Location"]?.ToString() ?? "";
                string itemType = drv.Row.Table.Columns.Contains("ItemType") ? drv["ItemType"]?.ToString() ?? "" : "";
                string operation = drv["Operation"]?.ToString() ?? "";
                string quantityStr = drv["Quantity"]?.ToString() ?? "";
                string notes = drv["Notes"]?.ToString() ?? "";
                if (!int.TryParse(quantityStr, out int originalQuantity))
                {
                    LoggingUtility.LogApplicationError(new Exception(
                        $"Invalid quantity value: '{quantityStr}' for PartID={partId}, Location={fromLocation}"));
                    continue;
                }

                int transferQuantity =
                    Math.Min((int)Control_TransferTab_NumericUpDown_Quantity.Value, originalQuantity);
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

            if (MainFormInstance != null)
            {
                string time = DateTime.Now.ToString("hh:mm tt");
                string fromLocDisplay = fromLocations.Count > 1
                    ? "Multiple Locations"
                    : fromLocations.FirstOrDefault() ?? "";
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
                    string qtyDisplay = partIds.Count == 1 ? totalQty.ToString() : "Multiple";
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $@"Last Transfer: Multiple Part ID's, From: {fromLocDisplay} To: {newLocation}, Qty: {qtyDisplay} @ {time}";
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
                    SetComboBoxForeColor(Control_TransferTab_ComboBox_Operation, true);
                    Model_AppVariables.Operation = Control_TransferTab_ComboBox_Operation.Text;
                }
                else
                {
                    SetComboBoxForeColor(Control_TransferTab_ComboBox_Operation, false);
                    if (Control_TransferTab_ComboBox_Operation.SelectedIndex != 0 &&
                        Control_TransferTab_ComboBox_Operation.Items.Count > 0)
                    {
                        Control_TransferTab_ComboBox_Operation.SelectedIndex = 0;
                    }

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
                    SetComboBoxForeColor(Control_TransferTab_ComboBox_Part, true);
                    Model_AppVariables.PartId = Control_TransferTab_ComboBox_Part.Text;
                }
                else
                {
                    SetComboBoxForeColor(Control_TransferTab_ComboBox_Part, false);
                    if (Control_TransferTab_ComboBox_Part.SelectedIndex != 0 &&
                        Control_TransferTab_ComboBox_Part.Items.Count > 0)
                    {
                        Control_TransferTab_ComboBox_Part.SelectedIndex = 0;
                    }

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
                bool hasData = Control_TransferTab_DataGridView_Main.Rows.Count > 0;
                bool hasSelection = Control_TransferTab_DataGridView_Main.SelectedRows.Count > 0;
                bool hasToLocation = Control_TransferTab_ComboBox_ToLocation.SelectedIndex > 0 &&
                                     !string.IsNullOrWhiteSpace(Control_TransferTab_ComboBox_ToLocation.Text);
                bool hasPart = Control_TransferTab_ComboBox_Part.SelectedIndex > 0;
                bool hasQuantity = Control_TransferTab_NumericUpDown_Quantity.Value > 0;

                Control_TransferTab_ComboBox_ToLocation.Enabled = hasData;
                Control_TransferTab_NumericUpDown_Quantity.Enabled =
                    hasData && Control_TransferTab_DataGridView_Main.SelectedRows.Count <= 1;

                bool toLocationIsSameAsRow = false;
                if (hasSelection && hasToLocation)
                {
                    foreach (DataGridViewRow row in Control_TransferTab_DataGridView_Main.SelectedRows)
                    {
                        if (row.DataBoundItem is DataRowView drv)
                        {
                            string rowLocation = drv["Location"]?.ToString() ?? string.Empty;
                            if (string.Equals(rowLocation, Control_TransferTab_ComboBox_ToLocation.Text,
                                    StringComparison.OrdinalIgnoreCase))
                            {
                                toLocationIsSameAsRow = true;
                                break;
                            }
                        }
                    }
                }

                Control_TransferTab_Button_Transfer.Enabled =
                    hasData && hasSelection && hasToLocation && hasPart && hasQuantity && !toLocationIsSameAsRow;
                // Print button enable/disable
                if (Control_TransferTab_Button_Print != null)
                {
                    Control_TransferTab_Button_Print.Enabled = hasData;
                }
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
                // Use lambda to match EventHandler signature for async void method
                Control_TransferTab_Button_Reset.Click += (s, e) => Control_TransferTab_Button_Reset_Click();

                // Helper for validation and state update
                void ValidateAndUpdate(ComboBox combo, string placeholder)
                {
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(combo, placeholder);
                    Control_TransferTab_Update_ButtonStates();
                }

                // PART ComboBox
                Control_TransferTab_ComboBox_Part.SelectedIndexChanged += (s, e) =>
                {
                    Control_TransferTab_ComboBox_Part_SelectedIndexChanged();
                    ValidateAndUpdate(Control_TransferTab_ComboBox_Part, "[ Enter Part Number ]");
                };
                Control_TransferTab_ComboBox_Part.Leave += (s, e) =>
                {
                    Control_TransferTab_ComboBox_Part.BackColor =
                        Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_Part,
                        "[ Enter Part Number ]");
                };
                Control_TransferTab_ComboBox_Part.Enter += (s, e) =>
                {
                    Control_TransferTab_ComboBox_Part.BackColor =
                        Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
                };

                // OPERATION ComboBox
                Control_TransferTab_ComboBox_Operation.SelectedIndexChanged += (s, e) =>
                {
                    Control_TransferTab_ComboBox_Operation_SelectedIndexChanged();
                    ValidateAndUpdate(Control_TransferTab_ComboBox_Operation, "[ Enter Operation ]");
                };
                Control_TransferTab_ComboBox_Operation.Leave += (s, e) =>
                {
                    Control_TransferTab_ComboBox_Operation.BackColor =
                        Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_Operation,
                        "[ Enter Operation ]");
                };
                Control_TransferTab_ComboBox_Operation.Enter += (s, e) =>
                {
                    Control_TransferTab_ComboBox_Operation.BackColor =
                        Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
                };

                // TO LOCATION ComboBox
                Control_TransferTab_ComboBox_ToLocation.SelectedIndexChanged += (s, e) =>
                {
                    ValidateAndUpdate(Control_TransferTab_ComboBox_ToLocation, "[ Enter Location ]");
                };
                Control_TransferTab_ComboBox_ToLocation.Leave += (s, e) =>
                {
                    Control_TransferTab_ComboBox_ToLocation.BackColor =
                        Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_TransferTab_ComboBox_ToLocation,
                        "[ Enter Location ]");
                };
                Control_TransferTab_ComboBox_ToLocation.Enter += (s, e) =>
                {
                    Control_TransferTab_ComboBox_ToLocation.BackColor =
                        Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
                };

                // NumericUpDown
                Control_TransferTab_NumericUpDown_Quantity.ValueChanged +=
                    (s, e) => Control_TransferTab_Update_ButtonStates();

                // DataGridView
                Control_TransferTab_DataGridView_Main.SelectionChanged +=
                    (s, e) => Control_TransferTab_Update_ButtonStates();
                Control_TransferTab_DataGridView_Main.SelectionChanged +=
                    Control_TransferTab_DataGridView_Main_SelectionChanged;
                Control_TransferTab_DataGridView_Main.DataSourceChanged +=
                    (s, e) => Control_TransferTab_Update_ButtonStates();

                // Transfer button
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
                    DataGridViewRow row = Control_TransferTab_DataGridView_Main.SelectedRows[0];
                    if (row.DataBoundItem is DataRowView drv && int.TryParse(drv["Quantity"]?.ToString(), out int qty))
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
                bool panelCollapsed = MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed;
                MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = !panelCollapsed;
                Control_TransferTab_Button_Toggle_RightPanel.Text = panelCollapsed ? "→" : "←";
                Control_TransferTab_Button_Toggle_RightPanel.ForeColor = panelCollapsed
                    ? Model_AppVariables.UserUiColors.SuccessColor ?? Color.Green
                    : Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
            }

            Helper_UI_ComboBoxes.DeselectAllComboBoxText(this);
        }

        #endregion

        #region Privileges

        private void ApplyPrivileges()
        {
            bool isAdmin = Model_AppVariables.UserTypeAdmin;
            bool isNormal = Model_AppVariables.UserTypeNormal;
            bool isReadOnly = Model_AppVariables.UserTypeReadOnly;

            // ComboBoxes
            Control_TransferTab_ComboBox_Part.Enabled = isAdmin || isNormal || isReadOnly;
            Control_TransferTab_ComboBox_Operation.Enabled = isAdmin || isNormal || isReadOnly;
            Control_TransferTab_ComboBox_ToLocation.Enabled = isAdmin || isNormal || isReadOnly;
            // NumericUpDown
            Control_TransferTab_NumericUpDown_Quantity.ReadOnly = isReadOnly;
            Control_TransferTab_NumericUpDown_Quantity.Enabled = isAdmin || isNormal || isReadOnly;
            // DataGridView
            Control_TransferTab_DataGridView_Main.ReadOnly = isReadOnly;
            Control_TransferTab_DataGridView_Main.Enabled = isAdmin || isNormal || isReadOnly;
            // Buttons
            Control_TransferTab_Button_Transfer.Visible = isAdmin || isNormal;
            Control_TransferTab_Button_Transfer.Enabled = isAdmin || isNormal;
            Control_TransferTab_Button_Reset.Visible = true;
            Control_TransferTab_Button_Reset.Enabled = true;
            Control_TransferTab_Button_Search.Visible = true;
            Control_TransferTab_Button_Search.Enabled = true;
            Control_TransferTab_Button_Toggle_RightPanel.Visible = true;
            Control_TransferTab_Button_Toggle_RightPanel.Enabled = true;
            // Panels, labels, images, etc. are always visible
            // If you add more, follow the same pattern

            // For Read-Only, hide Transfer button
            if (isReadOnly)
            {
                Control_TransferTab_Button_Transfer.Visible = false;
                Control_TransferTab_Button_Transfer.Enabled = false;
            }
            // TODO: If there are TreeView branches, set their .Visible property here as well.
        }

        #endregion

        #region Helper Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetComboBoxForeColor(ComboBox combo, bool valid) =>
            combo.ForeColor = valid
                ? Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black
                : Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;

        #endregion

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
        }

        private void Control_TransferTab_Button_Toggle_Split_Click(object sender, EventArgs e)
        {
            SplitContainer? splitContainer = Control_TransferTab_SplitContainer_Main;
            Button? button = sender as Button ?? Control_TransferTab_Button_Toggle_Split;

            if (splitContainer.Panel1Collapsed)
            {
                splitContainer.Panel1Collapsed = false;
                button.Text = "Collapse ◀";
            }
            else
            {
                splitContainer.Panel1Collapsed = true;
                button.Text = "Expand ▶";
            }
        }
    }

    #endregion
}
