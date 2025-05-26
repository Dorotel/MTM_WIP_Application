#pragma warning disable CS0649
using System.Reflection;
using System.Windows.Forms; // Add this at the top if not already present
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Models;

namespace MTM_WIP_Application.Core;

/// <summary>
///     Centralized static variables for application-wide state and configuration.
/// </summary>
internal static class WipAppVariables
{
    // Connection string for database access
    public static string EnteredUser = "Default User";
    public static string? Id; // Generic ID
    public static int InventoryQuantity; // Current inventory quantity

    // List of the last 10 items interacted with by the user (for quick access buttons)
    internal static List<Last10> List = [];

    public static string? Location; // Current location

    // Boolean flags for application state
    public static bool MainFormFormReset = false; // Indicates if the main form should be reset
    public static string? NewLocation; // New location (for transfers)
    public static string? Notes; // Notes for operations
    public static string? OldLocation; // Previous location (for transfers)
    public static string? Operation; // Current operation
    public static string? PartId; // Part ID
    public static string? PartType; // Type of part

    // String values for various operational and user context
    public static string? PrintOp; // Operation to print
    public static string? PrintTitle; // Title for print operations
    public static string? RemoveId; // ID for removal operations
    public static string? RemoveLocation; // Location for removal
    public static string? RemovePartNumber; // Part number for removal

    // Integer values for inventory and removal operations
    public static int RemoveQuantity; // Quantity to remove from inventory
    public static string? RemoveUser; // User performing removal

    // Theme font size (added from solution context)
    public static int ThemeFontSize = 9;
    public static string? TransferType; // Type of transfer

    // Current user (resolved at startup)
    public static string User = SystemDao.System_GetUserName();
    public static string? UserPin; // User's PIN
    public static string? UserShift; // User's shift
    public static bool UserTypeAdmin = false; // True if the current user is an admin
    public static bool UserTypeReadOnly = false; // True if the current user is read-only
    public static string UserVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";

    // App version (entry assembly)
    public static string? Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
    public static string? VisualPassword; // Visual password for UI

    // User settings and preferences
    public static string? VisualUserName; // Visual username for UI
    public static string? WipDataGridTheme; // Theme for data grid
    public static string? WipServerAddress; // WIP server address
    public static string? WipServerPort; // WIP server port

    // Operation used for removal, with internal setter
    public static string? RemoveOperation { get; internal set; }

// Connection string for database access
    public static string ConnectionString =
        ShowConnectionStringAndReturn(SqlVariables.GetConnectionString(null, null, null, null));

    private static string ShowConnectionStringAndReturn(string connectionString)
    {
        MessageBox.Show(connectionString, "Connection String", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return connectionString;
    }

    // No logging-specific variables are needed here.
    // Logging is now handled entirely by AppLogger and does not require state in WipAppVariables.
}