Create Table Actors
(
	Id Int Identity(1,1),
	ActorName NVarchar (200) Not Null,
	Primary Key (Id)
)

Insert Into Actors(ActorName)
	Values('SRK')

Insert Into Actors(ActorName)
	Values('Salman Kahan')

Insert Into Actors(ActorName)
	Values('Aamir Kahan')
	
Insert Into Actors(ActorName)
	Values('Ranveer Singh')

Insert Into Actors(ActorName)
	Values('Ranbir Kapoor')

Insert Into Actors(ActorName)
	Values('Amitabh')

Insert Into Actors(ActorName)
	Values('Hrithik')
	
Insert Into Actors(ActorName)
	Values('Shahid Kapoor')