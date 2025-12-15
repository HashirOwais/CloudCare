using CloudCare.Data.DbContexts;
using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.EFCore;
using CloudCare.Business.Repositories.Interfaces;
using CloudCare.Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

// Environment Variables
var authority = Environment.GetEnvironmentVariable("AUTH0_AUTHORITY") ?? throw new InvalidOperationException("Missing environment variable AUTH0_AUTHORITY");
var audience = Environment.GetEnvironmentVariable("AUTH0_AUDIENCE") ?? throw new InvalidOperationException("Missing environment variable AUTH0_AUDIENCE");
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? throw new InvalidOperationException("Missing environment variable CONNECTION_STRING");
var otelEndpoint = Environment.GetEnvironmentVariable("OTEL_ENDPOINT") ?? throw new InvalidOperationException("Missing environment variable OTEL_ENDPOINT for Production");

var builder = WebApplication.CreateBuilder(args);

// 1. Add Authentication Services
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = authority;
        options.Audience = audience;
    });


// #2 Logging and metrics
var serviceName = "Cloudcare-API";

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
                oltptpOptions.Endpoint = new Uri(otelEndpoint);
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
                oltptpOptions.Endpoint = new Uri(otelEndpoint);
            })
        )
        .WithMetrics(metrics => metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            
            .AddOtlpExporter(oltptpOptions =>
            {
                oltptpOptions.Endpoint = new Uri(otelEndpoint);
            }));
}

Console.WriteLine("Raw env: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
Console.WriteLine("Raw STRING: " + Environment.GetEnvironmentVariable("CONNECTION_STRING"));
Console.WriteLine("OTEL-ENDPOINT: " + Environment.GetEnvironmentVariable("OTEL_ENDPOINT"));

Console.WriteLine("isDev? " + builder.Environment.IsDevelopment());
Console.WriteLine("isProd? " + builder.Environment.IsProduction());
Console.WriteLine("isProd? " + builder.Environment.IsStaging());

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
builder.Services.AddScoped<CloudCare.Business.Services.IUserService, CloudCare.Business.Services.UserService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<CloudCare.API.Services.IUserService, CloudCare.API.Services.UserService>();

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
    options.AddPolicy("prodFrontendPolicy-netlifyDomain",
        policy =>
        {

            policy.WithOrigins("https://cloudcareweb.netlify.app")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    options.AddPolicy("prodFrontendPolicy",
        policy =>
        {
            policy.WithOrigins("https://cloudcare.hashirowais.com")
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
// app.UseHttpsRedirection();

// 3. Routing (defines endpoint pipeline)
app.UseRouting();

// 4. CORS 
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevFrontendPolicy");


}
else
{
    app.UseCors("prodFrontendPolicy");
    app.UseCors("prodFrontendPolicy-netlifyDomain");
}
// 5. Authentication and Authorization
app.UseAuthentication();   // Validates the JWT
app.UseAuthorization();    // Applies [Authorize] policies

// 6. Endpoint Mapping (must be last)
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<CloudCareContext>();
        dbContext.Database.Migrate();

        // Seed Users
        if (!dbContext.Users.Any())
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

            dbContext.Users.AddRange(user1, user2);
            dbContext.SaveChanges();

            // Seed Expenses for Alice (user1)
            dbContext.Expenses.AddRange(
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
            dbContext.Expenses.AddRange(
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

            dbContext.SaveChanges();
        }
    }
}

app.Run();