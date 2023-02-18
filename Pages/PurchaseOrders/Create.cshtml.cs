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

namespace Inventory.Pages.PurchaseOrders
{
    [Authorize]
    public class CreateModel : VendorNamePageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int itemsCount)
        {
        PopulateVendorsDropDownList(_context);
        PopulateItemsDropDownList(_context);
        count = itemsCount + 1;
        purchaseItems = new List<PurchaseItems>();
        for (int i = 0; i < count; i++)
        {
            purchaseItems.Add(new PurchaseItems());
        }
        return Page();
        }

        [BindProperty]
        public PurchaseOrder PurchaseOrder { get; set; } = default!;
        [BindProperty]
        public ICollection<PurchaseItems> purchaseItems { get; set; } = default!;
        [BindProperty]
        public  int count { get; set; } = default;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int [] itemsID, uint[] itemsQuantity)
        {
          if (!ModelState.IsValid || _context.PurchaseOrder == null || PurchaseOrder == null)
            {
                if (PurchaseOrder != null) PopulateVendorsDropDownList(_context, PurchaseOrder.VendorID);
                PopulateItemsDropDownList(_context);
                count = itemsID.Length;
                purchaseItems = new List<PurchaseItems>();
                for (int i = 0; i < count; i++)
                {
                    purchaseItems.Add(new PurchaseItems());
                }
                return Page();
            }

            _context.PurchaseOrder.Add(PurchaseOrder);
            await _context.SaveChangesAsync();

            for (int i = 0; i < itemsID.Length; i++)
            {
                var foundItem = await _context.Item.FindAsync(itemsID[i]);
                if (foundItem != null)
                {
                    var newItem = new PurchaseItems();
                    newItem.PurchaseID = PurchaseOrder.PurchaseID;
                    newItem.ItemID = itemsID[i];
                    newItem.Quantity = itemsQuantity[i];
                    _context.PurchaseItems.Add(newItem);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}