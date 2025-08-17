# Error Handling & Logging Migration Guide

## Overview
This section contains error handling and logging infrastructure that needs MAUI adaptation.

## Services (7-Error-Handling/Services)
Core error handling services requiring MAUI integration:

### Service_ErrorHandler.cs
Centralized error handling system that needs adaptation for MAUI dialogs:

#### Current WinForms Implementation:
```csharp
public class Service_ErrorHandler
{
    public static async Task HandleException(Exception ex, string context = "")
    {
        // Uses WinForms dialogs and forms
        using (var errorDialog = new EnhancedErrorDialog(ex, context))
        {
            errorDialog.ShowDialog();
        }
    }
}
```

#### MAUI Adaptation Required:
```csharp
public class Service_ErrorHandler
{
    private readonly IServiceProvider _serviceProvider;
    
    public Service_ErrorHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task HandleExceptionAsync(Exception ex, string context = "")
    {
        // Log to database (preserve existing logic)
        await Dao_ErrorLog.LogErrorToDatabaseAsync(ex, context);
        
        // Use MAUI DisplayAlert instead of WinForms dialog
        var page = Application.Current?.MainPage;
        if (page != null)
        {
            await page.DisplayAlert("Error", GetUserFriendlyMessage(ex), "OK");
        }
    }
    
    public async Task<bool> ShowConfirmationAsync(string title, string message)
    {
        var page = Application.Current?.MainPage;
        if (page != null)
        {
            return await page.DisplayAlert(title, message, "Yes", "No");
        }
        return false;
    }
    
    public async Task ShowWarningAsync(string message)
    {
        var page = Application.Current?.MainPage;
        if (page != null)
        {
            await page.DisplayAlert("Warning", message, "OK");
        }
    }
    
    public async Task ShowInformationAsync(string message)
    {
        var page = Application.Current?.MainPage;
        if (page != null)
        {
            await page.DisplayAlert("Information", message, "OK");
        }
    }
    
    // Preserve all existing error handling logic
    private string GetUserFriendlyMessage(Exception ex)
    {
        // Keep existing user-friendly message logic
        return ex switch
        {
            MySqlException sqlEx => GetDatabaseErrorMessage(sqlEx),
            UnauthorizedAccessException => "You don't have permission to perform this action.",
            FileNotFoundException => "Required file not found.",
            _ => "An unexpected error occurred. Please try again."
        };
    }
    
    // Keep all existing database error handling
    private string GetDatabaseErrorMessage(MySqlException ex)
    {
        // Preserve existing database error message logic
    }
}
```

## Logging (7-Error-Handling/Logging)
Logging infrastructure requiring cross-platform implementation:

### Current Logging System:
From LoggingUtility.cs - File-based logging with multiple log levels.

### MAUI Logging Adaptation:

#### 1. Platform-Specific Log Locations:
```csharp
public class LoggingService
{
    private string GetLogDirectory()
    {
        return DeviceInfo.Platform.ToString() switch
        {
            "Windows" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MTM_Inventory"),
            "Android" => Path.Combine(FileSystem.AppDataDirectory, "Logs"),
            "iOS" => Path.Combine(FileSystem.AppDataDirectory, "Logs"),
            "MacCatalyst" => Path.Combine(FileSystem.AppDataDirectory, "Logs"),
            _ => Path.Combine(FileSystem.AppDataDirectory, "Logs")
        };
    }
    
    public async Task LogAsync(LogLevel level, string message, Exception ex = null)
    {
        var logDirectory = GetLogDirectory();
        Directory.CreateDirectory(logDirectory);
        
        var logFile = Path.Combine(logDirectory, $"mtm_inventory_{DateTime.Now:yyyyMMdd}.log");
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
        
        if (ex != null)
        {
            logEntry += $"\nException: {ex}";
        }
        
        await File.AppendAllTextAsync(logFile, logEntry + Environment.NewLine);
    }
}
```

#### 2. Integration with Microsoft.Extensions.Logging:
```csharp
// In MauiProgram.cs
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    
    // Add logging
    builder.Logging.AddDebug();
    
#if DEBUG
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
#else
    builder.Logging.SetMinimumLevel(LogLevel.Information);
#endif
    
    // Register custom logging service
    builder.Services.AddSingleton<LoggingService>();
    
    return builder.Build();
}
```

#### 3. Enhanced Error Dialog for MAUI:
```csharp
public partial class ErrorPopup : Popup
{
    public ErrorPopup(Exception ex, string context)
    {
        InitializeComponent();
        
        // Populate error details
        ErrorMessage.Text = ex.Message;
        ErrorContext.Text = context;
        ErrorDetails.Text = ex.ToString();
        
        // Wire up button events
        CopyButton.Clicked += OnCopyClicked;
        ReportButton.Clicked += OnReportClicked;
        CloseButton.Clicked += OnCloseClicked;
    }
    
    private async void OnCopyClicked(object sender, EventArgs e)
    {
        await Clipboard.SetTextAsync(ErrorDetails.Text);
        await DisplayAlert("Copied", "Error details copied to clipboard", "OK");
    }
    
    private async void OnReportClicked(object sender, EventArgs e)
    {
        // Implementation for error reporting
        var email = new EmailMessage
        {
            Subject = "MTM Inventory Application Error Report",
            Body = ErrorDetails.Text,
            To = { "support@mtm.com" }
        };
        
        await Email.ComposeAsync(email);
    }
    
    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close();
    }
}
```

## Global Exception Handling

### MAUI Global Exception Handler:
```csharp
// In MauiProgram.cs or App.xaml.cs
public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        
        // Set up global exception handling
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }
    
    private async void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            var errorHandler = ServiceProvider.GetService<Service_ErrorHandler>();
            await errorHandler?.HandleExceptionAsync(ex, "Unhandled Exception");
        }
    }
    
    private async void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        var errorHandler = ServiceProvider.GetService<Service_ErrorHandler>();
        await errorHandler?.HandleExceptionAsync(e.Exception, "Unobserved Task Exception");
        e.SetObserved();
    }
}
```

## Database Error Handling

### Preserve Existing DAO Error Patterns:
```csharp
// Keep all existing DAO error handling patterns exactly as implemented
public class Dao_Inventory
{
    private readonly Service_ErrorHandler _errorHandler;
    private readonly LoggingService _logger;
    
    public Dao_Inventory(Service_ErrorHandler errorHandler, LoggingService logger)
    {
        _errorHandler = errorHandler;
        _logger = logger;
    }
    
    public async Task<DaoResult<List<Model_Inventory>>> GetInventoryByPartIdAsync(string partId)
    {
        try
        {
            // Existing database logic preserved exactly
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_PartID", partId)
            };
            
            var result = await Helper_Database_StoredProcedure
                .ExecuteDataTableWithStatus("inv_inventory_GetByPartID", parameters);
                
            return new DaoResult<List<Model_Inventory>>(success: true, data: inventoryList);
        }
        catch (MySqlException sqlEx)
        {
            await _logger.LogAsync(LogLevel.Error, $"Database error in GetInventoryByPartIdAsync: {sqlEx.Message}", sqlEx);
            await Dao_ErrorLog.LogErrorToDatabaseAsync(sqlEx, "GetInventoryByPartIdAsync");
            
            // Use MAUI error handler
            await _errorHandler.HandleExceptionAsync(sqlEx, "Database Operation");
            
            return new DaoResult<List<Model_Inventory>>(success: false, errorMessage: sqlEx.Message);
        }
        catch (Exception ex)
        {
            await _logger.LogAsync(LogLevel.Error, $"General error in GetInventoryByPartIdAsync: {ex.Message}", ex);
            await _errorHandler.HandleExceptionAsync(ex, "Inventory Operation");
            
            return new DaoResult<List<Model_Inventory>>(success: false, errorMessage: ex.Message);
        }
    }
}
```

## Error Handling in ViewModels

### MAUI ViewModel Error Handling Pattern:
```csharp
[ObservableObject]
public partial class InventoryViewModel
{
    private readonly Dao_Inventory _inventoryDao;
    private readonly Service_ErrorHandler _errorHandler;
    private readonly LoggingService _logger;
    
    public InventoryViewModel(Dao_Inventory inventoryDao, Service_ErrorHandler errorHandler, LoggingService logger)
    {
        _inventoryDao = inventoryDao;
        _errorHandler = errorHandler;
        _logger = logger;
    }
    
    [RelayCommand]
    private async Task LoadInventoryAsync()
    {
        try
        {
            IsBusy = true;
            
            var result = await _inventoryDao.GetInventoryByPartIdAsync(PartId);
            
            if (result.Success)
            {
                InventoryItems.Clear();
                foreach (var item in result.Data)
                {
                    InventoryItems.Add(item);
                }
            }
            else
            {
                await _errorHandler.ShowWarningAsync($"Failed to load inventory: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            await _logger.LogAsync(LogLevel.Error, $"Error in LoadInventoryAsync: {ex.Message}", ex);
            await _errorHandler.HandleExceptionAsync(ex, "Load Inventory");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
```

## Migration Implementation Plan

### 1. Error Handler Adaptation:
- [ ] Convert WinForms dialogs to MAUI DisplayAlert calls
- [ ] Implement custom error popup for detailed error display
- [ ] Preserve all existing error message logic
- [ ] Maintain database error logging exactly as implemented

### 2. Logging System Migration:
- [ ] Implement platform-specific log file locations
- [ ] Integrate with Microsoft.Extensions.Logging
- [ ] Preserve existing log levels and formatting
- [ ] Add structured logging capabilities

### 3. Global Exception Handling:
- [ ] Set up MAUI global exception handlers
- [ ] Preserve existing exception categorization
- [ ] Implement crash reporting mechanisms
- [ ] Test exception handling on all platforms

### 4. Database Error Integration:
- [ ] Keep all existing DAO error patterns
- [ ] Integrate new error handler with existing DAOs
- [ ] Preserve database error logging to error log table
- [ ] Test database error scenarios on all platforms

## Testing Strategy

### Error Scenarios to Test:
1. **Database Connection Failures** - Network disconnection scenarios
2. **Stored Procedure Errors** - Invalid parameters and data errors
3. **File System Errors** - Permission and storage issues
4. **Memory Pressure** - Low memory conditions on mobile
5. **Network Errors** - API call failures and timeouts
6. **User Input Validation** - Invalid data entry scenarios

### Platform-Specific Testing:
- **Windows**: Desktop error handling patterns
- **Android**: Mobile-specific error scenarios (battery, storage)
- **iOS**: iOS-specific error conditions and permissions
- **macOS**: Desktop macOS error handling

## Important Notes

1. **Preserve Database Logging**: All existing database error logging must remain exactly as implemented
2. **User Experience**: Error messages should be user-friendly and actionable
3. **Performance**: Error handling should not impact application performance
4. **Platform Consistency**: Error handling should feel native on each platform
5. **Testing Coverage**: Comprehensive error scenario testing on all platforms