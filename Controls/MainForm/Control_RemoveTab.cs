using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Text;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.MainForm.Classes;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
using static System.Int32;

namespace MTM_Inventory_Application.Controls.MainForm
{
    #region RemoveTab

    public partial class Control_RemoveTab : UserControl
    {
        #region Fields

        private readonly List<Model_HistoryRemove> _lastRemovedItems = [];
        private Helper_StoredProcedureProgress? _progressHelper;

        #endregion

        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

        #endregion

        #region Constructors

        public Control_RemoveTab()
        {
            InitializeComponent();

            // Apply comprehensive DPI scaling and runtime layout adjustments
            // THEME POLICY: Only update theme on startup, in settings menu, or on DPI change.
            // Do NOT call theme update methods from arbitrary event handlers or business logic.
            Core_Themes.ApplyDpiScaling(this); // Allowed: UserControl initialization
            Core_Themes.ApplyRuntimeLayoutAdjustments(this); // Allowed: UserControl initialization

            Control_RemoveTab_Initialize();
            Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(Control_RemoveTab_ComboBox_Part);
            Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(Control_RemoveTab_ComboBox_Operation);
            Control_RemoveTab_ComboBox_Part.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            Control_RemoveTab_ComboBox_Operation.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            Control_RemoveTab_Image_NothingFound.Visible = false;
            _ = Control_RemoveTab_OnStartup_LoadComboBoxesAsync();

            Button undoButton = new()
            {
                Name = "Control_RemoveTab_Button_Undo",
                Text = "Undo",
                Enabled = false,
                AutoSize = true,
                Anchor = AnchorStyles.Right
            };
            undoButton.Click += Control_RemoveTab_Button_Undo_Click;

            Control_RemoveTab_Button_Print.Click -= Control_RemoveTab_Button_Print_Click;
            Control_RemoveTab_Button_Print.Click += Control_RemoveTab_Button_Print_Click;
            // Tooltip for printButton intentionally omitted to avoid variable conflict


            ToolTip toolTip = new();
            toolTip.SetToolTip(Control_RemoveTab_Button_Search,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Search)}");
            toolTip.SetToolTip(Control_RemoveTab_Button_Delete,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Delete)}");
            toolTip.SetToolTip(Control_RemoveTab_Button_Reset,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Reset)}");
            ApplyPrivileges();
        }

        #endregion

        #region Methods

        #region Privileges

        private void ApplyPrivileges()
        {
            bool isAdmin = Model_AppVariables.UserTypeAdmin;
            bool isNormal = Model_AppVariables.UserTypeNormal;
            bool isReadOnly = Model_AppVariables.UserTypeReadOnly;

            // Buttons
            Control_RemoveTab_Button_AdvancedItemRemoval.Visible = isAdmin || isNormal;
            Control_RemoveTab_Button_Reset.Visible = true;
            Control_RemoveTab_Button_Delete.Visible = isAdmin || isNormal;
            Control_RemoveTab_Button_Search.Visible = true;
            Control_RemoveTab_Button_Toggle_RightPanel.Visible = true;
            Control_RemoveTab_Button_Undo.Visible = isAdmin || isNormal;


            // For Read-Only, hide buttons and disable ComboBoxes
            if (isReadOnly)
            {
                Control_RemoveTab_Button_AdvancedItemRemoval.Visible = false;
                Control_RemoveTab_Button_Delete.Visible = false;
                Control_RemoveTab_Button_Undo.Visible = false;
            }
        }

        #endregion

        #region Initialization

        private void Control_RemoveTab_Initialize() => Control_RemoveTab_Button_Reset.TabStop = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the progress controls for this tab to provide visual feedback during operations
        /// </summary>
        /// <param name="progressBar">The progress bar control from the parent form</param>
        /// <param name="statusLabel">The status label control from the parent form</param>
        public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, 
                this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
        }

        #endregion

        #region Startup / ComboBox Loading

        private async Task Control_RemoveTab_OnStartup_LoadComboBoxesAsync()
        {
            try
            {
                await Control_RemoveTab_OnStartup_LoadDataComboBoxesAsync();
                Control_RemoveTab_OnStartup_WireUpEvents();
                LoggingUtility.Log("Initial setup of ComboBoxes in the Remove Tab.");
                Control_RemoveTab_Button_Search.Enabled = false;
                Control_RemoveTab_Button_Delete.Enabled = false;

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
                        new StringBuilder().Append("Control_RemoveTab_OnStartup_GetUserFullName").ToString());
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_RemoveTab_OnStartup").ToString());
            }
        }

        public async Task Control_RemoveTab_OnStartup_LoadDataComboBoxesAsync()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_RemoveTab_ComboBox_Part);
                await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(Control_RemoveTab_ComboBox_Operation);
                LoggingUtility.Log("Remove tab ComboBoxes loaded.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("MainForm_LoadRemoveTabComboBoxesAsync").ToString());
            }
        }

        #endregion

        #region Key Processing

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

                if (keyData == Keys.Delete)
                {
                    Control_RemoveTab_Button_Delete.PerformClick();
                    return true;
                }

                if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed &&
                    keyData == (Keys.Alt | Keys.Right))
                {
                    Control_RemoveTab_Button_Toggle_RightPanel.PerformClick();
                    return true;
                }

                if (MainFormInstance != null && MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed &&
                    keyData == (Keys.Alt | Keys.Left))
                {
                    Control_RemoveTab_Button_Toggle_RightPanel.PerformClick();
                    return true;
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

        #region Button Clicks

        private async void Control_RemoveTab_Button_Delete_Click(object? sender, EventArgs? e)
        {
            try
            {
                MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Preparing to delete...");

                DataGridView? dgv = Control_RemoveTab_DataGridView_Main;
                int selectedCount = dgv.SelectedRows.Count;
                LoggingUtility.Log($"Delete clicked. Selected rows: {selectedCount}");

                if (selectedCount == 0)
                {
                    LoggingUtility.Log("No rows selected for deletion.");
                    return;
                }

                // --- Only add to history and inv_transaction if actually removed ---
                _lastRemovedItems.Clear();
                StringBuilder sb = new();
                int attempted = 0;
                var removeResult = await Dao_Inventory.RemoveInventoryItemsFromDataGridViewAsync(dgv, true);
                LoggingUtility.Log($"Remove operation result: Success={removeResult.IsSuccess}, Status={removeResult.StatusMessage}");
                
                int removedCount = 0;
                List<string> errorMessages = new();
                
                if (removeResult.IsSuccess)
                {
                    removedCount = removeResult.Data.RemovedCount;
                    errorMessages = removeResult.Data.ErrorMessages;
                }

                foreach (DataGridViewRow row in dgv.SelectedRows)
                {
                    if (row.DataBoundItem is DataRowView drv)
                    {
                        attempted++;
                        Model_HistoryRemove item = new()
                        {
                            PartId = drv["PartID"]?.ToString() ?? "",
                            Location = drv["Location"]?.ToString() ?? "",
                            Operation = drv["Operation"]?.ToString() ?? "",
                            Quantity = TryParse(drv["Quantity"]?.ToString(), out int qty) ? qty : 0,
                            ItemType =
                                drv.Row.Table.Columns.Contains("ItemType") ? drv["ItemType"]?.ToString() ?? "" : "",
                            User = drv.Row.Table.Columns.Contains("User") ? drv["User"]?.ToString() ?? "" : "",
                            BatchNumber =
                                drv.Row.Table.Columns.Contains("BatchNumber")
                                    ? drv["BatchNumber"]?.ToString() ?? ""
                                    : "",
                            Notes = drv.Row.Table.Columns.Contains("Notes") ? drv["Notes"]?.ToString() ?? "" : "",
                            ReceiveDate =
                                drv.Row.Table.Columns.Contains("ReceiveDate") &&
                                DateTime.TryParse(drv["ReceiveDate"]?.ToString(), out DateTime dt)
                                    ? dt
                                    : DateTime.Now
                        };
                        if (removedCount > 0)
                        {
                            _lastRemovedItems.Add(item);
                            sb.AppendLine(
                                $"PartID: {item.PartId}, Location: {item.Location}, Operation: {item.Operation}, Quantity: {item.Quantity}");

                            // Add to inv_transaction (OUT)
                            Model_TransactionHistory transaction = new()
                            {
                                TransactionType = "OUT",
                                PartId = item.PartId,
                                FromLocation = item.Location,
                                ToLocation = null,
                                Operation = item.Operation,
                                Quantity = item.Quantity,
                                Notes = item.Notes,
                                User = item.User,
                                ItemType = item.ItemType,
                                BatchNumber = item.BatchNumber,
                                DateTime = item.ReceiveDate
                            };
                            await Dao_History.AddTransactionHistoryAsync(transaction);
                        }
                    }
                }

                string summary = sb.ToString();

                if (_lastRemovedItems.Count == 0)
                {
                    string reason =
                        "No items were deleted. This may be because the selected items no longer exist in inventory, the data did not match exactly, or a database constraint prevented deletion.";
                    if (attempted == 0)
                    {
                        reason = "No items were deleted. No valid inventory rows were selected.";
                    }

                    if (errorMessages.Count > 0)
                    {
                        reason += "\n\n" + string.Join("\n", errorMessages);
                    }

                    MessageBox.Show(reason, @"Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult confirmResult = MessageBox.Show(
                    $@"The following items were deleted and added to history:\n\n{summary}",
                    @"Delete Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(80, "Refreshing results...");
                Control_RemoveTab_Button_Search_Click(null, null);
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(100, "Delete complete");

                // --- Enable Undo button if items were removed ---
                if (_lastRemovedItems.Count > 0)
                {
                    Control_RemoveTab_Button_Undo.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_RemoveTab_Button_Delete_Click").ToString());
            }
            finally
            {
                MainFormInstance?.TabLoadingControlProgress?.HideProgress();
            }
        }

        private async void Control_RemoveTab_Button_Undo_Click(object? sender, EventArgs? e)
        {
            if (_lastRemovedItems.Count == 0)
            {
                return;
            }

            MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
            MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Restoring removed items...");

            try
            {
                foreach (Model_HistoryRemove item in _lastRemovedItems)
                {
                    await Dao_Inventory.AddInventoryItemAsync(
                        item.PartId,
                        item.Location,
                        item.Operation,
                        item.Quantity,
                        item.ItemType,
                        item.User,
                        item.BatchNumber,
                        "Removal reversed via Undo Button.",
                        true
                    );
                }

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(80, "Refreshing results...");
                MessageBox.Show(@"Undo successful. Removed items have been restored.", @"Undo", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                LoggingUtility.Log("Undo: Removed items restored.");

                _lastRemovedItems.Clear();

                Control_RemoveTab_Button_Undo.Enabled = false;


                Control_RemoveTab_Button_Search_Click(null, null);
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(100, "Undo complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show(@"Undo failed: " + ex.Message, @"Undo Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                MainFormInstance?.TabLoadingControlProgress?.HideProgress();
            }
        }

        private void Control_RemoveTab_Button_Reset_Click()
        {
            MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
            MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Resetting Remove tab...");

            Control_RemoveTab_Button_Reset.Enabled = false;
            try
            {
                if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    _ = Control_RemoveTab_HardReset();
                }
                else
                {
                    Control_RemoveTab_SoftReset();
                }

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(100, "Reset complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    new StringBuilder().Append("MainForm_Remove_Button_Reset_Click").ToString());
            }
            finally
            {
                MainFormInstance?.TabLoadingControlProgress?.HideProgress();
            }
        }

        private async Task Control_RemoveTab_HardReset()
        {
            Control_RemoveTab_Button_Reset.Enabled = false;
            try
            {
                MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Resetting Remove tab...");

                if (MainFormInstance != null)
                {
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
                }

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(30, "Resetting data tables...");
                await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(60, "Refilling combo boxes...");
                await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_RemoveTab_ComboBox_Part);
                await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(Control_RemoveTab_ComboBox_Operation);

                Control_RemoveTab_DataGridView_Main.DataSource = null;
                Control_RemoveTab_Image_NothingFound.Visible = false;

                MainFormControlHelper.ResetComboBox(Control_RemoveTab_ComboBox_Part,
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(Control_RemoveTab_ComboBox_Operation,
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);

                Control_RemoveTab_ComboBox_Part.Focus();

                Control_RemoveTab_Button_Search.Enabled = true;
                Control_RemoveTab_Button_Delete.Enabled = false;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    new StringBuilder().Append("MainForm_Remove_HardReset").ToString());
            }
            finally
            {
                if (MainFormInstance != null)
                {
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                        @"Disconnected from Server, please standby...";
                    MainFormInstance.TabLoadingControlProgress?.HideProgress();
                }

                Control_RemoveTab_Button_Reset.Enabled = true;
            }
        }

        private void Control_RemoveTab_SoftReset()
        {
            Control_RemoveTab_Button_Reset.Enabled = false;
            try
            {
                if (MainFormInstance != null)
                {
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
                }

                MainFormControlHelper.ResetComboBox(Control_RemoveTab_ComboBox_Part,
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(Control_RemoveTab_ComboBox_Operation,
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);

                Control_RemoveTab_DataGridView_Main.DataSource = null;
                Control_RemoveTab_Image_NothingFound.Visible = false;

                Control_RemoveTab_Button_Search.Enabled = true;
                Control_RemoveTab_Button_Delete.Enabled = false;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    new StringBuilder().Append("MainForm_Remove_SoftReset").ToString());
            }
            finally
            {
                Control_RemoveTab_Button_Reset.Enabled = true;
                Control_RemoveTab_ComboBox_Part.Focus();
                if (MainFormInstance != null)
                {
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                        @"Disconnected from Server, please standby...";
                }
            }
        }

        private static void Control_RemoveTab_Button_AdvancedItemRemoval_Click()
        {
            try
            {
                if (Service_Timer_VersionChecker.MainFormInstance == null)
                {
                    LoggingUtility.Log("MainForm instance is null, cannot open Advanced Inventory Removal.");
                    return;
                }

                if (MainFormInstance != null)
                {
                    MainFormInstance.MainForm_UserControl_RemoveTab.Visible = false;
                }

                if (MainFormInstance != null)
                {
                    MainFormInstance.MainForm_UserControl_AdvancedRemove.Visible = true;
                }

                Control_AdvancedRemove? adv = MainFormInstance?.MainForm_UserControl_AdvancedRemove;
                if (adv != null)
                {
                    if (adv.Controls.Find("Control_AdvancedRemove_ComboBox_Part", true).FirstOrDefault() is ComboBox
                        part)
                    {
                        part.SelectedIndex = 0;
                        part.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                        part.Focus();
                    }

                    if (adv.Controls.Find("Control_AdvancedRemove_ComboBox_Op", true).FirstOrDefault() is ComboBox op)
                    {
                        op.SelectedIndex = 0;
                        op.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                    }

                    if (adv.Controls.Find("Control_AdvancedRemove_ComboBox_Loc", true).FirstOrDefault() is ComboBox loc)
                    {
                        loc.SelectedIndex = 0;
                        loc.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                    }

                    if (adv.Controls.Find("Control_AdvancedRemove_ComboBox_User", true).FirstOrDefault() is ComboBox
                        user)
                    {
                        user.SelectedIndex = 0;
                        user.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    new StringBuilder().Append("Control_RemoveTab_Button_AdvancedItemRemoval_Click").ToString());
            }
        }

        private static void Control_RemoveTab_Button_Normal_Click(object? sender, EventArgs? e)
        {
            try
            {
                if (Service_Timer_VersionChecker.MainFormInstance == null)
                {
                    LoggingUtility.Log("MainForm instance is null, cannot return to normal Remove tab.");
                    return;
                }

                if (MainFormInstance != null)
                {
                    MainFormInstance.MainForm_UserControl_RemoveTab.Visible = true;
                }

                if (MainFormInstance != null)
                {
                    MainFormInstance.MainForm_UserControl_AdvancedRemove.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    new StringBuilder().Append("Control_RemoveTab_Button_Normal_Click").ToString());
            }
        }

        private async void Control_RemoveTab_Button_Search_Click(object? sender, EventArgs? e)
        {
            try
            {
                MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Searching inventory...");

                LoggingUtility.Log("RemoveTab Search button clicked.");

                string partId = Control_RemoveTab_ComboBox_Part.Text;
                string op = Control_RemoveTab_ComboBox_Operation.Text;

                if (string.IsNullOrWhiteSpace(partId) || Control_RemoveTab_ComboBox_Part.SelectedIndex <= 0)
                {
                    MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_RemoveTab_ComboBox_Part.Focus();
                    return;
                }

                DataTable results;

                if (!string.IsNullOrWhiteSpace(op) && Control_RemoveTab_ComboBox_Operation.SelectedIndex > 0)
                {
                    MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(40,
                        "Querying by part and operation...");
                    results = await Dao_Inventory.GetInventoryByPartIdAndOperationAsync(partId, op, true);
                }
                else
                {
                    MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(40, "Querying by part...");
                    results = await Dao_Inventory.GetInventoryByPartIdAsync(partId, true);
                }

                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(70, "Updating results...");
                Control_RemoveTab_DataGridView_Main.DataSource = results;
                Control_RemoveTab_DataGridView_Main.ClearSelection();

                // Only show columns in this order: Location, PartID, Operation, Quantity, Notes
                string[] columnsToShow = { "Location", "PartID", "Operation", "Quantity", "Notes" };
                foreach (DataGridViewColumn column in Control_RemoveTab_DataGridView_Main.Columns)
                {
                    column.Visible = columnsToShow.Contains(column.Name);
                }

                // Reorder columns
                for (int i = 0; i < columnsToShow.Length; i++)
                {
                    if (Control_RemoveTab_DataGridView_Main.Columns.Contains(columnsToShow[i]))
                    {
                        Control_RemoveTab_DataGridView_Main.Columns[columnsToShow[i]].DisplayIndex = i;
                    }
                }

                Core_Themes.ApplyThemeToDataGridView(Control_RemoveTab_DataGridView_Main);
                Core_Themes.SizeDataGrid(Control_RemoveTab_DataGridView_Main);

                Control_RemoveTab_Image_NothingFound.Visible = results.Rows.Count == 0;
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(100, "Search complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_RemoveTab_Button_Search_Click").ToString());
            }
            finally
            {
                MainFormInstance?.TabLoadingControlProgress?.HideProgress();
            }
        }

        private void Control_RemoveTab_Button_Print_Click(object? sender, EventArgs? e)
        {
            MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
            MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Preparing print...");

            try
            {
                if (Control_RemoveTab_DataGridView_Main.Rows.Count == 0)
                {
                    MessageBox.Show(@"No data to print.", @"Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Get visible column names for print
                List<string> visibleColumns = new();
                foreach (DataGridViewColumn col in Control_RemoveTab_DataGridView_Main.Columns)
                {
                    if (col.Visible)
                    {
                        visibleColumns.Add(col.Name);
                    }
                }

                Core_DgvPrinter printer = new();
                Control_RemoveTab_DataGridView_Main.Tag = Control_RemoveTab_ComboBox_Part.Text;
                // Set visible columns for print
                printer.SetPrintVisibleColumns(visibleColumns);
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(60, "Printing...");
                printer.Print(Control_RemoveTab_DataGridView_Main);
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(100, "Print complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($@"Print failed: {ex.Message}", @"Print Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                MainFormInstance?.TabLoadingControlProgress?.HideProgress();
            }
        }

        #endregion

        #region ComboBox & UI Events

        private void Control_RemoveTab_ComboBox_Operation_SelectedIndexChanged()
        {
            try
            {
                LoggingUtility.Log("Inventory Op ComboBox selection changed.");

                if (Control_RemoveTab_ComboBox_Operation.SelectedIndex > 0)
                {
                    Control_RemoveTab_ComboBox_Operation.ForeColor =
                        Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
                    Model_AppVariables.Operation = Control_RemoveTab_ComboBox_Operation.Text;
                }
                else
                {
                    Control_RemoveTab_ComboBox_Operation.ForeColor =
                        Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                    if (Control_RemoveTab_ComboBox_Operation.SelectedIndex != 0 &&
                        Control_RemoveTab_ComboBox_Operation.Items.Count > 0)
                    {
                        Control_RemoveTab_ComboBox_Operation.SelectedIndex = 0;
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

        private void Control_RemoveTab_ComboBox_Part_SelectedIndexChanged()
        {
            try
            {
                LoggingUtility.Log("Inventory Part ComboBox selection changed.");

                if (Control_RemoveTab_ComboBox_Part.SelectedIndex > 0)
                {
                    Control_RemoveTab_ComboBox_Part.ForeColor =
                        Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
                    Model_AppVariables.PartId = Control_RemoveTab_ComboBox_Part.Text;
                }
                else
                {
                    Control_RemoveTab_ComboBox_Part.ForeColor =
                        Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                    if (Control_RemoveTab_ComboBox_Part.SelectedIndex != 0 &&
                        Control_RemoveTab_ComboBox_Part.Items.Count > 0)
                    {
                        Control_RemoveTab_ComboBox_Part.SelectedIndex = 0;
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

        private void Control_RemoveTab_Update_ButtonStates()
        {
            try
            {
                Control_RemoveTab_Button_Search.Enabled = Control_RemoveTab_ComboBox_Part.SelectedIndex > 0;
                bool hasData = Control_RemoveTab_DataGridView_Main.Rows.Count > 0;
                bool hasSelection = Control_RemoveTab_DataGridView_Main.SelectedRows.Count > 0;
                Control_RemoveTab_Button_Delete.Enabled = hasData && hasSelection;
                // Print button enable/disable
                if (Control_RemoveTab_Button_Print != null)
                {
                    Control_RemoveTab_Button_Print.Enabled = hasData;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    new StringBuilder().Append("Control_RemoveTab_Update_ButtonStates").ToString());
            }
        }

        private void Control_RemoveTab_OnStartup_WireUpEvents()
        {
            try
            {
                Control_RemoveTab_Button_Reset.Click += (s, e) => Control_RemoveTab_Button_Reset_Click();
                Control_RemoveTab_ComboBox_Part.SelectedIndexChanged += (s, e) =>
                {
                    Control_RemoveTab_ComboBox_Part_SelectedIndexChanged();
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Part, "[ Enter Part Number ]");
                    Control_RemoveTab_Update_ButtonStates();
                };
                Control_RemoveTab_ComboBox_Part.Leave += (s, e) =>
                {
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Part,
                        "[ Enter Part Number ]");
                };

                Control_RemoveTab_ComboBox_Operation.SelectedIndexChanged += (s, e) =>
                {
                    Control_RemoveTab_ComboBox_Operation_SelectedIndexChanged();
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Operation,
                        "[ Enter Operation ]");
                    Control_RemoveTab_Update_ButtonStates();
                };
                Control_RemoveTab_ComboBox_Operation.Leave += (s, e) =>
                {
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Operation,
                        "[ Enter Operation ]");
                };

                Control_RemoveTab_Button_AdvancedItemRemoval.Click +=
                    (s, e) => Control_RemoveTab_Button_AdvancedItemRemoval_Click();

                if (MainFormInstance != null)
                {
                    Control_AdvancedRemove? adv = MainFormInstance.MainForm_UserControl_AdvancedRemove;
                    Control[] btn = adv.Controls.Find("Control_AdvancedRemove_Button_Normal", true);
                    if (btn.Length > 0 && btn[0] is Button normalBtn)
                    {
                        normalBtn.Click -= Control_RemoveTab_Button_Normal_Click;
                        normalBtn.Click += Control_RemoveTab_Button_Normal_Click;
                    }
                }

                Control_RemoveTab_ComboBox_Part.Enter += (s, e) =>
                {
                    Control_RemoveTab_ComboBox_Part.BackColor =
                        Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
                };
                Control_RemoveTab_ComboBox_Part.Leave += (s, e) =>
                {
                    Control_RemoveTab_ComboBox_Part.BackColor =
                        Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Part, "[ Enter Part Number ]");
                };

                Control_RemoveTab_ComboBox_Operation.Enter += (s, e) =>
                {
                    Control_RemoveTab_ComboBox_Operation.BackColor =
                        Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
                };
                Control_RemoveTab_ComboBox_Operation.Leave += (s, e) =>
                {
                    Control_RemoveTab_ComboBox_Operation.BackColor =
                        Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Operation,
                        "[ Enter Operation ]");
                };

                Control_RemoveTab_DataGridView_Main.SelectionChanged +=
                    (s, e) => Control_RemoveTab_Update_ButtonStates();

                // Also update print button state on data source change
                Control_RemoveTab_DataGridView_Main.DataSourceChanged +=
                    (s, e) => Control_RemoveTab_Update_ButtonStates();

                Control_RemoveTab_Button_Delete.Click += Control_RemoveTab_Button_Delete_Click;

                LoggingUtility.Log("Removal tab events wired up.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    new StringBuilder().Append("MainForm_WireUpRemoveTabEvents").ToString());
            }
        }

        private void Control_RemoveTab_Button_Toggle_RightPanel_Click(object sender, EventArgs e)
        {
            if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed)
            {
                MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = true;

                Control_RemoveTab_Button_Toggle_RightPanel.Text = "⬅️";
                Control_RemoveTab_Button_Toggle_RightPanel.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
            }
            else
            {
                if (MainFormInstance != null)
                {
                    MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = false;
                    Control_RemoveTab_Button_Toggle_RightPanel.Text = "➡️";
                    Control_RemoveTab_Button_Toggle_RightPanel.ForeColor =
                        Model_AppVariables.UserUiColors.SuccessColor ?? Color.Green;
                }
            }

            Helper_UI_ComboBoxes.DeselectAllComboBoxText(this);
        }

        private async void Control_RemoveTab_Button_ShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                MainFormInstance?.TabLoadingControlProgress?.ShowProgress();
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(10, "Loading all inventory...");

                // Query all inventory using Dao_Inventory's public property, sorted by Location
                DataTable dt = await Dao_Inventory.PublicHelperDatabaseCore.ExecuteDataTable(
                    "SELECT * FROM inv_inventory ORDER BY Location", null, true, CommandType.Text);

                Control_RemoveTab_DataGridView_Main.DataSource = dt;
                Control_RemoveTab_DataGridView_Main.ClearSelection();

                // Only show columns in this order: Location, PartID, Operation, Quantity, Notes
                string[] columnsToShow = { "Location", "PartID", "Operation", "Quantity", "Notes" };
                foreach (DataGridViewColumn column in Control_RemoveTab_DataGridView_Main.Columns)
                {
                    column.Visible = columnsToShow.Contains(column.Name);
                }

                // Reorder columns
                for (int i = 0; i < columnsToShow.Length; i++)
                {
                    if (Control_RemoveTab_DataGridView_Main.Columns.Contains(columnsToShow[i]))
                    {
                        Control_RemoveTab_DataGridView_Main.Columns[columnsToShow[i]].DisplayIndex = i;
                    }
                }

                Core_Themes.ApplyThemeToDataGridView(Control_RemoveTab_DataGridView_Main);
                Core_Themes.SizeDataGrid(Control_RemoveTab_DataGridView_Main);
                Control_RemoveTab_Image_NothingFound.Visible = dt.Rows.Count == 0;
                MainFormInstance?.TabLoadingControlProgress?.UpdateProgress(100, "Show all complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($@"Show All failed: {ex.Message}", @"Show All Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                MainFormInstance?.TabLoadingControlProgress?.HideProgress();
            }
        }

        #endregion

        #region Helpers

        private List<(string PartID, string Location, int Quantity)> GetSelectedItemsToDelete(out string summary)
        {
            StringBuilder sb = new();
            List<(string PartID, string Location, int Quantity)> itemsToDelete = new();
            foreach (DataGridViewRow row in Control_RemoveTab_DataGridView_Main.SelectedRows)
            {
                if (row.DataBoundItem is DataRowView drv)
                {
                    string partId = drv["PartID"]?.ToString() ?? "";
                    string location = drv["Location"]?.ToString() ?? "";
                    string quantityStr = drv["Quantity"]?.ToString() ?? "";
                    if (!TryParse(quantityStr, out int quantity))
                    {
                        LoggingUtility.LogApplicationError(new Exception(
                            $"Invalid quantity value: '{quantityStr}' for PartID={partId}, Location={location}"));
                        continue;
                    }

                    sb.AppendLine($"PartID: {partId}, Location: {location}, Quantity: {quantity}");
                    LoggingUtility.Log(
                        $"Selected for deletion: PartID={partId}, Location={location}, Quantity={quantity}");

                    itemsToDelete.Add((partId, location, quantity));
                }
            }

            summary = sb.ToString();
            return itemsToDelete;
        }

        #endregion

        #endregion
    }

    #endregion
}
