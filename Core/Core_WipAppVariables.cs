using System.Reflection;
using System.Windows.Forms;
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

    #region UI Shortcuts

    // Inventory Tab
    public static Keys Shortcut_Inventory_Save = Keys.Control | Keys.S;
    public static Keys Shortcut_Inventory_Advanced = Keys.Control | Keys.Shift | Keys.A;
    public static Keys Shortcut_Inventory_Reset = Keys.Control | Keys.R;
    public static Keys Shortcut_Inventory_ToggleRightPanel_Right = Keys.Alt | Keys.Right;
    public static Keys Shortcut_Inventory_ToggleRightPanel_Left = Keys.Alt | Keys.Left;

    // Advanced Inventory Tab (Single)
    public static Keys Shortcut_AdvInv_Send = Keys.Control | Keys.P;
    public static Keys Shortcut_AdvInv_Save = Keys.Control | Keys.S;
    public static Keys Shortcut_AdvInv_Reset = Keys.Control | Keys.R;
    public static Keys Shortcut_AdvInv_Normal = Keys.Control | Keys.Shift | Keys.A;

    // Advanced Inventory Tab (MultiLoc)
    public static Keys Shortcut_AdvInv_Multi_AddLoc = Keys.Enter;
    public static Keys Shortcut_AdvInv_Multi_SaveAll = Keys.Control | Keys.S;
    public static Keys Shortcut_AdvInv_Multi_Reset = Keys.Control | Keys.R;
    public static Keys Shortcut_AdvInv_Multi_Normal = Keys.Control | Keys.Shift | Keys.A;

    // Advanced Inventory Tab (Import)
    public static Keys Shortcut_AdvInv_Import_OpenExcel = Keys.None; // No shortcut
    public static Keys Shortcut_AdvInv_Import_ImportExcel = Keys.None; // No shortcut
    public static Keys Shortcut_AdvInv_Import_Save = Keys.Control | Keys.S;
    public static Keys Shortcut_AdvInv_Import_Normal = Keys.Control | Keys.Shift | Keys.A;

    // Remove Tab
    public static Keys Shortcut_Remove_Search = Keys.Control | Keys.F;
    public static Keys Shortcut_Remove_Delete = Keys.Delete;
    public static Keys Shortcut_Remove_Undo = Keys.Control | Keys.Z;
    public static Keys Shortcut_Remove_Reset = Keys.Control | Keys.R;
    public static Keys Shortcut_Remove_Advanced = Keys.Control | Keys.Shift | Keys.A;
    public static Keys Shortcut_Remove_Normal = Keys.Control | Keys.Shift | Keys.N;

    // Transfer Tab
    public static Keys Shortcut_Transfer_Search = Keys.Control | Keys.F;
    public static Keys Shortcut_Transfer_Transfer = Keys.Control | Keys.T;
    public static Keys Shortcut_Transfer_Reset = Keys.Control | Keys.R;
    public static Keys Shortcut_Transfer_ToggleRightPanel_Right = Keys.Alt | Keys.Right;
    public static Keys Shortcut_Transfer_ToggleRightPanel_Left = Keys.Alt | Keys.Left;

    #endregion

    #region Shortcut Helper

    public static string ToShortcutString(Keys keys)
    {
        if (keys == Keys.None) return "";
        var parts = new List<string>();
        if (keys.HasFlag(Keys.Control)) parts.Add("CTRL");
        if (keys.HasFlag(Keys.Shift)) parts.Add("SHIFT");
        if (keys.HasFlag(Keys.Alt)) parts.Add("ALT");
        // Remove modifier flags to get the main key
        var keyOnly = keys & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;
        if (keyOnly != Keys.None)
        {
            // Special case for arrow keys
            if (keyOnly == Keys.Left) parts.Add("LEFT");
            else if (keyOnly == Keys.Right) parts.Add("RIGHT");
            else if (keyOnly == Keys.Up) parts.Add("UP");
            else if (keyOnly == Keys.Down) parts.Add("DOWN");
            else parts.Add(keyOnly.ToString().ToUpper());
        }

        return string.Join(" + ", parts);
    }

    #endregion
}

#endregion