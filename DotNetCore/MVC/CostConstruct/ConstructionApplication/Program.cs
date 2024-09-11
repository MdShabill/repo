using ConstructionApplication.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Get the configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

string CostConstructDBConnectionString = configuration.GetConnectionString("CostConstructDBConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddTransient<ICostMasterRepository>((svc) =>
{
    return new CostMasterRepository(CostConstructDBConnectionString);
});

builder.Services.AddTransient<IDailyAttendanceRepository>((svc) =>
{
    return new DailyAttendanceRepository(CostConstructDBConnectionString);
});

builder.Services.AddTransient<IAttendanceDetailsRepository>((svc) =>
{
    return new AttendanceDetailsRepository(CostConstructDBConnectionString);
});

builder.Services.AddTransient<IMaterialPurchaseRepository>((svc) =>
{
    return new MaterialPurchaseRepository(CostConstructDBConnectionString);
});

builder.Services.AddTransient<IMaterialRepository>((svc) =>
{
    return new MaterialRepository(CostConstructDBConnectionString);
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
