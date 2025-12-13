using CloudCare.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.Data.DbContexts
{
    public class CloudCareContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<User> Users { get; set; }

        public CloudCareContext(DbContextOptions<CloudCareContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Food & Snacks" },
                new Category { Id = 2, Name = "Educational Supplies" },
                new Category { Id = 3, Name = "Toys & Games" },
                new Category { Id = 4, Name = "Cleaning Supplies" },
                new Category { Id = 5, Name = "Utilities" },
                new Category { Id = 6, Name = "Office Supplies" },
                new Category { Id = 7, Name = "Furniture & Fixtures" },
                new Category { Id = 8, Name = "Repairs & Maintenance" },
                new Category { Id = 9, Name = "Transportation" },
                new Category { Id = 10, Name = "Insurance" },
                new Category { Id = 11, Name = "Professional Services" },
                new Category { Id = 12, Name = "Marketing & Advertising" },
                new Category { Id = 13, Name = "Staff Wages" },
                new Category { Id = 14, Name = "Training & Development" },
                new Category { Id = 15, Name = "Licenses & Permits" },
                new Category { Id = 99, Name = "Miscellaneous" }
            );

            // Vendors
            modelBuilder.Entity<Vendor>().HasData(
                new Vendor { Id = 1, Name = "Walmart" },
                new Vendor { Id = 2, Name = "Amazon" },
                new Vendor { Id = 3, Name = "Costco" },
                new Vendor { Id = 4, Name = "Staples" },
                new Vendor { Id = 5, Name = "Home Depot" },
                new Vendor { Id = 6, Name = "Best Buy" },
                new Vendor { Id = 7, Name = "Private Marketplace" },
                new Vendor { Id = 8, Name = "Local Vendor" },
                new Vendor { Id = 9, Name = "Government" },
                new Vendor { Id = 10, Name = "Real Canadian Superstore" },
                new Vendor { Id = 11, Name = "FreshCo" },
                new Vendor { Id = 12, Name = "No frills" },
                new Vendor { Id = 13, Name = "DollarStore" },
                new Vendor { Id = 14, Name = "GiantTiger" },
                new Vendor { Id = 15, Name = "Value Village" },
                new Vendor { Id = 99, Name = "Miscellaneous" }
            );

            // Payment Methods
            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { Id = 1, Name = "Credit Card" },
                new PaymentMethod { Id = 2, Name = "Debit Card" },
                new PaymentMethod { Id = 3, Name = "Cash" },
                new PaymentMethod { Id = 4, Name = "E-Transfer" },
                new PaymentMethod { Id = 5, Name = "Cheque" }
            );
        }
    }
}
