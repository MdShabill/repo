Create Table Restaurants
(
	Id Int Identity(1,1),
	RestaurantName NVarchar(200) Not Null,
	MobileNumber NVarchar(200) Not Null,
	Location NVarchar(200) Not Null,
	OpenTime DateTime Not Null,
	CloseTime DateTime Not Null,
	Rating Decimal(10,3) Null,

	Primary key(Id)
)
