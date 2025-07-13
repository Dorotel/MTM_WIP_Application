using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.MainForm.Classes;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;
using Timer = System.Windows.Forms.Timer;

namespace MTM_Inventory_Application.Controls.MainForm;

public partial class Control_AdvancedRemove : UserControl
{
    private readonly List<Model_HistoryRemove> _lastRemovedItems = [];
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    private static void Control_AdvancedRemove_Button_Normal_Click(object? sender, EventArgs? e)
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
                    part.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                    part.Focus();
                }

                if (removeTab.Controls.Find("Control_RemoveTab_ComboBox_Operation", true).FirstOrDefault() is ComboBox
                    op)
                {
                    op.SelectedIndex = 0;
                    op.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                }
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("Control_AdvancedRemove_Button_Normal_Click").ToString());
        }
    }

    public Control_AdvancedRemove()
    {
        InitializeComponent();
        Control_AdvancedRemove_Initialize();
        ApplyStandardComboBoxProperties();
        WireUpComboBoxEvents();
        Core_Themes.ApplyFocusHighlighting(this);
        // Wire up Back to Normal button
        var btn = Controls.Find("Control_AdvancedRemove_Button_Normal", true);
        if (btn.Length > 0 && btn[0] is Button normalBtn)
        {
            normalBtn.Click -= Control_AdvancedRemove_Button_Normal_Click;
            normalBtn.Click += Control_AdvancedRemove_Button_Normal_Click;
            var toolTip = new ToolTip();
            toolTip.SetToolTip(normalBtn,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Normal)}");
        }

        // Wire up Undo button
        var undoBtn = Controls.Find("Control_AdvancedRemove_Button_Undo", true);
        if (undoBtn.Length > 0 && undoBtn[0] is Button undoButton)
        {
            undoButton.Click -= Control_AdvancedRemove_Button_Undo_Click;
            undoButton.Click += Control_AdvancedRemove_Button_Undo_Click;
            var toolTip = new ToolTip();
            toolTip.SetToolTip(undoButton,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Undo)}");
        }

        // Wire up Search button
        var searchBtn = Controls.Find("Control_AdvancedRemove_Button_Search", true);
        if (searchBtn.Length > 0 && searchBtn[0] is Button searchButton)
        {
            searchButton.Click -= Control_AdvancedRemove_Button_Search_Click;
            searchButton.Click += Control_AdvancedRemove_Button_Search_Click;
            var toolTip = new ToolTip();
            toolTip.SetToolTip(searchButton,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Search)}");
        }

        // Wire up Reset button
        var resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
        if (resetBtn.Length > 0 && resetBtn[0] is Button resetButton)
        {
            resetButton.Click -= Control_AdvancedRemove_Button_Reset_Click;
            resetButton.Click += Control_AdvancedRemove_Button_Reset_Click;
            var toolTip = new ToolTip();
            toolTip.SetToolTip(resetButton,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Reset)}");
        }

        // Wire up Delete button
        var deleteBtn = Controls.Find("Control_AdvancedRemove_Button_Delete", true);
        if (deleteBtn.Length > 0 && deleteBtn[0] is Button deleteButton)
        {
            deleteButton.Click -= Control_AdvancedRemove_Button_Delete_Click;
            deleteButton.Click += Control_AdvancedRemove_Button_Delete_Click;
            var toolTip = new ToolTip();
            toolTip.SetToolTip(deleteButton,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Delete)}");
        }

        // Add Undo button if not present
        if (Controls.Find("Control_AdvancedRemove_Button_Undo", true).Length == 0)
            Control_AdvancedRemove_Button_Undo.Click += Control_AdvancedRemove_Button_Undo_Click;

        // Wire up Date checkbox event
        Control_AdvancedRemove_CheckBox_Date.CheckedChanged += (s, e) =>
        {
            var enabled = Control_AdvancedRemove_CheckBox_Date.Checked;
            Control_AdvancedRemove_DateTimePicker_From.Enabled = enabled;
            Control_AdvancedRemove_DateTimePicker_To.Enabled = enabled;
        };
        // Set initial state
        var dateEnabled = Control_AdvancedRemove_CheckBox_Date.Checked;
        Control_AdvancedRemove_DateTimePicker_From.Enabled = dateEnabled;
        Control_AdvancedRemove_DateTimePicker_To.Enabled = dateEnabled;

        _ = LoadComboBoxesAsync();
    }

    private void Control_AdvancedRemove_Initialize()
    {
        Control_AdvancedRemove_ComboBox_Part.ForeColor =
            Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        Control_AdvancedRemove_ComboBox_Op.ForeColor =
            Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        Control_AdvancedRemove_ComboBox_Loc.ForeColor =
            Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        Control_AdvancedRemove_ComboBox_User.ForeColor =
            Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        Control_AdvancedRemove_ComboBox_Like.Items.Clear();
        Control_AdvancedRemove_ComboBox_Like.Items.AddRange([
            "[ Deep Search ]",
            "Part ID",
            "Location",
            "User"
        ]);

        // Optionally set the default selected item
        if (Control_AdvancedRemove_ComboBox_Like.Items.Count > 0)
            Control_AdvancedRemove_ComboBox_Like.SelectedIndex = 0;
    }

    private void ApplyStandardComboBoxProperties()
    {
        Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(Control_AdvancedRemove_ComboBox_Part);
        Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(Control_AdvancedRemove_ComboBox_Op);
        Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(Control_AdvancedRemove_ComboBox_Loc);
        Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(Control_AdvancedRemove_ComboBox_User);
    }

    private async Task LoadComboBoxesAsync()
    {
        try
        {
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_AdvancedRemove_ComboBox_Part);
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(Control_AdvancedRemove_ComboBox_Op);
            await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(Control_AdvancedRemove_ComboBox_Loc);
            await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(Control_AdvancedRemove_ComboBox_User);
            // For user ComboBox, you may need a separate static/shared DataTable/DataAdapter if you want to cache users.
            // Otherwise, keep your current logic for users.
            Control_AdvancedRemove_ComboBox_Part.Visible = true;
            Control_AdvancedRemove_ComboBox_Op.Visible = true;
            Control_AdvancedRemove_ComboBox_Loc.Visible = true;
            Control_AdvancedRemove_ComboBox_User.Visible = true;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
        }
    }

    private void WireUpComboBoxEvents()
    {
        Control_AdvancedRemove_ComboBox_Part.SelectedIndexChanged += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_Part, "[ Enter Part Number ]");
            if (Control_AdvancedRemove_ComboBox_Part.SelectedIndex > 0)
                Control_AdvancedRemove_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
            else
                Control_AdvancedRemove_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        };
        Control_AdvancedRemove_ComboBox_Part.Leave += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_Part,
                "[ Enter Part Number ]");
        };

        Control_AdvancedRemove_ComboBox_Op.SelectedIndexChanged += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_Op, "[ Enter Operation ]");
            if (Control_AdvancedRemove_ComboBox_Op.SelectedIndex > 0)
                Control_AdvancedRemove_ComboBox_Op.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
            else
                Control_AdvancedRemove_ComboBox_Op.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        };
        Control_AdvancedRemove_ComboBox_Op.Leave += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_Op, "[ Enter Operation ]");
        };

        Control_AdvancedRemove_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_Loc, "[ Enter Location ]");
            if (Control_AdvancedRemove_ComboBox_Loc.SelectedIndex > 0)
                Control_AdvancedRemove_ComboBox_Loc.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
            else
                Control_AdvancedRemove_ComboBox_Loc.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        };
        Control_AdvancedRemove_ComboBox_Loc.Leave += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_Loc, "[ Enter Location ]");
        };

        Control_AdvancedRemove_ComboBox_User.SelectedIndexChanged += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_User, "[ Enter User ]");
            if (Control_AdvancedRemove_ComboBox_User.SelectedIndex > 0)
                Control_AdvancedRemove_ComboBox_User.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
            else
                Control_AdvancedRemove_ComboBox_User.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        };
        Control_AdvancedRemove_ComboBox_User.Leave += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_User, "[ Enter User ]");
        };

        // Highlight on enter/leave
        Control_AdvancedRemove_ComboBox_Part.Enter +=
            (s, e) => Control_AdvancedRemove_ComboBox_Part.BackColor =
                Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
        Control_AdvancedRemove_ComboBox_Part.Leave +=
            (s, e) => Control_AdvancedRemove_ComboBox_Part.BackColor =
                Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
        Control_AdvancedRemove_ComboBox_Op.Enter +=
            (s, e) => Control_AdvancedRemove_ComboBox_Op.BackColor =
                Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
        Control_AdvancedRemove_ComboBox_Op.Leave +=
            (s, e) => Control_AdvancedRemove_ComboBox_Op.BackColor =
                Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
        Control_AdvancedRemove_ComboBox_Loc.Enter +=
            (s, e) => Control_AdvancedRemove_ComboBox_Loc.BackColor =
                Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
        Control_AdvancedRemove_ComboBox_Loc.Leave +=
            (s, e) => Control_AdvancedRemove_ComboBox_Loc.BackColor =
                Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
        Control_AdvancedRemove_ComboBox_User.Enter +=
            (s, e) => Control_AdvancedRemove_ComboBox_User.BackColor =
                Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
        Control_AdvancedRemove_ComboBox_User.Leave +=
            (s, e) => Control_AdvancedRemove_ComboBox_User.BackColor =
                Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
    }

    private async void Control_AdvancedRemove_Button_Search_Click(object? sender, EventArgs? e)
    {
        try
        {
            // Declare variables once at the beginning
            var dt = new DataTable();
            MySqlCommand cmd;

            // Determine which type of search to perform and set up the command
            if (!string.IsNullOrWhiteSpace(Control_AdvancedRemove_TextBox_Like.Text) &&
                Control_AdvancedRemove_ComboBox_Like.SelectedIndex > 0)
            {
                // LIKE search setup code...
                // No changes needed here
                var searchText = Control_AdvancedRemove_TextBox_Like.Text.Trim();
                string searchColumn;

                // Determine which column to search based on combobox selection
                switch (Control_AdvancedRemove_ComboBox_Like.SelectedItem!.ToString())
                {
                    case "Part ID":
                        searchColumn = "PartID";
                        break;
                    case "Location":
                        searchColumn = "Location";
                        break;
                    case "User":
                        searchColumn = "User";
                        break;
                    default:
                        MessageBox.Show(@"Please select a valid search field.", @"Search Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }

                // Build the LIKE query
                var query =
                    $"SELECT PartID, Operation, Location, Quantity, Notes, User, ReceiveDate, LastUpdated, BatchNumber " +
                    $"FROM inv_inventory WHERE {searchColumn} LIKE @SearchPattern";

                cmd = new MySqlCommand(query, null);
                cmd.Parameters.AddWithValue("@SearchPattern", $"%{searchText}%");

                LoggingUtility.Log($"[SQL DEBUG] LIKE Search: {query} with pattern '%{searchText}%'");
                Debug.WriteLine($"[SQL DEBUG] LIKE Search: {query} with pattern '%{searchText}%'");
            }
            else
            {
                var part = Control_AdvancedRemove_ComboBox_Part.Text;
                var op = Control_AdvancedRemove_ComboBox_Op.Text;
                var loc = Control_AdvancedRemove_ComboBox_Loc.Text;
                var qtyMinText = Control_AdvancedRemove_TextBox_QtyMin.Text;
                var qtyMaxText = Control_AdvancedRemove_TextBox_QtyMax.Text;
                var notes = Control_AdvancedRemove_TextBox_Notes.Text;
                var user = Control_AdvancedRemove_ComboBox_User.Text;
                var filterByDate = Control_AdvancedRemove_CheckBox_Date.Checked;
                var dateFrom = filterByDate ? Control_AdvancedRemove_DateTimePicker_From.Value.Date : (DateTime?)null;
                var dateTo = filterByDate
                    ? Control_AdvancedRemove_DateTimePicker_To.Value.Date.AddDays(1).AddTicks(-1)
                    : (DateTime?)null;

                int? qtyMin = int.TryParse(qtyMinText, out var qmin) ? qmin : null;
                int? qtyMax = int.TryParse(qtyMaxText, out var qmax) ? qmax : null;

                // Treat ComboBox SelectedIndex == 0 as nothing selected
                var partSelected = Control_AdvancedRemove_ComboBox_Part.SelectedIndex > 0 &&
                                   !string.IsNullOrWhiteSpace(part);
                var opSelected = Control_AdvancedRemove_ComboBox_Op.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(op);
                var locSelected = Control_AdvancedRemove_ComboBox_Loc.SelectedIndex > 0 &&
                                  !string.IsNullOrWhiteSpace(loc);
                var userSelected = Control_AdvancedRemove_ComboBox_User.SelectedIndex > 0 &&
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

                // Build dynamic SQL query with StringBuilder
                var queryBuilder = new StringBuilder();
                queryBuilder.Append(
                    "SELECT * ");
                queryBuilder.Append("FROM inv_inventory WHERE 1=1 ");

                // Add conditions based on user input
                var parameters = new List<MySqlParameter>();

                if (partSelected)
                {
                    queryBuilder.Append("AND PartID = @PartID ");
                    parameters.Add(new MySqlParameter("@PartID", part));
                }

                if (opSelected)
                {
                    queryBuilder.Append("AND Operation = @Operation ");
                    parameters.Add(new MySqlParameter("@Operation", op));
                }

                if (locSelected)
                {
                    queryBuilder.Append("AND Location = @Location ");
                    parameters.Add(new MySqlParameter("@Location", loc));
                }

                if (qtyMin.HasValue)
                {
                    queryBuilder.Append("AND Quantity >= @QtyMin ");
                    parameters.Add(new MySqlParameter("@QtyMin", qtyMin.Value));
                }

                if (qtyMax.HasValue)
                {
                    queryBuilder.Append("AND Quantity <= @QtyMax ");
                    parameters.Add(new MySqlParameter("@QtyMax", qtyMax.Value));
                }

                if (!string.IsNullOrWhiteSpace(notes))
                {
                    queryBuilder.Append("AND Notes LIKE @Notes ");
                    parameters.Add(new MySqlParameter("@Notes", $"%{notes}%"));
                }

                if (userSelected)
                {
                    queryBuilder.Append("AND User = @User ");
                    parameters.Add(new MySqlParameter("@User", user));
                }

                if (filterByDate && dateFrom.HasValue && dateTo.HasValue)
                {
                    queryBuilder.Append("AND ReceiveDate BETWEEN @DateFrom AND @DateTo ");
                    parameters.Add(new MySqlParameter("@DateFrom", dateFrom));
                    parameters.Add(new MySqlParameter("@DateTo", dateTo));
                }

                // Create the command with the dynamically built query
                cmd = new MySqlCommand(queryBuilder.ToString(), null);

                // Add all parameters to command
                foreach (var param in parameters) cmd.Parameters.Add(param);

                // Debug: Show SQL command and parameters
                var debugSql = queryBuilder.ToString();
                foreach (MySqlParameter param in cmd.Parameters)
                    debugSql = debugSql.Replace(param.ParameterName, $"'{param.Value}'");

                LoggingUtility.Log("[SQL DEBUG] " + debugSql);
                Debug.WriteLine("[SQL DEBUG] " + debugSql);
            }

            // Use await using to ensure the connection is properly disposed
            await using (var conn = new MySqlConnection(Model_AppVariables.ConnectionString))
            {
                cmd.Connection = conn;
                await conn.OpenAsync();

                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            // Filter columns if necessary
            if (cmd.CommandType == CommandType.StoredProcedure)
            {
                var allowedColumns = new[]
                {
                    "PartID", "Operation", "Location", "Quantity", "Notes", "User", "ReceiveDate", "LastUpdated",
                    "BatchNumber"
                };
                foreach (var col in dt.Columns.Cast<DataColumn>().ToList()
                             .Where(col => !allowedColumns.Contains(col.ColumnName)))
                    dt.Columns.Remove(col.ColumnName);
            }

            // Display results - no changes needed here
            Control_AdvancedRemove_DataGridView_Results.DataSource = dt;
            Control_AdvancedRemove_DataGridView_Results.ClearSelection();
            foreach (DataGridViewColumn column in Control_AdvancedRemove_DataGridView_Results.Columns)
                column.Visible = true;

            Core_Themes.ApplyThemeToDataGridView(Control_AdvancedRemove_DataGridView_Results);
            Core_Themes.SizeDataGrid(Control_AdvancedRemove_DataGridView_Results);

            Control_AdvancedRemove_Image_NothingFound.Visible = dt.Rows.Count == 0;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show($@"Error during advanced search: {ex.Message}", @"Search Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void Control_AdvancedRemove_Button_Delete_Click(object? sender, EventArgs? e)
    {
        try
        {
            var dgv = Control_AdvancedRemove_DataGridView_Results;
            var selectedCount = dgv.SelectedRows.Count;
            LoggingUtility.Log($"[ADVANCED REMOVE] Delete clicked. Selected rows: {selectedCount}");
            if (selectedCount == 0)
            {
                LoggingUtility.Log("[ADVANCED REMOVE] No rows selected for deletion.");
                return;
            }

            // Build summary for confirmation
            var sb = new StringBuilder();
            foreach (DataGridViewRow row in dgv.SelectedRows)
            {
                var partId = row.Cells["PartID"].Value?.ToString() ?? "";
                var location = row.Cells["Location"].Value?.ToString() ?? "";
                var operation = row.Cells["Operation"].Value?.ToString() ?? "";
                var quantity = row.Cells["Quantity"].Value?.ToString() ?? "";
                sb.AppendLine($"PartID: {partId}, Location: {location}, Operation: {operation}, Quantity: {quantity}");
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
                return;
            }

            // Call DAO to remove items
            var removedCount = await Dao_Inventory.RemoveInventoryItemsFromDataGridViewAsync(dgv);

            // Optionally update undo and status logic here...

            LoggingUtility.Log($"[ADVANCED REMOVE] {removedCount} inventory items deleted.");
            Control_AdvancedRemove_Button_Search_Click(null, null);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true);
        }
    }

    private async Task Control_AdvancedRemove_HardReset()
    {
        var resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
        if (resetBtn.Length > 0 && resetBtn[0] is Button btn)
            btn.Enabled = false;
        try
        {
            ControlRemoveTab.MainFormInstance?.TabLoadingProgress?.ShowProgress();
            ControlRemoveTab.MainFormInstance?.TabLoadingProgress?.UpdateProgress(10,
                "Resetting Advanced Remove tab...");
            Debug.WriteLine("[DEBUG] AdvancedRemove HardReset - start");
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Updating status strip for hard reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            ControlRemoveTab.MainFormInstance?.TabLoadingProgress?.UpdateProgress(30, "Resetting data tables...");
            // Hide controls during reset
            Debug.WriteLine("[DEBUG] Hiding ComboBoxes");
            Control_AdvancedRemove_ComboBox_Part.Visible = false;
            Control_AdvancedRemove_ComboBox_Op.Visible = false;
            Control_AdvancedRemove_ComboBox_Loc.Visible = false;
            Control_AdvancedRemove_ComboBox_User.Visible = false;

            // Reset and refresh all ComboBox DataTables
            Debug.WriteLine("[DEBUG] Resetting and refreshing all ComboBox DataTables");
            await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
            Debug.WriteLine("[DEBUG] DataTables reset complete");

            ControlRemoveTab.MainFormInstance?.TabLoadingProgress?.UpdateProgress(60, "Refilling combo boxes...");
            // Refill each combobox with proper data
            Debug.WriteLine("[DEBUG] Refilling Part ComboBox");
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_AdvancedRemove_ComboBox_Part);
            Debug.WriteLine("[DEBUG] Refilling Operation ComboBox");
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(Control_AdvancedRemove_ComboBox_Op);
            Debug.WriteLine("[DEBUG] Refilling Location ComboBox");
            await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(Control_AdvancedRemove_ComboBox_Loc);
            Debug.WriteLine("[DEBUG] Refilling User ComboBox");
            await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(Control_AdvancedRemove_ComboBox_User);

            // Reset ComboBoxes to default state
            MainFormControlHelper.ResetComboBox(Control_AdvancedRemove_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_AdvancedRemove_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_AdvancedRemove_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_AdvancedRemove_ComboBox_User,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);

            // Clear text boxes
            Control_AdvancedRemove_TextBox_QtyMin.Text = string.Empty;
            Control_AdvancedRemove_TextBox_QtyMax.Text = string.Empty;
            Control_AdvancedRemove_TextBox_Notes.Text = string.Empty;

            // Reset Date controls
            Control_AdvancedRemove_CheckBox_Date.Checked = false;
            Control_AdvancedRemove_DateTimePicker_From.Value = DateTime.Today;
            Control_AdvancedRemove_DateTimePicker_To.Value = DateTime.Today;
            Control_AdvancedRemove_DateTimePicker_From.Enabled = false;
            Control_AdvancedRemove_DateTimePicker_To.Enabled = false;

            // Reset DataGridView
            Control_AdvancedRemove_DataGridView_Results.DataSource = null;
            Control_AdvancedRemove_DataGridView_Results.Rows.Clear();
            Control_AdvancedRemove_Image_NothingFound.Visible = false;

            // Restore controls and focus
            Debug.WriteLine("[DEBUG] Restoring ComboBox visibility and focus");
            Control_AdvancedRemove_ComboBox_Part.Visible = true;
            Control_AdvancedRemove_ComboBox_Op.Visible = true;
            Control_AdvancedRemove_ComboBox_Loc.Visible = true;
            Control_AdvancedRemove_ComboBox_User.Visible = true;
            if (Control_AdvancedRemove_ComboBox_Part.FindForm() is { } form)
                MainFormControlHelper.SetActiveControl(form, Control_AdvancedRemove_ComboBox_Part);
            Debug.WriteLine("[DEBUG] AdvancedRemove HardReset - end");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in AdvancedRemove HardReset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "Control_AdvancedRemove_HardReset");
        }
        finally
        {
            Debug.WriteLine("[DEBUG] AdvancedRemove HardReset button re-enabled");
            if (resetBtn.Length > 0 && resetBtn[0] is Button btn2)
                btn2.Enabled = true;
            if (ControlRemoveTab.MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Restoring status strip after hard reset");
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
                ControlRemoveTab.MainFormInstance?.TabLoadingProgress?.HideProgress();
            }
        }
    }

    private void Control_AdvancedRemove_SoftReset()
    {
        var resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
        if (resetBtn.Length > 0 && resetBtn[0] is Button btn)
            btn.Enabled = false;
        try
        {
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Updating status strip for Soft Reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            // Reset UI fields
            MainFormControlHelper.ResetComboBox(Control_AdvancedRemove_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_AdvancedRemove_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_AdvancedRemove_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_AdvancedRemove_ComboBox_User,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);

            Control_AdvancedRemove_TextBox_QtyMin.Text = string.Empty;
            Control_AdvancedRemove_TextBox_QtyMax.Text = string.Empty;
            Control_AdvancedRemove_TextBox_Notes.Text = string.Empty;

            Control_AdvancedRemove_CheckBox_Date.Checked = false;
            Control_AdvancedRemove_DateTimePicker_From.Value = DateTime.Today;
            Control_AdvancedRemove_DateTimePicker_To.Value = DateTime.Today;
            Control_AdvancedRemove_DateTimePicker_From.Enabled = false;
            Control_AdvancedRemove_DateTimePicker_To.Enabled = false;

            Control_AdvancedRemove_DataGridView_Results.DataSource = null;
            Control_AdvancedRemove_DataGridView_Results.Rows.Clear();
            Control_AdvancedRemove_Image_NothingFound.Visible = false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in AdvancedRemove SoftReset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "Control_AdvancedRemove_SoftReset");
        }
        finally
        {
            Debug.WriteLine("[DEBUG] AdvancedRemove SoftReset button re-enabled");
            if (resetBtn.Length > 0 && resetBtn[0] is Button btn2)
                btn2.Enabled = true;
            if (ControlRemoveTab.MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Restoring status strip after soft reset");
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
            }

            if (Control_AdvancedRemove_ComboBox_Part.FindForm() is { } form)
                MainFormControlHelper.SetActiveControl(form, Control_AdvancedRemove_ComboBox_Part);
        }
    }

    private async void Control_AdvancedRemove_Button_Reset_Click(object? sender, EventArgs? e)
    {
        Control_AdvancedRemove_TextBox_Like.Clear();
        Control_AdvancedRemove_ComboBox_Like.SelectedIndex = 0;
        if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            await Control_AdvancedRemove_HardReset();
        else
            Control_AdvancedRemove_SoftReset();
    }

    private async void Control_AdvancedRemove_Button_Undo_Click(object? sender, EventArgs? e)
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
            var undoBtn = Controls.Find("Control_AdvancedRemove_Button_Undo", true);
            if (undoBtn.Length > 0 && undoBtn[0] is Button btn)
                btn.Enabled = false;

            Control_AdvancedRemove_Button_Search_Click(null, null);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show(@"Undo failed: " + ex.Message, @"Undo Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Core_WipAppVariables.Shortcut_Remove_Delete)
        {
            Control_AdvancedRemove_Button_Delete.PerformClick();
            return true;
        }

        if (keyData == Core_WipAppVariables.Shortcut_Remove_Undo)
        {
            Control_AdvancedRemove_Button_Undo.PerformClick();
            return true;
        }

        if (keyData == Core_WipAppVariables.Shortcut_Remove_Reset)
        {
            var resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
            if (resetBtn.Length > 0 && resetBtn[0] is Button btn)
            {
                btn.PerformClick();
                return true;
            }
        }

        if (keyData == Core_WipAppVariables.Shortcut_Remove_Search)
        {
            var searchBtn = Controls.Find("Control_AdvancedRemove_Button_Search", true);
            if (searchBtn.Length > 0 && searchBtn[0] is Button btn)
            {
                btn.PerformClick();
                return true;
            }
        }

        if (keyData == Core_WipAppVariables.Shortcut_Remove_Normal)
        {
            var normalBtn = Controls.Find("Control_AdvancedRemove_Button_Normal", true);
            if (normalBtn.Length > 0 && normalBtn[0] is Button btn)
            {
                btn.PerformClick();
                return true;
            }
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void Control_AdvancedRemove_ComboBox_Like_SelectedIndexChanged(object sender, EventArgs e)
    {
        var isDeepSearch = Control_AdvancedRemove_ComboBox_Like.SelectedIndex == 0;

        // Enable/disable all search criteria controls based on selection
        Control_AdvancedRemove_ComboBox_Part.Enabled = isDeepSearch;
        Control_AdvancedRemove_ComboBox_Loc.Enabled = isDeepSearch;
        Control_AdvancedRemove_ComboBox_Op.Enabled = isDeepSearch;
        Control_AdvancedRemove_ComboBox_User.Enabled = isDeepSearch;
        Control_AdvancedRemove_TextBox_QtyMin.Enabled = isDeepSearch;
        Control_AdvancedRemove_TextBox_QtyMax.Enabled = isDeepSearch;
        Control_AdvancedRemove_CheckBox_Date.Enabled = isDeepSearch;
        Control_AdvancedRemove_DateTimePicker_From.Enabled =
            isDeepSearch && Control_AdvancedRemove_CheckBox_Date.Checked;
        Control_AdvancedRemove_DateTimePicker_To.Enabled = isDeepSearch && Control_AdvancedRemove_CheckBox_Date.Checked;
        Control_AdvancedRemove_TextBox_Notes.Enabled = isDeepSearch;

        // Enable/disable the Like textbox (opposite of other controls)
        Control_AdvancedRemove_TextBox_Like.Enabled = !isDeepSearch;

        // Set focus to appropriate control based on mode
        if (isDeepSearch)
        {
            Control_AdvancedRemove_ComboBox_Part.Focus();
            Control_AdvancedRemove_TextBox_Like.Clear();
        }
        else
        {
            Control_AdvancedRemove_TextBox_Like.Focus();
        }
    }


    private void Control_AdvancedRemove_Button_SidePanel_Click(object sender, EventArgs e)
    {
        // Toggle the visibility of Panel2 (right panel) in the split container
        var splitContainer = Control_AdvancedRemove_SplitContainer_Main;
        var button = sender as Button ?? Control_AdvancedRemove_Button_SidePanel;

        if (splitContainer.Panel1Collapsed)
        {
            splitContainer.Panel1Collapsed = false;
            button.Text = "Collapse ◀";
        }
        else
        {
            splitContainer.Panel1Collapsed = true;
            button.Text = "Expand ▶";
        }
    }
}