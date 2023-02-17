using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Inventory.Data;

namespace Inventory.Pages.PurchaseOrders
{
    public class VendorNamePageModel : PageModel
    {
        public SelectList VendorNameSL { get; set; }
        public SelectList ItemsSL { get; set; }

        public void PopulateVendorsDropDownList(ApplicationDbContext _context,
            object selectedVendor = null)
        {
            var vendorsQuery = from d in _context.Vendor
                                   orderby d.Name // Sort by name.
                                   select d;

            VendorNameSL = new SelectList(vendorsQuery.AsNoTracking(),
                        "VendorID", "Name", selectedVendor);
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