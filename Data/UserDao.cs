using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Data;

internal static class UserDao
{
    internal static async Task<DataTable> GetVitsUsers(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable(
                "SELECT `User` FROM `users` WHERE `VitsUser` = TRUE ORDER BY `User` ASC",
                useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
            return new DataTable();
        }
    }

    internal static async Task<bool> ValidateUserPin(string username, string pin, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@username"] = username,
                ["@pin"] = pin
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM `users` WHERE `User` = @username AND `PIN` = @pin",
                parameters,
                useAsync: useAsync);
            return Convert.ToInt32(result) > 0;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.LogErrorWithMethod(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return false;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.LogErrorWithMethod(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return false;
        }
    }

    internal static async Task<bool> UserExists(string email, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Email"] = email
            };
            var result = await SqlHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM `users` WHERE `User` = @Email",
                parameters,
                useAsync: useAsync);
            return Convert.ToInt32(result) > 0;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
            return false;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
            return false;
        }
    }

    internal static async Task InsertUser(string email, string fullName, string shift, string isVitsUser, string? pin,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Email"] = email,
                ["@FullName"] = fullName,
                ["@Shift"] = shift,
                ["@IsVitsUser"] = isVitsUser,
                ["@Pin"] = pin ?? (object)DBNull.Value
            };
            await SqlHelper.ExecuteNonQuery(
                "INSERT INTO `users` (`User`, `Full Name`, `Shift`, `VitsUser`, `Pin`) VALUES (@Email, @FullName, @Shift, @IsVitsUser, @Pin)",
                parameters,
                useAsync: useAsync);

            // Update WipAppVariables if the inserted user is the current user
            if (WipAppVariables.User == email)
            {
                WipAppVariables.PartType = null;
                WipAppVariables.UserShift = shift;
            }
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }

    internal static async Task UpdateUser(string email, string fullName, string shift, string isVitsUser, string? pin,
        bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Email"] = email,
                ["@FullName"] = fullName,
                ["@Shift"] = shift,
                ["@IsVitsUser"] = isVitsUser,
                ["@Pin"] = pin ?? (object)DBNull.Value
            };
            await SqlHelper.ExecuteNonQuery(
                "UPDATE `users` SET `Full Name` = @FullName, `Shift` = @Shift, `VitsUser` = @IsVitsUser, `Pin` = @Pin WHERE `User` = @Email",
                parameters,
                useAsync: useAsync);

            // Update WipAppVariables if the updated user is the current user
            if (WipAppVariables.User == email) WipAppVariables.UserShift = shift;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }

    internal static async Task SetUserAdminStatus(string email, bool isAdmin, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@Email"] = email };
            if (isAdmin)
            {
                var count = Convert.ToInt32(await SqlHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM `leads` WHERE `User` = @Email", parameters, useAsync: useAsync));
                if (count == 0)
                    await SqlHelper.ExecuteNonQuery(
                        "INSERT INTO `leads` (`User`) VALUES (@Email)", parameters, useAsync: useAsync);
            }
            else
            {
                await SqlHelper.ExecuteNonQuery(
                    "DELETE FROM `leads` WHERE `User` = @Email", parameters, useAsync: useAsync);
            }

            // Update WipAppVariables if the affected user is the current user
            if (WipAppVariables.User == email) WipAppVariables.userTypeAdmin = isAdmin;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }

    internal static async Task SetUserReadOnlyStatus(string email, bool isReadOnly, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@Email"] = email };
            if (isReadOnly)
            {
                var count = Convert.ToInt32(await SqlHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM `readonly` WHERE `User` = @Email", parameters, useAsync: useAsync));
                if (count == 0)
                    await SqlHelper.ExecuteNonQuery(
                        "INSERT INTO `readonly` (`User`) VALUES (@Email)", parameters, useAsync: useAsync);
            }
            else
            {
                await SqlHelper.ExecuteNonQuery(
                    "DELETE FROM `readonly` WHERE `User` = @Email", parameters, useAsync: useAsync);
            }

            // Update WipAppVariables if the affected user is the current user
            if (WipAppVariables.User == email) WipAppVariables.userTypeReadOnly = isReadOnly;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }

    internal static async Task AddUserToMySqlServer(string email, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@Email"] = email };
            var count = Convert.ToInt32(await SqlHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM mysql.user WHERE User = @Email AND Host = '%'", parameters,
                useAsync: useAsync));
            if (count == 0)
            {
                // Note: This uses string interpolation for SQL, which is generally not recommended.
                // If you need to support dynamic user names, ensure proper sanitization.
                var createUserSql =
                    $"CREATE USER '{email}'@'%' IDENTIFIED WITH mysql_native_password AS ''; " +
                    $"GRANT ALL PRIVILEGES ON *.* TO '{email}'@'%' REQUIRE NONE WITH GRANT OPTION " +
                    $"MAX_QUERIES_PER_HOUR 0 MAX_CONNECTIONS_PER_HOUR 0 MAX_UPDATES_PER_HOUR 0 MAX_USER_CONNECTIONS 0;";
                await SqlHelper.ExecuteNonQuery(createUserSql, useAsync: useAsync);
            }
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }

    internal static async Task<DataRow?> GetUserByEmail(string email, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@Email"] = email };
            var table = await SqlHelper.ExecuteDataTable("SELECT * FROM `users` WHERE `User` = @Email", parameters,
                useAsync: useAsync);
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
            return null;
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
            return null;
        }
    }

    internal static async Task<DataTable> GetAllUsers(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable("SELECT * FROM `users`", useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
            return new DataTable();
        }
    }

    internal static async Task DeleteUser(string email, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object> { ["@Email"] = email };
            await SqlHelper.ExecuteNonQuery("DELETE FROM `users` WHERE `User` = @Email", parameters,
                useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }

    internal static async Task ChangeUserPin(string email, string newPin, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Email"] = email,
                ["@Pin"] = newPin
            };
            await SqlHelper.ExecuteNonQuery("UPDATE `users` SET `Pin` = @Pin WHERE `User` = @Email", parameters,
                useAsync: useAsync);

            // Optionally update WipAppVariables if you store the pin (not shown in WipAppVariables)
            if (WipAppVariables.User == email) WipAppVariables.UserPin = newPin;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }

    internal static async Task SetUserShift(string email, string shift, bool useAsync = false)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Email"] = email,
                ["@Shift"] = shift
            };
            await SqlHelper.ExecuteNonQuery("UPDATE `users` SET `Shift` = @Shift WHERE `User` = @Email", parameters,
                useAsync: useAsync);

            // Update WipAppVariables if the affected user is the current user
            if (WipAppVariables.User == email) WipAppVariables.UserShift = shift;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
        }
    }


    internal static async Task<DataTable> GetAdmins(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable("SELECT `User` FROM `leads`", useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
            return new DataTable();
        }
    }

    internal static async Task<DataTable> GetReadOnlyUsers(bool useAsync = false)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable("SELECT `User` FROM `readonly`", useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_SQLError_CloseApp(ex);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogDatabaseError(ex);
            ErrorLogDao.HandleException_GeneralError_CloseApp(ex);
            return new DataTable();
        }
    }
}