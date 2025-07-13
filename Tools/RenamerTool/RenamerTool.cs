using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MTM_Inventory_Application.Tools.RenamerTool;

public class RenamerTool
{
    private readonly Dictionary<string, string> _renamingMap = new();
    private readonly List<string> _logEntries = new();
    private readonly string _logDirectory;
    private readonly bool _previewMode;

    public RenamerTool(string logDirectory, bool previewMode = false)
    {
        _logDirectory = logDirectory;
        _previewMode = previewMode;
        
        InitializeRenamingMap();
    }

    private void InitializeRenamingMap()
    {
        var transactionControlMappings = new Dictionary<string, string>
        {
            {"comboSortBy", "Transactions_ComboBox_SortBy"},
            {"lblSortBy", "Transactions_Label_SortBy"},
            {"txtSearchPartID", "Transactions_TextBox_SearchPartID"},
            {"lblSearchPartID", "Transactions_Label_SearchPartID"},
            {"btnReset", "Transactions_Button_Reset"},
            {"tabControlMain", "Transactions_TabControl_Main"},
            {"tabPartEntry", "Transactions_TabPage_PartEntry"},
            {"tabPartRemoval", "Transactions_TabPage_PartRemoval"},
            {"tabPartTransfer", "Transactions_TabPage_PartTransfer"},
            {"dataGridTransactions", "Transactions_DataGridView_Transactions"},
            {"panelBottom", "Transactions_Panel_Bottom"},
            {"lblSortByUser", "Transactions_Label_SortByUser"},
            {"comboUser", "Transactions_ComboBox_User"},
            {"lblUser", "Transactions_Label_User"},
            {"comboUserName", "Transactions_ComboBox_UserName"},
            {"lblUserName", "Transactions_Label_UserName"},
            {"comboShift", "Transactions_ComboBox_Shift"},
            {"lblShift", "Transactions_Label_Shift"}
        };

        foreach (var mapping in transactionControlMappings)
        {
            _renamingMap[mapping.Key] = mapping.Value;
        }
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

        foreach (var mapping in _renamingMap)
        {
            var oldName = mapping.Key;
            var newName = mapping.Value;
            
            var pattern = $@"\b{Regex.Escape(oldName)}\b";
            var matches = Regex.Matches(content, pattern);
            
            if (matches.Count > 0)
            {
                content = Regex.Replace(content, pattern, newName);
                LogEntry($"Renamed '{oldName}' to '{newName}' in {filePath} ({matches.Count} occurrences)");
            }
        }

        if (content != originalContent)
        {
            if (!_previewMode)
            {
                File.WriteAllText(filePath, content);
                LogEntry($"Successfully updated file: {filePath}");
            }
            else
            {
                LogEntry($"PREVIEW: Would update file: {filePath}");
            }
        }
    }

    public void ProcessDirectory(string directoryPath, string pattern = "*.cs")
    {
        if (!Directory.Exists(directoryPath))
        {
            LogEntry($"Directory not found: {directoryPath}");
            return;
        }

        var files = Directory.GetFiles(directoryPath, pattern, SearchOption.AllDirectories);
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
        var logFileName = $"RenamerTool_Run_{timestamp}.log";
        var logPath = Path.Combine(_logDirectory, logFileName);

        Directory.CreateDirectory(_logDirectory);
        File.WriteAllLines(logPath, _logEntries);
        LogEntry($"Log saved to: {logPath}");
    }

    public void PrintSummary()
    {
        var renameCount = _logEntries.Count(e => e.Contains("Renamed"));
        var fileCount = _logEntries.Count(e => e.Contains("Successfully updated file") || e.Contains("PREVIEW: Would update file"));
        
        Console.WriteLine("\n=== RENAMER TOOL SUMMARY ===");
        Console.WriteLine($"Mode: {(_previewMode ? "PREVIEW" : "EXECUTE")}");
        Console.WriteLine($"Total renamings: {renameCount}");
        Console.WriteLine($"Files affected: {fileCount}");
        Console.WriteLine($"Renaming mappings configured: {_renamingMap.Count}");
        Console.WriteLine("============================\n");
    }

    public static void Main(string[] args)
    {
        var baseDirectory = Directory.GetCurrentDirectory();
        var logDirectory = Path.Combine(baseDirectory, "Tools", "RenamerTool", "Logs");
        var previewMode = args.Contains("--preview");

        var renamer = new RenamerTool(logDirectory, previewMode);

        Console.WriteLine("MTM WIP Application - Renamer Tool");
        Console.WriteLine($"Running in {(previewMode ? "PREVIEW" : "EXECUTE")} mode");
        Console.WriteLine("=====================================");

        if (previewMode)
        {
            Console.WriteLine("Preview mode enabled - no files will be modified");
        }

        renamer.ProcessDirectory(baseDirectory);
        renamer.PrintSummary();
        renamer.SaveLog();
    }
}