using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class BrandRepositoryDapperUsingSp : IBrandRepository
    {
        private readonly string _connectionString;

        public BrandRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Brand> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Brand>("Sp_GetAllBrands", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
