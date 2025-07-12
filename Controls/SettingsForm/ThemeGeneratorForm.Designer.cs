namespace MTM_Inventory_Application.Controls.SettingsForm
{
    partial class ThemeGeneratorForm : UserControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblThemeName;
        private System.Windows.Forms.TextBox txtThemeName;
        private System.Windows.Forms.Label lblBaseColor;
        private System.Windows.Forms.Button btnGenerateSingleTheme;
        private System.Windows.Forms.Button btnPickBaseColor;
        private System.Windows.Forms.Panel pnlThemePreview;
        private System.Windows.Forms.ColorDialog colorDialog;

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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtLog = new TextBox();
            lblThemeName = new Label();
            txtThemeName = new TextBox();
            lblBaseColor = new Label();
            btnGenerateSingleTheme = new Button();
            btnPickBaseColor = new Button();
            pnlThemePreview = new Panel();
            colorDialog = new ColorDialog();
            SuspendLayout();
            // 
            // txtLog
            // 
            txtLog.Location = new Point(10, 240);
            txtLog.Margin = new Padding(3, 2, 3, 2);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(269, 91);
            txtLog.TabIndex = 1;
            // 
            // lblThemeName
            // 
            lblThemeName.Location = new Point(10, 12);
            lblThemeName.Name = "lblThemeName";
            lblThemeName.Size = new Size(105, 23);
            lblThemeName.TabIndex = 0;
            lblThemeName.Text = "Theme Name:";
            lblThemeName.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtThemeName
            // 
            txtThemeName.Location = new Point(121, 13);
            txtThemeName.Margin = new Padding(3, 2, 3, 2);
            txtThemeName.Name = "txtThemeName";
            txtThemeName.Size = new Size(158, 23);
            txtThemeName.TabIndex = 1;
            // 
            // lblBaseColor
            // 
            lblBaseColor.Location = new Point(10, 40);
            lblBaseColor.Name = "lblBaseColor";
            lblBaseColor.Size = new Size(105, 24);
            lblBaseColor.TabIndex = 2;
            lblBaseColor.Text = "Base Color:";
            lblBaseColor.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnGenerateSingleTheme
            // 
            btnGenerateSingleTheme.Location = new Point(10, 206);
            btnGenerateSingleTheme.Margin = new Padding(3, 2, 3, 2);
            btnGenerateSingleTheme.Name = "btnGenerateSingleTheme";
            btnGenerateSingleTheme.Size = new Size(269, 24);
            btnGenerateSingleTheme.TabIndex = 5;
            btnGenerateSingleTheme.Text = "Generate This Theme";
            btnGenerateSingleTheme.Click += btnGenerateSingleTheme_Click;
            // 
            // btnPickBaseColor
            // 
            btnPickBaseColor.Location = new Point(121, 40);
            btnPickBaseColor.Margin = new Padding(3, 2, 3, 2);
            btnPickBaseColor.Name = "btnPickBaseColor";
            btnPickBaseColor.Size = new Size(158, 24);
            btnPickBaseColor.TabIndex = 3;
            btnPickBaseColor.Text = "Pick Base Color";
            btnPickBaseColor.Click += BtnPickBaseColor_Click;
            // 
            // pnlThemePreview
            // 
            pnlThemePreview.BorderStyle = BorderStyle.FixedSingle;
            pnlThemePreview.Location = new Point(10, 68);
            pnlThemePreview.Margin = new Padding(3, 2, 3, 2);
            pnlThemePreview.Name = "pnlThemePreview";
            pnlThemePreview.Size = new Size(269, 134);
            pnlThemePreview.TabIndex = 4;
            // 
            // ThemeGeneratorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblThemeName);
            Controls.Add(txtThemeName);
            Controls.Add(lblBaseColor);
            Controls.Add(btnPickBaseColor);
            Controls.Add(pnlThemePreview);
            Controls.Add(btnGenerateSingleTheme);
            Controls.Add(txtLog);
            Margin = new Padding(3, 2, 3, 2);
            Name = "ThemeGeneratorForm";
            Size = new Size(288, 352);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
