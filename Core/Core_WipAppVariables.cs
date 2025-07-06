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

    public static Keys FromShortcutString(string shortcutString)
    {
        if (string.IsNullOrWhiteSpace(shortcutString)) return Keys.None;

        var keys = Keys.None;
        var parts = shortcutString.Split(new[] { " + ", "+", " " }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(p => p.Trim().ToUpper())
                                 .ToList();

        foreach (var part in parts)
        {
            switch (part)
            {
                case "CTRL":
                case "CONTROL":
                    keys |= Keys.Control;
                    break;
                case "SHIFT":
                    keys |= Keys.Shift;
                    break;
                case "ALT":
                    keys |= Keys.Alt;
                    break;
                case "LEFT":
                    keys |= Keys.Left;
                    break;
                case "RIGHT":
                    keys |= Keys.Right;
                    break;
                case "UP":
                    keys |= Keys.Up;
                    break;
                case "DOWN":
                    keys |= Keys.Down;
                    break;
                case "ENTER":
                    keys |= Keys.Enter;
                    break;
                case "DELETE":
                case "DEL":
                    keys |= Keys.Delete;
                    break;
                default:
                    // Try to parse as a standard key
                    if (Enum.TryParse<Keys>(part, true, out var key))
                    {
                        keys |= key;
                    }
                    break;
            }
        }

        return keys;
    }

    public static Dictionary<string, Keys> GetShortcutDictionary()
    {
        return new Dictionary<string, Keys>
        {
            ["Inventory - Save"] = Shortcut_Inventory_Save,
            ["Inventory - Advanced"] = Shortcut_Inventory_Advanced,
            ["Inventory - Reset"] = Shortcut_Inventory_Reset,
            ["Inventory - Toggle Right Panel (Right)"] = Shortcut_Inventory_ToggleRightPanel_Right,
            ["Inventory - Toggle Right Panel (Left)"] = Shortcut_Inventory_ToggleRightPanel_Left,
            ["Advanced Inventory - Send"] = Shortcut_AdvInv_Send,
            ["Advanced Inventory - Save"] = Shortcut_AdvInv_Save,
            ["Advanced Inventory - Reset"] = Shortcut_AdvInv_Reset,
            ["Advanced Inventory - Normal"] = Shortcut_AdvInv_Normal,
            ["Advanced Inventory MultiLoc - Add Location"] = Shortcut_AdvInv_Multi_AddLoc,
            ["Advanced Inventory MultiLoc - Save All"] = Shortcut_AdvInv_Multi_SaveAll,
            ["Advanced Inventory MultiLoc - Reset"] = Shortcut_AdvInv_Multi_Reset,
            ["Advanced Inventory MultiLoc - Normal"] = Shortcut_AdvInv_Multi_Normal,
            ["Advanced Inventory Import - Open Excel"] = Shortcut_AdvInv_Import_OpenExcel,
            ["Advanced Inventory Import - Import Excel"] = Shortcut_AdvInv_Import_ImportExcel,
            ["Advanced Inventory Import - Save"] = Shortcut_AdvInv_Import_Save,
            ["Advanced Inventory Import - Normal"] = Shortcut_AdvInv_Import_Normal,
            ["Remove - Search"] = Shortcut_Remove_Search,
            ["Remove - Delete"] = Shortcut_Remove_Delete,
            ["Remove - Undo"] = Shortcut_Remove_Undo,
            ["Remove - Reset"] = Shortcut_Remove_Reset,
            ["Remove - Advanced"] = Shortcut_Remove_Advanced,
            ["Remove - Normal"] = Shortcut_Remove_Normal,
            ["Transfer - Search"] = Shortcut_Transfer_Search,
            ["Transfer - Transfer"] = Shortcut_Transfer_Transfer,
            ["Transfer - Reset"] = Shortcut_Transfer_Reset,
            ["Transfer - Toggle Right Panel (Right)"] = Shortcut_Transfer_ToggleRightPanel_Right,
            ["Transfer - Toggle Right Panel (Left)"] = Shortcut_Transfer_ToggleRightPanel_Left
        };
    }

    public static void ApplyShortcutFromDictionary(string actionName, Keys newKeys)
    {
        switch (actionName)
        {
            case "Inventory - Save":
                Shortcut_Inventory_Save = newKeys;
                break;
            case "Inventory - Advanced":
                Shortcut_Inventory_Advanced = newKeys;
                break;
            case "Inventory - Reset":
                Shortcut_Inventory_Reset = newKeys;
                break;
            case "Inventory - Toggle Right Panel (Right)":
                Shortcut_Inventory_ToggleRightPanel_Right = newKeys;
                break;
            case "Inventory - Toggle Right Panel (Left)":
                Shortcut_Inventory_ToggleRightPanel_Left = newKeys;
                break;
            case "Advanced Inventory - Send":
                Shortcut_AdvInv_Send = newKeys;
                break;
            case "Advanced Inventory - Save":
                Shortcut_AdvInv_Save = newKeys;
                break;
            case "Advanced Inventory - Reset":
                Shortcut_AdvInv_Reset = newKeys;
                break;
            case "Advanced Inventory - Normal":
                Shortcut_AdvInv_Normal = newKeys;
                break;
            case "Advanced Inventory MultiLoc - Add Location":
                Shortcut_AdvInv_Multi_AddLoc = newKeys;
                break;
            case "Advanced Inventory MultiLoc - Save All":
                Shortcut_AdvInv_Multi_SaveAll = newKeys;
                break;
            case "Advanced Inventory MultiLoc - Reset":
                Shortcut_AdvInv_Multi_Reset = newKeys;
                break;
            case "Advanced Inventory MultiLoc - Normal":
                Shortcut_AdvInv_Multi_Normal = newKeys;
                break;
            case "Advanced Inventory Import - Open Excel":
                Shortcut_AdvInv_Import_OpenExcel = newKeys;
                break;
            case "Advanced Inventory Import - Import Excel":
                Shortcut_AdvInv_Import_ImportExcel = newKeys;
                break;
            case "Advanced Inventory Import - Save":
                Shortcut_AdvInv_Import_Save = newKeys;
                break;
            case "Advanced Inventory Import - Normal":
                Shortcut_AdvInv_Import_Normal = newKeys;
                break;
            case "Remove - Search":
                Shortcut_Remove_Search = newKeys;
                break;
            case "Remove - Delete":
                Shortcut_Remove_Delete = newKeys;
                break;
            case "Remove - Undo":
                Shortcut_Remove_Undo = newKeys;
                break;
            case "Remove - Reset":
                Shortcut_Remove_Reset = newKeys;
                break;
            case "Remove - Advanced":
                Shortcut_Remove_Advanced = newKeys;
                break;
            case "Remove - Normal":
                Shortcut_Remove_Normal = newKeys;
                break;
            case "Transfer - Search":
                Shortcut_Transfer_Search = newKeys;
                break;
            case "Transfer - Transfer":
                Shortcut_Transfer_Transfer = newKeys;
                break;
            case "Transfer - Reset":
                Shortcut_Transfer_Reset = newKeys;
                break;
            case "Transfer - Toggle Right Panel (Right)":
                Shortcut_Transfer_ToggleRightPanel_Right = newKeys;
                break;
            case "Transfer - Toggle Right Panel (Left)":
                Shortcut_Transfer_ToggleRightPanel_Left = newKeys;
                break;
        }
    }

    #endregion
}

#endregion