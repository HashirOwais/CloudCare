using CloudCare.API.DbContexts;
using CloudCare.API.Models;
using CloudCare.API.Repositories.EFCore;
using CloudCare.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//AUTH0 STUFF

// 1. Add Authentication Services
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = "https://dev-er3g7sg6jb76sxpw.us.auth0.com/";
        options.Audience = "https://api.cloudcare.hashirowais.com";
    });


//PLS EXPORT the two ENV VARS
//CONNECTION_STRING and ASPNETCORE_ENVIRONMENT=Production
//export CONNECTION_STRING='Server=192.168.69.200:5432;Database=Cloudcare_UAT;Username=CloudCare;Password=dw';
//export ASPNETCORE_ENVIRONMENT=Production
//unset envvar name



// //to get the connection string. It will first look at the env varibles if not found any then it will get it from the appsetting.json
// var connectionString =
//     builder.Configuration.GetConnectionString("Default")
//         ?? throw new InvalidOperationException("Connection string"
//         + "'DefaultConnection' not found.");

//getting connection string VIA ENV vars 

//using two diff dbs, sqllite for dev and then using ngpsql for prod

Console.WriteLine("Raw env: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
Console.WriteLine("Raw STRING: " + Environment.GetEnvironmentVariable("CONNECTION_STRING"));
Console.WriteLine("isDev? " + builder.Environment.IsDevelopment());
Console.WriteLine("isProd? " + builder.Environment.IsProduction());


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<FinanceContext>(options => options.UseSqlite("Data Source=Finance.db"));
}
else
{
    
    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                           ?? throw new InvalidOperationException("Missing environment variable CONNECTION_STRING");
    
    builder.Services.AddDbContext<FinanceContext>(options =>
        options.UseNpgsql(connectionString));

    
    
}



builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



#region Testing registering Services

//builder.Services.AddSingleton<IExpenseRepository, MockExpenseRepository>();
// builder.Services.AddSingleton<ICategoryRepository, MockCategoryRepository>();
// builder.Services.AddSingleton<IPaymentMethodRepository, MockPaymentMethodRepository>();
// builder.Services.AddSingleton<IVendorRepository, MockVendorRepository>();

#endregion

//for DB presistence, WE do addscoped becuasse each API call it creates a new instance. 
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();



//Auto mapper

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//cors FOR DEV

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevFrontendPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    
});

//TODO: ADD Cors for Front end for Prod


var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // 1. Dev error page (detailed)
    app.UseSwagger();                // 2. Swagger docs
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp => // 1. Production error handler (JSON)
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            if (error != null)
            {
                var err = System.Text.Json.JsonSerializer.Serialize(new
                {
                    error = "An unexpected error occurred.",
                    detail = error.Error.Message // Or omit this in prod!
                });
                await context.Response.WriteAsync(err);
            }
        });
    });
}

// 2. HTTPS redirection (always before routing)
app.UseHttpsRedirection();

// 3. Routing (defines endpoint pipeline)
app.UseRouting();

// 4. CORS 
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevFrontendPolicy");

    
}
else
{
    //TODO ADD CORS FOR PROD
}



// 5. Authentication and Authorization
app.UseAuthentication();   // Validates the JWT
app.UseAuthorization();    // Applies [Authorize] policies

// 6. Endpoint Mapping (must be last)
app.MapControllers();

#region seeding dev database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinanceContext>();
    // Only run for SQLite dev DB
    if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
    {
        context.Database.EnsureCreated();

        // Categories
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
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
        }

        // Vendors
        if (!context.Vendors.Any())
        {
            context.Vendors.AddRange(
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
        }

        // PaymentMethods
        if (!context.PaymentMethods.Any())
        {
            context.PaymentMethods.AddRange(
                new PaymentMethod { Id = 1, Name = "Credit Card" },
                new PaymentMethod { Id = 2, Name = "Debit Card" },
                new PaymentMethod { Id = 3, Name = "Cash" },
                new PaymentMethod { Id = 4, Name = "E-Transfer" },
                new PaymentMethod { Id = 5, Name = "Cheque" }
            );
        }

        // Users
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    Id = 1,
                    Auth0Id = "auth0|user1abc",
                    Email = "provider1@daycare.com",
                    Name = "Alice Johnson",
                    DaycareName = "Happy Kids Daycare",
                    DaycareAddress = "123 Main St, Cityville",
                    PhoneNumber = "555-1234",
                    WebsiteUrl = "https://happykidsdaycare.com",
                    Notes = "Open weekdays 7am-6pm",
                    UserCreated = new DateTime(2024, 6, 1, 8, 0, 0, DateTimeKind.Utc),
                    Role = "provider",
                    IsRegistered = false
                },
                new User
                {
                    Id = 2,
                    Auth0Id = "auth0|user2xyz",
                    Email = "provider2@daycare.com",
                    Name = "Bob Smith",
                    DaycareName = "Little Stars Childcare",
                    DaycareAddress = "456 Oak Ave, Townsville",
                    PhoneNumber = "555-5678",
                    WebsiteUrl = "https://littlestarschildcare.com",
                    Notes = "Accepts infants and toddlers",
                    UserCreated = new DateTime(2024, 6, 2, 9, 0, 0, DateTimeKind.Utc),
                    Role = "provider",
                    IsRegistered = false
                },
                new User
                {
                    Id = 3,
                    Auth0Id = "auth0|user3qwe",
                    Email = "provider3@daycare.com",
                    Name = "Carol Lee",
                    DaycareName = "Bright Minds Preschool",
                    DaycareAddress = "789 Pine Rd, Villagetown",
                    PhoneNumber = "555-9012",
                    WebsiteUrl = "https://brightmindspreschool.com",
                    Notes = "Focus on early learning",
                    UserCreated = new DateTime(2024, 6, 3, 10, 0, 0, DateTimeKind.Utc),
                    Role = "provider",
                    IsRegistered = true
                }
            );
        }

        // Expenses
        if (!context.Expenses.Any())
        {
            context.Expenses.AddRange(
                new Expense
                {
                    Id = 1, UserId = 1, Description = "Snacks for kids", Amount = 20.00m,
                    Date = new DateTime(2024, 7, 4, 0, 0, 0, DateTimeKind.Utc), CategoryId = 1, VendorId = 1,
                    PaymentMethodId = 1, IsRecurring = false
                },
                new Expense
                {
                    Id = 2, UserId = 1, Description = "Toys purchase", Amount = 35.00m,
                    Date = new DateTime(2024, 7, 2, 0, 0, 0, DateTimeKind.Utc), CategoryId = 3, VendorId = 2,
                    PaymentMethodId = 2, IsRecurring = true
                },
                new Expense
                {
                    Id = 3, UserId = 1, Description = "Field Trip Supplies", Amount = 50.00m,
                    Date = new DateTime(2024, 6, 29, 0, 0, 0, DateTimeKind.Utc), CategoryId = 2, VendorId = 3,
                    PaymentMethodId = 1, IsRecurring = false
                },
                new Expense
                {
                    Id = 4, UserId = 1, Description = "Office Supplies restock", Amount = 22.50m,
                    Date = new DateTime(2024, 6, 28, 0, 0, 0, DateTimeKind.Utc), CategoryId = 6, VendorId = 1,
                    PaymentMethodId = 1, IsRecurring = false
                },
                new Expense
                {
                    Id = 5, UserId = 1, Description = "Books for reading time", Amount = 80.00m,
                    Date = new DateTime(2024, 6, 27, 0, 0, 0, DateTimeKind.Utc), CategoryId = 2, VendorId = 2,
                    PaymentMethodId = 2, IsRecurring = false
                },
                new Expense
                {
                    Id = 6, UserId = 1, Description = "Monthly Cleaning Service", Amount = 120.00m,
                    Date = new DateTime(2024, 6, 24, 0, 0, 0, DateTimeKind.Utc), CategoryId = 4, VendorId = 3,
                    PaymentMethodId = 3, IsRecurring = true
                },
                new Expense
                {
                    Id = 7, UserId = 1, Description = "Birthday Party expenses", Amount = 150.00m,
                    Date = new DateTime(2024, 6, 19, 0, 0, 0, DateTimeKind.Utc), CategoryId = 1, VendorId = 1,
                    PaymentMethodId = 1, IsRecurring = false
                },
                new Expense
                {
                    Id = 8, UserId = 1, Description = "Printer Ink replacement", Amount = 60.00m,
                    Date = new DateTime(2024, 6, 14, 0, 0, 0, DateTimeKind.Utc), CategoryId = 6, VendorId = 2,
                    PaymentMethodId = 2, IsRecurring = false
                },
                new Expense
                {
                    Id = 9, UserId = 1, Description = "Online Workshop for Staff", Amount = 95.00m,
                    Date = new DateTime(2024, 6, 12, 0, 0, 0, DateTimeKind.Utc), CategoryId = 14, VendorId = 3,
                    PaymentMethodId = 1, IsRecurring = false
                },
                new Expense
                {
                    Id = 10, UserId = 1, Description = "Weekly Snacks", Amount = 25.00m,
                    Date = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc), CategoryId = 1, VendorId = 1,
                    PaymentMethodId = 3, IsRecurring = true
                },
                new Expense
                {
                    Id = 11, UserId = 1, Description = "Monthly Software Subscription", Amount = 45.99m,
                    Date = new DateTime(2024, 6, 5, 0, 0, 0, DateTimeKind.Utc), CategoryId = 12, VendorId = 2,
                    PaymentMethodId = 2, IsRecurring = true
                },

                // User 2
                new Expense
                {
                    Id = 12, UserId = 2, Description = "Art Supplies", Amount = 15.00m,
                    Date = new DateTime(2024, 7, 3, 0, 0, 0, DateTimeKind.Utc), CategoryId = 2, VendorId = 1,
                    PaymentMethodId = 1, IsRecurring = false
                },
                new Expense
                {
                    Id = 13, UserId = 2, Description = "Lunch for Staff", Amount = 45.00m,
                    Date = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc), CategoryId = 1, VendorId = 2,
                    PaymentMethodId = 3, IsRecurring = false
                },
                new Expense
                {
                    Id = 14, UserId = 2, Description = "Monthly Cleaning Service", Amount = 120.00m,
                    Date = new DateTime(2024, 6, 4, 0, 0, 0, DateTimeKind.Utc), CategoryId = 4, VendorId = 3,
                    PaymentMethodId = 2, IsRecurring = true
                }
            );
        }

        context.SaveChanges();
    }
}



#endregion


app.Run();


// docker build -t hashirowais/cloudcare-api:latest .  
// docker run -d --name cloudcare-api -p 5001:5000 -e CONNECTION_STRING="Server=192.168.69.200;Port=5432;Database=Cloudcare_UAT;Username=CloudCare;Password=dw;" \ hashirowais/cloudcare-api:latest