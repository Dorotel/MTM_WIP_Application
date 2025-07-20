using System.Reflection;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;

namespace MTM_Inventory_Application.Models
{
    #region Model_AppVariables

    internal static class Model_AppVariables
    {
        #region User Info

        public static string EnteredUser { get; set; } = "Default User";
        public static string User { get; set; } = Dao_System.System_GetUserName();
        public static string? UserPin { get; set; }
        public static string? UserShift { get; set; }
        public static bool UserTypeAdmin { get; set; } = false;
        public static bool UserTypeReadOnly { get; set; } = false;
        public static bool UserTypeNormal { get; set; } = true;

        public static string UserVersion { get; set; } =
            Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";

        public static string? UserFullName { get; set; }
        public static string? VisualUserName { get; set; }
        public static string? VisualPassword { get; set; }
        public static Model_UserUiColors UserUiColors { get; set; } = new();

        #endregion

        #region Inventory State

        public static int InventoryQuantity { get; set; }
        public static string? PartId { get; set; }
        public static string? PartType { get; set; }
        public static string? Location { get; set; }
        public static string? Operation { get; set; }
        public static string? Notes { get; set; }

        #endregion

        #region Theme & Version

        public static string? ThemeName { get; set; } = "Default";
        public static float ThemeFontSize { get; set; } = 9f;
        public static string? WipDataGridTheme { get; set; } = "Default";
        public static string? WipServerAddress { get; set; } = "172.16.1.104"; //172.16.1.104
        public static string? WipServerPort { get; set; } = "3306";
        public static string? Version { get; set; } = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();

        public static string ConnectionString { get; set; } =
            Helper_Database_Variables.GetConnectionString(null, null, null, null);

        #endregion
    }

    #endregion
}
