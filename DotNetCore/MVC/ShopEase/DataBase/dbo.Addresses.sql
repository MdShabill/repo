Create Table Addresses
(
	Id Int Identity(1,1),
	CustomerId int Not Null,
	AddressLine1 NVarchar(200) Not Null,
	AddressLine2 NVarchar(200) Null,
	PinCode Int Not Null,
	CountryId Int Not Null,
	AddressTypeId Int Not Null,
	CreatedOn DateTime Null,

	Primary Key(Id),
	FOREIGN KEY (CountryId) REFERENCES Countries (Id),
	FOREIGN KEY (AddressTypeId) REFERENCES AddressTypes (Id)
)




Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(2, 'SamastiPur Bihar', 'Sangam Vihar', 848101, 2, 1, DATEADD(Month, -3,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(3, 'Muzaffarpur Bihar', 'Najafgarh New Delhi', 842001, 1, 2, DATEADD(Month, -5,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(2, 'Shaheen Bagh', 'Najafgarh New Delhi', 110025, 1, 2, DATEADD(Month, -2,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(4, 'Kolkata West Bangal', 'SamastiPur Bihar', 700100, 2, 3, DATEADD(Month, -3,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(5, 'Patna Bihar', 'Shaheen bagh', 800004, 1, 4, DATEADD(Month, -2,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(6, 'Raipur Chhattis Garh', 'Kolkata West Bangal', 492001, 1, 5, DATEADD(Month, -5,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(5, 'Jamia Nagar New Delhi', 'Muzaffarpur ', 110025, 1, 8, DATEADD(Month, -2,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(4, 'Gurugram Haryana', 'Shheen Bagh', 122002, 1, 7, DATEADD(Month, -4,GETDATE()))


Select * From Addresses