using System.Data;
using System.Text;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Forms.Development
{
    #region Application Analyzer Form

    /// <summary>
    /// Comprehensive application analysis tool for MTM WIP Application
    /// Provides detailed analysis for MAUI migration planning
    /// </summary>
    public partial class ApplicationAnalyzerForm : Form
    {
        #region Fields

        private Model_ApplicationAnalysis? _currentAnalysis;
        private Helper_StoredProcedureProgress? _progressHelper;
        private Service_ApplicationAnalyzer? _analyzer;
        private Service_AnalysisFormatter? _formatter;
        private string _applicationRootPath;

        #endregion

        #region Properties

        public Model_ApplicationAnalysis? CurrentAnalysis => _currentAnalysis;

        #endregion

        #region Progress Control Methods

        public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, this);
        }

        #endregion

        #region Constructors

        public ApplicationAnalyzerForm()
        {
            LoggingUtility.LogInfo("[ApplicationAnalyzerForm] Initializing Application Analyzer", nameof(ApplicationAnalyzerForm));

            InitializeComponent();

            // Apply theme and DPI scaling
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

            // Initialize progress control
            SetProgressControls(toolStripProgressBar1, toolStripStatusLabel1);

            // Set application root path
            _applicationRootPath = Application.StartupPath;
            
            // Find the actual source root (go up from bin folder)
            var binIndex = _applicationRootPath.LastIndexOf("bin", StringComparison.OrdinalIgnoreCase);
            if (binIndex > 0)
            {
                _applicationRootPath = _applicationRootPath.Substring(0, binIndex).TrimEnd('\\', '/');
            }

            // Initialize analyzer
            _analyzer = new Service_ApplicationAnalyzer(_applicationRootPath, _progressHelper);

            InitializeUI();
            
            LoggingUtility.LogInfo($"[ApplicationAnalyzerForm] Initialized with root path: {_applicationRootPath}", nameof(ApplicationAnalyzerForm));
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the UI components and data grids
        /// </summary>
        private void InitializeUI()
        {
            try
            {
                // Configure data grids
                ConfigureDataGridViews();

                // Set initial UI state
                UpdateUIState(false);

                LoggingUtility.LogInfo("[ApplicationAnalyzerForm] UI initialization completed", nameof(InitializeUI));
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] UI initialization failed: {ex.Message}", nameof(InitializeUI), ex);
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    contextData: new Dictionary<string, object> { ["Method"] = nameof(InitializeUI) },
                    controlName: nameof(ApplicationAnalyzerForm));
            }
        }

        /// <summary>
        /// Configures the data grid views
        /// </summary>
        private void ConfigureDataGridViews()
        {
            // Configure UI Details grid
            dataGridView_UIDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_UIDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_UIDetails.MultiSelect = false;

            // Configure Database Details grid
            dataGridView_DatabaseDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_DatabaseDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_DatabaseDetails.MultiSelect = false;

            // Configure Business Details grid
            dataGridView_BusinessDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_BusinessDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_BusinessDetails.MultiSelect = false;
        }

        #endregion

        #region Analysis Methods

        /// <summary>
        /// Runs comprehensive analysis of the application
        /// </summary>
        private async void RunFullAnalysisAsync()
        {
            try
            {
                LoggingUtility.LogInfo("[ApplicationAnalyzerForm] Starting comprehensive analysis", nameof(RunFullAnalysisAsync));

                UpdateUIState(true);
                _progressHelper?.ShowProgress("Starting comprehensive application analysis...");

                if (_analyzer == null)
                {
                    Service_ErrorHandler.ShowValidationError("Analyzer not initialized", "Analysis Error");
                    return;
                }

                var result = await _analyzer.AnalyzeApplicationAsync();

                if (result.IsSuccess)
                {
                    _currentAnalysis = result.Data;
                    _formatter = new Service_AnalysisFormatter(_currentAnalysis!);

                    await PopulateAnalysisResultsAsync();
                    
                    _progressHelper?.ShowSuccess("Analysis completed successfully!");
                    LoggingUtility.LogInfo("[ApplicationAnalyzerForm] Analysis completed successfully", nameof(RunFullAnalysisAsync));
                }
                else
                {
                    _progressHelper?.ShowError($"Analysis failed: {result.ErrorMessage}");
                    Service_ErrorHandler.ShowValidationError($"Analysis failed: {result.ErrorMessage}", "Analysis Error");
                    LoggingUtility.LogError($"[ApplicationAnalyzerForm] Analysis failed: {result.ErrorMessage}", nameof(RunFullAnalysisAsync));
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Exception during analysis: {ex.Message}", nameof(RunFullAnalysisAsync), ex);
                _progressHelper?.ShowError($"Analysis failed: {ex.Message}");
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High,
                    contextData: new Dictionary<string, object> { ["Method"] = nameof(RunFullAnalysisAsync) },
                    controlName: nameof(ApplicationAnalyzerForm));
            }
            finally
            {
                UpdateUIState(false);
            }
        }

        /// <summary>
        /// Populates the UI with analysis results
        /// </summary>
        private async Task PopulateAnalysisResultsAsync()
        {
            try
            {
                if (_currentAnalysis == null || _formatter == null)
                    return;

                _progressHelper?.UpdateProgress("Populating analysis results...", 90);

                // Populate Summary tab
                await PopulateSummaryTabAsync();

                // Populate UI Analysis tab
                PopulateUIAnalysisTab();

                // Populate Database Analysis tab
                PopulateDatabaseAnalysisTab();

                // Populate Business Logic tab
                PopulateBusinessLogicTab();

                // Populate other tabs
                await PopulateOtherTabsAsync();

                // Populate Raw Data tab
                await PopulateRawDataTabAsync();

                LoggingUtility.LogInfo("[ApplicationAnalyzerForm] Analysis results populated", nameof(PopulateAnalysisResultsAsync));
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Failed to populate results: {ex.Message}", nameof(PopulateAnalysisResultsAsync), ex);
                throw;
            }
        }

        /// <summary>
        /// Populates the summary tab
        /// </summary>
        private async Task PopulateSummaryTabAsync()
        {
            try
            {
                var summaryResult = await _formatter!.ExportSummaryAsync();
                if (summaryResult.IsSuccess)
                {
                    richTextBox_Summary.Text = summaryResult.Data;
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Failed to populate summary: {ex.Message}", nameof(PopulateSummaryTabAsync), ex);
                richTextBox_Summary.Text = $"Failed to generate summary: {ex.Message}";
            }
        }

        /// <summary>
        /// Populates the UI analysis tab
        /// </summary>
        private void PopulateUIAnalysisTab()
        {
            try
            {
                // Clear existing data
                treeView_UIStructure.Nodes.Clear();
                dataGridView_UIDetails.DataSource = null;

                // Add root nodes
                var formsNode = treeView_UIStructure.Nodes.Add("Forms");
                var controlsNode = treeView_UIStructure.Nodes.Add("Controls");

                // Add forms
                foreach (var form in _currentAnalysis!.UIAnalysis.Forms)
                {
                    var formNode = formsNode.Nodes.Add(form.Name);
                    formNode.Tag = form;
                }

                // Add controls
                foreach (var control in _currentAnalysis.UIAnalysis.Controls)
                {
                    var controlNode = controlsNode.Nodes.Add(control.Name);
                    controlNode.Tag = control;
                }

                // Expand nodes
                formsNode.Expand();
                controlsNode.Expand();

                LoggingUtility.LogInfo($"[ApplicationAnalyzerForm] UI analysis populated - Forms: {_currentAnalysis.UIAnalysis.Forms.Count}, Controls: {_currentAnalysis.UIAnalysis.Controls.Count}", nameof(PopulateUIAnalysisTab));
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Failed to populate UI analysis: {ex.Message}", nameof(PopulateUIAnalysisTab), ex);
            }
        }

        /// <summary>
        /// Populates the database analysis tab
        /// </summary>
        private void PopulateDatabaseAnalysisTab()
        {
            try
            {
                // Clear existing data
                treeView_DatabaseStructure.Nodes.Clear();
                dataGridView_DatabaseDetails.DataSource = null;

                // Add root nodes
                var tablesNode = treeView_DatabaseStructure.Nodes.Add("Tables");
                var storedProcsNode = treeView_DatabaseStructure.Nodes.Add("Stored Procedures");

                // Add tables
                foreach (var table in _currentAnalysis!.DatabaseAnalysis.Tables)
                {
                    var tableNode = tablesNode.Nodes.Add(table.Name);
                    tableNode.Tag = table;
                }

                // Add stored procedures
                foreach (var sp in _currentAnalysis.DatabaseAnalysis.StoredProcedures)
                {
                    var spNode = storedProcsNode.Nodes.Add(sp.Name);
                    spNode.Tag = sp;
                }

                // Expand nodes
                tablesNode.Expand();
                storedProcsNode.Expand();

                LoggingUtility.LogInfo($"[ApplicationAnalyzerForm] Database analysis populated - Tables: {_currentAnalysis.DatabaseAnalysis.Tables.Count}, SPs: {_currentAnalysis.DatabaseAnalysis.StoredProcedures.Count}", nameof(PopulateDatabaseAnalysisTab));
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Failed to populate database analysis: {ex.Message}", nameof(PopulateDatabaseAnalysisTab), ex);
            }
        }

        /// <summary>
        /// Populates the business logic tab
        /// </summary>
        private void PopulateBusinessLogicTab()
        {
            try
            {
                // Clear existing data
                treeView_BusinessLogic.Nodes.Clear();
                dataGridView_BusinessDetails.DataSource = null;

                // Add root nodes
                var classesNode = treeView_BusinessLogic.Nodes.Add("Classes");
                var servicesNode = treeView_BusinessLogic.Nodes.Add("Services");
                var helpersNode = treeView_BusinessLogic.Nodes.Add("Helpers");

                // Add classes
                foreach (var cls in _currentAnalysis!.BusinessLogicAnalysis.Classes)
                {
                    var classNode = classesNode.Nodes.Add(cls.Name);
                    classNode.Tag = cls;
                }

                // Add services
                foreach (var service in _currentAnalysis.BusinessLogicAnalysis.Services)
                {
                    var serviceNode = servicesNode.Nodes.Add(service.Name);
                    serviceNode.Tag = service;
                }

                // Add helpers
                foreach (var helper in _currentAnalysis.BusinessLogicAnalysis.Helpers)
                {
                    var helperNode = helpersNode.Nodes.Add(helper.Name);
                    helperNode.Tag = helper;
                }

                // Expand nodes
                classesNode.Expand();
                servicesNode.Expand();
                helpersNode.Expand();

                LoggingUtility.LogInfo($"[ApplicationAnalyzerForm] Business logic populated - Classes: {_currentAnalysis.BusinessLogicAnalysis.Classes.Count}", nameof(PopulateBusinessLogicTab));
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Failed to populate business logic: {ex.Message}", nameof(PopulateBusinessLogicTab), ex);
            }
        }

        /// <summary>
        /// Populates other analysis tabs
        /// </summary>
        private async Task PopulateOtherTabsAsync()
        {
            try
            {
                // Theming analysis
                var themingText = GenerateThemingAnalysisText();
                richTextBox_ThemingAnalysis.Text = themingText;

                // Error handling analysis
                var errorHandlingText = GenerateErrorHandlingAnalysisText();
                richTextBox_ErrorHandling.Text = errorHandlingText;

                // Environment analysis
                var environmentText = GenerateEnvironmentAnalysisText();
                richTextBox_Environment.Text = environmentText;

                await Task.CompletedTask; // Placeholder for async operations
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Failed to populate other tabs: {ex.Message}", nameof(PopulateOtherTabsAsync), ex);
            }
        }

        /// <summary>
        /// Populates the raw data tab with JSON
        /// </summary>
        private async Task PopulateRawDataTabAsync()
        {
            try
            {
                var jsonResult = await _formatter!.ExportToJsonAsync(true);
                if (jsonResult.IsSuccess)
                {
                    textBox_RawJson.Text = jsonResult.Data;
                }
                else
                {
                    textBox_RawJson.Text = $"Failed to generate JSON: {jsonResult.ErrorMessage}";
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Failed to populate raw data: {ex.Message}", nameof(PopulateRawDataTabAsync), ex);
                textBox_RawJson.Text = $"Failed to generate JSON: {ex.Message}";
            }
        }

        #endregion

        #region UI Event Handlers

        /// <summary>
        /// Updates UI state based on analysis running state
        /// </summary>
        private void UpdateUIState(bool analysisRunning)
        {
            button_RunFullAnalysis.Enabled = !analysisRunning;
            runFullAnalysisToolStripMenuItem.Enabled = !analysisRunning;
            
            // Enable/disable export menu items based on whether analysis has been run
            var hasAnalysis = _currentAnalysis != null && !analysisRunning;
            exportToJSONToolStripMenuItem.Enabled = hasAnalysis;
            exportToMarkdownToolStripMenuItem.Enabled = hasAnalysis;
            exportToHTMLToolStripMenuItem.Enabled = hasAnalysis;
            exportSummaryToolStripMenuItem.Enabled = hasAnalysis;

            toolStripProgressBar1.Visible = analysisRunning;
        }

        #endregion

        #region Button Clicks

        /// <summary>
        /// Run Full Analysis button click handler
        /// </summary>
        private async void button_RunFullAnalysis_Click(object sender, EventArgs e)
        {
            await Task.Run(RunFullAnalysisAsync);
        }

        /// <summary>
        /// Run Full Analysis menu item click handler
        /// </summary>
        private async void runFullAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(RunFullAnalysisAsync);
        }

        #endregion

        #region TreeView Events

        /// <summary>
        /// UI Structure tree view selection handler
        /// </summary>
        private void treeView_UIStructure_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node?.Tag == null)
                    return;

                if (e.Node.Tag is Model_FormInfo formInfo)
                {
                    ShowFormDetails(formInfo);
                }
                else if (e.Node.Tag is Model_ControlInfo controlInfo)
                {
                    ShowControlDetails(controlInfo);
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] TreeView selection error: {ex.Message}", nameof(treeView_UIStructure_AfterSelect), ex);
            }
        }

        /// <summary>
        /// Database Structure tree view selection handler
        /// </summary>
        private void treeView_DatabaseStructure_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node?.Tag == null)
                    return;

                if (e.Node.Tag is Model_TableInfo tableInfo)
                {
                    ShowTableDetails(tableInfo);
                }
                else if (e.Node.Tag is Model_StoredProcedureInfo spInfo)
                {
                    ShowStoredProcedureDetails(spInfo);
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Database TreeView selection error: {ex.Message}", nameof(treeView_DatabaseStructure_AfterSelect), ex);
            }
        }

        /// <summary>
        /// Business Logic tree view selection handler
        /// </summary>
        private void treeView_BusinessLogic_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node?.Tag == null)
                    return;

                if (e.Node.Tag is Model_ClassInfo classInfo)
                {
                    ShowClassDetails(classInfo);
                }
                else if (e.Node.Tag is Model_ServiceInfo serviceInfo)
                {
                    ShowServiceDetails(serviceInfo);
                }
                else if (e.Node.Tag is Model_HelperInfo helperInfo)
                {
                    ShowHelperDetails(helperInfo);
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Business Logic TreeView selection error: {ex.Message}", nameof(treeView_BusinessLogic_AfterSelect), ex);
            }
        }

        #endregion

        #region Export Menu Events

        /// <summary>
        /// Export to JSON menu handler
        /// </summary>
        private async void exportToJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ExportToFileAsync("JSON", "json", (path) => _formatter!.SaveToJsonFileAsync(path));
        }

        /// <summary>
        /// Export to Markdown menu handler
        /// </summary>
        private async void exportToMarkdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ExportToFileAsync("Markdown", "md", (path) => _formatter!.SaveToMarkdownFileAsync(path));
        }

        /// <summary>
        /// Export to HTML menu handler
        /// </summary>
        private async void exportToHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ExportToFileAsync("HTML", "html", (path) => _formatter!.SaveToHtmlFileAsync(path));
        }

        /// <summary>
        /// Export summary menu handler
        /// </summary>
        private async void exportSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ExportToFileAsync("Summary Text", "txt", async (path) =>
            {
                var summaryResult = await _formatter!.ExportSummaryAsync();
                if (summaryResult.IsSuccess)
                {
                    await File.WriteAllTextAsync(path, summaryResult.Data);
                    return DaoResult<string>.Success(path);
                }
                return DaoResult<string>.Failure(summaryResult.ErrorMessage);
            });
        }

        /// <summary>
        /// Close menu handler
        /// </summary>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// About menu handler
        /// </summary>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutText = "MTM WIP Application Analyzer\n\n" +
                           "A comprehensive analysis tool for migrating the MTM WIP Application to MAUI.\n\n" +
                           $"Analysis includes:\n" +
                           "• UI Structure and Components\n" +
                           "• Database Schema and Stored Procedures\n" +
                           "• Business Logic and Data Access Patterns\n" +
                           "• Theming and Styling System\n" +
                           "• Error Handling Patterns\n" +
                           "• Environment Configuration\n\n" +
                           $"Application Path: {_applicationRootPath}\n" +
                           $"Version: 1.0.0";

            Service_ErrorHandler.ShowInformation(aboutText, "About Application Analyzer");
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Generic export to file helper
        /// </summary>
        private async Task ExportToFileAsync(string formatName, string extension, Func<string, Task<DaoResult<string>>> exportFunc)
        {
            try
            {
                if (_formatter == null)
                {
                    Service_ErrorHandler.ShowValidationError("No analysis data to export", "Export Error");
                    return;
                }

                using var saveDialog = new SaveFileDialog
                {
                    Filter = $"{formatName} Files (*.{extension})|*.{extension}|All Files (*.*)|*.*",
                    DefaultExt = extension,
                    FileName = $"MTM_WIP_Analysis_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    _progressHelper?.ShowProgress($"Exporting to {formatName}...");

                    var result = await exportFunc(saveDialog.FileName);

                    if (result.IsSuccess)
                    {
                        _progressHelper?.ShowSuccess($"Exported to {formatName} successfully!");
                        LoggingUtility.LogInfo($"[ApplicationAnalyzerForm] Exported to {formatName}: {saveDialog.FileName}", nameof(ExportToFileAsync));
                        
                        var openResult = Service_ErrorHandler.ShowConfirmation($"Export completed successfully!\n\nWould you like to open the exported file?", "Export Complete");
                        if (openResult == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = saveDialog.FileName,
                                UseShellExecute = true
                            });
                        }
                    }
                    else
                    {
                        _progressHelper?.ShowError($"Export failed: {result.ErrorMessage}");
                        Service_ErrorHandler.ShowValidationError($"Export failed: {result.ErrorMessage}", "Export Error");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[ApplicationAnalyzerForm] Export to {formatName} failed: {ex.Message}", nameof(ExportToFileAsync), ex);
                _progressHelper?.ShowError($"Export failed: {ex.Message}");
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium,
                    contextData: new Dictionary<string, object> { ["Format"] = formatName },
                    controlName: nameof(ApplicationAnalyzerForm));
            }
        }

        /// <summary>
        /// Shows form details in the UI details grid
        /// </summary>
        private void ShowFormDetails(Model_FormInfo formInfo)
        {
            var details = new List<object>
            {
                new { Property = "Name", Value = formInfo.Name },
                new { Property = "Full Path", Value = formInfo.FullPath },
                new { Property = "Namespace", Value = formInfo.Namespace },
                new { Property = "Base Class", Value = formInfo.BaseClass },
                new { Property = "Purpose", Value = formInfo.Purpose },
                new { Property = "Events Count", Value = formInfo.Events.Count },
                new { Property = "Methods Count", Value = formInfo.Methods.Count },
                new { Property = "Properties Count", Value = formInfo.Properties.Count },
                new { Property = "Dependencies Count", Value = formInfo.Dependencies.Count }
            };

            dataGridView_UIDetails.DataSource = details;
        }

        /// <summary>
        /// Shows control details in the UI details grid
        /// </summary>
        private void ShowControlDetails(Model_ControlInfo controlInfo)
        {
            var details = new List<object>
            {
                new { Property = "Name", Value = controlInfo.Name },
                new { Property = "Type", Value = controlInfo.Type },
                new { Property = "Full Path", Value = controlInfo.FullPath },
                new { Property = "Parent Form", Value = controlInfo.ParentForm },
                new { Property = "Purpose", Value = controlInfo.Purpose },
                new { Property = "Is Custom Control", Value = controlInfo.IsCustomControl ? "Yes" : "No" },
                new { Property = "Events Count", Value = controlInfo.Events.Count },
                new { Property = "Methods Count", Value = controlInfo.Methods.Count },
                new { Property = "Properties Count", Value = controlInfo.Properties.Count }
            };

            dataGridView_UIDetails.DataSource = details;
        }

        /// <summary>
        /// Shows table details in the database details grid
        /// </summary>
        private void ShowTableDetails(Model_TableInfo tableInfo)
        {
            var details = new List<object>
            {
                new { Property = "Table Name", Value = tableInfo.Name },
                new { Property = "Purpose", Value = tableInfo.Purpose },
                new { Property = "Columns Count", Value = tableInfo.Columns.Count },
                new { Property = "Indexes Count", Value = tableInfo.Indexes.Count },
                new { Property = "Foreign Keys Count", Value = tableInfo.ForeignKeys.Count }
            };

            dataGridView_DatabaseDetails.DataSource = details;
        }

        /// <summary>
        /// Shows stored procedure details in the database details grid
        /// </summary>
        private void ShowStoredProcedureDetails(Model_StoredProcedureInfo spInfo)
        {
            var details = new List<object>
            {
                new { Property = "Procedure Name", Value = spInfo.Name },
                new { Property = "File Path", Value = spInfo.FilePath },
                new { Property = "Purpose", Value = spInfo.Purpose },
                new { Property = "Parameters Count", Value = spInfo.Parameters.Count },
                new { Property = "Used By Classes", Value = string.Join(", ", spInfo.UsedByClasses) }
            };

            dataGridView_DatabaseDetails.DataSource = details;
        }

        /// <summary>
        /// Shows class details in the business details grid
        /// </summary>
        private void ShowClassDetails(Model_ClassInfo classInfo)
        {
            var details = new List<object>
            {
                new { Property = "Class Name", Value = classInfo.Name },
                new { Property = "Full Path", Value = classInfo.FullPath },
                new { Property = "Namespace", Value = classInfo.Namespace },
                new { Property = "Base Class", Value = classInfo.BaseClass },
                new { Property = "Category", Value = classInfo.Category },
                new { Property = "Purpose", Value = classInfo.Purpose },
                new { Property = "Methods Count", Value = classInfo.Methods.Count },
                new { Property = "Properties Count", Value = classInfo.Properties.Count },
                new { Property = "Fields Count", Value = classInfo.Fields.Count },
                new { Property = "Interfaces Count", Value = classInfo.Interfaces.Count }
            };

            dataGridView_BusinessDetails.DataSource = details;
        }

        /// <summary>
        /// Shows service details in the business details grid
        /// </summary>
        private void ShowServiceDetails(Model_ServiceInfo serviceInfo)
        {
            var details = new List<object>
            {
                new { Property = "Service Name", Value = serviceInfo.Name },
                new { Property = "Full Path", Value = serviceInfo.FullPath },
                new { Property = "Purpose", Value = serviceInfo.Purpose },
                new { Property = "Public Methods Count", Value = serviceInfo.PublicMethods.Count },
                new { Property = "Dependencies Count", Value = serviceInfo.Dependencies.Count }
            };

            dataGridView_BusinessDetails.DataSource = details;
        }

        /// <summary>
        /// Shows helper details in the business details grid
        /// </summary>
        private void ShowHelperDetails(Model_HelperInfo helperInfo)
        {
            var details = new List<object>
            {
                new { Property = "Helper Name", Value = helperInfo.Name },
                new { Property = "Full Path", Value = helperInfo.FullPath },
                new { Property = "Category", Value = helperInfo.Category },
                new { Property = "Purpose", Value = helperInfo.Purpose },
                new { Property = "Utility Methods Count", Value = helperInfo.UtilityMethods.Count }
            };

            dataGridView_BusinessDetails.DataSource = details;
        }

        /// <summary>
        /// Generates theming analysis text
        /// </summary>
        private string GenerateThemingAnalysisText()
        {
            if (_currentAnalysis?.ThemingAnalysis == null)
                return "No theming analysis data available.";

            var sb = new StringBuilder();
            sb.AppendLine("THEMING SYSTEM ANALYSIS");
            sb.AppendLine("======================");
            sb.AppendLine();

            sb.AppendLine($"DPI Scaling Enabled: {(_currentAnalysis.ThemingAnalysis.DpiScaling.IsEnabled ? "Yes" : "No")}");
            sb.AppendLine($"Theme Files: {_currentAnalysis.ThemingAnalysis.ThemeFiles.Count}");
            sb.AppendLine($"Style Patterns: {_currentAnalysis.ThemingAnalysis.StylePatterns.Count}");
            sb.AppendLine($"Theme Constants: {_currentAnalysis.ThemingAnalysis.ThemeConstants.Count}");
            sb.AppendLine();

            if (_currentAnalysis.ThemingAnalysis.ThemeFiles.Any())
            {
                sb.AppendLine("THEME FILES:");
                foreach (var themeFile in _currentAnalysis.ThemingAnalysis.ThemeFiles)
                {
                    sb.AppendLine($"  • {themeFile.Name} - {themeFile.Purpose}");
                }
                sb.AppendLine();
            }

            if (_currentAnalysis.ThemingAnalysis.StylePatterns.Any())
            {
                sb.AppendLine("STYLE PATTERNS:");
                foreach (var pattern in _currentAnalysis.ThemingAnalysis.StylePatterns)
                {
                    sb.AppendLine($"  • {pattern.PatternName}: {pattern.Description}");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates error handling analysis text
        /// </summary>
        private string GenerateErrorHandlingAnalysisText()
        {
            if (_currentAnalysis?.ErrorHandlingAnalysis == null)
                return "No error handling analysis data available.";

            var sb = new StringBuilder();
            sb.AppendLine("ERROR HANDLING ANALYSIS");
            sb.AppendLine("=======================");
            sb.AppendLine();

            sb.AppendLine($"Error Handlers: {_currentAnalysis.ErrorHandlingAnalysis.ErrorHandlers.Count}");
            sb.AppendLine($"Exception Patterns: {_currentAnalysis.ErrorHandlingAnalysis.ExceptionPatterns.Count}");
            sb.AppendLine($"Logging Framework: {_currentAnalysis.ErrorHandlingAnalysis.LoggingConfiguration.LoggingFramework}");
            sb.AppendLine();

            if (_currentAnalysis.ErrorHandlingAnalysis.ErrorHandlers.Any())
            {
                sb.AppendLine("ERROR HANDLERS:");
                foreach (var handler in _currentAnalysis.ErrorHandlingAnalysis.ErrorHandlers)
                {
                    sb.AppendLine($"  • {handler.Name} - {handler.Purpose}");
                }
                sb.AppendLine();
            }

            if (_currentAnalysis.ErrorHandlingAnalysis.ExceptionPatterns.Any())
            {
                sb.AppendLine("EXCEPTION PATTERNS:");
                foreach (var pattern in _currentAnalysis.ErrorHandlingAnalysis.ExceptionPatterns)
                {
                    sb.AppendLine($"  • {pattern.PatternName}: {pattern.Description}");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates environment analysis text
        /// </summary>
        private string GenerateEnvironmentAnalysisText()
        {
            if (_currentAnalysis?.EnvironmentAnalysis == null)
                return "No environment analysis data available.";

            var sb = new StringBuilder();
            sb.AppendLine("ENVIRONMENT ANALYSIS");
            sb.AppendLine("===================");
            sb.AppendLine();

            sb.AppendLine($"Dependencies: {_currentAnalysis.EnvironmentAnalysis.Dependencies.Count}");
            sb.AppendLine($"Configuration Files: {_currentAnalysis.EnvironmentAnalysis.Configuration.ConfigurationFiles.Count}");
            sb.AppendLine($"Deployment Type: {_currentAnalysis.EnvironmentAnalysis.Deployment.DeploymentType}");
            sb.AppendLine();

            if (_currentAnalysis.EnvironmentAnalysis.Dependencies.Any())
            {
                sb.AppendLine("DEPENDENCIES:");
                foreach (var dependency in _currentAnalysis.EnvironmentAnalysis.Dependencies)
                {
                    sb.AppendLine($"  • {dependency.Name} v{dependency.Version} ({dependency.Type})");
                }
                sb.AppendLine();
            }

            if (_currentAnalysis.EnvironmentAnalysis.Configuration.AppSettings.Any())
            {
                sb.AppendLine("APPLICATION SETTINGS:");
                foreach (var setting in _currentAnalysis.EnvironmentAnalysis.Configuration.AppSettings)
                {
                    sb.AppendLine($"  • {setting.Key}: {setting.Value}");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _progressHelper?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    #endregion
}