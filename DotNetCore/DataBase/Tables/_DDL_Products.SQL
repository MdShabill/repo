--Truncate Table Products

Create Table Products
(
	Id Int Identity(1,1),
	ProductName NVarchar(200) Not Null,
	BrandName NVarchar(200) Not Null,
	Size Int Not Null,
	Color Int Not Null,
	Fit NVarchar(200) Not Null,
	Fabric NVarchar(200) Not Null,
	Category NVarchar(100) Not Null,
	Discount Int Not Null,
	Price Int Not Null,
)


