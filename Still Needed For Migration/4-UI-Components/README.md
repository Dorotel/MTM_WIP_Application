# UI Components Migration Guide

## Overview
This section contains all UI components that need to be converted from WinForms to MAUI XAML.

## User Controls (4-UI-Components/User-Controls)
Major UserControls that need MAUI ContentView conversion:

### Main Application Controls:
- **Control_InventoryTab** - Primary inventory entry interface
- **Control_RemoveTab** - Item removal interface  
- **Control_TransferTab** - Transfer operations interface
- **Control_AdvancedInventory** - Advanced inventory management
- **Control_AdvancedRemove** - Advanced removal operations
- **Control_QuickButtons** - Quick action buttons

### Settings Controls:
- **Control_Add_*** - Add operations (User, Location, Operation, etc.)
- **Control_Edit_*** - Edit operations for master data
- **Control_Remove_*** - Remove operations for master data
- **Control_Database** - Database settings management

## Forms (4-UI-Components/Forms)
Forms that need conversion to MAUI Pages/Views:

### Main Forms:
- **MainForm** - Convert to Shell or TabView
- **SettingsForm** - Convert to Settings Page
- **Transactions** - Convert to Transactions Page
- **SplashScreenForm** - Convert to MAUI Splash

### Development Forms:
- **ApplicationAnalyzerForm** - Development tools
- **MAUIMigrationAssessmentForm** - Migration assessment
- **DebugDashboardForm** - Debug monitoring

## MAUI Conversion Strategy

### 1. UserControl → ContentView Conversion:

#### Example: Control_InventoryTab → InventoryView
```xml
<!-- InventoryView.xaml -->
<ContentView x:Class="MTM_MAUI_Application.Views.InventoryView">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <!-- Header -->
        <StackLayout Grid.Row="0" Orientation="Horizontal">
            <Label Text="Part ID:" />
            <Entry Text="{Binding PartId}" />
            <Label Text="Location:" />
            <Picker ItemsSource="{Binding Locations}" 
                    SelectedItem="{Binding SelectedLocation}" />
        </StackLayout>
        
        <!-- Content -->
        <CollectionView Grid.Row="1" 
                       ItemsSource="{Binding InventoryItems}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <Grid>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="*"/>
                            <ColumnDefinition Width="*"/>
                            <ColumnDefinition Width="*"/>
                        </Grid.ColumnDefinitions>
                        <Label Grid.Column="0" Text="{Binding PartId}"/>
                        <Label Grid.Column="1" Text="{Binding Location}"/>
                        <Label Grid.Column="2" Text="{Binding Quantity}"/>
                    </Grid>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
        
        <!-- Actions -->
        <StackLayout Grid.Row="2" Orientation="Horizontal">
            <Button Text="Save" Command="{Binding SaveCommand}"/>
            <Button Text="Reset" Command="{Binding ResetCommand}"/>
        </StackLayout>
    </Grid>
</ContentView>
```

```csharp
// InventoryView.xaml.cs
public partial class InventoryView : ContentView
{
    public InventoryView(InventoryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

### 2. Form → Page Conversion:

#### Example: MainForm → AppShell + MainPage
```xml
<!-- AppShell.xaml -->
<Shell x:Class="MTM_MAUI_Application.AppShell">
    <TabBar>
        <ShellContent Title="Inventory" 
                     Icon="inventory.png"
                     ContentTemplate="{DataTemplate views:InventoryPage}"/>
        <ShellContent Title="Remove" 
                     Icon="remove.png"
                     ContentTemplate="{DataTemplate views:RemovePage}"/>
        <ShellContent Title="Transfer" 
                     Icon="transfer.png"
                     ContentTemplate="{DataTemplate views:TransferPage}"/>
        <ShellContent Title="Transactions" 
                     Icon="transactions.png"
                     ContentTemplate="{DataTemplate views:TransactionsPage}"/>
    </TabBar>
</Shell>
```

### 3. Data Binding Conversion:

#### WinForms Pattern:
```csharp
// WinForms - Direct control manipulation
private void LoadLocations()
{
    comboBox_Location.DataSource = locations;
    comboBox_Location.DisplayMember = "Name";
    comboBox_Location.ValueMember = "Id";
}

private void Button_Save_Click(object sender, EventArgs e)
{
    var partId = textBox_PartId.Text;
    var location = comboBox_Location.SelectedValue.ToString();
    // Save logic
}
```

#### MAUI Pattern:
```csharp
// MAUI - ViewModel with binding
[ObservableProperty]
private ObservableCollection<Location> locations = new();

[ObservableProperty]
private Location selectedLocation;

[ObservableProperty]
private string partId;

[RelayCommand]
private async Task SaveAsync()
{
    // Save logic using injected DAO
    var result = await _inventoryDao.AddInventoryItemAsync(PartId, SelectedLocation.Id, Quantity);
    // Handle result
}
```

## Control Mapping Reference

### WinForms → MAUI Control Mapping:
- **TextBox** → **Entry**
- **ComboBox** → **Picker**
- **DataGridView** → **CollectionView**
- **Button** → **Button**
- **Label** → **Label**
- **TabControl** → **Shell.TabBar** or **TabView**
- **Panel** → **StackLayout** or **Grid**
- **GroupBox** → **Frame** or **Border**
- **MenuStrip** → **Shell.MenuBar** or **MenuFlyout**
- **ToolStrip** → **Toolbar**
- **ProgressBar** → **ProgressBar**
- **TreeView** → **TreeView** (Community Toolkit)
- **ListView** → **CollectionView**

## Custom Controls

### Connection Strength Control:
Convert Control_ConnectionStrengthControl to custom MAUI control:

```csharp
public class ConnectionStrengthView : ContentView
{
    public static readonly BindableProperty StrengthProperty = 
        BindableProperty.Create(nameof(Strength), typeof(int), typeof(ConnectionStrengthView), 0);
    
    public int Strength
    {
        get => (int)GetValue(StrengthProperty);
        set => SetValue(StrengthProperty, value);
    }
    
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(Strength))
        {
            UpdateDisplay();
        }
    }
    
    private void UpdateDisplay()
    {
        // Custom drawing logic for connection strength bars
    }
}
```

## Migration Priorities

1. **Core Inventory Operations** (Control_InventoryTab)
2. **Main Navigation** (MainForm → AppShell)
3. **Data Display** (DataGridViews → CollectionViews)
4. **Settings Management** (SettingsForm)
5. **Advanced Operations** (Advanced controls)
6. **Development Tools** (Analyzer forms)

## Implementation Notes

- **Preserve All Business Logic**: Move business logic to ViewModels, keep DAO operations identical
- **Maintain User Experience**: Keep same workflow patterns and UI organization
- **Responsive Design**: Use MAUI's responsive layout capabilities
- **Platform Optimization**: Leverage platform-specific UI improvements
- **Accessibility**: Implement proper accessibility features for all platforms