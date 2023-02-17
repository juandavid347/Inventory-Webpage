using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Inventory.Data;

namespace Inventory.Pages.SaleOrders
{
    public class CustomerNamePageModel : PageModel
    {
        public SelectList CustomerNameSL { get; set; }
        public SelectList ItemsSL { get; set; }

        public void PopulateCustomersDropDownList(ApplicationDbContext _context,
            object selectedCustomer = null)
        {
            var customersQuery = from d in _context.Customer
                                   orderby d.Name // Sort by name.
                                   select d;

            CustomerNameSL = new SelectList(customersQuery.AsNoTracking(),
                        "CustomerID", "Name", selectedCustomer);
        }

        public void PopulateItemsDropDownList(ApplicationDbContext _context,
            object selectedItem = null)
        {
            var itemsQuery = from d in _context.Item
                                   orderby d.Name // Sort by name.
                                   select d;

            ItemsSL = new SelectList(itemsQuery.AsNoTracking(),
                        "ItemID", "Name", selectedItem);
        }
    }
}