using System.Security.Principal;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Models;
using MySql.Data.MySqlClient;
using System.Data;
using MTM_WIP_Application.Helpers;

namespace MTM_WIP_Application.Data;

#region Dao_System

internal static class Dao_System
{
    #region Fields

    public static Helper_MySql HelperMySql =
        new(Helper_SqlVariables.GetConnectionString(
            Core_WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            Core_WipAppVariables.User,
            Core_WipAppVariables.UserPin
        ));

    #endregion

    #region User Roles / Access

    internal static async Task SetUserAccessTypeAsync(string userName, string accessType, bool useAsync = false)
    {
        try
        {
            // Get UserID from usr_users
            var userIdObj = await HelperMySql.ExecuteScalar(
                "SELECT `ID` FROM `usr_users` WHERE `User` = @userName",
                new Dictionary<string, object> { ["@userName"] = userName }, useAsync);

            if (userIdObj != null)
            {
                var userId = Convert.ToInt32(userIdObj);

                // Get RoleID for the intended role
                var roleName = accessType == "Admin" ? "Admin" : "ReadOnly";
                var roleIdObj = await HelperMySql.ExecuteScalar(
                    "SELECT `ID` FROM `sys_roles` WHERE `RoleName` = @roleName",
                    new Dictionary<string, object> { ["@roleName"] = roleName }, useAsync);

                if (roleIdObj != null)
                {
                    var roleId = Convert.ToInt32(roleIdObj);

                    // Remove all existing roles for this user
                    await HelperMySql.ExecuteNonQuery(
                        "DELETE FROM `sys_user_roles` WHERE `UserID` = @userId",
                        new Dictionary<string, object> { ["@userId"] = userId }, useAsync);

                    // Assign the new role
                    await HelperMySql.ExecuteNonQuery(
                        "INSERT INTO `sys_user_roles` (`UserID`, `RoleID`, `AssignedBy`) VALUES (@userId, @roleId, @assignedBy)",
                        new Dictionary<string, object>
                        {
                            ["@userId"] = userId,
                            ["@roleId"] = roleId,
                            ["@assignedBy"] = Core_WipAppVariables.User
                        },
                        useAsync);

                    if (Core_WipAppVariables.User == userName)
                    {
                        Core_WipAppVariables.UserTypeAdmin = accessType == "Admin";
                        Core_WipAppVariables.UserTypeReadOnly = accessType == "ReadOnly";
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
        var userIdWithDomain = Core_WipAppVariables.EnteredUser == "Default User"
            ? WindowsIdentity.GetCurrent().Name
            : Core_WipAppVariables.EnteredUser ??
              throw new InvalidOperationException("User identity could not be retrieved.");

        var posSlash = userIdWithDomain.IndexOf('\\');
        var user = (posSlash == -1 ? userIdWithDomain : userIdWithDomain[(posSlash + 1)..]).ToUpper();
        Core_WipAppVariables.User = user;
        return user;
    }

    internal static async Task<List<Model_Users>> System_UserAccessTypeAsync(bool useAsync = false)
    {
        var user = Core_WipAppVariables.User;
        var result = new List<Model_Users>();
        try
        {
            Core_WipAppVariables.UserTypeAdmin = false;
            Core_WipAppVariables.UserTypeReadOnly = false;

            var sql =
                @"SELECT u.ID, u.User, r.RoleName FROM usr_users u
                  JOIN sys_user_roles ur ON u.ID = ur.UserID
                  JOIN sys_roles r ON ur.RoleID = r.ID";

            using var reader = useAsync
                ? await HelperMySql.ExecuteReader(sql, useAsync: true)
                : HelperMySql.ExecuteReader(sql).Result;

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
                        Core_WipAppVariables.UserTypeAdmin = true;
                    if (roleName == "ReadOnly")
                        Core_WipAppVariables.UserTypeReadOnly = true;
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