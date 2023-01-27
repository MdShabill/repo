using Microsoft.Extensions.DependencyInjection;
using WebApiDemo1.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Get the configuration 
IConfigurationRoot? configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

string EcommerceDBConnectionString = configuration.GetConnectionString("EcommerceDBConnection");

builder.Services.AddTransient<ICustomerRepository>((svc) =>
{   
    return new CustomerRepository(EcommerceDBConnectionString);
});

builder.Services.AddTransient<IProductRepository>((svc) =>
{
    return new ProductRepository(EcommerceDBConnectionString);
});

builder.Services.AddTransient<IEmployeeRepository>((svc) =>
{
    string sqlConnectionString = configuration.GetConnectionString("EmployeeDBConnection");
    return new EmployeeRepository(sqlConnectionString);
});

builder.Services.AddTransient<IDoctorRepository>((svc) =>
{
    string sqlConnectionString = configuration.GetConnectionString("DoctorDBConnection");
    return new DoctorRepository(sqlConnectionString);
});

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
