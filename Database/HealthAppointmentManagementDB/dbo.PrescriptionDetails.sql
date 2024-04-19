CREATE TABLE PrescriptionDetails
(
    Id INT IDENTITY(1,1),
    PrescriptionId INT NOT NULL,
    Medicine NVARCHAR(200) NOT NULL,
    Dosage NVARCHAR(200) NOT NULL,
    Frequency NVARCHAR(200) NOT NULL,
	PRIMARY KEY (Id),
    FOREIGN KEY (PrescriptionId) REFERENCES PrescriptionDetails(Id)
)
