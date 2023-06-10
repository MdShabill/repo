using MyWebApp.Repositories;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// Get the configuration 
IConfigurationRoot? configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

string EcommerceDBConnectionString = configuration.GetConnectionString("EcommerceDBConnection");

builder.Services.AddTransient<IProductRepository>((svc) =>
{
    return new ProductRepository(EcommerceDBConnectionString);
});

