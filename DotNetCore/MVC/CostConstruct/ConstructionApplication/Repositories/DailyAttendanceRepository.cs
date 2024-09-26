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
                           Date, TotalMasterMason,  
                           TotalLabour, TotalCount, MasterMasonAmount,
                           LabourAmount, TotalAmount
                           From 
                           DailyAttendance
                           Where
                           (@DateFrom IS NULL OR Date >= @DateFrom) 
                           AND 
                           (@DateTo IS NULL OR Date <= @DateTo)
                           Order By Date DESC";
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
                        TotalMasterMason = (int)dataTable.Rows[i]["TotalMasterMason"],
                        TotalLabour = (int)dataTable.Rows[i]["TotalLabour"],
                        TotalCount = (int)dataTable.Rows[i]["TotalCount"],
                        MasterMasonAmount = (decimal)dataTable.Rows[i]["MasterMasonAmount"],
                        LabourAmount = (decimal)dataTable.Rows[i]["LabourAmount"],
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
                       (Date, TotalMasterMason, TotalLabour, TotalCount, MasterMasonAmount, LabourAmount, TotalAmount)
                       Values
                       (@date, @totalMasterMason, @totalLabour, @totalCount, @masterMasonAmount, @labourAmount, @totalAmount)
                        SELECT SCOPE_IDENTITY() ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@date", dailyAttendance.Date);
                sqlCommand.Parameters.AddWithValue("@totalMasterMason", dailyAttendance.TotalMasterMason);
                sqlCommand.Parameters.AddWithValue("@totalLabour", dailyAttendance.TotalLabour);
                sqlCommand.Parameters.AddWithValue("@totalCount", dailyAttendance.TotalCount);
                sqlCommand.Parameters.AddWithValue("@masterMasonAmount", dailyAttendance.MasterMasonAmount);
                sqlCommand.Parameters.AddWithValue("@labourAmount", dailyAttendance.LabourAmount);
                sqlCommand.Parameters.AddWithValue("@totalAmount", dailyAttendance.TotalAmount);

                sqlConnection.Open();
                dailyAttendance.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();

                return dailyAttendance.Id;
            }
        }
    }
}
