using System.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;
using MySql.Data.MySqlClient;

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

    internal static async Task<DaoResult> DeleteOperation(string operationNumber, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["p_Operation"] = operationNumber };
            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery("md_operation_numbers_Delete_ByOperation", parameters, useAsync,
                CommandType.StoredProcedure);

            return rowsAffected > 0 
                ? DaoResult.Success($"Operation {operationNumber} deleted successfully", rowsAffected)
                : DaoResult.Failure($"Operation {operationNumber} not found or could not be deleted");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error deleting operation {operationNumber}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Error deleting operation {operationNumber}", ex);
        }
    }

    #endregion

    #region Insert

    internal static async Task<DaoResult> InsertOperation(string operationNumber, string user, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_Operation"] = operationNumber,
                ["p_IssuedBy"] = user
            };
            
            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery("md_operation_numbers_Add_Operation", parameters, useAsync,
                CommandType.StoredProcedure);

            return rowsAffected > 0 
                ? DaoResult.Success($"Operation {operationNumber} created successfully", rowsAffected)
                : DaoResult.Failure($"Failed to create operation {operationNumber}");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error creating operation {operationNumber}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Error creating operation {operationNumber}", ex);
        }
    }

    #endregion

    #region Update

    internal static async Task<DaoResult> UpdateOperation(string oldOperation, string newOperationNumber, string user,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["p_Operation"] = oldOperation,
                ["p_NewOperation"] = newOperationNumber,
                ["p_IssuedBy"] = user
            };
            
            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery(
                "md_operation_numbers_Update_Operation",
                parameters, useAsync, CommandType.StoredProcedure);

            return rowsAffected > 0 
                ? DaoResult.Success($"Operation updated from {oldOperation} to {newOperationNumber}", rowsAffected)
                : DaoResult.Failure($"Operation {oldOperation} not found or could not be updated");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Database error updating operation {oldOperation}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult.Failure($"Error updating operation {oldOperation}", ex);
        }
    }

    #endregion

    #region Read

    internal static async Task<DaoResult<DataTable>> GetAllOperations(bool useAsync = false)
    {
        try
        {
            DataTable result = await HelperDatabaseCore.ExecuteDataTable(
                "md_operation_numbers_Get_All",
                useAsync: useAsync,
                commandType: CommandType.StoredProcedure);
                
            return DaoResult<DataTable>.Success(result, $"Retrieved {result.Rows.Count} operations");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult<DataTable>.Failure("Database error retrieving operations", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult<DataTable>.Failure("Error retrieving operations", ex);
        }
    }

    internal static async Task<DaoResult<DataRow>> GetOperationByNumber(string operationNumber, bool useAsync = false)
    {
        try
        {
            var allOperationsResult = await GetAllOperations(useAsync);
            if (!allOperationsResult.IsSuccess)
            {
                return DaoResult<DataRow>.Failure(allOperationsResult.ErrorMessage, allOperationsResult.Exception);
            }

            var table = allOperationsResult.Data!;
            var rows = table.Select($"Operation = '{operationNumber.Replace("'", "''")}'");
            
            if (rows.Length > 0)
            {
                return DaoResult<DataRow>.Success(rows[0], $"Found operation {operationNumber}");
            }

            return DaoResult<DataRow>.Failure($"Operation {operationNumber} not found");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            return DaoResult<DataRow>.Failure($"Error retrieving operation {operationNumber}", ex);
        }
    }

    internal static async Task<DaoResult<bool>> OperationExists(string operationNumber, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["p_Operation"] = operationNumber };
            var result = await HelperDatabaseCore.ExecuteScalar(
                "md_operation_numbers_Exists_ByOperation",
                parameters, useAsync, CommandType.StoredProcedure);
                
            bool exists = Convert.ToInt32(result) > 0;
            return DaoResult<bool>.Success(exists, exists ? $"Operation {operationNumber} exists" : $"Operation {operationNumber} does not exist");
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
            return DaoResult<bool>.Failure($"Database error checking operation {operationNumber}", ex);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
            return DaoResult<bool>.Failure($"Error checking operation {operationNumber}", ex);
        }
    }

    #endregion
}

#endregion
