using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Timers;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace MTM_WIP_Application.Forms.MainForm;

public partial class MainForm : Form
{
    private Timer? _connectionStrengthTimer;
    private MySqlConnectionStrengthChecker _connectionStrengthChecker;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ConnectionRecoveryManager ConnectionRecoveryManager { get; private set; }

    public MainForm()
    {
        try
        {
            InitializeComponent();
            SetupConnectionStrengthControl();
            ConnectionRecoveryManager = new ConnectionRecoveryManager(this);

            _ = OnStartup();
            AppLogger.Log("MainForm initialized.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, nameof(MainForm));
        }
    }

    private void SetupConnectionStrengthControl()
    {
        // Initialize the checker and timer
        _connectionStrengthChecker = new MySqlConnectionStrengthChecker();
        _connectionStrengthTimer = new Timer();
        _connectionStrengthTimer.Interval = 5000; // Check every 5 seconds
        _connectionStrengthTimer.Tick += async (s, e) => await UpdateConnectionStrengthAsync();
        _connectionStrengthTimer.Start();
    }

    private async Task UpdateConnectionStrengthAsync()
    {
        if (MainForm_Control_SignalStrength.InvokeRequired)
        {
            MainForm_Control_SignalStrength.Invoke(new Func<Task>(async () => await UpdateConnectionStrengthAsync()));
            return;
        }

        var (strength, pingMs) = await _connectionStrengthChecker.GetStrengthAsync();

        // If the disconnect timer is active, force strength to 0
        if (ConnectionRecoveryManager.IsDisconnectTimerActive) strength = 0;

        MainForm_Control_SignalStrength.Strength = strength;
        MainForm_Control_SignalStrength.Ping = pingMs;

        // Show disconnected status if strength is 0 (no connection)
        MainForm_StatusStrip_Disconnected.Visible = strength == 0;

        // Handle connection lost
        if (strength == 0 && !ConnectionRecoveryManager.IsDisconnectTimerActive)
            ConnectionRecoveryManager.HandleConnectionLost();
    }


    private async Task OnStartup()
    {
        try
        {
            WireUpInventoryTabEvents();


            try
            {
                WipAppVariables.UserFullName = await UserDao.GetUserFullNameAsync(WipAppVariables.User, true);
                AppLogger.Log($"User full name loaded: {WipAppVariables.UserFullName}");
            }
            catch (Exception ex)
            {
                AppLogger.LogApplicationError(ex);
                await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "MainForm_OnStartup_GetUserFullName");
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "MainForm_OnStartup");
        }
    }

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
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_ProcessCmdKey");
            return false;
        }
    }


    private void WireUpInventoryTabEvents()
    {
        try
        {
            AppLogger.Log("Inventory tab events wired up.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_WireUpInventoryTabEvents");
        }
    }
}