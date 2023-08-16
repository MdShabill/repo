Create Table Orders
(
	Id int Identity(1,1),
	CustomerId int Not Null,
	ProductId int Not Null,
	OrderDate DateTime Not Null,
	ShippingAddress Nvarchar(200) Not Null,
	ExpectedDeliveryDate DateTime Not Null,	
	TotalAmount Decimal(10,3) Null,
	Quantity int Not Null,	
	Primary Key (Id),
	FOREIGN KEY (CustomerId) REFERENCES Customers (Id),
	FOREIGN KEY (ProductId) REFERENCES Products (Id)
)