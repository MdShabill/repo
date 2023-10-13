Create Table AddressTypes
(
	Id Int Identity(1,1),
	AddressTypeName NVarchar(200) Null,
	Primary Key(Id)
)

MERGE INTO AddressTypes AS target
USING (
    VALUES
    ('Hospital'),
    ('Farm House'),
    ('Permanent'),
	('Apartment')
) AS source (AddressTypeName)
ON 0 = 1  
WHEN NOT MATCHED BY TARGET THEN
    INSERT (AddressTypeName)
    VALUES (source.AddressTypeName);


	






Insert Into AddressTypes(AddressTypeName)
Values('Home')

Insert Into AddressTypes(AddressTypeName)
Values('Office')

Insert Into AddressTypes(AddressTypeName)
Values('Residential')

Insert Into AddressTypes(AddressTypeName)
Values('Temporary ')

