Create Table Employees
(
	Id int IDENTITY(1,1),
	EmployeeAddressId Int Not Null,
	FullName NVarchar (100) Not Null,
	Email NVarchar (100) Not Null,
	AddressTypeId Int Not Null,
	DateOfJoining DateTime NOT NULL,
	Gender NVarchar (100) NOT NULL,
	Salary Int NOT NULL,
	Primary key(Id),
	
	Foreign Key (EmployeeAddressId) References EmployeeAddresses(Id),
	Foreign Key (AddressTypeId) References AddressTypes2(Id)
)


INSERT INTO Employees(EmployeeAddressId, FullName, Email, AddressTypeId, DateOfJoining, Gender, Salary)

			VALUES (1, 'Zahid', 'zahid80@gmail.com', 1, DATEADD(YEAR, -8, GETDATE()), 'M', 45000 )

INSERT INTO Employees(EmployeeAddressId, FullName, Email, AddressTypeId, DateOfJoining, Gender, Salary)

			VALUES (2, 'Ravinder Sharma', 'ravinder110@gmail.com', 2, DATEADD(YEAR, -6, GETDATE()), 'M', 35000 )

INSERT INTO Employees(EmployeeAddressId, FullName, Email, AddressTypeId, DateOfJoining, Gender, Salary )

			VALUES (3, 'Jiva Rathi', 'jiva080@gmail.com', 1, DATEADD(YEAR, -5, GETDATE()), 'F', 40000 )

INSERT INTO Employees(EmployeeAddressId, FullName, Email, AddressTypeId, DateOfJoining, Gender, Salary )

			VALUES (4, 'Bhusan Rajput', 'bhusan510@gmail.com', 2, DATEADD(YEAR, -3, GETDATE()), 'M', 47000 )

INSERT INTO Employees(EmployeeAddressId, FullName, Email, AddressTypeId, DateOfJoining, Gender, Salary)

			VALUES (6, 'Rayan Ahmad', 'ryan80@gmail.com', 1, DATEADD(YEAR, -2, GETDATE()), 'M', 35000 )

INSERT INTO Employees(EmployeeAddressId, FullName, Email, AddressTypeId, DateOfJoining, Gender, Salary)

			VALUES (5, 'Bilal Raza', 'bilalRaza106@gmail.com', 2, DATEADD(YEAR, -1, GETDATE()), 'M', 55000 )


Select * From Employees

Update Employees
Set FullName = 'Indra', Gender = 'F', Salary = 65000
Where Id = 6;














