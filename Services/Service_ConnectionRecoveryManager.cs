using System.Media;
using MTM_Inventory_Application.Controls.Addons;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Helpers;
using MySql.Data.MySqlClient;
using Timer = System.Windows.Forms.Timer;

namespace MTM_Inventory_Application.Services
{
    /// <summary>
    /// Service for managing database connection recovery and monitoring
    /// </summary>
    public class Service_ConnectionRecoveryManager
    {
        #region Fields

        private readonly MainForm _mainForm;
        private readonly Timer _reconnectTimer;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the disconnect timer is currently active
        /// </summary>
        public bool IsDisconnectTimerActive => _reconnectTimer.Enabled;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Service_ConnectionRecoveryManager class
        /// </summary>
        /// <param name="mainForm">Reference to the main form</param>
        /// <exception cref="ArgumentNullException">Thrown when mainForm is null</exception>
        public Service_ConnectionRecoveryManager(MainForm mainForm)
        {
            _mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
            _reconnectTimer = new Timer { Interval = 5000 };
            _reconnectTimer.Tick += async (s, e) => await TryReconnectAsync();
        }

        #endregion

        #region Connection Management Methods

        /// <summary>
        /// Handles when database connection is lost
        /// </summary>
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

        /// <summary>
        /// Handles when database connection is restored
        /// </summary>
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

        /// <summary>
        /// Updates the connection strength indicator on the main form
        /// </summary>
        public async Task UpdateConnectionStrengthAsync()
        {
            Control_ConnectionStrengthControl? signalStrength = _mainForm.MainForm_UserControl_SignalStrength;
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

        #endregion

        #region Private Methods

        /// <summary>
        /// Attempts to reconnect to the database
        /// </summary>
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
                // Reconnection failed, timer will try again
            }
        }

        #endregion
    }
}
