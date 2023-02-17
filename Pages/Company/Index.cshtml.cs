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

namespace Inventory.Pages.Company
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly Inventory.Data.ApplicationDbContext _context;

        public IndexModel(Inventory.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CompanyInfo> CompanyInfo { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.CompanyInfo != null)
            {
                CompanyInfo = await _context.CompanyInfo.ToListAsync();
            }
        }
    }
}
