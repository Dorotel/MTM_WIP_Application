using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class AddItemTypeControl : UserControl
{
    public event EventHandler? ItemTypeAdded;

    public AddItemTypeControl()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // Set the current user when the control loads
        if (issuedByValueLabel != null) issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
    }

    private async void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(itemTypeTextBox.Text))
            {
                MessageBox.Show(@"Item type is required.", @"Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                itemTypeTextBox.Focus();
                return;
            }

            var itemType = itemTypeTextBox.Text.Trim();

            // Check if item type already exists
            if (await Dao_ItemType.ItemTypeExists(itemType))
            {
                MessageBox.Show($@"Item type '{itemType}' already exists.", @"Duplicate Item Type",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                itemTypeTextBox.Focus();
                return;
            }

            // Insert the item type
            await Dao_ItemType.InsertItemType(itemType, Model_AppVariables.User ?? "Current User");

            // Clear the form
            ClearForm();

            // Notify parent
            ItemTypeAdded?.Invoke(this, EventArgs.Empty);

            MessageBox.Show(@"Item type added successfully!", @"Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error adding item type: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        itemTypeTextBox.Clear();
        itemTypeTextBox.Focus();
    }
}