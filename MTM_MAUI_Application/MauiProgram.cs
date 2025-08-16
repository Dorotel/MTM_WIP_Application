using MTM_MAUI_Application.Services;
using MTM_MAUI_Application.ViewModels;
using MTM_MAUI_Application.Views;
using Microsoft.Extensions.Logging;

namespace MTM_MAUI_Application;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Configuration first (needed by other services)
        builder.Services.AddSingleton<IConfiguration>(provider =>
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            return config;
        });

        // Register Logging Services
        builder.Services.AddLogging();
        builder.Services.AddSingleton<ILoggingService, LoggingService>();

        // Register Core Services (order matters for dependencies)
        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
        builder.Services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IThemeService, ThemeService>();
        builder.Services.AddSingleton<IConnectionService, ConnectionService>();

        // Register Data Services
        builder.Services.AddTransient<IInventoryService, InventoryService>();
        builder.Services.AddTransient<ITransactionService, TransactionService>();
        builder.Services.AddTransient<IPartService, PartService>();
        builder.Services.AddTransient<ILocationService, LocationService>();
        builder.Services.AddTransient<IOperationService, OperationService>();

        // Register ViewModels
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<InventoryViewModel>();
        builder.Services.AddTransient<TransferViewModel>();
        builder.Services.AddTransient<RemoveViewModel>();
        builder.Services.AddTransient<TransactionsViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        // Register Views
        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<InventoryPage>();

#if DEBUG
        builder.Services.AddLogging(configure => configure.AddDebug());
#endif

        return builder.Build();
    }
}