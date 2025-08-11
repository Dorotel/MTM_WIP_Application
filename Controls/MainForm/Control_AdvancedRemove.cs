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

namespace MTM_Inventory_Application.Controls.MainForm
{
    public partial class Control_AdvancedRemove : UserControl
    {
        #region Fields

        private readonly List<Model_HistoryRemove> _lastRemovedItems = [];
        public static Forms.MainForm.MainForm? MainFormInstance { get; set; }
        private Helper_StoredProcedureProgress? _progressHelper;

        #endregion

        #region Progress Control Methods

        /// <summary>
        /// Set progress controls for visual feedback during operations
        /// </summary>
        /// <param name="progressBar">Progress bar control</param>
        /// <param name="statusLabel">Status label control</param>
        public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, 
                this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
        }

        #endregion

        #region Constructors

        public Control_AdvancedRemove()
        {
            InitializeComponent();

            // Apply comprehensive DPI scaling and runtime layout adjustments
            // THEME POLICY: Only update theme on startup, in settings menu, or on DPI change.
            // Do NOT call theme update methods from arbitrary event handlers or business logic.
            Core_Themes.ApplyDpiScaling(this); // Allowed: UserControl initialization
            Core_Themes.ApplyRuntimeLayoutAdjustments(this); // Allowed: UserControl initialization

            // No longer need to initialize or wire up removed ComboBoxes or Like controls
            Control_AdvancedRemove_Initialize();
            ApplyStandardComboBoxProperties();
            WireUpComboBoxEvents();
            Core_Themes.ApplyFocusHighlighting(this);
            Control[] btn = Controls.Find("Control_AdvancedRemove_Button_Normal", true);
            if (btn.Length > 0 && btn[0] is Button normalBtn)
            {
                normalBtn.Click -= Control_AdvancedRemove_Button_Normal_Click;
                normalBtn.Click += Control_AdvancedRemove_Button_Normal_Click;
                ToolTip toolTip = new();
                toolTip.SetToolTip(normalBtn,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Normal)}");
            }

            Control[] undoBtn = Controls.Find("Control_AdvancedRemove_Button_Undo", true);
            if (undoBtn.Length > 0 && undoBtn[0] is Button undoButton)
            {
                undoButton.Click -= Control_AdvancedRemove_Button_Undo_Click;
                undoButton.Click += Control_AdvancedRemove_Button_Undo_Click;
                ToolTip toolTip = new();
                toolTip.SetToolTip(undoButton,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Undo)}");
            }

            Control[] searchBtn = Controls.Find("Control_AdvancedRemove_Button_Search", true);
            if (searchBtn.Length > 0 && searchBtn[0] is Button searchButton)
            {
                searchButton.Click -= Control_AdvancedRemove_Button_Search_Click;
                searchButton.Click += Control_AdvancedRemove_Button_Search_Click;
                ToolTip toolTip = new();
                toolTip.SetToolTip(searchButton,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Search)}");
            }

            Control[] resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
            if (resetBtn.Length > 0 && resetBtn[0] is Button resetButton)
            {
                resetButton.Click -= Control_AdvancedRemove_Button_Reset_Click;
                resetButton.Click += Control_AdvancedRemove_Button_Reset_Click;
                ToolTip toolTip = new();
                toolTip.SetToolTip(resetButton,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Reset)}");
            }

            Control[] deleteBtn = Controls.Find("Control_AdvancedRemove_Button_Delete", true);
            if (deleteBtn.Length > 0 && deleteBtn[0] is Button deleteButton)
            {
                deleteButton.Click -= Control_AdvancedRemove_Button_Delete_Click;
                deleteButton.Click += Control_AdvancedRemove_Button_Delete_Click;
                ToolTip toolTip = new();
                toolTip.SetToolTip(deleteButton,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Delete)}");
            }

            Control[] printBtn = Controls.Find("Control_AdvancedRemove_Button_Print", true);
            if (printBtn.Length > 0 && printBtn[0] is Button printButton)
            {
                printButton.Click -= Control_AdvancedRemove_Button_Print_Click;
                printButton.Click += Control_AdvancedRemove_Button_Print_Click;
                ToolTip toolTip = new();
                toolTip.SetToolTip(printButton, "Print the current results");
            }

            Control_AdvancedRemove_CheckBox_Date.CheckedChanged += (s, e) =>
            {
                bool enabled = Control_AdvancedRemove_CheckBox_Date.Checked;
                Control_AdvancedRemove_DateTimePicker_From.Enabled = enabled;
                Control_AdvancedRemove_DateTimePicker_To.Enabled = enabled;
            };
            bool dateEnabled = Control_AdvancedRemove_CheckBox_Date.Checked;
            Control_AdvancedRemove_DateTimePicker_From.Enabled = dateEnabled;
            Control_AdvancedRemove_DateTimePicker_To.Enabled = dateEnabled;

            _ = LoadComboBoxesAsync();
        }

        #endregion

        #region Methods

        private static void Control_AdvancedRemove_Button_Normal_Click(object? sender, EventArgs? e)
        {
            try
            {
                if (Services.Service_Timer_VersionChecker.MainFormInstance == null)
                {
                    LoggingUtility.Log("MainForm instance is null, cannot return to normal Remove tab.");
                    return;
                }

                if (Control_RemoveTab.MainFormInstance != null)
                {
                    Control_RemoveTab.MainFormInstance.MainForm_UserControl_RemoveTab.Visible = true;
                }

                if (Control_RemoveTab.MainFormInstance != null)
                {
                    Control_RemoveTab.MainFormInstance.MainForm_UserControl_AdvancedRemove.Visible = false;
                }

                Control_RemoveTab? removeTab = Control_RemoveTab.MainFormInstance?.MainForm_UserControl_RemoveTab;
                if (removeTab != null)
                {
                    if (removeTab.Controls.Find("Control_RemoveTab_ComboBox_Part", true).FirstOrDefault() is ComboBox
                        part)
                    {
                        part.SelectedIndex = 0;
                        part.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                        part.Focus();
                    }

                    if (removeTab.Controls.Find("Control_RemoveTab_ComboBox_Operation", true)
                            .FirstOrDefault() is ComboBox
                        op)
                    {
                        op.SelectedIndex = 0;
                        op.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    new StringBuilder().Append("Control_AdvancedRemove_Button_Normal_Click").ToString());
            }
        }

        private void Control_AdvancedRemove_Initialize()
        {
            // No longer need to set ComboBox colors for removed controls
        }

        private void ApplyStandardComboBoxProperties() =>
            // Only apply to User ComboBox
            Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(Control_AdvancedRemove_ComboBox_User);

        private async Task LoadComboBoxesAsync()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(Control_AdvancedRemove_ComboBox_User);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private void WireUpComboBoxEvents()
        {
            // Only wire up User ComboBox events
            Control_AdvancedRemove_ComboBox_User.SelectedIndexChanged += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_User, "[ Enter User ]");
                if (Control_AdvancedRemove_ComboBox_User.SelectedIndex > 0)
                {
                    Control_AdvancedRemove_ComboBox_User.ForeColor =
                        Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
                }
                else
                {
                    Control_AdvancedRemove_ComboBox_User.ForeColor =
                        Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                }
            };
            Control_AdvancedRemove_ComboBox_User.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_User, "[ Enter User ]");
            };

            Control_AdvancedRemove_ComboBox_User.Enter +=
                (s, e) => Control_AdvancedRemove_ComboBox_User.BackColor =
                    Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
            Control_AdvancedRemove_ComboBox_User.Leave +=
                (s, e) => Control_AdvancedRemove_ComboBox_User.BackColor =
                    Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;

            Control_AdvancedRemove_DataGridView_Results.SelectionChanged +=
                (s, e) => Control_AdvancedRemove_Update_ButtonStates();
            Control_AdvancedRemove_DataGridView_Results.DataSourceChanged +=
                (s, e) => Control_AdvancedRemove_Update_ButtonStates();
        }

        private async void Control_AdvancedRemove_Button_Search_Click(object? sender, EventArgs? e)
        {
            try
            {
                // Gather search fields from textboxes and user combobox
                string part = Control_AdvancedRemove_TextBox_Part.Text.Trim();
                string op = Control_AdvancedRemove_TextBox_Operation.Text.Trim();
                string loc = Control_AdvancedRemove_TextBox_Location.Text.Trim();
                string qtyMinText = Control_AdvancedRemove_TextBox_QtyMin.Text.Trim();
                string qtyMaxText = Control_AdvancedRemove_TextBox_QtyMax.Text.Trim();
                string notes = Control_AdvancedRemove_TextBox_Notes.Text.Trim();
                string user = Control_AdvancedRemove_ComboBox_User.Text.Trim();
                bool filterByDate = Control_AdvancedRemove_CheckBox_Date.Checked;
                DateTime? dateFrom =
                    filterByDate ? Control_AdvancedRemove_DateTimePicker_From.Value.Date : (DateTime?)null;
                DateTime? dateTo = filterByDate
                    ? Control_AdvancedRemove_DateTimePicker_To.Value.Date.AddDays(1).AddTicks(-1)
                    : (DateTime?)null;

                int? qtyMin = int.TryParse(qtyMinText, out int qmin) ? qmin : null;
                int? qtyMax = int.TryParse(qtyMaxText, out int qmax) ? qmax : null;

                bool userSelected = Control_AdvancedRemove_ComboBox_User.SelectedIndex > 0 &&
                                    !string.IsNullOrWhiteSpace(user);

                bool anyFieldFilled =
                    !string.IsNullOrWhiteSpace(part) ||
                    !string.IsNullOrWhiteSpace(op) ||
                    !string.IsNullOrWhiteSpace(loc) ||
                    qtyMin.HasValue ||
                    qtyMax.HasValue ||
                    !string.IsNullOrWhiteSpace(notes) ||
                    userSelected ||
                    (filterByDate && dateFrom != null && dateTo != null);

                if (!anyFieldFilled)
                {
                    MessageBox.Show(@"Please fill in at least one field to search.", @"Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (filterByDate && dateFrom > dateTo)
                {
                    MessageBox.Show(@"The 'From' date cannot be after the 'To' date.", @"Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // FIXED: Use Helper_Database_StoredProcedure instead of hardcoded MySQL query
                var searchParameters = new Dictionary<string, object>
                {
                    { "PartID", string.IsNullOrWhiteSpace(part) ? (object)DBNull.Value : part },
                    { "Operation", string.IsNullOrWhiteSpace(op) ? (object)DBNull.Value : op },
                    { "Location", string.IsNullOrWhiteSpace(loc) ? (object)DBNull.Value : loc },
                    { "QtyMin", qtyMin ?? (object)DBNull.Value },
                    { "QtyMax", qtyMax ?? (object)DBNull.Value },
                    { "Notes", string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes },
                    { "User", userSelected ? user : (object)DBNull.Value },
                    { "FilterByDate", filterByDate },
                    { "DateFrom", dateFrom ?? (object)DBNull.Value },
                    { "DateTo", dateTo ?? (object)DBNull.Value }
                };

                LoggingUtility.Log($"[SEARCH DEBUG] Executing search with parameters: " +
                    $"PartID='{part}', Operation='{op}', Location='{loc}', " +
                    $"QtyMin={qtyMin}, QtyMax={qtyMax}, Notes='{notes}', " +
                    $"User='{(userSelected ? user : "NULL")}', FilterByDate={filterByDate}, " +
                    $"DateFrom={dateFrom}, DateTo={dateTo}");

                var searchResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "inv_inventory_Search_Advanced",
                    searchParameters,
                    _progressHelper,
                    true);

                if (!searchResult.IsSuccess)
                {
                    string errorMsg = !string.IsNullOrEmpty(searchResult.ErrorMessage) 
                        ? searchResult.ErrorMessage 
                        : "Unknown error occurred during search";
                    MessageBox.Show($"Search failed: {errorMsg}", @"Search Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataTable dt = searchResult.Data ?? new DataTable();
                LoggingUtility.Log($"[SEARCH DEBUG] Search completed. Status: {searchResult.StatusMessage}, Rows: {dt.Rows.Count}");

                Control_AdvancedRemove_DataGridView_Results.DataSource = dt;
                Control_AdvancedRemove_DataGridView_Results.ClearSelection();
                // Only show columns in this order: Location, PartID, Operation, Quantity, Notes
                string[] columnsToShow = { "Location", "PartID", "Operation", "Quantity", "Notes" };
                foreach (DataGridViewColumn column in Control_AdvancedRemove_DataGridView_Results.Columns)
                {
                    column.Visible = columnsToShow.Contains(column.Name);
                }

                // Reorder columns
                for (int i = 0; i < columnsToShow.Length; i++)
                {
                    if (Control_AdvancedRemove_DataGridView_Results.Columns.Contains(columnsToShow[i]))
                    {
                        Control_AdvancedRemove_DataGridView_Results.Columns[columnsToShow[i]].DisplayIndex = i;
                    }
                }

                Core_Themes.ApplyThemeToDataGridView(Control_AdvancedRemove_DataGridView_Results);
                Core_Themes.SizeDataGrid(Control_AdvancedRemove_DataGridView_Results);
                Control_AdvancedRemove_Image_NothingFound.Visible = dt.Rows.Count == 0;
                Control_AdvancedRemove_Update_ButtonStates();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($@"Error during advanced search: {ex.Message}", @"Search Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void Control_AdvancedRemove_Button_Delete_Click(object? sender, EventArgs? e)
        {
            try
            {
                DataGridView? dgv = Control_AdvancedRemove_DataGridView_Results;
                int selectedCount = dgv.SelectedRows.Count;
                LoggingUtility.Log($"[ADVANCED REMOVE] Delete clicked. Selected rows: {selectedCount}");
                if (selectedCount == 0)
                {
                    LoggingUtility.Log("[ADVANCED REMOVE] No rows selected for deletion.");
                    return;
                }

                // Gather deleted items for undo
                _lastRemovedItems.Clear();
                foreach (DataGridViewRow row in dgv.SelectedRows)
                {
                    string partId = row.Cells["PartID"].Value?.ToString() ?? "";
                    string location = row.Cells["Location"].Value?.ToString() ?? "";
                    string operation = row.Cells["Operation"].Value?.ToString() ?? "";
                    int quantity = int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int q) ? q : 0;
                    string itemType = row.Cells["ItemType"]?.Value?.ToString() ?? ""; // If you have this column
                    string user = row.Cells["User"]?.Value?.ToString() ?? "";
                    string batchNumber = row.Cells["BatchNumber"]?.Value?.ToString() ?? "";

                    _lastRemovedItems.Add(new Model_HistoryRemove
                    {
                        PartId = partId,
                        Location = location,
                        Operation = operation,
                        Quantity = quantity,
                        ItemType = itemType,
                        User = user,
                        BatchNumber = batchNumber
                    });
                }

                StringBuilder sb = new();
                foreach (Model_HistoryRemove item in _lastRemovedItems)
                {
                    sb.AppendLine(
                        $"PartID: {item.PartId}, Location: {item.Location}, Operation: {item.Operation}, Quantity: {item.Quantity}");
                }

                string summary = sb.ToString();

                DialogResult confirmResult = MessageBox.Show(
                    $@"The following items will be deleted:

{summary}Are you sure?",
                    @"Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes)
                {
                    LoggingUtility.Log("[ADVANCED REMOVE] User cancelled deletion.");
                    _lastRemovedItems.Clear();
                    return;
                }

                var removeResult = await Dao_Inventory.RemoveInventoryItemsFromDataGridViewAsync(dgv);
                LoggingUtility.Log($"[ADVANCED REMOVE] Remove operation result: Success={removeResult.IsSuccess}, Message={removeResult.StatusMessage}");
                
                if (removeResult.IsSuccess)
                {
                    int removedCount = removeResult.Data.RemovedCount;
                    List<string> errorMessages = removeResult.Data.ErrorMessages;
                    
                    if (removedCount == 0)
                    {
                        string msg =
                            "No items were deleted. This may be because the selected items no longer exist in inventory, the data did not match exactly, or a database constraint prevented deletion.";
                        if (errorMessages.Count > 0)
                        {
                            msg += "\n\n" + string.Join("\n", errorMessages);
                        }

                        MessageBox.Show(msg, @"Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Control_AdvancedRemove_Button_Undo.Enabled = false;
                    }
                    else
                    {
                        // Enable Undo button if items were deleted
                        Control_AdvancedRemove_Button_Undo.Enabled = true;
                    }
                }
                else
                {
                    string errorMsg = !string.IsNullOrEmpty(removeResult.ErrorMessage) 
                        ? removeResult.ErrorMessage 
                        : "Unknown error occurred during removal";
                    MessageBox.Show($"Error removing items: {errorMsg}", @"Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Control_AdvancedRemove_Button_Undo.Enabled = false;
                }

                Control_AdvancedRemove_Button_Search_Click(null, null);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
            }
        }

        private async Task Control_AdvancedRemove_HardReset()
        {
            Control[] resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
            if (resetBtn.Length > 0 && resetBtn[0] is Button btn)
            {
                btn.Enabled = false;
            }

            try
            {
                _progressHelper?.ShowProgress();
                _progressHelper?.UpdateProgress(10,
                    "Resetting Advanced Remove tab...");
                Debug.WriteLine("[DEBUG] AdvancedRemove HardReset - start");
                if (MainFormInstance != null)
                {
                    Debug.WriteLine("[DEBUG] Updating status strip for hard reset");
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
                }

                _progressHelper?.UpdateProgress(30,
                    "Resetting data tables...");
                Debug.WriteLine("[DEBUG] Hiding ComboBoxes");

                Debug.WriteLine("[DEBUG] Resetting and refreshing all ComboBox DataTables");
                await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
                Debug.WriteLine("[DEBUG] DataTables reset complete");

                _progressHelper?.UpdateProgress(60,
                    "Refilling combo boxes...");
                Debug.WriteLine("[DEBUG] Refilling User ComboBox");
                await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(Control_AdvancedRemove_ComboBox_User);

                MainFormControlHelper.ResetComboBox(Control_AdvancedRemove_ComboBox_User,
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);

                Control_AdvancedRemove_TextBox_QtyMin.Text = string.Empty;
                Control_AdvancedRemove_TextBox_QtyMax.Text = string.Empty;
                Control_AdvancedRemove_TextBox_Notes.Text = string.Empty;
                Control_AdvancedRemove_TextBox_Part.Text = string.Empty;
                Control_AdvancedRemove_TextBox_Operation.Text = string.Empty;
                Control_AdvancedRemove_TextBox_Location.Text = string.Empty;

                Control_AdvancedRemove_CheckBox_Date.Checked = false;
                Control_AdvancedRemove_DateTimePicker_From.Value = DateTime.Today;
                Control_AdvancedRemove_DateTimePicker_To.Value = DateTime.Today;
                Control_AdvancedRemove_DateTimePicker_From.Enabled = false;
                Control_AdvancedRemove_DateTimePicker_To.Enabled = false;

                Control_AdvancedRemove_DataGridView_Results.DataSource = null;
                Control_AdvancedRemove_DataGridView_Results.Rows.Clear();
                Control_AdvancedRemove_Image_NothingFound.Visible = false;

                if (Control_AdvancedRemove_ComboBox_User.FindForm() is { } form)
                {
                    MainFormControlHelper.SetActiveControl(form, Control_AdvancedRemove_ComboBox_User);
                }

                Debug.WriteLine("[DEBUG] AdvancedRemove HardReset - end");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in AdvancedRemove HardReset: {ex}");
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "Control_AdvancedRemove_HardReset");
            }
            finally
            {
                Debug.WriteLine("[DEBUG] AdvancedRemove HardReset button re-enabled");
                if (resetBtn.Length > 0 && resetBtn[0] is Button btn2)
                {
                    btn2.Enabled = true;
                }

                if (Control_RemoveTab.MainFormInstance != null)
                {
                    Debug.WriteLine("[DEBUG] Restoring status strip after hard reset");
                    Control_RemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                    Control_RemoveTab.MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                    Control_RemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                        @"Disconnected from Server, please standby...";
                    _progressHelper?.HideProgress();
                }
            }
        }

        private void Control_AdvancedRemove_SoftReset()
        {
            Control[] resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
            if (resetBtn.Length > 0 && resetBtn[0] is Button btn)
            {
                btn.Enabled = false;
            }

            try
            {
                if (MainFormInstance != null)
                {
                    Debug.WriteLine("[DEBUG] Updating status strip for Soft Reset");
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
                }

                MainFormControlHelper.ResetComboBox(Control_AdvancedRemove_ComboBox_User,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                Control_AdvancedRemove_TextBox_QtyMin.Text = string.Empty;
                Control_AdvancedRemove_TextBox_QtyMin.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                Control_AdvancedRemove_TextBox_QtyMax.Text = string.Empty;
                Control_AdvancedRemove_TextBox_QtyMax.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                Control_AdvancedRemove_TextBox_Notes.Text = string.Empty;
                Control_AdvancedRemove_TextBox_Notes.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                Control_AdvancedRemove_TextBox_Part.Text = string.Empty;
                Control_AdvancedRemove_TextBox_Part.ForeColor = Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                Control_AdvancedRemove_TextBox_Operation.Text = string.Empty;
                Control_AdvancedRemove_TextBox_Operation.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                Control_AdvancedRemove_TextBox_Location.Text = string.Empty;
                Control_AdvancedRemove_TextBox_Location.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;

                Control_AdvancedRemove_CheckBox_Date.Checked = false;
                Control_AdvancedRemove_DateTimePicker_From.Value = DateTime.Today;
                Control_AdvancedRemove_DateTimePicker_To.Value = DateTime.Today;
                Control_AdvancedRemove_DateTimePicker_From.Enabled = false;
                Control_AdvancedRemove_DateTimePicker_To.Enabled = false;

                // Reset DataGridView as well
                Control_AdvancedRemove_DataGridView_Results.DataSource = null;
                Control_AdvancedRemove_DataGridView_Results.Rows.Clear();
                Control_AdvancedRemove_Image_NothingFound.Visible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in AdvancedRemove SoftReset: {ex}");
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "Control_AdvancedRemove_SoftReset");
            }
            finally
            {
                Debug.WriteLine("[DEBUG] AdvancedRemove SoftReset button re-enabled");
                if (resetBtn.Length > 0 && resetBtn[0] is Button btn2)
                {
                    btn2.Enabled = true;
                }

                if (Control_RemoveTab.MainFormInstance != null)
                {
                    Debug.WriteLine("[DEBUG] Restoring status strip after soft reset");
                    Control_RemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                    Control_RemoveTab.MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                    Control_RemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                        @"Disconnected from Server, please standby...";
                }

                if (Control_AdvancedRemove_ComboBox_User.FindForm() is { } form)
                {
                    MainFormControlHelper.SetActiveControl(form, Control_AdvancedRemove_ComboBox_User);
                }
            }
        }

        private async void Control_AdvancedRemove_Button_Reset_Click(object? sender, EventArgs? e)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                await Control_AdvancedRemove_HardReset();
            }
            else
            {
                Control_AdvancedRemove_SoftReset();
            }
        }

        private async void Control_AdvancedRemove_Button_Undo_Click(object? sender, EventArgs? e)
        {
            if (_lastRemovedItems.Count == 0)
            {
                return;
            }

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

                MessageBox.Show(@"Undo successful. Removed items have been restored.", @"Undo", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                LoggingUtility.Log("Undo: Removed items restored (Advanced Remove tab).");

                _lastRemovedItems.Clear();
                Control[] undoBtn = Controls.Find("Control_AdvancedRemove_Button_Undo", true);
                if (undoBtn.Length > 0 && undoBtn[0] is Button btn)
                {
                    btn.Enabled = false;
                }

                Control_AdvancedRemove_Button_Search_Click(null, null);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show(@"Undo failed: " + ex.Message, @"Undo Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void Control_AdvancedRemove_Button_Print_Click(object? sender, EventArgs? e)
        {
            try
            {
                if (Control_AdvancedRemove_DataGridView_Results.Rows.Count == 0)
                {
                    MessageBox.Show(@"No data to print.", @"Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Core_DgvPrinter printer = new();
                // Use Part textbox for tag
                Control_AdvancedRemove_DataGridView_Results.Tag = Control_AdvancedRemove_TextBox_Part.Text;
                printer.Print(Control_AdvancedRemove_DataGridView_Results);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($@"Print failed: {ex.Message}", @"Print Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void Control_AdvancedRemove_Update_ButtonStates()
        {
            try
            {
                bool hasData = Control_AdvancedRemove_DataGridView_Results.Rows.Count > 0;
                if (Control_AdvancedRemove_Button_Print != null)
                {
                    Control_AdvancedRemove_Button_Print.Enabled = hasData;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Core_WipAppVariables.Shortcut_Remove_Delete)
            {
                Control_AdvancedRemove_Button_Delete.PerformClick();
                return true;
            }

            if (keyData == Core_WipAppVariables.Shortcut_Remove_Undo)
            {
                Control_AdvancedRemove_Button_Undo.PerformClick();
                return true;
            }

            if (keyData == Core_WipAppVariables.Shortcut_Remove_Reset)
            {
                Control[] resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
                if (resetBtn.Length > 0 && resetBtn[0] is Button btn)
                {
                    btn.PerformClick();
                    return true;
                }
            }

            if (keyData == Core_WipAppVariables.Shortcut_Remove_Search)
            {
                Control[] searchBtn = Controls.Find("Control_AdvancedRemove_Button_Search", true);
                if (searchBtn.Length > 0 && searchBtn[0] is Button btn)
                {
                    btn.PerformClick();
                    return true;
                }
            }

            if (keyData == Core_WipAppVariables.Shortcut_Remove_Normal)
            {
                Control[] normalBtn = Controls.Find("Control_AdvancedRemove_Button_Normal", true);
                if (normalBtn.Length > 0 && normalBtn[0] is Button btn)
                {
                    btn.PerformClick();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Control_AdvancedRemove_Button_SidePanel_Click(object sender, EventArgs e)
        {
            SplitContainer? splitContainer = Control_AdvancedRemove_SplitContainer_Main;
            Button? button = sender as Button ?? Control_AdvancedRemove_Button_SidePanel;

            if (splitContainer.Panel1Collapsed)
            {
                splitContainer.Panel1Collapsed = false;
                button.Text = "Collapse ⬅️";
            }
            else
            {
                splitContainer.Panel1Collapsed = true;
                button.Text = "Expand ➡️";
            }
        }

        #endregion
    }
}
