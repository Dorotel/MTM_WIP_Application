using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Add_Operation : UserControl
    {
        #region Events

        public event EventHandler? OperationAdded;

        #endregion

        #region Constructors

        public Control_Add_Operation() => InitializeComponent();

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
                if (string.IsNullOrWhiteSpace(Control_Add_Operation_TextBox_Operation.Text))
                {
                    MessageBox.Show(@"Operation number is required.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_Operation_TextBox_Operation.Focus();
                    return;
                }

                string operationNumber = Control_Add_Operation_TextBox_Operation.Text.Trim();

                if (await Dao_Operation.OperationExists(operationNumber))
                {
                    MessageBox.Show($@"Operation number '{operationNumber}' already exists.", @"Duplicate Operation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Control_Add_Operation_TextBox_Operation.Focus();
                    return;
                }

                await Dao_Operation.InsertOperation(operationNumber, Model_AppVariables.User ?? "Current User");
                ClearForm();
                OperationAdded?.Invoke(this, EventArgs.Empty);
                MessageBox.Show(@"Operation added successfully!", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error adding operation: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
