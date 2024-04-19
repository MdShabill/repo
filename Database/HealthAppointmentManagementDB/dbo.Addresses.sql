CREATE TABLE Addresses 
(
    Id INT IDENTITY(1,1),
    AddressLine1 NVARCHAR(200) NOT NULL,
    AddressLine2 NVARCHAR(200) NULL,
    PinCode INT NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    AddressTypeId INT NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (AddressTypeId) REFERENCES AddressTypes(Id)
)