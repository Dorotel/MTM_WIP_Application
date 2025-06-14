using MTM_WIP_Application.Core;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace MTM_WIP_Application.Data;

/// <summary>
/// Data access and error handling for the application's error log.
/// </summary>
internal static class ErrorLogDao
{
    // --- Query Methods ---

    public static SqlHelper SqlHelper =
        new(SqlVariables.GetConnectionString(
            WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            WipAppVariables.User,
            WipAppVariables.UserPin
        ));

    internal static async Task<List<(string MethodName, string ErrorMessage)>> GetUniqueErrorsAsync(
        bool useAsync = false)
    {
        var uniqueErrors = new List<(string MethodName, string ErrorMessage)>();
        try
        {
            using var reader = useAsync
                ? await SqlHelper.ExecuteReader("SELECT DISTINCT `MethodName`, `ErrorMessage` FROM `log_error`",
                    useAsync: true)
                : SqlHelper.ExecuteReader("SELECT DISTINCT `MethodName`, `ErrorMessage` FROM `log_error`").Result;

            while (reader.Read())
                uniqueErrors.Add((reader.GetString("MethodName"), reader.GetString("ErrorMessage")));

            AppLogger.Log("GetUniqueErrors executed successfully.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
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
                ? await SqlHelper.ExecuteDataTable(sql, useAsync: useAsync)
                : await SqlHelper.ExecuteDataTable(sql, parameters, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    // --- Delete Methods ---

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
                await SqlHelper.ExecuteNonQuery(sql, useAsync: useAsync);
            else
                await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync);
        }
        catch (Exception ex)
        {
            // Use database error log for SQL exceptions, application error log otherwise
            if (ex is MySqlException)
                AppLogger.LogDatabaseError(ex);
            else
                AppLogger.LogApplicationError(ex);

            await HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    // --- Error Handling Methods ---

    // Prevents error message spam by tracking the last shown error and time
    private static string? _lastErrorMessage;
    private static DateTime _lastErrorTime = DateTime.MinValue;
    private static readonly TimeSpan ErrorMessageCooldown = TimeSpan.FromSeconds(5);

    private static string? _lastSqlErrorMessage;
    private static DateTime _lastSqlErrorTime = DateTime.MinValue;
    private static readonly TimeSpan SqlErrorMessageCooldown = TimeSpan.FromSeconds(5);

    // --- Helper: ShouldShowErrorMessage ---
    private static bool ShouldShowErrorMessage(string message)
    {
        var now = DateTime.Now;
        lock (typeof(ErrorLogDao))
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
        lock (typeof(ErrorLogDao))
        {
            if (_lastSqlErrorMessage == message && now - _lastSqlErrorTime < SqlErrorMessageCooldown)
                return false;
            _lastSqlErrorMessage = message;
            _lastSqlErrorTime = now;
            return true;
        }
    }

    internal static async Task HandleException_SQLError_CloseApp(
        Exception ex,
        bool useAsync = false,
        [System.Runtime.CompilerServices.CallerMemberName]
        string callerName = "",
        string controlName = "")
    {
        try
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log($"SQL Error in method: {callerName}, Control: {controlName}");

            if (ex is MySqlException mysqlEx)
            {
                AppLogger.Log($"MySQL Error Code: {mysqlEx.Number}");
                AppLogger.Log($"MySQL Error Details: {mysqlEx.Message}");
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
            AppLogger.LogApplicationError(innerEx);
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

            AppLogger.LogApplicationError(ex);

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

            AppLogger.Log("HandleException_GeneralError_CloseApp executed successfully.");
        }
        catch (Exception innerEx)
        {
            AppLogger.LogApplicationError(innerEx);
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
            ["@User"] = WipAppVariables.User,
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
        await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync, CommandType.Text);
    }

    internal static List<(string MethodName, string ErrorMessage)> GetUniqueErrors()
    {
        return GetUniqueErrorsAsync(false).GetAwaiter().GetResult();
    }

    internal static void LogErrorWithMethod(Exception ex,
        [System.Runtime.CompilerServices.CallerMemberName]
        string methodName = "")
    {
        AppLogger.LogApplicationError(ex);
        AppLogger.Log($"Error in {methodName}: {ex.Message}");
    }
}