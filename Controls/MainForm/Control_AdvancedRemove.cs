

using System.Data;
using System.Diagnostics;
using System.Text;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.MainForm.Classes;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Controls.MainForm;

public partial class Control_AdvancedRemove : UserControl
{
    #region Fields
    

    private readonly List<Model_HistoryRemove> _lastRemovedItems = [];
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }
    
    #endregion
    
    #region Constructors
    

    public Control_AdvancedRemove()
    {
        InitializeComponent();
        Control_AdvancedRemove_Initialize();
        ApplyStandardComboBoxProperties();
        WireUpComboBoxEvents();
        Core_Themes.ApplyFocusHighlighting(this);
        Control[] btn = Controls.Find("Control_AdvancedRemove_Button_Normal", true);
        if (btn.Length > 0 && btn[0] is Button normalBtn)
        {
            normalBtn.Click -= Control_AdvancedRemove_Button_Normal_Click;
            normalBtn.Click += Control_AdvancedRemove_Button_Normal_Click;
            ToolTip toolTip = new();
            toolTip.SetToolTip(normalBtn,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Normal)}");
        }

        Control[] undoBtn = Controls.Find("Control_AdvancedRemove_Button_Undo", true);
        if (undoBtn.Length > 0 && undoBtn[0] is Button undoButton)
        {
            undoButton.Click -= Control_AdvancedRemove_Button_Undo_Click;
            undoButton.Click += Control_AdvancedRemove_Button_Undo_Click;
            ToolTip toolTip = new();
            toolTip.SetToolTip(undoButton,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Undo)}");
        }

        Control[] searchBtn = Controls.Find("Control_AdvancedRemove_Button_Search", true);
        if (searchBtn.Length > 0 && searchBtn[0] is Button searchButton)
        {
            searchButton.Click -= Control_AdvancedRemove_Button_Search_Click;
            searchButton.Click += Control_AdvancedRemove_Button_Search_Click;
            ToolTip toolTip = new();
            toolTip.SetToolTip(searchButton,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Search)}");
        }

        Control[] resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
        if (resetBtn.Length > 0 && resetBtn[0] is Button resetButton)
        {
            resetButton.Click -= Control_AdvancedRemove_Button_Reset_Click;
            resetButton.Click += Control_AdvancedRemove_Button_Reset_Click;
            ToolTip toolTip = new();
            toolTip.SetToolTip(resetButton,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Reset)}");
        }

        Control[] deleteBtn = Controls.Find("Control_AdvancedRemove_Button_Delete", true);
        if (deleteBtn.Length > 0 && deleteBtn[0] is Button deleteButton)
        {
            deleteButton.Click -= Control_AdvancedRemove_Button_Delete_Click;
            deleteButton.Click += Control_AdvancedRemove_Button_Delete_Click;
            ToolTip toolTip = new();
            toolTip.SetToolTip(deleteButton,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Delete)}");
        }

        if (Controls.Find("Control_AdvancedRemove_Button_Undo", true).Length == 0)
        {
            Control_AdvancedRemove_Button_Undo.Click += Control_AdvancedRemove_Button_Undo_Click;
        }

        Control_AdvancedRemove_CheckBox_Date.CheckedChanged += (s, e) =>
        {
            bool enabled = Control_AdvancedRemove_CheckBox_Date.Checked;
            Control_AdvancedRemove_DateTimePicker_From.Enabled = enabled;
            Control_AdvancedRemove_DateTimePicker_To.Enabled = enabled;
        };
        bool dateEnabled = Control_AdvancedRemove_CheckBox_Date.Checked;
        Control_AdvancedRemove_DateTimePicker_From.Enabled = dateEnabled;
        Control_AdvancedRemove_DateTimePicker_To.Enabled = dateEnabled;

        _ = LoadComboBoxesAsync();
    }
    
    #endregion
    
    #region Methods
    

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
            {
                ControlRemoveTab.MainFormInstance.MainForm_RemoveTabNormalControl.Visible = true;
            }

            if (ControlRemoveTab.MainFormInstance != null)
            {
                ControlRemoveTab.MainFormInstance.MainForm_Control_AdvancedRemove.Visible = false;
            }

            ControlRemoveTab? removeTab = ControlRemoveTab.MainFormInstance?.MainForm_RemoveTabNormalControl;
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

        if (Control_AdvancedRemove_ComboBox_Like.Items.Count > 0)
        {
            Control_AdvancedRemove_ComboBox_Like.SelectedIndex = 0;
        }
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
            {
                Control_AdvancedRemove_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
            }
            else
            {
                Control_AdvancedRemove_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            }
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
            {
                Control_AdvancedRemove_ComboBox_Op.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
            }
            else
            {
                Control_AdvancedRemove_ComboBox_Op.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            }
        };
        Control_AdvancedRemove_ComboBox_Op.Leave += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_Op, "[ Enter Operation ]");
        };

        Control_AdvancedRemove_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_Loc, "[ Enter Location ]");
            if (Control_AdvancedRemove_ComboBox_Loc.SelectedIndex > 0)
            {
                Control_AdvancedRemove_ComboBox_Loc.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
            }
            else
            {
                Control_AdvancedRemove_ComboBox_Loc.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            }
        };
        Control_AdvancedRemove_ComboBox_Loc.Leave += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_Loc, "[ Enter Location ]");
        };

        Control_AdvancedRemove_ComboBox_User.SelectedIndexChanged += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_User, "[ Enter User ]");
            if (Control_AdvancedRemove_ComboBox_User.SelectedIndex > 0)
            {
                Control_AdvancedRemove_ComboBox_User.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
            }
            else
            {
                Control_AdvancedRemove_ComboBox_User.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            }
        };
        Control_AdvancedRemove_ComboBox_User.Leave += (s, e) =>
        {
            Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_AdvancedRemove_ComboBox_User, "[ Enter User ]");
        };

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
            DataTable dt = new();
            MySqlCommand cmd;

            if (!string.IsNullOrWhiteSpace(Control_AdvancedRemove_TextBox_Like.Text) &&
                Control_AdvancedRemove_ComboBox_Like.SelectedIndex > 0)
            {
                string searchText = Control_AdvancedRemove_TextBox_Like.Text.Trim();
                string searchColumn;

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

                string query =
                    $"SELECT PartID, Operation, Location, Quantity, Notes, User, ReceiveDate, LastUpdated, BatchNumber " +
                    $"FROM inv_inventory WHERE {searchColumn} LIKE @SearchPattern";

                cmd = new MySqlCommand(query, null);
                cmd.Parameters.AddWithValue("@SearchPattern", $"%{searchText}%");

                LoggingUtility.Log($"[SQL DEBUG] LIKE Search: {query} with pattern '%{searchText}%'");
                Debug.WriteLine($"[SQL DEBUG] LIKE Search: {query} with pattern '%{searchText}%'");
            }
            else
            {
                string part = Control_AdvancedRemove_ComboBox_Part.Text;
                string op = Control_AdvancedRemove_ComboBox_Op.Text;
                string loc = Control_AdvancedRemove_ComboBox_Loc.Text;
                string qtyMinText = Control_AdvancedRemove_TextBox_QtyMin.Text;
                string qtyMaxText = Control_AdvancedRemove_TextBox_QtyMax.Text;
                string notes = Control_AdvancedRemove_TextBox_Notes.Text;
                string user = Control_AdvancedRemove_ComboBox_User.Text;
                bool filterByDate = Control_AdvancedRemove_CheckBox_Date.Checked;
                DateTime? dateFrom =
                    filterByDate ? Control_AdvancedRemove_DateTimePicker_From.Value.Date : (DateTime?)null;
                DateTime? dateTo = filterByDate
                    ? Control_AdvancedRemove_DateTimePicker_To.Value.Date.AddDays(1).AddTicks(-1)
                    : (DateTime?)null;

                int? qtyMin = int.TryParse(qtyMinText, out int qmin) ? qmin : null;
                int? qtyMax = int.TryParse(qtyMaxText, out int qmax) ? qmax : null;

                bool partSelected = Control_AdvancedRemove_ComboBox_Part.SelectedIndex > 0 &&
                                    !string.IsNullOrWhiteSpace(part);
                bool opSelected = Control_AdvancedRemove_ComboBox_Op.SelectedIndex > 0 &&
                                  !string.IsNullOrWhiteSpace(op);
                bool locSelected = Control_AdvancedRemove_ComboBox_Loc.SelectedIndex > 0 &&
                                   !string.IsNullOrWhiteSpace(loc);
                bool userSelected = Control_AdvancedRemove_ComboBox_User.SelectedIndex > 0 &&
                                    !string.IsNullOrWhiteSpace(user);

                bool anyFieldFilled =
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

                StringBuilder queryBuilder = new();
                queryBuilder.Append(
                    "SELECT * ");
                queryBuilder.Append("FROM inv_inventory WHERE 1=1 ");

                List<MySqlParameter> parameters = new();

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

                cmd = new MySqlCommand(queryBuilder.ToString(), null);

                foreach (MySqlParameter param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                string debugSql = queryBuilder.ToString();
                foreach (MySqlParameter param in cmd.Parameters)
                {
                    debugSql = debugSql.Replace(param.ParameterName, $"'{param.Value}'");
                }

                LoggingUtility.Log("[SQL DEBUG] " + debugSql);
                Debug.WriteLine("[SQL DEBUG] " + debugSql);
            }

            await using (MySqlConnection conn = new(Model_AppVariables.ConnectionString))
            {
                cmd.Connection = conn;
                await conn.OpenAsync();

                using (MySqlDataAdapter adapter = new(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            if (cmd.CommandType == CommandType.StoredProcedure)
            {
                string[] allowedColumns = new[]
                {
                    "PartID", "Operation", "Location", "Quantity", "Notes", "User", "ReceiveDate", "LastUpdated",
                    "BatchNumber"
                };
                foreach (DataColumn? col in dt.Columns.Cast<DataColumn>().ToList()
                             .Where(col => !allowedColumns.Contains(col.ColumnName)))
                {
                    dt.Columns.Remove(col.ColumnName);
                }
            }

            Control_AdvancedRemove_DataGridView_Results.DataSource = dt;
            Control_AdvancedRemove_DataGridView_Results.ClearSelection();
            foreach (DataGridViewColumn column in Control_AdvancedRemove_DataGridView_Results.Columns)
            {
                column.Visible = true;
            }

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
            DataGridView? dgv = Control_AdvancedRemove_DataGridView_Results;
            int selectedCount = dgv.SelectedRows.Count;
            LoggingUtility.Log($"[ADVANCED REMOVE] Delete clicked. Selected rows: {selectedCount}");
            if (selectedCount == 0)
            {
                LoggingUtility.Log("[ADVANCED REMOVE] No rows selected for deletion.");
                return;
            }

            StringBuilder sb = new();
            foreach (DataGridViewRow row in dgv.SelectedRows)
            {
                string partId = row.Cells["PartID"].Value?.ToString() ?? "";
                string location = row.Cells["Location"].Value?.ToString() ?? "";
                string operation = row.Cells["Operation"].Value?.ToString() ?? "";
                string quantity = row.Cells["Quantity"].Value?.ToString() ?? "";
                sb.AppendLine($"PartID: {partId}, Location: {location}, Operation: {operation}, Quantity: {quantity}");
            }

            string summary = sb.ToString();

            DialogResult confirmResult = MessageBox.Show(
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

            int removedCount = await Dao_Inventory.RemoveInventoryItemsFromDataGridViewAsync(dgv);

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
        Control[] resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
        if (resetBtn.Length > 0 && resetBtn[0] is Button btn)
        {
            btn.Enabled = false;
        }

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
            Debug.WriteLine("[DEBUG] Hiding ComboBoxes");
            Control_AdvancedRemove_ComboBox_Part.Visible = false;
            Control_AdvancedRemove_ComboBox_Op.Visible = false;
            Control_AdvancedRemove_ComboBox_Loc.Visible = false;
            Control_AdvancedRemove_ComboBox_User.Visible = false;

            Debug.WriteLine("[DEBUG] Resetting and refreshing all ComboBox DataTables");
            await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
            Debug.WriteLine("[DEBUG] DataTables reset complete");

            ControlRemoveTab.MainFormInstance?.TabLoadingProgress?.UpdateProgress(60, "Refilling combo boxes...");
            Debug.WriteLine("[DEBUG] Refilling Part ComboBox");
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_AdvancedRemove_ComboBox_Part);
            Debug.WriteLine("[DEBUG] Refilling Operation ComboBox");
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(Control_AdvancedRemove_ComboBox_Op);
            Debug.WriteLine("[DEBUG] Refilling Location ComboBox");
            await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(Control_AdvancedRemove_ComboBox_Loc);
            Debug.WriteLine("[DEBUG] Refilling User ComboBox");
            await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(Control_AdvancedRemove_ComboBox_User);

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

            Debug.WriteLine("[DEBUG] Restoring ComboBox visibility and focus");
            Control_AdvancedRemove_ComboBox_Part.Visible = true;
            Control_AdvancedRemove_ComboBox_Op.Visible = true;
            Control_AdvancedRemove_ComboBox_Loc.Visible = true;
            Control_AdvancedRemove_ComboBox_User.Visible = true;
            if (Control_AdvancedRemove_ComboBox_Part.FindForm() is { } form)
            {
                MainFormControlHelper.SetActiveControl(form, Control_AdvancedRemove_ComboBox_Part);
            }

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
            {
                btn2.Enabled = true;
            }

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
        Control[] resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
        if (resetBtn.Length > 0 && resetBtn[0] is Button btn)
        {
            btn.Enabled = false;
        }

        try
        {
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Updating status strip for Soft Reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

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
            {
                btn2.Enabled = true;
            }

            if (ControlRemoveTab.MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Restoring status strip after soft reset");
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                ControlRemoveTab.MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
            }

            if (Control_AdvancedRemove_ComboBox_Part.FindForm() is { } form)
            {
                MainFormControlHelper.SetActiveControl(form, Control_AdvancedRemove_ComboBox_Part);
            }
        }
    }

    private async void Control_AdvancedRemove_Button_Reset_Click(object? sender, EventArgs? e)
    {
        Control_AdvancedRemove_TextBox_Like.Clear();
        Control_AdvancedRemove_ComboBox_Like.SelectedIndex = 0;
        if ((ModifierKeys & Keys.Shift) == Keys.Shift)
        {
            await Control_AdvancedRemove_HardReset();
        }
        else
        {
            Control_AdvancedRemove_SoftReset();
        }
    }

    private async void Control_AdvancedRemove_Button_Undo_Click(object? sender, EventArgs? e)
    {
        if (_lastRemovedItems.Count == 0)
        {
            return;
        }

        try
        {
            foreach (Model_HistoryRemove item in _lastRemovedItems)
            {
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
            }

            MessageBox.Show(@"Undo successful. Removed items have been restored.", @"Undo", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            LoggingUtility.Log("Undo: Removed items restored (Advanced Remove tab).");

            _lastRemovedItems.Clear();
            Control[] undoBtn = Controls.Find("Control_AdvancedRemove_Button_Undo", true);
            if (undoBtn.Length > 0 && undoBtn[0] is Button btn)
            {
                btn.Enabled = false;
            }

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
            Control[] resetBtn = Controls.Find("Control_AdvancedRemove_Button_Reset", true);
            if (resetBtn.Length > 0 && resetBtn[0] is Button btn)
            {
                btn.PerformClick();
                return true;
            }
        }

        if (keyData == Core_WipAppVariables.Shortcut_Remove_Search)
        {
            Control[] searchBtn = Controls.Find("Control_AdvancedRemove_Button_Search", true);
            if (searchBtn.Length > 0 && searchBtn[0] is Button btn)
            {
                btn.PerformClick();
                return true;
            }
        }

        if (keyData == Core_WipAppVariables.Shortcut_Remove_Normal)
        {
            Control[] normalBtn = Controls.Find("Control_AdvancedRemove_Button_Normal", true);
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
        bool isDeepSearch = Control_AdvancedRemove_ComboBox_Like.SelectedIndex == 0;

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

        Control_AdvancedRemove_TextBox_Like.Enabled = !isDeepSearch;

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
        SplitContainer? splitContainer = Control_AdvancedRemove_SplitContainer_Main;
        Button? button = sender as Button ?? Control_AdvancedRemove_Button_SidePanel;

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

    
    #endregion
}
