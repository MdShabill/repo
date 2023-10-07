Create Table Actors
(
	Id Int Identity(1,1),
	ActorName NVarchar (200) Not Null,
	Primary Key (Id)
)

Alter Table Actors
Add Gender NVarchar (200) Not Null

Insert Into Actors(ActorName, Gender)
	Values('SRK', 'Male')

Insert Into Actors(ActorName, Gender)
	Values('Salman Kahan', 'Male')

Insert Into Actors(ActorName, Gender)
	Values('Aamir Kahan', 'Male')
	
Insert Into Actors(ActorName, Gender)
	Values('Ranveer Singh', 'Male')

Insert Into Actors(ActorName, Gender)
	Values('Ranbir Kapoor', 'Male')

Insert Into Actors(ActorName, Gender)
	Values('Amitabh', 'Male')

Insert Into Actors(ActorName, Gender)
	Values('Hrithik', 'Male')
	
Insert Into Actors(ActorName, Gender)
	Values('Shahid Kapoor', 'Male')

Insert Into Actors(ActorName, Gender)
	Values('Preity Zinta', 'Female')

Insert Into Actors(ActorName, Gender)
	Values('Aishwarya Rai', 'Female')

Insert Into Actors(ActorName, Gender)
	Values('Deepika', 'Female')

Insert Into Actors(ActorName, Gender)
	Values('Priyanka', 'Female')

Insert Into Actors(ActorName, Gender)
	Values('Madhuri', 'Female')

Insert Into Actors(ActorName, Gender)
	Values('Kajol', 'Female')

Insert Into Actors(ActorName, Gender)
	Values('Katrina', 'Female')

Insert Into Actors(ActorName, Gender)
	Values('Alia Bhatt', 'Female')

Insert Into Actors(ActorName, Gender)
	Values('Anushka', 'Female')
