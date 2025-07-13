// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Forms.Transactions;

public partial class Transactions : Form
{
    private Dao_Transactions _dao;
    private BindingList<Model_Transactions> _displayedTransactions = null!;
    private int _currentPage = 1;
    private const int _pageSize = 20;
    private const bool _sortDescending = true;
    private readonly string _currentUser;
    private readonly bool _isAdmin;

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

        btnReset.Click += (s, e) => ResetFilters();
    }

    private async Task OnFormLoadAsync()
    {
        await LoadUserCombosAsync();
        await LoadTransactionsAsync();
    }

    private async Task LoadUserCombosAsync()
    {
        // User combos (for admin only)
        await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(comboUser);
        await Helper_UI_ComboBoxes.FillUserComboBoxesAsync(comboUserName);
        comboShift.Items.Clear();
        comboShift.Items.Add("[ All Users ]");
        comboShift.Items.Add("Day");
        comboShift.Items.Add("Night");
        comboShift.SelectedIndex = 0;

        comboUser.SelectedIndex = 0;
        comboUserName.SelectedIndex = 0;
    }

    private void SetupSortCombo()
    {
        comboSortBy.Items.Clear();
        comboSortBy.Items.Add("Date");
        comboSortBy.Items.Add("Quantity");
        comboSortBy.Items.Add("User");
        comboSortBy.Items.Add("ItemType");
        comboSortBy.SelectedIndex = 0;
    }

    private void SetupTabs()
    {
        // If you need specific tab logic, handle SelectedIndexChanged here
        tabControlMain.SelectedIndexChanged += (s, e) =>
        {
            _currentPage = 1;
            _ = LoadTransactionsAsync();
        };
    }

    private void SetupDataGrid()
    {
        dataGridTransactions.AutoGenerateColumns = false;
        dataGridTransactions.Columns.Clear();

        dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn
        { HeaderText = "Location", DataPropertyName = "FromLocation", Name = "colLocation" });
        dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn
        { HeaderText = "PartID", DataPropertyName = "PartID", Name = "colPartID" });
        dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn
        { HeaderText = "Quantity", DataPropertyName = "Quantity", Name = "colQuantity" });
        dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Date",
            DataPropertyName = "ReceiveDate",
            Name = "colDate",
            DefaultCellStyle = new DataGridViewCellStyle { Format = "g" }
        });
        dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn
        { HeaderText = "User", DataPropertyName = "User", Name = "colUser" });
        dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn
        { HeaderText = "ItemType", DataPropertyName = "TransactionType", Name = "colType" });

        dataGridTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridTransactions.ReadOnly = true;
        dataGridTransactions.AllowUserToAddRows = false;
        dataGridTransactions.AllowUserToDeleteRows = false;
        dataGridTransactions.AllowUserToOrderColumns = true;
        dataGridTransactions.AllowUserToResizeRows = false;

        dataGridTransactions.DataSource = new BindingList<Model_Transactions>();
    }

    private void ResetFilters()
    {
        comboSortBy.SelectedIndex = 0;
        txtSearchPartID.Text = string.Empty;
        tabControlMain.SelectedIndex = 0;
        comboUser.SelectedIndex = 0;
        comboUserName.SelectedIndex = 0;
        comboShift.SelectedIndex = 0;
        _currentPage = 1;
        _ = LoadTransactionsAsync();
    }

    private async Task LoadTransactionsAsync()
    {
        // Gather filter values
        var sortBy = comboSortBy.SelectedItem?.ToString() ?? "Date";
        var searchPartID = txtSearchPartID.Text.Trim();

        var user = comboUser.SelectedItem?.ToString() ?? string.Empty;
        var userName = comboUserName.SelectedItem?.ToString() ?? string.Empty;
        var shift = comboShift.SelectedItem?.ToString() ?? string.Empty;

        var partEntryType = tabControlMain.SelectedTab?.Text ?? string.Empty;

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
            _sortDescending, // Use the _sortDescending field here
            _currentPage,
            _pageSize
        ));

        _displayedTransactions = new BindingList<Model_Transactions>(result);
        dataGridTransactions.DataSource = _displayedTransactions;
        // Paging label logic can be added here if needed
    }
}