using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_About : UserControl
    {
        #region Fields

        public event EventHandler<string>? StatusMessageChanged;
        private string? _currentTempPdfPath;

        #endregion

        #region Constructors

        public Control_About()
        {
            InitializeComponent();
            
            // Apply comprehensive DPI scaling and runtime layout adjustments
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);
            
            Control_About_LoadControl();
        }

        #endregion

        #region Initialization

        private async void Control_About_LoadControl()
        {
            try
            {
                // Use Model_AppVariables instead of direct assembly calls
                Control_About_Label_Version_Data.Text = Model_AppVariables.ApplicationVersion;
                Control_About_Label_Copyright_Data.Text = Model_AppVariables.ApplicationCopyright;
                Control_About_Label_Author_Data.Text = Model_AppVariables.ApplicationAuthor;
                Control_About_Label_LastUpdate_Data.Text = Model_AppVariables.LastUpdated ?? "Unknown";

                // Initialize WebView2 and load PDF
                await LoadChangelogAsync();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                StatusMessageChanged?.Invoke(this, $"Error loading about information: {ex.Message}");
            }
        }

        #endregion

        #region Changelog Loading

        private async Task LoadChangelogAsync()
        {
            try
            {
                // Ensure WebView2 runtime is initialized first
                await Control_About_Label_WebView_ChangeLogView.EnsureCoreWebView2Async();

                string? pdfPath = await GetChangelogPdfPathAsync();
                
                if (!string.IsNullOrEmpty(pdfPath) && File.Exists(pdfPath))
                {
                    // Store reference to temp file for cleanup
                    _currentTempPdfPath = pdfPath;
                    
                    // Create proper file URI for WebView2
                    string fileUri = new Uri(pdfPath).AbsoluteUri;
                    
                    // Navigate to the PDF file
                    Control_About_Label_WebView_ChangeLogView.Source = new Uri(fileUri);
                    
                    StatusMessageChanged?.Invoke(this, "Changelog loaded successfully.");
                }
                else
                {
                    // If PDF is not available, show fallback content
                    ShowFallbackContent();
                    StatusMessageChanged?.Invoke(this, "Showing fallback changelog content.");
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                ShowErrorContent(ex.Message);
                StatusMessageChanged?.Invoke(this, $"Warning: Could not load changelog - {ex.Message}");
            }
        }

        private async Task<string?> GetChangelogPdfPathAsync()
        {
            try
            {
                // First, try to get from embedded resource
                var resourceStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("MTM_Inventory_Application.Resources.CHANGELOG.pdf");

                if (resourceStream != null)
                {
                    // Create a temporary file from embedded resource
                    string tempPdfPath = Path.Combine(Path.GetTempPath(), $"MTM_WIP_Application_CHANGELOG_{Guid.NewGuid()}.pdf");
                    
                    using (var fileStream = new FileStream(tempPdfPath, FileMode.Create, FileAccess.Write))
                    {
                        await resourceStream.CopyToAsync(fileStream);
                    }

                    // Verify the file was created successfully
                    if (File.Exists(tempPdfPath) && new FileInfo(tempPdfPath).Length > 0)
                    {
                        return tempPdfPath;
                    }
                }

                // Second, try to find in application directory (for Content files)
                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string[] possiblePaths = {
                    Path.Combine(appDir, "Resources", "CHANGELOG.pdf"),
                    Path.Combine(appDir, "CHANGELOG.pdf"),
                    Path.Combine(Directory.GetParent(appDir)?.FullName ?? appDir, "Resources", "CHANGELOG.pdf")
                };

                foreach (string path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                return null;
            }
        }

        #endregion

        #region HTML Content Display

        private void ShowFallbackContent()
        {
            string fallbackHtml = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Changelog</title>
    <style>
        body {{ 
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
            margin: 20px; 
            background-color: #f5f5f5;
            color: #333;
        }}
        .container {{
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }}
        h1 {{ 
            color: #2c5aa0; 
            border-bottom: 2px solid #2c5aa0;
            padding-bottom: 10px;
        }}
        .version {{ 
            background-color: #e8f4fd; 
            padding: 10px; 
            margin: 10px 0; 
            border-left: 4px solid #2c5aa0;
        }}
        .date {{ color: #666; font-style: italic; }}
        .feature {{ margin: 5px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>MTM WIP Application Changelog</h1>
        <div class='version'>
            <h3>Version {Model_AppVariables.ApplicationVersion}</h3>
            <p class='date'>Last Updated: {Model_AppVariables.LastUpdated}</p>
            <ul>
                <li class='feature'>Enhanced user interface and user experience</li>
                <li class='feature'>Improved database connectivity and performance</li>
                <li class='feature'>Added comprehensive settings management</li>
                <li class='feature'>Implemented advanced inventory tracking features</li>
                <li class='feature'>Updated security and user authentication</li>
                <li class='feature'>Progress tracking for all user operations</li>
                <li class='feature'>Enhanced error handling and logging</li>
            </ul>
        </div>
        <div class='version'>
            <h3>Recent Improvements</h3>
            <ul>
                <li class='feature'>Improved ComboBox data management</li>
                <li class='feature'>Optimized database operations with stored procedures</li>
                <li class='feature'>Better theme and UI customization</li>
                <li class='feature'>Enhanced user removal functionality</li>
                <li class='feature'>Comprehensive progress feedback system</li>
                <li class='feature'>Improved cross-thread operation safety</li>
            </ul>
        </div>
        <div class='version'>
            <h3>Technical Enhancements</h3>
            <ul>
                <li class='feature'>Upgraded to .NET 8 framework</li>
                <li class='feature'>Enhanced MySQL database integration</li>
                <li class='feature'>Improved WebView2 implementation</li>
                <li class='feature'>Better resource management and cleanup</li>
                <li class='feature'>Centralized application configuration</li>
            </ul>
        </div>
        <hr>
        <p><em>This is a summary of recent changes. For detailed changelog information, please refer to the application documentation or contact the system administrator.</em></p>
        <p><small>Developed by: {Model_AppVariables.ApplicationAuthor}<br>
        Company: {Model_AppVariables.ApplicationCopyright}</small></p>
    </div>
</body>
</html>";

            Control_About_Label_WebView_ChangeLogView.CoreWebView2.NavigateToString(fallbackHtml);
        }

        private void ShowErrorContent(string errorMessage)
        {
            string errorHtml = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Changelog Error</title>
    <style>
        body {{ 
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
            margin: 20px; 
            background-color: #fff5f5;
            color: #333;
        }}
        .error-container {{
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            border-left: 4px solid #dc3545;
        }}
        h1 {{ color: #dc3545; }}
        .error-details {{ 
            background-color: #f8f9fa; 
            padding: 10px; 
            border-radius: 4px;
            font-family: 'Courier New', monospace;
            font-size: 12px;
            margin: 10px 0;
        }}
    </style>
</head>
<body>
    <div class='error-container'>
        <h1>⚠️ Changelog Unavailable</h1>
        <p>The changelog could not be loaded at this time.</p>
        <div class='error-details'>Error: {errorMessage}</div>
        <p>Please check the application logs for more details or contact the system administrator.</p>
        <hr>
        <p><strong>Application Information:</strong></p>
        <ul>
            <li>Version: {Model_AppVariables.ApplicationVersion}</li>
            <li>Last Updated: {Model_AppVariables.LastUpdated}</li>
            <li>Author: {Model_AppVariables.ApplicationAuthor}</li>
            <li>Copyright: {Model_AppVariables.ApplicationCopyright}</li>
        </ul>
    </div>
</body>
</html>";

            Control_About_Label_WebView_ChangeLogView.CoreWebView2.NavigateToString(errorHtml);
        }

        #endregion

        #region Cleanup Methods

        internal void CleanupTempFiles()
        {
            try
            {
                // Clean up the current temp file if it exists
                if (!string.IsNullOrEmpty(_currentTempPdfPath) && File.Exists(_currentTempPdfPath))
                {
                    try
                    {
                        File.Delete(_currentTempPdfPath);
                        _currentTempPdfPath = null;
                    }
                    catch
                    {
                        // Ignore cleanup errors - file may be in use
                    }
                }

                // Clean up any orphaned temp files from previous sessions
                string tempPath = Path.GetTempPath();
                var tempFiles = Directory.GetFiles(tempPath, "MTM_WIP_Application_CHANGELOG_*.pdf");
                foreach (string file in tempFiles)
                {
                    try
                    {
                        // Only delete files older than 1 hour to avoid conflicts
                        if (File.GetCreationTime(file) < DateTime.Now.AddHours(-1))
                        {
                            File.Delete(file);
                        }
                    }
                    catch
                    {
                        // Ignore cleanup errors - files may be in use
                    }
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        /// <summary>
        /// Static method to clean up temp files at application shutdown
        /// </summary>
        public static void CleanupAllTempFiles()
        {
            try
            {
                string tempPath = Path.GetTempPath();
                var tempFiles = Directory.GetFiles(tempPath, "MTM_WIP_Application_CHANGELOG_*.pdf");
                foreach (string file in tempFiles)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        // Ignore cleanup errors - files may be in use
                    }
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        #endregion
    }
}
