using ConstructionApplication.Core.DataModels.Suppliers;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class SupplierRepositoryDapperUsingSp : ISupplierRepository
    {
        private readonly string _connectionString;

        public SupplierRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Supplier> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Supplier>("Sp_GetAllSuppliers", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
