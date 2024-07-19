Create Table CostMaster
(
	Id Int Identity(1,1),
	MasterMasonCost Decimal(10,2) Not Null,
	LabourCost Decimal(10,2) Not Null,
	Date Date Null,

	Primary Key(Id)
)