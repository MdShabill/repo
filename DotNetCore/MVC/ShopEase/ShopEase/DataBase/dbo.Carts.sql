Create Table Carts
(
	Id Int Identity(1,1),
	CustomerId Int Null,
	ProductId Int Not Null,
	Quantity Int Not Null,
	AddDate DateTime Null,

	Primary Key (Id),
	Foreign Key (CustomerId) REFERENCES Customers (Id),
	Foreign Key (ProductId) REFERENCES Products (Id)
)