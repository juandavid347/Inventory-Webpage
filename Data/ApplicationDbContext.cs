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
}
