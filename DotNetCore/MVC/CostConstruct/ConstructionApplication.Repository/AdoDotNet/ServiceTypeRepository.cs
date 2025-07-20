using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.ServiceTypes;
using System.Data.SqlClient;
using System.Data;
using ConstructionApplication.Repository.Interfaces;

namespace ConstructionApplication.Repository.AdoDotNet
{
    public class ServiceTypeRepository : IServiceTypeRepository
    {
        private readonly string _connectionString;

        public ServiceTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ServiceType> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name FROM ServiceTypes ";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ServiceType> serviceTypes = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ServiceType serviceType = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Name = (string)dataTable.Rows[i]["Name"]
                    };
                    serviceTypes.Add(serviceType);
                }
                return serviceTypes;
            }
        }
    }
}
