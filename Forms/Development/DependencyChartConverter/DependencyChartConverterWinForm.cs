using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Forms.Development.DependencyChartConverter
{
    #region DependencyChartConverter

    public partial class DependencyChartConverterForm : Form
    {
        #region Fields

        private string? _basePath;
        private string? _templatePath;
        private string? _chartsPath;
        private string? _outputDir;

        #endregion

        #region Properties

        public bool IsInitialized { get; private set; }

        #endregion

        #region Constructors

        public DependencyChartConverterForm()
        {
            try
            {
                InitializeComponent();
                Core_Themes.ApplyDpiScaling(this);
                Core_Themes.ApplyRuntimeLayoutAdjustments(this);
                InitializeForm();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, 
                    controlName: nameof(DependencyChartConverterForm));
            }
        }

        #endregion

        #region Progress Control Methods

        // Progress control methods would go here if needed for this form
        // Currently not required for DependencyChartConverter functionality

        #endregion

        #region Initialization

        private void InitializeForm()
        {
            try
            {
                IsInitialized = true;
                this.Text = "MTM Inventory Application - Dependency Chart Converter";
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(DependencyChartConverterForm));
            }
        }

        #endregion

        #region Key Processing

        // Keyboard shortcut processing would go here if needed
        // Currently not implemented for this development tool

        #endregion

        #region Button Clicks

        private void btnSelectBasePath_Click(object sender, EventArgs e)
        {
            try
            {
                using var fbd = new FolderBrowserDialog();
                fbd.Description = "Select the base path containing the Documentation folder";
                
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _basePath = fbd.SelectedPath;
                    txtBasePath.Text = _basePath;
                    _chartsPath = Path.Combine(_basePath, "Documentation", "Dependency Charts");
                    _templatePath = Path.Combine(_chartsPath, "Templates", "chart-template.html");
                    _outputDir = Path.Combine(_chartsPath, "HTML");
                    
                    ValidateDirectories();
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(DependencyChartConverterForm));
            }
        }

        private void btnConvertCharts_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                {
                    return;
                }

                txtOutput.Clear();
                txtOutput.AppendText("🚀 Starting HTML chart conversion...\r\n");

                var conversionResult = PerformConversion();
                
                if (conversionResult)
                {
                    txtOutput.AppendText("\r\n✅ Conversion completed successfully!\r\n");
                }
                else
                {
                    txtOutput.AppendText("\r\n❌ Conversion completed with errors.\r\n");
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, 
                    retryAction: () => { btnConvertCharts_Click(sender, e); return true; },
                    controlName: nameof(DependencyChartConverterForm));
            }
        }

        #endregion

        #region ComboBox & UI Events

        // UI event handlers would go here if needed
        // Currently not implemented for this development tool

        #endregion

        #region Chart Processing

        private bool PerformConversion()
        {
            try
            {
                if (string.IsNullOrEmpty(_outputDir) || string.IsNullOrEmpty(_chartsPath))
                {
                    return false;
                }

                Directory.CreateDirectory(_outputDir);
                var mdFiles = GetMarkdownFiles();

                int convertedCount = 0;
                int errorCount = 0;

                foreach (var mdFile in mdFiles)
                {
                    try
                    {
                        ConvertFile(mdFile, _outputDir);
                        txtOutput.AppendText($"✅ Converted: {Path.GetFileName(mdFile)}\r\n");
                        convertedCount++;
                        Application.DoEvents(); // Keep UI responsive
                    }
                    catch (Exception ex)
                    {
                        txtOutput.AppendText($"❌ Error converting {Path.GetFileName(mdFile)}: {ex.Message}\r\n");
                        errorCount++;
                        Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                            contextData: new Dictionary<string, object> { ["FilePath"] = mdFile },
                            controlName: nameof(DependencyChartConverterForm));
                    }
                }

                txtOutput.AppendText($"\r\n📊 Results: {convertedCount} converted, {errorCount} errors\r\n");
                txtOutput.AppendText($"📁 Output directory: {_outputDir}\r\n");

                return errorCount == 0;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, 
                    controlName: nameof(DependencyChartConverterForm));
                return false;
            }
        }

        private List<string> GetMarkdownFiles()
        {
            try
            {
                if (string.IsNullOrEmpty(_chartsPath) || !Directory.Exists(_chartsPath))
                {
                    return new List<string>();
                }

                var mdFiles = new List<string>();
                foreach (var file in Directory.EnumerateFiles(_chartsPath, "*.md", SearchOption.AllDirectories))
                {
                    var fileName = Path.GetFileName(file);
                    if (!file.Contains("Templates") && 
                        fileName != "README.md" && 
                        fileName != "ANALYSIS_REPORT.md")
                    {
                        mdFiles.Add(file);
                    }
                }
                return mdFiles;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(DependencyChartConverterForm));
                return new List<string>();
            }
        }

        private string ConvertFile(string mdFilePath, string outputDir)
        {
            try
            {
                string template = LoadTemplate();
                var data = ParseMarkdownChart(mdFilePath);
                string html = RenderHtml(template, data);

                if (string.IsNullOrEmpty(_chartsPath))
                {
                    throw new InvalidOperationException("Charts path not initialized");
                }

                var relPath = Path.GetRelativePath(_chartsPath, mdFilePath);
                var htmlPath = Path.Combine(outputDir, Path.ChangeExtension(relPath, ".html"));
                var directory = Path.GetDirectoryName(htmlPath);

                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(htmlPath, html);
                return htmlPath;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleFileError(ex, mdFilePath, 
                    controlName: nameof(DependencyChartConverterForm));
                throw;
            }
        }

        #endregion

        #region Template Processing

        private string LoadTemplate()
        {
            try
            {
                if (string.IsNullOrEmpty(_templatePath) || !File.Exists(_templatePath))
                {
                    throw new FileNotFoundException($"HTML template not found at: {_templatePath}");
                }
                return File.ReadAllText(_templatePath);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleFileError(ex, _templatePath ?? "", 
                    controlName: nameof(DependencyChartConverterForm));
                throw;
            }
        }

        private Dictionary<string, object> ParseMarkdownChart(string mdFilePath)
        {
            try
            {
                var content = File.ReadAllText(mdFilePath);
                var data = new Dictionary<string, object>
                {
                    { "file_name", "" },
                    { "file_path", "" },
                    { "file_type", "Unknown" },
                    { "complexity", "Medium" },
                    { "priority", "MEDIUM" },
                    { "dependency_count", "0" },
                    { "internal_dependencies", new List<string>() },
                    { "external_dependencies", new List<string>() },
                    { "compliance_items", new List<Tuple<string, string, string>>() },
                    { "refactor_actions", new List<string>() }
                };

                ExtractBasicInfo(content, data);
                ExtractDependencies(content, data);
                ExtractComplianceInfo(content, data);
                ExtractRefactorActions(content, data);
                CalculateDependencyCount(data);

                return data;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleFileError(ex, mdFilePath, 
                    controlName: nameof(DependencyChartConverterForm));
                throw;
            }
        }

        private string RenderHtml(string template, Dictionary<string, object> data)
        {
            try
            {
                string html = template;
                
                // Basic replacements
                html = html.Replace("{{FILE_NAME}}", data["file_name"].ToString() ?? "");
                html = html.Replace("{{FILE_PATH}}", data["file_path"].ToString() ?? "");
                html = html.Replace("{{GENERATION_DATE}}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss 'UTC'"));
                html = html.Replace("{{FILE_TYPE}}", data["file_type"].ToString() ?? "");
                html = html.Replace("{{COMPLEXITY}}", data["complexity"].ToString() ?? "");
                html = html.Replace("{{PRIORITY}}", data["priority"].ToString() ?? "");
                html = html.Replace("{{PRIORITY_CLASS}}", (data["priority"].ToString() ?? "").ToLower());
                html = html.Replace("{{DEPENDENCY_COUNT}}", data["dependency_count"].ToString() ?? "0");

                // Complex replacements
                html = RenderDependencies(html, data);
                html = RenderComplianceItems(html, data);
                html = RenderRefactorActions(html, data);

                return html;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(DependencyChartConverterForm));
                throw;
            }
        }

        #endregion

        #region Validation

        private bool ValidateInputs()
        {
            try
            {
                if (string.IsNullOrEmpty(_basePath) || !Directory.Exists(_basePath))
                {
                    Service_ErrorHandler.HandleValidationError("Please select a valid base path.", 
                        "Base Path", controlName: nameof(DependencyChartConverterForm));
                    return false;
                }

                return ValidateDirectories();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(DependencyChartConverterForm));
                return false;
            }
        }

        private bool ValidateDirectories()
        {
            try
            {
                if (string.IsNullOrEmpty(_chartsPath) || !Directory.Exists(_chartsPath))
                {
                    Service_ErrorHandler.HandleValidationError(
                        "Documentation/Dependency Charts folder not found in selected path.",
                        "Charts Directory", controlName: nameof(DependencyChartConverterForm));
                    return false;
                }

                if (string.IsNullOrEmpty(_templatePath) || !File.Exists(_templatePath))
                {
                    Service_ErrorHandler.HandleValidationError(
                        "chart-template.html not found in Templates folder.",
                        "Template File", controlName: nameof(DependencyChartConverterForm));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(DependencyChartConverterForm));
                return false;
            }
        }

        #endregion

        #region Helpers

        private static void ExtractBasicInfo(string content, Dictionary<string, object> data)
        {
            var fileMatch = Regex.Match(content, @"# Dependency Chart: (.+)");
            if (fileMatch.Success)
                data["file_name"] = fileMatch.Groups[1].Value;

            var pathMatch = Regex.Match(content, @"\*\*File Path\*\*: `(.+)`");
            if (pathMatch.Success)
                data["file_path"] = pathMatch.Groups[1].Value;

            var typeMatch = Regex.Match(content, @"- \*\*Type\*\*: (.+)");
            if (typeMatch.Success)
                data["file_type"] = typeMatch.Groups[1].Value;

            var complexityMatch = Regex.Match(content, @"- \*\*Complexity\*\*: (.+)");
            if (complexityMatch.Success)
                data["complexity"] = complexityMatch.Groups[1].Value;

            var priorityMatch = Regex.Match(content, @"- \*\*Priority\*\*: (.+)");
            if (priorityMatch.Success)
                data["priority"] = priorityMatch.Groups[1].Value;
        }

        private static void ExtractDependencies(string content, Dictionary<string, object> data)
        {
            // Internal dependencies
            var internalSection = Regex.Match(content, @"### Internal Dependencies\n(.*?)### External Dependencies", RegexOptions.Singleline);
            if (internalSection.Success)
            {
                var internalDeps = Regex.Matches(internalSection.Groups[1].Value, @"- ✅ `([^`]+)`");
                var list = (List<string>)data["internal_dependencies"];
                foreach (Match m in internalDeps)
                    list.Add(m.Groups[1].Value);
            }

            // External dependencies
            var externalSection = Regex.Match(content, @"### External Dependencies\n(.*?)## Direct Dependents", RegexOptions.Singleline);
            if (externalSection.Success)
            {
                var externalDeps = Regex.Matches(externalSection.Groups[1].Value, @"- 📦 `([^`]+)`");
                var list = (List<string>)data["external_dependencies"];
                foreach (Match m in externalDeps)
                    list.Add(m.Groups[1].Value);
            }
        }

        private static void ExtractComplianceInfo(string content, Dictionary<string, object> data)
        {
            var complianceSection = Regex.Match(content, @"## Compliance Status\n(.*?)## Refactor Priority", RegexOptions.Singleline);
            if (complianceSection.Success)
            {
                var complianceItems = (List<Tuple<string, string, string>>)data["compliance_items"];
                var failItems = Regex.Matches(complianceSection.Groups[1].Value, @"- \*\*([^*]+)\*\*: FAIL - (.+)");
                foreach (Match m in failItems)
                    complianceItems.Add(Tuple.Create("fail", m.Groups[1].Value, m.Groups[2].Value));
                var analyzeItems = Regex.Matches(complianceSection.Groups[1].Value, @"- \*\*([^*]+)\*\*: TO_ANALYZE - (.+)");
                foreach (Match m in analyzeItems)
                    complianceItems.Add(Tuple.Create("analyze", m.Groups[1].Value, m.Groups[2].Value));
            }
        }

        private static void ExtractRefactorActions(string content, Dictionary<string, object> data)
        {
            var actionsSection = Regex.Match(content, @"## Refactor Actions Required\n(.*?)(?=##|$)", RegexOptions.Singleline);
            if (actionsSection.Success)
            {
                var actions = Regex.Matches(actionsSection.Groups[1].Value, @"\d+\. \*\*(.+?)\*\*");
                var list = (List<string>)data["refactor_actions"];
                foreach (Match m in actions)
                    list.Add(m.Groups[1].Value);
            }
        }

        private static void CalculateDependencyCount(Dictionary<string, object> data)
        {
            var internalCount = ((List<string>)data["internal_dependencies"]).Count;
            var externalCount = ((List<string>)data["external_dependencies"]).Count;
            data["dependency_count"] = (internalCount + externalCount).ToString();
        }

        private static string RenderDependencies(string html, Dictionary<string, object> data)
        {
            // Internal dependencies
            string internalDepsHtml = "";
            foreach (string dep in (List<string>)data["internal_dependencies"])
                internalDepsHtml += $"<div class=\"dependency-item\"><span class=\"icon\">✅</span>{dep}</div>\n";
            html = html.Replace("{{INTERNAL_DEPENDENCIES}}", internalDepsHtml);

            // External dependencies
            string externalDepsHtml = "";
            foreach (string dep in (List<string>)data["external_dependencies"])
                externalDepsHtml += $"<div class=\"dependency-item external\"><span class=\"icon\">📦</span>{dep}</div>\n";
            html = html.Replace("{{EXTERNAL_DEPENDENCIES}}", externalDepsHtml);

            return html;
        }

        private static string RenderComplianceItems(string html, Dictionary<string, object> data)
        {
            string complianceHtml = "";
            foreach (var tuple in (List<Tuple<string, string, string>>)data["compliance_items"])
            {
                string icon = tuple.Item1 == "fail" ? "❌" : "⚠️";
                complianceHtml += $@"
                <div class=""status-item {tuple.Item1}"">
                    <span class=""status-icon"">{icon}</span>
                    <div>
                        <strong>{tuple.Item2}</strong><br>
                        <small>{tuple.Item3}</small>
                    </div>
                </div>
                ";
            }
            html = html.Replace("{{COMPLIANCE_ITEMS}}", complianceHtml);
            return html;
        }

        private static string RenderRefactorActions(string html, Dictionary<string, object> data)
        {
            string actionsHtml = "";
            foreach (string act in (List<string>)data["refactor_actions"])
                actionsHtml += $"<li>{act}</li>\n";
            html = html.Replace("{{REFACTOR_ACTIONS}}", actionsHtml);
            return html;
        }

        #endregion

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    // Dispose managed resources if any
                }
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                    controlName: nameof(DependencyChartConverterForm));
            }
        }

        #endregion
    }

    #endregion
}
