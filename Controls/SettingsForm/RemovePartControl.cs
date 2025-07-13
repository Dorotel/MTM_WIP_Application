using System;
using System.Data;
using System.Windows.Forms;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class RemovePartControl : UserControl
{
    #region Events

    public event EventHandler? PartRemoved;

    #endregion

    #region Fields

    private DataRow? _currentPart;

    #endregion

    #region Constructors

    public RemovePartControl()
    {
        InitializeComponent();
    }

    #endregion

    #region Initialization

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            if (issuedByValueLabel != null)
                issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
            await LoadParts();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "SettingsForm / RemovePartControl_OnLoadOverRide");
        }
    }

    private async Task LoadParts()
    {
        try
        {
            var parts = await Dao_Part.GetAllParts();
            partsComboBox.Items.Clear();
            partsComboBox.Items.Add("Select Part to Remove");
            foreach (DataRow row in parts.Rows)
            {
                var itemNumber = row["PartID"]?.ToString();
                var customer = row["Customer"]?.ToString();
                if (string.IsNullOrWhiteSpace(customer))
                    customer = "[ No Customer ]";
                partsComboBox.Items.Add($"{itemNumber} - {customer}");
            }

            partsComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading parts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #endregion

    #region Event Handlers

    private async void PartsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (partsComboBox.SelectedIndex <= 0)
        {
            ClearForm();
            SetFormEnabled(false);
            return;
        }

        try
        {
            var selectedText = partsComboBox.SelectedItem?.ToString() ?? string.Empty;
            var itemNumber = selectedText.Split(" - ")[0];
            _currentPart = await Dao_Part.GetPartByNumber(itemNumber);
            if (_currentPart != null)
            {
                LoadPartData();
                SetFormEnabled(true);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading part data: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void RemoveButton_Click(object sender, EventArgs e)
    {
        if (_currentPart == null) return;
        var itemNumber = _currentPart["PartID"]?.ToString();
        var customer = _currentPart["Customer"]?.ToString();
        if (string.IsNullOrEmpty(itemNumber))
        {
            MessageBox.Show("Item number is missing. Cannot remove part.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        var result =
            MessageBox.Show(
                $"Are you sure you want to remove part '{itemNumber}' for customer '{customer}'?\n\nThis action cannot be undone.",
                "Confirm Part Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (result != DialogResult.Yes) return;
        try
        {
            await Dao_Part.DeletePartByItemNumber(itemNumber);
            MessageBox.Show("Part removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadParts();
            ClearForm();
            SetFormEnabled(false);
            PartRemoved?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error removing part: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
        partsComboBox.SelectedIndex = 0;
        ClearForm();
        SetFormEnabled(false);
    }

    #endregion

    #region Methods

    private void LoadPartData()
    {
        if (_currentPart == null) return;
        itemNumberValueLabel.Text = _currentPart["PartID"].ToString();
        customerValueLabel.Text = _currentPart["Customer"].ToString();
        descriptionValueLabel.Text = _currentPart["Description"].ToString();
        typeValueLabel.Text = _currentPart["ItemType"].ToString();
        issuedByValueLabel.Text = _currentPart["IssuedBy"].ToString();
    }

    private void SetFormEnabled(bool enabled)
    {
        removeButton.Enabled = enabled;
        detailsGroupBox.Visible = enabled;
    }

    private void ClearForm()
    {
        itemNumberValueLabel.Text = "";
        customerValueLabel.Text = "";
        descriptionValueLabel.Text = "";
        typeValueLabel.Text = "";
        issuedByValueLabel.Text = "";
        _currentPart = null;
    }

    #endregion
}