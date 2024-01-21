Create Table Suppliers
(
	Id Int Identity(1,1),
	SupplierName NVarchar(200) Not Null,
	Primary Key(Id)
)

Insert Into Suppliers(SupplierName)
Values('India Mart')

Insert Into Suppliers(SupplierName)
Values('Trade India')

Insert Into Suppliers(SupplierName)
Values('Export India')

Insert Into Suppliers(SupplierName)
Values('WebDeal India')