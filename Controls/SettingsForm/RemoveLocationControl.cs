using MTM_Inventory_Application.Data;
using System.Data;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class RemoveLocationControl : UserControl
{
    public event EventHandler? LocationRemoved;
    private DataRow? _currentLocation;

    public RemoveLocationControl()
    {
        InitializeComponent();
    }

    private async Task LoadLocations()
    {
        try
        {
            var locations = await Dao_Location.GetAllLocations();
            locationsComboBox.Items.Clear();
            locationsComboBox.Items.Add("Select Location to Remove");

            foreach (DataRow row in locations.Rows)
            {
                var location = row["Location"]?.ToString();
                if (!string.IsNullOrEmpty(location)) locationsComboBox.Items.Add(location);
            }

            locationsComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error loading locations: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }

    private async void LocationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (locationsComboBox.SelectedIndex <= 0 ||
            locationsComboBox.SelectedItem?.ToString() == "Select Location to Remove")
        {
            ClearForm();
            return;
        }

        try
        {
            var selectedLocation = locationsComboBox.SelectedItem?.ToString();
            _currentLocation = await Dao_Location.GetLocationByName(selectedLocation ?? string.Empty);

            if (_currentLocation != null)
            {
                locationValueLabel.Text = _currentLocation["Location"]?.ToString() ?? string.Empty;
                buildingValueLabel.Text = _currentLocation["Building"]?.ToString() ?? string.Empty;
                issuedByValueLabel.Text = _currentLocation["Issued By"]?.ToString() ?? "Unknown";
                EnableControls(true);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error loading location details: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void RemoveButton_Click(object sender, EventArgs e)
    {
        if (_currentLocation == null)
        {
            MessageBox.Show(@"Please select a location to remove.", @"Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var location = _currentLocation["Location"]?.ToString() ?? string.Empty;

        var result = MessageBox.Show(
            $@"Are you sure you want to remove the location '{location}'?{Environment.NewLine}{Environment.NewLine}This action cannot be undone.",
            @"Confirm Removal",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
            return;

        try
        {
            await Dao_Location.DeleteLocation(location);

            // Refresh the dropdown
            await LoadLocations();

            // Clear the form
            ClearForm();
            EnableControls(false);

            // Notify parent
            LocationRemoved?.Invoke(this, EventArgs.Empty);

            MessageBox.Show(@"Location removed successfully!", @"Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error removing location: {ex.Message}", @"Error",
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
        locationValueLabel.Text = string.Empty;
        buildingValueLabel.Text = string.Empty;
        issuedByValueLabel.Text = string.Empty;
        locationsComboBox.SelectedIndex = 0;
        _currentLocation = null;
    }

    private void EnableControls(bool enabled)
    {
        removeButton.Enabled = enabled;
        cancelButton.Enabled = enabled;
    }
}