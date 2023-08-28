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

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(20);// here you can mention the timings
});

builder.Services.AddTransient<IProductRepository>((svc) =>
{
    return new ProductRepository(ECommerceDBConnectionString);
});

builder.Services.AddTransient<ICustomerRepository>((svc) =>
{
    return new CustomerRepository(ECommerceDBConnectionString);
});

builder.Services.AddTransient<IOrderRepository>((svc) =>
{
    return new OrderRepository(ECommerceDBConnectionString);
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
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();