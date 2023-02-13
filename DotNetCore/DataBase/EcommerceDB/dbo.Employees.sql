CREATE TABLE Employees
(
	Id INT IDENTITY(1,1),
	FullName NVARCHAR(100) Not Null,
	Gender Int Not Null,
	Email NVarchar(100) Not Null,
	MobileNumber NVarchar(100) Not Null,
	Salary INT,
	PRIMARY KEY (Id),
	CONSTRAINT UQ_Employees_Email UNIQUE (Email),
	CONSTRAINT UQ_Employees_MobileNumber UNIQUE (MobileNumber)
)
