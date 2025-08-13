namespace MTM_Inventory_Application.Forms.Development.DependencyChartConverter
{
    partial class DependencyChartConverterForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtBasePath;
        private System.Windows.Forms.Button btnSelectBasePath;
        private System.Windows.Forms.Button btnConvertCharts;
        private System.Windows.Forms.TextBox txtOutput;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtBasePath = new System.Windows.Forms.TextBox();
            this.btnSelectBasePath = new System.Windows.Forms.Button();
            this.btnConvertCharts = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtBasePath
            // 
            this.txtBasePath.Location = new System.Drawing.Point(12, 12);
            this.txtBasePath.Name = "txtBasePath";
            this.txtBasePath.Size = new System.Drawing.Size(400, 23);
            this.txtBasePath.TabIndex = 0;
            // 
            // btnSelectBasePath
            // 
            this.btnSelectBasePath.Location = new System.Drawing.Point(418, 12);
            this.btnSelectBasePath.Name = "btnSelectBasePath";
            this.btnSelectBasePath.Size = new System.Drawing.Size(80, 23);
            this.btnSelectBasePath.TabIndex = 1;
            this.btnSelectBasePath.Text = "Browse...";
            this.btnSelectBasePath.UseVisualStyleBackColor = true;
            this.btnSelectBasePath.Click += new System.EventHandler(this.btnSelectBasePath_Click);
            // 
            // btnConvertCharts
            // 
            this.btnConvertCharts.Location = new System.Drawing.Point(12, 41);
            this.btnConvertCharts.Name = "btnConvertCharts";
            this.btnConvertCharts.Size = new System.Drawing.Size(486, 30);
            this.btnConvertCharts.TabIndex = 2;
            this.btnConvertCharts.Text = "Convert Markdown Charts to HTML";
            this.btnConvertCharts.UseVisualStyleBackColor = true;
            this.btnConvertCharts.Click += new System.EventHandler(this.btnConvertCharts_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(12, 77);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(486, 280);
            this.txtOutput.TabIndex = 3;
            // 
            // DependencyChartConverterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 369);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnConvertCharts);
            this.Controls.Add(this.btnSelectBasePath);
            this.Controls.Add(this.txtBasePath);
            this.Name = "DependencyChartConverterForm";
            this.Text = "HTML Dependency Chart Converter";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}
