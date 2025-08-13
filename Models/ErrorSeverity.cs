namespace MTM_Inventory_Application.Models;

/// <summary>
/// Defines the severity levels for error handling in the MTM Inventory Application
/// </summary>
public enum ErrorSeverity
{
    /// <summary>
    /// Low severity - Information/Warning level, application continues normally
    /// </summary>
    Low = 0,
    
    /// <summary>
    /// Medium severity - Recoverable error, operation failed but can be retried
    /// </summary>
    Medium = 1,
    
    /// <summary>
    /// High severity - Critical error, data integrity or major functionality affected
    /// </summary>
    High = 2,
    
    /// <summary>
    /// Fatal severity - Application termination required, unrecoverable error
    /// </summary>
    Fatal = 3
}