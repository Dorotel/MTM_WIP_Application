using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Models;

internal class History_Inventory
{
    public int Id { get; set; }
    public string? Location { get; set; }
    public string? PartId { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public string? User { get; set; }
}