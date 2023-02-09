CREATE TABLE Doctors
(
	Id INT IDENTITY(1,1),
	FullName NVARCHAR(100) Not Null,
	Email NVarchar(100),
	Department NVARCHAR(100),
	Gender Int Not Null,
	City NVARCHAR(100),
	CONSTRAINT DoctorsUniqueEmail UNIQUE (Email)
)

--Truncate table Doctors
--Select * From Doctors

Insert into Doctors(FullName, Email, Department, Gender, City)
	   Values ('Shabill', 'shabill8@gmail.com', 'Physiotherapist', 1, 'India')

