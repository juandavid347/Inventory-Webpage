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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public PurchaseOrder PurchaseOrder { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PurchaseOrder == null)
            {
                return NotFound();
            }

            var purchaseorder = await _context.PurchaseOrder
            .Include(s => s.Vendor)
            .FirstOrDefaultAsync(m => m.PurchaseID == id);

            if (purchaseorder == null)
            {
                return NotFound();
            }
            else 
            {
                PurchaseOrder = purchaseorder;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.PurchaseOrder == null)
            {
                return NotFound();
            }
            var purchaseorder = await _context.PurchaseOrder.FindAsync(id);

            if (purchaseorder != null)
            {
                PurchaseOrder = purchaseorder;
                _context.PurchaseOrder.Remove(PurchaseOrder);
                await _context.SaveChangesAsync();
            }

            var purchaseItems = await _context.PurchaseItems
                .Where(s => s.PurchaseID == id)
                .ToListAsync();
            
            if (purchaseItems != null)
            {
                foreach (var item in purchaseItems)
                {
                    _context.PurchaseItems.Remove(item);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}