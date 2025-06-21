using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using MTM_WIP_Application.Controls.MainForm;
using Timer = System.Windows.Forms.Timer;

namespace MTM_WIP_Application.Forms.MainForm;

#region MainForm

public partial class MainForm : Form
{
    private Timer? _connectionStrengthTimer;
    public MySqlConnectionStrengthChecker ConnectionStrengthChecker = null!;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ConnectionRecoveryManager ConnectionRecoveryManager { get; private set; } = null!;

    #region Initialization

    public MainForm()
    {
        try
        {
            InitializeComponent();
            Debug.WriteLine(
                $"[DEBUG] MainForm_InventoryTab after InitializeComponent: {(MainForm_Control_InventoryTab == null ? "null" : "not null")}");
            LoggingUtility.Log("MainForm constructor called.");
            ConnectionStrengthChecker = new MySqlConnectionStrengthChecker();
            ConnectionRecoveryManager = new ConnectionRecoveryManager(this);
            MainForm_OnStartup_SetupConnectionStrengthControl();
            MainForm_OnStartup_WireUpEvents();
            Shown += async (s, e) =>
            {
                await MainForm_OnStartup_GetUserFullNameAsync();
                await Task.Delay(500); // Ensure controls are visible
                if (MainForm_Control_InventoryTab != null)
                {
                    MainForm_Control_InventoryTab.Control_InventoryTab_ComboBox_Part.Focus();
                    MainForm_Control_InventoryTab.Control_InventoryTab_ComboBox_Part.SelectAll();
                    MainForm_Control_InventoryTab.Control_InventoryTab_ComboBox_Part.BackColor = Color.LightBlue;
                }
            };
            LoggingUtility.Log("MainForm initialized.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(MainForm));
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
                Core_WipAppVariables.UserFullName =
                    await Dao_User.GetUserFullNameAsync(Core_WipAppVariables.User, true);


                if (string.IsNullOrEmpty(Core_WipAppVariables.UserFullName))
                    Core_WipAppVariables.UserFullName =
                        Core_WipAppVariables.User; // Fallback to username if full name not found

                LoggingUtility.Log($"User full name loaded: {Core_WipAppVariables.UserFullName}");
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
                "If you change the current tab now, any work will be lost.",
                "Warning",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );
            if (result == DialogResult.Cancel) e.Cancel = true; // Prevent the tab change
        }
    }

    private void MainForm_TabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
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
                            part.BackColor = Color.LightBlue;
                            part.ForeColor = Color.Red;
                        }

                        if (invTab.GetType().GetField("Control_InventoryTab_ComboBox_Operation",
                                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(invTab) is ComboBox op)
                        {
                            op.SelectedIndex = 0;
                            op.ForeColor = Color.Red;
                        }

                        if (invTab.GetType().GetField("Control_InventoryTab_ComboBox_Location",
                                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(invTab) is ComboBox loc)
                        {
                            loc.SelectedIndex = 0;
                            loc.ForeColor = Color.Red;
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
                            part.BackColor = Color.LightBlue;
                            part.ForeColor = Color.Red;
                        }

                        if (remTab.GetType().GetField("Control_RemoveTab_ComboBox_Operation",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(remTab) is ComboBox op)
                        {
                            op.SelectedIndex = 0;
                            op.ForeColor = Color.Red;
                        }

                        if (remTab.GetType().GetField("Control_RemoveTab_DataGridView_Main",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(remTab) is DataGridView dgv)
                        {
                            dgv.Rows.Clear();
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
                            part.BackColor = Color.LightBlue;
                            part.ForeColor = Color.Red;
                        }

                        if (transTab.GetType().GetField("Control_TransferTab_ComboBox_Operation",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(transTab) is ComboBox op)
                        {
                            op.SelectedIndex = 0;
                            op.ForeColor = Color.Red;
                        }

                        if (transTab.GetType().GetField("Control_TransferTab_ComboBox_ToLocation",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(transTab) is ComboBox loc)
                        {
                            loc.SelectedIndex = 0;
                            loc.ForeColor = Color.Red;
                        }

                        if (transTab.GetType().GetField("Control_TransferTab_DataGridView_Main",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                ?.GetValue(transTab) is DataGridView dgv)
                            dgv.DataSource = null;
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
    }

    #endregion

    #region Form Closing

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        try
        {
            _connectionStrengthTimer?.Stop();
            _connectionStrengthTimer?.Dispose();
            LoggingUtility.Log("MainForm is closing. Connection strength timer stopped.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm / " + "OnFormClosing");
        }

        base.OnFormClosing(e);
    }

    #endregion
}

#endregion