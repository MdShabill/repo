CREATE TABLE Appointments 
(
    Id INT IDENTITY(1,1),
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Purpose NVARCHAR(255) NOT NULL,
    Status NVARCHAR(200) NOT NULL,
	PRIMARY KEY (Id),
    FOREIGN KEY (PatientId) REFERENCES Patients(Id),
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id)
)