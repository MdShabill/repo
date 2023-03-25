CREATE TABLE Addresses 
(
	Id Int IDENTITY (1,1),
	CustomerId Int NOT NULL,
	AddressLine1 NVarchar (200) NOT NULL,
	AddressLine2 NVarchar (200) NULL,
	PinCode Int NOT NUll,
	Country NVarchar (200) NOT NULL,
	AddressType Int NOT NULL,
	CreatedOn DateTime Not NULL,
	LastEditedOn Datetime null,
	Primary Key (Id),
	Foreign Key (CustomerId) REFERENCES Customers (Id),
)