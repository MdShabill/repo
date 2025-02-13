using ConstructionApplication.Core.DataModels.Suppliers;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionApplication.Repository
{
    public class SupplierRepositoryUsingSp : ISupplierRepository
    {
        private readonly string _connectionString;

        public SupplierRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Supplier> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("Sp_GetAllSuppliers", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

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
