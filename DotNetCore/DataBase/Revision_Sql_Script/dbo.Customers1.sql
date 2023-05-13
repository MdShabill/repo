Create Table Customers1
(
	Id Int Identity (1,1),
	FullName NVarchar (100) Not Null,
	MobileNumber NVarchar (100) Not Null,
	Email NVarchar (100) Not Null,
	Password NVarchar (100) Not Null,
	AddressLine1 Nvarchar (100) Not Null,
	AddressLine2 NVarchar (100) Null,
	Pincode NVarchar(100) Not Null,

	Primary Key (Id),
	Constraint UQ_Customers1_Email Unique (Email),
);

--------------------------------------------------------
Insert Into Customers1 (FullName, MobileNumber, Email, Password, AddressLine1, AddressLine2, Pincode)
	Values ('Kashif', '790-350-7644', 'kashif@gmail.com', 'kashif123', 'Shaheen Bag New Delhi', 'SujawalPur Bihar', '110025')

Insert Into Customers1 (FullName, MobileNumber, Email, Password, AddressLine1, AddressLine2, Pincode)
	Values ('Zahid', '790-350-7644', 'zahid@gmail.com', 'zahid123', 'Mali Ghat KolKata', 'Samastipur Bihar', '843214')

Insert Into Customers1 (FullName, MobileNumber, Email, Password, AddressLine1, AddressLine2, Pincode)
	Values ('Shabill', '790-350-7644', 'shabill@gmail.com', 'shabillf123', 'Najafgarh New Delhi', 'Chakjado Bihar', '843104')

Insert Into Customers1 (FullName, MobileNumber, Email, Password, AddressLine1, AddressLine2, Pincode)
	Values ('Salman', '790-350-7644', 'salman@gmail.com', 'salman123', 'Chawala Gao New Delhi', 'Muzaffarpur Bihar', '842002')
----------------------------------

Select * From Customers1





