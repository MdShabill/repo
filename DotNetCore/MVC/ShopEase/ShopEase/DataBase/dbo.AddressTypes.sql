Create Table AddressTypes
(
	Id Int Identity(1,1),
	AddressTypeName NVarchar(200) Null,
	Primary Key(Id)
)

MERGE INTO AddressTypes AS target
USING (
    VALUES
	('Home'),
	('Office'),
	('Residential'),
	('Temporary'),
    ('Hospital'),
    ('Farm House'),
    ('Permanent'),
    ('Apartment')
) AS source (AddressTypeName)
ON target.AddressTypeName = source.AddressTypeName
WHEN NOT MATCHED THEN
    INSERT (AddressTypeName)
    VALUES (source.AddressTypeName);



