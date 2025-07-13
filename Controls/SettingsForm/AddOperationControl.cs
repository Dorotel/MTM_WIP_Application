// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class AddOperationControl : UserControl
    {
        #region Events
        public event EventHandler? OperationAdded;
        #endregion

        #region Constructors
        public AddOperationControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Initialization
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
                if (string.IsNullOrWhiteSpace(operationTextBox.Text))
                {
                    MessageBox.Show("Operation number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    operationTextBox.Focus();
                    return;
                }

                var operationNumber = operationTextBox.Text.Trim();

                if (await Dao_Operation.OperationExists(operationNumber))
                {
                    MessageBox.Show($"Operation number '{operationNumber}' already exists.", "Duplicate Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    operationTextBox.Focus();
                    return;
                }

                await Dao_Operation.InsertOperation(operationNumber, Model_AppVariables.User ?? "Current User");
                ClearForm();
                OperationAdded?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Operation added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding operation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        #endregion

        #region Methods
        private void ClearForm()
        {
            operationTextBox.Clear();
            operationTextBox.Focus();
        }
        #endregion
    }
}