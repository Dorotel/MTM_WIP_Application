using System.ComponentModel;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using System.Drawing.Printing;

namespace MTM_Inventory_Application.Forms.Transactions
{
    public partial class Transactions : Form
    {
        #region Fields

        private BindingList<Model_Transactions> _displayedTransactions = null!;
        private int _currentPage = 1;
        private const int _pageSize = 20;
        private const bool _sortDescending = true;
        private readonly string _currentUser;
        private readonly bool isAdmin;
        private ComboBox Transactions_ComboBox_SearchPartID;
        private Dao_Transactions _dao;

        #endregion

        #region Constructors

        public Transactions(string connectionString, string currentUser)
        {
            InitializeComponent();

            _dao = new Dao_Transactions(connectionString);
            _currentUser = currentUser;
            isAdmin = Model_AppVariables.UserTypeAdmin;

            SetupSortCombo();
            SetupDataGrid();

            Load += async (s, e) => await OnFormLoadAsync();

            Transactions_Button_Reset.Click += (s, e) => ResetFilters();

            // Paging logic
            Transfer_Button_Next.Click += async (s, e) =>
            {
                _currentPage++;
                await LoadTransactionsAsync();
            };
            Transfer_Button_Previous.Click += async (s, e) =>
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    await LoadTransactionsAsync();
                }
            };

            // Print button logic
            Transactions_Button_Print.Click += Transactions_Button_Print_Click;
            Core_Themes.ApplyTheme(this);
        }

        #endregion

        #region Methods

        private async Task OnFormLoadAsync()
        {
            Transactions_Button_Print.Enabled = false; // Disable print button on load
            Transfer_Button_SelectionHistory.Enabled = false; // Disable selection history button on load
            await LoadUserCombosAsync();
            await LoadBuildingComboAsync();
            await LoadPartComboAsync();
            SetupDateRangeDefaults();
            WireUpEvents();
        }

        private async Task LoadUserCombosAsync()
        {
            await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(Transactions_ComboBox_UserFullName);
            Transactions_ComboBox_UserFullName.SelectedIndex = 0;
            if (!isAdmin)
            {
                Transactions_ComboBox_UserFullName.Text = Model_AppVariables.User;

                Transactions_ComboBox_UserFullName.Enabled = false;
            }
            else
            {
                Transactions_ComboBox_UserFullName.Enabled = true;
            }
        }

        private async Task LoadBuildingComboAsync()
        {
            // Set Building ComboBox to only 3 items: [ Enter Building ], Expo Drive, Vits Drive
            Transactions_ComboBox_Building.Items.Clear();
            Transactions_ComboBox_Building.Items.Add("[ Enter Building ]");
            Transactions_ComboBox_Building.Items.Add("Expo Drive");
            Transactions_ComboBox_Building.Items.Add("Vits Drive");
            Transactions_ComboBox_Building.SelectedIndex = 0;
        }

        private async Task LoadPartComboAsync()
        {
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Transactions_ComboBox_SearchPartID);
            Transactions_ComboBox_SearchPartID.SelectedIndex = 0;
        }

        private void SetupDateRangeDefaults()
        {
            Control_AdvancedRemove_CheckBox_Date.Checked = false;
            Control_AdvancedRemove_DateTimePicker_From.Value = DateTime.Today.AddDays(-7);
            Control_AdvancedRemove_DateTimePicker_To.Value = DateTime.Today;
            Control_AdvancedRemove_DateTimePicker_From.Enabled = false;
            Control_AdvancedRemove_DateTimePicker_To.Enabled = false;
        }

        private void WireUpEvents()
        {
            Transactions_Button_Search.Click += async (s, e) => await LoadTransactionsAsync();
            Transactions_DataGridView_Transactions.SelectionChanged +=
                Transactions_DataGridView_Transactions_SelectionChanged;
            Control_AdvancedRemove_CheckBox_Date.CheckedChanged += (s, e) =>
            {
                Control_AdvancedRemove_DateTimePicker_From.Enabled = Control_AdvancedRemove_CheckBox_Date.Checked;
                Control_AdvancedRemove_DateTimePicker_To.Enabled = Control_AdvancedRemove_CheckBox_Date.Checked;
            };
            Transactions_Button_SidePanel.Click += Transactions_Button_SidePanel_Click;
            Transfer_Button_SelectionHistory.Click += Transfer_Button_BranchHistory_Click;

            // Enable/disable search button based on combo selection
            Transactions_ComboBox_SearchPartID.SelectedIndexChanged += Transactions_EnableSearchButtonIfValid;
            Transactions_ComboBox_UserFullName.SelectedIndexChanged += Transactions_EnableSearchButtonIfValid;
            Transactions_ComboBox_Building.SelectedIndexChanged += Transactions_EnableSearchButtonIfValid;
            Transactions_EnableSearchButtonIfValid(null, null);
        }

        private void Transactions_EnableSearchButtonIfValid(object? sender, EventArgs? e)
        {
            bool enable = Transactions_ComboBox_SearchPartID.SelectedIndex > 0
                          || Transactions_ComboBox_UserFullName.SelectedIndex > 0
                          || Transactions_ComboBox_Building.SelectedIndex > 0;
            Transactions_Button_Search.Enabled = enable;
        }

        private void Transactions_Button_SidePanel_Click(object? sender, EventArgs? e)
        {
            // Collapse/Expand the left panel (filters/inputs)
            if (Transactions_SplitContainer_Main.Panel1Collapsed)
            {
                Transactions_SplitContainer_Main.Panel1Collapsed = false;
                Transactions_Button_SidePanel.Text = "Collapse ◀";
            }
            else
            {
                Transactions_SplitContainer_Main.Panel1Collapsed = true;
                Transactions_Button_SidePanel.Text = "Expand ▶";
            }
        }

        private void SetupSortCombo()
        {
            Transactions_ComboBox_SortBy.Items.Clear();
            Transactions_ComboBox_SortBy.Items.Add("Date");
            Transactions_ComboBox_SortBy.Items.Add("Quantity");
            Transactions_ComboBox_SortBy.Items.Add("User");
            Transactions_ComboBox_SortBy.Items.Add("ItemType");
            Transactions_ComboBox_SortBy.SelectedIndex = 0;
        }

        private void SetupDataGrid()
        {
            Transactions_DataGridView_Transactions.AutoGenerateColumns = false;
            Transactions_DataGridView_Transactions.Columns.Clear();

            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "PartID",
                DataPropertyName = "PartID",
                Name = "colPartID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity",
                Name = "colQuantity",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Operation",
                DataPropertyName = "Operation",
                Name = "colOperation",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "User",
                DataPropertyName = "User",
                Name = "colUser",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "BatchNumber",
                DataPropertyName = "BatchNumber",
                Name = "colBatchNumber",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ReceiveDate",
                DataPropertyName = "ReceiveDate",
                Name = "colReceiveDate",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            var descCol = new DataGridViewTextBoxColumn
            {
                HeaderText = "Description",
                DataPropertyName = "Description",
                Name = "colDescription",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True }
            };
            Transactions_DataGridView_Transactions.Columns.Add(descCol);
            Transactions_DataGridView_Transactions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            Transactions_DataGridView_Transactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Transactions_DataGridView_Transactions.ReadOnly = true;
            Transactions_DataGridView_Transactions.AllowUserToAddRows = false;
            Transactions_DataGridView_Transactions.AllowUserToDeleteRows = false;
            Transactions_DataGridView_Transactions.AllowUserToOrderColumns = false;
            Transactions_DataGridView_Transactions.AllowUserToResizeRows = false;

            Transactions_DataGridView_Transactions.DataSource = new BindingList<Model_Transactions>();
        }

        private void ResetFilters()
        {
            Transactions_ComboBox_SortBy.SelectedIndex = 0;
            Transactions_ComboBox_SearchPartID.SelectedIndex = 0;
            Transactions_ComboBox_UserFullName.SelectedIndex = 0;
            Transactions_ComboBox_Building.SelectedIndex = 0;
            Control_AdvancedRemove_CheckBox_Date.Checked = false;
            SetupDateRangeDefaults();
            _currentPage = 1;
        }

        private void UpdatePagingButtons(int resultCount)
        {
            Transfer_Button_Previous.Enabled = _currentPage > 1;
            Transfer_Button_Next.Enabled = resultCount >= _pageSize;
        }

        private async Task LoadTransactionsAsync()
        {
            System.Diagnostics.Debug.WriteLine("[DEBUG] LoadTransactionsAsync started");
            DataTable dt = new();
            MySqlCommand cmd;

            bool hasLikeSearch = false;
            string likeSearchText = null;
            string likeSearchColumn = null;
            int likeComboIndex = -1;
            if (Controls.ContainsKey("Transactions_ComboBox_Like") && Controls.ContainsKey("Transactions_TextBox_Like"))
            {
                ComboBox? likeCombo = Controls["Transactions_ComboBox_Like"] as ComboBox;
                TextBox? likeText = Controls["Transactions_TextBox_Like"] as TextBox;
                if (likeCombo != null && likeText != null && likeCombo.SelectedIndex > 0 &&
                    !string.IsNullOrWhiteSpace(likeText.Text))
                {
                    hasLikeSearch = true;
                    likeSearchText = likeText.Text.Trim();
                    likeComboIndex = likeCombo.SelectedIndex;
                    switch (likeCombo.Text)
                    {
                        case "Part ID":
                            likeSearchColumn = "PartID";
                            break;
                        case "Location":
                            likeSearchColumn = "FromLocation";
                            break;
                        case "User":
                            likeSearchColumn = "User";
                            break;
                    }
                }
            }

            if (hasLikeSearch && !string.IsNullOrEmpty(likeSearchColumn))
            {
                string query = $"SELECT * FROM inv_transaction WHERE {likeSearchColumn} LIKE @SearchPattern";
                cmd = new MySqlCommand(query, null);
                cmd.Parameters.AddWithValue("@SearchPattern", $"%{likeSearchText}%");
                System.Diagnostics.Debug.WriteLine($"[DEBUG] LIKE Search: {query} with pattern '%{likeSearchText}%'");
            }
            else
            {
                string part = Transactions_ComboBox_SearchPartID.Text;
                string user = Transactions_ComboBox_UserFullName.Text;
                string building = Transactions_ComboBox_Building.Text;
                string notes = Transactions_TextBox_Notes.Text;
                bool filterByDate = Control_AdvancedRemove_CheckBox_Date.Checked;
                DateTime? dateFrom =
                    filterByDate ? Control_AdvancedRemove_DateTimePicker_From.Value.Date : (DateTime?)null;
                DateTime? dateTo = filterByDate
                    ? Control_AdvancedRemove_DateTimePicker_To.Value.Date.AddDays(1).AddTicks(-1)
                    : (DateTime?)null;

                bool partSelected = Transactions_ComboBox_SearchPartID.SelectedIndex > 0 &&
                                    !string.IsNullOrWhiteSpace(part);
                bool userSelected = Transactions_ComboBox_UserFullName.SelectedIndex > 0 &&
                                    !string.IsNullOrWhiteSpace(user);
                bool buildingSelected = Transactions_ComboBox_Building.SelectedIndex > 0 &&
                                        !string.IsNullOrWhiteSpace(building);
                bool anyFieldFilled = partSelected || userSelected || buildingSelected ||
                                      !string.IsNullOrWhiteSpace(notes) ||
                                      (filterByDate && dateFrom != null && dateTo != null);

                if (!anyFieldFilled)
                {
                    MessageBox.Show("Please fill in at least one field to search.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (filterByDate && dateFrom > dateTo)
                {
                    MessageBox.Show("The 'From' date cannot be after the 'To' date.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                StringBuilder queryBuilder = new();
                queryBuilder.Append("SELECT * FROM inv_transaction WHERE 1=1 ");
                List<MySqlParameter> parameters = new();

                if (partSelected)
                {
                    queryBuilder.Append("AND PartID = @PartID ");
                    parameters.Add(new MySqlParameter("@PartID", part));
                }

                if (userSelected)
                {
                    queryBuilder.Append("AND User = @User ");
                    parameters.Add(new MySqlParameter("@User", user));
                }

                if (buildingSelected)
                {
                    queryBuilder.Append("AND FromLocation = @FromLocation ");
                    parameters.Add(new MySqlParameter("@FromLocation", building));
                }

                if (!string.IsNullOrWhiteSpace(notes))
                {
                    queryBuilder.Append("AND Notes LIKE @Notes ");
                    parameters.Add(new MySqlParameter("@Notes", $"%{notes}%"));
                }

                if (filterByDate && dateFrom.HasValue && dateTo.HasValue)
                {
                    queryBuilder.Append("AND ReceiveDate BETWEEN @DateFrom AND @DateTo ");
                    parameters.Add(new MySqlParameter("@DateFrom", dateFrom));
                    parameters.Add(new MySqlParameter("@DateTo", dateTo));
                }

                string sortBy = Transactions_ComboBox_SortBy.Text ?? "Date";
                string orderBy = sortBy switch
                {
                    "Quantity" => "Quantity",
                    "User" => "User",
                    "ItemType" => "TransactionType",
                    _ => "ReceiveDate"
                };
                queryBuilder.Append($"ORDER BY {orderBy} {(_sortDescending ? "DESC" : "ASC")} ");
                queryBuilder.Append("LIMIT @Offset, @PageSize ");
                parameters.Add(new MySqlParameter("@Offset", (_currentPage - 1) * _pageSize));
                parameters.Add(new MySqlParameter("@PageSize", _pageSize));

                cmd = new MySqlCommand(queryBuilder.ToString(), null);
                foreach (MySqlParameter param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                System.Diagnostics.Debug.WriteLine($"[DEBUG] SQL Query: {queryBuilder}");
                foreach (MySqlParameter p in parameters)
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Param: {p.ParameterName} = {p.Value}");
                }
            }

            string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
            using (MySqlConnection conn = new(connectionString))
            {
                cmd.Connection = conn;
                await conn.OpenAsync();
                using (MySqlDataAdapter adapter = new(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            System.Diagnostics.Debug.WriteLine($"[DEBUG] Rows returned: {dt.Rows.Count}");

            List<Model_Transactions> result = new();
            foreach (DataRow row in dt.Rows)
            {
                Model_Transactions tx = new()
                {
                    TransactionType =
                        Enum.TryParse(row["TransactionType"].ToString(), out TransactionType ttype)
                            ? ttype
                            : TransactionType.IN,
                    BatchNumber = row["BatchNumber"]?.ToString(),
                    PartID = row["PartID"]?.ToString(),
                    FromLocation = row["FromLocation"]?.ToString(),
                    ToLocation = row["ToLocation"]?.ToString(),
                    Operation = row["Operation"]?.ToString(),
                    Quantity = int.TryParse(row["Quantity"]?.ToString(), out int qty) ? qty : 0,
                    Notes = row["Notes"]?.ToString(),
                    User = row["User"]?.ToString(),
                    ItemType = row["ItemType"]?.ToString(),
                    DateTime = DateTime.TryParse(row["ReceiveDate"]?.ToString(), out DateTime dtm)
                        ? dtm
                        : DateTime.MinValue
                };
                result.Add(tx);
            }

            System.Diagnostics.Debug.WriteLine($"[DEBUG] Transactions mapped: {result.Count}");

            _displayedTransactions = new BindingList<Model_Transactions>(result);
            Transactions_DataGridView_Transactions.AutoGenerateColumns = false;
            Transactions_DataGridView_Transactions.Columns.Clear();
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "PartID",
                DataPropertyName = "PartID",
                Name = "colPartID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Operation",
                DataPropertyName = "Operation",
                Name = "colOperation",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity",
                Name = "colQuantity",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "FromLocation",
                DataPropertyName = "FromLocation",
                Name = "colFromLocation",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ToLocation",
                DataPropertyName = "ToLocation",
                Name = "colToLocation",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ReceiveDate",
                DataPropertyName = "DateTime",
                Name = "colReceiveDate",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.DataSource = _displayedTransactions;
            Transactions_Image_NothingFound.Visible = result.Count == 0;
            Transactions_DataGridView_Transactions.Visible = result.Count > 0;
            Transactions_Button_Print.Enabled = result.Count > 0;
            Transfer_Button_SelectionHistory.Enabled = result.Count > 0;
            if (_displayedTransactions.Count > 0)
            {
                Transactions_DataGridView_Transactions.ClearSelection();
            }
            UpdatePagingButtons(result.Count);
            System.Diagnostics.Debug.WriteLine("[DEBUG] LoadTransactionsAsync finished");
        }

        private void Transactions_DataGridView_Transactions_SelectionChanged(object? sender, EventArgs? e)
        {
            if (Transactions_DataGridView_Transactions.SelectedRows.Count == 1)
            {
                DataGridViewRow row = Transactions_DataGridView_Transactions.SelectedRows[0];
                if (row.DataBoundItem is Model_Transactions tx)
                {
                    Transactions_TextBox_Report_BatchNumber.Text = tx.BatchNumber ?? "";
                    Transactions_TextBox_Report_PartID.Text = tx.PartID ?? "";
                    Transactions_TextBox_Report_FromLocation.Text = tx.FromLocation ?? "";
                    Transactions_TextBox_Report_ToLocation.Text = tx.ToLocation ?? "";
                    Transactions_TextBox_Report_Operation.Text = tx.Operation ?? "";
                    Transactions_TextBox_Report_Quantity.Text = tx.Quantity.ToString();
                    Transactions_TextBox_Notes.Text = tx.Notes ?? "";
                    Transactions_TextBox_Report_User.Text = tx.User ?? "";
                    Transactions_TextBox_Report_ItemType.Text = tx.ItemType ?? "";
                    Transactions_TextBox_Report_ReceiveDate.Text = tx.DateTime.ToString("g");
                }
            }
            else
            {
                Transactions_TextBox_Report_BatchNumber.Text = "";
                Transactions_TextBox_Report_PartID.Text = "";
                Transactions_TextBox_Report_FromLocation.Text = "";
                Transactions_TextBox_Report_ToLocation.Text = "";
                Transactions_TextBox_Report_Operation.Text = "";
                Transactions_TextBox_Report_Quantity.Text = "";
                Transactions_TextBox_Notes.Text = "";
                Transactions_TextBox_Report_User.Text = "";
                Transactions_TextBox_Report_ItemType.Text = "";
                Transactions_TextBox_Report_ReceiveDate.Text = "";
            }
        }

        private void Transactions_Button_Print_Click(object? sender, EventArgs? e)
        {
            try
            {
                Core_DgvPrinter printer = new();
                // Optionally set column widths/alignments here if needed
                printer.Print(Transactions_DataGridView_Transactions);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing transactions: {ex.Message}", "Print Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void Transfer_Button_BranchHistory_Click(object sender, EventArgs e)
        {
            if (Transactions_DataGridView_Transactions.SelectedRows.Count != 1)
                return;
            var selected = Transactions_DataGridView_Transactions.SelectedRows[0].DataBoundItem as Model_Transactions;
            if (selected == null || string.IsNullOrWhiteSpace(selected.BatchNumber))
            {
                MessageBox.Show("No Batch Number found for the selected transaction.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var progress = new Controls.Shared.Control_ProgressBarUserControl();
            Controls.Add(progress);
            progress.BringToFront();
            progress.ShowProgress();
            progress.UpdateProgress(10, "Loading batch history...");
            await Task.Delay(100);

            var batchNumber = selected.BatchNumber;
            var results = await Task.Run(() => _dao.SearchTransactions(
                userName: isAdmin ? string.Empty : _currentUser,
                isAdmin: isAdmin,
                batchNumber: batchNumber,
                sortColumn: "ReceiveDate",
                sortDescending: true,
                page: 1,
                pageSize: 1000
            ));
            progress.UpdateProgress(80, "Mapping results...");
            await Task.Delay(100);

            // Build description for each row
            var describedResults = new List<dynamic>();
            for (int i = 0; i < results.Count; i++)
            {
                var curr = results[i];
                string desc = "";
                if (i == results.Count - 1) // last row (oldest)
                {
                    desc = "Initial Transaction";
                }
                else
                {
                    var prev = results[i + 1];
                    if (curr.TransactionType == TransactionType.OUT)
                        desc = "Removed From System";
                    else if (curr.TransactionType == TransactionType.TRANSFER && prev.ToLocation != curr.FromLocation)
                        desc = $"Part transferred from {prev.ToLocation ?? "Unknown"} to {curr.ToLocation ?? "Unknown"}";
                    else if (curr.TransactionType == TransactionType.IN)
                        desc = "Received Into System";
                    else
                        desc = "Transaction";
                }
                describedResults.Add(new
                {
                    curr.PartID,
                    curr.Quantity,
                    curr.Operation,
                    curr.User,
                    curr.BatchNumber,
                    curr.FromLocation,
                    curr.ToLocation,
                    ReceiveDate = curr.DateTime,
                    Description = desc
                });
            }

            Transactions_DataGridView_Transactions.AutoGenerateColumns = false;
            Transactions_DataGridView_Transactions.Columns.Clear();
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "PartID",
                DataPropertyName = "PartID",
                Name = "colPartID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity",
                Name = "colQuantity",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Operation",
                DataPropertyName = "Operation",
                Name = "colOperation",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "User",
                DataPropertyName = "User",
                Name = "colUser",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "BatchNumber",
                DataPropertyName = "BatchNumber",
                Name = "colBatchNumber",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "FromLocation",
                DataPropertyName = "FromLocation",
                Name = "colFromLocation",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ToLocation",
                DataPropertyName = "ToLocation",
                Name = "colToLocation",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ReceiveDate",
                DataPropertyName = "ReceiveDate",
                Name = "colReceiveDate",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            var descCol = new DataGridViewTextBoxColumn
            {
                HeaderText = "Description",
                DataPropertyName = "Description",
                Name = "colDescription",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True }
            };
            Transactions_DataGridView_Transactions.Columns.Add(descCol);
            Transactions_DataGridView_Transactions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            Transactions_DataGridView_Transactions.DataSource = new BindingList<dynamic>(describedResults);
            Transactions_Image_NothingFound.Visible = describedResults.Count == 0;
            Transactions_DataGridView_Transactions.Visible = describedResults.Count > 0;
            Transactions_Button_Print.Enabled = describedResults.Count > 0;
            Transfer_Button_SelectionHistory.Enabled = describedResults.Count > 0;
            if (describedResults.Count > 0)
            {
                Transactions_DataGridView_Transactions.ClearSelection();
            }
            progress.UpdateProgress(100, "Complete");
            await Task.Delay(200);
            progress.HideProgress();
        }
        #endregion
    }
}
