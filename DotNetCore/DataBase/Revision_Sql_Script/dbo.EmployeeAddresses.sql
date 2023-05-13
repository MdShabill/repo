Create Table EmployeeAddresses
(
	Id Int Identity(1,1),
	AddressLine1 NVarchar (100) Not Null,
	AddressLine2 NVarchar (100) Not Null,
	PinCode Int,
	Country NVarchar (100),

	Primary Key (Id),
)

Insert Into EmployeeAddresses (AddressLine1, AddressLine2, PinCode, Country)
		Values ('Chakjado Bihar', 'Samastipur Bihar', 843104, 'India')

Insert Into EmployeeAddresses (AddressLine1, AddressLine2, PinCode, Country)
		Values ('Shaheen Bag Delhi', 'Uthrakhand U.P', 110025, 'India')

Insert Into EmployeeAddresses (AddressLine1, AddressLine2, PinCode, Country)
		Values ('London', 'Manchester', 908700, 'England')

Insert Into EmployeeAddresses (AddressLine1, AddressLine2, PinCode, Country)
		Values ('Melbourne', 'Sydney', 10080, 'Australia')

Insert Into EmployeeAddresses (AddressLine1, AddressLine2, PinCode, Country)
		Values ('New York', 'New Mexico', 2208, 'Amarica')

Insert Into EmployeeAddresses (AddressLine1, AddressLine2, PinCode, Country)
		Values ('Najafgarh New Delhi', 'Muzaffarpur Bihar', 110043, 'India')







