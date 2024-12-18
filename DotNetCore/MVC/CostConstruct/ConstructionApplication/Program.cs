using ConstructionApplication.Repositories;
using ConstructionApplication.Repository;
using ConstructionApplication.Repository.Interfaces;

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

builder.Services.AddTransient<ISupplierRepository>((svc) =>
{
    return new SupplierRepository(CostConstructDBConnectionString);
});

builder.Services.AddTransient<IBrandRepository>((svc) =>
{
    return new BrandRepository(CostConstructDBConnectionString);
});

builder.Services.AddTransient<IJobCategoryRepository>((svc) =>
{
    return new JobCategoryRepository(CostConstructDBConnectionString);
});

builder.Services.AddTransient<IContractorRepository>((svc) =>
{
    return new ContractorRepository(CostConstructDBConnectionString);
});

builder.Services.AddTransient<IAddressRepository>((svc) =>
{
    return new AddressRepository(CostConstructDBConnectionString);
});

builder.Services.AddTransient<ICountryRepository>((svc) =>
{
    return new CountryRepository(CostConstructDBConnectionString);
});

builder.Services.AddTransient<IAddressTypeRepository>((svc) =>
{
    return new AddressTypeRepository(CostConstructDBConnectionString);
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
