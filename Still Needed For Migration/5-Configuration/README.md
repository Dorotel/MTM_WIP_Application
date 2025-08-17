# Configuration & Settings Migration Guide

## Overview
This section contains all configuration and settings management that needs MAUI adaptation.

## App Settings (5-Configuration/App-Settings)
Core application configuration files:

### Key Configuration Files:
- **MTM_Inventory_Application.csproj** - Project dependencies and build settings
- **Program.cs** - Application entry point and startup configuration
- **AssemblyInfo.cs** - Assembly metadata and version information
- **Properties/launchSettings.json** - Debug launch configuration

### MAUI Configuration Adaptation:

#### Project File Conversion:
```xml
<!-- Original WinForms .csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
  </PropertyGroup>
</Project>

<!-- MAUI .csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net8.0-android;net8.0-ios;net8.0-maccatalyst;net8.0-windows10.0.19041.0</TargetFrameworks>
    <OutputType>Exe</OutputType>
    <RootNamespace>MTM_MAUI_Application</RootNamespace>
    <UseMaui>true</UseMaui>
    <SingleProject>true</SingleProject>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

#### Dependencies Migration:
```xml
<!-- Preserve these exact package versions -->
<PackageReference Include="MySql.Data" Version="8.4.0" />
<PackageReference Include="ClosedXML" Version="0.102.2" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="Microsoft.CodeAnalysis.CSharp" Version="4.8.0" />

<!-- Add MAUI packages -->
<PackageReference Include="Microsoft.Maui.Controls" Version="8.0.6" />
<PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="8.0.6" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
```

## User Settings (5-Configuration/User-Settings)
User preference and configuration management:

### Current Settings Pattern:
```csharp
// From Model_Users.cs
public class Model_Users
{
    public string Database { get; set; } = "mtm_wip_application";
    public string WipServerAddress { get; set; } = "172.16.1.104";
    public string Theme_Name { get; set; } = "Default";
    public int Theme_FontSize { get; set; } = 12;
    // ... other settings
}
```

### MAUI Settings Implementation:
```csharp
// Use MAUI Preferences for user settings
public class UserSettingsService
{
    public string Database
    {
        get => Preferences.Get(nameof(Database), "mtm_wip_application");
        set => Preferences.Set(nameof(Database), value);
    }
    
    public string WipServerAddress
    {
        get => Preferences.Get(nameof(WipServerAddress), "172.16.1.104");
        set => Preferences.Set(nameof(WipServerAddress), value);
    }
    
    public string ThemeName
    {
        get => Preferences.Get(nameof(ThemeName), "Default");
        set => Preferences.Set(nameof(ThemeName), value);
    }
    
    public int ThemeFontSize
    {
        get => Preferences.Get(nameof(ThemeFontSize), 12);
        set => Preferences.Set(nameof(ThemeFontSize), value);
    }
}
```

## Application Startup Configuration

### Original Program.cs Pattern:
```csharp
// WinForms startup
[STAThread]
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new Service_Onstartup_StartupSplashApplicationContext());
}
```

### MAUI Startup Configuration:
```csharp
// MauiProgram.cs
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

        // Register services
        builder.Services.AddSingleton<UserSettingsService>();
        builder.Services.AddTransient<MainViewModel>();
        
        // Database services
        builder.Services.AddTransient<Dao_Inventory>();
        builder.Services.AddTransient<Dao_User>();
        builder.Services.AddTransient<Dao_Transactions>();
        
        // Application services
        builder.Services.AddSingleton<Service_ErrorHandler>();
        builder.Services.AddSingleton<Service_ConnectionRecoveryManager>();

        return builder.Build();
    }
}
```

## Environment Configuration

### Development vs Production:
```csharp
// Environment detection logic (preserve exactly)
public static string GetEnvironmentDatabase()
{
#if DEBUG
    return "mtm_wip_application_test";
#else
    return "mtm_wip_application";
#endif
}

public static string GetServerAddress()
{
    var currentIP = GetCurrentIPAddress();
#if DEBUG
    // Debug: Use 172.16.1.104 if IP matches, otherwise localhost
    return currentIP.StartsWith("172.16.1.") ? "172.16.1.104" : "localhost";
#else
    // Production: Always 172.16.1.104
    return "172.16.1.104";
#endif
}
```

## Platform-Specific Configuration

### Windows Configuration:
```xml
<!-- Platforms/Windows/Package.appxmanifest -->
<Package>
  <Identity Name="MTM.InventoryApplication" />
  <Properties>
    <DisplayName>MTM Inventory Application</DisplayName>
    <PublisherDisplayName>MTM</PublisherDisplayName>
  </Properties>
</Package>
```

### Android Configuration:
```xml
<!-- Platforms/Android/AndroidManifest.xml -->
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
    <uses-permission android:name="android.permission.INTERNET" />
    <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
    <application android:allowBackup="true" android:icon="@mipmap/appicon">
    </application>
</manifest>
```

### iOS Configuration:
```xml
<!-- Platforms/iOS/Info.plist -->
<dict>
    <key>CFBundleName</key>
    <string>MTM Inventory</string>
    <key>CFBundleDisplayName</key>
    <string>MTM Inventory Application</string>
    <key>NSAppTransportSecurity</key>
    <dict>
        <key>NSAllowsArbitraryLoads</key>
        <true/>
    </dict>
</dict>
```

## Configuration Migration Checklist

### Application Settings:
- [ ] Update project file for MAUI multi-targeting
- [ ] Preserve all NuGet package versions
- [ ] Configure platform-specific settings
- [ ] Set up dependency injection in MauiProgram.cs
- [ ] Configure application metadata and icons

### User Settings:
- [ ] Implement UserSettingsService using MAUI Preferences
- [ ] Migrate database user settings to new service
- [ ] Preserve theme and font size settings
- [ ] Maintain server address configuration logic

### Environment Configuration:
- [ ] Preserve debug/production database switching
- [ ] Maintain server address detection logic
- [ ] Configure platform-specific network permissions
- [ ] Set up build configurations for different environments

### Database Configuration:
- [ ] Keep connection string management exactly as implemented
- [ ] Preserve Helper_Database_Variables logic
- [ ] Maintain production/debug environment detection
- [ ] Test database connectivity on all platforms

## Important Notes

1. **Preserve Environment Logic**: The debug/production server detection must work identically
2. **Database Compatibility**: All database settings must remain exactly the same
3. **User Settings**: Migrate from database storage to MAUI Preferences where appropriate
4. **Platform Support**: Ensure configuration works on Windows, Android, iOS, and macOS
5. **Build Configuration**: Maintain separate debug/release configurations