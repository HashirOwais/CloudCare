using CloudCare.API.DbContexts;
using CloudCare.API.Models;
using CloudCare.API.Repositories.EFCore;
using CloudCare.API.Repositories.Interfaces;
using CloudCare.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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
        options.Authority = "https://dev-hashir.ca.auth0.com/";
        options.Audience = "https://api.cloudcare.hashirowais.com";
    });


// #2 Logging and metrics 
var serviceName = "FinanceService";

if (builder.Environment.IsProduction())
{
    builder.Logging.AddOpenTelemetry(options =>
    {
        options
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName))
            .AddOtlpExporter(oltptpOptions =>
            {
                oltptpOptions.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL-ENDPOINT") ?? throw new InvalidOperationException("Missing environment variable OTEL-ENDPOINT"));
            }
            );
        options.IncludeFormattedMessage = true;
        options.IncludeScopes = true;
        options.ParseStateValues = true;
    });
    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(serviceName))
        .WithTracing(tracing => tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddNpgsql()
            .AddOtlpExporter(oltptpOptions =>
            {
                oltptpOptions.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL-ENDPOINT") ?? throw new InvalidOperationException("Missing environment variable OTEL-ENDPOINT"));
            })
        )
        .WithMetrics(metrics => metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(oltptpOptions =>
            {
                oltptpOptions.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL-ENDPOINT") ?? throw new InvalidOperationException("Missing environment variable OTEL-ENDPOINT"));
            }));
}

//PLS EXPORT the two ENV VARS
//CONNECTION_STRING and ASPNETCORE_ENVIRONMENT=Production
//export CONNECTION_STRING='Server=192.168.69.200:5432;Database=Cloudcare_UAT;Username=CloudCare;Password=dw;';


//for dev
//CONNECTION_STRING and ASPNETCORE_ENVIRONMENT=Production
//export CONNECTION_STRING='Server=192.168.69.200:5432;Database=Cloudcare_Dev;Username=hashir_dev;Password=dw;';

//export ASPNETCORE_ENVIRONMENT=Production
//unset envvar name

// //to get the connection string. It will first look at the env varibles if not found any then it will get it from the appsetting.json
// var connectionString =
//     builder.Configuration.GetConnectionString("Default")
//         ?? throw new InvalidOperationException("Connection string"
//         + "'DefaultConnection' not found.");

Console.WriteLine("Raw env: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
Console.WriteLine("Raw STRING: " + Environment.GetEnvironmentVariable("CONNECTION_STRING"));
Console.WriteLine("OTEL-ENDPOINT: " + Environment.GetEnvironmentVariable("OTEL-ENDPOINT"));

Console.WriteLine("isDev? " + builder.Environment.IsDevelopment());
Console.WriteLine("isProd? " + builder.Environment.IsProduction());
Console.WriteLine("isProd? " + builder.Environment.IsStaging());

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                       ?? throw new InvalidOperationException("Missing environment variable CONNECTION_STRING");

builder.Services.AddDbContext<CloudCareContext>(options => options.UseNpgsql(connectionString));

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

//Repositories
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

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
    var context = scope.ServiceProvider.GetRequiredService<CloudCareContext>();

    if (builder.Environment.IsDevelopment())
    {
        context.Database.EnsureCreated();

        // Seed Users
        if (!context.Users.Any())
        {
            var user1 = new User
            {
                Auth0Id = "auth0|user1abc",
                Email = "provider1@daycare.com",
                Name = "Alice Johnson",
                DaycareName = "Happy Kids Daycare",
                DaycareAddress = "123 Main St, Cityville",
                PhoneNumber = "555-1234",
                WebsiteUrl = "https://happykidsdaycare.com",
                Notes = "Open weekdays 7am-6pm",
                UserCreated = DateTime.UtcNow,
                Role = "provider",
                IsRegistered = false
            };

            var user2 = new User
            {
                Auth0Id = "auth0|user2xyz",
                Email = "provider2@daycare.com",
                Name = "Bob Smith",
                DaycareName = "Little Stars Childcare",
                DaycareAddress = "456 Oak Ave, Townsville",
                PhoneNumber = "555-5678",
                WebsiteUrl = "https://littlestarschildcare.com",
                Notes = "Accepts infants and toddlers",
                UserCreated = DateTime.UtcNow,
                Role = "provider",
                IsRegistered = false
            };

            context.Users.AddRange(user1, user2);
            context.SaveChanges();

            // Seed Expenses for Alice (user1)
            context.Expenses.AddRange(
                new Expense
                {
                    Amount = 150.75m,
                    CategoryId = 1, // Food & Snacks
                    VendorId = 1,   // Walmart
                    PaymentMethodId = 2, // Debit Card
                    Date = new DateOnly(2020, 10, 11),
                    Description = "Weekly snacks for kids",
                    Notes = "Purchased fruits, crackers, juice boxes",
                    UserId = user1.Id,
                    IsRecurring = false
                },
                new Expense
                {
                    Amount = 85.00m,
                    CategoryId = 4, // Cleaning Supplies
                    VendorId = 3,   // Costco
                    PaymentMethodId = 1, // Credit Card
                    Date = new DateOnly(2020, 10, 11),
                    Description = "Cleaning supplies bulk order",
                    Notes = "Disinfectant, wipes, soap",
                    UserId = user1.Id,
                    IsRecurring = false
                }
            );

            // Seed Expenses for Bob (user2)
            context.Expenses.AddRange(
                new Expense
                {
                    Amount = 1200.00m,
                    CategoryId = 7, // Furniture & Fixtures
                    VendorId = 4,   // Staples
                    PaymentMethodId = 4, // E-Transfer
                    Date = new DateOnly(2025, 9, 22),
                    Description = "New classroom tables and chairs",
                    Notes = "Setup for toddlers",
                    UserId = user2.Id,
                    IsRecurring = false
                },
                new Expense
                {
                    Amount = 300.50m,
                    CategoryId = 5, // Utilities
                    VendorId = 9,   // Government
                    PaymentMethodId = 5, // Cheque
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Description = "Monthly electricity + water bill",
                    Notes = "Paid via cheque",
                    UserId = user2.Id,
                    IsRecurring = true
                }
            );

            context.SaveChanges();
        }
    }
}
#endregion

app.Run();
