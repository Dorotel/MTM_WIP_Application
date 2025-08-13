using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace MTM_Inventory_Application.Forms.Development.DependencyChartConverter
{
    public partial class DependencyChartViewerForm : Form
    {
        private string htmlDir;
        private List<string> htmlFiles;

        public DependencyChartViewerForm()
        {
            InitializeComponent();
        }

        private void btnSelectHtmlDir_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the HTML output directory (Documentation/Dependency Charts/HTML)";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    htmlDir = fbd.SelectedPath;
                    txtHtmlDir.Text = htmlDir;
                    LoadHtmlFiles();
                }
            }
        }

        private void LoadHtmlFiles()
        {
            listHtmlFiles.Items.Clear();
            htmlFiles = new List<string>();
            if (!Directory.Exists(htmlDir)) return;

            foreach (var file in Directory.EnumerateFiles(htmlDir, "*.html", SearchOption.AllDirectories))
            {
                htmlFiles.Add(file);
                listHtmlFiles.Items.Add(Path.GetFileName(file));
            }
            lblCount.Text = $"Found {htmlFiles.Count} HTML charts";
        }

        private void listHtmlFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listHtmlFiles.SelectedIndex;
            if (idx < 0 || idx >= htmlFiles.Count) return;
            string htmlFile = htmlFiles[idx];
            try
            {
                webBrowser.DocumentText = File.ReadAllText(htmlFile);
                lblFile.Text = htmlFile;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load HTML file: " + ex.Message);
            }
        }

        private void btnOpenExternal_Click(object sender, EventArgs e)
        {
            int idx = listHtmlFiles.SelectedIndex;
            if (idx < 0 || idx >= htmlFiles.Count) return;
            string htmlFile = htmlFiles[idx];
            try
            {
                Process.Start(new ProcessStartInfo(htmlFile) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open in browser: " + ex.Message);
            }
        }
    }
}
