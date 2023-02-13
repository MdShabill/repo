CREATE TABLE Doctors
(
	Id INT IDENTITY(1,1),
	FullName NVARCHAR(100) Not Null,
	Gender Int Not Null,
	Email NVarchar(100),
	RegistrationNumber Int,
	Department NVARCHAR(100),
	City NVARCHAR(100),
	PRIMARY KEY (Id),
	CONSTRAINT UQ_Doctors_RegistrationNumber UNIQUE (RegistrationNumber),
	CONSTRAINT UQ_Doctors_Email UNIQUE (Email)
)
