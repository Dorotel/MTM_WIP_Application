using System.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using MTM_WIP_Application.Data;

namespace MTM_WIP_Application.Helpers;

#region Helper_ComboBoxes

public static class Helper_ComboBoxes
{
    #region ComboBox Reset & Clear

    public static async Task ClearAndResetAllComboBoxesAsync(
        ComboBox inventoryTabComboBoxPart,
        DataTable inventoryTabPartCbDataTable,
        ComboBox inventoryTabComboBoxOp,
        DataTable inventoryTabOpCbDataTable,
        ComboBox inventoryTabComboBoxLoc,
        DataTable inventoryTabLocationCbDataTable,
        ComboBox removeTabComboBoxPart,
        DataTable removeTabPartCbDataTable,
        ComboBox removeTabComboBoxOp,
        DataTable removeTabComboBoxOpDataTable,
        ComboBox removeTabCBoxShowAll,
        DataTable removeTabComboBoxSearchByTypeDataTable,
        ComboBox transferTabComboBoxPart,
        DataTable transferTabPartCbDataTable,
        ComboBox transferTabComboBoxLoc,
        DataTable transferTabLocationCbDataTable,
        Func<Task> fillAllComboBoxesAsync,
        Action? helperTabControlResetTab1,
        Action? helperTabControlResetTab2,
        Action? helperTabControlResetTab3,
        TabControl mainFormTabControl)
    {
        if (inventoryTabComboBoxPart.InvokeRequired)
        {
            await inventoryTabComboBoxPart.InvokeAsyncTask(
                async () =>
                {
                    await ClearAndResetAllComboBoxesAsync(
                        inventoryTabComboBoxPart, inventoryTabPartCbDataTable,
                        inventoryTabComboBoxOp, inventoryTabOpCbDataTable,
                        inventoryTabComboBoxLoc, inventoryTabLocationCbDataTable,
                        removeTabComboBoxPart, removeTabPartCbDataTable,
                        removeTabComboBoxOp, removeTabComboBoxOpDataTable,
                        removeTabCBoxShowAll, removeTabComboBoxSearchByTypeDataTable,
                        transferTabComboBoxPart, transferTabPartCbDataTable,
                        transferTabComboBoxLoc, transferTabLocationCbDataTable,
                        fillAllComboBoxesAsync, helperTabControlResetTab1,
                        helperTabControlResetTab2, helperTabControlResetTab3,
                        mainFormTabControl).ConfigureAwait(false);
                },
                CancellationToken.None
            ).ConfigureAwait(false);
            return;
        }

        inventoryTabComboBoxPart.DataSource = null;
        inventoryTabPartCbDataTable.Clear();
        inventoryTabPartCbDataTable.Dispose();

        inventoryTabComboBoxOp.DataSource = null;
        inventoryTabOpCbDataTable.Clear();
        inventoryTabOpCbDataTable.Dispose();

        inventoryTabComboBoxLoc.DataSource = null;
        inventoryTabLocationCbDataTable.Clear();
        inventoryTabLocationCbDataTable.Dispose();

        removeTabComboBoxPart.DataSource = null;
        removeTabPartCbDataTable.Clear();
        removeTabPartCbDataTable.Dispose();

        removeTabComboBoxOp.DataSource = null;
        removeTabComboBoxOpDataTable.Clear();
        removeTabComboBoxOpDataTable.Dispose();

        removeTabCBoxShowAll.DataSource = null;
        removeTabComboBoxSearchByTypeDataTable.Clear();
        removeTabComboBoxSearchByTypeDataTable.Dispose();

        transferTabComboBoxPart.DataSource = null;
        transferTabPartCbDataTable.Clear();
        transferTabPartCbDataTable.Dispose();

        transferTabComboBoxLoc.DataSource = null;
        transferTabLocationCbDataTable.Clear();
        transferTabLocationCbDataTable.Dispose();

        await fillAllComboBoxesAsync().ConfigureAwait(false);

        helperTabControlResetTab1!();
        helperTabControlResetTab2!();
        helperTabControlResetTab3!();

        if (mainFormTabControl.SelectedIndex == 0) inventoryTabComboBoxPart.Focus();
        if (mainFormTabControl.SelectedIndex == 1) removeTabComboBoxPart.Focus();
        if (mainFormTabControl.SelectedIndex == 2) transferTabComboBoxPart.Focus();
    }

    #endregion

    #region ComboBox Data Fill

    public static async Task FillAllComboBoxesAsync(
        MySqlConnection connection,
        MySqlDataAdapter inventoryTabPartCbDataAdapter,
        DataTable inventoryTabPartCbDataTable,
        ComboBox inventoryTabComboBoxPart,
        MySqlDataAdapter inventoryTabOpCbDataAdapter,
        DataTable inventoryTabOpCbDataTable,
        ComboBox inventoryTabComboBoxOp,
        MySqlDataAdapter inventoryTabLocationCbDataAdapter,
        DataTable inventoryTabLocationCbDataTable,
        ComboBox inventoryTabComboBoxLoc,
        MySqlDataAdapter removeTabPartCbDataAdapter,
        DataTable removeTabPartCbDataTable,
        ComboBox removeTabComboBoxPart,
        MySqlDataAdapter removeTabOpCbDataAdapter,
        DataTable removeTabComboBoxOpDataTable,
        ComboBox removeTabComboBoxOp,
        MySqlDataAdapter removeTabCBoxSearchByTypeDataAdapter,
        DataTable removeTabComboBoxSearchByTypeDataTable,
        ComboBox removeTabCBoxShowAll,
        MySqlDataAdapter transferTabLocationCbDataAdapter,
        DataTable transferTabLocationCbDataTable,
        ComboBox transferTabComboBoxLoc,
        MySqlDataAdapter transferTabPartCbDataAdapter,
        DataTable transferTabPartCbDataTable,
        ComboBox transferTabComboBoxPart)
    {
        await FillComboBoxAsync("SELECT * FROM md_part_ids", connection, inventoryTabPartCbDataAdapter,
                inventoryTabPartCbDataTable, inventoryTabComboBoxPart, "Item Number", "ID", "[ Enter Part ID ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM md_operation_numbers", connection, inventoryTabOpCbDataAdapter,
                inventoryTabOpCbDataTable, inventoryTabComboBoxOp, "Operation", "Operation", "[ Enter Op # ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM md_locations", connection, inventoryTabLocationCbDataAdapter,
                inventoryTabLocationCbDataTable, inventoryTabComboBoxLoc, "Location", "Location", "[ Enter Location ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM md_part_ids", connection, removeTabPartCbDataAdapter,
                removeTabPartCbDataTable, removeTabComboBoxPart, "Item Number", "Item Number", "[ Enter Part ID ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM md_operation_numbers", connection, removeTabOpCbDataAdapter,
                removeTabComboBoxOpDataTable, removeTabComboBoxOp, "Operation", "Operation", "[ Enter Op # ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM md_item_types", connection, removeTabCBoxSearchByTypeDataAdapter,
                removeTabComboBoxSearchByTypeDataTable, removeTabCBoxShowAll, "Type", "Type", "[ Select Type ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM md_locations", connection, transferTabLocationCbDataAdapter,
                transferTabLocationCbDataTable, transferTabComboBoxLoc, "Location", "Location",
                "[ Enter New Location ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM md_part_ids", connection, transferTabPartCbDataAdapter,
                transferTabPartCbDataTable, transferTabComboBoxPart, "Item Number", "Item Number", "[ Enter Part ID ]")
            .ConfigureAwait(false);
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
        finally
        {
            command?.Dispose();
        }
    }

    #endregion

    #region ComboBox Validation & State

    public static bool ValidateComboBoxItem(ComboBox comboBox, string placeholder)
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

    #endregion

    #region ComboBox UI Helpers

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

    #endregion

    #region Internal Helpers

    private static Task InvokeAsyncTask(this Control control, Action action, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() => control.Invoke(action), cancellationToken, TaskCreationOptions.None,
            TaskScheduler.Default);
    }

    #endregion
}

#endregion