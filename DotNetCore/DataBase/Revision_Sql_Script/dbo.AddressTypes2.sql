
CREATE TABLE AddressTypes2 
(
	Id Int IDENTITY (1,1),
	AddressTypeName Varchar (200)
	Primary Key (Id)
)

INSERT INTO AddressTypes2(AddressTypeName)
			       VALUES ('Home')

INSERT INTO AddressTypes2(AddressTypeName)
			       VALUES ('Office')

