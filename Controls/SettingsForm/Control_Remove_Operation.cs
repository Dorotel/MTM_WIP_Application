using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Remove_Operation : UserControl
    {
        #region Events

        public event EventHandler? OperationRemoved;

        #endregion

        #region Fields

        private DataRow? _currentOperation;

        #endregion


        #region Constructors

        public Control_Remove_Operation() => InitializeComponent();

        #endregion

        #region Initialization

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadOperations();
        }

        private async void LoadOperations()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(operationsComboBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error loading part types: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        private async void OperationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (operationsComboBox.SelectedIndex <= 0 ||
                operationsComboBox.Text == "Select Operation to Remove")
            {
                ClearForm();
                EnableControls(false);
                return;
            }

            try
            {
                string? selectedOperation = operationsComboBox.Text;
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
                MessageBox.Show($@"Error loading operation details: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void RemoveButton_Click(object sender, EventArgs e)
        {
            if (_currentOperation == null)
            {
                MessageBox.Show(@"Please select an operation to remove.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string operationNumber = _currentOperation["Operation"]?.ToString() ?? string.Empty;
            DialogResult result =
                MessageBox.Show(
                    $@"Are you sure you want to remove the operation number '{operationNumber}'?{Environment.NewLine}{Environment.NewLine}This action cannot be undone.",
                    @"Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                await Dao_Operation.DeleteOperation(operationNumber);
                LoadOperations();
                ClearForm();
                EnableControls(false);
                OperationRemoved?.Invoke(this, EventArgs.Empty);
                MessageBox.Show(@"Operation removed successfully!", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error removing operation: {ex.Message}", @"Error", MessageBoxButtons.OK,
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
}
