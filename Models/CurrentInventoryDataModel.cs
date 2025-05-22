using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Models;

internal class CurrentInventory
{
    public int Id { get; set; } // Maps to ID Primary, int(100), NOT NULL, AUTO_INCREMENT
    public string Location { get; set; } = string.Empty; // Maps to Location, varchar(100), NOT NULL
    public string ItemNumber { get; set; } = string.Empty; // Maps to Item Number, varchar(300), NOT NULL
    public string Op { get; set; } = string.Empty; // Maps to Op, varchar(100), NOT NULL
    public string Notes { get; set; } = string.Empty; // Maps to Notes, varchar(1000), NOT NULL
    public int Quantity { get; set; } // Maps to Quantity, int(100), NOT NULL

    public DateTime DateTime { get; set; } =
        DateTime.Now; // Maps to Date_Time, datetime, NOT NULL, default CURRENT_TIMESTAMP

    public string User { get; set; } = string.Empty; // Maps to User, varchar(100), NOT NULL
    public string ItemType { get; set; } = "WIP"; // Maps to Item Type, varchar(200), NOT NULL, default WIP
}

public class Last10
{
    public int Id { get; set; } // Maps to ID Primary, int(11), NOT NULL, AUTO_INCREMENT
    public string PartId { get; set; } = string.Empty; // Maps to PartID, varchar(1000), NOT NULL
    public string Op { get; set; } = string.Empty; // Maps to Op, text, NOT NULL
    public int Quantity { get; set; } // Maps to Quantity, int(10), NOT NULL
}