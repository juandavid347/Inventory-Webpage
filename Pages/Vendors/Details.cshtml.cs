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
    public class DetailsModel : PageModel
    {
        private readonly Inventory.Data.ApplicationDbContext _context;

        public DetailsModel(Inventory.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Vendor Vendor { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Vendor == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendor.FirstOrDefaultAsync(m => m.VendorID == id);
            if (vendor == null)
            {
                return NotFound();
            }
            else 
            {
                Vendor = vendor;
            }
            return Page();
        }
    }
}
