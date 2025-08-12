using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;
using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Controls.Shared;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.Settings;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
using MySql.Data.MySqlClient;
using Timer = System.Windows.Forms.Timer;

namespace MTM_Inventory_Application.Forms.MainForm
{
    #region MainForm

    public partial class MainForm : Form
    {
        #region Fields

        private Timer? _connectionStrengthTimer;
        public Helper_Control_MySqlSignal ConnectionStrengthChecker = null!;
        private Helper_StoredProcedureProgress? _progressHelper;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Service_ConnectionRecoveryManager ConnectionRecoveryManager { get; private set; } = null!;

        public Helper_StoredProcedureProgress? ProgressHelper => _progressHelper;

        private CancellationTokenSource? _batchCancelTokenSource;

        #endregion

        #region Constructors

        #region Initialization

        public MainForm()
        {
            Debug.WriteLine("[DEBUG] [MainForm.ctor] Constructing MainForm...");
            try
            {
                InitializeComponent();
                AutoScaleMode = AutoScaleMode.Dpi;

                // Apply comprehensive DPI scaling and runtime layout adjustments
                // THEME POLICY: Only update theme on startup, in settings menu, or on DPI change.
                // Do NOT call theme update methods from arbitrary event handlers or business logic.
                Core_Themes.ApplyDpiScaling(this); // Allowed: Form initialization
                Core_Themes.ApplyRuntimeLayoutAdjustments(this); // Allowed: Form initialization

                Debug.WriteLine("[DEBUG] [MainForm.ctor] InitializeComponent complete.");

                InitializeFormTitle();
                InitializeProgressControl();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] Progress control initialized.");

                ConnectionStrengthChecker = new Helper_Control_MySqlSignal();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] ConnectionStrengthChecker initialized.");

                ConnectionRecoveryManager = new Service_ConnectionRecoveryManager(this);
                Debug.WriteLine("[DEBUG] [MainForm.ctor] ConnectionRecoveryManager initialized.");

                InitializeStartupComponents();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] Startup components initialized.");

                WireUpFormShownEvent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DEBUG] [MainForm.ctor] Exception: {ex}");
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(MainForm));
            }

            Debug.WriteLine("[DEBUG] [MainForm.ctor] MainForm constructed.");
        }

        #endregion

        #region Initialization Methods

        private void InitializeFormTitle()
        {
            try
            {
                string privilege = GetUserPrivilegeDisplayText();
                Text = $"Manitowoc Tool and Manufacturing WIP Inventory System | {Model_AppVariables.User} | {privilege}";
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private static string GetUserPrivilegeDisplayText()
        {
            if (Model_AppVariables.UserTypeAdmin)
                return "Administrator";
            if (Model_AppVariables.UserTypeNormal)
                return "Normal User";
            if (Model_AppVariables.UserTypeReadOnly)
                return "Read Only";
            
            return "Unknown";
        }

        private void InitializeStartupComponents()
        {
            try
            {
                MainForm_OnStartup_SetupConnectionStrengthControl();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] ConnectionStrengthControl setup complete.");

                MainForm_OnStartup_WireUpEvents();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] Events wired up.");

                // Wire up DPI change handling for runtime DPI awareness
                MainForm_OnStartup_WireUpDpiChangeEvents();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] DPI change events wired up.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        private void WireUpFormShownEvent()
        {
            try
            {
                Shown += async (s, e) =>
                {
                    try
                    {
                        Debug.WriteLine("[DEBUG] [MainForm.ctor] MainForm Shown event triggered.");
                        await MainForm_OnStartup_GetUserFullNameAsync();
                        Debug.WriteLine("[DEBUG] [MainForm.ctor] User full name loaded.");
                        
                        await Task.Delay(500);
                        SetInitialFocusToInventoryTab();
                        
                        Debug.WriteLine("[DEBUG] [MainForm.ctor] MainForm is now idle and ready.");
                    }
                    catch (Exception ex)
                    {
                        LoggingUtility.LogApplicationError(ex);
                        await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "MainForm_Shown_Event");
                    }
                };
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        private void SetInitialFocusToInventoryTab()
        {
            try
            {
                if (MainForm_UserControl_InventoryTab?.Control_InventoryTab_ComboBox_Part != null)
                {
                    MainForm_UserControl_InventoryTab.Control_InventoryTab_ComboBox_Part.Focus();
                    MainForm_UserControl_InventoryTab.Control_InventoryTab_ComboBox_Part.SelectAll();
                    MainForm_UserControl_InventoryTab.Control_InventoryTab_ComboBox_Part.BackColor =
                        Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private void InitializeProgressControl()
        {
            try
            {
                // Initialize progress helper using StatusStrip components
                _progressHelper = Helper_StoredProcedureProgress.Create(
                    MainForm_ProgressBar,
                    MainForm_StatusText,
                    this);
                
                // Initialize progress controls for all UserControls
                InitializeUserControlsProgress();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private void InitializeUserControlsProgress()
        {
            try
            {
                // Set progress controls for main tab UserControls
                MainForm_UserControl_InventoryTab?.SetProgressControls(MainForm_ProgressBar, MainForm_StatusText);
                MainForm_UserControl_RemoveTab?.SetProgressControls(MainForm_ProgressBar, MainForm_StatusText);
                MainForm_UserControl_TransferTab?.SetProgressControls(MainForm_ProgressBar, MainForm_StatusText);
                
                // Set progress controls for advanced UserControls
                MainForm_UserControl_AdvancedInventory?.SetProgressControls(MainForm_ProgressBar, MainForm_StatusText);
                MainForm_UserControl_AdvancedRemove?.SetProgressControls(MainForm_ProgressBar, MainForm_StatusText);
                
                // Set progress controls for QuickButtons
                MainForm_UserControl_QuickButtons?.SetProgressControls(MainForm_ProgressBar, MainForm_StatusText);
                
                Debug.WriteLine("[DEBUG] [MainForm] UserControl progress helpers initialized.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        #endregion

        #region Toggle Panel Methods

        // Interface-based approach replacing reflection for toggle button text updates
        private void UpdateQuickButtonsToggleTextForAllTabs()
        {
            try
            {
                bool isCollapsed = MainForm_SplitContainer_Middle.Panel2Collapsed;
                string text = isCollapsed ? "⬅️" : "➡️";

                UpdateToggleButtonText(MainForm_UserControl_InventoryTab, text);
                UpdateToggleButtonText(MainForm_UserControl_RemoveTab, text);
                UpdateToggleButtonText(MainForm_UserControl_TransferTab, text);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private static void UpdateToggleButtonText(Control? control, string text)
        {
            if (control == null) return;

            try
            {
                // Use safe control search instead of reflection
                var toggleButtons = control.Controls.Find("*Toggle_RightPanel", true)
                    .OfType<Button>()
                    .Where(b => b.Name.Contains("Toggle_RightPanel"));

                foreach (Button btn in toggleButtons)
                {
                    btn.Text = text;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                Debug.WriteLine($"[DEBUG] Error updating toggle button text for {control.Name}: {ex.Message}");
            }
        }

        #endregion

        #region Startup / Events

        private void MainForm_OnStartup_WireUpEvents()
        {
            try
            {
                MainForm_TabControl.SelectedIndexChanged += (s, e) =>
                {
                    try
                    {
                        MainForm_TabControl_SelectedIndexChanged(s, e);
                    }
                    catch (Exception ex)
                    {
                        LoggingUtility.LogApplicationError(ex);
                        _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_TabControl_SelectedIndexChanged_Handler");
                    }
                };
                MainForm_TabControl.Selecting += MainForm_TabControl_Selecting;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                throw;
            }
        }

        /// <summary>
        /// Wires up DPI change event handling for runtime DPI awareness.
        /// This ensures the application responds properly to DPI changes when moving between monitors
        /// or when the user changes system DPI settings.
        /// </summary>
        private void MainForm_OnStartup_WireUpDpiChangeEvents()
        {
            try
            {
                // Handle DPI changes when form is moved between monitors or DPI settings change
                DpiChanged += MainForm_DpiChanged;

                // Handle system DPI changes
                SystemEvents.DisplaySettingsChanged += (s, e) =>
                {
                    try
                    {
                        // Refresh DPI scaling for all forms when display settings change
                        Core_Themes.RefreshDpiScalingForAllForms();
                        LoggingUtility.Log("Display settings changed - DPI scaling refreshed");
                    }
                    catch (Exception ex)
                    {
                        LoggingUtility.LogApplicationError(ex);
                    }
                };

                LoggingUtility.Log("DPI change event handlers wired up successfully");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                Debug.WriteLine($"[DEBUG] Error wiring up DPI change events: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles DPI changes for the main form and all its controls.
        /// </summary>
        private void MainForm_DpiChanged(object? sender, DpiChangedEventArgs e)
        {
            try
            {
                LoggingUtility.Log($"MainForm DPI changed from {e.DeviceDpiOld} to {e.DeviceDpiNew}");

                // Use the Core_Themes DPI change handler
                Core_Themes.HandleDpiChanged(this, e.DeviceDpiOld, e.DeviceDpiNew);

                // Reapply theme after DPI change to ensure proper color scaling
                Core_Themes.ApplyTheme(this);

                LoggingUtility.Log("MainForm DPI change handling completed");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                Debug.WriteLine($"[DEBUG] Error handling DPI change: {ex.Message}");
            }
        }

        private static async Task MainForm_OnStartup_GetUserFullNameAsync()
        {
            try
            {
                Model_AppVariables.UserFullName =
                    await Dao_User.GetUserFullNameAsync(Model_AppVariables.User, true);

                if (string.IsNullOrEmpty(Model_AppVariables.UserFullName))
                {
                    Model_AppVariables.UserFullName = Model_AppVariables.User;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    "MainForm_OnStartup_GetUserFullNameAsync");
            }
        }

        private void MainForm_OnStartup_SetupConnectionStrengthControl()
        {
            try
            {
                _connectionStrengthTimer = new Timer { Interval = 5000 };
                _connectionStrengthTimer.Tick += async (s, e) =>
                {
                    try
                    {
                        await ConnectionRecoveryManager.UpdateConnectionStrengthAsync();
                    }
                    catch (Exception ex)
                    {
                        LoggingUtility.LogApplicationError(ex);
                    }
                };
                _connectionStrengthTimer.Start();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    nameof(MainForm_OnStartup_SetupConnectionStrengthControl));
            }
        }

        #endregion

        #region Tab Control

        private void MainForm_TabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                Control_AdvancedInventory? advancedInvTab = MainForm_UserControl_AdvancedInventory;
                Control_AdvancedRemove? advancedRemoveTab = MainForm_UserControl_AdvancedRemove;

                if ((advancedInvTab?.Visible == true) || (advancedRemoveTab?.Visible == true))
                {
                    DialogResult result = MessageBox.Show(
                        @"If you change the current tab now, any work will be lost.",
                        @"Warning",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                    );
                    if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(MainForm_TabControl_Selecting));
            }
        }

        private async void MainForm_TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await ShowTabLoadingProgressAsync();
                Debug.WriteLine("Resetting user controls...");

                await ResetAllUserControlsAsync();

                // Update Quick Buttons toggle text for all tabs
                UpdateQuickButtonsToggleTextForAllTabs();

                SetTabVisibility();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    nameof(MainForm_TabControl_SelectedIndexChanged));
            }
            finally
            {
                SetFocusForCurrentTab();
                HideTabLoadingProgress();
            }
        }

        #region Tab Control Helper Methods

        private async Task ResetAllUserControlsAsync()
        {
            try
            {
                var resetTasks = new List<Task>();

                // Create reset tasks for each user control
                if (MainForm_UserControl_InventoryTab != null)
                    resetTasks.Add(Task.Run(() => InvokeResetMethod(MainForm_UserControl_InventoryTab, "Control_InventoryTab_SoftReset")));

                if (MainForm_UserControl_AdvancedInventory != null)
                    resetTasks.Add(Task.Run(() => InvokeResetMethod(MainForm_UserControl_AdvancedInventory, "Control_AdvancedInventory_SoftReset")));

                if (MainForm_UserControl_RemoveTab != null)
                    resetTasks.Add(Task.Run(() => InvokeResetMethod(MainForm_UserControl_RemoveTab, "Control_RemoveTab_SoftReset")));

                if (MainForm_UserControl_AdvancedRemove != null)
                    resetTasks.Add(Task.Run(() => InvokeResetMethod(MainForm_UserControl_AdvancedRemove, "Control_AdvancedRemove_SoftReset")));

                if (MainForm_UserControl_TransferTab != null)
                    resetTasks.Add(Task.Run(() => InvokeResetMethod(MainForm_UserControl_TransferTab, "Control_TransferTab_SoftReset")));

                // Execute all reset tasks concurrently
                await Task.WhenAll(resetTasks);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                Debug.WriteLine($"[DEBUG] Error resetting user controls: {ex.Message}");
            }
        }

        private static void InvokeResetMethod(UserControl control, string methodName)
        {
            try
            {
                Debug.WriteLine($"Attempting to invoke {methodName} on {control.GetType().Name}");
                
                MethodInfo? method = control.GetType().GetMethod(methodName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                
                if (method != null)
                {
                    Debug.WriteLine($"Invoking {method.Name} on {control.GetType().Name}");
                    
                    // Ensure method is invoked on UI thread if needed
                    if (control.InvokeRequired)
                    {
                        control.Invoke(new Action(() => method.Invoke(control, null)));
                    }
                    else
                    {
                        method.Invoke(control, null);
                    }
                }
                else
                {
                    Debug.WriteLine($"Method {methodName} not found on {control.GetType().Name}");
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                Debug.WriteLine($"[DEBUG] Error invoking {methodName} on {control.GetType().Name}: {ex.Message}");
            }
        }

        private void SetTabVisibility()
        {
            try
            {
                // Only handle visibility after resets
                switch (MainForm_TabControl.SelectedIndex)
                {
                    case 0:
                        SetInventoryTabVisibility();
                        break;
                    case 1:
                        SetRemoveTabVisibility();
                        break;
                    case 2:
                        SetTransferTabVisibility();
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private void SetInventoryTabVisibility()
        {
            if (MainForm_UserControl_InventoryTab != null)
            {
                MainForm_UserControl_InventoryTab.Visible = true;
                if (MainForm_UserControl_AdvancedInventory != null)
                {
                    MainForm_UserControl_AdvancedInventory.Visible = false;
                }
            }
        }

        private void SetRemoveTabVisibility()
        {
            if (MainForm_UserControl_RemoveTab != null)
            {
                MainForm_UserControl_RemoveTab.Visible = true;
                if (MainForm_UserControl_AdvancedRemove != null)
                {
                    MainForm_UserControl_AdvancedRemove.Visible = false;
                }
            }
        }

        private void SetTransferTabVisibility()
        {
            if (MainForm_UserControl_TransferTab != null)
            {
                MainForm_UserControl_TransferTab.Visible = true;
            }
        }

        private void SetFocusForCurrentTab()
        {
            try
            {
                // Set focus to the main input control for the currently visible tab
                switch (MainForm_TabControl.SelectedIndex)
                {
                    case 0:
                        MainForm_UserControl_InventoryTab?.Control_InventoryTab_ComboBox_Part?.Focus();
                        break;
                    case 1:
                        MainForm_UserControl_RemoveTab?.Control_RemoveTab_ComboBox_Part?.Focus();
                        break;
                    case 2:
                        MainForm_UserControl_TransferTab?.Control_TransferTab_ComboBox_Part?.Focus();
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        #endregion

        private async Task ShowTabLoadingProgressAsync()
        {
            try
            {
                if (_progressHelper != null)
                {
                    _progressHelper.ShowProgress("Switching tab...");
                    _progressHelper.UpdateProgress(25, "Loading controls...");

                    await Task.Delay(100);
                    _progressHelper.UpdateProgress(50, "Applying settings...");

                    await Task.Delay(100);
                    _progressHelper.UpdateProgress(75, "Ready");

                    await Task.Delay(100);
                    _progressHelper.UpdateProgress(100, "Tab loaded");
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private void HideTabLoadingProgress()
        {
            try
            {
                _progressHelper?.HideProgress();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        #endregion

        #region Tab Shortcuts

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == Core_WipAppVariables.Shortcut_MainForm_Tab1)
                {
                    MainForm_TabControl.SelectedIndex = 0;
                    return true;
                }

                if (keyData == Core_WipAppVariables.Shortcut_MainForm_Tab2)
                {
                    MainForm_TabControl.SelectedIndex = 1;
                    return true;
                }

                if (keyData == Core_WipAppVariables.Shortcut_MainForm_Tab3)
                {
                    MainForm_TabControl.SelectedIndex = 2;
                    return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                return false;
            }
        }

        #endregion

        #region Form Closing

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                _connectionStrengthTimer?.Stop();
                _connectionStrengthTimer?.Dispose();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_OnFormClosing");
            }

            base.OnFormClosing(e);
        }

        #endregion

        #region Menu Event Handlers

        private void MainForm_MenuStrip_File_Settings_Click(object sender, EventArgs e)
        {
            try
            {
                using SettingsForm settingsForm = new();
                if (settingsForm.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                MainForm_UserControl_InventoryTab?.Control_InventoryTab_HardReset();
                Core_Themes.ApplyTheme(this);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(MainForm_MenuStrip_File_Settings_Click));
            }
        }

        private void MainForm_MenuStrip_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    @"Are you sure you want to exit?",
                    @"Exit Application",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(MainForm_MenuStrip_Exit_Click));
            }
        }

        private void MainForm_MenuStrip_View_PersonalHistory_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = Model_AppVariables.ConnectionString;
                string currentUser = Model_AppVariables.User;

                Transactions.Transactions transactionsForm = new(connectionString, currentUser);
                transactionsForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(MainForm_MenuStrip_View_PersonalHistory_Click));
            }
        }

        #endregion
    }

    #endregion
}
#endregion
