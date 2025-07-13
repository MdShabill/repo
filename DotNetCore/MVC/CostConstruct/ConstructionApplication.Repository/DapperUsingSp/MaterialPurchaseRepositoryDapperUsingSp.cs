using ConstructionApplication.Core.DataModels.MaterialPurchase;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class MaterialPurchaseRepositoryDapperUsingSp : IMaterialPurchaseRepository
    {
        private readonly string _connectionString;

        public MaterialPurchaseRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<MaterialPurchase> GetAll(int siteId, DateTime? DateFrom, DateTime? DateTo, int? MaterialId, int? SupplierId, int? BrandId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Mode = "Get_All", DateFrom, DateTo, MaterialId, SupplierId, BrandId
                };
                return connection.Query<MaterialPurchase>("Sp_MaterialPurchaseCRUD", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public int Create(MaterialPurchase materialPurchase)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Mode = "Create",
                    materialPurchase.MaterialId,
                    materialPurchase.SupplierId,
                    materialPurchase.PhoneNumber,
                    materialPurchase.BrandId,
                    materialPurchase.Quantity,
                    materialPurchase.UnitOfMeasure,
                    materialPurchase.Date,
                    materialPurchase.MaterialCost,
                    materialPurchase.DeliveryCharge
                };
                return connection.ExecuteScalar<int>("Sp_MaterialPurchaseCRUD", parameters,commandType: CommandType.StoredProcedure);
            }
        }
    }
}
