using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Core;

namespace MTM_Inventory_Application.Forms.Transactions
{
    public partial class Transactions : Form
    {
        private Dao_Transactions _dao;
        private BindingList<Model_Transactions> _displayedTransactions;
        private int _currentPage = 1;
        private int _pageSize = 20;
        private bool _sortDescending = true;
        private string _sortColumn = "Date";
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

            this.Load += async (s, e) => await OnFormLoadAsync();

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
            comboSortBy.Items.Add("Type");
            comboSortBy.SelectedIndex = 0;
        }

        private void SetupTabs()
        {
            // If you need specific tab logic, handle SelectedIndexChanged here
            tabControlMain.SelectedIndexChanged += (s, e) => { _currentPage = 1; _ = LoadTransactionsAsync(); };
        }

        private void SetupDataGrid()
        {
            dataGridTransactions.AutoGenerateColumns = false;
            dataGridTransactions.Columns.Clear();

            dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Location", DataPropertyName = "FromLocation", Name = "colLocation" });
            dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "PartID", DataPropertyName = "PartID", Name = "colPartID" });
            dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Quantity", DataPropertyName = "Quantity", Name = "colQuantity" });
            dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Date", DataPropertyName = "DateTime", Name = "colDate", DefaultCellStyle = new DataGridViewCellStyle { Format = "g" } });
            dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "User", DataPropertyName = "User", Name = "colUser" });
            dataGridTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Type", DataPropertyName = "TransactionType", Name = "colType" });

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
            string sortBy = comboSortBy.SelectedItem?.ToString() ?? "Date";
            string searchPartID = txtSearchPartID.Text.Trim();

            string user = comboUser.SelectedItem?.ToString();
            string userName = comboUserName.SelectedItem?.ToString();
            string shift = comboShift.SelectedItem?.ToString();

            string partEntryType = tabControlMain.SelectedTab?.Text;

            var result = await Task.Run(() => _dao.SearchTransactions(
                userName: user,
                isAdmin: _isAdmin,
                partID: string.IsNullOrWhiteSpace(searchPartID) ? null : searchPartID,
                batchNumber: null,
                fromLocation: null,
                toLocation: null,
                operation: null,
                transactionType: null,
                quantity: null,
                notes: null,
                itemType: null,
                fromDate: null,
                toDate: null,
                sortColumn: sortBy,
                sortDescending: true,
                page: _currentPage,
                pageSize: _pageSize
            ));

            _displayedTransactions = new BindingList<Model_Transactions>(result);
            dataGridTransactions.DataSource = _displayedTransactions;
            // Paging label logic can be added here if needed
        }
    }
}