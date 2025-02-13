using ConstructionApplication.Core.DataModels.Material;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository
{
    public class MaterialRepositoryUsingDapper : IMaterialRepository
    {
        private readonly string _connectionString;

        public MaterialRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Material> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name FROM Materials";
                return connection.Query<Material>(sqlQuery).ToList();
            }
        }

        public Material GetMaterialInfo(int Id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"SELECT 
                        Id, UnitOfMeasure, UnitPrice 
                        FROM 
                            Materials
                        WHERE Id = @id";

                return connection.QueryFirstOrDefault<Material>(sqlQuery, new { id = Id }) ?? new Material();
            }
        }
    }
}
