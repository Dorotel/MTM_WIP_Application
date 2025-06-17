using System.ComponentModel;
using System.Data;
using System.Text;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Services;
using MTM_WIP_Application.Models;
using MySql.Data.MySqlClient;
using static System.Int32;

namespace MTM_WIP_Application.Controls.MainForm;

public partial class ControlRemoveTab : UserControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    private readonly List<Model_HistoryRemove> _lastRemovedItems = [];

    private readonly DataTable _partCbDataTable = new();
    private readonly DataTable _opCbDataTable = new();
    private readonly MySqlDataAdapter _partCbDataAdapter = new();
    private readonly MySqlDataAdapter _opCbDataAdapter = new();

    public ControlRemoveTab()
    {
        InitializeComponent();
        Control_RemoveTab_Initialize();
        Helper_ComboBoxes.ApplyStandardComboBoxProperties(Control_RemoveTab_ComboBox_Part);
        Helper_ComboBoxes.ApplyStandardComboBoxProperties(Control_RemoveTab_ComboBox_Operation);
        Control_RemoveTab_ComboBox_Part.ForeColor = Color.Red;
        Control_RemoveTab_ComboBox_Operation.ForeColor = Color.Red;
        Control_RemoveTab_Image_NothingFound.Visible = false;
        _ = Control_RemoveTab_OnStartup_LoadComboBoxesAsync();
        if (Control_RemoveTab_Panel_Footer.Controls["Control_RemoveTab_Button_Undo"] == null)
        {
            var undoButton = new Button
            {
                Name = "Control_RemoveTab_Button_Undo",
                Text = "Undo",
                Enabled = false,
                AutoSize = true,
                Anchor = AnchorStyles.Right
            };
            undoButton.Click += Control_RemoveTab_Button_Undo_Click;
            Control_RemoveTab_Panel_Footer.Controls.Add(undoButton);
        }
    }

    public void Control_RemoveTab_Initialize()
    {
        Control_RemoveTab_ComboBox_Operation.Visible = false;
        Control_RemoveTab_ComboBox_Part.Visible = false;
        Control_RemoveTab_Button_Reset.TabStop = false;
    }

    private async Task Control_RemoveTab_OnStartup_LoadComboBoxesAsync()
    {
        try
        {
            await Control_RemoveTab_OnStartup_LoadDataComboBoxesAsync();
            Control_RemoveTab_OnStartup_WireUpEvents();
            LoggingUtility.Log("Initial setup of ComboBoxes in the Remove Tab.");
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
                Core_WipAppVariables.UserFullName =
                    await Dao_User.GetUserFullNameAsync(Core_WipAppVariables.User, true);
                LoggingUtility.Log($"User full name loaded: {Core_WipAppVariables.UserFullName}");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_RemoveTab_OnStartup_GetUserFullName").ToString());
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("Control_RemoveTab_OnStartup").ToString());
        }
    }

    private async Task Control_RemoveTab_OnStartup_LoadDataComboBoxesAsync()
    {
        try
        {
            await using var connection = new MySqlConnection(Core_WipAppVariables.ConnectionString);
            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_part_ids_Get_All",
                connection,
                _partCbDataAdapter,
                _partCbDataTable,
                Control_RemoveTab_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                connection,
                _opCbDataAdapter,
                _opCbDataTable,
                Control_RemoveTab_ComboBox_Operation,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);

            LoggingUtility.Log("Remove tab ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("MainForm_LoadRemoveTabComboBoxesAsync").ToString());
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

            if (keyData == Keys.Delete)
            {
                Control_RemoveTab_Button_Delete.PerformClick();
                return true;
            }

            if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed &&
                keyData == (Keys.Alt | Keys.Right))
            {
                Control_RemoveTab_Button_Toggle_RightPanel.PerformClick();
                return true;
            }

            if (MainFormInstance != null && MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed &&
                keyData == (Keys.Alt | Keys.Left))
            {
                Control_RemoveTab_Button_Toggle_RightPanel.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_ProcessCmdKey").ToString());
            return false;
        }
    }

    private async void Control_RemoveTab_Button_Delete_Click(object? sender, EventArgs? e)
    {
        try
        {
            var selectedCount = Control_RemoveTab_DataGridView_Main.SelectedRows.Count;
            LoggingUtility.Log($"Delete clicked. Selected rows: {selectedCount}");
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Delete clicked. Selected rows: {selectedCount}");

            if (selectedCount == 0)
            {
                LoggingUtility.Log("No rows selected for deletion.");
                System.Diagnostics.Debug.WriteLine("[DEBUG] No rows selected for deletion.");
                return;
            }

            var itemsToDelete = GetSelectedItemsToDelete(out var summary);

            var confirmResult = MessageBox.Show(
                $@"The following items will be deleted:

{summary}
Are you sure?",
                @"Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes)
            {
                LoggingUtility.Log("User cancelled deletion.");
                System.Diagnostics.Debug.WriteLine("[DEBUG] User cancelled deletion.");
                return;
            }

            _lastRemovedItems.Clear();

            var partIds = new HashSet<string>();
            var operations = new HashSet<string>();
            var locations = new HashSet<string>();
            var totalQty = 0;

            foreach (DataGridViewRow row in Control_RemoveTab_DataGridView_Main.SelectedRows)
                if (row.DataBoundItem is DataRowView drv)
                {
                    var partId = drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("PartID")
                        ? drv["PartID"]?.ToString() ?? ""
                        : "";
                    var location = drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("Location")
                        ? drv["Location"]?.ToString() ?? ""
                        : "";
                    var quantity = drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("Quantity")
                        ? Convert.ToInt32(drv["Quantity"])
                        : 0;
                    var operation = drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("Operation")
                        ? drv["Operation"]?.ToString() ?? ""
                        : "";
                    var batchNumber = drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("Batch Number")
                        ? drv["Batch Number"]?.ToString() ?? ""
                        : "";
                    var partType = drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("Item Type")
                        ? drv["Item Type"]?.ToString() ?? ""
                        : "";
                    var receivedDate =
                        drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("Received Date")
                            ? Convert.ToDateTime(drv["Received Date"])
                            : DateTime.MinValue;
                    var lastUpdate = drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("Update Date")
                        ? Convert.ToDateTime(drv["Update Date"])
                        : DateTime.MinValue;
                    var user = drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("User")
                        ? drv["User"]?.ToString() ?? " [ Nothing Entered ] "
                        : " [ Nothing Entered ] ";
                    var notes = drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("Notes")
                        ? drv["Notes"]?.ToString() ?? ""
                        : "";

                    _lastRemovedItems.Add(new Model_HistoryRemove
                    {
                        PartId = partId,
                        Location = location,
                        Operation = operation,
                        Quantity = quantity,
                        ItemType = partType, // Use ItemType property for both ItemType/PartType
                        ReceiveDate = receivedDate,
                        LastUpdated = lastUpdate,
                        User = user,
                        BatchNumber = batchNumber,
                        Notes = notes
                    });

                    LoggingUtility.Log(
                        $"Deleting: PartID={partId}, Location={location}, Operation={operation}, Quantity={quantity}");
                    System.Diagnostics.Debug.WriteLine(
                        $"[DEBUG] Deleting: PartID={partId}, Location={location}, Operation={operation}, Quantity={quantity}");

                    await Dao_Inventory.DeleteInventoryByPartIdLocationOperationQuantityAsync(
                        partId,
                        location,
                        operation,
                        quantity: quantity,
                        batchNumber: batchNumber
                    );

                    LoggingUtility.Log(
                        $"Deleted: PartID={partId}, Location={location}, Operation={operation}, Quantity={quantity}");
                    System.Diagnostics.Debug.WriteLine(
                        $"[DEBUG] Deleted: PartID={partId}, Location={location}, Operation={operation}, Quantity={quantity}");

                    partIds.Add(partId);
                    operations.Add(operation);
                    locations.Add(location);
                    totalQty += quantity;
                }

            if (_lastRemovedItems.Count > 0 &&
                Control_RemoveTab_Panel_Footer.Controls["Control_RemoveTab_Button_Undo"] is Button undoBtn)
                undoBtn.Enabled = true;

            LoggingUtility.Log("Selected inventory items deleted.");
            System.Diagnostics.Debug.WriteLine("[DEBUG] Selected inventory items deleted.");

            if (MainFormInstance != null && itemsToDelete.Count > 0)
            {
                var time = DateTime.Now.ToString("hh:mm tt");
                var locDisplay = locations.Count > 1 ? "Multiple Locations" : locations.FirstOrDefault() ?? "";
                if (partIds.Count == 1 && operations.Count == 1)
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $@"Last Deleted: {partIds.First()} (Op: {operations.First()}), Location: {locDisplay}, Quantity: {totalQty} @ {time}";
                else if (partIds.Count == 1 && operations.Count > 1)
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $@"Last Deleted: {partIds.First()} (Multiple Ops), Location: {locDisplay}, Quantity: {totalQty} @ {time}";
                else
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $@"Last Deleted: Multiple Part IDs, Location: {locDisplay}, Quantity: Multiple @ {time}";
            }

            Control_RemoveTab_Button_Search_Click(null, null);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Exception: {ex}");
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("Control_RemoveTab_Button_Delete_Click").ToString());
        }
    }

    private async void Control_RemoveTab_Button_Undo_Click(object? sender, EventArgs? e)
    {
        if (_lastRemovedItems.Count == 0)
            return;

        try
        {
            foreach (var item in _lastRemovedItems)
                await Dao_Inventory.AddInventoryItemAsync(
                    item.PartId,
                    item.Location,
                    item.Operation,
                    item.Quantity,
                    item.ItemType, // Use ItemType property for both ItemType/PartType
                    item.User,
                    item.BatchNumber,
                    "Removal reversed via Undo Button.",
                    true
                );

            MessageBox.Show(@"Undo successful. Removed items have been restored.", @"Undo", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            LoggingUtility.Log("Undo: Removed items restored.");

            _lastRemovedItems.Clear();
            if (Control_RemoveTab_Panel_Footer.Controls["Control_RemoveTab_Button_Undo"] is Button undoBtn)
                undoBtn.Enabled = false;

            Control_RemoveTab_Button_Search_Click(null, null);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show(@"Undo failed: " + ex.Message, @"Undo Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void Control_RemoveTab_Button_Reset_Click()
    {
        try
        {
            LoggingUtility.Log("Inventory Reset button clicked.");
            Control_RemoveTab_ComboBox_Operation.Visible = false;
            Control_RemoveTab_ComboBox_Part.Visible = false;
            Control_RemoveTab_Image_NothingFound.Visible = false;

            Control_RemoveTab_DataGridView_Main.DataSource = null;
            Control_RemoveTab_DataGridView_Main.Rows.Clear();

            await using var connection = new MySqlConnection(Core_WipAppVariables.ConnectionString);
            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_part_ids_Get_All",
                connection,
                _partCbDataAdapter,
                _partCbDataTable,
                Control_RemoveTab_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                connection,
                _opCbDataAdapter,
                _opCbDataTable,
                Control_RemoveTab_ComboBox_Operation,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);

            MainFormControlHelper.ResetComboBox(Control_RemoveTab_ComboBox_Part, Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_RemoveTab_ComboBox_Operation, Color.Red, 0);

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
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Inventory_Button_Reset").ToString());
        }
    }

    private static void Control_RemoveTab_Button_AdvancedItemRemoval_Click()
    {
        try
        {
            if (Service_Timer_VersionChecker.MainFormInstance == null)
            {
                LoggingUtility.Log("MainForm instance is null, cannot open Advanced Inventory Removal.");
                return;
            }

            if (MainFormInstance != null) MainFormInstance.MainForm_RemoveTabNormalControl_Public.Visible = false;
            if (MainFormInstance != null) MainFormInstance.MainForm_RemoveTabAdvancedControl_Public.Visible = true;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("Control_RemoveTab_Button_AdvancedItemRemoval_Click").ToString());
        }
    }

    private static void Control_RemoveTab_Button_Normal_Click(object? sender, EventArgs? e)
    {
        try
        {
            if (Service_Timer_VersionChecker.MainFormInstance == null)
            {
                LoggingUtility.Log("MainForm instance is null, cannot return to normal Remove tab.");
                return;
            }

            if (MainFormInstance != null) MainFormInstance.MainForm_RemoveTabNormalControl_Public.Visible = true;
            if (MainFormInstance != null) MainFormInstance.MainForm_RemoveTabAdvancedControl_Public.Visible = false;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("Control_RemoveTab_Button_Normal_Click").ToString());
        }
    }

    private async void Control_RemoveTab_Button_Search_Click(object? sender, EventArgs? e)
    {
        try
        {
            LoggingUtility.Log("RemoveTab Search button clicked.");

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
                results = await Dao_Inventory.GetInventoryByPartIdAndOperationAsync(partId, op, true);
            else
                results = await Dao_Inventory.GetInventoryByPartIdAsync(partId, true);

            Control_RemoveTab_DataGridView_Main.DataSource = results;
            Control_RemoveTab_DataGridView_Main.ClearSelection();

            foreach (DataGridViewColumn column in Control_RemoveTab_DataGridView_Main.Columns)
                column.Visible = true;

            Core_DgvDesigner.ApplyThemeToDataGridView(
                Control_RemoveTab_DataGridView_Main,
                Core_AppThemes.GetTheme(Core_WipAppVariables.WipDataGridTheme ?? "Default (Black and White)")
            );
            Core_DgvDesigner.SizeDataGrid(Control_RemoveTab_DataGridView_Main);

            Control_RemoveTab_Image_NothingFound.Visible = results.Rows.Count == 0;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("Control_RemoveTab_Button_Search_Click").ToString());
        }
    }

    private void Control_RemoveTab_ComboBox_Operation_SelectedIndexChanged()
    {
        try
        {
            LoggingUtility.Log("Inventory Op ComboBox selection changed.");

            if (Control_RemoveTab_ComboBox_Operation.SelectedIndex > 0)
            {
                Control_RemoveTab_ComboBox_Operation.ForeColor = Color.Black;
                Core_WipAppVariables.Operation = Control_RemoveTab_ComboBox_Operation.Text;
            }
            else
            {
                Control_RemoveTab_ComboBox_Operation.ForeColor = Color.Red;
                if (Control_RemoveTab_ComboBox_Operation.SelectedIndex != 0 &&
                    Control_RemoveTab_ComboBox_Operation.Items.Count > 0)
                    Control_RemoveTab_ComboBox_Operation.SelectedIndex = 0;
                Core_WipAppVariables.Operation = null;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Inventory_ComboBox_Op").ToString());
        }
    }

    private void Control_RemoveTab_ComboBox_Part_SelectedIndexChanged()
    {
        try
        {
            LoggingUtility.Log("Inventory Part ComboBox selection changed.");

            if (Control_RemoveTab_ComboBox_Part.SelectedIndex > 0)
            {
                Control_RemoveTab_ComboBox_Part.ForeColor = Color.Black;
                Core_WipAppVariables.PartId = Control_RemoveTab_ComboBox_Part.Text;
            }
            else
            {
                Control_RemoveTab_ComboBox_Part.ForeColor = Color.Red;
                if (Control_RemoveTab_ComboBox_Part.SelectedIndex != 0 &&
                    Control_RemoveTab_ComboBox_Part.Items.Count > 0)
                    Control_RemoveTab_ComboBox_Part.SelectedIndex = 0;
                Core_WipAppVariables.PartId = null;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Inventory_ComboBox_Part").ToString());
        }
    }

    private void Control_RemoveTab_Update_ButtonStates()
    {
        try
        {
            Control_RemoveTab_Button_Search.Enabled = Control_RemoveTab_ComboBox_Part.SelectedIndex > 0;
            var hasData = Control_RemoveTab_DataGridView_Main.Rows.Count > 0;
            var hasSelection = Control_RemoveTab_DataGridView_Main.SelectedRows.Count > 0;
            Control_RemoveTab_Button_Delete.Enabled = hasData && hasSelection;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("Control_RemoveTab_Update_ButtonStates").ToString());
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

            if (MainFormInstance != null)
            {
                var adv = MainFormInstance.MainForm_RemoveTabAdvancedControl_Public;
                var btn = adv.Controls.Find("Control_AdvancedSearch_Button_Normal", true);
                if (btn.Length > 0 && btn[0] is Button normalBtn)
                {
                    normalBtn.Click -= Control_RemoveTab_Button_Normal_Click;
                    normalBtn.Click += Control_RemoveTab_Button_Normal_Click;
                }
            }

            Control_RemoveTab_ComboBox_Part.Enter += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Part.BackColor = Color.LightBlue;
            };
            Control_RemoveTab_ComboBox_Part.Leave += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Part.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Part, "[ Enter Part ID ]");
            };

            Control_RemoveTab_ComboBox_Operation.Enter += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Operation.BackColor = Color.LightBlue;
            };
            Control_RemoveTab_ComboBox_Operation.Leave += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Operation.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Operation, "[ Enter Op # ]");
            };

            Control_RemoveTab_DataGridView_Main.SelectionChanged += (s, e) => Control_RemoveTab_Update_ButtonStates();

            Control_RemoveTab_Button_Delete.Click += Control_RemoveTab_Button_Delete_Click;

            LoggingUtility.Log("Removal tab events wired up.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_WireUpRemoveTabEvents").ToString());
        }
    }

    private void Control_RemoveTab_Button_Toggle_RightPanel_Click(object sender, EventArgs e)
    {
        if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed)
        {
            MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = true;

            Control_RemoveTab_Button_Toggle_RightPanel.Text = "←";
            Control_RemoveTab_Button_Toggle_RightPanel.ForeColor = Color.Red;
        }
        else
        {
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = false;
                Control_RemoveTab_Button_Toggle_RightPanel.Text = "→";
                Control_RemoveTab_Button_Toggle_RightPanel.ForeColor = Color.Green;
            }
        }

        Helper_ComboBoxes.DeselectAllComboBoxText(this);
    }

    public void UpdateToggleRightPanelButton()
    {
        if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed)
        {
            Control_RemoveTab_Button_Toggle_RightPanel.Text = "→";
            Control_RemoveTab_Button_Toggle_RightPanel.ForeColor = Color.Green;
        }
        else
        {
            Control_RemoveTab_Button_Toggle_RightPanel.Text = "←";
            Control_RemoveTab_Button_Toggle_RightPanel.ForeColor = Color.Red;
        }
    }

    private List<(string PartID, string Location, int Quantity)> GetSelectedItemsToDelete(out string summary)
    {
        var sb = new StringBuilder();
        var itemsToDelete = new List<(string PartID, string Location, int Quantity)>();
        foreach (DataGridViewRow row in Control_RemoveTab_DataGridView_Main.SelectedRows)
            if (row.DataBoundItem is DataRowView drv)
            {
                var partId = drv["PartID"]?.ToString() ?? "";
                var location = drv["Location"]?.ToString() ?? "";
                var quantityStr = drv["Quantity"]?.ToString() ?? "";
                if (!TryParse(quantityStr, out var quantity))
                {
                    LoggingUtility.LogApplicationError(new Exception(
                        $"Invalid quantity value: '{quantityStr}' for PartID={partId}, Location={location}"));
                    continue;
                }

                sb.AppendLine($"PartID: {partId}, Location: {location}, Quantity: {quantity}");
                LoggingUtility.Log($"Selected for deletion: PartID={partId}, Location={location}, Quantity={quantity}");
                System.Diagnostics.Debug.WriteLine(
                    $"[DEBUG] Selected for deletion: PartID={partId}, Location={location}, Quantity={quantity}");

                itemsToDelete.Add((partId, location, quantity));
            }

        summary = sb.ToString();
        return itemsToDelete;
    }
}