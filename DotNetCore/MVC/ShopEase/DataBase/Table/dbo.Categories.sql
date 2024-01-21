Create Table Categories
(
	Id Int Identity(1,1),
	CategoryName NVarchar(200) Not NUll,
	Primary Key(Id)
)


MERGE INTO Categories AS target
USING (
    VALUES
	('Medicine'),
	('Toys'),
	('Fashion')
	
) AS source (CategoryName)
ON target.CategoryName = source.CategoryName
WHEN NOT MATCHED THEN
    INSERT (CategoryName)
    VALUES (source.CategoryName);




