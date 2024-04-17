CREATE TABLE Doctors 
(
    ID INT IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
	PorfileImage Image Null,
    Specialty NVARCHAR(100) NOT NULL,
	Qualification NVARCHAR(200) NOT NULL,
	WorkExperience NVARCHAR(200) NOT NULL,
    ContactNumber NVARCHAR(200) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
	AddressID INT NOT NULL,
	PRIMARY KEY (ID)
)