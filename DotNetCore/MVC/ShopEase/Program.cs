using ShopEase.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Get the configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://example.com")
                            .AllowAnyMethod()
                            .AllowAnyHeader());
});


string ShopEaseDBConnectionString = configuration.GetConnectionString("ShopEaseDBConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddTransient<IProductRepository>((svc) =>
{
    return new ProductRepository(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<IProductRepositoryUsingSP>((svc) =>
{
    return new ProductRepositoryUsingSP(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<IProductBrandRepository>((svc) =>
{
    return new ProductBrandRepository(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<IProductCategoryReopsitory>((svc) =>
{
    return new ProductCategoryRepository(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<IProductSupplierRepository>((svc) =>
{
    return new ProductSupplierRepository(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<IOrderRepository>((svc) =>
{
    return new OrderRepository(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<ICartRepository>((svc) =>
{
    return new CartRepository(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<IAddressRepository>((svc) =>
{
    return new AddressRepository(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<ICountryRepository>((svc) =>
{
    return new CountryRepository(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<ICardDetailRepository>((svc) =>
{
    return new CardDetailRepository(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<ICustomerRepository>((svc) =>
{
    return new CustomerRepository(ShopEaseDBConnectionString);
});

builder.Services.AddTransient<IHealthManagementRepository>((svc) =>
{
    string sqlConnectionString = configuration.GetConnectionString("HealthAppointmentManagementDBConnection");
    return new HealthManagementRepository(sqlConnectionString);
});


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();