using MTM_WIP_Application.Core;
using MySql.Data.MySqlClient;
using System;
using System.Media;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace MTM_WIP_Application.Forms.MainForm.Classes;

public class ConnectionRecoveryManager
{
    private readonly MainForm _mainForm;
    private readonly Timer _reconnectTimer;

    public bool IsDisconnectTimerActive => _reconnectTimer.Enabled;


    public ConnectionRecoveryManager(MainForm mainForm)
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
            using var conn = new MySqlConnection(WipAppVariables.ConnectionString);
            await conn.OpenAsync();
            HandleConnectionRestored();
        }
        catch
        {
            // Still disconnected, do nothing
        }
    }
}