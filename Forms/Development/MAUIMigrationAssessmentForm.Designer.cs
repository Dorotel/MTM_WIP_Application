namespace MTM_Inventory_Application.Forms.Development
{
    partial class MAUIMigrationAssessmentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MAUIMigrationAssessmentForm));
            this.tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
            this.panel_Header = new System.Windows.Forms.Panel();
            this.label_Title = new System.Windows.Forms.Label();
            this.label_Description = new System.Windows.Forms.Label();
            this.tabControl_Assessment = new System.Windows.Forms.TabControl();
            this.panel_Footer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel_Footer = new System.Windows.Forms.TableLayoutPanel();
            this.label_Progress = new System.Windows.Forms.Label();
            this.panel_Buttons = new System.Windows.Forms.Panel();
            this.button_Reset = new System.Windows.Forms.Button();
            this.button_ExportResults = new System.Windows.Forms.Button();
            this.button_GenerateResults = new System.Windows.Forms.Button();
            this.tableLayoutPanel_Main.SuspendLayout();
            this.panel_Header.SuspendLayout();
            this.panel_Footer.SuspendLayout();
            this.tableLayoutPanel_Footer.SuspendLayout();
            this.panel_Buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_Main
            // 
            this.tableLayoutPanel_Main.ColumnCount = 1;
            this.tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_Main.Controls.Add(this.panel_Header, 0, 0);
            this.tableLayoutPanel_Main.Controls.Add(this.tabControl_Assessment, 0, 1);
            this.tableLayoutPanel_Main.Controls.Add(this.panel_Footer, 0, 2);
            this.tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_Main.Name = "tableLayoutPanel_Main";
            this.tableLayoutPanel_Main.RowCount = 3;
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel_Main.Size = new System.Drawing.Size(984, 761);
            this.tableLayoutPanel_Main.TabIndex = 0;
            // 
            // panel_Header
            // 
            this.panel_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel_Header.Controls.Add(this.label_Description);
            this.panel_Header.Controls.Add(this.label_Title);
            this.panel_Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Header.Location = new System.Drawing.Point(3, 3);
            this.panel_Header.Name = "panel_Header";
            this.panel_Header.Padding = new System.Windows.Forms.Padding(10);
            this.panel_Header.Size = new System.Drawing.Size(978, 74);
            this.panel_Header.TabIndex = 0;
            // 
            // label_Title
            // 
            this.label_Title.AutoSize = true;
            this.label_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.label_Title.Location = new System.Drawing.Point(10, 10);
            this.label_Title.Name = "label_Title";
            this.label_Title.Size = new System.Drawing.Size(292, 24);
            this.label_Title.TabIndex = 0;
            this.label_Title.Text = "MAUI Migration Assessment";
            // 
            // label_Description
            // 
            this.label_Description.AutoSize = true;
            this.label_Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Description.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_Description.Location = new System.Drawing.Point(10, 40);
            this.label_Description.Name = "label_Description";
            this.label_Description.Size = new System.Drawing.Size(610, 15);
            this.label_Description.TabIndex = 1;
            this.label_Description.Text = "Evaluate your application\'s readiness for MAUI migration. Answer questions in ea" +
    "ch category to receive personalized recommendations.";
            // 
            // tabControl_Assessment
            // 
            this.tabControl_Assessment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Assessment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl_Assessment.Location = new System.Drawing.Point(3, 83);
            this.tabControl_Assessment.Name = "tabControl_Assessment";
            this.tabControl_Assessment.SelectedIndex = 0;
            this.tabControl_Assessment.Size = new System.Drawing.Size(978, 615);
            this.tabControl_Assessment.TabIndex = 1;
            // 
            // panel_Footer
            // 
            this.panel_Footer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.panel_Footer.Controls.Add(this.tableLayoutPanel_Footer);
            this.panel_Footer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Footer.Location = new System.Drawing.Point(3, 704);
            this.panel_Footer.Name = "panel_Footer";
            this.panel_Footer.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.panel_Footer.Size = new System.Drawing.Size(978, 54);
            this.panel_Footer.TabIndex = 2;
            // 
            // tableLayoutPanel_Footer
            // 
            this.tableLayoutPanel_Footer.ColumnCount = 2;
            this.tableLayoutPanel_Footer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel_Footer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel_Footer.Controls.Add(this.label_Progress, 0, 0);
            this.tableLayoutPanel_Footer.Controls.Add(this.panel_Buttons, 1, 0);
            this.tableLayoutPanel_Footer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Footer.Location = new System.Drawing.Point(10, 5);
            this.tableLayoutPanel_Footer.Name = "tableLayoutPanel_Footer";
            this.tableLayoutPanel_Footer.RowCount = 1;
            this.tableLayoutPanel_Footer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_Footer.Size = new System.Drawing.Size(958, 44);
            this.tableLayoutPanel_Footer.TabIndex = 0;
            // 
            // label_Progress
            // 
            this.label_Progress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_Progress.AutoSize = true;
            this.label_Progress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Progress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_Progress.Location = new System.Drawing.Point(3, 14);
            this.label_Progress.Name = "label_Progress";
            this.label_Progress.Size = new System.Drawing.Size(176, 15);
            this.label_Progress.TabIndex = 0;
            this.label_Progress.Text = "Progress: 0/0 questions answered";
            // 
            // panel_Buttons
            // 
            this.panel_Buttons.Controls.Add(this.button_Reset);
            this.panel_Buttons.Controls.Add(this.button_ExportResults);
            this.panel_Buttons.Controls.Add(this.button_GenerateResults);
            this.panel_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Buttons.Location = new System.Drawing.Point(577, 3);
            this.panel_Buttons.Name = "panel_Buttons";
            this.panel_Buttons.Size = new System.Drawing.Size(378, 38);
            this.panel_Buttons.TabIndex = 1;
            // 
            // button_Reset
            // 
            this.button_Reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Reset.Image = global::MTM_Inventory_Application.Properties.Resources.icon_reset_16;
            this.button_Reset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Reset.Location = new System.Drawing.Point(300, 5);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(75, 28);
            this.button_Reset.TabIndex = 2;
            this.button_Reset.Text = "Reset";
            this.button_Reset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.Button_Reset_Click);
            // 
            // button_ExportResults
            // 
            this.button_ExportResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ExportResults.Enabled = false;
            this.button_ExportResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ExportResults.Image = global::MTM_Inventory_Application.Properties.Resources.icon_export_16;
            this.button_ExportResults.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_ExportResults.Location = new System.Drawing.Point(190, 5);
            this.button_ExportResults.Name = "button_ExportResults";
            this.button_ExportResults.Size = new System.Drawing.Size(104, 28);
            this.button_ExportResults.TabIndex = 1;
            this.button_ExportResults.Text = "Export (Ctrl+E)";
            this.button_ExportResults.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_ExportResults.UseVisualStyleBackColor = true;
            this.button_ExportResults.Click += new System.EventHandler(this.Button_ExportResults_Click);
            // 
            // button_GenerateResults
            // 
            this.button_GenerateResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_GenerateResults.Enabled = false;
            this.button_GenerateResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_GenerateResults.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.button_GenerateResults.Image = global::MTM_Inventory_Application.Properties.Resources.icon_analyze_16;
            this.button_GenerateResults.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_GenerateResults.Location = new System.Drawing.Point(50, 5);
            this.button_GenerateResults.Name = "button_GenerateResults";
            this.button_GenerateResults.Size = new System.Drawing.Size(134, 28);
            this.button_GenerateResults.TabIndex = 0;
            this.button_GenerateResults.Text = "Generate Results (F5)";
            this.button_GenerateResults.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_GenerateResults.UseVisualStyleBackColor = true;
            this.button_GenerateResults.Click += new System.EventHandler(this.Button_GenerateResults_Click);
            // 
            // MAUIMigrationAssessmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tableLayoutPanel_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MAUIMigrationAssessmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MAUI Migration Assessment - MTM WIP Application";
            this.tableLayoutPanel_Main.ResumeLayout(false);
            this.panel_Header.ResumeLayout(false);
            this.panel_Header.PerformLayout();
            this.panel_Footer.ResumeLayout(false);
            this.tableLayoutPanel_Footer.ResumeLayout(false);
            this.tableLayoutPanel_Footer.PerformLayout();
            this.panel_Buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Main;
        private System.Windows.Forms.Panel panel_Header;
        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.Label label_Description;
        private System.Windows.Forms.TabControl tabControl_Assessment;
        private System.Windows.Forms.Panel panel_Footer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Footer;
        private System.Windows.Forms.Label label_Progress;
        private System.Windows.Forms.Panel panel_Buttons;
        private System.Windows.Forms.Button button_GenerateResults;
        private System.Windows.Forms.Button button_ExportResults;
        private System.Windows.Forms.Button button_Reset;
    }
}