using ConstructionApplication.Core.DataModels.DailyAttendance;
using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Repository.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.AdoDotNet
{
    public class DailyAttendanceRepository : IDailyAttendanceRepository
    {
        private readonly string _connectionString;

        public DailyAttendanceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<DailyAttendance> GetAll(int siteId, DateTime? DateFrom, DateTime? DateTo)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select 
                           DailyAttendance.Date, ServiceTypes.Name, ServiceProviders.Name As ServiceProviderName,
                           DailyAttendance.TotalWorker, DailyAttendance.AmountPerWorker,
                           DailyAttendance.TotalAmount
                           From 
                           DailyAttendance
                           Join ServiceTypes ON DailyAttendance.ServiceTypeId = ServiceTypes.Id
                           Join ServiceProviders ON DailyAttendance.ServiceProviderId = ServiceProviders.Id
                           Join Sites ON DailyAttendance.SiteId = Sites.Id
                           Where
                           DailyAttendance.SiteId = @SiteId AND
                           (@DateFrom IS NULL OR DailyAttendance.Date >= @DateFrom) 
                           AND 
                           (@DateTo IS NULL OR DailyAttendance.Date <= @DateTo)
                           Order By DailyAttendance.Date DESC";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@SiteId", siteId);
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
                string sqlQuery = @"Insert Into DailyAttendance
                       (Date, ServiceTypeId, ServiceProviderId, SiteId, TotalWorker, AmountPerWorker, TotalAmount)
                       Values
                       (@date, @serviceTypeId, @serviceProviderId, @siteId, @totalWorker, @amountPerWorker, @totalAmount)
                        SELECT SCOPE_IDENTITY() ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@date", dailyAttendance.Date);
                sqlCommand.Parameters.AddWithValue("@serviceTypeId", dailyAttendance.ServiceTypeId);
                sqlCommand.Parameters.AddWithValue("@serviceProviderId", dailyAttendance.ServiceProviderId);
                sqlCommand.Parameters.AddWithValue("@siteId", dailyAttendance.SiteId);
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
                string deleteAddressesQuery = @"DELETE FROM DailyAttendance WHERE ServiceProviderId = @serviceProviderId";
                SqlCommand deleteAddressesCommand = new(deleteAddressesQuery, sqlConnection);
                deleteAddressesCommand.Parameters.AddWithValue("@serviceProviderId", serviceProviderId);
                sqlConnection.Open();
                deleteAddressesCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
