using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MTM_Inventory_Application.Tools.CommentRemover;

public class CommentRemover
{
    private readonly List<string> _logEntries = new();
    private readonly string _logDirectory;
    private readonly bool _previewMode;

    public CommentRemover(string logDirectory, bool previewMode = false)
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
        var lines = content.Split('\n');
        var modifiedLines = new List<string>();
        var commentsRemoved = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var processedLine = ProcessLine(line, ref commentsRemoved);
            
            if (processedLine != null)
            {
                modifiedLines.Add(processedLine);
            }
        }

        var newContent = string.Join('\n', modifiedLines);
        
        if (newContent != originalContent)
        {
            if (!_previewMode)
            {
                File.WriteAllText(filePath, newContent);
                LogEntry($"Successfully updated file: {filePath} (removed {commentsRemoved} comment lines)");
            }
            else
            {
                LogEntry($"PREVIEW: Would update file: {filePath} (would remove {commentsRemoved} comment lines)");
            }
        }
        else
        {
            LogEntry($"No comments found in: {filePath}");
        }
    }

    private string? ProcessLine(string line, ref int commentsRemoved)
    {
        var trimmedLine = line.Trim();
        
        // Skip empty lines
        if (string.IsNullOrWhiteSpace(trimmedLine))
        {
            return line;
        }

        // Remove single line comments that start with //
        if (trimmedLine.StartsWith("//"))
        {
            commentsRemoved++;
            return null; // Remove the entire line
        }

        // Remove inline comments (// comment at end of line)
        var inlineCommentIndex = line.IndexOf("//");
        if (inlineCommentIndex >= 0)
        {
            // Make sure it's not inside a string literal
            if (!IsInsideStringLiteral(line, inlineCommentIndex))
            {
                commentsRemoved++;
                return line.Substring(0, inlineCommentIndex).TrimEnd();
            }
        }

        // Handle multi-line comments /* ... */
        var processedLine = RemoveMultiLineComments(line, ref commentsRemoved);
        
        return processedLine;
    }

    private bool IsInsideStringLiteral(string line, int position)
    {
        bool inString = false;
        bool inChar = false;
        
        for (int i = 0; i < position; i++)
        {
            if (line[i] == '"' && !inChar)
            {
                if (i == 0 || line[i - 1] != '\\')
                {
                    inString = !inString;
                }
            }
            else if (line[i] == '\'' && !inString)
            {
                if (i == 0 || line[i - 1] != '\\')
                {
                    inChar = !inChar;
                }
            }
        }
        
        return inString || inChar;
    }

    private string RemoveMultiLineComments(string line, ref int commentsRemoved)
    {
        var result = new StringBuilder();
        var i = 0;
        
        while (i < line.Length)
        {
            if (i < line.Length - 1 && line[i] == '/' && line[i + 1] == '*')
            {
                // Found start of multi-line comment
                if (!IsInsideStringLiteral(line, i))
                {
                    commentsRemoved++;
                    i += 2; // Skip /*
                    
                    // Find the end of the comment
                    while (i < line.Length - 1)
                    {
                        if (line[i] == '*' && line[i + 1] == '/')
                        {
                            i += 2; // Skip */
                            break;
                        }
                        i++;
                    }
                }
                else
                {
                    result.Append(line[i]);
                    i++;
                }
            }
            else
            {
                result.Append(line[i]);
                i++;
            }
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
        var logFileName = $"CommentRemover_Run_{timestamp}.log";
        var logPath = Path.Combine(_logDirectory, logFileName);

        Directory.CreateDirectory(_logDirectory);
        File.WriteAllLines(logPath, _logEntries);
        LogEntry($"Log saved to: {logPath}");
    }

    public void PrintSummary()
    {
        var filesProcessed = _logEntries.Count(e => e.Contains("Successfully updated file") || e.Contains("PREVIEW: Would update file"));
        var commentsRemoved = _logEntries.Where(e => e.Contains("removed") || e.Contains("would remove"))
                                        .Sum(e => ExtractCommentCount(e));
        
        Console.WriteLine("\n=== COMMENT REMOVER SUMMARY ===");
        Console.WriteLine($"Mode: {(_previewMode ? "PREVIEW" : "EXECUTE")}");
        Console.WriteLine($"Files processed: {filesProcessed}");
        Console.WriteLine($"Comments removed: {commentsRemoved}");
        Console.WriteLine("===============================\n");
    }

    private int ExtractCommentCount(string logEntry)
    {
        var match = Regex.Match(logEntry, @"removed (\d+) comment|would remove (\d+) comment");
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value != "" ? match.Groups[1].Value : match.Groups[2].Value);
        }
        return 0;
    }

    public static void Main(string[] args)
    {
        var baseDirectory = Directory.GetCurrentDirectory();
        var logDirectory = Path.Combine(baseDirectory, "Tools", "CommentRemover", "Logs");
        var previewMode = args.Contains("--preview");

        var remover = new CommentRemover(logDirectory, previewMode);

        Console.WriteLine("MTM WIP Application - Comment Remover Tool");
        Console.WriteLine($"Running in {(previewMode ? "PREVIEW" : "EXECUTE")} mode");
        Console.WriteLine("==========================================");

        if (previewMode)
        {
            Console.WriteLine("Preview mode enabled - no files will be modified");
        }

        remover.ProcessDirectory(baseDirectory);
        remover.PrintSummary();
        remover.SaveLog();
    }
}