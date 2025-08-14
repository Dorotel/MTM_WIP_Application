namespace MTM_Inventory_Application.Forms.ErrorDialog
{
    partial class EnhancedErrorDialog
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.tabControlDetails = new System.Windows.Forms.TabControl();
            this.tabPageSummary = new System.Windows.Forms.TabPage();
            this.labelPlainEnglish = new System.Windows.Forms.Label();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.tabPageTechnical = new System.Windows.Forms.TabPage();
            this.richTextBoxTechnical = new System.Windows.Forms.RichTextBox();
            this.tabPageCallStack = new System.Windows.Forms.TabPage();
            this.treeViewCallStack = new System.Windows.Forms.TreeView();
            this.panelActions = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonViewLogs = new System.Windows.Forms.Button();
            this.buttonReportIssue = new System.Windows.Forms.Button();
            this.buttonCopyDetails = new System.Windows.Forms.Button();
            this.buttonRetry = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelMain.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.tabControlDetails.SuspendLayout();
            this.tabPageSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.tabPageTechnical.SuspendLayout();
            this.tabPageCallStack.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelContent);
            this.panelMain.Controls.Add(this.panelActions);
            this.panelMain.Controls.Add(this.statusStrip);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(800, 600);
            this.panelMain.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.tabControlDetails);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(10);
            this.panelContent.Size = new System.Drawing.Size(800, 530);
            this.panelContent.TabIndex = 0;
            // 
            // tabControlDetails
            // 
            this.tabControlDetails.Controls.Add(this.tabPageSummary);
            this.tabControlDetails.Controls.Add(this.tabPageTechnical);
            this.tabControlDetails.Controls.Add(this.tabPageCallStack);
            this.tabControlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDetails.Location = new System.Drawing.Point(10, 10);
            this.tabControlDetails.Name = "tabControlDetails";
            this.tabControlDetails.SelectedIndex = 0;
            this.tabControlDetails.Size = new System.Drawing.Size(780, 510);
            this.tabControlDetails.TabIndex = 0;
            // 
            // tabPageSummary
            // 
            this.tabPageSummary.Controls.Add(this.labelPlainEnglish);
            this.tabPageSummary.Controls.Add(this.pictureBoxIcon);
            this.tabPageSummary.Location = new System.Drawing.Point(4, 24);
            this.tabPageSummary.Name = "tabPageSummary";
            this.tabPageSummary.Padding = new System.Windows.Forms.Padding(20);
            this.tabPageSummary.Size = new System.Drawing.Size(772, 482);
            this.tabPageSummary.TabIndex = 0;
            this.tabPageSummary.Text = "📋 Summary";
            this.tabPageSummary.UseVisualStyleBackColor = true;
            // 
            // labelPlainEnglish
            // 
            this.labelPlainEnglish.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPlainEnglish.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPlainEnglish.Location = new System.Drawing.Point(84, 20);
            this.labelPlainEnglish.Name = "labelPlainEnglish";
            this.labelPlainEnglish.Size = new System.Drawing.Size(668, 442);
            this.labelPlainEnglish.TabIndex = 1;
            this.labelPlainEnglish.Text = "Plain English description of the error will appear here.";
            this.labelPlainEnglish.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.Location = new System.Drawing.Point(20, 20);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(48, 48);
            this.pictureBoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxIcon.TabIndex = 0;
            this.pictureBoxIcon.TabStop = false;
            // 
            // tabPageTechnical
            // 
            this.tabPageTechnical.Controls.Add(this.richTextBoxTechnical);
            this.tabPageTechnical.Location = new System.Drawing.Point(4, 24);
            this.tabPageTechnical.Name = "tabPageTechnical";
            this.tabPageTechnical.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageTechnical.Size = new System.Drawing.Size(772, 482);
            this.tabPageTechnical.TabIndex = 1;
            this.tabPageTechnical.Text = "🔍 Technical Details";
            this.tabPageTechnical.UseVisualStyleBackColor = true;
            // 
            // richTextBoxTechnical
            // 
            this.richTextBoxTechnical.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.richTextBoxTechnical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxTechnical.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.richTextBoxTechnical.Location = new System.Drawing.Point(10, 10);
            this.richTextBoxTechnical.Name = "richTextBoxTechnical";
            this.richTextBoxTechnical.ReadOnly = true;
            this.richTextBoxTechnical.Size = new System.Drawing.Size(752, 462);
            this.richTextBoxTechnical.TabIndex = 0;
            this.richTextBoxTechnical.Text = "";
            // 
            // tabPageCallStack
            // 
            this.tabPageCallStack.Controls.Add(this.treeViewCallStack);
            this.tabPageCallStack.Location = new System.Drawing.Point(4, 24);
            this.tabPageCallStack.Name = "tabPageCallStack";
            this.tabPageCallStack.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageCallStack.Size = new System.Drawing.Size(772, 482);
            this.tabPageCallStack.TabIndex = 2;
            this.tabPageCallStack.Text = "🌳 Call Stack";
            this.tabPageCallStack.UseVisualStyleBackColor = true;
            // 
            // treeViewCallStack
            // 
            this.treeViewCallStack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCallStack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.treeViewCallStack.Location = new System.Drawing.Point(10, 10);
            this.treeViewCallStack.Name = "treeViewCallStack";
            this.treeViewCallStack.Size = new System.Drawing.Size(752, 462);
            this.treeViewCallStack.TabIndex = 0;
            // 
            // panelActions
            // 
            this.panelActions.Controls.Add(this.buttonClose);
            this.panelActions.Controls.Add(this.buttonViewLogs);
            this.panelActions.Controls.Add(this.buttonReportIssue);
            this.panelActions.Controls.Add(this.buttonCopyDetails);
            this.panelActions.Controls.Add(this.buttonRetry);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelActions.Location = new System.Drawing.Point(0, 530);
            this.panelActions.Name = "panelActions";
            this.panelActions.Padding = new System.Windows.Forms.Padding(10);
            this.panelActions.Size = new System.Drawing.Size(800, 48);
            this.panelActions.TabIndex = 1;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(158)))), ((int)(((byte)(158)))));
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonClose.ForeColor = System.Drawing.Color.White;
            this.buttonClose.Location = new System.Drawing.Point(720, 10);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(70, 28);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "❌ Close";
            this.buttonClose.UseVisualStyleBackColor = false;
            // 
            // buttonViewLogs
            // 
            this.buttonViewLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonViewLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.buttonViewLogs.FlatAppearance.BorderSize = 0;
            this.buttonViewLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonViewLogs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonViewLogs.ForeColor = System.Drawing.Color.White;
            this.buttonViewLogs.Location = new System.Drawing.Point(630, 10);
            this.buttonViewLogs.Name = "buttonViewLogs";
            this.buttonViewLogs.Size = new System.Drawing.Size(84, 28);
            this.buttonViewLogs.TabIndex = 3;
            this.buttonViewLogs.Text = "🔍 View Logs";
            this.buttonViewLogs.UseVisualStyleBackColor = false;
            // 
            // buttonReportIssue
            // 
            this.buttonReportIssue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.buttonReportIssue.FlatAppearance.BorderSize = 0;
            this.buttonReportIssue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReportIssue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonReportIssue.ForeColor = System.Drawing.Color.White;
            this.buttonReportIssue.Location = new System.Drawing.Point(216, 10);
            this.buttonReportIssue.Name = "buttonReportIssue";
            this.buttonReportIssue.Size = new System.Drawing.Size(96, 28);
            this.buttonReportIssue.TabIndex = 2;
            this.buttonReportIssue.Text = "📧 Report Issue";
            this.buttonReportIssue.UseVisualStyleBackColor = false;
            // 
            // buttonCopyDetails
            // 
            this.buttonCopyDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.buttonCopyDetails.FlatAppearance.BorderSize = 0;
            this.buttonCopyDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCopyDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonCopyDetails.ForeColor = System.Drawing.Color.White;
            this.buttonCopyDetails.Location = new System.Drawing.Point(114, 10);
            this.buttonCopyDetails.Name = "buttonCopyDetails";
            this.buttonCopyDetails.Size = new System.Drawing.Size(96, 28);
            this.buttonCopyDetails.TabIndex = 1;
            this.buttonCopyDetails.Text = "📋 Copy Details";
            this.buttonCopyDetails.UseVisualStyleBackColor = false;
            // 
            // buttonRetry
            // 
            this.buttonRetry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.buttonRetry.FlatAppearance.BorderSize = 0;
            this.buttonRetry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRetry.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonRetry.ForeColor = System.Drawing.Color.White;
            this.buttonRetry.Location = new System.Drawing.Point(10, 10);
            this.buttonRetry.Name = "buttonRetry";
            this.buttonRetry.Size = new System.Drawing.Size(98, 28);
            this.buttonRetry.TabIndex = 0;
            this.buttonRetry.Text = "🔄 Retry Operation";
            this.buttonRetry.UseVisualStyleBackColor = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 578);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel.Text = "🔴 Connection Status";
            // 
            // EnhancedErrorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.panelMain);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "EnhancedErrorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MTM Inventory Application - Enhanced Error Dialog";
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.tabControlDetails.ResumeLayout(false);
            this.tabPageSummary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.tabPageTechnical.ResumeLayout(false);
            this.tabPageCallStack.ResumeLayout(false);
            this.panelActions.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.TabControl tabControlDetails;
        private System.Windows.Forms.TabPage tabPageSummary;
        private System.Windows.Forms.Label labelPlainEnglish;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.TabPage tabPageTechnical;
        private System.Windows.Forms.RichTextBox richTextBoxTechnical;
        private System.Windows.Forms.TabPage tabPageCallStack;
        private System.Windows.Forms.TreeView treeViewCallStack;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonViewLogs;
        private System.Windows.Forms.Button buttonReportIssue;
        private System.Windows.Forms.Button buttonCopyDetails;
        private System.Windows.Forms.Button buttonRetry;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}