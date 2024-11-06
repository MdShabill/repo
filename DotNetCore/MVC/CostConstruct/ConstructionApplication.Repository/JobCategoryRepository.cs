using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.JobCategory;
using System.Data.SqlClient;
using System.Data;
using ConstructionApplication.Repository.Interfaces;

namespace ConstructionApplication.Repositories
{
    public class JobCategoryRepository : IJobCategoryRepository
    {
        private readonly string _connectionString;

        public JobCategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<JobCategory> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name FROM JobCategories ";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<JobCategory> jobCategories = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    JobCategory jobCategory = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Name = (string)dataTable.Rows[i]["Name"]
                    };
                    jobCategories.Add(jobCategory);
                }
                return jobCategories;
            }
        }
    }
}
