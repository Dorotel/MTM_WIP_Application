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
using System.Drawing.Drawing2D;

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

        // Smart search infrastructure
        private readonly Timer _searchDebounceTimer = new() { Interval = 500 };
        private string _lastSearchText = string.Empty;
        private CancellationTokenSource _searchCancellation = new();
        private TransactionViewMode _currentViewMode = TransactionViewMode.Grid;

        // Progress reporting
        private Helper_StoredProcedureProgress? _progressHelper;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current page being displayed
        /// </summary>
        public int CurrentPage 
        { 
            get => _currentPage; 
            private set => _currentPage = Math.Max(1, value); 
        }

        /// <summary>
        /// Gets whether the current user has admin privileges
        /// </summary>
        public bool IsAdmin => _isAdmin;

        /// <summary>
        /// Gets the currently displayed transactions
        /// </summary>
        public IReadOnlyList<Model_Transactions> DisplayedTransactions => _displayedTransactions.ToList();

        #endregion

        #region Progress Control Methods

        /// <summary>
        /// Sets up progress reporting for database operations
        /// </summary>
        /// <param name="progressBar">The progress bar control</param>
        /// <param name="statusLabel">The status label control</param>
        public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, 
                this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
        }

        #endregion

        #region Constructors

        public Transactions(string connectionString, string currentUser)
        {
            InitializeComponent();

            // Apply comprehensive DPI scaling and runtime layout adjustments
            AutoScaleMode = AutoScaleMode.Dpi;
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

            _currentUser = currentUser;
            _isAdmin = Model_AppVariables.UserTypeAdmin;

            // Initialize smart search functionality
            InitializeSmartSearch();
            InitializeSearchPerformance();
            
            // Apply modern styling using Core_Themes
            ApplyModernStyling();

            SetupSortCombo();
            SetupDataGrid();

            Load += async (s, e) => await OnFormLoadAsync();

            Transactions_Button_Reset.Click += (s, e) => ResetFilters();

            // Paging logic
            Transfer_Button_Next.Click += async (s, e) =>
            {
                CurrentPage++;
                await LoadTransactionsAsync();
            };
            Transfer_Button_Previous.Click += async (s, e) =>
            {
                if (CurrentPage > 1)
                {
                    CurrentPage--;
                    await LoadTransactionsAsync();
                }
            };

            // Print button logic
            Transactions_Button_Print.Click += Transactions_Button_Print_Click;
            Core_Themes.ApplyTheme(this);
        }

        #region Initialization

        /// <summary>
        /// Initializes smart search functionality and controls
        /// </summary>
        private void InitializeSmartSearch()
        {
            try
            {
                // Wire up events for smart search
                Transactions_TextBox_SmartSearch.TextChanged += OnSmartSearchTextChanged;
                
                // Filter change events
                Transactions_CheckBox_IN.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
                Transactions_CheckBox_OUT.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
                Transactions_CheckBox_TRANSFER.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
                
                Transactions_Radio_Today.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
                Transactions_Radio_ThisWeek.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
                Transactions_Radio_ThisMonth.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
                Transactions_Radio_CustomRange.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
                
                // View mode change events
                Transactions_Radio_GridView.CheckedChanged += async (s, e) => await HandleViewModeChangeAsync();
                Transactions_Radio_ChartView.CheckedChanged += async (s, e) => await HandleViewModeChangeAsync();
                Transactions_Radio_TimelineView.CheckedChanged += async (s, e) => await HandleViewModeChangeAsync();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "InitializeSmartSearch");
            }
        }

        /// <summary>
        /// Initializes search performance optimizations
        /// </summary>
        private void InitializeSearchPerformance()
        {
            try
            {
                _searchDebounceTimer.Tick += async (s, e) =>
                {
                    _searchDebounceTimer.Stop();
                    await PerformDebouncedSearchAsync();
                };
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "InitializeSearchPerformance");
            }
        }

        #endregion

        #endregion

        #region Database Operations

        /// <summary>
        /// Loads transactions asynchronously using the current search criteria
        /// </summary>
        private async Task LoadTransactionsAsync()
        {
            try
            {
                _progressHelper?.ShowProgress("Loading transactions...");
                _progressHelper?.UpdateProgress(10, "Preparing search criteria...");

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

                _progressHelper?.UpdateProgress(30, "Building query...");

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
                        Service_ErrorHandler.HandleValidationError("Please fill in at least one field to search.", "Search Validation");
                        return;
                    }

                    if (filterByDate && dateFrom > dateTo)
                    {
                        Service_ErrorHandler.HandleValidationError("The 'From' date cannot be after the 'To' date.", "Date Validation");
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
                    parameters.Add(new MySqlParameter("@Offset", (CurrentPage - 1) * PageSize));
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

                _progressHelper?.UpdateProgress(50, "Executing query...");

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

                _progressHelper?.UpdateProgress(70, "Processing results...");

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

                await DisplaySearchResultsAsync(result);
                
                _progressHelper?.UpdateProgress(100, "Complete");
                _progressHelper?.ShowSuccess($"Loaded {result.Count} transactions");
                
                System.Diagnostics.Debug.WriteLine("[DEBUG] LoadTransactionsAsync finished");
            }
            catch (Exception ex)
            {
                _progressHelper?.ShowError("Failed to load transactions");
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "LoadTransactionsAsync");
            }
        }

        /// <summary>
        /// Displays search results in the current view mode
        /// </summary>
        private async Task DisplaySearchResultsAsync(List<Model_Transactions> transactions)
        {
            try
            {
                _displayedTransactions = new BindingList<Model_Transactions>(transactions);
                
                switch (_currentViewMode)
                {
                    case TransactionViewMode.Grid:
                        await DisplayGridViewAsync(transactions);
                        break;
                    case TransactionViewMode.Chart:
                        await DisplayChartViewAsync(transactions);
                        break;
                    case TransactionViewMode.Timeline:
                        await DisplayTimelineViewAsync(transactions);
                        break;
                }
                
                UpdateResultsStatistics(transactions);
                UpdatePagingButtons(transactions.Count);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "DisplaySearchResultsAsync");
            }
        }

        /// <summary>
        /// Displays results in grid view
        /// </summary>
        private async Task DisplayGridViewAsync(List<Model_Transactions> transactions)
        {
            await Task.Run(() =>
            {
                this.Invoke(() =>
                {
                    SetupDataGridColumns();
                    Transactions_DataGridView_Transactions.DataSource = _displayedTransactions;
                    
                    // Apply row coloring based on transaction type
                    ApplyRowStyling();
                    
                    Transactions_Image_NothingFound.Visible = transactions.Count == 0;
                    Transactions_DataGridView_Transactions.Visible = transactions.Count > 0;
                    Transactions_Button_Print.Enabled = transactions.Count > 0;
                    Transfer_Button_SelectionHistory.Enabled = transactions.Count > 0;
                    
                    if (_displayedTransactions.Count > 0)
                    {
                        Transactions_DataGridView_Transactions.ClearSelection();
                    }
                });
            });
        }

        /// <summary>
        /// Displays results in chart view
        /// </summary>
        private async Task DisplayChartViewAsync(List<Model_Transactions> transactions)
        {
            await Task.Run(() =>
            {
                this.Invoke(() =>
                {
                    // TODO: Implement chart visualization
                    // For now, display a message indicating chart view
                    Transactions_Image_NothingFound.Visible = false;
                    Transactions_DataGridView_Transactions.Visible = true;
                    
                    // Create a simple chart representation in the DataGridView
                    CreateChartViewInDataGrid(transactions);
                });
            });
        }

        /// <summary>
        /// Displays results in timeline view
        /// </summary>
        private async Task DisplayTimelineViewAsync(List<Model_Transactions> transactions)
        {
            await Task.Run(() =>
            {
                this.Invoke(() =>
                {
                    // TODO: Implement timeline visualization
                    // For now, display chronological view in DataGridView
                    Transactions_Image_NothingFound.Visible = false;
                    Transactions_DataGridView_Transactions.Visible = true;
                    
                    // Create a timeline representation
                    CreateTimelineViewInDataGrid(transactions);
                });
            });
        }

        /// <summary>
        /// Creates a simple chart representation in the DataGridView
        /// </summary>
        private void CreateChartViewInDataGrid(List<Model_Transactions> transactions)
        {
            try
            {
                // Group transactions by type for chart display
                var chartData = transactions.GroupBy(t => t.TransactionType)
                    .Select(g => new
                    {
                        TransactionType = g.Key.ToString(),
                        Count = g.Count(),
                        TotalQuantity = g.Sum(t => t.Quantity),
                        Percentage = Math.Round((double)g.Count() / transactions.Count * 100, 1)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                // Setup chart columns
                Transactions_DataGridView_Transactions.AutoGenerateColumns = false;
                Transactions_DataGridView_Transactions.Columns.Clear();
                
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Transaction Type",
                    DataPropertyName = "TransactionType",
                    Name = "colChartType",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Count",
                    DataPropertyName = "Count", 
                    Name = "colChartCount",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Total Quantity",
                    DataPropertyName = "TotalQuantity",
                    Name = "colChartQuantity",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Percentage",
                    DataPropertyName = "Percentage", 
                    Name = "colChartPercentage",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });

                Transactions_DataGridView_Transactions.DataSource = new BindingList<dynamic>(chartData.Cast<dynamic>().ToList());
                
                // Apply color coding for chart view
                foreach (DataGridViewRow row in Transactions_DataGridView_Transactions.Rows)
                {
                    if (row.Cells["colChartType"].Value?.ToString() is string transactionType)
                    {
                        switch (transactionType)
                        {
                            case "IN":
                                row.DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 218);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(21, 87, 36);
                                break;
                            case "OUT":
                                row.DefaultCellStyle.BackColor = Color.FromArgb(248, 215, 218);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(132, 32, 41);
                                break;
                            case "TRANSFER":
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(133, 100, 4);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "CreateChartViewInDataGrid");
            }
        }

        /// <summary>
        /// Creates a timeline representation in the DataGridView
        /// </summary>
        private void CreateTimelineViewInDataGrid(List<Model_Transactions> transactions)
        {
            try
            {
                // Sort transactions chronologically for timeline view
                var timelineData = transactions.OrderByDescending(t => t.DateTime)
                    .Select(t => new
                    {
                        DateTime = t.DateTime.ToString("yyyy-MM-dd HH:mm"),
                        Type = GetTransactionTypeIcon(t.TransactionType),
                        PartID = t.PartID ?? "",
                        Quantity = t.Quantity,
                        Operation = t.Operation ?? "",
                        User = t.User ?? "",
                        Notes = t.Notes ?? ""
                    })
                    .ToList();

                // Setup timeline columns
                Transactions_DataGridView_Transactions.AutoGenerateColumns = false;
                Transactions_DataGridView_Transactions.Columns.Clear();
                
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Time",
                    DataPropertyName = "DateTime",
                    Name = "colTimelineTime",
                    Width = 120
                });
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Type",
                    DataPropertyName = "Type",
                    Name = "colTimelineType",
                    Width = 60
                });
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Part ID",
                    DataPropertyName = "PartID",
                    Name = "colTimelinePartID",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Qty",
                    DataPropertyName = "Quantity",
                    Name = "colTimelineQuantity",
                    Width = 60
                });
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Operation",
                    DataPropertyName = "Operation",
                    Name = "colTimelineOperation",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "User",
                    DataPropertyName = "User",
                    Name = "colTimelineUser",
                    Width = 80
                });
                Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Notes",
                    DataPropertyName = "Notes",
                    Name = "colTimelineNotes",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });

                Transactions_DataGridView_Transactions.DataSource = new BindingList<dynamic>(timelineData.Cast<dynamic>().ToList());
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "CreateTimelineViewInDataGrid");
            }
        }

        /// <summary>
        /// Gets the appropriate icon for transaction type
        /// </summary>
        private string GetTransactionTypeIcon(TransactionType transactionType)
        {
            return transactionType switch
            {
                TransactionType.IN => "📥",
                TransactionType.OUT => "📤", 
                TransactionType.TRANSFER => "🔄",
                _ => "❓"
            };
        }

        #endregion

        #region UI Event Handlers

        private async Task OnFormLoadAsync()
        {
            try
            {
                _progressHelper?.ShowProgress("Loading form...");
                
                Transactions_Button_Print.Enabled = false; // Disable print button on load
                Transfer_Button_SelectionHistory.Enabled = false; // Disable selection history button on load
                await LoadUserCombosAsync();
                LoadBuildingCombo(); // Remove await since method is no longer async
                await LoadPartComboAsync();
                SetupDateRangeDefaults();
                WireUpEvents();
                
                _progressHelper?.ShowSuccess("Form loaded successfully");
            }
            catch (Exception ex)
            {
                _progressHelper?.ShowError("Failed to load form");
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "OnFormLoadAsync");
            }
        }

        private async Task LoadUserCombosAsync()
        {
            try
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
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "LoadUserCombosAsync");
            }
        }

        private void LoadBuildingCombo()
        {
            try
            {
                // Set Building ComboBox to only 3 items: [ Enter Building ], Expo Drive, Vits Drive
                Transactions_ComboBox_Building.Items.Clear();
                Transactions_ComboBox_Building.Items.Add("[ Enter Building ]");
                Transactions_ComboBox_Building.Items.Add("Expo Drive");
                Transactions_ComboBox_Building.Items.Add("Vits Drive");
                Transactions_ComboBox_Building.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "LoadBuildingCombo");
            }
        }

        private async Task LoadPartComboAsync()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(_transactionsComboBoxSearchPartId);
                _transactionsComboBoxSearchPartId.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "LoadPartComboAsync");
            }
        }

        private void SetupDateRangeDefaults()
        {
            try
            {
                Control_AdvancedRemove_CheckBox_Date.Checked = false;
                Control_AdvancedRemove_DateTimePicker_From.Value = DateTime.Today.AddDays(-7);
                Control_AdvancedRemove_DateTimePicker_To.Value = DateTime.Today;
                Control_AdvancedRemove_DateTimePicker_From.Enabled = false;
                Control_AdvancedRemove_DateTimePicker_To.Enabled = false;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "SetupDateRangeDefaults");
            }
        }

        private void WireUpEvents()
        {
            try
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
                _transactionsComboBoxSearchPartId.SelectedIndexChanged += Transactions_EnableSearchButtonIfValid;
                Transactions_ComboBox_UserFullName.SelectedIndexChanged += Transactions_EnableSearchButtonIfValid;
                Transactions_ComboBox_Building.SelectedIndexChanged += Transactions_EnableSearchButtonIfValid;
                Transactions_EnableSearchButtonIfValid(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "WireUpEvents");
            }
        }

        private void Transactions_EnableSearchButtonIfValid(object? sender, EventArgs e)
        {
            try
            {
                bool enable = _transactionsComboBoxSearchPartId.SelectedIndex > 0
                              || Transactions_ComboBox_UserFullName.SelectedIndex > 0
                              || Transactions_ComboBox_Building.SelectedIndex > 0;
                Transactions_Button_Search.Enabled = enable;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "Transactions_EnableSearchButtonIfValid");
            }
        }

        private void Transactions_Button_SidePanel_Click(object? sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "Transactions_Button_SidePanel_Click");
            }
        }

        private void Transactions_DataGridView_Transactions_SelectionChanged(object? sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "Transactions_DataGridView_Transactions_SelectionChanged");
            }
        }

        private async void Transfer_Button_BranchHistory_Click(object? sender, EventArgs e)
        {
            try
            {
                _progressHelper?.ShowProgress("Loading batch history...");
                
                if (Transactions_DataGridView_Transactions.SelectedRows.Count != 1)
                {
                    return;
                }

                if (Transactions_DataGridView_Transactions.SelectedRows[0]
                        .DataBoundItem is not Model_Transactions selected ||
                    string.IsNullOrWhiteSpace(selected.BatchNumber))
                {
                    Service_ErrorHandler.HandleValidationError("No Batch Number found for the selected transaction.", "Selection Error");
                    return;
                }

                _progressHelper?.UpdateProgress(20, "Searching for batch history...");
                
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

                _progressHelper?.UpdateProgress(60, "Processing results...");

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

                _progressHelper?.UpdateProgress(80, "Setting up history view...");

                SetupHistoryDataGrid();
                Transactions_DataGridView_Transactions.DataSource = new BindingList<dynamic>(describedResults);
                Transactions_Image_NothingFound.Visible = describedResults.Count == 0;
                Transactions_DataGridView_Transactions.Visible = describedResults.Count > 0;
                Transactions_Button_Print.Enabled = describedResults.Count > 0;
                Transfer_Button_SelectionHistory.Enabled = describedResults.Count > 0;
                
                if (describedResults.Count > 0)
                {
                    Transactions_DataGridView_Transactions.ClearSelection();
                }

                _progressHelper?.ShowSuccess($"Loaded {describedResults.Count} history records");
            }
            catch (Exception ex)
            {
                _progressHelper?.ShowError("Failed to load batch history");
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    retryAction: () => Transfer_Button_BranchHistory_Click(sender, e),
                    controlName: "Transfer_Button_BranchHistory_Click");
            }
        }

        #endregion

        #region Button Clicks

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
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    retryAction: () => Transactions_Button_Print_Click(sender, e),
                    controlName: "Transactions_Button_Print_Click");
            }
        }

        private void ResetFilters()
        {
            try
            {
                Transactions_ComboBox_SortBy.SelectedIndex = 0;
                _transactionsComboBoxSearchPartId.SelectedIndex = 0;
                Transactions_ComboBox_UserFullName.SelectedIndex = 0;
                Transactions_ComboBox_Building.SelectedIndex = 0;
                Control_AdvancedRemove_CheckBox_Date.Checked = false;
                SetupDateRangeDefaults();
                CurrentPage = 1;
                
                // Reset smart search controls
                Transactions_TextBox_SmartSearch.Clear();
                Transactions_CheckBox_IN.Checked = true;
                Transactions_CheckBox_OUT.Checked = true;
                Transactions_CheckBox_TRANSFER.Checked = true;
                Transactions_Radio_Today.Checked = true;
                
                // Reset view mode to Grid
                Transactions_Radio_GridView.Checked = true;
                _currentViewMode = TransactionViewMode.Grid;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "ResetFilters");
            }
        }

        #endregion

        #region Smart Search Methods

        /// <summary>
        /// Handles universal smart search with debouncing and multi-field search
        /// </summary>
        /// <param name="searchText">Raw search input from user</param>
        private async Task HandleSmartSearchAsync(string searchText)
        {
            try
            {
                _progressHelper?.ShowProgress("Processing smart search...");

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    await LoadTransactionsAsync();
                    return;
                }

                // Parse search terms and build search criteria
                var searchCriteria = ParseSearchInput(searchText);
                
                _progressHelper?.UpdateProgress(30, "Executing smart search...");
                
                // Use DAO smart search with parsed criteria
                var dao = new Dao_Transactions();
                var searchTerms = BuildSearchTermsDictionary(searchCriteria);
                var transactionTypes = GetSelectedTransactionTypes();
                var timeRange = GetSelectedTimeRange();
                var locations = GetSelectedLocations();
                
                var searchResult = await dao.SmartSearchAsync(
                    searchTerms,
                    transactionTypes,
                    timeRange,
                    locations,
                    _isAdmin ? string.Empty : _currentUser,
                    _isAdmin,
                    CurrentPage,
                    PageSize
                );

                _progressHelper?.UpdateProgress(80, "Processing results...");

                if (searchResult.IsSuccess && searchResult.Data != null)
                {
                    await DisplaySearchResultsAsync(searchResult.Data);
                    await UpdateAnalyticsDashboardAsync(searchResult.Data);
                    _progressHelper?.ShowSuccess($"Smart search found {searchResult.Data.Count} transactions");
                }
                else
                {
                    Service_ErrorHandler.HandleException(
                        new Exception($"Smart search failed: {searchResult.ErrorMessage}"),
                        ErrorSeverity.Medium,
                        controlName: "Smart Search"
                    );
                    _progressHelper?.ShowError("Smart search failed");
                }
            }
            catch (Exception ex)
            {
                _progressHelper?.ShowError("Smart search failed");
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    retryAction: () => HandleSmartSearchAsync(searchText),
                    controlName: "HandleSmartSearchAsync");
            }
        }

        /// <summary>
        /// Builds search terms dictionary from parsed criteria
        /// </summary>
        /// <param name="criteria">Parsed search criteria</param>
        /// <returns>Dictionary of search terms for DAO</returns>
        private Dictionary<string, string> BuildSearchTermsDictionary(SmartSearchCriteria criteria)
        {
            var searchTerms = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(criteria.SpecificPartId))
                searchTerms["partid"] = criteria.SpecificPartId;

            if (!string.IsNullOrWhiteSpace(criteria.SpecificUser))
                searchTerms["user"] = criteria.SpecificUser;

            if (!string.IsNullOrWhiteSpace(criteria.SpecificLocation))
                searchTerms["location"] = criteria.SpecificLocation;

            if (!string.IsNullOrWhiteSpace(criteria.SpecificOperation))
                searchTerms["operation"] = criteria.SpecificOperation;

            if (criteria.SpecificQuantity.HasValue)
                searchTerms["quantity"] = criteria.SpecificQuantity.Value.ToString();

            // Combine general terms for broad search
            if (criteria.GeneralTerms.Count > 0)
                searchTerms["general"] = string.Join(" ", criteria.GeneralTerms);

            return searchTerms;
        }

        /// <summary>
        /// Updates the analytics dashboard with transaction data
        /// </summary>
        /// <param name="transactions">Transaction data</param>
        private async Task UpdateAnalyticsDashboardAsync(List<Model_Transactions> transactions)
        {
            try
            {
                if (transactions.Count == 0) return;

                // Calculate analytics from current transaction data
                var analytics = CalculateTransactionAnalytics(transactions);
                
                // Update the form title with analytics summary
                var summaryText = $"Transactions - {analytics["TotalTransactions"]} total | " +
                                $"📥 {analytics["InTransactions"]} IN | " +
                                $"📤 {analytics["OutTransactions"]} OUT | " +
                                $"🔄 {analytics["TransferTransactions"]} TRANSFER";
                
                this.Text = summaryText;
                
                // Log analytics for debugging
                System.Diagnostics.Debug.WriteLine($"[ANALYTICS] {summaryText}");
                System.Diagnostics.Debug.WriteLine($"[ANALYTICS] Top Part: {analytics["TopPartId"]}, Top User: {analytics["TopUser"]}");
                System.Diagnostics.Debug.WriteLine($"[ANALYTICS] Total Quantity: {analytics["TotalQuantity"]}, Unique Parts: {analytics["UniquePartIds"]}");
                
                // TODO: Update visual dashboard components when implemented
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "UpdateAnalyticsDashboardAsync");
            }
        }

        /// <summary>
        /// Calculates analytics from transaction data
        /// </summary>
        /// <param name="transactions">Transaction data to analyze</param>
        /// <returns>Dictionary containing calculated analytics</returns>
        private Dictionary<string, object> CalculateTransactionAnalytics(List<Model_Transactions> transactions)
        {
            var analytics = new Dictionary<string, object>();
            
            // Basic counts
            analytics["TotalTransactions"] = transactions.Count;
            analytics["InTransactions"] = transactions.Count(t => t.TransactionType == TransactionType.IN);
            analytics["OutTransactions"] = transactions.Count(t => t.TransactionType == TransactionType.OUT);
            analytics["TransferTransactions"] = transactions.Count(t => t.TransactionType == TransactionType.TRANSFER);
            
            // Quantity analysis
            analytics["TotalQuantity"] = transactions.Sum(t => t.Quantity);
            
            // Unique counts
            analytics["UniquePartIds"] = transactions.Select(t => t.PartID).Distinct().Count();
            analytics["ActiveUsers"] = transactions.Select(t => t.User).Distinct().Count();
            
            // Top items
            var topPartGroup = transactions.GroupBy(t => t.PartID)
                .OrderByDescending(g => g.Sum(t => t.Quantity))
                .FirstOrDefault();
            analytics["TopPartId"] = topPartGroup?.Key ?? "";
            
            var topUserGroup = transactions.GroupBy(t => t.User)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
            analytics["TopUser"] = topUserGroup?.Key ?? "";
            
            return analytics;
        }

        /// <summary>
        /// Parses user input into structured search criteria
        /// </summary>
        /// <param name="input">Raw search input</param>
        /// <returns>Structured search criteria</returns>
        private SmartSearchCriteria ParseSearchInput(string input)
        {
            var criteria = new SmartSearchCriteria();
            
            // Split by spaces but preserve quoted phrases
            var terms = SplitSearchTerms(input);
            
            foreach (var term in terms)
            {
                // Check for special search operators
                if (term.StartsWith("part:", StringComparison.OrdinalIgnoreCase))
                {
                    criteria.SpecificPartId = term.Substring(5);
                }
                else if (term.StartsWith("user:", StringComparison.OrdinalIgnoreCase))
                {
                    criteria.SpecificUser = term.Substring(5);
                }
                else if (term.StartsWith("loc:", StringComparison.OrdinalIgnoreCase))
                {
                    criteria.SpecificLocation = term.Substring(4);
                }
                else if (term.StartsWith("op:", StringComparison.OrdinalIgnoreCase))
                {
                    criteria.SpecificOperation = term.Substring(3);
                }
                else if (DateTime.TryParse(term, out DateTime date))
                {
                    criteria.SpecificDate = date;
                }
                else if (int.TryParse(term, out int quantity))
                {
                    criteria.SpecificQuantity = quantity;
                }
                else
                {
                    criteria.GeneralTerms.Add(term);
                }
            }
            
            criteria.SearchTerms = terms.ToArray();
            return criteria;
        }

        /// <summary>
        /// Splits search input preserving quoted phrases
        /// </summary>
        private List<string> SplitSearchTerms(string input)
        {
            var terms = new List<string>();
            var inQuotes = false;
            var currentTerm = new StringBuilder();
            
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ' ' && !inQuotes)
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
            
            return terms.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
        }

        /// <summary>
        /// Gets currently selected transaction types from checkboxes
        /// </summary>
        private List<TransactionType> GetSelectedTransactionTypes()
        {
            var types = new List<TransactionType>();
            
            if (Transactions_CheckBox_IN.Checked) 
                types.Add(TransactionType.IN);
            if (Transactions_CheckBox_OUT.Checked) 
                types.Add(TransactionType.OUT);
            if (Transactions_CheckBox_TRANSFER.Checked) 
                types.Add(TransactionType.TRANSFER);
            
            // If none selected, return all types
            if (types.Count == 0)
            {
                types.AddRange(Enum.GetValues<TransactionType>());
            }
            
            return types;
        }

        /// <summary>
        /// Gets selected time range from radio buttons
        /// </summary>
        private (DateTime from, DateTime to) GetSelectedTimeRange()
        {
            if (Transactions_Radio_Today.Checked)
            {
                return (DateTime.Today, DateTime.Today.AddDays(1).AddTicks(-1));
            }
            
            if (Transactions_Radio_ThisWeek.Checked)
            {
                var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);
                return (startOfWeek, endOfWeek);
            }
            
            if (Transactions_Radio_ThisMonth.Checked)
            {
                var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);
                return (startOfMonth, endOfMonth);
            }
            
            // Custom range
            if (Transactions_Radio_CustomRange.Checked)
            {
                return (Control_AdvancedRemove_DateTimePicker_From.Value.Date, 
                        Control_AdvancedRemove_DateTimePicker_To.Value.Date.AddDays(1).AddTicks(-1));
            }
            
            // Default to last 30 days
            return (DateTime.Today.AddDays(-30), DateTime.Today.AddDays(1).AddTicks(-1));
        }

        /// <summary>
        /// Gets selected locations from UI controls
        /// </summary>
        private List<string> GetSelectedLocations()
        {
            var locations = new List<string>();
            
            if (Transactions_ComboBox_Building.SelectedIndex > 0)
            {
                locations.Add(Transactions_ComboBox_Building.Text);
            }
            
            return locations;
        }

        #endregion

        #region Search Performance

        /// <summary>
        /// Handles text change with debouncing
        /// </summary>
        private void OnSmartSearchTextChanged(object? sender, EventArgs e)
        {
            _searchDebounceTimer.Stop();
            
            var currentText = Transactions_TextBox_SmartSearch.Text;
            
            // Skip if text hasn't actually changed
            if (currentText == _lastSearchText)
                return;
                
            _lastSearchText = currentText;
            
            // Cancel any ongoing search
            _searchCancellation.Cancel();
            _searchCancellation = new CancellationTokenSource();
            
            // Start debounce timer
            _searchDebounceTimer.Start();
        }

        /// <summary>
        /// Performs the actual search after debounce period
        /// </summary>
        private async Task PerformDebouncedSearchAsync()
        {
            try
            {
                if (!_searchCancellation.Token.IsCancellationRequested)
                {
                    await HandleSmartSearchAsync(_lastSearchText);
                }
            }
            catch (OperationCanceledException)
            {
                // Search was cancelled, ignore
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "PerformDebouncedSearchAsync");
            }
        }

        /// <summary>
        /// Handles filter change events
        /// </summary>
        private async Task HandleFilterChangeAsync()
        {
            if (!string.IsNullOrWhiteSpace(Transactions_TextBox_SmartSearch.Text))
            {
                await HandleSmartSearchAsync(Transactions_TextBox_SmartSearch.Text);
            }
        }

        /// <summary>
        /// Handles view mode change events
        /// </summary>
        private async Task HandleViewModeChangeAsync()
        {
            try
            {
                // Update current view mode based on selected radio button
                if (Transactions_Radio_GridView.Checked)
                    _currentViewMode = TransactionViewMode.Grid;
                else if (Transactions_Radio_ChartView.Checked)
                    _currentViewMode = TransactionViewMode.Chart;
                else if (Transactions_Radio_TimelineView.Checked)
                    _currentViewMode = TransactionViewMode.Timeline;

                // Refresh the current display with the new view mode
                if (_displayedTransactions != null && _displayedTransactions.Count > 0)
                {
                    await DisplaySearchResultsAsync(_displayedTransactions.ToList());
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "HandleViewModeChangeAsync");
            }
        }

        #endregion

        #region Modern UI Styling

        /// <summary>
        /// Applies modern UI styling using the existing Core_Themes system
        /// </summary>
        private void ApplyModernStyling()
        {
            try
            {
                // Apply DPI scaling and theme using existing Core_Themes methods
                Core_Themes.ApplyDpiScaling(this);
                Core_Themes.ApplyRuntimeLayoutAdjustments(this);
                Core_Themes.ApplyTheme(this);
                
                // Apply custom modern button styling that works with themes
                ApplyModernButtonStyles();
                
                // Apply header gradient using theme colors
                ApplyHeaderGradientWithTheme();
                
                // Setup DataGridView with theme integration
                Core_Themes.ApplyThemeToDataGridView(Transactions_DataGridView_Transactions);
                ApplyCustomDataGridStyling();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "ApplyModernStyling");
            }
        }

        /// <summary>
        /// Applies modern button styling that respects current theme
        /// </summary>
        private void ApplyModernButtonStyles()
        {
            try
            {
                // Modern button configurations with theme-aware colors
                var buttonConfigs = new[]
                {
                    new { Button = Transactions_Button_Search, Style = "primary", BaseColor = Color.FromArgb(13, 110, 253) },
                    new { Button = Transactions_Button_Reset, Style = "secondary", BaseColor = Color.FromArgb(108, 117, 125) },
                    new { Button = Transfer_Button_Next, Style = "info", BaseColor = Color.FromArgb(13, 202, 240) },
                    new { Button = Transfer_Button_Previous, Style = "info", BaseColor = Color.FromArgb(13, 202, 240) },
                    new { Button = Transactions_Button_Print, Style = "success", BaseColor = Color.FromArgb(25, 135, 84) },
                    new { Button = Transactions_Button_SidePanel, Style = "warning", BaseColor = Color.FromArgb(255, 193, 7) }
                };

                foreach (var config in buttonConfigs)
                {
                    ApplyModernButtonStyle(config.Button, config.BaseColor);
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "ApplyModernButtonStyles");
            }
        }

        /// <summary>
        /// Applies modern styling to individual button with theme awareness
        /// </summary>
        private void ApplyModernButtonStyle(Button button, Color baseColor)
        {
            try
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 1;
                button.BackColor = baseColor;
                button.ForeColor = Color.White;
                button.Font = new Font("Segoe UI", 9F);
                
                // Add hover effects
                button.MouseEnter += (s, e) =>
                {
                    var hoverColor = ControlPaint.Light(baseColor, 0.2f);
                    button.BackColor = hoverColor;
                };
                
                button.MouseLeave += (s, e) =>
                {
                    button.BackColor = baseColor;
                };
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "ApplyModernButtonStyle");
            }
        }

        /// <summary>
        /// Applies header gradient using theme-aware colors
        /// </summary>
        private void ApplyHeaderGradientWithTheme()
        {
            try
            {
                // Use theme colors for header if available
                var primaryColor = Color.FromArgb(13, 110, 253);
                var gradientColor = ControlPaint.Dark(primaryColor, 0.1f);
                
                // Apply to the main GroupBox header
                Transactions_GroupBox_Main.Paint += (s, e) =>
                {
                    var headerRect = new Rectangle(0, 0, Transactions_GroupBox_Main.Width, 25);
                    using var brush = new LinearGradientBrush(
                        headerRect,
                        primaryColor,
                        gradientColor,
                        LinearGradientMode.Horizontal);
                        
                    e.Graphics.FillRectangle(brush, headerRect);
                };
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "ApplyHeaderGradientWithTheme");
            }
        }

        /// <summary>
        /// Applies custom DataGridView styling that enhances theme integration
        /// </summary>
        private void ApplyCustomDataGridStyling()
        {
            try
            {
                // Enhanced column headers
                Transactions_DataGridView_Transactions.ColumnHeadersHeight = 35;
                Transactions_DataGridView_Transactions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                
                // Row styling for transaction types
                Transactions_DataGridView_Transactions.RowPrePaint += (s, e) =>
                {
                    ApplyRowStyling();
                };
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "ApplyCustomDataGridStyling");
            }
        }

        /// <summary>
        /// Applies color coding to DataGridView rows based on transaction type
        /// </summary>
        private void ApplyRowStyling()
        {
            try
            {
                foreach (DataGridViewRow row in Transactions_DataGridView_Transactions.Rows)
                {
                    if (row.DataBoundItem is Model_Transactions transaction)
                    {
                        switch (transaction.TransactionType)
                        {
                            case TransactionType.IN:
                                row.DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 218); // Light green
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(21, 87, 36);
                                break;
                            case TransactionType.OUT:
                                row.DefaultCellStyle.BackColor = Color.FromArgb(248, 215, 218); // Light red
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(132, 32, 41);
                                break;
                            case TransactionType.TRANSFER:
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205); // Light yellow
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(133, 100, 4);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "ApplyRowStyling");
            }
        }

        #endregion

        #region Helpers

        private void SetupSortCombo()
        {
            try
            {
                Transactions_ComboBox_SortBy.Items.Clear();
                Transactions_ComboBox_SortBy.Items.Add("Date");
                Transactions_ComboBox_SortBy.Items.Add("Quantity");
                Transactions_ComboBox_SortBy.Items.Add("User");
                Transactions_ComboBox_SortBy.Items.Add("ItemType");
                Transactions_ComboBox_SortBy.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "SetupSortCombo");
            }
        }

        private void SetupDataGrid()
        {
            try
            {
                SetupDataGridColumns();
                
                Transactions_DataGridView_Transactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                Transactions_DataGridView_Transactions.ReadOnly = true;
                Transactions_DataGridView_Transactions.AllowUserToAddRows = false;
                Transactions_DataGridView_Transactions.AllowUserToDeleteRows = false;
                Transactions_DataGridView_Transactions.AllowUserToOrderColumns = false;
                Transactions_DataGridView_Transactions.AllowUserToResizeRows = false;

                Transactions_DataGridView_Transactions.DataSource = new BindingList<Model_Transactions>();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "SetupDataGrid");
            }
        }

        private void SetupDataGridColumns()
        {
            try
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
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "SetupDataGridColumns");
            }
        }

        private void SetupHistoryDataGrid()
        {
            try
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
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    controlName: "SetupHistoryDataGrid");
            }
        }

        /// <summary>
        /// Updates the results statistics panel
        /// </summary>
        private void UpdateResultsStatistics(List<Model_Transactions> transactions)
        {
            try
            {
                var totalTransactions = transactions.Count;
                var inCount = transactions.Count(t => t.TransactionType == TransactionType.IN);
                var outCount = transactions.Count(t => t.TransactionType == TransactionType.OUT);
                var transferCount = transactions.Count(t => t.TransactionType == TransactionType.TRANSFER);
                
                var summaryText = $"Found: {totalTransactions} transactions | Page {CurrentPage} of {CalculateTotalPages(totalTransactions)}";
                var statsText = $"📥 IN: {inCount} | 📤 OUT: {outCount} | 🔄 TRANSFER: {transferCount}";
                
                // Update status via the form's status strip if available
                this.Text = $"Transactions - {summaryText}";
                
                System.Diagnostics.Debug.WriteLine($"[STATS] {summaryText} - {statsText}");
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "UpdateResultsStatistics");
            }
        }

        private int CalculateTotalPages(int totalItems)
        {
            return (int)Math.Ceiling((double)totalItems / PageSize);
        }

        private void UpdatePagingButtons(int resultCount)
        {
            try
            {
                Transfer_Button_Previous.Enabled = CurrentPage > 1;
                Transfer_Button_Next.Enabled = resultCount >= PageSize;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low,
                    controlName: "UpdatePagingButtons");
            }
        }

        #endregion

        #region Smart Search Support Classes

        /// <summary>
        /// Structured search criteria parsed from user input
        /// </summary>
        private class SmartSearchCriteria
        {
            public string[] SearchTerms { get; set; } = Array.Empty<string>();
            public List<string> GeneralTerms { get; set; } = new();
            public string? SpecificPartId { get; set; }
            public string? SpecificUser { get; set; }
            public string? SpecificLocation { get; set; }
            public string? SpecificOperation { get; set; }
            public DateTime? SpecificDate { get; set; }
            public int? SpecificQuantity { get; set; }
        }

        /// <summary>
        /// View modes for transaction display
        /// </summary>
        private enum TransactionViewMode
        {
            Grid,
            Chart,
            Timeline
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Performs cleanup when the form is being disposed
        /// </summary>
        /// <param name="disposing">True if disposing managed resources</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _searchDebounceTimer?.Dispose();
                    _searchCancellation?.Dispose();
                    _progressHelper?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
