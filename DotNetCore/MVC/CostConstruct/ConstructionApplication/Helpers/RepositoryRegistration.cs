using ConstructionApplication.Repository.AdoDotNet;
using ConstructionApplication.Repository.AdoDotNetUsingSp;
using ConstructionApplication.Repository.Dapper;
using ConstructionApplication.Repository.DapperUsingSp;
using ConstructionApplication.Repository.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace ConstructionApplication.Helpers
{
    public class RepositoryRegistration
    {
        //Approach: 1 = With constructor 
        //private readonly IServiceCollection _services;
        //private readonly string _connectionString;

        //public RepositoryRegistration(IServiceCollection services, string connectionString)
        //{
        //    _services = services;
        //    _connectionString = connectionString;
        //}

        //Approach: 2 Without constructor we use as parameters
        public void RegisterAdoDotNetRepositories(IServiceCollection services, string connectionString)
        {
            services.AddTransient<ICostMasterRepository>(svc => new CostMasterRepository(connectionString));
            services.AddTransient<IDailyAttendanceRepository>(svc => new DailyAttendanceRepository(connectionString));
            services.AddTransient<IMaterialPurchaseRepository>(svc => new MaterialPurchaseRepository(connectionString));
            services.AddTransient<IMaterialRepository>(svc => new MaterialRepository(connectionString));
            services.AddTransient<ISupplierRepository>(svc => new SupplierRepository(connectionString));
            services.AddTransient<IBrandRepository>(svc => new BrandRepository(connectionString));
            services.AddTransient<IJobCategoryRepository>(svc => new JobCategoryRepository(connectionString));
            services.AddTransient<IContractorRepository>(svc => new ContractorRepository(connectionString));
            services.AddTransient<IAddressRepository>(svc => new AddressRepository(connectionString));
            services.AddTransient<ICountryRepository>(svc => new CountryRepository(connectionString));
            services.AddTransient<IAddressTypeRepository>(svc => new AddressTypeRepository(connectionString));
            services.AddTransient<IUserRepository>(svc => new UserRepository(connectionString));
            services.AddTransient<ISiteRepository>(svc => new SiteRepository(connectionString));
        }

        public void RegisterAdoDotNetUsingSpRepositories(IServiceCollection services, string connectionString)
        {
            services.AddTransient<ICostMasterRepository>(svc => new CostMasterRepositoryUsingSp(connectionString));
            services.AddTransient<IDailyAttendanceRepository>(svc => new DailyAttendanceRepositoryUsingSp(connectionString));
            services.AddTransient<IMaterialPurchaseRepository>(svc => new MaterialPurchaseRepositoryUsingSp(connectionString));
            services.AddTransient<IMaterialRepository>(svc => new MaterialRepositoryUsingSp(connectionString));
            services.AddTransient<ISupplierRepository>(svc => new SupplierRepositoryUsingSp(connectionString));
            services.AddTransient<IBrandRepository>(svc => new BrandRepositoryUsingSp(connectionString));
            services.AddTransient<IJobCategoryRepository>(svc => new JobCategoryRepositoryUsingSp(connectionString));
            services.AddTransient<IContractorRepository>(svc => new ContractorRepositoryUsingSp(connectionString));
            services.AddTransient<IAddressRepository>(svc => new AddressRepositoryUsingSp(connectionString));
            services.AddTransient<ICountryRepository>(svc => new CountryRepositoryUsingSp(connectionString));
            services.AddTransient<IAddressTypeRepository>(svc => new AddressTypeRepositoryUsingSp(connectionString));
        }

        public void RegisterDapperRepositories(IServiceCollection services, string connectionString)
        {
            services.AddTransient<ICostMasterRepository>(svc => new CostMasterRepositoryUsingDapper(connectionString));
            services.AddTransient<IDailyAttendanceRepository>(svc => new DailyAttendanceRepositoryUsingDapper(connectionString));
            services.AddTransient<IMaterialPurchaseRepository>(svc => new MaterialPurchaseRepositoryUsingDapper(connectionString));
            services.AddTransient<IMaterialRepository>(svc => new MaterialRepositoryUsingDapper(connectionString));
            services.AddTransient<ISupplierRepository>(svc => new SupplierRepositoryUsingDapper(connectionString));
            services.AddTransient<IBrandRepository>(svc => new BrandRepositoryUsingDapper(connectionString));
            services.AddTransient<IJobCategoryRepository>(svc => new JobCategoryRepositoryUsingDapper(connectionString));
            services.AddTransient<IContractorRepository>(svc => new ContractorRepositoryUsingDapper(connectionString));
            services.AddTransient<IAddressRepository>(svc => new AddressRepositoryUsingDapper(connectionString));
            services.AddTransient<ICountryRepository>(svc => new CountryRepositoryUsingDapper(connectionString));
            services.AddTransient<IAddressTypeRepository>(svc => new AddressTypeRepositoryUsingDapper(connectionString));
        }

        public void RegisterDapperUsingSpRepositories(IServiceCollection services, string connectionString)
        {
            services.AddTransient<ICostMasterRepository>(svc => new CostMasterRepositoryDapperUsingSp(connectionString));
            services.AddTransient<IDailyAttendanceRepository>(svc => new DailyAttendanceRepositoryDapperUsingSp(connectionString));
            services.AddTransient<IMaterialPurchaseRepository>(svc => new MaterialPurchaseRepositoryDapperUsingSp(connectionString));
            services.AddTransient<IMaterialRepository>(svc => new MaterialRepositoryDapperUsingSp(connectionString));
            services.AddTransient<ISupplierRepository>(svc => new SupplierRepositoryDapperUsingSp(connectionString));
            services.AddTransient<IBrandRepository>(svc => new BrandRepositoryDapperUsingSp(connectionString));
            services.AddTransient<IJobCategoryRepository>(svc => new JobCategoryRepositoryDapperUsingSp(connectionString));
            services.AddTransient<IContractorRepository>(svc => new ContractorRepositoryDapperUsingSp(connectionString));
            services.AddTransient<IAddressRepository>(svc => new AddressRepositoryDapperUsingSp(connectionString));
            services.AddTransient<ICountryRepository>(svc => new CountryRepositoryDapperUsingSp(connectionString));
            services.AddTransient<IAddressTypeRepository>(svc => new AddressTypeRepositoryDapperUsingSp(connectionString));
        }
    }
}
