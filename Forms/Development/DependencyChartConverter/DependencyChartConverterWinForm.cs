using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Org.BouncyCastle.Math.Primes;

namespace MTM_Inventory_Application.Forms.Development.DependencyChartConverter
{
    public partial class DependencyChartConverterForm : Form
    {
        private string basePath;
        private string templatePath;
        private string chartsPath;
        private string outputDir;

        public DependencyChartConverterForm()
        {
            InitializeComponent();
        }

        private void btnSelectBasePath_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    basePath = fbd.SelectedPath;
                    txtBasePath.Text = basePath;
                    chartsPath = Path.Combine(basePath, "Documentation", "Dependency Charts");
                    templatePath = Path.Combine(chartsPath, "Templates", "chart-template.html");
                    outputDir = Path.Combine(chartsPath, "HTML");
                }
            }
        }

        private void btnConvertCharts_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(basePath) || !Directory.Exists(basePath))
            {
                MessageBox.Show("Please select a valid base path.");
                return;
            }

            txtOutput.Clear();
            txtOutput.AppendText("🚀 Starting HTML chart conversion...\r\n");

            try
            {
                Directory.CreateDirectory(outputDir);
                var mdFiles = new List<string>();
                foreach (var file in Directory.EnumerateFiles(chartsPath, "*.md", SearchOption.AllDirectories))
                {
                    if (!file.Contains("Templates") && Path.GetFileName(file) != "README.md" && Path.GetFileName(file) != "ANALYSIS_REPORT.md")
                        mdFiles.Add(file);
                }

                int convertedCount = 0;
                foreach (var mdFile in mdFiles)
                {
                    try
                    {
                        string htmlPath = ConvertFile(mdFile, outputDir);
                        txtOutput.AppendText($"✅ Converted: {Path.GetFileName(mdFile)}\r\n");
                        convertedCount++;
                    }
                    catch (Exception ex)
                    {
                        txtOutput.AppendText($"❌ Error converting {Path.GetFileName(mdFile)}: {ex.Message}\r\n");
                    }
                }

                txtOutput.AppendText($"\r\n✅ Conversion complete! Generated {convertedCount} HTML charts\r\n");
                txtOutput.AppendText($"📁 Output directory: {outputDir}\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string LoadTemplate()
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("HTML template not found.");
            return File.ReadAllText(templatePath);
        }

        private Dictionary<string, object> ParseMarkdownChart(string mdFilePath)
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

            // Extract file name and path
            var fileMatch = Regex.Match(content, @"# Dependency Chart: (.+)");
            if (fileMatch.Success)
                data["file_name"] = fileMatch.Groups[1].Value;

            var pathMatch = Regex.Match(content, @"\*\*File Path\*\*: `(.+)`");
            if (pathMatch.Success)
                data["file_path"] = pathMatch.Groups[1].Value;

            // Extract file type
            var typeMatch = Regex.Match(content, @"- \*\*Type\*\*: (.+)");
            if (typeMatch.Success)
                data["file_type"] = typeMatch.Groups[1].Value;

            // Extract complexity
            var complexityMatch = Regex.Match(content, @"- \*\*Complexity\*\*: (.+)");
            if (complexityMatch.Success)
                data["complexity"] = complexityMatch.Groups[1].Value;

            // Extract priority
            var priorityMatch = Regex.Match(content, @"- \*\*Priority\*\*: (.+)");
            if (priorityMatch.Success)
                data["priority"] = priorityMatch.Groups[1].Value;

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

            // Compliance items
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

            // Refactor actions
            var actionsSection = Regex.Match(content, @"## Refactor Actions Required\n(.*?)(?=##|$)", RegexOptions.Singleline);
            if (actionsSection.Success)
            {
                var actions = Regex.Matches(actionsSection.Groups[1].Value, @"\d+\. \*\*(.+?)\*\*");
                var list = (List<string>)data["refactor_actions"];
                foreach (Match m in actions)
                    list.Add(m.Groups[1].Value);
            }

            var depCount = ((List<string>)data["internal_dependencies"]).Count + ((List<string>)data["external_dependencies"]).Count;
            data["dependency_count"] = depCount.ToString();

            return data;
        }

        private string RenderHtml(string template, Dictionary<string, object> data)
        {
            string html = template;
            html = html.Replace("{{FILE_NAME}}", data["file_name"].ToString());
            html = html.Replace("{{FILE_PATH}}", data["file_path"].ToString());
            html = html.Replace("{{GENERATION_DATE}}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss 'UTC'"));
            html = html.Replace("{{FILE_TYPE}}", data["file_type"].ToString());
            html = html.Replace("{{COMPLEXITY}}", data["complexity"].ToString());
            html = html.Replace("{{PRIORITY}}", data["priority"].ToString());
            html = html.Replace("{{PRIORITY_CLASS}}", data["priority"].ToString().ToLower());
            html = html.Replace("{{DEPENDENCY_COUNT}}", data["dependency_count"].ToString());

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

            // Compliance items
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

            // Refactor actions
            string actionsHtml = "";
            foreach (string act in (List<string>)data["refactor_actions"])
                actionsHtml += $"<li>{act}</li>\n";
            html = html.Replace("{{REFACTOR_ACTIONS}}", actionsHtml);

            return html;
        }

        private string ConvertFile(string mdFilePath, string outputDir)
        {
            string template = LoadTemplate();
            var data = ParseMarkdownChart(mdFilePath);
            string html = RenderHtml(template, data);

            var relPath = Path.GetRelativePath(chartsPath, mdFilePath);
            var htmlPath = Path.Combine(outputDir, Path.ChangeExtension(relPath, ".html"));

            Directory.CreateDirectory(Path.GetDirectoryName(htmlPath));

            File.WriteAllText(htmlPath, html);
            return htmlPath;
        }
    }
}
