Create Table Brands
(
	Id Int Identity(1,1),
	BrandName NVarchar(200) Not NUll,
	Primary Key(Id)
)


MERGE INTO Brands AS target
USING (
    VALUES
	('Dell'),
	('Hp'),
	('Cipla'),
	('MRF'),
	('Sun Pharma')
) AS source (BrandName)
ON target.BrandName = source.BrandName
WHEN NOT MATCHED THEN
    INSERT (BrandName)
    VALUES (source.BrandName);





