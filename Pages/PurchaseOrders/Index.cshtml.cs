// Purchase Orders Index Page Controller

using System;
using System.IO;
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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PurchaseOrder> PurchaseOrder { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.PurchaseOrder != null)
            {
                PurchaseOrder = await _context.PurchaseOrder
                .Where(p => p.Status == Status.Created)
                .Include(p => p.Vendor).ToListAsync();
            }

            // To avoid cluttering with pdf files, erase the existing ones
            foreach (string file in Directory.EnumerateFiles("wwwroot", "*.pdf"))
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.Exists) fileInfo.Delete();
            }
        }
    }
}