Create Table Contractors 
(
	Id Int identity(1,1),
	JobCategoryId Int Not Null,
	Name NVarchar (200) Not Null,
	Image NVarchar (200) Null,
	MobileNumber NVarchar (200) Not Null,

	Primary Key (Id),
	FOREIGN KEY (JobCategoryId) REFERENCES JobCategories(Id)
)