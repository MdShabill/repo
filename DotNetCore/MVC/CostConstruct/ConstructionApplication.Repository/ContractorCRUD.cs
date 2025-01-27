using ConstructionApplication.Core.DataModels.Contractor;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace ConstructionApplication.Repositories
{
    public class ContractorCRUD : IContractorRepository
    {
        private readonly string _connectionString;

        public ContractorCRUD(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Contractor> GetAll(int? jobCategoryId, int? id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "EXEC GetContractors";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@jobCategoryId", jobCategoryId ?? (object)DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id ?? (object)DBNull.Value);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Contractor> contractors = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Contractor contractor = new()
                    {


                        JobCategoryId = (int)dataTable.Rows[i]["JobCategoryId"],
                        JobTypes = (string)dataTable.Rows[i]["JobTypes"],
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
                string sqlQuery = @"Insert Into Contractors
                       (JobCategoryId, Name, Gender, DOB, ImageName, MobileNumber, ReferredBy)
                       Values
                       (@jobCategoryId, @name, @gender, @dOB, @imageName, @mobileNumber, @referredBy)
                       Select Scope_Identity()";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
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
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteContractorQuery = @"DELETE FROM Contractors WHERE Id = @ContractorId";
                SqlCommand deleteContractorCommand = new(deleteContractorQuery, sqlConnection);
                deleteContractorCommand.Parameters.AddWithValue("@ContractorId", contractorId);
                sqlConnection.Open();
                deleteContractorCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public int Update(Contractor contractor)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"UPDATE Contractors SET
                            JobCategoryId = @jobCategoryId,
                            Name = @name,
                            Gender = @gender,
                            DOB = @dob,
                            MobileNumber = @mobileNumber,
                            ReferredBy = @referredBy
                            WHERE Id = @id";

                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", contractor.ContractorId);
                    sqlCommand.Parameters.AddWithValue("@jobCategoryId", contractor.JobCategoryId);
                    sqlCommand.Parameters.AddWithValue("@name", contractor.ContractorName);
                    sqlCommand.Parameters.AddWithValue("@gender", contractor.Gender);
                    sqlCommand.Parameters.AddWithValue("@dob", contractor.DOB);
                    sqlCommand.Parameters.AddWithValue("@mobileNumber", contractor.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@referredBy", contractor.ReferredBy);

                    sqlConnection.Open();
                    int affectedRowCount = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    return affectedRowCount;
                }
            }
        }
    }
}
