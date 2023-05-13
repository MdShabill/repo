Select * From Employees
Select * From EmployeeAddresses
Select * From AddressTypes2

-- Fetch all employees who belongs to country india, whose address type is home.

Select
	Employees.FullName,
	Employees.Email,
	EmployeeAddresses.AddressLine1,
	EmployeeAddresses.AddressLine2,
	EmployeeAddresses.Country,
	EmployeeAddresses.PinCode,
	AddressTypes2.AddressTypeName
From
	Employees

	Inner Join EmployeeAddresses ON Employees.EmployeeAddressId = EmployeeAddresses.Id
	Inner Join AddressTypes2 ON Employees.AddressTypeId = AddressTypes2.Id

	Where Country = 'India' And AddressTypeName = 'Home'

-- Count all the employees whose address type is home, who does'nt belong to india

Select
	Count(Employees.EmployeeAddressId) As EmployeeCount
From
	Employees

	Inner Join EmployeeAddresses ON Employees.EmployeeAddressId = EmployeeAddresses.Id
	Inner Join AddressTypes2 ON Employees.AddressTypeId = AddressTypes2.Id

	Where Not Country = 'India' And AddressTypeName = 'OFFICE'



		



















