Create Table Orders1
(
	Id Int Identity (1,1),
	CustomerId Int Not Null,
	ProductId Int NOt Null,
	OrderDate DateTime Not Null,
	ExpectedDeliverdDate DateTime Not Null,
	ActualDeliverdDate DateTime Null,
	Amount Decimal (10,3),

	Primary Key (Id),
	Foreign Key (CustomerId) References Customers1(Id),
	Foreign Key (ProductId) References Products1(Id)
);

Insert Into Orders1(CustomerId, ProductId, OrderDate, ExpectedDeliverdDate, ActualDeliverdDate, Amount)
	Values (1, 1, GetDate(), DateAdd(DAY, +20, GETDATE()), DateAdd(DAY, +15, GETDATE()), 75000.500)

Insert Into Orders1 (CustomerId, ProductId, OrderDate, ExpectedDeliverdDate, ActualDeliverdDate, Amount)
	Values (1, 1, DateAdd(MONTH, -4, GetDate()), DateAdd(DAY, +25, GETDATE()), DateAdd(DAY, +20, GETDATE()), 65000.500)

Insert Into Orders1 (CustomerId, ProductId, OrderDate, ExpectedDeliverdDate, ActualDeliverdDate, Amount)
	Values (2, 3, GetDate(), DateAdd(DAY, +10, GETDATE()), DateAdd(DAY, +7, GETDATE()), 22000.500)

Insert Into Orders1 (CustomerId, ProductId, OrderDate, ExpectedDeliverdDate, ActualDeliverdDate, Amount)
	Values (3, 2, DateAdd(MONTH, -3, GetDate()), DateAdd(DAY, +20, GETDATE()), DateAdd(DAY, +15, GETDATE()), 75000.500)
