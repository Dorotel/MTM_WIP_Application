using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_About : UserControl
    {
        public event EventHandler<string>? StatusMessageChanged;

        public Control_About()
        {
            InitializeComponent();
            
            // Apply comprehensive DPI scaling and runtime layout adjustments
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);
            
            Control_About_LoadControl();
        }

        private void Control_About_LoadControl()
        {
            try
            {
                Control_About_Label_Version_Data.Text = $"{Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown"}";
                Control_About_Label_Copyright_Data.Text = $"{Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "Unknown"}";
                Control_About_Label_Author_Data.Text = $"{Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyMetadataAttribute>()?.Value ??
                    Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>()
                        .FirstOrDefault(a => a.Key == "Authors")?.Value ??
                    "Unknown"}";
                Control_About_Label_LastUpdate_Data.Text = Models.Model_AppVariables.LastUpdated ?? "Unknown";

                // Get the path to a temporary file
                string tempPdfPath = Path.Combine(Path.GetTempPath(), "MTM_WIP_Application_CHANGELOG.pdf");

                // Write the embedded resource to the temp file
                var resourceStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("MTM_Inventory_Application.Resources.CHANGELOG.pdf");

                if (resourceStream != null)
                {
                    using (var fileStream = new FileStream(tempPdfPath, FileMode.Create, FileAccess.Write))
                    {
                        resourceStream.CopyTo(fileStream);
                    }

                    // Make sure WebView2 is initialized before setting Source
                    Control_About_Label_WebView_ChangeLogView.EnsureCoreWebView2Async().ContinueWith(t =>
                    {
                        if (t.IsCompletedSuccessfully)
                        {
                            // Set the source to the local PDF file
                            Control_About_Label_WebView_ChangeLogView.Invoke(new Action(() =>
                            {
                                Control_About_Label_WebView_ChangeLogView.Source = new Uri(tempPdfPath);
                            }));
                            StatusMessageChanged?.Invoke(this, "About information loaded successfully.");
                        }
                        else
                        {
                            LoggingUtility.LogApplicationError(t.Exception);
                            StatusMessageChanged?.Invoke(this, "Warning: Could not load changelog viewer.");
                        }
                    });
                }
                else
                {
                    StatusMessageChanged?.Invoke(this, "Warning: CHANGELOG.pdf resource not found.");
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                StatusMessageChanged?.Invoke(this, $"Error loading about information: {ex.Message}");
            }
        }
    }
}
