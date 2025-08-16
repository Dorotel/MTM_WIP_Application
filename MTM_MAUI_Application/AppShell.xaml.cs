namespace MTM_MAUI_Application;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute("inventory", typeof(Views.InventoryPage));
        Routing.RegisterRoute("transfer", typeof(Views.TransferPage));
        Routing.RegisterRoute("remove", typeof(Views.RemovePage));
        Routing.RegisterRoute("transactions", typeof(Views.TransactionsPage));
        Routing.RegisterRoute("settings", typeof(Views.SettingsPage));
        Routing.RegisterRoute("analyzer", typeof(Views.ApplicationAnalyzerPage));
        Routing.RegisterRoute("migration", typeof(Views.MigrationAssessmentPage));
        Routing.RegisterRoute("debug", typeof(Views.DebugDashboardPage));
    }
}