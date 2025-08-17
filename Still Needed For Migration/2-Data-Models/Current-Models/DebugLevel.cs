namespace MTM_Inventory_Application.Models;

/// <summary>
/// Debug tracing levels for Service_DebugTracer
/// </summary>
public enum DebugLevel
{
    /// <summary>
    /// Minimal tracing - only critical UI actions and major operations
    /// </summary>
    Low = 1,
    
    /// <summary>
    /// Moderate tracing - includes method entry/exit, database operations, business logic
    /// </summary>
    Medium = 2,
    
    /// <summary>
    /// Comprehensive tracing - includes all parameters, data, performance metrics, validation
    /// </summary>
    High = 3,
    
    /// <summary>
    /// Extreme tracing - includes everything, full JSON serialization, all data flows
    /// </summary>
    Verbose = 4
}