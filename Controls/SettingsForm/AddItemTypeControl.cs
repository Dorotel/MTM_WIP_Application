using System;
using System.Windows.Forms;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class AddItemTypeControl : UserControl
    {
        #region Events
        public event EventHandler? ItemTypeAdded;
        #endregion

        #region Constructors
        public AddItemTypeControl()
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
                if (string.IsNullOrWhiteSpace(itemTypeTextBox.Text))
                {
                    MessageBox.Show("Item type is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    itemTypeTextBox.Focus();
                    return;
                }

                var itemType = itemTypeTextBox.Text.Trim();

                if (await Dao_ItemType.ItemTypeExists(itemType))
                {
                    MessageBox.Show($"Item type '{itemType}' already exists.", "Duplicate Item Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    itemTypeTextBox.Focus();
                    return;
                }

                await Dao_ItemType.InsertItemType(itemType, Model_AppVariables.User ?? "Current User");
                ClearForm();
                ItemTypeAdded?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Item type added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding item type: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            itemTypeTextBox.Clear();
            itemTypeTextBox.Focus();
        }
        #endregion
    }
}