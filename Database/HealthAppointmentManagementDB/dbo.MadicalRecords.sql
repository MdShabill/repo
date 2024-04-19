CREATE TABLE MedicalRecords 
(
    Id INT IDENTITY(1,1),
    PatientId INT NOT NULL,
	DoctorId INT NOT NULL,
	AppointmentId INT NOT NULL,
	Height DECIMAL(5, 2) NOT NULL,
	Weight DECIMAL(5, 2) NOT NULL, 
    VisitDate DATETIME NOT NULL,
    Treatment NVARCHAR(255) NOT NULL,
	PRIMARY KEY (Id),
    FOREIGN KEY (PatientId) REFERENCES Patients(Id),
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id)
)

ALTER TABLE MedicalRecords
ADD FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id)