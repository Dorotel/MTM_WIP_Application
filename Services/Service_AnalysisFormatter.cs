using System.Text;
using Newtonsoft.Json;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Services
{
    #region Analysis Output Formatters

    /// <summary>
    /// Service for formatting and exporting application analysis results
    /// Supports multiple output formats for MAUI migration planning
    /// </summary>
    public class Service_AnalysisFormatter
    {
        #region Fields

        private readonly Model_ApplicationAnalysis _analysis;

        #endregion

        #region Constructors

        public Service_AnalysisFormatter(Model_ApplicationAnalysis analysis)
        {
            _analysis = analysis ?? throw new ArgumentNullException(nameof(analysis));
        }

        #endregion

        #region JSON Export

        /// <summary>
        /// Exports analysis results to JSON format
        /// </summary>
        public async Task<DaoResult<string>> ExportToJsonAsync(bool prettyFormat = true)
        {
            try
            {
                LoggingUtility.LogInfo("[Service_AnalysisFormatter] Starting JSON export", nameof(ExportToJsonAsync));

                var settings = new JsonSerializerSettings
                {
                    Formatting = prettyFormat ? Formatting.Indented : Formatting.None,
                    NullValueHandling = NullValueHandling.Include,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                };

                var json = await Task.Run(() => JsonConvert.SerializeObject(_analysis, settings));

                LoggingUtility.LogInfo($"[Service_AnalysisFormatter] JSON export completed - Size: {json.Length} characters", nameof(ExportToJsonAsync));
                return DaoResult<string>.Success(json);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_AnalysisFormatter] JSON export failed: {ex.Message}", nameof(ExportToJsonAsync), ex);
                return DaoResult<string>.Failure($"JSON export failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves analysis results to JSON file
        /// </summary>
        public async Task<DaoResult<string>> SaveToJsonFileAsync(string filePath, bool prettyFormat = true)
        {
            try
            {
                var jsonResult = await ExportToJsonAsync(prettyFormat);
                if (!jsonResult.IsSuccess)
                {
                    return DaoResult<string>.Failure($"Failed to generate JSON: {jsonResult.ErrorMessage}");
                }

                await File.WriteAllTextAsync(filePath, jsonResult.Data);

                LoggingUtility.LogInfo($"[Service_AnalysisFormatter] JSON file saved: {filePath}", nameof(SaveToJsonFileAsync));
                return DaoResult<string>.Success(filePath);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_AnalysisFormatter] Failed to save JSON file: {ex.Message}", nameof(SaveToJsonFileAsync), ex);
                return DaoResult<string>.Failure($"Failed to save JSON file: {ex.Message}");
            }
        }

        #endregion

        #region Markdown Export

        /// <summary>
        /// Exports analysis results to Markdown format
        /// </summary>
        public async Task<DaoResult<string>> ExportToMarkdownAsync()
        {
            try
            {
                LoggingUtility.LogInfo("[Service_AnalysisFormatter] Starting Markdown export", nameof(ExportToMarkdownAsync));

                var markdown = await Task.Run(() => GenerateMarkdownReport());

                LoggingUtility.LogInfo($"[Service_AnalysisFormatter] Markdown export completed - Size: {markdown.Length} characters", nameof(ExportToMarkdownAsync));
                return DaoResult<string>.Success(markdown);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_AnalysisFormatter] Markdown export failed: {ex.Message}", nameof(ExportToMarkdownAsync), ex);
                return DaoResult<string>.Failure($"Markdown export failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves analysis results to Markdown file
        /// </summary>
        public async Task<DaoResult<string>> SaveToMarkdownFileAsync(string filePath)
        {
            try
            {
                var markdownResult = await ExportToMarkdownAsync();
                if (!markdownResult.IsSuccess)
                {
                    return DaoResult<string>.Failure($"Failed to generate Markdown: {markdownResult.ErrorMessage}");
                }

                await File.WriteAllTextAsync(filePath, markdownResult.Data);

                LoggingUtility.LogInfo($"[Service_AnalysisFormatter] Markdown file saved: {filePath}", nameof(SaveToMarkdownFileAsync));
                return DaoResult<string>.Success(filePath);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_AnalysisFormatter] Failed to save Markdown file: {ex.Message}", nameof(SaveToMarkdownFileAsync), ex);
                return DaoResult<string>.Failure($"Failed to save Markdown file: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates comprehensive Markdown report
        /// </summary>
        private string GenerateMarkdownReport()
        {
            var sb = new StringBuilder();

            // Title and Overview
            sb.AppendLine("# MTM WIP Application Analysis Report");
            sb.AppendLine();
            sb.AppendLine($"**Analysis Date:** {_analysis.AnalysisTimestamp:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"**Application Version:** {_analysis.ApplicationVersion}");
            sb.AppendLine($"**Analysis Version:** {_analysis.AnalysisVersion}");
            sb.AppendLine();

            // Summary Statistics
            sb.AppendLine("## Summary Statistics");
            sb.AppendLine();
            var stats = _analysis.GetSummaryStatistics();
            foreach (var stat in stats)
            {
                sb.AppendLine($"- **{stat.Key}:** {stat.Value}");
            }
            sb.AppendLine();

            // UI Analysis
            GenerateUIAnalysisMarkdown(sb);

            // Database Analysis
            GenerateDatabaseAnalysisMarkdown(sb);

            // Business Logic Analysis
            GenerateBusinessLogicAnalysisMarkdown(sb);

            // Theming Analysis
            GenerateThemingAnalysisMarkdown(sb);

            // Error Handling Analysis
            GenerateErrorHandlingAnalysisMarkdown(sb);

            // Environment Analysis
            GenerateEnvironmentAnalysisMarkdown(sb);

            return sb.ToString();
        }

        /// <summary>
        /// Generates UI Analysis section for Markdown
        /// </summary>
        private void GenerateUIAnalysisMarkdown(StringBuilder sb)
        {
            sb.AppendLine("## UI Structure Analysis");
            sb.AppendLine();

            // Forms
            sb.AppendLine("### Forms");
            sb.AppendLine();
            if (_analysis.UIAnalysis.Forms.Any())
            {
                sb.AppendLine("| Form Name | Base Class | Events | Methods | Purpose |");
                sb.AppendLine("|-----------|------------|--------|---------|---------|");
                foreach (var form in _analysis.UIAnalysis.Forms)
                {
                    sb.AppendLine($"| {form.Name} | {form.BaseClass} | {form.Events.Count} | {form.Methods.Count} | {form.Purpose} |");
                }
            }
            else
            {
                sb.AppendLine("No forms analyzed.");
            }
            sb.AppendLine();

            // Controls
            sb.AppendLine("### Controls");
            sb.AppendLine();
            if (_analysis.UIAnalysis.Controls.Any())
            {
                sb.AppendLine("| Control Name | Type | Custom | Events | Purpose |");
                sb.AppendLine("|--------------|------|--------|--------|---------|");
                foreach (var control in _analysis.UIAnalysis.Controls)
                {
                    sb.AppendLine($"| {control.Name} | {control.Type} | {(control.IsCustomControl ? "Yes" : "No")} | {control.Events.Count} | {control.Purpose} |");
                }
            }
            else
            {
                sb.AppendLine("No controls analyzed.");
            }
            sb.AppendLine();

            // Navigation
            sb.AppendLine("### Navigation Structure");
            sb.AppendLine();
            sb.AppendLine($"- **Menu Items:** {_analysis.UIAnalysis.Navigation.MenuItems.Count}");
            sb.AppendLine($"- **Tab Pages:** {_analysis.UIAnalysis.Navigation.TabPages.Count}");
            sb.AppendLine();
        }

        /// <summary>
        /// Generates Database Analysis section for Markdown
        /// </summary>
        private void GenerateDatabaseAnalysisMarkdown(StringBuilder sb)
        {
            sb.AppendLine("## Database Structure Analysis");
            sb.AppendLine();

            // Configuration
            sb.AppendLine("### Database Configuration");
            sb.AppendLine();
            sb.AppendLine($"- **Server:** {_analysis.DatabaseAnalysis.Configuration.ServerAddress}");
            sb.AppendLine($"- **Database:** {_analysis.DatabaseAnalysis.Configuration.DatabaseName}");
            sb.AppendLine($"- **Version:** {_analysis.DatabaseAnalysis.Configuration.Version}");
            sb.AppendLine();

            // Stored Procedures
            sb.AppendLine("### Stored Procedures");
            sb.AppendLine();
            if (_analysis.DatabaseAnalysis.StoredProcedures.Any())
            {
                sb.AppendLine("| Procedure Name | Parameters | Purpose |");
                sb.AppendLine("|----------------|------------|---------|");
                foreach (var sp in _analysis.DatabaseAnalysis.StoredProcedures)
                {
                    sb.AppendLine($"| {sp.Name} | {sp.Parameters.Count} | {sp.Purpose} |");
                }
            }
            else
            {
                sb.AppendLine("No stored procedures analyzed.");
            }
            sb.AppendLine();

            // Tables
            sb.AppendLine("### Tables");
            sb.AppendLine();
            if (_analysis.DatabaseAnalysis.Tables.Any())
            {
                sb.AppendLine("| Table Name | Columns | Purpose |");
                sb.AppendLine("|------------|---------|---------|");
                foreach (var table in _analysis.DatabaseAnalysis.Tables)
                {
                    sb.AppendLine($"| {table.Name} | {table.Columns.Count} | {table.Purpose} |");
                }
            }
            else
            {
                sb.AppendLine("No tables analyzed.");
            }
            sb.AppendLine();
        }

        /// <summary>
        /// Generates Business Logic Analysis section for Markdown
        /// </summary>
        private void GenerateBusinessLogicAnalysisMarkdown(StringBuilder sb)
        {
            sb.AppendLine("## Business Logic Analysis");
            sb.AppendLine();

            // Classes by Category
            sb.AppendLine("### Classes by Category");
            sb.AppendLine();
            var classesByCategory = _analysis.BusinessLogicAnalysis.Classes.GroupBy(c => c.Category);
            foreach (var group in classesByCategory)
            {
                sb.AppendLine($"#### {group.Key}");
                sb.AppendLine();
                sb.AppendLine("| Class Name | Methods | Purpose |");
                sb.AppendLine("|------------|---------|---------|");
                foreach (var cls in group)
                {
                    sb.AppendLine($"| {cls.Name} | {cls.Methods.Count} | {cls.Purpose} |");
                }
                sb.AppendLine();
            }

            // Services
            sb.AppendLine("### Services");
            sb.AppendLine();
            if (_analysis.BusinessLogicAnalysis.Services.Any())
            {
                sb.AppendLine("| Service Name | Methods | Purpose |");
                sb.AppendLine("|--------------|---------|---------|");
                foreach (var service in _analysis.BusinessLogicAnalysis.Services)
                {
                    sb.AppendLine($"| {service.Name} | {service.PublicMethods.Count} | {service.Purpose} |");
                }
            }
            else
            {
                sb.AppendLine("No services analyzed.");
            }
            sb.AppendLine();

            // Helpers
            sb.AppendLine("### Helper Classes");
            sb.AppendLine();
            if (_analysis.BusinessLogicAnalysis.Helpers.Any())
            {
                sb.AppendLine("| Helper Name | Category | Methods | Purpose |");
                sb.AppendLine("|-------------|----------|---------|---------|");
                foreach (var helper in _analysis.BusinessLogicAnalysis.Helpers)
                {
                    sb.AppendLine($"| {helper.Name} | {helper.Category} | {helper.UtilityMethods.Count} | {helper.Purpose} |");
                }
            }
            else
            {
                sb.AppendLine("No helpers analyzed.");
            }
            sb.AppendLine();
        }

        /// <summary>
        /// Generates Theming Analysis section for Markdown
        /// </summary>
        private void GenerateThemingAnalysisMarkdown(StringBuilder sb)
        {
            sb.AppendLine("## Theming System Analysis");
            sb.AppendLine();

            // DPI Scaling
            sb.AppendLine("### DPI Scaling");
            sb.AppendLine();
            sb.AppendLine($"- **Enabled:** {(_analysis.ThemingAnalysis.DpiScaling.IsEnabled ? "Yes" : "No")}");
            sb.AppendLine($"- **Scaling Methods:** {_analysis.ThemingAnalysis.DpiScaling.ScalingMethods.Count}");
            sb.AppendLine();

            // Theme Files
            sb.AppendLine("### Theme Files");
            sb.AppendLine();
            if (_analysis.ThemingAnalysis.ThemeFiles.Any())
            {
                sb.AppendLine("| File Name | Methods | Purpose |");
                sb.AppendLine("|-----------|---------|---------|");
                foreach (var themeFile in _analysis.ThemingAnalysis.ThemeFiles)
                {
                    sb.AppendLine($"| {themeFile.Name} | {themeFile.ThemeMethods.Count} | {themeFile.Purpose} |");
                }
            }
            else
            {
                sb.AppendLine("No theme files analyzed.");
            }
            sb.AppendLine();

            // Style Patterns
            sb.AppendLine("### Style Patterns");
            sb.AppendLine();
            foreach (var pattern in _analysis.ThemingAnalysis.StylePatterns)
            {
                sb.AppendLine($"#### {pattern.PatternName}");
                sb.AppendLine($"**Description:** {pattern.Description}");
                sb.AppendLine($"**Usage:** {pattern.Usage}");
                sb.AppendLine();
            }
        }

        /// <summary>
        /// Generates Error Handling Analysis section for Markdown
        /// </summary>
        private void GenerateErrorHandlingAnalysisMarkdown(StringBuilder sb)
        {
            sb.AppendLine("## Error Handling Analysis");
            sb.AppendLine();

            // Error Handlers
            sb.AppendLine("### Error Handlers");
            sb.AppendLine();
            if (_analysis.ErrorHandlingAnalysis.ErrorHandlers.Any())
            {
                sb.AppendLine("| Handler Name | Handled Exceptions | Purpose |");
                sb.AppendLine("|--------------|-------------------|---------|");
                foreach (var handler in _analysis.ErrorHandlingAnalysis.ErrorHandlers)
                {
                    sb.AppendLine($"| {handler.Name} | {handler.HandledExceptions.Count} | {handler.Purpose} |");
                }
            }
            else
            {
                sb.AppendLine("No error handlers analyzed.");
            }
            sb.AppendLine();

            // Exception Patterns
            sb.AppendLine("### Exception Patterns");
            sb.AppendLine();
            foreach (var pattern in _analysis.ErrorHandlingAnalysis.ExceptionPatterns)
            {
                sb.AppendLine($"#### {pattern.PatternName}");
                sb.AppendLine($"**Description:** {pattern.Description}");
                sb.AppendLine($"**Usage:** {pattern.Usage}");
                sb.AppendLine();
            }

            // Logging Configuration
            sb.AppendLine("### Logging Configuration");
            sb.AppendLine();
            sb.AppendLine($"- **Framework:** {_analysis.ErrorHandlingAnalysis.LoggingConfiguration.LoggingFramework}");
            sb.AppendLine($"- **Log Levels:** {string.Join(", ", _analysis.ErrorHandlingAnalysis.LoggingConfiguration.LogLevels)}");
            sb.AppendLine($"- **Log Targets:** {string.Join(", ", _analysis.ErrorHandlingAnalysis.LoggingConfiguration.LogTargets)}");
            sb.AppendLine();
        }

        /// <summary>
        /// Generates Environment Analysis section for Markdown
        /// </summary>
        private void GenerateEnvironmentAnalysisMarkdown(StringBuilder sb)
        {
            sb.AppendLine("## Environment Configuration Analysis");
            sb.AppendLine();

            // Dependencies
            sb.AppendLine("### Dependencies");
            sb.AppendLine();
            if (_analysis.EnvironmentAnalysis.Dependencies.Any())
            {
                sb.AppendLine("| Name | Version | Type | Purpose |");
                sb.AppendLine("|------|---------|------|---------|");
                foreach (var dependency in _analysis.EnvironmentAnalysis.Dependencies)
                {
                    sb.AppendLine($"| {dependency.Name} | {dependency.Version} | {dependency.Type} | {dependency.Purpose} |");
                }
            }
            else
            {
                sb.AppendLine("No dependencies analyzed.");
            }
            sb.AppendLine();

            // Configuration
            sb.AppendLine("### Configuration Settings");
            sb.AppendLine();
            if (_analysis.EnvironmentAnalysis.Configuration.AppSettings.Any())
            {
                sb.AppendLine("#### Application Settings");
                foreach (var setting in _analysis.EnvironmentAnalysis.Configuration.AppSettings)
                {
                    sb.AppendLine($"- **{setting.Key}:** {setting.Value}");
                }
                sb.AppendLine();
            }

            if (_analysis.EnvironmentAnalysis.Configuration.ConnectionStrings.Any())
            {
                sb.AppendLine("#### Connection Strings");
                foreach (var connection in _analysis.EnvironmentAnalysis.Configuration.ConnectionStrings)
                {
                    sb.AppendLine($"- **{connection.Key}:** {connection.Value}");
                }
                sb.AppendLine();
            }

            // Deployment
            sb.AppendLine("### Deployment Information");
            sb.AppendLine();
            sb.AppendLine($"- **Deployment Type:** {_analysis.EnvironmentAnalysis.Deployment.DeploymentType}");
            sb.AppendLine($"- **Required Files:** {_analysis.EnvironmentAnalysis.Deployment.RequiredFiles.Count}");
            sb.AppendLine();
        }

        #endregion

        #region HTML Export

        /// <summary>
        /// Exports analysis results to HTML format
        /// </summary>
        public async Task<DaoResult<string>> ExportToHtmlAsync()
        {
            try
            {
                LoggingUtility.LogInfo("[Service_AnalysisFormatter] Starting HTML export", nameof(ExportToHtmlAsync));

                var html = await Task.Run(() => GenerateHtmlReport());

                LoggingUtility.LogInfo($"[Service_AnalysisFormatter] HTML export completed - Size: {html.Length} characters", nameof(ExportToHtmlAsync));
                return DaoResult<string>.Success(html);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_AnalysisFormatter] HTML export failed: {ex.Message}", nameof(ExportToHtmlAsync), ex);
                return DaoResult<string>.Failure($"HTML export failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves analysis results to HTML file
        /// </summary>
        public async Task<DaoResult<string>> SaveToHtmlFileAsync(string filePath)
        {
            try
            {
                var htmlResult = await ExportToHtmlAsync();
                if (!htmlResult.IsSuccess)
                {
                    return DaoResult<string>.Failure($"Failed to generate HTML: {htmlResult.ErrorMessage}");
                }

                await File.WriteAllTextAsync(filePath, htmlResult.Data);

                LoggingUtility.LogInfo($"[Service_AnalysisFormatter] HTML file saved: {filePath}", nameof(SaveToHtmlFileAsync));
                return DaoResult<string>.Success(filePath);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_AnalysisFormatter] Failed to save HTML file: {ex.Message}", nameof(SaveToHtmlFileAsync), ex);
                return DaoResult<string>.Failure($"Failed to save HTML file: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates interactive HTML report
        /// </summary>
        private string GenerateHtmlReport()
        {
            var sb = new StringBuilder();

            // HTML Header
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"UTF-8\">");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine("    <title>MTM WIP Application Analysis Report</title>");
            sb.AppendLine("    <style>");
            sb.AppendLine(GetHtmlStyles());
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            // Title and Overview
            sb.AppendLine("    <div class=\"container\">");
            sb.AppendLine("        <header>");
            sb.AppendLine("            <h1>MTM WIP Application Analysis Report</h1>");
            sb.AppendLine($"            <p class=\"subtitle\">Generated on {_analysis.AnalysisTimestamp:yyyy-MM-dd HH:mm:ss}</p>");
            sb.AppendLine("        </header>");

            // Navigation
            sb.AppendLine("        <nav>");
            sb.AppendLine("            <ul>");
            sb.AppendLine("                <li><a href=\"#summary\">Summary</a></li>");
            sb.AppendLine("                <li><a href=\"#ui\">UI Analysis</a></li>");
            sb.AppendLine("                <li><a href=\"#database\">Database</a></li>");
            sb.AppendLine("                <li><a href=\"#business\">Business Logic</a></li>");
            sb.AppendLine("                <li><a href=\"#theming\">Theming</a></li>");
            sb.AppendLine("                <li><a href=\"#errors\">Error Handling</a></li>");
            sb.AppendLine("                <li><a href=\"#environment\">Environment</a></li>");
            sb.AppendLine("            </ul>");
            sb.AppendLine("        </nav>");

            // Summary Section
            GenerateHtmlSummarySection(sb);

            // Other sections (simplified for initial implementation)
            sb.AppendLine("        <section id=\"ui\">");
            sb.AppendLine("            <h2>UI Structure Analysis</h2>");
            sb.AppendLine($"            <p>Forms: {_analysis.UIAnalysis.Forms.Count}, Controls: {_analysis.UIAnalysis.Controls.Count}</p>");
            sb.AppendLine("        </section>");

            sb.AppendLine("        <section id=\"database\">");
            sb.AppendLine("            <h2>Database Structure Analysis</h2>");
            sb.AppendLine($"            <p>Stored Procedures: {_analysis.DatabaseAnalysis.StoredProcedures.Count}, Tables: {_analysis.DatabaseAnalysis.Tables.Count}</p>");
            sb.AppendLine("        </section>");

            // Footer
            sb.AppendLine("        <footer>");
            sb.AppendLine($"            <p>Report generated by MTM WIP Application Analyzer v{_analysis.AnalysisVersion}</p>");
            sb.AppendLine("        </footer>");
            sb.AppendLine("    </div>");

            // JavaScript
            sb.AppendLine("    <script>");
            sb.AppendLine(GetHtmlScripts());
            sb.AppendLine("    </script>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        /// <summary>
        /// Generates HTML summary section
        /// </summary>
        private void GenerateHtmlSummarySection(StringBuilder sb)
        {
            sb.AppendLine("        <section id=\"summary\">");
            sb.AppendLine("            <h2>Summary Statistics</h2>");
            sb.AppendLine("            <div class=\"stats-grid\">");

            var stats = _analysis.GetSummaryStatistics();
            foreach (var stat in stats)
            {
                if (stat.Value is int || stat.Value is long)
                {
                    sb.AppendLine("                <div class=\"stat-card\">");
                    sb.AppendLine($"                    <h3>{stat.Value}</h3>");
                    sb.AppendLine($"                    <p>{stat.Key}</p>");
                    sb.AppendLine("                </div>");
                }
            }

            sb.AppendLine("            </div>");
            sb.AppendLine("        </section>");
        }

        /// <summary>
        /// Gets CSS styles for HTML report
        /// </summary>
        private string GetHtmlStyles()
        {
            return @"
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0; padding: 0; background-color: #f5f5f5; }
        .container { max-width: 1200px; margin: 0 auto; background-color: white; box-shadow: 0 0 10px rgba(0,0,0,0.1); }
        header { background-color: #2c3e50; color: white; padding: 20px; text-align: center; }
        header h1 { margin: 0; font-size: 2.5em; }
        .subtitle { margin: 10px 0 0 0; opacity: 0.8; }
        nav { background-color: #34495e; padding: 0; }
        nav ul { list-style: none; margin: 0; padding: 0; display: flex; }
        nav li { flex: 1; }
        nav a { display: block; padding: 15px; color: white; text-decoration: none; text-align: center; }
        nav a:hover { background-color: #4a6741; }
        section { padding: 20px; border-bottom: 1px solid #eee; }
        section h2 { color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 10px; }
        .stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 20px; margin-top: 20px; }
        .stat-card { background-color: #f8f9fa; padding: 20px; border-radius: 8px; text-align: center; border-left: 4px solid #3498db; }
        .stat-card h3 { font-size: 2em; margin: 0; color: #2c3e50; }
        .stat-card p { margin: 10px 0 0 0; color: #7f8c8d; }
        footer { background-color: #ecf0f1; padding: 20px; text-align: center; color: #7f8c8d; }
        ";
        }

        /// <summary>
        /// Gets JavaScript for HTML report
        /// </summary>
        private string GetHtmlScripts()
        {
            return @"
        // Smooth scrolling for navigation links
        document.querySelectorAll('nav a').forEach(anchor => {
            anchor.addEventListener('click', function (e) {
                e.preventDefault();
                const target = document.querySelector(this.getAttribute('href'));
                target.scrollIntoView({ behavior: 'smooth' });
            });
        });
        ";
        }

        #endregion

        #region Summary Export

        /// <summary>
        /// Exports a quick summary of analysis results
        /// </summary>
        public async Task<DaoResult<string>> ExportSummaryAsync()
        {
            try
            {
                LoggingUtility.LogInfo("[Service_AnalysisFormatter] Starting summary export", nameof(ExportSummaryAsync));

                var summary = await Task.Run(() => GenerateSummaryReport());

                LoggingUtility.LogInfo($"[Service_AnalysisFormatter] Summary export completed", nameof(ExportSummaryAsync));
                return DaoResult<string>.Success(summary);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_AnalysisFormatter] Summary export failed: {ex.Message}", nameof(ExportSummaryAsync), ex);
                return DaoResult<string>.Failure($"Summary export failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates a concise summary report
        /// </summary>
        private string GenerateSummaryReport()
        {
            var sb = new StringBuilder();

            sb.AppendLine("MTM WIP Application Analysis Summary");
            sb.AppendLine("=====================================");
            sb.AppendLine();
            sb.AppendLine($"Analysis Date: {_analysis.AnalysisTimestamp:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Application Version: {_analysis.ApplicationVersion}");
            sb.AppendLine();

            var stats = _analysis.GetSummaryStatistics();
            sb.AppendLine("Key Statistics:");
            foreach (var stat in stats)
            {
                sb.AppendLine($"  {stat.Key}: {stat.Value}");
            }

            sb.AppendLine();
            sb.AppendLine("MAUI Migration Readiness Assessment:");
            sb.AppendLine($"  - UI Components: {_analysis.UIAnalysis.Forms.Count + _analysis.UIAnalysis.Controls.Count} items to migrate");
            sb.AppendLine($"  - Database Layer: {_analysis.DatabaseAnalysis.StoredProcedures.Count} stored procedures identified");
            sb.AppendLine($"  - Business Logic: {_analysis.BusinessLogicAnalysis.Classes.Count} classes to review");
            sb.AppendLine($"  - Error Handling: {(_analysis.ErrorHandlingAnalysis.ErrorHandlers.Any() ? "Centralized system detected" : "Needs review")}");
            sb.AppendLine($"  - Theming System: {(_analysis.ThemingAnalysis.DpiScaling.IsEnabled ? "Advanced theming with DPI scaling" : "Basic theming")}");

            return sb.ToString();
        }

        #endregion
    }

    #endregion
}