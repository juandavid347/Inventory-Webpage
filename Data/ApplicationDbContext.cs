// Application Database Context Class modified to include many to many relationships

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Inventory.Models.Item> Item { get; set; } = default!;
    public DbSet<Inventory.Models.Customer> Customer { get; set; } = default!;
    public DbSet<Inventory.Models.Vendor> Vendor { get; set; } = default!;
    public DbSet<Inventory.Models.SaleOrder> SaleOrder { get; set; } = default!;
    // SaleItems Database Set added for many to many relationship between SaleOrder and Item
    public DbSet<Inventory.Models.SaleItems> SaleItems { get; set; } = default!;
    public DbSet<Inventory.Models.CompanyInfo> CompanyInfo { get; set; } = default!;
    public DbSet<Inventory.Models.PurchaseOrder> PurchaseOrder { get; set; } = default!;
    // PurchaseItems Database Set added for many to many relationship between PurchaseOrder and Item
    public DbSet<Inventory.Models.PurchaseItems> PurchaseItems { get; set; } = default!;
}
