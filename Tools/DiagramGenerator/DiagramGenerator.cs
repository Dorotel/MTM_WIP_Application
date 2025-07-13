using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MTM_Inventory_Application.Tools.DiagramGenerator;

public class DiagramGenerator
{
    private readonly List<string> _logEntries = new();
    private readonly string _logDirectory;
    private readonly string _outputDirectory;
    private readonly bool _previewMode;

    public DiagramGenerator(string logDirectory, string outputDirectory, bool previewMode = false)
    {
        _logDirectory = logDirectory;
        _outputDirectory = outputDirectory;
        _previewMode = previewMode;
    }

    public void GenerateClassDiagram(string sourceFile, string className)
    {
        if (!File.Exists(sourceFile))
        {
            LogEntry($"Source file not found: {sourceFile}");
            return;
        }

        var content = File.ReadAllText(sourceFile);
        var diagram = BuildClassDiagram(content, className);
        
        var outputPath = Path.Combine(_outputDirectory, $"{className}_Class_Diagram.puml");
        
        if (!_previewMode)
        {
            Directory.CreateDirectory(_outputDirectory);
            File.WriteAllText(outputPath, diagram);
            LogEntry($"Generated class diagram: {outputPath}");
        }
        else
        {
            LogEntry($"PREVIEW: Would generate class diagram: {outputPath}");
        }
    }

    public void GenerateDependencyDiagram(string sourceFile, string className)
    {
        if (!File.Exists(sourceFile))
        {
            LogEntry($"Source file not found: {sourceFile}");
            return;
        }

        var content = File.ReadAllText(sourceFile);
        var diagram = BuildDependencyDiagram(content, className);
        
        var outputPath = Path.Combine(_outputDirectory, $"{className}_Dependency_Diagram.puml");
        
        if (!_previewMode)
        {
            Directory.CreateDirectory(_outputDirectory);
            File.WriteAllText(outputPath, diagram);
            LogEntry($"Generated dependency diagram: {outputPath}");
        }
        else
        {
            LogEntry($"PREVIEW: Would generate dependency diagram: {outputPath}");
        }
    }

    private string BuildClassDiagram(string content, string className)
    {
        var diagram = new StringBuilder();
        diagram.AppendLine("@startuml");
        diagram.AppendLine("!define WINFORMS_STEREOTYPE <<WinForms>>");
        diagram.AppendLine("!define CONTROL_STEREOTYPE <<Control>>");
        diagram.AppendLine();
        
        diagram.AppendLine($"class \"{className}\" WINFORMS_STEREOTYPE {{");
        
        // Extract fields
        var fields = ExtractFields(content);
        foreach (var field in fields)
        {
            diagram.AppendLine($"    {field}");
        }
        
        diagram.AppendLine("    --");
        
        // Extract methods
        var methods = ExtractMethods(content);
        foreach (var method in methods)
        {
            diagram.AppendLine($"    {method}");
        }
        
        diagram.AppendLine("}");
        diagram.AppendLine();
        
        // Add control type definitions
        var controlTypes = ExtractControlTypes(content);
        foreach (var controlType in controlTypes)
        {
            diagram.AppendLine($"class \"{controlType}\" CONTROL_STEREOTYPE {{");
            diagram.AppendLine("    + Properties : various");
            diagram.AppendLine("    + Methods : various");
            diagram.AppendLine("}");
            diagram.AppendLine();
        }
        
        // Add relationships
        foreach (var controlType in controlTypes)
        {
            diagram.AppendLine($"\"{className}\" *-- \"{controlType}\" : contains");
        }
        
        diagram.AppendLine("@enduml");
        return diagram.ToString();
    }

    private string BuildDependencyDiagram(string content, string className)
    {
        var diagram = new StringBuilder();
        diagram.AppendLine("@startuml");
        diagram.AppendLine("!define FORM_STEREOTYPE <<Form>>");
        diagram.AppendLine("!define MODEL_STEREOTYPE <<Model>>");
        diagram.AppendLine("!define DAO_STEREOTYPE <<DAO>>");
        diagram.AppendLine("!define HELPER_STEREOTYPE <<Helper>>");
        diagram.AppendLine("!define CORE_STEREOTYPE <<Core>>");
        diagram.AppendLine();
        
        diagram.AppendLine($"class \"{className}\" FORM_STEREOTYPE {{");
        diagram.AppendLine("    + Main functionality");
        diagram.AppendLine("}");
        diagram.AppendLine();
        
        // Extract dependencies
        var dependencies = ExtractDependencies(content);
        foreach (var dependency in dependencies)
        {
            diagram.AppendLine($"class \"{dependency}\" {{");
            diagram.AppendLine("    + Functionality");
            diagram.AppendLine("}");
            diagram.AppendLine();
            diagram.AppendLine($"\"{className}\" --> \"{dependency}\" : uses");
        }
        
        diagram.AppendLine("@enduml");
        return diagram.ToString();
    }

    private List<string> ExtractFields(string content)
    {
        var fields = new List<string>();
        var lines = content.Split('\n');
        
        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            if (trimmed.StartsWith("private ") || trimmed.StartsWith("public ") || 
                trimmed.StartsWith("protected ") || trimmed.StartsWith("internal "))
            {
                if (trimmed.Contains(";") && !trimmed.Contains("("))
                {
                    var field = trimmed.Replace("private ", "- ")
                                      .Replace("public ", "+ ")
                                      .Replace("protected ", "# ")
                                      .Replace("internal ", "~ ");
                    fields.Add(field);
                }
            }
        }
        
        return fields;
    }

    private List<string> ExtractMethods(string content)
    {
        var methods = new List<string>();
        var lines = content.Split('\n');
        
        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            if ((trimmed.StartsWith("private ") || trimmed.StartsWith("public ") || 
                 trimmed.StartsWith("protected ") || trimmed.StartsWith("internal ")) && 
                trimmed.Contains("("))
            {
                var method = trimmed.Replace("private ", "- ")
                                   .Replace("public ", "+ ")
                                   .Replace("protected ", "# ")
                                   .Replace("internal ", "~ ");
                methods.Add(method);
            }
        }
        
        return methods;
    }

    private HashSet<string> ExtractControlTypes(string content)
    {
        var controlTypes = new HashSet<string>();
        var controlPattern = @"new\s+System\.Windows\.Forms\.(\w+)";
        var matches = Regex.Matches(content, controlPattern);
        
        foreach (Match match in matches)
        {
            controlTypes.Add(match.Groups[1].Value);
        }
        
        return controlTypes;
    }

    private List<string> ExtractDependencies(string content)
    {
        var dependencies = new List<string>();
        var usingPattern = @"using\s+([^;]+);";
        var matches = Regex.Matches(content, usingPattern);
        
        foreach (Match match in matches)
        {
            var usingStatement = match.Groups[1].Value;
            if (usingStatement.Contains("MTM_Inventory_Application"))
            {
                var parts = usingStatement.Split('.');
                if (parts.Length > 2)
                {
                    dependencies.Add(parts[2]); // Get the main namespace part
                }
            }
        }
        
        return dependencies.Distinct().ToList();
    }

    private void LogEntry(string message)
    {
        var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        _logEntries.Add(logMessage);
        Console.WriteLine(logMessage);
    }

    public void SaveLog()
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        var logFileName = $"DiagramGenerator_Run_{timestamp}.log";
        var logPath = Path.Combine(_logDirectory, logFileName);

        Directory.CreateDirectory(_logDirectory);
        File.WriteAllLines(logPath, _logEntries);
        LogEntry($"Log saved to: {logPath}");
    }

    public void PrintSummary()
    {
        var diagramsGenerated = _logEntries.Count(e => e.Contains("Generated") || e.Contains("PREVIEW: Would generate"));
        
        Console.WriteLine("\n=== DIAGRAM GENERATOR SUMMARY ===");
        Console.WriteLine($"Mode: {(_previewMode ? "PREVIEW" : "EXECUTE")}");
        Console.WriteLine($"Diagrams generated: {diagramsGenerated}");
        Console.WriteLine("==================================\n");
    }

    public static void Main(string[] args)
    {
        var baseDirectory = Directory.GetCurrentDirectory();
        var logDirectory = Path.Combine(baseDirectory, "Tools", "DiagramGenerator", "Logs");
        var outputDirectory = Path.Combine(baseDirectory, "Documents", "Diagrams");
        var previewMode = args.Contains("--preview");

        var generator = new DiagramGenerator(logDirectory, outputDirectory, previewMode);

        Console.WriteLine("MTM WIP Application - Diagram Generator Tool");
        Console.WriteLine($"Running in {(previewMode ? "PREVIEW" : "EXECUTE")} mode");
        Console.WriteLine("=============================================");

        if (previewMode)
        {
            Console.WriteLine("Preview mode enabled - no files will be created");
        }

        // Example usage
        var transactionsFile = Path.Combine(baseDirectory, "Forms", "Transactions", "Transactions.cs");
        generator.GenerateClassDiagram(transactionsFile, "Transactions");
        generator.GenerateDependencyDiagram(transactionsFile, "Transactions");

        generator.PrintSummary();
        generator.SaveLog();
    }
}