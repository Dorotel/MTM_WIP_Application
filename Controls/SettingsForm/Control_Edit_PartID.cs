using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Edit_PartID : UserControl
    {
        #region Fields

        #region Events

        public event EventHandler? PartUpdated;

        #endregion

        #region Fields

        private DataRow? _currentPart;

        #endregion

        #region Constructors

        public Control_Edit_PartID()
        {
            InitializeComponent();
            Control_Edit_PartID_ComboBox_Part.SelectedIndexChanged +=
                Control_Edit_PartID_ComboBox_Part_SelectedIndexChanged;
            saveButton.Click += SaveButton_Click;
            cancelButton.Click += CancelButton_Click;
            LoadPartTypes();
        }

        #endregion

        #region Methods

        #endregion

        #region Initialization

        private async void LoadPartTypes()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillItemTypeComboBoxesAsync(Control_Edit_PartID_ComboBox_ItemType);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error loading part types: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
                MessageBox.Show($@"Error loading parts: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    "SettingsForm / EditPartControl_OnLoadOverRide");
            }
        }

        #endregion

        #region Event Handlers

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
                    MessageBox.Show(@"Invalid selection.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($@"Error loading part data: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
                    MessageBox.Show(@"Item Number is required.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
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
                    MessageBox.Show(@"Description is required.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    descriptionTextBox.Focus();
                    return;
                }

                if (Control_Edit_PartID_ComboBox_ItemType.SelectedIndex <= 0)
                {
                    MessageBox.Show(@"Please select a part type.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Edit_PartID_ComboBox_ItemType.Focus();
                    return;
                }

                string? originalItemNumber = _currentPart["PartID"].ToString();
                string newItemNumber = itemNumberTextBox.Text.Trim();
                if (originalItemNumber != newItemNumber && await Dao_Part.PartExists(newItemNumber))
                {
                    MessageBox.Show($@"Part number '{newItemNumber}' already exists.", @"Duplicate Part Number",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    itemNumberTextBox.Focus();
                    return;
                }

                await UpdatePartAsync();
                MessageBox.Show(@"Part updated successfully!", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                LoadParts();
                PartUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error updating part: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

        #endregion
    }
}
