using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Services;

/// <summary>
/// Comprehensive debugging and tracing service for MTM Inventory Application
/// Provides detailed logging of actions, variables, data flow, and business logic execution
/// </summary>
internal static class Service_DebugTracer
{
    #region Fields

    private static readonly Dictionary<string, Stopwatch> _methodTimers = new();
    private static readonly Dictionary<string, int> _callDepth = new();
    private static readonly object _traceLock = new();
    private static bool _isInitialized = false;
    
    // Configuration
    private static DebugLevel _currentLevel = DebugLevel.Medium;
    private static bool _traceDatabase = true;
    private static bool _traceBusinessLogic = true;
    private static bool _traceUIActions = true;
    private static bool _tracePerformance = true;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the current debug tracing level
    /// </summary>
    public static DebugLevel CurrentLevel
    {
        get => _currentLevel;
        set => _currentLevel = value;
    }

    /// <summary>
    /// Enable/disable database operation tracing
    /// </summary>
    public static bool TraceDatabase
    {
        get => _traceDatabase;
        set => _traceDatabase = value;
    }

    /// <summary>
    /// Enable/disable business logic tracing
    /// </summary>
    public static bool TraceBusinessLogic
    {
        get => _traceBusinessLogic;
        set => _traceBusinessLogic = value;
    }

    /// <summary>
    /// Enable/disable UI action tracing
    /// </summary>
    public static bool TraceUIActions
    {
        get => _traceUIActions;
        set => _traceUIActions = value;
    }

    /// <summary>
    /// Enable/disable performance timing tracing
    /// </summary>
    public static bool TracePerformance
    {
        get => _tracePerformance;
        set => _tracePerformance = value;
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Initialize the debug tracing system
    /// </summary>
    public static void Initialize(DebugLevel level = DebugLevel.Medium)
    {
        if (_isInitialized) return;

        _currentLevel = level;
        _isInitialized = true;

        LogTrace("🚀 DEBUG TRACER INITIALIZED", DebugLevel.Low, new Dictionary<string, object>
        {
            ["Level"] = level.ToString(),
            ["TraceDatabase"] = _traceDatabase,
            ["TraceBusinessLogic"] = _traceBusinessLogic,
            ["TraceUIActions"] = _traceUIActions,
            ["TracePerformance"] = _tracePerformance,
            ["Timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
        });
    }

    #endregion

    #region Method Tracing

    /// <summary>
    /// Trace method entry with parameters
    /// </summary>
    /// <param name="parameters">Method parameters to log</param>
    /// <param name="callerName">Method name (auto-filled)</param>
    /// <param name="controlName">Control or form name</param>
    /// <param name="level">Debug level for this trace</param>
    public static void TraceMethodEntry(Dictionary<string, object>? parameters = null,
        [CallerMemberName] string callerName = "",
        string controlName = "",
        DebugLevel level = DebugLevel.Medium)
    {
        if (!ShouldTrace(level)) return;

        lock (_traceLock)
        {
            // Track call depth for indentation
            var key = $"{controlName}:{callerName}";
            _callDepth[key] = _callDepth.GetValueOrDefault(key, 0) + 1;

            // Start performance timer
            if (_tracePerformance)
            {
                var timerKey = $"{key}_{DateTime.Now.Ticks}";
                _methodTimers[timerKey] = Stopwatch.StartNew();
            }

            var indent = new string("  ", _callDepth[key] - 1);
            var logData = new Dictionary<string, object>
            {
                ["Action"] = "METHOD_ENTRY",
                ["Method"] = callerName,
                ["Control"] = controlName,
                ["CallDepth"] = _callDepth[key],
                ["Thread"] = Thread.CurrentThread.ManagedThreadId
            };

            if (parameters?.Any() == true)
            {
                logData["Parameters"] = SerializeParameters(parameters);
            }

            LogTrace($"{indent}➡️ ENTERING {controlName}.{callerName}", level, logData);
        }
    }

    /// <summary>
    /// Trace method exit with return value
    /// </summary>
    /// <param name="returnValue">Method return value</param>
    /// <param name="callerName">Method name (auto-filled)</param>
    /// <param name="controlName">Control or form name</param>
    /// <param name="level">Debug level for this trace</param>
    public static void TraceMethodExit(object? returnValue = null,
        [CallerMemberName] string callerName = "",
        string controlName = "",
        DebugLevel level = DebugLevel.Medium)
    {
        if (!ShouldTrace(level)) return;

        lock (_traceLock)
        {
            var key = $"{controlName}:{callerName}";
            var depth = _callDepth.GetValueOrDefault(key, 1);
            var indent = new string("  ", depth - 1);

            // Stop performance timer and get elapsed time
            string elapsedTime = "";
            if (_tracePerformance)
            {
                var timerKey = _methodTimers.Keys.FirstOrDefault(k => k.StartsWith($"{key}_"));
                if (timerKey != null && _methodTimers.TryGetValue(timerKey, out var timer))
                {
                    timer.Stop();
                    elapsedTime = $" ({timer.ElapsedMilliseconds}ms)";
                    _methodTimers.Remove(timerKey);
                }
            }

            var logData = new Dictionary<string, object>
            {
                ["Action"] = "METHOD_EXIT",
                ["Method"] = callerName,
                ["Control"] = controlName,
                ["CallDepth"] = depth,
                ["Thread"] = Thread.CurrentThread.ManagedThreadId
            };

            if (returnValue != null)
            {
                logData["ReturnValue"] = SerializeValue(returnValue);
            }

            if (!string.IsNullOrEmpty(elapsedTime))
            {
                logData["ElapsedTime"] = elapsedTime.Trim(' ', '(', ')');
            }

            LogTrace($"{indent}⬅️ EXITING {controlName}.{callerName}{elapsedTime}", level, logData);

            // Decrease call depth
            if (_callDepth[key] > 1)
                _callDepth[key]--;
            else
                _callDepth.Remove(key);
        }
    }

    #endregion

    #region Database Tracing

    /// <summary>
    /// Trace database operation start
    /// </summary>
    /// <param name="operation">Database operation (SELECT, INSERT, UPDATE, DELETE, PROCEDURE)</param>
    /// <param name="target">Table name or stored procedure name</param>
    /// <param name="parameters">SQL parameters</param>
    /// <param name="connectionString">Connection string (sanitized for logging)</param>
    /// <param name="callerName">Method name (auto-filled)</param>
    public static void TraceDatabaseStart(string operation, string target, 
        Dictionary<string, object>? parameters = null,
        string connectionString = "",
        [CallerMemberName] string callerName = "")
    {
        if (!ShouldTrace(DebugLevel.Medium) || !_traceDatabase) return;

        var logData = new Dictionary<string, object>
        {
            ["Action"] = "DATABASE_START",
            ["Operation"] = operation,
            ["Target"] = target,
            ["Caller"] = callerName,
            ["Server"] = ExtractServerFromConnectionString(connectionString),
            ["Database"] = ExtractDatabaseFromConnectionString(connectionString),
            ["Thread"] = Thread.CurrentThread.ManagedThreadId
        };

        if (parameters?.Any() == true)
        {
            logData["Parameters"] = SerializeParameters(parameters);
        }

        LogTrace($"🗄️ DB {operation} START: {target}", DebugLevel.Medium, logData);
    }

    /// <summary>
    /// Trace database operation completion
    /// </summary>
    /// <param name="operation">Database operation</param>
    /// <param name="target">Table name or stored procedure name</param>
    /// <param name="result">Operation result</param>
    /// <param name="rowsAffected">Number of rows affected</param>
    /// <param name="elapsedMs">Elapsed time in milliseconds</param>
    /// <param name="callerName">Method name (auto-filled)</param>
    public static void TraceDatabaseComplete(string operation, string target,
        object? result = null,
        int rowsAffected = 0,
        long elapsedMs = 0,
        [CallerMemberName] string callerName = "")
    {
        if (!ShouldTrace(DebugLevel.Medium) || !_traceDatabase) return;

        var logData = new Dictionary<string, object>
        {
            ["Action"] = "DATABASE_COMPLETE",
            ["Operation"] = operation,
            ["Target"] = target,
            ["Caller"] = callerName,
            ["RowsAffected"] = rowsAffected,
            ["ElapsedMs"] = elapsedMs,
            ["Thread"] = Thread.CurrentThread.ManagedThreadId
        };

        if (result != null)
        {
            logData["Result"] = SerializeValue(result);
        }

        var performance = elapsedMs > 0 ? $" ({elapsedMs}ms)" : "";
        LogTrace($"✅ DB {operation} COMPLETE: {target}{performance} - {rowsAffected} rows", DebugLevel.Medium, logData);
    }

    /// <summary>
    /// Trace stored procedure execution with full details
    /// </summary>
    /// <param name="procedureName">Stored procedure name</param>
    /// <param name="inputParameters">Input parameters</param>
    /// <param name="outputParameters">Output parameters</param>
    /// <param name="resultData">Result data (DataTable, scalar, etc.)</param>
    /// <param name="status">Procedure status code</param>
    /// <param name="errorMessage">Error message if any</param>
    /// <param name="elapsedMs">Elapsed time</param>
    /// <param name="callerName">Calling method</param>
    public static void TraceStoredProcedure(string procedureName,
        Dictionary<string, object>? inputParameters = null,
        Dictionary<string, object>? outputParameters = null,
        object? resultData = null,
        int status = 0,
        string errorMessage = "",
        long elapsedMs = 0,
        [CallerMemberName] string callerName = "")
    {
        if (!ShouldTrace(DebugLevel.High) || !_traceDatabase) return;

        var logData = new Dictionary<string, object>
        {
            ["Action"] = "STORED_PROCEDURE_EXECUTION",
            ["Procedure"] = procedureName,
            ["Caller"] = callerName,
            ["Status"] = status,
            ["ElapsedMs"] = elapsedMs,
            ["Thread"] = Thread.CurrentThread.ManagedThreadId
        };

        if (inputParameters?.Any() == true)
        {
            logData["InputParameters"] = SerializeParameters(inputParameters);
        }

        if (outputParameters?.Any() == true)
        {
            logData["OutputParameters"] = SerializeParameters(outputParameters);
        }

        if (resultData != null)
        {
            logData["ResultData"] = SerializeValue(resultData);
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            logData["ErrorMessage"] = errorMessage;
        }

        var statusIcon = status == 0 ? "✅" : "❌";
        var performance = elapsedMs > 0 ? $" ({elapsedMs}ms)" : "";
        LogTrace($"{statusIcon} PROCEDURE {procedureName}{performance} - Status: {status}", DebugLevel.High, logData);
    }

    #endregion

    #region Business Logic Tracing

    /// <summary>
    /// Trace business logic execution
    /// </summary>
    /// <param name="logicName">Name of the business logic operation</param>
    /// <param name="inputData">Input data being processed</param>
    /// <param name="outputData">Output data produced</param>
    /// <param name="businessRules">Business rules applied</param>
    /// <param name="validationResults">Validation results</param>
    /// <param name="callerName">Calling method</param>
    public static void TraceBusinessLogic(string logicName,
        object? inputData = null,
        object? outputData = null,
        Dictionary<string, object>? businessRules = null,
        Dictionary<string, object>? validationResults = null,
        [CallerMemberName] string callerName = "")
    {
        if (!ShouldTrace(DebugLevel.Medium) || !_traceBusinessLogic) return;

        var logData = new Dictionary<string, object>
        {
            ["Action"] = "BUSINESS_LOGIC",
            ["Logic"] = logicName,
            ["Caller"] = callerName,
            ["Thread"] = Thread.CurrentThread.ManagedThreadId
        };

        if (inputData != null)
        {
            logData["InputData"] = SerializeValue(inputData);
        }

        if (outputData != null)
        {
            logData["OutputData"] = SerializeValue(outputData);
        }

        if (businessRules?.Any() == true)
        {
            logData["BusinessRules"] = SerializeParameters(businessRules);
        }

        if (validationResults?.Any() == true)
        {
            logData["ValidationResults"] = SerializeParameters(validationResults);
        }

        LogTrace($"📊 BUSINESS LOGIC: {logicName}", DebugLevel.Medium, logData);
    }

    /// <summary>
    /// Trace data validation
    /// </summary>
    /// <param name="validationType">Type of validation</param>
    /// <param name="dataToValidate">Data being validated</param>
    /// <param name="validationRules">Validation rules applied</param>
    /// <param name="isValid">Whether validation passed</param>
    /// <param name="errorMessages">Validation error messages</param>
    /// <param name="callerName">Calling method</param>
    public static void TraceDataValidation(string validationType,
        object? dataToValidate = null,
        Dictionary<string, object>? validationRules = null,
        bool isValid = true,
        List<string>? errorMessages = null,
        [CallerMemberName] string callerName = "")
    {
        if (!ShouldTrace(DebugLevel.High) || !_traceBusinessLogic) return;

        var logData = new Dictionary<string, object>
        {
            ["Action"] = "DATA_VALIDATION",
            ["ValidationType"] = validationType,
            ["Caller"] = callerName,
            ["IsValid"] = isValid,
            ["Thread"] = Thread.CurrentThread.ManagedThreadId
        };

        if (dataToValidate != null)
        {
            logData["DataToValidate"] = SerializeValue(dataToValidate);
        }

        if (validationRules?.Any() == true)
        {
            logData["ValidationRules"] = SerializeParameters(validationRules);
        }

        if (errorMessages?.Any() == true)
        {
            logData["ErrorMessages"] = string.Join("; ", errorMessages);
        }

        var icon = isValid ? "✅" : "❌";
        LogTrace($"{icon} VALIDATION {validationType}: {(isValid ? "PASSED" : "FAILED")}", DebugLevel.High, logData);
    }

    #endregion

    #region UI Action Tracing

    /// <summary>
    /// Trace UI actions like button clicks, form loads, etc.
    /// </summary>
    /// <param name="actionType">Type of UI action</param>
    /// <param name="controlName">Name of the control</param>
    /// <param name="actionData">Data related to the action</param>
    /// <param name="userInput">User input if any</param>
    /// <param name="callerName">Calling method</param>
    public static void TraceUIAction(string actionType, string controlName = "",
        Dictionary<string, object>? actionData = null,
        object? userInput = null,
        [CallerMemberName] string callerName = "")
    {
        if (!ShouldTrace(DebugLevel.Low) || !_traceUIActions) return;

        var logData = new Dictionary<string, object>
        {
            ["Action"] = "UI_ACTION",
            ["ActionType"] = actionType,
            ["Control"] = controlName,
            ["Caller"] = callerName,
            ["Thread"] = Thread.CurrentThread.ManagedThreadId
        };

        if (actionData?.Any() == true)
        {
            logData["ActionData"] = SerializeParameters(actionData);
        }

        if (userInput != null)
        {
            logData["UserInput"] = SerializeValue(userInput);
        }

        LogTrace($"🖱️ UI ACTION: {actionType} on {controlName}", DebugLevel.Low, logData);
    }

    #endregion

    #region Performance Tracing

    /// <summary>
    /// Start performance measurement
    /// </summary>
    /// <param name="operationName">Name of the operation to measure</param>
    /// <param name="callerName">Calling method</param>
    /// <returns>Performance measurement key for stopping</returns>
    public static string StartPerformanceTrace(string operationName, [CallerMemberName] string callerName = "")
    {
        if (!_tracePerformance) return "";

        var key = $"{callerName}:{operationName}:{DateTime.Now.Ticks}";
        lock (_traceLock)
        {
            _methodTimers[key] = Stopwatch.StartNew();
        }

        if (ShouldTrace(DebugLevel.High))
        {
            LogTrace($"⏱️ PERFORMANCE START: {operationName}", DebugLevel.High, new Dictionary<string, object>
            {
                ["Action"] = "PERFORMANCE_START",
                ["Operation"] = operationName,
                ["Caller"] = callerName,
                ["Key"] = key
            });
        }

        return key;
    }

    /// <summary>
    /// Stop performance measurement
    /// </summary>
    /// <param name="performanceKey">Key returned from StartPerformanceTrace</param>
    /// <param name="additionalData">Additional data to log</param>
    public static long StopPerformanceTrace(string performanceKey, Dictionary<string, object>? additionalData = null)
    {
        if (!_tracePerformance || string.IsNullOrEmpty(performanceKey)) return 0;

        long elapsedMs = 0;
        lock (_traceLock)
        {
            if (_methodTimers.TryGetValue(performanceKey, out var timer))
            {
                timer.Stop();
                elapsedMs = timer.ElapsedMilliseconds;
                _methodTimers.Remove(performanceKey);
            }
        }

        if (ShouldTrace(DebugLevel.High))
        {
            var parts = performanceKey.Split(':');
            var operation = parts.Length > 1 ? parts[1] : "Unknown";
            
            var logData = new Dictionary<string, object>
            {
                ["Action"] = "PERFORMANCE_COMPLETE",
                ["Operation"] = operation,
                ["ElapsedMs"] = elapsedMs,
                ["Key"] = performanceKey
            };

            if (additionalData?.Any() == true)
            {
                foreach (var kvp in additionalData)
                {
                    logData[kvp.Key] = kvp.Value;
                }
            }

            LogTrace($"⏱️ PERFORMANCE COMPLETE: {operation} ({elapsedMs}ms)", DebugLevel.High, logData);
        }

        return elapsedMs;
    }

    #endregion

    #region Helper Methods

    private static bool ShouldTrace(DebugLevel level)
    {
        return _isInitialized && level <= _currentLevel;
    }

    private static void LogTrace(string message, DebugLevel level, Dictionary<string, object>? data = null)
    {
        try
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            var levelStr = level.ToString().ToUpper().PadRight(6);
            var formattedMessage = $"[{timestamp}] [{levelStr}] {message}";

            // Always write to debug output
            Debug.WriteLine(formattedMessage);

            // Write to logging system
            LoggingUtility.Log(formattedMessage);

            // If we have structured data, log it as JSON for detailed analysis
            if (data?.Any() == true && level >= DebugLevel.High)
            {
                try
                {
                    var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions 
                    { 
                        WriteIndented = true,
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    });
                    LoggingUtility.Log($"[{timestamp}] [DATA  ] {jsonData}");
                }
                catch (Exception jsonEx)
                {
                    Debug.WriteLine($"Failed to serialize trace data: {jsonEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to write debug trace: {ex.Message}");
        }
    }

    private static object SerializeParameters(Dictionary<string, object> parameters)
    {
        var result = new Dictionary<string, object>();
        foreach (var kvp in parameters)
        {
            result[kvp.Key] = SerializeValue(kvp.Value);
        }
        return result;
    }

    private static object SerializeValue(object? value)
    {
        if (value == null) return "NULL";
        if (value is string str) return $"\"{str}\"";
        if (value is DateTime dt) return dt.ToString("yyyy-MM-dd HH:mm:ss");
        if (value is System.Data.DataTable dt2) return $"DataTable[{dt2.Rows.Count} rows, {dt2.Columns.Count} columns]";
        if (value is Exception ex) return $"Exception: {ex.Message}";
        
        try
        {
            return JsonSerializer.Serialize(value);
        }
        catch
        {
            return value.ToString() ?? "NULL";
        }
    }

    private static string ExtractServerFromConnectionString(string connectionString)
    {
        try
        {
            if (string.IsNullOrEmpty(connectionString)) return "Unknown";
            var parts = connectionString.Split(';');
            var serverPart = parts.FirstOrDefault(p => p.Trim().StartsWith("Server=", StringComparison.OrdinalIgnoreCase));
            return serverPart?.Split('=')[1]?.Trim() ?? "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }

    private static string ExtractDatabaseFromConnectionString(string connectionString)
    {
        try
        {
            if (string.IsNullOrEmpty(connectionString)) return "Unknown";
            var parts = connectionString.Split(';');
            var dbPart = parts.FirstOrDefault(p => p.Trim().StartsWith("Database=", StringComparison.OrdinalIgnoreCase));
            return dbPart?.Split('=')[1]?.Trim() ?? "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }

    #endregion
}