
CREATE TABLE Products
(
	Id Int IDENTITY (1,1),
	ProductName NVarchar (200) NOT NULL,
	BrandId Int NOT NULL,
	Price Decimal(10,3) NOT NULL,
	Discount Decimal(10,3) NOT NULL,
	CategoryId Int NOT NULL,
	SupplierId Int NOT NULL,
	Primary Key (Id),	
)

Alter Table Products
Add ImageName Nvarchar(200)

Alter Table Products
Add Quantity int

Alter Table Products
Add FOREIGN KEY (BrandId) REFERENCES Brands (Id)

Alter Table Products
Add FOREIGN KEY (CategoryId) REFERENCES Categories (Id)

Alter Table Products
Add FOREIGN KEY (SupplierId) REFERENCES Suppliers (Id)







Insert Into Products(ProductName, BrandId, Price, Discount, CategoryId, SupplierId)
Values('Iphone 15 Pro-Max', 2, 200000, 40000.70, 2, 1)

Insert Into Products(ProductName, BrandId, Price, Discount, CategoryId, SupplierId)
Values('Samsung A54', 2, 35000.40, 5000.50, 2, 2)

Insert Into Products(ProductName, BrandId, Price, Discount, CategoryId, SupplierId)
Values('Jeans', 1, 2200.90, 600.60, 1, 3)

Insert Into Products(ProductName, BrandId, Price, Discount, CategoryId, SupplierId)
Values('Jacket', 2, 8000.00, 2000.30, 1, 4)




