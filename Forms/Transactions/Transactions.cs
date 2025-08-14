using System.ComponentModel;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using System.Drawing.Printing;
using MTM_Inventory_Application.Controls.Shared;
using MTM_Inventory_Application.Services;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Forms.Transactions
{
    public partial class Transactions : Form
    {
        #region Fields

        private BindingList<Model_Transactions> _displayedTransactions = null!;
        private int _currentPage = 1;
        private const int PageSize = 20;
        private const bool SortDescending = true;
        private readonly string _currentUser;
        private readonly bool _isAdmin;
        private ComboBox _transactionsComboBoxSearchPartId = new();

        // Enhanced smart search functionality
        private readonly Dictionary<string, string> _lastSearchCriteria = new();
        private bool _isPaginationNavigation = false;

        #endregion

        #region Constructors

        public Transactions(string connectionString, string currentUser)
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["FormType"] = nameof(Transactions),
                ["ConnectionString"] = connectionString,
                ["CurrentUser"] = currentUser,
                ["InitializationTime"] = DateTime.Now,
                ["Thread"] = Thread.CurrentThread.ManagedThreadId
            }, nameof(Transactions), nameof(Transactions));

            Service_DebugTracer.TraceUIAction("TRANSACTIONS_FORM_INITIALIZATION", nameof(Transactions),
                new Dictionary<string, object>
                {
                    ["Phase"] = "START",
                    ["ComponentType"] = "TransactionsForm"
                });

            InitializeComponent();

            Service_DebugTracer.TraceUIAction("THEME_APPLICATION", nameof(Transactions),
                new Dictionary<string, object>
                {
                    ["DpiScaling"] = "APPLIED",
                    ["LayoutAdjustments"] = "APPLIED",
                    ["AutoScaleMode"] = "Dpi"
                });
            // Apply comprehensive DPI scaling and runtime layout adjustments
            AutoScaleMode = AutoScaleMode.Dpi;
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

            Service_DebugTracer.TraceBusinessLogic("USER_CONTEXT_SETUP",
                inputData: new { connectionString, currentUser },
                outputData: new { 
                    _currentUser = currentUser,
                    _isAdmin = Model_AppVariables.UserTypeAdmin 
                });
            _currentUser = currentUser;
            _isAdmin = Model_AppVariables.UserTypeAdmin;

            Service_DebugTracer.TraceUIAction("CONTROLS_SETUP", nameof(Transactions),
                new Dictionary<string, object>
                {
                    ["Components"] = new[] { "SortCombo", "DataGrid", "SmartSearchControls" }
                });
            SetupSortCombo();
            SetupDataGrid();
            InitializeSmartSearchControls();

            Service_DebugTracer.TraceUIAction("EVENT_HANDLERS_SETUP", nameof(Transactions),
                new Dictionary<string, object>
                {
                    ["Events"] = new[] { "FormLoad", "ResetButton" }
                });
            Load += async (s, e) => await OnFormLoadAsync();

            Transactions_Button_Reset.Click += (s, e) => ResetFilters();

            Service_DebugTracer.TraceUIAction("PAGING_EVENTS_SETUP", nameof(Transactions),
                new Dictionary<string, object>
                {
                    ["PagingButtons"] = new[] { "Next", "Previous" },
                    ["PageSize"] = PageSize
                });
            // Enhanced paging logic with smart search support
            Transactions_Button_NextPage.Click += async (s, e) =>
            {
                Service_DebugTracer.TraceUIAction("NEXT_PAGE_CLICKED", nameof(Transactions),
                    new Dictionary<string, object>
                    {
                        ["CurrentPage"] = _currentPage,
                        ["NextPage"] = _currentPage + 1,
                        ["HasSearchCriteria"] = _lastSearchCriteria.Count > 0
                    });
                _currentPage++;
                _isPaginationNavigation = true;
                
                // Continue with smart search if criteria exist
                if (_lastSearchCriteria.Count > 0)
                {
                    await ExecuteSmartSearchAsync(_lastSearchCriteria);
                }
                else
                {
                    await LoadTransactionsAsync();
                }
            };
            Transactions_Button_PreviousPage.Click += async (s, e) =>
            {
                Service_DebugTracer.TraceUIAction("PREVIOUS_PAGE_CLICKED", nameof(Transactions),
                    new Dictionary<string, object>
                    {
                        ["CurrentPage"] = _currentPage,
                        ["CanGoBack"] = _currentPage > 1,
                        ["HasSearchCriteria"] = _lastSearchCriteria.Count > 0
                    });
                if (_currentPage > 1)
                {
                    _currentPage--;
                    _isPaginationNavigation = true;
                    
                    // Continue with smart search if criteria exist
                    if (_lastSearchCriteria.Count > 0)
                    {
                        await ExecuteSmartSearchAsync(_lastSearchCriteria);
                    }
                    else
                    {
                        await LoadTransactionsAsync();
                    }
                }
            };

            Service_DebugTracer.TraceUIAction("PRINT_EVENT_SETUP", nameof(Transactions),
                new Dictionary<string, object> { ["PrintButtonEnabled"] = false });
            // Print button logic
            Transactions_Button_Print.Click += Transactions_Button_Print_Click;

            Service_DebugTracer.TraceUIAction("THEME_APPLICATION_FINAL", nameof(Transactions));
            Core_Themes.ApplyTheme(this);

            Service_DebugTracer.TraceUIAction("TRANSACTIONS_FORM_INITIALIZATION", nameof(Transactions),
                new Dictionary<string, object>
                {
                    ["Phase"] = "COMPLETE",
                    ["Success"] = true,
                    ["CurrentUser"] = _currentUser,
                    ["IsAdmin"] = _isAdmin
                });

            Service_DebugTracer.TraceMethodExit(null, nameof(Transactions), nameof(Transactions));
        }

        #endregion

        #region Methods

        private async Task OnFormLoadAsync()
        {
            Transactions_Button_Print.Enabled = false; // Disable print button on load
            Transactions_Button_Help.Enabled = false; // Disable selection history button on load
            await LoadUserCombosAsync();
            LoadBuildingCombo(); // Remove await since method is no longer async
            await LoadPartComboAsync();
            SetupDateRangeDefaults();
            WireUpEvents();
        }

        private async Task LoadUserCombosAsync()
        {
            await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(Transactions_ComboBox_UserFullName);
            Transactions_ComboBox_UserFullName.SelectedIndex = 0;
            if (!_isAdmin)
            {
                Transactions_ComboBox_UserFullName.Text = Model_AppVariables.User;

                Transactions_ComboBox_UserFullName.Enabled = false;
            }
            else
            {
                Transactions_ComboBox_UserFullName.Enabled = true;
            }
        }

        private void LoadBuildingCombo()
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
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(_transactionsComboBoxSearchPartId);
            _transactionsComboBoxSearchPartId.SelectedIndex = 0;
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
            // Standard search functionality
            Transactions_Button_SmartSearch.Click += async (s, e) => await LoadTransactionsAsync();
            
            // Enhanced smart search functionality
            if (Controls.ContainsKey("Transactions_Button_SmartSearch"))
            {
                Controls["Transactions_Button_SmartSearch"].Click += async (s, e) => await HandleSmartSearchAsync();
            }

            // Smart search with Enter key support
            if (Controls.ContainsKey("Transactions_TextBox_SmartSearch"))
            {
                var smartSearchTextBox = Controls["Transactions_TextBox_SmartSearch"] as TextBox;
                if (smartSearchTextBox != null)
                {
                    smartSearchTextBox.KeyDown += async (s, e) =>
                    {
                        if (e.KeyCode == Keys.Enter)
                        {
                            await HandleSmartSearchAsync();
                            e.Handled = true;
                        }
                    };
                }
            }

            Transactions_DataGridView_Transactions.SelectionChanged +=
                Transactions_DataGridView_Transactions_SelectionChanged;
            Control_AdvancedRemove_CheckBox_Date.CheckedChanged += (s, e) =>
            {
                Control_AdvancedRemove_DateTimePicker_From.Enabled = Control_AdvancedRemove_CheckBox_Date.Checked;
                Control_AdvancedRemove_DateTimePicker_To.Enabled = Control_AdvancedRemove_CheckBox_Date.Checked;
            };
            Transactions_Button_SidePanel.Click += Transactions_Button_SidePanel_Click;
            Transactions_Button_Help.Click += Transactions_Button_Help_Click;

            // Enable/disable search button based on combo selection
            _transactionsComboBoxSearchPartId.SelectedIndexChanged += Transactions_EnableSearchButtonIfValid;
            Transactions_ComboBox_UserFullName.SelectedIndexChanged += Transactions_EnableSearchButtonIfValid;
            Transactions_ComboBox_Building.SelectedIndexChanged += Transactions_EnableSearchButtonIfValid;
            Transactions_EnableSearchButtonIfValid(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handle smart search functionality with advanced syntax
        /// </summary>
        private async Task HandleSmartSearchAsync()
        {
            Service_DebugTracer.TraceUIAction("SMART_SEARCH_CLICKED", nameof(Transactions),
                new Dictionary<string, object>
                {
                    ["HasSmartSearchBox"] = Controls.ContainsKey("Transactions_TextBox_SmartSearch"),
                    ["CurrentPage"] = _currentPage
                });

            try
            {
                string searchText = "";
                if (Controls.ContainsKey("Transactions_TextBox_SmartSearch"))
                {
                    searchText = (Controls["Transactions_TextBox_SmartSearch"] as TextBox)?.Text?.Trim() ?? "";
                }

                Service_DebugTracer.TraceUIAction("SMART_SEARCH_INPUT_PARSED", nameof(Transactions),
                    new Dictionary<string, object>
                    {
                        ["SearchText"] = searchText,
                        ["SearchLength"] = searchText.Length
                    });

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    // If no smart search text, fall back to regular search
                    await LoadTransactionsAsync();
                    return;
                }

                // Reset to first page for new search
                if (!_isPaginationNavigation)
                {
                    _currentPage = 1;
                }
                _isPaginationNavigation = false;

                // Parse smart search syntax
                var searchCriteria = ParseSmartSearchText(searchText);
                
                Service_DebugTracer.TraceBusinessLogic("SMART_SEARCH_CRITERIA_PARSED", nameof(Transactions),
                    new Dictionary<string, object>
                    {
                        ["CriteriaCount"] = searchCriteria.Count,
                        ["HasGeneralSearch"] = searchCriteria.ContainsKey("_general")
                    });

                await ExecuteSmartSearchAsync(searchCriteria);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, "Transactions_HandleSmartSearchAsync");
                UpdateSmartSearchStatus($"Search error: {ex.Message}");
            }
        }

        private void Transactions_EnableSearchButtonIfValid(object? sender, EventArgs e)
        {
            bool enable = _transactionsComboBoxSearchPartId.SelectedIndex > 0
                          || Transactions_ComboBox_UserFullName.SelectedIndex > 0
                          || Transactions_ComboBox_Building.SelectedIndex > 0;
            Transactions_Button_SmartSearch.Enabled = enable;
        }

        private void Transactions_Button_SidePanel_Click(object? sender, EventArgs e)
        {
            // Collapse/Expand the left panel (filters/inputs)
            if (Transactions_SplitContainer_Main.Panel1Collapsed)
            {
                Transactions_SplitContainer_Main.Panel1Collapsed = false;
                Transactions_Button_SidePanel.Text = @"Collapse ⬅️";
            }
            else
            {
                Transactions_SplitContainer_Main.Panel1Collapsed = true;
                Transactions_Button_SidePanel.Text = @"Expand ➡️";
            }
        }

        private void Transactions_DataGridView_Transactions_SelectionChanged(object? sender, EventArgs e)
        {
            Service_DebugTracer.TraceUIAction("SELECTION_CHANGED", nameof(Transactions),
                new Dictionary<string, object>
                {
                    ["SelectedRowCount"] = Transactions_DataGridView_Transactions.SelectedRows.Count,
                    ["TotalRowCount"] = Transactions_DataGridView_Transactions.Rows.Count
                });

            if (Transactions_DataGridView_Transactions.SelectedRows.Count == 1)
            {
                DataGridViewRow row = Transactions_DataGridView_Transactions.SelectedRows[0];
                if (row.DataBoundItem is Model_Transactions tx)
                {
                    Service_DebugTracer.TraceBusinessLogic("TRANSACTION_SELECTION_REPORT_UPDATE", nameof(Transactions),
                        new Dictionary<string, object>
                        {
                            ["TransactionID"] = tx.ID,
                            ["BatchNumber"] = tx.BatchNumber ?? "",
                            ["PartID"] = tx.PartID ?? "",
                            ["TransactionType"] = tx.TransactionType ?? "",
                            ["User"] = tx.User ?? ""
                        });

                    // Enhanced selection report with all transaction details
                    Transactions_TextBox_Report_TransactionType.Text = tx.TransactionType ?? "";
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

                    // Enable history and print buttons when a transaction is selected
                    Transactions_Button_Help.Enabled = true;
                    Transactions_Button_Print.Enabled = true;

                    Service_DebugTracer.TraceUIAction("TRANSACTION_DETAILS_POPULATED", nameof(Transactions),
                        new Dictionary<string, object>
                        {
                            ["HistoryButtonEnabled"] = true,
                            ["PrintButtonEnabled"] = true
                        });
                }
            }
            else
            {
                // Clear selection report when no single row is selected
                ClearSelectionReportControls();
                
                // Disable action buttons
                Transactions_Button_Help.Enabled = false;
                Transactions_Button_Print.Enabled = false;

                Service_DebugTracer.TraceUIAction("SELECTION_REPORT_CLEARED", nameof(Transactions),
                    new Dictionary<string, object>
                    {
                        ["HistoryButtonEnabled"] = false,
                        ["PrintButtonEnabled"] = false
                    });
            }
        }
                Transactions_TextBox_Notes.Text = "";
                Transactions_TextBox_Report_User.Text = "";
                Transactions_TextBox_Report_ItemType.Text = "";
                Transactions_TextBox_Report_ReceiveDate.Text = "";
            }
        }

        private void Transactions_Button_Print_Click(object? sender, EventArgs e)
        {
            try
            {
                Core_DgvPrinter printer = new();
                List<string> visibleColumns = [];
                foreach (DataGridViewColumn col in Transactions_DataGridView_Transactions.Columns)
                {
                    if (col.Visible)
                    {
                        visibleColumns.Add(col.Name);
                    }
                }

                printer.SetPrintVisibleColumns(visibleColumns);
                printer.Print(Transactions_DataGridView_Transactions);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error printing transactions: {ex.Message}", @"Print Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async Task LoadTransactionsAsync()
        {
            System.Diagnostics.Debug.WriteLine("[DEBUG] LoadTransactionsAsync started");
            DataTable dt = new();
            MySqlCommand cmd;

            bool hasLikeSearch = false;
            string? likeSearchText = null;
            string? likeSearchColumn = null;
            if (Controls.ContainsKey("Transactions_ComboBox_Like") && Controls.ContainsKey("Transactions_TextBox_Like"))
            {
                if (Controls["Transactions_ComboBox_Like"] is ComboBox likeCombo &&
                    Controls["Transactions_TextBox_Like"] is TextBox likeText && likeCombo.SelectedIndex > 0 &&
                    !string.IsNullOrWhiteSpace(likeText.Text))
                {
                    hasLikeSearch = true;
                    likeSearchText = likeText.Text.Trim();
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
                cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@SearchPattern", $"%{likeSearchText}%");
                System.Diagnostics.Debug.WriteLine($"[DEBUG] LIKE Search: {query} with pattern '%{likeSearchText}%'");
            }
            else
            {
                string part = _transactionsComboBoxSearchPartId.Text;
                string user = Transactions_ComboBox_UserFullName.Text;
                string building = Transactions_ComboBox_Building.Text;
                string notes = Transactions_TextBox_Notes.Text;
                bool filterByDate = Control_AdvancedRemove_CheckBox_Date.Checked;
                DateTime? dateFrom =
                    filterByDate ? Control_AdvancedRemove_DateTimePicker_From.Value.Date : (DateTime?)null;
                DateTime? dateTo = filterByDate
                    ? Control_AdvancedRemove_DateTimePicker_To.Value.Date.AddDays(1).AddTicks(-1)
                    : (DateTime?)null;

                bool partSelected = _transactionsComboBoxSearchPartId.SelectedIndex > 0 &&
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
                    MessageBox.Show(@"Please fill in at least one field to search.", @"Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (filterByDate && dateFrom > dateTo)
                {
                    MessageBox.Show(@"The 'From' date cannot be after the 'To' date.", @"Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                StringBuilder queryBuilder = new();
                queryBuilder.Append("SELECT * FROM inv_transaction WHERE 1=1 ");
                List<MySqlParameter> parameters = [];

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
                queryBuilder.Append($"ORDER BY {orderBy} {(SortDescending ? "DESC" : "ASC")} ");
                queryBuilder.Append("LIMIT @Offset, @PageSize ");
                parameters.Add(new MySqlParameter("@Offset", (_currentPage - 1) * PageSize));
                parameters.Add(new MySqlParameter("@PageSize", PageSize));

                cmd = new MySqlCommand(queryBuilder.ToString());
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

            List<Model_Transactions> result = [];
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
            Transactions_Button_Help.Enabled = result.Count > 0;
            if (_displayedTransactions.Count > 0)
            {
                Transactions_DataGridView_Transactions.ClearSelection();
            }

            UpdatePagingButtons(result.Count);
            System.Diagnostics.Debug.WriteLine("[DEBUG] LoadTransactionsAsync finished");
        }

        private void ResetFilters()
        {
            Service_DebugTracer.TraceUIAction("RESET_FILTERS_INITIATED", nameof(Transactions),
                new Dictionary<string, object>
                {
                    ["HasSearchCriteria"] = _lastSearchCriteria.Count > 0,
                    ["CurrentPage"] = _currentPage
                });

            // Reset traditional filters
            Transactions_ComboBox_SortBy.SelectedIndex = 0;
            _transactionsComboBoxSearchPartId.SelectedIndex = 0;
            Transactions_ComboBox_UserFullName.SelectedIndex = 0;
            Transactions_ComboBox_Building.SelectedIndex = 0;
            Control_AdvancedRemove_CheckBox_Date.Checked = false;
            SetupDateRangeDefaults();
            
            // Reset smart search functionality
            if (Controls.ContainsKey("Transactions_TextBox_SmartSearch"))
            {
                (Controls["Transactions_TextBox_SmartSearch"] as TextBox)!.Text = "";
            }
            
            // Clear smart search criteria
            _lastSearchCriteria.Clear();
            _isPaginationNavigation = false;
            _currentPage = 1;
            
            // Clear selection report
            ClearSelectionReportControls();
            
            // Clear data grid
            Transactions_DataGridView_Transactions.DataSource = null;
            
            // Reset status
            UpdateSmartSearchStatus("Filters reset - ready for new search");

            Service_DebugTracer.TraceUIAction("RESET_FILTERS_COMPLETED", nameof(Transactions),
                new Dictionary<string, object>
                {
                    ["SmartSearchCleared"] = true,
                    ["TraditionalFiltersReset"] = true,
                    ["PageReset"] = true
                });
        }

        private void UpdatePagingButtons(int resultCount)
        {
            Transactions_Button_PreviousPage.Enabled = _currentPage > 1;
            Transactions_Button_NextPage.Enabled = resultCount >= PageSize;
        }

        private async void Transactions_Button_Help_Click(object? sender, EventArgs e)
        {
            if (Transactions_DataGridView_Transactions.SelectedRows.Count != 1)
            {
                return;
            }

            if (Transactions_DataGridView_Transactions.SelectedRows[0]
                    .DataBoundItem is not Model_Transactions selected ||
                string.IsNullOrWhiteSpace(selected.BatchNumber))
            {
                MessageBox.Show(@"No Batch Number found for the selected transaction.", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Control_ProgressBarUserControl progress = new();
            Controls.Add(progress);
            progress.BringToFront();
            progress.ShowProgress();
            progress.UpdateProgress(10, "Loading batch history...");
            await Task.Delay(100);

            string? batchNumber = selected.BatchNumber;
            var dao = new Dao_Transactions();
            var searchResult = await dao.SearchTransactionsAsync(
                _isAdmin ? string.Empty : _currentUser,
                _isAdmin,
                batchNumber: batchNumber,
                sortColumn: "ReceiveDate",
                sortDescending: true,
                page: 1,
                pageSize: 1000
            );

            progress.UpdateProgress(80, "Mapping results...");
            await Task.Delay(100);

            List<Model_Transactions> results = searchResult.IsSuccess && searchResult.Data != null 
                ? searchResult.Data 
                : new List<Model_Transactions>();

            // Build description for each row
            List<dynamic> describedResults = [];
            for (int i = 0; i < results.Count; i++)
            {
                Model_Transactions curr = results[i];
                string desc = "";
                if (i == results.Count - 1) // last row (oldest)
                {
                    desc = "Initial Transaction";
                }
                else
                {
                    Model_Transactions prev = results[i + 1];
                    if (curr.TransactionType == TransactionType.OUT)
                    {
                        desc = "Removed From System";
                    }
                    else if (curr.TransactionType == TransactionType.TRANSFER && prev.ToLocation != curr.FromLocation)
                    {
                        desc =
                            $"Part transferred from {prev.ToLocation ?? "Unknown"} to {curr.ToLocation ?? "Unknown"}";
                    }
                    else if (curr.TransactionType == TransactionType.IN)
                    {
                        desc = "Received Into System";
                    }
                    else
                    {
                        desc = "Transaction";
                    }
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
            DataGridViewTextBoxColumn descCol = new()
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
            Transactions_Button_Help.Enabled = describedResults.Count > 0;
            if (describedResults.Count > 0)
            {
                Transactions_DataGridView_Transactions.ClearSelection();
            }

            progress.UpdateProgress(100, "Complete");
            await Task.Delay(200);
            progress.HideProgress();
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
            DataGridViewTextBoxColumn descCol = new()
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

        /// <summary>
        /// Initializes smart search controls and adds them to the form
        /// </summary>
        private void InitializeSmartSearchControls()
        {
            try
            {
                // Create smart search panel
                Transactions_Panel_SmartSearch = new Panel
                {
                    Height = 35,
                    Dock = DockStyle.None,
                    Margin = new Padding(3),
                    BackColor = SystemColors.Control
                };

                // Create smart search label
                Transactions_Label_SmartSearch = new Label
                {
                    Text = "Smart Search:",
                    Location = new Point(5, 8),
                    Size = new Size(80, 20),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                // Create smart search textbox
                Transactions_TextBox_SmartSearch = new TextBox
                {
                    Location = new Point(90, 6),
                    Size = new Size(200, 23),
                    PlaceholderText = "e.g., partid:A123, user:JSMITH, #urgent"
                };

                // Create smart search button
                Transactions_Button_SmartSearch = new Button
                {
                    Text = "Search",
                    Location = new Point(295, 5),
                    Size = new Size(60, 25),
                    UseVisualStyleBackColor = true
                };

                // Create help label
                Transactions_Label_SmartSearchHelp = new Label
                {
                    Text = "Use: partid:value, batch:value, user:value, qty:value, notes:text",
                    Location = new Point(90, 28),
                    Size = new Size(270, 15),
                    Font = new Font(Font.FontFamily, 7.5f, FontStyle.Italic),
                    ForeColor = SystemColors.GrayText
                };

                // Add controls to panel
                Transactions_Panel_SmartSearch.Controls.AddRange(new Control[]
                {
                    Transactions_Label_SmartSearch,
                    Transactions_TextBox_SmartSearch,
                    Transactions_Button_SmartSearch,
                    Transactions_Label_SmartSearchHelp
                });

                // Wire up events
                Transactions_Button_SmartSearch.Click += async (s, e) => 
                    await HandleSmartSearchAsync(Transactions_TextBox_SmartSearch.Text);
                
                Transactions_TextBox_SmartSearch.KeyPress += async (s, e) =>
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        e.Handled = true;
                        await HandleSmartSearchAsync(Transactions_TextBox_SmartSearch.Text);
                    }
                };

                // Add panel to the input table layout at the top (row 0, shifting others down)
                if (Transactions_TableLayout_Inputs != null)
                {
                    // Insert at the beginning (row 0)
                    Transactions_TableLayout_Inputs.RowCount++;
                    Transactions_TableLayout_Inputs.RowStyles.Insert(0, new RowStyle(SizeType.Absolute, 50F));
                    
                    // Shift existing controls down by 1 row
                    for (int i = Transactions_TableLayout_Inputs.Controls.Count - 1; i >= 0; i--)
                    {
                        var control = Transactions_TableLayout_Inputs.Controls[i];
                        var position = Transactions_TableLayout_Inputs.GetPositionFromControl(control);
                        Transactions_TableLayout_Inputs.SetRow(control, position.Row + 1);
                    }
                    
                    // Add smart search panel at row 0
                    Transactions_TableLayout_Inputs.Controls.Add(Transactions_Panel_SmartSearch, 0, 0);
                }

                // Apply theme to individual controls in the panel
                foreach (Control ctrl in Transactions_Panel_SmartSearch.Controls)
                {
                    // Apply theme colors based on control type
                    if (ctrl is Label label)
                    {
                        label.ForeColor = Model_AppVariables.UserUiColors?.LabelForeColor ?? SystemColors.ControlText;
                        label.BackColor = Model_AppVariables.UserUiColors?.LabelBackColor ?? SystemColors.Control;
                    }
                    else if (ctrl is TextBox textBox)
                    {
                        textBox.ForeColor = Model_AppVariables.UserUiColors?.TextBoxForeColor ?? SystemColors.WindowText;
                        textBox.BackColor = Model_AppVariables.UserUiColors?.TextBoxBackColor ?? SystemColors.Window;
                    }
                    else if (ctrl is Button button)
                    {
                        button.ForeColor = Model_AppVariables.UserUiColors?.ButtonForeColor ?? SystemColors.ControlText;
                        button.BackColor = Model_AppVariables.UserUiColors?.ButtonBackColor ?? SystemColors.Control;
                    }
                }

                LoggingUtility.LogApplicationInfo("Smart search controls initialized successfully");
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex,
                    ErrorSeverity.Medium,
                    controlName: "InitializeSmartSearchControls");
            }
        }

        #endregion

        #region Smart Search Methods

        /// <summary>
        /// Handles smart search input with intelligent parsing
        /// </summary>
        /// <param name="searchText">Raw search input from user</param>
        private async Task HandleSmartSearchAsync(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    await LoadTransactionsAsync();
                    return;
                }

                // Parse search terms and build search criteria
                var searchCriteria = ParseSearchInput(searchText);
                
                var dao = new Dao_Transactions();
                var searchResult = await dao.SmartSearchAsync(
                    searchCriteria.SearchTerms,
                    GetSelectedTransactionTypes(),
                    GetSelectedTimeRange(),
                    GetSelectedLocations(),
                    _isAdmin ? string.Empty : _currentUser,
                    _isAdmin,
                    _currentPage,
                    PageSize
                );

                if (searchResult.IsSuccess && searchResult.Data != null)
                {
                    await DisplaySearchResultsAsync(searchResult.Data);
                    await UpdateAnalyticsDashboardAsync(searchResult.Data);
                }
                else
                {
                    Service_ErrorHandler.HandleException(
                        new Exception($"Smart search failed: {searchResult.ErrorMessage}"),
                        ErrorSeverity.Medium,
                        controlName: "Transactions_SmartSearch"
                    );
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleDatabaseError(ex, 
                    controlName: nameof(Transactions));
            }
        }

        /// <summary>
        /// Parses user search input into structured search criteria
        /// </summary>
        /// <param name="searchText">Raw search input</param>
        /// <returns>Parsed search criteria</returns>
        private (Dictionary<string, string> SearchTerms, List<string> Tags, SearchType Type) ParseSearchInput(string searchText)
        {
            var searchTerms = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var tags = new List<string>();
            var searchType = SearchType.General;

            if (string.IsNullOrWhiteSpace(searchText))
                return (searchTerms, tags, searchType);

            // Split by spaces but preserve quoted strings
            var terms = SplitSearchTerms(searchText.Trim());

            foreach (var term in terms)
            {
                if (string.IsNullOrWhiteSpace(term))
                    continue;

                // Check for field-specific searches (field:value)
                if (term.Contains(':'))
                {
                    var parts = term.Split(':', 2);
                    if (parts.Length == 2)
                    {
                        var field = parts[0].ToLowerInvariant();
                        var value = parts[1].Trim('"', '\'');

                        switch (field)
                        {
                            case "part":
                            case "partid":
                                searchTerms["partid"] = value;
                                searchType = SearchType.Specific;
                                break;
                            case "batch":
                            case "batchnumber":
                                searchTerms["batch"] = value;
                                searchType = SearchType.Specific;
                                break;
                            case "op":
                            case "operation":
                                searchTerms["operation"] = value;
                                searchType = SearchType.Specific;
                                break;
                            case "user":
                                searchTerms["user"] = value;
                                searchType = SearchType.Specific;
                                break;
                            case "qty":
                            case "quantity":
                                searchTerms["quantity"] = value;
                                searchType = SearchType.Specific;
                                break;
                            case "notes":
                                searchTerms["notes"] = value;
                                searchType = SearchType.Specific;
                                break;
                            case "type":
                            case "itemtype":
                                searchTerms["itemtype"] = value;
                                searchType = SearchType.Specific;
                                break;
                        }
                        continue;
                    }
                }

                // Check for tags (words starting with #)
                if (term.StartsWith("#"))
                {
                    tags.Add(term.Substring(1));
                    continue;
                }

                // General search term
                if (!searchTerms.ContainsKey("general"))
                    searchTerms["general"] = term;
                else
                    searchTerms["general"] += " " + term;
            }

            return (searchTerms, tags, searchType);
        }

        /// <summary>
        /// Splits search terms while preserving quoted strings
        /// </summary>
        /// <param name="searchText">Search text to split</param>
        /// <returns>Array of search terms</returns>
        private string[] SplitSearchTerms(string searchText)
        {
            var terms = new List<string>();
            var currentTerm = new StringBuilder();
            var inQuotes = false;
            var quoteChar = '\0';

            for (int i = 0; i < searchText.Length; i++)
            {
                var c = searchText[i];

                if (!inQuotes && (c == '"' || c == '\''))
                {
                    inQuotes = true;
                    quoteChar = c;
                    continue;
                }

                if (inQuotes && c == quoteChar)
                {
                    inQuotes = false;
                    continue;
                }

                if (!inQuotes && char.IsWhiteSpace(c))
                {
                    if (currentTerm.Length > 0)
                    {
                        terms.Add(currentTerm.ToString());
                        currentTerm.Clear();
                    }
                    continue;
                }

                currentTerm.Append(c);
            }

            if (currentTerm.Length > 0)
                terms.Add(currentTerm.ToString());

            return terms.ToArray();
        }

        /// <summary>
        /// Gets selected transaction types from UI controls
        /// </summary>
        /// <returns>List of selected transaction types</returns>
        private List<TransactionType> GetSelectedTransactionTypes()
        {
            var types = new List<TransactionType>();

            // Add logic to read from UI checkboxes/filters
            // For now, return all types (this would be customized based on UI controls)
            types.AddRange(Enum.GetValues<TransactionType>());

            return types;
        }

        /// <summary>
        /// Gets selected time range from UI controls
        /// </summary>
        /// <returns>Time range tuple</returns>
        private (DateTime? from, DateTime? to) GetSelectedTimeRange()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;

            // Read from date picker controls
            if (Control_AdvancedRemove_CheckBox_Date.Checked)
            {
                fromDate = Control_AdvancedRemove_DateTimePicker_From.Value.Date;
                toDate = Control_AdvancedRemove_DateTimePicker_To.Value.Date.AddDays(1).AddSeconds(-1);
            }

            return (fromDate, toDate);
        }

        /// <summary>
        /// Gets selected locations from UI controls
        /// </summary>
        /// <returns>List of selected locations</returns>
        private List<string> GetSelectedLocations()
        {
            var locations = new List<string>();

            // Add logic to read from location filters
            // This would be customized based on UI location controls
            if (!string.IsNullOrEmpty(Transactions_ComboBox_Building.Text))
            {
                locations.Add(Transactions_ComboBox_Building.Text);
            }

            return locations;
        }

        /// <summary>
        /// Displays search results in the data grid
        /// </summary>
        /// <param name="transactions">Search results</param>
        private async Task DisplaySearchResultsAsync(List<Model_Transactions> transactions)
        {
            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => DisplaySearchResults(transactions)));
                }
                else
                {
                    DisplaySearchResults(transactions);
                }
            });
        }

        /// <summary>
        /// Synchronous version of DisplaySearchResultsAsync for UI thread
        /// </summary>
        /// <param name="transactions">Search results</param>
        private void DisplaySearchResults(List<Model_Transactions> transactions)
        {
            try
            {
                _displayedTransactions = new BindingList<Model_Transactions>(transactions);
                Transactions_DataGridView_Transactions.DataSource = _displayedTransactions;

                // Update pagination controls
                Transactions_Button_PreviousPage.Enabled = _currentPage > 1;
                Transactions_Button_NextPage.Enabled = transactions.Count == PageSize;

                // Update selection-related controls
                UpdateSelectionControls();

                // Enable/disable buttons based on results
                Transactions_Button_Print.Enabled = transactions.Count > 0;
                Transactions_Button_Help.Enabled = transactions.Count > 0;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex,
                    ErrorSeverity.Medium,
                    controlName: "DisplaySearchResults");
            }
        }

        /// <summary>
        /// Updates analytics dashboard with search results
        /// </summary>
        /// <param name="transactions">Transaction data for analytics</param>
        private async Task UpdateAnalyticsDashboardAsync(List<Model_Transactions> transactions)
        {
            try
            {
                // Get comprehensive analytics from database
                var dao = new Dao_Transactions();
                var analyticsResult = await dao.GetTransactionAnalyticsAsync(
                    _currentUser,
                    _isAdmin,
                    GetSelectedTimeRange()
                );

                if (analyticsResult.IsSuccess && analyticsResult.Data != null)
                {
                    var analytics = analyticsResult.Data;

                    // Update analytics display (would need UI controls for this)
                    await UpdateAnalyticsDisplay(analytics);
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex,
                    ErrorSeverity.Low,
                    controlName: "UpdateAnalyticsDashboard");
            }
        }

        /// <summary>
        /// Updates the analytics display with calculated metrics
        /// </summary>
        /// <param name="analytics">Analytics data dictionary</param>
        private async Task UpdateAnalyticsDisplay(Dictionary<string, object> analytics)
        {
            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        // Update analytics controls
                        // This would be implemented when analytics UI controls are added
                        LoggingUtility.LogApplicationInfo($"Analytics updated: {analytics.Count} metrics");
                    }));
                }
            });
        }

        /// <summary>
        /// Updates selection-related controls based on current data
        /// </summary>
        private void UpdateSelectionControls()
        {
            try
            {
                if (Transactions_DataGridView_Transactions.SelectedRows.Count > 0)
                {
                    var selectedTransaction = (Model_Transactions)Transactions_DataGridView_Transactions.SelectedRows[0].DataBoundItem;
                    
                    // Update selection report controls
                    Transactions_TextBox_Report_PartID.Text = selectedTransaction.PartID ?? "";
                    Transactions_TextBox_Report_BatchNumber.Text = selectedTransaction.BatchNumber ?? "";
                    Transactions_TextBox_Report_FromLocation.Text = selectedTransaction.FromLocation ?? "";
                    Transactions_TextBox_Report_ToLocation.Text = selectedTransaction.ToLocation ?? "";
                    Transactions_TextBox_Report_Operation.Text = selectedTransaction.Operation ?? "";
                    Transactions_TextBox_Report_Quantity.Text = selectedTransaction.Quantity.ToString();
                    Transactions_TextBox_Notes.Text = selectedTransaction.Notes ?? "";
                    Transactions_TextBox_Report_User.Text = selectedTransaction.User ?? "";
                    Transactions_TextBox_Report_ItemType.Text = selectedTransaction.ItemType ?? "";
                    Transactions_TextBox_Report_ReceiveDate.Text = selectedTransaction.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    Transactions_TextBox_Report_TransactionType.Text = selectedTransaction.TransactionType.ToString();
                }
                else
                {
                    // Clear selection report controls
                    ClearSelectionReportControls();
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex,
                    ErrorSeverity.Low,
                    controlName: "UpdateSelectionControls");
            }
        }

        /// <summary>
        /// Clears all selection report controls
        /// </summary>
        private void ClearSelectionReportControls()
        {
            Transactions_TextBox_Report_PartID.Text = "";
            Transactions_TextBox_Report_BatchNumber.Text = "";
            Transactions_TextBox_Report_FromLocation.Text = "";
            Transactions_TextBox_Report_ToLocation.Text = "";
            Transactions_TextBox_Report_Operation.Text = "";
            Transactions_TextBox_Report_Quantity.Text = "";
            Transactions_TextBox_Notes.Text = "";
            Transactions_TextBox_Report_User.Text = "";
            Transactions_TextBox_Report_ItemType.Text = "";
            Transactions_TextBox_Report_ReceiveDate.Text = "";
            Transactions_TextBox_Report_TransactionType.Text = "";
        }

        #endregion

        #region Smart Search Methods

        /// <summary>
        /// Parse smart search text with advanced syntax support
        /// Supports: partid:PART123, qty:>50, location:A1, user:JSMITH, batch:0000123
        /// </summary>
        /// <param name="searchText">Raw search input from user</param>
        /// <returns>Dictionary of search criteria</returns>
        private Dictionary<string, string> ParseSmartSearchText(string searchText)
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["SearchText"] = searchText ?? "",
                ["SearchLength"] = searchText?.Length ?? 0
            }, nameof(Transactions), nameof(ParseSmartSearchText));

            var criteria = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Service_DebugTracer.TraceMethodExit(criteria, nameof(Transactions), nameof(ParseSmartSearchText));
                return criteria;
            }

            // Split by comma and space, handle quoted strings
            var terms = SplitSearchTerms(searchText);
            
            foreach (string term in terms)
            {
                var cleanTerm = term.Trim();
                if (string.IsNullOrEmpty(cleanTerm)) continue;

                if (cleanTerm.Contains(':'))
                {
                    var parts = cleanTerm.Split(':', 2);
                    if (parts.Length == 2)
                    {
                        var field = parts[0].Trim().ToLower();
                        var value = parts[1].Trim().Trim('"', '\'');
                        
                        // Map field aliases to database columns
                        string dbField = MapSearchFieldToColumn(field);
                        criteria[dbField] = value;

                        Service_DebugTracer.TraceBusinessLogic("SMART_SEARCH_FIELD_PARSED", nameof(Transactions),
                            new Dictionary<string, object>
                            {
                                ["OriginalField"] = field,
                                ["MappedField"] = dbField,
                                ["Value"] = value
                            });
                    }
                }
                else if (cleanTerm.StartsWith('#'))
                {
                    // Handle hashtag searches for batch numbers
                    criteria["BatchNumber"] = cleanTerm.Substring(1);
                }
                else
                {
                    // General search term
                    criteria["_general"] = cleanTerm;
                }
            }

            Service_DebugTracer.TraceMethodExit(criteria, nameof(Transactions), nameof(ParseSmartSearchText));
            return criteria;
        }

        /// <summary>
        /// Split search terms handling quoted strings properly
        /// </summary>
        private List<string> SplitSearchTerms(string searchText)
        {
            var terms = new List<string>();
            var currentTerm = new StringBuilder();
            bool inQuotes = false;
            char quoteChar = '\0';

            foreach (char c in searchText)
            {
                if (!inQuotes && (c == '"' || c == '\''))
                {
                    inQuotes = true;
                    quoteChar = c;
                    currentTerm.Append(c);
                }
                else if (inQuotes && c == quoteChar)
                {
                    inQuotes = false;
                    currentTerm.Append(c);
                }
                else if (!inQuotes && (c == ',' || c == ' '))
                {
                    if (currentTerm.Length > 0)
                    {
                        terms.Add(currentTerm.ToString());
                        currentTerm.Clear();
                    }
                }
                else
                {
                    currentTerm.Append(c);
                }
            }

            if (currentTerm.Length > 0)
            {
                terms.Add(currentTerm.ToString());
            }

            return terms;
        }

        /// <summary>
        /// Map search field names to database column names
        /// </summary>
        private string MapSearchFieldToColumn(string field)
        {
            return field switch
            {
                "partid" or "part" => "PartID",
                "user" => "User", 
                "location" or "fromlocation" or "from" => "FromLocation",
                "tolocation" or "to" => "ToLocation",
                "operation" or "op" => "Operation",
                "quantity" or "qty" => "Quantity",
                "batch" or "batchnumber" => "BatchNumber",
                "type" or "transactiontype" => "TransactionType",
                "itemtype" => "ItemType",
                "notes" => "Notes",
                "date" or "receivedate" => "ReceiveDate",
                _ => field // Return as-is if no mapping found
            };
        }

        /// <summary>
        /// Execute smart search with parsed criteria
        /// </summary>
        private async Task ExecuteSmartSearchAsync(Dictionary<string, string> criteria)
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["CriteriaCount"] = criteria.Count,
                ["Criteria"] = criteria
            }, nameof(Transactions), nameof(ExecuteSmartSearchAsync));

            try
            {
                _lastSearchCriteria.Clear();
                foreach (var kvp in criteria)
                {
                    _lastSearchCriteria[kvp.Key] = kvp.Value;
                }

                var searchResult = await PerformSmartDatabaseSearch(criteria);
                
                if (searchResult.success)
                {
                    await DisplaySearchResultsAsync(searchResult.results);
                    UpdateSmartSearchStatus($"Found {searchResult.results.Rows.Count} results");
                }
                else
                {
                    Service_ErrorHandler.HandleException(
                        new Exception($"Smart search failed: {searchResult.error}"),
                        ErrorSeverity.Medium,
                        "Transactions_SmartSearch"
                    );
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, "Transactions_ExecuteSmartSearchAsync");
            }
            finally
            {
                Service_DebugTracer.TraceMethodExit(null, nameof(Transactions), nameof(ExecuteSmartSearchAsync));
            }
        }

        /// <summary>
        /// Perform database search with smart criteria
        /// </summary>
        private async Task<(bool success, DataTable results, string error)> PerformSmartDatabaseSearch(Dictionary<string, string> criteria)
        {
            var queryBuilder = new StringBuilder();
            var parameters = new List<MySqlParameter>();

            queryBuilder.Append("SELECT * FROM inv_transaction WHERE 1=1 ");

            foreach (var kvp in criteria)
            {
                if (kvp.Key == "_general")
                {
                    // General search across multiple fields
                    queryBuilder.Append("AND (PartID LIKE @General OR User LIKE @General OR FromLocation LIKE @General OR ToLocation LIKE @General OR Notes LIKE @General) ");
                    parameters.Add(new MySqlParameter("@General", $"%{kvp.Value}%"));
                }
                else if (kvp.Value.StartsWith('>'))
                {
                    // Greater than search
                    queryBuilder.Append($"AND {kvp.Key} > @{kvp.Key} ");
                    if (decimal.TryParse(kvp.Value.Substring(1), out decimal val))
                        parameters.Add(new MySqlParameter($"@{kvp.Key}", val));
                }
                else if (kvp.Value.StartsWith('<'))
                {
                    // Less than search
                    queryBuilder.Append($"AND {kvp.Key} < @{kvp.Key} ");
                    if (decimal.TryParse(kvp.Value.Substring(1), out decimal val))
                        parameters.Add(new MySqlParameter($"@{kvp.Key}", val));
                }
                else
                {
                    // Exact match or LIKE search
                    if (kvp.Value.Contains('*') || kvp.Value.Contains('%'))
                    {
                        queryBuilder.Append($"AND {kvp.Key} LIKE @{kvp.Key} ");
                        parameters.Add(new MySqlParameter($"@{kvp.Key}", kvp.Value.Replace('*', '%')));
                    }
                    else
                    {
                        queryBuilder.Append($"AND {kvp.Key} = @{kvp.Key} ");
                        parameters.Add(new MySqlParameter($"@{kvp.Key}", kvp.Value));
                    }
                }
            }

            // Add pagination
            queryBuilder.Append($"ORDER BY ID DESC LIMIT {PageSize} OFFSET {(_currentPage - 1) * PageSize}");

            try
            {
                using var connection = new MySqlConnection(Model_Users.Database);
                await connection.OpenAsync();
                
                using var command = new MySqlCommand(queryBuilder.ToString(), connection);
                command.Parameters.AddRange(parameters.ToArray());

                Service_DebugTracer.TraceDataAccess("SMART_SEARCH_QUERY_EXECUTION", nameof(Transactions),
                    new Dictionary<string, object>
                    {
                        ["Query"] = queryBuilder.ToString(),
                        ["ParameterCount"] = parameters.Count,
                        ["Page"] = _currentPage,
                        ["PageSize"] = PageSize
                    });

                using var adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(command);
                var results = new DataTable();
                adapter.Fill(results);

                return (true, results, "");
            }
            catch (Exception ex)
            {
                return (false, new DataTable(), ex.Message);
            }
        }

        /// <summary>
        /// Display search results with enhanced formatting
        /// </summary>
        private async Task DisplaySearchResultsAsync(DataTable results)
        {
            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    Invoke(() => DisplaySearchResults(results));
                }
                else
                {
                    DisplaySearchResults(results);
                }
            });
        }

        /// <summary>
        /// Display search results in DataGridView
        /// </summary>
        private void DisplaySearchResults(DataTable results)
        {
            try
            {
                Transactions_DataGridView_Transactions.DataSource = results;
                
                // Update status
                if (results.Rows.Count == 0)
                {
                    UpdateSmartSearchStatus("No results found");
                }
                else
                {
                    UpdateSmartSearchStatus($"Showing {results.Rows.Count} results (Page {_currentPage})");
                }

                // Enable/disable pagination buttons
                UpdatePaginationControls(results.Rows.Count);
                
                Service_DebugTracer.TraceUIAction("SEARCH_RESULTS_DISPLAYED", nameof(Transactions),
                    new Dictionary<string, object>
                    {
                        ["ResultCount"] = results.Rows.Count,
                        ["CurrentPage"] = _currentPage
                    });
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, "Transactions_DisplaySearchResults");
            }
        }

        /// <summary>
        /// Update smart search status message
        /// </summary>
        private void UpdateSmartSearchStatus(string message)
        {
            if (Controls.ContainsKey("Transactions_Label_SmartSearchStatus"))
            {
                if (Controls["Transactions_Label_SmartSearchStatus"] is Label statusLabel)
                {
                    statusLabel.Text = message;
                    statusLabel.ForeColor = message.Contains("error") ? Color.Red : Color.Green;
                }
            }
        }

        /// <summary>
        /// Update pagination control states
        /// </summary>
        private void UpdatePaginationControls(int resultCount)
        {
            // Enable Previous button if not on first page
            if (Controls.ContainsKey("Transactions_Button_PreviousPage"))
            {
                Controls["Transactions_Button_PreviousPage"].Enabled = _currentPage > 1;
            }

            // Enable Next button if we got a full page of results
            if (Controls.ContainsKey("Transactions_Button_NextPage"))
            {
                Controls["Transactions_Button_NextPage"].Enabled = resultCount == PageSize;
            }
        }

        #endregion

        #region Search Type Enumeration

        /// <summary>
        /// Enumeration for different types of search operations
        /// </summary>
        private enum SearchType
        {
            General,    // General keyword search across multiple fields
            Specific,   // Field-specific search with exact criteria
            Advanced    // Complex search with multiple criteria and filters
        }

        #endregion
    }
}
