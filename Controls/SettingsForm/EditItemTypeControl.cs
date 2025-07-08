using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using System.Data;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class EditItemTypeControl : UserControl
{
    public event EventHandler? ItemTypeUpdated;
    private DataRow? _currentItemType;

    public EditItemTypeControl()
    {
        InitializeComponent();
    }

    private async Task LoadItemTypes()
    {
        try
        {
            var itemTypes = await Dao_ItemType.GetAllItemTypes();
            itemTypesComboBox.Items.Clear();
            itemTypesComboBox.Items.Add("Select Item Type to Edit");

            foreach (DataRow row in itemTypes.Rows)
            {
                var itemType = row["Type"]?.ToString();
                if (!string.IsNullOrWhiteSpace(itemType))
                    itemTypesComboBox.Items.Add(itemType);
            }

            itemTypesComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error loading item types: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            if (issuedByValueLabel != null) issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
            await LoadItemTypes();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "SettingsForm / " + "EditItemTypeControl_OnLoadOverRide");
        }
    }

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
            var selectedType = itemTypesComboBox.SelectedItem?.ToString();
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
            MessageBox.Show($@"Error loading item type data: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadItemTypeData()
    {
        if (_currentItemType == null) return;

        itemTypeTextBox.Text = _currentItemType["Type"]?.ToString() ?? string.Empty;
        originalIssuedByValueLabel.Text = _currentItemType["Issued By"]?.ToString() ?? string.Empty;
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

    private async void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (_currentItemType == null)
            {
                MessageBox.Show(@"No item type selected.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate input
            if (string.IsNullOrWhiteSpace(itemTypeTextBox.Text))
            {
                MessageBox.Show(@"Item type is required.", @"Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                itemTypeTextBox.Focus();
                return;
            }

            var newItemType = itemTypeTextBox.Text.Trim();
            var currentItemType = _currentItemType["Type"]?.ToString();

            // Check if the new item type already exists (unless it's the same as current)
            if (!string.Equals(newItemType, currentItemType, StringComparison.OrdinalIgnoreCase))
            {
                if (await Dao_ItemType.ItemTypeExists(newItemType))
                {
                    MessageBox.Show($@"Item type '{newItemType}' already exists.", @"Duplicate Item Type",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    itemTypeTextBox.Focus();
                    return;
                }
            }

            // Update the item type
            var itemTypeId = Convert.ToInt32(_currentItemType["ID"]);
            await Dao_ItemType.UpdateItemType(itemTypeId, newItemType, Model_AppVariables.User ?? "Current User");

            // Refresh the combo box
            await LoadItemTypes();

            // Clear the form
            ClearForm();
            SetFormEnabled(false);

            // Notify that item type was updated
            ItemTypeUpdated?.Invoke(this, EventArgs.Empty);

            MessageBox.Show(@"Item type updated successfully.", @"Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error updating item type: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
        ClearForm();
        SetFormEnabled(false);
        itemTypesComboBox.SelectedIndex = 0;
    }
}