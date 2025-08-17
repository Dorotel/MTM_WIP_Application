using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Add_Location : UserControl
    {
        #region Events

        public event EventHandler? LocationAdded;

        #endregion

        #region Constructors

        public Control_Add_Location()
        {
            InitializeComponent();
            LoadBuildingOptions();
        }

        #endregion

        #region Initialization

        private void LoadBuildingOptions()
        {
            Control_Add_Location_ComboBox_Building.Items.Clear();
            Control_Add_Location_ComboBox_Building.Items.Add("[ Select Building ]");
            Control_Add_Location_ComboBox_Building.Items.Add("Expo");
            Control_Add_Location_ComboBox_Building.Items.Add("Vits");
            Control_Add_Location_ComboBox_Building.SelectedIndex = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Control_Add_Location_Label_IssuedByValue != null)
            {
                Control_Add_Location_Label_IssuedByValue.Text = Model_AppVariables.User ?? "Current User";
            }
        }

        #endregion

        #region Event Handlers

        private async void Control_Add_Location_Button_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Control_Add_Location_TextBox_Location.Text))
                {
                    MessageBox.Show(@"Location is required.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_Location_TextBox_Location.Focus();
                    return;
                }

                if (Control_Add_Location_ComboBox_Building.SelectedIndex <= 0 ||
                    Control_Add_Location_ComboBox_Building.Text == "Select Building")
                {
                    MessageBox.Show(@"Building is required.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_Location_ComboBox_Building.Focus();
                    return;
                }

                string location = Control_Add_Location_TextBox_Location.Text.Trim();
                string building = Control_Add_Location_ComboBox_Building.Text ?? string.Empty;

                if (await Dao_Location.LocationExists(location))
                {
                    MessageBox.Show($@"Location '{location}' already exists.", @"Duplicate Location",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Control_Add_Location_TextBox_Location.Focus();
                    return;
                }

                await Dao_Location.InsertLocation(location, building, true);
                ClearForm();
                LocationAdded?.Invoke(this, EventArgs.Empty);
                MessageBox.Show(@"Location added successfully!", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error adding location: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void Control_Add_Location_Button_Clear_Click(object sender, EventArgs e) => ClearForm();

        #endregion

        #region Methods

        private void ClearForm()
        {
            Control_Add_Location_TextBox_Location.Clear();
            Control_Add_Location_ComboBox_Building.SelectedIndex = 0;
            Control_Add_Location_TextBox_Location.Focus();
        }

        #endregion
    }
}
