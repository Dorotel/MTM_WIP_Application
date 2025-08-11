using System;
using System.Drawing;
using System.Windows.Forms;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Helpers
{
    /// <summary>
    /// Enhanced progress reporting system with visual error feedback for stored procedures
    /// Provides success/failure status reporting with red progress bars on errors
    /// </summary>
    public class Helper_StoredProcedureProgress
    {
        #region Fields

        private readonly ToolStripProgressBar _progressBar;
        private readonly ToolStripStatusLabel _statusLabel;
        private readonly Form _parentForm;
        private readonly Color _originalProgressBarColor;
        private readonly Color _errorColor = Color.Red;
        private readonly Color _successColor = Color.Green;
        private bool _isErrorState = false;

        #endregion

        #region Constructors

        public Helper_StoredProcedureProgress(
            ToolStripProgressBar progressBar, 
            ToolStripStatusLabel statusLabel, 
            Form parentForm)
        {
            _progressBar = progressBar ?? throw new ArgumentNullException(nameof(progressBar));
            _statusLabel = statusLabel ?? throw new ArgumentNullException(nameof(statusLabel));
            _parentForm = parentForm ?? throw new ArgumentNullException(nameof(parentForm));
            
            // Store the original progress bar color
            _originalProgressBarColor = _progressBar.ForeColor;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Show progress with initialization message
        /// </summary>
        /// <param name="status">Initial status message</param>
        public void ShowProgress(string status = "Loading...")
        {
            ThreadSafeInvoke(() =>
            {
                _isErrorState = false;
                _progressBar.Visible = true;
                _progressBar.Value = 0;
                _progressBar.ForeColor = _originalProgressBarColor;
                _statusLabel.ForeColor = SystemColors.ControlText;
                Application.DoEvents();
                _statusLabel.Text = status;
            });
        }

        /// <summary>
        /// Update progress with success status
        /// </summary>
        /// <param name="progress">Progress percentage (0-100)</param>
        /// <param name="status">Status message</param>
        public void UpdateProgress(int progress, string status)
        {
            ThreadSafeInvoke(() =>
            {
                if (!_isErrorState)
                {
                    progress = Math.Max(0, Math.Min(100, progress));
                    _progressBar.Value = progress;
                    _progressBar.ForeColor = _originalProgressBarColor;
                    _statusLabel.ForeColor = SystemColors.ControlText;
                    Application.DoEvents();
                    _statusLabel.Text = $"{status} ({progress}%)";
                }
            });
        }

        /// <summary>
        /// Show error state with red progress bar and error message
        /// </summary>
        /// <param name="errorMessage">Error message to display</param>
        /// <param name="progress">Optional progress percentage (defaults to current value)</param>
        public void ShowError(string errorMessage, int? progress = null)
        {
            ThreadSafeInvoke(() =>
            {
                _isErrorState = true;
                _progressBar.ForeColor = _errorColor;
                _statusLabel.ForeColor = _errorColor;
                
                if (progress.HasValue)
                {
                    int clampedProgress = Math.Max(0, Math.Min(100, progress.Value));
                    _progressBar.Value = clampedProgress;
                }
                
                Application.DoEvents();
                _statusLabel.Text = $"ERROR: {errorMessage}";
                
                // Keep error visible for a moment
                System.Threading.Tasks.Task.Delay(100).Wait();
            });
        }

        /// <summary>
        /// Show success completion with green progress bar
        /// </summary>
        /// <param name="successMessage">Success message to display</param>
        public void ShowSuccess(string successMessage)
        {
            ThreadSafeInvoke(() =>
            {
                _isErrorState = false;
                _progressBar.Value = 100;
                _progressBar.ForeColor = _successColor;
                _statusLabel.ForeColor = _successColor;
                Application.DoEvents();
                _statusLabel.Text = $"SUCCESS: {successMessage}";
            });
        }

        /// <summary>
        /// Hide progress and reset to ready state
        /// </summary>
        public void HideProgress()
        {
            ThreadSafeInvoke(() =>
            {
                _isErrorState = false;
                _progressBar.Visible = false;
                _progressBar.ForeColor = _originalProgressBarColor;
                _statusLabel.ForeColor = SystemColors.ControlText;
                Application.DoEvents();
                _statusLabel.Text = "Ready";
            });
        }

        /// <summary>
        /// Update status message without changing progress
        /// </summary>
        /// <param name="message">Status message</param>
        public void UpdateStatus(string message)
        {
            ThreadSafeInvoke(() =>
            {
                if (!_isErrorState)
                {
                    _statusLabel.ForeColor = SystemColors.ControlText;
                }
                _statusLabel.Text = message;
            });
        }

        /// <summary>
        /// Check if currently in error state
        /// </summary>
        public bool IsInErrorState => _isErrorState;

        /// <summary>
        /// Reset error state to allow normal progress updates
        /// </summary>
        public void ResetErrorState()
        {
            ThreadSafeInvoke(() =>
            {
                _isErrorState = false;
                _progressBar.ForeColor = _originalProgressBarColor;
                _statusLabel.ForeColor = SystemColors.ControlText;
            });
        }

        /// <summary>
        /// Process stored procedure result and update progress accordingly
        /// </summary>
        /// <param name="status">Stored procedure status (0 = success, 1 = warning, -1 = error)</param>
        /// <param name="errorMessage">Error/success message from stored procedure</param>
        /// <param name="successMessage">Custom success message (optional)</param>
        /// <param name="finalProgress">Final progress percentage (defaults to 100)</param>
        public void ProcessStoredProcedureResult(int status, string errorMessage, string successMessage = null, int finalProgress = 100)
        {
            switch (status)
            {
                case 0: // Success
                    ShowSuccess(successMessage ?? errorMessage);
                    break;
                case 1: // Warning/Not Found
                    ShowError($"Warning: {errorMessage}", finalProgress);
                    break;
                case -1: // Error
                    ShowError(errorMessage, finalProgress);
                    break;
                default:
                    ShowError($"Unknown status ({status}): {errorMessage}", finalProgress);
                    break;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Thread-safe invoke for UI operations
        /// </summary>
        /// <param name="action">Action to invoke on UI thread</param>
        private void ThreadSafeInvoke(Action action)
        {
            if (_parentForm.InvokeRequired)
            {
                _parentForm.Invoke(action);
            }
            else
            {
                action();
            }
        }

        #endregion

        #region Static Factory Methods

        /// <summary>
        /// Create a new progress helper for the given controls
        /// </summary>
        /// <param name="progressBar">Progress bar control</param>
        /// <param name="statusLabel">Status label control</param>
        /// <param name="parentForm">Parent form for thread-safe operations</param>
        /// <returns>New progress helper instance</returns>
        public static Helper_StoredProcedureProgress Create(
            ToolStripProgressBar progressBar, 
            ToolStripStatusLabel statusLabel, 
            Form parentForm)
        {
            return new Helper_StoredProcedureProgress(progressBar, statusLabel, parentForm);
        }

        #endregion
    }

    /// <summary>
    /// Result class for stored procedure operations with progress reporting
    /// </summary>
    public class StoredProcedureResult<T>
    {
        public bool IsSuccess => Status == 0;
        public int Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public T? Data { get; set; }
        public Exception? Exception { get; set; }

        public static StoredProcedureResult<T> Success(T data, string message = "Operation completed successfully")
        {
            return new StoredProcedureResult<T>
            {
                Status = 0,
                ErrorMessage = message,
                Data = data
            };
        }

        public static StoredProcedureResult<T> Warning(string message, T? data = default)
        {
            return new StoredProcedureResult<T>
            {
                Status = 1,
                ErrorMessage = message,
                Data = data
            };
        }

        public static StoredProcedureResult<T> Error(string message, Exception? exception = null, T? data = default)
        {
            return new StoredProcedureResult<T>
            {
                Status = -1,
                ErrorMessage = message,
                Exception = exception,
                Data = data
            };
        }
    }

    /// <summary>
    /// Non-generic result class for stored procedure operations
    /// </summary>
    public class StoredProcedureResult
    {
        public bool IsSuccess => Status == 0;
        public int Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public Exception? Exception { get; set; }

        public static StoredProcedureResult Success(string message = "Operation completed successfully")
        {
            return new StoredProcedureResult
            {
                Status = 0,
                ErrorMessage = message
            };
        }

        public static StoredProcedureResult Warning(string message)
        {
            return new StoredProcedureResult
            {
                Status = 1,
                ErrorMessage = message
            };
        }

        public static StoredProcedureResult Error(string message, Exception? exception = null)
        {
            return new StoredProcedureResult
            {
                Status = -1,
                ErrorMessage = message,
                Exception = exception
            };
        }
    }
}
