using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Forms.Transactions
{
    partial class Transactions : Form
    {
        #region Fields

        private System.ComponentModel.IContainer components = null;

        // Top-level containers
        private GroupBox Transactions_GroupBox_Main;
        private TableLayoutPanel Transactions_TableLayout_Main;

        // Header Bar Components
        private Panel Transactions_Panel_Header;
        private Label Transactions_Label_HeaderTitle;
        private Label Transactions_Label_UserStatus;
        private PictureBox Transactions_PictureBox_ConnectionStatus;

        // Toolbar Components
        private Panel Transactions_Panel_Toolbar;
        private Button Transactions_Button_QuickSearch;
        private Button Transactions_Button_Today;
        private Button Transactions_Button_ThisWeek;
        private Button Transactions_Button_Export;
        private Button Transactions_Button_Refresh;

        // 3-Panel Layout Container
        private TableLayoutPanel Transactions_TableLayout_ThreePanels;

        // LEFT PANEL - Smart Search & Filters
        private Panel Transactions_Panel_FiltersContainer;
        private Label Transactions_Label_FiltersHeader;
        private Panel Transactions_Panel_SmartSearch;
        private Label Transactions_Label_SmartSearch;
        private TextBox Transactions_TextBox_SmartSearch;
        private Button Transactions_Button_SmartSearch;
        private Label Transactions_Label_SmartSearchHelp;
        
        // Filter sections
        private Panel Transactions_Panel_TimeRangeFilter;
        private Label Transactions_Label_TimeRange;
        private RadioButton Transactions_RadioButton_Today;
        private RadioButton Transactions_RadioButton_Week;
        private RadioButton Transactions_RadioButton_Month;
        private RadioButton Transactions_RadioButton_CustomRange;
        
        private Panel Transactions_Panel_LocationFilter;
        private Label Transactions_Label_LocationFilter;
        private CheckBox Transactions_CheckBox_ExpoLocation;
        private CheckBox Transactions_CheckBox_VitsLocation;
        
        private Panel Transactions_Panel_TransactionTypesFilter;
        private Label Transactions_Label_TransactionTypes;
        private CheckBox Transactions_CheckBox_TypeReceive;
        private CheckBox Transactions_CheckBox_TypeRemove;
        private CheckBox Transactions_CheckBox_TypeTransfer;
        
        private Panel Transactions_Panel_FilterActions;
        private Button Transactions_Button_ApplyFilters;
        private Button Transactions_Button_ClearFilters;
        private Button Transactions_Button_SaveFilters;

        // MIDDLE PANEL - Results Grid & Controls
        private Panel Transactions_Panel_ResultsContainer;
        private Panel Transactions_Panel_ResultsSummary;
        private Label Transactions_Label_ResultsOverview;
        private Label Transactions_Label_ResultsStats;
        private Panel Transactions_Panel_ViewOptions;
        private Button Transactions_Button_GridView;
        private Button Transactions_Button_ChartView;
        private Button Transactions_Button_Timeline;
        
        // Data Grid Container
        private Panel Transactions_Panel_DataGridView;
        private DataGridView Transactions_DataGridView_Transactions;
        private PictureBox Transactions_Image_NothingFound;
        
        // Pagination Controls
        private Panel Transactions_Panel_Pagination;
        private Button Transactions_Button_FirstPage;
        private Button Transactions_Button_PreviousPage;
        private Label Transactions_Label_PageInfo;
        private Button Transactions_Button_NextPage;
        private Button Transactions_Button_LastPage;

        // RIGHT PANEL - Transaction Details & Analytics
        private Panel Transactions_Panel_DetailsContainer;
        private Label Transactions_Label_DetailsHeader;
        private Panel Transactions_Panel_TransactionDetails;
        private Label Transactions_Label_SelectedPartID;
        private Panel Transactions_Panel_DetailFields;
        private Label Transactions_Label_BatchNumber;
        private TextBox Transactions_TextBox_BatchNumber;
        private Label Transactions_Label_TransactionType;
        private TextBox Transactions_TextBox_TransactionType;
        private Label Transactions_Label_Quantity;
        private TextBox Transactions_TextBox_Quantity;
        private Label Transactions_Label_Location;
        private TextBox Transactions_TextBox_Location;
        private Label Transactions_Label_User;
        private TextBox Transactions_TextBox_User;
        private Label Transactions_Label_DateTime;
        private TextBox Transactions_TextBox_DateTime;
        
        // Analytics Dashboard
        private Panel Transactions_Panel_AnalyticsHeader;
        private Label Transactions_Label_AnalyticsHeader;
        private Panel Transactions_Panel_Analytics;
        private Label Transactions_Label_DailyActivity;
        private Panel Transactions_Panel_ChartPlaceholder;
        private Label Transactions_Label_TopOperations;
        private Label Transactions_Label_OperationStats;
        private Panel Transactions_Panel_SystemPerformance;
        private Label Transactions_Label_SystemStatus;

        // Bottom toolbar
        private Panel Transactions_Panel_Footer;
        private Panel Transactions_Panel_FooterLeft;
        private Button Transactions_Button_SelectAll;
        private Button Transactions_Button_BulkExport;
        private Button Transactions_Button_BatchActions;
        private Panel Transactions_Panel_FooterCenter;
        private Label Transactions_Label_DatabaseStatus;
        private Panel Transactions_Panel_FooterRight;
        private Button Transactions_Button_Settings;
        private Button Transactions_Button_Help;
        private Button Transactions_Button_Support;

        // Legacy controls for compatibility
        private Panel Transactions_Panel_Row_SortBy;
        private Label Transactions_Label_SortBy;
        private ComboBox Transactions_ComboBox_SortBy;
        private ComboBox Transactions_ComboBox_UserFullName;
        private Button Transactions_Button_Reset;

        #endregion

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // Initialize main containers
            Transactions_GroupBox_Main = new GroupBox();
            Transactions_TableLayout_Main = new TableLayoutPanel();
            
            // Initialize header components
            Transactions_Panel_Header = new Panel();
            Transactions_Label_HeaderTitle = new Label();
            Transactions_Label_UserStatus = new Label();
            Transactions_PictureBox_ConnectionStatus = new PictureBox();
            
            // Initialize toolbar components
            Transactions_Panel_Toolbar = new Panel();
            Transactions_Button_QuickSearch = new Button();
            Transactions_Button_Today = new Button();
            Transactions_Button_ThisWeek = new Button();
            Transactions_Button_Export = new Button();
            Transactions_Button_Refresh = new Button();
            
            // Initialize 3-panel layout
            Transactions_TableLayout_ThreePanels = new TableLayoutPanel();
            
            // LEFT PANEL - Filters
            Transactions_Panel_FiltersContainer = new Panel();
            Transactions_Label_FiltersHeader = new Label();
            Transactions_Panel_SmartSearch = new Panel();
            Transactions_Label_SmartSearch = new Label();
            Transactions_TextBox_SmartSearch = new TextBox();
            Transactions_Button_SmartSearch = new Button();
            Transactions_Label_SmartSearchHelp = new Label();
            
            // Filter sections
            Transactions_Panel_TimeRangeFilter = new Panel();
            Transactions_Label_TimeRange = new Label();
            Transactions_RadioButton_Today = new RadioButton();
            Transactions_RadioButton_Week = new RadioButton();
            Transactions_RadioButton_Month = new RadioButton();
            Transactions_RadioButton_CustomRange = new RadioButton();
            
            Transactions_Panel_LocationFilter = new Panel();
            Transactions_Label_LocationFilter = new Label();
            Transactions_CheckBox_ExpoLocation = new CheckBox();
            Transactions_CheckBox_VitsLocation = new CheckBox();
            
            Transactions_Panel_TransactionTypesFilter = new Panel();
            Transactions_Label_TransactionTypes = new Label();
            Transactions_CheckBox_TypeReceive = new CheckBox();
            Transactions_CheckBox_TypeRemove = new CheckBox();
            Transactions_CheckBox_TypeTransfer = new CheckBox();
            
            Transactions_Panel_FilterActions = new Panel();
            Transactions_Button_ApplyFilters = new Button();
            Transactions_Button_ClearFilters = new Button();
            Transactions_Button_SaveFilters = new Button();
            
            // MIDDLE PANEL - Results
            Transactions_Panel_ResultsContainer = new Panel();
            Transactions_Panel_ResultsSummary = new Panel();
            Transactions_Label_ResultsOverview = new Label();
            Transactions_Label_ResultsStats = new Label();
            Transactions_Panel_ViewOptions = new Panel();
            Transactions_Button_GridView = new Button();
            Transactions_Button_ChartView = new Button();
            Transactions_Button_Timeline = new Button();
            
            // Data Grid
            Transactions_Panel_DataGridView = new Panel();
            Transactions_DataGridView_Transactions = new DataGridView();
            Transactions_Image_NothingFound = new PictureBox();
            
            // Pagination
            Transactions_Panel_Pagination = new Panel();
            Transactions_Button_FirstPage = new Button();
            Transactions_Button_PreviousPage = new Button();
            Transactions_Label_PageInfo = new Label();
            Transactions_Button_NextPage = new Button();
            Transactions_Button_LastPage = new Button();
            
            // RIGHT PANEL - Details & Analytics
            Transactions_Panel_DetailsContainer = new Panel();
            Transactions_Label_DetailsHeader = new Label();
            Transactions_Panel_TransactionDetails = new Panel();
            Transactions_Label_SelectedPartID = new Label();
            Transactions_Panel_DetailFields = new Panel();
            Transactions_Label_BatchNumber = new Label();
            Transactions_TextBox_BatchNumber = new TextBox();
            Transactions_Label_TransactionType = new Label();
            Transactions_TextBox_TransactionType = new TextBox();
            Transactions_Label_Quantity = new Label();
            Transactions_TextBox_Quantity = new TextBox();
            Transactions_Label_Location = new Label();
            Transactions_TextBox_Location = new TextBox();
            Transactions_Label_User = new Label();
            Transactions_TextBox_User = new TextBox();
            Transactions_Label_DateTime = new Label();
            Transactions_TextBox_DateTime = new TextBox();
            
            // Analytics
            Transactions_Panel_AnalyticsHeader = new Panel();
            Transactions_Label_AnalyticsHeader = new Label();
            Transactions_Panel_Analytics = new Panel();
            Transactions_Label_DailyActivity = new Label();
            Transactions_Panel_ChartPlaceholder = new Panel();
            Transactions_Label_TopOperations = new Label();
            Transactions_Label_OperationStats = new Label();
            Transactions_Panel_SystemPerformance = new Panel();
            Transactions_Label_SystemStatus = new Label();
            
            // Footer
            Transactions_Panel_Footer = new Panel();
            Transactions_Panel_FooterLeft = new Panel();
            Transactions_Button_SelectAll = new Button();
            Transactions_Button_BulkExport = new Button();
            Transactions_Button_BatchActions = new Button();
            Transactions_Panel_FooterCenter = new Panel();
            Transactions_Label_DatabaseStatus = new Label();
            Transactions_Panel_FooterRight = new Panel();
            Transactions_Button_Settings = new Button();
            Transactions_Button_Help = new Button();
            Transactions_Button_Support = new Button();
            
            // Legacy compatibility controls
            Transactions_Panel_Row_SortBy = new Panel();
            Transactions_Label_SortBy = new Label();
            Transactions_ComboBox_SortBy = new ComboBox();
            Transactions_ComboBox_UserFullName = new ComboBox();
            Transactions_Button_Reset = new Button();

            // Suspend layouts
            Transactions_GroupBox_Main.SuspendLayout();
            Transactions_TableLayout_Main.SuspendLayout();
            Transactions_Panel_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Transactions_PictureBox_ConnectionStatus).BeginInit();
            Transactions_Panel_Toolbar.SuspendLayout();
            Transactions_TableLayout_ThreePanels.SuspendLayout();
            Transactions_Panel_FiltersContainer.SuspendLayout();
            Transactions_Panel_SmartSearch.SuspendLayout();
            Transactions_Panel_ResultsContainer.SuspendLayout();
            Transactions_Panel_DataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Transactions_DataGridView_Transactions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Transactions_Image_NothingFound).BeginInit();
            Transactions_Panel_DetailsContainer.SuspendLayout();
            Transactions_Panel_Footer.SuspendLayout();
            SuspendLayout();

            // 
            // Transactions_GroupBox_Main
            // 
            Transactions_GroupBox_Main.Controls.Add(Transactions_TableLayout_Main);
            Transactions_GroupBox_Main.Dock = DockStyle.Fill;
            Transactions_GroupBox_Main.Location = new Point(0, 0);
            Transactions_GroupBox_Main.Name = "Transactions_GroupBox_Main";
            Transactions_GroupBox_Main.Size = new Size(1200, 700);
            Transactions_GroupBox_Main.TabIndex = 0;
            Transactions_GroupBox_Main.TabStop = false;
            Transactions_GroupBox_Main.Text = "📊 Transaction History & Analytics";

            // 
            // Transactions_TableLayout_Main
            // 
            Transactions_TableLayout_Main.ColumnCount = 1;
            Transactions_TableLayout_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Transactions_TableLayout_Main.Controls.Add(Transactions_Panel_Header, 0, 0);
            Transactions_TableLayout_Main.Controls.Add(Transactions_Panel_Toolbar, 0, 1);
            Transactions_TableLayout_Main.Controls.Add(Transactions_TableLayout_ThreePanels, 0, 2);
            Transactions_TableLayout_Main.Controls.Add(Transactions_Panel_Footer, 0, 3);
            Transactions_TableLayout_Main.Dock = DockStyle.Fill;
            Transactions_TableLayout_Main.Location = new Point(3, 19);
            Transactions_TableLayout_Main.Name = "Transactions_TableLayout_Main";
            Transactions_TableLayout_Main.RowCount = 4;
            Transactions_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Transactions_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Transactions_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Transactions_TableLayout_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Transactions_TableLayout_Main.Size = new Size(1194, 678);
            Transactions_TableLayout_Main.TabIndex = 0;

            // 
            // Transactions_Panel_Header
            // 
            Transactions_Panel_Header.BackColor = Color.FromArgb(13, 110, 253);
            Transactions_Panel_Header.Controls.Add(Transactions_Label_HeaderTitle);
            Transactions_Panel_Header.Controls.Add(Transactions_Label_UserStatus);
            Transactions_Panel_Header.Controls.Add(Transactions_PictureBox_ConnectionStatus);
            Transactions_Panel_Header.Dock = DockStyle.Fill;
            Transactions_Panel_Header.Location = new Point(3, 3);
            Transactions_Panel_Header.Name = "Transactions_Panel_Header";
            Transactions_Panel_Header.Size = new Size(1188, 34);
            Transactions_Panel_Header.TabIndex = 0;

            // 
            // Transactions_Label_HeaderTitle
            // 
            Transactions_Label_HeaderTitle.AutoSize = true;
            Transactions_Label_HeaderTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            Transactions_Label_HeaderTitle.ForeColor = Color.White;
            Transactions_Label_HeaderTitle.Location = new Point(12, 8);
            Transactions_Label_HeaderTitle.Name = "Transactions_Label_HeaderTitle";
            Transactions_Label_HeaderTitle.Size = new Size(235, 19);
            Transactions_Label_HeaderTitle.TabIndex = 0;
            Transactions_Label_HeaderTitle.Text = "📊 Transaction History & Analytics";

            // 
            // Transactions_Panel_Toolbar
            // 
            Transactions_Panel_Toolbar.BackColor = Color.FromArgb(233, 236, 239);
            Transactions_Panel_Toolbar.Controls.Add(Transactions_Button_QuickSearch);
            Transactions_Panel_Toolbar.Controls.Add(Transactions_Button_Today);
            Transactions_Panel_Toolbar.Controls.Add(Transactions_Button_ThisWeek);
            Transactions_Panel_Toolbar.Controls.Add(Transactions_Button_Export);
            Transactions_Panel_Toolbar.Controls.Add(Transactions_Button_Refresh);
            Transactions_Panel_Toolbar.Dock = DockStyle.Fill;
            Transactions_Panel_Toolbar.Location = new Point(3, 43);
            Transactions_Panel_Toolbar.Name = "Transactions_Panel_Toolbar";
            Transactions_Panel_Toolbar.Size = new Size(1188, 34);
            Transactions_Panel_Toolbar.TabIndex = 1;

            // 
            // Transactions_TableLayout_ThreePanels
            // 
            Transactions_TableLayout_ThreePanels.ColumnCount = 3;
            Transactions_TableLayout_ThreePanels.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            Transactions_TableLayout_ThreePanels.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Transactions_TableLayout_ThreePanels.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320F));
            Transactions_TableLayout_ThreePanels.Controls.Add(Transactions_Panel_FiltersContainer, 0, 0);
            Transactions_TableLayout_ThreePanels.Controls.Add(Transactions_Panel_ResultsContainer, 1, 0);
            Transactions_TableLayout_ThreePanels.Controls.Add(Transactions_Panel_DetailsContainer, 2, 0);
            Transactions_TableLayout_ThreePanels.Dock = DockStyle.Fill;
            Transactions_TableLayout_ThreePanels.Location = new Point(3, 83);
            Transactions_TableLayout_ThreePanels.Name = "Transactions_TableLayout_ThreePanels";
            Transactions_TableLayout_ThreePanels.RowCount = 1;
            Transactions_TableLayout_ThreePanels.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Transactions_TableLayout_ThreePanels.Size = new Size(1188, 552);
            Transactions_TableLayout_ThreePanels.TabIndex = 2;

            // 
            // LEFT PANEL - Filters Container
            // 
            Transactions_Panel_FiltersContainer.BackColor = Color.White;
            Transactions_Panel_FiltersContainer.BorderStyle = BorderStyle.FixedSingle;
            Transactions_Panel_FiltersContainer.Controls.Add(Transactions_Label_FiltersHeader);
            Transactions_Panel_FiltersContainer.Controls.Add(Transactions_Panel_SmartSearch);
            Transactions_Panel_FiltersContainer.Controls.Add(Transactions_Panel_TimeRangeFilter);
            Transactions_Panel_FiltersContainer.Controls.Add(Transactions_Panel_LocationFilter);
            Transactions_Panel_FiltersContainer.Controls.Add(Transactions_Panel_TransactionTypesFilter);
            Transactions_Panel_FiltersContainer.Controls.Add(Transactions_Panel_FilterActions);
            Transactions_Panel_FiltersContainer.Dock = DockStyle.Fill;
            Transactions_Panel_FiltersContainer.Location = new Point(3, 3);
            Transactions_Panel_FiltersContainer.Name = "Transactions_Panel_FiltersContainer";
            Transactions_Panel_FiltersContainer.Size = new Size(294, 546);
            Transactions_Panel_FiltersContainer.TabIndex = 0;

            // 
            // Transactions_Label_FiltersHeader
            // 
            Transactions_Label_FiltersHeader.BackColor = Color.FromArgb(248, 249, 250);
            Transactions_Label_FiltersHeader.Dock = DockStyle.Top;
            Transactions_Label_FiltersHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            Transactions_Label_FiltersHeader.Location = new Point(0, 0);
            Transactions_Label_FiltersHeader.Name = "Transactions_Label_FiltersHeader";
            Transactions_Label_FiltersHeader.Size = new Size(292, 32);
            Transactions_Label_FiltersHeader.TabIndex = 0;
            Transactions_Label_FiltersHeader.Text = "🎯 Smart Search";
            Transactions_Label_FiltersHeader.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // Smart Search Panel
            // 
            Transactions_Panel_SmartSearch.Controls.Add(Transactions_Label_SmartSearch);
            Transactions_Panel_SmartSearch.Controls.Add(Transactions_TextBox_SmartSearch);
            Transactions_Panel_SmartSearch.Controls.Add(Transactions_Button_SmartSearch);
            Transactions_Panel_SmartSearch.Controls.Add(Transactions_Label_SmartSearchHelp);
            Transactions_Panel_SmartSearch.Dock = DockStyle.Top;
            Transactions_Panel_SmartSearch.Location = new Point(0, 32);
            Transactions_Panel_SmartSearch.Name = "Transactions_Panel_SmartSearch";
            Transactions_Panel_SmartSearch.Size = new Size(292, 120);
            Transactions_Panel_SmartSearch.TabIndex = 1;

            // 
            // Transactions_TextBox_SmartSearch
            // 
            Transactions_TextBox_SmartSearch.BorderStyle = BorderStyle.FixedSingle;
            Transactions_TextBox_SmartSearch.Font = new Font("Segoe UI", 10F);
            Transactions_TextBox_SmartSearch.Location = new Point(12, 35);
            Transactions_TextBox_SmartSearch.Name = "Transactions_TextBox_SmartSearch";
            Transactions_TextBox_SmartSearch.PlaceholderText = "partid:ABC123 qty:>50 user:john";
            Transactions_TextBox_SmartSearch.Size = new Size(268, 25);
            Transactions_TextBox_SmartSearch.TabIndex = 0;

            // 
            // MIDDLE PANEL - Results Container
            // 
            Transactions_Panel_ResultsContainer.BackColor = Color.White;
            Transactions_Panel_ResultsContainer.BorderStyle = BorderStyle.FixedSingle;
            Transactions_Panel_ResultsContainer.Controls.Add(Transactions_Panel_ResultsSummary);
            Transactions_Panel_ResultsContainer.Controls.Add(Transactions_Panel_DataGridView);
            Transactions_Panel_ResultsContainer.Controls.Add(Transactions_Panel_Pagination);
            Transactions_Panel_ResultsContainer.Dock = DockStyle.Fill;
            Transactions_Panel_ResultsContainer.Location = new Point(303, 3);
            Transactions_Panel_ResultsContainer.Name = "Transactions_Panel_ResultsContainer";
            Transactions_Panel_ResultsContainer.Size = new Size(562, 546);
            Transactions_Panel_ResultsContainer.TabIndex = 1;

            // 
            // Results Summary Panel
            // 
            Transactions_Panel_ResultsSummary.BackColor = Color.FromArgb(227, 242, 253);
            Transactions_Panel_ResultsSummary.BorderStyle = BorderStyle.FixedSingle;
            Transactions_Panel_ResultsSummary.Controls.Add(Transactions_Label_ResultsOverview);
            Transactions_Panel_ResultsSummary.Controls.Add(Transactions_Label_ResultsStats);
            Transactions_Panel_ResultsSummary.Controls.Add(Transactions_Panel_ViewOptions);
            Transactions_Panel_ResultsSummary.Dock = DockStyle.Top;
            Transactions_Panel_ResultsSummary.Location = new Point(0, 0);
            Transactions_Panel_ResultsSummary.Name = "Transactions_Panel_ResultsSummary";
            Transactions_Panel_ResultsSummary.Size = new Size(560, 80);
            Transactions_Panel_ResultsSummary.TabIndex = 0;

            // 
            // Data Grid View Panel
            // 
            Transactions_Panel_DataGridView.Controls.Add(Transactions_DataGridView_Transactions);
            Transactions_Panel_DataGridView.Controls.Add(Transactions_Image_NothingFound);
            Transactions_Panel_DataGridView.Dock = DockStyle.Fill;
            Transactions_Panel_DataGridView.Location = new Point(0, 80);
            Transactions_Panel_DataGridView.Name = "Transactions_Panel_DataGridView";
            Transactions_Panel_DataGridView.Size = new Size(560, 426);
            Transactions_Panel_DataGridView.TabIndex = 1;

            // 
            // Transactions_DataGridView_Transactions
            // 
            Transactions_DataGridView_Transactions.AllowUserToAddRows = false;
            Transactions_DataGridView_Transactions.AllowUserToDeleteRows = false;
            Transactions_DataGridView_Transactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Transactions_DataGridView_Transactions.BackgroundColor = Color.White;
            Transactions_DataGridView_Transactions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Transactions_DataGridView_Transactions.Dock = DockStyle.Fill;
            Transactions_DataGridView_Transactions.Location = new Point(0, 0);
            Transactions_DataGridView_Transactions.MultiSelect = false;
            Transactions_DataGridView_Transactions.Name = "Transactions_DataGridView_Transactions";
            Transactions_DataGridView_Transactions.ReadOnly = true;
            Transactions_DataGridView_Transactions.RowHeadersVisible = false;
            Transactions_DataGridView_Transactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Transactions_DataGridView_Transactions.Size = new Size(560, 426);
            Transactions_DataGridView_Transactions.TabIndex = 0;

            // 
            // Transactions_Image_NothingFound
            // 
            Transactions_Image_NothingFound.BackColor = Color.White;
            Transactions_Image_NothingFound.Dock = DockStyle.Fill;
            Transactions_Image_NothingFound.Location = new Point(0, 0);
            Transactions_Image_NothingFound.Name = "Transactions_Image_NothingFound";
            Transactions_Image_NothingFound.Size = new Size(560, 426);
            Transactions_Image_NothingFound.SizeMode = PictureBoxSizeMode.CenterImage;
            Transactions_Image_NothingFound.TabIndex = 1;
            Transactions_Image_NothingFound.TabStop = false;
            Transactions_Image_NothingFound.Visible = false;

            // 
            // Pagination Panel
            // 
            Transactions_Panel_Pagination.BackColor = Color.FromArgb(248, 249, 250);
            Transactions_Panel_Pagination.BorderStyle = BorderStyle.FixedSingle;
            Transactions_Panel_Pagination.Controls.Add(Transactions_Button_FirstPage);
            Transactions_Panel_Pagination.Controls.Add(Transactions_Button_PreviousPage);
            Transactions_Panel_Pagination.Controls.Add(Transactions_Label_PageInfo);
            Transactions_Panel_Pagination.Controls.Add(Transactions_Button_NextPage);
            Transactions_Panel_Pagination.Controls.Add(Transactions_Button_LastPage);
            Transactions_Panel_Pagination.Dock = DockStyle.Bottom;
            Transactions_Panel_Pagination.Location = new Point(0, 506);
            Transactions_Panel_Pagination.Name = "Transactions_Panel_Pagination";
            Transactions_Panel_Pagination.Size = new Size(560, 38);
            Transactions_Panel_Pagination.TabIndex = 2;

            // 
            // RIGHT PANEL - Details Container
            // 
            Transactions_Panel_DetailsContainer.BackColor = Color.White;
            Transactions_Panel_DetailsContainer.BorderStyle = BorderStyle.FixedSingle;
            Transactions_Panel_DetailsContainer.Controls.Add(Transactions_Label_DetailsHeader);
            Transactions_Panel_DetailsContainer.Controls.Add(Transactions_Panel_TransactionDetails);
            Transactions_Panel_DetailsContainer.Controls.Add(Transactions_Panel_AnalyticsHeader);
            Transactions_Panel_DetailsContainer.Controls.Add(Transactions_Panel_Analytics);
            Transactions_Panel_DetailsContainer.Dock = DockStyle.Fill;
            Transactions_Panel_DetailsContainer.Location = new Point(871, 3);
            Transactions_Panel_DetailsContainer.Name = "Transactions_Panel_DetailsContainer";
            Transactions_Panel_DetailsContainer.Size = new Size(314, 546);
            Transactions_Panel_DetailsContainer.TabIndex = 2;

            // 
            // Transactions_Panel_Footer
            // 
            Transactions_Panel_Footer.BackColor = Color.FromArgb(248, 249, 250);
            Transactions_Panel_Footer.BorderStyle = BorderStyle.FixedSingle;
            Transactions_Panel_Footer.Controls.Add(Transactions_Panel_FooterLeft);
            Transactions_Panel_Footer.Controls.Add(Transactions_Panel_FooterCenter);
            Transactions_Panel_Footer.Controls.Add(Transactions_Panel_FooterRight);
            Transactions_Panel_Footer.Dock = DockStyle.Fill;
            Transactions_Panel_Footer.Location = new Point(3, 641);
            Transactions_Panel_Footer.Name = "Transactions_Panel_Footer";
            Transactions_Panel_Footer.Size = new Size(1188, 34);
            Transactions_Panel_Footer.TabIndex = 3;

            // Legacy compatibility controls - Hidden but accessible
            Transactions_Panel_Row_SortBy.Visible = false;
            Transactions_ComboBox_SortBy.Visible = false;
            Transactions_ComboBox_UserFullName.Visible = false;
            Transactions_Button_Reset.Visible = false;

            // Resume layouts
            Transactions_GroupBox_Main.ResumeLayout(false);
            Transactions_TableLayout_Main.ResumeLayout(false);
            Transactions_Panel_Header.ResumeLayout(false);
            Transactions_Panel_Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Transactions_PictureBox_ConnectionStatus).EndInit();
            Transactions_Panel_Toolbar.ResumeLayout(false);
            Transactions_TableLayout_ThreePanels.ResumeLayout(false);
            Transactions_Panel_FiltersContainer.ResumeLayout(false);
            Transactions_Panel_SmartSearch.ResumeLayout(false);
            Transactions_Panel_SmartSearch.PerformLayout();
            Transactions_Panel_ResultsContainer.ResumeLayout(false);
            Transactions_Panel_DataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Transactions_DataGridView_Transactions).EndInit();
            ((System.ComponentModel.ISupportInitialize)Transactions_Image_NothingFound).EndInit();
            Transactions_Panel_DetailsContainer.ResumeLayout(false);
            Transactions_Panel_Footer.ResumeLayout(false);
            ResumeLayout(false);

            // Form properties
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(Transactions_GroupBox_Main);
            this.Name = "Transactions";
            this.Text = "📊 MTM Inventory - Transaction History";
            this.MinimumSize = new Size(1000, 600);
        }

        #endregion
    }
}