Create Table Addresses
(
	Id Int Identity(1,1),
	CustomerId int Not Null,
	AddressLine1 NVarchar(200) Not Null,
	AddressLine2 NVarchar(200) Null,
	PinCode Int Not Null,
	CountryId Int Not Null,
	AddressTypeId Int Not Null,
	CreatedOn DateTime Null,

	Primary Key(Id),
	FOREIGN KEY (CustomerId) REFERENCES Customers (Id),
	FOREIGN KEY (CountryId) REFERENCES Countries (Id),
	FOREIGN KEY (AddressTypeId) REFERENCES AddressTypes (Id)
)






Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(1, 'L-49 B, ATM Building, Abul Fazal Enclave, Jamia Nagar, New Delehi', 'House No.90, Chakjado, Bihar', 110025, 2, 1, DATEADD(Month, -3,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(1, 'House No.7, Zakariya Colony, Muzaffarpur Bihar', 'Rz- H-47, B-J, RoshanPura, Najafgarh New Delhi', 842001, 1, 2, DATEADD(Month, 5,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(1, 'Paharw Colony, Gali No.4, Plot No.1-A-KH, Najafgarh', 'Falt No.303, 3rd Floor, Ummat Apartment, Shaheen Bagh', 110071, 1, 2, DATEADD(Month, -2,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(2, 'House No.90, Chakjado, Muzaffarpur, Bihar', 'House No.7, Zakariya Colony, Muzaffarpur Bihar', 843104, 1, 7, DATEADD(YEAR, 10,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(2, '20-B, 3rd Floor, Martin Para, Gram Kolkata, West Bengal', 'Ahmed Manzil House No.92, Chakjado, Bihar', 700100, 1, 4, DATEADD(Month, -3,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(2, 'G-12/195, Sangam Vihar New Delhi', 'Dharampur, SamastiPur, Bihar', 110080, 1, 5, DATEADD(Month, -5,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(3, 'House No.87, 3rd Floor, Darbhanga, Bihar', 'Ahmed Manzil House No.92, Chakjado, Bihar', 85230, 1, 8, DATEADD(Month, -2,GETDATE()))

Insert Into Addresses(CustomerId, AddressLine1, AddressLine2, PinCode, CountryId, AddressTypeId, CreatedOn)
Values(3, 'A-90/180, Sangam Vihar New Delhi', 'House No.22, Zakir Nagar, Jamia Nagar, New Delhi', 110078, 1, 7, DATEADD(Month, -4,GETDATE()))


Select * From Addresses