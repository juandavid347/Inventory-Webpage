// Edit Sale Order Page Controller

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

namespace Inventory.Pages.SaleOrders
{
    [Authorize]
    public class EditModel : CustomerNamePageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SaleOrder SaleOrder { get; set; } = default!;
        [BindProperty]
        public IList<SaleItems> saleItems { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SaleOrder == null)
            {
                return NotFound();
            }

            var saleorder =  await _context.SaleOrder.FirstOrDefaultAsync(m => m.SaleID == id);
            var items = await _context.SaleItems
                .Where(i => i.SaleID == id.Value)
                .Include(i => i.Item).ToListAsync();
            if (saleorder == null)
            {
                return NotFound();
            }

            SaleOrder = saleorder;
            saleItems = items;
            PopulateCustomersDropDownList(_context, SaleOrder.CustomerID);
            PopulateItemsDropDownList(_context);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                PopulateCustomersDropDownList(_context, SaleOrder.CustomerID);
                PopulateItemsDropDownList(_context);
                return Page();
            }
            
            _context.Attach(SaleOrder).State = EntityState.Modified;

            // Update items one by one
            var salesToUpdate = await _context.SaleItems
                .Include(i => i.Item)
                .Where(i => i.SaleID == id).ToListAsync();
            var itemsToUpdate = await _context.Item.ToListAsync();
            for (int i = 0; i < saleItems.Count; i++)
            {
                salesToUpdate[i].ItemID = saleItems[i].ItemID;
                salesToUpdate[i].Quantity = saleItems[i].Quantity;
                _context.Attach(salesToUpdate[i]).State = EntityState.Modified;

                // Once completed, each item is added to the stock
                if (SaleOrder.Status == Status.Completed)
                {
                    foreach (var item in itemsToUpdate)
                    {
                        if (saleItems[i].ItemID == item.ItemID)
                        {

                            // Check if there's enough items available
                            if (item.Quantity < saleItems[i].Quantity)
                            {
                                PopulateCustomersDropDownList(_context, SaleOrder.CustomerID);
                                PopulateItemsDropDownList(_context);
                                return Page();
                            }
                            item.Quantity -= saleItems[i].Quantity;
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
                if (!SaleOrderExists(SaleOrder.SaleID))
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

        private bool SaleOrderExists(int id)
        {
          return (_context.SaleOrder?.Any(e => e.SaleID == id)).GetValueOrDefault();
        }
    }
}