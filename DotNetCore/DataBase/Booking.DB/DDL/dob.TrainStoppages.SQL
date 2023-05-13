Create Table TrainStoppages
(
	Id Int Identity(1,1) Not Null,
	TrainId Int Not Null,
	Station Varchar (100) Not Null,
	ReachTime Time Not Null
	Primary Key (Id)
	FOREIGN KEY (TrainId) REFERENCES Trains (Id)

)

