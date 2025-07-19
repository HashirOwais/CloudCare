using System;
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
            // Seed Categories
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

            // Seed Vendors
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
                new Vendor { Id = 99, Name = "Miscellaneous" }
            );

            // Seed Payment Methods
            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { Id = 1, Name = "Credit Card" },
                new PaymentMethod { Id = 2, Name = "Debit Card" },
                new PaymentMethod { Id = 3, Name = "Cash" },
                new PaymentMethod { Id = 4, Name = "E-Transfer" },
                new PaymentMethod { Id = 5, Name = "Cheque" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "provider1@daycare.com",
                    Name = "Alice Johnson",
                    DaycareName = "Happy Kids Daycare",
                    DaycareAddress = "123 Main St, Cityville",
                    PhoneNumber = "555-1234",
                    WebsiteUrl = "https://happykidsdaycare.com",
                    Notes = "Open weekdays 7am-6pm",
                    UserCreated = new DateTime(2024, 6, 1, 8, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Id = 2,
                    Email = "provider2@daycare.com",
                    Name = "Bob Smith",
                    DaycareName = "Little Stars Childcare",
                    DaycareAddress = "456 Oak Ave, Townsville",
                    PhoneNumber = "555-5678",
                    WebsiteUrl = "https://littlestarschildcare.com",
                    Notes = "Accepts infants and toddlers",
                    UserCreated = new DateTime(2024, 6, 2, 9, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Id = 3,
                    Email = "provider3@daycare.com",
                    Name = "Carol Lee",
                    DaycareName = "Bright Minds Preschool",
                    DaycareAddress = "789 Pine Rd, Villagetown",
                    PhoneNumber = "555-9012",
                    WebsiteUrl = "https://brightmindspreschool.com",
                    Notes = "Focus on early learning",
                    UserCreated = new DateTime(2024, 6, 3, 10, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed Expenses (all DateTimes are now UTC)
            modelBuilder.Entity<Expense>().HasData(
                new Expense { Id = 1, UserId = 1, Description = "Snacks for kids", Amount = 20.00m, Date = new DateTime(2024, 7, 4, 0, 0, 0, DateTimeKind.Utc), CategoryId = 1, VendorId = 1, PaymentMethodId = 1, IsRecurring = false },
                new Expense { Id = 2, UserId = 1, Description = "Toys purchase", Amount = 35.00m, Date = new DateTime(2024, 7, 2, 0, 0, 0, DateTimeKind.Utc), CategoryId = 3, VendorId = 2, PaymentMethodId = 2, IsRecurring = true },
                new Expense { Id = 3, UserId = 1, Description = "Field Trip Supplies", Amount = 50.00m, Date = new DateTime(2024, 6, 29, 0, 0, 0, DateTimeKind.Utc), CategoryId = 2, VendorId = 3, PaymentMethodId = 1, IsRecurring = false },
                new Expense { Id = 4, UserId = 1, Description = "Office Supplies restock", Amount = 22.50m, Date = new DateTime(2024, 6, 28, 0, 0, 0, DateTimeKind.Utc), CategoryId = 6, VendorId = 1, PaymentMethodId = 1, IsRecurring = false },
                new Expense { Id = 5, UserId = 1, Description = "Books for reading time", Amount = 80.00m, Date = new DateTime(2024, 6, 27, 0, 0, 0, DateTimeKind.Utc), CategoryId = 2, VendorId = 2, PaymentMethodId = 2, IsRecurring = false },
                new Expense { Id = 6, UserId = 1, Description = "Monthly Cleaning Service", Amount = 120.00m, Date = new DateTime(2024, 6, 24, 0, 0, 0, DateTimeKind.Utc), CategoryId = 4, VendorId = 3, PaymentMethodId = 3, IsRecurring = true },
                new Expense { Id = 7, UserId = 1, Description = "Birthday Party expenses", Amount = 150.00m, Date = new DateTime(2024, 6, 19, 0, 0, 0, DateTimeKind.Utc), CategoryId = 1, VendorId = 1, PaymentMethodId = 1, IsRecurring = false },
                new Expense { Id = 8, UserId = 1, Description = "Printer Ink replacement", Amount = 60.00m, Date = new DateTime(2024, 6, 14, 0, 0, 0, DateTimeKind.Utc), CategoryId = 6, VendorId = 2, PaymentMethodId = 2, IsRecurring = false },
                new Expense { Id = 9, UserId = 1, Description = "Online Workshop for Staff", Amount = 95.00m, Date = new DateTime(2024, 6, 12, 0, 0, 0, DateTimeKind.Utc), CategoryId = 14, VendorId = 3, PaymentMethodId = 1, IsRecurring = false },
                new Expense { Id = 10, UserId = 1, Description = "Weekly Snacks", Amount = 25.00m, Date = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc), CategoryId = 1, VendorId = 1, PaymentMethodId = 3, IsRecurring = true },
                new Expense { Id = 11, UserId = 1, Description = "Monthly Software Subscription", Amount = 45.99m, Date = new DateTime(2024, 6, 5, 0, 0, 0, DateTimeKind.Utc), CategoryId = 12, VendorId = 2, PaymentMethodId = 2, IsRecurring = true },

                // User 2
                new Expense { Id = 12, UserId = 2, Description = "Art Supplies", Amount = 15.00m, Date = new DateTime(2024, 7, 3, 0, 0, 0, DateTimeKind.Utc), CategoryId = 2, VendorId = 1, PaymentMethodId = 1, IsRecurring = false },
                new Expense { Id = 13, UserId = 2, Description = "Lunch for Staff", Amount = 45.00m, Date = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc), CategoryId = 1, VendorId = 2, PaymentMethodId = 3, IsRecurring = false },
                new Expense { Id = 14, UserId = 2, Description = "Monthly Cleaning Service", Amount = 120.00m, Date = new DateTime(2024, 6, 4, 0, 0, 0, DateTimeKind.Utc), CategoryId = 4, VendorId = 3, PaymentMethodId = 2, IsRecurring = true }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
    }