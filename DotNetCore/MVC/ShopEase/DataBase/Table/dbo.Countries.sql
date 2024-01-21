Create Table Countries
(
	Id Int Identity(1,1),
	CountryName NVarchar(200) Not Null,
	Primary Key(Id)
)


Insert Into Countries(CountryName)
Values('India')

Insert Into Countries(CountryName)
Values('U.S.A')

Insert Into Countries(CountryName)
Values('England')

Insert Into Countries(CountryName)
Values('Canada')



