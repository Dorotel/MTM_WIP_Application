using System;
using System.Media;
using System.Threading.Tasks;
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

        // Play a system exclamation sound
        SystemSounds.Exclamation.Play();

        // 1. Show disconnected status
        _mainForm.MainForm_StatusStrip_Disconnected.Visible = true;
        // 2. Disable main tab control
        _mainForm.MainForm_TabControl.Enabled = false;
        // 3. Start reconnect timer
        _reconnectTimer.Start();
    }

    public void HandleConnectionRestored()
    {
        if (_mainForm.InvokeRequired)
        {
            _mainForm.Invoke(new Action(HandleConnectionRestored));
            return;
        }

        // Play a system exclamation sound
        SystemSounds.Question.Play();
        // 1. Hide disconnected status
        _mainForm.MainForm_StatusStrip_Disconnected.Visible = false;
        // 2. Enable main tab control
        _mainForm.MainForm_TabControl.Enabled = true;
        // 3. Stop reconnect timer
        _reconnectTimer.Stop();
    }

    private async Task TryReconnectAsync()
    {
        try
        {
            using var conn = new MySqlConnection(Core_WipAppVariables.ReConnectionString);
            await conn.OpenAsync();
            HandleConnectionRestored();
        }
        catch
        {
            // Still disconnected, do nothing
        }
    }

    public async Task UpdateConnectionStrengthAsync()
    {
        // Use the MainForm's control references via _mainForm
        var signalStrength = _mainForm.MainForm_Control_SignalStrength;
        var statusStripDisconnected = _mainForm.MainForm_StatusStrip_Disconnected;

        if (signalStrength.InvokeRequired)
        {
            await Task.Run(async () => await UpdateConnectionStrengthAsync());
            return;
        }

        // Access the connection checker via _mainForm (make it internal or provide a getter)
        var (strength, pingMs) = await Helper_Control_MySqlSignal.GetStrengthAsync();

        // Use the instance's IsDisconnectTimerActive property
        if (IsDisconnectTimerActive) strength = 0;

        signalStrength.Strength = strength;
        signalStrength.Ping = pingMs;

        statusStripDisconnected.Visible = strength == 0;

        // Use the instance method for connection lost
        if (strength == 0 && !IsDisconnectTimerActive)
            HandleConnectionLost();
    }
}