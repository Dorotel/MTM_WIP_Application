using System.ComponentModel;

namespace MTM_Inventory_Application.Models
{
    #region Application Analysis Models

    /// <summary>
    /// Comprehensive analysis result container for MTM WIP Application
    /// Used for MAUI migration planning and documentation
    /// </summary>
    public class Model_ApplicationAnalysis
    {
        #region Properties

        public DateTime AnalysisTimestamp { get; set; } = DateTime.Now;
        public string ApplicationVersion { get; set; } = string.Empty;
        public string AnalysisVersion { get; set; } = "1.0.0";
        public Model_UIAnalysis UIAnalysis { get; set; } = new();
        public Model_DatabaseAnalysis DatabaseAnalysis { get; set; } = new();
        public Model_BusinessLogicAnalysis BusinessLogicAnalysis { get; set; } = new();
        public Model_ThemingAnalysis ThemingAnalysis { get; set; } = new();
        public Model_ErrorHandlingAnalysis ErrorHandlingAnalysis { get; set; } = new();
        public Model_EnvironmentAnalysis EnvironmentAnalysis { get; set; } = new();

        #endregion

        #region Helpers

        public Dictionary<string, object> GetSummaryStatistics()
        {
            return new Dictionary<string, object>
            {
                ["TotalForms"] = UIAnalysis.Forms.Count,
                ["TotalControls"] = UIAnalysis.Controls.Count,
                ["TotalStoredProcedures"] = DatabaseAnalysis.StoredProcedures.Count,
                ["TotalTables"] = DatabaseAnalysis.Tables.Count,
                ["TotalBusinessLogicClasses"] = BusinessLogicAnalysis.Classes.Count,
                ["TotalThemeFiles"] = ThemingAnalysis.ThemeFiles.Count,
                ["TotalErrorHandlers"] = ErrorHandlingAnalysis.ErrorHandlers.Count,
                ["AnalysisTimestamp"] = AnalysisTimestamp,
                ["ApplicationVersion"] = ApplicationVersion
            };
        }

        #endregion
    }

    /// <summary>
    /// UI Structure and Components Analysis
    /// </summary>
    public class Model_UIAnalysis
    {
        #region Properties

        public List<Model_FormInfo> Forms { get; set; } = new();
        public List<Model_ControlInfo> Controls { get; set; } = new();
        public List<Model_LayoutInfo> Layouts { get; set; } = new();
        public Model_NavigationInfo Navigation { get; set; } = new();
        public Dictionary<string, object> UIMetrics { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Database Schema and Stored Procedures Analysis
    /// </summary>
    public class Model_DatabaseAnalysis
    {
        #region Properties

        public List<Model_TableInfo> Tables { get; set; } = new();
        public List<Model_StoredProcedureInfo> StoredProcedures { get; set; } = new();
        public List<Model_DatabaseRelationship> Relationships { get; set; } = new();
        public Model_DatabaseConfiguration Configuration { get; set; } = new();
        public Dictionary<string, object> DatabaseMetrics { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Business Logic and Data Access Patterns Analysis
    /// </summary>
    public class Model_BusinessLogicAnalysis
    {
        #region Properties

        public List<Model_ClassInfo> Classes { get; set; } = new();
        public List<Model_ServiceInfo> Services { get; set; } = new();
        public List<Model_HelperInfo> Helpers { get; set; } = new();
        public List<Model_DataAccessPattern> DataAccessPatterns { get; set; } = new();
        public Dictionary<string, object> BusinessLogicMetrics { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Theming and Styling System Analysis
    /// </summary>
    public class Model_ThemingAnalysis
    {
        #region Properties

        public List<Model_ThemeFileInfo> ThemeFiles { get; set; } = new();
        public List<Model_StylePattern> StylePatterns { get; set; } = new();
        public Model_DpiScalingInfo DpiScaling { get; set; } = new();
        public Dictionary<string, string> ThemeConstants { get; set; } = new();
        public Dictionary<string, object> ThemingMetrics { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Error Handling Patterns Analysis
    /// </summary>
    public class Model_ErrorHandlingAnalysis
    {
        #region Properties

        public List<Model_ErrorHandlerInfo> ErrorHandlers { get; set; } = new();
        public List<Model_ExceptionPattern> ExceptionPatterns { get; set; } = new();
        public Model_LoggingConfiguration LoggingConfiguration { get; set; } = new();
        public Dictionary<string, object> ErrorHandlingMetrics { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Environment and Configuration Analysis
    /// </summary>
    public class Model_EnvironmentAnalysis
    {
        #region Properties

        public Model_ConfigurationInfo Configuration { get; set; } = new();
        public List<Model_DependencyInfo> Dependencies { get; set; } = new();
        public Model_DeploymentInfo Deployment { get; set; } = new();
        public Dictionary<string, object> EnvironmentMetrics { get; set; } = new();

        #endregion
    }

    #endregion

    #region Detailed Analysis Models

    /// <summary>
    /// Form Information Details
    /// </summary>
    public class Model_FormInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public string BaseClass { get; set; } = string.Empty;
        public List<string> Controls { get; set; } = new();
        public List<string> Events { get; set; } = new();
        public List<string> Methods { get; set; } = new();
        public Dictionary<string, object> Properties { get; set; } = new();
        public List<string> Dependencies { get; set; } = new();
        public string Purpose { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Control Information Details
    /// </summary>
    public class Model_ControlInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ParentForm { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public List<string> Properties { get; set; } = new();
        public List<string> Events { get; set; } = new();
        public List<string> Methods { get; set; } = new();
        public string Purpose { get; set; } = string.Empty;
        public bool IsCustomControl { get; set; }

        #endregion
    }

    /// <summary>
    /// Layout Information Details
    /// </summary>
    public class Model_LayoutInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ParentContainer { get; set; } = string.Empty;
        public List<string> ChildControls { get; set; } = new();
        public Dictionary<string, object> LayoutProperties { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Navigation Structure Information
    /// </summary>
    public class Model_NavigationInfo
    {
        #region Properties

        public List<string> MenuItems { get; set; } = new();
        public List<string> TabPages { get; set; } = new();
        public List<string> NavigationMethods { get; set; } = new();
        public Dictionary<string, string> NavigationFlow { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Table Information Details
    /// </summary>
    public class Model_TableInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public List<Model_ColumnInfo> Columns { get; set; } = new();
        public List<string> Indexes { get; set; } = new();
        public List<string> ForeignKeys { get; set; } = new();
        public string Purpose { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Column Information Details
    /// </summary>
    public class Model_ColumnInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsAutoIncrement { get; set; }
        public string DefaultValue { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Stored Procedure Information Details
    /// </summary>
    public class Model_StoredProcedureInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public List<Model_ParameterInfo> Parameters { get; set; } = new();
        public string Purpose { get; set; } = string.Empty;
        public List<string> UsedByClasses { get; set; } = new();
        public string Body { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Parameter Information Details
    /// </summary>
    public class Model_ParameterInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty; // IN, OUT, INOUT
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Database Relationship Information
    /// </summary>
    public class Model_DatabaseRelationship
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string ParentTable { get; set; } = string.Empty;
        public string ChildTable { get; set; } = string.Empty;
        public string ParentColumn { get; set; } = string.Empty;
        public string ChildColumn { get; set; } = string.Empty;
        public string RelationshipType { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Database Configuration Information
    /// </summary>
    public class Model_DatabaseConfiguration
    {
        #region Properties

        public string ServerAddress { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public Dictionary<string, string> ConnectionSettings { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Class Information Details
    /// </summary>
    public class Model_ClassInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public string BaseClass { get; set; } = string.Empty;
        public List<string> Interfaces { get; set; } = new();
        public List<string> Methods { get; set; } = new();
        public List<string> Properties { get; set; } = new();
        public List<string> Fields { get; set; } = new();
        public string Purpose { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // DAO, Service, Helper, etc.

        #endregion
    }

    /// <summary>
    /// Service Information Details
    /// </summary>
    public class Model_ServiceInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public List<string> PublicMethods { get; set; } = new();
        public List<string> Dependencies { get; set; } = new();
        public string Purpose { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Helper Information Details
    /// </summary>
    public class Model_HelperInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public List<string> UtilityMethods { get; set; } = new();
        public string Category { get; set; } = string.Empty; // Database, UI, File, etc.
        public string Purpose { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Data Access Pattern Information
    /// </summary>
    public class Model_DataAccessPattern
    {
        #region Properties

        public string PatternName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Examples { get; set; } = new();
        public List<string> UsedByClasses { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Theme File Information
    /// </summary>
    public class Model_ThemeFileInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public List<string> ThemeMethods { get; set; } = new();
        public Dictionary<string, object> ThemeProperties { get; set; } = new();
        public string Purpose { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Style Pattern Information
    /// </summary>
    public class Model_StylePattern
    {
        #region Properties

        public string PatternName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Examples { get; set; } = new();
        public string Usage { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// DPI Scaling Information
    /// </summary>
    public class Model_DpiScalingInfo
    {
        #region Properties

        public bool IsEnabled { get; set; }
        public List<string> ScalingMethods { get; set; } = new();
        public Dictionary<string, object> ScalingProperties { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Error Handler Information
    /// </summary>
    public class Model_ErrorHandlerInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public List<string> HandledExceptions { get; set; } = new();
        public List<string> Methods { get; set; } = new();
        public string Purpose { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Exception Pattern Information
    /// </summary>
    public class Model_ExceptionPattern
    {
        #region Properties

        public string PatternName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Examples { get; set; } = new();
        public string Usage { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Logging Configuration Information
    /// </summary>
    public class Model_LoggingConfiguration
    {
        #region Properties

        public string LoggingFramework { get; set; } = string.Empty;
        public List<string> LogLevels { get; set; } = new();
        public List<string> LogTargets { get; set; } = new();
        public Dictionary<string, object> Configuration { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Configuration Information
    /// </summary>
    public class Model_ConfigurationInfo
    {
        #region Properties

        public Dictionary<string, string> AppSettings { get; set; } = new();
        public Dictionary<string, string> ConnectionStrings { get; set; } = new();
        public List<string> ConfigurationFiles { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// Dependency Information
    /// </summary>
    public class Model_DependencyInfo
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // NuGet, Assembly, etc.
        public string Purpose { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// Deployment Information
    /// </summary>
    public class Model_DeploymentInfo
    {
        #region Properties

        public string DeploymentType { get; set; } = string.Empty;
        public List<string> RequiredFiles { get; set; } = new();
        public Dictionary<string, string> EnvironmentSettings { get; set; } = new();

        #endregion
    }

    #endregion
}