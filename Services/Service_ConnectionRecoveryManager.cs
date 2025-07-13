using System.Media;
using MTM_Inventory_Application.Controls.Addons;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Helpers;
using MySql.Data.MySqlClient;
using Timer = System.Windows.Forms.Timer;

namespace MTM_Inventory_Application.Services;

public class Service_ConnectionRecoveryManager
{
    private readonly MainForm _mainForm;
    private readonly Timer _reconnectTimer;

    public bool IsDisconnectTimerActive => _reconnectTimer.Enabled;

    public Service_ConnectionRecoveryManager(MainForm mainForm)
    {
        _mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
        _reconnectTimer = new Timer { Interval = 5000 };
        _reconnectTimer.Tick += async (s, e) => await TryReconnectAsync();
    }

    public void HandleConnectionLost()
    {
        if (_mainForm.InvokeRequired)
        {
            _mainForm.Invoke(new Action(HandleConnectionLost));
            return;
        }

        SystemSounds.Exclamation.Play();

        _mainForm.MainForm_StatusStrip_Disconnected.Visible = true;
        _mainForm.MainForm_TabControl.Enabled = false;
        _reconnectTimer.Start();
    }

    public void HandleConnectionRestored()
    {
        if (_mainForm.InvokeRequired)
        {
            _mainForm.Invoke(new Action(HandleConnectionRestored));
            return;
        }

        SystemSounds.Question.Play();
        _mainForm.MainForm_StatusStrip_Disconnected.Visible = false;
        _mainForm.MainForm_TabControl.Enabled = true;
        _reconnectTimer.Stop();
    }

    private async Task TryReconnectAsync()
    {
        try
        {
            using MySqlConnection conn = new(Core_WipAppVariables.ReConnectionString);
            await conn.OpenAsync();
            HandleConnectionRestored();
        }
        catch
        {
        }
    }

    public async Task UpdateConnectionStrengthAsync()
    {
        ConnectionStrengthControl? signalStrength = _mainForm.MainForm_Control_SignalStrength;
        ToolStripStatusLabel? statusStripDisconnected = _mainForm.MainForm_StatusStrip_Disconnected;

        if (signalStrength.InvokeRequired)
        {
            await Task.Run(async () => await UpdateConnectionStrengthAsync());
            return;
        }

        (int strength, int pingMs) = await Helper_Control_MySqlSignal.GetStrengthAsync();

        if (IsDisconnectTimerActive)
        {
            strength = 0;
        }

        signalStrength.Strength = strength;
        signalStrength.Ping = pingMs;

        statusStripDisconnected.Visible = strength == 0;

        if (strength == 0 && !IsDisconnectTimerActive)
        {
            HandleConnectionLost();
        }
    }
}
