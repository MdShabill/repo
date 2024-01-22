CREATE TABLE Addresses 
(
    Id INT Identity(1,1),
    JobSeekerId INT Not Null,
    AddressLine1 NVARCHAR(200) Not Null,
	AddressLine2 NVARCHAR(200) Not Null,
	AddressTypeId Int Not Null,
	CountryId Int Not Null,
    City NVARCHAR(200) Not Null,
    State NVARCHAR(200) Not Null,
    ZipCode NVARCHAR(200) Not Null,

	PRIMARY KEY(Id),
	FOREIGN KEY (JobSeekerId) REFERENCES JobSeekers (Id),
	FOREIGN KEY (AddressTypeId) REFERENCES AddressTypes (Id),
	FOREIGN KEY (CountryId) REFERENCES Countries (Id)
)

