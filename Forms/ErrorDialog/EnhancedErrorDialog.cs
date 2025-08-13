using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Forms.ErrorDialog
{
    #region EnhancedErrorDialog

    public partial class EnhancedErrorDialog : Form
    {
        #region Fields

        private Exception _exception;
        private string _callerName;
        private string _controlName;
        private ErrorSeverity _severity;
        private Func<bool>? _retryAction;
        private readonly Dictionary<string, object> _contextData;

        #endregion

        #region Properties

        public DialogResult ErrorDialogResult { get; private set; } = DialogResult.None;
        public bool ShouldRetry { get; private set; } = false;

        #endregion

        #region Constructors

        public EnhancedErrorDialog(Exception exception, string callerName, string controlName, 
            ErrorSeverity severity = ErrorSeverity.Medium, Func<bool>? retryAction = null,
            Dictionary<string, object>? contextData = null)
        {
            InitializeComponent();
            _exception = exception;
            _callerName = callerName;
            _controlName = controlName;
            _severity = severity;
            _retryAction = retryAction;
            _contextData = contextData ?? new Dictionary<string, object>();
            
            InitializeErrorDialog();
            WireUpEvents();
            ApplyTheme();
        }

        #endregion

        #region Initialization

        private void InitializeErrorDialog()
        {
            try
            {
                SetTitleAndIcon();
                PopulatePlainEnglishSummary();
                PopulateTechnicalDetails();
                BuildCallStackTree();
                ConfigureActionButtons();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                // Fallback to basic display
                labelPlainEnglish.Text = "An error occurred while displaying the error dialog details.";
            }
        }

        private void SetTitleAndIcon()
        {
            string severityText = _severity switch
            {
                ErrorSeverity.Low => "⚠️ Application Warning",
                ErrorSeverity.Medium => "🚨 Application Error", 
                ErrorSeverity.High => "🚨 Critical Database Issue",
                ErrorSeverity.Fatal => "💀 Fatal Application Error",
                _ => "🚨 Application Error"
            };

            this.Text = $"MTM Inventory Application - {severityText}";
            
            // Set the icon based on severity
            var iconText = _severity switch
            {
                ErrorSeverity.Low => "⚠️",
                ErrorSeverity.Medium => "❌",
                ErrorSeverity.High => "🚨", 
                ErrorSeverity.Fatal => "💀",
                _ => "❌"
            };

            // Create a simple text-based icon (in a real implementation, you'd use proper icons)
            var bitmap = new Bitmap(48, 48);
            using (var g = Graphics.FromImage(bitmap))
            {
                var severityColor = _severity switch
                {
                    ErrorSeverity.Low => Color.Orange,
                    ErrorSeverity.Medium => Color.Red,
                    ErrorSeverity.High => Color.DarkRed,
                    ErrorSeverity.Fatal => Color.Black,
                    _ => Color.Red
                };
                g.Clear(severityColor);
                using (var brush = new SolidBrush(Color.White))
                using (var font = new Font("Segoe UI", 20, FontStyle.Bold))
                {
                    g.DrawString(iconText, font, brush, new PointF(4, 4));
                }
            }
            pictureBoxIcon.Image = bitmap;
        }

        private void PopulatePlainEnglishSummary()
        {
            var summary = new StringBuilder();
            summary.AppendLine("**What Happened:**");
            
            string plainDescription = _exception switch
            {
                MySql.Data.MySqlClient.MySqlException => 
                    "The application lost connection to the MySQL database while trying to save data. " +
                    "This usually means the database server is down or network issues occurred.",
                ArgumentNullException => 
                    "The application tried to use data that wasn't provided. This is usually a programming error.",
                InvalidOperationException => 
                    "The application tried to perform an operation that isn't allowed in the current state.",
                UnauthorizedAccessException => 
                    "The application doesn't have permission to perform this action. You may need to run as administrator.",
                FileNotFoundException => 
                    "The application couldn't find a required file. It may have been moved or deleted.",
                OutOfMemoryException => 
                    "The application ran out of memory. Try closing other applications and restart this one.",
                _ => $"An unexpected {_exception.GetType().Name} occurred in the application."
            };
            
            summary.AppendLine(plainDescription);
            summary.AppendLine();
            
            summary.AppendLine("**Impact:**");
            string impact = _severity switch
            {
                ErrorSeverity.Low => "Minor functionality may be affected, but the application should continue working normally.",
                ErrorSeverity.Medium => "Your recent changes may not have been saved. The operation that failed will need to be retried.",
                ErrorSeverity.High => "Data integrity may be affected. Recent inventory changes may not have been saved properly.",
                ErrorSeverity.Fatal => "The application cannot continue and will need to be restarted.",
                _ => "Some functionality may be temporarily unavailable."
            };
            summary.AppendLine(impact);
            summary.AppendLine();
            
            summary.AppendLine("**What You Should Do:**");
            var recommendations = _severity switch
            {
                ErrorSeverity.Low => new[] { "• Continue using the application normally", "• Report this if it happens frequently" },
                ErrorSeverity.Medium => new[] { "• Try the operation again", "• Check your network connection", "• Contact IT support if this persists" },
                ErrorSeverity.High => new[] { "• Check your network connection", "• Contact IT support immediately", "• Try the operation again in a few minutes" },
                ErrorSeverity.Fatal => new[] { "• Restart the application", "• Contact IT support", "• Report this error with the details below" },
                _ => new[] { "• Try the operation again", "• Contact support if the problem persists" }
            };
            
            foreach (var recommendation in recommendations)
            {
                summary.AppendLine(recommendation);
            }
            
            labelPlainEnglish.Text = summary.ToString();
        }

        private void PopulateTechnicalDetails()
        {
            var technical = new StringBuilder();
            technical.AppendLine("ERROR INFORMATION");
            technical.AppendLine("".PadRight(50, '='));
            technical.AppendLine($"Error Type: {_exception.GetType().Name}");
            technical.AppendLine($"Severity: {GetSeverityDisplay(_severity)}");
            technical.AppendLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            technical.AppendLine($"User: {Environment.UserName}");
            technical.AppendLine($"Machine: {Environment.MachineName}");
            technical.AppendLine();
            
            technical.AppendLine("LOCATION INFORMATION");
            technical.AppendLine("".PadRight(50, '='));
            technical.AppendLine($"Calling Method: {_callerName}");
            technical.AppendLine($"Control/Form: {_controlName}");
            technical.AppendLine();
            
            technical.AppendLine("ERROR MESSAGE");
            technical.AppendLine("".PadRight(50, '='));
            technical.AppendLine(_exception.Message);
            technical.AppendLine();
            
            if (_exception.InnerException != null)
            {
                technical.AppendLine("INNER EXCEPTION");
                technical.AppendLine("".PadRight(50, '='));
                technical.AppendLine($"Type: {_exception.InnerException.GetType().Name}");
                technical.AppendLine($"Message: {_exception.InnerException.Message}");
                technical.AppendLine();
            }
            
            technical.AppendLine("STACK TRACE");
            technical.AppendLine("".PadRight(50, '='));
            technical.AppendLine(_exception.StackTrace ?? "No stack trace available");
            
            if (_contextData.Any())
            {
                technical.AppendLine();
                technical.AppendLine("CONTEXT DATA");
                technical.AppendLine("".PadRight(50, '='));
                foreach (var kvp in _contextData)
                {
                    technical.AppendLine($"{kvp.Key}: {kvp.Value}");
                }
            }
            
            richTextBoxTechnical.Text = technical.ToString();
        }

        private void BuildCallStackTree()
        {
            treeViewCallStack.Nodes.Clear();
            
            try
            {
                var stackTrace = new StackTrace(_exception, true);
                var frames = stackTrace.GetFrames();
                
                if (frames == null || frames.Length == 0)
                {
                    var noStackNode = new TreeNode("No detailed call stack available");
                    noStackNode.ForeColor = Color.Gray;
                    treeViewCallStack.Nodes.Add(noStackNode);
                    return;
                }
                
                TreeNode? parentNode = null;
                
                for (int i = 0; i < frames.Length; i++)
                {
                    var frame = frames[i];
                    var method = frame.GetMethod();
                    
                    if (method == null) continue;
                    
                    string displayText = $"{GetMethodIcon(method)} {method.DeclaringType?.Name ?? "Unknown"}.{method.Name}()";
                    
                    var node = new TreeNode(displayText);
                    node.Tag = frame;
                    
                    // Color code by component type
                    if (method.DeclaringType?.Namespace?.Contains("MTM_Inventory_Application") == true)
                    {
                        if (method.DeclaringType.Name.StartsWith("Control_"))
                            node.ForeColor = Color.Purple;
                        else if (method.DeclaringType.Name.StartsWith("Dao_"))
                            node.ForeColor = Color.Orange;
                        else if (method.DeclaringType.Name.StartsWith("Helper_"))
                            node.ForeColor = Color.Red;
                        else
                            node.ForeColor = Color.Blue;
                    }
                    else
                    {
                        node.ForeColor = Color.Gray; // External/system methods
                    }
                    
                    if (parentNode == null)
                    {
                        treeViewCallStack.Nodes.Add(node);
                    }
                    else
                    {
                        parentNode.Nodes.Add(node);
                    }
                    
                    parentNode = node;
                }
                
                // Expand the first few levels
                foreach (TreeNode node in treeViewCallStack.Nodes)
                {
                    ExpandNodeRecursively(node, 3);
                }
            }
            catch (Exception ex)
            {
                var errorNode = new TreeNode($"Error building call stack: {ex.Message}");
                errorNode.ForeColor = Color.Red;
                treeViewCallStack.Nodes.Add(errorNode);
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private void ExpandNodeRecursively(TreeNode node, int maxDepth)
        {
            if (maxDepth <= 0) return;
            
            node.Expand();
            foreach (TreeNode child in node.Nodes)
            {
                ExpandNodeRecursively(child, maxDepth - 1);
            }
        }

        private string GetMethodIcon(System.Reflection.MethodBase method)
        {
            if (method.DeclaringType?.Name.StartsWith("Control_") == true) return "🎯";
            if (method.DeclaringType?.Name.StartsWith("Dao_") == true) return "🔍";
            if (method.DeclaringType?.Name.StartsWith("Helper_") == true) return "🗄️";
            if (method.DeclaringType?.Name.Contains("Form") == true) return "📋";
            return "⚡";
        }

        private void ConfigureActionButtons()
        {
            buttonRetry.Visible = _retryAction != null && _severity != ErrorSeverity.Fatal;
            buttonReportIssue.Visible = _severity >= ErrorSeverity.Medium;
            // Enable view logs if logging system is initialized
            buttonViewLogs.Enabled = true; // Always enabled for now - logging system is always initialized
        }

        private void UpdateStatusBar()
        {
            string statusIcon = _severity switch
            {
                ErrorSeverity.Low => "🟡",
                ErrorSeverity.Medium => "🔴",
                ErrorSeverity.High => "🔴",
                ErrorSeverity.Fatal => "⚫",
                _ => "🔴"
            };
            
            toolStripStatusLabel.Text = $"{statusIcon} {GetSeverityDisplay(_severity)} | 🕐 {DateTime.Now:HH:mm:ss}";
        }

        private string GetSeverityDisplay(ErrorSeverity severity)
        {
            return severity switch
            {
                ErrorSeverity.Low => "Low (Information/Warning)",
                ErrorSeverity.Medium => "Medium (Recoverable Error)",
                ErrorSeverity.High => "High (Critical Error)",
                ErrorSeverity.Fatal => "Fatal (Application Termination)",
                _ => "Unknown"
            };
        }

        #endregion

        #region Event Handlers

        private void WireUpEvents()
        {
            buttonRetry.Click += ButtonRetry_Click;
            buttonCopyDetails.Click += ButtonCopyDetails_Click;
            buttonReportIssue.Click += ButtonReportIssue_Click;
            buttonViewLogs.Click += ButtonViewLogs_Click;
            buttonClose.Click += ButtonClose_Click;
        }

        private void ButtonRetry_Click(object sender, EventArgs e)
        {
            if (_retryAction != null)
            {
                try
                {
                    ShouldRetry = true;
                    bool success = _retryAction();
                    if (success)
                    {
                        ErrorDialogResult = DialogResult.Retry;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("The retry operation failed. Please check the technical details for more information.", 
                            "Retry Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    MessageBox.Show($"Error during retry: {ex.Message}", "Retry Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ButtonCopyDetails_Click(object sender, EventArgs e)
        {
            try
            {
                var details = new StringBuilder();
                details.AppendLine($"MTM Inventory Application Error Report");
                details.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                details.AppendLine("".PadRight(60, '='));
                details.AppendLine();
                details.AppendLine(richTextBoxTechnical.Text);
                
                Clipboard.SetText(details.ToString());
                MessageBox.Show("Error details copied to clipboard.", "Copy Successful", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($"Failed to copy details: {ex.Message}", "Copy Failed", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonReportIssue_Click(object sender, EventArgs e)
        {
            try
            {
                // In a real implementation, this would open an issue reporting system
                var message = "This would typically open your issue reporting system or send an email to IT support.\n\n" +
                             "For now, please copy the technical details and send them to your system administrator.";
                MessageBox.Show(message, "Report Issue", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private void ButtonViewLogs_Click(object sender, EventArgs e)
        {
            try
            {
                // In a real implementation, this would open the log viewer
                var message = "This would typically open the application log viewer.\n\n" +
                             "For now, please check the application's log directory for detailed logs.";
                MessageBox.Show(message, "View Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            ErrorDialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        #region Theme Support

        private void ApplyTheme()
        {
            try
            {
                // Apply theme using the existing Core_Themes system
                Core_Themes.ApplyDpiScaling(this);
                Core_Themes.ApplyRuntimeLayoutAdjustments(this);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                // Don't fail the error dialog if theming fails
            }
        }

        #endregion

        #region Helpers

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                // Allow Escape to close the dialog
                if (keyData == Keys.Escape)
                {
                    ButtonClose_Click(this, EventArgs.Empty);
                    return true;
                }

                // Allow Ctrl+C to copy details
                if (keyData == (Keys.Control | Keys.C))
                {
                    ButtonCopyDetails_Click(this, EventArgs.Empty);
                    return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        #endregion
    }

    #endregion
}