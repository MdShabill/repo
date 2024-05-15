Create Table Customers
(
	Id Int Identity(1,1),
	FirstName NVarchar(200) Not Null,
	LastName NVarchar(200) Not Null,
	Email NVarchar(200) Not Null,
	RegistrationDate DateTime,

	Primary Key(Id)
)