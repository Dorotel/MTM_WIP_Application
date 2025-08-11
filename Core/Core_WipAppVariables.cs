

using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;

namespace MTM_Inventory_Application.Core;

#region Core_WipAppVariables

internal static class Core_WipAppVariables
{
    #region User Info

    public static readonly string User = Dao_System.System_GetUserName();

    #endregion

    #region Theme & Version

    public static readonly string ReConnectionString =
        Helper_Database_Variables.GetConnectionString(null, null, null, null);

    #endregion

    #region UI Shortcuts

    public static Keys Shortcut_Inventory_Save = Keys.Control | Keys.S;
    public static Keys Shortcut_Inventory_Advanced = Keys.Control | Keys.Shift | Keys.A;
    public static Keys Shortcut_Inventory_Reset = Keys.Control | Keys.R;
    public static Keys Shortcut_Inventory_ToggleRightPanel_Right = Keys.Alt | Keys.Right;
    public static Keys Shortcut_Inventory_ToggleRightPanel_Left = Keys.Alt | Keys.Left;

    public static Keys Shortcut_AdvInv_Send = Keys.Control | Keys.P;
    public static Keys Shortcut_AdvInv_Save = Keys.Control | Keys.S;
    public static Keys Shortcut_AdvInv_Reset = Keys.Control | Keys.R;
    public static Keys Shortcut_AdvInv_Normal = Keys.Control | Keys.Shift | Keys.A;

    public static Keys Shortcut_AdvInv_Multi_AddLoc = Keys.Enter;
    public static Keys Shortcut_AdvInv_Multi_SaveAll = Keys.Control | Keys.S;
    public static Keys Shortcut_AdvInv_Multi_Reset = Keys.Control | Keys.R;
    public static Keys Shortcut_AdvInv_Multi_Normal = Keys.Control | Keys.Shift | Keys.A;

    public static Keys Shortcut_AdvInv_Import_OpenExcel = Keys.None;
    public static Keys Shortcut_AdvInv_Import_ImportExcel = Keys.None;
    public static Keys Shortcut_AdvInv_Import_Save = Keys.Control | Keys.S;
    public static Keys Shortcut_AdvInv_Import_Normal = Keys.Control | Keys.Shift | Keys.A;

    public static Keys Shortcut_Remove_Search = Keys.Control | Keys.F;
    public static Keys Shortcut_Remove_Delete = Keys.Delete;
    public static Keys Shortcut_Remove_Undo = Keys.Control | Keys.Z;
    public static Keys Shortcut_Remove_Reset = Keys.Control | Keys.R;
    public static Keys Shortcut_Remove_Advanced = Keys.Control | Keys.Shift | Keys.A;
    public static Keys Shortcut_Remove_Normal = Keys.Control | Keys.Shift | Keys.N;

    public static Keys Shortcut_Transfer_Search = Keys.Control | Keys.F;
    public static Keys Shortcut_Transfer_Transfer = Keys.Control | Keys.T;
    public static Keys Shortcut_Transfer_Reset = Keys.Control | Keys.R;
    public static Keys Shortcut_Transfer_ToggleRightPanel_Right = Keys.Alt | Keys.Right;
    public static Keys Shortcut_Transfer_ToggleRightPanel_Left = Keys.Alt | Keys.Left;

    public static Keys Shortcut_MainForm_Tab1 = Keys.Control | Keys.D1;
    public static Keys Shortcut_MainForm_Tab2 = Keys.Control | Keys.D2;
    public static Keys Shortcut_MainForm_Tab3 = Keys.Control | Keys.D3;

    #endregion
}

#endregion
