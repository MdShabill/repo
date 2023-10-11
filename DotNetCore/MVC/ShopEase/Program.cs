using Microsoft.AspNetCore.DataProtection.Repositories;
using ShopEase.Repositories.Product;

var builder = WebApplication.CreateBuilder(args);

// Get the configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();


string ShopEaseDBConnectionString = configuration.GetConnectionString("ShopEaseDBConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IProductRepository>((svc) =>
{
    return new ProductRepository(ShopEaseDBConnectionString);
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();