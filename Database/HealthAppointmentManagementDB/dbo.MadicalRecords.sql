CREATE TABLE MedicalRecords 
(
    ID INT IDENTITY(1,1),
    PatientID INT NOT NULL,
	DoctorID INT NOT NULL,
	AppointmentID INT NOT NULL,
	Height DECIMAL(5, 2) NOT NULL,
	Weight DECIMAL(5, 2) NOT NULL, 
    VisitDate DATETIME NOT NULL,
    Treatment NVARCHAR(255) NOT NULL,
	PRIMARY KEY (ID),
    FOREIGN KEY (PatientID) REFERENCES Patients(ID),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(ID)
)

ALTER TABLE MedicalRecords
ADD FOREIGN KEY (AppointmentID) REFERENCES Appointments(ID)