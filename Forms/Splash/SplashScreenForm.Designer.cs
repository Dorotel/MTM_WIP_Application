#nullable enable
namespace MTM_Inventory_Application.Forms.Splash
{
    partial class SplashScreenForm
    {
        #region Fields
        

        #region Fields
        
        private System.ComponentModel.IContainer components = null!;
        private Controls.Shared.ProgressBarUserControl? _progressControl;
        private Label? _titleLabel;
        private Label? _versionLabel;
        
        #endregion
        
        #region Methods

        
        #endregion
        
        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._titleLabel = new System.Windows.Forms.Label();
            this._versionLabel = new System.Windows.Forms.Label();
            this._progressControl = new Controls.Shared.ProgressBarUserControl();

            this.SuspendLayout();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ClientSize = new System.Drawing.Size(400, 250);
            this.Name = "SplashScreenForm";
            this.BackColor = System.Drawing.Color.DarkBlue;

            int margin = 16;
            float scale = 0.9f;
            int watermarkWidth = (int)((Properties.Resources.MTM?.Width ?? 0) * scale);
            int labelX = watermarkWidth + (margin * 2);
            int labelWidth = this.ClientSize.Width - labelX - margin;

            this._titleLabel.Text = "MTM WIP Application";
            this._titleLabel.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            this._titleLabel.ForeColor = System.Drawing.Color.White;
            this._titleLabel.BackColor = System.Drawing.Color.Transparent;
            this._titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._titleLabel.Size = new System.Drawing.Size(labelWidth, 40);
            this._titleLabel.Location = new System.Drawing.Point(labelX, margin);

            this._versionLabel.Text = "Version 4.6.0.0";
            this._versionLabel.Font = new System.Drawing.Font("Segoe UI", 10);
            this._versionLabel.ForeColor = System.Drawing.Color.LightGray;
            this._versionLabel.BackColor = System.Drawing.Color.Transparent;
            this._versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._versionLabel.Size = new System.Drawing.Size(labelWidth, 20);
            this._versionLabel.Location = new System.Drawing.Point(labelX, margin + 45);

            this._progressControl.Size = new System.Drawing.Size(360, 120);
            this._progressControl.Location = new System.Drawing.Point(20, 110);

            this.Controls.Add(this._titleLabel);
            this.Controls.Add(this._versionLabel);
            this.Controls.Add(this._progressControl);

            this.ResumeLayout(false);
        }
        
        #endregion
    }

        
        #endregion
    }