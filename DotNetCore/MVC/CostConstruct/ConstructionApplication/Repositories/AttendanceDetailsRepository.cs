using ConstructionApplication.Core.DataModels.AttendanceDetails;
using ConstructionApplication.Core.DataModels.CostMaster;
using System.Data.SqlClient;

namespace ConstructionApplication.Repositories
{
    public class AttendanceDetailsRepository : IAttendanceDetailsRepository
    {
        private readonly string _connectionString;

        public AttendanceDetailsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Create(AttendanceDetails attendanceDetails)
        {
            using(SqlConnection sqlConnection = new(_connectionString)) 
            {
                string sqlQuery = @"Insert Into AttendanceDetails
                       (AttendanceId, Name, Role)
                       Values
                       (@attendanceId, @name, @role) ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@attendanceId", attendanceDetails.AttendanceId);
                sqlCommand.Parameters.AddWithValue("@name", attendanceDetails.Name);
                sqlCommand.Parameters.AddWithValue("@role", attendanceDetails.Role);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
