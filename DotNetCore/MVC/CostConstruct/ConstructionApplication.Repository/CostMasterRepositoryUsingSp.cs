using ConstructionApplication.Core.DataModels.CostMaster;
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
    public class CostMasterRepositoryUsingSp : ICostMasterRepository
    {
        private readonly string _connectionString;

        public CostMasterRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CostMaster> GetByJobCategory(int jobCategoryId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlCommand sqlCommand = new("Sp_CostMasterCRUD", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Mode", "GET_BY_JOBCATEGORY");
                sqlCommand.Parameters.AddWithValue("@jobCategoryId", jobCategoryId);
                SqlDataAdapter sqlDataAdapter = new(sqlCommand);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<CostMaster> costMasters = new();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    CostMaster costMaster = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        JobCategoryId = (int)dataTable.Rows[i]["JobCategoryId"],
                        Name = (string)dataTable.Rows[i]["Name"],
                        Cost = (decimal)dataTable.Rows[i]["Cost"],
                        Date = (DateTime)dataTable.Rows[i]["Date"]
                    };
                    costMasters.Add(costMaster);
                }
                return costMasters;
            }
        }

        public CostMaster GetActiveCostDetail(int jobCategoryId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlCommand command = new SqlCommand("Sp_CostMasterCRUD", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Mode", "GET_ACTIVE_COST");
                command.Parameters.AddWithValue("@JobCategoryId", jobCategoryId);
                command.Parameters.AddWithValue("@CurrentDate", DateTime.Now);

                SqlDataAdapter sqlDataAdapter = new(command);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    return new CostMaster
                    {
                        JobCategoryId = (int)row["JobCategoryId"],
                        Cost = (decimal)row["Cost"],
                        Date = (DateTime)row["Date"]
                    };
                }
                return null;
            }
        }

        public int Create(CostMaster costMaster)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                SqlCommand insertCommand = new SqlCommand("Sp_CostMasterCRUD", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                insertCommand.Parameters.AddWithValue("@Mode", "CREATE");
                insertCommand.Parameters.AddWithValue("@jobCategoryId", costMaster.JobCategoryId);
                insertCommand.Parameters.AddWithValue("@cost", costMaster.Cost);
                insertCommand.Parameters.AddWithValue("@date", costMaster.Date);
                sqlConnection.Open();
                object result = insertCommand.ExecuteScalar();
                sqlConnection.Close();
                int affectedRowCount = (result != null) ? Convert.ToInt32(result) : 0;

                return affectedRowCount;
            }
        }
    }
}
