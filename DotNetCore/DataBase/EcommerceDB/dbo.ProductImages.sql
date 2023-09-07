Create Table ProductImages
(
	Id int Identity(1,1),
	ProductName Nvarchar(200) Not Null,
	Description NVarchar(200) Null,
	ImageName NVarchar(200) Not Null,
	Primary Key(Id)
)