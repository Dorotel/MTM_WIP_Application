using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Remove_PartID : UserControl
    {
        #region Fields

        #region Events

        public event EventHandler? PartRemoved;

        #endregion

        #region Fields

        private DataRow? _currentPart;

        #endregion

        #region Constructors

        #endregion

        #region Constructors

        public Control_Remove_PartID() => InitializeComponent();

        #endregion

        #region Methods

        #endregion

        #region Initialization

        protected override async void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                if (issuedByValueLabel != null)
                {
                    issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
                }

                LoadParts();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    "SettingsForm / RemovePartControl_OnLoadOverRide");
            }
        }

        private async void LoadParts()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(partsComboBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading part types: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        private async void PartsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (partsComboBox.SelectedIndex <= 0)
            {
                ClearForm();
                SetFormEnabled(false);
                return;
            }

            try
            {
                string selectedText = partsComboBox.Text;
                string itemNumber = selectedText;
                _currentPart = await Dao_Part.GetPartByNumber(itemNumber);
                if (_currentPart != null)
                {
                    LoadPartData();
                    SetFormEnabled(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading part data: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void RemoveButton_Click(object sender, EventArgs e)
        {
            if (_currentPart == null)
            {
                return;
            }

            string? itemNumber = _currentPart["PartID"]?.ToString();
            string? customer = _currentPart["Customer"]?.ToString();
            if (string.IsNullOrEmpty(itemNumber))
            {
                MessageBox.Show("Item number is missing. Cannot remove part.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            DialogResult result =
                MessageBox.Show(
                    $"Are you sure you want to remove part '{itemNumber}' for customer '{customer}'?\n\nThis action cannot be undone.",
                    "Confirm Part Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                await Dao_Part.DeletePartByItemNumber(itemNumber);
                MessageBox.Show("Part removed successfully!", "Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                LoadParts();
                ClearForm();
                SetFormEnabled(false);
                PartRemoved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing part: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            partsComboBox.SelectedIndex = 0;
            ClearForm();
            SetFormEnabled(false);
        }

        #endregion

        #region Methods

        private void LoadPartData()
        {
            if (_currentPart == null)
            {
                return;
            }

            itemNumberValueLabel.Text = _currentPart["PartID"].ToString();
            customerValueLabel.Text = _currentPart["Customer"].ToString();
            descriptionValueLabel.Text = _currentPart["Description"].ToString();
            typeValueLabel.Text = _currentPart["ItemType"].ToString();
            issuedByValueLabel.Text = _currentPart["IssuedBy"].ToString();
        }

        private void SetFormEnabled(bool enabled)
        {
            removeButton.Enabled = enabled;
            detailsGroupBox.Visible = enabled;
        }

        private void ClearForm()
        {
            itemNumberValueLabel.Text = "";
            customerValueLabel.Text = "";
            descriptionValueLabel.Text = "";
            typeValueLabel.Text = "";
            issuedByValueLabel.Text = "";
            _currentPart = null;
        }

        #endregion

        #endregion
    }
}
