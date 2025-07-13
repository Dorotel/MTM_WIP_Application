using System.ComponentModel;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Forms.Transactions;

public partial class Transactions : Form
{
    #region Fields
    
    private Dao_Transactions _dao;
    private BindingList<Model_Transactions> _displayedTransactions = null!;
    private int _currentPage = 1;
    private const int _pageSize = 20;
    private const bool _sortDescending = true;
    private readonly string _currentUser;
    private readonly bool _isAdmin;
    
    #endregion
    
    #region Constructors
    
    public Transactions(string connectionString, string currentUser, bool isAdmin)
    {
        InitializeComponent();

        _dao = new Dao_Transactions(connectionString);
        _currentUser = currentUser;
        _isAdmin = isAdmin;

        Core_Themes.ApplyTheme(this);

        SetupSortCombo();
        SetupTabs();
        SetupDataGrid();

        Load += async (s, e) => await OnFormLoadAsync();

        Transactions_Button_Reset.Click += (s, e) => ResetFilters();
    }
    
    #endregion
    
    #region Methods
    
    private async Task OnFormLoadAsync()
    {
        await LoadUserCombosAsync();
        await LoadTransactionsAsync();
    }

    private async Task LoadUserCombosAsync()
    {
        await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(Transactions_ComboBox_User);
        await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(Transactions_ComboBox_UserName);
        Transactions_ComboBox_Shift.Items.Clear();
        Transactions_ComboBox_Shift.Items.Add("[ All Users ]");
        Transactions_ComboBox_Shift.Items.Add("Day");
        Transactions_ComboBox_Shift.Items.Add("Night");
        Transactions_ComboBox_Shift.SelectedIndex = 0;

        Transactions_ComboBox_User.SelectedIndex = 0;
        Transactions_ComboBox_UserName.SelectedIndex = 0;
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

    private void SetupTabs()
    {
        Transactions_TabControl_Main.SelectedIndexChanged += (s, e) =>
        {
            _currentPage = 1;
            _ = LoadTransactionsAsync();
        };
    }

    private void SetupDataGrid()
    {
        Transactions_DataGridView_Transactions.AutoGenerateColumns = false;
        Transactions_DataGridView_Transactions.Columns.Clear();

        Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
        { HeaderText = "Location", DataPropertyName = "FromLocation", Name = "colLocation" });
        Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
        { HeaderText = "PartID", DataPropertyName = "PartID", Name = "colPartID" });
        Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
        { HeaderText = "Quantity", DataPropertyName = "Quantity", Name = "colQuantity" });
        Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Date",
            DataPropertyName = "ReceiveDate",
            Name = "colDate",
            DefaultCellStyle = new DataGridViewCellStyle { Format = "g" }
        });
        Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
        { HeaderText = "User", DataPropertyName = "User", Name = "colUser" });
        Transactions_DataGridView_Transactions.Columns.Add(new DataGridViewTextBoxColumn
        { HeaderText = "ItemType", DataPropertyName = "TransactionType", Name = "colType" });

        Transactions_DataGridView_Transactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        Transactions_DataGridView_Transactions.ReadOnly = true;
        Transactions_DataGridView_Transactions.AllowUserToAddRows = false;
        Transactions_DataGridView_Transactions.AllowUserToDeleteRows = false;
        Transactions_DataGridView_Transactions.AllowUserToOrderColumns = true;
        Transactions_DataGridView_Transactions.AllowUserToResizeRows = false;

        Transactions_DataGridView_Transactions.DataSource = new BindingList<Model_Transactions>();
    }

    private void ResetFilters()
    {
        Transactions_ComboBox_SortBy.SelectedIndex = 0;
        Transactions_TextBox_SearchPartID.Text = string.Empty;
        Transactions_TabControl_Main.SelectedIndex = 0;
        Transactions_ComboBox_User.SelectedIndex = 0;
        Transactions_ComboBox_UserName.SelectedIndex = 0;
        Transactions_ComboBox_Shift.SelectedIndex = 0;
        _currentPage = 1;
        _ = LoadTransactionsAsync();
    }

    private async Task LoadTransactionsAsync()
    {
        var sortBy = Transactions_ComboBox_SortBy.SelectedItem?.ToString() ?? "Date";
        var searchPartID = Transactions_TextBox_SearchPartID.Text.Trim();

        var user = Transactions_ComboBox_User.SelectedItem?.ToString() ?? string.Empty;
        var userName = Transactions_ComboBox_UserName.SelectedItem?.ToString() ?? string.Empty;
        var shift = Transactions_ComboBox_Shift.SelectedItem?.ToString() ?? string.Empty;

        var partEntryType = Transactions_TabControl_Main.SelectedTab?.Text ?? string.Empty;

        var result = await Task.Run(() => _dao.SearchTransactions(
            user,
            _isAdmin,
            string.IsNullOrWhiteSpace(searchPartID) ? string.Empty : searchPartID,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            null,
            null,
            string.Empty,
            string.Empty,
            null,
            null,
            sortBy,
            _sortDescending,
            _currentPage,
            _pageSize
        ));

        _displayedTransactions = new BindingList<Model_Transactions>(result);
        Transactions_DataGridView_Transactions.DataSource = _displayedTransactions;
    }
    
    #endregion
}