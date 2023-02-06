CREATE TABLE Doctors
(
    Id INT IDENTITY(1,1),
	FullName NVARCHAR(100) Not Null,
	Email NVarchar(100),
	Department NVARCHAR(100),
	Gender Int Not Null,
	City NVARCHAR(100),
	CONSTRAINT UC_Doctors UNIQUE (Email)
)

--Truncate table Doctors
--Select * From Doctors



