using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Add_Operation : UserControl
    {
        #region Events

        public event EventHandler? OperationAdded;
        public event EventHandler<string>? StatusMessageChanged;

        #endregion

        #region Fields

        private Helper_StoredProcedureProgress? _progressHelper;

        #endregion

        #region Constructors

        public Control_Add_Operation() => InitializeComponent();

        #endregion

        #region Public Methods

        public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, 
                this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
        }

        #endregion

        #region Initialization

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Control_Add_Operation_Label_IssuedByValue != null)
            {
                Control_Add_Operation_Label_IssuedByValue.Text = Model_AppVariables.User ?? "Current User";
            }
        }

        #endregion

        #region Event Handlers

        private async void Control_Add_Operation_Button_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Control_Add_Operation_Button_Save.Enabled = false;

                _progressHelper?.ShowProgress("Initializing operation creation...");
                _progressHelper?.UpdateProgress(10, "Validating form data...");
                await Task.Delay(50);

                if (string.IsNullOrWhiteSpace(Control_Add_Operation_TextBox_Operation.Text))
                {
                    _progressHelper?.ShowError("Operation number is required");
                    Control_Add_Operation_TextBox_Operation.Focus();
                    return;
                }

                string operationNumber = Control_Add_Operation_TextBox_Operation.Text.Trim();

                _progressHelper?.UpdateProgress(30, "Checking for existing operation...");

                // Check if operation already exists using enhanced stored procedure
                var existsResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
                    Model_AppVariables.ConnectionString,
                    "md_operation_numbers_Exists_ByOperation",
                    new Dictionary<string, object> { ["Operation"] = operationNumber },
                    _progressHelper,
                    true
                );

                if (!existsResult.IsSuccess)
                {
                    _progressHelper?.ShowError($"Error checking operation existence: {existsResult.ErrorMessage}");
                    return;
                }

                if (existsResult.Data != null && existsResult.Data.Rows.Count > 0)
                {
                    int operationExists = Convert.ToInt32(existsResult.Data.Rows[0]["OperationExists"]);
                    if (operationExists > 0)
                    {
                        _progressHelper?.ShowError($"Operation number '{operationNumber}' already exists");
                        Control_Add_Operation_TextBox_Operation.Focus();
                        return;
                    }
                }

                _progressHelper?.UpdateProgress(60, "Creating operation...");

                // Create the operation using enhanced stored procedure
                var createResult = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
                    Model_AppVariables.ConnectionString,
                    "md_operation_numbers_Add_Operation",
                    new Dictionary<string, object>
                    {
                        ["Operation"] = operationNumber,
                        ["IssuedBy"] = Model_AppVariables.User ?? "Current User"
                    },
                    _progressHelper,
                    true
                );

                if (!createResult.IsSuccess)
                {
                    _progressHelper?.ShowError($"Error creating operation: {createResult.ErrorMessage}");
                    return;
                }

                _progressHelper?.UpdateProgress(90, "Finalizing operation creation...");
                ClearForm();

                _progressHelper?.ShowSuccess($"Operation '{operationNumber}' created successfully!");
                await Task.Delay(500);
                _progressHelper?.HideProgress();

                OperationAdded?.Invoke(this, EventArgs.Empty);
                StatusMessageChanged?.Invoke(this, $"Operation '{operationNumber}' added successfully!");

                MessageBox.Show(@"Operation added successfully!", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _progressHelper?.ShowError($"Unexpected error creating operation: {ex.Message}");
                LoggingUtility.LogApplicationError(ex);
                StatusMessageChanged?.Invoke(this, $"Error adding operation: {ex.Message}");
                MessageBox.Show($@"Error adding operation: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Control_Add_Operation_Button_Save.Enabled = true;
            }
        }

        private void Control_Add_Operation_Button_Clear_Click(object sender, EventArgs e) => ClearForm();

        #endregion

        #region Methods

        private void ClearForm()
        {
            Control_Add_Operation_TextBox_Operation.Clear();
            Control_Add_Operation_TextBox_Operation.Focus();
        }

        #endregion
    }
}
