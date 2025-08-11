

namespace MTM_Inventory_Application.Models
{
    internal class Model_Transactions
    {
        public int ID { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? BatchNumber { get; set; }
        public string? PartID { get; set; }
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public string? Operation { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public string? User { get; set; }
        public string? ItemType { get; set; }
        public DateTime DateTime { get; set; }
    }

    internal enum TransactionType
    {
        IN,
        OUT,
        TRANSFER
    }
}