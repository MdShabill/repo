namespace ConstructionApplication.DataModels.DailyAttendance
{
    public class DailyAttendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TotalMasterMason { get; set; }
        public int TotalLabour { get; set; }
        public decimal MasterMasonAmount { get; set; }
        public decimal LabourAmount { get; set; }
        public decimal TotalAmount {  get; set; }
    }
}
