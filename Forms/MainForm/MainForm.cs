using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
using System.ComponentModel;
using MTM_Inventory_Application.Forms.Settings;
using MTM_Inventory_Application.Controls.Shared;
using MTM_Inventory_Application.Forms.Transactions;
using Timer = System.Windows.Forms.Timer;

namespace MTM_Inventory_Application.Forms.MainForm;

#region MainForm

public partial class MainForm : Form
{
    private Timer? _connectionStrengthTimer;
    public Helper_Control_MySqlSignal ConnectionStrengthChecker = null!;
    private ProgressBarUserControl _tabLoadingProgress = null!;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Service_ConnectionRecoveryManager ConnectionRecoveryManager { get; private set; } = null!;

    // Make the progress bar accessible to other controls
    public ProgressBarUserControl TabLoadingProgress => _tabLoadingProgress;

    #region Initialization

    public MainForm()
    {
        System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] Constructing MainForm...");
        try
        {
            // Before InitializeComponent
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] InitializeComponent complete.");

            // Initialize progress control
            InitializeProgressControl();
            System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] Progress control initialized.");

            // Do NOT apply user UI settings colors here; theme will be applied globally after construction
            ConnectionStrengthChecker = new Helper_Control_MySqlSignal();
            System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] ConnectionStrengthChecker initialized.");

            ConnectionRecoveryManager = new Service_ConnectionRecoveryManager(this);
            System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] ConnectionRecoveryManager initialized.");

            MainForm_OnStartup_SetupConnectionStrengthControl();
            System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] ConnectionStrengthControl setup complete.");

            MainForm_OnStartup_WireUpEvents();
            System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] Events wired up.");

            Shown += async (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] MainForm Shown event triggered.");
                await MainForm_OnStartup_GetUserFullNameAsync();
                System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] User full name loaded.");
                await Task.Delay(500); // Ensure controls are visible
                if (MainForm_Control_InventoryTab != null)
                {
                    MainForm_Control_InventoryTab.Control_InventoryTab_ComboBox_Part.Focus();
                    MainForm_Control_InventoryTab.Control_InventoryTab_ComboBox_Part.SelectAll();
                    MainForm_Control_InventoryTab.Control_InventoryTab_ComboBox_Part.BackColor =
                        Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
                }

                System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] MainForm is now idle and ready.");
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DEBUG] [MainForm.ctor] Exception: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(MainForm));
        }

        System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] MainForm constructed.");
    }

    private void InitializeProgressControl()
    {
        try
        {
            // Create and configure the progress control
            _tabLoadingProgress = new ProgressBarUserControl
            {
                Size = new Size(300, 120),
                Visible = false,
                Anchor = AnchorStyles.None,
                StatusText = "Loading tab..."
            };

            // Position the progress control over the tab control area
            _tabLoadingProgress.Location = new Point(
                (MainForm_TabControl.Width - _tabLoadingProgress.Width) / 2,
                (MainForm_TabControl.Height - _tabLoadingProgress.Height) / 2
            );

            // Add to form so it appears on top of the tab control
            Controls.Add(_tabLoadingProgress);
            _tabLoadingProgress.BringToFront();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
        }
    }

    #endregion

    #region Startup / Events

    private void MainForm_OnStartup_WireUpEvents()
    {
        // Wire up tab selection event to focus part ComboBox
        MainForm_TabControl.SelectedIndexChanged += (s, e) =>
        {
            MainForm_TabControl_SelectedIndexChanged(null!, null!);
        };
        MainForm_TabControl.Selecting += MainForm_TabControl_Selecting!;
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
                    Model_AppVariables.UserFullName =
                        Model_AppVariables.User; // Fallback to username if full name not found
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
            _connectionStrengthTimer = new Timer
            {
                Interval = 5000 // Check every 5 seconds
            };
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
        var advancedInvTab = MainForm_AdvancedInventory;
        var advancedRemoveTab = MainForm_Control_AdvancedRemove;

        if ((advancedInvTab != null && advancedInvTab.Visible) ||
            (advancedRemoveTab != null && advancedRemoveTab.Visible))
        {
            var result = MessageBox.Show(
                @"If you change the current tab now, any work will be lost.",
                @"Warning",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );
            if (result == DialogResult.Cancel) e.Cancel = true; // Prevent the tab change
        }
    }

    private async void MainForm_TabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Show progress while switching tabs
            await ShowTabLoadingProgressAsync();

            switch (MainForm_TabControl.SelectedIndex)
            {
                case 0: // Inventory Tab
                    var invTab = MainForm_Control_InventoryTab;
                    var advancedInvTab = MainForm_AdvancedInventory;
                    if (invTab is not null)
                    {
                        if (invTab.GetType().GetField("Control_InventoryTab_ComboBox_Part",
                                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(invTab) is ComboBox part)
                        {
                            part.SelectedIndex = 0;
                            part.Focus();
                            part.SelectAll();
                            part.BackColor = Model_AppVariables.UserUiColors.ControlFocusedBackColor ??
                                             Color.LightBlue;
                            part.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                            ;
                        }

                        if (invTab.GetType().GetField("Control_InventoryTab_TextBox_Quantity",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(invTab) is TextBox qty)
                        {
                            qty.Text = @"[ Enter Valid Quantity ]";
                            qty.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                            ;
                        }

                        if (invTab.GetType().GetField("Control_InventoryTab_ComboBox_Operation",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(invTab) is ComboBox op)
                        {
                            op.SelectedIndex = 0;
                            op.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                            ;
                        }

                        if (invTab.GetType().GetField("Control_InventoryTab_ComboBox_Location",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(invTab) is ComboBox loc)
                        {
                            loc.SelectedIndex = 0;
                            loc.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                            ;
                        }

                        invTab.Visible = true;
                        if (advancedInvTab is not null) advancedInvTab.Visible = false;
                    }

                    break;
                case 1: // Remove Tab
                    var remTab = MainForm_RemoveTabNormalControl;
                    var advancedRemoveTab = MainForm_Control_AdvancedRemove;
                    if (remTab is not null)
                    {
                        if (remTab.GetType().GetField("Control_RemoveTab_ComboBox_Part",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(remTab) is ComboBox part)
                        {
                            part.SelectedIndex = 0;
                            part.Focus();
                            part.SelectAll();
                            part.BackColor = Model_AppVariables.UserUiColors.ControlFocusedBackColor ??
                                             Color.LightBlue;
                            part.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                            ;
                        }

                        if (remTab.GetType().GetField("Control_RemoveTab_ComboBox_Operation",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(remTab) is ComboBox op)
                        {
                            op.SelectedIndex = 0;
                            op.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                            ;
                        }

                        if (remTab.GetType().GetField("Control_RemoveTab_DataGridView_Main",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(remTab) is DataGridView dgv)
                        {
                            if (dgv.DataSource == null)
                                dgv.Rows.Clear();
                            else
                                dgv.DataSource = null;
                        }

                        remTab.Visible = true;
                        if (advancedRemoveTab is not null) advancedRemoveTab.Visible = false;
                    }

                    break;
                case 2: // Transfer Tab
                    var transTab = MainForm_Control_TransferTab;
                    if (transTab is not null)
                    {
                        if (transTab.GetType().GetField("Control_TransferTab_ComboBox_Part",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(transTab) is ComboBox part)
                        {
                            part.SelectedIndex = 0;
                            part.Focus();
                            part.SelectAll();
                            part.BackColor = Model_AppVariables.UserUiColors.ControlFocusedBackColor ??
                                             Color.LightBlue;
                            part.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                        }

                        if (transTab.GetType().GetField("Control_TransferTab_ComboBox_Operation",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(transTab) is ComboBox op)
                        {
                            op.SelectedIndex = 0;
                            op.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                        }

                        if (transTab.GetType().GetField("Control_TransferTab_ComboBox_ToLocation",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(transTab) is ComboBox loc)
                        {
                            loc.SelectedIndex = 0;
                            loc.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                        }

                        if (transTab.GetType().GetField("Control_TransferTab_DataGridView_Main",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(transTab) is DataGridView dgv)
                        {
                            if (dgv.DataSource == null)
                                dgv.Rows.Clear();
                            else
                                dgv.DataSource = null;
                        }

                        if (transTab.GetType().GetField("Control_TransferTab_NumericUpDown_Quantity",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(transTab) is NumericUpDown nud)
                            nud.Value = nud.Minimum;
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
            // Hide progress when tab switching is complete
            HideTabLoadingProgress();
        }
    }

    private async Task ShowTabLoadingProgressAsync()
    {
        try
        {
            if (_tabLoadingProgress != null)
            {
                // Center the progress control on the tab control
                _tabLoadingProgress.Location = new Point(
                    (MainForm_TabControl.Width - _tabLoadingProgress.Width) / 2,
                    (MainForm_TabControl.Height - _tabLoadingProgress.Height) / 2
                );

                _tabLoadingProgress.ShowProgress();
                _tabLoadingProgress.UpdateProgress(25, "Switching tab...");

                // Simulate loading time to show progress
                await Task.Delay(100);
                _tabLoadingProgress.UpdateProgress(50, "Loading controls...");

                await Task.Delay(100);
                _tabLoadingProgress.UpdateProgress(75, "Applying settings...");

                await Task.Delay(100);
                _tabLoadingProgress.UpdateProgress(100, "Ready");
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
            _tabLoadingProgress?.HideProgress();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
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
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm / " + "OnFormClosing");
        }

        base.OnFormClosing(e);
    }

    #endregion

    private void MainForm_MenuStrip_File_Settings_Click(object sender, EventArgs e)
    {
        using var settingsForm = new SettingsForm();
        if (settingsForm.ShowDialog(this) != DialogResult.OK) return;
        MainForm_Control_InventoryTab?.Control_InventoryTab_HardReset();
        Core_Themes.ApplyTheme(this);
    }

    private void MainForm_MenuStrip_Exit_Click(object sender, EventArgs e)
    {
        // Optional: Prompt user for confirmation before exiting
        var result = MessageBox.Show(
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
        // Use global application variables for the user and connection info
        string connectionString = Model_AppVariables.ConnectionString;
        string currentUser = Model_AppVariables.User;
        bool isAdmin = Model_AppVariables.UserTypeAdmin;

        var transactionsForm = new MTM_Inventory_Application.Forms.Transactions.Transactions(connectionString, currentUser, isAdmin);
        transactionsForm.ShowDialog(this); // Show as modal dialog
    }
}

#endregion