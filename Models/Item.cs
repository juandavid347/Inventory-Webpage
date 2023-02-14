using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        [Display(Name = "Part Number")]
        public string? PartNumber { get; set; }
        [Display(Name = "Serial Number")]
        public string? SerialNumber { get; set; }
        public string? Units { get; set; }
        public decimal Price { get; set; }
        public uint Quantity { get; set; }

        public ICollection<SaleItems>? SaleItems { get; set; }
    }
}