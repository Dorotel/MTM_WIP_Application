using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTM_WIP_Application.Forms.MainForm;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        _ = OnStartup();
    }

    private async Task OnStartup()
    {
        WireUpInventoryTabEvents();
        await LoadInventoryTabComboBoxesAsync();
        AppLogger.Log("Initial setup of ComboBoxes in the Inventory Tab.");
        MainFormTabResetHelper.ResetInventoryTab(
            MainForm_Inventory_ComboBox_Loc,
            MainForm_Inventory_ComboBox_Op,
            MainForm_Inventory_ComboBox_Part,
            new CheckBox(),
            new CheckBox(),
            null,
            MainForm_Inventory_TextBox_Qty,
            MainForm_Inventory_RichTextBox_Notes,
            MainForm_Inventory_Button_Save,
            MainForm_MenuStrip_File_Save
        );
    }

    private void WireUpInventoryTabEvents()
    {
        MainForm_Inventory_Button_Save.Click += async (s, e) => await InventoryButtonSave_ClickAsync();
        MainForm_Inventory_Button_Reset.Click += (s, e) => InventoryButtonReset_Click();
        MainForm_Inventory_ComboBox_Part.SelectedIndexChanged += (s, e) => InventoryComboBoxPart_SelectedIndexChanged();
        MainForm_Inventory_ComboBox_Op.SelectedIndexChanged += (s, e) => InventoryComboBoxOp_SelectedIndexChanged();
        MainForm_Inventory_ComboBox_Loc.SelectedIndexChanged += (s, e) => InventoryComboBoxLoc_SelectedIndexChanged();
        MainForm_Inventory_TextBox_Qty.TextChanged += (s, e) => InventoryTextBoxQty_TextChanged();
        MainForm_Inventory_Button_ShowHideLast10.Click += (s, e) => InventoryButtonShowHideLast10_Click();
        MainForm_InventoryTab_Button_AdvancedEntry.Click += (s, e) => InventoryButtonAdvancedEntry_Click();
    }

    private async Task LoadInventoryTabComboBoxesAsync()
    {
        await using var connection = new MySqlConnection(WipAppVariables.ConnectionString);

        // Prepare DataAdapters and DataTables for each ComboBox
        var inventoryTabPartCbDataAdapter = new MySqlDataAdapter();
        var inventoryTabPartCbDataTable = new DataTable();

        var inventoryTabOpCbDataAdapter = new MySqlDataAdapter();
        var inventoryTabOpCbDataTable = new DataTable();

        var inventoryTabLocationCbDataAdapter = new MySqlDataAdapter();
        var inventoryTabLocationCbDataTable = new DataTable();

        // If you only want to fill the Inventory tab, you can pass null or dummy objects for the other tabs
        var dummyAdapter = new MySqlDataAdapter();
        var dummyTable = new DataTable();
        var dummyComboBox = new ComboBox();

        await MainFormComboBoxDataHelper.FillAllComboBoxesAsync(
            connection,
            inventoryTabPartCbDataAdapter, inventoryTabPartCbDataTable, MainForm_Inventory_ComboBox_Part,
            inventoryTabOpCbDataAdapter, inventoryTabOpCbDataTable, MainForm_Inventory_ComboBox_Op,
            inventoryTabLocationCbDataAdapter, inventoryTabLocationCbDataTable, MainForm_Inventory_ComboBox_Loc,
            dummyAdapter, dummyTable, dummyComboBox, // Remove tab
            dummyAdapter, dummyTable, dummyComboBox, // Remove tab
            dummyAdapter, dummyTable, dummyComboBox, // Remove tab
            dummyAdapter, dummyTable, dummyComboBox, // Transfer tab
            dummyAdapter, dummyTable, dummyComboBox // Transfer tab
        );
    }

    private async Task InventoryButtonSave_ClickAsync()
    {
        try
        {
            AppLogger.Log("Inventory Save button clicked.");
            // Validate input
            // Call your DAO to save the transaction
            // Update UI as needed
            // Optionally reset controls or show a success message
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, nameof(InventoryButtonSave_ClickAsync),
                "MainForm_Inventory_Button_Save");
        }
    }

    private void InventoryComboBoxPart_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Part ComboBox selection changed.");
            // Example: Update WipAppVariables.PartId and reset related controls
            if (MainForm_Inventory_ComboBox_Part.SelectedIndex > 0)
                WipAppVariables.PartId = MainForm_Inventory_ComboBox_Part.Text;
            // Optionally, update dependent ComboBoxes or fields here
            else
                WipAppVariables.PartId = null;
            // Optionally reset or update other controls as needed
            MainFormTabResetHelper.ResetInventoryTab(
                MainForm_Inventory_ComboBox_Loc,
                MainForm_Inventory_ComboBox_Op,
                MainForm_Inventory_ComboBox_Part,
                new CheckBox(),
                new CheckBox(),
                null,
                MainForm_Inventory_TextBox_Qty,
                MainForm_Inventory_RichTextBox_Notes,
                MainForm_Inventory_Button_Save,
                MainForm_MenuStrip_File_Save
            );
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                nameof(InventoryComboBoxPart_SelectedIndexChanged), "MainForm_Inventory_ComboBox_Part");
        }
    }

    private void InventoryComboBoxOp_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Op ComboBox selection changed.");
            if (MainForm_Inventory_ComboBox_Op.SelectedIndex > 0)
                WipAppVariables.Operation = MainForm_Inventory_ComboBox_Op.Text;
            else
                WipAppVariables.Operation = null;
            // Optionally reset or update other controls as needed
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                nameof(InventoryComboBoxOp_SelectedIndexChanged), "MainForm_Inventory_ComboBox_Op");
        }
    }

    private void InventoryComboBoxLoc_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Location ComboBox selection changed.");
            if (MainForm_Inventory_ComboBox_Loc.SelectedIndex > 0)
                WipAppVariables.Location = MainForm_Inventory_ComboBox_Loc.Text;
            else
                WipAppVariables.Location = null;
            // Optionally reset or update other controls as needed
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                nameof(InventoryComboBoxLoc_SelectedIndexChanged), "MainForm_Inventory_ComboBox_Loc");
        }
    }

    private void InventoryButtonReset_Click()
    {
        try
        {
            AppLogger.Log("Inventory Reset button clicked.");
            MainFormTabResetHelper.ResetInventoryTab(
                MainForm_Inventory_ComboBox_Loc,
                MainForm_Inventory_ComboBox_Op,
                MainForm_Inventory_ComboBox_Part,
                new CheckBox(),
                new CheckBox(),
                null,
                MainForm_Inventory_TextBox_Qty,
                MainForm_Inventory_RichTextBox_Notes,
                MainForm_Inventory_Button_Save,
                MainForm_MenuStrip_File_Save
            );
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, nameof(InventoryButtonReset_Click),
                "MainForm_Inventory_Button_Reset");
        }
    }

    private void InventoryTextBoxQty_TextChanged()
    {
        try
        {
            AppLogger.Log("Inventory Quantity TextBox changed.");
            // TODO: Validate quantity input
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, nameof(InventoryTextBoxQty_TextChanged),
                "MainForm_Inventory_TextBox_Qty");
        }
    }

    private void InventoryButtonShowHideLast10_Click()
    {
        try
        {
            AppLogger.Log("Inventory Show/Hide Last 10 button clicked.");
            // TODO: Show/hide last 10 transactions
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                nameof(InventoryButtonShowHideLast10_Click), "MainForm_Inventory_Button_ShowHideLast10");
        }
    }

    private void InventoryButtonAdvancedEntry_Click()
    {
        try
        {
            AppLogger.Log("Inventory Advanced Entry button clicked.");
            // TODO: Open advanced entry dialog or logic
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, nameof(InventoryButtonAdvancedEntry_Click),
                "MainForm_InventoryTab_Button_AdvancedEntry");
        }
    }
}