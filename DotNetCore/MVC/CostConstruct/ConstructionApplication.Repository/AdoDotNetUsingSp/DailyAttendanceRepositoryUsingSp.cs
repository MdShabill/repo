using ConstructionApplication.Core.DataModels.DailyAttendance;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionApplication.Core.DataModels.ServiceProviders;

namespace ConstructionApplication.Repository.AdoDotNetUsingSp
{
    public class DailyAttendanceRepositoryUsingSp : IDailyAttendanceRepository
    {
        private readonly string _connectionString;

        public DailyAttendanceRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<DailyAttendance> GetAll(int siteId, DateTime? DateFrom, DateTime? DateTo)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlCommand sqlCommand = new("Sp_DailyAttendanceCRUD", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Mode", "GET_ALL");
                sqlCommand.Parameters.AddWithValue("@DateFrom", (object)DateFrom ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@DateTo", (object)DateTo ?? DBNull.Value);

                SqlDataAdapter sqlDataAdapter = new(sqlCommand);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);


                List<DailyAttendance> attendances = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DailyAttendance attendance = new()
                    {
                        Date = (DateTime)dataTable.Rows[i]["Date"],
                        Name = (string)dataTable.Rows[i]["Name"],
                        ServiceProviderName = (string)dataTable.Rows[i]["ServiceProviderName"],
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
                SqlCommand sqlCommand = new("Sp_DailyAttendanceCRUD", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Mode", "CREATE");
                sqlCommand.Parameters.AddWithValue("@date", dailyAttendance.Date);
                sqlCommand.Parameters.AddWithValue("@serviceTypeId", dailyAttendance.ServiceTypeId);
                sqlCommand.Parameters.AddWithValue("@cerviceProviderId", dailyAttendance.ServiceProviderId);
                sqlCommand.Parameters.AddWithValue("@totalWorker", dailyAttendance.TotalWorker);
                sqlCommand.Parameters.AddWithValue("@amountPerWorker", dailyAttendance.AmountPerWorker);
                sqlCommand.Parameters.AddWithValue("@totalAmount", dailyAttendance.TotalAmount);

                sqlConnection.Open();
                dailyAttendance.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();

                return dailyAttendance.Id;
            }
        }

        public void Delete(int serviceProviderId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlCommand sqlCommand = new("Sp_DailyAttendanceCRUD", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Mode", "DELETE");
                sqlCommand.Parameters.AddWithValue("@serviceProviderId", serviceProviderId);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
