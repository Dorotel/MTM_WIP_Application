# Resources & Assets Migration Guide

## Overview
This section contains all resources and assets that need MAUI platform adaptation.

## Icons & Images (6-Resources/Icons-Images)
Visual assets requiring multi-platform implementation:

### Current Assets:
- **MTMWIPApp.ico** - Main application icon (Windows format)
- **Various UI images** - Button icons, status indicators, etc.

### MAUI Resource Structure Required:
```
Resources/
├── AppIcon/
│   ├── appicon.svg (Vector format - scales to all sizes)
│   └── appiconfg.svg (Foreground variant)
├── Splash/
│   └── splash.svg (Splash screen image)
├── Images/
│   ├── inventory.svg
│   ├── remove.svg
│   ├── transfer.svg
│   ├── transactions.svg
│   └── settings.svg
└── Raw/
    └── (Any non-image resources)
```

### Icon Conversion Process:

#### 1. Convert ICO to SVG:
```xml
<!-- appicon.svg - Main app icon -->
<svg width="100" height="100" viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
    <!-- Convert MTMWIPApp.ico design to SVG -->
    <rect width="100" height="100" fill="#2196F3"/>
    <text x="50" y="50" font-family="Arial" font-size="24" fill="white" text-anchor="middle" dy=".3em">MTM</text>
</svg>
```

#### 2. Configure in Project File:
```xml
<MauiIcon Include="Resources\AppIcon\appicon.svg" ForegroundFile="Resources\AppIcon\appiconfg.svg" Color="#512BD4" />
<MauiSplashScreen Include="Resources\Splash\splash.svg" Color="#512BD4" BaseSize="128,128" />
```

## Fonts (6-Resources/Fonts)
Typography resources for consistent UI:

### Font Implementation:
```xml
<!-- In MauiProgram.cs configuration -->
.ConfigureFonts(fonts =>
{
    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    // Add custom MTM fonts if needed
})
```

### Custom Font Usage:
```xml
<!-- In XAML -->
<Label Text="MTM Inventory Application" 
       FontFamily="OpenSansRegular" 
       FontSize="18" />
```

## Styling (6-Resources/Styling)
Theme and styling system adaptation:

### Current Theming System:
From Core_Themes.cs - Comprehensive theming with DPI scaling and color management.

### MAUI Styling Implementation:

#### 1. Colors.xaml:
```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    
    <!-- Light Theme Colors -->
    <Color x:Key="Primary">#512BD4</Color>
    <Color x:Key="Secondary">#DFD8F7</Color>
    <Color x:Key="Tertiary">#2B0B98</Color>
    <Color x:Key="White">White</Color>
    <Color x:Key="Black">Black</Color>
    <Color x:Key="Gray100">#E1E1E1</Color>
    <Color x:Key="Gray200">#C8C8C8</Color>
    <Color x:Key="Gray300">#ACACAC</Color>
    <Color x:Key="Gray400">#919191</Color>
    <Color x:Key="Gray500">#6E6E6E</Color>
    <Color x:Key="Gray600">#404040</Color>
    <Color x:Key="Gray900">#212121</Color>
    <Color x:Key="Gray950">#141414</Color>
    
    <!-- MTM Specific Colors -->
    <Color x:Key="MTMBlue">#2196F3</Color>
    <Color x:Key="MTMGreen">#4CAF50</Color>
    <Color x:Key="MTMRed">#F44336</Color>
    <Color x:Key="MTMOrange">#FF9800</Color>
</ResourceDictionary>
```

#### 2. Styles.xaml:
```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">

    <!-- Button Styles -->
    <Style x:Key="PrimaryButtonStyle" TargetType="Button">
        <Setter Property="BackgroundColor" Value="{StaticResource Primary}" />
        <Setter Property="TextColor" Value="{StaticResource White}" />
        <Setter Property="FontFamily" Value="OpenSansRegular" />
        <Setter Property="FontSize" Value="14" />
        <Setter Property="CornerRadius" Value="8" />
        <Setter Property="Padding" Value="14,10" />
    </Style>
    
    <!-- Entry Styles -->
    <Style x:Key="StandardEntryStyle" TargetType="Entry">
        <Setter Property="FontFamily" Value="OpenSansRegular" />
        <Setter Property="FontSize" Value="14" />
        <Setter Property="PlaceholderColor" Value="{StaticResource Gray400}" />
        <Setter Property="TextColor" Value="{StaticResource Gray900}" />
        <Setter Property="BackgroundColor" Value="{StaticResource Gray100}" />
    </Style>
    
    <!-- Label Styles -->
    <Style x:Key="HeaderLabelStyle" TargetType="Label">
        <Setter Property="FontFamily" Value="OpenSansSemibold" />
        <Setter Property="FontSize" Value="18" />
        <Setter Property="TextColor" Value="{StaticResource Gray900}" />
    </Style>
    
    <!-- DataGrid equivalent styles -->
    <Style x:Key="CollectionViewStyle" TargetType="CollectionView">
        <Setter Property="BackgroundColor" Value="{StaticResource White}" />
        <Setter Property="SelectionMode" Value="Single" />
    </Style>
</ResourceDictionary>
```

#### 3. Theme Management Service:
```csharp
public class ThemeService
{
    public enum AppTheme
    {
        Default,
        Light,
        Dark
    }
    
    public void ApplyTheme(AppTheme theme)
    {
        Application.Current.UserAppTheme = theme switch
        {
            AppTheme.Dark => AppTheme.Dark,
            AppTheme.Light => AppTheme.Light,
            _ => AppTheme.Unspecified
        };
    }
    
    public void ApplyFontSize(int fontSize)
    {
        // Implement dynamic font sizing
        var scale = fontSize / 12.0; // Base size 12
        
        Application.Current.Resources["SmallFontSize"] = 10 * scale;
        Application.Current.Resources["MediumFontSize"] = 14 * scale;
        Application.Current.Resources["LargeFontSize"] = 18 * scale;
    }
}
```

## Platform-Specific Resources

### Windows Resources:
```xml
<!-- Platforms/Windows/app.ico -->
<!-- Convert MTMWIPApp.ico for Windows platform -->

<!-- Platforms/Windows/Package.appxmanifest -->
<Package>
  <Applications>
    <Application>
      <uap:VisualElements
        DisplayName="MTM Inventory Application"
        Square150x150Logo="Images\Square150x150Logo.png"
        Square44x44Logo="Images\Square44x44Logo.png"
        BackgroundColor="transparent">
        <uap:DefaultTile Wide310x150Logo="Images\Wide310x150Logo.png" />
      </uap:VisualElements>
    </Application>
  </Applications>
</Package>
```

### Android Resources:
```xml
<!-- Platforms/Android/Resources/mipmap-*/appicon.png -->
<!-- Generated from appicon.svg in multiple sizes: -->
<!-- mipmap-mdpi/appicon.png (48x48) -->
<!-- mipmap-hdpi/appicon.png (72x72) -->
<!-- mipmap-xhdpi/appicon.png (96x96) -->
<!-- mipmap-xxhdpi/appicon.png (144x144) -->
<!-- mipmap-xxxhdpi/appicon.png (192x192) -->
```

### iOS Resources:
```xml
<!-- Platforms/iOS/Assets.xcassets/AppIcon.appiconset/ -->
<!-- Generated from appicon.svg in multiple sizes for iOS -->
```

## Resource Migration Strategy

### 1. Asset Conversion:
- Convert MTMWIPApp.ico to SVG format for scalability
- Create platform-specific icon sets from SVG source
- Design consistent splash screen for all platforms

### 2. Styling Migration:
- Extract color schemes from Core_Themes.cs
- Create MAUI resource dictionaries for colors and styles
- Implement theme switching compatible with existing user preferences

### 3. Font Management:
- Include OpenSans fonts for consistency
- Implement dynamic font sizing to match existing functionality
- Ensure accessibility compliance across platforms

### 4. Icon Set Creation:
Create SVG icons for all major features:
```
inventory.svg - Inventory management
remove.svg - Item removal
transfer.svg - Transfer operations
transactions.svg - Transaction history
settings.svg - Application settings
search.svg - Search functionality
print.svg - Print operations
save.svg - Save operations
reset.svg - Reset operations
```

## Implementation Checklist

### Resource Setup:
- [ ] Convert MTMWIPApp.ico to appicon.svg
- [ ] Create splash.svg for splash screen
- [ ] Generate platform-specific icon sets
- [ ] Include OpenSans font files
- [ ] Create SVG icon set for UI elements

### Styling Implementation:
- [ ] Create Colors.xaml with theme colors
- [ ] Create Styles.xaml with control styles
- [ ] Implement ThemeService for dynamic theming
- [ ] Configure font scaling system
- [ ] Test theme switching functionality

### Platform Configuration:
- [ ] Configure Windows app manifest and icons
- [ ] Set up Android app icons and permissions
- [ ] Configure iOS app icons and metadata
- [ ] Test resource loading on all platforms

### Resource Optimization:
- [ ] Optimize SVG files for size and performance
- [ ] Implement resource caching where appropriate
- [ ] Configure build actions for all resources
- [ ] Test resource scaling on different screen densities

## Important Notes

1. **Scalability**: Use SVG format for all vector graphics to ensure crisp display on all screen densities
2. **Platform Consistency**: Maintain visual consistency while respecting platform conventions
3. **Theme Compatibility**: Preserve existing theme preferences and font size settings
4. **Performance**: Optimize resource loading to maintain application startup performance
5. **Accessibility**: Ensure all visual assets support accessibility features on each platform