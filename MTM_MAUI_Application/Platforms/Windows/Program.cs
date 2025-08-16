using Microsoft.Extensions.Logging;

namespace MTM_MAUI_Application.Platforms.Windows;

/// <summary>
/// Windows platform-specific application entry point
/// </summary>
public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var app = MauiProgram.CreateMauiApp();
        app.Run();
    }
}