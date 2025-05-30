﻿using System.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using MTM_WIP_Application.Data;
using IDataAdapter = MTM_WIP_Application.Data.IDataAdapter;

namespace MTM_WIP_Application.Forms.MainForm.Classes;

/// <summary>
/// 
/// Testing Passed: 05/31/2025
/// 
/// Contains unit tests for the DgvDesigner class, which provides utilities for applying visual themes to Windows Forms and DataGridView controls.
/// 
/// Features Tested:
/// - Recursive application of theme colors, fonts, and styles to forms and all child controls.
/// - Control-specific theming for buttons, tab controls, text boxes, and labels.
/// - Customization of DataGridView appearance, including background, headers, selection, and column sizing.
/// - Helper methods for theme information, color retrieval, and testing support.
/// - Integration with WipAppVariables for theme selection.
/// 
/// Usage:
/// 1. Verifies ApplyTheme applies the current global theme to forms and controls.
/// 2. Tests SizeDataGrid for optimizing DataGridView column sizing.
/// 3. Validates static helpers for querying theme names, colors, and theme application in test scenarios.
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
        Action? helperTabControlResetTab1,
        Action? helperTabControlResetTab2,
        Action? helperTabControlResetTab3,
        TabControl mainFormTabControl)
    {
        if (inventoryTabComboBoxPart.InvokeRequired)
        {
            await inventoryTabComboBoxPart.InvokeAsync(
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

    public static async Task FillAllComboBoxesAsync(
        MySqlConnection connection,
        IDataAdapter inventoryTabPartCbDataAdapter,
        DataTable inventoryTabPartCbDataTable,
        ComboBox inventoryTabComboBoxPart,
        IDataAdapter inventoryTabOpCbDataAdapter,
        DataTable inventoryTabOpCbDataTable,
        ComboBox inventoryTabComboBoxOp,
        IDataAdapter inventoryTabLocationCbDataAdapter,
        DataTable inventoryTabLocationCbDataTable,
        ComboBox inventoryTabComboBoxLoc,
        IDataAdapter removeTabPartCbDataAdapter,
        DataTable removeTabPartCbDataTable,
        ComboBox removeTabComboBoxPart,
        IDataAdapter removeTabOpCbDataAdapter,
        DataTable removeTabComboBoxOpDataTable,
        ComboBox removeTabComboBoxOp,
        IDataAdapter removeTabCBoxSearchByTypeDataAdapter,
        DataTable removeTabComboBoxSearchByTypeDataTable,
        ComboBox removeTabCBoxShowAll,
        IDataAdapter transferTabLocationCbDataAdapter,
        DataTable transferTabLocationCbDataTable,
        ComboBox transferTabComboBoxLoc,
        IDataAdapter transferTabPartCbDataAdapter,
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
        IDataAdapter dataAdapter,
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
            if (dataAdapter is MySqlDataAdapterWrapper wrapper)
                wrapper.SelectCommand = command;
            await Task.Run(() => dataAdapter.Fill(dataTable)).ConfigureAwait(false);

            void SetComboBox()
            {
                if (dataTable.Rows.Count == 0 || !Equals(dataTable.Rows[0][0], defaultText))
                {
                    var row = dataTable.NewRow();
                    row[0] = defaultText;
                    dataTable.Rows.InsertAt(row, 0);
                }

                comboBox.DataSource = dataTable;
                comboBox.DisplayMember = displayMember;
                comboBox.ValueMember = valueMember;
            }

            if (comboBox.InvokeRequired)
                comboBox.Invoke((Action)SetComboBox);
            else
                SetComboBox();
        }
        finally
        {
            command?.Dispose();
        }
    }

    private static Task InvokeAsync(this Control control, Action action, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() => control.Invoke(action), cancellationToken, TaskCreationOptions.None,
            TaskScheduler.Default);
    }
}