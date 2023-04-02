using Microsoft.EntityFrameworkCore;
using VeendingMachine.API.DataAccess.Entities;

namespace VeendingMachine.API.DataAccess.DatabaseContext
{
    public class VendorContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<DepositStack> DepositStacks { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<MoneyUnit> MoneyUnits { get; set; }

        public VendorContext(DbContextOptions<VendorContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
