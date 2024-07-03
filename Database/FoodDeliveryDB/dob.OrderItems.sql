Create Table OrderItems
(
	Id Int Identity(1,1),
	OrderId Int Not Null,
	OrderNumber NVarchar(200),
	FoodItemId Int Not Null,
	Quantity Int Not Null,

	Primary Key(Id),
	FOREIGN KEY (OrderId) REFERENCES Orders (Id),
	FOREIGN KEY (FoodItemId) REFERENCES FoodItems (Id)
)