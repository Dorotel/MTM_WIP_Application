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
                $"[DEBUG] MainForm_InventoryTab after InitializeComponent: {(MainForm_InventoryTab == null ? "null" : "not null")}");
            LoggingUtility.Log("MainForm constructor called.");
            ConnectionStrengthChecker = new MySqlConnectionStrengthChecker();
            ConnectionRecoveryManager = new ConnectionRecoveryManager(this);
            MainForm_OnStartup_SetupConnectionStrengthControl();
            MainForm_OnStartup_WireUpEvents();
            Shown += async (s, e) =>
            {
                await MainForm_OnStartup_GetUserFullNameAsync();
                await Task.Delay(100); // Ensure controls are visible
                if (MainForm_InventoryTab != null)
                {
                    MainForm_InventoryTab.Control_InventoryTab_ComboBox_Part.Focus();
                    MainForm_InventoryTab.Control_InventoryTab_ComboBox_Part.SelectAll();
                    MainForm_InventoryTab.Control_InventoryTab_ComboBox_Part.BackColor = Color.LightBlue;
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
#if DEBUG
            Debug.WriteLine(
                $"[DEBUG] MainForm_InventoryTab in SelectedIndexChanged: {(MainForm_InventoryTab == null ? "null" : "not null")}");
#endif
            ComboBox? partCombo = null;
            // Inventory Tab (index 0)
            if (MainForm_TabControl.SelectedIndex == 0 && MainForm_InventoryTab != null)
            {
                MainForm_InventoryTab.UpdateToggleRightPanelButton();
                partCombo = MainForm_InventoryTab.GetType()
                    .GetField("Control_InventoryTab_ComboBox_Part",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(MainForm_InventoryTab) as ComboBox;
            }
            // Remove Tab (index 1)
            else if (MainForm_TabControl.SelectedIndex == 1 && MainForm_RemoveTab != null)
            {
                MainForm_RemoveTab.UpdateToggleRightPanelButton();
                partCombo = MainForm_RemoveTab.GetType()
                    .GetField("Control_RemoveTab_ComboBox_Part",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(MainForm_RemoveTab) as ComboBox;
            }
            // Transfer Tab (index 2)
            else if (MainForm_TabControl.SelectedIndex == 2 && controlTransferTab1 != null)
            {
                controlTransferTab1.UpdateToggleRightPanelButton();
                partCombo = controlTransferTab1.GetType()
                    .GetField("Control_TransferTab_ComboBox_Part",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(controlTransferTab1) as ComboBox;
            }

            Helper_ComboBoxes.DeselectAllComboBoxText(this);
#if DEBUG
            Debug.WriteLine(
                $"[DEBUG] TabIndex: {MainForm_TabControl.SelectedIndex}, partCombo: {(partCombo != null ? partCombo.Name : "null")}");
#endif
            if (partCombo != null)
            {
#if DEBUG
                Debug.WriteLine($"[DEBUG] Focusing and selecting all for: {partCombo.Name}");
#endif
                partCombo.Focus();
                partCombo.SelectAll();
                partCombo.BackColor = Color.LightBlue;
            }
        };

        // Move focus logic to OnShown for after everything is loaded
        Shown += async (s, e) =>
        {
            // Wait for InventoryTab controls to be loaded and visible
            await Task.Delay(100); // Small delay to ensure controls are ready
            var partCombo = MainForm_InventoryTab.GetType()
                .GetField("Control_InventoryTab_ComboBox_Part",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(MainForm_InventoryTab) as ComboBox;
        };
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

            return base.ProcessCmdKey(ref msg, keyData);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm / " + "ProcessCmdKey");
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

    private void changeLogMakerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // Open the Exporter_Do_Not_Pack form as a dialog, disabling the main form while open
        using var exporter = new Exporter_Do_Not_Pack();
        Enabled = false;
        exporter.FormClosed += (_, _) => Enabled = true;
        exporter.Show(this);
    }
}

#endregion