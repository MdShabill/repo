CREATE TABLE Loctions 
(
    Id INT IDENTITY(1,1),
    JobProviderId INT NOT NULL,
    AddresslLine1 NVARCHAR(200) NOT NULL,
	AddresslLine2 NVARCHAR(200) NOT NULL,
	LoctionTypeId int NOT NULL,
    City NVARCHAR(200) NOT NULL,
    State NVARCHAR(200) NOT NULL,
    ZipCode NVARCHAR(200) NOT NULL,
    CountryTypeId INT NOT NULL,

	PRIMARY KEY(Id),
	FOREIGN KEY (JobProviderId) REFERENCES JobProvider(Id),
	FOREIGN KEY (LoctionTypeId) REFERENCES LoctionTypes(Id),
	FOREIGN KEY (CountryTypeId) REFERENCES Countries1(Id)
)