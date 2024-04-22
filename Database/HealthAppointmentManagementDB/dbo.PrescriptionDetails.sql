CREATE TABLE PrescriptionDetails
(
    Id INT IDENTITY(1,1),
    MadicalRecordId INT NOT NULL,
	MedicineCategoryId INT NOT NULL,
    Medicine NVARCHAR(200) NOT NULL,
    Strength NVARCHAR(200) NULL,
	AdditionalFrequency NVARCHAR(200) NULL,
    FrequencyMorning Bit NULL,
	FrequencyAfternoon Bit NULL,
	FrequencyNight Bit NULL,
	Instruction NVARCHAR(200) NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (MadicalRecordId) REFERENCES MedicalRecords(Id),
	FOREIGN KEY (MedicineCategoryId) REFERENCES MedicineCategories(Id)
)