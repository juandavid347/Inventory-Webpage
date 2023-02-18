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

namespace Inventory.Pages.SaleOrders
{
    [Authorize]
    public class PrintModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PrintModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public SaleOrder SaleOrder { get; set; } = default!;
      public IEnumerable<SaleItems> saleItems { get; set; } = default!;
      public CompanyInfo CompanyInfo { get; set; } = default!;
      public string filename { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SaleOrder == null)
            {
                return NotFound();
            }

            var saleorder = await _context.SaleOrder
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(m => m.SaleID == id);
            var items = await _context.SaleItems
                .Where(i => i.SaleID == id.Value)
                .Include(i => i.Item).ToListAsync();
            var companyInfo = await _context.CompanyInfo.FirstOrDefaultAsync(s => s.ID == 1);
            if (saleorder == null || companyInfo == null)
            {
                return NotFound();
            }
            else 
            {
                SaleOrder = saleorder;
                saleItems = items;
                CompanyInfo = companyInfo;
            }

            filename = SalesOrderPdf.CreateSalesOrderPdf(SaleOrder, saleItems, CompanyInfo);

            return Page();
        }
    }
}