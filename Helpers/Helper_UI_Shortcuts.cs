using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;

namespace MTM_Inventory_Application.Helpers;

internal class Helper_UI_Shortcuts
{
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
                    if (Enum.TryParse<Keys>(part, true, out var key)) keys |= key;
                    break;
            }

        return keys;
    }

    public static Dictionary<string, Keys> GetShortcutDictionary()
    {
        return new Dictionary<string, Keys>
        {
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

    #endregion
}