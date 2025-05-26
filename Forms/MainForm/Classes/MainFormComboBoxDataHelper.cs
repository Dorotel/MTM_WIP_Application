using System.Data;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Forms.MainForm.Classes;

/// <summary>
///     Provides helper methods for loading and resetting ComboBox data from the database.
/// </summary>
public static class MainFormComboBoxDataHelper
{
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
        Action helperTabControlResetTab1,
        Action helperTabControlResetTab2,
        Action helperTabControlResetTab3,
        TabControl mainFormTabControl)
    {
        if (inventoryTabComboBoxPart.InvokeRequired)
        {
            await inventoryTabComboBoxPart.InvokeAsync(async () =>
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
            }).ConfigureAwait(false);
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

        helperTabControlResetTab1();
        helperTabControlResetTab2();
        helperTabControlResetTab3();

        if (mainFormTabControl.SelectedIndex == 0) inventoryTabComboBoxPart.Focus();
        if (mainFormTabControl.SelectedIndex == 1) removeTabComboBoxPart.Focus();
        if (mainFormTabControl.SelectedIndex == 2) transferTabComboBoxPart.Focus();
    }

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
        await FillComboBoxAsync("SELECT * FROM part_ids", connection, inventoryTabPartCbDataAdapter,
                inventoryTabPartCbDataTable, inventoryTabComboBoxPart, "Item Number", "ID", "[ Enter Part ID ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM `operation_numbers`", connection, inventoryTabOpCbDataAdapter,
                inventoryTabOpCbDataTable, inventoryTabComboBoxOp, "Operation", "Operation", "[ Enter Op # ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM `locations`", connection, inventoryTabLocationCbDataAdapter,
                inventoryTabLocationCbDataTable, inventoryTabComboBoxLoc, "Location", "Location", "[ Enter Location ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM part_ids", connection, removeTabPartCbDataAdapter,
                removeTabPartCbDataTable, removeTabComboBoxPart, "Item Number", "Item Number", "[ Enter Part ID ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM `operation_numbers`", connection, removeTabOpCbDataAdapter,
                removeTabComboBoxOpDataTable, removeTabComboBoxOp, "Operation", "Operation", "[ Enter Op # ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM `item_types`", connection, removeTabCBoxSearchByTypeDataAdapter,
                removeTabComboBoxSearchByTypeDataTable, removeTabCBoxShowAll, "Type", "Type", "[ Select Type ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM `locations`", connection, transferTabLocationCbDataAdapter,
                transferTabLocationCbDataTable, transferTabComboBoxLoc, "Location", "Location",
                "[ Enter New Location ]")
            .ConfigureAwait(false);

        await FillComboBoxAsync("SELECT * FROM part_ids", connection, transferTabPartCbDataAdapter,
                transferTabPartCbDataTable, transferTabComboBoxPart, "Item Number", "Item Number", "[ Enter Part ID ]")
            .ConfigureAwait(false);
    }

    public static async Task FillComboBoxAsync(
        string query,
        MySqlConnection connection,
        MySqlDataAdapter dataAdapter,
        DataTable dataTable,
        ComboBox comboBox,
        string displayMember,
        string valueMember,
        string defaultText)
    {
        MySqlCommand? command = null;
        try
        {
            command = new MySqlCommand(query, connection);
            dataAdapter.SelectCommand = command;
            await Task.Run(() => dataAdapter.Fill(dataTable)).ConfigureAwait(false);
            var itemRow = dataTable.NewRow();
            itemRow[0] = defaultText;
            dataTable.Rows.InsertAt(itemRow, 0);

            if (comboBox.InvokeRequired)
            {
                comboBox.Invoke(() =>
                {
                    comboBox.DataSource = dataTable;
                    comboBox.DisplayMember = displayMember;
                    comboBox.ValueMember = valueMember;
                });
            }
            else
            {
                comboBox.DataSource = dataTable;
                comboBox.DisplayMember = displayMember;
                comboBox.ValueMember = valueMember;
            }
        }
        finally
        {
            command?.Dispose();
        }
    }

    private static Task InvokeAsync(this Control control, Action action)
    {
        return Task.Factory.StartNew(() => control.Invoke(action), CancellationToken.None, TaskCreationOptions.None,
            TaskScheduler.Default);
    }
}