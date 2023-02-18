// Class that represents the customer's information

using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string? Name { get; set; }
        public string? Company { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public ICollection<SaleOrder>? SaleOrders { get; set; }
    }
}