using MTM_MAUI_Application.Models;
using System.Data;

namespace MTM_MAUI_Application.Services;

/// <summary>
/// User service implementation
/// </summary>
public class UserService : IUserService
{
    private readonly IDatabaseService _database;
    private readonly ILoggingService _logging;
    private User? _currentUser;

    public UserService(IDatabaseService database, ILoggingService logging)
    {
        _database = database;
        _logging = logging;
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        if (_currentUser == null)
        {
            // Get current Windows user as default
            var windowsUser = Environment.UserName;
            _currentUser = await GetUserByUsernameAsync(windowsUser);
            
            // If user doesn't exist in database, create default user
            _currentUser ??= new User
            {
                Username = windowsUser,
                FullName = windowsUser,
                ThemeName = "Default",
                ThemeFontSize = 12
            };
        }

        return _currentUser;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "User", username }
            };

            var dataTable = await _database.ExecuteStoredProcedureAsync("usr_users_Get_ByUser", parameters);
            
            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new User
                {
                    Username = row.Field<string>("User") ?? string.Empty,
                    FullName = row.Field<string>("Full Name") ?? string.Empty,
                    Shift = row.Field<string>("Shift") ?? string.Empty,
                    VitsUser = row.Field<bool>("VitsUser"),
                    Pin = row.Field<string>("Pin") ?? string.Empty,
                    LastShownVersion = row.Field<string>("LastShownVersion") ?? string.Empty,
                    HideChangeLog = row.Field<string>("HideChangeLog") == "True",
                    ThemeName = row.Field<string>("Theme_Name") ?? "Default",
                    ThemeFontSize = row.Field<int>("Theme_FontSize") ?? 12,
                    VisualUserName = row.Field<string>("VisualUserName") ?? string.Empty,
                    VisualPassword = row.Field<string>("VisualPassword") ?? string.Empty,
                    WipServerAddress = row.Field<string>("WipServerAddress") ?? string.Empty,
                    WipDatabase = row.Field<string>("WIPDatabase") ?? string.Empty,
                    WipServerPort = row.Field<string>("WipServerPort") ?? "3306"
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error getting user {username}", ex);
            return null;
        }
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            var dataTable = await _database.ExecuteStoredProcedureAsync("usr_users_Get_All");
            var users = new List<User>();

            foreach (DataRow row in dataTable.Rows)
            {
                users.Add(new User
                {
                    Username = row.Field<string>("User") ?? string.Empty,
                    FullName = row.Field<string>("Full Name") ?? string.Empty,
                    Shift = row.Field<string>("Shift") ?? string.Empty,
                    VitsUser = row.Field<bool>("VitsUser"),
                    ThemeName = row.Field<string>("Theme_Name") ?? "Default",
                    ThemeFontSize = row.Field<int>("Theme_FontSize") ?? 12
                });
            }

            return users;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync("Error getting all users", ex);
            return new List<User>();
        }
    }

    public Task<bool> SetCurrentUserAsync(string username)
    {
        _currentUser = null; // Clear cached user
        return Task.FromResult(true);
    }

    public async Task<bool> UpdateUserSettingsAsync(User user)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "User", user.Username },
                { "FullName", user.FullName },
                { "Shift", user.Shift },
                { "Pin", user.Pin },
                { "VisualUserName", user.VisualUserName },
                { "VisualPassword", user.VisualPassword }
            };

            await _database.ExecuteNonQueryAsync("usr_users_Update_User", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error updating user {user.Username}", ex);
            return false;
        }
    }
}