using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class RemoveItemTypeControl : UserControl
{
    #region Fields
    

    #region Events

    public event EventHandler? ItemTypeRemoved;

    #endregion

    #region Fields

    private DataRow? _currentItemType;
    
    #endregion
    
    #region Constructors
    

    #endregion

    #region Constructors

    public RemoveItemTypeControl()
    {
        InitializeComponent();
    }
    
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
                issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
            await LoadItemTypes();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "SettingsForm / RemoveItemTypeControl_OnLoadOverRide");
        }
    }

    private async Task LoadItemTypes()
    {
        try
        {
            var itemTypes = await Dao_ItemType.GetAllItemTypes();
            itemTypesComboBox.Items.Clear();
            itemTypesComboBox.Items.Add("Select ItemType to Remove");
            foreach (DataRow row in itemTypes.Rows)
            {
                var itemType = row["ItemType"]?.ToString();
                if (!string.IsNullOrWhiteSpace(itemType))
                    itemTypesComboBox.Items.Add(itemType);
            }

            itemTypesComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading ItemTypes: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    public async Task ReloadComboBoxDataAsync()
    {
        await LoadItemTypes();
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
            var selectedType = itemTypesComboBox.SelectedItem?.ToString() ?? string.Empty;
            _currentItemType = await Dao_ItemType.GetItemTypeByName(selectedType);
            if (_currentItemType != null)
            {
                LoadItemTypeData();
                SetFormEnabled(true);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading ItemType data: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void RemoveButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (_currentItemType == null)
            {
                MessageBox.Show("No ItemType selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var itemType = _currentItemType["ItemType"]?.ToString();
            if (string.IsNullOrEmpty(itemType))
            {
                MessageBox.Show("Invalid ItemType selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to remove the ItemType '{itemType}'?{Environment.NewLine}{Environment.NewLine}This action cannot be undone.",
                "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                await Dao_ItemType.DeleteItemType(itemType);
                await LoadItemTypes();
                ClearForm();
                SetFormEnabled(false);
                ItemTypeRemoved?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("ItemType removed successfully.", "Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error removing ItemType: {ex.Message}", "Error", MessageBoxButtons.OK,
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
        if (_currentItemType == null) return;
        itemTypeValueLabel.Text = _currentItemType["ItemType"]?.ToString() ?? string.Empty;
        originalIssuedByValueLabel.Text = _currentItemType["IssuedBy"]?.ToString() ?? string.Empty;
    }

    private void SetFormEnabled(bool enabled)
    {
        removeButton.Enabled = enabled;
    }

    private void ClearForm()
    {
        itemTypeValueLabel.Text = string.Empty;
        originalIssuedByValueLabel.Text = string.Empty;
    }

    #endregion

    
    #endregion
}