CREATE TABLE Customers
(
	Id INT IDENTITY(1,1),
	Name NVARCHAR(100) Not Null,
	Gender Int Not Null,
	Age INT,
	Email NVarchar(100),
	MobileNumber NVarchar(100),
	Country NVARCHAR(100),
	PRIMARY KEY (Id),
	CONSTRAINT UQ_Customers_Email UNIQUE (Email),
	CONSTRAINT UQ_Customers_MobileNumber UNIQUE (MobileNumber)
)

