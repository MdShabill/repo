using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class AddressTypeRepositoryDapperUsingSp : IAddressTypeRepository
    {
        private readonly string _connectionString;

        public AddressTypeRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<AddressType> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<AddressType>("Sp_GetAllAddressTypes", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
