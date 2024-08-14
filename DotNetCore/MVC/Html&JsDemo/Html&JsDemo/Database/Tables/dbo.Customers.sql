Create Table Customers
(
	Id Int Identity(1,1),
	FullName NVarchar(200) Not Null,
	Email NVarchar(200) Not Null,
	Gender NVarchar(200) Not Null,
	Age Int Not Null,

	Primary Key(Id)
)