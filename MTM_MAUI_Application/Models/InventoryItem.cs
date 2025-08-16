using System.ComponentModel.DataAnnotations;

namespace MTM_MAUI_Application.Models;

/// <summary>
/// Represents an inventory item in the MTM WIP system
/// </summary>
public class InventoryItem
{
    public int Id { get; set; }
    
    [Required]
    public string PartId { get; set; } = string.Empty;
    
    [Required]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    public string Operation { get; set; } = string.Empty;
    
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    
    public string? BatchNumber { get; set; }
    
    public string? Notes { get; set; }
    
    public string User { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    public DateTime? ModifiedDate { get; set; }
    
    public string ItemType { get; set; } = string.Empty;
}