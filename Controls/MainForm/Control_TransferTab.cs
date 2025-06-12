using System.ComponentModel;
using System.Data;
using System.Text;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.AdvancedInventoryEntryForm;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Services;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Controls.MainForm;

public partial class ControlTransferTab : UserControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    public ControlTransferTab()
    {
        InitializeComponent();
        Control_TransferTab_Initialize();

        Control_TransferTab_ComboBox_Part.ForeColor = Color.Red;
        Control_TransferTab_ComboBox_Operation.ForeColor = Color.Red;
        Control_TransferTab_ComboBox_ToLocation.ForeColor = Color.Red;
        Control_TransferTab_Image_NothingFound.Visible = false;
        _ = Control_TransferTab_OnStartup_LoadComboBoxes();
    }

    public void Control_TransferTab_Initialize()
    {
        Control_TransferTab_ComboBox_Operation.Visible = false;
        Control_TransferTab_ComboBox_Part.Visible = false;
        Control_TransferTab_Button_Reset.TabStop = false;
    }

    private async void Control_TransferTab_Button_Reset_Click()
    {
        try
        {
            AppLogger.Log("Inventory Reset button clicked.");
            Control_TransferTab_ComboBox_Operation.Visible = false;
            Control_TransferTab_ComboBox_Part.Visible = false;
            Control_TransferTab_Image_NothingFound.Visible = false;

            // Reinitialize ComboBox DataTables
            await ComboBoxHelpers.FillComboBoxAsync(
                "md_part_ids_Get_All",
                new MySqlConnection(WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                Control_TransferTab_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);

            await ComboBoxHelpers.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                new MySqlConnection(WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                Control_TransferTab_ComboBox_Operation,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);

            if (MainFormInstance != null)
                MainFormTabResetHelper.ResetTransferTab(
                    Control_TransferTab_ComboBox_Part,
                    Control_TransferTab_ComboBox_Operation,
                    Control_TransferTab_Button_Search,
                    Control_TransferTab_Button_Delete
                );

            Control_TransferTab_ComboBox_Operation.Visible = true;
            Control_TransferTab_ComboBox_Part.Visible = true;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_Button_Reset");
        }
    }

    private void Control_TransferTab_ComboBox_Operation_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Op ComboBox selection changed.");

            if (Control_TransferTab_ComboBox_Operation.SelectedIndex > 0)
            {
                Control_TransferTab_ComboBox_Operation.ForeColor = Color.Black;
                WipAppVariables.Operation = Control_TransferTab_ComboBox_Operation.Text;
            }
            else
            {
                Control_TransferTab_ComboBox_Operation.ForeColor = Color.Red;
                if (Control_TransferTab_ComboBox_Operation.SelectedIndex != 0 &&
                    Control_TransferTab_ComboBox_Operation.Items.Count > 0)
                    Control_TransferTab_ComboBox_Operation.SelectedIndex = 0;
                WipAppVariables.Operation = null;
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Op");
        }
    }

    private void Control_TransferTab_ComboBox_Part_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Part ComboBox selection changed.");

            if (Control_TransferTab_ComboBox_Part.SelectedIndex > 0)
            {
                Control_TransferTab_ComboBox_Part.ForeColor = Color.Black;
                WipAppVariables.PartId = Control_TransferTab_ComboBox_Part.Text;
            }
            else
            {
                Control_TransferTab_ComboBox_Part.ForeColor = Color.Red;
                if (Control_TransferTab_ComboBox_Part.SelectedIndex != 0 &&
                    Control_TransferTab_ComboBox_Part.Items.Count > 0)
                    Control_TransferTab_ComboBox_Part.SelectedIndex = 0;
                WipAppVariables.PartId = null;
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Part");
        }
    }

    private async Task Control_TransferTab_LoadData_ComboBoxes_Async()
    {
        try
        {
            await using var connection = new MySqlConnection(WipAppVariables.ConnectionString);

            var comboBoxSets =
                new (MySqlDataAdapter Adapter, DataTable Table, ComboBox ComboBox, string ProcName, string Display,
                    string Value, string Placeholder, CommandType CommandType)[]
                    {
                        (new MySqlDataAdapter(), new DataTable(), Control_TransferTab_ComboBox_Part,
                            "md_part_ids_Get_All", "Item Number", "ID", "[ Enter Part ID ]",
                            CommandType.StoredProcedure),
                        (new MySqlDataAdapter(), new DataTable(), Control_TransferTab_ComboBox_Operation,
                            "md_operation_numbers_Get_All", "Operation", "Operation", "[ Enter Op # ]",
                            CommandType.StoredProcedure),
                        (new MySqlDataAdapter(), new DataTable(), Control_TransferTab_ComboBox_ToLocation,
                            "md_locations_Get_All", "Location", "Location", "[ Enter Location ]",
                            CommandType.StoredProcedure)
                    };

            foreach (var (adapter, table, comboBox, procName, display, value, placeholder, cmdType) in comboBoxSets)
                await ComboBoxHelpers.FillComboBoxAsync(
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
            AppLogger.Log("Transfer tab ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                "MainForm_LoadTransferTabComboBoxesAsync");
        }
    }

    private async Task Control_TransferTab_OnStartup_LoadComboBoxes()
    {
        try
        {
            await Control_TransferTab_LoadData_ComboBoxes_Async();
            Control_TransferTab_OnStartup_WireUpEvents();
            AppLogger.Log("Initial setup of ComboBoxes in the Inventory Tab.");
            if (MainFormInstance != null)
                MainFormTabResetHelper.ResetTransferTab(
                    Control_TransferTab_ComboBox_Part,
                    Control_TransferTab_ComboBox_Operation,
                    Control_TransferTab_Button_Search,
                    Control_TransferTab_Button_Delete
                );
            Control_TransferTab_ComboBox_Operation.Visible = true;
            Control_TransferTab_ComboBox_Part.Visible = true;

            try
            {
                WipAppVariables.UserFullName = await UserDao.GetUserFullNameAsync(WipAppVariables.User, true);
                AppLogger.Log($"User full name loaded: {WipAppVariables.UserFullName}");
            }
            catch (Exception ex)
            {
                AppLogger.LogApplicationError(ex);
                await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_TransferTab_OnStartup_GetUserFullName").ToString());
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "Control_TransferTab_OnStartup");
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

            if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed &&
                keyData == (Keys.Alt | Keys.Right))
            {
                Control_TransferTab_Button_Toggle_RightPanel.PerformClick(); // Triggers the button's Click event
                return true;
            }

            if (MainFormInstance != null && MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed &&
                keyData == (Keys.Alt | Keys.Left))
            {
                Control_TransferTab_Button_Toggle_RightPanel.PerformClick(); // Triggers the button's Click event
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

    private void Control_TransferTab_Update_ButtonStates()
    {
        try
        {
            Control_TransferTab_Button_Search.Enabled = Control_TransferTab_ComboBox_Part.SelectedIndex > 0;
            var hasData = Control_TransferTab_DataGridView_Main.Rows.Count > 0;
            var hasSelection = Control_TransferTab_DataGridView_Main.SelectedRows.Count > 0;
            var hasToLocation = Control_TransferTab_ComboBox_ToLocation.SelectedIndex >= 0 &&
                                !string.IsNullOrWhiteSpace(Control_TransferTab_ComboBox_ToLocation.Text);
            var hasQuantity = Control_TransferTab_NumericUpDown_Quantity.Value > 0;
            Control_TransferTab_Button_Delete.Enabled = hasData && hasSelection && hasToLocation && hasQuantity;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "Control_TransferTab_Update_ButtonStates");
        }
    }

    private void Control_TransferTab_OnStartup_WireUpEvents()
    {
        try
        {
            Control_TransferTab_Button_Reset.Click += (s, e) => Control_TransferTab_Button_Reset_Click();
            Control_TransferTab_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                Control_TransferTab_ComboBox_Part_SelectedIndexChanged();
                Control_TransferTab_Update_ButtonStates();
            };
            Control_TransferTab_ComboBox_Operation.SelectedIndexChanged += (s, e) =>
            {
                Control_TransferTab_ComboBox_Operation_SelectedIndexChanged();
                Control_TransferTab_Update_ButtonStates();
            };
            Control_TransferTab_ComboBox_ToLocation.SelectedIndexChanged +=
                (s, e) => Control_TransferTab_Update_ButtonStates();
            Control_TransferTab_NumericUpDown_Quantity.ValueChanged +=
                (s, e) => Control_TransferTab_Update_ButtonStates();
            Control_TransferTab_ComboBox_Part.Enter += (s, e) =>
            {
                Control_TransferTab_ComboBox_Part.BackColor = SystemColors.Window;
            };
            Control_TransferTab_ComboBox_Part.Leave += (s, e) =>
            {
                Control_TransferTab_ComboBox_Part.BackColor = SystemColors.Window;
                ComboBoxHelpers.ValidateComboBoxItem(Control_TransferTab_ComboBox_Part, "[ Enter Part ID ]");
            };
            Control_TransferTab_ComboBox_Operation.Enter += (s, e) =>
            {
                Control_TransferTab_ComboBox_Operation.BackColor = SystemColors.Window;
            };
            Control_TransferTab_ComboBox_Operation.Leave += (s, e) =>
            {
                Control_TransferTab_ComboBox_Operation.BackColor = SystemColors.Window;
                ComboBoxHelpers.ValidateComboBoxItem(Control_TransferTab_ComboBox_Operation, "[ Enter Op # ]");
            };
            Control_TransferTab_ComboBox_ToLocation.Enter += (s, e) =>
            {
                Control_TransferTab_ComboBox_ToLocation.BackColor = SystemColors.Window;
            };
            Control_TransferTab_ComboBox_ToLocation.Leave += (s, e) =>
            {
                Control_TransferTab_ComboBox_ToLocation.BackColor = SystemColors.Window;
                ComboBoxHelpers.ValidateComboBoxItem(Control_TransferTab_ComboBox_ToLocation, "[ Enter Location ]");
            };
            Control_TransferTab_DataGridView_Main.SelectionChanged +=
                (s, e) => Control_TransferTab_Update_ButtonStates();
            Control_TransferTab_Button_Delete.Click += async (s, e) =>
            {
                try
                {
                    if (Control_TransferTab_DataGridView_Main.SelectedRows.Count == 0)
                    {
                        MessageBox.Show(@"Please select a row to transfer from.", @"Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (Control_TransferTab_ComboBox_ToLocation.SelectedIndex < 0 ||
                        string.IsNullOrWhiteSpace(Control_TransferTab_ComboBox_ToLocation.Text))
                    {
                        MessageBox.Show(@"Please select a valid destination location.", @"Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var row = Control_TransferTab_DataGridView_Main.SelectedRows[0];
                    if (row.DataBoundItem is not DataRowView drv)
                    {
                        MessageBox.Show(@"Invalid row selection.", @"Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    var batchNumber = drv["Batch Number"]?.ToString() ?? "";
                    var partId = drv["PartID"]?.ToString() ?? "";
                    var operation = drv["Operation"]?.ToString() ?? "";
                    var quantity = drv["Quantity"]?.ToString() ?? "";
                    var newLocation = Control_TransferTab_ComboBox_ToLocation.Text;

                    await InventoryDao.TransferPartSimpleAsync(
                        batchNumber,
                        partId,
                        operation,
                        quantity,
                        newLocation);

                    MessageBox.Show($@"Transferred {quantity} of {partId} (Batch: {batchNumber}) to {newLocation}.",
                        @"Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Control_TransferTab_Button_Search_Click(null, null);
                }
                catch (Exception ex)
                {
                    AppLogger.LogApplicationError(ex);
                    await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                        new StringBuilder().Append("Control_TransferTab_Button_Delete_Click").ToString());
                }
            };
            AppLogger.Log("Transfer tab events wired up.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_WireUpTransferTabEvents").ToString());
        }
    }

    private void Control_TransferTab_Button_Toggle_RightPanel_Click(object sender, EventArgs e)
    {
        if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed)
        {
            MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = true;

            Control_TransferTab_Button_Toggle_RightPanel.Text = "←";
            Control_TransferTab_Button_Toggle_RightPanel.ForeColor = Color.Red;
        }
        else
        {
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = false;
                Control_TransferTab_Button_Toggle_RightPanel.Text = "→";
                Control_TransferTab_Button_Toggle_RightPanel.ForeColor = Color.Green;
            }
        }
    }

    public void UpdateToggleRightPanelButton()
    {
        if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed)
        {
            Control_TransferTab_Button_Toggle_RightPanel.Text = "→";
            Control_TransferTab_Button_Toggle_RightPanel.ForeColor = Color.Green;
        }
        else
        {
            Control_TransferTab_Button_Toggle_RightPanel.Text = "←";
            Control_TransferTab_Button_Toggle_RightPanel.ForeColor = Color.Red;
        }
    }

    private async void Control_TransferTab_Button_Search_Click(object? sender, EventArgs? e)
    {
        try
        {
            AppLogger.Log("TransferTab Search button clicked.");

            var partId = Control_TransferTab_ComboBox_Part.Text;
            var op = Control_TransferTab_ComboBox_Operation.Text;

            if (string.IsNullOrWhiteSpace(partId) || Control_TransferTab_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Control_TransferTab_ComboBox_Part.Focus();
                return;
            }

            DataTable results;

            if (!string.IsNullOrWhiteSpace(op) && Control_TransferTab_ComboBox_Operation.SelectedIndex > 0)
                // Filter by part and operation (assuming operation is stored as Location or similar)
                results = await InventoryDao.GetInventoryByPartIdAndOperationAsync(partId, op, true);
            else
                // Only filter by part
                results = await InventoryDao.GetInventoryByPartIdAsync(partId, true);

            Control_TransferTab_DataGridView_Main.DataSource = results;
            Control_TransferTab_DataGridView_Main.ClearSelection();

            // Show only the desired columns
            foreach (DataGridViewColumn column in Control_TransferTab_DataGridView_Main.Columns)
                column.Visible = column.Name == "PartID" ||
                                 column.Name == "Operation" ||
                                 column.Name == "Quantity" ||
                                 column.Name == "Location";

            if (Control_TransferTab_DataGridView_Main.Columns.Contains("Batch Number"))
                Control_TransferTab_DataGridView_Main.Columns["Batch Number"].Visible = false;

            // Apply theme and size columns
            DgvDesigner.ApplyThemeToDataGridView(
                Control_TransferTab_DataGridView_Main,
                AppThemes.GetTheme(WipAppVariables.WipDataGridTheme ?? "Default (Black and White)")
            );
            DgvDesigner.SizeDataGrid(Control_TransferTab_DataGridView_Main);

            if (results.Rows.Count == 0)
                Control_TransferTab_Image_NothingFound.Visible = true;
            else
                Control_TransferTab_Image_NothingFound.Visible = false;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                "Control_TransferTab_Button_Search_Click");
        }
    }
}