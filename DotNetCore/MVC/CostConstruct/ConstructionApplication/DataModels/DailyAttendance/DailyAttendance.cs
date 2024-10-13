namespace ConstructionApplication.DataModels.DailyAttendance
{
    public class DailyAttendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int JobCategoryId { get; set; }
        public string Name { get; set; }
        public int TotalWorker { get; set; }
        public decimal AmountPerWorker { get; set; }
        public decimal TotalAmount {  get; set; }
    }
}
