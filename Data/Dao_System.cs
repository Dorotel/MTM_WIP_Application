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

    internal static async Task SetUserAccessTypeAsync(string userName, string accessType, bool useAsync = false)
    {
        try
        {
            // Get UserID from usr_users
            var userIdObj = await HelperDatabaseCore.ExecuteScalar(
                "SELECT `ID` FROM `usr_users` WHERE `User` = @userName",
                new Dictionary<string, object> { ["@userName"] = userName }, useAsync);

            if (userIdObj != null)
            {
                var userId = Convert.ToInt32(userIdObj);

                // Get RoleID for the intended role
                var roleName = accessType == "Admin" ? "Admin" : "ReadOnly";
                var roleIdObj = await HelperDatabaseCore.ExecuteScalar(
                    "SELECT `ID` FROM `sys_roles` WHERE `RoleName` = @roleName",
                    new Dictionary<string, object> { ["@roleName"] = roleName }, useAsync);

                if (roleIdObj != null)
                {
                    var roleId = Convert.ToInt32(roleIdObj);

                    // Remove all existing roles for this user
                    await HelperDatabaseCore.ExecuteNonQuery(
                        "DELETE FROM `sys_user_roles` WHERE `UserID` = @userId",
                        new Dictionary<string, object> { ["@userId"] = userId }, useAsync);

                    // Assign the new role
                    await HelperDatabaseCore.ExecuteNonQuery(
                        "INSERT INTO `sys_user_roles` (`UserID`, `RoleID`, `AssignedBy`) VALUES (@userId, @roleId, @assignedBy)",
                        new Dictionary<string, object>
                        {
                            ["@userId"] = userId,
                            ["@roleId"] = roleId,
                            ["@assignedBy"] = Model_AppVariables.User
                        },
                        useAsync);

                    if (Model_AppVariables.User == userName)
                    {
                        Model_AppVariables.UserTypeAdmin = accessType == "Admin";
                        Model_AppVariables.UserTypeReadOnly = accessType == "ReadOnly";
                    }
                }
                else
                {
                    throw new Exception("Role not found.");
                }
            }
            else
            {
                throw new Exception("User not found.");
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await HandleSystemDaoExceptionAsync(ex, "SetUserAccessType", useAsync);
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

    internal static async Task<List<Model_Users>> System_UserAccessTypeAsync(bool useAsync = false)
    {
        var user = Model_AppVariables.User;
        var result = new List<Model_Users>();
        try
        {
            Model_AppVariables.UserTypeAdmin = false;
            Model_AppVariables.UserTypeReadOnly = false;

            var sql =
                @"SELECT u.ID, u.User, r.RoleName FROM usr_users u
                  JOIN sys_user_roles ur ON u.ID = ur.UserID
                  JOIN sys_roles r ON ur.RoleID = r.ID";

            using var reader = useAsync
                ? await HelperDatabaseCore.ExecuteReader(sql, useAsync: true)
                : HelperDatabaseCore.ExecuteReader(sql).Result;

            while (reader.Read())
            {
                var u = new Model_Users
                {
                    Id = reader.GetInt32(0),
                    User = reader.GetString(1)
                    // Add more fields as necessary
                };

                var roleName = reader.GetString(2);
                if (u.User == user)
                {
                    if (roleName == "Admin")
                        Model_AppVariables.UserTypeAdmin = true;
                    if (roleName == "ReadOnly")
                        Model_AppVariables.UserTypeReadOnly = true;
                }

                result.Add(u);
            }

            LoggingUtility.Log($"System_UserAccessType executed successfully for user: {user}");
            return result;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await HandleSystemDaoExceptionAsync(ex, "System_UserAccessType", useAsync);
            return [];
        }
    }

    #endregion

    #region Helpers

    private static async Task HandleSystemDaoExceptionAsync(Exception ex, string method, bool useAsync)
    {
        LoggingUtility.LogApplicationError(new Exception($"Error in {method}: {ex.Message}", ex));
        if (ex is MySqlException)
            await Dao_ErrorLog.HandleException_SQLError_CloseApp(ex, useAsync);
        else
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync);
    }

    #endregion
}

#endregion