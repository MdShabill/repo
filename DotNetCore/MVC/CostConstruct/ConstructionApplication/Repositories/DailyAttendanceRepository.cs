using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.DataModels.DailyAttendance;
using ConstructionApplication.ViewModels.CostMasterVm;
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

        public int Create(DailyAttendance dailyAttendance)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into DailyAttendance
                       (Date, TotalMasterMason, TotalLabour, MasterMasonAmount, LabourAmount, TotalAmount)
                       Values
                       (@date, @totalMasterMason, @totalLabour, @masterMasonAmount, @labourAmount, @totalAmount) ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@date", dailyAttendance.Date);
                sqlCommand.Parameters.AddWithValue("@totalMasterMason", dailyAttendance.TotalMasterMason);
                sqlCommand.Parameters.AddWithValue("@totalLabour", dailyAttendance.TotalLabour);
                sqlCommand.Parameters.AddWithValue("@masterMasonAmount", dailyAttendance.MasterMasonAmount);
                sqlCommand.Parameters.AddWithValue("@labourAmount", dailyAttendance.LabourAmount);
                sqlCommand.Parameters.AddWithValue("@totalAmount", dailyAttendance.TotalAmount);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
