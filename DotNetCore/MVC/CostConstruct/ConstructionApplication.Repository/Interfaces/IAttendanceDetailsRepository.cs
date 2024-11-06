using ConstructionApplication.Core.DataModels.AttendanceDetails;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IAttendanceDetailsRepository
    {
        public int Create(AttendanceDetails attendanceDetails);
    }
}
