// See Purchase Order Details Page Controller

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using Inventory.Data;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Pages.PurchaseOrders
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public PurchaseOrder PurchaseOrder { get; set; } = default!;
      public IEnumerable<PurchaseItems> purchaseItems { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PurchaseOrder == null)
            {
                return NotFound();
            }

            var purchaseorder = await _context.PurchaseOrder
            .Include(s => s.Vendor)
            .FirstOrDefaultAsync(m => m.PurchaseID == id);
            var items = await _context.PurchaseItems
                .Where(i => i.PurchaseID == id.Value)
                .Include(i => i.Item).ToListAsync();
            if (purchaseorder == null)
            {
                return NotFound();
            }
            else 
            {
                PurchaseOrder = purchaseorder;
                purchaseItems = items;
            }
            return Page();
        }
    }
}