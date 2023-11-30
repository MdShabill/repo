Create Table Orders
(
	Id Int Identity(1,1),
	OrderNumber int Not Null,
	ProductId int Not Null,
	CustomerId int Not Null,
	OrderDate DateTime Not NUll,
	ExpectedDeliveryDate DateTime Null,
	ActualDeliverdDate DateTime Null,
	Price Decimal(10,3) Null,
	Quantity int Not Null,
	AddressId int Not Null

	Primary Key(Id),
	FOREIGN KEY (ProductId) REFERENCES Products (Id),
	FOREIGN KEY (CustomerId) REFERENCES Customers (Id),
	FOREIGN KEY (AddressId) REFERENCES Addresses (Id)
)


DECLARE @randomNumber NVARCHAR(10)
SET @randomNumber = CAST(CAST(NEWID() AS VARBINARY) AS INT)

Insert Into Orders(OrderNumber, ProductId, CustomerId, OrderDate, ExpectedDeliveryDate, ActualDeliverdDate, Price, Quantity, AddressId)
Values(@randomNumber, 3, 1, DATEADD(DAY, -5,GETDATE()), DATEADD(DAY, 5,GETDATE()), DATEADD(DAY, -2,GETDATE()), 170000, 1, 1)

Insert Into Orders(ProductId, CustomerId, OrderDate, ExpectedDeliveryDate, ActualDeliverdDate, AmountPaid, Quantity)
Values(3, 4, DATEADD(DAY, -7,GETDATE()), DATEADD(DAY, 5,GETDATE()), DATEADD(DAY, -3,GETDATE()), 1200, 1)


