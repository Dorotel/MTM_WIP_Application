using MTM_MAUI_Application.ViewModels;

namespace MTM_MAUI_Application.Views;

public partial class InventoryPage : ContentPage
{
    public InventoryPage(InventoryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is InventoryViewModel viewModel)
        {
            await viewModel.LoadDataCommand.ExecuteAsync(null);
        }
    }
}