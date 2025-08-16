using System.ComponentModel;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Forms.Development
{
    #region MAUI Migration Assessment Form

    /// <summary>
    /// Interactive MAUI Migration Assessment tool
    /// Provides questionnaire-based evaluation of application readiness for MAUI migration
    /// </summary>
    public partial class MAUIMigrationAssessmentForm : Form
    {
        #region Fields

        private Model_MAUIMigrationAssessment _assessment = new();
        private List<Model_AssessmentQuestion> _questions = new();
        private int _currentQuestionIndex = 0;
        private Helper_StoredProcedureProgress? _progressHelper;
        private Service_MAUIMigrationAssessment _assessmentService;

        #endregion

        #region Properties

        public Model_MAUIMigrationAssessment Assessment => _assessment;
        public Model_AssessmentResults? Results { get; private set; }

        #endregion

        #region Progress Control Methods

        public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, this);
        }

        #endregion

        #region Constructors

        public MAUIMigrationAssessmentForm()
        {
            LoggingUtility.LogInfo("[MAUIMigrationAssessmentForm] Initializing MAUI Migration Assessment", nameof(MAUIMigrationAssessmentForm));
            
            InitializeComponent();
            InitializeAssessment();
            
            LoggingUtility.LogInfo("[MAUIMigrationAssessmentForm] Form initialized successfully", nameof(MAUIMigrationAssessmentForm));
        }

        #endregion

        #region Initialization

        private void InitializeAssessment()
        {
            try
            {
                _assessmentService = Service_MAUIMigrationAssessment.Instance;
                _questions = _assessmentService.GenerateAllQuestions();
                
                // Group questions by category for tabs
                SetupCategoryTabs();
                
                // Load first question
                if (_questions.Count > 0)
                {
                    LoadQuestion(0);
                }
                
                UpdateProgress();
                
                LoggingUtility.LogInfo($"[MAUIMigrationAssessmentForm] Assessment initialized with {_questions.Count} questions", nameof(InitializeAssessment));
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[MAUIMigrationAssessmentForm] Error initializing assessment: {ex.Message}", ex, nameof(InitializeAssessment));
                Service_ErrorHandler.HandleError(ex, "Failed to initialize MAUI Migration Assessment");
            }
        }

        private void SetupCategoryTabs()
        {
            var categories = _questions.GroupBy(q => q.Category).ToList();
            
            tabControl_Assessment.TabPages.Clear();
            
            foreach (var category in categories)
            {
                var tabPage = new TabPage(category.Key)
                {
                    Name = $"tab_{category.Key.Replace(" ", "")}",
                    UseVisualStyleBackColor = true
                };
                
                var panel = CreateCategoryPanel(category.Key, category.ToList());
                tabPage.Controls.Add(panel);
                
                tabControl_Assessment.TabPages.Add(tabPage);
            }
        }

        private Panel CreateCategoryPanel(string categoryName, List<Model_AssessmentQuestion> categoryQuestions)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };
            
            var tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = categoryQuestions.Count + 1,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            
            // Add category description
            var descLabel = new Label
            {
                Text = GetCategoryDescription(categoryName),
                Font = new Font(Font.FontFamily, 9F, FontStyle.Italic),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };
            tableLayoutPanel.Controls.Add(descLabel, 0, 0);
            
            // Add questions
            for (int i = 0; i < categoryQuestions.Count; i++)
            {
                var questionPanel = CreateQuestionPanel(categoryQuestions[i]);
                tableLayoutPanel.Controls.Add(questionPanel, 0, i + 1);
            }
            
            panel.Controls.Add(tableLayoutPanel);
            return panel;
        }

        private Panel CreateQuestionPanel(Model_AssessmentQuestion question)
        {
            var panel = new Panel
            {
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 15),
                MinimumSize = new Size(600, 0)
            };
            
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2
            };
            
            // Question label
            var questionLabel = new Label
            {
                Text = question.Question,
                Font = new Font(Font.FontFamily, 9F, FontStyle.Bold),
                AutoSize = true,
                MaximumSize = new Size(580, 0),
                Margin = new Padding(0, 0, 0, 5)
            };
            layout.Controls.Add(questionLabel, 0, 0);
            
            // Answer control
            Control answerControl = CreateAnswerControl(question);
            layout.Controls.Add(answerControl, 0, 1);
            
            panel.Controls.Add(layout);
            return panel;
        }

        private Control CreateAnswerControl(Model_AssessmentQuestion question)
        {
            switch (question.QuestionType)
            {
                case QuestionType.YesNo:
                    return CreateYesNoControl(question);
                    
                case QuestionType.MultipleChoice:
                    return CreateMultipleChoiceControl(question);
                    
                case QuestionType.Text:
                    return CreateTextControl(question);
                    
                case QuestionType.Numeric:
                    return CreateNumericControl(question);
                    
                default:
                    return new Label { Text = "Unknown question type" };
            }
        }

        private Control CreateYesNoControl(Model_AssessmentQuestion question)
        {
            var panel = new Panel { AutoSize = true, Height = 30 };
            
            var yesRadio = new RadioButton
            {
                Text = "Yes",
                Location = new Point(0, 5),
                AutoSize = true,
                Tag = question
            };
            yesRadio.CheckedChanged += (s, e) => 
            {
                if (yesRadio.Checked) question.Answer = true;
            };
            
            var noRadio = new RadioButton
            {
                Text = "No",
                Location = new Point(60, 5),
                AutoSize = true,
                Tag = question
            };
            noRadio.CheckedChanged += (s, e) => 
            {
                if (noRadio.Checked) question.Answer = false;
            };
            
            panel.Controls.AddRange(new Control[] { yesRadio, noRadio });
            return panel;
        }

        private Control CreateMultipleChoiceControl(Model_AssessmentQuestion question)
        {
            var comboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 300,
                Tag = question
            };
            
            comboBox.Items.AddRange(question.Options.ToArray());
            comboBox.SelectedIndexChanged += (s, e) =>
            {
                if (comboBox.SelectedIndex >= 0)
                {
                    question.Answer = comboBox.SelectedIndex;
                }
            };
            
            return comboBox;
        }

        private Control CreateTextControl(Model_AssessmentQuestion question)
        {
            var textBox = new TextBox
            {
                Multiline = true,
                Height = 60,
                Width = 400,
                ScrollBars = ScrollBars.Vertical,
                Tag = question
            };
            
            textBox.TextChanged += (s, e) =>
            {
                question.Answer = textBox.Text;
            };
            
            return textBox;
        }

        private Control CreateNumericControl(Model_AssessmentQuestion question)
        {
            var numericUpDown = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 1000,
                Width = 100,
                Tag = question
            };
            
            numericUpDown.ValueChanged += (s, e) =>
            {
                question.Answer = (int)numericUpDown.Value;
            };
            
            return numericUpDown;
        }

        private string GetCategoryDescription(string category)
        {
            return category switch
            {
                "UI Architecture" => "Evaluate the complexity and structure of your current user interface implementation.",
                "Dependencies" => "Assess third-party libraries and platform-specific dependencies that may affect migration.",
                "Business Logic" => "Review how business logic is separated from UI code and architectural patterns used.",
                "Data Access" => "Examine data access patterns, database technologies, and data layer architecture.",
                "Platform Features" => "Identify platform-specific features that may require special consideration in MAUI.",
                "Team Readiness" => "Assess your team's experience and readiness for cross-platform development.",
                "Project Scope" => "Define the scope, timeline, and approach for your MAUI migration project.",
                _ => "Answer the questions in this category to help assess migration readiness."
            };
        }

        #endregion

        #region Question Navigation

        private void LoadQuestion(int index)
        {
            if (index < 0 || index >= _questions.Count) return;
            
            _currentQuestionIndex = index;
            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            // Update progress and navigation state
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            var answeredCount = _questions.Count(q => q.Answer != null);
            var progressPercent = _questions.Count > 0 ? (answeredCount * 100) / _questions.Count : 0;
            
            label_Progress.Text = $"Progress: {answeredCount}/{_questions.Count} questions answered ({progressPercent}%)";
            
            button_GenerateResults.Enabled = answeredCount == _questions.Count;
            
            _progressHelper?.UpdateProgress(progressPercent, $"Assessment Progress: {answeredCount}/{_questions.Count}");
        }

        #endregion

        #region Button Event Handlers

        private void Button_GenerateResults_Click(object sender, EventArgs e)
        {
            try
            {
                LoggingUtility.LogInfo("[MAUIMigrationAssessmentForm] Generating assessment results", nameof(Button_GenerateResults_Click));
                
                _progressHelper?.StartProgress("Generating assessment results...");
                
                // Apply answers to assessment model
                ApplyAnswersToAssessment();
                
                // Calculate results
                Results = _assessmentService.CalculateResults(_assessment);
                
                // Show results
                ShowResults();
                
                _progressHelper?.CompleteProgress("Assessment completed successfully");
                
                LoggingUtility.LogInfo($"[MAUIMigrationAssessmentForm] Results generated. Score: {Results.OverallReadinessScore}", nameof(Button_GenerateResults_Click));
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[MAUIMigrationAssessmentForm] Error generating results: {ex.Message}", ex, nameof(Button_GenerateResults_Click));
                Service_ErrorHandler.HandleError(ex, "Failed to generate assessment results");
                _progressHelper?.CompleteProgress("Assessment failed");
            }
        }

        private void Button_ExportResults_Click(object sender, EventArgs e)
        {
            if (Results == null)
            {
                MessageBox.Show("Please complete the assessment first.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    DefaultExt = "json",
                    FileName = $"MAUI_Migration_Assessment_{DateTime.Now:yyyyMMdd_HHmmss}"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportResults(saveDialog.FileName);
                    MessageBox.Show($"Assessment results exported to:\n{saveDialog.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[MAUIMigrationAssessmentForm] Error exporting results: {ex.Message}", ex, nameof(Button_ExportResults_Click));
                Service_ErrorHandler.HandleError(ex, "Failed to export assessment results");
            }
        }

        private void Button_Reset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to reset the assessment? All answers will be lost.", 
                "Reset Assessment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
            if (result == DialogResult.Yes)
            {
                ResetAssessment();
            }
        }

        #endregion

        #region Assessment Processing

        private void ApplyAnswersToAssessment()
        {
            foreach (var question in _questions)
            {
                if (question.Answer == null) continue;
                
                try
                {
                    ApplyAnswerToProperty(question);
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogError($"[MAUIMigrationAssessmentForm] Error applying answer for {question.PropertyName}: {ex.Message}", ex, nameof(ApplyAnswersToAssessment));
                }
            }
        }

        private void ApplyAnswerToProperty(Model_AssessmentQuestion question)
        {
            // This is a simplified approach - in a full implementation, you'd use reflection
            // to set properties based on the PropertyName and category
            
            var category = question.Category;
            var answer = question.Answer;
            
            switch (category)
            {
                case "UI Architecture":
                    ApplyUIArchitectureAnswer(question.PropertyName, answer);
                    break;
                case "Dependencies":
                    ApplyDependencyAnswer(question.PropertyName, answer);
                    break;
                case "Business Logic":
                    ApplyBusinessLogicAnswer(question.PropertyName, answer);
                    break;
                // Add other categories as needed
            }
        }

        private void ApplyUIArchitectureAnswer(string propertyName, object? answer)
        {
            switch (propertyName)
            {
                case nameof(Model_UIArchitectureAssessment.FormComplexity):
                    if (answer is int index) _assessment.UIArchitecture.FormComplexity = (UIComplexityLevel)index;
                    break;
                case nameof(Model_UIArchitectureAssessment.UsesCustomControls):
                    if (answer is bool value) _assessment.UIArchitecture.UsesCustomControls = value;
                    break;
                case nameof(Model_UIArchitectureAssessment.FormCount):
                    if (answer is int count) _assessment.UIArchitecture.FormCount = count;
                    break;
                // Add other properties as needed
            }
        }

        private void ApplyDependencyAnswer(string propertyName, object? answer)
        {
            switch (propertyName)
            {
                case nameof(Model_DependencyAssessment.UsesWindowsSpecificAPIs):
                    if (answer is bool value) _assessment.Dependencies.UsesWindowsSpecificAPIs = value;
                    break;
                case nameof(Model_DependencyAssessment.UsesCOMComponents):
                    if (answer is bool value2) _assessment.Dependencies.UsesCOMComponents = value2;
                    break;
                // Add other properties as needed
            }
        }

        private void ApplyBusinessLogicAnswer(string propertyName, object? answer)
        {
            switch (propertyName)
            {
                case nameof(Model_BusinessLogicAssessment.LogicSeparation):
                    if (answer is int index) _assessment.BusinessLogic.LogicSeparation = (LogicSeparationLevel)index;
                    break;
                case nameof(Model_BusinessLogicAssessment.HasServiceLayer):
                    if (answer is bool value) _assessment.BusinessLogic.HasServiceLayer = value;
                    break;
                // Add other properties as needed
            }
        }

        #endregion

        #region Results Display

        private void ShowResults()
        {
            if (Results == null) return;
            
            // Switch to results tab
            if (tabControl_Assessment.TabPages.Count > 0)
            {
                // Add results tab if it doesn't exist
                var resultsTab = tabControl_Assessment.TabPages.Cast<TabPage>().FirstOrDefault(t => t.Name == "tab_Results");
                if (resultsTab == null)
                {
                    resultsTab = new TabPage("Results")
                    {
                        Name = "tab_Results",
                        UseVisualStyleBackColor = true
                    };
                    
                    var resultsPanel = CreateResultsPanel();
                    resultsTab.Controls.Add(resultsPanel);
                    
                    tabControl_Assessment.TabPages.Add(resultsTab);
                }
                
                tabControl_Assessment.SelectedTab = resultsTab;
                UpdateResultsDisplay();
            }
        }

        private Panel CreateResultsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };
            
            var tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                AutoSize = true
            };
            
            // Score display
            var scoreLabel = new Label
            {
                Name = "label_Score",
                Text = "Overall Readiness Score: ",
                Font = new Font(Font.FontFamily, 12F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };
            tableLayout.Controls.Add(scoreLabel, 0, 0);
            
            // Complexity display
            var complexityLabel = new Label
            {
                Name = "label_Complexity",
                Text = "Migration Complexity: ",
                Font = new Font(Font.FontFamily, 10F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };
            tableLayout.Controls.Add(complexityLabel, 0, 1);
            
            // Recommendations
            var recGroupBox = new GroupBox
            {
                Text = "Recommendations",
                Dock = DockStyle.Top,
                Height = 150
            };
            var recListBox = new ListBox
            {
                Name = "listBox_Recommendations",
                Dock = DockStyle.Fill
            };
            recGroupBox.Controls.Add(recListBox);
            tableLayout.Controls.Add(recGroupBox, 0, 2);
            
            // Challenges
            var chalGroupBox = new GroupBox
            {
                Text = "Challenges",
                Dock = DockStyle.Top,
                Height = 150
            };
            var chalListBox = new ListBox
            {
                Name = "listBox_Challenges",
                Dock = DockStyle.Fill
            };
            chalGroupBox.Controls.Add(chalListBox);
            tableLayout.Controls.Add(chalGroupBox, 0, 3);
            
            // Next Steps
            var stepsGroupBox = new GroupBox
            {
                Text = "Next Steps",
                Dock = DockStyle.Top,
                Height = 150
            };
            var stepsListBox = new ListBox
            {
                Name = "listBox_NextSteps",
                Dock = DockStyle.Fill
            };
            stepsGroupBox.Controls.Add(stepsListBox);
            tableLayout.Controls.Add(stepsGroupBox, 0, 4);
            
            // Export button
            var exportButton = new Button
            {
                Text = "Export Detailed Report",
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0)
            };
            exportButton.Click += Button_ExportResults_Click;
            tableLayout.Controls.Add(exportButton, 0, 5);
            
            panel.Controls.Add(tableLayout);
            return panel;
        }

        private void UpdateResultsDisplay()
        {
            if (Results == null) return;
            
            var resultsTab = tabControl_Assessment.TabPages.Cast<TabPage>().FirstOrDefault(t => t.Name == "tab_Results");
            if (resultsTab == null) return;
            
            // Update score
            var scoreLabel = resultsTab.Controls.Find("label_Score", true).FirstOrDefault() as Label;
            if (scoreLabel != null)
            {
                scoreLabel.Text = $"Overall Readiness Score: {Results.OverallReadinessScore}/100";
                scoreLabel.ForeColor = Results.OverallReadinessScore >= 70 ? Color.Green : 
                                     Results.OverallReadinessScore >= 50 ? Color.Orange : Color.Red;
            }
            
            // Update complexity
            var complexityLabel = resultsTab.Controls.Find("label_Complexity", true).FirstOrDefault() as Label;
            if (complexityLabel != null)
            {
                complexityLabel.Text = $"Migration Complexity: {Results.EstimatedComplexity}";
                complexityLabel.ForeColor = Results.EstimatedComplexity <= MigrationComplexity.Medium ? Color.Green : Color.Red;
            }
            
            // Update recommendations
            var recListBox = resultsTab.Controls.Find("listBox_Recommendations", true).FirstOrDefault() as ListBox;
            if (recListBox != null)
            {
                recListBox.Items.Clear();
                recListBox.Items.AddRange(Results.Recommendations.ToArray());
            }
            
            // Update challenges
            var chalListBox = resultsTab.Controls.Find("listBox_Challenges", true).FirstOrDefault() as ListBox;
            if (chalListBox != null)
            {
                chalListBox.Items.Clear();
                chalListBox.Items.AddRange(Results.Challenges.ToArray());
            }
            
            // Update next steps
            var stepsListBox = resultsTab.Controls.Find("listBox_NextSteps", true).FirstOrDefault() as ListBox;
            if (stepsListBox != null)
            {
                stepsListBox.Items.Clear();
                stepsListBox.Items.AddRange(Results.NextSteps.ToArray());
            }
        }

        #endregion

        #region Export Methods

        private void ExportResults(string filePath)
        {
            if (Results == null) return;
            
            var export = new
            {
                Assessment = _assessment,
                Results = Results,
                ExportDate = DateTime.Now,
                ApplicationInfo = new
                {
                    Name = "MTM WIP Application",
                    AssessmentVersion = _assessment.AssessmentVersion
                }
            };
            
            var json = System.Text.Json.JsonSerializer.Serialize(export, new System.Text.Json.JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            File.WriteAllText(filePath, json);
        }

        #endregion

        #region Helper Methods

        private void ResetAssessment()
        {
            _assessment = new Model_MAUIMigrationAssessment();
            Results = null;
            
            foreach (var question in _questions)
            {
                question.Answer = null;
            }
            
            // Remove results tab if it exists
            var resultsTab = tabControl_Assessment.TabPages.Cast<TabPage>().FirstOrDefault(t => t.Name == "tab_Results");
            if (resultsTab != null)
            {
                tabControl_Assessment.TabPages.Remove(resultsTab);
            }
            
            // Reset all controls
            foreach (TabPage tab in tabControl_Assessment.TabPages)
            {
                ResetTabControls(tab);
            }
            
            UpdateProgress();
            
            LoggingUtility.LogInfo("[MAUIMigrationAssessmentForm] Assessment reset", nameof(ResetAssessment));
        }

        private void ResetTabControls(TabPage tab)
        {
            foreach (Control control in tab.Controls)
            {
                ResetControlRecursive(control);
            }
        }

        private void ResetControlRecursive(Control control)
        {
            switch (control)
            {
                case RadioButton radio:
                    radio.Checked = false;
                    break;
                case ComboBox combo:
                    combo.SelectedIndex = -1;
                    break;
                case TextBox text:
                    text.Text = "";
                    break;
                case NumericUpDown numeric:
                    numeric.Value = 0;
                    break;
            }
            
            foreach (Control child in control.Controls)
            {
                ResetControlRecursive(child);
            }
        }

        #endregion

        #region Key Processing

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F5:
                    if (button_GenerateResults.Enabled)
                    {
                        Button_GenerateResults_Click(this, EventArgs.Empty);
                        return true;
                    }
                    break;
                    
                case Keys.Control | Keys.E:
                    Button_ExportResults_Click(this, EventArgs.Empty);
                    return true;
                    
                case Keys.Control | Keys.R:
                    Button_Reset_Click(this, EventArgs.Empty);
                    return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }

    #endregion
}