using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Edit_PartID : UserControl
    {
        #region Fields

        private DataRow? _currentPart;

        #endregion

        #region Properties

        // Public properties would go here if needed

        #endregion

        #region Progress Control Methods

        // Progress control methods would go here if needed for this control

        #endregion

        #region Constructors

        public Control_Edit_PartID()
        {
            try
            {
                InitializeComponent();
                Control_Edit_PartID_ComboBox_Part.SelectedIndexChanged +=
                    Control_Edit_PartID_ComboBox_Part_SelectedIndexChanged;
                saveButton.Click += SaveButton_Click;
                cancelButton.Click += CancelButton_Click;
                LoadPartTypes();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, 
                    controlName: nameof(Control_Edit_PartID));
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Event for notifying when a part has been updated
        /// </summary>
        public event EventHandler? PartUpdated;

        private async void LoadPartTypes()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillItemTypeComboBoxesAsync(Control_Edit_PartID_ComboBox_ItemType);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(Control_Edit_PartID));
            }
        }

        private async void LoadParts()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_Edit_PartID_ComboBox_Part);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(Control_Edit_PartID), methodName: nameof(LoadParts));
            }
        }

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
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, 
                    controlName: nameof(Control_Edit_PartID), methodName: nameof(OnLoad));
            }
        }

        #endregion

        #region Key Processing

        // Keyboard shortcut processing would go here if needed
        // Currently not implemented for this settings control

        #endregion

        #region Button Clicks

        // Button click event handlers will be moved here

        #endregion

        #region ComboBox & UI Events

        private async void Control_Edit_PartID_ComboBox_Part_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Control_Edit_PartID_ComboBox_Part.SelectedIndex <= 0)
            {
                ClearForm();
                SetFormEnabled(false);
                return;
            }

            try
            {
                string? selectedText = Control_Edit_PartID_ComboBox_Part.Text;
                if (string.IsNullOrEmpty(selectedText))
                {
                    Service_ErrorHandler.HandleValidationError(
                        "Please select a valid part from the dropdown list.",
                        "Part Selection", controlName: nameof(Control_Edit_PartID));
                    return;
                }

                _currentPart = await Dao_Part.GetPartByNumber(selectedText);
                if (_currentPart != null)
                {
                    LoadPartData();
                    SetFormEnabled(true);
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleDatabaseError(ex, controlName: nameof(Control_Edit_PartID));
            }
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            if (_currentPart == null)
            {
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(itemNumberTextBox.Text))
                {
                    Service_ErrorHandler.HandleValidationError(
                        "Item Number is required to save the part.",
                        "Item Number", controlName: nameof(Control_Edit_PartID));
                    itemNumberTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(customerTextBox.Text))
                {
                    customerTextBox.Text = "[ No Customer ]";
                    return;
                }

                if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
                {
                    Service_ErrorHandler.HandleValidationError(
                        "Description is required to save the part.",
                        "Description", controlName: nameof(Control_Edit_PartID));
                    descriptionTextBox.Focus();
                    return;
                }

                if (Control_Edit_PartID_ComboBox_ItemType.SelectedIndex <= 0)
                {
                    Service_ErrorHandler.HandleValidationError(
                        "Please select a part type from the dropdown list.",
                        "Part Type", controlName: nameof(Control_Edit_PartID));
                    Control_Edit_PartID_ComboBox_ItemType.Focus();
                    return;
                }

                string? originalItemNumber = _currentPart["PartID"].ToString();
                string newItemNumber = itemNumberTextBox.Text.Trim();
                if (originalItemNumber != newItemNumber && await Dao_Part.PartExists(newItemNumber))
                {
                    Service_ErrorHandler.HandleValidationError(
                        $"Part number '{newItemNumber}' already exists. Please use a different part number.",
                        "Duplicate Part Number", controlName: nameof(Control_Edit_PartID));
                    itemNumberTextBox.Focus();
                    return;
                }

                await UpdatePartAsync();
                Service_ErrorHandler.ShowInformation(
                    "Part has been updated successfully!",
                    "Update Complete", controlName: nameof(Control_Edit_PartID));
                LoadParts();
                PartUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleDatabaseError(ex, controlName: nameof(Control_Edit_PartID));
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (_currentPart != null)
            {
                LoadPartData();
            }
            else
            {
                ClearForm();
            }
        }

        #endregion

        #region Methods

        private void LoadPartData()
        {
            if (_currentPart == null)
            {
                return;
            }

            itemNumberTextBox.Text = _currentPart["PartID"].ToString();
            customerTextBox.Text = _currentPart["Customer"].ToString();
            descriptionTextBox.Text = _currentPart["Description"].ToString();
            string? partType = _currentPart["ItemType"].ToString();

            int index = -1;
            for (int i = 0; i < Control_Edit_PartID_ComboBox_ItemType.Items.Count; i++)
            {
                if (Control_Edit_PartID_ComboBox_ItemType.GetItemText(Control_Edit_PartID_ComboBox_ItemType.Items[i]) ==
                    partType)
                {
                    index = i;
                    break;
                }
            }

            Control_Edit_PartID_ComboBox_ItemType.SelectedIndex = index > 0 ? index : 0;

            issuedByValueLabel.Text = _currentPart["IssuedBy"].ToString();
        }

        #endregion

        #region Helpers

        private void SetFormEnabled(bool enabled)
        {
            itemNumberTextBox.Enabled = enabled;
            customerTextBox.Enabled = enabled;
            descriptionTextBox.Enabled = enabled;
            Control_Edit_PartID_ComboBox_ItemType.Enabled = enabled;
            saveButton.Enabled = enabled;
        }

        private async Task UpdatePartAsync()
        {
            if (_currentPart == null)
            {
                return;
            }

            int id = Convert.ToInt32(_currentPart["ID"]);
            string itemNumber = itemNumberTextBox.Text.Trim();
            string customer = customerTextBox.Text.Trim();
            string description = descriptionTextBox.Text.Trim();
            string issuedBy = Model_AppVariables.User;
            string type = Control_Edit_PartID_ComboBox_ItemType.Text;
            await Dao_Part.UpdatePartWithStoredProcedure(id, itemNumber, customer, description, issuedBy, type);
        }

        private void ClearForm()
        {
            itemNumberTextBox.Clear();
            customerTextBox.Clear();
            descriptionTextBox.Clear();
            Control_Edit_PartID_ComboBox_ItemType.SelectedIndex = 0;
            issuedByValueLabel.Text = "";
            _currentPart = null;
        }

        #endregion

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    // Dispose managed resources if any
                }
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                    controlName: nameof(Control_Edit_PartID));
            }
        }

        #endregion
    }
}
