using MTM_Inventory_Application.Data;
using System.Data;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class RemovePartControl : UserControl
{
    public event EventHandler? PartRemoved;
    private DataRow? _currentPart;
    
    public RemovePartControl()
    {
        InitializeComponent();
        LoadParts();
    }

    private async void LoadParts()
    {
        try
        {
            var parts = await Dao_Part.GetAllParts();
            partsComboBox.Items.Clear();
            partsComboBox.Items.Add("Select Part to Remove");
            
            foreach (DataRow row in parts.Rows)
            {
                partsComboBox.Items.Add($"{row["Item Number"]} - {row["Customer"]}");
            }
            
            partsComboBox.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading parts: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void partsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (partsComboBox.SelectedIndex <= 0)
        {
            ClearForm();
            SetFormEnabled(false);
            return;
        }

        try
        {
            var selectedText = partsComboBox.SelectedItem.ToString();
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
            MessageBox.Show($"Error loading part data: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadPartData()
    {
        if (_currentPart == null) return;

        itemNumberValueLabel.Text = _currentPart["Item Number"].ToString();
        customerValueLabel.Text = _currentPart["Customer"].ToString();
        descriptionValueLabel.Text = _currentPart["Description"].ToString();
        typeValueLabel.Text = _currentPart["Type"].ToString();
        issuedByValueLabel.Text = _currentPart["Issued By"].ToString();
    }

    private void SetFormEnabled(bool enabled)
    {
        removeButton.Enabled = enabled;
        detailsGroupBox.Visible = enabled;
    }

    private async void removeButton_Click(object sender, EventArgs e)
    {
        if (_currentPart == null) return;

        var itemNumber = _currentPart["Item Number"].ToString();
        var customer = _currentPart["Customer"].ToString();
        
        var result = MessageBox.Show(
            $"Are you sure you want to remove part '{itemNumber}' for customer '{customer}'?\n\nThis action cannot be undone.",
            "Confirm Part Removal",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes) return;

        try
        {
            // Remove the part using stored procedure
            await Dao_Part.DeletePartByItemNumber(itemNumber);
            
            MessageBox.Show("Part removed successfully!", "Success", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reload parts list
            await LoadParts();
            
            // Clear the form
            ClearForm();
            SetFormEnabled(false);
            
            // Notify parent that part was removed
            PartRemoved?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error removing part: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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

    private void cancelButton_Click(object sender, EventArgs e)
    {
        partsComboBox.SelectedIndex = 0;
        ClearForm();
        SetFormEnabled(false);
    }
}