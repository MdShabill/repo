CREATE TABLE Appointments 
(
    ID INT IDENTITY(1,1),
    PatientID INT NOT NULL,
    DoctorID INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Purpose NVARCHAR(255) NOT NULL,
    Status NVARCHAR(20) NOT NULL,
	PRIMARY KEY (ID),
    FOREIGN KEY (PatientID) REFERENCES Patients(ID),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(ID)
)