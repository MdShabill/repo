Create Table CostMaster
(
	Id Int Identity(1,1),
	JobCategoryId Int Not Null,
	Cost Decimal(10,0) Not Null,
	Date Date Null,

	Primary Key(Id),
	FOREIGN KEY (JobCategoryId) REFERENCES JobCategories(Id)
)