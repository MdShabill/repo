CREATE TABLE Prescriptions
(
    Id INT IDENTITY(1,1),
    MedicalRecordId INT NOT NULL,
    Advice NVARCHAR(200) NOT NULL,
	PRIMARY KEY (Id),
    FOREIGN KEY (MedicalRecordId) REFERENCES MedicalRecords(Id)
)
