using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;
using System.Data;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class AddPartControl : UserControl
{
    public event EventHandler? PartAdded;
    
    public AddPartControl()
    {
        InitializeComponent();
        LoadPartTypes();
    }

    private async void LoadPartTypes()
    {
        try
        {
            // Load part types from md_item_types table
            var partTypes = await GetPartTypesAsync();
            typeComboBox.Items.Clear();
            typeComboBox.Items.Add("Select Type");
            
            foreach (DataRow row in partTypes.Rows)
            {
                typeComboBox.Items.Add(row["Type"].ToString());
            }
            
            typeComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading part types: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task<DataTable> GetPartTypesAsync()
    {
        return await Dao_Part.GetPartTypes();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // Set the current user when the control loads
        if (issuedByValueLabel != null)
        {
            issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
        }
    }

    private async void saveButton_Click(object sender, EventArgs e)
    {
        try
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(itemNumberTextBox.Text))
            {
                MessageBox.Show("Item Number is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                itemNumberTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(customerTextBox.Text))
            {
                MessageBox.Show("Customer is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                customerTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                MessageBox.Show("Description is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                descriptionTextBox.Focus();
                return;
            }

            if (typeComboBox.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a part type.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                typeComboBox.Focus();
                return;
            }

            // Check for duplicate part number
            if (await Dao_Part.PartExists(itemNumberTextBox.Text.Trim()))
            {
                MessageBox.Show($"Part number '{itemNumberTextBox.Text.Trim()}' already exists.", 
                    "Duplicate Part Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                itemNumberTextBox.Focus();
                return;
            }

            // Add the part using stored procedure
            await AddPartAsync();
            
            MessageBox.Show("Part added successfully!", "Success", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear the form
            ClearForm();
            
            // Notify parent that part was added
            PartAdded?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error adding part: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task AddPartAsync()
    {
        var itemNumber = itemNumberTextBox.Text.Trim();
        var customer = customerTextBox.Text.Trim();
        var description = descriptionTextBox.Text.Trim();
        var issuedBy = Model_AppVariables.User;
        var type = typeComboBox.SelectedItem.ToString();

        // This will need to be implemented in Dao_Part to use the stored procedure
        // md_part_ids_Add_Part
        await Dao_Part.AddPartWithStoredProcedure(itemNumber, customer, description, issuedBy, type);
    }

    private void ClearForm()
    {
        itemNumberTextBox.Clear();
        customerTextBox.Clear();
        descriptionTextBox.Clear();
        typeComboBox.SelectedIndex = 0;
        itemNumberTextBox.Focus();
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
        ClearForm();
    }
}