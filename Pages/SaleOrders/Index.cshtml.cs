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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SaleOrder> SaleOrder { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.SaleOrder != null)
            {
                SaleOrder = await _context.SaleOrder
                .Where(p => p.Status == Status.Created)
                .Include(s => s.Customer).ToListAsync();
            }

            foreach (string file in Directory.EnumerateFiles("wwwroot", "*.pdf"))
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.Exists) fileInfo.Delete();
            }
        }
    }
}