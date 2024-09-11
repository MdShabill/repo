Create Table Materials
(
	Id Int Identity(1,1),
	Name Nvarchar(200) Not Null,
	MaterialTypeId INT NOT NULL,
    BrandId INT NOT NULL,
    SupplierId INT NOT NULL,
    UnitOfMeasure VARCHAR(200) NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    Date DATETIME Not Null,
    
	PRIMARY KEY (Id),
    FOREIGN KEY (MaterialTypeId) REFERENCES MaterialTypes(Id),
    FOREIGN KEY (BrandId) REFERENCES Brands(Id),
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id)
)