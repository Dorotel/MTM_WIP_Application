using System.Data;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class EditLocationControl : UserControl
{
    #region Fields
    

    #region Events

    public event EventHandler? LocationUpdated;

    #endregion

    #region Fields

    private DataRow? _currentLocation;
    
    #endregion
    
    #region Constructors
    

    #endregion

    #region Constructors

    public EditLocationControl()
    {
        InitializeComponent();
        LoadBuildingOptions();
    }
    
    #endregion
    
    #region Methods
    

    #endregion

    #region Initialization

    private void LoadBuildingOptions()
    {
        buildingComboBox.Items.Clear();
        buildingComboBox.Items.Add("Select Building");
        buildingComboBox.Items.Add("Expo");
        buildingComboBox.Items.Add("Vits");
        buildingComboBox.SelectedIndex = 0;
    }

    private async Task LoadLocations()
    {
        try
        {
            var locations = await Dao_Location.GetAllLocations();
            locationsComboBox.Items.Clear();
            locationsComboBox.Items.Add("Select Location to Edit");
            foreach (DataRow row in locations.Rows)
            {
                var location = row["Location"]?.ToString();
                if (!string.IsNullOrEmpty(location))
                    locationsComboBox.Items.Add(location);
            }

            locationsComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading locations: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    public async Task ReloadComboBoxDataAsync()
    {
        await LoadLocations();
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadLocations();
        if (issuedByValueLabel != null)
            issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
    }

    #endregion

    #region Event Handlers

    private async void LocationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (locationsComboBox.SelectedIndex <= 0 ||
            locationsComboBox.SelectedItem?.ToString() == "Select Location to Edit")
        {
            ClearForm();
            EnableControls(false);
            return;
        }

        try
        {
            var selectedLocation = locationsComboBox.SelectedItem?.ToString();
            _currentLocation = await Dao_Location.GetLocationByName(selectedLocation ?? string.Empty);
            if (_currentLocation != null)
            {
                locationTextBox.Text = _currentLocation["Location"]?.ToString() ?? string.Empty;
                var building = _currentLocation["Building"]?.ToString() ?? string.Empty;
                for (var i = 0; i < buildingComboBox.Items.Count; i++)
                    if (buildingComboBox.Items[i]?.ToString() == building)
                    {
                        buildingComboBox.SelectedIndex = i;
                        break;
                    }

                issuedByValueLabel.Text = _currentLocation["IssuedBy"]?.ToString() ?? "Current User";
                EnableControls(true);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading location details: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void SaveButton_Click(object sender, EventArgs e)
    {
        if (_currentLocation == null)
        {
            MessageBox.Show("Please select a location to edit.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            if (string.IsNullOrWhiteSpace(locationTextBox.Text))
            {
                MessageBox.Show("Location is required.", "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                locationTextBox.Focus();
                return;
            }

            if (buildingComboBox.SelectedIndex <= 0 || buildingComboBox.SelectedItem?.ToString() == "Select Building")
            {
                MessageBox.Show("Building is required.", "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                buildingComboBox.Focus();
                return;
            }

            var newLocation = locationTextBox.Text.Trim();
            var originalLocation = _currentLocation["Location"]?.ToString() ?? string.Empty;
            var updatedBy = Core_WipAppVariables.User;
            if (newLocation != originalLocation && await Dao_Location.LocationExists(newLocation))
            {
                MessageBox.Show($"Location '{newLocation}' already exists.", "Duplicate Location", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                locationTextBox.Focus();
                return;
            }

            await Dao_Location.UpdateLocation(originalLocation, newLocation, updatedBy, true);
            await LoadLocations();
            ClearForm();
            EnableControls(false);
            LocationUpdated?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Location updated successfully!", "Success", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating location: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
        ClearForm();
        EnableControls(false);
    }

    #endregion

    #region Methods

    private void ClearForm()
    {
        locationTextBox.Clear();
        buildingComboBox.SelectedIndex = 0;
        locationsComboBox.SelectedIndex = 0;
        issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
        _currentLocation = null;
    }

    private void EnableControls(bool enabled)
    {
        locationTextBox.Enabled = enabled;
        buildingComboBox.Enabled = enabled;
        saveButton.Enabled = enabled;
        cancelButton.Enabled = enabled;
    }

    #endregion

    
    #endregion
}