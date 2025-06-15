using System;
using System.Linq;
using System.Windows.Forms;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace MTM_WIP_Application;

public partial class Exporter_Do_Not_Pack : Form
{
    public Exporter_Do_Not_Pack()
    {
        InitializeComponent();
    }

    private void BtnExport_Click(object? sender, EventArgs e)
    {
        try
        {
            using (var doc = DocX.Create(@"C:\Users\johnk\Documents\WIPAppChangeLog.docx"))
            {
                // Title
                doc.InsertParagraph("WIP App – Weekly Change Log")
                    .FontSize(20)
                    .Bold()
                    .SpacingAfter(20)
                    .Alignment = Alignment.center;

                // Section: Server-Side Upgrades
                doc.InsertParagraph("Server-Side Upgrades")
                    .FontSize(16)
                    .Bold()
                    .SpacingAfter(8);
                doc.InsertParagraph(FormatSectionWithIcons(rtbServerSideUpgrades.Text))
                    .FontSize(12)
                    .SpacingAfter(16);

                // Section: Logging & Error Handling
                doc.InsertParagraph("Logging & Error Handling")
                    .FontSize(16)
                    .Bold()
                    .SpacingAfter(8);
                doc.InsertParagraph(FormatSectionWithIcons(rtbLogging.Text))
                    .FontSize(12)
                    .SpacingAfter(16);

                // Section: User Interface & Usability
                doc.InsertParagraph("User Interface & Usability")
                    .FontSize(16)
                    .Bold()
                    .SpacingAfter(8);
                doc.InsertParagraph(FormatSectionWithIcons(rtbUI.Text))
                    .FontSize(12)
                    .SpacingAfter(16);

                // Section: Inventory Management Features
                doc.InsertParagraph("Inventory Management Features")
                    .FontSize(16)
                    .Bold()
                    .SpacingAfter(8);
                doc.InsertParagraph(FormatSectionWithIcons(rtbInventoryFeatures.Text))
                    .FontSize(12)
                    .SpacingAfter(16);

                // Section: Advanced Functionality & Enhancements
                doc.InsertParagraph("Advanced Functionality & Enhancements")
                    .FontSize(16)
                    .Bold()
                    .SpacingAfter(8);
                doc.InsertParagraph(FormatSectionWithIcons(rtbAdvancedEnhancements.Text))
                    .FontSize(12)
                    .SpacingAfter(16);

                // Section: Additional Fixes & Improvements
                doc.InsertParagraph("Additional Fixes & Improvements")
                    .FontSize(16)
                    .Bold()
                    .SpacingAfter(8);
                doc.InsertParagraph(FormatSectionWithIcons(rtbAdditionalFixes.Text))
                    .FontSize(12)
                    .SpacingAfter(16);

                doc.Save();
            }

            MessageBox.Show("Change log exported successfully to WIPAppChangeLog.docx.", @"Export Complete",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}", @"Export Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string FormatSectionWithIcons(string sectionText)
    {
        if (string.IsNullOrWhiteSpace(sectionText))
            return string.Empty;

        var lines = sectionText.Split(["\r\n", "\n"], StringSplitOptions.None);
        var formattedLines = lines.Select(line =>
        {
            var trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed))
                return string.Empty;

            var firstWord = trimmed.Split(' ', 2)[0];
            var icon = GetIconForUpdateType(firstWord);
            return $"{icon} {trimmed}";
        });

        return string.Join(Environment.NewLine, formattedLines);
    }

    private static string GetIconForUpdateType(string updateType)
    {
        return updateType.ToLower() switch
        {
            "added" => "🟢",
            "fixed" => "🛠️",
            "removed" => "❌",
            "updated" => "🔄",
            "refactored" => "♻️",
            "bulk" => "💾",
            "async" => "🔄",
            "import" => "📥",
            "export" => "📤",
            "search" => "🔍",
            "validation" => "✅",
            "nothing" => "❗",
            "undo" => "↩️",
            "tracking" => "📋",
            "transaction" => "📈",
            "centralizes" => "🗃️",
            "error" => "🚨",
            "thread" => "🧵",
            "connection" => "🧩",
            "file" => "📄",
            "standardized" => "⚙️",
            "general" => "🧹",
            "tab" => "🖥️",
            "resource" => "📁",
            "focus" => "🎯",
            "handler" => "✂️",
            "feature" => "🆕",
            "move" => "🚚",
            "template" => "📦",
            "code" => "🔧",
            "namespace" => "🧩",
            "improved" => "✨",
            _ => "•"
        };
    }
}