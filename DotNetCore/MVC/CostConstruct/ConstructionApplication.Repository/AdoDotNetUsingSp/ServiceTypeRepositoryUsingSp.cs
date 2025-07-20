using ConstructionApplication.Core.DataModels.ServiceTypes;
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
    public class ServiceTypeRepositoryUsingSp : IServiceTypeRepository
    {
        private readonly string _connectionString;

        public ServiceTypeRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ServiceType> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                using (SqlCommand sqlCommand = new("Sp_GetAllJobCategories", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();

                    using (SqlDataAdapter sqlDataAdapter = new(sqlCommand))
                    {
                        DataTable dataTable = new();
                        sqlDataAdapter.Fill(dataTable);

                        List<ServiceType> jobCategories = new();

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            ServiceType serviceType = new()
                            {
                                Id = Convert.ToInt32(dataTable.Rows[i]["Id"]),
                                Name = Convert.ToString(dataTable.Rows[i]["Name"])!
                            };
                            jobCategories.Add(serviceType);
                        }
                        return jobCategories;
                    }
                }
            }
        }
    }
}
