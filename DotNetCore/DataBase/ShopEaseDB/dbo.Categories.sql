Create Table Categories
(
	Id Int Identity(1,1),
	CategoryName NVarchar(200) Not NUll,
	Primary Key(Id)
)


Insert Into Categories(CategoryName)
Values('Fashion')

Insert Into Categories(CategoryName)
Values('Mobiles')

Insert Into Categories(CategoryName)
Values('Electornics')

Insert Into Categories(CategoryName)
Values('Sports')



