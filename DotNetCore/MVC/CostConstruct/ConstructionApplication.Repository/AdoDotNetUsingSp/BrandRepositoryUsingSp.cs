using ConstructionApplication.Core.DataModels.Brands;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionApplication.Repository.AdoDotNetUsingSp
{
    public class BrandRepositoryUsingSp : IBrandRepository
    {
        private readonly string _connectionString;

        public BrandRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Brand> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("Sp_GetAllBrands", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
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
