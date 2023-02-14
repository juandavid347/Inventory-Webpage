using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class PurchaseItems
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int PurchaseID { get; set; }
        public uint Quantity { get; set; }

        public Item? Item { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
    }
}