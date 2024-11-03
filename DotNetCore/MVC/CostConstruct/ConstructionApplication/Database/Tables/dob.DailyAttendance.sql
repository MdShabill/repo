Create Table DailyAttendance
(
	Id Int Identity(1,1),
	Date Date Not Null,
	JobCategoryId Int Null,
	ContractorId Int Null,
	TotalWorker Int Not Null,
	AmountPerWorker Decimal(10,2) Not Null,
	TotalAmount Decimal(10,2) Null,
	Primary Key (Id),
	FOREIGN KEY (JobCategoryId) REFERENCES JobCategories(Id),
	FOREIGN KEY (ContractorId) REFERENCES Contractors(Id)
)