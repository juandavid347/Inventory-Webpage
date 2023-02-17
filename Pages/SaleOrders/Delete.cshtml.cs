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

namespace Inventory.Pages.SaleOrders
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
      public SaleOrder SaleOrder { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SaleOrder == null)
            {
                return NotFound();
            }

            var saleorder = await _context.SaleOrder
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(m => m.SaleID == id);

            if (saleorder == null)
            {
                return NotFound();
            }
            else 
            {
                SaleOrder = saleorder;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.SaleOrder == null)
            {
                return NotFound();
            }
            var saleorder = await _context.SaleOrder.FindAsync(id);

            if (saleorder != null)
            {
                SaleOrder = saleorder;
                _context.SaleOrder.Remove(SaleOrder);
                await _context.SaveChangesAsync();
            }

            var saleItems = await _context.SaleItems
                .Where(s => s.SaleID == id)
                .ToListAsync();
            
            if (saleItems != null)
            {
                foreach (var item in saleItems)
                {
                    _context.SaleItems.Remove(item);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}