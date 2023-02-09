CREATE TABLE Customers
(
	Id INT IDENTITY(1,1),
	Name NVARCHAR(100) Not Null,
	Email NVarchar(100),
	Gender Int Not Null,
	Age INT,
	Country NVARCHAR(100),
	CONSTRAINT CustomersUniqueEmail UNIQUE (Email)
)

