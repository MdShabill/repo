namespace ShopEase.DataModels.HealthManagement
{
    public class HealthManagement
    {
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public int MadicalRecordId { get; set;}
        public DateTime VisitDate { get; set;}
        public int MedicineCategoryId { get; set;}
        public string Medicine { get; set; }
        public string Strength { get; set; }
        public string AdditionalFrequency { get; set; }
        public bool FrequencyMorning { get; set; }
        public bool FrequencyAfternoon { get; set; }
        public bool FrequencyNight { get; set; }
        public string Instruction { get; set; }
        public string MedicineType { get; set; }
    }
}
