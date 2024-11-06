using ConstructionApplication.Core.DataModels.DailyAttendance;

namespace ConstructionApplication.Repositories
{
    public interface IDailyAttendanceRepository
    {
        public List<DailyAttendance> GetAll(DateTime? DateFrom, DateTime? DateTo);
        public int Create(DailyAttendance dailyAttendance);
        public void Delete(int ContractorId);
    }
}
