
Create Table Customers
(
	Id Int Identity(1,1),
	FullName NVarchar(200) Not Null,
	Mobile NVarchar(200) Not Null,
	Gender Int Not Null,
	Email NVarchar(200) Not Null,
	Password NVarchar(200) Not Null,
	Primary Key(Id)
)
















Insert Into Customers(FullName, Mobile, Gender, Email, Password)
Values('Zahid Ahmed', '9908765420', 1, 'zahid20@gmail.com', 'zahid123')

Insert Into Customers(FullName, Mobile, Gender, Email, Password)
Values('Nazish Ahmed', '8808765460', 1, 'nazishamd@gmail.com', 'nazish101')

Insert Into Customers(FullName, Mobile, Gender, Email, Password)
Values('Salman Irfani', '959946909', 1, 'salman21@gmail.com', 'salman0002')

Insert Into Customers(FullName, Mobile, Gender, Email, Password)
Values('Adil Khan', '7708765880', 1, 'khan360@gmail.com', 'adilkhan002')






