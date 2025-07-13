using System;
using System.Data;
using System.Windows.Forms;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class RemoveOperationControl : UserControl
{
    #region Events

    public event EventHandler? OperationRemoved;

    #endregion

    #region Fields

    private DataRow? _currentOperation;

    #endregion

    #region Constructors

    public RemoveOperationControl()
    {
        InitializeComponent();
    }

    #endregion

    #region Initialization

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadOperations();
    }

    private async Task LoadOperations()
    {
        try
        {
            var operations = await Dao_Operation.GetAllOperations();
            operationsComboBox.Items.Clear();
            operationsComboBox.Items.Add("Select Operation to Remove");
            foreach (DataRow row in operations.Rows)
            {
                var operationNumber = row["Operation"]?.ToString();
                if (!string.IsNullOrEmpty(operationNumber))
                    operationsComboBox.Items.Add(operationNumber);
            }

            operationsComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading operations: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    #endregion

    #region Event Handlers

    private async void OperationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (operationsComboBox.SelectedIndex <= 0 ||
            operationsComboBox.SelectedItem?.ToString() == "Select Operation to Remove")
        {
            ClearForm();
            EnableControls(false);
            return;
        }

        try
        {
            var selectedOperation = operationsComboBox.SelectedItem?.ToString();
            _currentOperation = await Dao_Operation.GetOperationByNumber(selectedOperation ?? string.Empty);
            if (_currentOperation != null)
            {
                operationValueLabel.Text = _currentOperation["Operation"]?.ToString() ?? string.Empty;
                issuedByValueLabel.Text = _currentOperation["IssuedBy"]?.ToString() ?? "Unknown";
                EnableControls(true);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading operation details: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void RemoveButton_Click(object sender, EventArgs e)
    {
        if (_currentOperation == null)
        {
            MessageBox.Show("Please select an operation to remove.", "Validation Error", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var operationNumber = _currentOperation["Operation"]?.ToString() ?? string.Empty;
        var result =
            MessageBox.Show(
                $"Are you sure you want to remove the operation number '{operationNumber}'?{Environment.NewLine}{Environment.NewLine}This action cannot be undone.",
                "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (result != DialogResult.Yes)
            return;
        try
        {
            await Dao_Operation.DeleteOperation(operationNumber);
            await LoadOperations();
            ClearForm();
            EnableControls(false);
            OperationRemoved?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Operation removed successfully!", "Success", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error removing operation: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
        ClearForm();
        EnableControls(false);
    }

    #endregion

    #region Methods

    private void ClearForm()
    {
        operationValueLabel.Text = string.Empty;
        issuedByValueLabel.Text = string.Empty;
        operationsComboBox.SelectedIndex = 0;
        _currentOperation = null;
    }

    private void EnableControls(bool enabled)
    {
        removeButton.Enabled = enabled;
        cancelButton.Enabled = enabled;
    }

    #endregion
}