using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Vendor
    {
        public int VendorID { get; set; }
        public string? Name { get; set; }
        public string? Company { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}