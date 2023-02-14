using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;

namespace Inventory.Pages.Items
{
    public class DeleteModel : PageModel
    {
        private readonly Inventory.Data.ApplicationDbContext _context;

        public DeleteModel(Inventory.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Item Item { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FirstOrDefaultAsync(m => m.ItemID == id);

            if (item == null)
            {
                return NotFound();
            }
            else 
            {
                Item = item;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }
            var item = await _context.Item.FindAsync(id);

            if (item != null)
            {
                Item = item;
                _context.Item.Remove(Item);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
