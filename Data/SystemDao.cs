using MTM_WIP_Application.Core;
using MTM_WIP_Application.Forms.MainForm;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Data;

internal static class SystemDao
{
    // =====================
    // SystemDao
    // =====================


    internal static string System_GetUserName()
    {
        if (Program.enteredUser == "Default User")
        {
            var userIdWithDomain = WindowsIdentity.GetCurrent().Name;
            var posSlash = userIdWithDomain.IndexOf('\\');
            var user = (posSlash == -1 ? userIdWithDomain : userIdWithDomain[(posSlash + 1)..]).ToUpper();
            WipAppVariables.User = user; // Keep WipAppVariables in sync
            return user;
        }
        else
        {
            var userIdWithDomain = Program.enteredUser;
            if (userIdWithDomain == null) throw new InvalidOperationException("User identity could not be retrieved.");

            var posSlash = userIdWithDomain.IndexOf('\\');
            var user = (posSlash == -1 ? userIdWithDomain : userIdWithDomain[(posSlash + 1)..]).ToUpper();
            WipAppVariables.User = user; // Keep WipAppVariables in sync
            return user;
        }
    }

    internal static async Task<List<AdminList>> System_UserAccessTypeAsync(bool useAsync = false)
    {
        var user = WipAppVariables.User;
        List<AdminList> returnThese = [];
        try
        {
            // Reset before checking
            WipAppVariables.userTypeAdmin = false;
            WipAppVariables.userTypeReadOnly = false;

            // Admin check
            using (var reader = useAsync
                       ? await SqlHelper.ExecuteReader("SELECT * FROM `leads`", useAsync: true)
                       : SqlHelper.ExecuteReader("SELECT * FROM `leads`").Result)
            {
                while (reader.Read())
                {
                    AdminList a = new()
                    {
                        Id = reader.GetInt32(0),
                        User = reader.GetString(1)
                    };
                    if (a.User == user) WipAppVariables.userTypeAdmin = true;

                    returnThese.Add(a);
                }
            }

            // ReadOnly check
            using (var reader2 = useAsync
                       ? await SqlHelper.ExecuteReader("SELECT * FROM `readonly`", useAsync: true)
                       : SqlHelper.ExecuteReader("SELECT * FROM `readonly`").Result)
            {
                while (reader2.Read())
                {
                    AdminList a = new()
                    {
                        Id = reader2.GetInt32(0),
                        User = reader2.GetString(1)
                    };
                    if (a.User == user) WipAppVariables.userTypeReadOnly = true;

                    returnThese.Add(a);
                }
            }

            AppLogger.Log("System_UserAccessType executed successfully for user: " + user);
            return returnThese;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in System_UserAccessType: " + ex.Message);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return [];
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in System_UserAccessType: " + ex.Message);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return [];
        }
    }

    internal static async Task SetUserAccessTypeAsync(string email, string accessType, bool useAsync = false)
    {
        try
        {
            // Remove from both tables first
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

            // Update WipAppVariables if the affected user is the current user
            if (WipAppVariables.User == email)
            {
                WipAppVariables.userTypeAdmin = accessType == "Admin";
                WipAppVariables.userTypeReadOnly = accessType == "ReadOnly";
            }
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

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

            await SqlHelper.ExecuteNonQuery(com1 + com2 + com3 + com4 + com5 + com6 + com7 + com8,
                useAsync: useAsync);

            AppLogger.Log("System_Last10_Buttons_Changed executed successfully.");
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in System_Last10_Buttons_Changed: " + ex.Message);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            AppLogger.Log("Error in System_Last10_Buttons_Changed: " + ex.Message);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
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
}