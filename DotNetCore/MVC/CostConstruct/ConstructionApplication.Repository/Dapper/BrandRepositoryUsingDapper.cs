using ConstructionApplication.Core.DataModels.Brands;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.Dapper
{
    public class BrandRepositoryUsingDapper : IBrandRepository
    {
        private readonly string _connectionString;

        public BrandRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Brand> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name FROM Brands";
                return connection.Query<Brand>(sqlQuery).ToList();
            }
        }
    }
}
