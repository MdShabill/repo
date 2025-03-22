using ConstructionApplication.Core.DataModels.Material;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class MaterialRepositoryDapperUsingSp : IMaterialRepository
    {
        private readonly string _connectionString;

        public MaterialRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Material> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Mode", "GetAll");

                return connection.Query<Material>("Sp_MaterialsDetails", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Material GetMaterialInfo(int Id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Mode", "GetMaterialInfo");
                parameters.Add("@Id", Id);

                return connection.QueryFirstOrDefault<Material>("Sp_MaterialsDetails", parameters, commandType: CommandType.StoredProcedure) ?? new Material();
            }
        }
    }
}
