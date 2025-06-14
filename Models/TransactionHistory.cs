namespace MTM_WIP_Application.Models;

internal class TransactionHistory
{
    public int Id { get; set; }
    public string TransactionType { get; set; } = string.Empty; // "IN", "OUT", "TRANSFER"
    public string PartId { get; set; } = string.Empty;
    public string? FromLocation { get; set; }
    public string? ToLocation { get; set; }
    public string? Operation { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
    public string User { get; set; } = string.Empty;
    public string? ItemType { get; set; }
    public string? BatchNumber { get; set; } // Added for new inv_transaction structure
    public DateTime DateTime { get; set; } = DateTime.Now;
}