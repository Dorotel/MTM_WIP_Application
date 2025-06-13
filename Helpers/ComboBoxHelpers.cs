using System.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using MTM_WIP_Application.Data;

namespace MTM_WIP_Application.Helpers;

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
public static class ComboBoxHelpers
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

    // Use System.Data.MySqlDataAdapter for the adapter parameter
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

            // Assign the command if the adapter is a MySqlDataAdapter
            if (adapter is { } mySqlAdapter)
                mySqlAdapter.SelectCommand = command;
            // else: handle your custom wrapper here if you need

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

    private static Task InvokeAsyncTask(this Control control, Action action, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() => control.Invoke(action), cancellationToken, TaskCreationOptions.None,
            TaskScheduler.Default);
    }

    // --- ComboBox Utility Methods ---

    /// <summary>
    /// Validates that the ComboBox's text matches either the placeholder or a valid item in the ComboBox.
    /// Sets ForeColor accordingly. Returns true if valid, false otherwise.
    /// </summary>
    public static bool ValidateComboBoxItem(ComboBox comboBox, string placeholder)
    {
        if (string.IsNullOrWhiteSpace(comboBox.Text))
            return false;

        // Accept placeholder as valid
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

    /// <summary>
    /// Sets the ComboBox's text and color to the placeholder.
    /// </summary>
    public static void SetComboBoxPlaceholder(ComboBox comboBox, string placeholder)
    {
        comboBox.Text = placeholder;
        comboBox.ForeColor = Color.Red;
    }

    /// <summary>
    /// Sets the ComboBox's ForeColor to indicate a valid selection.
    /// </summary>
    public static void SetComboBoxValid(ComboBox comboBox)
    {
        comboBox.ForeColor = Color.Black;
    }

    /// <summary>
    /// Sets the ComboBox's ForeColor to indicate an invalid selection.
    /// </summary>
    public static void SetComboBoxInvalid(ComboBox comboBox)
    {
        comboBox.ForeColor = Color.Red;
    }

    /// <summary>
    /// Applies the standard ComboBox properties used in ControlTransferTab to the given ComboBox.
    /// </summary>
    /// <param name="comboBox">The ComboBox to standardize.</param>
    /// <param name="ownerDraw">Set true for OwnerDrawVariable (e.g., ToLocation), false otherwise.</param>
    public static void ApplyStandardComboBoxProperties(ComboBox comboBox, bool ownerDraw = false)
    {
        if (comboBox == null) return;
        comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
        comboBox.FormattingEnabled = true;
        comboBox.DropDownStyle = ComboBoxStyle.DropDown; // Default unless you want DropDownList
        comboBox.DrawMode = ownerDraw ? DrawMode.OwnerDrawVariable : DrawMode.Normal;
    }

    /// <summary>
    /// Deselects all text in all ComboBox controls within the specified parent control (recursively).
    /// </summary>
    /// <param name="parent">The parent control (UserControl or Form) to search for ComboBoxes.</param>
    public static void DeselectAllComboBoxText(Control parent)
    {
        if (parent == null) return;

        foreach (Control control in parent.Controls)
        {
            if (control is ComboBox comboBox)
                // Only applicable if the ComboBox is editable (DropDown or Simple)
                if (comboBox.DropDownStyle != ComboBoxStyle.DropDownList)
                {
                    if (comboBox.InvokeRequired)
                        comboBox.Invoke(new MethodInvoker(() => comboBox.SelectionLength = 0));
                    else
                        comboBox.SelectionLength = 0;
                }

            // Recursively process child controls
            if (control.HasChildren)
                DeselectAllComboBoxText(control);
        }
    }
}