using ConstructionApplication.Repository.AdoDotNet;
using ConstructionApplication.Repository.AdoDotNetUsingSp;
using ConstructionApplication.Repository.Dapper;
using ConstructionApplication.Repository.DapperUsingSp;
using ConstructionApplication.Repository.Interfaces;

namespace ConstructionApplication.Helpers
{
    public class RepositoryRegistration
    {
        private readonly IServiceCollection _services;
        private readonly string _connectionString;

        public RepositoryRegistration(IServiceCollection services, string connectionString)
        {
            _services = services;
            _connectionString = connectionString;
        }

        public void RegisterAdoDotNetRepositories()
        {
            _services.AddTransient<ICostMasterRepository>(svc => new CostMasterRepository(_connectionString));
            _services.AddTransient<IDailyAttendanceRepository>(svc => new DailyAttendanceRepository(_connectionString));
            _services.AddTransient<IMaterialPurchaseRepository>(svc => new MaterialPurchaseRepository(_connectionString));
            _services.AddTransient<IMaterialRepository>(svc => new MaterialRepository(_connectionString));
            _services.AddTransient<ISupplierRepository>(svc => new SupplierRepository(_connectionString));
            _services.AddTransient<IBrandRepository>(svc => new BrandRepository(_connectionString));
            _services.AddTransient<IJobCategoryRepository>(svc => new JobCategoryRepository(_connectionString));
            _services.AddTransient<IContractorRepository>(svc => new ContractorRepository(_connectionString));
            _services.AddTransient<IAddressRepository>(svc => new AddressRepository(_connectionString));
            _services.AddTransient<ICountryRepository>(svc => new CountryRepository(_connectionString));
            _services.AddTransient<IAddressTypeRepository>(svc => new AddressTypeRepository(_connectionString));
        }

        public void RegisterAdoDotNetUsingSpRepositories()
        {
            _services.AddTransient<ICostMasterRepository>(svc => new CostMasterRepositoryUsingSp(_connectionString));
            _services.AddTransient<IDailyAttendanceRepository>(svc => new DailyAttendanceRepositoryUsingSp(_connectionString));
            _services.AddTransient<IMaterialPurchaseRepository>(svc => new MaterialPurchaseRepositoryUsingSp(_connectionString));
            _services.AddTransient<IMaterialRepository>(svc => new MaterialRepositoryUsingSp(_connectionString));
            _services.AddTransient<ISupplierRepository>(svc => new SupplierRepositoryUsingSp(_connectionString));
            _services.AddTransient<IBrandRepository>(svc => new BrandRepositoryUsingSp(_connectionString));
            _services.AddTransient<IJobCategoryRepository>(svc => new JobCategoryRepositoryUsingSp(_connectionString));
            _services.AddTransient<IContractorRepository>(svc => new ContractorRepositoryUsingSp(_connectionString));
            _services.AddTransient<IAddressRepository>(svc => new AddressRepositoryUsingSp(_connectionString));
            _services.AddTransient<ICountryRepository>(svc => new CountryRepositoryUsingSp(_connectionString));
            _services.AddTransient<IAddressTypeRepository>(svc => new AddressTypeRepositoryUsingSp(_connectionString));
        }

        public void RegisterDapperRepositories()
        {
            _services.AddTransient<ICostMasterRepository>(svc => new CostMasterRepositoryUsingDapper(_connectionString));
            _services.AddTransient<IDailyAttendanceRepository>(svc => new DailyAttendanceRepositoryUsingDapper(_connectionString));
            _services.AddTransient<IMaterialPurchaseRepository>(svc => new MaterialPurchaseRepositoryUsingDapper(_connectionString));
            _services.AddTransient<IMaterialRepository>(svc => new MaterialRepositoryUsingDapper(_connectionString));
            _services.AddTransient<ISupplierRepository>(svc => new SupplierRepositoryUsingDapper(_connectionString));
            _services.AddTransient<IBrandRepository>(svc => new BrandRepositoryUsingDapper(_connectionString));
            _services.AddTransient<IJobCategoryRepository>(svc => new JobCategoryRepositoryUsingDapper(_connectionString));
            _services.AddTransient<IContractorRepository>(svc => new ContractorRepositoryUsingDapper(_connectionString));
            _services.AddTransient<IAddressRepository>(svc => new AddressRepositoryUsingDapper(_connectionString));
            _services.AddTransient<ICountryRepository>(svc => new CountryRepositoryUsingDapper(_connectionString));
            _services.AddTransient<IAddressTypeRepository>(svc => new AddressTypeRepositoryUsingDapper(_connectionString));
        }

        public void RegisterDapperUsingSpRepositories()
        {
            _services.AddTransient<ICostMasterRepository>(svc => new CostMasterRepositoryDapperUsingSp(_connectionString));
            _services.AddTransient<IDailyAttendanceRepository>(svc => new DailyAttendanceRepositoryDapperUsingSp(_connectionString));
            _services.AddTransient<IMaterialPurchaseRepository>(svc => new MaterialPurchaseRepositoryDapperUsingSp(_connectionString));
            _services.AddTransient<IMaterialRepository>(svc => new MaterialRepositoryDapperUsingSp(_connectionString));
            _services.AddTransient<ISupplierRepository>(svc => new SupplierRepositoryDapperUsingSp(_connectionString));
            _services.AddTransient<IBrandRepository>(svc => new BrandRepositoryDapperUsingSp(_connectionString));
            _services.AddTransient<IJobCategoryRepository>(svc => new JobCategoryRepositoryDapperUsingSp(_connectionString));
            _services.AddTransient<IContractorRepository>(svc => new ContractorRepositoryDapperUsingSp(_connectionString));
            _services.AddTransient<IAddressRepository>(svc => new AddressRepositoryDapperUsingSp(_connectionString));
            _services.AddTransient<ICountryRepository>(svc => new CountryRepositoryDapperUsingSp(_connectionString));
            _services.AddTransient<IAddressTypeRepository>(svc => new AddressTypeRepositoryDapperUsingSp(_connectionString));
        }
    }
}
