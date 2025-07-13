using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MTM_Inventory_Application.Tools.RegionOrganizer;

public class RegionOrganizer
{
    private readonly List<string> _logEntries = new();
    private readonly string _logDirectory;
    private readonly bool _previewMode;

    public RegionOrganizer(string logDirectory, bool previewMode = false)
    {
        _logDirectory = logDirectory;
        _previewMode = previewMode;
    }

    public void ProcessFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            LogEntry($"File not found: {filePath}");
            return;
        }

        var content = File.ReadAllText(filePath);
        var originalContent = content;

        var organizedContent = OrganizeIntoRegions(content, filePath);

        if (organizedContent != originalContent)
        {
            if (!_previewMode)
            {
                File.WriteAllText(filePath, organizedContent);
                LogEntry($"Successfully organized regions in: {filePath}");
            }
            else
            {
                LogEntry($"PREVIEW: Would organize regions in: {filePath}");
            }
        }
        else
        {
            LogEntry($"No changes needed in: {filePath}");
        }
    }

    private string OrganizeIntoRegions(string content, string filePath)
    {
        var lines = content.Split('\n');
        var result = new StringBuilder();
        var currentRegion = "";
        var inClass = false;
        var classIndentation = "";
        var regions = new Dictionary<string, List<string>>();
        var beforeClass = new List<string>();
        var afterClass = new List<string>();
        var classStartLine = "";
        var classEndLine = "";
        var inNamespace = false;
        var foundClassStart = false;
        var foundClassEnd = false;
        var classDepth = 0;

        InitializeRegions(regions);

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("namespace "))
            {
                inNamespace = true;
                beforeClass.Add(line);
                continue;
            }

            if (!foundClassStart && (trimmedLine.StartsWith("public partial class") || 
                                   trimmedLine.StartsWith("public class") || 
                                   trimmedLine.StartsWith("partial class")))
            {
                foundClassStart = true;
                inClass = true;
                classIndentation = GetIndentation(line);
                classStartLine = line;
                
                if (trimmedLine.EndsWith("{"))
                {
                    classDepth = 1;
                }
                continue;
            }

            if (!foundClassStart)
            {
                beforeClass.Add(line);
                continue;
            }

            if (inClass)
            {
                if (trimmedLine.Contains("{"))
                {
                    classDepth++;
                }
                if (trimmedLine.Contains("}"))
                {
                    classDepth--;
                    if (classDepth == 0)
                    {
                        foundClassEnd = true;
                        inClass = false;
                        classEndLine = line;
                        continue;
                    }
                }

                if (classDepth == 1)
                {
                    CategorizeClassMember(line, regions, classIndentation);
                }
                else if (classDepth > 1)
                {
                    if (regions.ContainsKey(currentRegion))
                    {
                        regions[currentRegion].Add(line);
                    }
                    else
                    {
                        regions["Methods"].Add(line);
                    }
                }
            }
            else if (foundClassEnd)
            {
                afterClass.Add(line);
            }
        }

        return BuildOrganizedContent(beforeClass, classStartLine, regions, classEndLine, afterClass, classIndentation);
    }

    private void InitializeRegions(Dictionary<string, List<string>> regions)
    {
        var regionNames = new[] { "Fields", "Properties", "Constructors", "Methods", "Events" };
        foreach (var regionName in regionNames)
        {
            regions[regionName] = new List<string>();
        }
    }

    private void CategorizeClassMember(string line, Dictionary<string, List<string>> regions, string classIndentation)
    {
        var trimmedLine = line.Trim();
        var indentedLine = line;

        if (string.IsNullOrWhiteSpace(trimmedLine))
        {
            return;
        }

        if (trimmedLine.StartsWith("private") || trimmedLine.StartsWith("public") || 
            trimmedLine.StartsWith("protected") || trimmedLine.StartsWith("internal"))
        {
            if (IsField(trimmedLine))
            {
                regions["Fields"].Add(indentedLine);
            }
            else if (IsProperty(trimmedLine))
            {
                regions["Properties"].Add(indentedLine);
            }
            else if (IsConstructor(trimmedLine))
            {
                regions["Constructors"].Add(indentedLine);
            }
            else if (IsEvent(trimmedLine))
            {
                regions["Events"].Add(indentedLine);
            }
            else
            {
                regions["Methods"].Add(indentedLine);
            }
        }
        else
        {
            regions["Methods"].Add(indentedLine);
        }
    }

    private bool IsField(string line)
    {
        return !line.Contains("(") && !line.Contains("{") && line.Contains(";");
    }

    private bool IsProperty(string line)
    {
        return line.Contains("{ get") || line.Contains("{ set") || 
               (line.Contains("{") && !line.Contains("(") && !line.Contains("="));
    }

    private bool IsConstructor(string line)
    {
        var words = line.Split(' ');
        return words.Any(w => w.Contains("(") && !w.StartsWith("="));
    }

    private bool IsEvent(string line)
    {
        return line.Contains("event ") || line.Contains("EventHandler") || line.Contains("+=") || line.Contains("-=");
    }

    private string GetIndentation(string line)
    {
        var indentCount = 0;
        foreach (var ch in line)
        {
            if (ch == ' ' || ch == '\t')
            {
                indentCount++;
            }
            else
            {
                break;
            }
        }
        return new string(' ', indentCount);
    }

    private string BuildOrganizedContent(List<string> beforeClass, string classStartLine, 
                                       Dictionary<string, List<string>> regions, 
                                       string classEndLine, List<string> afterClass, 
                                       string classIndentation)
    {
        var result = new StringBuilder();

        foreach (var line in beforeClass)
        {
            result.AppendLine(line);
        }

        result.AppendLine(classStartLine);

        var regionIndentation = classIndentation + "    ";
        foreach (var regionName in new[] { "Fields", "Properties", "Constructors", "Methods", "Events" })
        {
            if (regions[regionName].Count > 0)
            {
                result.AppendLine();
                result.AppendLine($"{regionIndentation}#region {regionName}");
                result.AppendLine();
                
                foreach (var line in regions[regionName])
                {
                    result.AppendLine(line);
                }
                
                result.AppendLine();
                result.AppendLine($"{regionIndentation}#endregion");
            }
        }

        result.AppendLine(classEndLine);

        foreach (var line in afterClass)
        {
            result.AppendLine(line);
        }

        return result.ToString();
    }

    public void ProcessDirectory(string directoryPath, string pattern = "*.cs")
    {
        if (!Directory.Exists(directoryPath))
        {
            LogEntry($"Directory not found: {directoryPath}");
            return;
        }

        var files = Directory.GetFiles(directoryPath, pattern, SearchOption.AllDirectories)
                          .Where(f => !f.Contains("AssemblyInfo.cs") && 
                                     !f.Contains("Resources.Designer.cs") && 
                                     !f.Contains(".Designer.cs"))
                          .ToArray();

        LogEntry($"Processing {files.Length} files in directory: {directoryPath}");

        foreach (var file in files)
        {
            ProcessFile(file);
        }
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
        var logFileName = $"RegionOrganizer_Run_{timestamp}.log";
        var logPath = Path.Combine(_logDirectory, logFileName);

        Directory.CreateDirectory(_logDirectory);
        File.WriteAllLines(logPath, _logEntries);
        LogEntry($"Log saved to: {logPath}");
    }

    public void PrintSummary()
    {
        var filesProcessed = _logEntries.Count(e => e.Contains("Successfully organized") || e.Contains("PREVIEW: Would organize"));
        var filesNoChange = _logEntries.Count(e => e.Contains("No changes needed"));
        
        Console.WriteLine("\n=== REGION ORGANIZER SUMMARY ===");
        Console.WriteLine($"Mode: {(_previewMode ? "PREVIEW" : "EXECUTE")}");
        Console.WriteLine($"Files organized: {filesProcessed}");
        Console.WriteLine($"Files unchanged: {filesNoChange}");
        Console.WriteLine("=================================\n");
    }

    public static void Main(string[] args)
    {
        var baseDirectory = Directory.GetCurrentDirectory();
        var logDirectory = Path.Combine(baseDirectory, "Tools", "RegionOrganizer", "Logs");
        var previewMode = args.Contains("--preview");

        var organizer = new RegionOrganizer(logDirectory, previewMode);

        Console.WriteLine("MTM WIP Application - Region Organizer Tool");
        Console.WriteLine($"Running in {(previewMode ? "PREVIEW" : "EXECUTE")} mode");
        Console.WriteLine("============================================");

        if (previewMode)
        {
            Console.WriteLine("Preview mode enabled - no files will be modified");
        }

        organizer.ProcessDirectory(baseDirectory);
        organizer.PrintSummary();
        organizer.SaveLog();
    }
}