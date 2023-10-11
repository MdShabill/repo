Create Table Orders
(
	Id Int Identity(1,1),
	ProductId int Not Null,
	CustomerId int Not Null,
	OrderDate DateTime Not NUll,
	ExpectedDeliveryDate DateTime Not Null,
	ActualDeliverdDate DateTime Null,
	AmountPaid Decimal(10,3) Null,

	Primary Key(Id),
	FOREIGN KEY (ProductId) REFERENCES Products (Id),
	FOREIGN KEY (CustomerId) REFERENCES Customers (Id)
)













Insert Into Orders(ProductId, CustomerId, OrderDate, ExpectedDeliveryDate, ActualDeliverdDate, AmountPaid)
Values(1, 2, DATEADD(DAY, -5,GETDATE()), DATEADD(DAY, 5,GETDATE()), DATEADD(DAY, -2,GETDATE()), 150000)

Insert Into Orders(ProductId, CustomerId, OrderDate, ExpectedDeliveryDate, ActualDeliverdDate, AmountPaid)
Values(3, 4, DATEADD(DAY, -7,GETDATE()), DATEADD(DAY, 5,GETDATE()), DATEADD(DAY, -3,GETDATE()), 1200)
