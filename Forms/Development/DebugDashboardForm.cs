using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
using Timer = System.Windows.Forms.Timer;

namespace MTM_Inventory_Application.Forms.Development
{
    /// <summary>
    /// Real-time debugging dashboard for monitoring application activity
    /// Provides comprehensive visibility into actions, variables, data flow, and business logic
    /// </summary>
    public partial class DebugDashboardForm : Form
    {
        #region Fields

        private readonly Timer _refreshTimer;
        private readonly List<string> _debugLog;
        private bool _isCapturingDebug;

        #endregion

        #region Properties

        private TextBox txtDebugOutput = null!;
        private ComboBox cmbDebugLevel = null!;
        private CheckBox chkTraceDatabase = null!;
        private CheckBox chkTraceBusinessLogic = null!;
        private CheckBox chkTraceUIActions = null!;
        private CheckBox chkTracePerformance = null!;
        private Button btnClearLog = null!;
        private Button btnSaveLog = null!;
        private Button btnStartStop = null!;
        private GroupBox grpFilters = null!;
        private GroupBox grpActions = null!;
        private Label lblStatus = null!;

        #endregion

        #region Constructors

        public DebugDashboardForm()
        {
            _debugLog = new List<string>();
            _refreshTimer = new Timer { Interval = 1000 };
            _refreshTimer.Tick += RefreshTimer_Tick;
            _isCapturingDebug = true;

            InitializeComponent();
            LoadCurrentConfiguration();
            
            Service_DebugTracer.TraceUIAction("DEBUG_DASHBOARD_OPENED", nameof(DebugDashboardForm));
        }

        #endregion

        #region Form Setup

        private void InitializeComponent()
        {
            SuspendLayout();

            // Form setup
            Text = "🔍 MTM Debug Dashboard - Real-time Application Monitoring";
            Size = new Size(1200, 800);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(800, 600);

            // Main layout
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(10)
            };

            // Configuration panel
            var configPanel = CreateConfigurationPanel();
            mainPanel.Controls.Add(configPanel, 0, 0);
            mainPanel.SetRowSpan(configPanel, 2);

            // Debug output panel  
            var outputPanel = CreateDebugOutputPanel();
            mainPanel.Controls.Add(outputPanel, 1, 0);
            mainPanel.SetRowSpan(outputPanel, 2);

            // Status and actions panel
            var statusPanel = CreateStatusPanel();
            mainPanel.Controls.Add(statusPanel, 0, 2);
            mainPanel.SetColumnSpan(statusPanel, 2);

            // Set column and row styles
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));

            Controls.Add(mainPanel);
            ResumeLayout(false);
        }

        private Panel CreateConfigurationPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray,
                Padding = new Padding(5)
            };

            var configGroup = new GroupBox
            {
                Text = "🛠️ Debug Configuration",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Padding = new Padding(10)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                Padding = new Padding(5)
            };

            // Debug level
            layout.Controls.Add(new Label { Text = "Debug Level:", Font = new Font("Segoe UI", 9, FontStyle.Bold) }, 0, 0);
            cmbDebugLevel = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDebugLevel.Items.AddRange(new[] { "Low", "Medium", "High", "Verbose" });
            cmbDebugLevel.SelectedIndexChanged += CmbDebugLevel_SelectedIndexChanged;
            layout.Controls.Add(cmbDebugLevel, 0, 1);

            // Tracing options
            layout.Controls.Add(new Label { Text = "Tracing Options:", Font = new Font("Segoe UI", 9, FontStyle.Bold) }, 0, 2);
            
            chkTraceDatabase = new CheckBox { Text = "🗄️ Database Operations", Dock = DockStyle.Fill };
            chkTraceDatabase.CheckedChanged += ChkTraceDatabase_CheckedChanged;
            layout.Controls.Add(chkTraceDatabase, 0, 3);

            chkTraceBusinessLogic = new CheckBox { Text = "📊 Business Logic", Dock = DockStyle.Fill };
            chkTraceBusinessLogic.CheckedChanged += ChkTraceBusinessLogic_CheckedChanged;
            layout.Controls.Add(chkTraceBusinessLogic, 0, 4);

            chkTraceUIActions = new CheckBox { Text = "🖱️ UI Actions", Dock = DockStyle.Fill };
            chkTraceUIActions.CheckedChanged += ChkTraceUIActions_CheckedChanged;
            layout.Controls.Add(chkTraceUIActions, 0, 5);

            chkTracePerformance = new CheckBox { Text = "⏱️ Performance", Dock = DockStyle.Fill };
            chkTracePerformance.CheckedChanged += ChkTracePerformance_CheckedChanged;
            layout.Controls.Add(chkTracePerformance, 0, 6);

            // Preset buttons
            var presetPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown };
            
            var btnDevelopment = new Button { Text = "Development Mode", Width = 150, BackColor = Color.LightBlue };
            btnDevelopment.Click += (s, e) => { Service_DebugConfiguration.SetDevelopmentMode(); LoadCurrentConfiguration(); };
            presetPanel.Controls.Add(btnDevelopment);

            var btnProduction = new Button { Text = "Production Mode", Width = 150, BackColor = Color.LightCoral };
            btnProduction.Click += (s, e) => { Service_DebugConfiguration.SetProductionMode(); LoadCurrentConfiguration(); };
            presetPanel.Controls.Add(btnProduction);

            var btnDatabase = new Button { Text = "Database Debug", Width = 150, BackColor = Color.LightGreen };
            btnDatabase.Click += (s, e) => { Service_DebugConfiguration.SetDatabaseTroubleshootingMode(); LoadCurrentConfiguration(); };
            presetPanel.Controls.Add(btnDatabase);

            layout.Controls.Add(presetPanel, 0, 7);

            // Set row styles
            for (int i = 0; i < 8; i++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            layout.RowStyles[7] = new RowStyle(SizeType.Percent, 100);

            configGroup.Controls.Add(layout);
            panel.Controls.Add(configGroup);
            return panel;
        }

        private Panel CreateDebugOutputPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(5)
            };

            var outputGroup = new GroupBox
            {
                Text = "📝 Real-time Debug Output",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Padding = new Padding(10)
            };

            txtDebugOutput = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Consolas", 9),
                BackColor = Color.Black,
                ForeColor = Color.Lime,
                ReadOnly = true,
                WordWrap = false
            };

            outputGroup.Controls.Add(txtDebugOutput);
            panel.Controls.Add(outputGroup);
            return panel;
        }

        private Panel CreateStatusPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightYellow,
                Padding = new Padding(5)
            };

            var statusGroup = new GroupBox
            {
                Text = "📊 Status & Actions",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 2,
                Padding = new Padding(5)
            };

            // Status label
            lblStatus = new Label
            {
                Text = "🟢 Debug Tracing Active",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            layout.Controls.Add(lblStatus, 0, 0);

            // Action buttons
            btnStartStop = new Button
            {
                Text = "⏸️ Pause Capture",
                Dock = DockStyle.Fill,
                BackColor = Color.Orange
            };
            btnStartStop.Click += BtnStartStop_Click;
            layout.Controls.Add(btnStartStop, 1, 0);

            btnClearLog = new Button
            {
                Text = "🗑️ Clear Log",
                Dock = DockStyle.Fill,
                BackColor = Color.LightCoral
            };
            btnClearLog.Click += BtnClearLog_Click;
            layout.Controls.Add(btnClearLog, 2, 0);

            btnSaveLog = new Button
            {
                Text = "💾 Save Log",
                Dock = DockStyle.Fill,
                BackColor = Color.LightGreen
            };
            btnSaveLog.Click += BtnSaveLog_Click;
            layout.Controls.Add(btnSaveLog, 3, 0);

            // Set column styles
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            statusGroup.Controls.Add(layout);
            panel.Controls.Add(statusGroup);
            return panel;
        }

        #endregion

        #region Event Handlers

        private void CmbDebugLevel_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbDebugLevel.SelectedItem is string levelStr && Enum.TryParse<DebugLevel>(levelStr, out var level))
            {
                Service_DebugTracer.CurrentLevel = level;
                Service_DebugTracer.TraceUIAction("DEBUG_LEVEL_CHANGED", nameof(DebugDashboardForm), 
                    new Dictionary<string, object> { ["NewLevel"] = levelStr });
            }
        }

        private void ChkTraceDatabase_CheckedChanged(object? sender, EventArgs e)
        {
            Service_DebugTracer.TraceDatabase = chkTraceDatabase.Checked;
            Service_DebugTracer.TraceUIAction("TRACE_DATABASE_CHANGED", nameof(DebugDashboardForm),
                new Dictionary<string, object> { ["Enabled"] = chkTraceDatabase.Checked });
        }

        private void ChkTraceBusinessLogic_CheckedChanged(object? sender, EventArgs e)
        {
            Service_DebugTracer.EnableBusinessLogicTracing = chkTraceBusinessLogic.Checked;
            Service_DebugTracer.TraceUIAction("TRACE_BUSINESS_LOGIC_CHANGED", nameof(DebugDashboardForm),
                new Dictionary<string, object> { ["Enabled"] = chkTraceBusinessLogic.Checked });
        }

        private void ChkTraceUIActions_CheckedChanged(object? sender, EventArgs e)
        {
            Service_DebugTracer.TraceUIActions = chkTraceUIActions.Checked;
            Service_DebugTracer.TraceUIAction("TRACE_UI_ACTIONS_CHANGED", nameof(DebugDashboardForm),
                new Dictionary<string, object> { ["Enabled"] = chkTraceUIActions.Checked });
        }

        private void ChkTracePerformance_CheckedChanged(object? sender, EventArgs e)
        {
            Service_DebugTracer.TracePerformance = chkTracePerformance.Checked;
            Service_DebugTracer.TraceUIAction("TRACE_PERFORMANCE_CHANGED", nameof(DebugDashboardForm),
                new Dictionary<string, object> { ["Enabled"] = chkTracePerformance.Checked });
        }

        private void BtnStartStop_Click(object? sender, EventArgs e)
        {
            _isCapturingDebug = !_isCapturingDebug;
            
            if (_isCapturingDebug)
            {
                btnStartStop.Text = "⏸️ Pause Capture";
                btnStartStop.BackColor = Color.Orange;
                lblStatus.Text = "🟢 Debug Tracing Active";
                _refreshTimer.Start();
            }
            else
            {
                btnStartStop.Text = "▶️ Resume Capture";
                btnStartStop.BackColor = Color.LightGreen;
                lblStatus.Text = "🟡 Debug Tracing Paused";
                _refreshTimer.Stop();
            }

            Service_DebugTracer.TraceUIAction("DEBUG_CAPTURE_TOGGLED", nameof(DebugDashboardForm),
                new Dictionary<string, object> { ["IsCapturing"] = _isCapturingDebug });
        }

        private void BtnClearLog_Click(object? sender, EventArgs e)
        {
            txtDebugOutput.Clear();
            _debugLog.Clear();
            Service_DebugTracer.TraceUIAction("DEBUG_LOG_CLEARED", nameof(DebugDashboardForm));
        }

        private void BtnSaveLog_Click(object? sender, EventArgs e)
        {
            using var saveDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|Log files (*.log)|*.log|All files (*.*)|*.*",
                FileName = $"MTM_Debug_Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
                Title = "Save Debug Log"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllLines(saveDialog.FileName, _debugLog);
                    MessageBox.Show($"Debug log saved to:\n{saveDialog.FileName}", "Log Saved", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    Service_DebugTracer.TraceUIAction("DEBUG_LOG_SAVED", nameof(DebugDashboardForm),
                        new Dictionary<string, object> 
                        { 
                            ["FileName"] = saveDialog.FileName,
                            ["LineCount"] = _debugLog.Count 
                        });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving debug log:\n{ex.Message}", "Save Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RefreshTimer_Tick(object? sender, EventArgs e)
        {
            if (_isCapturingDebug)
            {
                // In a real implementation, this would capture actual debug output
                // For demo purposes, we'll show current configuration status
                UpdateStatusDisplay();
            }
        }

        #endregion

        #region Helper Methods

        private void LoadCurrentConfiguration()
        {
            // Load current debug configuration
            cmbDebugLevel.SelectedItem = Service_DebugTracer.CurrentLevel.ToString();
            chkTraceDatabase.Checked = Service_DebugTracer.TraceDatabase;
            chkTraceBusinessLogic.Checked = Service_DebugTracer.EnableBusinessLogicTracing;
            chkTraceUIActions.Checked = Service_DebugTracer.TraceUIActions;
            chkTracePerformance.Checked = Service_DebugTracer.TracePerformance;

            Service_DebugConfiguration.LogCurrentStatus();
        }

        private void UpdateStatusDisplay()
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            var status = Service_DebugConfiguration.GetCurrentStatus();
            
            var statusText = $"[{timestamp}] Configuration Status:\n" +
                           $"Level: {status["GlobalLevel"]}, " +
                           $"DB: {status["DatabaseTracing"]}, " +
                           $"BL: {status["BusinessLogicTracing"]}, " +
                           $"UI: {status["UIActionsTracing"]}, " +
                           $"Perf: {status["PerformanceTracing"]}";

            _debugLog.Add(statusText);
            
            // Keep log size manageable
            if (_debugLog.Count > 1000)
            {
                _debugLog.RemoveRange(0, 100);
            }

            // Update display
            if (txtDebugOutput.InvokeRequired)
            {
                txtDebugOutput.Invoke(() => UpdateTextBox());
            }
            else
            {
                UpdateTextBox();
            }
        }

        private void UpdateTextBox()
        {
            var displayText = string.Join(Environment.NewLine, _debugLog.TakeLast(100));
            txtDebugOutput.Text = displayText;
            txtDebugOutput.SelectionStart = txtDebugOutput.Text.Length;
            txtDebugOutput.ScrollToCaret();
        }

        #endregion

        #region Form Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _refreshTimer.Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
            Service_DebugTracer.TraceUIAction("DEBUG_DASHBOARD_CLOSED", nameof(DebugDashboardForm));
            base.OnFormClosed(e);
        }

        #endregion
    }
}
