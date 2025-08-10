using System.Data;
using System.Diagnostics;
using System.Reflection;
using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;
using MethodInvoker = System.Windows.Forms.MethodInvoker;

namespace MTM_Inventory_Application.Helpers
{
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
        private static readonly DataTable ComboBoxItemType_DataTable = new();
        private static readonly MySqlDataAdapter ComboBoxPart_DataAdapter = new();
        private static readonly MySqlDataAdapter ComboBoxOperation_DataAdapter = new();
        private static readonly MySqlDataAdapter ComboBoxLocation_DataAdapter = new();
        private static readonly MySqlDataAdapter ComboBoxUser_DataAdapter = new();
        private static readonly MySqlDataAdapter ComboBoxItemType_DataAdapter = new();

        #endregion

        #region Private Variables - Locks

        private static readonly object PartDataLock = new();
        private static readonly object OperationDataLock = new();
        private static readonly object LocationDataLock = new();
        private static readonly object UserDataLock = new();
        private static readonly object ItemTypeDataLock = new();

        #endregion

        #region DataTableSetup

        public static async Task SetupPartDataTable()
        {
            await using MySqlConnection connection = new(Model_AppVariables.ConnectionString);
            await connection.OpenAsync();

            MySqlCommand command = new("md_part_ids_Get_All", connection) { CommandType = CommandType.StoredProcedure };

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
            await using MySqlConnection connection = new(Model_AppVariables.ConnectionString);
            await connection.OpenAsync();

            MySqlCommand command = new("md_operation_numbers_Get_All", connection)
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
            await using MySqlConnection connection = new(Model_AppVariables.ConnectionString);
            await connection.OpenAsync();

            MySqlCommand command =
                new("md_locations_Get_All", connection) { CommandType = CommandType.StoredProcedure };

            lock (LocationDataLock)
            {
                ComboBoxLocation_DataAdapter.SelectCommand = command;
                ComboBoxLocation_DataTable.Clear();
                ComboBoxLocation_DataAdapter.Fill(ComboBoxLocation_DataTable);
            }

            await connection.CloseAsync();
        }

        public static async Task SetupUserDataTable()
        {
            await using MySqlConnection connection = new(Model_AppVariables.ConnectionString);
            await connection.OpenAsync();

            MySqlCommand command = new("usr_users_Get_All", connection) { CommandType = CommandType.StoredProcedure };

            lock (UserDataLock)
            {
                ComboBoxUser_DataAdapter.SelectCommand = command;
                ComboBoxUser_DataTable.Clear();
                ComboBoxUser_DataAdapter.Fill(ComboBoxUser_DataTable);
                // Remove any row where the User column contains '[ All Users ]' or similar
                List<DataRow> rowsToRemove = new();
                foreach (DataRow row in ComboBoxUser_DataTable.Rows)
                {
                    string userVal = row["User"]?.ToString() ?? string.Empty;
                    if (userVal.Contains("All Users"))
                    {
                        rowsToRemove.Add(row);
                    }
                }

                foreach (DataRow row in rowsToRemove)
                {
                    ComboBoxUser_DataTable.Rows.Remove(row);
                }
            }

            await connection.CloseAsync();
        }

        public static async Task SetupItemTypeDataTable()
        {
            await using MySqlConnection connection = new(Model_AppVariables.ConnectionString);
            await connection.OpenAsync();

            MySqlCommand command =
                new("md_item_types_Get_All", connection) { CommandType = CommandType.StoredProcedure };

            lock (ItemTypeDataLock)
            {
                ComboBoxItemType_DataAdapter.SelectCommand = command;
                ComboBoxItemType_DataTable.Clear();
                ComboBoxItemType_DataAdapter.Fill(ComboBoxItemType_DataTable);
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
                    "PartID",
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

        public static async Task FillItemTypeComboBoxesAsync(ComboBox comboBox)
        {
            try
            {
                await FillComboBoxAsync(
                    ComboBoxItemType_DataTable,
                    comboBox,
                    "ItemType",
                    "ID",
                    "[ Enter Item Type ]",
                    ItemTypeDataLock
                ).ConfigureAwait(false);
                comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while filling item type combo boxes.", ex);
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
                {
                    lock (dataLock)
                    {
                        SetComboBoxInternal();
                    }
                }
                else
                {
                    SetComboBoxInternal();
                }
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
                {
                    throw new InvalidOperationException(
                        $"DataTable does not contain required columns: '{displayMember}' or '{valueMember}'. " +
                        $"Actual columns: {string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");
                }

                // Create a copy of the DataTable to avoid shared reference issues
                DataTable comboDataTable = dataTable.Copy();

                // Check if placeholder already exists in the copy
                bool hasPlaceholder = comboDataTable.Rows.Count > 0 &&
                                      comboDataTable.Rows[0][displayMember]?.ToString() == placeholder;

                if (!hasPlaceholder)
                {
                    DataRow row = comboDataTable.NewRow();
                    row[displayMember] = placeholder;
                    if (comboDataTable.Columns[valueMember] != null &&
                        comboDataTable.Columns[valueMember]!.DataType == typeof(int))
                    {
                        row[valueMember] = -1;
                    }
                    else
                    {
                        row[valueMember] = placeholder;
                    }

                    comboDataTable.Rows.InsertAt(row, 0);
                }

                comboBox.DataSource = comboDataTable;
                comboBox.DisplayMember = displayMember;
                comboBox.ValueMember = valueMember;
                comboBox.SelectedIndex = 0;
            }

            if (comboBox.InvokeRequired)
            {
                comboBox.Invoke(SetComboBox);
            }
            else
            {
                SetComboBox();
            }

            return Task.CompletedTask;
        }

        #endregion

        #region DataTableResetAndRefresh

        public static async Task SetupDataTables()
        {
            await SetupPartDataTable();
            await SetupOperationDataTable();
            await SetupLocationDataTable();
            await SetupUserDataTable();
            await SetupItemTypeDataTable();
        }

        public static async Task ResetAndRefreshAllDataTablesAsync()
        {
            if (MainFormInstance != null)
            {
                await UnbindAllComboBoxDataSourcesAsync(MainFormInstance);
            }

            await SetupDataTables();

            await ReloadAllTabComboBoxesAsync();
        }

        public static Task UnbindAllComboBoxDataSourcesAsync(Control root) =>
            Task.Run(() =>
            {
                void Unbind(Control parent)
                {
                    foreach (Control control in parent.Controls)
                    {
                        if (control is ComboBox combo)
                        {
                            if (combo.InvokeRequired)
                            {
                                combo.Invoke(new Action(() => combo.DataSource = null));
                            }
                            else
                            {
                                combo.DataSource = null;
                            }
                        }

                        if (control.HasChildren)
                        {
                            Unbind(control);
                        }
                    }
                }

                Unbind(root);
            });

        #endregion

        #region ComboBoxValidation

        public static bool ValidateComboBoxItem(ComboBox comboBox, string placeholder)
        {
            if (comboBox == null)
            {
                return false;
            }

            if (comboBox.DataSource is not DataTable dt)
            {
                return false;
            }

            string text = comboBox.Text?.Trim() ?? string.Empty;
            string displayMember = comboBox.DisplayMember;

            if (string.IsNullOrWhiteSpace(displayMember) || !dt.Columns.Contains(displayMember))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                comboBox.Text = placeholder;
                if (comboBox.Items.Count > 0)
                {
                    comboBox.SelectedIndex = 0;
                }

                return false;
            }

            if (text.Equals(placeholder, StringComparison.OrdinalIgnoreCase))
            {
                comboBox.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                if (comboBox.Items.Count > 0)
                {
                    comboBox.SelectedIndex = 0;
                }

                return true;
            }

            bool found = false;
            foreach (DataRow row in dt.Rows)
            {
                string? value = row[displayMember]?.ToString();
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
                {
                    comboBox.SelectedIndex = 0;
                }

                return false;
            }
        }

        #endregion

        #region ComboBoxUIHelpers

        public static void ApplyStandardComboBoxProperties(ComboBox comboBox, bool ownerDraw = false)
        {
            if (comboBox == null)
            {
                return;
            }

            comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox.FormattingEnabled = true;
            comboBox.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox.DrawMode = ownerDraw ? DrawMode.OwnerDrawVariable : DrawMode.Normal;
        }

        public static void DeselectAllComboBoxText(Control parent) => ComboBoxHelpers.DeselectAllComboBoxText(parent);

        #endregion

        #region ComboBoxHelpers (Nested static class)

        private static class ComboBoxHelpers
        {
            public static void DeselectAllComboBoxText(Control parent)
            {
                if (parent == null)
                {
                    return;
                }

                foreach (Control control in parent.Controls)
                {
                    if (control is ComboBox comboBox)
                    {
                        if (comboBox.DropDownStyle != ComboBoxStyle.DropDownList)
                        {
                            if (comboBox.InvokeRequired)
                            {
                                comboBox.Invoke(new MethodInvoker(() => comboBox.SelectionLength = 0));
                            }
                            else
                            {
                                comboBox.SelectionLength = 0;
                            }
                        }
                    }

                    if (control.HasChildren)
                    {
                        DeselectAllComboBoxText(control);
                    }
                }
            }
        }

        #endregion

        #region TabComboBoxReload

        public static async Task ReloadAllTabComboBoxesAsync()
        {
            if (MainFormInstance!.MainForm_UserControl_RemoveTab != null)
            {
                await MainFormInstance.MainForm_UserControl_RemoveTab
                    .Control_RemoveTab_OnStartup_LoadDataComboBoxesAsync();
            }

            if (MainFormInstance!.MainForm_UserControl_TransferTab != null)
            {
                await MainFormInstance!.MainForm_UserControl_TransferTab
                    .Control_TransferTab_OnStartup_LoadDataComboBoxesAsync();
            }

            if (MainFormInstance!.MainForm_UserControl_InventoryTab != null)
            {
                await MainFormInstance!.MainForm_UserControl_InventoryTab
                    .Control_InventoryTab_OnStartup_LoadDataComboBoxesAsync();
            }

            if (MainFormInstance!.MainForm_UserControl_AdvancedRemove != null)
            {
                Control_AdvancedRemove? advRemove = MainFormInstance!.MainForm_UserControl_AdvancedRemove;
                MethodInfo? loadComboBoxesAsync = advRemove.GetType().GetMethod("LoadComboBoxesAsync",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (loadComboBoxesAsync != null)
                {
                    await ((Task)loadComboBoxesAsync.Invoke(advRemove, null)!)!;
                }
            }

            if (MainFormInstance!.MainForm_UserControl_AdvancedInventory != null)
            {
                Control_AdvancedInventory? advInv = MainFormInstance!.MainForm_UserControl_AdvancedInventory;
                MethodInfo? loadAllComboBoxesAsync = advInv.GetType().GetMethod("LoadAllComboBoxesAsync",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (loadAllComboBoxesAsync != null)
                {
                    await ((Task)loadAllComboBoxesAsync.Invoke(advInv, null)!)!;
                }
            }
        }

        #endregion
    }

    #endregion
}
