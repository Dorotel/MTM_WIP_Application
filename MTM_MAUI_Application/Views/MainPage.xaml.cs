namespace MTM_MAUI_Application.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        // Navigate to inventory page
        await Shell.Current.GoToAsync("//inventory");
    }
}