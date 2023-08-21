Create Table Orders
(
	Id int Identity(1,1),
	CustomerId int Not Null,
	ProductId int Not Null,
	OrderDate DateTime Not Null,
	ShippingAddress Nvarchar(200) Null,
	ExpectedDeliveryDate DateTime Null,	
	Price int Not Null,
	Quantity int Not Null,	
	Primary Key (Id),
	FOREIGN KEY (CustomerId) REFERENCES Customers (Id),
	FOREIGN KEY (ProductId) REFERENCES Products (Id)
)