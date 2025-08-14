using System.Data;
using System.Reflection;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Add_PartID : UserControl
    {
        #region Fields

        #region Events

        public event EventHandler? PartAdded;

        #endregion

        #region Constructors

        public Control_Add_PartID()
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["ControlType"] = nameof(Control_Add_PartID),
                ["InitializationTime"] = DateTime.Now,
                ["Thread"] = Thread.CurrentThread.ManagedThreadId
            }, nameof(Control_Add_PartID), nameof(Control_Add_PartID));

            Service_DebugTracer.TraceUIAction("ADD_PARTID_INITIALIZATION", nameof(Control_Add_PartID),
                new Dictionary<string, object>
                {
                    ["Phase"] = "START",
                    ["ComponentType"] = "UserControl"
                });

            InitializeComponent();

            Service_DebugTracer.TraceUIAction("PART_TYPES_LOADING", nameof(Control_Add_PartID),
                new Dictionary<string, object> { ["DataSource"] = "ItemTypes" });
            LoadPartTypes();

            Service_DebugTracer.TraceMethodExit(null, nameof(Control_Add_PartID), nameof(Control_Add_PartID));
        }

        #endregion

        #region Initialization

        private async void LoadPartTypes()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillItemTypeComboBoxesAsync(Control_Add_PartID_ComboBox_ItemType);

                // Set default item to "WIP" if it exists
                for (int i = 0; i < Control_Add_PartID_ComboBox_ItemType.Items.Count; i++)
                {
                    if (Control_Add_PartID_ComboBox_ItemType.Items[i]?.ToString()
                            ?.Equals("WIP", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Control_Add_PartID_ComboBox_ItemType.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error loading part types: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (issuedByValueLabel != null)
            {
                issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
            }
        }

        #endregion

        #region Event Handlers

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(itemNumberTextBox.Text))
                {
                    MessageBox.Show(@"Item Number is required.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    itemNumberTextBox.Focus();
                    return;
                }

                if (Control_Add_PartID_ComboBox_ItemType.SelectedIndex < 0)
                {
                    MessageBox.Show(@"Please select a part type.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_PartID_ComboBox_ItemType.Focus();
                    return;
                }

                if (await Dao_Part.PartExists(itemNumberTextBox.Text.Trim()))
                {
                    MessageBox.Show($@"Part number '{itemNumberTextBox.Text.Trim()}' already exists.",
                        @"Duplicate Part Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    itemNumberTextBox.Focus();
                    return;
                }

                await AddPartAsync();
                MessageBox.Show(@"Part added successfully!", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ClearForm();
                PartAdded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error adding part: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async Task AddPartAsync()
        {
            string itemNumber = itemNumberTextBox.Text.Trim();
            string issuedBy = Model_AppVariables.User;
            string type = Control_Add_PartID_ComboBox_ItemType.Text ?? string.Empty;
            // Removed customer and description parameters
            await Dao_Part.AddPartWithStoredProcedure(itemNumber, null, null, issuedBy, type);
        }

        private void CancelButton_Click(object sender, EventArgs e) => ClearForm();

        #endregion

        #region Methods

        private void ClearForm()
        {
            itemNumberTextBox.Clear();

            // Set ComboBox to "WIP" if it exists
            for (int i = 0; i < Control_Add_PartID_ComboBox_ItemType.Items.Count; i++)
            {
                if (Control_Add_PartID_ComboBox_ItemType.Items[i]?.ToString()
                        ?.Equals("WIP", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Control_Add_PartID_ComboBox_ItemType.SelectedIndex = i;
                    break;
                }
            }

            itemNumberTextBox.Focus();
        }

        // Helper class for ComboBox items
        private class ComboBoxItem
        {
            public string Display { get; set; }
            public string FullName { get; set; }
            public string FileName { get; set; }
            public override string ToString() => Display;
        }

        #endregion

        #endregion
    }
}
