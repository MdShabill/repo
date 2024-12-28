Create Table MaterialPurchase
(
	Id Int Identity(1,1),
	MaterialId Int Not Null,
	SupplirId Int Null,
	PhoneNumber NVarchar(200) Null,
	BrandId Int NUll,
	Quantity Int Null,
	UnitOfMeasure NVarchar(200) Not Null,
	Date DateTime Not Null,
	MaterialCost Decimal(10,2) Not Null,
	DeliveryCharge Decimal(10,2) Null,
	PaymentStatus NVarchar(200) Null,

	Primary Key (Id),
	FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
	FOREIGN KEY (SupplirId) REFERENCES Suppliers(Id),
	FOREIGN KEY (BrandId) REFERENCES Brands(Id)
)