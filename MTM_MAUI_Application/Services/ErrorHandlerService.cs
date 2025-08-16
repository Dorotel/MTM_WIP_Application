namespace MTM_MAUI_Application.Services;

/// <summary>
/// Interface for error handling services
/// </summary>
public interface IErrorHandlerService
{
    Task HandleExceptionAsync(Exception exception, string context = "");
    Task HandleDatabaseErrorAsync(Exception exception, string context = "");
    Task ShowErrorAsync(string title, string message);
    Task ShowWarningAsync(string title, string message);
    Task ShowInfoAsync(string title, string message);
    Task<bool> ShowConfirmationAsync(string title, string message);
}

/// <summary>
/// Error handler service implementation
/// </summary>
public class ErrorHandlerService : IErrorHandlerService
{
    private readonly ILoggingService _logging;

    public ErrorHandlerService(ILoggingService logging)
    {
        _logging = logging;
    }

    public async Task HandleExceptionAsync(Exception exception, string context = "")
    {
        await _logging.LogErrorAsync($"Unhandled exception in {context}", exception);
        
        var message = GetUserFriendlyMessage(exception);
        await ShowErrorAsync("Error", message);
    }

    public async Task HandleDatabaseErrorAsync(Exception exception, string context = "")
    {
        await _logging.LogErrorAsync($"Database error in {context}", exception);
        
        var message = "A database error occurred. Please check your connection and try again.";
        if (exception.Message.Contains("connection", StringComparison.OrdinalIgnoreCase))
        {
            message = "Unable to connect to the database. Please check your network connection.";
        }
        
        await ShowErrorAsync("Database Error", message);
    }

    public async Task ShowErrorAsync(string title, string message)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
            }
        });
    }

    public async Task ShowWarningAsync(string title, string message)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
            }
        });
    }

    public async Task ShowInfoAsync(string title, string message)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
            }
        });
    }

    public async Task<bool> ShowConfirmationAsync(string title, string message)
    {
        return await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (Application.Current?.MainPage != null)
            {
                return await Application.Current.MainPage.DisplayAlert(title, message, "Yes", "No");
            }
            return false;
        });
    }

    private string GetUserFriendlyMessage(Exception exception)
    {
        return exception switch
        {
            UnauthorizedAccessException => "You don't have permission to perform this action.",
            TimeoutException => "The operation timed out. Please try again.",
            ArgumentException => "Invalid input provided. Please check your data and try again.",
            InvalidOperationException => "This operation cannot be performed at this time.",
            _ => "An unexpected error occurred. Please try again or contact support if the problem persists."
        };
    }
}