using Microsoft.Extensions.DependencyInjection;
using WebApiDemo1.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Get the configuration 
IConfigurationRoot? configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

string EcommerceDBConnectionString = configuration.GetConnectionString("EcommerceDBConnection");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://example.com")
                            .AllowAnyMethod()
                            .AllowAnyHeader());
});

builder.Services.AddTransient<ICustomerRepository>((svc) =>
{   
    return new CustomerRepository(EcommerceDBConnectionString);
});

builder.Services.AddTransient<IAddressRepository>((svc) =>
{
    return new AddressRepository(EcommerceDBConnectionString);
});

builder.Services.AddTransient<IProductRepository>((svc) =>
{
    return new ProductRepository(EcommerceDBConnectionString);
});

builder.Services.AddTransient<IOrderRepository>((svc) =>
{
    return new OrderRepository(EcommerceDBConnectionString);
});

builder.Services.AddTransient<IBankAccountServiceRepository>((svc) =>
{
    return new BankAccountServiceRepository(EcommerceDBConnectionString);
});

builder.Services.AddTransient<IEmployeeRepository>((svc) =>
{
    string sqlConnectionString = configuration.GetConnectionString("EmployeeDBConnection");
    return new EmployeeRepository(sqlConnectionString);
});

builder.Services.AddTransient<IDoctorRepository>((svc) =>
{
    string sqlConnectionString = configuration.GetConnectionString("SchoolManagementDB");
    return new DoctorRepository(sqlConnectionString);
});

builder.Services.AddTransient<ITeacherRepository>((svc) =>
{
    string sqlConnectionString = configuration.GetConnectionString("TeacherDBConnection");
    return new TeacherRepository(sqlConnectionString);
});

builder.Services.AddTransient<IStudentRepository>((svc) =>
{
    string sqlConnectionString = configuration.GetConnectionString("SchoolManagementDB");
    return new StudentRepository(sqlConnectionString);
});

builder.Services.AddTransient<IMovieRepository>((svc) =>
{
    string sqlConnectionString = configuration.GetConnectionString("MoviesDB");
    return new MovieRepository(sqlConnectionString);
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseCors("AllowSpecificOrigin");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
