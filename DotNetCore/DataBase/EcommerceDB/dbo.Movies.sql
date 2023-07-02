Create Table Movies
(
	Id int Identity(1,1),
	MovieName NVarchar(200) Not Null,
	DirectorName Nvarchar (200) Not Null,
	ActorId int Not Null,
	Primary Key (Id),
	FOREIGN KEY (ActorId) REFERENCES Actors (Id)
)






