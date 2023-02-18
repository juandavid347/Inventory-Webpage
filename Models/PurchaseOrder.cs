// Class that represents each purchase order and associated vendor and items

using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int PurchaseID { get; set; }
        public int VendorID { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }

        public Vendor? Vendor { get; set; }
        public ICollection<PurchaseItems>? PurchaseItems { get; set; }
    }

    public enum Status
    {
        Created, Completed
    }
}