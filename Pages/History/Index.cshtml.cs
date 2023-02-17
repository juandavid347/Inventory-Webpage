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

namespace Inventory.Pages.History
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SaleOrder> Sales { get;set; } = default!;
        public IList<PurchaseOrder> Purchases { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if ((_context.PurchaseOrder != null)&&(_context.SaleOrder != null))
            {
                Purchases = await _context.PurchaseOrder
                .Where(p => p.Status == Status.Completed)
                .Include(p => p.Vendor)
                .OrderBy(p => p.Date)
                .ToListAsync();
                Sales = await _context.SaleOrder
                .Where(p => p.Status == Status.Completed)
                .Include(p => p.Customer)
                .OrderBy(p => p.Date)
                .ToListAsync();
            }
        }
    }
}