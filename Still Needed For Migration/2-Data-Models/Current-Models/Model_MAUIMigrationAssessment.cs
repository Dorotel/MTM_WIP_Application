using System.ComponentModel;

namespace MTM_Inventory_Application.Models
{
    #region MAUI Migration Assessment Models

    /// <summary>
    /// Model for MAUI Migration Assessment questionnaire and analysis
    /// Provides structured assessment of application readiness for MAUI migration
    /// </summary>
    public class Model_MAUIMigrationAssessment
    {
        #region Properties

        public DateTime AssessmentDate { get; set; } = DateTime.Now;
        public string AssessmentVersion { get; set; } = "1.0";
        public Model_UIArchitectureAssessment UIArchitecture { get; set; } = new();
        public Model_DependencyAssessment Dependencies { get; set; } = new();
        public Model_BusinessLogicAssessment BusinessLogic { get; set; } = new();
        public Model_DataAccessAssessment DataAccess { get; set; } = new();
        public Model_PlatformFeaturesAssessment PlatformFeatures { get; set; } = new();
        public Model_TeamReadinessAssessment TeamReadiness { get; set; } = new();
        public Model_ProjectScopeAssessment ProjectScope { get; set; } = new();
        public Model_AssessmentResults Results { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Assessment of current UI architecture and complexity
    /// </summary>
    public class Model_UIArchitectureAssessment
    {
        #region Properties

        [Description("How complex are your current forms and UI layouts?")]
        public UIComplexityLevel FormComplexity { get; set; }

        [Description("Do you use custom controls or third-party UI components?")]
        public bool UsesCustomControls { get; set; }

        [Description("List any third-party UI libraries used")]
        public List<string> ThirdPartyUILibraries { get; set; } = new();

        [Description("How is your UI styling/theming currently implemented?")]
        public StylingApproach CurrentStyling { get; set; }

        [Description("Do you have responsive or multi-DPI support?")]
        public bool HasResponsiveDesign { get; set; }

        [Description("How many different form types do you have?")]
        public int FormCount { get; set; }

        [Description("Do you use data binding extensively?")]
        public DataBindingUsage DataBindingLevel { get; set; }

        #endregion
    }

    /// <summary>
    /// Assessment of dependencies and their MAUI compatibility
    /// </summary>
    public class Model_DependencyAssessment
    {
        #region Properties

        [Description("List all NuGet packages your application uses")]
        public List<string> NuGetPackages { get; set; } = new();

        [Description("Do you use any Windows-specific APIs?")]
        public bool UsesWindowsSpecificAPIs { get; set; }

        [Description("List Windows-specific features used")]
        public List<string> WindowsSpecificFeatures { get; set; } = new();

        [Description("Do you use COM components or legacy libraries?")]
        public bool UsesCOMComponents { get; set; }

        [Description("Do you have any native DLL dependencies?")]
        public bool HasNativeDLLs { get; set; }

        [Description("List native DLL dependencies")]
        public List<string> NativeDLLs { get; set; } = new();

        [Description("What .NET version are you currently targeting?")]
        public string CurrentDotNetVersion { get; set; } = "";

        #endregion
    }

    /// <summary>
    /// Assessment of business logic separation and architecture
    /// </summary>
    public class Model_BusinessLogicAssessment
    {
        #region Properties

        [Description("Is your business logic separated from UI code?")]
        public LogicSeparationLevel LogicSeparation { get; set; }

        [Description("Do you use any architectural patterns (MVP, MVVM, MVC)?")]
        public ArchitecturalPattern CurrentPattern { get; set; }

        [Description("How much business logic is in code-behind files?")]
        public CodeBehindUsage CodeBehindLevel { get; set; }

        [Description("Do you have a service layer or business logic layer?")]
        public bool HasServiceLayer { get; set; }

        [Description("How is dependency injection currently handled?")]
        public DependencyInjectionUsage DIUsage { get; set; }

        [Description("Do you use interfaces for abstraction?")]
        public bool UsesInterfaces { get; set; }

        #endregion
    }

    /// <summary>
    /// Assessment of data access patterns and database usage
    /// </summary>
    public class Model_DataAccessAssessment
    {
        #region Properties

        [Description("What database system do you use?")]
        public string DatabaseSystem { get; set; } = "";

        [Description("How do you access data? (Entity Framework, ADO.NET, etc.)")]
        public DataAccessTechnology DataAccessTech { get; set; }

        [Description("Do you use stored procedures extensively?")]
        public bool UsesStoredProcedures { get; set; }

        [Description("Is your data access code centralized?")]
        public bool HasCentralizedDataAccess { get; set; }

        [Description("Do you handle offline scenarios?")]
        public bool SupportsOfflineMode { get; set; }

        [Description("Do you use caching mechanisms?")]
        public bool UsesCaching { get; set; }

        [Description("How do you handle database connections?")]
        public ConnectionManagementApproach ConnectionManagement { get; set; }

        #endregion
    }

    /// <summary>
    /// Assessment of platform-specific features usage
    /// </summary>
    public class Model_PlatformFeaturesAssessment
    {
        #region Properties

        [Description("Do you use file system access extensively?")]
        public bool UsesFileSystem { get; set; }

        [Description("Do you use printing functionality?")]
        public bool UsesPrinting { get; set; }

        [Description("Do you integrate with Windows services?")]
        public bool UsesWindowsServices { get; set; }

        [Description("Do you use system tray or notifications?")]
        public bool UsesSystemTray { get; set; }

        [Description("Do you use registry access?")]
        public bool UsesRegistry { get; set; }

        [Description("Do you require elevated permissions?")]
        public bool RequiresElevatedPermissions { get; set; }

        [Description("Do you use hardware-specific features?")]
        public bool UsesHardwareFeatures { get; set; }

        [Description("List hardware features used")]
        public List<string> HardwareFeatures { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Assessment of team readiness for MAUI migration
    /// </summary>
    public class Model_TeamReadinessAssessment
    {
        #region Properties

        [Description("How many developers will work on the migration?")]
        public int DeveloperCount { get; set; }

        [Description("What is the team's experience with cross-platform development?")]
        public ExperienceLevel CrossPlatformExperience { get; set; }

        [Description("Does the team have MAUI/Xamarin experience?")]
        public ExperienceLevel MAUIExperience { get; set; }

        [Description("How familiar is the team with MVVM pattern?")]
        public ExperienceLevel MVVMExperience { get; set; }

        [Description("Are you planning to target multiple platforms?")]
        public bool TargetingMultiplePlatforms { get; set; }

        [Description("Which platforms do you want to target?")]
        public List<string> TargetPlatforms { get; set; } = new();

        [Description("Do you have resources for testing on multiple platforms?")]
        public bool HasMultiPlatformTesting { get; set; }

        #endregion
    }

    /// <summary>
    /// Assessment of project scope and timeline
    /// </summary>
    public class Model_ProjectScopeAssessment
    {
        #region Properties

        [Description("What is your estimated timeline for migration?")]
        public string EstimatedTimeline { get; set; } = "";

        [Description("What is your budget range for this migration?")]
        public BudgetRange Budget { get; set; }

        [Description("Is this a complete rewrite or gradual migration?")]
        public MigrationApproach Approach { get; set; }

        [Description("Do you need to maintain the existing application during migration?")]
        public bool MaintainExistingApp { get; set; }

        [Description("What is the primary driver for MAUI migration?")]
        public MigrationDriver PrimaryDriver { get; set; }

        [Description("How critical is maintaining exact UI appearance?")]
        public ImportanceLevel UIFidelity { get; set; }

        [Description("How important is cross-platform compatibility?")]
        public ImportanceLevel CrossPlatformImportance { get; set; }

        #endregion
    }

    /// <summary>
    /// Assessment results and recommendations
    /// </summary>
    public class Model_AssessmentResults
    {
        #region Properties

        public int OverallReadinessScore { get; set; }
        public MigrationComplexity EstimatedComplexity { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public List<string> Challenges { get; set; } = new();
        public List<string> Prerequisites { get; set; } = new();
        public string EstimatedEffort { get; set; } = "";
        public List<string> RiskFactors { get; set; } = new();
        public List<string> NextSteps { get; set; } = new();

        #endregion
    }

    #endregion

    #region Enums

    public enum UIComplexityLevel
    {
        Simple,
        Moderate,
        Complex,
        VeryComplex
    }

    public enum StylingApproach
    {
        None,
        BasicStyling,
        CustomThemes,
        AdvancedTheming
    }

    public enum DataBindingUsage
    {
        None,
        Minimal,
        Moderate,
        Extensive
    }

    public enum LogicSeparationLevel
    {
        Poor,
        Fair,
        Good,
        Excellent
    }

    public enum ArchitecturalPattern
    {
        None,
        MVP,
        MVVM,
        MVC,
        Other
    }

    public enum CodeBehindUsage
    {
        Minimal,
        Moderate,
        Heavy,
        Extensive
    }

    public enum DependencyInjectionUsage
    {
        None,
        Manual,
        Container,
        Framework
    }

    public enum DataAccessTechnology
    {
        ADONET,
        EntityFramework,
        Dapper,
        Custom,
        Other
    }

    public enum ConnectionManagementApproach
    {
        Manual,
        Pooling,
        Framework,
        Custom
    }

    public enum ExperienceLevel
    {
        None,
        Beginner,
        Intermediate,
        Advanced,
        Expert
    }

    public enum BudgetRange
    {
        Small,
        Medium,
        Large,
        Enterprise
    }

    public enum MigrationApproach
    {
        CompleteRewrite,
        GradualMigration,
        Hybrid,
        Undecided
    }

    public enum MigrationDriver
    {
        CrossPlatform,
        Modernization,
        Performance,
        Maintenance,
        BusinessRequirement
    }

    public enum ImportanceLevel
    {
        NotImportant,
        SomewhatImportant,
        Important,
        VeryImportant,
        Critical
    }

    public enum MigrationComplexity
    {
        Low,
        Medium,
        High,
        VeryHigh
    }

    #endregion
}