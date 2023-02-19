// Edit Purchase Order Page Controller

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using Inventory.Data;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Pages.PurchaseOrders
{
    [Authorize]
    public class EditModel : VendorNamePageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PurchaseOrder PurchaseOrder { get; set; } = default!;
        [BindProperty]
        public IList<PurchaseItems> purchaseItems { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PurchaseOrder == null)
            {
                return NotFound();
            }

            var purchaseorder =  await _context.PurchaseOrder.FirstOrDefaultAsync(m => m.PurchaseID == id);
            var items = await _context.PurchaseItems
                .Where(i => i.PurchaseID == id.Value)
                .Include(i => i.Item).ToListAsync();
            if (purchaseorder == null)
            {
                return NotFound();
            }
            PurchaseOrder = purchaseorder;
            purchaseItems = items;
            PopulateVendorsDropDownList(_context, PurchaseOrder.VendorID);
            PopulateItemsDropDownList(_context);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PurchaseOrder).State = EntityState.Modified;

            // Update items one by one
            var purchasesToUpdate = await _context.PurchaseItems
                .Include(i => i.Item)
                .Where(i => i.PurchaseID == id).ToListAsync();
            var itemsToUpdate = await _context.Item.ToListAsync();
            for (int i = 0; i < purchaseItems.Count; i++)
            {
                purchasesToUpdate[i].ItemID = purchaseItems[i].ItemID;
                purchasesToUpdate[i].Quantity = purchaseItems[i].Quantity;
                _context.Attach(purchasesToUpdate[i]).State = EntityState.Modified;

                // Once completed, each item is added to the stock
                if (PurchaseOrder.Status == Status.Completed)
                {
                    foreach (var item in itemsToUpdate)
                    {
                        if (purchaseItems[i].ItemID == item.ItemID)
                        {
                            item.Quantity += purchaseItems[i].Quantity;
                            _context.Attach(item).State = EntityState.Modified;
                        }
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderExists(PurchaseOrder.PurchaseID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PurchaseOrderExists(int id)
        {
          return (_context.PurchaseOrder?.Any(e => e.PurchaseID == id)).GetValueOrDefault();
        }
    }
}