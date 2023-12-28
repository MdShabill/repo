Create Table FoodItems
(
	Id Int Identity(1,1),
	RestaurantId Int Not Null,
	Name Nvarchar(200) Null,
	Image Nvarchar(200) Not Null,
	Offers Decimal(10,3) Not Null,
	Price Decimal(10,3) Not Null,
	Rating Decimal(10,3) Null,
	IsVeg BIT NOT NULL,

	Primary Key(Id),
	FOREIGN KEY (RestaurantId) REFERENCES Restaurants (Id)
)

Alter Table FoodItems
Add TotalRating int 

Select * From FoodItems

