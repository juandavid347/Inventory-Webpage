// Print Purchase Order Page Controller

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using Inventory.PDF;
using Inventory.Data;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Pages.PurchaseOrders
{
    [Authorize]
    public class PrintModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PrintModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public PurchaseOrder PurchaseOrder { get; set; } = default!;
      public IEnumerable<PurchaseItems> purchaseItems { get; set; } = default!;
      public CompanyInfo CompanyInfo { get; set; } = default!;
      public string filename { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PurchaseOrder == null)
            {
                return NotFound();
            }

            var purchaseorder = await _context.PurchaseOrder
            .Include(s => s.Vendor)
            .FirstOrDefaultAsync(m => m.PurchaseID == id);
            var items = await _context.PurchaseItems
                .Where(i => i.PurchaseID == id.Value)
                .Include(i => i.Item).ToListAsync();
            var companyInfo = await _context.CompanyInfo.FirstOrDefaultAsync(s => s.ID == 1);
            if (purchaseorder == null || companyInfo == null)
            {
                return NotFound();
            }
            else 
            {
                PurchaseOrder = purchaseorder;
                purchaseItems = items;
                CompanyInfo = companyInfo;
            }

            // Each Purchase Order Pdf File requires your company information besides the purchase order details
            filename = PurchaseOrderPdf.CreatePurchaseOrderPdf(PurchaseOrder, purchaseItems, CompanyInfo);

            return Page();
        }
    }
}