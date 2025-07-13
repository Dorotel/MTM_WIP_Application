namespace MTM_Inventory_Application.Controls.Shared
{
    partial class ProgressBarUserControl
    {
        #region Fields
        


        private System.ComponentModel.IContainer components = null;
        
        #endregion
        
        #region Methods


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            
            if (Tag is System.Windows.Forms.Timer timer)
            {
                timer.Stop();
                timer.Dispose();
                Tag = null;
            }
            
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Name = "ProgressBarUserControl";
            Size = new Size(298, 118);
            ResumeLayout(false);
        }

        #endregion
    }

        
        #endregion
    }