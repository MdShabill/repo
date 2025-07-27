using ConstructEase.WebApp.Helpers;
using ConstructionApplication.Repository.AdoDotNet;
using ConstructionApplication.Repository.AdoDotNetUsingSp;
using ConstructionApplication.Repository.Dapper;
using ConstructionApplication.Repository.DapperUsingSp;
using ConstructionApplication.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Get the configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

string CostConstructDBConnectionString = configuration.GetConnectionString("CostConstructDBConnection");
string repositoryType = configuration["ApplicationSettings:DalTechnology"] ?? "AdoDotNet";

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Register repositories based on repositoryType
var repositoryRegistration = new RepositoryRegistration();

if (repositoryType == "AdoDotNet")
{
    repositoryRegistration.RegisterAdoDotNetRepositories(builder.Services, CostConstructDBConnectionString);
}
else if (repositoryType == "AdoDotNetUsingSp")
{
    repositoryRegistration.RegisterAdoDotNetUsingSpRepositories(builder.Services, CostConstructDBConnectionString);
}
else if (repositoryType == "Dapper")
{
    repositoryRegistration.RegisterDapperRepositories(builder.Services, CostConstructDBConnectionString);
}
else
{
    repositoryRegistration.RegisterDapperUsingSpRepositories(builder.Services, CostConstructDBConnectionString);
}

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // This shows detailed errors in development mode
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
