using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.Dapper
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
                try
                {
                    List<AddressType> result = connection.Query<AddressType>(sqlQuery).ToList();

                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}