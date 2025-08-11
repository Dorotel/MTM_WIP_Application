using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data
{
    /// <summary>
    /// Enhanced database helper for stored procedures with comprehensive status reporting
    /// UPDATED: August 10, 2025 - UNIFORM PARAMETER NAMING (WITH p_ prefixes)
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
                string errorMsg = $"Exception executing {procedureName}: {ex.Message}";
                LoggingUtility.LogDatabaseError(ex);
                progressHelper?.ShowError(errorMsg);
                return StoredProcedureResult<DataTable>.Error(errorMsg, ex);
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
                string errorMsg = $"Exception executing {procedureName}: {ex.Message}";
                LoggingUtility.LogDatabaseError(ex);
                progressHelper?.ShowError(errorMsg);
                return StoredProcedureResult<object>.Error(errorMsg, ex);
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
                string errorMsg = $"Exception executing {procedureName}: {ex.Message}";
                LoggingUtility.LogDatabaseError(ex);
                progressHelper?.ShowError(errorMsg);
                return StoredProcedureResult.Error(errorMsg, ex);
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
                string errorMsg = $"Exception executing {procedureName}: {ex.Message}";
                LoggingUtility.LogDatabaseError(ex);
                progressHelper?.ShowError(errorMsg);
                return StoredProcedureResult<Dictionary<string, object>>.Error(errorMsg, ex);
            }
        }
    }
}
