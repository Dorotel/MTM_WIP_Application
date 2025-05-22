using MTM_WIP_Application.Core;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_WIP_Application.Models;

namespace MTM_WIP_Application.Data;

internal static class SystemDao
{
    /// <summary>
    /// Gets the current user name, normalizing and updating WipAppVariables.User.
    /// </summary>
    internal static string System_GetUserName()
    {
        var userIdWithDomain = Program.enteredUser == "Default User"
            ? WindowsIdentity.GetCurrent().Name
            : Program.enteredUser ?? throw new InvalidOperationException("User identity could not be retrieved.");

        var posSlash = userIdWithDomain.IndexOf('\\');
        var user = (posSlash == -1 ? userIdWithDomain : userIdWithDomain[(posSlash + 1)..]).ToUpper();
        WipAppVariables.User = user;
        return user;
    }

    /// <summary>
    /// Checks and sets the current user's admin and read-only access types.
    /// </summary>
    internal static async Task<List<Users>> System_UserAccessTypeAsync(bool useAsync = false)
    {
        var user = WipAppVariables.User;
        var result = new List<Users>();
        try
        {
            WipAppVariables.userTypeAdmin = false;
            WipAppVariables.userTypeReadOnly = false;

            // Check admin access
            await foreach (var admin in GetUserAccessListAsync("leads", useAsync))
            {
                if (admin.User == user) WipAppVariables.userTypeAdmin = true;
                result.Add(admin);
            }

            // Check read-only access
            await foreach (var readOnly in GetUserAccessListAsync("readonly", useAsync))
            {
                if (readOnly.User == user) WipAppVariables.userTypeReadOnly = true;
                result.Add(readOnly);
            }

            AppLogger.Log($"System_UserAccessType executed successfully for user: {user}");
            return result;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await HandleSystemDaoExceptionAsync(ex, "System_UserAccessType", useAsync);
            return new List<Users>();
        }
    }

    private static async IAsyncEnumerable<Users> GetUserAccessListAsync(string table, bool useAsync)
    {
        using var reader = useAsync
            ? await SqlHelper.ExecuteReader($"SELECT * FROM `{table}`", useAsync: true)
            : SqlHelper.ExecuteReader($"SELECT * FROM `{table}`").Result;
        while (reader.Read())
            yield return new Users
            {
                Id = reader.GetInt32(0),
                User = reader.GetString(1)
                // Other Users properties can be set here if needed and available in the table
            };
    }

    /// <summary>
    /// Sets the user's access type (Admin or ReadOnly) in the database and updates WipAppVariables.
    /// </summary>
    internal static async Task SetUserAccessTypeAsync(string email, string accessType, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@Email"] = email };
            await SqlHelper.ExecuteNonQuery("DELETE FROM `leads` WHERE `User` = @Email", parameters,
                useAsync: useAsync);
            await SqlHelper.ExecuteNonQuery("DELETE FROM `readonly` WHERE `User` = @Email", parameters,
                useAsync: useAsync);

            if (accessType == "Admin")
                await SqlHelper.ExecuteNonQuery("INSERT INTO `leads` (`User`) VALUES (@Email)", parameters,
                    useAsync: useAsync);
            else if (accessType == "ReadOnly")
                await SqlHelper.ExecuteNonQuery("INSERT INTO `readonly` (`User`) VALUES (@Email)", parameters,
                    useAsync: useAsync);

            if (WipAppVariables.User == email)
            {
                WipAppVariables.userTypeAdmin = accessType == "Admin";
                WipAppVariables.userTypeReadOnly = accessType == "ReadOnly";
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await HandleSystemDaoExceptionAsync(ex, "SetUserAccessType", useAsync);
        }
    }

    /// <summary>
    /// Updates the last 10 transactions table and updates the main form status.
    /// </summary>
    internal static async Task System_Last10_Buttons_ChangedAsync(bool useAsync = false)
    {
        try
        {
            var com1 = "INSERT INTO `last_10_transactions`(`ID`, `PartID`, `Op`, `Quantity`) VALUES(11, '" +
                       WipAppVariables.partId + "', '" + WipAppVariables.Operation + "', '" +
                       WipAppVariables.InventoryQuantity + "');";
            var com2 = "DELETE FROM `last_10_transactions` WHERE `ID` = 10;";
            var com3 = "ALTER TABLE  `mtm database`.`last_10_transactions` MODIFY COLUMN `ID` INT;";
            var com4 = "ALTER TABLE  `mtm database`.`last_10_transactions` DROP PRIMARY KEY;";
            var com5 =
                "UPDATE `mtm database`.`last_10_transactions` SET `mtm database`.`last_10_transactions`.`ID` = `ID` +1 LIMIT 9;";
            var com6 = "ALTER TABLE  `mtm database`.`last_10_transactions` ADD PRIMARY KEY(id);";
            var com7 = "ALTER TABLE  `mtm database`.`last_10_transactions` MODIFY COLUMN `ID` INT AUTO_INCREMENT;";
            var com8 = "UPDATE `last_10_transactions` SET `ID`= 1 WHERE `ID` = 11;";

            await SqlHelper.ExecuteNonQuery(com1 + com2 + com3 + com4 + com5 + com6 + com7 + com8, useAsync: useAsync);

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
                mainForm.Invoke(() =>
                {
                    mainForm.MainForm_StatusStrip_Disconnected.Visible = false;
                    mainForm.MainForm_StatusStrip_SavedStatus.Visible = true;
                    mainForm.Enabled = true;
                });
            }
        }
    }

    // --- Helper for consistent error handling ---
    private static async Task HandleSystemDaoExceptionAsync(Exception ex, string method, bool useAsync)
    {
        AppLogger.LogApplicationError(new Exception($"Error in {method}: {ex.Message}", ex));
        if (ex is MySqlException)
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        else
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
    }
}