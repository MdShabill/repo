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
	FOREIGN KEY (CountryId) REFERENCES AddressTypes (Id)
)

















Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(2, 'SamastiPur Bihar', 'Sangam Vihar', 848101, 2, 1, DATEADD(Month, -3,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(3, 'Muzaffarpur Bihar', 'Najafgarh New Delhi', 842001, 1, 2, DATEADD(Month, -5,GETDATE()))

