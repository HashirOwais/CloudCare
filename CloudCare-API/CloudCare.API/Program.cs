using CloudCare.API.DbContexts;
using CloudCare.API.Models;
using CloudCare.API.Repositories.EFCore;
using CloudCare.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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



//Auto mapper

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



var app = builder.Build();

Console.WriteLine(app.Environment.IsDevelopment());
Console.WriteLine(app.Environment.IsProduction());


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseRouting();

//can add CORS


//can add authentication 

app.UseAuthorization();


app.MapControllers();

//Console.WriteLine(Environment.GetEnvironmentVariable("LOGNAME")); to get the actual env value 
//Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {builder.Environment.EnvironmentName}");


app.Run();

