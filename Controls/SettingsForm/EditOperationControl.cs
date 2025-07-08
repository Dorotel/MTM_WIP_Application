using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using System.Data;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class EditOperationControl : UserControl
{
    public event EventHandler? OperationUpdated;
    private DataRow? _currentOperation;

    public EditOperationControl()
    {
        InitializeComponent();
    }

    private async Task LoadOperations()
    {
        try
        {
            var operations = await Dao_Operation.GetAllOperations();
            operationsComboBox.Items.Clear();
            operationsComboBox.Items.Add("Select Operation to Edit");

            foreach (DataRow row in operations.Rows)
            {
                var operationNumber = row["Operation"]?.ToString();
                if (!string.IsNullOrEmpty(operationNumber))
                {
                    operationsComboBox.Items.Add(operationNumber);
                }
            }

            operationsComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error loading operations: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadOperations();
        
        // Set the current user when the control loads
        if (issuedByValueLabel != null) issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
    }

    private async void OperationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (operationsComboBox.SelectedIndex <= 0 || operationsComboBox.SelectedItem?.ToString() == "Select Operation to Edit")
        {
            ClearForm();
            return;
        }

        try
        {
            var selectedOperation = operationsComboBox.SelectedItem.ToString();
            _currentOperation = await Dao_Operation.GetOperationByNumber(selectedOperation ?? string.Empty);

            if (_currentOperation != null)
            {
                operationTextBox.Text = _currentOperation["Operation"]?.ToString() ?? string.Empty;
                issuedByValueLabel.Text = _currentOperation["Issued By"]?.ToString() ?? "Current User";
                EnableControls(true);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error loading operation details: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void SaveButton_Click(object sender, EventArgs e)
    {
        if (_currentOperation == null)
        {
            MessageBox.Show(@"Please select an operation to edit.", @"Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

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

            var newOperationNumber = operationTextBox.Text.Trim();
            var originalOperationNumber = _currentOperation["Operation"]?.ToString() ?? string.Empty;

            // Check if operation number already exists (if changed)
            if (newOperationNumber != originalOperationNumber && 
                await Dao_Operation.OperationExists(newOperationNumber))
            {
                MessageBox.Show($@"Operation number '{newOperationNumber}' already exists.", @"Duplicate Operation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                operationTextBox.Focus();
                return;
            }

            // Update the operation
            await Dao_Operation.UpdateOperation(originalOperationNumber, newOperationNumber, Model_AppVariables.User ?? "Current User");

            // Refresh the dropdown
            await LoadOperations();

            // Clear the form
            ClearForm();
            EnableControls(false);

            // Notify parent
            OperationUpdated?.Invoke(this, EventArgs.Empty);

            MessageBox.Show(@"Operation updated successfully!", @"Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($@"Error updating operation: {ex.Message}", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
        ClearForm();
        EnableControls(false);
    }

    private void ClearForm()
    {
        operationTextBox.Clear();
        operationsComboBox.SelectedIndex = 0;
        issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
        _currentOperation = null;
    }

    private void EnableControls(bool enabled)
    {
        operationTextBox.Enabled = enabled;
        saveButton.Enabled = enabled;
        cancelButton.Enabled = enabled;
    }
}