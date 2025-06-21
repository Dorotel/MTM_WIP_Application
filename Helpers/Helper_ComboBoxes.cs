using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;

namespace MTM_WIP_Application.Helpers;

#region Helper_ComboBoxes

public static class Helper_ComboBoxes
{
    #region ComboBox Data Fill

    private static readonly DataTable ComboBoxPart_DataTable = new();
    private static readonly DataTable ComboBoxOperation_DataTable = new();
    private static readonly DataTable ComboBoxLocation_DataTable = new();
    private static readonly DataTable ComboBoxUser_DataTable = new();
    private static readonly MySqlDataAdapter ComboBoxPart_DataAdapter = new();
    private static readonly MySqlDataAdapter ComboBoxOperation_DataAdapter = new();
    private static readonly MySqlDataAdapter ComboBoxLocation_DataAdapter = new();
    private static readonly MySqlDataAdapter ComboBoxUser_DataAdapter = new();

    private static readonly object PartDataLock = new();
    private static readonly object OperationDataLock = new();
    private static readonly object LocationDataLock = new();
    private static readonly object UserDataLock = new();

    public static async Task SetupPartDataTable()
    {
        await using var connection = new MySqlConnection(Core_WipAppVariables.ConnectionString);
        await connection.OpenAsync();

        var command = new MySqlCommand("md_part_ids_Get_All", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        lock (PartDataLock)
        {
            ComboBoxPart_DataAdapter.SelectCommand = command;
            ComboBoxPart_DataTable.Clear();
            ComboBoxPart_DataAdapter.Fill(ComboBoxPart_DataTable);
            // Debug: Log columns
            System.Diagnostics.Debug.WriteLine(
                $"[DEBUG] Part DataTable Columns: {string.Join(", ", ComboBoxPart_DataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");
        }
    }

    public static async Task SetupOperationDataTable()
    {
        await using var connection = new MySqlConnection(Core_WipAppVariables.ConnectionString);
        await connection.OpenAsync();

        var command = new MySqlCommand("md_operation_numbers_Get_All", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        lock (OperationDataLock)
        {
            ComboBoxOperation_DataAdapter.SelectCommand = command;
            ComboBoxOperation_DataTable.Clear();
            ComboBoxOperation_DataAdapter.Fill(ComboBoxOperation_DataTable);
            // Debug: Log columns
            System.Diagnostics.Debug.WriteLine(
                $"[DEBUG] Operation DataTable Columns: {string.Join(", ", ComboBoxOperation_DataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");
        }
    }

    public static async Task SetupLocationDataTable()
    {
        await using var connection = new MySqlConnection(Core_WipAppVariables.ConnectionString);
        await connection.OpenAsync();

        var command = new MySqlCommand("md_locations_Get_All", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        lock (LocationDataLock)
        {
            ComboBoxLocation_DataAdapter.SelectCommand = command;
            ComboBoxLocation_DataTable.Clear();
            ComboBoxLocation_DataAdapter.Fill(ComboBoxLocation_DataTable);
            // Debug: Log columns
            System.Diagnostics.Debug.WriteLine(
                $"[DEBUG] Location DataTable Columns: {string.Join(", ", ComboBoxLocation_DataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");
        }
    }

    public static async Task SetupUserDataTable()
    {
        await using var connection = new MySqlConnection(Core_WipAppVariables.ConnectionString);
        await connection.OpenAsync();

        var command = new MySqlCommand("usr_users_Get_All", connection) { CommandType = CommandType.StoredProcedure };

        lock (UserDataLock)
        {
            ComboBoxUser_DataAdapter.SelectCommand = command;
            ComboBoxUser_DataTable.Clear();
            ComboBoxUser_DataAdapter.Fill(ComboBoxUser_DataTable);
            // Debug: Log columns
            System.Diagnostics.Debug.WriteLine(
                $"[DEBUG] User DataTable Columns: {string.Join(", ", ComboBoxUser_DataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");
        }
    }

    public static async Task FillPartComboBoxesAsync(ComboBox comboBox)
    {
        try
        {
            await FillComboBoxAsync(
                ComboBoxPart_DataTable,
                comboBox,
                "Item Number",
                "ID",
                "[ Enter Part Number ]",
                PartDataLock
            ).ConfigureAwait(false);
            System.Diagnostics.Debug.WriteLine($"[DEBUG] FillPartComboBoxesAsync: ComboBox Name: {comboBox.Name}, Owner: {comboBox.FindForm()?.Name}");
            comboBox.ForeColor = Color.Red;
            await Task.Delay(100);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while filling part combo boxes.", ex);
        }
    }

    public static async Task FillOperationComboBoxesAsync(ComboBox comboBox)
    {
        try
        {
            await FillComboBoxAsync(
                ComboBoxOperation_DataTable,
                comboBox,
                "Operation",
                "ID",
                "[ Enter Operation ]",
                OperationDataLock
            ).ConfigureAwait(false);
            System.Diagnostics.Debug.WriteLine($"[DEBUG] FillOperationComboBoxesAsync: ComboBox Name: {comboBox.Name}, Owner: {comboBox.FindForm()?.Name}");
            comboBox.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while filling operation combo boxes.", ex);
        }
    }

    public static async Task FillLocationComboBoxesAsync(ComboBox comboBox)
    {
        try
        {
            await FillComboBoxAsync(
                ComboBoxLocation_DataTable,
                comboBox,
                "Location",
                "ID",
                "[ Enter Location ]",
                LocationDataLock
            ).ConfigureAwait(false);
            System.Diagnostics.Debug.WriteLine($"[DEBUG] FillLocationComboBoxesAsync: ComboBox Name: {comboBox.Name}, Owner: {comboBox.FindForm()?.Name}");
            comboBox.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while filling location combo boxes.", ex);
        }
    }

    public static async Task FillUserComboBoxesAsync(ComboBox comboBox)
    {
        try
        {
            await FillComboBoxAsync(ComboBoxUser_DataTable, comboBox, "User", "ID", "[ Enter User ]", UserDataLock)
                .ConfigureAwait(false);
            System.Diagnostics.Debug.WriteLine($"[DEBUG] FillUserComboBoxesAsync: ComboBox Name: {comboBox.Name}, Owner: {comboBox.FindForm()?.Name}");
            comboBox.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while filling user combo boxes.", ex);
        }
    }

    public static Task FillComboBoxAsync(
        DataTable dataTable,
        ComboBox comboBox,
        string displayMember,
        string valueMember,
        string placeholder,
        object? dataLock = null)
    {
        void SetComboBox()
        {
            if (dataLock != null)
                lock (dataLock)
                {
                    SetComboBoxInternal();
                }
            else
                SetComboBoxInternal();
        }

        void SetComboBoxInternal()
        {
            // Guard: skip if DataTable has no columns
            if (dataTable.Columns.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[WARNING] FillComboBoxAsync called with empty DataTable schema. Skipping ComboBox fill for '{displayMember}'/'{valueMember}'.");
                return;
            }

            // Debug: Check columns
            System.Diagnostics.Debug.WriteLine(
                $"[DEBUG] FillComboBoxAsync DataTable Columns: {string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");
            if (!dataTable.Columns.Contains(displayMember) || !dataTable.Columns.Contains(valueMember))
                throw new InvalidOperationException(
                    $"DataTable does not contain required columns: '{displayMember}' or '{valueMember}'. " +
                    $"Actual columns: {string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");

            // Only insert placeholder if not present
            var hasPlaceholder = dataTable.Rows.Count > 0 &&
                                 dataTable.Rows[0][displayMember]?.ToString() == placeholder;

            if (!hasPlaceholder)
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

        return Task.CompletedTask;
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
}

#endregion