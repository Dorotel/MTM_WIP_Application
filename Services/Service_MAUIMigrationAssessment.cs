using System.ComponentModel;
using System.Reflection;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Services
{
    #region MAUI Migration Assessment Service

    /// <summary>
    /// Service for conducting MAUI Migration Assessment
    /// Provides questionnaire generation, scoring, and recommendations
    /// </summary>
    public class Service_MAUIMigrationAssessment
    {
        #region Fields

        private static Service_MAUIMigrationAssessment? _instance;
        private static readonly object _lock = new();

        #endregion

        #region Properties

        public static Service_MAUIMigrationAssessment Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new Service_MAUIMigrationAssessment();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Constructors

        private Service_MAUIMigrationAssessment()
        {
            LoggingUtility.LogInfo("[Service_MAUIMigrationAssessment] Service initialized", nameof(Service_MAUIMigrationAssessment));
        }

        #endregion

        #region Assessment Generation Methods

        /// <summary>
        /// Generate assessment questions for all categories
        /// </summary>
        public List<Model_AssessmentQuestion> GenerateAllQuestions()
        {
            try
            {
                LoggingUtility.LogInfo("[Service_MAUIMigrationAssessment] Generating all assessment questions", nameof(GenerateAllQuestions));

                var questions = new List<Model_AssessmentQuestion>();

                // UI Architecture Questions
                questions.AddRange(GenerateUIArchitectureQuestions());
                
                // Dependency Questions
                questions.AddRange(GenerateDependencyQuestions());
                
                // Business Logic Questions
                questions.AddRange(GenerateBusinessLogicQuestions());
                
                // Data Access Questions
                questions.AddRange(GenerateDataAccessQuestions());
                
                // Platform Features Questions
                questions.AddRange(GeneratePlatformFeaturesQuestions());
                
                // Team Readiness Questions
                questions.AddRange(GenerateTeamReadinessQuestions());
                
                // Project Scope Questions
                questions.AddRange(GenerateProjectScopeQuestions());

                LoggingUtility.LogInfo($"[Service_MAUIMigrationAssessment] Generated {questions.Count} total questions", nameof(GenerateAllQuestions));
                return questions;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_MAUIMigrationAssessment] Error generating questions: {ex.Message}", ex, nameof(GenerateAllQuestions));
                return new List<Model_AssessmentQuestion>();
            }
        }

        private List<Model_AssessmentQuestion> GenerateUIArchitectureQuestions()
        {
            return new List<Model_AssessmentQuestion>
            {
                new()
                {
                    Category = "UI Architecture",
                    Question = "How complex are your current forms and UI layouts?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "Simple forms with basic controls", "Moderate complexity with some custom layouts", "Complex forms with advanced controls", "Very complex with extensive customization" },
                    PropertyName = nameof(Model_UIArchitectureAssessment.FormComplexity),
                    Weight = 15
                },
                new()
                {
                    Category = "UI Architecture",
                    Question = "Do you use custom controls or third-party UI components?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_UIArchitectureAssessment.UsesCustomControls),
                    Weight = 10
                },
                new()
                {
                    Category = "UI Architecture",
                    Question = "List any third-party UI libraries used (e.g., DevExpress, Telerik, ComponentOne):",
                    QuestionType = QuestionType.Text,
                    PropertyName = nameof(Model_UIArchitectureAssessment.ThirdPartyUILibraries),
                    Weight = 8
                },
                new()
                {
                    Category = "UI Architecture",
                    Question = "How is your UI styling/theming currently implemented?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "No consistent styling", "Basic styling with colors/fonts", "Custom themes and styles", "Advanced theming system with DPI scaling" },
                    PropertyName = nameof(Model_UIArchitectureAssessment.CurrentStyling),
                    Weight = 8
                },
                new()
                {
                    Category = "UI Architecture",
                    Question = "Do you have responsive or multi-DPI support in your current application?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_UIArchitectureAssessment.HasResponsiveDesign),
                    Weight = 7
                },
                new()
                {
                    Category = "UI Architecture",
                    Question = "How many different form types do you have approximately?",
                    QuestionType = QuestionType.Numeric,
                    PropertyName = nameof(Model_UIArchitectureAssessment.FormCount),
                    Weight = 5
                }
            };
        }

        private List<Model_AssessmentQuestion> GenerateDependencyQuestions()
        {
            return new List<Model_AssessmentQuestion>
            {
                new()
                {
                    Category = "Dependencies",
                    Question = "Do you use any Windows-specific APIs or libraries?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_DependencyAssessment.UsesWindowsSpecificAPIs),
                    Weight = 15
                },
                new()
                {
                    Category = "Dependencies",
                    Question = "List Windows-specific features used (e.g., Registry, Windows Services, P/Invoke):",
                    QuestionType = QuestionType.Text,
                    PropertyName = nameof(Model_DependencyAssessment.WindowsSpecificFeatures),
                    Weight = 12
                },
                new()
                {
                    Category = "Dependencies",
                    Question = "Do you use COM components or legacy libraries?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_DependencyAssessment.UsesCOMComponents),
                    Weight = 15
                },
                new()
                {
                    Category = "Dependencies",
                    Question = "Do you have any native DLL dependencies?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_DependencyAssessment.HasNativeDLLs),
                    Weight = 12
                },
                new()
                {
                    Category = "Dependencies",
                    Question = "What .NET version are you currently targeting?",
                    QuestionType = QuestionType.Text,
                    PropertyName = nameof(Model_DependencyAssessment.CurrentDotNetVersion),
                    Weight = 8
                }
            };
        }

        private List<Model_AssessmentQuestion> GenerateBusinessLogicQuestions()
        {
            return new List<Model_AssessmentQuestion>
            {
                new()
                {
                    Category = "Business Logic",
                    Question = "Is your business logic separated from UI code?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "Poor - Logic mixed with UI", "Fair - Some separation", "Good - Mostly separated", "Excellent - Clean separation with layers" },
                    PropertyName = nameof(Model_BusinessLogicAssessment.LogicSeparation),
                    Weight = 15
                },
                new()
                {
                    Category = "Business Logic",
                    Question = "Do you use any architectural patterns (MVP, MVVM, MVC)?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "None", "MVP (Model-View-Presenter)", "MVVM (Model-View-ViewModel)", "MVC (Model-View-Controller)", "Other" },
                    PropertyName = nameof(Model_BusinessLogicAssessment.CurrentPattern),
                    Weight = 12
                },
                new()
                {
                    Category = "Business Logic",
                    Question = "How much business logic is in code-behind files?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "Minimal - Just UI logic", "Moderate - Some business logic", "Heavy - Significant business logic", "Extensive - Most logic in code-behind" },
                    PropertyName = nameof(Model_BusinessLogicAssessment.CodeBehindLevel),
                    Weight = 10
                },
                new()
                {
                    Category = "Business Logic",
                    Question = "Do you have a service layer or business logic layer?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_BusinessLogicAssessment.HasServiceLayer),
                    Weight = 8
                }
            };
        }

        private List<Model_AssessmentQuestion> GenerateDataAccessQuestions()
        {
            return new List<Model_AssessmentQuestion>
            {
                new()
                {
                    Category = "Data Access",
                    Question = "What database system do you use?",
                    QuestionType = QuestionType.Text,
                    PropertyName = nameof(Model_DataAccessAssessment.DatabaseSystem),
                    Weight = 5
                },
                new()
                {
                    Category = "Data Access",
                    Question = "How do you access data?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "ADO.NET", "Entity Framework", "Dapper", "Custom Data Access Layer", "Other" },
                    PropertyName = nameof(Model_DataAccessAssessment.DataAccessTech),
                    Weight = 8
                },
                new()
                {
                    Category = "Data Access",
                    Question = "Do you use stored procedures extensively?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_DataAccessAssessment.UsesStoredProcedures),
                    Weight = 6
                },
                new()
                {
                    Category = "Data Access",
                    Question = "Is your data access code centralized?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_DataAccessAssessment.HasCentralizedDataAccess),
                    Weight = 8
                }
            };
        }

        private List<Model_AssessmentQuestion> GeneratePlatformFeaturesQuestions()
        {
            return new List<Model_AssessmentQuestion>
            {
                new()
                {
                    Category = "Platform Features",
                    Question = "Do you use file system access extensively?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_PlatformFeaturesAssessment.UsesFileSystem),
                    Weight = 6
                },
                new()
                {
                    Category = "Platform Features",
                    Question = "Do you use printing functionality?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_PlatformFeaturesAssessment.UsesPrinting),
                    Weight = 8
                },
                new()
                {
                    Category = "Platform Features",
                    Question = "Do you integrate with Windows services?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_PlatformFeaturesAssessment.UsesWindowsServices),
                    Weight = 10
                },
                new()
                {
                    Category = "Platform Features",
                    Question = "Do you use system tray or notifications?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_PlatformFeaturesAssessment.UsesSystemTray),
                    Weight = 8
                },
                new()
                {
                    Category = "Platform Features",
                    Question = "Do you require elevated permissions?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_PlatformFeaturesAssessment.RequiresElevatedPermissions),
                    Weight = 12
                }
            };
        }

        private List<Model_AssessmentQuestion> GenerateTeamReadinessQuestions()
        {
            return new List<Model_AssessmentQuestion>
            {
                new()
                {
                    Category = "Team Readiness",
                    Question = "How many developers will work on the migration?",
                    QuestionType = QuestionType.Numeric,
                    PropertyName = nameof(Model_TeamReadinessAssessment.DeveloperCount),
                    Weight = 5
                },
                new()
                {
                    Category = "Team Readiness",
                    Question = "What is the team's experience with cross-platform development?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "None", "Beginner", "Intermediate", "Advanced", "Expert" },
                    PropertyName = nameof(Model_TeamReadinessAssessment.CrossPlatformExperience),
                    Weight = 12
                },
                new()
                {
                    Category = "Team Readiness",
                    Question = "Does the team have MAUI/Xamarin experience?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "None", "Beginner", "Intermediate", "Advanced", "Expert" },
                    PropertyName = nameof(Model_TeamReadinessAssessment.MAUIExperience),
                    Weight = 15
                },
                new()
                {
                    Category = "Team Readiness",
                    Question = "Are you planning to target multiple platforms?",
                    QuestionType = QuestionType.YesNo,
                    PropertyName = nameof(Model_TeamReadinessAssessment.TargetingMultiplePlatforms),
                    Weight = 8
                }
            };
        }

        private List<Model_AssessmentQuestion> GenerateProjectScopeQuestions()
        {
            return new List<Model_AssessmentQuestion>
            {
                new()
                {
                    Category = "Project Scope",
                    Question = "What is your estimated timeline for migration?",
                    QuestionType = QuestionType.Text,
                    PropertyName = nameof(Model_ProjectScopeAssessment.EstimatedTimeline),
                    Weight = 5
                },
                new()
                {
                    Category = "Project Scope",
                    Question = "What is your budget range for this migration?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "Small (< $50k)", "Medium ($50k - $200k)", "Large ($200k - $500k)", "Enterprise (> $500k)" },
                    PropertyName = nameof(Model_ProjectScopeAssessment.Budget),
                    Weight = 6
                },
                new()
                {
                    Category = "Project Scope",
                    Question = "Is this a complete rewrite or gradual migration?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "Complete Rewrite", "Gradual Migration", "Hybrid Approach", "Undecided" },
                    PropertyName = nameof(Model_ProjectScopeAssessment.Approach),
                    Weight = 10
                },
                new()
                {
                    Category = "Project Scope",
                    Question = "What is the primary driver for MAUI migration?",
                    QuestionType = QuestionType.MultipleChoice,
                    Options = new List<string> { "Cross-Platform Support", "Modernization", "Performance", "Maintenance", "Business Requirement" },
                    PropertyName = nameof(Model_ProjectScopeAssessment.PrimaryDriver),
                    Weight = 8
                }
            };
        }

        #endregion

        #region Assessment Scoring Methods

        /// <summary>
        /// Calculate overall readiness score and generate recommendations
        /// </summary>
        public Model_AssessmentResults CalculateResults(Model_MAUIMigrationAssessment assessment)
        {
            try
            {
                LoggingUtility.LogInfo("[Service_MAUIMigrationAssessment] Calculating assessment results", nameof(CalculateResults));

                var results = new Model_AssessmentResults();
                
                // Calculate individual category scores
                var uiScore = CalculateUIArchitectureScore(assessment.UIArchitecture);
                var depScore = CalculateDependencyScore(assessment.Dependencies);
                var logicScore = CalculateBusinessLogicScore(assessment.BusinessLogic);
                var dataScore = CalculateDataAccessScore(assessment.DataAccess);
                var platformScore = CalculatePlatformFeaturesScore(assessment.PlatformFeatures);
                var teamScore = CalculateTeamReadinessScore(assessment.TeamReadiness);
                var scopeScore = CalculateProjectScopeScore(assessment.ProjectScope);

                // Calculate weighted overall score
                results.OverallReadinessScore = (int)Math.Round(
                    (uiScore * 0.20) +           // UI complexity is major factor
                    (depScore * 0.25) +          // Dependencies are critical
                    (logicScore * 0.15) +        // Business logic separation important
                    (dataScore * 0.10) +         // Data access patterns matter
                    (platformScore * 0.20) +     // Platform features are major blocker
                    (teamScore * 0.08) +         // Team readiness affects timeline
                    (scopeScore * 0.02)          // Scope affects planning
                );

                // Determine complexity
                results.EstimatedComplexity = DetermineComplexity(results.OverallReadinessScore, assessment);

                // Generate recommendations
                results.Recommendations = GenerateRecommendations(assessment, results.OverallReadinessScore);
                results.Challenges = IdentifyChallenges(assessment);
                results.Prerequisites = IdentifyPrerequisites(assessment);
                results.RiskFactors = IdentifyRiskFactors(assessment);
                results.NextSteps = GenerateNextSteps(assessment, results.EstimatedComplexity);
                results.EstimatedEffort = EstimateEffort(results.EstimatedComplexity, assessment);

                LoggingUtility.LogInfo($"[Service_MAUIMigrationAssessment] Assessment complete. Score: {results.OverallReadinessScore}, Complexity: {results.EstimatedComplexity}", nameof(CalculateResults));
                return results;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogError($"[Service_MAUIMigrationAssessment] Error calculating results: {ex.Message}", ex, nameof(CalculateResults));
                return new Model_AssessmentResults();
            }
        }

        private int CalculateUIArchitectureScore(Model_UIArchitectureAssessment ui)
        {
            int score = 100;
            
            // Form complexity penalty
            score -= (int)ui.FormComplexity * 15;
            
            // Custom controls penalty
            if (ui.UsesCustomControls) score -= 20;
            
            // Third-party libraries penalty
            score -= ui.ThirdPartyUILibraries.Count * 5;
            
            // Styling bonus
            if (ui.CurrentStyling >= StylingApproach.CustomThemes) score += 10;
            
            // Responsive design bonus
            if (ui.HasResponsiveDesign) score += 15;

            return Math.Max(0, Math.Min(100, score));
        }

        private int CalculateDependencyScore(Model_DependencyAssessment deps)
        {
            int score = 100;
            
            // Windows-specific APIs major penalty
            if (deps.UsesWindowsSpecificAPIs) score -= 30;
            
            // COM components major penalty
            if (deps.UsesCOMComponents) score -= 40;
            
            // Native DLLs penalty
            if (deps.HasNativeDLLs) score -= 25;
            
            // Windows features penalty
            score -= deps.WindowsSpecificFeatures.Count * 8;

            return Math.Max(0, Math.Min(100, score));
        }

        private int CalculateBusinessLogicScore(Model_BusinessLogicAssessment logic)
        {
            int score = 100;
            
            // Logic separation bonus/penalty
            score += (int)logic.LogicSeparation * 10 - 15;
            
            // Architectural pattern bonus
            if (logic.CurrentPattern == ArchitecturalPattern.MVVM) score += 20;
            else if (logic.CurrentPattern != ArchitecturalPattern.None) score += 10;
            
            // Code-behind penalty
            score -= (int)logic.CodeBehindLevel * 15;
            
            // Service layer bonus
            if (logic.HasServiceLayer) score += 15;

            return Math.Max(0, Math.Min(100, score));
        }

        private int CalculateDataAccessScore(Model_DataAccessAssessment data)
        {
            int score = 100;
            
            // Centralized data access bonus
            if (data.HasCentralizedDataAccess) score += 20;
            
            // Modern data access bonus
            if (data.DataAccessTech == DataAccessTechnology.EntityFramework) score += 10;
            
            return Math.Max(0, Math.Min(100, score));
        }

        private int CalculatePlatformFeaturesScore(Model_PlatformFeaturesAssessment platform)
        {
            int score = 100;
            
            // Platform-specific feature penalties
            if (platform.UsesWindowsServices) score -= 30;
            if (platform.UsesRegistry) score -= 25;
            if (platform.UsesSystemTray) score -= 15;
            if (platform.RequiresElevatedPermissions) score -= 20;
            if (platform.UsesPrinting) score -= 10;
            if (platform.UsesFileSystem) score -= 5;

            return Math.Max(0, Math.Min(100, score));
        }

        private int CalculateTeamReadinessScore(Model_TeamReadinessAssessment team)
        {
            int score = 100;
            
            // Experience bonuses
            score += (int)team.CrossPlatformExperience * 5;
            score += (int)team.MAUIExperience * 8;
            score += (int)team.MVVMExperience * 5;
            
            // Multi-platform testing bonus
            if (team.HasMultiPlatformTesting) score += 10;

            return Math.Max(0, Math.Min(100, score));
        }

        private int CalculateProjectScopeScore(Model_ProjectScopeAssessment scope)
        {
            int score = 100;
            
            // Migration approach bonus
            if (scope.Approach == MigrationApproach.GradualMigration) score += 10;
            
            return Math.Max(0, Math.Min(100, score));
        }

        #endregion

        #region Recommendation Generation Methods

        private MigrationComplexity DetermineComplexity(int score, Model_MAUIMigrationAssessment assessment)
        {
            if (score >= 80) return MigrationComplexity.Low;
            if (score >= 60) return MigrationComplexity.Medium;
            if (score >= 40) return MigrationComplexity.High;
            return MigrationComplexity.VeryHigh;
        }

        private List<string> GenerateRecommendations(Model_MAUIMigrationAssessment assessment, int score)
        {
            var recommendations = new List<string>();

            if (score >= 80)
            {
                recommendations.Add("✅ Your application shows good potential for MAUI migration");
                recommendations.Add("📋 Consider starting with a proof-of-concept for core functionality");
            }
            else if (score >= 60)
            {
                recommendations.Add("⚠️ Migration is feasible but will require careful planning");
                recommendations.Add("🔧 Address dependency issues before starting migration");
            }
            else if (score >= 40)
            {
                recommendations.Add("🚨 Migration will be challenging and resource-intensive");
                recommendations.Add("📚 Consider significant refactoring or training before migration");
            }
            else
            {
                recommendations.Add("❌ Migration not recommended without major architectural changes");
                recommendations.Add("🏗️ Consider complete application redesign");
            }

            // Specific recommendations based on assessment
            if (assessment.Dependencies.UsesWindowsSpecificAPIs)
            {
                recommendations.Add("🔄 Identify cross-platform alternatives for Windows-specific APIs");
            }

            if (assessment.BusinessLogic.LogicSeparation == LogicSeparationLevel.Poor)
            {
                recommendations.Add("🏗️ Refactor to separate business logic from UI code");
                recommendations.Add("📐 Implement MVVM pattern for better separation of concerns");
            }

            if (assessment.TeamReadiness.MAUIExperience == ExperienceLevel.None)
            {
                recommendations.Add("📚 Provide MAUI training for the development team");
                recommendations.Add("👥 Consider hiring MAUI experienced developers or consultants");
            }

            return recommendations;
        }

        private List<string> IdentifyChallenges(Model_MAUIMigrationAssessment assessment)
        {
            var challenges = new List<string>();

            if (assessment.UIArchitecture.UsesCustomControls)
            {
                challenges.Add("Custom controls will need to be rewritten or replaced");
            }

            if (assessment.Dependencies.UsesCOMComponents)
            {
                challenges.Add("COM components are not supported in MAUI");
            }

            if (assessment.PlatformFeatures.UsesWindowsServices)
            {
                challenges.Add("Windows Services integration requires alternative approach");
            }

            if (assessment.BusinessLogic.CodeBehindLevel == CodeBehindUsage.Extensive)
            {
                challenges.Add("Extensive code-behind will require significant refactoring");
            }

            if (assessment.UIArchitecture.ThirdPartyUILibraries.Count > 0)
            {
                challenges.Add($"Third-party UI libraries may not have MAUI equivalents: {string.Join(", ", assessment.UIArchitecture.ThirdPartyUILibraries)}");
            }

            return challenges;
        }

        private List<string> IdentifyPrerequisites(Model_MAUIMigrationAssessment assessment)
        {
            var prerequisites = new List<string>();

            prerequisites.Add("Upgrade to .NET 8 or later");
            prerequisites.Add("Set up MAUI development environment");

            if (assessment.TeamReadiness.MAUIExperience == ExperienceLevel.None)
            {
                prerequisites.Add("Complete MAUI training for development team");
            }

            if (assessment.BusinessLogic.LogicSeparation == LogicSeparationLevel.Poor)
            {
                prerequisites.Add("Refactor application to implement proper separation of concerns");
            }

            if (assessment.Dependencies.UsesWindowsSpecificAPIs)
            {
                prerequisites.Add("Research and identify cross-platform alternatives for Windows-specific features");
            }

            return prerequisites;
        }

        private List<string> IdentifyRiskFactors(Model_MAUIMigrationAssessment assessment)
        {
            var risks = new List<string>();

            if (assessment.Dependencies.UsesCOMComponents)
            {
                risks.Add("HIGH: COM components will require complete replacement");
            }

            if (assessment.PlatformFeatures.RequiresElevatedPermissions)
            {
                risks.Add("MEDIUM: Elevated permissions may not be available on mobile platforms");
            }

            if (assessment.TeamReadiness.CrossPlatformExperience == ExperienceLevel.None)
            {
                risks.Add("MEDIUM: Learning curve for cross-platform development");
            }

            if (assessment.ProjectScope.MaintainExistingApp)
            {
                risks.Add("LOW: Resource allocation between maintenance and migration");
            }

            return risks;
        }

        private List<string> GenerateNextSteps(Model_MAUIMigrationAssessment assessment, MigrationComplexity complexity)
        {
            var steps = new List<string>();

            switch (complexity)
            {
                case MigrationComplexity.Low:
                    steps.Add("1. Create MAUI proof-of-concept with core functionality");
                    steps.Add("2. Set up CI/CD pipeline for MAUI");
                    steps.Add("3. Begin incremental migration of UI components");
                    break;

                case MigrationComplexity.Medium:
                    steps.Add("1. Address dependency compatibility issues");
                    steps.Add("2. Refactor business logic separation");
                    steps.Add("3. Create detailed migration plan");
                    steps.Add("4. Start with pilot project");
                    break;

                case MigrationComplexity.High:
                    steps.Add("1. Conduct detailed architectural analysis");
                    steps.Add("2. Plan major refactoring effort");
                    steps.Add("3. Provide team training");
                    steps.Add("4. Consider phased migration approach");
                    break;

                case MigrationComplexity.VeryHigh:
                    steps.Add("1. Consider complete application redesign");
                    steps.Add("2. Evaluate business case for migration");
                    steps.Add("3. Plan extensive refactoring or rewrite");
                    steps.Add("4. Consider hiring external expertise");
                    break;
            }

            return steps;
        }

        private string EstimateEffort(MigrationComplexity complexity, Model_MAUIMigrationAssessment assessment)
        {
            var devCount = Math.Max(1, assessment.TeamReadiness.DeveloperCount);
            
            return complexity switch
            {
                MigrationComplexity.Low => $"Estimated 3-6 months with {devCount} developer(s)",
                MigrationComplexity.Medium => $"Estimated 6-12 months with {devCount} developer(s)",
                MigrationComplexity.High => $"Estimated 12-18 months with {devCount} developer(s)",
                MigrationComplexity.VeryHigh => $"Estimated 18+ months with {devCount} developer(s)",
                _ => "Effort estimation not available"
            };
        }

        #endregion
    }

    #endregion

    #region Assessment Question Model

    /// <summary>
    /// Model for individual assessment questions
    /// </summary>
    public class Model_AssessmentQuestion
    {
        #region Properties

        public string Category { get; set; } = "";
        public string Question { get; set; } = "";
        public QuestionType QuestionType { get; set; }
        public List<string> Options { get; set; } = new();
        public string PropertyName { get; set; } = "";
        public int Weight { get; set; }
        public object? Answer { get; set; }

        #endregion
    }

    public enum QuestionType
    {
        YesNo,
        MultipleChoice,
        Text,
        Numeric
    }

    #endregion
}