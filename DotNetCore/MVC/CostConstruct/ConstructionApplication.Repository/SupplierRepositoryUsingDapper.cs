using ConstructionApplication.Core.DataModels.Suppliers;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ConstructionApplication.Repository
{
    public class SupplierRepositoryUsingDapper : ISupplierRepository
    {
        private readonly string _connectionString;

        public SupplierRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Supplier> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name FROM Suppliers";
                return connection.Query<Supplier>(sqlQuery).ToList();
            }
        }
    }
}
