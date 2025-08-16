namespace MTM_Inventory_Application.Forms.Development
{
    partial class ApplicationAnalyzerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationAnalyzerForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToMarkdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runFullAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.analyzeUIStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzeBusinessLogicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzeThemingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzeErrorHandlingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_RunFullAnalysis = new System.Windows.Forms.Button();
            this.checkBox_AnalyzeEnvironment = new System.Windows.Forms.CheckBox();
            this.checkBox_AnalyzeErrorHandling = new System.Windows.Forms.CheckBox();
            this.checkBox_AnalyzeTheming = new System.Windows.Forms.CheckBox();
            this.checkBox_AnalyzeBusinessLogic = new System.Windows.Forms.CheckBox();
            this.checkBox_AnalyzeDatabase = new System.Windows.Forms.CheckBox();
            this.checkBox_AnalyzeUI = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Summary = new System.Windows.Forms.TabPage();
            this.richTextBox_Summary = new System.Windows.Forms.RichTextBox();
            this.tabPage_UIAnalysis = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView_UIStructure = new System.Windows.Forms.TreeView();
            this.dataGridView_UIDetails = new System.Windows.Forms.DataGridView();
            this.tabPage_DatabaseAnalysis = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.treeView_DatabaseStructure = new System.Windows.Forms.TreeView();
            this.dataGridView_DatabaseDetails = new System.Windows.Forms.DataGridView();
            this.tabPage_BusinessLogic = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.treeView_BusinessLogic = new System.Windows.Forms.TreeView();
            this.dataGridView_BusinessDetails = new System.Windows.Forms.DataGridView();
            this.tabPage_Theming = new System.Windows.Forms.TabPage();
            this.richTextBox_ThemingAnalysis = new System.Windows.Forms.RichTextBox();
            this.tabPage_ErrorHandling = new System.Windows.Forms.TabPage();
            this.richTextBox_ErrorHandling = new System.Windows.Forms.RichTextBox();
            this.tabPage_Environment = new System.Windows.Forms.TabPage();
            this.richTextBox_Environment = new System.Windows.Forms.RichTextBox();
            this.tabPage_RawData = new System.Windows.Forms.TabPage();
            this.textBox_RawJson = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_Summary.SuspendLayout();
            this.tabPage_UIAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UIDetails)).BeginInit();
            this.tabPage_DatabaseAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DatabaseDetails)).BeginInit();
            this.tabPage_BusinessLogic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_BusinessDetails)).BeginInit();
            this.tabPage_Theming.SuspendLayout();
            this.tabPage_ErrorHandling.SuspendLayout();
            this.tabPage_Environment.SuspendLayout();
            this.tabPage_RawData.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.analysisToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1200, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToJSONToolStripMenuItem,
            this.exportToMarkdownToolStripMenuItem,
            this.exportToHTMLToolStripMenuItem,
            this.exportSummaryToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.exportToolStripMenuItem.Text = "&Export Results";
            // 
            // exportToJSONToolStripMenuItem
            // 
            this.exportToJSONToolStripMenuItem.Name = "exportToJSONToolStripMenuItem";
            this.exportToJSONToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.exportToJSONToolStripMenuItem.Text = "Export to &JSON";
            this.exportToJSONToolStripMenuItem.Click += new System.EventHandler(this.exportToJSONToolStripMenuItem_Click);
            // 
            // exportToMarkdownToolStripMenuItem
            // 
            this.exportToMarkdownToolStripMenuItem.Name = "exportToMarkdownToolStripMenuItem";
            this.exportToMarkdownToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.exportToMarkdownToolStripMenuItem.Text = "Export to &Markdown";
            this.exportToMarkdownToolStripMenuItem.Click += new System.EventHandler(this.exportToMarkdownToolStripMenuItem_Click);
            // 
            // exportToHTMLToolStripMenuItem
            // 
            this.exportToHTMLToolStripMenuItem.Name = "exportToHTMLToolStripMenuItem";
            this.exportToHTMLToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.exportToHTMLToolStripMenuItem.Text = "Export to &HTML";
            this.exportToHTMLToolStripMenuItem.Click += new System.EventHandler(this.exportToHTMLToolStripMenuItem_Click);
            // 
            // exportSummaryToolStripMenuItem
            // 
            this.exportSummaryToolStripMenuItem.Name = "exportSummaryToolStripMenuItem";
            this.exportSummaryToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.exportSummaryToolStripMenuItem.Text = "Export &Summary";
            this.exportSummaryToolStripMenuItem.Click += new System.EventHandler(this.exportSummaryToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // analysisToolStripMenuItem
            // 
            this.analysisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runFullAnalysisToolStripMenuItem,
            this.toolStripSeparator2,
            this.analyzeUIStructureToolStripMenuItem,
            this.analyzeDatabaseToolStripMenuItem,
            this.analyzeBusinessLogicToolStripMenuItem,
            this.analyzeThemingToolStripMenuItem,
            this.analyzeErrorHandlingToolStripMenuItem});
            this.analysisToolStripMenuItem.Name = "analysisToolStripMenuItem";
            this.analysisToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.analysisToolStripMenuItem.Text = "&Analysis";
            // 
            // runFullAnalysisToolStripMenuItem
            // 
            this.runFullAnalysisToolStripMenuItem.Name = "runFullAnalysisToolStripMenuItem";
            this.runFullAnalysisToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.runFullAnalysisToolStripMenuItem.Text = "&Run Full Analysis";
            this.runFullAnalysisToolStripMenuItem.Click += new System.EventHandler(this.runFullAnalysisToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(247, 6);
            // 
            // analyzeUIStructureToolStripMenuItem
            // 
            this.analyzeUIStructureToolStripMenuItem.Name = "analyzeUIStructureToolStripMenuItem";
            this.analyzeUIStructureToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.analyzeUIStructureToolStripMenuItem.Text = "Analyze &UI Structure";
            // 
            // analyzeDatabaseToolStripMenuItem
            // 
            this.analyzeDatabaseToolStripMenuItem.Name = "analyzeDatabaseToolStripMenuItem";
            this.analyzeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.analyzeDatabaseToolStripMenuItem.Text = "Analyze &Database";
            // 
            // analyzeBusinessLogicToolStripMenuItem
            // 
            this.analyzeBusinessLogicToolStripMenuItem.Name = "analyzeBusinessLogicToolStripMenuItem";
            this.analyzeBusinessLogicToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.analyzeBusinessLogicToolStripMenuItem.Text = "Analyze &Business Logic";
            // 
            // analyzeThemingToolStripMenuItem
            // 
            this.analyzeThemingToolStripMenuItem.Name = "analyzeThemingToolStripMenuItem";
            this.analyzeThemingToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.analyzeThemingToolStripMenuItem.Text = "Analyze &Theming";
            // 
            // analyzeErrorHandlingToolStripMenuItem
            // 
            this.analyzeErrorHandlingToolStripMenuItem.Name = "analyzeErrorHandlingToolStripMenuItem";
            this.analyzeErrorHandlingToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.analyzeErrorHandlingToolStripMenuItem.Text = "Analyze &Error Handling";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 726);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1200, 26);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(49, 20);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 18);
            this.toolStripProgressBar1.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1200, 698);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_RunFullAnalysis);
            this.groupBox1.Controls.Add(this.checkBox_AnalyzeEnvironment);
            this.groupBox1.Controls.Add(this.checkBox_AnalyzeErrorHandling);
            this.groupBox1.Controls.Add(this.checkBox_AnalyzeTheming);
            this.groupBox1.Controls.Add(this.checkBox_AnalyzeBusinessLogic);
            this.groupBox1.Controls.Add(this.checkBox_AnalyzeDatabase);
            this.groupBox1.Controls.Add(this.checkBox_AnalyzeUI);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 698);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Analysis Options";
            // 
            // button_RunFullAnalysis
            // 
            this.button_RunFullAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_RunFullAnalysis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.button_RunFullAnalysis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_RunFullAnalysis.ForeColor = System.Drawing.Color.White;
            this.button_RunFullAnalysis.Location = new System.Drawing.Point(12, 280);
            this.button_RunFullAnalysis.Name = "button_RunFullAnalysis";
            this.button_RunFullAnalysis.Size = new System.Drawing.Size(276, 40);
            this.button_RunFullAnalysis.TabIndex = 7;
            this.button_RunFullAnalysis.Text = "Run Full Analysis";
            this.button_RunFullAnalysis.UseVisualStyleBackColor = false;
            this.button_RunFullAnalysis.Click += new System.EventHandler(this.button_RunFullAnalysis_Click);
            // 
            // checkBox_AnalyzeEnvironment
            // 
            this.checkBox_AnalyzeEnvironment.AutoSize = true;
            this.checkBox_AnalyzeEnvironment.Checked = true;
            this.checkBox_AnalyzeEnvironment.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AnalyzeEnvironment.Location = new System.Drawing.Point(12, 230);
            this.checkBox_AnalyzeEnvironment.Name = "checkBox_AnalyzeEnvironment";
            this.checkBox_AnalyzeEnvironment.Size = new System.Drawing.Size(181, 24);
            this.checkBox_AnalyzeEnvironment.TabIndex = 6;
            this.checkBox_AnalyzeEnvironment.Text = "Environment Analysis";
            this.checkBox_AnalyzeEnvironment.UseVisualStyleBackColor = true;
            // 
            // checkBox_AnalyzeErrorHandling
            // 
            this.checkBox_AnalyzeErrorHandling.AutoSize = true;
            this.checkBox_AnalyzeErrorHandling.Checked = true;
            this.checkBox_AnalyzeErrorHandling.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AnalyzeErrorHandling.Location = new System.Drawing.Point(12, 200);
            this.checkBox_AnalyzeErrorHandling.Name = "checkBox_AnalyzeErrorHandling";
            this.checkBox_AnalyzeErrorHandling.Size = new System.Drawing.Size(196, 24);
            this.checkBox_AnalyzeErrorHandling.TabIndex = 5;
            this.checkBox_AnalyzeErrorHandling.Text = "Error Handling Analysis";
            this.checkBox_AnalyzeErrorHandling.UseVisualStyleBackColor = true;
            // 
            // checkBox_AnalyzeTheming
            // 
            this.checkBox_AnalyzeTheming.AutoSize = true;
            this.checkBox_AnalyzeTheming.Checked = true;
            this.checkBox_AnalyzeTheming.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AnalyzeTheming.Location = new System.Drawing.Point(12, 170);
            this.checkBox_AnalyzeTheming.Name = "checkBox_AnalyzeTheming";
            this.checkBox_AnalyzeTheming.Size = new System.Drawing.Size(151, 24);
            this.checkBox_AnalyzeTheming.TabIndex = 4;
            this.checkBox_AnalyzeTheming.Text = "Theming Analysis";
            this.checkBox_AnalyzeTheming.UseVisualStyleBackColor = true;
            // 
            // checkBox_AnalyzeBusinessLogic
            // 
            this.checkBox_AnalyzeBusinessLogic.AutoSize = true;
            this.checkBox_AnalyzeBusinessLogic.Checked = true;
            this.checkBox_AnalyzeBusinessLogic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AnalyzeBusinessLogic.Location = new System.Drawing.Point(12, 140);
            this.checkBox_AnalyzeBusinessLogic.Name = "checkBox_AnalyzeBusinessLogic";
            this.checkBox_AnalyzeBusinessLogic.Size = new System.Drawing.Size(196, 24);
            this.checkBox_AnalyzeBusinessLogic.TabIndex = 3;
            this.checkBox_AnalyzeBusinessLogic.Text = "Business Logic Analysis";
            this.checkBox_AnalyzeBusinessLogic.UseVisualStyleBackColor = true;
            // 
            // checkBox_AnalyzeDatabase
            // 
            this.checkBox_AnalyzeDatabase.AutoSize = true;
            this.checkBox_AnalyzeDatabase.Checked = true;
            this.checkBox_AnalyzeDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AnalyzeDatabase.Location = new System.Drawing.Point(12, 110);
            this.checkBox_AnalyzeDatabase.Name = "checkBox_AnalyzeDatabase";
            this.checkBox_AnalyzeDatabase.Size = new System.Drawing.Size(158, 24);
            this.checkBox_AnalyzeDatabase.TabIndex = 2;
            this.checkBox_AnalyzeDatabase.Text = "Database Analysis";
            this.checkBox_AnalyzeDatabase.UseVisualStyleBackColor = true;
            // 
            // checkBox_AnalyzeUI
            // 
            this.checkBox_AnalyzeUI.AutoSize = true;
            this.checkBox_AnalyzeUI.Checked = true;
            this.checkBox_AnalyzeUI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AnalyzeUI.Location = new System.Drawing.Point(12, 80);
            this.checkBox_AnalyzeUI.Name = "checkBox_AnalyzeUI";
            this.checkBox_AnalyzeUI.Size = new System.Drawing.Size(120, 24);
            this.checkBox_AnalyzeUI.TabIndex = 1;
            this.checkBox_AnalyzeUI.Text = "UI Analysis";
            this.checkBox_AnalyzeUI.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the analysis types to perform on \r\nthe MTM WIP Application:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_Summary);
            this.tabControl1.Controls.Add(this.tabPage_UIAnalysis);
            this.tabControl1.Controls.Add(this.tabPage_DatabaseAnalysis);
            this.tabControl1.Controls.Add(this.tabPage_BusinessLogic);
            this.tabControl1.Controls.Add(this.tabPage_Theming);
            this.tabControl1.Controls.Add(this.tabPage_ErrorHandling);
            this.tabControl1.Controls.Add(this.tabPage_Environment);
            this.tabControl1.Controls.Add(this.tabPage_RawData);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(896, 698);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_Summary
            // 
            this.tabPage_Summary.Controls.Add(this.richTextBox_Summary);
            this.tabPage_Summary.Location = new System.Drawing.Point(4, 29);
            this.tabPage_Summary.Name = "tabPage_Summary";
            this.tabPage_Summary.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Summary.Size = new System.Drawing.Size(888, 665);
            this.tabPage_Summary.TabIndex = 0;
            this.tabPage_Summary.Text = "Summary";
            this.tabPage_Summary.UseVisualStyleBackColor = true;
            // 
            // richTextBox_Summary
            // 
            this.richTextBox_Summary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Summary.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.richTextBox_Summary.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_Summary.Name = "richTextBox_Summary";
            this.richTextBox_Summary.ReadOnly = true;
            this.richTextBox_Summary.Size = new System.Drawing.Size(882, 659);
            this.richTextBox_Summary.TabIndex = 0;
            this.richTextBox_Summary.Text = "Run analysis to see summary results...";
            // 
            // tabPage_UIAnalysis
            // 
            this.tabPage_UIAnalysis.Controls.Add(this.splitContainer2);
            this.tabPage_UIAnalysis.Location = new System.Drawing.Point(4, 29);
            this.tabPage_UIAnalysis.Name = "tabPage_UIAnalysis";
            this.tabPage_UIAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_UIAnalysis.Size = new System.Drawing.Size(888, 665);
            this.tabPage_UIAnalysis.TabIndex = 1;
            this.tabPage_UIAnalysis.Text = "UI Analysis";
            this.tabPage_UIAnalysis.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeView_UIStructure);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGridView_UIDetails);
            this.splitContainer2.Size = new System.Drawing.Size(882, 659);
            this.splitContainer2.SplitterDistance = 329;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeView_UIStructure
            // 
            this.treeView_UIStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_UIStructure.Location = new System.Drawing.Point(0, 0);
            this.treeView_UIStructure.Name = "treeView_UIStructure";
            this.treeView_UIStructure.Size = new System.Drawing.Size(882, 329);
            this.treeView_UIStructure.TabIndex = 0;
            this.treeView_UIStructure.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_UIStructure_AfterSelect);
            // 
            // dataGridView_UIDetails
            // 
            this.dataGridView_UIDetails.AllowUserToAddRows = false;
            this.dataGridView_UIDetails.AllowUserToDeleteRows = false;
            this.dataGridView_UIDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_UIDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_UIDetails.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_UIDetails.Name = "dataGridView_UIDetails";
            this.dataGridView_UIDetails.ReadOnly = true;
            this.dataGridView_UIDetails.RowHeadersWidth = 51;
            this.dataGridView_UIDetails.RowTemplate.Height = 29;
            this.dataGridView_UIDetails.Size = new System.Drawing.Size(882, 326);
            this.dataGridView_UIDetails.TabIndex = 0;
            // 
            // tabPage_DatabaseAnalysis
            // 
            this.tabPage_DatabaseAnalysis.Controls.Add(this.splitContainer3);
            this.tabPage_DatabaseAnalysis.Location = new System.Drawing.Point(4, 29);
            this.tabPage_DatabaseAnalysis.Name = "tabPage_DatabaseAnalysis";
            this.tabPage_DatabaseAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_DatabaseAnalysis.Size = new System.Drawing.Size(888, 665);
            this.tabPage_DatabaseAnalysis.TabIndex = 2;
            this.tabPage_DatabaseAnalysis.Text = "Database";
            this.tabPage_DatabaseAnalysis.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.treeView_DatabaseStructure);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.dataGridView_DatabaseDetails);
            this.splitContainer3.Size = new System.Drawing.Size(882, 659);
            this.splitContainer3.SplitterDistance = 329;
            this.splitContainer3.TabIndex = 0;
            // 
            // treeView_DatabaseStructure
            // 
            this.treeView_DatabaseStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_DatabaseStructure.Location = new System.Drawing.Point(0, 0);
            this.treeView_DatabaseStructure.Name = "treeView_DatabaseStructure";
            this.treeView_DatabaseStructure.Size = new System.Drawing.Size(882, 329);
            this.treeView_DatabaseStructure.TabIndex = 0;
            this.treeView_DatabaseStructure.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_DatabaseStructure_AfterSelect);
            // 
            // dataGridView_DatabaseDetails
            // 
            this.dataGridView_DatabaseDetails.AllowUserToAddRows = false;
            this.dataGridView_DatabaseDetails.AllowUserToDeleteRows = false;
            this.dataGridView_DatabaseDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_DatabaseDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_DatabaseDetails.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_DatabaseDetails.Name = "dataGridView_DatabaseDetails";
            this.dataGridView_DatabaseDetails.ReadOnly = true;
            this.dataGridView_DatabaseDetails.RowHeadersWidth = 51;
            this.dataGridView_DatabaseDetails.RowTemplate.Height = 29;
            this.dataGridView_DatabaseDetails.Size = new System.Drawing.Size(882, 326);
            this.dataGridView_DatabaseDetails.TabIndex = 0;
            // 
            // tabPage_BusinessLogic
            // 
            this.tabPage_BusinessLogic.Controls.Add(this.splitContainer4);
            this.tabPage_BusinessLogic.Location = new System.Drawing.Point(4, 29);
            this.tabPage_BusinessLogic.Name = "tabPage_BusinessLogic";
            this.tabPage_BusinessLogic.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_BusinessLogic.Size = new System.Drawing.Size(888, 665);
            this.tabPage_BusinessLogic.TabIndex = 3;
            this.tabPage_BusinessLogic.Text = "Business Logic";
            this.tabPage_BusinessLogic.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.treeView_BusinessLogic);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.dataGridView_BusinessDetails);
            this.splitContainer4.Size = new System.Drawing.Size(882, 659);
            this.splitContainer4.SplitterDistance = 329;
            this.splitContainer4.TabIndex = 0;
            // 
            // treeView_BusinessLogic
            // 
            this.treeView_BusinessLogic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_BusinessLogic.Location = new System.Drawing.Point(0, 0);
            this.treeView_BusinessLogic.Name = "treeView_BusinessLogic";
            this.treeView_BusinessLogic.Size = new System.Drawing.Size(882, 329);
            this.treeView_BusinessLogic.TabIndex = 0;
            this.treeView_BusinessLogic.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_BusinessLogic_AfterSelect);
            // 
            // dataGridView_BusinessDetails
            // 
            this.dataGridView_BusinessDetails.AllowUserToAddRows = false;
            this.dataGridView_BusinessDetails.AllowUserToDeleteRows = false;
            this.dataGridView_BusinessDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_BusinessDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_BusinessDetails.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_BusinessDetails.Name = "dataGridView_BusinessDetails";
            this.dataGridView_BusinessDetails.ReadOnly = true;
            this.dataGridView_BusinessDetails.RowHeadersWidth = 51;
            this.dataGridView_BusinessDetails.RowTemplate.Height = 29;
            this.dataGridView_BusinessDetails.Size = new System.Drawing.Size(882, 326);
            this.dataGridView_BusinessDetails.TabIndex = 0;
            // 
            // tabPage_Theming
            // 
            this.tabPage_Theming.Controls.Add(this.richTextBox_ThemingAnalysis);
            this.tabPage_Theming.Location = new System.Drawing.Point(4, 29);
            this.tabPage_Theming.Name = "tabPage_Theming";
            this.tabPage_Theming.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Theming.Size = new System.Drawing.Size(888, 665);
            this.tabPage_Theming.TabIndex = 4;
            this.tabPage_Theming.Text = "Theming";
            this.tabPage_Theming.UseVisualStyleBackColor = true;
            // 
            // richTextBox_ThemingAnalysis
            // 
            this.richTextBox_ThemingAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_ThemingAnalysis.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.richTextBox_ThemingAnalysis.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_ThemingAnalysis.Name = "richTextBox_ThemingAnalysis";
            this.richTextBox_ThemingAnalysis.ReadOnly = true;
            this.richTextBox_ThemingAnalysis.Size = new System.Drawing.Size(882, 659);
            this.richTextBox_ThemingAnalysis.TabIndex = 0;
            this.richTextBox_ThemingAnalysis.Text = "Run analysis to see theming analysis results...";
            // 
            // tabPage_ErrorHandling
            // 
            this.tabPage_ErrorHandling.Controls.Add(this.richTextBox_ErrorHandling);
            this.tabPage_ErrorHandling.Location = new System.Drawing.Point(4, 29);
            this.tabPage_ErrorHandling.Name = "tabPage_ErrorHandling";
            this.tabPage_ErrorHandling.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ErrorHandling.Size = new System.Drawing.Size(888, 665);
            this.tabPage_ErrorHandling.TabIndex = 5;
            this.tabPage_ErrorHandling.Text = "Error Handling";
            this.tabPage_ErrorHandling.UseVisualStyleBackColor = true;
            // 
            // richTextBox_ErrorHandling
            // 
            this.richTextBox_ErrorHandling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_ErrorHandling.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.richTextBox_ErrorHandling.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_ErrorHandling.Name = "richTextBox_ErrorHandling";
            this.richTextBox_ErrorHandling.ReadOnly = true;
            this.richTextBox_ErrorHandling.Size = new System.Drawing.Size(882, 659);
            this.richTextBox_ErrorHandling.TabIndex = 0;
            this.richTextBox_ErrorHandling.Text = "Run analysis to see error handling analysis results...";
            // 
            // tabPage_Environment
            // 
            this.tabPage_Environment.Controls.Add(this.richTextBox_Environment);
            this.tabPage_Environment.Location = new System.Drawing.Point(4, 29);
            this.tabPage_Environment.Name = "tabPage_Environment";
            this.tabPage_Environment.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Environment.Size = new System.Drawing.Size(888, 665);
            this.tabPage_Environment.TabIndex = 6;
            this.tabPage_Environment.Text = "Environment";
            this.tabPage_Environment.UseVisualStyleBackColor = true;
            // 
            // richTextBox_Environment
            // 
            this.richTextBox_Environment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Environment.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.richTextBox_Environment.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_Environment.Name = "richTextBox_Environment";
            this.richTextBox_Environment.ReadOnly = true;
            this.richTextBox_Environment.Size = new System.Drawing.Size(882, 659);
            this.richTextBox_Environment.TabIndex = 0;
            this.richTextBox_Environment.Text = "Run analysis to see environment analysis results...";
            // 
            // tabPage_RawData
            // 
            this.tabPage_RawData.Controls.Add(this.textBox_RawJson);
            this.tabPage_RawData.Location = new System.Drawing.Point(4, 29);
            this.tabPage_RawData.Name = "tabPage_RawData";
            this.tabPage_RawData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_RawData.Size = new System.Drawing.Size(888, 665);
            this.tabPage_RawData.TabIndex = 7;
            this.tabPage_RawData.Text = "Raw Data";
            this.tabPage_RawData.UseVisualStyleBackColor = true;
            // 
            // textBox_RawJson
            // 
            this.textBox_RawJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_RawJson.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox_RawJson.Location = new System.Drawing.Point(3, 3);
            this.textBox_RawJson.Multiline = true;
            this.textBox_RawJson.Name = "textBox_RawJson";
            this.textBox_RawJson.ReadOnly = true;
            this.textBox_RawJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_RawJson.Size = new System.Drawing.Size(882, 659);
            this.textBox_RawJson.TabIndex = 0;
            this.textBox_RawJson.Text = "Run analysis to see raw JSON data...";
            // 
            // ApplicationAnalyzerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 752);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.Name = "ApplicationAnalyzerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MTM WIP Application Analyzer - MAUI Migration Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Summary.ResumeLayout(false);
            this.tabPage_UIAnalysis.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UIDetails)).EndInit();
            this.tabPage_DatabaseAnalysis.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DatabaseDetails)).EndInit();
            this.tabPage_BusinessLogic.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_BusinessDetails)).EndInit();
            this.tabPage_Theming.ResumeLayout(false);
            this.tabPage_ErrorHandling.ResumeLayout(false);
            this.tabPage_Environment.ResumeLayout(false);
            this.tabPage_RawData.ResumeLayout(false);
            this.tabPage_RawData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem exportToJSONToolStripMenuItem;
        private ToolStripMenuItem exportToMarkdownToolStripMenuItem;
        private ToolStripMenuItem exportToHTMLToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem analysisToolStripMenuItem;
        private ToolStripMenuItem runFullAnalysisToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem analyzeUIStructureToolStripMenuItem;
        private ToolStripMenuItem analyzeDatabaseToolStripMenuItem;
        private ToolStripMenuItem analyzeBusinessLogicToolStripMenuItem;
        private ToolStripMenuItem analyzeThemingToolStripMenuItem;
        private ToolStripMenuItem analyzeErrorHandlingToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripProgressBar toolStripProgressBar1;
        private SplitContainer splitContainer1;
        private GroupBox groupBox1;
        private Label label1;
        private CheckBox checkBox_AnalyzeUI;
        private CheckBox checkBox_AnalyzeDatabase;
        private CheckBox checkBox_AnalyzeBusinessLogic;
        private CheckBox checkBox_AnalyzeTheming;
        private CheckBox checkBox_AnalyzeErrorHandling;
        private CheckBox checkBox_AnalyzeEnvironment;
        private Button button_RunFullAnalysis;
        private TabControl tabControl1;
        private TabPage tabPage_Summary;
        private RichTextBox richTextBox_Summary;
        private TabPage tabPage_UIAnalysis;
        private SplitContainer splitContainer2;
        private TreeView treeView_UIStructure;
        private DataGridView dataGridView_UIDetails;
        private TabPage tabPage_DatabaseAnalysis;
        private SplitContainer splitContainer3;
        private TreeView treeView_DatabaseStructure;
        private DataGridView dataGridView_DatabaseDetails;
        private TabPage tabPage_BusinessLogic;
        private SplitContainer splitContainer4;
        private TreeView treeView_BusinessLogic;
        private DataGridView dataGridView_BusinessDetails;
        private TabPage tabPage_Theming;
        private RichTextBox richTextBox_ThemingAnalysis;
        private TabPage tabPage_ErrorHandling;
        private RichTextBox richTextBox_ErrorHandling;
        private TabPage tabPage_Environment;
        private RichTextBox richTextBox_Environment;
        private TabPage tabPage_RawData;
        private TextBox textBox_RawJson;
        private ToolStripMenuItem exportSummaryToolStripMenuItem;
    }
}