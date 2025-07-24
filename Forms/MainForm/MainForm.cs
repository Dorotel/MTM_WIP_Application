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
        private Control_ProgressBarUserControl _tabLoadingControlProgress = null!;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Service_ConnectionRecoveryManager ConnectionRecoveryManager { get; private set; } = null!;

        public Control_ProgressBarUserControl TabLoadingControlProgress => _tabLoadingControlProgress;

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

                // Set the form title with user and privilege info
                string privilege = "Unknown";
                if (Model_AppVariables.UserTypeAdmin)
                {
                    privilege = "Administrator";
                }
                else if (Model_AppVariables.UserTypeNormal)
                {
                    privilege = "Normal User";
                }
                else if (Model_AppVariables.UserTypeReadOnly)
                {
                    privilege = "Read Only";
                }

                Text =
                    $"Manitowoc Tool and Manufacturing WIP Inventory System | {Model_AppVariables.User} | {privilege}";

                InitializeProgressControl();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] Progress control initialized.");

                ConnectionStrengthChecker = new Helper_Control_MySqlSignal();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] ConnectionStrengthChecker initialized.");

                ConnectionRecoveryManager = new Service_ConnectionRecoveryManager(this);
                Debug.WriteLine("[DEBUG] [MainForm.ctor] ConnectionRecoveryManager initialized.");

                MainForm_OnStartup_SetupConnectionStrengthControl();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] ConnectionStrengthControl setup complete.");

                MainForm_OnStartup_WireUpEvents();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] Events wired up.");

                // Wire up DPI change handling for runtime DPI awareness
                MainForm_OnStartup_WireUpDpiChangeEvents();
                Debug.WriteLine("[DEBUG] [MainForm.ctor] DPI change events wired up.");

                Shown += async (s, e) =>
                {
                    Debug.WriteLine("[DEBUG] [MainForm.ctor] MainForm Shown event triggered.");
                    await MainForm_OnStartup_GetUserFullNameAsync();
                    Debug.WriteLine("[DEBUG] [MainForm.ctor] User full name loaded.");
                    await Task.Delay(500);
                    if (MainForm_UserControl_InventoryTab != null)
                    {
                        MainForm_UserControl_InventoryTab.Control_InventoryTab_ComboBox_Part.Focus();
                        MainForm_UserControl_InventoryTab.Control_InventoryTab_ComboBox_Part.SelectAll();
                        MainForm_UserControl_InventoryTab.Control_InventoryTab_ComboBox_Part.BackColor =
                            Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
                    }

                    Debug.WriteLine("[DEBUG] [MainForm.ctor] MainForm is now idle and ready.");
                };
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

        #region Methods

        private void InitializeProgressControl()
        {
            try
            {
                _tabLoadingControlProgress = new Control_ProgressBarUserControl
                {
                    Size = new Size(300, 120),
                    Visible = false,
                    Anchor = AnchorStyles.None,
                    StatusText = "Loading tab..."
                };

                _tabLoadingControlProgress.Location = new Point(
                    (MainForm_TabControl.Width - _tabLoadingControlProgress.Width) / 2,
                    (MainForm_TabControl.Height - _tabLoadingControlProgress.Height) / 2
                );

                Controls.Add(_tabLoadingControlProgress);
                _tabLoadingControlProgress.BringToFront();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        // Ensures all user controls with a right panel toggle button have the correct text for the current split state
        private void UpdateQuickButtonsToggleTextForAllTabs()
        {
            bool isCollapsed = MainForm_SplitContainer_Middle.Panel2Collapsed;
            string text = isCollapsed ? "⬅️" : "➡️";

            // Inventory Tab
            if (MainForm_UserControl_InventoryTab != null)
            {
                var field = MainForm_UserControl_InventoryTab.GetType().GetField("Control_InventoryTab_Button_Toggle_RightPanel", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field?.GetValue(MainForm_UserControl_InventoryTab) is Button btn)
                {
                    btn.Text = text;
                }
            }
            // Remove Tab
            if (MainForm_UserControl_RemoveTab != null)
            {
                var field = MainForm_UserControl_RemoveTab.GetType().GetField("Control_RemoveTab_Button_Toggle_RightPanel", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field?.GetValue(MainForm_UserControl_RemoveTab) is Button btn)
                {
                    btn.Text = text;
                }
            }
            // Transfer Tab
            if (MainForm_UserControl_TransferTab != null)
            {
                var field = MainForm_UserControl_TransferTab.GetType().GetField("Control_TransferTab_Button_Toggle_RightPanel", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field?.GetValue(MainForm_UserControl_TransferTab) is Button btn)
                {
                    btn.Text = text;
                }
            }
        }

        #endregion

        #region Startup / Events

        private void MainForm_OnStartup_WireUpEvents()
        {
            MainForm_TabControl.SelectedIndexChanged += (s, e) =>
            {
                MainForm_TabControl_SelectedIndexChanged(null!, null!);
            };
            MainForm_TabControl.Selecting += MainForm_TabControl_Selecting!;
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
                try
                {
                    Model_AppVariables.UserFullName =
                        await Dao_User.GetUserFullNameAsync(Model_AppVariables.User, true);

                    if (string.IsNullOrEmpty(Model_AppVariables.UserFullName))
                    {
                        Model_AppVariables.UserFullName =
                            Model_AppVariables.User;
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                        "MainForm / " + "MainForm_OnStartup_GetUserFullNameAsync / " + "GetUserFullNameAsync");
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    "MainForm / " + "MainForm_OnStartup_GetUserFullNameAsync");
            }
        }

        private void MainForm_OnStartup_SetupConnectionStrengthControl()
        {
            try
            {
                _connectionStrengthTimer = new Timer { Interval = 5000 };
                _connectionStrengthTimer.Tick +=
                    async (s, e) => await ConnectionRecoveryManager.UpdateConnectionStrengthAsync();
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
            Control_AdvancedInventory? advancedInvTab = MainForm_UserControl_AdvancedInventory;
            Control_AdvancedRemove? advancedRemoveTab = MainForm_UserControl_AdvancedRemove;

            if ((advancedInvTab != null && advancedInvTab.Visible) ||
                (advancedRemoveTab != null && advancedRemoveTab.Visible))
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

        private async void MainForm_TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await ShowTabLoadingProgressAsync();
                Debug.WriteLine("Resetting");
                // Call all SoftReset methods on all user controls
                UserControl[] allUserControls = new UserControl[]
                {
                    MainForm_UserControl_InventoryTab, MainForm_UserControl_AdvancedInventory,
                    MainForm_UserControl_RemoveTab, MainForm_UserControl_AdvancedRemove,
                    MainForm_UserControl_TransferTab
                };

                foreach (UserControl ctrl in allUserControls)
                {
                    Debug.WriteLine(ctrl?.ToString());
                    if (ctrl == null)
                    {
                        continue;
                    }

                    string[] methods = new[]
                    {
                        "Control_InventoryTab_SoftReset", "Control_AdvancedInventory_SoftReset",
                        "Control_RemoveTab_SoftReset", "Control_AdvancedRemove_SoftReset",
                        "Control_TransferTab_SoftReset"
                    };
                    foreach (string methodName in methods)
                    {
                        MethodInfo? method = ctrl.GetType().GetMethod(methodName,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (method != null)
                        {
                            Debug.WriteLine($"Invoking {method.Name} on {ctrl.GetType().Name}");
                            method.Invoke(ctrl, null);
                        }
                        else
                        {
                            Debug.WriteLine($"Method {methodName} not found on {ctrl.GetType().Name}");
                        }
                    }
                }

                // Update Quick Buttons toggle text for all tabs
                UpdateQuickButtonsToggleTextForAllTabs();

                // Only handle visibility after resets
                switch (MainForm_TabControl.SelectedIndex)
                {
                    case 0:
                        if (MainForm_UserControl_InventoryTab is not null)
                        {
                            MainForm_UserControl_InventoryTab.Visible = true;
                            if (MainForm_UserControl_AdvancedInventory is not null)
                            {
                                MainForm_UserControl_AdvancedInventory.Visible = false;
                            }
                        }

                        break;
                    case 1:
                        if (MainForm_UserControl_RemoveTab is not null)
                        {
                            MainForm_UserControl_RemoveTab.Visible = true;
                            if (MainForm_UserControl_AdvancedRemove is not null)
                            {
                                MainForm_UserControl_AdvancedRemove.Visible = false;
                            }
                        }

                        break;
                    case 2:
                        if (MainForm_UserControl_TransferTab is not null)
                        {
                            MainForm_UserControl_TransferTab.Visible = true;
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    nameof(MainForm_TabControl_SelectedIndexChanged));
            }
            finally
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

                HideTabLoadingProgress();
            }
        }

        private async Task ShowTabLoadingProgressAsync()
        {
            try
            {
                if (_tabLoadingControlProgress != null)
                {
                    _tabLoadingControlProgress.Location = new Point(
                        (MainForm_TabControl.Width - _tabLoadingControlProgress.Width) / 2,
                        (MainForm_TabControl.Height - _tabLoadingControlProgress.Height) / 2
                    );

                    _tabLoadingControlProgress.ShowProgress();
                    _tabLoadingControlProgress.UpdateProgress(25, "Switching tab...");

                    await Task.Delay(100);
                    _tabLoadingControlProgress.UpdateProgress(50, "Loading controls...");

                    await Task.Delay(100);
                    _tabLoadingControlProgress.UpdateProgress(75, "Applying settings...");

                    await Task.Delay(100);
                    _tabLoadingControlProgress.UpdateProgress(100, "Ready");
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
                _tabLoadingControlProgress?.HideProgress();
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
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm / " + "OnFormClosing");
            }

            base.OnFormClosing(e);
        }

        #endregion

        private void MainForm_MenuStrip_File_Settings_Click(object sender, EventArgs e)
        {
            using SettingsForm settingsForm = new();
            if (settingsForm.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            MainForm_UserControl_InventoryTab?.Control_InventoryTab_HardReset();
            Core_Themes.ApplyTheme(this);
        }

        private void MainForm_MenuStrip_Exit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Exit Application",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void MainForm_MenuStrip_View_PersonalHistory_Click(object sender, EventArgs e)
        {
            string connectionString = Model_AppVariables.ConnectionString;
            string currentUser = Model_AppVariables.User;

            Transactions.Transactions transactionsForm = new(connectionString, currentUser);
            transactionsForm.ShowDialog(this);
        }
    }

    #endregion

    #endregion
}
