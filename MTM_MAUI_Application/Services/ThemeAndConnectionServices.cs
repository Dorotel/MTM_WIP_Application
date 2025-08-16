namespace MTM_MAUI_Application.Services;

/// <summary>
/// Theme service implementation
/// </summary>
public class ThemeService : IThemeService
{
    private readonly IUserService _userService;
    private readonly ILoggingService _logging;
    private string _currentTheme = "Default";

    public ThemeService(IUserService userService, ILoggingService logging)
    {
        _userService = userService;
        _logging = logging;
    }

    public async Task ApplyThemeAsync(string themeName)
    {
        try
        {
            _currentTheme = themeName;
            
            // Apply theme to application resources
            var app = Application.Current;
            if (app != null)
            {
                // Clear existing theme resources
                app.Resources.Clear();
                
                // Load base theme resources
                app.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("Resources/Styles/Colors.xaml", UriKind.Relative)
                });
                
                app.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("Resources/Styles/Styles.xaml", UriKind.Relative)
                });

                // Apply theme-specific styles based on theme name
                ApplyThemeColors(themeName);
            }

            await _logging.LogInfoAsync($"Applied theme: {themeName}");
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error applying theme {themeName}", ex);
        }
    }

    public Task<string> GetCurrentThemeAsync()
    {
        return Task.FromResult(_currentTheme);
    }

    public Task<List<string>> GetAvailableThemesAsync()
    {
        var themes = new List<string>
        {
            "Default",
            "Dark",
            "Light",
            "Blue",
            "Green"
        };

        return Task.FromResult(themes);
    }

    public async Task SetFontSizeAsync(int fontSize)
    {
        try
        {
            var app = Application.Current;
            if (app != null)
            {
                // Update font size in resources
                app.Resources["DefaultFontSize"] = fontSize;
                app.Resources["SmallFontSize"] = fontSize - 2;
                app.Resources["LargeFontSize"] = fontSize + 4;
                app.Resources["HeaderFontSize"] = fontSize + 8;
            }

            await _logging.LogInfoAsync($"Set font size to: {fontSize}");
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error setting font size {fontSize}", ex);
        }
    }

    public Task<int> GetFontSizeAsync()
    {
        try
        {
            var app = Application.Current;
            if (app?.Resources.ContainsKey("DefaultFontSize") == true)
            {
                return Task.FromResult((int)app.Resources["DefaultFontSize"]);
            }
        }
        catch
        {
            // Fallback to default
        }

        return Task.FromResult(14);
    }

    private void ApplyThemeColors(string themeName)
    {
        var app = Application.Current;
        if (app == null) return;

        // Define color schemes for different themes
        switch (themeName.ToLower())
        {
            case "dark":
                app.Resources["PrimaryColor"] = Color.FromArgb("#2196F3");
                app.Resources["SecondaryColor"] = Color.FromArgb("#FFC107");
                app.Resources["BackgroundColor"] = Color.FromArgb("#121212");
                app.Resources["SurfaceColor"] = Color.FromArgb("#1E1E1E");
                app.Resources["TextColor"] = Color.FromArgb("#FFFFFF");
                app.Resources["SecondaryTextColor"] = Color.FromArgb("#CCCCCC");
                break;

            case "light":
                app.Resources["PrimaryColor"] = Color.FromArgb("#2196F3");
                app.Resources["SecondaryColor"] = Color.FromArgb("#FFC107");
                app.Resources["BackgroundColor"] = Color.FromArgb("#FFFFFF");
                app.Resources["SurfaceColor"] = Color.FromArgb("#F5F5F5");
                app.Resources["TextColor"] = Color.FromArgb("#000000");
                app.Resources["SecondaryTextColor"] = Color.FromArgb("#666666");
                break;

            case "blue":
                app.Resources["PrimaryColor"] = Color.FromArgb("#1976D2");
                app.Resources["SecondaryColor"] = Color.FromArgb("#42A5F5");
                app.Resources["BackgroundColor"] = Color.FromArgb("#E3F2FD");
                app.Resources["SurfaceColor"] = Color.FromArgb("#BBDEFB");
                app.Resources["TextColor"] = Color.FromArgb("#0D47A1");
                app.Resources["SecondaryTextColor"] = Color.FromArgb("#1565C0");
                break;

            case "green":
                app.Resources["PrimaryColor"] = Color.FromArgb("#388E3C");
                app.Resources["SecondaryColor"] = Color.FromArgb("#66BB6A");
                app.Resources["BackgroundColor"] = Color.FromArgb("#E8F5E8");
                app.Resources["SurfaceColor"] = Color.FromArgb("#C8E6C9");
                app.Resources["TextColor"] = Color.FromArgb("#1B5E20");
                app.Resources["SecondaryTextColor"] = Color.FromArgb("#2E7D32");
                break;

            default: // Default theme
                app.Resources["PrimaryColor"] = Color.FromArgb("#2196F3");
                app.Resources["SecondaryColor"] = Color.FromArgb("#FFC107");
                app.Resources["BackgroundColor"] = Color.FromArgb("#FFFFFF");
                app.Resources["SurfaceColor"] = Color.FromArgb("#F5F5F5");
                app.Resources["TextColor"] = Color.FromArgb("#000000");
                app.Resources["SecondaryTextColor"] = Color.FromArgb("#666666");
                break;
        }
    }
}

/// <summary>
/// Connection service implementation
/// </summary>
public class ConnectionService : IConnectionService
{
    private readonly IDatabaseService _database;
    private readonly ILoggingService _logging;
    private Timer? _monitoringTimer;
    private bool _isMonitoring;
    private bool _lastConnectionState = true;

    public event EventHandler<bool>? ConnectionStatusChanged;

    public ConnectionService(IDatabaseService database, ILoggingService logging)
    {
        _database = database;
        _logging = logging;
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            return await _database.TestConnectionAsync();
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync("Error testing connection", ex);
            return false;
        }
    }

    public async Task<int> GetConnectionStrengthAsync()
    {
        try
        {
            var isConnected = await TestConnectionAsync();
            if (!isConnected) return 0;

            // Measure connection quality by testing response time
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            await _database.TestConnectionAsync();
            stopwatch.Stop();

            var responseTime = stopwatch.ElapsedMilliseconds;
            
            // Convert response time to strength (0-5 bars)
            return responseTime switch
            {
                < 100 => 5,   // Excellent
                < 300 => 4,   // Good
                < 800 => 3,   // Fair
                < 2000 => 2,  // Poor
                < 5000 => 1,  // Very Poor
                _ => 0        // No connection
            };
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync("Error getting connection strength", ex);
            return 0;
        }
    }

    public async Task StartMonitoringAsync()
    {
        if (_isMonitoring) return;

        _isMonitoring = true;
        _monitoringTimer = new Timer(MonitorConnection, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        
        await _logging.LogInfoAsync("Connection monitoring started");
    }

    public async Task StopMonitoringAsync()
    {
        if (!_isMonitoring) return;

        _isMonitoring = false;
        _monitoringTimer?.Dispose();
        _monitoringTimer = null;
        
        await _logging.LogInfoAsync("Connection monitoring stopped");
    }

    private async void MonitorConnection(object? state)
    {
        try
        {
            var isConnected = await TestConnectionAsync();
            
            if (isConnected != _lastConnectionState)
            {
                _lastConnectionState = isConnected;
                ConnectionStatusChanged?.Invoke(this, isConnected);
                
                var status = isConnected ? "restored" : "lost";
                await _logging.LogInfoAsync($"Database connection {status}");
            }
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync("Error monitoring connection", ex);
        }
    }
}