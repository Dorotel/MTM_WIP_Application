using System.Security.Principal;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MTM_WIP_Application.Data;

internal static class SystemDao
{
    public static SqlHelper SqlHelper =
        new(SqlVariables.GetConnectionString(
            WipAppVariables.WipServerAddress,
            "mtm_wip_application",
            WipAppVariables.User,
            WipAppVariables.UserPin
        ));

    // --- Helper for consistent error handling ---
    private static async Task HandleSystemDaoExceptionAsync(Exception ex, string method, bool useAsync)
    {
        AppLogger.LogApplicationError(new Exception($"Error in {method}: {ex.Message}", ex));
        if (ex is MySqlException)
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        else
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
    }

    // --- User Roles / Access (based on new sys_user_roles and sys_roles) ---

    /// <summary>
    ///     Assigns a role to a user (Admin or ReadOnly) in sys_user_roles.
    ///     Removes all existing roles for the user first.
    /// </summary>
    internal static async Task SetUserAccessTypeAsync(string userName, string accessType, bool useAsync = false)
    {
        try
        {
            // Get UserID from usr_users
            var userIdObj = await SqlHelper.ExecuteScalar(
                "SELECT `ID` FROM `usr_users` WHERE `User` = @userName",
                new Dictionary<string, object> { ["@userName"] = userName }, useAsync);

            if (userIdObj == null) throw new Exception("User not found.");

            var userId = Convert.ToInt32(userIdObj);

            // Get RoleID for the intended role
            var roleName = accessType == "Admin" ? "Admin" : "ReadOnly";
            var roleIdObj = await SqlHelper.ExecuteScalar(
                "SELECT `ID` FROM `sys_roles` WHERE `RoleName` = @roleName",
                new Dictionary<string, object> { ["@roleName"] = roleName }, useAsync);

            if (roleIdObj == null) throw new Exception("Role not found.");

            var roleId = Convert.ToInt32(roleIdObj);

            // Remove all existing roles for this user
            await SqlHelper.ExecuteNonQuery(
                "DELETE FROM `sys_user_roles` WHERE `UserID` = @userId",
                new Dictionary<string, object> { ["@userId"] = userId }, useAsync);

            // Assign the new role
            await SqlHelper.ExecuteNonQuery(
                "INSERT INTO `sys_user_roles` (`UserID`, `RoleID`, `AssignedBy`) VALUES (@userId, @roleId, @assignedBy)",
                new Dictionary<string, object>
                {
                    ["@userId"] = userId,
                    ["@roleId"] = roleId,
                    ["@assignedBy"] = WipAppVariables.User
                },
                useAsync);

            if (WipAppVariables.User == userName)
            {
                WipAppVariables.UserTypeAdmin = accessType == "Admin";
                WipAppVariables.UserTypeReadOnly = accessType == "ReadOnly";
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await HandleSystemDaoExceptionAsync(ex, "SetUserAccessType", useAsync);
        }
    }

    /// <summary>
    ///     Gets the current user name, normalizing and updating WipAppVariables.User.
    /// </summary>
    internal static string System_GetUserName()
    {
        var userIdWithDomain = WipAppVariables.EnteredUser == "Default User"
            ? WindowsIdentity.GetCurrent().Name
            : WipAppVariables.EnteredUser ??
              throw new InvalidOperationException("User identity could not be retrieved.");

        var posSlash = userIdWithDomain.IndexOf('\\');
        var user = (posSlash == -1 ? userIdWithDomain : userIdWithDomain[(posSlash + 1)..]).ToUpper();
        WipAppVariables.User = user;
        return user;
    }

    /// <summary>
    ///     Updates the sys_last_10_transacitons table and updates the main form status.
    ///     (This version uses only standard DML; schema-altering queries are removed)
    /// </summary>
    internal static async Task System_Last10_Buttons_ChangedAsync(bool useAsync = false)
    {
        try
        {
            // Insert the new transaction
            await SqlHelper.ExecuteNonQuery(
                @"INSERT INTO `sys_last_10_transacitons` (`User`, `PartID`, `Operation`, `Quantity`, `DateTime`)
                  VALUES (@user, @partId, @operation, @quantity, NOW());",
                new Dictionary<string, object>
                {
                    ["@user"] = WipAppVariables.User,
                    ["@partId"] = WipAppVariables.PartId,
                    ["@operation"] = WipAppVariables.Operation,
                    ["@quantity"] = WipAppVariables.InventoryQuantity
                },
                useAsync);

            // Remove oldest if there are more than 10 records for this user
            await SqlHelper.ExecuteNonQuery(
                @"DELETE FROM `sys_last_10_transacitons`
                  WHERE `ID` NOT IN (
                      SELECT ID FROM (
                          SELECT ID FROM `sys_last_10_transacitons`
                          WHERE `User` = @user
                          ORDER BY `DateTime` DESC
                          LIMIT 10
                      ) AS t
                  ) AND `User` = @user;",
                new Dictionary<string, object>
                {
                    ["@user"] = WipAppVariables.User
                },
                useAsync);

            AppLogger.Log("System_Last10_Buttons_Changed executed successfully.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await HandleSystemDaoExceptionAsync(ex, "System_Last10_Buttons_Changed", useAsync);
        }
        finally
        {
            if (Application.OpenForms.OfType<MainForm>().Any())
            {
                var mainForm = Application.OpenForms.OfType<MainForm>().First();
                mainForm.Invoke(() => { mainForm.Enabled = true; });
            }
        }
    }

    /// <summary>
    ///     Gets all users and their roles, and sets current user's admin/read-only flags.
    /// </summary>
    internal static async Task<List<Users>> System_UserAccessTypeAsync(bool useAsync = false)
    {
        var user = WipAppVariables.User;
        var result = new List<Users>();
        try
        {
            WipAppVariables.UserTypeAdmin = false;
            WipAppVariables.UserTypeReadOnly = false;

            var sql =
                @"SELECT u.ID, u.User, r.RoleName FROM usr_users u
                  JOIN sys_user_roles ur ON u.ID = ur.UserID
                  JOIN sys_roles r ON ur.RoleID = r.ID";

            using var reader = useAsync
                ? await SqlHelper.ExecuteReader(sql, useAsync: true)
                : SqlHelper.ExecuteReader(sql).Result;

            while (reader.Read())
            {
                var u = new Users
                {
                    Id = reader.GetInt32(0),
                    User = reader.GetString(1)
                    // Add more fields as necessary
                };

                var roleName = reader.GetString(2);
                if (u.User == user)
                {
                    if (roleName == "Admin")
                        WipAppVariables.UserTypeAdmin = true;
                    if (roleName == "ReadOnly")
                        WipAppVariables.UserTypeReadOnly = true;
                }

                result.Add(u);
            }

            AppLogger.Log($"System_UserAccessType executed successfully for user: {user}");
            return result;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await HandleSystemDaoExceptionAsync(ex, "System_UserAccessType", useAsync);
            return [];
        }
    }
}