# Data Models Migration Guide

## Overview
This section contains all data models that need to be adapted for MAUI MVVM architecture.

## Current Models (2-Data-Models/Current-Models)
Existing model classes from the WinForms application:

### Key Models:
- **Model_Users** - User data and settings
- **Model_Inventory** - Inventory item data structures
- **Model_Transactions** - Transaction history models
- **Model_ApplicationAnalysis** - Application analysis data structures
- **Model_MAUIMigrationAssessment** - Migration assessment models

### Migration Strategy:
1. **Keep Entity Models**: Preserve all database entity models exactly as they are
2. **Add ViewModels**: Create corresponding ViewModels for UI binding
3. **Add Observable Properties**: Use CommunityToolkit.Mvvm for property change notifications

## MAUI ViewModel Implementation

### Example Conversion:
```csharp
// Original Model (keep as-is)
public class Model_Inventory
{
    public string PartId { get; set; }
    public string Location { get; set; }
    public int Quantity { get; set; }
    // ... other properties
}

// New ViewModel for MAUI
[ObservableObject]
public partial class InventoryViewModel
{
    [ObservableProperty]
    private string partId;
    
    [ObservableProperty]
    private string location;
    
    [ObservableProperty]
    private int quantity;
    
    [ObservableProperty]
    private ObservableCollection<Model_Inventory> inventoryItems;
    
    // Commands for UI interactions
    [RelayCommand]
    private async Task AddInventoryAsync()
    {
        // Implementation using existing DAO classes
    }
    
    [RelayCommand]
    private async Task SaveInventoryAsync()
    {
        // Implementation using existing DAO classes
    }
}
```

## Database Entity Models
All existing models should be preserved exactly as implemented:

### Preserve These Models:
- Connection string models
- Database result models
- Stored procedure parameter models
- Error handling models
- User settings models

### Migration Notes:
- **DO NOT CHANGE** existing entity models
- These models work perfectly with existing stored procedures
- Keep all database-related models in a separate Models/Entities folder
- Add new ViewModels in Models/ViewModels folder

## ViewModel Architecture

### Main ViewModels Needed:
1. **MainViewModel** - Main application shell
2. **InventoryViewModel** - Inventory management
3. **TransactionsViewModel** - Transaction history
4. **RemoveViewModel** - Item removal operations
5. **TransferViewModel** - Transfer operations
6. **SettingsViewModel** - Application settings

### Binding Patterns:
```csharp
// In XAML
<Entry Text="{Binding PartId}" />
<Button Command="{Binding SaveCommand}" />
<CollectionView ItemsSource="{Binding InventoryItems}" />

// In ViewModel
[ObservableProperty]
private ObservableCollection<Model_Inventory> inventoryItems = new();

[RelayCommand]
private async Task RefreshInventoryAsync()
{
    var dao = ServiceProvider.GetService<Dao_Inventory>();
    var results = await dao.GetInventoryByPartIdAsync(PartId);
    InventoryItems.Clear();
    foreach (var item in results)
    {
        InventoryItems.Add(item);
    }
}
```

## Data Binding Strategy

1. **Two-Way Binding**: Use for form inputs
2. **Commands**: Use RelayCommand for all user actions
3. **Collections**: Use ObservableCollection for lists
4. **Validation**: Implement INotifyDataErrorInfo for form validation

## Database Integration

- **Keep all existing DAO classes**: They work perfectly with MAUI
- **Use dependency injection**: Inject DAOs into ViewModels
- **Maintain async patterns**: All database operations remain async
- **Preserve error handling**: Use existing error handling patterns