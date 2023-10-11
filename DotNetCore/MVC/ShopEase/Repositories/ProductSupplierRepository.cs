using ShopEase.DataModels.Product;
using System.Data.SqlClient;
using System.Data;

namespace ShopEase.Repositories
{
    public class ProductSupplierRepository : IProductSupplierRepository
    {
        private readonly string _connectionString;

        public ProductSupplierRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ProductSupplier> GetSuppliers()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Id, SupplierName From Suppliers";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductSupplier> productSuppliers = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductSupplier productSupplier = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        SupplierName = (string)dataTable.Rows[i]["SupplierName"]
                    };
                    productSuppliers.Add(productSupplier);
                }
                return productSuppliers;
            }
        }
    }
}
