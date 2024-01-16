Create table OrderItems 
(
	Id int Identity(1,1),
	OrderId int Not Null,
	OrderNumber NVarchar(200),
	ProductId int Not Null,
	Quantity int Not NUll,
	
	Primary Key (Id),
	Foreign KEY (OrderId) REFERENCES Orders (Id),
	Foreign KEY (ProductId) REFERENCES Products (Id)
)


