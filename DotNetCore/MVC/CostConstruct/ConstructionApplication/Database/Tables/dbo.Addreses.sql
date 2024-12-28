Create Table Addresses
(
	Id Int Identity(1,1),
	AddressLine1 NVarchar(200) Null,
	ContractorId Int Null,
	AddressTypeId Int Null,
	CountryId Int Null,
	PinCode Int Null,
	
	Primary Key (Id),
	FOREIGN KEY (ContractorId) REFERENCES Contractors(Id),
	FOREIGN KEY (AddressTypeId) REFERENCES AddressTypes(Id),
	FOREIGN KEY (CountryId) REFERENCES Countries(Id)
)