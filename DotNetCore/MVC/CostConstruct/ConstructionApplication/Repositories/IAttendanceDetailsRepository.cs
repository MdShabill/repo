using ConstructionApplication.DataModels.AttendanceDetails;

namespace ConstructionApplication.Repositories
{
    public interface IAttendanceDetailsRepository
    {
        public int Create(AttendanceDetails attendanceDetails);
    }
}
