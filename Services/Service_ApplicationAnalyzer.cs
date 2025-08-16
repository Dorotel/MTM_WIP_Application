using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Services
{
    #region Application Analysis Services

    /// <summary>
    /// Core service for analyzing the MTM WIP Application structure and components
    /// Provides comprehensive analysis for MAUI migration planning
    /// </summary>
    public class Service_ApplicationAnalyzer
    {
        #region Fields

        private readonly string _applicationRootPath;
        private readonly Helper_StoredProcedureProgress? _progressHelper;

        #endregion

        #region Properties

        public string ApplicationRootPath => _applicationRootPath;

        #endregion

        #region Constructors

        public Service_ApplicationAnalyzer(string applicationRootPath, Helper_StoredProcedureProgress? progressHelper = null)
        {
            _applicationRootPath = applicationRootPath ?? throw new ArgumentNullException(nameof(applicationRootPath));
            _progressHelper = progressHelper;
        }

        #endregion

        #region Analysis Methods

        /// <summary>
        /// Performs comprehensive analysis of the entire application
        /// </summary>
        public async Task<DaoResult<Model_ApplicationAnalysis>> AnalyzeApplicationAsync()
        {
            LoggingUtility.LogInfo($"[Service_ApplicationAnalyzer] Starting comprehensive application analysis", nameof(AnalyzeApplicationAsync));

            try
            {
                _progressHelper?.ShowProgress("Starting application analysis...");

                var analysis = new Model_ApplicationAnalysis
                {
                    AnalysisTimestamp = DateTime.Now,
                    ApplicationVersion = GetApplicationVersion(),
                    AnalysisVersion = "1.0.0"
                };

                // Perform UI Analysis
                _progressHelper?.UpdateProgress("Analyzing UI structure...", 10);
                var uiAnalysisResult = await AnalyzeUIStructureAsync();
                if (!uiAnalysisResult.IsSuccess)
                {
                    LoggingUtility.LogError($"[Service_ApplicationAnalyzer] UI analysis failed: {uiAnalysisResult.ErrorMessage}", nameof(AnalyzeApplicationAsync));
                    return DaoResult<Model_ApplicationAnalysis>.Failure($"UI analysis failed: {uiAnalysisResult.ErrorMessage}");
                }
                analysis.UIAnalysis = uiAnalysisResult.Data!;

                // Perform Database Analysis
                _progressHelper?.UpdateProgress("Analyzing database structure...", 25);
                var dbAnalysisResult = await AnalyzeDatabaseStructureAsync();
                if (!dbAnalysisResult.IsSuccess)
                {
                    LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Database analysis failed: {dbAnalysisResult.ErrorMessage}", nameof(AnalyzeApplicationAsync));
                    return DaoResult<Model_ApplicationAnalysis>.Failure($"Database analysis failed: {dbAnalysisResult.ErrorMessage}");
                }
                analysis.DatabaseAnalysis = dbAnalysisResult.Data!;

                // Perform Business Logic Analysis
                _progressHelper?.UpdateProgress("Analyzing business logic...", 45);
                var businessLogicResult = await AnalyzeBusinessLogicAsync();
                if (!businessLogicResult.IsSuccess)
                {
                    LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Business logic analysis failed: {businessLogicResult.ErrorMessage}", nameof(AnalyzeApplicationAsync));
                    return DaoResult<Model_ApplicationAnalysis>.Failure($"Business logic analysis failed: {businessLogicResult.ErrorMessage}");
                }
                analysis.BusinessLogicAnalysis = businessLogicResult.Data!;

                // Perform Theming Analysis
                _progressHelper?.UpdateProgress("Analyzing theming system...", 65);
                var themingResult = await AnalyzeThemingSystemAsync();
                if (!themingResult.IsSuccess)
                {
                    LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Theming analysis failed: {themingResult.ErrorMessage}", nameof(AnalyzeApplicationAsync));
                    return DaoResult<Model_ApplicationAnalysis>.Failure($"Theming analysis failed: {themingResult.ErrorMessage}");
                }
                analysis.ThemingAnalysis = themingResult.Data!;

                // Perform Error Handling Analysis
                _progressHelper?.UpdateProgress("Analyzing error handling patterns...", 80);
                var errorHandlingResult = await AnalyzeErrorHandlingAsync();
                if (!errorHandlingResult.IsSuccess)
                {
                    LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Error handling analysis failed: {errorHandlingResult.ErrorMessage}", nameof(AnalyzeApplicationAsync));
                    return DaoResult<Model_ApplicationAnalysis>.Failure($"Error handling analysis failed: {errorHandlingResult.ErrorMessage}");
                }
                analysis.ErrorHandlingAnalysis = errorHandlingResult.Data!;

                // Perform Environment Analysis
                _progressHelper?.UpdateProgress("Analyzing environment configuration...", 95);
                var environmentResult = await AnalyzeEnvironmentConfigurationAsync();
                if (!environmentResult.IsSuccess)
                {
                    LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Environment analysis failed: {environmentResult.ErrorMessage}", nameof(AnalyzeApplicationAsync));
                    return DaoResult<Model_ApplicationAnalysis>.Failure($"Environment analysis failed: {environmentResult.ErrorMessage}");
                }
                analysis.EnvironmentAnalysis = environmentResult.Data!;

                _progressHelper?.ShowSuccess("Application analysis completed successfully!");

                LoggingUtility.LogInfo($"[Service_ApplicationAnalyzer] Comprehensive analysis completed successfully", nameof(AnalyzeApplicationAsync));
                return DaoResult<Model_ApplicationAnalysis>.Success(analysis);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception during analysis: {ex.Message}", nameof(AnalyzeApplicationAsync), ex);
                _progressHelper?.ShowError($"Analysis failed: {ex.Message}");
                return DaoResult<Model_ApplicationAnalysis>.Failure($"Analysis failed: {ex.Message}");
            }
        }

        #endregion

        #region UI Analysis

        /// <summary>
        /// Analyzes UI structure, forms, controls, and layouts
        /// </summary>
        private async Task<DaoResult<Model_UIAnalysis>> AnalyzeUIStructureAsync()
        {
            try
            {
                LoggingUtility.LogInfo("[Service_ApplicationAnalyzer] Starting UI structure analysis", nameof(AnalyzeUIStructureAsync));

                var uiAnalysis = new Model_UIAnalysis();

                // Analyze Forms
                var formsPath = Path.Combine(_applicationRootPath, "Forms");
                if (Directory.Exists(formsPath))
                {
                    await AnalyzeFormsAsync(formsPath, uiAnalysis);
                }

                // Analyze Controls
                var controlsPath = Path.Combine(_applicationRootPath, "Controls");
                if (Directory.Exists(controlsPath))
                {
                    await AnalyzeControlsAsync(controlsPath, uiAnalysis);
                }

                // Analyze Navigation
                await AnalyzeNavigationStructureAsync(uiAnalysis);

                // Calculate UI metrics
                uiAnalysis.UIMetrics = new Dictionary<string, object>
                {
                    ["TotalForms"] = uiAnalysis.Forms.Count,
                    ["TotalControls"] = uiAnalysis.Controls.Count,
                    ["CustomControlsCount"] = uiAnalysis.Controls.Count(c => c.IsCustomControl),
                    ["FormsWithEvents"] = uiAnalysis.Forms.Count(f => f.Events.Any()),
                    ["AnalysisTimestamp"] = DateTime.Now
                };

                LoggingUtility.LogInfo($"[Service_ApplicationAnalyzer] UI analysis completed - Forms: {uiAnalysis.Forms.Count}, Controls: {uiAnalysis.Controls.Count}", nameof(AnalyzeUIStructureAsync));
                return DaoResult<Model_UIAnalysis>.Success(uiAnalysis);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception during UI analysis: {ex.Message}", nameof(AnalyzeUIStructureAsync), ex);
                return DaoResult<Model_UIAnalysis>.Failure($"UI analysis failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Analyzes all forms in the application
        /// </summary>
        private async Task AnalyzeFormsAsync(string formsPath, Model_UIAnalysis uiAnalysis)
        {
            var formFiles = Directory.GetFiles(formsPath, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.EndsWith(".Designer.cs"))
                .ToList();

            foreach (var formFile in formFiles)
            {
                try
                {
                    var formInfo = await AnalyzeFormFileAsync(formFile);
                    if (formInfo != null)
                    {
                        uiAnalysis.Forms.Add(formInfo);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogWarning($"[Service_ApplicationAnalyzer] Failed to analyze form file {formFile}: {ex.Message}", nameof(AnalyzeFormsAsync));
                }
            }
        }

        /// <summary>
        /// Analyzes a single form file
        /// </summary>
        private async Task<Model_FormInfo?> AnalyzeFormFileAsync(string formFile)
        {
            try
            {
                var content = await File.ReadAllTextAsync(formFile);
                var tree = CSharpSyntaxTree.ParseText(content);
                var root = tree.GetRoot();

                var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                if (classDeclaration == null) return null;

                var formInfo = new Model_FormInfo
                {
                    Name = classDeclaration.Identifier.Text,
                    FullPath = formFile,
                    Namespace = GetNamespace(root),
                    BaseClass = GetBaseClass(classDeclaration),
                    Purpose = ExtractPurposeFromComments(classDeclaration)
                };

                // Analyze methods
                var methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    formInfo.Methods.Add(method.Identifier.Text);
                }

                // Analyze events (methods ending with event patterns)
                var events = methods.Where(m => m.Identifier.Text.Contains("_Click") || 
                                               m.Identifier.Text.Contains("_Changed") ||
                                               m.Identifier.Text.Contains("_Enter") ||
                                               m.Identifier.Text.Contains("_Leave"));
                foreach (var eventMethod in events)
                {
                    formInfo.Events.Add(eventMethod.Identifier.Text);
                }

                // Analyze properties
                var properties = classDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>();
                foreach (var property in properties)
                {
                    formInfo.Properties[property.Identifier.Text] = property.Type.ToString();
                }

                // Analyze dependencies (using statements)
                var usingStatements = root.DescendantNodes().OfType<UsingDirectiveSyntax>();
                foreach (var usingStatement in usingStatements)
                {
                    formInfo.Dependencies.Add(usingStatement.Name?.ToString() ?? "");
                }

                return formInfo;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception analyzing form file {formFile}: {ex.Message}", nameof(AnalyzeFormFileAsync), ex);
                return null;
            }
        }

        /// <summary>
        /// Analyzes all controls in the application
        /// </summary>
        private async Task AnalyzeControlsAsync(string controlsPath, Model_UIAnalysis uiAnalysis)
        {
            var controlFiles = Directory.GetFiles(controlsPath, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.EndsWith(".Designer.cs"))
                .ToList();

            foreach (var controlFile in controlFiles)
            {
                try
                {
                    var controlInfo = await AnalyzeControlFileAsync(controlFile);
                    if (controlInfo != null)
                    {
                        uiAnalysis.Controls.Add(controlInfo);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogWarning($"[Service_ApplicationAnalyzer] Failed to analyze control file {controlFile}: {ex.Message}", nameof(AnalyzeControlsAsync));
                }
            }
        }

        /// <summary>
        /// Analyzes a single control file
        /// </summary>
        private async Task<Model_ControlInfo?> AnalyzeControlFileAsync(string controlFile)
        {
            try
            {
                var content = await File.ReadAllTextAsync(controlFile);
                var tree = CSharpSyntaxTree.ParseText(content);
                var root = tree.GetRoot();

                var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                if (classDeclaration == null) return null;

                var controlInfo = new Model_ControlInfo
                {
                    Name = classDeclaration.Identifier.Text,
                    FullPath = controlFile,
                    Type = GetBaseClass(classDeclaration),
                    Purpose = ExtractPurposeFromComments(classDeclaration),
                    IsCustomControl = IsCustomControl(classDeclaration)
                };

                // Analyze methods
                var methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    controlInfo.Methods.Add(method.Identifier.Text);
                }

                // Analyze events
                var events = methods.Where(m => m.Identifier.Text.Contains("_Click") || 
                                               m.Identifier.Text.Contains("_Changed") ||
                                               m.Identifier.Text.Contains("_Enter") ||
                                               m.Identifier.Text.Contains("_Leave"));
                foreach (var eventMethod in events)
                {
                    controlInfo.Events.Add(eventMethod.Identifier.Text);
                }

                // Analyze properties
                var properties = classDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>();
                foreach (var property in properties)
                {
                    controlInfo.Properties.Add($"{property.Identifier.Text}: {property.Type}");
                }

                return controlInfo;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception analyzing control file {controlFile}: {ex.Message}", nameof(AnalyzeControlFileAsync), ex);
                return null;
            }
        }

        /// <summary>
        /// Analyzes navigation structure of the application
        /// </summary>
        private async Task AnalyzeNavigationStructureAsync(Model_UIAnalysis uiAnalysis)
        {
            try
            {
                var navigation = new Model_NavigationInfo();

                // Look for main form and analyze its navigation structure
                var mainFormPath = Path.Combine(_applicationRootPath, "Forms", "MainForm", "MainForm.cs");
                if (File.Exists(mainFormPath))
                {
                    var content = await File.ReadAllTextAsync(mainFormPath);
                    
                    // Extract menu items (simplified pattern matching)
                    var menuItemMatches = Regex.Matches(content, @"ToolStripMenuItem\s+(\w+)", RegexOptions.IgnoreCase);
                    foreach (Match match in menuItemMatches)
                    {
                        navigation.MenuItems.Add(match.Groups[1].Value);
                    }

                    // Extract tab pages
                    var tabPageMatches = Regex.Matches(content, @"TabPage\s+(\w+)", RegexOptions.IgnoreCase);
                    foreach (Match match in tabPageMatches)
                    {
                        navigation.TabPages.Add(match.Groups[1].Value);
                    }
                }

                uiAnalysis.Navigation = navigation;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception analyzing navigation: {ex.Message}", nameof(AnalyzeNavigationStructureAsync), ex);
            }
        }

        #endregion

        #region Database Analysis

        /// <summary>
        /// Analyzes database structure, stored procedures, and relationships
        /// </summary>
        private async Task<DaoResult<Model_DatabaseAnalysis>> AnalyzeDatabaseStructureAsync()
        {
            try
            {
                LoggingUtility.LogInfo("[Service_ApplicationAnalyzer] Starting database structure analysis", nameof(AnalyzeDatabaseStructureAsync));

                var dbAnalysis = new Model_DatabaseAnalysis();

                // Analyze stored procedures
                var storedProceduresPath = Path.Combine(_applicationRootPath, "Database", "UpdatedStoredProcedures");
                if (Directory.Exists(storedProceduresPath))
                {
                    await AnalyzeStoredProceduresAsync(storedProceduresPath, dbAnalysis);
                }

                // Analyze database schema from SQL files
                var databasePath = Path.Combine(_applicationRootPath, "Database", "UpdatedDatabase");
                if (Directory.Exists(databasePath))
                {
                    await AnalyzeDatabaseSchemaAsync(databasePath, dbAnalysis);
                }

                // Get database configuration
                dbAnalysis.Configuration = GetDatabaseConfiguration();

                // Calculate database metrics
                dbAnalysis.DatabaseMetrics = new Dictionary<string, object>
                {
                    ["TotalStoredProcedures"] = dbAnalysis.StoredProcedures.Count,
                    ["TotalTables"] = dbAnalysis.Tables.Count,
                    ["TotalRelationships"] = dbAnalysis.Relationships.Count,
                    ["AnalysisTimestamp"] = DateTime.Now
                };

                LoggingUtility.LogInfo($"[Service_ApplicationAnalyzer] Database analysis completed - Procedures: {dbAnalysis.StoredProcedures.Count}, Tables: {dbAnalysis.Tables.Count}", nameof(AnalyzeDatabaseStructureAsync));
                return DaoResult<Model_DatabaseAnalysis>.Success(dbAnalysis);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception during database analysis: {ex.Message}", nameof(AnalyzeDatabaseStructureAsync), ex);
                return DaoResult<Model_DatabaseAnalysis>.Failure($"Database analysis failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Analyzes stored procedures
        /// </summary>
        private async Task AnalyzeStoredProceduresAsync(string storedProcPath, Model_DatabaseAnalysis dbAnalysis)
        {
            var spFiles = Directory.GetFiles(storedProcPath, "*.sql", SearchOption.AllDirectories);

            foreach (var spFile in spFiles)
            {
                try
                {
                    var spInfo = await AnalyzeStoredProcedureFileAsync(spFile);
                    if (spInfo != null)
                    {
                        dbAnalysis.StoredProcedures.Add(spInfo);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogWarning($"[Service_ApplicationAnalyzer] Failed to analyze stored procedure {spFile}: {ex.Message}", nameof(AnalyzeStoredProceduresAsync));
                }
            }
        }

        /// <summary>
        /// Analyzes a single stored procedure file
        /// </summary>
        private async Task<Model_StoredProcedureInfo?> AnalyzeStoredProcedureFileAsync(string spFile)
        {
            try
            {
                var content = await File.ReadAllTextAsync(spFile);
                var fileName = Path.GetFileNameWithoutExtension(spFile);

                var spInfo = new Model_StoredProcedureInfo
                {
                    Name = fileName,
                    FilePath = spFile,
                    Body = content,
                    Purpose = ExtractPurposeFromSqlComments(content)
                };

                // Extract parameters using regex
                var parameterMatches = Regex.Matches(content, @"(IN|OUT|INOUT)\s+p_(\w+)\s+(\w+(?:\(\d+\))?)", RegexOptions.IgnoreCase);
                foreach (Match match in parameterMatches)
                {
                    var parameter = new Model_ParameterInfo
                    {
                        Name = "p_" + match.Groups[2].Value,
                        DataType = match.Groups[3].Value,
                        Direction = match.Groups[1].Value.ToUpper(),
                        IsRequired = !match.Groups[3].Value.Contains("DEFAULT")
                    };
                    spInfo.Parameters.Add(parameter);
                }

                return spInfo;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception analyzing stored procedure {spFile}: {ex.Message}", nameof(AnalyzeStoredProcedureFileAsync), ex);
                return null;
            }
        }

        /// <summary>
        /// Analyzes database schema from SQL files
        /// </summary>
        private async Task AnalyzeDatabaseSchemaAsync(string databasePath, Model_DatabaseAnalysis dbAnalysis)
        {
            var sqlFiles = Directory.GetFiles(databasePath, "*.sql", SearchOption.AllDirectories);

            foreach (var sqlFile in sqlFiles)
            {
                try
                {
                    var content = await File.ReadAllTextAsync(sqlFile);
                    var tables = ExtractTablesFromSql(content);
                    dbAnalysis.Tables.AddRange(tables);
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogWarning($"[Service_ApplicationAnalyzer] Failed to analyze SQL file {sqlFile}: {ex.Message}", nameof(AnalyzeDatabaseSchemaAsync));
                }
            }
        }

        #endregion

        #region Business Logic Analysis

        /// <summary>
        /// Analyzes business logic patterns and data access
        /// </summary>
        private async Task<DaoResult<Model_BusinessLogicAnalysis>> AnalyzeBusinessLogicAsync()
        {
            try
            {
                LoggingUtility.LogInfo("[Service_ApplicationAnalyzer] Starting business logic analysis", nameof(AnalyzeBusinessLogicAsync));

                var businessLogic = new Model_BusinessLogicAnalysis();

                // Analyze Data Access Objects
                var dataPath = Path.Combine(_applicationRootPath, "Data");
                if (Directory.Exists(dataPath))
                {
                    await AnalyzeDataAccessClassesAsync(dataPath, businessLogic);
                }

                // Analyze Services
                var servicesPath = Path.Combine(_applicationRootPath, "Services");
                if (Directory.Exists(servicesPath))
                {
                    await AnalyzeServicesAsync(servicesPath, businessLogic);
                }

                // Analyze Helpers
                var helpersPath = Path.Combine(_applicationRootPath, "Helpers");
                if (Directory.Exists(helpersPath))
                {
                    await AnalyzeHelpersAsync(helpersPath, businessLogic);
                }

                // Analyze data access patterns
                await AnalyzeDataAccessPatternsAsync(businessLogic);

                // Calculate business logic metrics
                businessLogic.BusinessLogicMetrics = new Dictionary<string, object>
                {
                    ["TotalClasses"] = businessLogic.Classes.Count,
                    ["TotalServices"] = businessLogic.Services.Count,
                    ["TotalHelpers"] = businessLogic.Helpers.Count,
                    ["TotalDataAccessPatterns"] = businessLogic.DataAccessPatterns.Count,
                    ["AnalysisTimestamp"] = DateTime.Now
                };

                LoggingUtility.LogInfo($"[Service_ApplicationAnalyzer] Business logic analysis completed - Classes: {businessLogic.Classes.Count}, Services: {businessLogic.Services.Count}", nameof(AnalyzeBusinessLogicAsync));
                return DaoResult<Model_BusinessLogicAnalysis>.Success(businessLogic);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception during business logic analysis: {ex.Message}", nameof(AnalyzeBusinessLogicAsync), ex);
                return DaoResult<Model_BusinessLogicAnalysis>.Failure($"Business logic analysis failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Analyzes data access classes
        /// </summary>
        private async Task AnalyzeDataAccessClassesAsync(string dataPath, Model_BusinessLogicAnalysis businessLogic)
        {
            var classFiles = Directory.GetFiles(dataPath, "*.cs", SearchOption.AllDirectories);

            foreach (var classFile in classFiles)
            {
                try
                {
                    var classInfo = await AnalyzeClassFileAsync(classFile, "DAO");
                    if (classInfo != null)
                    {
                        businessLogic.Classes.Add(classInfo);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogWarning($"[Service_ApplicationAnalyzer] Failed to analyze data access class {classFile}: {ex.Message}", nameof(AnalyzeDataAccessClassesAsync));
                }
            }
        }

        /// <summary>
        /// Analyzes service classes
        /// </summary>
        private async Task AnalyzeServicesAsync(string servicesPath, Model_BusinessLogicAnalysis businessLogic)
        {
            var serviceFiles = Directory.GetFiles(servicesPath, "*.cs", SearchOption.AllDirectories);

            foreach (var serviceFile in serviceFiles)
            {
                try
                {
                    var serviceInfo = await AnalyzeServiceFileAsync(serviceFile);
                    if (serviceInfo != null)
                    {
                        businessLogic.Services.Add(serviceInfo);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogWarning($"[Service_ApplicationAnalyzer] Failed to analyze service {serviceFile}: {ex.Message}", nameof(AnalyzeServicesAsync));
                }
            }
        }

        /// <summary>
        /// Analyzes helper classes
        /// </summary>
        private async Task AnalyzeHelpersAsync(string helpersPath, Model_BusinessLogicAnalysis businessLogic)
        {
            var helperFiles = Directory.GetFiles(helpersPath, "*.cs", SearchOption.AllDirectories);

            foreach (var helperFile in helperFiles)
            {
                try
                {
                    var helperInfo = await AnalyzeHelperFileAsync(helperFile);
                    if (helperInfo != null)
                    {
                        businessLogic.Helpers.Add(helperInfo);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogWarning($"[Service_ApplicationAnalyzer] Failed to analyze helper {helperFile}: {ex.Message}", nameof(AnalyzeHelpersAsync));
                }
            }
        }

        #endregion

        #region Theming Analysis

        /// <summary>
        /// Analyzes theming and styling system
        /// </summary>
        private async Task<DaoResult<Model_ThemingAnalysis>> AnalyzeThemingSystemAsync()
        {
            try
            {
                LoggingUtility.LogInfo("[Service_ApplicationAnalyzer] Starting theming system analysis", nameof(AnalyzeThemingSystemAsync));

                var theming = new Model_ThemingAnalysis();

                // Analyze Core theme files
                var corePath = Path.Combine(_applicationRootPath, "Core");
                if (Directory.Exists(corePath))
                {
                    await AnalyzeThemeFilesAsync(corePath, theming);
                }

                // Analyze DPI scaling
                await AnalyzeDpiScalingAsync(theming);

                // Extract theme constants
                await ExtractThemeConstantsAsync(theming);

                // Calculate theming metrics
                theming.ThemingMetrics = new Dictionary<string, object>
                {
                    ["TotalThemeFiles"] = theming.ThemeFiles.Count,
                    ["TotalStylePatterns"] = theming.StylePatterns.Count,
                    ["DpiScalingEnabled"] = theming.DpiScaling.IsEnabled,
                    ["AnalysisTimestamp"] = DateTime.Now
                };

                LoggingUtility.LogInfo($"[Service_ApplicationAnalyzer] Theming analysis completed - Theme files: {theming.ThemeFiles.Count}", nameof(AnalyzeThemingSystemAsync));
                return DaoResult<Model_ThemingAnalysis>.Success(theming);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception during theming analysis: {ex.Message}", nameof(AnalyzeThemingSystemAsync), ex);
                return DaoResult<Model_ThemingAnalysis>.Failure($"Theming analysis failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Analyzes theme files
        /// </summary>
        private async Task AnalyzeThemeFilesAsync(string corePath, Model_ThemingAnalysis theming)
        {
            var themeFiles = Directory.GetFiles(corePath, "*Theme*.cs", SearchOption.AllDirectories);

            foreach (var themeFile in themeFiles)
            {
                try
                {
                    var themeInfo = await AnalyzeThemeFileAsync(themeFile);
                    if (themeInfo != null)
                    {
                        theming.ThemeFiles.Add(themeInfo);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogWarning($"[Service_ApplicationAnalyzer] Failed to analyze theme file {themeFile}: {ex.Message}", nameof(AnalyzeThemeFilesAsync));
                }
            }
        }

        #endregion

        #region Error Handling Analysis

        /// <summary>
        /// Analyzes error handling patterns and logging
        /// </summary>
        private async Task<DaoResult<Model_ErrorHandlingAnalysis>> AnalyzeErrorHandlingAsync()
        {
            try
            {
                LoggingUtility.LogInfo("[Service_ApplicationAnalyzer] Starting error handling analysis", nameof(AnalyzeErrorHandlingAsync));

                var errorHandling = new Model_ErrorHandlingAnalysis();

                // Analyze error handling services
                var servicesPath = Path.Combine(_applicationRootPath, "Services");
                if (Directory.Exists(servicesPath))
                {
                    await AnalyzeErrorHandlingServicesAsync(servicesPath, errorHandling);
                }

                // Analyze logging configuration
                var loggingPath = Path.Combine(_applicationRootPath, "Logging");
                if (Directory.Exists(loggingPath))
                {
                    await AnalyzeLoggingConfigurationAsync(loggingPath, errorHandling);
                }

                // Analyze exception patterns across the application
                await AnalyzeExceptionPatternsAsync(errorHandling);

                // Calculate error handling metrics
                errorHandling.ErrorHandlingMetrics = new Dictionary<string, object>
                {
                    ["TotalErrorHandlers"] = errorHandling.ErrorHandlers.Count,
                    ["TotalExceptionPatterns"] = errorHandling.ExceptionPatterns.Count,
                    ["LoggingConfigured"] = !string.IsNullOrEmpty(errorHandling.LoggingConfiguration.LoggingFramework),
                    ["AnalysisTimestamp"] = DateTime.Now
                };

                LoggingUtility.LogInfo($"[Service_ApplicationAnalyzer] Error handling analysis completed - Handlers: {errorHandling.ErrorHandlers.Count}", nameof(AnalyzeErrorHandlingAsync));
                return DaoResult<Model_ErrorHandlingAnalysis>.Success(errorHandling);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception during error handling analysis: {ex.Message}", nameof(AnalyzeErrorHandlingAsync), ex);
                return DaoResult<Model_ErrorHandlingAnalysis>.Failure($"Error handling analysis failed: {ex.Message}");
            }
        }

        #endregion

        #region Environment Analysis

        /// <summary>
        /// Analyzes environment configuration and deployment
        /// </summary>
        private async Task<DaoResult<Model_EnvironmentAnalysis>> AnalyzeEnvironmentConfigurationAsync()
        {
            try
            {
                LoggingUtility.LogInfo("[Service_ApplicationAnalyzer] Starting environment analysis", nameof(AnalyzeEnvironmentConfigurationAsync));

                var environment = new Model_EnvironmentAnalysis();

                // Analyze configuration files
                await AnalyzeConfigurationFilesAsync(environment);

                // Analyze dependencies
                await AnalyzeDependenciesAsync(environment);

                // Analyze deployment information
                await AnalyzeDeploymentInfoAsync(environment);

                // Calculate environment metrics
                environment.EnvironmentMetrics = new Dictionary<string, object>
                {
                    ["TotalDependencies"] = environment.Dependencies.Count,
                    ["ConfigurationFilesCount"] = environment.Configuration.ConfigurationFiles.Count,
                    ["AnalysisTimestamp"] = DateTime.Now
                };

                LoggingUtility.LogInfo($"[Service_ApplicationAnalyzer] Environment analysis completed - Dependencies: {environment.Dependencies.Count}", nameof(AnalyzeEnvironmentConfigurationAsync));
                return DaoResult<Model_EnvironmentAnalysis>.Success(environment);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_ApplicationAnalyzer] Exception during environment analysis: {ex.Message}", nameof(AnalyzeEnvironmentConfigurationAsync), ex);
                return DaoResult<Model_EnvironmentAnalysis>.Failure($"Environment analysis failed: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the application version
        /// </summary>
        private string GetApplicationVersion()
        {
            try
            {
                return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Extracts namespace from syntax root
        /// </summary>
        private string GetNamespace(SyntaxNode root)
        {
            var namespaceDeclaration = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            return namespaceDeclaration?.Name.ToString() ?? "";
        }

        /// <summary>
        /// Gets base class from class declaration
        /// </summary>
        private string GetBaseClass(ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.BaseList?.Types.FirstOrDefault()?.ToString() ?? "";
        }

        /// <summary>
        /// Extracts purpose from XML documentation comments
        /// </summary>
        private string ExtractPurposeFromComments(ClassDeclarationSyntax classDeclaration)
        {
            var documentationComment = classDeclaration.GetLeadingTrivia()
                .FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
                                    t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));
            
            if (documentationComment.IsKind(SyntaxKind.None))
                return "";

            var text = documentationComment.ToString();
            var summaryMatch = Regex.Match(text, @"<summary>\s*(.*?)\s*</summary>", RegexOptions.Singleline);
            return summaryMatch.Success ? summaryMatch.Groups[1].Value.Trim() : "";
        }

        /// <summary>
        /// Extracts purpose from SQL comments
        /// </summary>
        private string ExtractPurposeFromSqlComments(string content)
        {
            var lines = content.Split('\n');
            var purposeLine = lines.FirstOrDefault(l => l.TrimStart().StartsWith("-- Purpose:") || 
                                                       l.TrimStart().StartsWith("-- Description:"));
            
            if (purposeLine != null)
            {
                return purposeLine.Substring(purposeLine.IndexOf(':') + 1).Trim();
            }

            return "";
        }

        /// <summary>
        /// Determines if a class is a custom control
        /// </summary>
        private bool IsCustomControl(ClassDeclarationSyntax classDeclaration)
        {
            var baseClass = GetBaseClass(classDeclaration);
            return baseClass.Contains("UserControl") || baseClass.Contains("Control") || 
                   classDeclaration.Identifier.Text.StartsWith("Control_");
        }

        /// <summary>
        /// Extracts table information from SQL content
        /// </summary>
        private List<Model_TableInfo> ExtractTablesFromSql(string content)
        {
            var tables = new List<Model_TableInfo>();
            
            // Simple regex to extract CREATE TABLE statements
            var tableMatches = Regex.Matches(content, @"CREATE TABLE\s+(\w+)\s*\((.*?)\)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            
            foreach (Match match in tableMatches)
            {
                var tableName = match.Groups[1].Value;
                var columnsText = match.Groups[2].Value;
                
                var table = new Model_TableInfo
                {
                    Name = tableName,
                    Purpose = ExtractTablePurpose(content, tableName)
                };

                // Extract columns (simplified)
                var columnMatches = Regex.Matches(columnsText, @"(\w+)\s+(\w+(?:\(\d+\))?)", RegexOptions.IgnoreCase);
                foreach (Match columnMatch in columnMatches)
                {
                    var column = new Model_ColumnInfo
                    {
                        Name = columnMatch.Groups[1].Value,
                        DataType = columnMatch.Groups[2].Value,
                        IsNullable = !columnsText.Contains($"{columnMatch.Groups[1].Value}.*NOT NULL"),
                        IsPrimaryKey = columnsText.Contains($"PRIMARY KEY.*{columnMatch.Groups[1].Value}")
                    };
                    table.Columns.Add(column);
                }

                tables.Add(table);
            }

            return tables;
        }

        /// <summary>
        /// Extracts table purpose from SQL comments
        /// </summary>
        private string ExtractTablePurpose(string content, string tableName)
        {
            var lines = content.Split('\n');
            var tableLineIndex = Array.FindIndex(lines, l => l.Contains($"CREATE TABLE {tableName}"));
            
            if (tableLineIndex > 0)
            {
                var commentLine = lines[tableLineIndex - 1];
                if (commentLine.TrimStart().StartsWith("--"))
                {
                    return commentLine.TrimStart().Substring(2).Trim();
                }
            }

            return "";
        }

        /// <summary>
        /// Gets database configuration
        /// </summary>
        private Model_DatabaseConfiguration GetDatabaseConfiguration()
        {
            return new Model_DatabaseConfiguration
            {
                ServerAddress = "172.16.1.104", // From the documented configuration
                DatabaseName = "mtm_wip_application",
                Version = "MySQL 5.7.24+",
                ConnectionSettings = new Dictionary<string, string>
                {
                    ["Environment"] = "Production/Debug Auto-Detection",
                    ["ConnectionMethod"] = "Helper_Database_Variables.GetConnectionString()"
                }
            };
        }

        /// <summary>
        /// Placeholder methods for remaining analysis (to be implemented)
        /// </summary>
        private async Task<Model_ClassInfo?> AnalyzeClassFileAsync(string classFile, string category)
        {
            // Implementation placeholder - would analyze individual class files
            await Task.Delay(1); // Placeholder
            return null;
        }

        private async Task<Model_ServiceInfo?> AnalyzeServiceFileAsync(string serviceFile)
        {
            // Implementation placeholder
            await Task.Delay(1);
            return null;
        }

        private async Task<Model_HelperInfo?> AnalyzeHelperFileAsync(string helperFile)
        {
            // Implementation placeholder
            await Task.Delay(1);
            return null;
        }

        private async Task AnalyzeDataAccessPatternsAsync(Model_BusinessLogicAnalysis businessLogic)
        {
            // Implementation placeholder - would analyze common patterns like DaoResult<T>, Helper usage, etc.
            await Task.Delay(1);
        }

        private async Task<Model_ThemeFileInfo?> AnalyzeThemeFileAsync(string themeFile)
        {
            // Implementation placeholder
            await Task.Delay(1);
            return null;
        }

        private async Task AnalyzeDpiScalingAsync(Model_ThemingAnalysis theming)
        {
            // Implementation placeholder
            await Task.Delay(1);
        }

        private async Task ExtractThemeConstantsAsync(Model_ThemingAnalysis theming)
        {
            // Implementation placeholder
            await Task.Delay(1);
        }

        private async Task AnalyzeErrorHandlingServicesAsync(string servicesPath, Model_ErrorHandlingAnalysis errorHandling)
        {
            // Implementation placeholder
            await Task.Delay(1);
        }

        private async Task AnalyzeLoggingConfigurationAsync(string loggingPath, Model_ErrorHandlingAnalysis errorHandling)
        {
            // Implementation placeholder
            await Task.Delay(1);
        }

        private async Task AnalyzeExceptionPatternsAsync(Model_ErrorHandlingAnalysis errorHandling)
        {
            // Implementation placeholder
            await Task.Delay(1);
        }

        private async Task AnalyzeConfigurationFilesAsync(Model_EnvironmentAnalysis environment)
        {
            // Implementation placeholder
            await Task.Delay(1);
        }

        private async Task AnalyzeDependenciesAsync(Model_EnvironmentAnalysis environment)
        {
            // Implementation placeholder
            await Task.Delay(1);
        }

        private async Task AnalyzeDeploymentInfoAsync(Model_EnvironmentAnalysis environment)
        {
            // Implementation placeholder
            await Task.Delay(1);
        }

        #endregion
    }

    #endregion
}