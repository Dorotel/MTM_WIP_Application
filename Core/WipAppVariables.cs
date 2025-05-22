using MTM_WIP_Application.Data;
using MTM_WIP_Application.Models;
using System.Reflection;

namespace MTM_WIP_Application.Core;

/// <summary>
/// Centralized static variables for application-wide state and configuration.
/// </summary>
internal static class WipAppVariables
{
    // Boolean flags for application state
    public static bool mainFormFormReset = false; // Indicates if the main form should be reset
    public static bool userTypeAdmin = false; // True if the current user is an admin
    public static bool userTypeReadOnly = false; // True if the current user is read-only
    public static string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    public static string enteredUser = "Default User";

    // Integer values for inventory and removal operations
    public static int RemoveQuantity; // Quantity to remove from inventory
    public static int InventoryQuantity; // Current inventory quantity

    // String values for various operational and user context
    public static string? PrintOp; // Operation to print
    public static string? PrintTitle; // Title for print operations
    public static string? removeId; // ID for removal operations
    public static string? RemoveLocation; // Location for removal
    public static string? RemovePartNumber; // Part number for removal
    public static string? RemoveUser; // User performing removal
    public static string? id; // Generic ID
    public static string? Location; // Current location
    public static string? OldLocation; // Previous location (for transfers)
    public static string? NewLocation; // New location (for transfers)
    public static string? PartType; // Type of part
    public static string? partId; // Part ID
    public static string? Operation; // Current operation
    public static string? Notes; // Notes for operations
    public static string? TransferType; // Type of transfer
    public static string? Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString(); // App version
    public static string? VisualUserName; // Visual username for UI
    public static string? VisualPassword; // Visual password for UI
    public static string? WipServerPort; // WIP server port
    public static string? WipServerAddress; // WIP server address
    public static string? WipDataGridTheme; // Theme for data grid
    public static string? UserShift; // User's shift
    public static string? UserPin; // User's PIN

    // Current user (resolved at startup)
    public static string User = SystemDao.System_GetUserName();

    // Connection string for database access
    public static string connectionString = SqlVariables.GetConnectionString(null, null, null, null);

    // List of the last 10 items interacted with by the user (for quick access buttons)
    internal static List<Last10> list = new();

    // Operation used for removal, with internal setter
    public static string? RemoveOperation { get; internal set; }

    // No logging-specific variables are needed here.
    // Logging is now handled entirely by AppLogger and does not require state in WipAppVariables.
}