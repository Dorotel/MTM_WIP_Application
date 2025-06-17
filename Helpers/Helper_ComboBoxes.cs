using System.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.Helpers;

public static class Helper_ComboBoxes
{
    public static async Task ClearAndResetAllComboBoxesAsync(
        ComboBox inventoryTabComboBoxPart,
        ComboBox inventoryTabComboBoxOp,
        ComboBox inventoryTabComboBoxLoc,
        ComboBox removeTabComboBoxPart,
        ComboBox removeTabComboBoxOp,
        ComboBox removeTabCBoxShowAll,
        ComboBox transferTabComboBoxPart,
        ComboBox transferTabComboBoxLoc,
        DataTable partCbDataTable,
        DataTable opCbDataTable,
        DataTable locationCbDataTable,
        DataTable removeTabComboBoxSearchByTypeDataTable,
        Func<Task> fillAllComboBoxesAsync,
        Action? helperTabControlResetTab1,
        Action? helperTabControlResetTab2,
        Action? helperTabControlResetTab3,
        TabControl mainFormTabControl)
    {
        try
        {
            if (inventoryTabComboBoxPart.InvokeRequired)
            {
                await inventoryTabComboBoxPart.InvokeAsyncTask(
                    async () =>
                    {
                        await ClearAndResetAllComboBoxesAsync(
                            inventoryTabComboBoxPart,
                            inventoryTabComboBoxOp,
                            inventoryTabComboBoxLoc,
                            removeTabComboBoxPart,
                            removeTabComboBoxOp,
                            removeTabCBoxShowAll,
                            transferTabComboBoxPart,
                            transferTabComboBoxLoc,
                            partCbDataTable,
                            opCbDataTable,
                            locationCbDataTable,
                            removeTabComboBoxSearchByTypeDataTable,
                            fillAllComboBoxesAsync, helperTabControlResetTab1,
                            helperTabControlResetTab2, helperTabControlResetTab3,
                            mainFormTabControl).ConfigureAwait(false);
                    },
                    CancellationToken.None
                ).ConfigureAwait(false);
                return;
            }

            inventoryTabComboBoxPart.DataSource = null;
            removeTabComboBoxPart.DataSource = null;
            transferTabComboBoxPart.DataSource = null;
            partCbDataTable.Clear();
            partCbDataTable.Dispose();

            inventoryTabComboBoxOp.DataSource = null;
            removeTabComboBoxOp.DataSource = null;
            opCbDataTable.Clear();
            opCbDataTable.Dispose();

            inventoryTabComboBoxLoc.DataSource = null;
            transferTabComboBoxLoc.DataSource = null;
            locationCbDataTable.Clear();
            locationCbDataTable.Dispose();

            removeTabCBoxShowAll.DataSource = null;
            removeTabComboBoxSearchByTypeDataTable.Clear();
            removeTabComboBoxSearchByTypeDataTable.Dispose();

            await fillAllComboBoxesAsync().ConfigureAwait(false);

            helperTabControlResetTab1!();
            helperTabControlResetTab2!();
            helperTabControlResetTab3!();

            if (mainFormTabControl.SelectedIndex == 0) inventoryTabComboBoxPart.Focus();
            if (mainFormTabControl.SelectedIndex == 1) removeTabComboBoxPart.Focus();
            if (mainFormTabControl.SelectedIndex == 2) transferTabComboBoxPart.Focus();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, nameof(ClearAndResetAllComboBoxesAsync));
        }
    }

    public static async Task FillAllComboBoxesAsync(
        MySqlConnection connection,
        MySqlDataAdapter partCbDataAdapter,
        DataTable partCbDataTable,
        ComboBox inventoryTabComboBoxPart,
        ComboBox? removeTabComboBoxPart,
        ComboBox? transferTabComboBoxPart,
        MySqlDataAdapter opCbDataAdapter,
        DataTable opCbDataTable,
        ComboBox inventoryTabComboBoxOp,
        ComboBox? removeTabComboBoxOp,
        MySqlDataAdapter locationCbDataAdapter,
        DataTable locationCbDataTable,
        ComboBox inventoryTabComboBoxLoc,
        ComboBox? transferTabComboBoxLoc,
        MySqlDataAdapter? removeTabCBoxSearchByTypeDataAdapter,
        DataTable? removeTabComboBoxSearchByTypeDataTable,
        ComboBox? removeTabCBoxShowAll)
    {
        try
        {
            await FillComboBoxAsync("SELECT * FROM md_part_ids", connection, partCbDataAdapter,
                    partCbDataTable, inventoryTabComboBoxPart, "Item Number", "ID", "[ Enter Part ID ]")
                .ConfigureAwait(false);
            if (removeTabComboBoxPart != null)
            {
                removeTabComboBoxPart.DataSource = partCbDataTable;
                removeTabComboBoxPart.DisplayMember = "Item Number";
                removeTabComboBoxPart.ValueMember = "ID";
                removeTabComboBoxPart.SelectedIndex = 0;
            }
            if (transferTabComboBoxPart != null)
            {
                transferTabComboBoxPart.DataSource = partCbDataTable;
                transferTabComboBoxPart.DisplayMember = "Item Number";
                transferTabComboBoxPart.ValueMember = "ID";
                transferTabComboBoxPart.SelectedIndex = 0;
            }

            await FillComboBoxAsync("SELECT * FROM md_operation_numbers", connection, opCbDataAdapter,
                    opCbDataTable, inventoryTabComboBoxOp, "Operation", "Operation", "[ Enter Op # ]")
                .ConfigureAwait(false);
            if (removeTabComboBoxOp != null)
            {
                removeTabComboBoxOp.DataSource = opCbDataTable;
                removeTabComboBoxOp.DisplayMember = "Operation";
                removeTabComboBoxOp.ValueMember = "Operation";
                removeTabComboBoxOp.SelectedIndex = 0;
            }

            await FillComboBoxAsync("SELECT * FROM md_locations", connection, locationCbDataAdapter,
                    locationCbDataTable, inventoryTabComboBoxLoc, "Location", "Location", "[ Enter Location ]")
                .ConfigureAwait(false);
            if (transferTabComboBoxLoc != null)
            {
                transferTabComboBoxLoc.DataSource = locationCbDataTable;
                transferTabComboBoxLoc.DisplayMember = "Location";
                transferTabComboBoxLoc.ValueMember = "Location";
                transferTabComboBoxLoc.SelectedIndex = 0;
            }

            if (removeTabCBoxSearchByTypeDataAdapter != null && removeTabComboBoxSearchByTypeDataTable != null && removeTabCBoxShowAll != null)
            {
                await FillComboBoxAsync("SELECT * FROM md_item_types", connection, removeTabCBoxSearchByTypeDataAdapter,
                        removeTabComboBoxSearchByTypeDataTable, removeTabCBoxShowAll, "Type", "Type", "[ Select Type ]")
                    .ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, nameof(FillAllComboBoxesAsync));
        }
    }

    public static async Task FillComboBoxAsync(
        string procedureName,
        MySqlConnection connection,
        MySqlDataAdapter adapter,
        DataTable dataTable,
        ComboBox comboBox,
        string displayMember,
        string valueMember,
        string placeholder,
        CommandType commandType = CommandType.Text)
    {
        MySqlCommand? command = null;
        try
        {
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            command = new MySqlCommand(procedureName, connection)
            {
                CommandType = commandType
            };

            if (adapter is { } mySqlAdapter)
                mySqlAdapter.SelectCommand = command;

            dataTable.Clear();
            await Task.Run(() => adapter.Fill(dataTable)).ConfigureAwait(false);

            void SetComboBox()
            {
                var needsPlaceholder = dataTable.Rows.Count == 0 ||
                                       !Equals(dataTable.Rows[0][displayMember]?.ToString(), placeholder);

                if (needsPlaceholder)
                {
                    var row = dataTable.NewRow();
                    row[displayMember] = placeholder;
                    if (dataTable.Columns[valueMember] != null &&
                        dataTable.Columns[valueMember]!.DataType == typeof(int))
                        row[valueMember] = -1;
                    else
                        row[valueMember] = placeholder;
                    dataTable.Rows.InsertAt(row, 0);
                }

                comboBox.DataSource = dataTable;
                comboBox.DisplayMember = displayMember;
                comboBox.ValueMember = valueMember;
                comboBox.SelectedIndex = 0;
            }

            if (comboBox.InvokeRequired)
                comboBox.Invoke(SetComboBox);
            else
                SetComboBox();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, nameof(FillComboBoxAsync));
        }
        finally
        {
            command?.Dispose();
        }
    }

    public static bool ValidateComboBoxItem(ComboBox comboBox, string placeholder)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(comboBox.Text))
                return false;
            if (comboBox.Text.Equals(placeholder, StringComparison.OrdinalIgnoreCase))
            {
                comboBox.ForeColor = Color.Red;
                return true;
            }

            var found = false;
            var displayMember = comboBox.DisplayMember;
            foreach (var item in comboBox.Items)
            {
                string? itemText = null;
                if (!string.IsNullOrEmpty(displayMember) && item is DataRowView drv)
                    itemText = drv[displayMember]?.ToString();
                else
                    itemText = item?.ToString();
                if (!string.IsNullOrEmpty(itemText) &&
                    itemText.Equals(comboBox.Text, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
            }

            comboBox.ForeColor = found ? Color.Black : Color.Red;
            return found;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, nameof(ValidateComboBoxItem));
            return false;
        }
    }

    public static void SetComboBoxPlaceholder(ComboBox comboBox, string placeholder)
    {
        comboBox.Text = placeholder;
        comboBox.ForeColor = Color.Red;
    }

    public static void SetComboBoxValid(ComboBox comboBox)
    {
        comboBox.ForeColor = Color.Black;
    }

    public static void SetComboBoxInvalid(ComboBox comboBox)
    {
        comboBox.ForeColor = Color.Red;
    }

    public static void ApplyStandardComboBoxProperties(ComboBox comboBox, bool ownerDraw = false)
    {
        if (comboBox == null) return;
        comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
        comboBox.FormattingEnabled = true;
        comboBox.DropDownStyle = ComboBoxStyle.DropDown;
        comboBox.DrawMode = ownerDraw ? DrawMode.OwnerDrawVariable : DrawMode.Normal;
    }

    public static void DeselectAllComboBoxText(Control parent)
    {
        if (parent == null) return;
        foreach (Control control in parent.Controls)
        {
            if (control is ComboBox comboBox)
                if (comboBox.DropDownStyle != ComboBoxStyle.DropDownList)
                {
                    if (comboBox.InvokeRequired)
                        comboBox.Invoke(new MethodInvoker(() => comboBox.SelectionLength = 0));
                    else
                        comboBox.SelectionLength = 0;
                }

            if (control.HasChildren)
                DeselectAllComboBoxText(control);
        }
    }

    private static Task InvokeAsyncTask(this Control control, Action action, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() => control.Invoke(action), cancellationToken, TaskCreationOptions.None,
            TaskScheduler.Default);
    }
}