using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
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
                
                // Wire up Load event for WebBrowser configuration
                this.Load += DependencyChartViewerForm_Load;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        private void DependencyChartViewerForm_Load(object? sender, EventArgs e)
        {
            // Use a completely safe approach that doesn't even attempt WebBrowser configuration if it's problematic
            SafeWebBrowserInitialization();
        }

        /// <summary>
        /// Ultra-safe WebBrowser initialization that never fails
        /// </summary>
        private void SafeWebBrowserInitialization()
        {
            try
            {
                // First, test if the WebBrowser control is even functional
                if (!IsWebBrowserControlFunctional())
                {
                    DisableWebBrowserAndShowFallback("WebBrowser control is not functional on this system");
                    return;
                }

                // If we reach here, the control is functional, try minimal configuration
                TryMinimalSafeConfiguration();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                DisableWebBrowserAndShowFallback($"WebBrowser initialization failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Test if WebBrowser control is functional without setting any properties
        /// </summary>
        /// <returns>True if functional, false otherwise</returns>
        private bool IsWebBrowserControlFunctional()
        {
            try
            {
                // Absolutely minimal test - just verify we can access the control
                var isVisible = webBrowser.Visible;
                var isDisposed = webBrowser.IsDisposed;
                
                // If we can read these basic properties without exception, the control is functional
                LoggingUtility.LogApplicationInfo($"WebBrowser control basic test passed - Visible: {isVisible}, IsDisposed: {isDisposed}");
                return !isDisposed;
            }
            catch (COMException comEx)
            {
                LoggingUtility.LogApplicationError(comEx);
                LoggingUtility.LogApplicationInfo($"WebBrowser control failed basic COM test: {comEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                LoggingUtility.LogApplicationInfo($"WebBrowser control failed basic test: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Try minimal configuration only if control is proven functional
        /// </summary>
        private void TryMinimalSafeConfiguration()
        {
            try
            {
                // Only try the most critical setting and only if basic access works
                if (SafelyAccessWebBrowserProperty("ScriptErrorsSuppressed", () => webBrowser.ScriptErrorsSuppressed = true))
                {
                    LoggingUtility.LogApplicationInfo("WebBrowser configured with minimal safe settings");
                }
                else
                {
                    LoggingUtility.LogApplicationInfo("WebBrowser running with no configuration (default settings only)");
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                LoggingUtility.LogApplicationInfo("WebBrowser will run with completely default settings");
            }
        }

        /// <summary>
        /// Safely access a WebBrowser property with complete error isolation
        /// </summary>
        /// <param name="propertyName">Name of the property for logging</param>
        /// <param name="propertyAction">Action to execute</param>
        /// <returns>True if successful, false otherwise</returns>
        private bool SafelyAccessWebBrowserProperty(string propertyName, Action propertyAction)
        {
            try
            {
                propertyAction();
                LoggingUtility.LogApplicationInfo($"WebBrowser property '{propertyName}' set successfully");
                return true;
            }
            catch (COMException comEx)
            {
                LoggingUtility.LogApplicationError(comEx);
                LoggingUtility.LogApplicationInfo($"WebBrowser property '{propertyName}' failed with COM error: {comEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                LoggingUtility.LogApplicationInfo($"WebBrowser property '{propertyName}' failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Disable WebBrowser and show user-friendly fallback interface
        /// </summary>
        /// <param name="reason">Reason for disabling</param>
        private void DisableWebBrowserAndShowFallback(string reason)
        {
            try
            {
                LoggingUtility.LogApplicationInfo($"Disabling WebBrowser control: {reason}");

                // Hide the WebBrowser control
                webBrowser.Visible = false;

                // Create a replacement label in the same location
                var replacementLabel = new Label
                {
                    Text = "🚧 Embedded HTML Viewer Unavailable\n\n" +
                           "The embedded HTML viewer cannot be initialized on this system.\n\n" +
                           "Please use the 'Open in Browser' button to view HTML files\n" +
                           "in your default web browser.\n\n" +
                           $"Technical Details: {reason}",
                    Location = webBrowser.Location,
                    Size = webBrowser.Size,
                    BackColor = SystemColors.Control,
                    ForeColor = SystemColors.ControlText,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BorderStyle = BorderStyle.Fixed3D,
                    Font = new Font(Font.FontFamily, 10f)
                };

                // Add the replacement to the same parent
                webBrowser.Parent?.Controls.Add(replacementLabel);
                replacementLabel.BringToFront();

                LoggingUtility.LogApplicationInfo("WebBrowser fallback interface created successfully");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                // If even the fallback fails, just update the file label
                lblFile.Text = "HTML Viewer Unavailable - Use 'Open in Browser' button";
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
                    var result = Service_ErrorHandler.ShowWarning(
                        "This HTML file may contain potentially unsafe content. Do you want to continue loading it?",
                        "Security Warning", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Warning);
                    
                    if (result != DialogResult.Yes)
                    {
                        return;
                    }
                }

                // Try to load content with fallback handling
                LoadHtmlContentSafely(content, htmlFile);
                lblFile.Text = htmlFile;
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleFileError(ex, htmlFile, 
                    retryAction: () => { LoadHtmlFileInBrowser(htmlFile); return true; },
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        private void LoadHtmlContentSafely(string content, string htmlFile)
        {
            // First check if WebBrowser is even visible (might have been disabled in fallback mode)
            if (!webBrowser.Visible)
            {
                LoggingUtility.LogApplicationInfo("WebBrowser is disabled - HTML content loading skipped");
                lblFile.Text = $"File: {Path.GetFileName(htmlFile)} - Use 'Open in Browser' button to view";
                return;
            }

            try
            {
                // Check if WebBrowser is accessible before trying to use it
                if (webBrowser.IsDisposed)
                {
                    ShowWebBrowserError("WebBrowser control has been disposed", htmlFile);
                    return;
                }

                // Test basic access before attempting to load content
                if (!TestWebBrowserAccess())
                {
                    ShowWebBrowserError("WebBrowser control is not accessible", htmlFile);
                    return;
                }
                
                // Primary method: Set DocumentText (safest approach)
                webBrowser.DocumentText = content;
                LoggingUtility.LogApplicationInfo($"HTML content loaded successfully: {Path.GetFileName(htmlFile)}");
            }
            catch (COMException comEx)
            {
                LoggingUtility.LogApplicationError(comEx);
                LoggingUtility.LogApplicationInfo($"COM error loading HTML content: {comEx.Message}");
                TryFallbackLoadingMethods(content, htmlFile, comEx);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                LoggingUtility.LogApplicationInfo($"Error loading HTML content: {ex.Message}");
                TryFallbackLoadingMethods(content, htmlFile, ex);
            }
        }

        /// <summary>
        /// Test if WebBrowser control is accessible for content operations
        /// </summary>
        /// <returns>True if accessible, false otherwise</returns>
        private bool TestWebBrowserAccess()
        {
            try
            {
                // Test the most basic property access
                _ = webBrowser.Visible;
                _ = webBrowser.IsDisposed;
                
                // Test if we can access the Handle property (critical for COM operations)
                if (!webBrowser.IsHandleCreated)
                {
                    try
                    {
                        _ = webBrowser.Handle; // Force handle creation
                        System.Threading.Thread.Sleep(10); // Brief pause for handle initialization
                    }
                    catch (COMException)
                    {
                        LoggingUtility.LogApplicationInfo("WebBrowser handle creation failed");
                        return false;
                    }
                }

                return webBrowser.IsHandleCreated && !webBrowser.IsDisposed;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                return false;
            }
        }

        private void TryFallbackLoadingMethods(string content, string htmlFile, Exception originalException)
        {
            try
            {
                // Fallback method 1: Try navigating to the file directly
                if (File.Exists(htmlFile))
                {
                    webBrowser.Navigate(htmlFile);
                    return;
                }
            }
            catch (Exception navEx)
            {
                LoggingUtility.LogApplicationError(navEx);
            }

            try
            {
                // Fallback method 2: Create a temporary file and navigate to it
                string tempFile = Path.GetTempFileName() + ".html";
                File.WriteAllText(tempFile, content);
                webBrowser.Url = new Uri(tempFile);
                
                // Clean up temp file after a delay
                Task.Delay(10000).ContinueWith(t =>
                {
                    try
                    {
                        if (File.Exists(tempFile))
                            File.Delete(tempFile);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }, TaskScheduler.Default);
                return;
            }
            catch (Exception tempEx)
            {
                LoggingUtility.LogApplicationError(tempEx);
            }

            // All methods failed - show comprehensive error message
            ShowWebBrowserError($"Original error: {originalException.Message}", htmlFile);
        }

        private void ShowWebBrowserError(string errorDetails, string htmlFile)
        {
            // Show error in the WebBrowser area itself
            string errorHtml = $@"
            <html>
            <head>
                <title>WebBrowser Error</title>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }}
                    .error-container {{ background-color: #fff3cd; border: 1px solid #ffeaa7; border-radius: 5px; padding: 20px; }}
                    .error-title {{ color: #856404; font-size: 18px; font-weight: bold; margin-bottom: 10px; }}
                    .error-message {{ color: #856404; margin-bottom: 15px; }}
                    .error-details {{ background-color: #fff; border: 1px solid #ddd; border-radius: 3px; padding: 10px; font-family: monospace; font-size: 12px; }}
                    .suggestion {{ margin-top: 15px; padding: 10px; background-color: #d1ecf1; border: 1px solid #bee5eb; border-radius: 3px; }}
                </style>
            </head>
            <body>
                <div class='error-container'>
                    <div class='error-title'>🚧 WebBrowser Compatibility Issue</div>
                    <div class='error-message'>
                        Unable to display the HTML file in the embedded browser due to Internet Explorer compatibility issues.
                    </div>
                    <div class='error-details'>
                        File: {System.Security.SecurityElement.Escape(htmlFile)}<br>
                        Error: {System.Security.SecurityElement.Escape(errorDetails)}
                    </div>
                    <div class='suggestion'>
                        💡 <strong>Suggestion:</strong> Use the 'Open in Browser' button to view this file in your default web browser, 
                        which will provide better compatibility and functionality.
                    </div>
                </div>
            </body>
            </html>";

            try
            {
                webBrowser.DocumentText = errorHtml;
            }
            catch
            {
                // If even error display fails, give up on WebBrowser entirely
                lblFile.Text = "WebBrowser Error - Use 'Open in Browser' button";
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

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _htmlFiles?.Clear();
                }
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                    controlName: nameof(DependencyChartViewerForm));
            }
        }

        #endregion
    }

    #endregion
}
