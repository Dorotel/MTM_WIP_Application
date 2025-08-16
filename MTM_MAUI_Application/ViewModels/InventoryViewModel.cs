using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MTM_MAUI_Application.Models;
using MTM_MAUI_Application.Services;
using System.Collections.ObjectModel;

namespace MTM_MAUI_Application.ViewModels;

/// <summary>
/// ViewModel for the Inventory page
/// </summary>
public partial class InventoryViewModel : ObservableObject
{
    private readonly IInventoryService _inventoryService;
    private readonly IPartService _partService;
    private readonly ILocationService _locationService;
    private readonly IOperationService _operationService;
    private readonly IUserService _userService;
    private readonly IErrorHandlerService _errorHandler;

    [ObservableProperty]
    private string selectedPartId = string.Empty;

    [ObservableProperty]
    private string selectedLocation = string.Empty;

    [ObservableProperty]
    private string selectedOperation = string.Empty;

    [ObservableProperty]
    private int quantity;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private string batchNumber = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool canSave;

    public ObservableCollection<string> Parts { get; } = new();
    public ObservableCollection<string> Locations { get; } = new();
    public ObservableCollection<string> Operations { get; } = new();
    public ObservableCollection<InventoryItem> CurrentInventory { get; } = new();

    public InventoryViewModel(
        IInventoryService inventoryService,
        IPartService partService,
        ILocationService locationService,
        IOperationService operationService,
        IUserService userService,
        IErrorHandlerService errorHandler)
    {
        _inventoryService = inventoryService;
        _partService = partService;
        _locationService = locationService;
        _operationService = operationService;
        _userService = userService;
        _errorHandler = errorHandler;
        
        PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectedPartId) || 
            e.PropertyName == nameof(SelectedLocation) || 
            e.PropertyName == nameof(SelectedOperation) || 
            e.PropertyName == nameof(Quantity))
        {
            UpdateCanSave();
        }

        if (e.PropertyName == nameof(SelectedPartId) && !string.IsNullOrEmpty(SelectedPartId))
        {
            _ = LoadCurrentInventoryAsync();
        }
    }

    private void UpdateCanSave()
    {
        CanSave = !string.IsNullOrEmpty(SelectedPartId) &&
                  !string.IsNullOrEmpty(SelectedLocation) &&
                  !string.IsNullOrEmpty(SelectedOperation) &&
                  Quantity > 0 &&
                  !IsLoading;
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;

            // Load all master data
            var parts = await _partService.GetAllPartsAsync();
            var locations = await _locationService.GetAllLocationsAsync();
            var operations = await _operationService.GetAllOperationsAsync();

            // Update collections
            Parts.Clear();
            foreach (var part in parts)
            {
                Parts.Add(part.PartNumber);
            }

            Locations.Clear();
            foreach (var location in locations)
            {
                Locations.Add(location.Name);
            }

            Operations.Clear();
            foreach (var operation in operations)
            {
                Operations.Add(operation.Number);
            }
        }
        catch (Exception ex)
        {
            await _errorHandler.HandleExceptionAsync(ex, nameof(LoadDataAsync));
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task SaveInventoryAsync()
    {
        if (!CanSave) return;

        try
        {
            IsLoading = true;

            var currentUser = await _userService.GetCurrentUserAsync();
            var item = new InventoryItem
            {
                PartId = SelectedPartId,
                Location = SelectedLocation,
                Operation = SelectedOperation,
                Quantity = Quantity,
                Notes = Notes,
                BatchNumber = BatchNumber,
                User = currentUser?.Username ?? "Unknown",
                ItemType = "Production" // Default item type
            };

            var success = await _inventoryService.AddInventoryItemAsync(item);

            if (success)
            {
                await _errorHandler.ShowInfoAsync("Success", "Inventory item added successfully.");
                ResetForm();
                await LoadCurrentInventoryAsync();
            }
            else
            {
                await _errorHandler.ShowErrorAsync("Error", "Failed to add inventory item.");
            }
        }
        catch (Exception ex)
        {
            await _errorHandler.HandleExceptionAsync(ex, nameof(SaveInventoryAsync));
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public void ResetForm()
    {
        SelectedPartId = string.Empty;
        SelectedLocation = string.Empty;
        SelectedOperation = string.Empty;
        Quantity = 0;
        Notes = string.Empty;
        BatchNumber = string.Empty;
        CurrentInventory.Clear();
    }

    private async Task LoadCurrentInventoryAsync()
    {
        if (string.IsNullOrEmpty(SelectedPartId)) return;

        try
        {
            var inventory = await _inventoryService.GetInventoryByPartIdAsync(SelectedPartId);
            
            CurrentInventory.Clear();
            foreach (var item in inventory)
            {
                CurrentInventory.Add(item);
            }
        }
        catch (Exception ex)
        {
            await _errorHandler.HandleExceptionAsync(ex, nameof(LoadCurrentInventoryAsync));
        }
    }
}