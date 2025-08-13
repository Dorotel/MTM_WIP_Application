Based on the analysis of the Transactions form redesign requirements and the existing Core_Themes.cs implementation, here are the specific methods that should be implemented for the smart search functionality and the proper theme integration approach:

## SMART SEARCH FUNCTIONALITY - SPECIFIC METHODS REQUIRED

### **1. Core Smart Search Methods**

```csharp
#region Smart Search Methods

/// <summary>
/// Handles universal smart search with debouncing and multi-field search
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
        
        var searchResult = await Dao_Transactions.SmartSearchAsync(
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
        Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
            controlName: "HandleSmartSearchAsync");
    }
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
```

### **2. Search Results Display Methods**

```csharp
#region Search Results Display

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
        UpdatePagingControls(transactions.Count);
    }
    catch (Exception ex)
    {
        Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
            controlName: "DisplaySearchResultsAsync");
    }
}

/// <summary>
/// Updates the results statistics panel
/// </summary>
private void UpdateResultsStatistics(List<Model_Transactions> transactions)
{
    var totalTransactions = transactions.Count;
    var inCount = transactions.Count(t => t.TransactionType == TransactionType.IN);
    var outCount = transactions.Count(t => t.TransactionType == TransactionType.OUT);
    var transferCount = transactions.Count(t => t.TransactionType == TransactionType.TRANSFER);
    
    var summaryText = $"Found: {totalTransactions} transactions | Page {_currentPage} of {CalculateTotalPages(totalTransactions)}";
    var statsText = $"📥 IN: {inCount} | 📤 OUT: {outCount} | 🔄 TRANSFER: {transferCount}";
    
    Transactions_Label_ResultsOverview.Text = summaryText;
    Transactions_Label_ResultsStats.Text = statsText;
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
            Transactions_DataGridView_Transactions.AutoGenerateColumns = false;
            SetupDataGridColumns();
            Transactions_DataGridView_Transactions.DataSource = _displayedTransactions;
            
            // Apply row coloring based on transaction type
            ApplyRowStyling();
            
            Transactions_Image_NothingFound.Visible = transactions.Count == 0;
            Transactions_DataGridView_Transactions.Visible = transactions.Count > 0;
        });
    });
}

/// <summary>
/// Applies color coding to DataGridView rows based on transaction type
/// </summary>
private void ApplyRowStyling()
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

#endregion
```

### **3. Search Debouncing and Performance Methods**

```csharp
#region Search Performance

private readonly Timer _searchDebounceTimer = new() { Interval = 500 };
private string _lastSearchText = string.Empty;
private CancellationTokenSource _searchCancellation = new();

/// <summary>
/// Initializes search performance optimizations
/// </summary>
private void InitializeSearchPerformance()
{
    _searchDebounceTimer.Tick += async (s, e) =>
    {
        _searchDebounceTimer.Stop();
        await PerformDebouncedSearchAsync();
    };
    
    // Cancel previous searches when starting new ones
    Transactions_TextBox_SmartSearch.TextChanged += OnSmartSearchTextChanged;
}

/// <summary>
/// Handles text change with debouncing
/// </summary>
private void OnSmartSearchTextChanged(object sender, EventArgs e)
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

#endregion
```

### **4. Support Classes and Structures**

```csharp
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
```

## PHASE 4: MODERN UI STYLING AND THEMES - PROPER IMPLEMENTATION

Instead of custom painting and styling, leverage the existing Core_Themes.cs system:

```csharp
#region Theme Integration with Core_Themes

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
    var currentTheme = Core_Themes.Core_AppThemes.GetCurrentTheme();
    var colors = currentTheme.Colors;
    
    // Modern button configurations with theme-aware colors
    var buttonConfigs = new[]
    {
        new { Button = Transactions_Button_QuickSearch, Style = "info", BaseColor = Color.FromArgb(13, 202, 240) },
        new { Button = Transactions_Button_Today, Style = "secondary", BaseColor = Color.FromArgb(108, 117, 125) },
        new { Button = Transactions_Button_ThisWeek, Style = "secondary", BaseColor = Color.FromArgb(108, 117, 125) },
        new { Button = Transactions_Button_Export, Style = "success", BaseColor = Color.FromArgb(25, 135, 84) },
        new { Button = Transactions_Button_Refresh, Style = "warning", BaseColor = Color.FromArgb(255, 193, 7) }
    };

    foreach (var config in buttonConfigs)
    {
        ApplyModernButtonStyle(config.Button, config.BaseColor, colors);
    }
}

/// <summary>
/// Applies modern styling to individual button with theme awareness
/// </summary>
private void ApplyModernButtonStyle(Button button, Color baseColor, Model_UserUiColors themeColors)
{
    // Use theme colors if available, otherwise use modern defaults
    var buttonBackColor = themeColors.ButtonBackColor ?? baseColor;
    var buttonForeColor = themeColors.ButtonForeColor ?? Color.White;
    
    button.FlatStyle = FlatStyle.Flat;
    button.FlatAppearance.BorderSize = 0;
    button.BackColor = buttonBackColor;
    button.ForeColor = buttonForeColor;
    button.Font = new Font("Segoe UI", 9F);
    button.Height = 35;
    
    // Add hover effects that work with themes
    button.MouseEnter += (s, e) =>
    {
        var hoverColor = themeColors.ButtonHoverBackColor ?? ControlPaint.Light(buttonBackColor, 0.2f);
        button.BackColor = hoverColor;
    };
    
    button.MouseLeave += (s, e) =>
    {
        button.BackColor = buttonBackColor;
    };
}

/// <summary>
/// Applies header gradient using theme-aware colors
/// </summary>
private void ApplyHeaderGradientWithTheme()
{
    var currentTheme = Core_Themes.Core_AppThemes.GetCurrentTheme();
    var colors = currentTheme.Colors;
    
    // Use theme colors for header if available
    var primaryColor = colors.FormBackColor ?? Color.FromArgb(13, 110, 253);
    var gradientColor = ControlPaint.Dark(primaryColor, 0.1f);
    
    Transactions_Panel_HeaderBar.Paint += (s, e) =>
    {
        using var brush = new LinearGradientBrush(
            Transactions_Panel_HeaderBar.ClientRectangle,
            primaryColor,
            gradientColor,
            LinearGradientMode.Horizontal);
            
        e.Graphics.FillRectangle(brush, Transactions_Panel_HeaderBar.ClientRectangle);
    };
    
    // Apply theme-aware text colors
    var headerTextColor = colors.FormForeColor ?? Color.White;
    Transactions_Label_Branding.ForeColor = headerTextColor;
    Transactions_Label_FormTitle.ForeColor = headerTextColor;
    Transactions_Label_UserInfo.ForeColor = headerTextColor;
}

/// <summary>
/// Applies custom DataGridView styling that enhances theme integration
/// </summary>
private void ApplyCustomDataGridStyling()
{
    var currentTheme = Core_Themes.Core_AppThemes.GetCurrentTheme();
    var colors = currentTheme.Colors;
    
    // Enhanced column headers
    Transactions_DataGridView_Transactions.ColumnHeadersHeight = 35;
    Transactions_DataGridView_Transactions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
    
    // Row styling for transaction types (respecting theme)
    Transactions_DataGridView_Transactions.RowPrePaint += (s, e) =>
    {
        if (e.RowIndex < Transactions_DataGridView_Transactions.Rows.Count)
        {
            var row = Transactions_DataGridView_Transactions.Rows[e.RowIndex];
            if (row.DataBoundItem is Model_Transactions transaction)
            {
                var baseBackColor = colors.DataGridRowBackColor ?? Color.White;
                
                Color rowColor = transaction.TransactionType switch
                {
                    TransactionType.IN => BlendWithTheme(Color.FromArgb(212, 237, 218), baseBackColor),
                    TransactionType.OUT => BlendWithTheme(Color.FromArgb(248, 215, 218), baseBackColor),
                    TransactionType.TRANSFER => BlendWithTheme(Color.FromArgb(255, 243, 205), baseBackColor),
                    _ => baseBackColor
                };
                
                row.DefaultCellStyle.BackColor = rowColor;
            }
        }
    };
}

/// <summary>
/// Blends transaction type colors with theme colors
/// </summary>
private Color BlendWithTheme(Color transactionColor, Color themeColor)
{
    // Simple blending - can be enhanced
    return Color.FromArgb(
        (transactionColor.R + themeColor.R) / 2,
        (transactionColor.G + themeColor.G) / 2,
        (transactionColor.B + themeColor.B) / 2
    );
}

#endregion
```

## KEY INTEGRATION POINTS

### **Constructor Integration**
```csharp
public Transactions(string connectionString, string currentUser)
{
    InitializeComponent();

    // Apply comprehensive DPI scaling and runtime layout adjustments
    AutoScaleMode = AutoScaleMode.Dpi;
    Core_Themes.ApplyDpiScaling(this);
    Core_Themes.ApplyRuntimeLayoutAdjustments(this);

    _currentUser = currentUser;
    _isAdmin = Model_AppVariables.UserTypeAdmin;

    // Initialize smart search
    InitializeSmartSearch();
    InitializeSearchPerformance();
    
    // Apply modern styling using Core_Themes
    ApplyModernStyling();
    
    SetupSortCombo();
    SetupDataGrid();

    Load += async (s, e) => await OnFormLoadAsync();
    
    // ... existing initialization code ...

    Core_Themes.ApplyTheme(this);
}
```

### **Smart Search Initialization**
```csharp
private void InitializeSmartSearch()
{
    // Setup search box with modern styling
    Transactions_TextBox_SmartSearch.PlaceholderText = "Search anything... Part ID, User, Location, Notes";
    
    // Initialize transaction type checkboxes as checked
    Transactions_CheckBox_IN.Checked = true;
    Transactions_CheckBox_OUT.Checked = true;
    Transactions_CheckBox_TRANSFER.Checked = true;
    
    // Initialize time range to "Today"
    Transactions_Radio_Today.Checked = true;
    
    // Wire up events
    Transactions_TextBox_SmartSearch.TextChanged += OnSmartSearchTextChanged;
    
    // Filter change events
    Transactions_CheckBox_IN.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
    Transactions_CheckBox_OUT.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
    Transactions_CheckBox_TRANSFER.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
    
    Transactions_Radio_Today.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
    Transactions_Radio_ThisWeek.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
    Transactions_Radio_ThisMonth.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
    Transactions_Radio_CustomRange.CheckedChanged += async (s, e) => await HandleFilterChangeAsync();
}

private async Task HandleFilterChangeAsync()
{
    if (!string.IsNullOrWhiteSpace(Transactions_TextBox_SmartSearch.Text))
    {
        await HandleSmartSearchAsync(Transactions_TextBox_SmartSearch.Text);
    }
}
```

This approach ensures that the Transactions form integrates seamlessly with the existing Core_Themes system while providing the modern UI functionality specified in the HTML template. The smart search functionality is comprehensive yet maintainable, and the styling respects the application's theming architecture.
