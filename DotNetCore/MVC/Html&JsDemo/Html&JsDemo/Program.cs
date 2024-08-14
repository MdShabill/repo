
using Html_JsDemo.Repositories;

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


string htmlJsDemoDBConnectionString = configuration.GetConnectionString("HtmlJsDemoDBConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();


builder.Services.AddTransient<ICustomerRepository>((svc) =>
{
    return new CustomerRepository(htmlJsDemoDBConnectionString);
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

app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();