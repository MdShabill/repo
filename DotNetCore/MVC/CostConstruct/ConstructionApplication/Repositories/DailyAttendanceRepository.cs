using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.DataModels.DailyAttendance;
using ConstructionApplication.ViewModels.CostMasterVm;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repositories
{
    public class DailyAttendanceRepository : IDailyAttendanceRepository
    {
        private readonly string _connectionString;

        public DailyAttendanceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<DailyAttendance> GetAll(DateTime? DateFrom, DateTime? DateTo)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select 
                           DailyAttendance.Date, JobCategories.Name, Contractors.Name As ContractorName,
                           DailyAttendance.TotalWorker, DailyAttendance.AmountPerWorker,
                           DailyAttendance.TotalAmount
                           From 
                           DailyAttendance
                           Join JobCategories ON DailyAttendance.JobCategoryId = JobCategories.Id
                           Join Contractors ON DailyAttendance.ContractorId = Contractors.Id
                           Where
                           (@DateFrom IS NULL OR DailyAttendance.Date >= @DateFrom) 
                           AND 
                           (@DateTo IS NULL OR DailyAttendance.Date <= @DateTo)
                           Order By DailyAttendance.Date DESC";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@DateFrom", (object)DateFrom ?? DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@DateTo", (object)DateTo ?? DBNull.Value);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<DailyAttendance> attendances = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DailyAttendance attendance = new()
                    {
                        Date = (DateTime)dataTable.Rows[i]["Date"],
                        Name = (string)dataTable.Rows[i]["Name"],
                        ContractorName = (string)dataTable.Rows[i]["ContractorName"],
                        TotalWorker = (int)dataTable.Rows[i]["TotalWorker"],
                        AmountPerWorker = (decimal)dataTable.Rows[i]["AmountPerWorker"],
                        TotalAmount = (decimal)dataTable.Rows[i]["TotalAmount"],
                    };
                    attendances.Add(attendance);
                }
                return attendances;
            }
        }

        public int Create(DailyAttendance dailyAttendance)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into DailyAttendance
                       (Date, JobCategoryId, TotalWorker, AmountPerWorker, TotalAmount)
                       Values
                       (@date, @jobCategoryId, @totalWorker, @amountPerWorker, @totalAmount)
                        SELECT SCOPE_IDENTITY() ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@date", dailyAttendance.Date);
                sqlCommand.Parameters.AddWithValue("@jobCategoryId", dailyAttendance.JobCategoryId);
                sqlCommand.Parameters.AddWithValue("@totalWorker", dailyAttendance.TotalWorker);
                sqlCommand.Parameters.AddWithValue("@amountPerWorker", dailyAttendance.AmountPerWorker);
                sqlCommand.Parameters.AddWithValue("@totalAmount", dailyAttendance.TotalAmount);

                sqlConnection.Open();
                dailyAttendance.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();

                return dailyAttendance.Id;
            }
        }

        public void Delete(int ContractorId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteAddressesQuery = @"DELETE FROM DailyAttendance WHERE ContractorId = @contractorId";
                SqlCommand deleteAddressesCommand = new(deleteAddressesQuery, sqlConnection);
                deleteAddressesCommand.Parameters.AddWithValue("@contractorId", ContractorId);
                sqlConnection.Open();
                deleteAddressesCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
