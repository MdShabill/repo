Create Table Products1
(
	Id Int Identity(1,1),
	ProductName NVarchar (100) Not Null,
	BrandName NVarchar (100) Not Null,
	Color NVarchar (100) Not Null,
	Price int Not Null,
	DicountPrice Decimal (10,2),
	primary Key (Id),
);

Insert Into Products1 (ProductName, BrandName, Color, Price, DicountPrice)
	Values ('Iphone 13', 'Apple', 'Black', 60000, 2000.50)

Insert Into Products1 (ProductName, BrandName, Color, Price, DicountPrice)
	Values ('M 31s', 'Samsung', 'Blue', 20000, 1200.50)

Insert Into Products1 (ProductName, BrandName, Color, Price, DicountPrice)
	Values ('Redmi Note 11i', 'Mi', 'Silver', 25000, 1100.50)

Insert Into Products1 (ProductName, BrandName, Color, Price, DicountPrice)
	Values ('M 8 pro', 'Poco', 'Black', 18000, 1000.50)
-------------------------------------------

Select * from Products1
--------------------------------------------
Update Products1
Set ProductName = 'Redmi Note 11i', BrandName = 'Mi'
Where Id = 3;
--------------------------------------------
Delete From Products1 Where Id = 4



