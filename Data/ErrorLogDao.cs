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

    internal static async Task<List<(string Method, string Error)>> GetUniqueErrorsAsync(bool useAsync = false)
    {
        var uniqueErrors = new List<(string Method, string Error)>();
        try
        {
            using var reader = useAsync
                ? await SqlHelper.ExecuteReader("SELECT DISTINCT `Method`, `Error` FROM `wipapp_errorlog`",
                    useAsync: true)
                : SqlHelper.ExecuteReader("SELECT DISTINCT `Method`, `Error` FROM `wipapp_errorlog`").Result;

            while (reader.Read())
                uniqueErrors.Add((reader.GetString("Method"), reader.GetString("Error")));

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
        return await GetErrorsByQueryAsync("SELECT * FROM `wipapp_errorlog` ORDER BY `DateTime` DESC", null, useAsync);
    }

    internal static async Task<DataTable> GetErrorsByUserAsync(string user, bool useAsync = false)
    {
        return await GetErrorsByQueryAsync(
            "SELECT * FROM `wipapp_errorlog` WHERE `User` = @User ORDER BY `DateTime` DESC",
            new Dictionary<string, object> { ["@User"] = user }, useAsync);
    }

    internal static async Task<DataTable> GetErrorsByDateRangeAsync(DateTime start, DateTime end, bool useAsync = false)
    {
        return await GetErrorsByQueryAsync(
            "SELECT * FROM `wipapp_errorlog` WHERE `DateTime` BETWEEN @Start AND @End ORDER BY `DateTime` DESC",
            new Dictionary<string, object> { ["@Start"] = start, ["@End"] = end }, useAsync);
    }

    private static async Task<DataTable> GetErrorsByQueryAsync(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            return parameters == null
                ? await SqlHelper.ExecuteDataTable(sql, useAsync: useAsync)
                : await SqlHelper.ExecuteDataTable(sql, parameters, useAsync: useAsync);
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
        await ExecuteNonQueryAsync("DELETE FROM `wipapp_errorlog` WHERE `ID` = @Id",
            new Dictionary<string, object> { ["@Id"] = id }, useAsync);
    }

    internal static async Task DeleteAllErrorsAsync(bool useAsync = false)
    {
        await ExecuteNonQueryAsync("DELETE FROM `wipapp_errorlog`", null, useAsync);
    }

    private static async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object>? parameters, bool useAsync)
    {
        try
        {
            if (parameters == null)
                await SqlHelper.ExecuteNonQuery(sql, useAsync: useAsync);
            else
                await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync: useAsync);
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

            if (isConnectionError)
            {
                MessageBox.Show(
                    @"Database connection error. The application will now close.",
                    @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                await LogErrorToDatabaseAsync(ex.Message, callerName, controlName, useAsync);
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

            if (Application.OpenForms.OfType<MainForm>().Any())
            {
                var mainForm = Application.OpenForms.OfType<MainForm>().First();
                mainForm.BeginInvoke(new Action(() =>
                {
                    foreach (Control c in mainForm.Controls) c.Enabled = false;
                }));
            }

            AppLogger.LogApplicationError(ex);

            await LogErrorToDatabaseAsync(ex.Message, callerName, controlName, useAsync);

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

            AppLogger.Log("HandleException_GeneralError_CloseApp executed successfully.");
        }
        catch (Exception innerEx)
        {
            AppLogger.LogApplicationError(innerEx);
            await HandleException_GeneralError_CloseApp(innerEx, useAsync, controlName: controlName);
        }
    }

    private static async Task LogErrorToDatabaseAsync(string error, string method, string control, bool useAsync)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@Method"] = method,
            ["@Error"] = error,
            ["@User"] = WipAppVariables.User,
            ["@DateTime"] = DateTime.Now,
            ["@Control"] = control
        };
        var sql = "INSERT INTO `wipapp_errorlog` (`Method`, `Error`, `User`, `DateTime`, `Control`) " +
                  "VALUES (@Method, @Error, @User, @DateTime, @Control)";
        await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync: useAsync);
    }

    internal static List<(string Method, string Error)> GetUniqueErrors()
    {
        return GetUniqueErrorsAsync(false).GetAwaiter().GetResult();
    }

    internal static void LogErrorWithMethod(Exception ex,
        [System.Runtime.CompilerServices.CallerMemberName]
        string methodName = "")
    {
        // Use application error log for general exceptions
        AppLogger.LogApplicationError(ex);
        AppLogger.Log($"Error in {methodName}: {ex.Message}");
    }
}