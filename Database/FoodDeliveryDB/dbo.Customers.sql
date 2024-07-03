Create Table Customers
(
	Id Int Identity(1,1),
	FullName Nvarchar(200) Not Null,
	Gender Int Not Null,
	Age Int Not Null,
	Email NVarchar(200) Not Null,
	Password NVarchar(200) Not Null,
	MobileNumber Nvarchar(200) Not Null,
	RegistrationDate DateTime Not Null,

	Primary Key(Id)
)