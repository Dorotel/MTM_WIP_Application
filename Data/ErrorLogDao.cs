using MTM_WIP_Application.Core;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Data;

internal static class ErrorLogDao
{
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
            {
                var method = reader.GetString("Method");
                var error = reader.GetString("Error");
                uniqueErrors.Add((method, error));
            }

            AppLogger.Log("GetUniqueErrors executed successfully.");
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in GetUniqueErrors: " + ex.Message);
            HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in GetUniqueErrors: " + ex.Message);
            HandleException_GeneralError_CloseApp(ex, useAsync);
        }

        return uniqueErrors;
    }

    internal static async Task<DataTable> GetAllErrorsAsync(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable("SELECT * FROM `wipapp_errorlog` ORDER BY `DateTime` DESC",
                useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task<DataTable> GetErrorsByUserAsync(string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@User"] = user
            };
            return await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `wipapp_errorlog` WHERE `User` = @User ORDER BY `DateTime` DESC",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    internal static async Task DeleteErrorByIdAsync(int id, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Id"] = id
            };
            await SqlHelper.ExecuteNonQuery(
                "DELETE FROM `wipapp_errorlog` WHERE `ID` = @Id",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task DeleteAllErrorsAsync(bool useAsync = false)
    {
        try
        {
            await SqlHelper.ExecuteNonQuery("DELETE FROM `wipapp_errorlog`", useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    internal static async Task<DataTable> GetErrorsByDateRangeAsync(DateTime start, DateTime end,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Start"] = start,
                ["@End"] = end
            };
            return await SqlHelper.ExecuteDataTable(
                "SELECT * FROM `wipapp_errorlog` WHERE `DateTime` BETWEEN @Start AND @End ORDER BY `DateTime` DESC",
                parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
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
            AppLogger.Log($"SQL Error in method: {callerName}, Control: {controlName}");
            AppLogger.Log($"Exception Message: {ex.Message}");
            AppLogger.Log($"Stack Trace: {ex.StackTrace}");

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
                var parameters = new Dictionary<string, object>
                {
                    ["@Method"] = callerName,
                    ["@Error"] = ex.Message,
                    ["@User"] = WipAppVariables.User,
                    ["@DateTime"] = DateTime.Now,
                    ["@Control"] = controlName
                };
                var sql = "INSERT INTO `wipapp_errorlog` (`Method`, `Error`, `User`, `DateTime`, `Control`) " +
                          "VALUES (@Method, @Error, @User, @DateTime, @Control)";
                if (useAsync)
                    await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync: true);
                else
                    SqlHelper.ExecuteNonQuery(sql, parameters).Wait();
            }
        }
        catch (Exception innerEx)
        {
            AppLogger.Log($"Error while handling exception: {innerEx.Message}");
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
                mainForm.Invoke(() =>
                {
                    mainForm.MainForm_StatusStrip_Disconnected.Visible = true;
                    mainForm.MainForm_StatusStrip_SavedStatus.Visible = false;
                    foreach (Control c in mainForm.Controls) c.Enabled = false;
                });
            }

            var parameters = new Dictionary<string, object>
            {
                ["@Method"] = callerName,
                ["@Error"] = ex.Message,
                ["@User"] = WipAppVariables.User,
                ["@DateTime"] = DateTime.Now,
                ["@Control"] = controlName
            };
            var sql = "INSERT INTO `wipapp_errorlog` (`Method`, `Error`, `User`, `DateTime`, `Control`) " +
                      "VALUES (@Method, @Error, @User, @DateTime, @Control)";
            if (useAsync)
                await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync: true);
            else
                SqlHelper.ExecuteNonQuery(sql, parameters).Wait();

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
            await HandleException_GeneralError_CloseApp(innerEx, useAsync, controlName: controlName);
        }
    }

    internal static List<(string Method, string Error)> GetUniqueErrors()
    {
        return GetUniqueErrorsAsync(false).GetAwaiter().GetResult();
    }

    public static void HandleException_SQLError_CloseApp(MySqlException mySqlException, bool useAsync,
        string controlName)
    {
        throw new NotImplementedException();
    }

    internal static void LogErrorWithMethod(Exception ex,
        [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
    {
        AppLogger.Log($"Error in {methodName}: {ex.Message}");
    }
}