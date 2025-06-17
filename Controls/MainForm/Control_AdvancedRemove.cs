using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Logging;
using MySql.Data.MySqlClient;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Forms.MainForm.Classes;
using System.Collections.Generic;
using MTM_WIP_Application.Models;

namespace MTM_WIP_Application.Controls.MainForm;

public partial class Control_AdvancedRemove : UserControl
{
    private readonly List<Model_HistoryRemove> _lastRemovedItems = new();

    // Shared DataTables and Adapters for ComboBoxes
    private readonly DataTable _partCbDataTable = new();
    private readonly DataTable _opCbDataTable = new();
    private readonly DataTable _locCbDataTable = new();
    private readonly DataTable _userCbDataTable = new();
    private readonly MySqlDataAdapter _partCbDataAdapter = new();
    private readonly MySqlDataAdapter _opCbDataAdapter = new();
    private readonly MySqlDataAdapter _locCbDataAdapter = new();
    private readonly MySqlDataAdapter _userCbDataAdapter = new();

    private static void Control_AdvancedSearch_Button_Normal_Click(object? sender, EventArgs? e)
    {
        try
        {
            if (Services.Service_Timer_VersionChecker.MainFormInstance == null)
            {
                LoggingUtility.Log("MainForm instance is null, cannot return to normal Remove tab.");
                return;
            }

            if (ControlRemoveTab.MainFormInstance != null)
                ControlRemoveTab.MainFormInstance.MainForm_RemoveTabNormalControl_Public.Visible = true;
            if (ControlRemoveTab.MainFormInstance != null)
                ControlRemoveTab.MainFormInstance.MainForm_RemoveTabAdvancedControl_Public.Visible = false;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("Control_AdvancedSearch_Button_Normal_Click").ToString());
        }
    }

    public Control_AdvancedRemove()
    {
        InitializeComponent();
        Control_AdvancedRemove_Initialize();
        ApplyStandardComboBoxProperties();
        WireUpComboBoxEvents();
        // Wire up Back to Normal button
        var btn = Controls.Find("Control_AdvancedSearch_Button_Normal", true);
        if (btn.Length > 0 && btn[0] is Button normalBtn)
        {
            normalBtn.Click -= Control_AdvancedSearch_Button_Normal_Click;
            normalBtn.Click += Control_AdvancedSearch_Button_Normal_Click;
        }

        // Wire up Undo button
        var undoBtn = Controls.Find("Control_AdvancedSearch_Button_Undo", true);
        if (undoBtn.Length > 0 && undoBtn[0] is Button undoButton)
        {
            undoButton.Click -= Control_AdvancedSearch_Button_Undo_Click;
            undoButton.Click += Control_AdvancedSearch_Button_Undo_Click;
        }

        // Wire up Search button
        var searchBtn = Controls.Find("Control_AdvancedSearch_Button_Search", true);
        if (searchBtn.Length > 0 && searchBtn[0] is Button searchButton)
        {
            searchButton.Click -= Control_AdvancedSearch_Button_Search_Click;
            searchButton.Click += Control_AdvancedSearch_Button_Search_Click;
        }

        // Wire up Reset button
        var resetBtn = Controls.Find("Control_AdvancedSearch_Button_Reset", true);
        if (resetBtn.Length > 0 && resetBtn[0] is Button resetButton)
        {
            resetButton.Click -= Control_AdvancedSearch_Button_Reset_Click;
            resetButton.Click += Control_AdvancedSearch_Button_Reset_Click;
        }

        // Wire up Delete button
        var deleteBtn = Controls.Find("Control_AdvancedSearch_Button_Delete", true);
        if (deleteBtn.Length > 0 && deleteBtn[0] is Button deleteButton)
        {
            deleteButton.Click -= Control_AdvancedSearch_Button_Delete_Click;
            deleteButton.Click += Control_AdvancedSearch_Button_Delete_Click;
        }

        // Add Undo button if not present
        if (Controls.Find("Control_AdvancedSearch_Button_Undo", true).Length == 0)
            Control_AdvancedSearch_Button_Undo.Click += Control_AdvancedSearch_Button_Undo_Click;

        // Wire up Date checkbox event
        Control_AdvancedSearch_CheckBox_Date.CheckedChanged += (s, e) =>
        {
            var enabled = Control_AdvancedSearch_CheckBox_Date.Checked;
            Control_AdvancedSearch_DateTimePicker_From.Enabled = enabled;
            Control_AdvancedSearch_DateTimePicker_To.Enabled = enabled;
        };
        // Set initial state
        var dateEnabled = Control_AdvancedSearch_CheckBox_Date.Checked;
        Control_AdvancedSearch_DateTimePicker_From.Enabled = dateEnabled;
        Control_AdvancedSearch_DateTimePicker_To.Enabled = dateEnabled;

        _ = LoadComboBoxesAsync();
    }

    private void Control_AdvancedRemove_Initialize()
    {
        Control_AdvancedSearch_ComboBox_Part.Visible = false;
        Control_AdvancedSearch_ComboBox_Op.Visible = false;
        Control_AdvancedSearch_ComboBox_Loc.Visible = false;
        Control_AdvancedSearch_ComboBox_User.Visible = false;
        Control_AdvancedSearch_ComboBox_Part.ForeColor = Color.Red;
        Control_AdvancedSearch_ComboBox_Op.ForeColor = Color.Red;
        Control_AdvancedSearch_ComboBox_Loc.ForeColor = Color.Red;
        Control_AdvancedSearch_ComboBox_User.ForeColor = Color.Red;
    }

    private void ApplyStandardComboBoxProperties()
    {
        Helper_ComboBoxes.ApplyStandardComboBoxProperties(Control_AdvancedSearch_ComboBox_Part);
        Helper_ComboBoxes.ApplyStandardComboBoxProperties(Control_AdvancedSearch_ComboBox_Op);
        Helper_ComboBoxes.ApplyStandardComboBoxProperties(Control_AdvancedSearch_ComboBox_Loc);
        Helper_ComboBoxes.ApplyStandardComboBoxProperties(Control_AdvancedSearch_ComboBox_User);
    }

    private async Task LoadComboBoxesAsync()
    {
        try
        {
            await using var connection = new MySqlConnection(Core_WipAppVariables.ConnectionString);
            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_part_ids_Get_All",
                connection,
                _partCbDataAdapter,
                _partCbDataTable,
                Control_AdvancedSearch_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                connection,
                _opCbDataAdapter,
                _opCbDataTable,
                Control_AdvancedSearch_ComboBox_Op,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "md_locations_Get_All",
                connection,
                _locCbDataAdapter,
                _locCbDataTable,
                Control_AdvancedSearch_ComboBox_Loc,
                "Location",
                "Location",
                "[ Enter Location ]",
                CommandType.StoredProcedure);

            await Helper_ComboBoxes.FillComboBoxAsync(
                "usr_users_Get_All",
                connection,
                _userCbDataAdapter,
                _userCbDataTable,
                Control_AdvancedSearch_ComboBox_User,
                "User",
                "User",
                "[ Enter User ]",
                CommandType.StoredProcedure);

            Control_AdvancedSearch_ComboBox_Part.Visible = true;
            Control_AdvancedSearch_ComboBox_Op.Visible = true;
            Control_AdvancedSearch_ComboBox_Loc.Visible = true;
            Control_AdvancedSearch_ComboBox_User.Visible = true;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
        }
    }

    private void WireUpComboBoxEvents()
    {
        Control_AdvancedSearch_ComboBox_Part.SelectedIndexChanged += (s, e) =>
        {
            if (Control_AdvancedSearch_ComboBox_Part.SelectedIndex > 0)
                Control_AdvancedSearch_ComboBox_Part.ForeColor = Color.Black;
            else
                Control_AdvancedSearch_ComboBox_Part.ForeColor = Color.Red;
        };
        Control_AdvancedSearch_ComboBox_Op.SelectedIndexChanged += (s, e) =>
        {
            if (Control_AdvancedSearch_ComboBox_Op.SelectedIndex > 0)
                Control_AdvancedSearch_ComboBox_Op.ForeColor = Color.Black;
            else
                Control_AdvancedSearch_ComboBox_Op.ForeColor = Color.Red;
        };
        Control_AdvancedSearch_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
        {
            if (Control_AdvancedSearch_ComboBox_Loc.SelectedIndex > 0)
                Control_AdvancedSearch_ComboBox_Loc.ForeColor = Color.Black;
            else
                Control_AdvancedSearch_ComboBox_Loc.ForeColor = Color.Red;
        };
        Control_AdvancedSearch_ComboBox_User.SelectedIndexChanged += (s, e) =>
        {
            if (Control_AdvancedSearch_ComboBox_User.SelectedIndex > 0)
                Control_AdvancedSearch_ComboBox_User.ForeColor = Color.Black;
            else
                Control_AdvancedSearch_ComboBox_User.ForeColor = Color.Red;
        };
        // Highlight on enter/leave
        Control_AdvancedSearch_ComboBox_Part.Enter +=
            (s, e) => Control_AdvancedSearch_ComboBox_Part.BackColor = Color.LightBlue;
        Control_AdvancedSearch_ComboBox_Part.Leave +=
            (s, e) => Control_AdvancedSearch_ComboBox_Part.BackColor = SystemColors.Window;
        Control_AdvancedSearch_ComboBox_Op.Enter +=
            (s, e) => Control_AdvancedSearch_ComboBox_Op.BackColor = Color.LightBlue;
        Control_AdvancedSearch_ComboBox_Op.Leave +=
            (s, e) => Control_AdvancedSearch_ComboBox_Op.BackColor = SystemColors.Window;
        Control_AdvancedSearch_ComboBox_Loc.Enter +=
            (s, e) => Control_AdvancedSearch_ComboBox_Loc.BackColor = Color.LightBlue;
        Control_AdvancedSearch_ComboBox_Loc.Leave +=
            (s, e) => Control_AdvancedSearch_ComboBox_Loc.BackColor = SystemColors.Window;
        Control_AdvancedSearch_ComboBox_User.Enter +=
            (s, e) => Control_AdvancedSearch_ComboBox_User.BackColor = Color.LightBlue;
        Control_AdvancedSearch_ComboBox_User.Leave +=
            (s, e) => Control_AdvancedSearch_ComboBox_User.BackColor = SystemColors.Window;
    }

    private async void Control_AdvancedSearch_Button_Search_Click(object? sender, EventArgs? e)
    {
        try
        {
            var part = Control_AdvancedSearch_ComboBox_Part.Text.Trim();
            var op = Control_AdvancedSearch_ComboBox_Op.Text.Trim();
            var loc = Control_AdvancedSearch_ComboBox_Loc.Text.Trim();
            var qtyMinText = Control_AdvancedSearch_TextBox_QtyMin.Text.Trim();
            var qtyMaxText = Control_AdvancedSearch_TextBox_QtyMax.Text.Trim();
            var notes = Control_AdvancedSearch_TextBox_Notes.Text.Trim();
            var user = Control_AdvancedSearch_ComboBox_User.Text.Trim();
            var filterByDate = Control_AdvancedSearch_CheckBox_Date.Checked;
            var dateFrom = filterByDate ? Control_AdvancedSearch_DateTimePicker_From.Value.Date : (DateTime?)null;
            var dateTo = filterByDate ? Control_AdvancedSearch_DateTimePicker_To.Value.Date : (DateTime?)null;

            int? qtyMin = int.TryParse(qtyMinText, out var qmin) ? qmin : null;
            int? qtyMax = int.TryParse(qtyMaxText, out var qmax) ? qmax : null;

            var partSelected = Control_AdvancedSearch_ComboBox_Part.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(part);
            var opSelected = Control_AdvancedSearch_ComboBox_Op.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(op);
            var locSelected = Control_AdvancedSearch_ComboBox_Loc.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(loc);
            var userSelected = Control_AdvancedSearch_ComboBox_User.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(user);

            var anyFieldFilled =
                partSelected ||
                opSelected ||
                locSelected ||
                qtyMin.HasValue ||
                qtyMax.HasValue ||
                !string.IsNullOrWhiteSpace(notes) ||
                userSelected ||
                (filterByDate && dateFrom != null && dateTo != null);

            if (!anyFieldFilled)
            {
                MessageBox.Show(@"Please fill in at least one field to search.", @"Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (filterByDate && dateFrom > dateTo)
            {
                MessageBox.Show(@"The 'From' date cannot be after the 'To' date.", @"Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoggingUtility.Log(
                $"[DEBUG] Advanced Search Parameters: PartID='{(partSelected ? part : null)}', Operation='{(opSelected ? op : null)}', Location='{(locSelected ? loc : null)}', QtyMin='{qtyMin}', QtyMax='{qtyMax}', Notes='{notes}', User='{(userSelected ? user : null)}', DateFrom='{(dateFrom.HasValue ? dateFrom.Value.ToString("yyyy-MM-dd") : "NULL")}', DateTo='{(dateTo.HasValue ? dateTo.Value.ToString("yyyy-MM-dd") : "NULL")}'");
            System.Diagnostics.Debug.WriteLine(
                $"[DEBUG] Advanced Search Parameters: PartID='{(partSelected ? part : null)}', Operation='{(opSelected ? op : null)}', Location='{(locSelected ? loc : null)}', QtyMin='{qtyMin}', QtyMax='{qtyMax}', Notes='{notes}', User='{(userSelected ? user : null)}', DateFrom='{(dateFrom.HasValue ? dateFrom.Value.ToString("yyyy-MM-dd") : "NULL")}', DateTo='{(dateTo.HasValue ? dateTo.Value.ToString("yyyy-MM-dd") : "NULL")}'");

            var dt = await Dao_Inventory.GetInventoryAdvancedSearchAsync(
                partSelected ? part : null,
                opSelected ? op : null,
                locSelected ? loc : null,
                qtyMin,
                qtyMax,
                string.IsNullOrWhiteSpace(notes) ? null : notes,
                userSelected ? user : null,
                dateFrom,
                dateTo
            );

            LoggingUtility.Log($"[DEBUG] Advanced Search Results: {dt.Rows.Count} rows returned.");
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Advanced Search Results: {dt.Rows.Count} rows returned.");

            var allowedColumns = new[]
            {
                "PartID", "Operation", "Location", "Quantity", "Notes", "User", "ReceiveDate", "LastUpdated",
                "BatchNumber"
            };
            foreach (var col in dt.Columns.Cast<DataColumn>().ToList()
                         .Where(col => !allowedColumns.Contains(col.ColumnName)))
                dt.Columns.Remove(col.ColumnName);

            Control_AdvancedSearch_DataGridView_Results.DataSource = dt;
            Control_AdvancedSearch_DataGridView_Results.ClearSelection();
            foreach (DataGridViewColumn column in Control_AdvancedSearch_DataGridView_Results.Columns)
                column.Visible = true;

            Core_DgvDesigner.ApplyThemeToDataGridView(
                Control_AdvancedSearch_DataGridView_Results,
                Core_AppThemes.GetTheme(Core_WipAppVariables.WipDataGridTheme ?? "Default (Black and White)")
            );
            Core_DgvDesigner.SizeDataGrid(Control_AdvancedSearch_DataGridView_Results);

            Control_AdvancedSearch_Image_NothingFound.Visible = dt.Rows.Count == 0;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show(@$"Error during advanced search: {ex.Message}", @"Search Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void Control_AdvancedSearch_Button_Delete_Click(object? sender, EventArgs? e)
    {
        try
        {
            var selectedCount = Control_AdvancedSearch_DataGridView_Results.SelectedRows.Count;
            LoggingUtility.Log($"[ADVANCED REMOVE] Delete clicked. Selected rows: {selectedCount}");
            System.Diagnostics.Debug.WriteLine($"[ADVANCED REMOVE] Delete clicked. Selected rows: {selectedCount}");

            if (selectedCount == 0)
            {
                LoggingUtility.Log("[ADVANCED REMOVE] No rows selected for deletion.");
                System.Diagnostics.Debug.WriteLine("[ADVANCED REMOVE] No rows selected for deletion.");
                return;
            }

            // Build summary and collect items
            var sb = new StringBuilder();
            var itemsToDelete = new List<Model_HistoryRemove>();
            var partIds = new HashSet<string>();
            var operations = new HashSet<string>();
            var locations = new HashSet<string>();
            var totalQty = 0;
            foreach (DataGridViewRow row in Control_AdvancedSearch_DataGridView_Results.SelectedRows)
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
                    var itemType = drv.DataView.Table != null && drv.DataView.Table.Columns.Contains("Item Type")
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

                    itemsToDelete.Add(new Model_HistoryRemove
                    {
                        PartId = partId,
                        Location = location,
                        Operation = operation,
                        Quantity = quantity,
                        ItemType = itemType,
                        ReceiveDate = receivedDate,
                        LastUpdated = lastUpdate,
                        User = user,
                        BatchNumber = batchNumber,
                        Notes = notes
                    });

                    sb.AppendLine(
                        $"PartID: {partId}, Location: {location}, Operation: {operation}, Quantity: {quantity}");
                    partIds.Add(partId);
                    operations.Add(operation);
                    locations.Add(location);
                    totalQty += quantity;
                }

            var summary = sb.ToString();
            var confirmResult = MessageBox.Show(
                $@"The following items will be deleted:

{summary}Are you sure?",
                @"Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes)
            {
                LoggingUtility.Log("[ADVANCED REMOVE] User cancelled deletion.");
                System.Diagnostics.Debug.WriteLine("[ADVANCED REMOVE] User cancelled deletion.");
                return;
            }

            _lastRemovedItems.Clear();
            foreach (var item in itemsToDelete)
            {
                _lastRemovedItems.Add(item);
                LoggingUtility.Log(
                    $"[ADVANCED REMOVE] Deleting: PartID={item.PartId}, Location={item.Location}, Operation={item.Operation}, Quantity={item.Quantity}");
                System.Diagnostics.Debug.WriteLine(
                    $"[ADVANCED REMOVE] Deleting: PartID={item.PartId}, Location={item.Location}, Operation={item.Operation}, Quantity={item.Quantity}");
                await Dao_Inventory.DeleteInventoryByPartIdLocationOperationQuantityAsync(
                    item.PartId,
                    item.Location,
                    item.Operation,
                    item.BatchNumber,
                    item.Quantity
                );
                LoggingUtility.Log(
                    $"[ADVANCED REMOVE] Deleted: PartID={item.PartId}, Location={item.Location}, Operation={item.Operation}, Quantity={item.Quantity}");
                System.Diagnostics.Debug.WriteLine(
                    $"[ADVANCED REMOVE] Deleted: PartID={item.PartId}, Location={item.Location}, Operation={item.Operation}, Quantity={item.Quantity}");
            }

            // Enable Undo button if items were deleted
            var undoBtn = Controls.Find("Control_AdvancedSearch_Button_Undo", true);
            if (_lastRemovedItems.Count > 0 && undoBtn.Length > 0 && undoBtn[0] is Button btn)
                btn.Enabled = true;

            // Update status strip
            if (ControlRemoveTab.MainFormInstance != null && itemsToDelete.Count > 0)
            {
                var time = DateTime.Now.ToString("hh:mm tt");
                var locDisplay = locations.Count > 1 ? "Multiple Locations" : locations.FirstOrDefault() ?? "";
                if (partIds.Count == 1 && operations.Count == 1)
                    ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $@"Last Deleted: {partIds.First()} (Op: {operations.First()}), Location: {locDisplay}, Quantity: {totalQty} @ {time}";
                else if (partIds.Count == 1 && operations.Count > 1)
                    ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $@"Last Deleted: {partIds.First()} (Multiple Ops), Location: {locDisplay}, Quantity: {totalQty} @ {time}";
                else
                    ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $@"Last Deleted: Multiple Part IDs, Location: {locDisplay}, Quantity: Multiple @ {time}";
            }

            LoggingUtility.Log("[ADVANCED REMOVE] Selected inventory items deleted.");
            System.Diagnostics.Debug.WriteLine("[ADVANCED REMOVE] Selected inventory items deleted.");

            Control_AdvancedSearch_Button_Search_Click(null, null);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            System.Diagnostics.Debug.WriteLine($"[ADVANCED REMOVE] Exception: {ex}");
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
        }
    }

    private async void Control_AdvancedSearch_Button_Reset_Click(object? sender, EventArgs? e)
    {
        // Hide controls while resetting
        Control_AdvancedSearch_ComboBox_Part.Visible = false;
        Control_AdvancedSearch_ComboBox_Op.Visible = false;
        Control_AdvancedSearch_ComboBox_Loc.Visible = false;
        Control_AdvancedSearch_ComboBox_User.Visible = false;

        // Reinitialize ComboBox DataTables using shared DataTables/adapters
        await using var connection = new MySqlConnection(Core_WipAppVariables.ConnectionString);
        await Helper_ComboBoxes.FillComboBoxAsync(
            "md_part_ids_Get_All",
            connection,
            _partCbDataAdapter,
            _partCbDataTable,
            Control_AdvancedSearch_ComboBox_Part,
            "Item Number",
            "ID",
            "[ Enter Part ID ]",
            CommandType.StoredProcedure);

        await Helper_ComboBoxes.FillComboBoxAsync(
            "md_operation_numbers_Get_All",
            connection,
            _opCbDataAdapter,
            _opCbDataTable,
            Control_AdvancedSearch_ComboBox_Op,
            "Operation",
            "Operation",
            "[ Enter Op # ]",
            CommandType.StoredProcedure);

        await Helper_ComboBoxes.FillComboBoxAsync(
            "md_locations_Get_All",
            connection,
            _locCbDataAdapter,
            _locCbDataTable,
            Control_AdvancedSearch_ComboBox_Loc,
            "Location",
            "Location",
            "[ Enter Location ]",
            CommandType.StoredProcedure);

        await Helper_ComboBoxes.FillComboBoxAsync(
            "usr_users_Get_All",
            connection,
            _userCbDataAdapter,
            _userCbDataTable,
            Control_AdvancedSearch_ComboBox_User,
            "User",
            "User",
            "[ Enter User ]",
            CommandType.StoredProcedure);

        // Reset all ComboBoxes
        MainFormControlHelper.ResetComboBox(Control_AdvancedSearch_ComboBox_Part, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(Control_AdvancedSearch_ComboBox_Op, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(Control_AdvancedSearch_ComboBox_Loc, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(Control_AdvancedSearch_ComboBox_User, Color.Red, 0);

        // Reset TextBoxes
        Control_AdvancedSearch_TextBox_QtyMin.Text = string.Empty;
        Control_AdvancedSearch_TextBox_QtyMax.Text = string.Empty;
        Control_AdvancedSearch_TextBox_Notes.Text = string.Empty;

        // Reset Date controls
        Control_AdvancedSearch_CheckBox_Date.Checked = false;
        Control_AdvancedSearch_DateTimePicker_From.Value = DateTime.Today;
        Control_AdvancedSearch_DateTimePicker_To.Value = DateTime.Today;
        Control_AdvancedSearch_DateTimePicker_From.Enabled = false;
        Control_AdvancedSearch_DateTimePicker_To.Enabled = false;

        // Reset DataGridView
        Control_AdvancedSearch_DataGridView_Results.DataSource = null;
        Control_AdvancedSearch_DataGridView_Results.Rows.Clear();
        Control_AdvancedSearch_Image_NothingFound.Visible = false;

        // Show controls again
        Control_AdvancedSearch_ComboBox_Part.Visible = true;
        Control_AdvancedSearch_ComboBox_Op.Visible = true;
        Control_AdvancedSearch_ComboBox_Loc.Visible = true;
        Control_AdvancedSearch_ComboBox_User.Visible = true;

        // Set focus to first ComboBox
        if (Control_AdvancedSearch_ComboBox_Part.FindForm() is { } form)
            MainFormControlHelper.SetActiveControl(form, Control_AdvancedSearch_ComboBox_Part);
    }

    private async void Control_AdvancedSearch_Button_Undo_Click(object? sender, EventArgs? e)
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
                    item.ItemType,
                    item.User,
                    item.BatchNumber,
                    "Removal reversed via Undo Button.",
                    true
                );

            MessageBox.Show(@"Undo successful. Removed items have been restored.", @"Undo", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            LoggingUtility.Log("Undo: Removed items restored (Advanced Remove tab).");

            _lastRemovedItems.Clear();
            var undoBtn = Controls.Find("Control_AdvancedSearch_Button_Undo", true);
            if (undoBtn.Length > 0 && undoBtn[0] is Button btn)
                btn.Enabled = false;

            Control_AdvancedSearch_Button_Search_Click(null, null);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show(@"Undo failed: " + ex.Message, @"Undo Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Delete)
        {
            // Simulate Delete button click
            Control_AdvancedSearch_Button_Delete.PerformClick();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}