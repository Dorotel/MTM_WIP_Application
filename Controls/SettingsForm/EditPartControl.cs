// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class EditPartControl : UserControl
{
    #region Events

    public event EventHandler? PartUpdated;

    #endregion

    #region Fields

    private DataRow? _currentPart;

    #endregion

    #region Constructors

    public EditPartControl()
    {
        InitializeComponent();
        LoadPartTypes();
    }

    #endregion

    #region Initialization

    private async void LoadPartTypes()
    {
        try
        {
            var partTypes = await Dao_Part.GetPartTypes();
            typeComboBox.Items.Clear();
            typeComboBox.Items.Add("Select Type");
            foreach (DataRow row in partTypes.Rows)
                if (row["ItemType"] is string type)
                    typeComboBox.Items.Add(type);
            typeComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading part types: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async Task LoadParts()
    {
        try
        {
            var parts = await Dao_Part.GetAllParts();
            partsComboBox.Items.Clear();
            partsComboBox.Items.Add("Select Part to Edit");
            foreach (DataRow row in parts.Rows)
            {
                var itemNumber = row["PartID"]?.ToString();
                var customer = row["Customer"]?.ToString();
                if (string.IsNullOrWhiteSpace(customer))
                    customer = "[ No Customer ]";
                partsComboBox.Items.Add($"{itemNumber} - {customer}");
            }

            partsComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading parts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            if (issuedByValueLabel != null)
                issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
            await LoadParts();
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
            var selectedText = partsComboBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedText))
            {
                MessageBox.Show("Invalid selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var itemNumber = selectedText.Split(" - ")[0];
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

    private async void SaveButton_Click(object sender, EventArgs e)
    {
        if (_currentPart == null) return;
        try
        {
            if (string.IsNullOrWhiteSpace(itemNumberTextBox.Text))
            {
                MessageBox.Show("Item Number is required.", "Validation Error", MessageBoxButtons.OK,
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
                MessageBox.Show("Description is required.", "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                descriptionTextBox.Focus();
                return;
            }

            if (typeComboBox.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a part type.", "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                typeComboBox.Focus();
                return;
            }

            var originalItemNumber = _currentPart["PartID"].ToString();
            var newItemNumber = itemNumberTextBox.Text.Trim();
            if (originalItemNumber != newItemNumber && await Dao_Part.PartExists(newItemNumber))
            {
                MessageBox.Show($"Part number '{newItemNumber}' already exists.", "Duplicate Part Number",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                itemNumberTextBox.Focus();
                return;
            }

            await UpdatePartAsync();
            MessageBox.Show("Part updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadParts();
            PartUpdated?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating part: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
        if (_currentPart != null)
            LoadPartData();
        else
            ClearForm();
    }

    #endregion

    #region Methods

    private void LoadPartData()
    {
        if (_currentPart == null) return;
        itemNumberTextBox.Text = _currentPart["PartID"].ToString();
        customerTextBox.Text = _currentPart["Customer"].ToString();
        descriptionTextBox.Text = _currentPart["Description"].ToString();
        var partType = _currentPart["ItemType"].ToString();
        var typeIndex = typeComboBox.Items.IndexOf(partType);
        typeComboBox.SelectedIndex = typeIndex >= 0 ? typeIndex : 0;
        issuedByValueLabel.Text = _currentPart["IssuedBy"].ToString();
    }

    private void SetFormEnabled(bool enabled)
    {
        itemNumberTextBox.Enabled = enabled;
        customerTextBox.Enabled = enabled;
        descriptionTextBox.Enabled = enabled;
        typeComboBox.Enabled = enabled;
        saveButton.Enabled = enabled;
    }

    private async Task UpdatePartAsync()
    {
        if (_currentPart == null) return;
        var id = Convert.ToInt32(_currentPart["ID"]);
        var itemNumber = itemNumberTextBox.Text.Trim();
        var customer = customerTextBox.Text.Trim();
        var description = descriptionTextBox.Text.Trim();
        var issuedBy = Model_AppVariables.User;
        var type = typeComboBox.SelectedItem?.ToString() ?? string.Empty;
        await Dao_Part.UpdatePartWithStoredProcedure(id, itemNumber, customer, description, issuedBy, type);
    }

    private void ClearForm()
    {
        itemNumberTextBox.Clear();
        customerTextBox.Clear();
        descriptionTextBox.Clear();
        typeComboBox.SelectedIndex = 0;
        issuedByValueLabel.Text = "";
        _currentPart = null;
    }

    #endregion
}