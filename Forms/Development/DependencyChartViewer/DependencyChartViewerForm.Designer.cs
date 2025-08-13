namespace MTM_Inventory_Application.Forms.Development.DependencyChartConverter
{
    partial class DependencyChartViewerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtHtmlDir;
        private System.Windows.Forms.Button btnSelectHtmlDir;
        private System.Windows.Forms.ListBox listHtmlFiles;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Button btnOpenExternal;



        private void InitializeComponent()
        {
            this.txtHtmlDir = new System.Windows.Forms.TextBox();
            this.btnSelectHtmlDir = new System.Windows.Forms.Button();
            this.listHtmlFiles = new System.Windows.Forms.ListBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.lblFile = new System.Windows.Forms.Label();
            this.btnOpenExternal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtHtmlDir
            // 
            this.txtHtmlDir.Location = new System.Drawing.Point(12, 12);
            this.txtHtmlDir.Name = "txtHtmlDir";
            this.txtHtmlDir.Size = new System.Drawing.Size(400, 23);
            this.txtHtmlDir.TabIndex = 0;
            // 
            // btnSelectHtmlDir
            // 
            this.btnSelectHtmlDir.Location = new System.Drawing.Point(418, 12);
            this.btnSelectHtmlDir.Name = "btnSelectHtmlDir";
            this.btnSelectHtmlDir.Size = new System.Drawing.Size(80, 23);
            this.btnSelectHtmlDir.TabIndex = 1;
            this.btnSelectHtmlDir.Text = "Browse...";
            this.btnSelectHtmlDir.UseVisualStyleBackColor = true;
            this.btnSelectHtmlDir.Click += new System.EventHandler(this.btnSelectHtmlDir_Click);
            // 
            // listHtmlFiles
            // 
            this.listHtmlFiles.FormattingEnabled = true;
            this.listHtmlFiles.ItemHeight = 15;
            this.listHtmlFiles.Location = new System.Drawing.Point(12, 41);
            this.listHtmlFiles.Name = "listHtmlFiles";
            this.listHtmlFiles.Size = new System.Drawing.Size(200, 334);
            this.listHtmlFiles.TabIndex = 2;
            this.listHtmlFiles.SelectedIndexChanged += new System.EventHandler(this.listHtmlFiles_SelectedIndexChanged);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(218, 41);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 15);
            this.lblCount.TabIndex = 3;
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(218, 64);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(480, 311);
            this.webBrowser.TabIndex = 4;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(218, 377);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(0, 15);
            this.lblFile.TabIndex = 5;
            // 
            // btnOpenExternal
            // 
            this.btnOpenExternal.Location = new System.Drawing.Point(618, 12);
            this.btnOpenExternal.Name = "btnOpenExternal";
            this.btnOpenExternal.Size = new System.Drawing.Size(80, 23);
            this.btnOpenExternal.TabIndex = 6;
            this.btnOpenExternal.Text = "Open in Browser";
            this.btnOpenExternal.UseVisualStyleBackColor = true;
            this.btnOpenExternal.Click += new System.EventHandler(this.btnOpenExternal_Click);
            // 
            // DependencyChartViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 410);
            this.Controls.Add(this.btnOpenExternal);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.listHtmlFiles);
            this.Controls.Add(this.btnSelectHtmlDir);
            this.Controls.Add(this.txtHtmlDir);
            this.Name = "DependencyChartViewerForm";
            this.Text = "Dependency Chart HTML Viewer";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
