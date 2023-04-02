using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VeendingMachine.API.DataAccess.DatabaseContext;
using VeendingMachine.API.DataAccess.Interfaces;
using VeendingMachine.API.DataAccess.Repositories;
using VendingMachine.API.BusinessLogic.Interfaces;
using VendingMachine.API.BusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddDbContext<VendorContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")), ServiceLifetime.Transient);
builder.Services.AddScoped<IDepositStackRepository, DepositStackRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IDataPopulationService, DataPopulationService>();
builder.Services.AddScoped<IVendingMachineService, VendingMaschineService>();
builder.Services.AddScoped<IMoneyUnitRepository, MoneyUnitRepository>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
//issiaiskinti, ar tikrai scoped ir KODEL
builder.Services.AddScoped<VendorContext>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    try
    {
        var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataPopulationService>();
        await dataInitializer.SeedInitialData();
    }
    catch (SqlException ex)
    {
        Console.WriteLine("SQL server error occurred: " + ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Server error occurred: " + ex.Message);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
