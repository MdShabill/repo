Create Table Addresses
(
	Id Int Identity(1,1),
	AddressLine1 NVarchar(200) Not Null,
	ContractorId Int Not Null,
	AddressTypeId Int Not Null,
	CountryId Int Not Null,
	PinCode Int Not Null,
	
	Primary Key (Id),
	FOREIGN KEY (ContractorId) REFERENCES Contractors(Id),
	FOREIGN KEY (AddressTypeId) REFERENCES AddressTypes(Id),
	FOREIGN KEY (CountryId) REFERENCES Countries(Id)
)