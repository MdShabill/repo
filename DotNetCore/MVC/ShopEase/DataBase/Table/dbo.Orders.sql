Create Table Orders
(
	Id Int Identity(1,1),
	OrderNumber NVarchar(200) Not Null,
	CustomerId int Not Null,
	OrderDate DateTime Not NUll,
	Price Decimal(10,3) Null,
	AddressId int Not Null

	Primary Key(Id),
	FOREIGN KEY (CustomerId) REFERENCES Customers (Id),
	FOREIGN KEY (AddressId) REFERENCES Addresses (Id)
)


DECLARE @randomNumber NVARCHAR(10)
SET @randomNumber = CAST(CAST(NEWID() AS VARBINARY) AS INT)

Insert Into Orders(OrderNumber, CustomerId, OrderDate, Price, AddressId)
Values(@randomNumber, 1, DATEADD(DAY, -5,GETDATE()), 170000, 9)



