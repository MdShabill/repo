Create Table Brands
(
	Id Int Identity(1,1),
	BrandName NVarchar(200) Not NUll,
	Primary Key(Id)
)


Insert Into Brands(BrandName)
Values('Levis')

Insert Into Brands(BrandName)
Values('Nike')

Insert Into Brands(BrandName)
Values('Apple')

Insert Into Brands(BrandName)
Values('Sumsung')