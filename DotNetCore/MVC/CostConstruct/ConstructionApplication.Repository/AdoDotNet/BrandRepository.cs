using ConstructionApplication.Core.DataModels.Brands;
using ConstructionApplication.Core.DataModels.Suppliers;
using System.Data.SqlClient;
using System.Data;
using ConstructionApplication.Repository.Interfaces;

namespace ConstructionApplication.Repository.AdoDotNet
{
    public class BrandRepository : IBrandRepository
    {
        private readonly string _connectionString;

        public BrandRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Brand> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name FROM Brands";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Brand> brands = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Brand brand = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Name = (string)dataTable.Rows[i]["Name"]
                    };
                    brands.Add(brand);
                }
                return brands;
            }
        }
    }
}
