// Class that represents each sold item and their associated sales order information

using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class SaleItems
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int SaleID { get; set; }
        public uint Quantity { get; set; }

        public Item? Item { get; set; }
        public SaleOrder? SaleOrder { get; set; }
    }
}