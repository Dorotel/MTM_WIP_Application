using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Models;

internal class SavedLocationsTable
{
    public int Id { get; set; }
    public string? Location { get; set; }
    public string? PartId { get; set; }
    public string? Op { get; set; }
    public string? Notes { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public string? User { get; set; }
    public string? Type { get; set; }
}

internal class HistoryB
{
    public int Id { get; set; }
    public string? Location { get; set; }
    public string? PartId { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public string? User { get; set; }
    public string? Type { get; set; }
}

internal class AdminList
{
    public int Id { get; set; }
    public string? User { get; set; }
    public string? HideChangeLog { get; set; }
    public string? LastShownVersion { get; set; }
    public string? VisualUserName { get; set; }
    public string? VisualPassword { get; set; }
    public string? ThemeName { get; set; }
    public int? ThemeFontSize { get; set; }
}

internal class Last10
{
    public int? Id { get; set; }
    public string? PartId { get; set; }
    public string? Op { get; set; }
    public int? Quantity { get; set; }
}

internal class VersionHistory
{
    public int? Id { get; set; }
    public string? Version { get; set; }
    public string? Notes { get; set; }
}

internal class ChangeNotes
{
    internal int Id { get; set; }
    public string? PartId { get; set; }
    public string? Operation { get; set; }
    public string? Location { get; set; }
    public string? Note { get; set; }
}