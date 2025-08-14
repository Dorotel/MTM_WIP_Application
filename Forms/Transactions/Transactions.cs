using System.ComponentModel;
using System.Data;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
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
        private readonly Dictionary<string, string> _lastSearchCriteria = new();
        private bool _isPaginationNavigation = false;

        #endregion

        #region Properties
        // Properties will be added here as needed
        #endregion

        #region Progress Control Methods
        // Progress control methods will be added here as needed
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

            SetupFormInitialization();
            WireUpEventHandlers();
            Core_Themes.ApplyTheme(this);
        }

        #endregion

        #region Form Initialization

        private void SetupFormInitialization()
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["User"] = _currentUser,
                ["IsAdmin"] = _isAdmin
            }, nameof(Transactions), nameof(SetupFormInitialization));

            SetupSortCombo();
            SetupDataGrid();
            SetupFilterControls();
            InitializeSmartSearchControls();

            Load += async (s, e) => await OnFormLoadAsync();

            Service_DebugTracer.TraceMethodExit(null, nameof(Transactions), nameof(SetupFormInitialization));
        }

        private async Task OnFormLoadAsync()
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["FormName"] = "Transactions"
            }, nameof(Transactions), nameof(OnFormLoadAsync));

            try
            {
                Transactions_Button_Help.Enabled = false;
                await LoadUserCombosAsync();
                await LoadPartComboAsync();
                InitializeFiltersToDefaults();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, null, null, nameof(Transactions), nameof(OnFormLoadAsync));
            }
            finally
            {
                Service_DebugTracer.TraceMethodExit(null, nameof(Transactions), nameof(OnFormLoadAsync));
            }
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

        private async Task LoadPartComboAsync()
        {
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(_transactionsComboBoxSearchPartId);
            _transactionsComboBoxSearchPartId.SelectedIndex = 0;
        }

        private void WireUpEventHandlers()
        {
            Service_DebugTracer.TraceMethodEntry(null, nameof(Transactions), nameof(WireUpEventHandlers));

            // Smart search functionality
            Transactions_Button_SmartSearch.Click += async (s, e) => await HandleSmartSearchAsync();
            
            // Smart search Enter key support
            Transactions_TextBox_SmartSearch.KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    await HandleSmartSearchAsync();
                    e.Handled = true;
                }
            };

            // Pagination controls
            Transactions_Button_NextPage.Click += async (s, e) => await NavigateToNextPageAsync();
            Transactions_Button_PreviousPage.Click += async (s, e) => await NavigateToPreviousPageAsync();
            Transactions_Button_FirstPage.Click += async (s, e) => await NavigateToFirstPageAsync();
            Transactions_Button_LastPage.Click += async (s, e) => await NavigateToLastPageAsync();

            // Filter controls
            Transactions_Button_ApplyFilters.Click += async (s, e) => await ApplyFiltersAsync();
            Transactions_Button_ClearFilters.Click += (s, e) => ClearAllFilters();
            Transactions_Button_Reset.Click += (s, e) => ResetFilters();

            // Data grid events
            Transactions_DataGridView_Transactions.SelectionChanged += Transactions_DataGridView_Transactions_SelectionChanged;
            Transactions_Button_Help.Click += Transactions_Button_Help_Click;

            // Toolbar buttons
            Transactions_Button_QuickSearch.Click += async (s, e) => await HandleQuickSearchAsync();
            Transactions_Button_Today.Click += async (s, e) => await FilterTodayAsync();
            Transactions_Button_ThisWeek.Click += async (s, e) => await FilterThisWeekAsync();
            Transactions_Button_Export.Click += (s, e) => ExportResults();
            Transactions_Button_Refresh.Click += async (s, e) => await RefreshDataAsync();

            Service_DebugTracer.TraceMethodExit(null, nameof(Transactions), nameof(WireUpEventHandlers));
        }

        #endregion

        #region Smart Search Functionality

        /// <summary>
        /// Handle smart search functionality with advanced syntax
        /// </summary>
        private async Task HandleSmartSearchAsync()
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["SearchText"] = Transactions_TextBox_SmartSearch.Text ?? ""
            }, nameof(Transactions), nameof(HandleSmartSearchAsync));

            try
            {
                string searchText = Transactions_TextBox_SmartSearch.Text?.Trim() ?? "";

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
                await ExecuteSmartSearchAsync(searchCriteria);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, null, null, nameof(Transactions), nameof(HandleSmartSearchAsync));
                UpdateSearchStatus($"Search error: {ex.Message}");
            }
            finally
            {
                Service_DebugTracer.TraceMethodExit(null, nameof(Transactions), nameof(HandleSmartSearchAsync));
            }
        }

        /// <summary>
        /// Parse smart search text with advanced syntax support
        /// Supports: partid:PART123, qty:>50, location:A1, user:JSMITH, batch:0000123
        /// </summary>
        private Dictionary<string, string> ParseSmartSearchText(string searchText)
        {
            var criteria = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(searchText)) return criteria;

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

            return criteria;
        }

        /// <summary>
        /// Split search terms handling quoted strings properly
        /// </summary>
        private List<string> SplitSearchTerms(string searchText)
        {
            var terms = new List<string>();
            var currentTerm = new System.Text.StringBuilder();
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
                _ => field
            };
        }

        /// <summary>
        /// Execute smart search with parsed criteria using DAO
        /// </summary>
        private async Task ExecuteSmartSearchAsync(Dictionary<string, string> criteria)
        {
            try
            {
                _lastSearchCriteria.Clear();
                foreach (var kvp in criteria)
                {
                    _lastSearchCriteria[kvp.Key] = kvp.Value;
                }

                var dao = new Dao_Transactions();
                
                // Map criteria to DAO parameters
                string partID = criteria.ContainsKey("PartID") ? criteria["PartID"] : "";
                string batchNumber = criteria.ContainsKey("BatchNumber") ? criteria["BatchNumber"] : "";
                string operation = criteria.ContainsKey("Operation") ? criteria["Operation"] : "";
                string user = criteria.ContainsKey("User") ? criteria["User"] : "";
                string fromLocation = criteria.ContainsKey("FromLocation") ? criteria["FromLocation"] : "";
                string toLocation = criteria.ContainsKey("ToLocation") ? criteria["ToLocation"] : "";
                string notes = criteria.ContainsKey("Notes") ? criteria["Notes"] : "";
                string itemType = criteria.ContainsKey("ItemType") ? criteria["ItemType"] : "";

                int? quantity = null;
                if (criteria.ContainsKey("Quantity") && int.TryParse(criteria["Quantity"], out int qty))
                {
                    quantity = qty;
                }

                var result = await dao.SearchTransactionsAsync(
                    _isAdmin ? string.Empty : _currentUser,
                    _isAdmin,
                    partID: partID,
                    batchNumber: batchNumber,
                    fromLocation: fromLocation,
                    toLocation: toLocation,
                    operation: operation,
                    quantity: quantity,
                    notes: notes,
                    itemType: itemType,
                    sortColumn: "ReceiveDate",
                    sortDescending: SortDescending,
                    page: _currentPage,
                    pageSize: PageSize
                );

                if (result.IsSuccess && result.Data != null)
                {
                    DisplaySearchResults(result.Data);
                    UpdateSearchStatus($"Found {result.Data.Count} results");
                }
                else
                {
                    UpdateSearchStatus($"Search failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, null, null, nameof(Transactions), nameof(ExecuteSmartSearchAsync));
                UpdateSearchStatus($"Search error: {ex.Message}");
            }
        }

        #endregion

        #region Button Clicks

        private async Task NavigateToNextPageAsync()
        {
            _currentPage++;
            _isPaginationNavigation = true;
            
            if (_lastSearchCriteria.Count > 0)
            {
                await ExecuteSmartSearchAsync(_lastSearchCriteria);
            }
            else
            {
                await LoadTransactionsAsync();
            }
        }

        private async Task NavigateToPreviousPageAsync()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                _isPaginationNavigation = true;
                
                if (_lastSearchCriteria.Count > 0)
                {
                    await ExecuteSmartSearchAsync(_lastSearchCriteria);
                }
                else
                {
                    await LoadTransactionsAsync();
                }
            }
        }

        private async Task NavigateToFirstPageAsync()
        {
            _currentPage = 1;
            _isPaginationNavigation = true;
            
            if (_lastSearchCriteria.Count > 0)
            {
                await ExecuteSmartSearchAsync(_lastSearchCriteria);
            }
            else
            {
                await LoadTransactionsAsync();
            }
        }

        private async Task NavigateToLastPageAsync()
        {
            // For now, just navigate to a high page number
            // In a full implementation, you'd calculate the actual last page
            _currentPage = 999;
            _isPaginationNavigation = true;
            
            if (_lastSearchCriteria.Count > 0)
            {
                await ExecuteSmartSearchAsync(_lastSearchCriteria);
            }
            else
            {
                await LoadTransactionsAsync();
            }
        }

        private async Task ApplyFiltersAsync()
        {
            try
            {
                // Reset to first page when applying filters
                _currentPage = 1;
                _isPaginationNavigation = false;

                // Build criteria from filter controls
                var criteria = BuildFilterCriteria();
                await ExecuteSmartSearchAsync(criteria);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(ApplyFiltersAsync));
            }
        }

        private async Task HandleQuickSearchAsync()
        {
            try
            {
                // Quick search using traditional controls
                await LoadTransactionsAsync();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(HandleQuickSearchAsync));
            }
        }

        private async Task FilterTodayAsync()
        {
            try
            {
                Transactions_RadioButton_Today.Checked = true;
                await ApplyFiltersAsync();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(FilterTodayAsync));
            }
        }

        private async Task FilterThisWeekAsync()
        {
            try
            {
                Transactions_RadioButton_Week.Checked = true;
                await ApplyFiltersAsync();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(FilterThisWeekAsync));
            }
        }

        private void ExportResults()
        {
            try
            {
                if (_displayedTransactions == null || _displayedTransactions.Count == 0)
                {
                    Service_ErrorHandler.HandleException(
                        new Exception("No data to export"),
                        ErrorSeverity.Low, null, null, nameof(Transactions), nameof(ExportResults));
                    return;
                }

                // Export functionality would be implemented here
                // For now, just show a message
                Service_ErrorHandler.HandleException(
                    new NotImplementedException("Export functionality not yet implemented"),
                    ErrorSeverity.Low, null, null, nameof(Transactions), nameof(ExportResults));
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(ExportResults));
            }
        }

        private async Task RefreshDataAsync()
        {
            try
            {
                // Reset page and reload current view
                _currentPage = 1;
                if (_lastSearchCriteria.Count > 0)
                {
                    await ExecuteSmartSearchAsync(_lastSearchCriteria);
                }
                else
                {
                    await LoadTransactionsAsync();
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(RefreshDataAsync));
            }
        }

        private async void Transactions_Button_Help_Click(object? sender, EventArgs e)
        {
            try
            {
                if (Transactions_DataGridView_Transactions.SelectedRows.Count != 1)
                    return;

                if (Transactions_DataGridView_Transactions.SelectedRows[0].DataBoundItem is not Model_Transactions selected ||
                    string.IsNullOrWhiteSpace(selected.BatchNumber))
                {
                    Service_ErrorHandler.HandleException(
                        new Exception("No Batch Number found for the selected transaction."),
                        ErrorSeverity.Low, null, null, nameof(Transactions), nameof(Transactions_Button_Help_Click));
                    return;
                }

                // Load batch history
                await LoadBatchHistoryAsync(selected.BatchNumber);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(Transactions_Button_Help_Click));
            }
        }

        #endregion

        #region ComboBox & UI Events

        private void Transactions_DataGridView_Transactions_SelectionChanged(object? sender, EventArgs e)
        {
            try
            {
                if (Transactions_DataGridView_Transactions.SelectedRows.Count == 1)
                {
                    DataGridViewRow row = Transactions_DataGridView_Transactions.SelectedRows[0];
                    if (row.DataBoundItem is Model_Transactions tx)
                    {
                        Transactions_Button_Help.Enabled = true;
                        UpdateTransactionDetails(tx);

                        Service_DebugTracer.TraceUIAction("TRANSACTION_SELECTED", nameof(Transactions),
                            new Dictionary<string, object>
                            {
                                ["TransactionID"] = tx.ID,
                                ["PartID"] = tx.PartID ?? "",
                                ["BatchNumber"] = tx.BatchNumber ?? ""
                            });
                    }
                }
                else
                {                
                    Transactions_Button_Help.Enabled = false;
                    ClearTransactionDetails();
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(Transactions_DataGridView_Transactions_SelectionChanged));
            }
        }

        private void UpdateTransactionDetails(Model_Transactions transaction)
        {
            try
            {
                // Update the right panel details if controls exist
                if (Transactions_TextBox_BatchNumber != null)
                    Transactions_TextBox_BatchNumber.Text = transaction.BatchNumber ?? "";
                
                if (Transactions_TextBox_TransactionType != null)
                    Transactions_TextBox_TransactionType.Text = transaction.TransactionType.ToString();
                
                if (Transactions_TextBox_Quantity != null)
                    Transactions_TextBox_Quantity.Text = transaction.Quantity.ToString();
                
                if (Transactions_TextBox_Location != null)
                    Transactions_TextBox_Location.Text = $"{transaction.FromLocation} → {transaction.ToLocation}";
                
                if (Transactions_TextBox_User != null)
                    Transactions_TextBox_User.Text = transaction.User ?? "";
                
                if (Transactions_TextBox_DateTime != null)
                    Transactions_TextBox_DateTime.Text = transaction.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                
                if (Transactions_Label_SelectedPartID != null)
                    Transactions_Label_SelectedPartID.Text = $"Selected: {transaction.PartID}";
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, null, null, nameof(Transactions), nameof(UpdateTransactionDetails));
            }
        }

        private void ClearTransactionDetails()
        {
            try
            {
                // Clear the right panel details if controls exist
                if (Transactions_TextBox_BatchNumber != null)
                    Transactions_TextBox_BatchNumber.Text = "";
                
                if (Transactions_TextBox_TransactionType != null)
                    Transactions_TextBox_TransactionType.Text = "";
                
                if (Transactions_TextBox_Quantity != null)
                    Transactions_TextBox_Quantity.Text = "";
                
                if (Transactions_TextBox_Location != null)
                    Transactions_TextBox_Location.Text = "";
                
                if (Transactions_TextBox_User != null)
                    Transactions_TextBox_User.Text = "";
                
                if (Transactions_TextBox_DateTime != null)
                    Transactions_TextBox_DateTime.Text = "";
                
                if (Transactions_Label_SelectedPartID != null)
                    Transactions_Label_SelectedPartID.Text = "No selection";
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, null, null, nameof(Transactions), nameof(ClearTransactionDetails));
            }
        }

        #endregion

        #region Helpers

        private async Task LoadTransactionsAsync()
        {
            try
            {
                Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
                {
                    ["Page"] = _currentPage,
                    ["PageSize"] = PageSize
                }, nameof(Transactions), nameof(LoadTransactionsAsync));

                var dao = new Dao_Transactions();
                
                // Get search parameters from traditional controls
                string part = _transactionsComboBoxSearchPartId.Text;
                string user = Transactions_ComboBox_UserFullName.Text;

                bool partSelected = _transactionsComboBoxSearchPartId.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(part);
                bool userSelected = Transactions_ComboBox_UserFullName.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(user);

                if (!partSelected && !userSelected)
                {
                    Service_ErrorHandler.HandleException(
                        new Exception("Please fill in at least one field to search."),
                        ErrorSeverity.Low, null, null, nameof(Transactions), nameof(LoadTransactionsAsync));
                    return;
                }

                var result = await dao.SearchTransactionsAsync(
                    _isAdmin ? string.Empty : _currentUser,
                    _isAdmin,
                    partID: partSelected ? part : "",
                    batchNumber: "",
                    fromLocation: "",
                    toLocation: "",
                    operation: "",
                    quantity: null,
                    notes: "",
                    itemType: "",
                    fromDate: null,
                    toDate: null,
                    sortColumn: Transactions_ComboBox_SortBy.Text ?? "Date",
                    sortDescending: SortDescending,
                    page: _currentPage,
                    pageSize: PageSize
                );

                if (result.IsSuccess && result.Data != null)
                {
                    DisplaySearchResults(result.Data);
                    UpdateSearchStatus($"Loaded {result.Data.Count} transactions");
                }
                else
                {
                    UpdateSearchStatus($"Load failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, null, null, nameof(Transactions), nameof(LoadTransactionsAsync));
            }
            finally
            {
                Service_DebugTracer.TraceMethodExit(null, nameof(Transactions), nameof(LoadTransactionsAsync));
            }
        }

        private void DisplaySearchResults(List<Model_Transactions> results)
        {
            try
            {
                _displayedTransactions = new BindingList<Model_Transactions>(results);
                Transactions_DataGridView_Transactions.DataSource = _displayedTransactions;
                
                // Update visibility of no results image
                Transactions_Image_NothingFound.Visible = results.Count == 0;
                Transactions_DataGridView_Transactions.Visible = results.Count > 0;
                Transactions_Button_Help.Enabled = results.Count > 0;
                
                if (results.Count > 0)
                {
                    Transactions_DataGridView_Transactions.ClearSelection();
                }

                UpdatePagingButtons(results.Count);
                UpdateResultsSummary(results.Count);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(DisplaySearchResults));
            }
        }

        private void UpdatePagingButtons(int resultCount)
        {
            try
            {
                Transactions_Button_PreviousPage.Enabled = _currentPage > 1;
                Transactions_Button_FirstPage.Enabled = _currentPage > 1;
                Transactions_Button_NextPage.Enabled = resultCount >= PageSize;
                Transactions_Button_LastPage.Enabled = resultCount >= PageSize;

                // Update page info label if it exists
                if (Transactions_Label_PageInfo != null)
                {
                    Transactions_Label_PageInfo.Text = $"Page {_currentPage} ({resultCount} items)";
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, null, null, nameof(Transactions), nameof(UpdatePagingButtons));
            }
        }

        private void UpdateResultsSummary(int resultCount)
        {
            try
            {
                if (Transactions_Label_ResultsOverview != null)
                {
                    Transactions_Label_ResultsOverview.Text = $"Showing {resultCount} transactions";
                }

                if (Transactions_Label_ResultsStats != null)
                {
                    Transactions_Label_ResultsStats.Text = $"Page {_currentPage} • {PageSize} per page";
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, null, null, nameof(Transactions), nameof(UpdateResultsSummary));
            }
        }

        private void UpdateSearchStatus(string message)
        {
            try
            {
                // Update status in the database status label if it exists
                if (Transactions_Label_DatabaseStatus != null)
                {
                    Transactions_Label_DatabaseStatus.Text = message;
                    Transactions_Label_DatabaseStatus.ForeColor = message.Contains("error") || message.Contains("failed") 
                        ? Color.Red : Color.Green;
                }

                Service_DebugTracer.TraceUIAction("SEARCH_STATUS_UPDATED", nameof(Transactions),
                    new Dictionary<string, object> { ["Message"] = message });
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, null, null, nameof(Transactions), nameof(UpdateSearchStatus));
            }
        }

        private async Task LoadBatchHistoryAsync(string batchNumber)
        {
            try
            {
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

                if (searchResult.IsSuccess && searchResult.Data != null)
                {
                    DisplayBatchHistory(searchResult.Data);
                }
                else
                {
                    Service_ErrorHandler.HandleException(
                        new Exception($"Failed to load batch history: {searchResult.ErrorMessage}"),
                        ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(LoadBatchHistoryAsync));
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(LoadBatchHistoryAsync));
            }
        }

        private void DisplayBatchHistory(List<Model_Transactions> results)
        {
            try
            {
                // Create enhanced batch history with descriptions
                List<dynamic> describedResults = [];
                for (int i = 0; i < results.Count; i++)
                {
                    Model_Transactions curr = results[i];
                    string desc = GenerateTransactionDescription(curr, i, results);

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

                // Update DataGrid with batch history
                Transactions_DataGridView_Transactions.DataSource = new BindingList<dynamic>(describedResults);
                Transactions_Image_NothingFound.Visible = describedResults.Count == 0;
                Transactions_DataGridView_Transactions.Visible = describedResults.Count > 0;
                Transactions_Button_Help.Enabled = describedResults.Count > 0;

                if (describedResults.Count > 0)
                {
                    Transactions_DataGridView_Transactions.ClearSelection();
                }

                UpdateSearchStatus($"Batch history: {describedResults.Count} transactions");
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(DisplayBatchHistory));
            }
        }

        private string GenerateTransactionDescription(Model_Transactions transaction, int index, List<Model_Transactions> allTransactions)
        {
            if (index == allTransactions.Count - 1) // last row (oldest)
            {
                return "Initial Transaction";
            }
            
            return transaction.TransactionType switch
            {
                TransactionType.OUT => "Removed From System",
                TransactionType.IN => "Received Into System",
                TransactionType.TRANSFER => $"Transferred from {transaction.FromLocation ?? "Unknown"} to {transaction.ToLocation ?? "Unknown"}",
                _ => "Transaction"
            };
        }

        private Dictionary<string, string> BuildFilterCriteria()
        {
            var criteria = new Dictionary<string, string>();

            try
            {
                // Build criteria from time range filters
                if (Transactions_RadioButton_Today?.Checked == true)
                {
                    criteria["date"] = DateTime.Today.ToString("yyyy-MM-dd");
                }
                else if (Transactions_RadioButton_Week?.Checked == true)
                {
                    criteria["date"] = ">=" + DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                }
                else if (Transactions_RadioButton_Month?.Checked == true)
                {
                    criteria["date"] = ">=" + DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                }

                // Add location filters
                if (Transactions_CheckBox_ExpoLocation?.Checked == true)
                {
                    criteria["location"] = "Expo";
                }
                if (Transactions_CheckBox_VitsLocation?.Checked == true)
                {
                    criteria["location"] = criteria.ContainsKey("location") ? criteria["location"] + ",Vits" : "Vits";
                }

                // Add transaction type filters
                var types = new List<string>();
                if (Transactions_CheckBox_TypeReceive?.Checked == true) types.Add("IN");
                if (Transactions_CheckBox_TypeRemove?.Checked == true) types.Add("OUT");
                if (Transactions_CheckBox_TypeTransfer?.Checked == true) types.Add("TRANSFER");
                
                if (types.Count > 0)
                {
                    criteria["type"] = string.Join(",", types);
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, null, null, nameof(Transactions), nameof(BuildFilterCriteria));
            }

            return criteria;
        }

        private void ResetFilters()
        {
            try
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
                
                // Reset smart search functionality
                Transactions_TextBox_SmartSearch.Text = "";
                
                // Clear smart search criteria
                _lastSearchCriteria.Clear();
                _isPaginationNavigation = false;
                _currentPage = 1;
                
                // Clear data grid
                Transactions_DataGridView_Transactions.DataSource = null;
                
                // Reset status
                UpdateSearchStatus("Filters reset - ready for new search");

                Service_DebugTracer.TraceUIAction("RESET_FILTERS_COMPLETED", nameof(Transactions), null);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(ResetFilters));
            }
        }

        private void ClearAllFilters()
        {
            try
            {
                // Clear all filter checkboxes and radio buttons
                if (Transactions_RadioButton_Today != null) Transactions_RadioButton_Today.Checked = false;
                if (Transactions_RadioButton_Week != null) Transactions_RadioButton_Week.Checked = false;
                if (Transactions_RadioButton_Month != null) Transactions_RadioButton_Month.Checked = false;
                if (Transactions_RadioButton_CustomRange != null) Transactions_RadioButton_CustomRange.Checked = false;

                if (Transactions_CheckBox_ExpoLocation != null) Transactions_CheckBox_ExpoLocation.Checked = false;
                if (Transactions_CheckBox_VitsLocation != null) Transactions_CheckBox_VitsLocation.Checked = false;

                if (Transactions_CheckBox_TypeReceive != null) Transactions_CheckBox_TypeReceive.Checked = false;
                if (Transactions_CheckBox_TypeRemove != null) Transactions_CheckBox_TypeRemove.Checked = false;
                if (Transactions_CheckBox_TypeTransfer != null) Transactions_CheckBox_TypeTransfer.Checked = false;

                // Also clear smart search and traditional filters
                ResetFilters();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(ClearAllFilters));
            }
        }

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
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, null, null, nameof(Transactions), nameof(SetupSortCombo));
            }
        }

        private void SetupDataGrid()
        {
            try
            {
                Transactions_DataGridView_Transactions.AutoGenerateColumns = false;
                Transactions_DataGridView_Transactions.Columns.Clear();

                // Add standard columns
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

                // Configure grid properties
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
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(SetupDataGrid));
            }
        }

        private void SetupFilterControls()
        {
            try
            {
                // Initialize filter controls to default states
                InitializeFiltersToDefaults();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, null, null, nameof(Transactions), nameof(SetupFilterControls));
            }
        }

        private void InitializeFiltersToDefaults()
        {
            try
            {
                // Set default filter states
                if (Transactions_RadioButton_Today != null) Transactions_RadioButton_Today.Checked = false;
                if (Transactions_RadioButton_Week != null) Transactions_RadioButton_Week.Checked = false;
                if (Transactions_RadioButton_Month != null) Transactions_RadioButton_Month.Checked = false;
                if (Transactions_RadioButton_CustomRange != null) Transactions_RadioButton_CustomRange.Checked = false;

                if (Transactions_CheckBox_ExpoLocation != null) Transactions_CheckBox_ExpoLocation.Checked = true;
                if (Transactions_CheckBox_VitsLocation != null) Transactions_CheckBox_VitsLocation.Checked = true;

                if (Transactions_CheckBox_TypeReceive != null) Transactions_CheckBox_TypeReceive.Checked = true;
                if (Transactions_CheckBox_TypeRemove != null) Transactions_CheckBox_TypeRemove.Checked = true;
                if (Transactions_CheckBox_TypeTransfer != null) Transactions_CheckBox_TypeTransfer.Checked = true;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, null, null, nameof(Transactions), nameof(InitializeFiltersToDefaults));
            }
        }

        private void InitializeSmartSearchControls()
        {
            try
            {
                // Smart search controls are already initialized in the designer
                // Just ensure proper placeholder text
                if (Transactions_TextBox_SmartSearch != null)
                {
                    Transactions_TextBox_SmartSearch.PlaceholderText = "partid:ABC123 qty:>50 user:john";
                }

                LoggingUtility.LogApplicationInfo("Smart search controls initialized successfully");
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, null, null, nameof(Transactions), nameof(InitializeSmartSearchControls));
            }
        }

        #endregion

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _displayedTransactions?.Clear();
                _lastSearchCriteria?.Clear();
                _transactionsComboBoxSearchPartId?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}