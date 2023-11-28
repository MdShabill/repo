Create Table CardDetails
(
	Id int Identity(1,1),
	OrderId int Not Null,
	CustomerId int Not Null,
	FullName NVarchar(200) Not Null,
	CardNumber Int Not Null,
	ExpiryDate DateTime Null,
	CVV int Not Null,
	Primary Key (Id),
	FOREIGN KEY (CustomerId) REFERENCES Customers (Id),
	FOREIGN KEY (OrderId) REFERENCES Orders (Id),
	CONSTRAINT UQ_CardDetails_CVV UNIQUE (CVV)
)

