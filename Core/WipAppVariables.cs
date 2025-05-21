using MTM_WIP_Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Core;

internal static class WipAppVariables
{
    // Create Bools
    public static bool mainFormFormReset = false;
    public static bool userTypeAdmin = false;
    public static bool userTypeReadOnly = false;
    // Create Ints

    public static int RemoveQuantity;

    public static int InventoryQuantity;

    // Create Strings
    public static string? PrintOp;
    public static string? PrintTitle;
    public static string? removeId;
    public static string? RemoveLocation;
    public static string? RemovePartNumber;
    public static string? RemoveUser;
    public static string? id;
    public static string? Location;
    public static string? OldLocation;
    public static string? NewLocation;
    public static string? PartType;
    public static string? partId;
    public static string? Operation;
    public static string? Notes;
    public static string? TransferType;
    public static string? Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
    public static string? VisualUserName;
    public static string? VisualPassword;
    public static string? WipServerPort;
    public static string? WipServerAddress;
    public static string? WipDataGridTheme;
    public static string? UserShift;
    public static string? UserPin;


    public static string
        User = SystemDao.System_GetUserName();

    public static string connectionString = SqlVariables.GetConnectionString(null, null, null, null);
    internal static List<Last10> list = [];

    public static string? RemoveOperation { get; internal set; }
}