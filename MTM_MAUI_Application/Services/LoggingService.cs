namespace MTM_MAUI_Application.Services;

/// <summary>
/// Interface for logging services
/// </summary>
public interface ILoggingService
{
    Task LogInfoAsync(string message, object? data = null);
    Task LogWarningAsync(string message, object? data = null);
    Task LogErrorAsync(string message, Exception? exception = null, object? data = null);
    Task LogDebugAsync(string message, object? data = null);
}

/// <summary>
/// Logging service implementation
/// </summary>
public class LoggingService : ILoggingService
{
    private readonly ILogger<LoggingService> _logger;

    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger;
    }

    public Task LogInfoAsync(string message, object? data = null)
    {
        _logger.LogInformation("{Message} {Data}", message, data);
        return Task.CompletedTask;
    }

    public Task LogWarningAsync(string message, object? data = null)
    {
        _logger.LogWarning("{Message} {Data}", message, data);
        return Task.CompletedTask;
    }

    public Task LogErrorAsync(string message, Exception? exception = null, object? data = null)
    {
        _logger.LogError(exception, "{Message} {Data}", message, data);
        return Task.CompletedTask;
    }

    public Task LogDebugAsync(string message, object? data = null)
    {
        _logger.LogDebug("{Message} {Data}", message, data);
        return Task.CompletedTask;
    }
}