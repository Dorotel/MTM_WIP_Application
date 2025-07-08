using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;
using System.Data;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class EditLocationControl : UserControl
{
    public event EventHandler? LocationUpdated;
    private DataRow? _currentLocation;

    public EditLocationControl()
    {
        InitializeComponent();
        LoadBuildingOptions();
    }

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
                {
                    locationsComboBox.Items.Add(location);
                }
            }

            locationsComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error loading locations: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadLocations();
        
        // Set the current user when the control loads
        if (issuedByValueLabel != null) issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
    }

    private async void LocationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (locationsComboBox.SelectedIndex <= 0 || locationsComboBox.SelectedItem?.ToString() == "Select Location to Edit")
        {
            ClearForm();
            return;
        }

        try
        {
            var selectedLocation = locationsComboBox.SelectedItem.ToString();
            _currentLocation = await Dao_Location.GetLocationByName(selectedLocation ?? string.Empty);

            if (_currentLocation != null)
            {
                locationTextBox.Text = _currentLocation["Location"]?.ToString() ?? string.Empty;
                var building = _currentLocation["Building"]?.ToString() ?? string.Empty;
                
                // Set building combo box
                for (int i = 0; i < buildingComboBox.Items.Count; i++)
                {
                    if (buildingComboBox.Items[i].ToString() == building)
                    {
                        buildingComboBox.SelectedIndex = i;
                        break;
                    }
                }
                
                issuedByValueLabel.Text = _currentLocation["Issued By"]?.ToString() ?? "Current User";
                EnableControls(true);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error loading location details: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void SaveButton_Click(object sender, EventArgs e)
    {
        if (_currentLocation == null)
        {
            MessageBox.Show(@"Please select a location to edit.", @"Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(locationTextBox.Text))
            {
                MessageBox.Show(@"Location is required.", @"Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                locationTextBox.Focus();
                return;
            }

            if (buildingComboBox.SelectedIndex <= 0 || buildingComboBox.SelectedItem?.ToString() == "Select Building")
            {
                MessageBox.Show(@"Building is required.", @"Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                buildingComboBox.Focus();
                return;
            }

            var newLocation = locationTextBox.Text.Trim();
            var building = buildingComboBox.SelectedItem?.ToString() ?? string.Empty;
            var originalLocation = _currentLocation["Location"]?.ToString() ?? string.Empty;

            // Check if location already exists (if changed)
            if (newLocation != originalLocation && await Dao_Location.LocationExists(newLocation))
            {
                MessageBox.Show($@"Location '{newLocation}' already exists.", @"Duplicate Location",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                locationTextBox.Focus();
                return;
            }

            // Update the location
            await Dao_Location.UpdateLocation(originalLocation, newLocation, building, Model_AppVariables.User ?? "Current User");

            // Refresh the dropdown
            await LoadLocations();

            // Clear the form
            ClearForm();
            EnableControls(false);

            // Notify parent
            LocationUpdated?.Invoke(this, EventArgs.Empty);

            MessageBox.Show(@"Location updated successfully!", @"Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error updating location: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
        ClearForm();
        EnableControls(false);
    }

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
}