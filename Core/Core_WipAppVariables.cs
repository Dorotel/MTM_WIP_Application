using System.Reflection;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;

namespace MTM_Inventory_Application.Core;

#region Core_WipAppVariables

internal static class Core_WipAppVariables
{
    #region User Info

    public static string EnteredUser = "Default User";
    public static string User = Dao_System.System_GetUserName();
    public static string? UserPin;
    public static string? UserShift;
    public static bool UserTypeAdmin = false;
    public static bool UserTypeReadOnly = false;
    public static string UserVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
    public static string? UserFullName;
    public static string? VisualUserName;
    public static string? VisualPassword;

    #endregion

    #region Inventory State

    public static int InventoryQuantity;
    public static string? PartId;
    public static string? PartType;
    public static string? Location;
    public static string? Operation;
    public static string? Notes;

    #endregion

    #region Theme & Version

    public static float ThemeFontSize = 9f;
    public static string? WipDataGridTheme;
    public static string? WipServerAddress;
    public static string? WipServerPort;
    public static string? Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
    public static string ConnectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);

    #endregion
}

#endregion