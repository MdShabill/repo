Create Table DailyAttendance
(
	Id Int Identity(1,1),
	Date Date Not Null,
	TotalMasterMason Int Not Null,
	TotalLabour Int Not Null,
	Primary Key (Id),
)

Alter Table DailyAttendance
Add
	MasterMasonAmount Decimal(10,2), 
	LabourAmount Decimal(10,2),
	TotalAmount Decimal(10,2)
