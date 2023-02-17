using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventory.Models;
using Inventory.Data;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Pages.SaleOrders
{
    [Authorize]
    public class CreateModel : CustomerNamePageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int itemsCount)
        {
        PopulateCustomersDropDownList(_context);
        PopulateItemsDropDownList(_context);
        count = itemsCount + 1;
        saleItems = new List<SaleItems>();
        for (int i = 0; i < count; i++)
        {
            saleItems.Add(new SaleItems());
        }
        return Page();
        }

        [BindProperty]
        public SaleOrder SaleOrder { get; set; } = default!;
        [BindProperty]
        public ICollection<SaleItems> saleItems { get; set; } = default!;
        [BindProperty]
        public  int count { get; set; } = default;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int [] itemsID, uint[] itemsQuantity)
        {
          if (!ModelState.IsValid || _context.SaleOrder == null || SaleOrder == null)
            {
                PopulateCustomersDropDownList(_context, SaleOrder.CustomerID);
                PopulateItemsDropDownList(_context);
                count = itemsID.Length;
                saleItems = new List<SaleItems>();
                for (int i = 0; i < count; i++)
                {
                    saleItems.Add(new SaleItems());
                }
                return Page();
            }

            SaleOrder.Status = Status.Created;
            _context.SaleOrder.Add(SaleOrder);
            await _context.SaveChangesAsync();

            for (int i = 0; i < itemsID.Length; i++)
            {
                var foundItem = await _context.Item.FindAsync(itemsID[i]);
                if (foundItem != null)
                {
                    var newItem = new SaleItems();
                    newItem.SaleID = SaleOrder.SaleID;
                    newItem.ItemID = itemsID[i];
                    newItem.Quantity = itemsQuantity[i];
                    _context.SaleItems.Add(newItem);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}