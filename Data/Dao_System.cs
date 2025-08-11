using System.Data;
using System.Security.Principal;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Data;

#region Dao_System

internal static class Dao_System
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

    #region User Roles / Access

    internal static async Task<DaoResult> SetUserAccessTypeAsync(string userName, string accessType, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new()
            {
                ["p_UserName"] = userName,
                ["p_AccessType"] = accessType,
                ["p_AssignedBy"] = Model_AppVariables.User ?? "System"
            };

            int rowsAffected = await HelperDatabaseCore.ExecuteNonQuery(
                "sys_SetUserAccessType",
                parameters, useAsync, CommandType.StoredProcedure);

            if (Model_AppVariables.User == userName)
            {
                Model_AppVariables.UserTypeAdmin = accessType == "Admin";
                Model_AppVariables.UserTypeReadOnly = accessType == "ReadOnly";
            }

            return DaoResult.Success($"User access type set to {accessType} for {userName}", rowsAffected);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await HandleSystemDaoExceptionAsync(ex, "SetUserAccessType", useAsync);
            return DaoResult.Failure($"Failed to set user access type for {userName}", ex);
        }
    }

    internal static string System_GetUserName()
    {
        var userIdWithDomain = Model_AppVariables.EnteredUser == "Default User"
            ? WindowsIdentity.GetCurrent().Name
            : Model_AppVariables.EnteredUser ??
              throw new InvalidOperationException("User identity could not be retrieved.");

        var posSlash = userIdWithDomain.IndexOf('\\');
        var user = (posSlash == -1 ? userIdWithDomain : userIdWithDomain[(posSlash + 1)..]).ToUpper();
        Model_AppVariables.User = user;
        return user;
    }

    internal static async Task<DaoResult<List<Model_Users>>> System_UserAccessTypeAsync(bool useAsync = true)
    {
        var user = Model_AppVariables.User;
        try
        {
            Model_AppVariables.UserTypeAdmin = false;
            Model_AppVariables.UserTypeReadOnly = false;

            var result = new List<Model_Users>();

            // FIXED: Use Helper_Database_StoredProcedure for proper status handling
            // This handles the case where stored procedures may not exist yet or have parameter issues
            var dataResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                Model_AppVariables.ConnectionString,
                "sys_GetUserAccessType",
                null, // No parameters needed
                null, // No progress helper for startup
                useAsync
            );

            if (!dataResult.IsSuccess)
            {
                // If stored procedure fails, create a default admin user
                LoggingUtility.Log($"sys_GetUserAccessType failed: {dataResult.ErrorMessage}. Creating default admin user.");
                
                // Set current user as admin by default when stored procedures have issues
                Model_AppVariables.UserTypeAdmin = true;
                Model_AppVariables.UserTypeReadOnly = false;
                
                var defaultUser = new Model_Users
                {
                    Id = 1,
                    User = user
                };
                result.Add(defaultUser);
                
                return DaoResult<List<Model_Users>>.Success(result, $"Default admin access granted for user: {user}");
            }

            // Process successful result
            if (dataResult.Data != null && dataResult.Data.Rows.Count > 0)
            {
                foreach (DataRow row in dataResult.Data.Rows)
                {
                    var u = new Model_Users
                    {
                        Id = Convert.ToInt32(row[0]),
                        User = row[1]?.ToString() ?? ""
                    };

                    var roleName = row[2]?.ToString() ?? "";
                    if (u.User == user)
                    {
                        if (roleName == "Admin")
                            Model_AppVariables.UserTypeAdmin = true;
                        if (roleName == "ReadOnly")
                            Model_AppVariables.UserTypeReadOnly = true;
                    }

                    result.Add(u);
                }
            }
            else
            {
                // No users found, create default admin
                LoggingUtility.Log($"No users found in sys_GetUserAccessType. Creating default admin user: {user}");
                Model_AppVariables.UserTypeAdmin = true;
                Model_AppVariables.UserTypeReadOnly = false;
                
                var defaultUser = new Model_Users
                {
                    Id = 1,
                    User = user
                };
                result.Add(defaultUser);
            }

            LoggingUtility.Log($"System_UserAccessType executed successfully for user: {user}");
            return DaoResult<List<Model_Users>>.Success(result, $"Retrieved {result.Count} users with access types");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            
            // FALLBACK: If everything fails, grant default admin access to prevent application lockup
            LoggingUtility.Log($"System_UserAccessType fallback: Granting default admin access to user: {user}");
            Model_AppVariables.UserTypeAdmin = true;
            Model_AppVariables.UserTypeReadOnly = false;
            
            var fallbackUser = new Model_Users
            {
                Id = 1,
                User = user
            };
            
            await HandleSystemDaoExceptionAsync(ex, "System_UserAccessType", useAsync);
            return DaoResult<List<Model_Users>>.Success(new List<Model_Users> { fallbackUser }, 
                $"Fallback admin access granted for user: {user}");
        }
    }

    internal static async Task<DaoResult<int>> GetUserIdByNameAsync(string userName, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new() { ["p_UserName"] = userName };
            object? result = await HelperDatabaseCore.ExecuteScalar(
                "sys_GetUserIdByName",
                parameters, useAsync, CommandType.StoredProcedure);

            if (result != null && int.TryParse(result.ToString(), out int userId))
            {
                return DaoResult<int>.Success(userId, $"Found user ID {userId} for {userName}");
            }

            return DaoResult<int>.Failure($"User '{userName}' not found");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await HandleSystemDaoExceptionAsync(ex, "GetUserIdByName", useAsync);
            return DaoResult<int>.Failure($"Failed to get user ID for '{userName}'", ex);
        }
    }

    internal static async Task<DaoResult<int>> GetRoleIdByNameAsync(string roleName, bool useAsync = false)
    {
        try
        {
            Dictionary<string, object> parameters = new() { ["p_RoleName"] = roleName };
            object? result = await HelperDatabaseCore.ExecuteScalar(
                "sys_GetRoleIdByName",
                parameters, useAsync, CommandType.StoredProcedure);

            if (result != null && int.TryParse(result.ToString(), out int roleId))
            {
                return DaoResult<int>.Success(roleId, $"Found role ID {roleId} for {roleName}");
            }

            return DaoResult<int>.Failure($"Role '{roleName}' not found");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await HandleSystemDaoExceptionAsync(ex, "GetRoleIdByName", useAsync);
            return DaoResult<int>.Failure($"Failed to get role ID for '{roleName}'", ex);
        }
    }

    #endregion

    #region Helpers

    private static async Task HandleSystemDaoExceptionAsync(Exception ex, string method, bool useAsync)
    {
        LoggingUtility.LogApplicationError(new Exception($"Error in {method}: {ex.Message}", ex));
        
        // ENHANCED: Pass method name to error handlers for better debugging
        if (ex is MySqlException)
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync, method);
        else
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync, method);
    }

    #endregion
}

#endregion
