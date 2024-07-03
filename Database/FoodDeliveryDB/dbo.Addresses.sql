Create Table Addresses
(
	Id Int Identity(1,1),
	CustomerId Int Not Null,
	AddressTypeId Int Not Null,
	AddressLine1 NVarchar(200) Not Null,
	AddredssLine2 NVarchar(200) Not Null,
	CountryId Int Not Null,
	PinCode int Not Null,

	Primary Key(Id),
	FOREIGN KEY (CustomerId) REFERENCES Customers (Id),
	FOREIGN KEY (AddressTypeId) REFERENCES AddressTypes (Id),
	FOREIGN KEY (CountryId) REFERENCES Countries (Id)
)