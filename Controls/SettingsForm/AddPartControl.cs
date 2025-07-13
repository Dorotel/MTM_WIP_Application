using System;
using System.Data;
using System.Windows.Forms;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class AddPartControl : UserControl
{
    #region Events

    public event EventHandler? PartAdded;

    #endregion

    #region Constructors

    public AddPartControl()
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
            var partTypes = await GetPartTypesAsync();
            typeComboBox.Items.Clear();
            typeComboBox.Items.Add("Select Type");
            foreach (DataRow row in partTypes.Rows)
                typeComboBox.Items.Add(row["ItemType"]?.ToString() ?? string.Empty);
            typeComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading part types: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private static async Task<DataTable> GetPartTypesAsync()
    {
        return await Dao_Part.GetPartTypes();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (issuedByValueLabel != null)
            issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
    }

    #endregion

    #region Event Handlers

    private async void SaveButton_Click(object sender, EventArgs e)
    {
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

            if (await Dao_Part.PartExists(itemNumberTextBox.Text.Trim()))
            {
                MessageBox.Show($"Part number '{itemNumberTextBox.Text.Trim()}' already exists.",
                    "Duplicate Part Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                itemNumberTextBox.Focus();
                return;
            }

            await AddPartAsync();
            MessageBox.Show("Part added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
            PartAdded?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error adding part: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task AddPartAsync()
    {
        var itemNumber = itemNumberTextBox.Text.Trim();
        var customer = customerTextBox.Text.Trim();
        var description = descriptionTextBox.Text.Trim();
        var issuedBy = Model_AppVariables.User;
        var type = typeComboBox.SelectedItem?.ToString() ?? string.Empty;
        await Dao_Part.AddPartWithStoredProcedure(itemNumber, customer, description, issuedBy, type);
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    #endregion

    #region Methods

    private void ClearForm()
    {
        itemNumberTextBox.Clear();
        customerTextBox.Clear();
        descriptionTextBox.Clear();
        typeComboBox.SelectedIndex = 0;
        itemNumberTextBox.Focus();
    }

    #endregion
}