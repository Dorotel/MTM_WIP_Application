using System.ComponentModel;
using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Controls.Shared;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.Settings;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
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
            System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] Constructing MainForm...");
            try
            {
                InitializeComponent();
                AutoScaleMode = AutoScaleMode.Dpi;
                System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] InitializeComponent complete.");

                // Set the form title with user and privilege info
                string privilege = "Unknown";
                if (Model_AppVariables.UserTypeAdmin)
                    privilege = "Administrator";
                else if (Model_AppVariables.UserTypeNormal)
                    privilege = "Normal User";
                else if (Model_AppVariables.UserTypeReadOnly)
                    privilege = "Read Only";
                this.Text = $"Manitowoc Tool and Manufacturing WIP Inventory System | {Model_AppVariables.User} | {privilege}";

                InitializeProgressControl();
                System.Diagnostics.Debug.WriteLine("[DEBUG] [MainForm.ctor] Progress control initialized.");

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
                    await Task.Delay(500);
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
            Control_AdvancedInventory? advancedInvTab = MainForm_AdvancedInventory;
            Control_AdvancedRemove? advancedRemoveTab = MainForm_Control_AdvancedRemove;

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

                switch (MainForm_TabControl.SelectedIndex)
                {
                    case 0:
                        Control_InventoryTab? invTab = MainForm_Control_InventoryTab;
                        Control_AdvancedInventory? advancedInvTab = MainForm_AdvancedInventory;
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
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
                                    ?.GetValue(invTab) is TextBox qty)
                            {
                                qty.Text = @"[ Enter Valid Quantity ]";
                                qty.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                                ;
                            }

                            if (invTab.GetType().GetField("Control_InventoryTab_ComboBox_Operation",
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
                                    ?.GetValue(invTab) is ComboBox op)
                            {
                                op.SelectedIndex = 0;
                                op.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                                ;
                            }

                            if (invTab.GetType().GetField("Control_InventoryTab_ComboBox_Location",
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
                                    ?.GetValue(invTab) is ComboBox loc)
                            {
                                loc.SelectedIndex = 0;
                                loc.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                                ;
                            }

                            invTab.Visible = true;
                            if (advancedInvTab is not null)
                            {
                                advancedInvTab.Visible = false;
                            }
                        }

                        break;
                    case 1:
                        Control_RemoveTab? remTab = MainForm_RemoveTabNormalControl;
                        Control_AdvancedRemove? advancedRemoveTab = MainForm_Control_AdvancedRemove;
                        if (remTab is not null)
                        {
                            if (remTab.GetType().GetField("Control_RemoveTab_ComboBox_Part",
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
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
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
                                    ?.GetValue(remTab) is ComboBox op)
                            {
                                op.SelectedIndex = 0;
                                op.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                                ;
                            }

                            if (remTab.GetType().GetField("Control_RemoveTab_DataGridView_Main",
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
                                    ?.GetValue(remTab) is DataGridView dgv)
                            {
                                if (dgv.DataSource == null)
                                {
                                    dgv.Rows.Clear();
                                }
                                else
                                {
                                    dgv.DataSource = null;
                                }
                            }

                            remTab.Visible = true;
                            if (advancedRemoveTab is not null)
                            {
                                advancedRemoveTab.Visible = false;
                            }
                        }

                        break;
                    case 2:
                        Control_TransferTab? transTab = MainForm_Control_TransferTab;
                        if (transTab is not null)
                        {
                            if (transTab.GetType().GetField("Control_TransferTab_ComboBox_Part",
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
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
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
                                    ?.GetValue(transTab) is ComboBox op)
                            {
                                op.SelectedIndex = 0;
                                op.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                            }

                            if (transTab.GetType().GetField("Control_TransferTab_ComboBox_ToLocation",
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
                                    ?.GetValue(transTab) is ComboBox loc)
                            {
                                loc.SelectedIndex = 0;
                                loc.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                            }

                            if (transTab.GetType().GetField("Control_TransferTab_DataGridView_Main",
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
                                    ?.GetValue(transTab) is DataGridView dgv)
                            {
                                if (dgv.DataSource == null)
                                {
                                    dgv.Rows.Clear();
                                }
                                else
                                {
                                    dgv.DataSource = null;
                                }
                            }

                            if (transTab.GetType().GetField("Control_TransferTab_NumericUpDown_Quantity",
                                        System.Reflection.BindingFlags.NonPublic |
                                        System.Reflection.BindingFlags.Instance)
                                    ?.GetValue(transTab) is NumericUpDown nud)
                            {
                                nud.Value = nud.Minimum;
                            }
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

            MainForm_Control_InventoryTab?.Control_InventoryTab_HardReset();
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

        private async void squashBatchNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Query the number of problematic batches before running the fix
                string connectionString = MTM_Inventory_Application.Helpers.Helper_Database_Variables.GetConnectionString(null, null, null, null);
                int totalProblematicBatches = 0;
                using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    const string countBatchesSql = @"
                SELECT COUNT(*)
                FROM (
                    SELECT BatchNumber
                    FROM mtm_wip_application.inv_transaction
                    GROUP BY BatchNumber
                    HAVING SUM(CASE WHEN TransactionType = 'IN' THEN 1 ELSE 0 END) > 1
                       AND SUM(CASE WHEN TransactionType = 'OUT' THEN 1 ELSE 0 END) > 1
                ) t;";
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(countBatchesSql, connection))
                    {
                        object? result = await cmd.ExecuteScalarAsync();
                        totalProblematicBatches = Convert.ToInt32(result);
                    }
                }
                int cyclesRequired = (int)Math.Ceiling(totalProblematicBatches / 250.0);

                if (totalProblematicBatches == 0)
                {
                    MessageBox.Show("No problematic batches found.", "Batch Number Squash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var prompt = $"Total problematic batches: {totalProblematicBatches}\n" +
                             $"Estimated runs needed (max 250 per run): {cyclesRequired}\n\n" +
                             "Do you want to continue?";
                var resultPrompt = MessageBox.Show(prompt, "Batch Number Squash", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultPrompt != DialogResult.Yes)
                    return;

                if (_tabLoadingControlProgress != null)
                {
                    _tabLoadingControlProgress.Location = new Point(
                        (Width - _tabLoadingControlProgress.Width) / 2,
                        (Height - _tabLoadingControlProgress.Height) / 2
                    );
                    _tabLoadingControlProgress.ShowProgress();
                    _tabLoadingControlProgress.UpdateProgress(0, "Starting batch number squash...");
                    _tabLoadingControlProgress.EnableCancel(true);
                }

                _batchCancelTokenSource = new CancellationTokenSource();
                _tabLoadingControlProgress.CancelRequested += () => _batchCancelTokenSource?.Cancel();

                var cycleTimes = new List<TimeSpan>();
                int batchesFixed = 0;
                bool wasCancelled = false;
                var swCycle = new System.Diagnostics.Stopwatch();

                var progress = new Progress<(int percent, string status, int cycle, int totalCycles, int batchInCycle, int batchesInCycle, int totalFixed)>(tuple =>
                {
                    // Estimate time remaining
                    string timeLeft = "";
                    if (cycleTimes.Count > 0 && tuple.cycle <= tuple.totalCycles)
                    {
                        double avgSeconds = cycleTimes.Average(ts => ts.TotalSeconds);
                        int cyclesLeft = tuple.totalCycles - tuple.cycle + 1;
                        var est = TimeSpan.FromSeconds(avgSeconds * cyclesLeft);
                        timeLeft = $" | Est. time left: {est:mm\\:ss}";
                    }

                    string detail = $"Cycle {tuple.cycle} of {tuple.totalCycles} | " +
                                    $"Batch {tuple.batchInCycle} of {tuple.batchesInCycle} in this cycle | " +
                                    $"Total fixed: {tuple.totalFixed} of {totalProblematicBatches}{timeLeft}";

                    _tabLoadingControlProgress?.UpdateProgress(tuple.percent, $"{tuple.status}\n{detail}");
                });

                try
                {
                    int lastCycle = 0;
                    swCycle.Restart();
                    await Dao_Inventory.SplitBatchNumbersByReceiveDateAsync(
                        progress,
                        _batchCancelTokenSource.Token
                    );
                }
                catch (OperationCanceledException)
                {
                    wasCancelled = true;
                }
                finally
                {
                    _batchCancelTokenSource = null;
                    if (_tabLoadingControlProgress != null)
                    {
                        _tabLoadingControlProgress.EnableCancel(false);
                        _tabLoadingControlProgress.CancelRequested -= () => _batchCancelTokenSource?.Cancel();
                    }
                }

                if (_tabLoadingControlProgress != null)
                {
                    await _tabLoadingControlProgress.CompleteProgressAsync();
                }

                if (wasCancelled)
                {
                    MessageBox.Show("Batch number squash was cancelled by the user.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Batch number squash complete.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (_tabLoadingControlProgress != null)
                {
                    _tabLoadingControlProgress.UpdateProgress(0, "Error occurred.");
                    _tabLoadingControlProgress.HideProgress();
                }
                MTM_Inventory_Application.Logging.LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($"Error during batch number squash: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    #endregion
    #endregion
}
