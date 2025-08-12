using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Threading.Tasks;
using MTM_Inventory_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Helpers
{
    /// <summary>
    /// Enhanced database helper for stored procedures with comprehensive status reporting
    /// UPDATED: August 10, 2025 - UNIFORM PARAMETER NAMING (WITH p_ prefixes)
    /// UPDATED: January 27, 2025 - Enhanced MySQL connection error handling
    /// </summary>
    public static class Helper_Database_StoredProcedure
    {
        /// <summary>
        /// Execute stored procedure with status output parameters and progress reporting
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <param name="procedureName">Stored procedure name</param>
        /// <param name="parameters">Input parameters (p_ prefixes added automatically)</param>
        /// <param name="progressHelper">Progress helper for visual feedback (optional)</param>
        /// <param name="useAsync">Execute asynchronously</param>
        /// <returns>Result with status and error message</returns>
        public static async Task<StoredProcedureResult<DataTable>> ExecuteDataTableWithStatus(
            string connectionString,
            string procedureName,
            Dictionary<string, object>? parameters = null,
            Helper_StoredProcedureProgress? progressHelper = null,
            bool useAsync = false)
        {
            try
            {
                progressHelper?.UpdateProgress(10, $"Connecting to database for {procedureName}...");

                using var connection = new MySqlConnection(connectionString);
                if (useAsync)
                    await connection.OpenAsync();
                else
                    connection.Open();

                progressHelper?.UpdateProgress(30, $"Executing stored procedure {procedureName}...");

                using var command = new MySqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add input parameters (WITH automatic p_ prefix addition)
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        string paramName = param.Key.StartsWith("p_") ? param.Key : $"p_{param.Key}";
                        command.Parameters.AddWithValue(paramName, param.Value ?? DBNull.Value);
                    }
                }

                // Add standard output parameters (WITH p_ prefix)
                var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
                {
                    Direction = ParameterDirection.Output
                };
                
                command.Parameters.Add(statusParam);
                command.Parameters.Add(errorMsgParam);

                progressHelper?.UpdateProgress(50, $"Retrieving data from {procedureName}...");

                var dataTable = new DataTable();
                using (var adapter = new MySqlDataAdapter(command))
                {
                    if (useAsync)
                    {
                        await Task.Run(() => adapter.Fill(dataTable));
                    }
                    else
                    {
                        adapter.Fill(dataTable);
                    }
                }

                progressHelper?.UpdateProgress(80, $"Processing results from {procedureName}...");

                int status = Convert.ToInt32(statusParam.Value ?? 0);
                string errorMessage = errorMsgParam.Value?.ToString() ?? string.Empty;

                progressHelper?.UpdateProgress(100, $"Completed {procedureName}");

                if (status == 0)
                {
                    progressHelper?.ProcessStoredProcedureResult(status, errorMessage, 
                        $"Successfully retrieved {dataTable.Rows.Count} records");
                    return StoredProcedureResult<DataTable>.Success(dataTable, errorMessage);
                }
                else
                {
                    progressHelper?.ProcessStoredProcedureResult(status, errorMessage);
                    if (status == 1)
                        return StoredProcedureResult<DataTable>.Warning(errorMessage, dataTable);
                    else
                        return StoredProcedureResult<DataTable>.Error(errorMessage, null, dataTable);
                }
            }
            catch (Exception ex)
            {
                string userFriendlyMessage = GetUserFriendlyConnectionError(ex, procedureName);
                LoggingUtility.LogDatabaseError(ex);
                progressHelper?.ShowError(userFriendlyMessage);
                return StoredProcedureResult<DataTable>.Error(userFriendlyMessage, ex);
            }
        }

        /// <summary>
        /// Execute stored procedure and return MySqlDataReader for streaming large result sets
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <param name="procedureName">Stored procedure name</param>
        /// <param name="parameters">Input parameters (p_ prefixes added automatically)</param>
        /// <param name="useAsync">Execute asynchronously</param>
        /// <param name="commandType">Command type (usually StoredProcedure)</param>
        /// <returns>MySqlDataReader for reading results</returns>
        public static async Task<MySqlDataReader> ExecuteReader(
            string connectionString,
            string procedureName,
            Dictionary<string, object>? parameters = null,
            bool useAsync = false,
            CommandType commandType = CommandType.StoredProcedure)
        {
            var connection = new MySqlConnection(connectionString);
            
            try
            {
                if (useAsync)
                    await connection.OpenAsync();
                else
                    connection.Open();

                using var command = new MySqlCommand(procedureName, connection)
                {
                    CommandType = commandType
                };

                // Add input parameters (WITH automatic p_ prefix addition)
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        string paramName = param.Key.StartsWith("p_") ? param.Key : $"p_{param.Key}";
                        command.Parameters.AddWithValue(paramName, param.Value ?? DBNull.Value);
                    }
                }

                if (useAsync)
                    return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                else
                    return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                connection.Dispose();
                
                // Re-throw with user-friendly message for connection errors
                if (IsConnectionRelatedError(ex))
                {
                    string userFriendlyMessage = GetUserFriendlyConnectionError(ex, procedureName);
                    LoggingUtility.LogDatabaseError(ex);
                    throw new InvalidOperationException(userFriendlyMessage, ex);
                }
                
                throw;
            }
        }

        /// <summary>
        /// Execute stored procedure that returns a scalar value with status reporting
        /// </summary>
        public static async Task<StoredProcedureResult<object>> ExecuteScalarWithStatus(
            string connectionString,
            string procedureName,
            Dictionary<string, object>? parameters = null,
            Helper_StoredProcedureProgress? progressHelper = null,
            bool useAsync = false)
        {
            try
            {
                progressHelper?.UpdateProgress(10, $"Connecting to database for {procedureName}...");

                using var connection = new MySqlConnection(connectionString);
                if (useAsync)
                    await connection.OpenAsync();
                else
                    connection.Open();

                progressHelper?.UpdateProgress(30, $"Executing stored procedure {procedureName}...");

                using var command = new MySqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add input parameters (WITH automatic p_ prefix addition)
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        string paramName = param.Key.StartsWith("p_") ? param.Key : $"p_{param.Key}";
                        command.Parameters.AddWithValue(paramName, param.Value ?? DBNull.Value);
                    }
                }

                // Add standard output parameters (WITH p_ prefix)
                var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
                {
                    Direction = ParameterDirection.Output
                };
                
                command.Parameters.Add(statusParam);
                command.Parameters.Add(errorMsgParam);

                progressHelper?.UpdateProgress(50, $"Retrieving data from {procedureName}...");

                object result;
                if (useAsync)
                    result = await command.ExecuteScalarAsync();
                else
                    result = command.ExecuteScalar();

                progressHelper?.UpdateProgress(80, $"Processing results from {procedureName}...");

                int status = Convert.ToInt32(statusParam.Value ?? 0);
                string errorMessage = errorMsgParam.Value?.ToString() ?? string.Empty;

                progressHelper?.UpdateProgress(100, $"Completed {procedureName}");
                progressHelper?.ProcessStoredProcedureResult(status, errorMessage);

                if (status == 0)
                    return StoredProcedureResult<object>.Success(result, errorMessage);
                else if (status == 1)
                    return StoredProcedureResult<object>.Warning(errorMessage, result);
                else
                    return StoredProcedureResult<object>.Error(errorMessage, null, result);
            }
            catch (Exception ex)
            {
                string userFriendlyMessage = GetUserFriendlyConnectionError(ex, procedureName);
                LoggingUtility.LogDatabaseError(ex);
                progressHelper?.ShowError(userFriendlyMessage);
                return StoredProcedureResult<object>.Error(userFriendlyMessage, ex);
            }
        }

        /// <summary>
        /// Execute stored procedure that doesn't return data (INSERT, UPDATE, DELETE) with status reporting
        /// </summary>
        public static async Task<StoredProcedureResult> ExecuteNonQueryWithStatus(
            string connectionString,
            string procedureName,
            Dictionary<string, object>? parameters = null,
            Helper_StoredProcedureProgress? progressHelper = null,
            bool useAsync = false)
        {
            try
            {
                progressHelper?.UpdateProgress(10, $"Connecting to database for {procedureName}...");

                using var connection = new MySqlConnection(connectionString);
                if (useAsync)
                    await connection.OpenAsync();
                else
                    connection.Open();

                progressHelper?.UpdateProgress(30, $"Executing stored procedure {procedureName}...");

                using var command = new MySqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add input parameters (WITH automatic p_ prefix addition)
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        string paramName = param.Key.StartsWith("p_") ? param.Key : $"p_{param.Key}";
                        command.Parameters.AddWithValue(paramName, param.Value ?? DBNull.Value);
                    }
                }

                // Add standard output parameters (WITH p_ prefix)
                var statusParam = new MySqlParameter("p_Status", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                var errorMsgParam = new MySqlParameter("p_ErrorMsg", MySqlDbType.VarChar, 255)
                {
                    Direction = ParameterDirection.Output
                };
                
                command.Parameters.Add(statusParam);
                command.Parameters.Add(errorMsgParam);

                progressHelper?.UpdateProgress(50, $"Executing {procedureName}...");

                int rowsAffected;
                if (useAsync)
                    rowsAffected = await command.ExecuteNonQueryAsync();
                else
                    rowsAffected = command.ExecuteNonQuery();

                progressHelper?.UpdateProgress(80, $"Processing results from {procedureName}...");

                int status = Convert.ToInt32(statusParam.Value ?? 0);
                string errorMessage = errorMsgParam.Value?.ToString() ?? string.Empty;

                progressHelper?.UpdateProgress(100, $"Completed {procedureName}");
                progressHelper?.ProcessStoredProcedureResult(status, errorMessage, 
                    $"Operation completed successfully ({rowsAffected} rows affected)");

                if (status == 0)
                    return StoredProcedureResult.Success(errorMessage);
                else if (status == 1)
                    return StoredProcedureResult.Warning(errorMessage);
                else
                    return StoredProcedureResult.Error(errorMessage);
            }
            catch (Exception ex)
            {
                string userFriendlyMessage = GetUserFriendlyConnectionError(ex, procedureName);
                LoggingUtility.LogDatabaseError(ex);
                progressHelper?.ShowError(userFriendlyMessage);
                return StoredProcedureResult.Error(userFriendlyMessage, ex);
            }
        }

        /// <summary>
        /// Execute stored procedure with custom output parameters
        /// </summary>
        public static async Task<StoredProcedureResult<Dictionary<string, object>>> ExecuteWithCustomOutput(
            string connectionString,
            string procedureName,
            Dictionary<string, object>? inputParameters = null,
            Dictionary<string, MySqlDbType>? outputParameters = null,
            Helper_StoredProcedureProgress? progressHelper = null,
            bool useAsync = false)
        {
            try
            {
                progressHelper?.UpdateProgress(10, $"Connecting to database for {procedureName}...");

                using var connection = new MySqlConnection(connectionString);
                if (useAsync)
                    await connection.OpenAsync();
                else
                    connection.Open();

                progressHelper?.UpdateProgress(30, $"Executing stored procedure {procedureName}...");

                using var command = new MySqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add input parameters (WITH automatic p_ prefix addition)
                if (inputParameters != null)
                {
                    foreach (var param in inputParameters)
                    {
                        string paramName = param.Key.StartsWith("p_") ? param.Key : $"p_{param.Key}";
                        command.Parameters.AddWithValue(paramName, param.Value ?? DBNull.Value);
                    }
                }

                // Add output parameters (WITH automatic p_ prefix addition)
                var outputValues = new Dictionary<string, object>();
                if (outputParameters != null)
                {
                    foreach (var param in outputParameters)
                    {
                        string paramName = param.Key.StartsWith("p_") ? param.Key : $"p_{param.Key}";
                        var outputParam = new MySqlParameter(paramName, param.Value)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);
                    }
                }

                progressHelper?.UpdateProgress(50, $"Executing {procedureName}...");

                if (useAsync)
                    await command.ExecuteNonQueryAsync();
                else
                    command.ExecuteNonQuery();

                progressHelper?.UpdateProgress(80, $"Processing results from {procedureName}...");

                // Collect output parameter values
                if (outputParameters != null)
                {
                    foreach (var param in outputParameters)
                    {
                        string paramName = param.Key.StartsWith("p_") ? param.Key : $"p_{param.Key}";
                        outputValues[param.Key] = command.Parameters[paramName].Value ?? DBNull.Value;
                    }
                }

                progressHelper?.UpdateProgress(100, $"Completed {procedureName}");

                // Check for standard status parameters
                if (outputValues.ContainsKey("Status") && outputValues.ContainsKey("ErrorMsg"))
                {
                    int status = Convert.ToInt32(outputValues["Status"]);
                    string errorMessage = outputValues["ErrorMsg"]?.ToString() ?? string.Empty;
                    
                    progressHelper?.ProcessStoredProcedureResult(status, errorMessage);

                    if (status == 0)
                        return StoredProcedureResult<Dictionary<string, object>>.Success(outputValues, errorMessage);
                    else if (status == 1)
                        return StoredProcedureResult<Dictionary<string, object>>.Warning(errorMessage, outputValues);
                    else
                        return StoredProcedureResult<Dictionary<string, object>>.Error(errorMessage, null, outputValues);
                }
                else
                {
                    progressHelper?.ShowSuccess("Operation completed successfully");
                    return StoredProcedureResult<Dictionary<string, object>>.Success(outputValues);
                }
            }
            catch (Exception ex)
            {
                string userFriendlyMessage = GetUserFriendlyConnectionError(ex, procedureName);
                LoggingUtility.LogDatabaseError(ex);
                progressHelper?.ShowError(userFriendlyMessage);
                return StoredProcedureResult<Dictionary<string, object>>.Error(userFriendlyMessage, ex);
            }
        }

        #region Connection Error Handling

        /// <summary>
        /// Convert technical MySQL connection errors into user-friendly messages
        /// </summary>
        /// <param name="exception">The original exception</param>
        /// <param name="procedureName">Name of the procedure being executed</param>
        /// <returns>User-friendly error message</returns>
        private static string GetUserFriendlyConnectionError(Exception exception, string procedureName)
        {
            if (IsConnectionRelatedError(exception))
            {
                return GetConnectionErrorMessage(exception, procedureName);
            }
            
            // For non-connection errors, return a generic message with the procedure name
            return $"An error occurred while executing {procedureName}: {exception.Message}";
        }

        /// <summary>
        /// Determine if an exception is connection-related
        /// </summary>
        /// <param name="exception">Exception to check</param>
        /// <returns>True if connection-related, false otherwise</returns>
        private static bool IsConnectionRelatedError(Exception exception)
        {
            // Check the exception chain for connection-related errors
            Exception? currentEx = exception;
            while (currentEx != null)
            {
                if (currentEx is MySqlException mysqlEx)
                {
                    // MySQL connection error codes
                    return mysqlEx.Message.Contains("Unable to connect") ||
                           mysqlEx.Message.Contains("Connection refused") ||
                           mysqlEx.Message.Contains("Connection timeout") ||
                           mysqlEx.Message.Contains("Host not found") ||
                           mysqlEx.Message.Contains("Access denied");
                }

                if (currentEx is SocketException)
                {
                    return true;
                }

                if (currentEx is TimeoutException)
                {
                    return true;
                }

                currentEx = currentEx.InnerException;
            }

            return false;
        }

        /// <summary>
        /// Generate specific user-friendly message for connection errors
        /// </summary>
        /// <param name="exception">Connection-related exception</param>
        /// <param name="procedureName">Name of the procedure being executed</param>
        /// <returns>User-friendly connection error message</returns>
        private static string GetConnectionErrorMessage(Exception exception, string procedureName)
        {
            Exception? currentEx = exception;
            while (currentEx != null)
            {
                if (currentEx is MySqlException mysqlEx)
                {
                    if (mysqlEx.Message.Contains("Unable to connect to any of the specified MySQL hosts"))
                    {
                        return $"Cannot connect to the database server to execute {procedureName}.\n\n" +
                               "This usually means:\n" +
                               "• The database server is not running\n" +
                               "• The server address or port is incorrect\n" +
                               "• A firewall is blocking the connection\n\n" +
                               "Please check with your system administrator or verify the server is running.";
                    }

                    if (mysqlEx.Message.Contains("Access denied"))
                    {
                        return $"Access denied when connecting to the database for {procedureName}.\n\n" +
                               "This usually means:\n" +
                               "• Your username or password is incorrect\n" +
                               "• Your account doesn't have permission to access the database\n\n" +
                               "Please check your credentials with your system administrator.";
                    }

                    if (mysqlEx.Message.Contains("Connection timeout") || mysqlEx.Message.Contains("timeout"))
                    {
                        return $"Connection to the database timed out while executing {procedureName}.\n\n" +
                               "This usually means:\n" +
                               "• The database server is responding slowly\n" +
                               "• Network connectivity issues\n" +
                               "• The server is overloaded\n\n" +
                               "Please try again in a few moments.";
                    }
                }

                if (currentEx is SocketException socketEx)
                {
                    if (socketEx.Message.Contains("actively refused"))
                    {
                        return $"The database server refused the connection for {procedureName}.\n\n" +
                               "This usually means:\n" +
                               "• The MySQL service is not running on the server\n" +
                               "• The server is not accepting connections on the configured port\n" +
                               "• The server address is incorrect\n\n" +
                               "Please verify the MySQL service is running and check the server configuration.";
                    }

                    if (socketEx.Message.Contains("host not found") || socketEx.Message.Contains("Name or service not known"))
                    {
                        return $"Cannot find the database server to execute {procedureName}.\n\n" +
                               "This usually means:\n" +
                               "• The server name or IP address is incorrect\n" +
                               "• DNS resolution is not working\n" +
                               "• The server is not accessible from this network\n\n" +
                               "Please check the server address in your connection settings.";
                    }
                }

                if (currentEx is TimeoutException)
                {
                    return $"Connection to the database timed out while executing {procedureName}.\n\n" +
                           "Please try again in a few moments. If the problem persists, " +
                           "contact your system administrator.";
                }

                currentEx = currentEx.InnerException;
            }

            // Fallback message
            return $"Unable to connect to the database server to execute {procedureName}.\n\n" +
                   "Please check your network connection and verify the database server is running. " +
                   "If the problem persists, contact your system administrator.\n\n" +
                   $"Technical details: {exception.Message}";
        }

        #endregion
    }
}
