using ConstructionApplication.Core.DataModels.Contractor;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace ConstructionApplication.Repository.AdoDotNetUsingSp
{
    public class ContractorRepositoryUsingSp : IContractorRepository
    {
        private readonly string _connectionString;

        public ContractorRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Contractor> GetAll(int? jobCategoryId, int? id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SP_ConreactorCRUD";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection)
                {
                    SelectCommand = { CommandType = CommandType.StoredProcedure }
                };
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Mode", 2);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@jobCategoryId", jobCategoryId ?? (object)DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id ?? (object)DBNull.Value);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Contractor> contractors = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Contractor contractor = new()
                    {
                        JobCategoryId = dataTable.Rows[i]["JobCategoryId"] != DBNull.Value ? (int)dataTable.Rows[i]["JobCategoryId"] : 0,
                        JobTypes = dataTable.Rows[i]["JobTypes"] != DBNull.Value ? (string)dataTable.Rows[i]["JobTypes"] : null,
                        ContractorId = (int)dataTable.Rows[i]["ContractorId"],
                        ContractorName = (string)dataTable.Rows[i]["ContractorName"],
                        Gender = (GenderTypes)dataTable.Rows[i]["Gender"],
                        DOB = (DateTime)dataTable.Rows[i]["DOB"],
                        MobileNumber = (string)dataTable.Rows[i]["MobileNumber"],
                        ReferredBy = dataTable.Rows[i]["ReferredBy"] != DBNull.Value ? (string)dataTable.Rows[i]["ReferredBy"] : null,

                        AddressLine1 = dataTable.Rows[i]["AddressLine1"] != DBNull.Value ? (string)dataTable.Rows[i]["AddressLine1"] : null,
                        AddressTypeId = dataTable.Rows[i]["AddressTypeId"] != DBNull.Value ? (int)dataTable.Rows[i]["AddressTypeId"] : 0,
                        AddressTypes = dataTable.Rows[i]["AddressTypes"] != DBNull.Value ? (string)dataTable.Rows[i]["AddressTypes"] : null,
                        CountryId = dataTable.Rows[i]["CountryId"] != DBNull.Value ? (int)dataTable.Rows[i]["CountryId"] : 0,
                        CountryName = dataTable.Rows[i]["CountryName"] != DBNull.Value ? (string)dataTable.Rows[i]["CountryName"] : null,
                        PinCode = dataTable.Rows[i]["PinCode"] != DBNull.Value ? (int)dataTable.Rows[i]["PinCode"] : 0
                    };
                    contractors.Add(contractor);
                }
                return contractors;
            }
        }

        public int Add(Contractor contractor)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SP_ConreactorCRUD";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Mode", 1);
                sqlCommand.Parameters.AddWithValue("@jobCategoryId", contractor.JobCategoryId);
                sqlCommand.Parameters.AddWithValue("@name", contractor.ContractorName);
                sqlCommand.Parameters.AddWithValue("@gender", contractor.Gender);
                sqlCommand.Parameters.AddWithValue("@dOB", contractor.DOB);
                if (string.IsNullOrEmpty(contractor.ImageName))
                    sqlCommand.Parameters.AddWithValue("@imageName", DBNull.Value);
                else
                    sqlCommand.Parameters.AddWithValue("@imageName", contractor.ImageName);

                sqlCommand.Parameters.AddWithValue("@mobileNumber", contractor.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@referredBy", contractor.ReferredBy);

                sqlConnection.Open();
                contractor.ContractorId = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();

                return contractor.ContractorId;
            }
        }

        public void Delete(int contractorId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string storedProcedureName = "SP_ConreactorCRUD";

                using (SqlCommand deleteContractorCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    // Set the command type to StoredProcedure
                    deleteContractorCommand.CommandType = CommandType.StoredProcedure;

                    deleteContractorCommand.Parameters.AddWithValue("@Mode", 4);
                    deleteContractorCommand.Parameters.AddWithValue("@Id", contractorId);
                    sqlConnection.Open();
                    deleteContractorCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }


        public int Update(Contractor contractor)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SP_ConreactorCRUD";

                using (SqlCommand sqlUpdateCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    // Set the command type to StoredProcedure
                    sqlUpdateCommand.CommandType = CommandType.StoredProcedure;

                    sqlUpdateCommand.Parameters.AddWithValue("@Mode", 3);
                    sqlUpdateCommand.Parameters.AddWithValue("@id", contractor.ContractorId);
                    sqlUpdateCommand.Parameters.AddWithValue("@jobCategoryId", contractor.JobCategoryId);
                    sqlUpdateCommand.Parameters.AddWithValue("@name", contractor.ContractorName);
                    sqlUpdateCommand.Parameters.AddWithValue("@gender", contractor.Gender);
                    sqlUpdateCommand.Parameters.AddWithValue("@dob", contractor.DOB);
                    sqlUpdateCommand.Parameters.AddWithValue("@mobileNumber", contractor.MobileNumber);
                    sqlUpdateCommand.Parameters.AddWithValue("@referredBy", contractor.ReferredBy);

                    sqlConnection.Open();
                    object result = sqlUpdateCommand.ExecuteScalar();
                    sqlConnection.Close();
                    int affectedRowCount = result != null ? Convert.ToInt32(result) : 0;
                    return affectedRowCount;
                }
            }
        }
    }
}
