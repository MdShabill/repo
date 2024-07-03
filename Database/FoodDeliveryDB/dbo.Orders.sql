Create Table Orders
(
	Id Int Identity(1,1),
	OrderNumber NVarchar(200),
	CustomerId Int Not Null,
	RestaurantId Int Not Null,
	OrderDate DateTime Not Null,
	DeliveryDate DateTime Null,
	AddressId Int Not Null,
	ActualPrice Int Not Null,

	Primary Key(Id),
	FOREIGN KEY (CustomerId) REFERENCES Customers (Id),
	FOREIGN KEY (RestaurantId) REFERENCES Restaurants (Id),
	FOREIGN KEY (AddressId) REFERENCES Addresses (Id)
)