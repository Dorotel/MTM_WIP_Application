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

    public MySqlConnectionStrengthChecker ConnectionStrengthChecker = null!;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ConnectionRecoveryManager ConnectionRecoveryManager { get; private set; } = null!;

    public MainForm()
    {
        try
        {
            InitializeComponent();

            AppLogger.Log("MainForm constructor called.");

            ConnectionStrengthChecker = new MySqlConnectionStrengthChecker();

            ConnectionRecoveryManager = new ConnectionRecoveryManager(this);

            SetupConnectionStrengthControl();

            _ = OnStartup();
            AppLogger.Log("MainForm initialized.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, nameof(MainForm));
        }
    }

    private static async Task OnStartup()
    {
        try
        {
            try
            {
                WipAppVariables.UserFullName = await UserDao.GetUserFullNameAsync(WipAppVariables.User, true);
                AppLogger.Log($"User full name loaded: {WipAppVariables.UserFullName}");
            }
            catch (Exception ex)
            {
                AppLogger.LogApplicationError(ex);
                await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                    "MainForm / " + "OnStartup / " + "GetUserFullNameAsync");
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "MainForm / " + "OnStartup");
        }
    }

    private void SetupConnectionStrengthControl()
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
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, nameof(SetupConnectionStrengthControl));
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
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm / " + "ProcessCmdKey");
            return false;
        }
    }
}