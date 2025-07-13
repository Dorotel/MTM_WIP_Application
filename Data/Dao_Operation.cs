

using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Data;

#region Dao_Operation

internal static class Dao_Operation
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

    internal static async Task DeleteOperation(string operationNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["p_Operation"] = operationNumber };
        await HelperDatabaseCore.ExecuteNonQuery("md_operation_numbers_Delete_ByOperation", parameters, useAsync,
            CommandType.StoredProcedure);
    }

    #endregion

    #region Insert

    internal static async Task InsertOperation(string operationNumber, string user, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["p_Operation"] = operationNumber,
            ["p_IssuedBy"] = user
        };
        await HelperDatabaseCore.ExecuteNonQuery("md_operation_numbers_Add_Operation", parameters, useAsync,
            CommandType.StoredProcedure);
    }

    #endregion

    #region Update

    internal static async Task UpdateOperation(string oldOperation, string newOperationNumber, string user,
        bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["p_Operation"] = oldOperation,
            ["p_NewOperation"] = newOperationNumber,
            ["p_IssuedBy"] = user
        };
        await HelperDatabaseCore.ExecuteNonQuery(
            "md_operation_numbers_Update_Operation",
            parameters,
            useAsync,
            CommandType.StoredProcedure
        );
    }

    #endregion

    #region Read

    internal static async Task<DataTable> GetAllOperations(bool useAsync = false)
    {
        return await HelperDatabaseCore.ExecuteDataTable(
            "md_operation_numbers_Get_All",
            useAsync: useAsync,
            commandType: CommandType.StoredProcedure
        );
    }

    internal static async Task<DataRow?> GetOperationByNumber(string operationNumber, bool useAsync = false)
    {
        var table = await GetAllOperations(useAsync);
        var rows = table.Select($"Operation = '{operationNumber.Replace("'", "''")}'");
        return rows.Length > 0 ? rows[0] : null;
    }

    internal static async Task<bool> OperationExists(string operationNumber, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@operationNumber"] = operationNumber };
        var result = await HelperDatabaseCore.ExecuteScalar(
            "SELECT COUNT(*) FROM `md_operation_numbers` WHERE `Operation` = @operationNumber",
            parameters, useAsync, CommandType.Text);
        return Convert.ToInt32(result) > 0;
    }

    #endregion

    #endregion
}