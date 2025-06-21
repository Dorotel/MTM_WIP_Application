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
    private readonly List<Model_HistoryRemove> _lastRemovedItems = [];

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
                ControlRemoveTab.MainFormInstance.MainForm_RemoveTabNormalControl.Visible = true;
            if (ControlRemoveTab.MainFormInstance != null)
                ControlRemoveTab.MainFormInstance.MainForm_Control_AdvancedRemove.Visible = false;

            // Reset all Control_RemoveTab.cs ComboBoxes' SelectedIndex to 0 and color to Red
            var removeTab = ControlRemoveTab.MainFormInstance?.MainForm_RemoveTabNormalControl;
            if (removeTab != null)
            {
                if (removeTab.Controls.Find("Control_RemoveTab_ComboBox_Part", true).FirstOrDefault() is ComboBox part)
                {
                    part.SelectedIndex = 0;
                    part.ForeColor = Color.Red;
                    part.Focus();
                }

                if (removeTab.Controls.Find("Control_RemoveTab_ComboBox_Operation", true).FirstOrDefault() is ComboBox
                    op)
                {
                    op.SelectedIndex = 0;
                    op.ForeColor = Color.Red;
                }
            }
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
            await Helper_ComboBoxes.FillPartComboBoxesAsync(Control_AdvancedSearch_ComboBox_Part);
            await Helper_ComboBoxes.FillOperationComboBoxesAsync(Control_AdvancedSearch_ComboBox_Op);
            await Helper_ComboBoxes.FillLocationComboBoxesAsync(Control_AdvancedSearch_ComboBox_Loc);
            await Helper_ComboBoxes.FillUserComboBoxesAsync(Control_AdvancedSearch_ComboBox_User);
            // For user ComboBox, you may need a separate static/shared DataTable/DataAdapter if you want to cache users.
            // Otherwise, keep your current logic for users.
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
            // Validate input fields
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

            // Treat ComboBox SelectedIndex == 0 as nothing selected
            var partSelected = Control_AdvancedSearch_ComboBox_Part.SelectedIndex > 0 &&
                               !string.IsNullOrWhiteSpace(part);
            var opSelected = Control_AdvancedSearch_ComboBox_Op.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(op);
            var locSelected = Control_AdvancedSearch_ComboBox_Loc.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(loc);
            var userSelected = Control_AdvancedSearch_ComboBox_User.SelectedIndex > 0 &&
                               !string.IsNullOrWhiteSpace(user);

            // Check if at least one field is filled
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

            // Debug: Log parameter values
            LoggingUtility.Log(
                $"[DEBUG] Advanced Search Parameters: PartID='{(partSelected ? part : null)}', Operation='{(opSelected ? op : null)}', Location='{(locSelected ? loc : null)}', QtyMin='{qtyMin}', QtyMax='{qtyMax}', Notes='{notes}', User='{(userSelected ? user : null)}', DateFrom='{(dateFrom.HasValue ? dateFrom.Value.ToString("yyyy-MM-dd") : "NULL")}', DateTo='{(dateTo.HasValue ? dateTo.Value.ToString("yyyy-MM-dd") : "NULL")}'");
            System.Diagnostics.Debug.WriteLine(
                $"[DEBUG] Advanced Search Parameters: PartID='{(partSelected ? part : null)}', Operation='{(opSelected ? op : null)}', Location='{(locSelected ? loc : null)}', QtyMin='{qtyMin}', QtyMax='{qtyMax}', Notes='{notes}', User='{(userSelected ? user : null)}', DateFrom='{(dateFrom.HasValue ? dateFrom.Value.ToString("yyyy-MM-dd") : "NULL")}', DateTo='{(dateTo.HasValue ? dateTo.Value.ToString("yyyy-MM-dd") : "NULL")}'");

            // Build query
            var dt = new DataTable();
            await using var conn = new MySqlConnection(Core_WipAppVariables.ConnectionString);
            await using var cmd = new MySqlCommand("inv_inventory_Advanced_Search", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@p_PartID", partSelected ? part : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_Operation", opSelected ? op : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_Location", locSelected ? loc : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_QtyMin", qtyMin ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_QtyMax", qtyMax ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_Notes", string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes);
            cmd.Parameters.AddWithValue("@p_User", userSelected ? user : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_DateFrom", dateFrom ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_DateTo", dateTo ?? (object)DBNull.Value);

            // Debug: Show SQL command and parameters
            var debugSql =
                $"CALL inv_inventory_Advanced_Search(\n  @p_PartID = '{cmd.Parameters["@p_PartID"].Value}',\n  @p_Operation = '{cmd.Parameters["@p_Operation"].Value}',\n  @p_Location = '{cmd.Parameters["@p_Location"].Value}',\n  @p_QtyMin = '{cmd.Parameters["@p_QtyMin"].Value}',\n  @p_QtyMax = '{cmd.Parameters["@p_QtyMax"].Value}',\n  @p_Notes = '{cmd.Parameters["@p_Notes"].Value}',\n  @p_User = '{cmd.Parameters["@p_User"].Value}',\n  @p_DateFrom = '{cmd.Parameters["@p_DateFrom"].Value}',\n  @p_DateTo = '{cmd.Parameters["@p_DateTo"].Value}'\n)";
            LoggingUtility.Log($"[DEBUG] SQL Command: {debugSql}");
            System.Diagnostics.Debug.WriteLine($"[DEBUG] SQL Command: {debugSql}");

            await conn.OpenAsync();
            using var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            // Debug: Log result count
            LoggingUtility.Log($"[DEBUG] Advanced Search Results: {dt.Rows.Count} rows returned.");
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Advanced Search Results: {dt.Rows.Count} rows returned.");

            // Only show columns that exist in the procedure's SELECT
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
            MessageBox.Show($@"Error during advanced search: {ex.Message}", @"Search Error", MessageBoxButtons.OK,
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
        var resetBtn = Controls.Find("Control_AdvancedSearch_Button_Reset", true);
        if (resetBtn.Length > 0 && resetBtn[0] is Button btn)
            btn.Enabled = false;
        try
        {
            // Hide controls during reset
            Control_AdvancedSearch_ComboBox_Part.Visible = false;
            Control_AdvancedSearch_ComboBox_Op.Visible = false;
            Control_AdvancedSearch_ComboBox_Loc.Visible = false;
            Control_AdvancedSearch_ComboBox_User.Visible = false;
            if (ControlRemoveTab.MainFormInstance != null)
            {
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Please wait while resetting...";
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            await Helper_ComboBoxes.SetupPartDataTable();
            await Helper_ComboBoxes.SetupOperationDataTable();
            await Helper_ComboBoxes.SetupLocationDataTable();
            await Helper_ComboBoxes.SetupUserDataTable();
            await Helper_ComboBoxes.FillPartComboBoxesAsync(Control_AdvancedSearch_ComboBox_Part);
            await Helper_ComboBoxes.FillOperationComboBoxesAsync(Control_AdvancedSearch_ComboBox_Op);
            await Helper_ComboBoxes.FillLocationComboBoxesAsync(Control_AdvancedSearch_ComboBox_Loc);
            await Helper_ComboBoxes.FillUserComboBoxesAsync(Control_AdvancedSearch_ComboBox_User);
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
            // Restore controls and focus
            Control_AdvancedSearch_ComboBox_Part.Visible = true;
            Control_AdvancedSearch_ComboBox_Op.Visible = true;
            Control_AdvancedSearch_ComboBox_Loc.Visible = true;
            Control_AdvancedSearch_ComboBox_User.Visible = true;
            if (Control_AdvancedSearch_ComboBox_Part.FindForm() is { } form)
                MainFormControlHelper.SetActiveControl(form, Control_AdvancedSearch_ComboBox_Part);
            if (ControlRemoveTab.MainFormInstance != null)
            {
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
            }
        }
        finally
        {
            if (resetBtn.Length > 0 && resetBtn[0] is Button btn2)
                btn2.Enabled = true;
        }
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