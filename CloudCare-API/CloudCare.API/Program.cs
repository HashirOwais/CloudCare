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




//to get the connection string. It will first look at the env varibles if not found any then it will get it from the appsetting.json
var connectionString =
    builder.Configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

builder.Services.AddDbContext<FinanceContext>(options =>
    options.UseNpgsql(connectionString));

Console.WriteLine(connectionString);

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
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

//TODO: ADD Cors for Front end for Prod


var app = builder.Build();

Console.WriteLine("isDev? "+app.Environment.IsDevelopment());
Console.WriteLine("isProd? "  +app.Environment.IsProduction());


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

app.Run();


