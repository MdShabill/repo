using ConstructionApplication.DataModels.Material;
using System.Data.SqlClient;
using System.Data;

namespace ConstructionApplication.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly string _connectionString;

        public MaterialRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Material> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT 
                       Materials.Id, Materials.Name,
                       Materials.BrandId, Brands.Name As BrandName,
                       Materials.SupplierId, Suppliers.Name As SupplierName 
                       FROM Materials
                       JOIN Brands ON Materials.BrandId = Brands.Id
                       JOIN Suppliers  ON Materials.SupplierId = Suppliers.Id";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Material> materials = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Material material = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Name = (string)dataTable.Rows[i]["Name"],
                        BrandId = (int)dataTable.Rows[i]["BrandId"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        SupplierId = (int)dataTable.Rows[i]["SupplierId"],
                        SupplierName = (string)dataTable.Rows[i]["SupplierName"]
                    };
                    material.Name = $"{material.Name} - {material.BrandName} - {material.SupplierName}";
                    materials.Add(material);
                }
                return materials;
            }
        }
    }
}
