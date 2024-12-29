Create Table Contractors 
(
	Id Int identity(1,1),
	JobCategoryId Int Not Null,
	Name NVarchar (200) Not Null,
	Gender Int Null,
	DOB DateTime Null,
	Image NVarchar (200) Null,
	MobileNumber NVarchar (200) Not Null,
	ReferredBy NVarchar (200) Not Null

	Primary Key (Id),
	FOREIGN KEY (JobCategoryId) REFERENCES JobCategories(Id)
)