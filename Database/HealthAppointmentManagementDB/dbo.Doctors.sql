CREATE TABLE Doctors 
(
    Id INT IDENTITY(1,1),
    FirstName NVARCHAR(200) NOT NULL,
    LastName NVARCHAR(200) NOT NULL,
	PorfileImage NVARCHAR(200) Null,
    Specialty NVARCHAR(100) NOT NULL,
	Qualification NVARCHAR(200) NOT NULL,
	WorkExperience NVARCHAR(200) NOT NULL,
	LicenceNumber NVarchar(200) NOT NULL,
    ContactNumber NVARCHAR(200) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
	AddressId INT NOT NULL,
	PRIMARY KEY (Id)
)

ALTER TABLE Doctors
ADD FOREIGN KEY (AddressId) REFERENCES Addresses(Id)