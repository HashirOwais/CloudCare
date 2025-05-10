using CloudCare.API.Repositories.Interfaces;
using CloudCare.API.Repositories.Mock;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



#region Testing registering Services

builder.Services.AddScoped<IExpenseRepository, MockExpenseRepository>();
builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, MockPaymentMethodRepository>();
builder.Services.AddScoped<IVendorRepository, MockVendorRepository>();

#endregion



//Auto mapper

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



var app = builder.Build();

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





app.Run();

