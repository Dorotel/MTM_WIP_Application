# Core Infrastructure Migration Guide

## Overview
This section contains core infrastructure components that provide foundation services for the MAUI application.

## Helpers (8-Core-Infrastructure/Helpers)
Core infrastructure from the Core/ directory that needs MAUI adaptation:

### Core_Themes.cs
Comprehensive theming system with DPI scaling - **Critical for Migration**

#### Current Implementation Features:
- Runtime DPI scaling for all forms and controls
- Async/await UI responsiveness improvements  
- Theme application with proper color handling
- Runtime layout adjustments
- Dynamic DPI change handling for multi-monitor scenarios

#### MAUI Adaptation Strategy:
```csharp
public class ThemeManager
{
    private readonly IServiceProvider _serviceProvider;
    
    public ThemeManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task ApplyThemeAsync(string themeName)
    {
        // Preserve existing theme logic, adapt to MAUI ResourceDictionary
        var themeColors = await GetUserThemeColorsAsync(themeName);
        
        Application.Current.Resources["PrimaryColor"] = Color.FromHex(themeColors.Primary);
        Application.Current.Resources["SecondaryColor"] = Color.FromHex(themeColors.Secondary);
        Application.Current.Resources["BackgroundColor"] = Color.FromHex(themeColors.Background);
        Application.Current.Resources["TextColor"] = Color.FromHex(themeColors.Text);
    }
    
    public void ApplyFontScaling(double fontScale)
    {
        // Adapt existing font scaling logic to MAUI
        Application.Current.Resources["SmallFontSize"] = 12 * fontScale;
        Application.Current.Resources["MediumFontSize"] = 16 * fontScale;
        Application.Current.Resources["LargeFontSize"] = 20 * fontScale;
    }
    
    // Preserve all existing theme color calculation methods
    private async Task<ThemeColors> GetUserThemeColorsAsync(string themeName)
    {
        // Keep existing database theme loading logic
        var userDao = _serviceProvider.GetRequiredService<Dao_User>();
        // ... existing logic
    }
}
```

### Core Infrastructure Components:

#### 1. Application Context Management:
```csharp
public class ApplicationContextService
{
    public string CurrentUser { get; private set; }
    public string ServerAddress { get; private set; }
    public string DatabaseName { get; private set; }
    public bool IsProduction { get; private set; }
    
    public async Task InitializeAsync()
    {
        CurrentUser = Environment.UserName;
        ServerAddress = Helper_Database_Variables.GetServerAddress();
        DatabaseName = Helper_Database_Variables.GetDatabaseName();
        IsProduction = !DatabaseName.Contains("test");
    }
    
    public string GetConnectionString()
    {
        // Preserve existing connection string logic exactly
        return Helper_Database_Variables.GetConnectionString();
    }
}
```

#### 2. Progress Management Service:
```csharp
public class ProgressService
{
    private readonly IServiceProvider _serviceProvider;
    
    public ProgressService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task ShowProgressAsync(string message)
    {
        // Convert Helper_StoredProcedureProgress logic to MAUI
        var page = Application.Current?.MainPage;
        if (page is ContentPage contentPage)
        {
            // Show progress indicator on current page
            // Implement using MAUI ActivityIndicator or custom progress view
        }
    }
    
    public async Task UpdateProgressAsync(string message, int percentage = -1)
    {
        // Update progress display
    }
    
    public async Task HideProgressAsync()
    {
        // Hide progress indicator
    }
    
    public async Task ShowSuccessAsync(string message)
    {
        // Show success indicator (green progress bar equivalent)
        var page = Application.Current?.MainPage;
        await page?.DisplayAlert("Success", message, "OK");
    }
    
    public async Task ShowErrorAsync(string message)
    {
        // Show error indicator (red progress bar equivalent) 
        var page = Application.Current?.MainPage;
        await page?.DisplayAlert("Error", message, "OK");
    }
}
```

## Extensions (8-Core-Infrastructure/Extensions)
Extension methods for cross-platform functionality:

### Platform Extensions:
```csharp
public static class PlatformExtensions
{
    public static bool IsWindows(this DeviceInfo deviceInfo)
    {
        return DeviceInfo.Platform == DevicePlatform.WinUI;
    }
    
    public static bool IsAndroid(this DeviceInfo deviceInfo)
    {
        return DeviceInfo.Platform == DevicePlatform.Android;
    }
    
    public static bool IsIOS(this DeviceInfo deviceInfo)
    {
        return DeviceInfo.Platform == DevicePlatform.iOS;
    }
    
    public static bool IsMacOS(this DeviceInfo deviceInfo)
    {
        return DeviceInfo.Platform == DevicePlatform.MacCatalyst;
    }
}
```

### Database Extensions:
```csharp
public static class DatabaseExtensions
{
    public static string GetPlatformConnectionString(this string baseConnectionString)
    {
        // Adapt connection string for platform-specific requirements
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Mobile platforms may need different connection settings
            return baseConnectionString + ";Connect Timeout=30;";
        }
        
        return baseConnectionString;
    }
}
```

### MAUI-Specific Extensions:
```csharp
public static class UIExtensions
{
    public static async Task FadeInAsync(this VisualElement element, uint duration = 250)
    {
        element.Opacity = 0;
        await element.FadeTo(1, duration);
    }
    
    public static async Task FadeOutAsync(this VisualElement element, uint duration = 250)
    {
        await element.FadeTo(0, duration);
    }
    
    public static void SetBusyState(this ContentPage page, bool isBusy, string message = "Loading...")
    {
        // Implement busy state management for pages
        // Could use a global loading overlay or per-page indicators
    }
}
```

## Dependency Injection Configuration

### Complete Service Registration:
```csharp
// In MauiProgram.cs
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Core Infrastructure Services
        builder.Services.AddSingleton<ApplicationContextService>();
        builder.Services.AddSingleton<ThemeManager>();
        builder.Services.AddSingleton<ProgressService>();
        
        // Database Services (All DAO classes)
        builder.Services.AddTransient<Dao_Inventory>();
        builder.Services.AddTransient<Dao_User>();
        builder.Services.AddTransient<Dao_Transactions>();
        builder.Services.AddTransient<Dao_ErrorLog>();
        builder.Services.AddTransient<Dao_History>();
        builder.Services.AddTransient<Dao_ItemType>();
        builder.Services.AddTransient<Dao_Location>();
        builder.Services.AddTransient<Dao_Operation>();
        builder.Services.AddTransient<Dao_Part>();
        builder.Services.AddTransient<Dao_QuickButtons>();
        builder.Services.AddTransient<Dao_System>();
        
        // Application Services
        builder.Services.AddSingleton<Service_ErrorHandler>();
        builder.Services.AddSingleton<Service_ConnectionRecoveryManager>();
        builder.Services.AddSingleton<Service_ApplicationAnalyzer>();
        builder.Services.AddSingleton<Service_AnalysisFormatter>();
        builder.Services.AddSingleton<Service_MAUIMigrationAssessment>();
        builder.Services.AddSingleton<Service_DebugConfiguration>();
        builder.Services.AddSingleton<Service_DebugTracer>();
        builder.Services.AddSingleton<LoggingService>();
        
        // ViewModels
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<InventoryViewModel>();
        builder.Services.AddTransient<RemoveViewModel>();
        builder.Services.AddTransient<TransferViewModel>();
        builder.Services.AddTransient<TransactionsViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        
        // Pages and Views
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<InventoryPage>();
        builder.Services.AddTransient<RemovePage>();
        builder.Services.AddTransient<TransferPage>();
        builder.Services.AddTransient<TransactionsPage>();
        builder.Services.AddTransient<SettingsPage>();

        return builder.Build();
    }
}
```

## Platform-Specific Implementations

### Windows Platform Services:
```csharp
#if WINDOWS
public class WindowsPlatformService : IPlatformService
{
    public async Task<string> GetAppDataFolderAsync()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
    
    public async Task<bool> HasNetworkAccessAsync()
    {
        return NetworkInterface.GetIsNetworkAvailable();
    }
}
#endif
```

### Android Platform Services:
```csharp
#if ANDROID
public class AndroidPlatformService : IPlatformService
{
    public async Task<string> GetAppDataFolderAsync()
    {
        return FileSystem.AppDataDirectory;
    }
    
    public async Task<bool> HasNetworkAccessAsync()
    {
        var connectivity = Connectivity.Current;
        return connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}
#endif
```

## Application Lifecycle Management

### MAUI Application Lifecycle:
```csharp
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        
        MainPage = serviceProvider.GetRequiredService<AppShell>();
    }
    
    protected override async void OnStart()
    {
        // Initialize application context
        var contextService = _serviceProvider.GetRequiredService<ApplicationContextService>();
        await contextService.InitializeAsync();
        
        // Apply user theme
        var themeManager = _serviceProvider.GetRequiredService<ThemeManager>();
        var userDao = _serviceProvider.GetRequiredService<Dao_User>();
        var themeName = await userDao.GetThemeNameAsync(Environment.UserName);
        await themeManager.ApplyThemeAsync(themeName);
        
        // Start background services
        var connectionManager = _serviceProvider.GetRequiredService<Service_ConnectionRecoveryManager>();
        connectionManager.StartMonitoring();
    }
    
    protected override void OnSleep()
    {
        // Pause background operations
    }
    
    protected override void OnResume()
    {
        // Resume background operations
    }
}
```

## Cross-Platform Compatibility

### File System Operations:
```csharp
public class FileSystemService
{
    public async Task<string> GetLogDirectoryAsync()
    {
        var baseDirectory = DeviceInfo.Platform switch
        {
            DevicePlatform.WinUI => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            DevicePlatform.Android => FileSystem.AppDataDirectory,
            DevicePlatform.iOS => FileSystem.AppDataDirectory,
            DevicePlatform.MacCatalyst => FileSystem.AppDataDirectory,
            _ => FileSystem.AppDataDirectory
        };
        
        var logDirectory = Path.Combine(baseDirectory, "MTM_Inventory", "Logs");
        Directory.CreateDirectory(logDirectory);
        return logDirectory;
    }
    
    public async Task<bool> IsDirectoryWritableAsync(string path)
    {
        try
        {
            var testFile = Path.Combine(path, "test.tmp");
            await File.WriteAllTextAsync(testFile, "test");
            File.Delete(testFile);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
```

## Migration Implementation Checklist

### Core Infrastructure Setup:
- [ ] Implement ApplicationContextService for app-wide context
- [ ] Adapt ThemeManager from Core_Themes.cs logic
- [ ] Create ProgressService to replace Helper_StoredProcedureProgress
- [ ] Set up complete dependency injection configuration
- [ ] Implement platform-specific services

### Extension Methods:
- [ ] Create platform detection extensions
- [ ] Implement database connection extensions
- [ ] Add MAUI-specific UI extensions
- [ ] Create file system operation extensions

### Application Lifecycle:
- [ ] Configure App.xaml.cs with proper service initialization
- [ ] Implement startup sequence with database connection
- [ ] Set up theme application on startup
- [ ] Configure background service management

### Cross-Platform Compatibility:
- [ ] Test file system operations on all platforms
- [ ] Verify database connectivity on all platforms
- [ ] Validate theme application across platforms
- [ ] Test application lifecycle on mobile platforms

## Important Notes

1. **Preserve Business Logic**: All existing business logic and database operations must remain identical
2. **Theme Compatibility**: Maintain exact compatibility with existing theme preferences stored in database
3. **Performance**: Ensure infrastructure services don't impact application startup performance
4. **Platform Native Feel**: While maintaining consistency, respect platform conventions
5. **Testing Requirements**: Comprehensive testing on all target platforms for infrastructure reliability