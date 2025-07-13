using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_Location

internal static class Dao_Location
{
    #region Fields

    public static Helper_Database_Core HelperDatabaseCore =
        new(Helper_Database_Variables.GetConnectionString(
            Model_AppVariables.WipServerAddress,
            "mtm_wip_application",
            Model_AppVariables.User,
            Model_AppVariables.UserPin
        ));

    #endregion

    #region Delete

    internal static async Task DeleteLocation(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["p_Location"] = location };
        await HelperDatabaseCore.ExecuteNonQuery("md_locations_Delete_ByLocation", parameters, useAsync,
            CommandType.StoredProcedure);
    }

    #endregion

    #region Insert

    internal static async Task InsertLocation(string location, string building, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["p_Location"] = location,
            ["p_IssuedBy"] = Model_AppVariables.User,
            ["p_Building"] = building
        };
        await HelperDatabaseCore.ExecuteNonQuery("md_locations_Add_Location", parameters, useAsync,
            CommandType.StoredProcedure);
    }

    #endregion

    #region Update

    internal static async Task UpdateLocation(string oldLocation, string newLocation, string building,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["p_OldLocation"] = oldLocation,
            ["p_Location"] = newLocation,
            ["p_IssuedBy"] = Model_AppVariables.User,
            ["p_Building"] = building
        };
        await HelperDatabaseCore.ExecuteNonQuery("md_locations_Update_Location", parameters, useAsync,
            CommandType.StoredProcedure);
    }

    internal static async Task<DataTable> GetAllLocations(bool useAsync = false)
    {
        return await HelperDatabaseCore.ExecuteDataTable("md_locations_Get_All", null, useAsync,
            CommandType.StoredProcedure);
    }

    internal static async Task<DataRow?> GetLocationByName(string location, bool useAsync = false)
    {
        var table = await GetAllLocations(useAsync);
        var rows = table.Select($"Location = '{location.Replace("'", "''")}'");
        return rows.Length > 0 ? rows[0] : null;
    }

    #endregion

    #region Existence Check

    internal static async Task<bool> LocationExists(string location, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@location"] = location };
        var result = await HelperDatabaseCore.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_locations` WHERE `Location` = @location",
            parameters, useAsync, CommandType.Text);
        return Convert.ToInt32(result) > 0;
    }

    #endregion
}

#endregion