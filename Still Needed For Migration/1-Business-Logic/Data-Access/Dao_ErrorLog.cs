using System.Data;
using System.Diagnostics;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_ErrorLog

internal static class Dao_ErrorLog
{


    #region Query Methods

    internal static async Task<List<(string MethodName, string ErrorMessage)>> GetUniqueErrorsAsync(
        bool useAsync = true) // Default to async for better performance
    {
        List<(string MethodName, string ErrorMessage)> uniqueErrors = new();
        try
        {
            // FIXED: Use Helper_Database_StoredProcedure instead of Helper_Database_Core
            var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "log_error_Get_Unique",
                null, // No parameters needed
                null, // No progress helper for this method
                useAsync
            );

            if (dataResult.IsSuccess && dataResult.Data != null)
            {
                foreach (DataRow row in dataResult.Data.Rows)
                {
                    uniqueErrors.Add((
                        row["MethodName"]?.ToString() ?? "", 
                        row["ErrorMessage"]?.ToString() ?? ""
                    ));
                }
            }
            else
            {
                LoggingUtility.Log($"GetUniqueErrors failed: {dataResult.ErrorMessage}");
            }

            LoggingUtility.Log("GetUniqueErrors executed successfully.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            // Don't call HandleException_GeneralError_CloseApp here to avoid recursion during startup
            LoggingUtility.Log($"GetUniqueErrors failed with exception: {ex.Message}");
        }

        return uniqueErrors;
    }

    internal static async Task<DataTable> GetAllErrorsAsync(bool useAsync = false) =>
        await GetErrorsByStoredProcedureAsync("log_error_Get_All", null, useAsync);

    internal static async Task<DataTable> GetErrorsByUserAsync(string user, bool useAsync = false) =>
        await GetErrorsByStoredProcedureAsync(
            "log_error_Get_ByUser",
            new Dictionary<string, object> { ["User"] = user }, useAsync); // FIXED: Remove p_ prefix

    internal static async Task<DataTable>
        GetErrorsByDateRangeAsync(DateTime start, DateTime end, bool useAsync = false) =>
        await GetErrorsByStoredProcedureAsync(
            "log_error_Get_ByDateRange",
            new Dictionary<string, object> { ["StartDate"] = start, ["EndDate"] = end }, useAsync); // FIXED: Remove p_ prefix

    private static async Task<DataTable> GetErrorsByStoredProcedureAsync(string procedureName, 
        Dictionary<string, object>? parameters, bool useAsync)
    {
        try
        {
            // FIXED: Use Helper_Database_StoredProcedure for proper status handling
            var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                procedureName,
                parameters,
                null, // No progress helper for these methods
                useAsync
            );

            if (dataResult.IsSuccess && dataResult.Data != null)
            {
                return dataResult.Data;
            }
            else
            {
                LoggingUtility.Log($"{procedureName} failed: {dataResult.ErrorMessage}");
                return new DataTable();
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            // Don't call HandleException_GeneralError_CloseApp here to avoid recursion during startup
            LoggingUtility.Log($"{procedureName} failed with exception: {ex.Message}");
            return new DataTable();
        }
    }

    #endregion

    #region Delete Methods

    internal static async Task DeleteErrorByIdAsync(int id, bool useAsync = false) =>
        await ExecuteStoredProcedureNonQueryAsync("log_error_Delete_ById",
            new Dictionary<string, object> { ["Id"] = id }, useAsync); // FIXED: Remove p_ prefix

    internal static async Task DeleteAllErrorsAsync(bool useAsync = false) =>
        await ExecuteStoredProcedureNonQueryAsync("log_error_Delete_All", null, useAsync);

    private static async Task ExecuteStoredProcedureNonQueryAsync(string procedureName, 
        Dictionary<string, object>? parameters, bool useAsync)
    {
        try
        {
            // FIXED: Use Helper_Database_StoredProcedure for proper status handling
            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                procedureName,
                parameters,
                null, // No progress helper for these methods
                useAsync
            );

            if (!result.IsSuccess)
            {
                LoggingUtility.Log($"{procedureName} failed: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            if (ex is MySqlException)
            {
                LoggingUtility.LogDatabaseError(ex);
            }
            else
            {
                LoggingUtility.LogApplicationError(ex);
            }

            // Don't call HandleException_GeneralError_CloseApp here to avoid recursion during startup
            LoggingUtility.Log($"{procedureName} failed with exception: {ex.Message}");
        }
    }

    #endregion

    #region Error Handling Methods

    private static string? _lastErrorMessage;
    private static DateTime _lastErrorTime = DateTime.MinValue;
    private static readonly TimeSpan ErrorMessageCooldown = TimeSpan.FromSeconds(5);

    private static string? _lastSqlErrorMessage;
    private static DateTime _lastSqlErrorTime = DateTime.MinValue;
    private static readonly TimeSpan SqlErrorMessageCooldown = TimeSpan.FromSeconds(5);

    #region Error Message Helpers

    private static bool ShouldShowErrorMessage(string message)
    {
        DateTime now = DateTime.Now;
        lock (typeof(Dao_ErrorLog))
        {
            if (_lastErrorMessage == message && now - _lastErrorTime < ErrorMessageCooldown)
            {
                return false;
            }

            _lastErrorMessage = message;
            _lastErrorTime = now;
            return true;
        }
    }

    private static bool ShouldShowSqlErrorMessage(string message)
    {
        DateTime now = DateTime.Now;
        lock (typeof(Dao_ErrorLog))
        {
            if (_lastSqlErrorMessage == message && now - _lastSqlErrorTime < SqlErrorMessageCooldown)
            {
                return false;
            }

            _lastSqlErrorMessage = message;
            _lastSqlErrorTime = now;
            return true;
        }
    }

    #endregion

    internal static async Task HandleException_SQLError_CloseApp(
        Exception ex,
        bool useAsync = false,
        [System.Runtime.CompilerServices.CallerMemberName]
        string callerName = "",
        string controlName = "")
    {
        try
        {
            LoggingUtility.LogDatabaseError(ex);
            LoggingUtility.Log($"SQL Error in method: {callerName}, Control: {controlName}");

            if (ex is MySqlException mysqlEx)
            {
                LoggingUtility.Log($"MySQL Error Code: {mysqlEx.Number}");
                LoggingUtility.Log($"MySQL Error Details: {mysqlEx.Message}");
            }

            bool isConnectionError = ex.Message.Contains("Unable to connect to any of the specified MySQL hosts.")
                                     || ex.Message.Contains("Access denied for user")
                                     || ex.Message.Contains("Can't connect to MySQL server on")
                                     || ex.Message.Contains("Unknown MySQL server host")
                                     || ex.Message.Contains("Lost connection to MySQL server")
                                     || ex.Message.Contains("MySQL server has gone away");

            string message = $"SQL Error in method: {callerName}, Control: {controlName}\n{ex.Message}";

            if (isConnectionError)
            {
                if (ShouldShowSqlErrorMessage(message))
                {
                    MessageBox.Show(
                        @"Database connection error. The application will now close.",
                        @"Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                Process.GetCurrentProcess().Kill();
            }
            else
            {
                await LogErrorToDatabaseAsync(
                    "Critical",
                    ex.GetType().ToString(),
                    ex.Message,
                    ex.StackTrace,
                    "",
                    callerName,
                    controlName,
                    useAsync
                );

                if (ShouldShowSqlErrorMessage(message))
                {
                    MessageBox.Show(message, @"SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception innerEx)
        {
            LoggingUtility.LogApplicationError(innerEx);
        }
    }

    internal static async Task HandleException_GeneralError_CloseApp(
        Exception ex,
        bool useAsync = false,
        [System.Runtime.CompilerServices.CallerMemberName]
        string callerName = "",
        string controlName = "")
    {
        try
        {
            string errorType = ex switch
            {
                ArgumentNullException => "A required argument was null.",
                ArgumentOutOfRangeException => "An argument was out of range.",
                InvalidOperationException => "An invalid operation occurred.",
                FormatException => "A format error occurred.",
                NullReferenceException => "A null reference occurred.",
                OutOfMemoryException => "The application ran out of memory.",
                StackOverflowException => "A stack overflow occurred.",
                AccessViolationException => "An access violation occurred.",
                _ => "An unexpected error occurred."
            };

            string message = $"{errorType}\nMethod: {callerName}\nControl: {controlName}\nException:\n{ex.Message}";

            bool isCritical = ex is OutOfMemoryException || ex is StackOverflowException ||
                              ex is AccessViolationException;

            MainForm? mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.ConnectionRecoveryManager.HandleConnectionLost();
            }

            LoggingUtility.LogApplicationError(ex);

            await LogErrorToDatabaseAsync(
                isCritical ? "Critical" : "Error",
                ex.GetType().ToString(),
                ex.Message,
                ex.StackTrace,
                "",
                callerName,
                controlName,
                useAsync
            );

            if (ShouldShowErrorMessage(message))
            {
                if (isCritical)
                {
                    MessageBox.Show(message + "\n\nThe application will now close due to a critical error.",
                        @"Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    MessageBox.Show(message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoggingUtility.Log("HandleException_GeneralError_CloseApp executed successfully.");
        }
        catch (Exception innerEx)
        {
            LoggingUtility.LogApplicationError(innerEx);
            await HandleException_GeneralError_CloseApp(innerEx, useAsync, controlName: controlName);
        }
    }

    private static async Task LogErrorToDatabaseAsync(
        string severity,
        string errorType,
        string errorMessage,
        string? stackTrace,
        string moduleName,
        string methodName,
        string? additionalInfo,
        bool useAsync)
    {
        try
        {
            // FIXED: Use Helper_Database_StoredProcedure for proper status handling
            Dictionary<string, object> parameters = new()
            {
                ["User"] = Model_AppVariables.User ?? "Unknown", // FIXED: Remove p_ prefix
                ["Severity"] = severity,
                ["ErrorType"] = errorType,
                ["ErrorMessage"] = errorMessage,
                ["StackTrace"] = stackTrace ?? "",
                ["ModuleName"] = moduleName ?? "",
                ["MethodName"] = methodName ?? "",
                ["AdditionalInfo"] = additionalInfo ?? "",
                ["MachineName"] = Environment.MachineName,
                ["OSVersion"] = Environment.OSVersion.ToString(),
                ["AppVersion"] = Application.ProductVersion,
                ["ErrorTime"] = DateTime.Now
            };

            var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                Model_AppVariables.ConnectionString,
                "log_error_Add_Error",
                parameters,
                null, // No progress helper for this method
                useAsync
            );

            if (!result.IsSuccess)
            {
                // If database logging fails, just log to file system
                LoggingUtility.Log($"Failed to log error to database: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            // If logging to database fails, we don't want to create a recursive error loop
            // Just log to the file system via LoggingUtility
            LoggingUtility.LogApplicationError(new Exception($"Failed to log error to database: {ex.Message}", ex));
        }
    }

    #endregion

    #region Synchronous Helpers

    internal static List<(string MethodName, string ErrorMessage)> GetUniqueErrors() =>
        GetUniqueErrorsAsync(false).GetAwaiter().GetResult();

    internal static void LogErrorWithMethod(Exception ex,
        [System.Runtime.CompilerServices.CallerMemberName]
        string methodName = "")
    {
        LoggingUtility.LogApplicationError(ex);
        LoggingUtility.Log($"Error in {methodName}: {ex.Message}");
    }

    #endregion
}

#endregion
