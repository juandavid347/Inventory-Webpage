// Vendors Index Page Controller

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Pages.Vendors
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly Inventory.Data.ApplicationDbContext _context;

        public IndexModel(Inventory.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Vendor> Vendor { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Vendor != null)
            {
                Vendor = await _context.Vendor.ToListAsync();
            }
        }
    }
}
