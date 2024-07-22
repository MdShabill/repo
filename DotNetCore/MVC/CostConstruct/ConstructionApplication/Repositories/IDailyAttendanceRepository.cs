using ConstructionApplication.DataModels.DailyAttendance;

namespace ConstructionApplication.Repositories
{
    public interface IDailyAttendanceRepository
    {
        public int Create(DailyAttendance dailyAttendance);
    }
}
