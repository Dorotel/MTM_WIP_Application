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
    #region Fields

    public static Helper_Database_Core HelperDatabaseCore =
        new(Helper_Database_Variables.GetConnectionString(
            Model_AppVariables.WipServerAddress,
            "mtm_wip_application",
            Model_AppVariables.User,
            Model_AppVariables.UserPin
        ));

    #endregion

    #region Query Methods

    internal static async Task<List<(string MethodName, string ErrorMessage)>> GetUniqueErrorsAsync(
        bool useAsync = false)
    {
        var uniqueErrors = new List<(string MethodName, string ErrorMessage)>();
        try
        {
            using var reader = useAsync
                ? await HelperDatabaseCore.ExecuteReader(
                    "SELECT DISTINCT `MethodName`, `ErrorMessage` FROM `log_error`",
                    useAsync: true)
                : HelperDatabaseCore.ExecuteReader("SELECT DISTINCT `MethodName`, `ErrorMessage` FROM `log_error`")
                    .Result;

            while (reader.Read())
                uniqueErrors.Add((reader.GetString("MethodName"), reader.GetString("ErrorMessage")));

            LoggingUtility.Log("GetUniqueErrors executed successfully.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await HandleException_GeneralError_CloseApp(ex, useAsync);
        }

        return uniqueErrors;
    }

    internal static async Task<DataTable> GetAllErrorsAsync(bool useAsync = false)
    {
        return await GetErrorsByQueryAsync("SELECT * FROM `log_error` ORDER BY `ErrorTime` DESC", null, useAsync);
    }

    internal static async Task<DataTable> GetErrorsByUserAsync(string user, bool useAsync = false)
    {
        return await GetErrorsByQueryAsync(
            "SELECT * FROM `log_error` WHERE `User` = @User ORDER BY `ErrorTime` DESC",
            new Dictionary<string, object> { ["@User"] = user }, useAsync);
    }

    internal static async Task<DataTable> GetErrorsByDateRangeAsync(DateTime start, DateTime end, bool useAsync = false)
    {
        return await GetErrorsByQueryAsync(
            "SELECT * FROM `log_error` WHERE `ErrorTime` BETWEEN @Start AND @End ORDER BY `ErrorTime` DESC",
            new Dictionary<string, object> { ["@Start"] = start, ["@End"] = end }, useAsync);
    }

    private static async Task<DataTable> GetErrorsByQueryAsync(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            return parameters == null
                ? await HelperDatabaseCore.ExecuteDataTable(sql, useAsync: useAsync)
                : await HelperDatabaseCore.ExecuteDataTable(sql, parameters, useAsync);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    #endregion

    #region Delete Methods

    internal static async Task DeleteErrorByIdAsync(int id, bool useAsync = false)
    {
        await ExecuteNonQueryAsync("DELETE FROM `log_error` WHERE `ID` = @Id",
            new Dictionary<string, object> { ["@Id"] = id }, useAsync);
    }

    internal static async Task DeleteAllErrorsAsync(bool useAsync = false)
    {
        await ExecuteNonQueryAsync("DELETE FROM `log_error`", null, useAsync);
    }

    private static async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object>? parameters, bool useAsync)
    {
        try
        {
            if (parameters == null)
                await HelperDatabaseCore.ExecuteNonQuery(sql, useAsync: useAsync);
            else
                await HelperDatabaseCore.ExecuteNonQuery(sql, parameters, useAsync);
        }
        catch (Exception ex)
        {
            // Use database error log for SQL exceptions, application error log otherwise
            if (ex is MySqlException)
                LoggingUtility.LogDatabaseError(ex);
            else
                LoggingUtility.LogApplicationError(ex);

            await HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    #endregion

    #region Error Handling Methods

    // Prevents error message spam by tracking the last shown error and time
    private static string? _lastErrorMessage;
    private static DateTime _lastErrorTime = DateTime.MinValue;
    private static readonly TimeSpan ErrorMessageCooldown = TimeSpan.FromSeconds(5);

    private static string? _lastSqlErrorMessage;
    private static DateTime _lastSqlErrorTime = DateTime.MinValue;
    private static readonly TimeSpan SqlErrorMessageCooldown = TimeSpan.FromSeconds(5);

    #region Error Message Helpers

    private static bool ShouldShowErrorMessage(string message)
    {
        var now = DateTime.Now;
        lock (typeof(Dao_ErrorLog))
        {
            if (_lastErrorMessage == message && now - _lastErrorTime < ErrorMessageCooldown) return false;
            _lastErrorMessage = message;
            _lastErrorTime = now;
            return true;
        }
    }

    private static bool ShouldShowSqlErrorMessage(string message)
    {
        var now = DateTime.Now;
        lock (typeof(Dao_ErrorLog))
        {
            if (_lastSqlErrorMessage == message && now - _lastSqlErrorTime < SqlErrorMessageCooldown)
                return false;
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

            var isConnectionError = ex.Message.Contains("Unable to connect to any of the specified MySQL hosts.")
                                    || ex.Message.Contains("Access denied for user")
                                    || ex.Message.Contains("Can't connect to MySQL server on")
                                    || ex.Message.Contains("Unknown MySQL server host")
                                    || ex.Message.Contains("Lost connection to MySQL server")
                                    || ex.Message.Contains("MySQL server has gone away");

            var message = $"SQL Error in method: {callerName}, Control: {controlName}\n{ex.Message}";

            if (isConnectionError)
            {
                if (ShouldShowSqlErrorMessage(message))
                    MessageBox.Show(
                        @"Database connection error. The application will now close.",
                        @"Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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
                    MessageBox.Show(message, @"SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var errorType = ex switch
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

            var message = $"{errorType}\nMethod: {callerName}\nControl: {controlName}\nException:\n{ex.Message}";

            var isCritical = ex is OutOfMemoryException || ex is StackOverflowException ||
                             ex is AccessViolationException;

            var mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
            if (mainForm != null) mainForm.ConnectionRecoveryManager.HandleConnectionLost();

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
        var parameters = new Dictionary<string, object>
        {
            ["@User"] = Model_AppVariables.User,
            ["@Severity"] = severity,
            ["@ErrorType"] = errorType,
            ["@ErrorMessage"] = errorMessage,
            ["@StackTrace"] = stackTrace ?? "",
            ["@ModuleName"] = moduleName ?? "",
            ["@MethodName"] = methodName ?? "",
            ["@AdditionalInfo"] = additionalInfo ?? "",
            ["@MachineName"] = Environment.MachineName,
            ["@OSVersion"] = Environment.OSVersion.ToString(),
            ["@AppVersion"] = Application.ProductVersion,
            ["@ErrorTime"] = DateTime.Now
        };

        var sql = @"
            INSERT INTO `log_error` 
            (`User`, `Severity`, `ErrorType`, `ErrorMessage`, `StackTrace`, `ModuleName`, `MethodName`, `AdditionalInfo`, `MachineName`, `OSVersion`, `AppVersion`, `ErrorTime`) 
            VALUES 
            (@User, @Severity, @ErrorType, @ErrorMessage, @StackTrace, @ModuleName, @MethodName, @AdditionalInfo, @MachineName, @OSVersion, @AppVersion, @ErrorTime)";
        await HelperDatabaseCore.ExecuteNonQuery(sql, parameters, useAsync, CommandType.Text);
    }

    #endregion

    #region Synchronous Helpers

    internal static List<(string MethodName, string ErrorMessage)> GetUniqueErrors()
    {
        return GetUniqueErrorsAsync(false).GetAwaiter().GetResult();
    }

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