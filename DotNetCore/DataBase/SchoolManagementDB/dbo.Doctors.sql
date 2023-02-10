CREATE TABLE Doctors
(
	Id INT IDENTITY(1,1),
	RegistrationNumber Int,
	FullName NVARCHAR(100) Not Null,
	Email NVarchar(100),
	Department NVARCHAR(100),
	Gender Int Not Null,
	City NVARCHAR(100),
	PRIMARY KEY (Id),
	CONSTRAINT DoctorsUniqueRegistrationNumber UNIQUE (RegistrationNumber),
	CONSTRAINT DoctorsUniqueEmail UNIQUE (Email)
)
