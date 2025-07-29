using CloudCare.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.API.DbContexts
{
    public class FinanceContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<User> Users { get; set; }

        public FinanceContext(DbContextOptions<FinanceContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // No seeding here!
            base.OnModelCreating(modelBuilder);
        }
    }
}