namespace MTM_MAUI_Application.Models;

/// <summary>
/// Represents a transaction in the MTM WIP system
/// </summary>
public class Transaction
{
    public int Id { get; set; }
    public TransactionType TransactionType { get; set; }
    public string? BatchNumber { get; set; }
    public string PartId { get; set; } = string.Empty;
    public string? FromLocation { get; set; }
    public string? ToLocation { get; set; }
    public string Operation { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? Notes { get; set; }
    public string User { get; set; } = string.Empty;
    public string ItemType { get; set; } = string.Empty;
    public DateTime DateTime { get; set; } = DateTime.Now;
}

/// <summary>
/// Types of transactions in the system
/// </summary>
public enum TransactionType
{
    Inventory,
    Transfer,
    Remove,
    Adjustment
}