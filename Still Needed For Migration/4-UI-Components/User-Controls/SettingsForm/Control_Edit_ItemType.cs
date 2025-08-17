using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Edit_ItemType : UserControl
    {
        #region Events

        public event EventHandler? ItemTypeUpdated;

        #endregion

        #region Fields

        private DataRow? _currentItemType;

        #endregion

        #region Constructors

        public Control_Edit_ItemType() => InitializeComponent();

        #endregion

        #region Initialization

        private async void LoadItemTypes()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillItemTypeComboBoxesAsync(itemTypesComboBox);
            }
            catch (Exception ex)
            {
                // Log and rethrow to be caught in OnLoad
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

                LoadItemTypes();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    "SettingsForm / EditItemTypeControl_OnLoadOverRide");
            }
        }

        #endregion

        #region Event Handlers

        private async void ItemTypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (itemTypesComboBox.SelectedIndex <= 0)
            {
                ClearForm();
                SetFormEnabled(false);
                return;
            }

            try
            {
                string? selectedType = itemTypesComboBox.Text;
                if (string.IsNullOrEmpty(selectedType))
                {
                    MessageBox.Show(@"Invalid selection.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _currentItemType = await Dao_ItemType.GetItemTypeByName(selectedType);
                if (_currentItemType != null)
                {
                    LoadItemTypeData();
                    SetFormEnabled(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error loading ItemType data: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentItemType == null)
                {
                    MessageBox.Show(@"No ItemType selected.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(itemTypeTextBox.Text))
                {
                    MessageBox.Show(@"ItemType is required.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    itemTypeTextBox.Focus();
                    return;
                }

                string newItemType = itemTypeTextBox.Text.Trim();
                string? currentItemType = _currentItemType["ItemType"]?.ToString();
                if (!string.Equals(newItemType, currentItemType, StringComparison.OrdinalIgnoreCase))
                {
                    if (await Dao_ItemType.ItemTypeExists(newItemType))
                    {
                        MessageBox.Show($@"ItemType '{newItemType}' already exists.", @"Duplicate ItemType",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        itemTypeTextBox.Focus();
                        return;
                    }
                }

                int itemTypeId = Convert.ToInt32(_currentItemType["ID"]);
                await Dao_ItemType.UpdateItemType(itemTypeId, newItemType, Model_AppVariables.User ?? "Current User");
                LoadItemTypes();
                ClearForm();
                SetFormEnabled(false);
                ItemTypeUpdated?.Invoke(this, EventArgs.Empty);
                MessageBox.Show(@"ItemType updated successfully.", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error updating ItemType: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetFormEnabled(false);
            itemTypesComboBox.SelectedIndex = 0;
        }

        #endregion

        #region Methods

        private void LoadItemTypeData()
        {
            if (_currentItemType == null)
            {
                return;
            }

            itemTypeTextBox.Text = _currentItemType["ItemType"]?.ToString() ?? string.Empty;
            originalIssuedByValueLabel.Text = _currentItemType["IssuedBy"]?.ToString() ?? string.Empty;
        }

        private void SetFormEnabled(bool enabled)
        {
            itemTypeTextBox.Enabled = enabled;
            saveButton.Enabled = enabled;
        }

        private void ClearForm()
        {
            itemTypeTextBox.Clear();
            originalIssuedByValueLabel.Text = string.Empty;
        }

        #endregion
    }
}
