using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Forms.Development.DependencyChartConverter
{
    #region DependencyChartViewer

    public partial class DependencyChartViewerForm : Form
    {
        #region Fields

        private string? _htmlDir;
        private List<string> _htmlFiles;

        #endregion

        #region Properties

        public bool HasFilesLoaded => _htmlFiles?.Count > 0;
        public int FileCount => _htmlFiles?.Count ?? 0;

        #endregion

        #region Constructors

        public DependencyChartViewerForm()
        {
            try
            {
                InitializeComponent();
                _htmlFiles = new List<string>();
                Core_Themes.ApplyDpiScaling(this);
                Core_Themes.ApplyRuntimeLayoutAdjustments(this);
                InitializeForm();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, 
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        #endregion

        #region Initialization

        private void InitializeForm()
        {
            try
            {
                this.Text = "MTM Inventory Application - Dependency Chart Viewer";
                lblCount.Text = "No files loaded";
                lblFile.Text = "Select an HTML file to view";
                
                // Configure web browser for better compatibility
                ConfigureWebBrowser();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        private void ConfigureWebBrowser()
        {
            try
            {
                // Set modern browser compatibility
                webBrowser.ScriptErrorsSuppressed = true;
                webBrowser.AllowWebBrowserDrop = false;
                webBrowser.IsWebBrowserContextMenuEnabled = true;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        #endregion

        #region Button Clicks

        private void btnSelectHtmlDir_Click(object sender, EventArgs e)
        {
            try
            {
                using var fbd = new FolderBrowserDialog();
                fbd.Description = "Select the HTML output directory (Documentation/Dependency Charts/HTML)";
                fbd.ShowNewFolderButton = false;
                
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _htmlDir = fbd.SelectedPath;
                    txtHtmlDir.Text = _htmlDir;
                    LoadHtmlFiles();
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    retryAction: () => { btnSelectHtmlDir_Click(sender, e); return true; },
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        private void btnOpenExternal_Click(object sender, EventArgs e)
        {
            try
            {
                int idx = listHtmlFiles.SelectedIndex;
                if (!IsValidFileIndex(idx))
                {
                    Service_ErrorHandler.HandleValidationError("Please select an HTML file to open.", 
                        "File Selection", controlName: nameof(DependencyChartViewerForm));
                    return;
                }

                string htmlFile = _htmlFiles[idx];
                OpenFileInExternalBrowser(htmlFile);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    retryAction: () => { btnOpenExternal_Click(sender, e); return true; },
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        #endregion

        #region File Operations

        private void LoadHtmlFiles()
        {
            try
            {
                listHtmlFiles.Items.Clear();
                _htmlFiles.Clear();
                
                if (string.IsNullOrEmpty(_htmlDir) || !Directory.Exists(_htmlDir))
                {
                    Service_ErrorHandler.HandleValidationError("Selected directory does not exist.", 
                        "Directory Validation", controlName: nameof(DependencyChartViewerForm));
                    return;
                }

                var foundFiles = GetHtmlFilesFromDirectory(_htmlDir);
                
                foreach (var file in foundFiles)
                {
                    _htmlFiles.Add(file);
                    listHtmlFiles.Items.Add(Path.GetFileName(file));
                }
                
                UpdateFileCount();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    retryAction: () => { LoadHtmlFiles(); return true; },
                    contextData: new Dictionary<string, object> { ["Directory"] = _htmlDir ?? "null" },
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        private List<string> GetHtmlFilesFromDirectory(string directory)
        {
            try
            {
                var files = new List<string>();
                foreach (var file in Directory.EnumerateFiles(directory, "*.html", SearchOption.AllDirectories))
                {
                    if (IsValidHtmlFile(file))
                    {
                        files.Add(file);
                    }
                }
                return files;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleFileError(ex, directory, 
                    controlName: nameof(DependencyChartViewerForm));
                return new List<string>();
            }
        }

        private static bool IsValidHtmlFile(string filePath)
        {
            try
            {
                var info = new FileInfo(filePath);
                return info.Exists && info.Length > 0 && info.Length < 50 * 1024 * 1024; // Max 50MB
            }
            catch
            {
                return false;
            }
        }

        private void OpenFileInExternalBrowser(string htmlFile)
        {
            try
            {
                if (!File.Exists(htmlFile))
                {
                    Service_ErrorHandler.HandleFileError(new FileNotFoundException("HTML file not found"), 
                        htmlFile, controlName: nameof(DependencyChartViewerForm));
                    return;
                }

                var startInfo = new ProcessStartInfo(htmlFile)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    contextData: new Dictionary<string, object> { ["FilePath"] = htmlFile },
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        #endregion

        #region UI Events

        private void listHtmlFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idx = listHtmlFiles.SelectedIndex;
                if (!IsValidFileIndex(idx))
                {
                    return;
                }

                string htmlFile = _htmlFiles[idx];
                LoadHtmlFileInBrowser(htmlFile);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    retryAction: () => { listHtmlFiles_SelectedIndexChanged(sender, e); return true; },
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        #endregion

        #region Browser Operations

        private void LoadHtmlFileInBrowser(string htmlFile)
        {
            try
            {
                if (!File.Exists(htmlFile))
                {
                    Service_ErrorHandler.HandleFileError(new FileNotFoundException("HTML file not found"), 
                        htmlFile, controlName: nameof(DependencyChartViewerForm));
                    return;
                }

                var content = File.ReadAllText(htmlFile);
                
                // Basic security check for potentially malicious content
                if (ContainsPotentiallyMaliciousContent(content))
                {
                    var result = MessageBox.Show(
                        "This HTML file may contain potentially unsafe content. Do you want to continue loading it?",
                        "Security Warning", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Warning);
                    
                    if (result != DialogResult.Yes)
                    {
                        return;
                    }
                }

                webBrowser.DocumentText = content;
                lblFile.Text = htmlFile;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleFileError(ex, htmlFile, 
                    retryAction: () => { LoadHtmlFileInBrowser(htmlFile); return true; },
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        private static bool ContainsPotentiallyMaliciousContent(string content)
        {
            try
            {
                // Basic checks for potentially malicious content
                var dangerousPatterns = new[] { 
                    "<script", "javascript:", "vbscript:", "onload=", "onerror=", 
                    "eval(", "document.cookie", "window.location" 
                };
                
                var lowerContent = content.ToLowerInvariant();
                return Array.Exists(dangerousPatterns, pattern => lowerContent.Contains(pattern));
            }
            catch
            {
                return true; // Err on the side of caution
            }
        }

        #endregion

        #region Validation

        private bool IsValidFileIndex(int index)
        {
            return index >= 0 && index < _htmlFiles.Count;
        }

        #endregion

        #region Helpers

        private void UpdateFileCount()
        {
            try
            {
                if (_htmlFiles.Count == 0)
                {
                    lblCount.Text = "No HTML files found";
                }
                else
                {
                    lblCount.Text = $"Found {_htmlFiles.Count} HTML chart{(_htmlFiles.Count == 1 ? "" : "s")}";
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                // F5 to refresh file list
                if (keyData == Keys.F5)
                {
                    if (!string.IsNullOrEmpty(_htmlDir))
                    {
                        LoadHtmlFiles();
                    }
                    return true;
                }

                // Enter to open selected file externally
                if (keyData == Keys.Enter && listHtmlFiles.SelectedIndex >= 0)
                {
                    btnOpenExternal_Click(this, EventArgs.Empty);
                    return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                    controlName: nameof(DependencyChartViewerForm));
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        #endregion
    }

    #endregion
}
