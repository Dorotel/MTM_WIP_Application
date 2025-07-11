using System.Data;
using System.Diagnostics;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;
using static MTM_Inventory_Application.Core.Core_Themes;

namespace MTM_Inventory_Application.Helpers;

#region Helper_UI_ComboBoxes

public static class Helper_UI_ComboBoxes
{
    #region Public Variables - MainForm Instance

    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    #endregion

    #region Private Variables - DataTables

    private static readonly DataTable ComboBoxPart_DataTable = new();
    private static readonly DataTable ComboBoxOperation_DataTable = new();
    private static readonly DataTable ComboBoxLocation_DataTable = new();
    private static readonly DataTable ComboBoxUser_DataTable = new();
    private static readonly DataTable ComboBox2ndLocation_DataTable = new();
    private static readonly MySqlDataAdapter ComboBoxPart_DataAdapter = new();
    private static readonly MySqlDataAdapter ComboBoxOperation_DataAdapter = new();
    private static readonly MySqlDataAdapter ComboBoxLocation_DataAdapter = new();
    private static readonly MySqlDataAdapter ComboBoxUser_DataAdapter = new();
    private static readonly MySqlDataAdapter Combobox2ndLocation_DataAdapter = new();

    #endregion

    #region Private Variables - Locks

    private static readonly object PartDataLock = new();
    private static readonly object OperationDataLock = new();
    private static readonly object LocationDataLock = new();
    private static readonly object UserDataLock = new();
    private static readonly object Combobox2ndLocationLock = new();

    #endregion

    #region DataTableSetup

    public static async Task SetupPartDataTable()
    {
        await using var connection = new MySqlConnection(Model_AppVariables.ConnectionString);
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
        }

        await connection.CloseAsync();
    }

    public static async Task SetupOperationDataTable()
    {
        await using var connection = new MySqlConnection(Model_AppVariables.ConnectionString);
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
        }

        await connection.CloseAsync();
    }

    public static async Task SetupLocationDataTable()
    {
        await using var connection = new MySqlConnection(Model_AppVariables.ConnectionString);
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
        }

        await connection.CloseAsync();
    }

    public static async Task Setup2ndLocationDataTable()
    {
        await using var connection = new MySqlConnection(Model_AppVariables.ConnectionString);
        await connection.OpenAsync();

        var command = new MySqlCommand("md_locations_Get_All", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        lock (Combobox2ndLocationLock)
        {
            Combobox2ndLocation_DataAdapter.SelectCommand = command;
            ComboBox2ndLocation_DataTable.Clear();
            Combobox2ndLocation_DataAdapter.Fill(ComboBoxLocation_DataTable);
        }

        await connection.CloseAsync();
    }

    public static async Task SetupUserDataTable()
    {
        await using var connection = new MySqlConnection(Model_AppVariables.ConnectionString);
        await connection.OpenAsync();

        var command = new MySqlCommand("usr_users_Get_All", connection) { CommandType = CommandType.StoredProcedure };

        lock (UserDataLock)
        {
            ComboBoxUser_DataAdapter.SelectCommand = command;
            ComboBoxUser_DataTable.Clear();
            ComboBoxUser_DataAdapter.Fill(ComboBoxUser_DataTable);
        }

        await connection.CloseAsync();
    }

    #endregion

    #region FillComboBoxes

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
            comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
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
            comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
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
            comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while filling location combo boxes.", ex);
        }
    }

    public static async Task Fill2ndLocationComboBoxesAsync(ComboBox comboBox)
    {
        try
        {
            await FillComboBoxAsync(
                ComboBox2ndLocation_DataTable,
                comboBox,
                "Location",
                "ID",
                "[ Enter Location ]",
                Combobox2ndLocationLock
            ).ConfigureAwait(false);
            comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
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
            comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
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
            if (dataTable.Columns.Count == 0)
            {
                Debug.WriteLine(
                    $"[WARNING] FillComboBoxAsync called with empty DataTable schema. Skipping ComboBox fill for '{displayMember}'/'{valueMember}'.");
                return;
            }

            if (!dataTable.Columns.Contains(displayMember) || !dataTable.Columns.Contains(valueMember))
                throw new InvalidOperationException(
                    $"DataTable does not contain required columns: '{displayMember}' or '{valueMember}'. " +
                    $"Actual columns: {string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");

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

    #region DataTableResetAndRefresh

    public static async Task ResetAndRefreshAllDataTablesAsync()
    {
        if (MainFormInstance != null) await UnbindAllComboBoxDataSourcesAsync(MainFormInstance);

        await SetupPartDataTable();
        await SetupOperationDataTable();
        await SetupLocationDataTable();
        await SetupUserDataTable();

        await ReloadAllTabComboBoxesAsync();
    }

    public static Task UnbindAllComboBoxDataSourcesAsync(Control root)
    {
        return Task.Run(() =>
        {
            void Unbind(Control parent)
            {
                foreach (Control control in parent.Controls)
                {
                    if (control is ComboBox combo)
                    {
                        // UI updates must be invoked on the UI thread
                        if (combo.InvokeRequired)
                            combo.Invoke(new Action(() => combo.DataSource = null));
                        else
                            combo.DataSource = null;
                    }

                    if (control.HasChildren) Unbind(control);
                }
            }

            Unbind(root);
        });
    }

    #endregion

    #region ComboBoxValidation

    public static bool ValidateComboBoxItem(ComboBox comboBox, string placeholder)
    {
        if (comboBox == null)
            return false;

        if (comboBox.DataSource is not DataTable dt)
            return false;

        var text = comboBox.Text?.Trim() ?? string.Empty;
        var displayMember = comboBox.DisplayMember;

        if (string.IsNullOrWhiteSpace(displayMember) || !dt.Columns.Contains(displayMember))
            return false;

        if (string.IsNullOrWhiteSpace(text))
        {
            comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            comboBox.Text = placeholder;
            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
            return false;
        }

        if (text.Equals(placeholder, StringComparison.OrdinalIgnoreCase))
        {
            comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
            return true;
        }

        var found = false;
        foreach (DataRow row in dt.Rows)
        {
            var value = row[displayMember]?.ToString();
            if (!string.IsNullOrEmpty(value) && value.Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                found = true;
                break;
            }
        }

        if (found)
        {
            comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
            return true;
        }
        else
        {
            comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            comboBox.Text = placeholder;
            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
            return false;
        }
    }

    #endregion

    #region ComboBoxUIHelpers

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
        ComboBoxHelpers.DeselectAllComboBoxText(parent);
    }

    #endregion

    #region ComboBoxHelpers (Nested static class)

    private static class ComboBoxHelpers
    {
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
    }

    #endregion

    #region TabComboBoxReload

    public static async Task ReloadAllTabComboBoxesAsync()
    {
        if (MainFormInstance!.MainForm_RemoveTabNormalControl != null)
            await MainFormInstance.MainForm_RemoveTabNormalControl
                .Control_RemoveTab_OnStartup_LoadDataComboBoxesAsync();

        if (MainFormInstance!.MainForm_Control_TransferTab != null)
            await MainFormInstance!.MainForm_Control_TransferTab
                .Control_TransferTab_OnStartup_LoadDataComboBoxesAsync();

        if (MainFormInstance!.MainForm_Control_InventoryTab != null)
            await MainFormInstance!.MainForm_Control_InventoryTab
                .Control_InventoryTab_OnStartup_LoadDataComboBoxesAsync();

        if (MainFormInstance!.MainForm_Control_AdvancedRemove != null)
        {
            var advRemove = MainFormInstance!.MainForm_Control_AdvancedRemove;
            var loadComboBoxesAsync = advRemove.GetType().GetMethod("LoadComboBoxesAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (loadComboBoxesAsync != null)
                await ((Task)loadComboBoxesAsync.Invoke(advRemove, null)!)!;
        }

        if (MainFormInstance!.MainForm_AdvancedInventory != null)
        {
            var advInv = MainFormInstance!.MainForm_AdvancedInventory;
            var loadAllComboBoxesAsync = advInv.GetType().GetMethod("LoadAllComboBoxesAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (loadAllComboBoxesAsync != null)
                await ((Task)loadAllComboBoxesAsync.Invoke(advInv, null)!)!;
        }
    }

    #endregion
}

#endregion