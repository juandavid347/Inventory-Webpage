using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class SaleOrder
    {
        [Key]
        public int SaleID { get; set; }
        public int CustomerID { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }

        public Customer? Customer { get; set; }
        public ICollection<SaleItems>? SaleItems { get; set; }
    }
}