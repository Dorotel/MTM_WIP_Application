// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_About : UserControl
    {
        public Control_About()
        {
            InitializeComponent();
            Control_About_LoadControl();
        }

        private void Control_About_LoadControl()
        {
            Control_About_Label_Version_Data.Text = $"{Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown"}";
            Control_About_Label_Copyright_Data.Text = $"{Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "Unknown"}";
            Control_About_Label_Author_Data.Text = $"{Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyMetadataAttribute>()?.Value ??
                Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>()
                    .FirstOrDefault(a => a.Key == "Authors")?.Value ??
                "Unknown"}";
            Control_About_Label_LastUpdate_Data.Text = Models.Model_AppVariables.LastUpdated?? "Unknown";

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
                    // Set the source to the local PDF file
                    Control_About_Label_WebView_ChangeLogView.Invoke(new Action(() =>
                    {
                        Control_About_Label_WebView_ChangeLogView.Source = new Uri(tempPdfPath);
                    }));
                });
            }
            else
            {
                MessageBox.Show("CHANGELOG.pdf resource not found!");
            }
        }
    }
}
