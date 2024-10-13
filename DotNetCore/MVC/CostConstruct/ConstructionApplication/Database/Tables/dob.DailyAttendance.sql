Create Table DailyAttendance
(
	Id Int Identity(1,1),
	Date Date Not Null,
	JobCategoryId Int Not Null,
	TotalWorker Int Not Null,
	AmountPerWorker Decimal(10,2) Not Null,
	Primary Key (Id),
	FOREIGN KEY (JobCategoryId) REFERENCES JobCategories(Id)
)