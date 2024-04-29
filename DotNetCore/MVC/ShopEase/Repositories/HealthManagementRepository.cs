using ShopEase.DataModels.HealthManagement;
using System.Data;
using System.Data.SqlClient;

namespace ShopEase.Repositories
{
    public class HealthManagementRepository : IHealthManagementRepository
    {
        private readonly string _connectionString;

        public HealthManagementRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<HealthManagement> GetPatientMedicineDetailBymadicalRecordId(int madicalRecordId)
        {
            using (SqlConnection sqlConnection = new(_connectionString)) 
            {
                string sqlQuery = @"SELECT 
                                    Patients.FirstName as PatientFirstName, Patients.LastName as PatientLastName, 
                                    Patients.DOB, Patients.Gender, Patients.BloodGroup, Patients.ContactNumber,
                                    Patients.Email, PrescriptionDetails.Medicine, PrescriptionDetails.Strength,
                                    PrescriptionDetails.AdditionalFrequency,PrescriptionDetails.FrequencyMorning, 
                                    PrescriptionDetails.FrequencyAfternoon, PrescriptionDetails.FrequencyNight, 
                                    PrescriptionDetails.Instruction
                                    FROM MedicalRecords
                                    JOIN Patients ON MedicalRecords.PatientId = Patients.Id
                                    JOIN PrescriptionDetails ON PrescriptionDetails.MadicalRecordId = MedicalRecords.Id
                                    WHERE PrescriptionDetails.MadicalRecordId = @madicalRecordId ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);

                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@madicalRecordId", madicalRecordId);

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<HealthManagement> healthManagements = new();

                foreach (DataRow row in dataTable.Rows)
                {
                    HealthManagement healthManagement = new()
                    {
                        PatientFirstName = row["PatientFirstName"] == DBNull.Value ? null : (string)row["PatientFirstName"],
                        PatientLastName = row["PatientLastName"] == DBNull.Value ? null : (string)row["PatientLastName"],
                        DOB = (DateTime)(row["DOB"] == DBNull.Value ? (DateTime?)null : (DateTime)row["DOB"]),
                        Gender = row["Gender"] == DBNull.Value ? null : (string)row["Gender"],
                        BloodGroup = row["BloodGroup"] == DBNull.Value ? null : (string)row["BloodGroup"],
                        ContactNumber = row["ContactNumber"] == DBNull.Value ? null : (string)row["ContactNumber"],
                        Email = row["Email"] == DBNull.Value ? null : (string)row["Email"],
                        Medicine = row["Medicine"] == DBNull.Value ? null : (string)row["Medicine"],
                        Strength = row["Strength"] == DBNull.Value ? null : (string)row["Strength"],
                        AdditionalFrequency = row["AdditionalFrequency"] == DBNull.Value ? null : (string)row["AdditionalFrequency"],
                        FrequencyMorning = row["FrequencyMorning"] == DBNull.Value ? false : (bool)row["FrequencyMorning"],
                        FrequencyAfternoon = row["FrequencyAfternoon"] == DBNull.Value ? false : (bool)row["FrequencyAfternoon"],
                        FrequencyNight = row["FrequencyNight"] == DBNull.Value ? false : (bool)row["FrequencyNight"],
                        Instruction = row["Instruction"] == DBNull.Value ? null : (string)row["Instruction"],
                    };
                    healthManagements.Add(healthManagement);
                }
                return healthManagements;
            }
        }

        public List<HealthManagement> GetDoctorPrescriptionBymadicalRecordId(int madicalRecordId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT
                                    MedicalRecords.VisitDate, Patients.FirstName as PatientFirstName,
                                    Patients.LastName as PatientLastName, PrescriptionDetails.Medicine,
                                    PrescriptionDetails.Strength, PrescriptionDetails.AdditionalFrequency,
                                    PrescriptionDetails.FrequencyMorning, PrescriptionDetails.FrequencyAfternoon,
                                    PrescriptionDetails.FrequencyNight, PrescriptionDetails.Instruction,
                                    MedicineCategories.MedicineType
                                    FROM MedicalRecords
                                    JOIN Patients ON MedicalRecords.PatientId = Patients.Id
                                    JOIN PrescriptionDetails ON PrescriptionDetails.MadicalRecordId = MedicalRecords.Id
                                    JOIN MedicineCategories ON PrescriptionDetails.MedicineCategoryId = MedicineCategories.Id
                                    WHERE PrescriptionDetails.MadicalRecordId = @madicalRecordId ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);

                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@madicalRecordId", madicalRecordId);

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<HealthManagement> healthManagements = new();

                foreach (DataRow row in dataTable.Rows)
                {
                    HealthManagement healthManagement = new()
                    {
                        PatientFirstName = row["PatientFirstName"] == DBNull.Value ? null : (string)row["PatientFirstName"],
                        PatientLastName = row["PatientLastName"] == DBNull.Value ? null : (string)row["PatientLastName"],
                        VisitDate = (DateTime)(row["VisitDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["VisitDate"]),
                        Medicine = row["Medicine"] == DBNull.Value ? null : (string)row["Medicine"],
                        Strength = row["Strength"] == DBNull.Value ? null : (string)row["Strength"],
                        AdditionalFrequency = row["AdditionalFrequency"] == DBNull.Value ? null : (string)row["AdditionalFrequency"],
                        FrequencyMorning = row["FrequencyMorning"] == DBNull.Value ? false : (bool)row["FrequencyMorning"],
                        FrequencyAfternoon = row["FrequencyAfternoon"] == DBNull.Value ? false : (bool)row["FrequencyAfternoon"],
                        FrequencyNight = row["FrequencyNight"] == DBNull.Value ? false : (bool)row["FrequencyNight"],
                        Instruction = row["Instruction"] == DBNull.Value ? null : (string)row["Instruction"],
                        MedicineType = row["MedicineType"] == DBNull.Value ? null : (string)row["MedicineType"],
                    };
                    healthManagements.Add(healthManagement);
                }
                return healthManagements;
            }
        }
    }
}
