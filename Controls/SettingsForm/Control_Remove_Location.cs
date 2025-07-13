using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Remove_Location : UserControl
    {
        #region Events

        public event EventHandler? LocationRemoved;

        #endregion

        #region Fields

        private DataRow? _currentLocation;

        #endregion

        #region Constructors

        public Control_Remove_Location() => InitializeComponent();

        #endregion

        #region Initialization

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadLocations();
        }

        private async void LoadLocations()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(locationsComboBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading part types: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        private async void LocationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (locationsComboBox.SelectedIndex <= 0 ||
                locationsComboBox.Text == "Select Location to Remove")
            {
                ClearForm();
                EnableControls(false);
                return;
            }

            try
            {
                string? selectedLocation = locationsComboBox.Text;
                _currentLocation = await Dao_Location.GetLocationByName(selectedLocation ?? string.Empty);
                if (_currentLocation != null)
                {
                    locationValueLabel.Text = _currentLocation["Location"]?.ToString() ?? string.Empty;
                    buildingValueLabel.Text = _currentLocation["Building"]?.ToString() ?? string.Empty;
                    issuedByValueLabel.Text = _currentLocation["IssuedBy"]?.ToString() ?? "Unknown";
                    EnableControls(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading location details: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void RemoveButton_Click(object sender, EventArgs e)
        {
            if (_currentLocation == null)
            {
                MessageBox.Show("Please select a location to remove.", "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string location = _currentLocation["Location"]?.ToString() ?? string.Empty;
            DialogResult result =
                MessageBox.Show(
                    $"Are you sure you want to remove the location '{location}'?{Environment.NewLine}{Environment.NewLine}This action cannot be undone.",
                    "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                await Dao_Location.DeleteLocation(location);
                LoadLocations();
                ClearForm();
                EnableControls(false);
                LocationRemoved?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Location removed successfully!", "Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing location: {ex.Message}", "Error", MessageBoxButtons.OK,
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

        #endregion
    }
}
