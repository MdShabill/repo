﻿namespace ConstructionApplication.ViewModels.DailyAttendance
{
    public class DailyAttendanceVm
    {
        public DateTime Date { get; set; }
        public int TotalMasterMason { get; set;}
        public int TotalLabour { get; set;}
        public int TotalCount { get; set; }
        public decimal MasterMasonAmount { get; set; }
        public decimal LabourAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
