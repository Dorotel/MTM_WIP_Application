using System;
using System.Collections.Generic;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Services;

/// <summary>
/// Debug configuration management for comprehensive application tracing
/// Provides centralized control over what debugging information is captured
/// </summary>
internal static class Service_DebugConfiguration
{
    #region Fields

    private static readonly Dictionary<string, bool> _componentTracing = new();
    private static readonly Dictionary<string, DebugLevel> _componentLevels = new();
    
    #endregion

    #region Initialization

    /// <summary>
    /// Initialize debug configuration with default settings
    /// </summary>
    public static void InitializeDefaults()
    {
        // Default component tracing settings
        _componentTracing["Database"] = true;
        _componentTracing["BusinessLogic"] = true;
        _componentTracing["UIActions"] = true;
        _componentTracing["Performance"] = true;
        _componentTracing["DataValidation"] = true;
        _componentTracing["ErrorHandling"] = true;
        _componentTracing["FileOperations"] = false;
        _componentTracing["NetworkOperations"] = false;

        // Default component debug levels
        _componentLevels["MainForm"] = DebugLevel.Medium;
        _componentLevels["InventoryTab"] = DebugLevel.High;
        _componentLevels["TransactionTab"] = DebugLevel.High;
        _componentLevels["SettingsForm"] = DebugLevel.Medium;
        _componentLevels["Database"] = DebugLevel.High;
        _componentLevels["ErrorHandling"] = DebugLevel.High;
        _componentLevels["BusinessLogic"] = DebugLevel.Medium;

        Service_DebugTracer.TraceUIAction("DEBUG_CONFIGURATION_INITIALIZED", "Service_DebugConfiguration", 
            new Dictionary<string, object>
            {
                ["ComponentCount"] = _componentTracing.Count,
                ["DatabaseTracing"] = _componentTracing["Database"],
                ["BusinessLogicTracing"] = _componentTracing["BusinessLogic"],
                ["PerformanceTracing"] = _componentTracing["Performance"]
            });
    }

    #endregion

    #region Component Control

    /// <summary>
    /// Check if tracing is enabled for a specific component
    /// </summary>
    /// <param name="componentName">Name of the component</param>
    /// <returns>True if tracing is enabled</returns>
    public static bool IsComponentTracingEnabled(string componentName)
    {
        return _componentTracing.GetValueOrDefault(componentName, false);
    }

    /// <summary>
    /// Get the debug level for a specific component
    /// </summary>
    /// <param name="componentName">Name of the component</param>
    /// <returns>Debug level for the component</returns>
    public static DebugLevel GetComponentLevel(string componentName)
    {
        return _componentLevels.GetValueOrDefault(componentName, DebugLevel.Medium);
    }

    /// <summary>
    /// Enable or disable tracing for a specific component
    /// </summary>
    /// <param name="componentName">Name of the component</param>
    /// <param name="enabled">Whether to enable tracing</param>
    public static void SetComponentTracing(string componentName, bool enabled)
    {
        _componentTracing[componentName] = enabled;
        
        Service_DebugTracer.TraceUIAction("COMPONENT_TRACING_CHANGED", "Service_DebugConfiguration",
            new Dictionary<string, object>
            {
                ["Component"] = componentName,
                ["Enabled"] = enabled
            });
    }

    /// <summary>
    /// Set the debug level for a specific component
    /// </summary>
    /// <param name="componentName">Name of the component</param>
    /// <param name="level">Debug level to set</param>
    public static void SetComponentLevel(string componentName, DebugLevel level)
    {
        _componentLevels[componentName] = level;
        
        Service_DebugTracer.TraceUIAction("COMPONENT_LEVEL_CHANGED", "Service_DebugConfiguration",
            new Dictionary<string, object>
            {
                ["Component"] = componentName,
                ["Level"] = level.ToString()
            });
    }

    #endregion

    #region Predefined Configurations

    /// <summary>
    /// Set debug configuration for development mode (verbose tracing)
    /// </summary>
    public static void SetDevelopmentMode()
    {
        Service_DebugTracer.CurrentLevel = DebugLevel.Verbose;
        Service_DebugTracer.TraceDatabase = true;
        Service_DebugTracer.TraceBusinessLogic = true;
        Service_DebugTracer.TraceUIActions = true;
        Service_DebugTracer.TracePerformance = true;

        // Enable all component tracing
        foreach (var key in _componentTracing.Keys.ToList())
        {
            _componentTracing[key] = true;
        }

        // Set all components to high level
        foreach (var key in _componentLevels.Keys.ToList())
        {
            _componentLevels[key] = DebugLevel.High;
        }

        Service_DebugTracer.TraceUIAction("DEBUG_MODE_SET", "Service_DebugConfiguration",
            new Dictionary<string, object>
            {
                ["Mode"] = "DEVELOPMENT",
                ["Level"] = "VERBOSE",
                ["AllComponentsEnabled"] = true
            });
    }

    /// <summary>
    /// Set debug configuration for production mode (minimal tracing)
    /// </summary>
    public static void SetProductionMode()
    {
        Service_DebugTracer.CurrentLevel = DebugLevel.Low;
        Service_DebugTracer.TraceDatabase = false;
        Service_DebugTracer.TraceBusinessLogic = false;
        Service_DebugTracer.TraceUIActions = false;
        Service_DebugTracer.TracePerformance = false;

        // Disable most component tracing
        foreach (var key in _componentTracing.Keys.ToList())
        {
            _componentTracing[key] = key == "ErrorHandling"; // Only keep error handling
        }

        // Set all components to low level
        foreach (var key in _componentLevels.Keys.ToList())
        {
            _componentLevels[key] = DebugLevel.Low;
        }

        Service_DebugTracer.TraceUIAction("DEBUG_MODE_SET", "Service_DebugConfiguration",
            new Dictionary<string, object>
            {
                ["Mode"] = "PRODUCTION",
                ["Level"] = "LOW",
                ["OnlyErrorHandling"] = true
            });
    }

    /// <summary>
    /// Set debug configuration for database troubleshooting
    /// </summary>
    public static void SetDatabaseTroubleshootingMode()
    {
        Service_DebugTracer.CurrentLevel = DebugLevel.Verbose;
        Service_DebugTracer.TraceDatabase = true;
        Service_DebugTracer.TraceBusinessLogic = true;
        Service_DebugTracer.TraceUIActions = false;
        Service_DebugTracer.TracePerformance = true;

        // Enable database-related components
        _componentTracing["Database"] = true;
        _componentTracing["BusinessLogic"] = true;
        _componentTracing["DataValidation"] = true;
        _componentTracing["Performance"] = true;
        _componentTracing["ErrorHandling"] = true;

        // Disable UI-related components
        _componentTracing["UIActions"] = false;
        _componentTracing["FileOperations"] = false;
        _componentTracing["NetworkOperations"] = false;

        // Set database components to verbose
        _componentLevels["Database"] = DebugLevel.Verbose;
        _componentLevels["BusinessLogic"] = DebugLevel.Verbose;
        _componentLevels["ErrorHandling"] = DebugLevel.High;

        Service_DebugTracer.TraceUIAction("DEBUG_MODE_SET", "Service_DebugConfiguration",
            new Dictionary<string, object>
            {
                ["Mode"] = "DATABASE_TROUBLESHOOTING",
                ["DatabaseTracing"] = true,
                ["UITracing"] = false,
                ["PerformanceTracing"] = true
            });
    }

    #endregion

    #region Status Information

    /// <summary>
    /// Get current debug configuration status
    /// </summary>
    /// <returns>Dictionary with current configuration</returns>
    public static Dictionary<string, object> GetCurrentStatus()
    {
        return new Dictionary<string, object>
        {
            ["GlobalLevel"] = Service_DebugTracer.CurrentLevel.ToString(),
            ["DatabaseTracing"] = Service_DebugTracer.TraceDatabase,
            ["BusinessLogicTracing"] = Service_DebugTracer.TraceBusinessLogic,
            ["UIActionsTracing"] = Service_DebugTracer.TraceUIActions,
            ["PerformanceTracing"] = Service_DebugTracer.TracePerformance,
            ["ComponentTracingEnabled"] = _componentTracing.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList(),
            ["ComponentLevels"] = _componentLevels.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString())
        };
    }

    /// <summary>
    /// Log current debug configuration status
    /// </summary>
    public static void LogCurrentStatus()
    {
        var status = GetCurrentStatus();
        Service_DebugTracer.TraceUIAction("DEBUG_STATUS_REPORT", "Service_DebugConfiguration", status);
    }

    #endregion
}