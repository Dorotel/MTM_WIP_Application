using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class AddOperationControl : UserControl
{
    public event EventHandler? OperationAdded;

    public AddOperationControl()
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
            if (string.IsNullOrWhiteSpace(operationTextBox.Text))
            {
                MessageBox.Show(@"Operation number is required.", @"Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                operationTextBox.Focus();
                return;
            }

            var operationNumber = operationTextBox.Text.Trim();

            // Check if operation number already exists
            if (await Dao_Operation.OperationExists(operationNumber))
            {
                MessageBox.Show($@"Operation number '{operationNumber}' already exists.", @"Duplicate Operation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                operationTextBox.Focus();
                return;
            }

            // Insert the operation
            await Dao_Operation.InsertOperation(operationNumber, Model_AppVariables.User ?? "Current User");

            // Clear the form
            ClearForm();

            // Notify parent
            OperationAdded?.Invoke(this, EventArgs.Empty);

            MessageBox.Show(@"Operation added successfully!", @"Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error adding operation: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        operationTextBox.Clear();
        operationTextBox.Focus();
    }
}