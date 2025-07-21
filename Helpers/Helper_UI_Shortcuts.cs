

using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Forms.MainForm;

namespace MTM_Inventory_Application.Helpers;

internal class Helper_UI_Shortcuts
{
    #region Shortcut Helper

    public static string ToShortcutString(Keys keys)
    {
        if (keys == Keys.None) return "";
        var parts = new List<string>();

        // Modifiers
        if (keys.HasFlag(Keys.Control)) parts.Add("Ctrl");
        if (keys.HasFlag(Keys.Shift)) parts.Add("Shift");
        if (keys.HasFlag(Keys.Alt)) parts.Add("Alt");

        // Remove modifiers to get the main key
        var keyOnly = keys & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;
        if (keyOnly != Keys.None)
        {
            // Handle D0-D9 as "0"-"9"
            if (keyOnly >= Keys.D0 && keyOnly <= Keys.D9)
            {
                parts.Add(((int)keyOnly - (int)Keys.D0).ToString());
            }
            // Handle NumPad0-NumPad9 as "Num0"-"Num9"
            else if (keyOnly >= Keys.NumPad0 && keyOnly <= Keys.NumPad9)
            {
                parts.Add("Num" + ((int)keyOnly - (int)Keys.NumPad0));
            }
            // Handle arrows and other common keys
            else if (keyOnly == Keys.Left) parts.Add("Left");
            else if (keyOnly == Keys.Right) parts.Add("Right");
            else if (keyOnly == Keys.Up) parts.Add("Up");
            else if (keyOnly == Keys.Down) parts.Add("Down");
            else if (keyOnly == Keys.Enter) parts.Add("Enter");
            else if (keyOnly == Keys.Delete) parts.Add("Delete");
            else if (keyOnly == Keys.Escape) parts.Add("Esc");
            else parts.Add(keyOnly.ToString());
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
                    if (Enum.TryParse<Keys>(part, true, out var key)) keys |= key;
                    break;
            }

        return keys;
    }

    public static Dictionary<string, Keys> GetShortcutDictionary()
    {
        return new Dictionary<string, Keys>
        {
            ["Main Form - Inventory Tab"] = Core_WipAppVariables.Shortcut_MainForm_Tab1,
            ["Main Form - Transfer Tab"] = Core_WipAppVariables.Shortcut_MainForm_Tab2,
            ["Main Form - Remove Tab"] = Core_WipAppVariables.Shortcut_MainForm_Tab3,
            ["Inventory - Save"] = Core_WipAppVariables.Shortcut_Inventory_Save,
            ["Inventory - Advanced"] = Core_WipAppVariables.Shortcut_Inventory_Advanced,
            ["Inventory - Reset"] = Core_WipAppVariables.Shortcut_Inventory_Reset,
            ["Inventory - Toggle Right Panel (Right)"] = Core_WipAppVariables.Shortcut_Inventory_ToggleRightPanel_Right,
            ["Inventory - Toggle Right Panel (Left)"] = Core_WipAppVariables.Shortcut_Inventory_ToggleRightPanel_Left,
            ["Advanced Inventory - Send"] = Core_WipAppVariables.Shortcut_AdvInv_Send,
            ["Advanced Inventory - Save"] = Core_WipAppVariables.Shortcut_AdvInv_Save,
            ["Advanced Inventory - Reset"] = Core_WipAppVariables.Shortcut_AdvInv_Reset,
            ["Advanced Inventory - Normal"] = Core_WipAppVariables.Shortcut_AdvInv_Normal,
            ["Advanced Inventory MultiLoc - Add Location"] = Core_WipAppVariables.Shortcut_AdvInv_Multi_AddLoc,
            ["Advanced Inventory MultiLoc - Save All"] = Core_WipAppVariables.Shortcut_AdvInv_Multi_SaveAll,
            ["Advanced Inventory MultiLoc - Reset"] = Core_WipAppVariables.Shortcut_AdvInv_Multi_Reset,
            ["Advanced Inventory MultiLoc - Normal"] = Core_WipAppVariables.Shortcut_AdvInv_Multi_Normal,
            ["Advanced Inventory Import - Open Excel"] = Core_WipAppVariables.Shortcut_AdvInv_Import_OpenExcel,
            ["Advanced Inventory Import - Import Excel"] = Core_WipAppVariables.Shortcut_AdvInv_Import_ImportExcel,
            ["Advanced Inventory Import - Save"] = Core_WipAppVariables.Shortcut_AdvInv_Import_Save,
            ["Advanced Inventory Import - Normal"] = Core_WipAppVariables.Shortcut_AdvInv_Import_Normal,
            ["Remove - Search"] = Core_WipAppVariables.Shortcut_Remove_Search,
            ["Remove - Delete"] = Core_WipAppVariables.Shortcut_Remove_Delete,
            ["Remove - Undo"] = Core_WipAppVariables.Shortcut_Remove_Undo,
            ["Remove - Reset"] = Core_WipAppVariables.Shortcut_Remove_Reset,
            ["Remove - Advanced"] = Core_WipAppVariables.Shortcut_Remove_Advanced,
            ["Remove - Normal"] = Core_WipAppVariables.Shortcut_Remove_Normal,
            ["Transfer - Search"] = Core_WipAppVariables.Shortcut_Transfer_Search,
            ["Transfer - Transfer"] = Core_WipAppVariables.Shortcut_Transfer_Transfer,
            ["Transfer - Reset"] = Core_WipAppVariables.Shortcut_Transfer_Reset,
            ["Transfer - Toggle Right Panel (Right)"] = Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Right,
            ["Transfer - Toggle Right Panel (Left)"] = Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Left

        };
    }

    public static void ApplyShortcutFromDictionary(string actionName, Keys newKeys)
    {
        switch (actionName)
        {
            case "Main Form - Inventory Tab":
                Core_WipAppVariables.Shortcut_MainForm_Tab1 = newKeys;
                break;
            case "Main Form - Transfer Tab":
                Core_WipAppVariables.Shortcut_MainForm_Tab2 = newKeys;
                break;
            case "Main Form - Remove Tab":
                Core_WipAppVariables.Shortcut_MainForm_Tab3 = newKeys;
                break;
            case "Inventory - Save":
                Core_WipAppVariables.Shortcut_Inventory_Save = newKeys;
                break;
            case "Inventory - Advanced":
                Core_WipAppVariables.Shortcut_Inventory_Advanced = newKeys;
                break;
            case "Inventory - Reset":
                Core_WipAppVariables.Shortcut_Inventory_Reset = newKeys;
                break;
            case "Inventory - Toggle Right Panel (Right)":
                Core_WipAppVariables.Shortcut_Inventory_ToggleRightPanel_Right = newKeys;
                break;
            case "Inventory - Toggle Right Panel (Left)":
                Core_WipAppVariables.Shortcut_Inventory_ToggleRightPanel_Left = newKeys;
                break;
            case "Advanced Inventory - Send":
                Core_WipAppVariables.Shortcut_AdvInv_Send = newKeys;
                break;
            case "Advanced Inventory - Save":
                Core_WipAppVariables.Shortcut_AdvInv_Save = newKeys;
                break;
            case "Advanced Inventory - Reset":
                Core_WipAppVariables.Shortcut_AdvInv_Reset = newKeys;
                break;
            case "Advanced Inventory - Normal":
                Core_WipAppVariables.Shortcut_AdvInv_Normal = newKeys;
                break;
            case "Advanced Inventory MultiLoc - Add Location":
                Core_WipAppVariables.Shortcut_AdvInv_Multi_AddLoc = newKeys;
                break;
            case "Advanced Inventory MultiLoc - Save All":
                Core_WipAppVariables.Shortcut_AdvInv_Multi_SaveAll = newKeys;
                break;
            case "Advanced Inventory MultiLoc - Reset":
                Core_WipAppVariables.Shortcut_AdvInv_Multi_Reset = newKeys;
                break;
            case "Advanced Inventory MultiLoc - Normal":
                Core_WipAppVariables.Shortcut_AdvInv_Multi_Normal = newKeys;
                break;
            case "Advanced Inventory Import - Open Excel":
                Core_WipAppVariables.Shortcut_AdvInv_Import_OpenExcel = newKeys;
                break;
            case "Advanced Inventory Import - Import Excel":
                Core_WipAppVariables.Shortcut_AdvInv_Import_ImportExcel = newKeys;
                break;
            case "Advanced Inventory Import - Save":
                Core_WipAppVariables.Shortcut_AdvInv_Import_Save = newKeys;
                break;
            case "Advanced Inventory Import - Normal":
                Core_WipAppVariables.Shortcut_AdvInv_Import_Normal = newKeys;
                break;
            case "Remove - Search":
                Core_WipAppVariables.Shortcut_Remove_Search = newKeys;
                break;
            case "Remove - Delete":
                Core_WipAppVariables.Shortcut_Remove_Delete = newKeys;
                break;
            case "Remove - Undo":
                Core_WipAppVariables.Shortcut_Remove_Undo = newKeys;
                break;
            case "Remove - Reset":
                Core_WipAppVariables.Shortcut_Remove_Reset = newKeys;
                break;
            case "Remove - Advanced":
                Core_WipAppVariables.Shortcut_Remove_Advanced = newKeys;
                break;
            case "Remove - Normal":
                Core_WipAppVariables.Shortcut_Remove_Normal = newKeys;
                break;
            case "Transfer - Search":
                Core_WipAppVariables.Shortcut_Transfer_Search = newKeys;
                break;
            case "Transfer - Transfer":
                Core_WipAppVariables.Shortcut_Transfer_Transfer = newKeys;
                break;
            case "Transfer - Reset":
                Core_WipAppVariables.Shortcut_Transfer_Reset = newKeys;
                break;
            case "Transfer - Toggle Right Panel (Right)":
                Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Right = newKeys;
                break;
            case "Transfer - Toggle Right Panel (Left)":
                Core_WipAppVariables.Shortcut_Transfer_ToggleRightPanel_Left = newKeys;
                break;
        }
    }

    public static void UpdateMainFormTabShortcuts(MainForm mainForm)
    {
        var tabShortcuts = new[]
        {
        (Index: 0, Shortcut: Core_WipAppVariables.Shortcut_MainForm_Tab1),
        (Index: 1, Shortcut: Core_WipAppVariables.Shortcut_MainForm_Tab2),
        (Index: 2, Shortcut: Core_WipAppVariables.Shortcut_MainForm_Tab3)
    };

        var tabNames = new[]
        {
        "New (Pick-Up)",
        "Remove (Deliver)",
        "Transfer (Location to Location)"
    };

        for (int i = 0; i < tabShortcuts.Length; i++)
        {
            if (mainForm.MainForm_TabControl.TabPages.Count > i)
            {
                var tabPage = mainForm.MainForm_TabControl.TabPages[i];
                var shortcutStr = "Shortcut: " + ToShortcutString(tabShortcuts[i].Shortcut);
                tabPage.Text = $"{tabNames[i]}";
                // Set tooltip for tab header, not TabPage control
                mainForm.MainForm_ToolTip?.SetToolTip(mainForm.MainForm_TabControl, shortcutStr);
                mainForm.MainForm_TabControl.TabPages[i].ToolTipText = shortcutStr; // Also set ToolTipText property
            }
        }
    }

    #endregion
}
