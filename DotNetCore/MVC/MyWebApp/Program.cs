using MyWebApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Get the configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();


string ECommerceDBConnectionString = configuration.GetConnectionString("ECommerceDBConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IProductRepository>((svc) =>
{
    return new ProductRepository(ECommerceDBConnectionString);
});

builder.Services.AddTransient<ICustomerRepository>((svc) =>
{
    return new CustomerRepository(ECommerceDBConnectionString);
});

builder.Services.AddTransient<IEmployeeRepository>((svc) =>
{
    return new EmployeeRepository(ECommerceDBConnectionString);
});

builder.Services.AddTransient<IMovieRepository>((svc) =>
{
    return new MovieRepository(ECommerceDBConnectionString);
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