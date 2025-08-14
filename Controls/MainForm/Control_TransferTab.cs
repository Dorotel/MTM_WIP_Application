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
using MTM_Inventory_Application.Services;

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
        private Helper_StoredProcedureProgress? _progressHelper;
        
        // Enhanced functionality fields
        private int _currentPage = 1;
        private const int PageSize = 20;

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

        #region Initialization

        public Control_TransferTab()
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["ControlType"] = nameof(Control_TransferTab),
                ["InitializationTime"] = DateTime.Now,
                ["Thread"] = Thread.CurrentThread.ManagedThreadId
            }, nameof(Control_TransferTab), nameof(Control_TransferTab));

            Service_DebugTracer.TraceUIAction("TRANSFER_TAB_INITIALIZATION", nameof(Control_TransferTab),
                new Dictionary<string, object>
                {
                    ["Phase"] = "START",
                    ["ComponentType"] = "UserControl",
                    ["DesignType"] = "REDESIGNED_MODERN_LAYOUT"
                });

            InitializeComponent();

            // Apply comprehensive DPI scaling and runtime layout adjustments
            // THEME POLICY: Only update theme on startup, in settings menu, or on DPI change.
            // Do NOT call theme update methods from arbitrary event handlers or business logic.
            Core_Themes.ApplyDpiScaling(this); // Allowed: UserControl initialization
            Core_Themes.ApplyRuntimeLayoutAdjustments(this); // Allowed: UserControl initialization

            Control_TransferTab_Initialize();
            ApplyPrivileges();
            InitializeNewControls();
            SetupEnhancedEventHandlers();

            Color errorColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            Control_TransferTab_ComboBox_Part.ForeColor = errorColor;
            Control_TransferTab_ComboBox_Operation.ForeColor = errorColor;
            Control_TransferTab_ComboBox_ToLocation.ForeColor = errorColor;
            Control_TransferTab_Image_NothingFound.Visible = false;

            // Use cached ToolTip for enhanced UI
            SharedToolTip.SetToolTip(Control_TransferTab_Button_Search, "Search inventory items for transfer");
            SharedToolTip.SetToolTip(Control_TransferTab_Button_Transfer, "Execute transfer operation");
            SharedToolTip.SetToolTip(Control_TransferTab_Button_Reset, "Reset all fields and clear results");
            SharedToolTip.SetToolTip(Control_TransferTab_Button_SidePanel, "Toggle side panel visibility");
            SharedToolTip.SetToolTip(Control_TransferTab_Button_SmartSearch, "Advanced search with field-specific syntax");

            // Setup Print button event and tooltip directly
            Control_TransferTab_Button_Print.Click -= Control_TransferTab_Button_Print_Click;
            Control_TransferTab_Button_Print.Click += Control_TransferTab_Button_Print_Click;
            SharedToolTip.SetToolTip(Control_TransferTab_Button_Print, "Print the current results");

            _ = Control_TransferTab_OnStartup_LoadComboBoxesAsync();
            Control_TransferTab_Update_ButtonStates();
        }

        #endregion

        #region Enhanced Event Handlers

        /// <summary>
        /// Handle smart search functionality with advanced syntax
        /// </summary>
        private async void Control_TransferTab_Button_SmartSearch_Click(object? sender, EventArgs? e)
        {
            Service_DebugTracer.TraceUIAction("SMART_SEARCH_CLICKED", nameof(Control_TransferTab),
                new Dictionary<string, object>
                {
                    ["SearchText"] = Control_TransferTab_TextBox_SmartSearch.Text ?? "",
                    ["SearchLength"] = Control_TransferTab_TextBox_SmartSearch.Text?.Length ?? 0
                });

            try
            {
                _progressHelper?.ShowProgress();
                _progressHelper?.UpdateProgress(10, "Processing smart search...");

                string searchText = Control_TransferTab_TextBox_SmartSearch.Text?.Trim() ?? "";
                
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    // If no smart search text, fall back to regular search
                    Control_TransferTab_Button_Search_Click(sender, e);
                    return;
                }

                // Parse smart search syntax
                var searchCriteria = ParseSmartSearchText(searchText);
                await ExecuteSmartSearchAsync(searchCriteria);

                _progressHelper?.UpdateProgress(100, "Smart search complete");
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: "Control_TransferTab_SmartSearch");
            }
            finally
            {
                _progressHelper?.HideProgress();
            }
        }

        /// <summary>
        /// Toggle the side panel visibility
        /// </summary>
        private void Control_TransferTab_Button_SidePanel_Click(object? sender, EventArgs? e)
        {
            Service_DebugTracer.TraceUIAction("SIDE_PANEL_TOGGLE", nameof(Control_TransferTab),
                new Dictionary<string, object>
                {
                    ["CurrentCollapsed"] = Control_TransferTab_SplitContainer_Main.Panel1Collapsed,
                    ["Action"] = Control_TransferTab_SplitContainer_Main.Panel1Collapsed ? "EXPAND" : "COLLAPSE"
                });

            bool isCollapsed = Control_TransferTab_SplitContainer_Main.Panel1Collapsed;
            Control_TransferTab_SplitContainer_Main.Panel1Collapsed = !isCollapsed;
            
            Control_TransferTab_Button_SidePanel.Text = isCollapsed ? "Hide Panel ⬅️" : "Show Panel ➡️";
        }

        /// <summary>
        /// Show transfer history dialog
        /// </summary>
        private void Control_TransferTab_Button_SelectionHistory_Click(object? sender, EventArgs? e)
        {
            Service_DebugTracer.TraceUIAction("SELECTION_HISTORY_CLICKED", nameof(Control_TransferTab), new Dictionary<string, object>());
            
            try
            {
                // Open transfer history form - this would be implemented based on your history system
                Service_ErrorHandler.ShowInformation(
                    "Transfer History", 
                    "Transfer history functionality will be implemented here.",
                    controlName: "Control_TransferTab_History"
                );
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                    controlName: "Control_TransferTab_History");
            }
        }

        /// <summary>
        /// Navigate to previous page
        /// </summary>
        private void Control_TransferTab_Button_Previous_Click(object? sender, EventArgs? e)
        {
            Service_DebugTracer.TraceUIAction("PREVIOUS_PAGE_CLICKED", nameof(Control_TransferTab), 
                new Dictionary<string, object> { ["CurrentPage"] = _currentPage });
            // Pagination logic would be implemented here
        }

        /// <summary>
        /// Navigate to next page
        /// </summary>
        private void Control_TransferTab_Button_Next_Click(object? sender, EventArgs? e)
        {
            Service_DebugTracer.TraceUIAction("NEXT_PAGE_CLICKED", nameof(Control_TransferTab), 
                new Dictionary<string, object> { ["CurrentPage"] = _currentPage });
            // Pagination logic would be implemented here
        }

        /// <summary>
        /// Update selection report when DataGridView selection changes
        /// </summary>
        private void Control_TransferTab_DataGridView_SelectionReport_Changed(object? sender, EventArgs? e)
        {
            try
            {
                if (Control_TransferTab_DataGridView_Main.SelectedRows.Count == 1)
                {
                    DataGridViewRow row = Control_TransferTab_DataGridView_Main.SelectedRows[0];
                    if (row.DataBoundItem is DataRowView drv)
                    {
                        Service_DebugTracer.TraceUIAction("SELECTION_REPORT_UPDATE", nameof(Control_TransferTab),
                            new Dictionary<string, object>
                            {
                                ["PartID"] = drv["PartID"]?.ToString() ?? "",
                                ["Location"] = drv["Location"]?.ToString() ?? "",
                                ["Quantity"] = drv["Quantity"]?.ToString() ?? ""
                            });

                        // Populate selection report
                        Control_TransferTab_TextBox_Report_PartID.Text = drv["PartID"]?.ToString() ?? "";
                        Control_TransferTab_TextBox_Report_Operation.Text = drv["Operation"]?.ToString() ?? "";
                        Control_TransferTab_TextBox_Report_FromLocation.Text = drv["Location"]?.ToString() ?? "";
                        Control_TransferTab_TextBox_Report_ToLocation.Text = Control_TransferTab_ComboBox_ToLocation.Text;
                        Control_TransferTab_TextBox_Report_Quantity.Text = drv["Quantity"]?.ToString() ?? "";
                        Control_TransferTab_TextBox_Report_BatchNumber.Text = drv["BatchNumber"]?.ToString() ?? "";
                    }
                }
                else
                {
                    ClearSelectionReport();
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                    controlName: "Control_TransferTab_SelectionReport");
            }
        }

        #endregion

        #region Smart Search Processing

        /// <summary>
        /// Parse smart search text into search criteria
        /// </summary>
        private Dictionary<string, string> ParseSmartSearchText(string searchText)
        {
            var criteria = new Dictionary<string, string>();
            
            // Support syntax like: partid:PART123, qty:>50, location:A1
            var terms = searchText.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string term in terms)
            {
                if (term.Contains(':'))
                {
                    var parts = term.Split(':', 2);
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim().ToLower();
                        string value = parts[1].Trim();
                        criteria[key] = value;
                    }
                }
                else
                {
                    // Default search in part ID or general search
                    criteria["general"] = term.Trim();
                }
            }

            return criteria;
        }

        /// <summary>
        /// Execute smart search with parsed criteria
        /// </summary>
        private async Task ExecuteSmartSearchAsync(Dictionary<string, string> criteria)
        {
            Service_DebugTracer.TraceBusinessLogic("SMART_SEARCH_EXECUTION",
                inputData: criteria,
                outputData: new { CriteriaCount = criteria.Count });

            try
            {
                _progressHelper?.UpdateProgress(40, "Executing search query...");

                // Build search parameters based on criteria
                string partId = "";
                string operation = "";
                string location = "";
                
                if (criteria.ContainsKey("partid") || criteria.ContainsKey("part"))
                {
                    partId = criteria.ContainsKey("partid") ? criteria["partid"] : criteria["part"];
                }
                else if (criteria.ContainsKey("general"))
                {
                    partId = criteria["general"];
                }
                else if (!string.IsNullOrWhiteSpace(Control_TransferTab_ComboBox_Part.Text) && 
                         Control_TransferTab_ComboBox_Part.SelectedIndex > 0)
                {
                    partId = Control_TransferTab_ComboBox_Part.Text;
                }

                if (criteria.ContainsKey("operation") || criteria.ContainsKey("op"))
                {
                    operation = criteria.ContainsKey("operation") ? criteria["operation"] : criteria["op"];
                }
                else if (Control_TransferTab_ComboBox_Operation.SelectedIndex > 0)
                {
                    operation = Control_TransferTab_ComboBox_Operation.Text;
                }

                if (criteria.ContainsKey("location") || criteria.ContainsKey("loc"))
                {
                    location = criteria.ContainsKey("location") ? criteria["location"] : criteria["loc"];
                }
                else if (Control_TransferTab_ComboBox_FromLocation.SelectedIndex > 0)
                {
                    location = Control_TransferTab_ComboBox_FromLocation.Text;
                }

                DataTable results;
                if (!string.IsNullOrWhiteSpace(partId))
                {
                    if (!string.IsNullOrWhiteSpace(operation))
                    {
                        results = await Dao_Inventory.GetInventoryByPartIdAndOperationAsync(partId, operation, true);
                    }
                    else
                    {
                        results = await Dao_Inventory.GetInventoryByPartIdAsync(partId, true);
                    }
                }
                else
                {
                    // General search without specific part
                    results = new DataTable(); // This would be enhanced with more general search capabilities
                }

                _progressHelper?.UpdateProgress(70, "Applying filters and displaying results...");
                
                // Apply additional filters if needed (location, quantity, etc.)
                if (!string.IsNullOrWhiteSpace(location) && results.Rows.Count > 0)
                {
                    var filteredRows = results.AsEnumerable()
                        .Where(row => row.Field<string>("Location")?.ToLower().Contains(location.ToLower()) == true);
                    
                    if (filteredRows.Any())
                    {
                        results = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        results.Clear();
                    }
                }

                await DisplaySearchResultsAsync(results);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: "Control_TransferTab_SmartSearchExecution");
                throw;
            }
        }

        /// <summary>
        /// Display search results in the DataGridView
        /// </summary>
        private async Task DisplaySearchResultsAsync(DataTable results)
        {
            await Task.Run(() =>
            {
                this.Invoke((Action)(() =>
                {
                    DataGridView dgv = Control_TransferTab_DataGridView_Main;
                    dgv.SuspendLayout();
                    dgv.DataSource = results;

                    // Show only relevant columns in proper order
                    string[] columnsToShowArr = { "Location", "PartID", "Operation", "Quantity", "BatchNumber", "Notes" };
                    HashSet<string> columnsToShow = new(columnsToShowArr);
                    
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        column.Visible = columnsToShow.Contains(column.Name);
                    }

                    // Reorder columns
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
                            dgv.ClearSelection();
                            dgv.Rows[0].Selected = true;
                        }
                    }

                    dgv.ResumeLayout();
                }));
            });
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
            _progressHelper?.ShowProgress();
            _progressHelper?.UpdateProgress(10, "Resetting Transfer tab...");

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

                _progressHelper?.UpdateProgress(100, "Reset complete");
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
                _progressHelper?.HideProgress();
            }
        }

        private async Task Control_TransferTab_HardReset()
        {
            Control_TransferTab_Button_Reset.Enabled = false;
            try
            {
                _progressHelper?.ShowProgress();
                _progressHelper?.UpdateProgress(10, "Resetting Transfer tab...");
                Debug.WriteLine("[DEBUG] TransferTab HardReset - start");
                if (MainFormInstance != null)
                {
                    Debug.WriteLine("[DEBUG] Updating status strip for hard reset");
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
                }

                _progressHelper?.UpdateProgress(30, "Resetting data tables...");
                Debug.WriteLine("[DEBUG] Hiding ComboBoxes");

                Debug.WriteLine("[DEBUG] Resetting and refreshing all ComboBox DataTables");
                await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
                Debug.WriteLine("[DEBUG] DataTables reset complete");

                _progressHelper?.UpdateProgress(60, "Refilling combo boxes...");
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
                    _progressHelper?.HideProgress();
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
                _progressHelper?.ShowProgress();
                _progressHelper?.UpdateProgress(10, "Searching inventory...");

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
                    _progressHelper?.UpdateProgress(40,
                        "Querying by part and operation...");
                    results = await Dao_Inventory.GetInventoryByPartIdAndOperationAsync(partId, op, true);
                }
                else
                {
                    LoggingUtility.Log($"Searching inventory for Part ID: {partId} without specific operation.");
                    _progressHelper?.UpdateProgress(40, "Querying by part...");
                    results = await Dao_Inventory.GetInventoryByPartIdAsync(partId, true);
                }

                _progressHelper?.UpdateProgress(70, "Updating results...");
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
                _progressHelper?.UpdateProgress(100, "Search complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_TransferTab_Button_Search_Click").ToString());
            }
            finally
            {
                _progressHelper?.HideProgress();
            }
        }

        private async Task Control_TransferTab_Button_Save_ClickAsync(object? sender, EventArgs? e)
        {
            try
            {
                // Show progress bar at the start
                _progressHelper?.ShowProgress();
                _progressHelper?.UpdateProgress(10, "Transferring inventory...");

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

                _progressHelper?.UpdateProgress(40, "Processing transfer...");

                if (selectedRows.Count == 1)
                {
                    await TransferSingleRowAsync(selectedRows[0]);
                }
                else
                {
                    await TransferMultipleRowsAsync(selectedRows);
                }

                _progressHelper?.UpdateProgress(80, "Refreshing results...");
                Control_TransferTab_Button_Search_Click(null, null);

                _progressHelper?.UpdateProgress(100, "Transfer complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_TransferTab_Button_Transfer_Click").ToString());
            }
            finally
            {
                _progressHelper?.HideProgress();
            }
        }

        private void Control_TransferTab_Button_Print_Click(object? sender, EventArgs? e)
        {
            // Show progress bar at the start
            _progressHelper?.ShowProgress();
            _progressHelper?.UpdateProgress(10, "Preparing print...");

            try
            {
                if (Control_TransferTab_DataGridView_Main.Rows.Count == 0)
                {
                    MessageBox.Show(@"No data to print.", @"Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Get visible column names for print
                List<string> visibleColumns = new();
                foreach (DataGridViewColumn col in Control_TransferTab_DataGridView_Main.Columns)
                {
                    if (col.Visible)
                    {
                        visibleColumns.Add(col.Name);
                    }
                }

                Core_DgvPrinter printer = new();
                Control_TransferTab_DataGridView_Main.Tag = Control_TransferTab_ComboBox_Part.Text;
                // Set visible columns for print
                printer.SetPrintVisibleColumns(visibleColumns);
                _progressHelper?.UpdateProgress(60, "Printing...");
                printer.Print(Control_TransferTab_DataGridView_Main);
                _progressHelper?.UpdateProgress(100, "Print complete");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($@"Print failed: {ex.Message}", @"Print Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _progressHelper?.HideProgress();
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

        #region Privileges

        /// <summary>
        /// Apply user privileges to the enhanced transfer interface
        /// </summary>
        private void ApplyPrivileges()
        {
            bool isAdmin = Model_AppVariables.UserTypeAdmin;
            bool isNormal = Model_AppVariables.UserTypeNormal;
            bool isReadOnly = Model_AppVariables.UserTypeReadOnly;

            Service_DebugTracer.TraceBusinessLogic("USER_PRIVILEGE_APPLICATION",
                inputData: new { isAdmin, isNormal, isReadOnly },
                outputData: new { ControlsConfigured = "Transfer Tab Enhanced UI" });

            // Input Controls
            Control_TransferTab_ComboBox_Part.Enabled = isAdmin || isNormal || isReadOnly;
            Control_TransferTab_ComboBox_Operation.Enabled = isAdmin || isNormal || isReadOnly;
            Control_TransferTab_ComboBox_FromLocation.Enabled = isAdmin || isNormal || isReadOnly;
            Control_TransferTab_ComboBox_ToLocation.Enabled = isAdmin || isNormal || isReadOnly;
            Control_TransferTab_ComboBox_SortBy.Enabled = isAdmin || isNormal || isReadOnly;
            
            // Smart Search
            Control_TransferTab_TextBox_SmartSearch.Enabled = isAdmin || isNormal || isReadOnly;
            Control_TransferTab_Button_SmartSearch.Enabled = isAdmin || isNormal || isReadOnly;
            
            // NumericUpDown
            Control_TransferTab_NumericUpDown_Quantity.ReadOnly = isReadOnly;
            Control_TransferTab_NumericUpDown_Quantity.Enabled = isAdmin || isNormal || isReadOnly;
            
            // DataGridView
            Control_TransferTab_DataGridView_Main.ReadOnly = isReadOnly;
            Control_TransferTab_DataGridView_Main.Enabled = isAdmin || isNormal || isReadOnly;
            
            // Action Buttons - Main operations
            Control_TransferTab_Button_Transfer.Visible = isAdmin || isNormal;
            Control_TransferTab_Button_Transfer.Enabled = isAdmin || isNormal;
            
            // Utility Buttons - Always available
            Control_TransferTab_Button_Reset.Visible = true;
            Control_TransferTab_Button_Reset.Enabled = true;
            Control_TransferTab_Button_Search.Visible = true;
            Control_TransferTab_Button_Search.Enabled = true;
            Control_TransferTab_Button_Print.Enabled = isAdmin || isNormal || isReadOnly;
            
            // Interface Controls
            Control_TransferTab_Button_SidePanel.Enabled = true;
            Control_TransferTab_Button_SelectionHistory.Enabled = isAdmin || isNormal || isReadOnly;
            Control_TransferTab_Button_Previous.Enabled = isAdmin || isNormal || isReadOnly;
            Control_TransferTab_Button_Next.Enabled = isAdmin || isNormal || isReadOnly;

            // For Read-Only users, hide transfer capability
            if (isReadOnly)
            {
                Control_TransferTab_Button_Transfer.Visible = false;
                Control_TransferTab_Button_Transfer.Enabled = false;
            }
        }

        #endregion

        #region Helper Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetComboBoxForeColor(ComboBox combo, bool valid) =>
            combo.ForeColor = valid
                ? Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black
                : Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;

        #endregion

        #region Cleanup

        /// <summary>
        /// Update button states based on current selection and input
        /// </summary>
        private void Control_TransferTab_Update_ButtonStates()
        {
            try
            {
                Control_TransferTab_Button_Search.Enabled = Control_TransferTab_ComboBox_Part.SelectedIndex > 0 ||
                                                          !string.IsNullOrWhiteSpace(Control_TransferTab_TextBox_SmartSearch.Text);
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
                    hasData && hasSelection && hasToLocation && hasQuantity && !toLocationIsSameAsRow;
                
                // Smart search button
                Control_TransferTab_Button_SmartSearch.Enabled = !string.IsNullOrWhiteSpace(Control_TransferTab_TextBox_SmartSearch.Text) ||
                                                               Control_TransferTab_ComboBox_Part.SelectedIndex > 0;
                
                // Print and history buttons
                if (Control_TransferTab_Button_Print != null)
                    Control_TransferTab_Button_Print.Enabled = hasData;
                Control_TransferTab_Button_SelectionHistory.Enabled = true; // Always enabled
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                    controlName: "Control_TransferTab_Update_ButtonStates");
            }
        }

        #endregion
    }

    #endregion
}
