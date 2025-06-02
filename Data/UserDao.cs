using System.Data;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Data;

internal static class UserDao
{
    internal static async Task AddUserToMySqlServer(string email, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@Email"] = email };
        var count = await ExecuteScalarIntSafe("SELECT COUNT(*) FROM mysql.user WHERE User = @Email AND Host = '%'",
            parameters, useAsync);
        if (count == 0)
        {
            var createUserSql =
                $"CREATE USER '{email}'@'%' IDENTIFIED WITH mysql_native_password AS ''; " +
                $"GRANT ALL PRIVILEGES ON *.* TO '{email}'@'%' REQUIRE NONE WITH GRANT OPTION " +
                $"MAX_QUERIES_PER_HOUR 0 MAX_CONNECTIONS_PER_HOUR 0 MAX_UPDATES_PER_HOUR 0 MAX_USER_CONNECTIONS 0;";
            await ExecuteNonQuerySafe(createUserSql, null, useAsync);
        }
    }

    internal static async Task<string?> GetUserFullNameAsync(string email, bool useAsync = false)
    {
        var row = await GetUserByEmail(email, useAsync);
        return row?["Full Name"] as string;
    }

    internal static async Task ChangeUserPin(string email, string newPin, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@Email"] = email,
            ["@Pin"] = newPin
        };
        await ExecuteNonQuerySafe("UPDATE `users` SET `Pin` = @Pin WHERE `User` = @Email", parameters, useAsync);

        if (WipAppVariables.User == email)
            WipAppVariables.UserPin = newPin;
    }

    internal static async Task DeleteUser(string email, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@Email"] = email };
        await ExecuteNonQuerySafe("DELETE FROM `users` WHERE `User` = @Email", parameters, useAsync);
    }

    // --- Private helpers for error handling and DRY ---

    private static async Task<DataTable> ExecuteDataTableSafe(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            return await SqlHelper.ExecuteDataTable(sql, parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return new DataTable();
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return new DataTable();
        }
    }

    private static async Task ExecuteNonQuerySafe(string sql, Dictionary<string, object>? parameters, bool useAsync)
    {
        try
        {
            await SqlHelper.ExecuteNonQuery(sql, parameters, useAsync: useAsync);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
        }
    }

    private static async Task<bool> ExecuteScalarBoolSafe(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            var result = await SqlHelper.ExecuteScalar(sql, parameters, useAsync: useAsync);
            return Convert.ToInt32(result) > 0;
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return false;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return false;
        }
    }

    private static async Task<int> ExecuteScalarIntSafe(string sql, Dictionary<string, object>? parameters,
        bool useAsync)
    {
        try
        {
            var result = await SqlHelper.ExecuteScalar(sql, parameters, useAsync: useAsync);
            return Convert.ToInt32(result);
        }
        catch (MySqlException ex)
        {
            AppLogger.LogDatabaseError(ex);
            await ErrorLogDao.HandleException_SQLError_CloseApp(ex, useAsync);
            return 0;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, useAsync);
            return 0;
        }
    }

    internal static async Task<DataTable> GetAdmins(bool useAsync = false)
    {
        return await ExecuteDataTableSafe("SELECT `User` FROM `leads`", null, useAsync);
    }

    internal static async Task<DataTable> GetAllUsers(bool useAsync = false)
    {
        return await ExecuteDataTableSafe("SELECT * FROM `users`", null, useAsync);
    }

    internal static async Task<DataTable> GetReadOnlyUsers(bool useAsync = false)
    {
        return await ExecuteDataTableSafe("SELECT `User` FROM `readonly`", null, useAsync);
    }

    internal static async Task<DataRow?> GetUserByEmail(string email, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@Email"] = email };
        var table = await ExecuteDataTableSafe("SELECT * FROM `users` WHERE `User` = @Email", parameters, useAsync);
        return table.Rows.Count > 0 ? table.Rows[0] : null;
    }

    internal static async Task<DataTable> GetVitsUsers(bool useAsync = false)
    {
        // Use a parameterized query for consistency
        var parameters = new Dictionary<string, object>
        {
            ["@IsVitsUser"] = true
        };
        return await ExecuteDataTableSafe(
            "SELECT `User` FROM `users` WHERE `VitsUser` = @IsVitsUser ORDER BY `User` ASC",
            parameters, useAsync);
    }

    internal static async Task InsertUser(string email, string fullName, string shift, string isVitsUser, string? pin,
        bool useAsync = false)
    {
        // Convert isVitsUser string to boolean first
        var vitsUserBool = isVitsUser.ToUpper() == "TRUE";

        var parameters = new Dictionary<string, object>
        {
            ["@Email"] = email,
            ["@FullName"] = fullName,
            ["@Shift"] = shift,
            ["@IsVitsUser"] = vitsUserBool, // Now passing a boolean value
            ["@Pin"] = pin ?? (object)DBNull.Value
        };
        await ExecuteNonQuerySafe(
            "INSERT INTO `users` (`User`, `Full Name`, `Shift`, `VitsUser`, `Pin`) VALUES (@Email, @FullName, @Shift, @IsVitsUser, @Pin)",
            parameters, useAsync);

        if (WipAppVariables.User == email) WipAppVariables.UserShift = shift;
    }

    internal static async Task SetUserAdminStatus(string email, bool isAdmin, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@Email"] = email };
        if (isAdmin)
        {
            var count = await ExecuteScalarIntSafe("SELECT COUNT(*) FROM `leads` WHERE `User` = @Email", parameters,
                useAsync);
            if (count == 0)
                await ExecuteNonQuerySafe("INSERT INTO `leads` (`User`) VALUES (@Email)", parameters, useAsync);
        }
        else
        {
            await ExecuteNonQuerySafe("DELETE FROM `leads` WHERE `User` = @Email", parameters, useAsync);
        }

        if (WipAppVariables.User == email)
            WipAppVariables.UserTypeAdmin = isAdmin;
    }

    internal static async Task SetUserReadOnlyStatus(string email, bool isReadOnly, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@Email"] = email };
        if (isReadOnly)
        {
            var count = await ExecuteScalarIntSafe("SELECT COUNT(*) FROM `readonly` WHERE `User` = @Email", parameters,
                useAsync);
            if (count == 0)
                await ExecuteNonQuerySafe("INSERT INTO `readonly` (`User`) VALUES (@Email)", parameters, useAsync);
        }
        else
        {
            await ExecuteNonQuerySafe("DELETE FROM `readonly` WHERE `User` = @Email", parameters, useAsync);
        }

        if (WipAppVariables.User == email)
            WipAppVariables.UserTypeReadOnly = isReadOnly;
    }

    internal static async Task SetUserShift(string email, string shift, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@Email"] = email,
            ["@Shift"] = shift
        };
        await ExecuteNonQuerySafe("UPDATE `users` SET `Shift` = @Shift WHERE `User` = @Email", parameters, useAsync);

        if (WipAppVariables.User == email)
            WipAppVariables.UserShift = shift;
    }

    internal static async Task UpdateUser(string email, string fullName, string shift, string isVitsUser, string? pin,
        bool useAsync = false)
    {
        // Convert isVitsUser string to boolean first
        var vitsUserBool = isVitsUser.ToUpper() == "TRUE";

        var parameters = new Dictionary<string, object>
        {
            ["@Email"] = email,
            ["@FullName"] = fullName,
            ["@Shift"] = shift,
            ["@IsVitsUser"] = vitsUserBool, // Now passing a boolean value
            ["@Pin"] = pin ?? (object)DBNull.Value
        };
        await ExecuteNonQuerySafe(
            "UPDATE `users` SET `Full Name` = @FullName, `Shift` = @Shift, `VitsUser` = @IsVitsUser, `Pin` = @Pin WHERE `User` = @Email",
            parameters, useAsync);

        if (WipAppVariables.User == email)
            WipAppVariables.UserShift = shift;
    }

    internal static async Task<bool> UserExists(string email, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object> { ["@Email"] = email };
        return await ExecuteScalarBoolSafe(
            "SELECT COUNT(*) FROM `users` WHERE `User` = @Email",
            parameters, useAsync);
    }

    internal static async Task<bool> ValidateUserPin(string username, string pin, bool useAsync = false)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@username"] = username,
            ["@pin"] = pin
        };
        return await ExecuteScalarBoolSafe(
            "SELECT COUNT(*) FROM `users` WHERE `User` = @username AND `PIN` = @pin",
            parameters, useAsync);
    }
}