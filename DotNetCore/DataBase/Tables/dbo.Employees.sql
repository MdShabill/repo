CREATE TABLE Employees
(
	Id INT IDENTITY(1,1),
	FullName NVARCHAR(100) Not Null,
	Email NVarchar(100) Not Null,
	Gender Int Not Null,
	Salary INT,
	CONSTRAINT EmployeesUniqueEmail UNIQUE (Email)
)
