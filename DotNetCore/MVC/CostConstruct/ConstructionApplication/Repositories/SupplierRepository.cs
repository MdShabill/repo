using ConstructionApplication.DataModels.Material;
using ConstructionApplication.DataModels.Suppliers;
using System.Data.SqlClient;
using System.Data;

namespace ConstructionApplication.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly string _connectionString;

        public SupplierRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Supplier> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name FROM Suppliers";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Supplier> suppliers = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Supplier supplier = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Name = (string)dataTable.Rows[i]["Name"]
                    };
                    suppliers.Add(supplier);
                }
                return suppliers;
            }
        }
    }
}
