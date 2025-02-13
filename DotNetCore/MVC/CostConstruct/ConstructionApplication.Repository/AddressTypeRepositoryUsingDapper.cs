using ConstructionApplication.Core.DataModels.AddressType;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository
{
    public class AddressTypeRepositoryUsingDapper : IAddressTypeRepository
    {
        private readonly string _connectionString;

        public AddressTypeRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<AddressType> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "Select Id, Name From AddressTypes";
                return connection.Query<AddressType>(sqlQuery).ToList();
            }
        }
    }
}
