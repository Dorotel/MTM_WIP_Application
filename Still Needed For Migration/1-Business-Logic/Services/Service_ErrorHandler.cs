using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Forms.ErrorDialog;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Services;

internal static class Service_ErrorHandler
{
    #region Fields
    
    private static readonly Dictionary<string, int> _errorFrequency = new();
    private static readonly object _errorLock = new();
    
    #endregion

    #region Public Methods - Enhanced Error Handling

    /// <summary>
    /// Handle any exception with enhanced error dialog and automatic logging
    /// </summary>
    /// <param name="ex">The exception that occurred</param>
    /// <param name="severity">The severity level of the error</param>
    /// <param name="retryAction">Optional action to retry the failed operation</param>
    /// <param name="contextData">Additional context information</param>
    /// <param name="callerName">Automatically filled caller method name</param>
    /// <param name="controlName">Name of the control or form where error occurred</param>
    /// <returns>True if user chose to retry and retry succeeded, false otherwise</returns>
    public static bool HandleException(Exception ex, 
        ErrorSeverity severity = ErrorSeverity.Medium,
        Func<bool>? retryAction = null,
        Dictionary<string, object>? contextData = null,
        [CallerMemberName] string callerName = "",
        string controlName = "")
    {
        try
        {
            // Always log the error first
            LoggingUtility.LogApplicationError(ex);
            LogErrorContext(ex, callerName, controlName, contextData);
            
            // Handle connection recovery if it's a database error
            if (IsDatabaseError(ex))
            {
                HandleConnectionRecovery();
            }
            
            // Check error frequency to prevent spam
            if (ShouldSuppressError(ex, callerName))
            {
                return false;
            }
            
            // Show enhanced error dialog
            using var errorDialog = new EnhancedErrorDialog(ex, callerName, controlName, severity, retryAction, contextData);
            var result = errorDialog.ShowDialog();
            
            // Handle critical errors that should terminate the application
            if (severity == ErrorSeverity.Fatal || IsFatalError(ex))
            {
                HandleFatalError(ex, callerName, controlName);
                return false;
            }
            
            return errorDialog.ShouldRetry && result == DialogResult.Retry;
        }
        catch (Exception innerEx)
        {
            // Fallback error handling if our enhanced handler fails
            LoggingUtility.LogApplicationError(innerEx);
            FallbackErrorDisplay(ex, callerName);
            return false;
        }
    }

    /// <summary>
    /// Handle database-specific errors with automatic connection recovery
    /// </summary>
    public static bool HandleDatabaseError(Exception ex,
        Func<bool>? retryAction = null,
        Dictionary<string, object>? contextData = null,
        [CallerMemberName] string callerName = "",
        string controlName = "",
        string methodName = "")
    {
        // Add database-specific context
        var dbContextData = contextData ?? new Dictionary<string, object>();
        dbContextData["ErrorType"] = "Database";
        dbContextData["ConnectionString"] = "Hidden for security";
        
        LoggingUtility.LogDatabaseError(ex);
        
        // Use methodName if provided, otherwise use callerName
        var effectiveCallerName = !string.IsNullOrEmpty(methodName) ? methodName : callerName;
        
        return HandleException(ex, ErrorSeverity.High, retryAction, dbContextData, effectiveCallerName, controlName);
    }

    /// <summary>
    /// Handle validation errors (user input errors)
    /// </summary>
    public static void HandleValidationError(string message, string field = "", 
        [CallerMemberName] string callerName = "",
        string controlName = "")
    {
        try
        {
            LoggingUtility.Log($"Validation error in {callerName}: {message}");
            
            var validationEx = new ArgumentException($"Validation failed for {field}: {message}");
            var contextData = new Dictionary<string, object>
            {
                ["ValidationType"] = "Input Validation",
                ["Field"] = field,
                ["UserMessage"] = message
            };
            
            HandleException(validationEx, ErrorSeverity.Low, null, contextData, callerName, controlName);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
        }
    }

    /// <summary>
    /// Handle unauthorized access with appropriate messaging
    /// </summary>
    public static void HandleUnauthorizedAccess(string operation = "",
        [CallerMemberName] string callerName = "",
        string controlName = "")
    {
        try
        {
            var unauthorizedEx = new UnauthorizedAccessException(
                $"Access denied for operation: {operation}. Please check your permissions or run as administrator.");
            
            var contextData = new Dictionary<string, object>
            {
                ["Operation"] = operation,
                ["UserName"] = Environment.UserName,
                ["IsAdmin"] = IsRunningAsAdministrator()
            };
            
            HandleException(unauthorizedEx, ErrorSeverity.Medium, null, contextData, callerName, controlName);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
        }
    }

    /// <summary>
    /// Handle file operation errors
    /// </summary>
    public static bool HandleFileError(Exception ex, string filePath = "",
        Func<bool>? retryAction = null,
        [CallerMemberName] string callerName = "",
        string controlName = "")
    {
        try
        {
            var contextData = new Dictionary<string, object>
            {
                ["FilePath"] = filePath,
                ["FileExists"] = !string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath),
                ["ErrorType"] = "File Operation"
            };
            
            return HandleException(ex, ErrorSeverity.Medium, retryAction, contextData, callerName, controlName);
        }
        catch (Exception innerEx)
        {
            LoggingUtility.LogApplicationError(innerEx);
            return false;
        }
    }

    /// <summary>
    /// Handle network/connectivity errors
    /// </summary>
    public static bool HandleNetworkError(Exception ex,
        Func<bool>? retryAction = null,
        [CallerMemberName] string callerName = "",
        string controlName = "")
    {
        try
        {
            var contextData = new Dictionary<string, object>
            {
                ["ErrorType"] = "Network/Connectivity",
                ["NetworkAvailable"] = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()
            };
            
            return HandleException(ex, ErrorSeverity.High, retryAction, contextData, callerName, controlName);
        }
        catch (Exception innerEx)
        {
            LoggingUtility.LogApplicationError(innerEx);
            return false;
        }
    }

    /// <summary>
    /// Show a confirmation dialog (not an error - just a user confirmation)
    /// </summary>
    public static DialogResult ShowConfirmation(string message, string title = "Confirmation",
        MessageBoxButtons buttons = MessageBoxButtons.YesNo,
        MessageBoxIcon icon = MessageBoxIcon.Question)
    {
        try
        {
            LoggingUtility.Log($"Confirmation dialog shown: {title} - {message}");
            return MessageBox.Show(message, title, buttons, icon);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            return DialogResult.Cancel;
        }
    }

    /// <summary>
    /// Show a warning dialog (not an error - just a user warning)
    /// </summary>
    public static DialogResult ShowWarning(string message, string title = "Warning", 
        MessageBoxButtons buttons = MessageBoxButtons.OKCancel,
        MessageBoxIcon icon = MessageBoxIcon.Warning)
    {
        try
        {
            LoggingUtility.Log($"Warning dialog shown: {title} - {message}");
            return MessageBox.Show(message, title, buttons, icon);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            return DialogResult.Cancel;
        }
    }

    /// <summary>
    /// Show an information dialog (not an error - just informational message)
    /// </summary>
    public static DialogResult ShowInformation(string message, string title = "Information", 
        MessageBoxButtons buttons = MessageBoxButtons.OK,
        MessageBoxIcon icon = MessageBoxIcon.Information,
        string controlName = "")
    {
        try
        {
            var logMessage = $"Information dialog shown: {title} - {message}";
            if (!string.IsNullOrEmpty(controlName))
            {
                logMessage += $" (Control: {controlName})";
            }
            LoggingUtility.Log(logMessage);
            return MessageBox.Show(message, title, buttons, icon);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            return DialogResult.Cancel;
        }
    }

    #endregion

    #region Legacy Compatibility Methods

    [Obsolete("Use HandleException or HandleDatabaseError instead", false)]
    public static void HandleDatabaseError()
    {
        var dbEx = new Exception("Legacy database error handling - please update to use new HandleDatabaseError method");
        HandleDatabaseError(dbEx);
    }

    [Obsolete("Use HandleException instead", false)]
    public static void HandleGeneralException(Exception ex)
    {
        HandleException(ex, ErrorSeverity.Medium);
    }

    [Obsolete("Use HandleUnauthorizedAccess instead", false)]  
    public static void HandleUnauthorizedAccessException(UnauthorizedAccessException ex)
    {
        HandleUnauthorizedAccess(ex.Message);
    }

    #endregion

    #region Private Helper Methods

    private static void LogErrorContext(Exception ex, string callerName, string controlName, Dictionary<string, object>? contextData)
    {
        try
        {
            var contextLog = $"Error context - Caller: {callerName}, Control: {controlName}";
            if (contextData?.Any() == true)
            {
                contextLog += $", Context: {string.Join(", ", contextData.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";
            }
            LoggingUtility.Log(contextLog);
        }
        catch (Exception logEx)
        {
            // Don't let logging errors break error handling
            System.Diagnostics.Debug.WriteLine($"Failed to log error context: {logEx.Message}");
        }
    }

    private static bool IsDatabaseError(Exception ex)
    {
        return ex is MySql.Data.MySqlClient.MySqlException ||
               ex.Message.ToLower().Contains("database") ||
               ex.Message.ToLower().Contains("connection") ||
               ex.Message.ToLower().Contains("mysql") ||
               ex.Message.ToLower().Contains("sql");
    }

    private static bool IsFatalError(Exception ex)
    {
        return ex is OutOfMemoryException ||
               ex is StackOverflowException ||
               ex is AccessViolationException ||
               ex is System.Security.SecurityException;
    }

    private static void HandleConnectionRecovery()
    {
        try
        {
            if (Application.OpenForms.OfType<MainForm>().Any())
            {
                var mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
                mainForm?.ConnectionRecoveryManager.HandleConnectionLost();
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
        }
    }

    private static bool ShouldSuppressError(Exception ex, string callerName)
    {
        lock (_errorLock)
        {
            var errorKey = $"{ex.GetType().Name}:{callerName}:{ex.Message.GetHashCode()}";
            
            if (_errorFrequency.ContainsKey(errorKey))
            {
                _errorFrequency[errorKey]++;
                // Suppress if we've seen this exact error more than 3 times in this session
                return _errorFrequency[errorKey] > 3;
            }
            
            _errorFrequency[errorKey] = 1;
            return false;
        }
    }

    private static void HandleFatalError(Exception ex, string callerName, string controlName)
    {
        try
        {
            LoggingUtility.Log($"Fatal error occurred in {callerName} ({controlName}). Application will terminate.");
            
            // Give user a chance to save work or see what happened
            var message = $"A fatal error has occurred and the application must close.\n\n" +
                         $"Error: {ex.Message}\n\n" +
                         $"Location: {callerName}\n" +
                         $"The error details have been logged for analysis.";
            
            MessageBox.Show(message, "Fatal Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            
            // Force application termination
            Environment.Exit(1);
        }
        catch
        {
            // If even our fatal error handler fails, just terminate
            Environment.Exit(1);
        }
    }

    private static void FallbackErrorDisplay(Exception ex, string callerName)
    {
        try
        {
            var message = $"An error occurred in {callerName}:\n\n{ex.Message}";
            MessageBox.Show(message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch
        {
            // Last resort - show basic system error
            MessageBox.Show($"Critical error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static bool IsRunningAsAdministrator()
    {
        try
        {
            using var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
        catch
        {
            return false;
        }
    }

    #endregion
}
