using System.ComponentModel;
using System.Data;
using System.Text;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.AdvancedInventoryEntryForm;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Services;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Controls.MainForm;

public partial class ControlRemoveTab : UserControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    public ControlRemoveTab()
    {
        InitializeComponent();
        Control_RemoveTab_Initialize();
        Control_RemoveTab_ComboBox_Part.ForeColor = Color.Red;
        Control_RemoveTab_ComboBox_Operation.ForeColor = Color.Red;
        _ = Control_RemoveTab_OnStartup_LoadComboBoxes();
    }

    public void Control_RemoveTab_Initialize()
    {
        Control_RemoveTab_ComboBox_Operation.Visible = false;
        Control_RemoveTab_ComboBox_Part.Visible = false;
        Control_RemoveTab_Button_Reset.TabStop = false;
    }

    private static void Control_RemoveTab_Button_AdvancedItemRemoval_Click()
    {
        try
        {
            if (VersionCheckerService.MainFormInstance == null)
            {
                AppLogger.Log("MainForm instance is null, cannot open Advanced Inventory Removal.");
                return;
            }

            // var advancedEntryForm = new AdvancedInventoryEntryForm();
            // advancedEntryForm.ShowDialog(VersionCheckerService.MainFormInstance);
            // AppLogger.Log("Inventory Advanced Removal button clicked.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                "Control_RemoveTab_Button_AdvancedItemRemoval_Click");
        }
    }

    private async void Control_RemoveTab_Button_Delete_Click(object? sender, EventArgs? e)
    {
        try
        {
            var selectedCount = Control_RemoveTab_DataGridView_Main.SelectedRows.Count;
            AppLogger.Log($"Delete clicked. Selected rows: {selectedCount}");
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Delete clicked. Selected rows: {selectedCount}");

            if (selectedCount == 0)
            {
                AppLogger.Log("No rows selected for deletion.");
                System.Diagnostics.Debug.WriteLine("[DEBUG] No rows selected for deletion.");
                return;
            }

            // Build a summary of items to delete
            var sb = new StringBuilder();
            var itemsToDelete = new List<(string PartID, string Location, int Quantity)>();
            foreach (DataGridViewRow row in Control_RemoveTab_DataGridView_Main.SelectedRows)
                if (row.DataBoundItem is DataRowView drv)
                {
                    var partId = drv["PartID"]?.ToString() ?? "";
                    var location = drv["Location"]?.ToString() ?? "";
                    var quantity = 0;
                    int.TryParse(drv["Quantity"]?.ToString(), out quantity);

                    sb.AppendLine($"PartID: {partId}, Location: {location}, Quantity: {quantity}");
                    AppLogger.Log($"Selected for deletion: PartID={partId}, Location={location}, Quantity={quantity}");
                    System.Diagnostics.Debug.WriteLine(
                        $"[DEBUG] Selected for deletion: PartID={partId}, Location={location}, Quantity={quantity}");

                    itemsToDelete.Add((partId, location, quantity));
                }

            var confirmResult = MessageBox.Show(
                $"The following items will be deleted:\n\n{sb}\nAre you sure?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes)
            {
                AppLogger.Log("User cancelled deletion.");
                System.Diagnostics.Debug.WriteLine("[DEBUG] User cancelled deletion.");
                return;
            }

            // Delete each selected item using InventoryDao
            foreach (DataGridViewRow row in Control_RemoveTab_DataGridView_Main.SelectedRows)
                if (row.DataBoundItem is DataRowView drv)
                {
                    var partId = drv["PartID"]?.ToString() ?? "";
                    var location = drv["Location"]?.ToString() ?? "";
                    var operation = drv["Operation"]?.ToString() ?? "";
                    var quantity = 0;
                    int.TryParse(drv["Quantity"]?.ToString(), out quantity);

                    AppLogger.Log(
                        $"Deleting: PartID={partId}, Location={location}, Operation={operation}, Quantity={quantity}");
                    System.Diagnostics.Debug.WriteLine(
                        $"[DEBUG] Deleting: PartID={partId}, Location={location}, Operation={operation}, Quantity={quantity}");

                    await InventoryDao.DeleteInventoryByPartIdLocationOperationQuantityAsync(
                        partId,
                        location,
                        operation,
                        quantity
                    );

                    AppLogger.Log(
                        $"Deleted: PartID={partId}, Location={location}, Operation={operation}, Quantity={quantity}");
                    System.Diagnostics.Debug.WriteLine(
                        $"[DEBUG] Deleted: PartID={partId}, Location={location}, Operation={operation}, Quantity={quantity}");
                }

            AppLogger.Log("Selected inventory items deleted.");
            System.Diagnostics.Debug.WriteLine("[DEBUG] Selected inventory items deleted.");

            // Refresh the DataGridView after deletion
            Control_RemoveTab_Button_Search_Click(null, null);
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Exception: {ex}");
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "Control_RemoveTab_Button_Delete_Click");
        }
    }

    private async void Control_RemoveTab_Button_Reset_Click()
    {
        try
        {
            AppLogger.Log("Inventory Reset button clicked.");
            Control_RemoveTab_ComboBox_Operation.Visible = false;
            Control_RemoveTab_ComboBox_Part.Visible = false;
            Control_RemoveTab_Image_NothingFound.Visible = false;

            // Reinitialize ComboBox DataTables
            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "md_part_ids_Get_All",
                new MySqlConnection(WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                Control_RemoveTab_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);

            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                new MySqlConnection(WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                Control_RemoveTab_ComboBox_Operation,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);

            if (MainFormInstance != null)
                MainFormTabResetHelper.ResetRemoveTab(
                    Control_RemoveTab_ComboBox_Part,
                    Control_RemoveTab_ComboBox_Operation,
                    Control_RemoveTab_Button_Search,
                    Control_RemoveTab_Button_Delete
                );

            Control_RemoveTab_ComboBox_Operation.Visible = true;
            Control_RemoveTab_ComboBox_Part.Visible = true;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_Button_Reset");
        }
    }

    private void Control_RemoveTab_ComboBox_Operation_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Op ComboBox selection changed.");

            if (Control_RemoveTab_ComboBox_Operation.SelectedIndex > 0)
            {
                Control_RemoveTab_ComboBox_Operation.ForeColor = Color.Black;
                WipAppVariables.Operation = Control_RemoveTab_ComboBox_Operation.Text;
            }
            else
            {
                Control_RemoveTab_ComboBox_Operation.ForeColor = Color.Red;
                if (Control_RemoveTab_ComboBox_Operation.SelectedIndex != 0 &&
                    Control_RemoveTab_ComboBox_Operation.Items.Count > 0)
                    Control_RemoveTab_ComboBox_Operation.SelectedIndex = 0;
                WipAppVariables.Operation = null;
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Op");
        }
    }

    private void Control_RemoveTab_ComboBox_Part_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Part ComboBox selection changed.");

            if (Control_RemoveTab_ComboBox_Part.SelectedIndex > 0)
            {
                Control_RemoveTab_ComboBox_Part.ForeColor = Color.Black;
                WipAppVariables.PartId = Control_RemoveTab_ComboBox_Part.Text;
            }
            else
            {
                Control_RemoveTab_ComboBox_Part.ForeColor = Color.Red;
                if (Control_RemoveTab_ComboBox_Part.SelectedIndex != 0 &&
                    Control_RemoveTab_ComboBox_Part.Items.Count > 0)
                    Control_RemoveTab_ComboBox_Part.SelectedIndex = 0;
                WipAppVariables.PartId = null;
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Part");
        }
    }

    private async Task Control_RemoveTab_LoadData_ComboBoxes_Async()
    {
        try
        {
            await using var connection = new MySqlConnection(WipAppVariables.ConnectionString);

            var comboBoxSets =
                new (MySqlDataAdapter Adapter, DataTable Table, ComboBox ComboBox, string ProcName, string Display,
                    string Value, string Placeholder, CommandType CommandType)[]
                    {
                        (new MySqlDataAdapter(), new DataTable(), Control_RemoveTab_ComboBox_Part,
                            "md_part_ids_Get_All", "Item Number", "ID", "[ Enter Part ID ]",
                            CommandType.StoredProcedure),
                        (new MySqlDataAdapter(), new DataTable(), Control_RemoveTab_ComboBox_Operation,
                            "md_operation_numbers_Get_All", "Operation", "Operation", "[ Enter Op # ]",
                            CommandType.StoredProcedure)
                    };

            foreach (var (adapter, table, comboBox, procName, display, value, placeholder, cmdType) in comboBoxSets)
                await MainFormComboBoxDataHelper.FillComboBoxAsync(
                    procName,
                    connection,
                    adapter,
                    table,
                    comboBox,
                    display,
                    value,
                    placeholder,
                    cmdType
                );
            AppLogger.Log("Inventory tab ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                "MainForm_LoadRemoveTabComboBoxesAsync");
        }
    }

    private async Task Control_RemoveTab_OnStartup_LoadComboBoxes()
    {
        try
        {
            await Control_RemoveTab_LoadData_ComboBoxes_Async();
            Control_RemoveTab_OnStartup_WireUpEvents();
            AppLogger.Log("Initial setup of ComboBoxes in the Inventory Tab.");
            if (MainFormInstance != null)
                MainFormTabResetHelper.ResetRemoveTab(
                    Control_RemoveTab_ComboBox_Part,
                    Control_RemoveTab_ComboBox_Operation,
                    Control_RemoveTab_Button_Search,
                    Control_RemoveTab_Button_Delete
                );
            Control_RemoveTab_ComboBox_Operation.Visible = true;
            Control_RemoveTab_ComboBox_Part.Visible = true;

            try
            {
                WipAppVariables.UserFullName = await UserDao.GetUserFullNameAsync(WipAppVariables.User, true);
                AppLogger.Log($"User full name loaded: {WipAppVariables.UserFullName}");
            }
            catch (Exception ex)
            {
                AppLogger.LogApplicationError(ex);
                await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_RemoveTab_OnStartup_GetUserFullName").ToString());
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "Control_RemoveTab_OnStartup");
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        try
        {
            if (keyData == Keys.Enter)
            {
                SelectNextControl(
                    ActiveControl,
                    true,
                    true,
                    true,
                    true
                );
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_ProcessCmdKey");
            return false;
        }
    }

    private void Control_RemoveTab_Update_ButtonStates()
    {
        try
        {
            // Enable Search if Part ComboBox has a valid selection
            Control_RemoveTab_Button_Search.Enabled = Control_RemoveTab_ComboBox_Part.SelectedIndex > 0;

            // Enable Delete if there is data and a row is selected
            var hasData = Control_RemoveTab_DataGridView_Main.Rows.Count > 0;
            var hasSelection = Control_RemoveTab_DataGridView_Main.SelectedRows.Count > 0;
            Control_RemoveTab_Button_Delete.Enabled = hasData && hasSelection;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                "Control_RemoveTab_Update_ButtonStates");
        }
    }

    private void Control_RemoveTab_OnStartup_WireUpEvents()
    {
        try
        {
            Control_RemoveTab_Button_Reset.Click += (s, e) => Control_RemoveTab_Button_Reset_Click();
            Control_RemoveTab_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Part_SelectedIndexChanged();
                Control_RemoveTab_Update_ButtonStates();
            };
            Control_RemoveTab_ComboBox_Operation.SelectedIndexChanged += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Operation_SelectedIndexChanged();
                Control_RemoveTab_Update_ButtonStates();
            };

            Control_RemoveTab_Button_AdvancedItemRemoval.Click +=
                (s, e) => Control_RemoveTab_Button_AdvancedItemRemoval_Click();

            Control_RemoveTab_ComboBox_Part.Enter += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Part.BackColor = Color.LightBlue;
            };
            Control_RemoveTab_ComboBox_Part.Leave += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Part.BackColor = SystemColors.Window;
            };

            Control_RemoveTab_ComboBox_Operation.Enter += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Operation.BackColor = Color.LightBlue;
            };
            Control_RemoveTab_ComboBox_Operation.Leave += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Operation.BackColor = SystemColors.Window;
            };

            // Wire up DataGridView selection change to update button states
            Control_RemoveTab_DataGridView_Main.SelectionChanged += (s, e) => Control_RemoveTab_Update_ButtonStates();

            Control_RemoveTab_Button_Delete.Click += Control_RemoveTab_Button_Delete_Click;

            AppLogger.Log("Removal tab events wired up.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_WireUpRemoveTabEvents");
        }
    }

    private void Control_RemoveTab_Button_Toggle_RightPanel_Click(object sender, EventArgs e)
    {
        if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed)
        {
            MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = true;
            Control_RemoveTab_Button_Toggle_RightPanel.Text = @"Toggle Panel (Off)";
        }
        else
        {
            if (MainFormInstance != null)
                MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = false;
            Control_RemoveTab_Button_Toggle_RightPanel.Text = @"Toggle Panel (On)";
        }
    }

    private async void Control_RemoveTab_Button_Search_Click(object sender, EventArgs e)
    {
        try
        {
            AppLogger.Log("RemoveTab Search button clicked.");

            var partId = Control_RemoveTab_ComboBox_Part.Text;
            var op = Control_RemoveTab_ComboBox_Operation.Text;

            if (string.IsNullOrWhiteSpace(partId) || Control_RemoveTab_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Control_RemoveTab_ComboBox_Part.Focus();
                return;
            }

            DataTable results;

            if (!string.IsNullOrWhiteSpace(op) && Control_RemoveTab_ComboBox_Operation.SelectedIndex > 0)
                // Filter by part and operation (assuming operation is stored as Location or similar)
                results = await InventoryDao.GetInventoryByPartIdAndOperationAsync(partId, op, true);
            else
                // Only filter by part
                results = await InventoryDao.GetInventoryByPartIdAsync(partId, true);

            Control_RemoveTab_DataGridView_Main.DataSource = results;
            Control_RemoveTab_DataGridView_Main.ClearSelection();

            // Show only the desired columns
            foreach (DataGridViewColumn column in Control_RemoveTab_DataGridView_Main.Columns)
                column.Visible = column.Name == "PartID" ||
                                 column.Name == "Operation" ||
                                 column.Name == "Quantity" ||
                                 column.Name == "Location";

            // Apply theme and size columns
            DgvDesigner.ApplyThemeToDataGridView(
                Control_RemoveTab_DataGridView_Main,
                AppThemes.GetTheme(WipAppVariables.WipDataGridTheme ?? "Default (Black and White)")
            );
            DgvDesigner.SizeDataGrid(Control_RemoveTab_DataGridView_Main);

            if (results.Rows.Count == 0)
                Control_RemoveTab_Image_NothingFound.Visible = true;
            else
                Control_RemoveTab_Image_NothingFound.Visible = false;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "Control_RemoveTab_Button_Search_Click");
        }
    }
}