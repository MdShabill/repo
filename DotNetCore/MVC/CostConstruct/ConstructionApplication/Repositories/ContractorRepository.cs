using ConstructionApplication.DataModels.Contractor;
using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.DataModels.JobCategory;
using System.Data.SqlClient;
using System.Data;
using ConstructionApplication.DataModels.Brands;

namespace ConstructionApplication.Repositories
{
    public class ContractorRepository : IContractorRepository
    {
        private readonly string _connectionString;

        public ContractorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Contractor> GetAllContractors(int jobCategoryId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select Id As ContractorId, Name From Contractors 
                                    WHERE JobCategoryId = @jobCategoryId ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@jobCategoryId", jobCategoryId);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Contractor> contractors = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Contractor contractor = new()
                    {
                        ContractorId = (int)dataTable.Rows[i]["ContractorId"],
                        Name = (string)dataTable.Rows[i]["Name"]
                    };
                    contractors.Add(contractor);
                }
                return contractors;
            }
        }
    }
}
