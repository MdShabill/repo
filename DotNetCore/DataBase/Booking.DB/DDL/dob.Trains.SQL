Create Table Trains
(
	Id Int Identity (1,1) Not Null,
	Name Varchar(100) Not Null,
	TrainNo Int Not Null,
	TrainTypes Varchar (100) Not Null,
	SourceCity Varchar (100) Not Null,
	DestinationCity Varchar (100) Not Null,
	Primary Key (Id)
)	