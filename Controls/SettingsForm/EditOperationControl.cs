using System;
using System.Data;
using System.Windows.Forms;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class EditOperationControl : UserControl
    {
        #region Events
        public event EventHandler? OperationUpdated;
        #endregion

        #region Fields
        private DataRow? _currentOperation;
        #endregion

        #region Constructors
        public EditOperationControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Initialization
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
                        operationsComboBox.Items.Add(operationNumber);
                }
                operationsComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading operations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadOperations();
            if (issuedByValueLabel != null)
                issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
        }
        #endregion

        #region Event Handlers
        private async void OperationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (operationsComboBox.SelectedIndex <= 0 ||
                operationsComboBox.SelectedItem?.ToString() == "Select Operation to Edit")
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
                    operationTextBox.Text = _currentOperation["Operation"]?.ToString() ?? string.Empty;
                    issuedByValueLabel.Text = _currentOperation["Issued By"]?.ToString() ?? "Current User";
                    EnableControls(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading operation details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            if (_currentOperation == null)
            {
                MessageBox.Show("Please select an operation to edit.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                if (string.IsNullOrWhiteSpace(operationTextBox.Text))
                {
                    MessageBox.Show("Operation number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    operationTextBox.Focus();
                    return;
                }
                var newOperationNumber = operationTextBox.Text.Trim();
                var originalOperationNumber = _currentOperation["Operation"]?.ToString() ?? string.Empty;
                if (newOperationNumber != originalOperationNumber && await Dao_Operation.OperationExists(newOperationNumber))
                {
                    MessageBox.Show($"Operation number '{newOperationNumber}' already exists.", "Duplicate Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    operationTextBox.Focus();
                    return;
                }
                await Dao_Operation.UpdateOperation(originalOperationNumber, newOperationNumber, Model_AppVariables.User ?? "Current User", true);
                await LoadOperations();
                ClearForm();
                EnableControls(false);
                OperationUpdated?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Operation updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating operation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        #endregion
    }
}