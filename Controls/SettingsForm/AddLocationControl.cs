using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class AddLocationControl : UserControl
{
    public event EventHandler? LocationAdded;

    public AddLocationControl()
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

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // Set the current user when the control loads
        if (issuedByValueLabel != null) issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
    }

    private async void SaveButton_Click(object sender, EventArgs e)
    {
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

            var location = locationTextBox.Text.Trim();
            var building = buildingComboBox.SelectedItem?.ToString() ?? string.Empty;

            // Check if location already exists
            if (await Dao_Location.LocationExists(location))
            {
                MessageBox.Show($@"Location '{location}' already exists.", @"Duplicate Location",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                locationTextBox.Focus();
                return;
            }

            // Insert the location
            // Update the method call to pass the correct arguments
            await Dao_Location.InsertLocation(location, building, true);

            // Clear the form
            ClearForm();

            // Notify parent
            LocationAdded?.Invoke(this, EventArgs.Empty);

            MessageBox.Show(@"Location added successfully!", @"Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error adding location: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        locationTextBox.Clear();
        buildingComboBox.SelectedIndex = 0;
        locationTextBox.Focus();
    }
}