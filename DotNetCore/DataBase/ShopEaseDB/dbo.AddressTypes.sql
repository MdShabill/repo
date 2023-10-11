Create Table AddressTypes
(
	Id Int Identity(1,1),
	AddressTypeName NVarchar(200) Null,
	Primary Key(Id)
)

Insert Into AddressTypes(AddressTypeName)
Values('Home')

Insert Into AddressTypes(AddressTypeName)
Values('Office')

Insert Into AddressTypes(AddressTypeName)
Values('Residential')

Insert Into AddressTypes(AddressTypeName)
Values('Temporary ')

