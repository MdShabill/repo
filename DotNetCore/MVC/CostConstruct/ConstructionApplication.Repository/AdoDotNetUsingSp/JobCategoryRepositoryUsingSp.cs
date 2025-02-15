using ConstructionApplication.Core.DataModels.JobCategory;
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
    public class JobCategoryRepositoryUsingSp : IJobCategoryRepository
    {
        private readonly string _connectionString;

        public JobCategoryRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<JobCategory> GetAll()
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

                        List<JobCategory> jobCategories = new();

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            JobCategory jobCategory = new()
                            {
                                Id = Convert.ToInt32(dataTable.Rows[i]["Id"]),
                                Name = Convert.ToString(dataTable.Rows[i]["Name"])!
                            };
                            jobCategories.Add(jobCategory);
                        }
                        return jobCategories;
                    }
                }
            }
        }
    }
}
