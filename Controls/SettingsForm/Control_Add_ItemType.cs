using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Add_ItemType : UserControl
    {
        #region Events

        public event EventHandler? ItemTypeAdded;

        #endregion

        #region Constructors

        public Control_Add_ItemType() => InitializeComponent();

        #endregion

        #region Initialization

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Control_Add_ItemType_Label_IssuedByValue != null)
            {
                Control_Add_ItemType_Label_IssuedByValue.Text = Model_AppVariables.User ?? "Current User";
            }
        }

        #endregion

        #region Event Handlers

        private async void Control_Add_ItemType_Button_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Control_Add_ItemType_TextBox_ItemType.Text))
                {
                    MessageBox.Show(@"PartID is required.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_ItemType_TextBox_ItemType.Focus();
                    return;
                }

                string itemType = Control_Add_ItemType_TextBox_ItemType.Text.Trim();

                if (await Dao_ItemType.ItemTypeExists(itemType))
                {
                    MessageBox.Show($@"PartID '{itemType}' already exists.", @"Duplicate PartID", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_ItemType_TextBox_ItemType.Focus();
                    return;
                }

                await Dao_ItemType.InsertItemType(itemType, Model_AppVariables.User ?? "Current User");
                ClearForm();
                ItemTypeAdded?.Invoke(this, EventArgs.Empty);
                MessageBox.Show(@"PartID added successfully!", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error adding PartID: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void Control_Add_ItemType_Button_Clear_Click(object sender, EventArgs e) => ClearForm();

        #endregion

        #region Methods

        private void ClearForm()
        {
            Control_Add_ItemType_TextBox_ItemType.Clear();
            Control_Add_ItemType_TextBox_ItemType.Focus();
        }

        #endregion
    }
}
