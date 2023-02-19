// Create New Vendor Page Controller

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventory.Data;
using Inventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Pages.Vendors
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly Inventory.Data.ApplicationDbContext _context;

        public CreateModel(Inventory.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Vendor Vendor { get; set; } = default!;
        

        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Vendor == null || Vendor == null)
            {
                return Page();
            }

            _context.Vendor.Add(Vendor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
